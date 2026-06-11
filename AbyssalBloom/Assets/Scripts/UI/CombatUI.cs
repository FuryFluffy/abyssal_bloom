using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// CombatUI — debug/test UI. Functional, not pretty.
// Zero combat logic. Subscribes to CombatManager events, reads state,
// calls SubmitAction() only.
//
// Wiring: create a Canvas, build the hierarchy described in the wiring guide,
// then drag every public field into this component's Inspector.
// ════════════════════════════════════════════════════════════════════════════

public class CombatUI : MonoBehaviour
{
    // ── References ─────────────────────────────────────────────────────────

    [Header("Combat Manager")]
    public CombatManager combatManager;

    // ── Heroine panels (one per slot: index 0 = active, 1 & 2 = supports) ─

    [Header("Heroine Panels (index 0 = active slot)")]
    public Text[] heroineNameText   = new Text[3];
    public Text[] heroineHPText     = new Text[3];
    public Text[] heroineMPText     = new Text[3];
    public Text[] heroineResolveText = new Text[3];
    public Text[] heroineCorruptText = new Text[3];
    public Image[] heroineHPBar     = new Image[3];   // fillAmount 0–1
    public Image[] heroineResolveBar = new Image[3];

    [Header("Active Heroine Indicator (highlights active slot)")]
    public Image activeIndicator; // colour-tinted panel behind slot 0

    // ── Enemy panel ────────────────────────────────────────────────────────
    // DECISION: one row per enemy, dynamically shown/hidden. Max display = 6.
    // For a single-enemy test, only the first row matters.

    [Header("Enemy Rows (one per possible enemy, 1–6)")]
    public Text[]  enemyNameText = new Text[6];
    public Text[]  enemyHPText   = new Text[6];
    public Image[] enemyHPBar    = new Image[6];
    public GameObject[] enemyRowRoot = new GameObject[6]; // parent to show/hide

    // ── Grapple / Stage display ────────────────────────────────────────────

    [Header("Grapple State")]
    public GameObject grapplePanel;    // shown only during grapple
    public Text       grappleStageText;

    // ── Round counter ──────────────────────────────────────────────────────

    [Header("Round Counter")]
    public Text roundText;

    // ── Action menu ────────────────────────────────────────────────────────
    // A generic pool of buttons. We enable/label only what's needed each turn.
    // Max 8 is enough: Attack/Defend/Skill/Item/Run + assist variants.

    [Header("Action Buttons (pool of 8)")]
    public Button[] actionButtons = new Button[8];
    public Text[]   actionButtonLabels = new Text[8];

    // Skill sub-menu: shown when the player picks "Skill" — lists abilities.
    [Header("Skill Sub-Menu (pool of 6)")]
    public GameObject skillMenuRoot;
    public Button[] skillButtons = new Button[6];
    public Text[]   skillButtonLabels = new Text[6];

    // ── Combat log ─────────────────────────────────────────────────────────

    [Header("Combat Log")]
    public ScrollRect logScrollRect;
    public Text       logText;
    private const int MaxLogLines = 200;
    private readonly System.Text.StringBuilder _logBuffer = new System.Text.StringBuilder();
    private int _logLineCount;

    // ── Internal state ─────────────────────────────────────────────────────

    private List<ActionType> _currentActions = new List<ActionType>();
    private RuntimeCharacterState _currentActor;
    private bool _awaitingInput;

    // ════════════════════════════════════════════════════════════════════════
    #region Lifecycle
    // ════════════════════════════════════════════════════════════════════════

    private void Awake()
    {
        // All buttons start disabled; they enable only on OnAwaitingAction.
        SetAllButtonsInteractable(false);
        if (skillMenuRoot != null) skillMenuRoot.SetActive(false);
        if (grapplePanel  != null) grapplePanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (combatManager == null) return;
        SubscribeEvents();
    }

    private void OnDisable()
    {
        if (combatManager == null) return;
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        combatManager.OnTurnStarted       += HandleTurnStarted;
        combatManager.OnAwaitingAction    += HandleAwaitingAction;
        combatManager.OnDamageDealt       += HandleDamageDealt;
        combatManager.OnHealingDone       += HandleHealingDone;
        combatManager.OnResolveLost       += HandleResolveLost;
        combatManager.OnCorruptionGained  += HandleCorruptionGained;
        combatManager.OnStatusApplied     += HandleStatusApplied;
        combatManager.OnStatusExpired     += HandleStatusExpired;
        combatManager.OnUnitDefeated      += HandleUnitDefeated;
        combatManager.OnGrappleStarted    += HandleGrappleStarted;
        combatManager.OnGrappleEnded      += HandleGrappleEnded;
        combatManager.OnSceneStageAdvanced+= HandleSceneStageAdvanced;
        combatManager.OnClimaxRecoil      += HandleClimaxRecoil;
        combatManager.OnPassiveTriggered  += HandlePassiveTriggered;
        combatManager.OnStruggleAttempt   += HandleStruggleAttempt;
        combatManager.OnEncounterEnd      += HandleEncounterEnd;
        combatManager.OnDotTick           += HandleDotTick;
        combatManager.OnRoundStarted      += HandleRoundStarted;
        combatManager.OnActionMiss        += HandleActionMiss;
    }

    private void UnsubscribeEvents()
    {
        combatManager.OnTurnStarted       -= HandleTurnStarted;
        combatManager.OnAwaitingAction    -= HandleAwaitingAction;
        combatManager.OnDamageDealt       -= HandleDamageDealt;
        combatManager.OnHealingDone       -= HandleHealingDone;
        combatManager.OnResolveLost       -= HandleResolveLost;
        combatManager.OnCorruptionGained  -= HandleCorruptionGained;
        combatManager.OnStatusApplied     -= HandleStatusApplied;
        combatManager.OnStatusExpired     -= HandleStatusExpired;
        combatManager.OnUnitDefeated      -= HandleUnitDefeated;
        combatManager.OnGrappleStarted    -= HandleGrappleStarted;
        combatManager.OnGrappleEnded      -= HandleGrappleEnded;
        combatManager.OnSceneStageAdvanced-= HandleSceneStageAdvanced;
        combatManager.OnClimaxRecoil      -= HandleClimaxRecoil;
        combatManager.OnPassiveTriggered  -= HandlePassiveTriggered;
        combatManager.OnStruggleAttempt   -= HandleStruggleAttempt;
        combatManager.OnEncounterEnd      -= HandleEncounterEnd;
        combatManager.OnDotTick           -= HandleDotTick;
        combatManager.OnRoundStarted      -= HandleRoundStarted;
        combatManager.OnActionMiss        -= HandleActionMiss;
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Event Handlers
    // ════════════════════════════════════════════════════════════════════════

    private void HandleRoundStarted(int round)
    {
        if (roundText != null) roundText.text = $"Round {round}";
        Log($"── Round {round} ──");
    }

    private void HandleTurnStarted(RuntimeCharacterState unit)
    {
        Log($"  {unit.displayName}'s turn");
        RefreshAllStats();
    }

    private void HandleAwaitingAction(
        RuntimeCharacterState unit, List<ActionType> actions)
    {
        _currentActor   = unit;
        _currentActions = actions;
        _awaitingInput  = true;

        BuildActionMenu(unit, actions);
    }

    private void HandleDamageDealt(
        RuntimeCharacterState src, RuntimeCharacterState tgt, int amount)
    {
        Log($"  {src.displayName} → {tgt.displayName}: {amount} damage");
        RefreshAllStats();
    }

    private void HandleHealingDone(RuntimeCharacterState tgt, int amount)
    {
        Log($"  {tgt.displayName} healed {amount} HP");
        RefreshAllStats();
    }

    private void HandleResolveLost(RuntimeCharacterState tgt, int amount)
    {
        Log($"  {tgt.displayName} lost {amount} Resolve");
        RefreshAllStats();
    }

    private void HandleCorruptionGained(RuntimeCharacterState tgt, int amount)
    {
        Log($"  {tgt.displayName} +{amount} Corruption");
        RefreshAllStats();
    }

    private void HandleStatusApplied(
        RuntimeCharacterState tgt, StatusEffectSO status, bool wasNegated)
    {
        if (wasNegated)
            Log($"  [NEGATED] {status.displayName} on {tgt.displayName}");
        else
            Log($"  {tgt.displayName} gained status: {status.displayName}");
    }

    private void HandleStatusExpired(
        RuntimeCharacterState tgt, StatusEffectSO status)
    {
        Log($"  {status.displayName} expired on {tgt.displayName}");
    }

    private void HandleUnitDefeated(RuntimeCharacterState unit)
    {
        Log($"  *** {unit.displayName} defeated! ***");
        RefreshAllStats();
    }

    private void HandleGrappleStarted()
    {
        Log("  [GRAPPLE STARTED]");
        if (grapplePanel != null) grapplePanel.SetActive(true);
        UpdateGrapplePanel();
    }

    private void HandleGrappleEnded()
    {
        Log("  [GRAPPLE ENDED]");
        if (grapplePanel != null) grapplePanel.SetActive(false);
    }

    private void HandleSceneStageAdvanced(int stage)
    {
        Log($"  Scene Stage → {stage}");
        UpdateGrapplePanel();
    }

    private void HandleClimaxRecoil(RuntimeCharacterState enemy, int damage)
    {
        Log($"  CLIMAX RECOIL — {enemy.displayName} takes {damage}");
        RefreshAllStats();
    }

    private void HandlePassiveTriggered(string passiveId, RuntimeCharacterState src)
    {
        Log($"  [PASSIVE] {src.displayName}: {passiveId}");
    }

    private void HandleStruggleAttempt(RuntimeCharacterState heroine, bool escaped)
    {
        Log(escaped
            ? $"  {heroine.displayName} ESCAPED the grapple!"
            : $"  {heroine.displayName} failed to escape.");
        RefreshAllStats();
    }

    private void HandleEncounterEnd(bool victory)
    {
        _awaitingInput = false;
        SetAllButtonsInteractable(false);
        Log(victory ? "═══ VICTORY ═══" : "═══ DEFEAT ═══");
    }

    private void HandleDotTick(RuntimeCharacterState unit)
    {
        Log($"  DOT tick on {unit.displayName}");
        RefreshAllStats();
    }

    private void HandleActionMiss()
    {
        Log("  MISS!");
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Action Menu
    // ════════════════════════════════════════════════════════════════════════

    private void BuildActionMenu(
        RuntimeCharacterState unit, List<ActionType> actions)
    {
        // Hide skill sub-menu
        if (skillMenuRoot != null) skillMenuRoot.SetActive(false);

        // Clear all buttons first
        SetAllButtonsInteractable(false);
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtonLabels[i] != null)
                actionButtonLabels[i].text = "";
        }

        // Populate buttons for each available action
        int slot = 0;
        foreach (var actionType in actions)
        {
            if (slot >= actionButtons.Length) break;

            string label = GetActionLabel(actionType, unit);
            int capturedSlot = slot;
            ActionType capturedType = actionType;

            actionButtonLabels[slot].text = label;
            actionButtons[slot].interactable = true;

            // Clear old listeners and add new one
            actionButtons[slot].onClick.RemoveAllListeners();
            actionButtons[slot].onClick.AddListener(() =>
                OnActionButtonClicked(capturedType, capturedSlot));

            slot++;
        }
    }

    private string GetActionLabel(ActionType type, RuntimeCharacterState unit)
    {
        return type switch
        {
            ActionType.Attack         => "Attack",
            ActionType.Defend         => "Defend",
            ActionType.Skill          => "Skill ▶",
            ActionType.UseItem        => "Item",
            ActionType.Run            => "Run",
            ActionType.AssistAttack   => "Assist Attack",
            ActionType.AssistAbility  => "Assist Ability",
            ActionType.SwapIn         => $"Swap In ({unit.displayName})",
            ActionType.Struggle       => "Struggle",
            ActionType.Submit         => "Submit",
            ActionType.Intervene      => "Intervene (8 MP)",
            ActionType.Watch          => "Watch (+5 Corrupt)",
            ActionType.Encourage      => "Encourage (+8 Corrupt)",
            _                         => type.ToString()
        };
    }

    private void OnActionButtonClicked(ActionType type, int buttonSlot)
    {
        if (!_awaitingInput) return;

        // Skill: open sub-menu instead of submitting immediately
        if (type == ActionType.Skill || type == ActionType.AssistAbility)
        {
            OpenSkillMenu(_currentActor);
            return;
        }

        SubmitActionForType(type);
    }

    private void OpenSkillMenu(RuntimeCharacterState actor)
    {
        if (skillMenuRoot == null) return;
        skillMenuRoot.SetActive(true);

        // Disable main action buttons while skill menu is open
        SetAllButtonsInteractable(false);

        var abilities = actor.abilities;
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < abilities.Count)
            {
                var ability = abilities[i];
                int capturedIdx = i;
                skillButtonLabels[i].text = $"{ability.displayName} ({ability.mpCost} MP)";
                skillButtons[i].interactable = (actor.currentMP >= ability.mpCost);
                skillButtons[i].onClick.RemoveAllListeners();
                skillButtons[i].onClick.AddListener(() => OnSkillChosen(capturedIdx));
            }
            else
            {
                skillButtonLabels[i].text = "";
                skillButtons[i].interactable = false;
                skillButtons[i].onClick.RemoveAllListeners();
            }
        }
    }

    private void OnSkillChosen(int abilityIndex)
    {
        if (skillMenuRoot != null) skillMenuRoot.SetActive(false);

        var ability = _currentActor.abilities[abilityIndex];

        // Determine action type: support heroines use AssistAbility, active use Skill
        bool isActive = (_currentActor == combatManager.ActiveHeroine);
        ActionType type = isActive ? ActionType.Skill : ActionType.AssistAbility;

        var action = new CombatAction
        {
            type    = type,
            actor   = _currentActor,
            target  = PickDefaultTarget(ability),
            ability = ability
        };

        CommitAction(action);
    }

    private void SubmitActionForType(ActionType type)
    {
        RuntimeCharacterState target = null;

        // Only attack-style actions need a target; grapple/support actions don't.
        switch (type)
        {
            case ActionType.Attack:
            case ActionType.AssistAttack:
                target = FirstAliveEnemy();
                break;

            case ActionType.SwapIn:
                // actor IS the support heroine to swap in; no separate target field needed
                break;
        }

        var action = new CombatAction
        {
            type   = type,
            actor  = _currentActor,
            target = target
        };

        CommitAction(action);
    }

    private void CommitAction(CombatAction action)
    {
        _awaitingInput = false;
        SetAllButtonsInteractable(false);
        if (skillMenuRoot != null) skillMenuRoot.SetActive(false);
        combatManager.SubmitAction(action);
    }

    // ── Target selection ───────────────────────────────────────────────────
    // TODO: Replace with a proper click-to-select target system.
    // For now, all attack/skill actions auto-target the first alive enemy.

    private RuntimeCharacterState PickDefaultTarget(CharacterAbilitySO ability)
    {
        // Healing abilities target the Active heroine (or actor if no Active)
        if (ability.abilityType == CharacterAbilitySO.AbilityType.Healing)
            return combatManager.ActiveHeroine;

        return FirstAliveEnemy();
    }

    private RuntimeCharacterState FirstAliveEnemy()
    {
        foreach (var e in combatManager.Enemies)
            if (e.IsAlive) return e;
        return null;
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Stat Refresh
    // ════════════════════════════════════════════════════════════════════════

    private void RefreshAllStats()
    {
        RefreshHeroinePanels();
        RefreshEnemyPanels();
    }

    private void RefreshHeroinePanels()
    {
        var heroines = combatManager.Heroines;

        for (int i = 0; i < 3; i++)
        {
            if (i >= heroines.Count) break;
            var h = heroines[i];

            // Reorder display: active heroine always in slot 0 visually.
            // DECISION: we show heroines in party-slot order (0,1,2) and
            // highlight the active slot with activeIndicator instead of
            // reordering panels. Simpler; revisit if UI needs reorder.

            bool isActive = (h == combatManager.ActiveHeroine);
            string prefix = isActive ? "► " : "  ";

            if (heroineNameText[i]    != null) heroineNameText[i].text    = prefix + h.displayName;
            if (heroineHPText[i]      != null) heroineHPText[i].text      = $"HP {h.currentHP}/{h.maxHP}";
            if (heroineMPText[i]      != null) heroineMPText[i].text      = $"MP {h.currentMP}/{h.maxMP}";
            if (heroineResolveText[i] != null) heroineResolveText[i].text = $"RES {h.resolve}/{h.maxResolve}";
            if (heroineCorruptText[i] != null) heroineCorruptText[i].text = $"COR {h.corruption}";

            if (heroineHPBar[i]      != null)
                heroineHPBar[i].fillAmount = h.maxHP > 0 ? (float)h.currentHP / h.maxHP : 0f;
            if (heroineResolveBar[i] != null)
                heroineResolveBar[i].fillAmount = h.maxResolve > 0 ? (float)h.resolve / h.maxResolve : 0f;
        }
    }

    private void RefreshEnemyPanels()
    {
        var enemies = combatManager.Enemies;

        for (int i = 0; i < enemyRowRoot.Length; i++)
        {
            bool hasEnemy = (i < enemies.Count);
            if (enemyRowRoot[i] != null) enemyRowRoot[i].SetActive(hasEnemy);

            if (!hasEnemy) continue;

            var e = enemies[i];
            if (enemyNameText[i] != null) enemyNameText[i].text = e.IsAlive ? e.displayName : $"[Dead] {e.displayName}";
            if (enemyHPText[i]   != null) enemyHPText[i].text   = $"HP {e.currentHP}/{e.maxHP}";
            if (enemyHPBar[i]    != null)
                enemyHPBar[i].fillAmount = e.maxHP > 0 ? (float)e.currentHP / e.maxHP : 0f;
        }
    }

    private void UpdateGrapplePanel()
    {
        if (grappleStageText != null)
            grappleStageText.text = $"GRAPPLE — Stage {combatManager.SceneStage}";
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Helpers
    // ════════════════════════════════════════════════════════════════════════

    private void SetAllButtonsInteractable(bool state)
    {
        foreach (var btn in actionButtons)
            if (btn != null) btn.interactable = state;
        foreach (var btn in skillButtons)
            if (btn != null) btn.interactable = state;
    }

    private void Log(string line)
    {
        if (logText == null) return;

        _logLineCount++;
        if (_logLineCount > MaxLogLines)
        {
            // Trim oldest ~50 lines to avoid unbounded growth
            int trimAt = _logBuffer.ToString().IndexOf('\n', 50 * 40);
            if (trimAt > 0)
            {
                _logBuffer.Remove(0, trimAt + 1);
                _logLineCount = MaxLogLines - 50;
            }
        }

        _logBuffer.AppendLine(line);
        logText.text = _logBuffer.ToString();

        // Scroll to bottom
        if (logScrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            logScrollRect.verticalNormalizedPosition = 0f;
        }
    }

    #endregion
}
