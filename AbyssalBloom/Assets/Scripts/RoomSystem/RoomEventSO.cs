using System;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RoomEventSO — one event room's content
// ════════════════════════════════════════════════════════════════════════════
// Create via Assets > Create > AbyssalBloom > Room Event
//
// Holds display text, 2–4 choices, and per-choice flag conditions + effects.
// Resolve/Corruption gating is done through flag conditions — not hardcoded.
// Example: a choice requiring Corruption ≥ 40 would have a flag condition
// that checks a dynamically-set flag like "active_corruption_band_tempted".
//
// DESIGN NOTE: Flag conditions use FlagManager string keys.  At room entry,
// RoomManager sets temporary run_state flags for the Active heroine's current
// Resolve/Corruption bands so event choices can gate on them generically.
// This keeps gating data-driven per the spec.
// ════════════════════════════════════════════════════════════════════════════

[CreateAssetMenu(fileName = "NewRoomEvent", menuName = "AbyssalBloom/Room Event")]
public class RoomEventSO : ScriptableObject
{
    [Header("Event Identity")]
    public string eventId;
    public string displayTitle;

    [Header("Narrative Text")]
    [TextArea(4, 12)]
    public string bodyText;

    [Header("Choices (2–4)")]
    public EventChoice[] choices;

    // ── Choice ─────────────────────────────────────────────────────────────

    [Serializable]
    public class EventChoice
    {
        [Tooltip("Button label the player sees.")]
        public string choiceText;

        [TextArea(2, 6)]
        [Tooltip("Outcome narration shown after choosing.")]
        public string outcomeText;

        [Header("Conditions (ALL must be met to show this choice)")]
        [Tooltip("Each entry is a FlagManager key that must equal '1' for this choice to appear.")]
        public FlagCondition[] requiredFlags;

        [Header("Effects (applied when this choice is selected)")]
        public FlagEffect[] flagEffects;

        [Header("Optional Encounter")]
        [Tooltip("If set, selecting this choice triggers a combat encounter with this enemy group.")]
        public EnemyDataSO[] encounterEnemies;

        [Tooltip("Abilities for encounterEnemies (parallel array).")]
        public EncounterPoolSO.EnemyAbilityList[] encounterAbilities;

        [Header("Optional Stat Changes")]
        [Tooltip("Heal the active heroine by this amount.  0 = no heal.")]
        public int healHP;
        [Tooltip("Restore MP by this amount.  0 = no restore.")]
        public int restoreMP;
        [Tooltip("Resolve change (positive = restore, negative = lose).")]
        public int resolveChange;
        [Tooltip("Corruption change (positive = gain).")]
        public int corruptionChange;
    }

    // ── Flag condition ─────────────────────────────────────────────────────

    [Serializable]
    public class FlagCondition
    {
        public FlagManager.Scope scope;
        public string key;

        [Tooltip("The value the flag must have.  Defaults to '1' (flag is set).")]
        public string requiredValue = "1";
    }

    // ── Flag effect ────────────────────────────────────────────────────────

    [Serializable]
    public class FlagEffect
    {
        public FlagManager.Scope scope;
        public string key;

        [Tooltip("The value to set.  '1' for a boolean flag, or any string.")]
        public string value = "1";
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns true if all required flags for a choice are satisfied.
    /// </summary>
    public static bool AreConditionsMet(EventChoice choice)
    {
        if (choice.requiredFlags == null || choice.requiredFlags.Length == 0)
            return true;

        if (FlagManager.Instance == null) return true;

        foreach (var cond in choice.requiredFlags)
        {
            string actual = FlagManager.Instance.GetFlag(cond.scope, cond.key, "0");
            if (actual != cond.requiredValue) return false;
        }

        return true;
    }

    /// <summary>
    /// Apply all flag effects for a choice.
    /// </summary>
    public static void ApplyEffects(EventChoice choice)
    {
        if (choice.flagEffects == null || FlagManager.Instance == null) return;

        foreach (var effect in choice.flagEffects)
        {
            FlagManager.Instance.SetFlag(effect.scope, effect.key, effect.value);
        }
    }
}
