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

public enum EncounterResult { Victory, Fled, Defeated }

// ── Action data submitted by UI ────────────────────────────────────────────

[System.Serializable]
public struct CombatAction
{
    public ActionType             type;
    public RuntimeCharacterState  actor;
    public RuntimeCharacterState  target;   // null for some actions
    public CharacterAbilitySO     ability;  // only for ActionType.Skill / AssistAbility
    public ItemSO                 item;     // only for ActionType.UseItem
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
    public event Action<EncounterResult>                                  OnEncounterEnd;
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
            OnEncounterEnd?.Invoke(EncounterResult.Victory);
            return true;
        }

        // All heroines at 0 HP → defeat
        bool heroinesAlive = false;
        foreach (var h in _heroines)
            if (h.IsAlive) { heroinesAlive = true; break; }

        if (!heroinesAlive)
        {
            _phase = CombatPhase.EncounterLost;
            OnEncounterEnd?.Invoke(EncounterResult.Defeated);
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
                ResolveRun(action.actor);
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
                ResolveUseItem(action);
                break;
        }
    }

    private void ResolveUseItem(CombatAction action)
    {
        if (action.item == null)
        {
            Debug.LogWarning("[CombatManager] UseItem: action.item is null.");
            return;
        }

        switch (action.item.target)
        {
            case ItemTarget.AllAllies:
                // ItemManager.UseItem requires a concrete target — broadcast to each living heroine.
                foreach (var h in _heroines)
                {
                    if (h.IsAlive)
                        ItemManager.Instance.UseItem(action.item, h);
                }
                break;

            case ItemTarget.AllEnemies:
                // Broadcast to each living enemy.
                // NOTE: UseItem removes the item after the first call if it is a Consumable.
                // AllEnemies consumables are therefore expected to be non-consumable CombatTools
                // or the designer must be aware only the first enemy call triggers removal.
                // TODO: add a multi-target variant to ItemManager when AllEnemies consumables are designed.
                foreach (var e in _enemies)
                {
                    if (e.IsAlive)
                        ItemManager.Instance.UseItem(action.item, e);
                }
                break;

            default:
                // Self, SingleAlly, SingleEnemy — target already resolved by CombatUI.
                if (action.target != null)
                    ItemManager.Instance.UseItem(action.item, action.target);
                else
                    Debug.LogWarning($"[CombatManager] UseItem: null target for item '{action.item.displayName}' with target type {action.item.target}.");
                break;
        }
    }

    // ── Flee ───────────────────────────────────────────────────────────────

    private void ResolveRun(RuntimeCharacterState actor)
    {
        // Flee chance: base 30%, modified by active heroine SPD vs total enemy SPD.
        // Clamped to [10, 70] — never trivial, never impossible.
        int totalEnemySPD = 0;
        foreach (var e in _enemies)
            if (e.IsAlive) totalEnemySPD += e.SPD;

        int fleeChance = Mathf.Clamp(
            30 + (actor.SPD - totalEnemySPD) * 2,
            10, 70);

        bool fled = CombatFormulas.RollHit(fleeChance);

        if (fled)
        {
            _phase = CombatPhase.EncounterWon;   // stops turn loop
            OnEncounterEnd?.Invoke(EncounterResult.Fled);
        }
        else
        {
            // Failed flee — log it, turn ends normally
            Debug.Log($"[CombatManager] Flee failed (chance was {fleeChance}%).");
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
        if (target.isDefending) damage = Mathf.RoundToInt(damage * CombatFormulas.DefendDamageMultiplier);

        ApplyDamage(attacker, target, damage);
    }

    // ── Assist Attack ──────────────────────────────────────────────────────

    private void ResolveAssistAttack(
        RuntimeCharacterState attacker, RuntimeCharacterState target)
    {
        if (target == null || !target.IsAlive) return;

        float power = CombatFormulas.PowerMultiplier(PowerBand.Low);
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
        if (actor.currentMP < ability.mpCost)
        {
            Debug.Log($"[CombatManager] {actor.displayName} can't afford {ability.displayName} ({ability.mpCost} MP).");
            return;
        }

        actor.SpendMP(ability.mpCost);

        switch (ability.abilityType)
        {
            case CharacterAbilitySO.AbilityType.Physical:
                ResolveOffensiveAbility(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.Healing:
                ResolveHealingAbility(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.Grapple:
                ResolveGrappleAbility(actor, target, ability);
                break;

            case CharacterAbilitySO.AbilityType.ResolveAttack:
            case CharacterAbilitySO.AbilityType.CorruptionAttack:
            case CharacterAbilitySO.AbilityType.Magic:
                ResolveOffensiveAbility(actor, target, ability);
                break;
        }
    }

    private void ResolveOffensiveAbility(
        RuntimeCharacterState actor,
        RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null || !target.IsAlive) return;

        int hitChance = CombatFormulas.HitChance(ability.baseHitChance, actor.SPD, target.SPD);
        if (!CombatFormulas.RollHit(hitChance))
        {
            OnActionMiss?.Invoke();
            return;
        }

        float power = CombatFormulas.PowerMultiplier(ability.powerBand);

        if (ability.abilityType == CharacterAbilitySO.AbilityType.Physical)
        {
            int damage = CombatFormulas.PhysicalDamage(actor.ATK, power, target.DEF);
            if (target.isDefending) damage = Mathf.RoundToInt(damage * CombatFormulas.DefendDamageMultiplier);
            ApplyDamage(actor, target, damage);
        }
        else if (ability.abilityType == CharacterAbilitySO.AbilityType.Magic)
        {
            int damage = CombatFormulas.MagicDamage(actor.MAG, power, target.RES);
            ApplyDamage(actor, target, damage);
        }

        if (ability.baseResolveDamage != 0)
            ApplyResolveDelta(target, -ability.baseResolveDamage);

        if (ability.baseCorruptionGain != 0)
            ApplyCorruptionDelta(target, ability.baseCorruptionGain);

        ApplyAbilityStatuses(actor, target, ability);
    }

    private void ResolveHealingAbility(
        RuntimeCharacterState actor,
        RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null) return;

        int amount = CombatFormulas.Healing(ability.baseHeal, actor.MAG, CombatFormulas.PowerMultiplier(ability.powerBand));
        target.Heal(amount);
        OnHealingDone?.Invoke(target, amount);

        ApplyAbilityStatuses(actor, target, ability);
    }

    private void ResolveBuffAbility(
        RuntimeCharacterState actor,
        RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null) return;
        ApplyAbilityStatuses(actor, target, ability);
    }

    private void ResolveGrappleAbility(
        RuntimeCharacterState actor,
        RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (target == null || !target.IsAlive) return;

        int hitChance = CombatFormulas.HitChance(ability.baseHitChance, actor.SPD, target.SPD);
        if (!CombatFormulas.RollHit(hitChance))
        {
            OnActionMiss?.Invoke();
            return;
        }

        StartGrapple(actor, target);
        ApplyAbilityStatuses(actor, target, ability);
    }

    private void ApplyAbilityStatuses(
        RuntimeCharacterState actor,
        RuntimeCharacterState target,
        CharacterAbilitySO ability)
    {
        if (ability.statusEffect == null) return;
        int statusChance = CombatFormulas.StatusChance(ability.statusBaseChance, actor.MAG, target.RES);
        if (CombatFormulas.RollStatus(statusChance))
        {
            bool negated = StatusEffectManager.Apply(target, ability.statusEffect, actor);
            OnStatusApplied?.Invoke(target, ability.statusEffect, negated);
        }
    }

    // ── Swap In ────────────────────────────────────────────────────────────

    private void ResolveSwapIn(RuntimeCharacterState newActive)
    {
        int idx = _heroines.IndexOf(newActive);
        if (idx < 0 || idx == _activeIndex || !newActive.IsAlive)
        {
            Debug.LogWarning("[CombatManager] ResolveSwapIn: invalid swap target.");
            return;
        }
        _activeIndex = idx;
        Debug.Log($"[CombatManager] Swapped to {newActive.displayName}.");
    }

    // ── Grapple ────────────────────────────────────────────────────────────

    private void StartGrapple(
        RuntimeCharacterState grappler, RuntimeCharacterState target)
    {
        if (_grappleActive) return;
        _grappleActive = true;
        _grapplers.Clear();
        _grapplers.Add(grappler);
        _sceneStage = 0;
        OnGrappleStarted?.Invoke();
    }

    private void ProcessGrappleAction(RuntimeCharacterState grappler)
    {
        var target = ActiveHeroine;

        _sceneStage++;
        OnSceneStageAdvanced?.Invoke(_sceneStage);

        int resolveHit = CombatFormulas.GrappleResolveDamage(grappler.MAG, target.RES);
        ApplyResolveDelta(target, -resolveHit);

        if (_sceneStage >= 3)  // Stage 3 = climax
        {
            int recoil = CombatFormulas.ClimaxRecoilDamage(grappler.maxHP, ActiveHeroine.corruption);
            ApplyDamage(grappler, grappler, recoil);
            OnClimaxRecoil?.Invoke(grappler, recoil);
            EndGrapple();
        }
    }

    private void EndGrapple()
    {
        _grappleActive = false;
        _grapplers.Clear();
        _sceneStage = 0;
        OnGrappleEnded?.Invoke();
    }

    private void ResolveStruggle(RuntimeCharacterState actor)
    {
        if (!_grappleActive || _grapplers.Count == 0) return;
        var grappler = _grapplers[0];

        int escapeChance = CombatFormulas.StruggleChance(actor.ATK, grappler.ATK, ResolveBand.Steady);  // TODO: read actual resolve band
        bool escaped = CombatFormulas.RollHit(escapeChance);

        OnStruggleAttempt?.Invoke(actor, escaped);
        if (escaped) EndGrapple();
    }

    private void ResolveSubmit(RuntimeCharacterState actor)
    {
        if (!_grappleActive) return;
        ApplyCorruptionDelta(actor, CombatFormulas.SubmitCorruptionGain(actor.RES));
        EndGrapple();
    }

    private void ResolveIntervene(RuntimeCharacterState supporter)
    {
        if (!_grappleActive) return;
        const int interveneCost = 8;
        if (supporter.currentMP < interveneCost) return;
        supporter.SpendMP(interveneCost);
        EndGrapple();
    }

    private void ResolveWatch(RuntimeCharacterState supporter)
    {
        ApplyCorruptionDelta(supporter, 2);  // Watch: +2 Corruption (master ref Section 6)
    }

    private void ResolveEncourage(RuntimeCharacterState supporter)
    {
        ApplyCorruptionDelta(supporter, 4);  // Encourage: +4 Corruption (master ref Section 6)
        if (_grappleActive)
        {
            _sceneStage++;
            OnSceneStageAdvanced?.Invoke(_sceneStage);
        }
    }

    // ── Damage / Resource Helpers ──────────────────────────────────────────

    private void ApplyDamage(
        RuntimeCharacterState source,
        RuntimeCharacterState target,
        int amount)
    {
        if (amount <= 0) return;
        target.TakeDamage(amount);
        OnDamageDealt?.Invoke(source, target, amount);

        if (!target.IsAlive)
            HandleKnockout(target);
    }

    private void ApplyResolveDelta(RuntimeCharacterState target, int delta)
    {
        if (delta < 0)
        {
            target.LoseResolve(-delta);
            OnResolveLost?.Invoke(target, -delta);
        }
        else if (delta > 0)
        {
            target.RestoreResolve(delta);
        }
    }

    private void ApplyCorruptionDelta(RuntimeCharacterState target, int delta)
    {
        if (delta > 0)
        {
            target.GainCorruption(delta);
            OnCorruptionGained?.Invoke(target, delta);
        }
    }

    private void HandleKnockout(RuntimeCharacterState unit)
    {
        OnUnitDefeated?.Invoke(unit);

        if (!unit.isHeroine) return;

        // A support heroine knocked out — remove from grapple if involved
        if (_grapplers.Contains(unit))
            EndGrapple();

        // Active heroine knocked out — require forced swap
        if (unit == ActiveHeroine)
        {
            var candidates = new List<RuntimeCharacterState>();
            foreach (var h in _heroines)
            {
                if (h != unit && h.IsAlive)
                    candidates.Add(h);
            }

            if (candidates.Count > 0)
            {
                _phase = CombatPhase.AwaitingForcedSwap;
                OnForcedSwapRequired?.Invoke(candidates);
                // Combat is now paused; SubmitForcedSwap() will resume it.
            }
            // If no candidates, CheckEncounterEnd() will catch the defeat.
        }
    }

    #endregion

    // ====================================================================
    #region Passive Triggers
    // ====================================================================

    private void TryTriggerPassive(string passiveId, RuntimeCharacterState source)
    {
        OnPassiveTriggered?.Invoke(passiveId, source);
    }

    #endregion

    // ====================================================================
    #region Enemy AI
    // ====================================================================

    private CombatAction DecideEnemyAction(RuntimeCharacterState enemy)
    {
        return enemy.characterId switch
        {
            "hollow_servant"   => AI_HollowServant(enemy),
            "knife_footman"    => AI_KnifeFootman(enemy),
            "prayer_rag_novice"=> AI_PrayerRagNovice(enemy),
            "corrupted_butler" => AI_CorruptedButler(enemy),
            "red_wax_acolyte"  => AI_RedWaxAcolyte(enemy),
            "blood_nun"        => AI_BloodNun(enemy),
            _                  => AI_Default(enemy)
        };
    }

    private CombatAction? TrySkill(
        RuntimeCharacterState enemy,
        string abilityId,
        RuntimeCharacterState target)
    {
        var ability = enemy.abilities.Find(a => a.abilityId == abilityId);
        if (ability == null || enemy.currentMP < ability.mpCost) return null;
        // hpThresholdToUse not on CharacterAbilitySO — threshold filtering deferred

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
            if (enemy.currentMP >= ability.mpCost) // hpThresholdToUse deferred — all abilities available
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
