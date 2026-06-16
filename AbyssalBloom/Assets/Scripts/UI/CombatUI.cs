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
    public Image[] heroineHPBar      = new Image[3];   // fillAmount 0–1
    public Image[] heroineMPBar      = new Image[3];   // fillAmount 0–1
    public Image[] heroineResolveBar = new Image[3];
    public Image[] heroineCorruptBar = new Image[3];   // fillAmount 0–1

    [Header("Active Heroine Indicator + Portrait")]
    public Image activeIndicator;       // colour-tinted panel behind slot 0
    public Image activeHeroinePortrait; // large portrait on right side

    // ── Enemy panel ────────────────────────────────────────────────────────
    // DECISION: one row per enemy, dynamically shown/hidden. Max display = 6.
    // For a single-enemy test, only the first row matters.

    [Header("Enemy Rows (one per possible enemy, 1–6)")]
    public Text[]  enemyNameText = new Text[6];
    public Text[]  enemyHPText   = new Text[6];
    public Text[]  enemyMPText   = new Text[6];
    public Text[]  enemyCORText  = new Text[6];
    public Image[] enemyHPBar    = new Image[6];
    public Image[] enemyMPBar    = new Image[6];
    public Image[] enemyCORBar   = new Image[6];
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

    // Special sub-menu: shown when the player picks "Special" — lists specials.
    [Header("Special Sub-Menu (pool of 6)")]
    public GameObject specialMenuRoot;
    public Button[] specialButtons = new Button[6];
    public Text[]   specialButtonLabels = new Text[6];

    // ── Inventory slots ──────────────────────────────────────────────────
    // 6 item slots shown in the item sub-menu during combat.

    [Header("Inventory Slots (6)")]
    public GameObject[] inventorySlots    = new GameObject[6];
    public Image[]      inventoryIcons    = new Image[6];
    public Text[]       inventoryNameTexts = new Text[6];
    public Text[]       inventoryQtyTexts  = new Text[6];

    // Root panel that wraps all inventory slots (shown/hidden as a unit).
    [Header("Item Menu Root")]
    public GameObject itemMenuRoot;

    // ── Combat log ─────────────────────────────────────────────────────────

    [Header("Combat Log")]
    public ScrollRect logScrollRect;
    public Text       logText;
    public Button     logToggleButton;  // collapse/expand
    public GameObject logBodyPanel;     // toggled active/inactive
    private const int MaxLogLines = 200;
    private readonly System.Text.StringBuilder _logBuffer = new System.Text.StringBuilder();
    private int _logLineCount;

    // ── Internal state ─────────────────────────────────────────────────────

    private List<ActionType> _currentActions = new List<ActionType>();
    private RuntimeCharacterState _currentActor;
    private bool _awaitingInput;

    // ── Target selection state ─────────────────────────────────────────────

    private bool _selectingTarget;
    private System.Action<RuntimeCharacterState> _onTargetSelected;
    private ItemSO _pendingItem;   // non-null when selecting a target for UseItem

    // Cached Button components added to enemy row roots for click-to-select.
    private Button[] _enemyClickButtons;

    // Highlight colour applied to enemy name text during target selection.
    private static readonly Color TargetHighlightColour = new Color(1f, 0.85f, 0.2f); // gold
    private Color[] _enemyNameOriginalColours;

    // Cached Button components added to inventory slot roots for item clicking.
    private Button[] _inventorySlotButtons;

    // ════════════════════════════════════════════════════════════════════════
    #region Lifecycle
    // ════════════════════════════════════════════════════════════════════════

    private void Awake()
    {
        // All buttons start disabled; they enable only on OnAwaitingAction.
        SetAllButtonsInteractable(false);
        if (skillMenuRoot   != null) skillMenuRoot.SetActive(false);
        if (specialMenuRoot != null) specialMenuRoot.SetActive(false);
        if (itemMenuRoot    != null) itemMenuRoot.SetActive(false);
        if (grapplePanel    != null) grapplePanel.SetActive(false);

        // Wire log collapse/expand toggle
        if (logToggleButton != null)
            logToggleButton.onClick.AddListener(ToggleLogBody);

        // Wire enemy row click buttons for target selection.
        _enemyClickButtons       = new Button[enemyRowRoot.Length];
        _enemyNameOriginalColours = new Color[enemyRowRoot.Length];

        for (int i = 0; i < enemyRowRoot.Length; i++)
        {
            if (enemyRowRoot[i] == null) continue;

            // Capture original name text colour for restoration.
            if (enemyNameText[i] != null)
                _enemyNameOriginalColours[i] = enemyNameText[i].color;

            var btn = enemyRowRoot[i].GetComponent<Button>()
                   ?? enemyRowRoot[i].AddComponent<Button>();
            btn.transition  = Selectable.Transition.None; // visual handled by name text colour
            btn.interactable = false; // only active during target selection
            int captured = i;
            btn.onClick.AddListener(() => OnEnemyPanelClicked(captured));
            _enemyClickButtons[i] = btn;
        }

        // Wire inventory slot click buttons for item selection.
        _inventorySlotButtons = new Button[inventorySlots.Length];
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null) continue;

            var btn = inventorySlots[i].GetComponent<Button>()
                   ?? inventorySlots[i].AddComponent<Button>();
            btn.transition  = Selectable.Transition.ColorTint;
            btn.interactable = false;
            int captured = i;
            btn.onClick.AddListener(() => OnItemChosen(captured));
            _inventorySlotButtons[i] = btn;
        }
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
        combatManager.OnForcedSwapRequired+= HandleForcedSwapRequired;
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
        combatManager.OnForcedSwapRequired-= HandleForcedSwapRequired;
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

    private void HandleForcedSwapRequired(List<RuntimeCharacterState> candidates)
    {
        _awaitingInput = true;
        Log("  *** Choose a heroine to take the Active slot ***");
        BuildForcedSwapMenu(candidates);
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

    private void HandleEncounterEnd(EncounterResult result)
    {
        _awaitingInput = false;
        SetAllButtonsInteractable(false);
        ExitTargetSelection(); // clean up any in-progress selection
        switch (result)
        {
            case EncounterResult.Victory:
                Log("═══ VICTORY ═══");
                break;
            case EncounterResult.Fled:
                Log("═══ ESCAPED ═══");
                break;
            case EncounterResult.Defeated:
                Log("═══ DEFEAT ═══");
                break;
        }
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
        // Hide all sub-menus
        if (skillMenuRoot != null) skillMenuRoot.SetActive(false);
        if (specialMenuRoot != null) specialMenuRoot.SetActive(false);
        if (itemMenuRoot  != null) itemMenuRoot.SetActive(false);

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

    /// <summary>
    /// Replaces the normal action menu with one button per candidate heroine.
    /// Clicking a button calls SubmitForcedSwap() with that heroine.
    /// Uses the same action button pool — no extra UI needed.
    /// </summary>
    private void BuildForcedSwapMenu(List<RuntimeCharacterState> candidates)
    {
        if (skillMenuRoot  != null) skillMenuRoot.SetActive(false);
        if (specialMenuRoot != null) specialMenuRoot.SetActive(false);
        if (itemMenuRoot   != null) itemMenuRoot.SetActive(false);

        SetAllButtonsInteractable(false);
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtonLabels[i] != null)
                actionButtonLabels[i].text = "";
        }

        for (int i = 0; i < candidates.Count && i < actionButtons.Length; i++)
        {
            var heroine = candidates[i];
            actionButtonLabels[i].text =
                $"Swap In: {heroine.displayName}  HP {heroine.currentHP}/{heroine.maxHP}";
            actionButtons[i].interactable = true;

            actionButtons[i].onClick.RemoveAllListeners();
            actionButtons[i].onClick.AddListener(() => OnForcedSwapChosen(heroine));
        }
    }

    private void OnForcedSwapChosen(RuntimeCharacterState heroine)
    {
        if (!_awaitingInput) return;

        _awaitingInput = false;
        SetAllButtonsInteractable(false);
        Log($"  → {heroine.displayName} steps forward.");
        combatManager.SubmitForcedSwap(heroine);
    }

    private string GetActionLabel(ActionType type, RuntimeCharacterState unit)
    {
        return type switch
        {
            ActionType.Attack         => "Attack",
            ActionType.Defend         => "Defend",
            ActionType.Skill          => "Skill ▶",
            ActionType.UseItem        => "Item ▶",
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

        // Skill / AssistAbility: open skill sub-menu
        if (type == ActionType.Skill || type == ActionType.AssistAbility)
        {
            OpenSkillMenu(_currentActor);
            return;
        }

        // UseItem: open item sub-menu
        if (type == ActionType.UseItem)
        {
            OpenItemMenu();
            return;
        }

        SubmitActionForType(type);
    }

    // ── Skill sub-menu ─────────────────────────────────────────────────────

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

        if (AbilityNeedsEnemyTarget(ability))
        {
            // Enter click-to-select; callback builds and commits the action.
            EnterTargetSelection(target =>
            {
                var action = new CombatAction
                {
                    type    = type,
                    actor   = _currentActor,
                    target  = target,
                    ability = ability
                };
                CommitAction(action);
            });
        }
        else
        {
            // Healing / Buff: resolve target immediately, no click needed.
            var action = new CombatAction
            {
                type    = type,
                actor   = _currentActor,
                target  = PickNonEnemyTarget(ability),
                ability = ability
            };
            CommitAction(action);
        }
    }

    // ── Item sub-menu ──────────────────────────────────────────────────────

    private void OpenItemMenu()
    {
        if (itemMenuRoot == null) return;
        itemMenuRoot.SetActive(true);

        // Disable main action buttons while item menu is open
        SetAllButtonsInteractable(false);

        var inventory = ItemManager.Instance != null
            ? ItemManager.Instance.GetInventory()
            : new List<ItemSO>();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null) continue;

            bool hasItem = (i < inventory.Count) && (inventory[i] != null);
            inventorySlots[i].SetActive(hasItem);

            if (!hasItem) continue;

            var item = inventory[i];
            bool usable = item.usableInCombat;

            if (inventoryNameTexts[i] != null)
                inventoryNameTexts[i].text = item.displayName;
            if (inventoryQtyTexts[i] != null)
                inventoryQtyTexts[i].text = ""; // quantity not tracked per-SO; clear for now
            if (inventoryIcons[i] != null && item.itemIcon != null)
                inventoryIcons[i].sprite = item.itemIcon;

            if (_inventorySlotButtons[i] != null)
                _inventorySlotButtons[i].interactable = usable;
        }
    }

    private void OnItemChosen(int slotIndex)
    {
        var inventory = ItemManager.Instance != null
            ? ItemManager.Instance.GetInventory()
            : new List<ItemSO>();

        if (slotIndex >= inventory.Count || inventory[slotIndex] == null) return;
        var item = inventory[slotIndex];

        // Belt-and-suspenders guard (button should already be greyed)
        if (!item.usableInCombat) return;

        // Close item menu before proceeding
        if (itemMenuRoot != null) itemMenuRoot.SetActive(false);

        if (item.target == ItemTarget.SingleEnemy || item.target == ItemTarget.AllEnemies)
        {
            // Need player to click an enemy target (AllEnemies still picks a target to satisfy
            // ItemManager.UseItem signature — CombatManager.ResolveUseItem broadcasts to all).
            _pendingItem = item;
            EnterTargetSelection(target =>
            {
                var action = new CombatAction
                {
                    type   = ActionType.UseItem,
                    actor  = _currentActor,
                    target = target,   // AllEnemies: CombatManager broadcasts; target used as fallback
                    item   = _pendingItem
                };
                _pendingItem = null;
                CommitAction(action);
            });
        }
        else
        {
            // Self, SingleAlly, AllAllies — no enemy click needed.
            var action = new CombatAction
            {
                type   = ActionType.UseItem,
                actor  = _currentActor,
                target = ResolveItemTarget(item),
                item   = item
            };
            CommitAction(action);
        }
    }

    private RuntimeCharacterState ResolveItemTarget(ItemSO item)
    {
        return item.target switch
        {
            ItemTarget.Self       => _currentActor,
            ItemTarget.SingleAlly => combatManager.ActiveHeroine,
            ItemTarget.AllAllies  => null,   // CombatManager.ResolveUseItem broadcasts to all
            ItemTarget.AllEnemies => null,   // CombatManager.ResolveUseItem broadcasts to all
            _                     => _currentActor
        };
    }

    // ── Submit ─────────────────────────────────────────────────────────────

    private void SubmitActionForType(ActionType type)
    {
        RuntimeCharacterState target = null;

        switch (type)
        {
            case ActionType.Attack:
            case ActionType.AssistAttack:
                int aliveCount = CountAliveEnemies();
                if (aliveCount <= 1)
                {
                    // Single enemy (or none) — auto-select.
                    target = FirstAliveEnemy();
                    var action = new CombatAction
                    {
                        type   = type,
                        actor  = _currentActor,
                        target = target
                    };
                    CommitAction(action);
                }
                else
                {
                    // Multiple enemies — require player click.
                    ActionType capturedType = type;
                    EnterTargetSelection(t =>
                    {
                        var action = new CombatAction
                        {
                            type   = capturedType,
                            actor  = _currentActor,
                            target = t
                        };
                        CommitAction(action);
                    });
                }
                return;

            case ActionType.SwapIn:
                // actor IS the support heroine to swap in; no separate target field needed
                break;
        }

        var plainAction = new CombatAction
        {
            type   = type,
            actor  = _currentActor,
            target = target
        };
        CommitAction(plainAction);
    }

    private void CommitAction(CombatAction action)
    {
        _awaitingInput = false;
        SetAllButtonsInteractable(false);
        if (skillMenuRoot  != null) skillMenuRoot.SetActive(false);
        if (specialMenuRoot != null) specialMenuRoot.SetActive(false);
        if (itemMenuRoot   != null) itemMenuRoot.SetActive(false);
        ExitTargetSelection();
        combatManager.SubmitAction(action);
    }

    // ── Target selection (click-to-select enemy) ───────────────────────────

    private void EnterTargetSelection(System.Action<RuntimeCharacterState> callback)
    {
        _selectingTarget   = true;
        _onTargetSelected  = callback;

        Log("  [Select a target]");

        var enemies = combatManager.Enemies;
        for (int i = 0; i < _enemyClickButtons.Length; i++)
        {
            if (_enemyClickButtons[i] == null) continue;

            bool alive = (i < enemies.Count) && enemies[i].IsAlive;
            _enemyClickButtons[i].interactable = alive;

            // Highlight alive enemy name text
            if (alive && enemyNameText[i] != null)
                enemyNameText[i].color = TargetHighlightColour;
        }
    }

    private void ExitTargetSelection()
    {
        _selectingTarget  = false;
        _onTargetSelected = null;

        // Disable all enemy click buttons and restore name text colours.
        for (int i = 0; i < _enemyClickButtons.Length; i++)
        {
            if (_enemyClickButtons[i] != null)
                _enemyClickButtons[i].interactable = false;

            if (enemyNameText[i] != null)
                enemyNameText[i].color = _enemyNameOriginalColours[i];
        }
    }

    private void OnEnemyPanelClicked(int enemyIndex)
    {
        if (!_selectingTarget) return;

        var enemies = combatManager.Enemies;
        if (enemyIndex >= enemies.Count || !enemies[enemyIndex].IsAlive) return;

        var selected = enemies[enemyIndex];
        var callback = _onTargetSelected;

        ExitTargetSelection();
        callback?.Invoke(selected);
    }

    // ── Target helpers ─────────────────────────────────────────────────────

    /// <summary>Returns true when the ability should target an enemy (needs click-select).</summary>
    private bool AbilityNeedsEnemyTarget(CharacterAbilitySO ability)
    {
        return ability.abilityType != CharacterAbilitySO.AbilityType.Healing
            && ability.abilityType != CharacterAbilitySO.AbilityType.Healing;
    }

    /// <summary>Resolves a non-enemy target for healing/buff abilities.</summary>
    private RuntimeCharacterState PickNonEnemyTarget(CharacterAbilitySO ability)
    {
        if (ability.abilityType == CharacterAbilitySO.AbilityType.Healing)
            return combatManager.ActiveHeroine;

        return _currentActor; // Buff defaults to self
    }

    private RuntimeCharacterState FirstAliveEnemy()
    {
        foreach (var e in combatManager.Enemies)
            if (e.IsAlive) return e;
        return null;
    }

    private int CountAliveEnemies()
    {
        int count = 0;
        foreach (var e in combatManager.Enemies)
            if (e.IsAlive) count++;
        return count;
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Stat Refresh
    // ════════════════════════════════════════════════════════════════════════

    private void RefreshAllStats()
    {
        RefreshHeroinePanels();
        RefreshEnemyPanels();
        RefreshInventorySlots();
    }

    private void RefreshHeroinePanels()
    {
        var heroines = combatManager.Heroines;

        for (int i = 0; i < 3; i++)
        {
            if (i >= heroines.Count) break;
            var h = heroines[i];

            bool isActive = (h == combatManager.ActiveHeroine);
            string prefix = isActive ? "► " : "  ";

            if (heroineNameText[i]    != null) heroineNameText[i].text    = prefix + h.displayName;
            if (heroineHPText[i]      != null) heroineHPText[i].text      = $"HP {h.currentHP}/{h.maxHP}";
            if (heroineMPText[i]      != null) heroineMPText[i].text      = $"MP {h.currentMP}/{h.maxMP}";
            if (heroineResolveText[i] != null) heroineResolveText[i].text = $"RES {h.resolve}/{h.maxResolve}";
            if (heroineCorruptText[i] != null) heroineCorruptText[i].text = $"COR {h.corruption}";

            if (heroineHPBar[i]      != null)
                heroineHPBar[i].fillAmount = h.maxHP > 0 ? (float)h.currentHP / h.maxHP : 0f;
            if (heroineMPBar[i]      != null)
                heroineMPBar[i].fillAmount = h.maxMP > 0 ? (float)h.currentMP / h.maxMP : 0f;
            if (heroineResolveBar[i] != null)
                heroineResolveBar[i].fillAmount = h.maxResolve > 0 ? (float)h.resolve / h.maxResolve : 0f;
            if (heroineCorruptBar[i] != null)
                heroineCorruptBar[i].fillAmount = h.maxResolve > 0 ? (float)h.corruption / h.maxResolve : 0f;
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
            if (enemyMPText[i]   != null) enemyMPText[i].text   = $"MP {e.currentMP}/{e.maxMP}";
            if (enemyCORText[i]  != null) enemyCORText[i].text   = $"COR {e.corruption}";
            if (enemyHPBar[i]    != null)
                enemyHPBar[i].fillAmount = e.maxHP > 0 ? (float)e.currentHP / e.maxHP : 0f;
            if (enemyMPBar[i]    != null)
                enemyMPBar[i].fillAmount = e.maxMP > 0 ? (float)e.currentMP / e.maxMP : 0f;
            if (enemyCORBar[i]   != null)
                enemyCORBar[i].fillAmount = e.maxResolve > 0 ? (float)e.corruption / e.maxResolve : 0f;
        }
    }

    /// <summary>
    /// Populates inventory slot labels from ItemManager.
    /// Called from RefreshAllStats() so the item menu stays current.
    /// Also disables slot GameObjects for empty slots.
    /// </summary>
    private void RefreshInventorySlots()
    {
        if (ItemManager.Instance == null) return;

        var inventory = ItemManager.Instance.GetInventory();
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null) continue;

            bool hasItem = (i < inventory.Count) && (inventory[i] != null);
            inventorySlots[i].SetActive(hasItem);

            if (!hasItem) continue;

            var item = inventory[i];
            if (inventoryNameTexts[i] != null) inventoryNameTexts[i].text = item.displayName;
            if (inventoryIcons[i] != null && item.itemIcon != null)
                inventoryIcons[i].sprite = item.itemIcon;
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
        foreach (var btn in specialButtons)
            if (btn != null) btn.interactable = state;
        // Inventory slot buttons are managed separately (only active in item menu)
    }

    private void ToggleLogBody()
    {
        if (logBodyPanel == null) return;
        logBodyPanel.SetActive(!logBodyPanel.activeSelf);

        // Update toggle button label
        if (logToggleButton != null)
        {
            var label = logToggleButton.GetComponentInChildren<Text>();
            if (label != null)
                label.text = logBodyPanel.activeSelf ? "▼" : "▲";
        }
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
