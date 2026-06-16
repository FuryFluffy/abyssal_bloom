using System;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RoomHotspotSO — one hotspot variant in a room
// ════════════════════════════════════════════════════════════════════════════
// Defines a single possible interaction at a fixed anchor position.
// Examples: "HP Potion item at wine rack", "Ambush event near barrels"
//
// Hotspot type determines which header fields are relevant:
//   Item  → itemReward, itemPickupText
//   Door  → doorDestination, doorText
//   Event → eventBodyText, eventChoices
//   Lore  → loreTitle, loreBodyText, loreFlags, loreResolveChange
// ════════════════════════════════════════════════════════════════════════════

[CreateAssetMenu(menuName = "AbyssalBloom/Room Hotspot")]
public class RoomHotspotSO : ScriptableObject
{
    // ── Hotspot Type Enum ──────────────────────────────────────────────────

    public enum HotspotType { Item, Door, Event, Lore }

    // ── Identity ───────────────────────────────────────────────────────────

    [Header("Hotspot Identity")]
    [Tooltip("Unique ID, e.g. hotspot_item_hp_potion_l01")]
    public string hotspotId;

    public HotspotType type;

    // ── Display ────────────────────────────────────────────────────────────

    [Header("Display")]
    [Tooltip("Short label shown in UI, e.g. 'HP Potion'")]
    public string displayName;

    [TextArea(2, 4)]
    [Tooltip("Tooltip shown on hover, e.g. 'A curious bottle'")]
    public string hoverText;

    [Tooltip("Icon or sprite for this hotspot")]
    public Sprite hotspotSprite;

    // ══════════════════════════════════════════════════════════════════════
    // ITEM HOTSPOT (type = Item)
    // ══════════════════════════════════════════════════════════════════════

    [Header("Item Hotspot (if type = Item)")]
    [Tooltip("The item awarded when the player picks this up")]
    public ItemSO itemReward;

    [TextArea(2, 4)]
    [Tooltip("Flavour text shown during pickup dialog")]
    public string itemPickupText;

    // ══════════════════════════════════════════════════════════════════════
    // DOOR HOTSPOT (type = Door)
    // ══════════════════════════════════════════════════════════════════════

    [Header("Door Hotspot (if type = Door)")]
    [Tooltip("Room ID or 'next_room' — where this door leads")]
    public string doorDestination;

    [TextArea(2, 4)]
    [Tooltip("Short narration when the player exits through this door")]
    public string doorText;

    // ══════════════════════════════════════════════════════════════════════
    // EVENT HOTSPOT (type = Event)
    // ══════════════════════════════════════════════════════════════════════

    [Header("Event Hotspot (if type = Event)")]
    [TextArea(3, 8)]
    [Tooltip("Scene description shown above the choice buttons")]
    public string eventBodyText;

    [Tooltip("Choices the player can make for this event")]
    public EventChoice[] eventChoices;

    [Serializable]
    public class EventChoice
    {
        [Tooltip("Button label shown to the player")]
        public string choiceText;

        [TextArea(2, 5)]
        [Tooltip("Outcome narration shown after the choice is made")]
        public string outcomeText;

        [Tooltip("Flags that must be set for this choice to appear")]
        public FlagCondition[] requiredFlags;

        [Tooltip("Flags applied when this choice is resolved")]
        public FlagEffect[] flagEffects;

        // ── Stat changes ──────────────────────────────────────────────────
        [Tooltip("HP restored to the active heroine (positive only)")]
        public int healHP;

        [Tooltip("MP restored to the active heroine (positive only)")]
        public int restoreMP;

        [Tooltip("Resolve change (positive = restore, negative = lose)")]
        public int resolveChange;

        [Tooltip("Corruption change (positive = gain)")]
        public int corruptionChange;

        // ── Optional combat ───────────────────────────────────────────────
        [Tooltip("If non-empty, this choice immediately starts a combat encounter")]
        public EnemyDataSO[] encounterEnemies;

        [Tooltip("Parallel ability list for encounterEnemies")]
        public EncounterPoolSO.EnemyAbilityList[] encounterAbilities;
    }

    // ══════════════════════════════════════════════════════════════════════
    // LORE HOTSPOT (type = Lore)
    // ══════════════════════════════════════════════════════════════════════

    [Header("Lore Hotspot (if type = Lore)")]
    [Tooltip("Title shown in the lore reader panel")]
    public string loreTitle;

    [TextArea(5, 15)]
    [Tooltip("Full lore body text")]
    public string loreBodyText;

    [TextArea(1, 3)]
    [Tooltip("Optional heroine reaction line, e.g. \"Lysandra's eyes linger...\"")]
    public string characterReactionLine;

    [Tooltip("Flags applied when the lore is read")]
    public FlagEffect[] loreFlags;

    [Tooltip("Resolve change on reading this lore (positive = restore, negative = lose)")]
    public int loreResolveChange;

    // ══════════════════════════════════════════════════════════════════════
    // SHARED
    // ══════════════════════════════════════════════════════════════════════

    [Header("Shared")]
    [Tooltip("Which heroine IDs can interact with this hotspot. Empty = all heroines.")]
    public string[] heroineLock;

    [Tooltip("Hotspot only appears if ALL these flags are currently set")]
    public FlagCondition[] visibilityConditions;

    [Tooltip("True = hotspot disappears after one use; False = can be interacted with repeatedly")]
    public bool consumeAfterUse = true;

    // ── Flag helpers (mirrors RoomEventSO pattern) ─────────────────────────

    [Serializable]
    public class FlagCondition
    {
        public FlagManager.Scope scope;
        public string key;
        [Tooltip("Flag must equal this value to pass (default '1' = set)")]
        public string requiredValue = "1";
    }

    [Serializable]
    public class FlagEffect
    {
        public FlagManager.Scope scope;
        public string key;
        [Tooltip("Value to write (default '1' = set)")]
        public string value = "1";
    }

    // ── Condition check ────────────────────────────────────────────────────

    /// <summary>
    /// Returns true if all requiredFlags on an EventChoice are currently satisfied.
    /// Returns true (unlocked) when FlagManager is absent — safe for editor.
    /// </summary>
    public static bool AreConditionsMet(EventChoice choice)
    {
        if (choice.requiredFlags == null || choice.requiredFlags.Length == 0)
            return true;

        if (FlagManager.Instance == null)
            return true;

        foreach (var cond in choice.requiredFlags)
        {
            string actual = FlagManager.Instance.GetFlag(cond.scope, cond.key, "0");
            if (actual != cond.requiredValue) return false;
        }
        return true;
    }

    /// <summary>
    /// Returns true if all visibilityConditions on this hotspot are satisfied.
    /// Used by RoomManager / UI to decide whether to display the hotspot at all.
    /// </summary>
    public bool IsVisible()
    {
        if (visibilityConditions == null || visibilityConditions.Length == 0)
            return true;

        if (FlagManager.Instance == null)
            return true;

        foreach (var cond in visibilityConditions)
        {
            string actual = FlagManager.Instance.GetFlag(cond.scope, cond.key, "0");
            if (actual != cond.requiredValue) return false;
        }
        return true;
    }

    /// <summary>
    /// Apply the flagEffects from an EventChoice.
    /// </summary>
    public static void ApplyEffects(EventChoice choice)
    {
        if (choice.flagEffects == null || FlagManager.Instance == null) return;

        foreach (var effect in choice.flagEffects)
            FlagManager.Instance.SetFlag(effect.scope, effect.key, effect.value);
    }
}
