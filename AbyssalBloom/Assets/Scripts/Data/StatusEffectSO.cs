using UnityEngine;

// Create one asset per status effect (Assets > Create > AbyssalBloom > Status Effect).
// 45 total per design doc. All fields match the spec.
[CreateAssetMenu(fileName = "NewStatus", menuName = "AbyssalBloom/Status Effect")]
public class StatusEffectSO : ScriptableObject
{
    // ── Identity ───────────────────────────────────────────────────────────
    [Header("Identity")]
    [Tooltip("Unique machine ID, e.g. 'bleed', 'snared', 'comforted'")]
    public string statusId;
    public string displayName;

    [Tooltip("Logical group: Restraint / Comfort / Mental / DOT / Buff / Debuff / etc.")]
    public string group;

    public enum Category { Buff, Debuff, Restraint, Comfort }
    public Category category;

    // ── Targeting ──────────────────────────────────────────────────────────
    [Header("Targeting")]
    [Tooltip("True = only heroines can receive this status; False = enemies can also receive it")]
    public bool targetHeroine = true;

    // ── Visibility ─────────────────────────────────────────────────────────
    public enum Visibility { VisibleUI, VisibleWarning, DebugOnly }
    [Header("Visibility")]
    public Visibility visibility = Visibility.VisibleUI;

    // ── Duration ───────────────────────────────────────────────────────────
    public enum DurationType { Turns, Room, NextPressure, NextAction }
    [Header("Duration")]
    public DurationType durationType = DurationType.Turns;
    [Tooltip("Default duration in whichever unit durationType specifies")]
    public int defaultDuration = 2;

    // ── Stacking ───────────────────────────────────────────────────────────
    public enum StackingRule { RefreshDuration, ReplaceWeaker, ExclusiveGroup }
    [Header("Stacking")]
    public StackingRule stackingRule = StackingRule.RefreshDuration;
    [Tooltip("Maximum concurrent stacks. 1 = no stacking beyond one instance.")]
    public int maxStacks = 1;

    // ── Removal ────────────────────────────────────────────────────────────
    [Header("Removal")]
    [Tooltip("Can this status be removed by items or abilities?")]
    public bool removable = true;
    [Tooltip("Item IDs that can remove this status, e.g. 'item_purity_tonic'")]
    public string[] removedBy;

    // ── Mechanical Effects ─────────────────────────────────────────────────
    [Header("Mechanical Effects")]
    [TextArea(3, 6)]
    [Tooltip("Human-readable description of what this status does. " +
             "Actual stat changes are applied by the combat system reading this SO's data.")]
    public string mechanicalEffects;

    // ── Numeric Effect Data ────────────────────────────────────────────────
    // DECISION POINT: The simplest approach is keeping mechanicalEffects as a
    // description and having CombatManager switch on statusId to apply effects.
    // The alternative is a data-driven struct here (statId + delta + operation).
    // The switch approach is faster to get working; the struct approach is cleaner
    // for large status counts. For 45 statuses, either is fine — flag this if you
    // want the data-driven version later.
    [Header("Numeric DOT Values (for Bleed / Poison / similar)")]
    [Tooltip("Damage or Resolve loss per tick. 0 = not a DOT.")]
    public int dotAmountPerTick = 0;
    [Tooltip("If true this DOT ticks as Resolve damage, not HP damage")]
    public bool dotTargetsResolve = false;

    [Header("Stat Modifiers (flat delta applied while status is active)")]
    public int modATK;
    public int modDEF;
    public int modRES;
    public int modSPD;
    public int modMAG;
}
