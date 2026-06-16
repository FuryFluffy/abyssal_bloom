// ── Layer1HotspotAssetGenerator.cs ────────────────────────────────────────
// EDITOR ONLY — place in Assets/Editor/
// Usage: Unity menu → AbyssalBloom → Generate Layer 1 Hotspot SOs
//
// Creates all Layer 1 RoomHotspotSO and RoomHotspotPoolSO assets from the
// Godot event CSV data (room_event_implementation_v0_1.csv, Layer 1 rows).
//
// Asset layout:
//   Assets/ScriptableObjects/Rooms/Layer1/Hotspots/  ← event, lore, door hotspots
//   Assets/ScriptableObjects/Rooms/Layer1/Pools/     ← pool SOs
//
// Safe to run multiple times — skips assets that already exist.
//
// NOTE ON STATUS EFFECTS:
//   RoomHotspotSO.EventChoice has no statusesToApply[] field.
//   The "uneasy" and "wavering" statuses noted in the CSV are tracked here
//   as flagEffects (e.g. "uneasy_applied" = "1") so the EventUI can read
//   them and call StatusEffectManager. Add a statusesToApply[] field to
//   EventChoice and wire it up when the status application path is built.
//
// NOTE ON ITEM REWARDS:
//   ItemSO does not yet exist. Item hotspots are NOT generated here —
//   they depend on Sub-Chat A (Item System). When ItemSOs are ready,
//   add a second generator method for item hotspots and pools.
// ──────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class Layer1HotspotAssetGenerator
{
    private const string ROOT     = "Assets/ScriptableObjects/Rooms/Layer1";
    private const string HOTSPOTS = "Assets/ScriptableObjects/Rooms/Layer1/Hotspots";
    private const string POOLS    = "Assets/ScriptableObjects/Rooms/Layer1/Pools";

    [MenuItem("AbyssalBloom/Generate Layer 1 Hotspot SOs")]
    public static void GenerateAll()
    {
        EnsureFolder(ROOT);
        EnsureFolder(HOTSPOTS);
        EnsureFolder(POOLS);

        // ── 1. Servant Ledger Alcove ──────────────────────────────────────
        // CSV: l01_servant_ledger_alcove | lore_discovery | soft_lore_seed
        // Two choices: inspect_ledger, leave
        // No blocked flags to set (visibility open).
        // Guardrail: does NOT set missing_men_truth_known.

        var ledgerLore = CreateLore(
            id:          "l01_servant_ledger_alcove",
            displayName: "Servant Ledger Alcove",
            hoverText:   "An open ledger, names half-scratched out",
            loreTitle:   "Servant Registry — Lower Castle",
            loreBody:
                "The ledger is open to a recent page. Names fill the columns in careful copperplate: " +
                "Aldric. Fenwick. Doss. Halvard. Each name crossed through with a single horizontal " +
                "stroke — not erased, just marked done. The dates beside them trail off three months " +
                "ago. The pages after that are blank.\n\n" +
                "A smaller note in the margin reads: *reassigned to upper service.* " +
                "The handwriting is the same throughout. Whoever kept this record kept it faithfully " +
                "until they stopped.",
            reactionLine: "The names sit in your memory longer than you expect.",
            resolveChange: -3,  // very low Resolve pressure as noted in CSV effects
            loreFlags: new[] {
                MakeFlag(FlagManager.Scope.RunState, "l01_ledger_names_seen", "1")
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]  // no blocked flags
        );

        // ── 2. Wine Cellar of Warm Bottles ────────────────────────────────
        // CSV: l01_wine_cellar_warm_bottles | risk_reward | early_recovery_with_hidden_cost
        // Choices: drink, take_bottle, leave
        // Drink: +HP, +MP, hidden_feeding_flag_minor, uneasy status flag
        // Guardrail: do NOT reveal feeding; warmth should feel suspicious but useful.

        var wineEvent = CreateEvent(
            id:          "l01_wine_cellar_warm_bottles",
            displayName: "Wine Cellar of Warm Bottles",
            hoverText:   "Bottles warming on stone racks",
            bodyText:
                "A forgotten cellar, cut from bare stone. Bottles line the warming racks in orderly " +
                "rows — dozens of them, still warm to the touch. The wax seals are unbroken. " +
                "The air smells of dark fruit and something sweet underneath, something that doesn't " +
                "quite belong. You uncork one. Steam rises from the dark liquid inside.",
            choices: new[]
            {
                MakeChoice(
                    text: "Drink",
                    outcome:
                        "The wine is thick and warm. It settles in your belly like a blanket, " +
                        "loosening tired muscles, dulling the ache behind your eyes. " +
                        "Your body thanks you. Something in the taste lingers, though — " +
                        "sweet in a way that doesn't match the year on the label. " +
                        "Not poison. Something else entirely.",
                    healHP:       15,
                    restoreMP:    8,
                    resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "hidden_feeding_flag_minor", "1"),
                        MakeFlag(FlagManager.Scope.RunState, "uneasy_applied", "1")
                    }
                ),
                MakeChoice(
                    text: "Take a bottle",
                    outcome:
                        "You pocket one of the sealed bottles. It's warm through the glass, " +
                        "heavier than it looks. You can use it later — or not.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "item_warm_wine_bottle_taken", "1")
                    }
                    // itemReward: item_l01_warm_wine_bottle — wire when ItemSO exists
                ),
                MakeChoice(
                    text: "Leave it",
                    outcome: "You back away from the racks. The cellar stays undisturbed.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new RoomHotspotSO.FlagEffect[0]
                )
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // Pool for Wine Cellar events (single-event pool for now; expand when
        // encounter variants and "nothing" variant are added)
        var winePool = CreatePool(
            id:           "pool_wine_cellar_events",
            displayName:  "Pool_WineCellar_Events",
            hotspotType:  RoomHotspotSO.HotspotType.Event,
            pickCount:    1,
            allowDupes:   false,
            variants:     new[] { wineEvent }
        );

        // ── 3. Servant Dormitory ──────────────────────────────────────────
        // CSV: l01_servant_dormitory | false_rest | early_false_rest
        // Choices: rest, search, leave
        // Rest: minor HP recovery, uneasy status flag
        // Search: minor lore scrap (flag only — no item yet)
        // Guardrail: no hidden feeding by default.

        var dormEvent = CreateEvent(
            id:          "l01_servant_dormitory",
            displayName: "Servant Dormitory",
            hoverText:   "A row of narrow cots, tidily made",
            bodyText:
                "A long room of narrow cots, six to a wall, each one made with the same mechanical " +
                "precision — corners tucked, blankets smoothed, personal items absent. The room smells " +
                "faintly of candle smoke and old wool. It looks like a rest stop. It feels like something " +
                "has been waiting here for a long time.",
            choices: new[]
            {
                MakeChoice(
                    text: "Rest",
                    outcome:
                        "You lie down. The cot is narrow but the blanket is warm. " +
                        "Sleep comes faster than it should. You wake stiff, moderately restored, " +
                        "and with an odd certainty that something in the room shifted while you slept. " +
                        "Nothing is out of place. Nothing is different. You tell yourself that.",
                    healHP:        12,
                    restoreMP:     0,
                    resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "uneasy_applied", "1"),
                        MakeFlag(FlagManager.Scope.RunState, "l01_dormitory_rested", "1")
                    }
                ),
                MakeChoice(
                    text: "Search the room",
                    outcome:
                        "Under the last cot: a folded page. A duty roster. Two names on it match " +
                        "names you've seen crossed out somewhere else. You pocket it.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l01_dormitory_roster_found", "1")
                    }
                ),
                MakeChoice(
                    text: "Leave",
                    outcome: "You don't touch anything. The room watches you go.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new RoomHotspotSO.FlagEffect[0]
                )
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        var dormPool = CreatePool(
            id:          "pool_servant_dormitory_events",
            displayName: "Pool_ServantDormitory_Events",
            hotspotType: RoomHotspotSO.HotspotType.Event,
            pickCount:   1,
            allowDupes:  false,
            variants:    new[] { dormEvent }
        );

        // ── 4. Ruined Confessional ────────────────────────────────────────
        // CSV: l01_ruined_confessional | corruption_resolve_pressure | seraphine_doctrine_pressure
        // Choices: confess, break_silence, leave
        // Confess: Resolve check — if Seraphine, wavering risk; successful resistance = +Resolve
        // Break silence: minor Resolve loss but resists pressure
        // Guardrail: doctrine pressure only; do NOT set blessed_hypocrisy_flag.

        var confessEvent = CreateEvent(
            id:          "l01_ruined_confessional",
            displayName: "Ruined Confessional",
            hoverText:   "A carved booth, screen still intact",
            bodyText:
                "The confessional stands alone in the broken chapel — wood still solid, " +
                "the carved screen between booths uncracked. The kneeler is worn smooth by " +
                "many knees. You hear nothing from the other side. But the booth is waiting. " +
                "It has been waiting for a long time, and it does not feel empty.",
            choices: new[]
            {
                MakeChoice(
                    text: "Confess",
                    outcome:
                        "You kneel. Words come — things you didn't intend to say, " +
                        "things you thought you'd set aside. The booth receives them without comment. " +
                        "The silence on the other side presses back. Not absolution. Not judgment. " +
                        "Something older than either.",
                    healHP:        0,
                    restoreMP:     0,
                    resolveChange: -8,  // wavering pressure; Seraphine-specific flavour in narration
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "wavering_applied", "1"),
                        MakeFlag(FlagManager.Scope.RunState, "l01_confessional_spoken", "1")
                    }
                ),
                MakeChoice(
                    text: "Break the silence",
                    outcome:
                        "You knock once on the screen, hard. The booth says nothing. " +
                        "The pressure in the air breaks. Your head clears. Whatever it wanted, " +
                        "it didn't get it.",
                    healHP:        0,
                    restoreMP:     0,
                    resolveChange: 5,  // small Resolve recovery for successful resistance
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l01_confessional_resisted", "1")
                    }
                ),
                MakeChoice(
                    text: "Leave",
                    outcome: "You don't kneel. The booth watches you go.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new RoomHotspotSO.FlagEffect[0]
                )
            },
            heroineLock: new[] { "seraphine" },  // Seraphine-specific pressure
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        var confessPool = CreatePool(
            id:          "pool_ruined_confessional_events",
            displayName: "Pool_RuinedConfessional_Events",
            hotspotType: RoomHotspotSO.HotspotType.Event,
            pickCount:   1,
            allowDupes:  false,
            variants:    new[] { confessEvent }
        );

        // ── 5. Coat Beside the Service Door ──────────────────────────────
        // CSV: l01_coat_beside_service_door | recognition_hint | lysandra_loved_one_seed
        // Choices: inspect_coat, ignore
        // Inspect: recognition_hint_torn_cuff flag if Lysandra; optional Resolve pressure
        // Guardrail: loved-one hint only; do NOT reveal Vowbroken Duelist identity fully.

        var coatEvent = CreateEvent(
            id:          "l01_coat_beside_service_door",
            displayName: "Coat Beside the Service Door",
            hoverText:   "A coat hanging by the service door",
            bodyText:
                "A wool coat hangs from a hook beside the service door — good quality, " +
                "dark grey, neatly hung by someone who cared for it. The left cuff is torn. " +
                "Not frayed from wear. Torn, sharply, as if caught on something and pulled free. " +
                "The rest of the coat is undamaged.",
            choices: new[]
            {
                MakeChoice(
                    text: "Inspect the coat",
                    outcome:
                        "The lining is worn at the shoulder from carrying something heavy. " +
                        "The torn cuff — you know this cut. You've seen it before, on someone who " +
                        "moved their left hand like that, who always favoured that side. " +
                        "The coat smells of cedar and frost. Your breath shortens. " +
                        "You let go of the fabric.",
                    healHP:        0,
                    restoreMP:     0,
                    resolveChange: -5,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "recognition_hint_torn_cuff", "1")
                    }
                ),
                MakeChoice(
                    text: "Ignore it",
                    outcome: "You walk past. It's just a coat.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new RoomHotspotSO.FlagEffect[0]
                )
            },
            heroineLock: new[] { "lysandra" },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        var coatPool = CreatePool(
            id:          "pool_coat_beside_service_door_events",
            displayName: "Pool_CoatBesideServiceDoor_Events",
            hotspotType: RoomHotspotSO.HotspotType.Event,
            pickCount:   1,
            allowDupes:  false,
            variants:    new[] { coatEvent }
        );

        // ── 6. Bell-Pull Pantry ────────────────────────────────────────────
        // CSV: l01_bell_pull_pantry | open_map_utility | servant_route_reveal
        // Choices: pull_bell, trace_wires, leave
        // Utility: reveals servant route / hidden node via flag
        // Guardrail: utility only; no major truth flags.

        var bellEvent = CreateEvent(
            id:          "l01_bell_pull_pantry",
            displayName: "Bell-Pull Pantry",
            hoverText:   "Wires run through the wall above the shelves",
            bodyText:
                "A narrow pantry, shelves bare except for a few sealed jars. " +
                "Bell-pulls run along the upper wall — six of them, labelled in faded ink: " +
                "kitchen, cellar, east corridor, west passage, upper service, unlabelled. " +
                "The wires pass through gaps in the stone and disappear into the wall.",
            choices: new[]
            {
                MakeChoice(
                    text: "Pull the unlabelled bell",
                    outcome:
                        "A faint sound somewhere in the walls — not a bell. A latch. " +
                        "Somewhere near, a passage has opened. You didn't know it was there.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l01_servant_route_revealed", "1")
                    }
                ),
                MakeChoice(
                    text: "Trace the wires",
                    outcome:
                        "Following the wires with your finger, you map out a rough route — " +
                        "the passages they connect, the rooms they bypass. " +
                        "A shortcut, if the route holds.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l01_pantry_route_mapped", "1")
                    }
                ),
                MakeChoice(
                    text: "Leave",
                    outcome: "You leave the bells untouched.",
                    healHP:       0,
                    restoreMP:    0,
                    resolveChange: 0,
                    flagEffects: new RoomHotspotSO.FlagEffect[0]
                )
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        var bellPool = CreatePool(
            id:          "pool_bell_pull_pantry_events",
            displayName: "Pool_BellPullPantry_Events",
            hotspotType: RoomHotspotSO.HotspotType.Event,
            pickCount:   1,
            allowDupes:  false,
            variants:    new[] { bellEvent }
        );

        // ── 7. Exit doors (one per room, always fixed) ────────────────────

        CreateDoor("l01_exit_servant_ledger",    "Stone Doorway",         "Return to the corridor",    "You step back into the corridor.");
        CreateDoor("l01_exit_wine_cellar",        "Stone Archway",         "Exit the wine cellar",       "You push through the archway back to the corridor.");
        CreateDoor("l01_exit_servant_dormitory",  "Dormitory Door",        "Leave the dormitory",        "You step out. The door swings shut behind you.");
        CreateDoor("l01_exit_ruined_confessional","Chapel Archway",        "Leave the chapel",           "You walk back out into the corridor.");
        CreateDoor("l01_exit_coat_service_door",  "Service Door",          "Through the service door",   "The service door opens onto a back passage.");
        CreateDoor("l01_exit_bell_pull_pantry",   "Pantry Door",           "Leave the pantry",           "You step back into the corridor.");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Layer1HotspotAssetGenerator] Done. All Layer 1 hotspot SOs created.");
    }

    // ════════════════════════════════════════════════════════════════════════
    // Factory helpers
    // ════════════════════════════════════════════════════════════════════════

    private static RoomHotspotSO CreateEvent(
        string id, string displayName, string hoverText,
        string bodyText, RoomHotspotSO.EventChoice[] choices,
        string[] heroineLock = null,
        RoomHotspotSO.FlagCondition[] visibilityConditions = null)
    {
        string path = $"{HOTSPOTS}/{id}.asset";
        var existing = AssetDatabase.LoadAssetAtPath<RoomHotspotSO>(path);
        if (existing != null)
        {
            Debug.Log($"[Layer1HotspotAssetGenerator] Skipped (exists): {path}");
            return existing;
        }

        var so = ScriptableObject.CreateInstance<RoomHotspotSO>();
        so.hotspotId             = id;
        so.type                  = RoomHotspotSO.HotspotType.Event;
        so.displayName           = displayName;
        so.hoverText             = hoverText;
        so.eventBodyText         = bodyText;
        so.eventChoices          = choices;
        so.heroineLock           = heroineLock ?? new string[0];
        so.visibilityConditions  = visibilityConditions ?? new RoomHotspotSO.FlagCondition[0];
        so.consumeAfterUse       = true;

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[Layer1HotspotAssetGenerator] Created event: {path}");
        return so;
    }

    private static RoomHotspotSO CreateLore(
        string id, string displayName, string hoverText,
        string loreTitle, string loreBody, string reactionLine,
        int resolveChange,
        RoomHotspotSO.FlagEffect[] loreFlags,
        RoomHotspotSO.FlagCondition[] visibilityConditions = null)
    {
        string path = $"{HOTSPOTS}/{id}.asset";
        var existing = AssetDatabase.LoadAssetAtPath<RoomHotspotSO>(path);
        if (existing != null)
        {
            Debug.Log($"[Layer1HotspotAssetGenerator] Skipped (exists): {path}");
            return existing;
        }

        var so = ScriptableObject.CreateInstance<RoomHotspotSO>();
        so.hotspotId             = id;
        so.type                  = RoomHotspotSO.HotspotType.Lore;
        so.displayName           = displayName;
        so.hoverText             = hoverText;
        so.loreTitle             = loreTitle;
        so.loreBodyText          = loreBody;
        so.characterReactionLine = reactionLine;
        so.loreResolveChange     = resolveChange;
        so.loreFlags             = loreFlags;
        so.visibilityConditions  = visibilityConditions ?? new RoomHotspotSO.FlagCondition[0];
        so.consumeAfterUse       = false;  // lore can be re-read

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[Layer1HotspotAssetGenerator] Created lore: {path}");
        return so;
    }

    private static void CreateDoor(
        string id, string displayName, string hoverText, string doorText)
    {
        string path = $"{HOTSPOTS}/{id}.asset";
        if (AssetDatabase.LoadAssetAtPath<RoomHotspotSO>(path) != null)
        {
            Debug.Log($"[Layer1HotspotAssetGenerator] Skipped (exists): {path}");
            return;
        }

        var so = ScriptableObject.CreateInstance<RoomHotspotSO>();
        so.hotspotId        = id;
        so.type             = RoomHotspotSO.HotspotType.Door;
        so.displayName      = displayName;
        so.hoverText        = hoverText;
        so.doorDestination  = "next_room";
        so.doorText         = doorText;
        so.consumeAfterUse  = false;

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[Layer1HotspotAssetGenerator] Created door: {path}");
    }

    private static RoomHotspotPoolSO CreatePool(
        string id, string displayName,
        RoomHotspotSO.HotspotType hotspotType,
        int pickCount, bool allowDupes,
        RoomHotspotSO[] variants)
    {
        string path = $"{POOLS}/{id}.asset";
        var existing = AssetDatabase.LoadAssetAtPath<RoomHotspotPoolSO>(path);
        if (existing != null)
        {
            Debug.Log($"[Layer1HotspotAssetGenerator] Skipped (exists): {path}");
            return existing;
        }

        var so = ScriptableObject.CreateInstance<RoomHotspotPoolSO>();
        so.poolId           = id;
        so.type             = hotspotType;
        so.hotspotVariants  = variants;
        so.pickCount        = pickCount;
        so.allowDuplicates  = allowDupes;

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[Layer1HotspotAssetGenerator] Created pool: {path}");
        return so;
    }

    // ── Inner data builders ─────────────────────────────────────────────────

    private static RoomHotspotSO.EventChoice MakeChoice(
        string text, string outcome,
        int healHP, int restoreMP, int resolveChange,
        RoomHotspotSO.FlagEffect[] flagEffects,
        RoomHotspotSO.FlagCondition[] requiredFlags = null)
    {
        return new RoomHotspotSO.EventChoice
        {
            choiceText     = text,
            outcomeText    = outcome,
            healHP         = healHP,
            restoreMP      = restoreMP,
            resolveChange  = resolveChange,
            flagEffects    = flagEffects,
            requiredFlags  = requiredFlags ?? new RoomHotspotSO.FlagCondition[0]
        };
    }

    private static RoomHotspotSO.FlagEffect MakeFlag(
        FlagManager.Scope scope, string key, string value)
    {
        return new RoomHotspotSO.FlagEffect
        {
            scope = scope,
            key   = key,
            value = value
        };
    }

    private static RoomHotspotSO.FlagCondition MakeCondition(
        FlagManager.Scope scope, string key, string requiredValue = "1")
    {
        return new RoomHotspotSO.FlagCondition
        {
            scope         = scope,
            key           = key,
            requiredValue = requiredValue
        };
    }

    // ── Folder utility ─────────────────────────────────────────────────────

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;

        string[] parts = path.Split('/');
        string current = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            string next = $"{current}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);
            current = next;
        }
    }
}
#endif
