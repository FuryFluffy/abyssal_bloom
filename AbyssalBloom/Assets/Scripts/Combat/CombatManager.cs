using System;
using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// CombatManager — turn-based combat orchestrator
// ════════════════════════════════════════════════════════════════════════════
// Attach to a persistent GameObject (e.g. "_Managers").
// Does NOT handle UI.  Fires C# events for every game-state change;
// a separate CombatUI MonoBehaviour subscribes and renders.
//
// Flow:  StartEncounter → [BeginRound → (AdvanceTurn → TurnStart →
//        AwaitAction → SubmitAction → ResolveAction → TurnEnd) … →
//        RoundEnd] … → EncounterWon / EncounterLost
//
// The only pause point is AwaitingAction — the UI calls SubmitAction()
// to resume.  No coroutines, no Update polling.
// ════════════════════════════════════════════════════════════════════════════

// ── Enums ──────────────────────────────────────────────────────────────────

public enum CombatPhase
{
    Idle,
    TurnStart,
    AwaitingAction,
    AwaitingForcedSwap,  // Heroine KO'd mid-turn; combat paused for player to pick replacement
    ResolvingAction,
    TurnEnd,
    EncounterWon,
    EncounterLost
}

public enum ActionType
{
    // Active heroine — normal combat
    Attack, Defend, Skill, UseItem, Run,
    // Support heroine — normal combat
    AssistAttack, AssistAbility, SwapIn,
    // Active heroine — grapple
    Struggle, Submit,
    // Support heroine — grapple
    Intervene, Watch, Encourage
}

// ── Action data submitted by UI ────────────────────────────────────────────

[System.Serializable]
public struct CombatAction
{
    public ActionType             type;
    public RuntimeCharacterState  actor;
    public RuntimeCharacterState  target;   // null for some actions
    public CharacterAbilitySO     ability;  // only for ActionType.Skill
}

// ── CombatManager ──────────────────────────────────────────────────────────

public class CombatManager : MonoBehaviour
{
    // ====================================================================
    #region State
    // ====================================================================

    [Header("Debug (read-only at runtime)")]
    [SerializeField] private CombatPhase _phase = CombatPhase.Idle;
    public CombatPhase Phase => _phase;

    // Party
    private List<RuntimeCharacterState> _heroines = new();   // always 3
    private int _activeIndex;                                 // index into _heroines
    private List<RuntimeCharacterState> _enemies  = new();

    // Turn order
    private List<RuntimeCharacterState> _turnOrder = new();
    private int _currentTurnIdx;
    private int _roundNumber;

    // Grapple
    private bool _grappleActive;
    private List<RuntimeCharacterState> _grapplers = new();
    private int _sceneStage;

    // Boss AI state
    private bool _bloodNunHealedPhase2;   // tracks Blood Rite usage in phase 2

    // Quick accessors
    public RuntimeCharacterState ActiveHeroine => _heroines[_activeIndex];
    public IReadOnlyList<RuntimeCharacterState> Heroines => _heroines;
    public IReadOnlyList<RuntimeCharacterState> Enemies  => _enemies;
    public bool IsGrappleActive => _grappleActive;
    public int  SceneStage      => _sceneStage;
    public int  RoundNumber     => _roundNumber;

    public RuntimeCharacterState CurrentUnit =>
        (_turnOrder != null && _currentTurnIdx >= 0 && _currentTurnIdx < _turnOrder.Count)
            ? _turnOrder[_currentTurnIdx]
            : null;

    #endregion

    // ====================================================================
    #region Events (UI subscribes to these)
    // ====================================================================

    /// <summary>Fired when a unit's turn begins. Arg: the unit.</summary>
    public event Action<RuntimeCharacterState> OnTurnStarted;

    /// <summary>
    /// Fired when the manager needs player input.
    /// Args: unit whose turn it is, available action types.
    /// </summary>
    public event Action<RuntimeCharacterState, List<ActionType>> OnAwaitingAction;

    public event Action<RuntimeCharacterState, RuntimeCharacterState, int> OnDamageDealt;
    public event Action<RuntimeCharacterState, int>                       OnHealingDone;
    public event Action<RuntimeCharacterState, int>                       OnResolveLost;
    public event Action<RuntimeCharacterState, int>                       OnCorruptionGained;
    public event Action<RuntimeCharacterState, StatusEffectSO, bool>      OnStatusApplied;  // bool = wasNegated
    public event Action<RuntimeCharacterState, StatusEffectSO>            OnStatusExpired;
    public event Action<RuntimeCharacterState>                            OnUnitDefeated;
    public event Action                                                   OnGrappleStarted;
    public event Action                                                   OnGrappleEnded;
    public event Action<int>                                              OnSceneStageAdvanced;
    public event Action<RuntimeCharacterState, int>                       OnClimaxRecoil;
    public event Action<string, RuntimeCharacterState>                    OnPassiveTriggered; // passiveId, source
    public event Action<RuntimeCharacterState, bool>                      OnStruggleAttempt;  // bool = escaped
    public event Action<bool>                                             OnEncounterEnd;     // true = victory
    public event Action<RuntimeCharacterState>                            OnDotTick;
    public event Action<int>                                              OnRoundStarted;
    public event Action                                                   OnActionMiss;

    /// <summary>
    /// Fired when the Active heroine is KO'd and a replacement must be chosen.
    /// Args: list of living support heroines the player may pick from.
    /// Combat is paused (Phase == AwaitingForcedSwap) until SubmitForcedSwap() is called.
    /// </summary>
    public event Action<List<RuntimeCharacterState>>                      OnForcedSwapRequired;

    #endregion

    // ====================================================================
    #region Public API — Encounter Lifecycle
    // ====================================================================

    /// <summary>
    /// Begin a new encounter.  Caller provides fully-initialised states with
    /// abilities already wired.  heroines list must have exactly 3 entries;
    /// index 0 is the starting Active heroine.
    /// </summary>
    public void StartEncounter(
        List<RuntimeCharacterState> heroines,
        List<RuntimeCharacterState> enemies)
    {
        if (heroines == null || heroines.Count != 3)
        {
            Debug.LogError("[CombatManager] Exactly 3 heroines required.");
            return;
        }
        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogError("[CombatManager] At least 1 enemy required.");
            return;
        }

        _heroines    = new List<RuntimeCharacterState>(heroines);
        _enemies     = new List<RuntimeCharacterState>(enemies);
        _activeIndex = 0;

        // Reset grapple
        _grappleActive = false;
        _grapplers.Clear();
        _sceneStage = 0;

        // Reset boss AI state
        _bloodNunHealedPhase2 = false;

        // Roll initiative (fixed for entire encounter)
        _turnOrder.Clear();
        foreach (var h in _heroines)
        {
            h.initiative = CombatFormulas.RollInitiative(h.SPD);
            _turnOrder.Add(h);
        }
        foreach (var e in _enemies)
        {
            e.initiative = CombatFormulas.RollInitiative(e.SPD);
            _turnOrder.Add(e);
        }
        _turnOrder.Sort((a, b) => b.initiative.CompareTo(a.initiative)); // descending

        _roundNumber = 0;
        BeginRound();
    }

    /// <summary>
    /// Called by UI when the player has chosen an action.
    /// Only valid when Phase == AwaitingAction.
    /// </summary>
    public void SubmitAction(CombatAction action)
    {
        if (_phase != CombatPhase.AwaitingAction)
        {
            Debug.LogWarning("[CombatManager] SubmitAction called outside AwaitingAction phase.");
            return;
        }

        _phase = CombatPhase.ResolvingAction;
        ResolveAction(action);

        if (_phase == CombatPhase.ResolvingAction)
            ProcessTurnEnd();
    }

    /// <summary>
    /// Called by UI when the player has chosen a replacement Active heroine
    /// after a forced swap (Active KO'd mid-encounter).
    /// Only valid when Phase == AwaitingForcedSwap.
    /// </summary>
    public void SubmitForcedSwap(RuntimeCharacterState chosenHeroine)
    {
        if (_phase != CombatPhase.AwaitingForcedSwap)
        {
            Debug.LogWarning("[CombatManager] SubmitForcedSwap called outside AwaitingForcedSwap phase.");
            return;
        }

        int idx = _heroines.IndexOf(chosenHeroine);
        if (idx < 0 || !chosenHeroine.IsAlive || idx == _activeIndex)
        {
            Debug.LogWarning("[CombatManager] SubmitForcedSwap: invalid heroine chosen.");
            return;
        }

        _activeIndex = idx;
        _phase = CombatPhase.ResolvingAction; // return to the flow that was interrupted

        // The knockout already happened; resume from wherever HandleKnockout was called.
        // If the encounter isn't over, ProcessTurnEnd will advance the turn normally.
        if (!CheckEncounterEnd())
            ProcessTurnEnd();
    }

    #endregion

    // ====================================================================
    #region Turn Loop (private)
    // ====================================================================

    private void BeginRound()
    {
        _roundNumber++;
        _currentTurnIdx = -1;
        OnRoundStarted?.Invoke(_roundNumber);
        AdvanceTurn();
    }

    private void AdvanceTurn()
    {
        _currentTurnIdx++;

        // Skip dead units
        while (_currentTurnIdx < _turnOrder.Count &&
               !_turnOrder[_currentTurnIdx].IsAlive)
        {
            _currentTurnIdx++;
        }

        if (_currentTurnIdx >= _turnOrder.Count)
        {
            // Round over — start next round
            BeginRound();
            return;
        }

        ProcessTurnStart();
    }

    private void ProcessTurnStart()
    {
        var unit = CurrentUnit;
        _phase = CombatPhase.TurnStart;

        // Clear defend flag (set during previous round)
        unit.isDefending = false;

        // Tick passive cooldowns for this unit
        unit.TickPassiveCooldowns();

        // Tick DOTs and status durations (doc: DOTs tick at START of turn)
        var dotResults = StatusEffectManager.Tick(unit);
        foreach (var dot in dotResults)
        {
            OnDotTick?.Invoke(unit);

            // Check if DOT killed the unit
            if (!unit.IsAlive)
            {
                HandleKnockout(unit);
                if (CheckEncounterEnd()) return;
                AdvanceTurn();
                return;
            }
        }

        OnTurnStarted?.Invoke(unit);

        // ── Is this a grappling enemy? Auto Grapple Action. ───────────
        if (_grappleActive && !unit.isHeroine && _grapplers.Contains(unit))
        {
            ProcessGrappleAction(unit);
            if (CheckEncounterEnd()) return;
            ProcessTurnEnd();
            return;
        }

        // ── Decide what actions are available ──────────────────────────
        RequestAction(unit);
    }

    private void RequestAction(RuntimeCharacterState unit)
    {
        _phase = CombatPhase.AwaitingAction;

        if (unit.isHeroine)
        {
            var actions = GetAvailableActions(unit);
            OnAwaitingAction?.Invoke(unit, actions);
            // Now waiting for SubmitAction() callback from UI
        }
        else
        {
            // Enemy AI — pick and resolve immediately
            var aiAction = DecideEnemyAction(unit);
            _phase = CombatPhase.ResolvingAction;
            ResolveAction(aiAction);
            if (_phase == CombatPhase.ResolvingAction)
                ProcessTurnEnd();
        }
    }

    private void ProcessTurnEnd()
    {
        _phase = CombatPhase.TurnEnd;
        if (CheckEncounterEnd()) return;
        AdvanceTurn();
    }

    private bool CheckEncounterEnd()
    {
        // All enemies dead → victory
        bool enemiesAlive = false;
        foreach (var e in _enemies)
            if (e.IsAlive) { enemiesAlive = true; break; }

        if (!enemiesAlive)
        {
            _phase = CombatPhase.EncounterWon;
            OnEncounterEnd?.Invoke(true);
            return true;
        }

        // All heroines at 0 HP → defeat
        bool heroinesAlive = false;
        foreach (var h in _heroines)
            if (h.IsAlive) { heroinesAlive = true; break; }

        if (!heroinesAlive)
        {
            _phase = CombatPhase.EncounterLost;
            OnEncounterEnd?.Invoke(false);
            return true;
        }

        return false;
    }

    #endregion

    // ====================================================================
    #region Available Actions
    // ====================================================================

    private List<ActionType> GetAvailableActions(RuntimeCharacterState unit)
    {
        var actions = new List<ActionType>();
        bool isActive = (unit == ActiveHeroine);

        if (_grappleActive)
        {
            if (isActive)
            {
                if (unit.GetResolveBand() != ResolveBand.Broken)
                    actions.Add(ActionType.Struggle);
                actions.Add(ActionType.Submit);
                actions.Add(ActionType.UseItem);
            }
            else
            {
                actions.Add(ActionType.Intervene);
                actions.Add(ActionType.Watch);
                actions.Add(ActionType.Encourage);
                actions.Add(ActionType.UseItem);
            }
        }
        else
        {
            if (isActive)
            {
                actions.Add(ActionType.Attack);
                actions.Add(ActionType.Defend);
                if (unit.abilities.Count > 0)
                    actions.Add(ActionType.Skill);
                actions.Add(ActionType.UseItem);
                actions.Add(ActionType.Run);
            }
            else
            {
                actions.Add(ActionType.AssistAttack);
                // ── CHANGE 1: gate AssistAbility on 3-turn cooldown ──
                if (unit.IsPassiveReady("assist_ability"))
                    actions.Add(ActionType.AssistAbility);
                // Swap In is unavailable during grapple (already handled above)
                actions.Add(ActionType.SwapIn);
                actions.Add(ActionType.UseItem);
            }
        }

        return actions;
    }

    #endregion

    // ====================================================================
    #region Action Resolution
    // ====================================================================

    private void ResolveAction(CombatAction action)
    {
        switch (action.type)
        {
            // ── Active heroine normal combat ───────────────────────────
            case ActionType.Attack:
                ResolveUniversalAttack(action.actor, action.target);
                break;

            case ActionType.Defend:
                action.actor.isDefending = true;
                break;

            case ActionType.Skill:
                if (action.ability != null)
                    ResolveAbility(action.actor, action.target, action.ability);
                break;

            case ActionType.Run:
                // TODO: Flee logic (not yet designed). For now, always fails.
                Debug.Log("[CombatManager] Run attempted — not yet implemented.");
                break;

            // ── Support heroine normal combat ──────────────────────────
            case ActionType.AssistAttack:
                ResolveAssistAttack(action.actor, action.target);
                break;

            // ── CHANGE 2: start 3-turn cooldown after AssistAbility ──
            case ActionType.AssistAbility:
                if (action.ability != null)
                {
                    ResolveAbility(action.actor, action.target, action.ability);
                    action.actor.StartPassiveCooldown("assist_ability", 3);
                }
                break;

            case ActionType.SwapIn:
                ResolveSwapIn(action.actor);
                break;

            // ── Grapple actions (Active) ───────────────────────────────
            case ActionType.Struggle:
                ResolveStruggle(action.actor);
                break;

            case ActionType.Submit:
                ResolveSubmit(action.actor);
                break;

            // ── Grapple actions (Support) ──────────────────────────────
            case ActionType.Intervene:
                ResolveIntervene(action.actor);
                break;

            case ActionType.Watch:
                ResolveWatch(action.actor);
                break;

            case ActionType.Encourage:
                ResolveEncourage(action.actor);
                break;

            // ── Shared ─────────────────────────────────────────────────
            case ActionType.UseItem:
                // TODO: Item system not yet designed.
                Debug.Log("[CombatManager] UseItem — not yet implemented.");
                break;
        }
    }

    // ── Universal Attack ───────────────────────────────────────────────────

    private void ResolveUniversalAttack(
        RuntimeCharacterState attacker, RuntimeCharacterState target)
    {
        if (target == null || !target.IsAlive) return;

        float power = CombatFormulas.PowerMultiplier(CombatFormulas.UniversalAttackPower);
        int hitChance = CombatFormulas.HitChance(
            CombatFormulas.UniversalAttackBaseHit, attacker.SPD, target.SPD);

        if (!CombatFormulas.RollHit(hitChance))
        {
            OnActionMiss?.Invoke();
            return;
        }

        int damage = CombatFormulas.PhysicalDamage(attacker.ATK, power, target.DEF);
        ApplyDamage(attacker, target, damage);
    }

    // ── Ability Resolution ─────────────────────────────────────────────────

    private void ResolveAbility(
        RuntimeCharacterState actor,
        RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        // MP check
        if (actor.currentMP < ability.mpCost)
        {
            Debug.LogWarning($"[CombatManager] {actor.displayName} lacks MP for {ability.displayName}.");
            return;
        }
        actor.SpendMP(ability.mpCost);

        switch (ability.abilityType)
        {
            case CharacterAbilitySO.AbilityType.Physical:
                ResolvePhysicalAbility(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.Magic:
                ResolveMagicAbility(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.Healing:
                ResolveHealingAbility(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.ResolveAttack:
                ResolveResolveAttack(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.CorruptionAttack:
                ResolveCorruptionAttack(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.Grapple:
                ResolveGrappleAbility(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.Buff:
                ResolveBuffAbility(actor, target, ability);
                break;
        }

        // Grapple initiation (physical/magic abilities can also start grapple)
        if (ability.isGrappleInitiator && ability.abilityType != CharacterAbilitySO.AbilityType.Grapple
            && target != null && target.isHeroine)
        {
            TryInitiateGrapple(actor, target);
        }
    }

    private void ResolvePhysicalAbility(
        RuntimeCharacterState actor, RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null || !target.IsAlive) return;

        int hitChance = CombatFormulas.HitChance(
            ability.baseHitChance, actor.SPD, target.SPD);

        if (!CombatFormulas.RollHit(hitChance))
        {
            OnActionMiss?.Invoke();
            return;
        }

        int damage = CombatFormulas.PhysicalDamage(actor.ATK, ability.Power, target.DEF);
        ApplyDamage(actor, target, damage);

        // Status application (MAG governs chance even for physical)
        TryApplyAbilityStatus(actor, target, ability);
    }

    private void ResolveMagicAbility(
        RuntimeCharacterState actor, RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null || !target.IsAlive) return;

        // Magic auto-hits
        int damage = CombatFormulas.MagicDamage(actor.MAG, ability.Power, target.RES);
        ApplyDamage(actor, target, damage);

        TryApplyAbilityStatus(actor, target, ability);
    }

    private void ResolveHealingAbility(
        RuntimeCharacterState actor, RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        var healTarget = target ?? actor;
        int amount = CombatFormulas.Healing(ability.baseHeal, actor.MAG, ability.Power);
        healTarget.Heal(amount);
        OnHealingDone?.Invoke(healTarget, amount);
    }

    private void ResolveResolveAttack(
        RuntimeCharacterState actor, RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null || !target.isHeroine) return;

        // Magic-type resolve attacks auto-hit
        int resolveDmg = CombatFormulas.ResolveDamage(
            ability.baseResolveDamage, actor.MAG, target.RES);
        target.LoseResolve(resolveDmg);
        OnResolveLost?.Invoke(target, resolveDmg);

        // Combined resolve+corruption abilities (e.g. Whisper of Doubt, Communion)
        // also deal corruption in the same action if baseCorruptionGain is set.
        if (ability.baseCorruptionGain > 0)
        {
            int corruptGain = CombatFormulas.CorruptionGain(
                ability.baseCorruptionGain, target.RES);
            target.GainCorruption(corruptGain);
            OnCorruptionGained?.Invoke(target, corruptGain);
        }

        TryApplyAbilityStatus(actor, target, ability);
    }

    private void ResolveCorruptionAttack(
        RuntimeCharacterState actor, RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null || !target.isHeroine) return;

        int corruptGain = CombatFormulas.CorruptionGain(
            ability.baseCorruptionGain, target.RES);
        target.GainCorruption(corruptGain);
        OnCorruptionGained?.Invoke(target, corruptGain);
    }

    // ── Grapple Ability ────────────────────────────────────────────────────
    // Rolls hit chance; on hit calls TryInitiateGrapple(). No HP damage.

    private void ResolveGrappleAbility(
        RuntimeCharacterState actor, RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null || !target.IsAlive || !target.isHeroine) return;

        int hitChance = CombatFormulas.HitChance(
            ability.baseHitChance, actor.SPD, target.SPD);

        if (!CombatFormulas.RollHit(hitChance))
        {
            OnActionMiss?.Invoke();
            return;
        }

        TryInitiateGrapple(actor, target);
    }

    // ── Buff Ability ───────────────────────────────────────────────────────
    // Applies a status effect to any target (ally enemy or heroine).
    // statusBaseChance = 100 for guaranteed buffs.

    private void ResolveBuffAbility(
        RuntimeCharacterState actor, RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null || !target.IsAlive) return;
        if (ability.statusEffect == null) return;

        // Buffs bypass Mira's Alchemist's Reflex — they're not debuffs.
        // Apply directly without the Mira check.
        bool applied = StatusEffectManager.Apply(
            target, ability.statusEffect, actor, ability.statusDurationOverride);
        OnStatusApplied?.Invoke(target, ability.statusEffect, !applied);
    }

    // ── Status application (with Mira passive check) ───────────────────────

    private void TryApplyAbilityStatus(
        RuntimeCharacterState actor,
        RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (ability.statusEffect == null || ability.statusBaseChance <= 0) return;

        int chance = CombatFormulas.StatusChance(
            ability.statusBaseChance, actor.MAG, target.RES);

        if (!CombatFormulas.RollStatus(chance)) return;

        TryApplyStatus(target, ability.statusEffect, actor, ability.statusDurationOverride);
    }

    /// <summary>
    /// Central status application — checks Mira's Alchemist's Reflex before
    /// forwarding to StatusEffectManager.
    /// </summary>
    private void TryApplyStatus(
        RuntimeCharacterState target,
        StatusEffectSO statusDef,
        RuntimeCharacterState source,
        int durationOverride = -1)
    {
        // ── Mira's Alchemist's Reflex passive check ────────────────────
        // Fires when a status is about to be applied to the Active heroine.
        // Does NOT fire on Corruption gain (Corruption is a stat, not a status).
        if (target == ActiveHeroine)
        {
            var mira = FindSupportHeroine("mira_voss");
            if (mira != null && mira.IsAlive && mira.IsPassiveReady("alchemist_reflex"))
            {
                mira.StartPassiveCooldown("alchemist_reflex", 3);
                OnPassiveTriggered?.Invoke("alchemist_reflex", mira);

                if (UnityEngine.Random.value < 0.50f)
                {
                    // Negated!
                    OnStatusApplied?.Invoke(target, statusDef, true);
                    return;
                }
            }
        }

        bool applied = StatusEffectManager.Apply(
            target, statusDef, source, durationOverride);
        OnStatusApplied?.Invoke(target, statusDef, !applied);
    }

    // ── Assist Attack (Support heroine, Very Low power, free) ──────────────

    private void ResolveAssistAttack(
        RuntimeCharacterState actor, RuntimeCharacterState target)
    {
        if (target == null || !target.IsAlive) return;

        float power = CombatFormulas.PowerMultiplier(PowerBand.VeryLow);

        // Seraphine's assist is Magic type (Minor Smite); others are Physical.
        // DECISION POINT: For now, detect by characterId. A cleaner approach
        // is an assist-attack SO on each heroine — defer until roster expands.
        if (actor.characterId == "seraphine")
        {
            int damage = CombatFormulas.MagicDamage(actor.MAG, power, target.RES);
            ApplyDamage(actor, target, damage);
        }
        else
        {
            int hitChance = CombatFormulas.HitChance(90, actor.SPD, target.SPD);
            if (!CombatFormulas.RollHit(hitChance))
            {
                OnActionMiss?.Invoke();
                return;
            }
            int damage = CombatFormulas.PhysicalDamage(actor.ATK, power, target.DEF);
            ApplyDamage(actor, target, damage);
        }
    }

    // ── Swap In ────────────────────────────────────────────────────────────

    private void ResolveSwapIn(RuntimeCharacterState supportHeroine)
    {
        if (_grappleActive) return; // Swap In unavailable during grapple

        int idx = _heroines.IndexOf(supportHeroine);
        if (idx < 0 || idx == _activeIndex) return;

        _activeIndex = idx;
        // Swap In costs the support heroine's action — already consumed by
        // being the current unit's turn.
    }

    #endregion

    // ====================================================================
    #region Damage & Knockouts
    // ====================================================================

    private void ApplyDamage(
        RuntimeCharacterState source,
        RuntimeCharacterState target,
        int rawDamage)
    {
        int finalDamage = rawDamage;

        // Defend halves damage
        if (target.isDefending)
            finalDamage = Mathf.Max(1, Mathf.RoundToInt(rawDamage * CombatFormulas.DefendDamageMultiplier));

        target.TakeDamage(finalDamage);
        OnDamageDealt?.Invoke(source, target, finalDamage);

        // ── Check passives that fire on damage to Active heroine ───────
        if (target == ActiveHeroine && target.IsAlive)
        {
            CheckLysandraPassive(source, finalDamage);
            CheckSeraphinePassive();
        }

        // ── Knockout ───────────────────────────────────────────────────
        if (!target.IsAlive)
            HandleKnockout(target);
    }

    private void HandleKnockout(RuntimeCharacterState unit)
    {
        OnUnitDefeated?.Invoke(unit);

        if (!unit.isHeroine) return; // Enemy death — just remove from turn order

        // ── Heroine KO during grapple: scene → climax, grapple breaks ──
        if (_grappleActive && unit == ActiveHeroine)
        {
            ProcessClimax();
        }

        // ── Forced swap: pause and ask the player to choose ────────────
        if (unit == ActiveHeroine)
        {
            // Build the list of valid choices: living heroines not in the active slot
            var candidates = new List<RuntimeCharacterState>();
            for (int i = 0; i < _heroines.Count; i++)
            {
                if (i != _activeIndex && _heroines[i].IsAlive)
                    candidates.Add(_heroines[i]);
            }

            if (candidates.Count == 0)
            {
                // No living heroines remain — CheckEncounterEnd will catch this
                Debug.Log("[CombatManager] No heroines available to swap in.");
                return;
            }

            // Pause combat and hand control to the UI.
            // SubmitForcedSwap() resumes the flow.
            _phase = CombatPhase.AwaitingForcedSwap;
            OnForcedSwapRequired?.Invoke(candidates);
        }
    }

    #endregion

    // ====================================================================
    #region Grapple System
    // ====================================================================

    private void TryInitiateGrapple(
        RuntimeCharacterState enemy, RuntimeCharacterState target)
    {
        if (!target.isHeroine) return;
        if (target != ActiveHeroine) return; // Only Active can be grappled

        // Check max grapplers
        if (_grappleActive)
        {
            if (_grapplers.Count >= ActiveHeroine.MaxGrapplers) return;
            if (_grapplers.Contains(enemy)) return;

            // At ClearLow, only the initial grapple-ability enemy can grapple.
            // Additional enemies can join only at Tempted+ (any enemy can join).
            // This enemy has a grapple ability, so they can join regardless.
            _grapplers.Add(enemy);
        }
        else
        {
            _grappleActive = true;
            _sceneStage = 1;
            _grapplers.Clear();
            _grapplers.Add(enemy);
            OnGrappleStarted?.Invoke();
        }
    }

    /// <summary>Automatic Grapple Action on a grappling enemy's turn.</summary>
    private void ProcessGrappleAction(RuntimeCharacterState grappler)
    {
        var target = ActiveHeroine;

        // Stage 2 bonus
        int resolveBonus = _sceneStage >= 2 ? 2 : 0;
        int corruptBonus = _sceneStage >= 2 ? 2 : 0;

        // Resolve damage
        int resolveDmg = CombatFormulas.GrappleResolveDamage(grappler.MAG, target.RES)
                         + resolveBonus;
        target.LoseResolve(resolveDmg);
        OnResolveLost?.Invoke(target, resolveDmg);

        // Corruption gain
        int corruptGain = CombatFormulas.GrappleCorruptionGain(target.RES)
                          + corruptBonus;
        target.GainCorruption(corruptGain);
        OnCorruptionGained?.Invoke(target, corruptGain);

        // Frenzy: +1 ATK and +1 MAG permanently
        grappler.PermanentStatBoost(1, 1);
    }

    // ── Struggle ───────────────────────────────────────────────────────────

    private void ResolveStruggle(RuntimeCharacterState heroine)
    {
        if (!_grappleActive || _grapplers.Count == 0) return;

        // Use primary (first) grappler for ATK comparison
        var primaryGrappler = _grapplers[0];

        int chance = CombatFormulas.StruggleChance(
            heroine.ATK, primaryGrappler.ATK, heroine.GetResolveBand());

        if (chance < 0)
        {
            // Broken — should not reach here (UI shouldn't offer Struggle)
            OnStruggleAttempt?.Invoke(heroine, false);
            return;
        }

        bool escaped = CombatFormulas.RollHit(chance); // reuse the roll helper
        OnStruggleAttempt?.Invoke(heroine, escaped);

        if (escaped)
            BreakGrapple();
    }

    // ── Submit ─────────────────────────────────────────────────────────────

    private void ResolveSubmit(RuntimeCharacterState heroine)
    {
        if (!_grappleActive || _grapplers.Count == 0) return;

        var primaryGrappler = _grapplers[0];

        // +1 Scene Stage
        AdvanceSceneStage();

        // Resolve damage from Submit
        int resolveDmg = CombatFormulas.SubmitResolveDamage(
            primaryGrappler.MAG, heroine.RES);
        heroine.LoseResolve(resolveDmg);
        OnResolveLost?.Invoke(heroine, resolveDmg);

        // Corruption from Submit
        int corruptGain = CombatFormulas.SubmitCorruptionGain(heroine.RES);
        heroine.GainCorruption(corruptGain);
        OnCorruptionGained?.Invoke(heroine, corruptGain);
    }

    // ── Intervene (Support, guaranteed break) ──────────────────────────────

    private void ResolveIntervene(RuntimeCharacterState support)
    {
        if (!_grappleActive) return;
        if (support.currentMP < 8)
        {
            Debug.LogWarning($"[CombatManager] {support.displayName} lacks MP to Intervene (8 required).");
            return;
        }

        support.SpendMP(8);
        support.LoseResolve(3);
        OnResolveLost?.Invoke(support, 3);

        BreakGrapple();
    }

    // ── Watch (Support, free) ──────────────────────────────────────────────

    private void ResolveWatch(RuntimeCharacterState support)
    {
        support.GainCorruption(5);
        OnCorruptionGained?.Invoke(support, 5);
    }

    // ── Encourage (Support, free) ──────────────────────────────────────────

    private void ResolveEncourage(RuntimeCharacterState support)
    {
        if (!_grappleActive || _grapplers.Count == 0) return;

        // Capture reference before stage advance — AdvanceSceneStage may
        // trigger Climax → BreakGrapple → clear _grapplers.
        var primaryGrappler = _grapplers[0];

        // +1 Scene Stage
        AdvanceSceneStage();

        // Corruption: +8 to encourager, +3 to Active
        support.GainCorruption(8);
        OnCorruptionGained?.Invoke(support, 8);

        ActiveHeroine.GainCorruption(3);
        OnCorruptionGained?.Invoke(ActiveHeroine, 3);

        // Enemy +1 ATK permanent (primary grappler)
        primaryGrappler.PermanentStatBoost(1, 0);
    }

    // ── Scene stage management ─────────────────────────────────────────────

    private void AdvanceSceneStage()
    {
        // Stage 4+ requires Active or encourager Corruption >= 40.
        // For now (Decision M), Stage 4+ effects are deferred to scene writing,
        // so climax always fires at Stage 3 regardless of corruption.
        if (_sceneStage >= 3)
        {
            return;
        }

        _sceneStage++;
        OnSceneStageAdvanced?.Invoke(_sceneStage);

        if (_sceneStage >= 3)
            ProcessClimax();
    }

    private void ProcessClimax()
    {
        // Climax Recoil damage to each grappler
        foreach (var grappler in _grapplers)
        {
            int recoil = CombatFormulas.ClimaxRecoilDamage(
                grappler.maxHP, ActiveHeroine.corruption);
            grappler.TakeDamage(recoil);
            OnClimaxRecoil?.Invoke(grappler, recoil);

            if (!grappler.IsAlive)
                OnUnitDefeated?.Invoke(grappler);
        }

        BreakGrapple();
    }

    private void BreakGrapple()
    {
        // Enemies keep Frenzy bonuses (permanent stat changes already applied)
        _grappleActive = false;
        _grapplers.Clear();
        _sceneStage = 0;
        OnGrappleEnded?.Invoke();
    }

    #endregion

    // ====================================================================
    #region Passive Triggers
    // ====================================================================

    /// <summary>
    /// Lysandra — Blade Instinct:
    /// Active takes hit >= 20% max HP -> auto-attack at Very Low, 90% hit.
    /// 3-turn cooldown (Lysandra's turns).
    /// </summary>
    private void CheckLysandraPassive(RuntimeCharacterState attacker, int damageDealt)
    {
        var lysandra = FindSupportHeroine("lysandra");
        if (lysandra == null || !lysandra.IsAlive) return;
        if (!lysandra.IsPassiveReady("blade_instinct")) return;

        float threshold = ActiveHeroine.maxHP * 0.20f;
        if (damageDealt < threshold) return;

        lysandra.StartPassiveCooldown("blade_instinct", 3);
        OnPassiveTriggered?.Invoke("blade_instinct", lysandra);

        RuntimeCharacterState enemyTarget = (attacker != null && attacker.IsAlive && !attacker.isHeroine)
            ? attacker
            : null;

        if (enemyTarget == null)
        {
            foreach (var e in _enemies)
                if (e.IsAlive) { enemyTarget = e; break; }
        }

        if (enemyTarget == null) return;

        float power = CombatFormulas.PowerMultiplier(PowerBand.VeryLow);
        int hitChance = CombatFormulas.HitChance(90, lysandra.SPD, enemyTarget.SPD);

        if (CombatFormulas.RollHit(hitChance))
        {
            int damage = CombatFormulas.PhysicalDamage(
                lysandra.ATK, power, enemyTarget.DEF);
            enemyTarget.TakeDamage(damage);
            OnDamageDealt?.Invoke(lysandra, enemyTarget, damage);

            if (!enemyTarget.IsAlive)
                OnUnitDefeated?.Invoke(enemyTarget);
        }
        else
        {
            OnActionMiss?.Invoke();
        }
    }

    /// <summary>
    /// Seraphine — Divine Vigil:
    /// Active drops below 35% HP -> auto-heal 12 HP flat.
    /// 4-turn cooldown (Seraphine's turns).
    /// </summary>
    private void CheckSeraphinePassive()
    {
        var seraphine = FindSupportHeroine("seraphine");
        if (seraphine == null || !seraphine.IsAlive) return;
        if (!seraphine.IsPassiveReady("divine_vigil")) return;
        if (ActiveHeroine.HPPercent >= 0.35f) return;

        seraphine.StartPassiveCooldown("divine_vigil", 4);
        OnPassiveTriggered?.Invoke("divine_vigil", seraphine);

        ActiveHeroine.Heal(12);
        OnHealingDone?.Invoke(ActiveHeroine, 12);
    }

    // Note: Mira's Alchemist's Reflex is checked in TryApplyStatus(),
    // not here, because it intercepts BEFORE status application.

    #endregion

    // ====================================================================
    #region Enemy AI — Layer 1
    // ====================================================================

    private CombatAction DecideEnemyAction(RuntimeCharacterState enemy)
    {
        return enemy.characterId switch
        {
            "hollow_servant"    => AI_HollowServant(enemy),
            "knife_footman"     => AI_KnifeFootman(enemy),
            "prayer_rag_novice" => AI_PrayerRagNovice(enemy),
            "corrupted_butler"  => AI_CorruptedButler(enemy),
            "red_wax_acolyte"   => AI_RedWaxAcolyte(enemy),
            "blood_nun"         => AI_BloodNun(enemy),
            _                   => AI_Default(enemy)
        };
    }

    private CombatAction? TrySkill(
        RuntimeCharacterState enemy,
        string abilityId,
        RuntimeCharacterState target)
    {
        var ability = enemy.abilities.Find(a => a.abilityId == abilityId);
        if (ability == null) return null;
        if (enemy.currentMP < ability.mpCost) return null;
        if (ability.hpThresholdToUse > 0f &&
            (float)enemy.currentHP / enemy.maxHP > ability.hpThresholdToUse) return null;

        return new CombatAction
        {
            type    = ActionType.Skill,
            actor   = enemy,
            target  = target,
            ability = ability
        };
    }

    private CombatAction AttackAction(
        RuntimeCharacterState enemy, RuntimeCharacterState target)
        => new CombatAction { type = ActionType.Attack, actor = enemy, target = target };

    private CombatAction SkillAction(
        RuntimeCharacterState enemy, CharacterAbilitySO ability, RuntimeCharacterState target)
        => new CombatAction
        {
            type    = ActionType.Skill,
            actor   = enemy,
            target  = target,
            ability = ability
        };

    private CombatAction AI_HollowServant(RuntimeCharacterState enemy)
    {
        return TrySkill(enemy, "clutching_grab",    ActiveHeroine)
            ?? TrySkill(enemy, "shambling_strike",  ActiveHeroine)
            ?? AttackAction(enemy, ActiveHeroine);
    }

    private CombatAction AI_KnifeFootman(RuntimeCharacterState enemy)
    {
        bool targetBleeding = StatusEffectManager.HasStatus(ActiveHeroine, "bleed");

        if (!targetBleeding)
        {
            var thrust = TrySkill(enemy, "driven_thrust", ActiveHeroine);
            if (thrust.HasValue) return thrust.Value;
        }

        return TrySkill(enemy, "quick_slash", ActiveHeroine)
            ?? AttackAction(enemy, ActiveHeroine);
    }

    private CombatAction AI_PrayerRagNovice(RuntimeCharacterState enemy)
    {
        return TrySkill(enemy, "whisper_of_doubt", ActiveHeroine)
            ?? TrySkill(enemy, "dark_prayer",      ActiveHeroine)
            ?? AttackAction(enemy, ActiveHeroine);
    }

    private CombatAction AI_CorruptedButler(RuntimeCharacterState enemy)
    {
        if (!_grappleActive)
        {
            var embrace = TrySkill(enemy, "courteous_embrace", ActiveHeroine);
            if (embrace.HasValue) return embrace.Value;
        }

        return TrySkill(enemy, "silver_tray", ActiveHeroine)
            ?? AttackAction(enemy, ActiveHeroine);
    }

    private CombatAction AI_RedWaxAcolyte(RuntimeCharacterState enemy)
    {
        if (enemy.currentMP >= 4)
        {
            var blessing = enemy.abilities.Find(a => a.abilityId == "crimson_blessing");
            if (blessing != null)
            {
                var woundedAlly = FindWoundedAlly(enemy, 0.6f);
                if (woundedAlly != null)
                    return SkillAction(enemy, blessing, woundedAlly);
            }
        }

        if (enemy.currentMP >= 3)
        {
            var ward = enemy.abilities.Find(a => a.abilityId == "ember_ward");
            if (ward != null)
            {
                var buffTarget = FindUnbuffedHighAtkAlly(enemy);
                if (buffTarget != null)
                    return SkillAction(enemy, ward, buffTarget);
            }
        }

        return TrySkill(enemy, "wax_drip", ActiveHeroine)
            ?? AttackAction(enemy, ActiveHeroine);
    }

    private RuntimeCharacterState FindWoundedAlly(
        RuntimeCharacterState self, float hpThreshold)
    {
        RuntimeCharacterState lowest = null;
        float lowestPct = hpThreshold;

        foreach (var e in _enemies)
        {
            if (e == self || !e.IsAlive) continue;
            float pct = (float)e.currentHP / e.maxHP;
            if (pct < lowestPct) { lowestPct = pct; lowest = e; }
        }

        return lowest;
    }

    private RuntimeCharacterState FindUnbuffedHighAtkAlly(RuntimeCharacterState self)
    {
        RuntimeCharacterState best = null;
        int bestATK = 0;

        foreach (var e in _enemies)
        {
            if (e == self || !e.IsAlive) continue;
            if (StatusEffectManager.HasStatus(e, "def_up_enemy")) continue;
            if (e.ATK > bestATK) { bestATK = e.ATK; best = e; }
        }

        return best;
    }

    private CombatAction AI_BloodNun(RuntimeCharacterState enemy)
    {
        bool phase2 = (float)enemy.currentHP / enemy.maxHP < 0.5f;

        if (phase2)
        {
            if (!_bloodNunHealedPhase2)
            {
                var rite = TrySkill(enemy, "blood_rite", enemy);
                if (rite.HasValue)
                {
                    _bloodNunHealedPhase2 = true;
                    return rite.Value;
                }
            }

            if (ActiveHeroine.resolve <= 50)
            {
                var embrace = TrySkill(enemy, "sanctified_embrace", ActiveHeroine);
                if (embrace.HasValue) return embrace.Value;
            }

            return TrySkill(enemy, "flagellants_lash", ActiveHeroine)
                ?? AttackAction(enemy, ActiveHeroine);
        }

        if (_roundNumber == 1)
        {
            return TrySkill(enemy, "flagellants_lash", ActiveHeroine)
                ?? AttackAction(enemy, ActiveHeroine);
        }

        if (_roundNumber == 2)
        {
            return TrySkill(enemy, "communion", ActiveHeroine)
                ?? TrySkill(enemy, "flagellants_lash", ActiveHeroine)
                ?? AttackAction(enemy, ActiveHeroine);
        }

        if (ActiveHeroine.resolve <= 60)
        {
            var embrace = TrySkill(enemy, "sanctified_embrace", ActiveHeroine);
            if (embrace.HasValue) return embrace.Value;
        }

        if (_roundNumber % 2 == 0)
        {
            return TrySkill(enemy, "communion",        ActiveHeroine)
                ?? TrySkill(enemy, "flagellants_lash", ActiveHeroine)
                ?? AttackAction(enemy, ActiveHeroine);
        }

        return TrySkill(enemy, "flagellants_lash", ActiveHeroine)
            ?? TrySkill(enemy, "communion",        ActiveHeroine)
            ?? AttackAction(enemy, ActiveHeroine);
    }

    private CombatAction AI_Default(RuntimeCharacterState enemy)
    {
        foreach (var ability in enemy.abilities)
        {
            if (enemy.currentMP >= ability.mpCost &&
                (ability.hpThresholdToUse <= 0f ||
                 (float)enemy.currentHP / enemy.maxHP <= ability.hpThresholdToUse))
            {
                return SkillAction(enemy, ability, ActiveHeroine);
            }
        }
        return AttackAction(enemy, ActiveHeroine);
    }

    #endregion

    // ====================================================================
    #region Helpers
    // ====================================================================

    private RuntimeCharacterState FindSupportHeroine(string characterId)
    {
        for (int i = 0; i < _heroines.Count; i++)
        {
            if (i == _activeIndex) continue;
            if (_heroines[i].characterId == characterId)
                return _heroines[i];
        }
        return null;
    }

    public bool IsSupport(RuntimeCharacterState heroine)
    {
        int idx = _heroines.IndexOf(heroine);
        return idx >= 0 && idx != _activeIndex;
    }

    public int GetHeroineIndex(RuntimeCharacterState heroine)
    {
        return _heroines.IndexOf(heroine);
    }

    #endregion
}
