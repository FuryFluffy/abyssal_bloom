using System;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// ItemSO — ScriptableObject definition for one item asset.
// Create via: Assets → Create → AbyssalBloom → Item
// ════════════════════════════════════════════════════════════════════════════

[CreateAssetMenu(fileName = "New Item", menuName = "AbyssalBloom/Item")]
public class ItemSO : ScriptableObject
{
    // ── Identity ───────────────────────────────────────────────────────────

    [Header("Identity")]
    public string      itemId;
    public string      displayName;
    public ItemRarity  rarity;

    // ── Mechanics ──────────────────────────────────────────────────────────

    [Header("Mechanics")]
    public ItemCategory category;
    public ItemType     type;
    public ItemTarget   target;

    // ── Numeric Effects ────────────────────────────────────────────────────

    [Header("Effects")]
    [Tooltip("HP restored on use (heroines only).")]
    public int healHP;

    [Tooltip("MP restored on use.")]
    public int restoreMP;

    [Tooltip("Positive = restore Resolve. Negative = lose Resolve.")]
    public int resolveChange;

    [Tooltip("Positive = gain Corruption. Negative = reduce Corruption (deferred).")]
    public int corruptionChange;

    // ── Status Management ──────────────────────────────────────────────────

    [Header("Status Management")]
    [Tooltip("statusIds to apply when this item is used. Must match a StatusEffectSO.statusId.")]
    public string[] statusesToAdd;

    [Tooltip(
        "statusIds to remove when this item is used. " +
        "Supports prefix wildcard: 'Restrain*' removes every status whose id starts with 'Restrain'. " +
        "Exact match otherwise.")]
    public string[] statusesToRemove;

    // ── Combat Availability ────────────────────────────────────────────────

    [Header("Combat")]
    [Tooltip("Can be used during a combat turn.")]
    public bool usableInCombat  = true;

    [Tooltip("Can be used on the map or in the Refuge hub.")]
    public bool usableOutOfCombat = true;

    // ── Grapple ────────────────────────────────────────────────────────────

    [Header("Grapple")]
    [Tooltip("If true, using this item instantly breaks all active grapples.")]
    public bool canBreakGrapple = false;

    // ── Visuals ────────────────────────────────────────────────────────────

    [Header("Visual")]
    public Sprite itemSprite;
    public Sprite itemIcon;

    [TextArea(2, 4)]
    public string flavorText;

    // ── Flag Effects ───────────────────────────────────────────────────────

    [Header("Flags")]
    [Tooltip("Flags to set in FlagManager when this item is used.")]
    public ItemFlagEffect[] onUseFlags;

    // ── Metadata (display only) ────────────────────────────────────────────

    [Header("Metadata")]
    [Tooltip("Where this item is typically found. Shown in tooltips.")]
    public string mainSource;
}

// ── Supporting enums ───────────────────────────────────────────────────────

public enum ItemRarity   { Common, Uncommon, Rare, Unique }
public enum ItemCategory { Consumable, CombatTool, Key, LoreItem }
public enum ItemType     { Healing, MPRecovery, Cleanse, Buff, GrappleBreaker, CorruptionReducer, Currency }
public enum ItemTarget   { Self, SingleEnemy, SingleAlly, AllAllies, AllEnemies }

// ── ItemFlagEffect ─────────────────────────────────────────────────────────
// Inline data class (not a ScriptableObject) so it serialises directly
// inside the ItemSO Inspector array without needing a separate asset.

[Serializable]
public class ItemFlagEffect
{
    public FlagManager.Scope scope;
    public string            key;
    public string            value = "1";
}
