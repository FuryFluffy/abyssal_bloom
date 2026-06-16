// ── ItemAssetGenerator.cs ──────────────────────────────────────────────────
// EDITOR ONLY — lives in Assets/Editor/
// Usage: Unity menu → AbyssalBloom → Generate All Layer 1 Items
//
// Creates all 14 Layer 1 ItemSO assets in Assets/ScriptableObjects/Items/Layer1/
// Safe to run multiple times — skips assets that already exist.
// ───────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ItemAssetGenerator
{
    private const string ROOT = "Assets/ScriptableObjects/Items/Layer1";

    [MenuItem("AbyssalBloom/Generate All Layer 1 Items")]
    public static void GenerateAll()
    {
        // ── Healing Items ──────────────────────────────────────────────────

        Create("item_plain_bandage", "Plain Bandage",
            rarity:           ItemRarity.Common,
            category:         ItemCategory.Consumable,
            type:             ItemType.Healing,
            healHP:           15,
            statusesToRemove: new[] { "Bleed" },
            usableInCombat:   true,
            usableOutOfCombat: true,
            flavorText:       "A basic wrapping of clean cloth. Effective for simple cuts.",
            mainSource:       "Common early supplies");

        Create("item_servant_tonic", "Servant Tonic",
            rarity:           ItemRarity.Common,
            category:         ItemCategory.Consumable,
            type:             ItemType.MPRecovery,
            restoreMP:        12,
            statusesToAdd:    new[] { "Self_Conscious" },
            usableInCombat:   true,
            usableOutOfCombat: true,
            flavorText:       "A bitter herbal drink. Restores energy, but leaves you unsettled.",
            mainSource:       "Servant rooms");

        Create("item_l01_warm_wine_bottle", "Warm Wine Bottle",
            rarity:           ItemRarity.Uncommon,
            category:         ItemCategory.Consumable,
            type:             ItemType.Healing,
            healHP:           15,
            restoreMP:        8,
            corruptionChange: 3,
            usableInCombat:   true,
            usableOutOfCombat: true,
            flavorText:       "Warm, syrupy wine. Restores both body and mind, but tastes of the Castle's influence.",
            mainSource:       "Wine Cellar of Warm Bottles",
            onUseFlagKey:     "drank_warm_wine",
            onUseFlagScope:   "RunState");

        // ── Cleanse Items ──────────────────────────────────────────────────

        Create("item_smelling_salts", "Smelling Salts",
            rarity:           ItemRarity.Uncommon,
            category:         ItemCategory.Consumable,
            type:             ItemType.Cleanse,
            resolveChange:    3,
            statusesToRemove: new[] { "Drowsy*", "Self_Conscious", "Uneasy" },
            usableInCombat:   true,
            usableOutOfCombat: true,
            flavorText:       "Sharp, acrid crystals. Clears drowsiness and daze.",
            mainSource:       "Early supplies");

        Create("item_resolve_charm", "Resolve Charm",
            rarity:           ItemRarity.Uncommon,
            category:         ItemCategory.Consumable,
            type:             ItemType.Cleanse,
            resolveChange:    8,
            statusesToRemove: new[] { "Wavering", "Self_Conscious" },
            usableInCombat:   true,
            usableOutOfCombat: true,
            flavorText:       "A small charm inscribed with protective symbols.",
            mainSource:       "Early supplies / chapel");

        Create("item_cracked_ward_candle", "Cracked Ward Candle",
            rarity:           ItemRarity.Uncommon,
            category:         ItemCategory.Consumable,
            type:             ItemType.Cleanse,
            statusesToRemove: new[] { "Marked", "Stained" },
            usableInCombat:   true,
            usableOutOfCombat: true,
            flavorText:       "A partially melted candle that glows faintly.",
            mainSource:       "Chapel / blood-wax rooms");

        Create("item_l01_chapel_thread", "Chapel Thread",
            rarity:           ItemRarity.Uncommon,
            category:         ItemCategory.Consumable,
            type:             ItemType.Cleanse,
            resolveChange:    6,
            statusesToRemove: new[] { "Wavering", "Hushed" },
            usableInCombat:   true,
            usableOutOfCombat: true,
            flavorText:       "A thread of blessed cloth. Restores clarity of mind.",
            mainSource:       "Ruined Confessional / Novice");

        // ── Combat Tools ───────────────────────────────────────────────────

        Create("item_fire_oil_flask", "Fire-Oil Flask",
            rarity:           ItemRarity.Uncommon,
            category:         ItemCategory.CombatTool,
            type:             ItemType.Buff,
            statusesToAdd:    new[] { "Burning" },
            statusesToRemove: new[] { "Snared", "Restrained", "Bound" },
            usableInCombat:   true,
            usableOutOfCombat: false,
            flavorText:       "A volatile mixture. Ignites on contact; melts bonds.",
            mainSource:       "Caches / Mira supplies");

        Create("item_l01_polished_tray", "Polished Tray",
            rarity:           ItemRarity.Uncommon,
            category:         ItemCategory.CombatTool,
            type:             ItemType.Buff,
            statusesToAdd:    new[] { "Guarded" },
            usableInCombat:   true,
            usableOutOfCombat: false,
            flavorText:       "A gleaming serving tray, uselessly beautiful. Surprisingly durable.",
            mainSource:       "Butler / servant rooms");

        // ── Keys & Route Unlock ────────────────────────────────────────────

        Create("item_l01_service_key", "Service Key",
            rarity:           ItemRarity.Unique,
            category:         ItemCategory.Key,
            type:             ItemType.Currency,
            usableInCombat:   false,
            usableOutOfCombat: false,
            flavorText:       "A small brass key, worn from use. Unlocks servant passages.",
            mainSource:       "Bell-Pull Pantry / Butler route",
            onUseFlagKey:     "l01_service_key_obtained",
            onUseFlagScope:   "SaveSlot");

        // ── Crafting Materials ─────────────────────────────────────────────

        Create("item_l01_red_wax_chip", "Red-Wax Chip",
            rarity:           ItemRarity.Uncommon,
            category:         ItemCategory.CombatTool,
            type:             ItemType.Currency,
            usableInCombat:   false,
            usableOutOfCombat: false,
            flavorText:       "A fragment of congealed red wax. Used in crafting.",
            mainSource:       "Red-Wax Acolyte / Blood Nun");

        // ── Lore Items ─────────────────────────────────────────────────────

        Create("lore_l01_servant_ledger_page", "Servant Ledger Page",
            rarity:           ItemRarity.Unique,
            category:         ItemCategory.LoreItem,
            type:             ItemType.Currency,
            usableInCombat:   false,
            usableOutOfCombat: false,
            flavorText:       "A page from a ledger. Names of missing servants...",
            mainSource:       "Servant Ledger Alcove",
            onUseFlagKey:     "l01_servant_ledger_page_obtained",
            onUseFlagScope:   "SaveSlot");

        Create("lore_l01_torn_cuff", "Torn Cuff",
            rarity:           ItemRarity.Unique,
            category:         ItemCategory.LoreItem,
            type:             ItemType.Currency,
            usableInCombat:   false,
            usableOutOfCombat: false,
            flavorText:       "A torn cuff from a fine coat. Lysandra's style, perhaps?",
            mainSource:       "Coat Beside Service Door",
            onUseFlagKey:     "recognition_hint_torn_cuff",
            onUseFlagScope:   "SaveSlot");

        // ── Boss Rewards ───────────────────────────────────────────────────

        Create("boss_reward_l01_blood_nun_seal", "Blood Nun Seal",
            rarity:           ItemRarity.Unique,
            category:         ItemCategory.Key,
            type:             ItemType.Currency,
            usableInCombat:   false,
            usableOutOfCombat: false,
            flavorText:       "A seal of red wax bearing the Blood Nun's mark. Proof of survival.",
            mainSource:       "Blood Nun",
            onUseFlagKey:     "l01_blood_nun_defeated",
            onUseFlagScope:   "SaveSlot",
            secondFlagKey:    "l01_blood_nun_seal_obtained",
            secondFlagScope:  "SaveSlot");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[ItemAssetGenerator] All Layer 1 items created successfully.");
    }

    // ── Factory ────────────────────────────────────────────────────────────

    private static void Create(
        string itemId,
        string displayName,
        ItemRarity rarity             = ItemRarity.Common,
        ItemCategory category         = ItemCategory.Consumable,
        ItemType type                 = ItemType.Healing,
        int healHP                    = 0,
        int restoreMP                 = 0,
        int resolveChange             = 0,
        int corruptionChange          = 0,
        string[] statusesToAdd        = null,
        string[] statusesToRemove     = null,
        bool usableInCombat           = true,
        bool usableOutOfCombat        = true,
        bool canBreakGrapple          = false,
        string flavorText             = "",
        string mainSource             = "",
        string onUseFlagKey           = "",
        string onUseFlagScope         = "",
        string secondFlagKey          = "",
        string secondFlagScope        = "")
    {
        string path = $"{ROOT}/{itemId}.asset";

        // Skip if already exists
        if (AssetDatabase.LoadAssetAtPath<ItemSO>(path) != null)
        {
            Debug.Log($"[ItemAssetGenerator] Skipped (already exists): {path}");
            return;
        }

        // Ensure folder exists
        if (!AssetDatabase.IsValidFolder(ROOT))
        {
            string[] parts = ROOT.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }

        var so = ScriptableObject.CreateInstance<ItemSO>();
        so.itemId            = itemId;
        so.displayName       = displayName;
        so.rarity            = rarity;
        so.category          = category;
        so.type              = type;
        so.healHP            = healHP;
        so.restoreMP         = restoreMP;
        so.resolveChange     = resolveChange;
        so.corruptionChange  = corruptionChange;
        so.statusesToAdd     = statusesToAdd ?? new string[0];
        so.statusesToRemove  = statusesToRemove ?? new string[0];
        so.usableInCombat    = usableInCombat;
        so.usableOutOfCombat = usableOutOfCombat;
        so.canBreakGrapple   = canBreakGrapple;
        so.flavorText        = flavorText;
        so.mainSource        = mainSource;

        // Build onUseFlags array
        if (!string.IsNullOrEmpty(onUseFlagKey))
        {
            var flags = new System.Collections.Generic.List<ItemFlagEffect>();
            flags.Add(new ItemFlagEffect { 
                scope = ParseScope(onUseFlagScope), 
                key = onUseFlagKey, 
                value = "1" 
            });

            if (!string.IsNullOrEmpty(secondFlagKey))
            {
                flags.Add(new ItemFlagEffect { 
                    scope = ParseScope(secondFlagScope), 
                    key = secondFlagKey, 
                    value = "1" 
                });
            }

            so.onUseFlags = flags.ToArray();
        }
        else
        {
            so.onUseFlags = new ItemFlagEffect[0];
        }

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[ItemAssetGenerator] Created: {path}");
    }

    private static FlagManager.Scope ParseScope(string scope)
    {
        return scope switch
        {
            "RunState" => FlagManager.Scope.RunState,
            "SaveSlot" => FlagManager.Scope.SaveSlot,
            "PersistentKnowledge" => FlagManager.Scope.PersistentKnowledge,
            _ => FlagManager.Scope.RunState
        };
    }
}
#endif
