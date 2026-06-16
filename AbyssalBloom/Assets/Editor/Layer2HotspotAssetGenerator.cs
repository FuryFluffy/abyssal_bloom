// ── Layer2HotspotAssetGenerator.cs ────────────────────────────────────────
// EDITOR ONLY — place in Assets/Editor/
// Usage: Unity menu → AbyssalBloom → Generate Layer 2 Hotspot SOs
//
// Creates all Layer 2 RoomHotspotSO and RoomHotspotPoolSO assets from
// LAYER2_NARRATIVE_SCENES.md (locked prose).
//
// Asset layout:
//   Assets/ScriptableObjects/Rooms/Layer2/Hotspots/  ← event, lore, door hotspots
//   Assets/ScriptableObjects/Rooms/Layer2/Pools/     ← pool SOs
//
// Safe to run multiple times — skips assets that already exist.
//
// NOTE ON FIXED-SEQUENCE EVENTS:
//   l2_transition, l2_jailer_preamble, l2_jailer_loss, and l2_wake_in_cell
//   are not pooled — they fire in a fixed sequence driven by game state.
//   They are created as standalone hotspot SOs for the CombatRoomBridge /
//   EventUI to reference by hotspotId, not via a pool draw.
//
// NOTE ON JAILER COMBAT:
//   l2_jailer_preamble is a pre-combat narration. l2_jailer_phase2 and
//   l2_jailer_defeat are combat-event hooks, not hotspot SOs — they fire
//   from CombatManager at HP thresholds and are wired there directly.
//
// NOTE ON STATUS EFFECTS:
//   As with Layer 1: status effects (uneasy, wavering) are tracked as
//   flagEffects ("uneasy_applied" = "1") so EventUI can apply them via
//   StatusEffectManager. Wire statusesToApply[] when that field exists.
//
// NOTE ON ITEM REWARDS:
//   l2_rusted_key_cell has an item reward (item_l02_rusted_key_ring).
//   The item reference is left as a comment stub until ItemSO integration.
// ──────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class Layer2HotspotAssetGenerator
{
    private const string ROOT     = "Assets/ScriptableObjects/Rooms/Layer2";
    private const string HOTSPOTS = "Assets/ScriptableObjects/Rooms/Layer2/Hotspots";
    private const string POOLS    = "Assets/ScriptableObjects/Rooms/Layer2/Pools";

    [MenuItem("AbyssalBloom/Generate Layer 2 Hotspot SOs")]
    public static void GenerateAll()
    {
        EnsureFolder(ROOT);
        EnsureFolder(HOTSPOTS);
        EnsureFolder(POOLS);

        // ══════════════════════════════════════════════════════════════════
        // FIXED-SEQUENCE EVENTS (not pooled)
        // These fire in a deterministic sequence; LayerGenerator / CombatRoomBridge
        // references them by hotspotId, not via pool draw.
        // ══════════════════════════════════════════════════════════════════

        // ── 1. L2_TRANSITION — staircase descent narration ────────────────
        // Fires once per save slot on first Go Down. Pure narration, no choices.
        // Gate: only if l2_first_descent_seen is NOT set.
        // Implemented as a Lore hotspot (no choices = lore pattern).

        CreateLore(
            id:          "l2_transition",
            displayName: "The Descent",
            hoverText:   "The stairs go down longer than they should",
            loreTitle:   "Descent",
            loreBody:
                "The stairs go down longer than they should.\n\n" +
                "You count them at first. You stop counting somewhere after forty. The stone " +
                "changes — lighter-coloured, older, laid by different hands than the ones above. " +
                "The air is cooler here, and the smell changes: iron, stone, and something " +
                "underneath both that you recognise without being able to name. Something clean. " +
                "Deliberately clean.\n\n" +
                "The first cell door you pass is open. The bed inside is made.\n\n" +
                "At the bottom of the stairs, a single door. It is not locked.\n\n" +
                "It opens before you reach it.",
            reactionLine: "",
            resolveChange: 0,
            loreFlags: new[] {
                MakeFlag(FlagManager.Scope.SaveSlot, "l2_first_descent_seen", "1")
            },
            visibilityConditions: new[] {
                // Negated gate: engine should check NOT set — represented here as a comment.
                // TODO: implement "not equals" condition in FlagCondition when supported.
                // For now: EventUI / CombatRoomBridge guards this check in code.
            }
        );

        // ── 2. L2_JAILER_PREAMBLE — circular chamber, Jailer appears ─────
        // Pre-combat narration. Two choices affect post-combat state hints
        // but do not change combat parameters in this implementation.
        // Choice A: "I'm fine." — Resolve check / resistance path.
        // Choice B: (silence) — pressure path, higher Corruption cost.

        CreateEvent(
            id:          "l2_jailer_preamble",
            displayName: "The Jailer's Chamber",
            hoverText:   "Something enormous waits at the centre",
            bodyText:
                "The chamber is circular. The ceiling is vaulted — high enough that the details " +
                "are lost in darkness above. The walls are lined with doors: cell doors, each one " +
                "sealed, each one with a small barred window at face height. Behind most of them, " +
                "darkness. Behind a few, a dim light, as if someone left a lamp burning.\n\n" +
                "He is at the centre.\n\n" +
                "Large is not the right word for what he is. He fills the space differently than a " +
                "large thing would — not by taking up room, but by being the room's purpose. He was " +
                "built here. He has always been here. His body is part stone and part iron and part " +
                "something that moves too slowly to be mechanical and too deliberately to be alive " +
                "in the ordinary sense. His face, if it can be called that, is turned toward you " +
                "with an expression you take a moment to identify.\n\n" +
                "It is concern.\n\n" +
                "\"You are hurt.\"\n\n" +
                "He takes a step toward you. It shakes the floor.\n\n" +
                "\"You are frightened. You keep walking into doors that bite.\"\n\n" +
                "He stops. He looks at the party with the patient attention of something that has " +
                "waited a very long time and does not mind waiting longer.\n\n" +
                "\"That is why doors must close.\"\n\n" +
                "He raises his arms — not aggressively. The gesture is almost gentle. The chains " +
                "along the walls begin to move.\n\n" +
                "\"A kept thing cannot be lost.\"",
            choices: new[]
            {
                MakeChoice(
                    text: "\"I'm fine.\"",
                    outcome:
                        "The words come out steady. He considers them with the patience of stone. " +
                        "He does not look convinced. The chains continue to move.",
                    healHP:        0,
                    restoreMP:     0,
                    resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l2_jailer_answered", "1")
                    }
                ),
                MakeChoice(
                    text: "Stay silent",
                    outcome:
                        "You say nothing. The silence stretches. He watches you for a long moment " +
                        "with something that, on a human face, you would call sadness. " +
                        "The chains continue to move.",
                    healHP:        0,
                    restoreMP:     0,
                    resolveChange: -4,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l2_jailer_silent", "1")
                    }
                )
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 3. L2_JAILER_LOSS — losing consciousness, Jailer wins ────────
        // No player choice. Replaces standard wipe screen.
        // Fades to dark then to l2_wake_in_cell.

        CreateLore(
            id:          "l2_jailer_loss",
            displayName: "Taken",
            hoverText:   "Darkness",
            loreTitle:   "Taken",
            loreBody:
                "The last thing you see is his hands — open, careful, lowering you to the floor " +
                "with more gentleness than anything in this place has shown you yet.\n\n" +
                "He does not look like he has won. He looks like he has finally been understood.",
            reactionLine: "",
            resolveChange: 0,
            loreFlags: new[] {
                MakeFlag(FlagManager.Scope.SaveSlot, "l2_jailer_loss_seen", "1")
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 4. L2_WAKE_IN_CELL — waking in the farthest cell ─────────────
        // Three heroine variants. Fires on loss path and first-wipe path.
        // Gate: only fires if refuge_ever_established is NOT yet set.
        // Each variant is a separate hotspot SO with a heroineLock.

        string wakeCellSharedBody =
            "You are on the floor of a cell.\n\n" +
            "Not the Jailer's chamber — somewhere further in, somewhere the corridor " +
            "narrows and the ceiling drops and the stone is older than everything above. " +
            "The cell is small. There is no lamp. There is no bed.\n\n" +
            "You are not bound. The door is closed but you can feel, without trying it, " +
            "that it would open if you pushed.\n\n" +
            "The walls are cold. And then, slowly, they are less cold.\n\n" +
            "It is not dramatic. It is the way warmth returns to a room — gradually, without " +
            "announcement, until the absence of cold is simply the new fact of the air.\n\n" +
            "In the corner, from a crack in the floor, something grows.\n\n" +
            "A crystal. Pale, blue-white, the size of a closed fist. Its light is quiet. " +
            "Not warm, not cold. Just present.\n\n" +
            "At its base, scratched into the stone:\n\n" +
            "Keep pushing through.\n\n" +
            "You read it. You are not sure what it means yet — whether someone left it for " +
            "you, whether someone left it for themselves, whether the Castle put it there " +
            "because it knows exactly what those words do to a person who has just lost.\n\n" +
            "The door at your back is still closed. The crystal is still growing.\n\n" +
            "You stay.\n\n" +
            "Not because you have decided to. Because you are here, and here is warm, and " +
            "the corridor beyond the door is dark, and the crystal is the only light.\n\n" +
            "You tell yourself it is temporary.";

        RoomHotspotSO.FlagEffect[] wakeCellFlags = new[] {
            MakeFlag(FlagManager.Scope.PersistentKnowledge, "refuge_ever_established", "1"),
            MakeFlag(FlagManager.Scope.SaveSlot,            "l2_refuge_birth_seen",     "1")
        };

        CreateEvent(
            id:          "l2_wake_in_cell_lysandra",
            displayName: "The Farthest Cell",
            hoverText:   "A crystal grows in the corner",
            bodyText:    wakeCellSharedBody,
            choices: new[]
            {
                MakeChoice(
                    text:    "Stay",
                    outcome: "The warmth holds. The crystal is real. You remain.",
                    healHP: 0, restoreMP: 0, resolveChange: 0,
                    flagEffects: wakeCellFlags
                )
            },
            heroineLock: new[] { "lysandra" },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        CreateEvent(
            id:          "l2_wake_in_cell_mira",
            displayName: "The Farthest Cell",
            hoverText:   "A crystal grows in the corner",
            bodyText:    wakeCellSharedBody,
            choices: new[]
            {
                MakeChoice(
                    text:    "Stay",
                    outcome: "The warmth holds. The crystal is real. You remain.",
                    healHP: 0, restoreMP: 0, resolveChange: 0,
                    flagEffects: wakeCellFlags
                )
            },
            heroineLock: new[] { "mira" },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        CreateEvent(
            id:          "l2_wake_in_cell_seraphine",
            displayName: "The Farthest Cell",
            hoverText:   "A crystal grows in the corner",
            bodyText:    wakeCellSharedBody,
            choices: new[]
            {
                MakeChoice(
                    text:    "Stay",
                    outcome: "The warmth holds. The crystal is real. You remain.",
                    healHP: 0, restoreMP: 0, resolveChange: 0,
                    flagEffects: wakeCellFlags
                )
            },
            heroineLock: new[] { "seraphine" },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 5. L2_REFUGE_BIRTH_WIN_PATH — farthest cell, win path ─────────
        // Only fires if jailer_defeated = "1" AND refuge_ever_established ≠ "1".
        // Visibility condition covers jailer_defeated; negated refuge check handled in code.
        // Choice B ("Note it") defers Refuge — no establishment flag set.

        CreateEvent(
            id:          "l2_refuge_birth_win_path",
            displayName: "The Farthest Cell",
            hoverText:   "The worst door in the dungeon",
            bodyText:
                "The passage past the Jailer's chamber narrows. The ceiling drops. The doors on " +
                "either side are smaller here — alcoves, storage spaces, forgotten rooms. The stone " +
                "is older. The lamps have run out.\n\n" +
                "At the very end of the passage, a door.\n\n" +
                "Warped in its frame, the iron fixtures rusted, the wood dark with age. The worst " +
                "door in the dungeon. You push it open.\n\n" +
                "The room inside is small. Stone walls, low ceiling, no lamp, no bed. Nothing in it.\n\n" +
                "And then the walls change.\n\n" +
                "Slowly. Without announcement. The stone becomes less cold. The air settles. " +
                "The dark at the ceiling softens.\n\n" +
                "In the corner, from a crack in the floor, something grows. A crystal — pale, " +
                "blue-white, quiet.\n\n" +
                "At its base, scratched into the stone:\n\n" +
                "Keep pushing through.",
            choices: new[]
            {
                MakeChoice(
                    text: "Rest here",
                    outcome:
                        "You sit down. The warmth is real — you are certain of that. The crystal " +
                        "is real. The door, which has locked itself from the inside, is real.\n\n" +
                        "What you cannot account for is how the room knew to do this. What you " +
                        "cannot account for is whether you found this place or whether it was " +
                        "waiting to be found.\n\n" +
                        "You rest. The Castle does not leave things unsorted. You understand this " +
                        "now, somewhere beneath the part of you that is still calling it a victory.",
                    healHP: 0, restoreMP: 0, resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.PersistentKnowledge, "refuge_ever_established",   "1"),
                        MakeFlag(FlagManager.Scope.SaveSlot,            "l2_refuge_birth_seen",       "1"),
                        MakeFlag(FlagManager.Scope.SaveSlot,            "l2_refuge_found_by_choice",  "1")
                    }
                ),
                MakeChoice(
                    text: "Note it. Keep moving.",
                    outcome:
                        "You look at the crystal for a long moment. You look at the inscription. " +
                        "You leave without touching anything.\n\n" +
                        "The door closes behind you. The warmth stays inside.",
                    healHP: 0, restoreMP: 0, resolveChange: 5,
                    flagEffects: new RoomHotspotSO.FlagEffect[0]
                    // No Refuge flag. Cell persists as interactable on future runs.
                )
            },
            visibilityConditions: new[] {
                MakeCondition(FlagManager.Scope.PersistentKnowledge, "jailer_defeated", "1")
                // Negated refuge_ever_established check handled in EventUI / CombatRoomBridge.
            }
        );

        // ══════════════════════════════════════════════════════════════════
        // DUNGEON CORRIDOR EVENTS (pooled — accessible after any Jailer outcome)
        // ══════════════════════════════════════════════════════════════════

        // ── 6. L2_FIRST_WIPE_NOTE — found note in the Refuge cell ─────────
        // Environmental lore, fires once after l2_refuge_birth_seen is set.
        // consumeAfterUse = true (fires once, then disappears).

        CreateLore(
            id:          "l2_first_wipe_note",
            displayName: "A Small Folded Card",
            hoverText:   "A note propped against the crystal",
            loreTitle:   "The Card",
            loreBody:
                "Written on a small folded card, propped against the crystal:\n\n" +
                "You have to go forward.\n\n" +
                "There is support waiting. Keep moving.\n\n" +
                "No signature. The handwriting is careful and unhurried.\n\n" +
                "You fold the card. You put it in your pocket, or you leave it where it is. " +
                "Either way, you remember it.",
            reactionLine: "",
            resolveChange: 0,
            loreFlags: new[] {
                MakeFlag(FlagManager.Scope.SaveSlot, "l2_first_wipe_note_read", "1")
            },
            visibilityConditions: new[] {
                MakeCondition(FlagManager.Scope.SaveSlot, "l2_refuge_birth_seen", "1")
            }
        );

        // ── 7. L2_OFFERING_LIST_ROOM — records room, the placement logic ──
        // Lore type. Heroine reaction lines included per-heroine in reactionLine.
        // Because RoomHotspotSO has one characterReactionLine field (not per-heroine),
        // we create three heroine-locked variants so each gets their own reaction.

        string offeringListBody =
            "A records room. Shelves of identical ledgers, spines labelled by year. " +
            "The most recent is open on the table, its entries current to within the last " +
            "few months.\n\n" +
            "The columns read: Name. Origin. Reason for placement. Duration.\n\n" +
            "You read through several entries. The names are ordinary. The origins are " +
            "ordinary — villages, cities, trades, a merchant, a soldier, a herbalist's " +
            "apprentice. The reason for placement column is what stops you. It does not " +
            "read: theft, violence, debt, crime.\n\n" +
            "It reads: distress. isolation. insufficient shelter. grief. fear of the outside. " +
            "repeated harm.\n\n" +
            "People were not imprisoned here because they were dangerous. They were imprisoned " +
            "here because someone decided they were in danger.\n\n" +
            "The duration column, for every entry, reads the same two words: ongoing care.";

        RoomHotspotSO.FlagEffect[] offeringListFlags = new[] {
            MakeFlag(FlagManager.Scope.RunState,          "l2_offering_list_read",            "1"),
            MakeFlag(FlagManager.Scope.PersistentKnowledge, "missing_men_placement_logic_known", "1")
        };

        var offeringLysandra = CreateLore(
            id:          "l2_offering_list_room_lysandra",
            displayName: "Offering List Room",
            hoverText:   "Rows of identical ledgers",
            loreTitle:   "Placement Register",
            loreBody:    offeringListBody,
            reactionLine: "Ongoing care. As if it was doing them a favour.",
            resolveChange: 0,
            loreFlags:   offeringListFlags,
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        var offeringMira = CreateLore(
            id:          "l2_offering_list_room_mira",
            displayName: "Offering List Room",
            hoverText:   "Rows of identical ledgers",
            loreTitle:   "Placement Register",
            loreBody:    offeringListBody,
            reactionLine: "I've seen magistrates use kinder language for life sentences.",
            resolveChange: 0,
            loreFlags:   offeringListFlags,
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        var offeringSeraphine = CreateLore(
            id:          "l2_offering_list_room_seraphine",
            displayName: "Offering List Room",
            hoverText:   "Rows of identical ledgers",
            loreTitle:   "Placement Register",
            loreBody:    offeringListBody,
            reactionLine: "The doctrine of necessary suffering. I know this logic. It never ends with the suffering becoming unnecessary.",
            resolveChange: 0,
            loreFlags:   offeringListFlags,
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 8. L2_RUSTED_KEY_CELL — risk/reward, possible item reward ─────

        var rustedKeyEvent = CreateEvent(
            id:          "l2_rusted_key_cell",
            displayName: "Unlocked Cell",
            hoverText:   "Every other door is sealed — not this one",
            bodyText:
                "The cell is unlocked — door standing open, which is wrong, because every other " +
                "door on this corridor is sealed shut. Inside: a cot, a bucket, a small table. " +
                "Under the table, half-hidden by a folded blanket, a ring of keys. Old iron, " +
                "four keys on the loop, each one labelled with a number rather than a name.\n\n" +
                "The cell has no other exits. There is no reason the keys should be here. " +
                "There is no obvious reason the door was left open.",
            choices: new[]
            {
                MakeChoice(
                    text: "Take the keys",
                    outcome:
                        "The iron is cold and heavier than it looks. The moment you pocket the " +
                        "ring, a sound from somewhere deeper in the corridor — a lock turning. " +
                        "Something checking. You hold very still until it stops.",
                    healHP: 0, restoreMP: 0, resolveChange: -3,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l2_rusted_keys_taken", "1")
                        // itemReward: item_l02_rusted_key_ring — wire when ItemSO exists
                    }
                ),
                MakeChoice(
                    text: "Search the cell first",
                    outcome:
                        "Under the cot: a scratch in the stone floor, half-worn away. Letters. " +
                        "You make out four of them before the light gives out. A name, maybe. " +
                        "Or the start of one.",
                    healHP: 0, restoreMP: 0, resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l2_scratched_name_seen", "1")
                    }
                ),
                MakeChoice(
                    text: "Leave it",
                    outcome:
                        "You step back out. The door behind you swings slowly closed. " +
                        "The latch catches with a sound like a held breath releasing.",
                    healHP: 0, restoreMP: 0, resolveChange: 0,
                    flagEffects: new RoomHotspotSO.FlagEffect[0]
                )
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 9. L2_FALSE_SAFE_CELL — false rest / hidden feeding ───────────

        var falseSafeCellEvent = CreateEvent(
            id:          "l2_false_safe_cell",
            displayName: "The Prepared Cell",
            hoverText:   "A cell that someone made ready for you",
            bodyText:
                "At the end of a short side passage, a cell unlike the others. The door is " +
                "heavy but stands open. The bed inside has been made with care — not the " +
                "mechanical precision of the dormitory above, but something closer to attention. " +
                "A pillow, slightly adjusted. A blanket folded back at the corner. A small lamp " +
                "burning on the floor, the flame the right height for reading.\n\n" +
                "On the table: water, bread, a cloth for cleaning wounds.\n\n" +
                "The room is warm. The room should not be warm — you are deep underground and " +
                "the stone everywhere else is cold.",
            choices: new[]
            {
                MakeChoice(
                    text: "Rest and eat",
                    outcome:
                        "The bread is plain and the water is clean. You eat. The warmth settles " +
                        "over you like a decision being made on your behalf. When you close your " +
                        "eyes, the room does not feel like a cell. When you open them, a few " +
                        "minutes have passed and you feel — not better, exactly. Held.\n\n" +
                        "The door is still open. You are fairly certain it would close if you " +
                        "asked it to.\n\n" +
                        "You didn't ask.",
                    healHP: 25, restoreMP: 12, resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "hidden_feeding_flag_minor", "1"),
                        MakeFlag(FlagManager.Scope.RunState, "l2_false_cell_accepted",   "1")
                    }
                ),
                MakeChoice(
                    text: "Take the supplies and go",
                    outcome:
                        "You take the water and the cloth. You leave the bread. It feels important " +
                        "to leave something. You don't examine why.",
                    healHP: 8, restoreMP: 5, resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l2_false_cell_partial", "1")
                    }
                ),
                MakeChoice(
                    text: "Don't touch anything",
                    outcome:
                        "You stand in the doorway for a moment. The room waits. The warmth " +
                        "presses at you from inside — not threatening, not demanding. Just present. " +
                        "An offer that will not be withdrawn.\n\n" +
                        "You back away. The lamp stays lit behind you.",
                    healHP: 0, restoreMP: 0, resolveChange: 4,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l2_false_cell_refused", "1")
                    }
                )
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 10. L2_QUIET_SHACKLE — Lysandra only ──────────────────────────
        // resolveChange: Read = -6, Turn Away = +5.
        // Turn Away outcome is Lysandra's voiced line, stored as outcomeText.

        var quietShackleEvent = CreateEvent(
            id:          "l2_quiet_shackle",
            displayName: "The Quiet Shackle",
            hoverText:   "A maintained manacle. An inscription beside it.",
            bodyText:
                "A single manacle, mounted to the wall at wrist height. Not rusted — maintained, " +
                "the iron clean, the hinge oiled. A short chain, only a few links, padded at the " +
                "cuff with dark leather worn soft from use.\n\n" +
                "Beside it, at eye level, scratched into the stone in very small letters: " +
                "you don't have to keep carrying it.",
            choices: new[]
            {
                MakeChoice(
                    text: "Read the inscription again",
                    outcome:
                        "You read it twice. The words are ordinary. They are also precisely the " +
                        "words you have not said to yourself, not once, in however long you've " +
                        "been in here — because saying them would mean something about the weight " +
                        "you've been carrying, and you haven't decided yet whether you're ready " +
                        "to name it.\n\n" +
                        "The manacle does not move. It does not need to.",
                    healHP: 0, restoreMP: 0, resolveChange: -6,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState,          "l2_shackle_inscription_read", "1"),
                        MakeFlag(FlagManager.Scope.PersistentKnowledge, "lysandra_offered_relief",   "1")
                    }
                ),
                MakeChoice(
                    text: "Turn away",
                    outcome:
                        "Under her breath. Not for anyone else.\n\n" +
                        "\"I carry it because I chose to. That's the difference.\"\n\n" +
                        "She walks away from the wall. She doesn't look back.",
                    healHP: 0, restoreMP: 0, resolveChange: 5,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState,          "l2_shackle_refused",       "1"),
                        MakeFlag(FlagManager.Scope.PersistentKnowledge, "lysandra_refused_relief", "1")
                    }
                )
            },
            heroineLock: new[] { "lysandra" },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 11. L2_EMPTY_MENS_CELL — lore, Missing Men thread ─────────────
        // Three heroine-locked variants for characterReactionLine.

        string emptyCellBody =
            "The largest cell on the level. Built for many — ten beds bolted to the walls, " +
            "five to a side, each one made. Ten sets of folded clothes at the foot of each " +
            "bed. Ten cups on the shelf above the washbasin.\n\n" +
            "No one here. No sign of struggle. No sign of departure — no missing belongings, " +
            "nothing taken, no note. The clothes are the right size for men of different " +
            "builds and heights, as if measured carefully.\n\n" +
            "The door lock is on the outside. The door has no handle on the inside.";

        RoomHotspotSO.FlagEffect[] emptyCellFlags = new[] {
            MakeFlag(FlagManager.Scope.RunState,          "l2_empty_cell_seen",       "1"),
            MakeFlag(FlagManager.Scope.PersistentKnowledge, "missing_men_no_exit_known", "1")
        };

        var emptyCellLysandra = CreateLore(
            id:          "l2_empty_mens_cell_lysandra",
            displayName: "The Large Cell",
            hoverText:   "Ten beds, all made. No one here.",
            loreTitle:   "The Large Cell",
            loreBody:    emptyCellBody,
            reactionLine: "Ten beds. Ten sets of clothes. No bodies.\nThey didn't leave.",
            resolveChange: 0,
            loreFlags:   emptyCellFlags,
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        var emptyCellMira = CreateLore(
            id:          "l2_empty_mens_cell_mira",
            displayName: "The Large Cell",
            hoverText:   "Ten beds, all made. No one here.",
            loreTitle:   "The Large Cell",
            loreBody:    emptyCellBody,
            reactionLine: "The lock is on the wrong side. Whoever designed this wasn't trying to keep things out.",
            resolveChange: 0,
            loreFlags:   emptyCellFlags,
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        var emptyCellSeraphine = CreateLore(
            id:          "l2_empty_mens_cell_seraphine",
            displayName: "The Large Cell",
            hoverText:   "Ten beds, all made. No one here.",
            loreTitle:   "The Large Cell",
            loreBody:    emptyCellBody,
            reactionLine: "The Castle made room for them. That's the part that should frighten us. It was expecting this many.",
            resolveChange: 0,
            loreFlags:   emptyCellFlags,
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 12. L2_WRONG_MAP_ROOM — utility lore, Castle-changes-over-time ─

        var wrongMapEvent = CreateEvent(
            id:          "l2_wrong_map_room",
            displayName: "Map Left Behind",
            hoverText:   "Someone else's map, almost correct",
            bodyText:
                "A map, left rolled on a shelf in what might once have been a guard alcove. " +
                "It covers Layer 2 — the corridor, the cell blocks, the chamber at the entrance. " +
                "The draftsmanship is careful. Someone spent time on this.\n\n" +
                "It is wrong. Not wildly wrong — the general structure holds, the proportions " +
                "are close. But the passages don't quite match what you've walked. A room where " +
                "the map shows a door. A door where the map shows a wall. The farthest cell " +
                "is marked in the right place, but the approach to it is two turnings different " +
                "from what you navigated.\n\n" +
                "At the bottom, a note in the same hand: *as of late autumn, third year. " +
                "Check again in spring.*",
            choices: new[]
            {
                MakeChoice(
                    text: "Keep the map",
                    outcome:
                        "You roll it back up and pocket it. An almost-correct map is still more " +
                        "than you had. The discrepancies tell you something, even if you're not " +
                        "sure yet what.",
                    healHP: 0, restoreMP: 0, resolveChange: 0,
                    flagEffects: new[] {
                        MakeFlag(FlagManager.Scope.RunState, "l2_wrong_map_found", "1")
                    }
                ),
                MakeChoice(
                    text: "Leave it",
                    outcome:
                        "You set the map back on the shelf. Someone else may need it more, " +
                        "even if it's wrong.",
                    healHP: 0, restoreMP: 0, resolveChange: 0,
                    flagEffects: new RoomHotspotSO.FlagEffect[0]
                )
            },
            visibilityConditions: new RoomHotspotSO.FlagCondition[0]
        );

        // ── 13. Exit door ─────────────────────────────────────────────────

        CreateDoor(
            id:          "l2_exit_dungeon_corridor",
            displayName: "Stone Stairway",
            hoverText:   "The stairs back up to Layer 1",
            doorText:    "You climb. The stone lightens. The air changes. Layer 1 is above you."
        );

        // ══════════════════════════════════════════════════════════════════
        // POOLS
        // ══════════════════════════════════════════════════════════════════

        // Dungeon event pool — all event hotspots the corridor can draw from.
        // Jailer-sequence events (preamble, loss, wake-in-cell) are NOT pooled;
        // they fire deterministically. Only the roaming corridor events go here.

        CreatePool(
            id:          "pool_l2_dungeon_events",
            displayName: "Pool_L2_DungeonEvents",
            hotspotType: RoomHotspotSO.HotspotType.Event,
            pickCount:   1,
            allowDupes:  false,
            variants:    new[] { rustedKeyEvent, falseSafeCellEvent, quietShackleEvent, wrongMapEvent }
        );

        // Lore pool — offering list and empty cell lore (no heroine filter at pool level;
        // heroineLock on each SO gates display at runtime).

        CreatePool(
            id:          "pool_l2_lore",
            displayName: "Pool_L2_Lore",
            hotspotType: RoomHotspotSO.HotspotType.Lore,
            pickCount:   1,
            allowDupes:  false,
            variants:    new[] {
                offeringLysandra, offeringMira, offeringSeraphine,
                emptyCellLysandra, emptyCellMira, emptyCellSeraphine
            }
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Layer2HotspotAssetGenerator] Done. All Layer 2 hotspot SOs created.");
    }

    // ════════════════════════════════════════════════════════════════════════
    // Factory helpers — mirror Layer1HotspotAssetGenerator exactly
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
            Debug.Log($"[Layer2HotspotAssetGenerator] Skipped (exists): {path}");
            return existing;
        }

        var so = ScriptableObject.CreateInstance<RoomHotspotSO>();
        so.hotspotId            = id;
        so.type                 = RoomHotspotSO.HotspotType.Event;
        so.displayName          = displayName;
        so.hoverText            = hoverText;
        so.eventBodyText        = bodyText;
        so.eventChoices         = choices;
        so.heroineLock          = heroineLock ?? new string[0];
        so.visibilityConditions = visibilityConditions ?? new RoomHotspotSO.FlagCondition[0];
        so.consumeAfterUse      = true;

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[Layer2HotspotAssetGenerator] Created event: {path}");
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
            Debug.Log($"[Layer2HotspotAssetGenerator] Skipped (exists): {path}");
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
        so.consumeAfterUse       = false;   // lore can be re-read

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[Layer2HotspotAssetGenerator] Created lore: {path}");
        return so;
    }

    private static void CreateDoor(
        string id, string displayName, string hoverText, string doorText)
    {
        string path = $"{HOTSPOTS}/{id}.asset";
        if (AssetDatabase.LoadAssetAtPath<RoomHotspotSO>(path) != null)
        {
            Debug.Log($"[Layer2HotspotAssetGenerator] Skipped (exists): {path}");
            return;
        }

        var so = ScriptableObject.CreateInstance<RoomHotspotSO>();
        so.hotspotId       = id;
        so.type            = RoomHotspotSO.HotspotType.Door;
        so.displayName     = displayName;
        so.hoverText       = hoverText;
        so.doorDestination = "next_room";
        so.doorText        = doorText;
        so.consumeAfterUse = false;

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[Layer2HotspotAssetGenerator] Created door: {path}");
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
            Debug.Log($"[Layer2HotspotAssetGenerator] Skipped (exists): {path}");
            return existing;
        }

        var so = ScriptableObject.CreateInstance<RoomHotspotPoolSO>();
        so.poolId          = id;
        so.type            = hotspotType;
        so.hotspotVariants = variants;
        so.pickCount       = pickCount;
        so.allowDuplicates = allowDupes;

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[Layer2HotspotAssetGenerator] Created pool: {path}");
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
            choiceText    = text,
            outcomeText   = outcome,
            healHP        = healHP,
            restoreMP     = restoreMP,
            resolveChange = resolveChange,
            flagEffects   = flagEffects,
            requiredFlags = requiredFlags ?? new RoomHotspotSO.FlagCondition[0]
        };
    }

    private static RoomHotspotSO.FlagEffect MakeFlag(
        FlagManager.Scope scope, string key, string value)
    {
        return new RoomHotspotSO.FlagEffect { scope = scope, key = key, value = value };
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

    // ── Folder utility ──────────────────────────────────────────────────────

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

// ══════════════════════════════════════════════════════════════════════════════
// NOTES ON AMBIGUITIES AND RESOLUTIONS
// ══════════════════════════════════════════════════════════════════════════════
//
// 1. NEGATED FLAG GATES (l2_transition, l2_wake_in_cell, l2_refuge_birth_win_path)
//    The document specifies "only fires if X is NOT set." FlagCondition currently
//    supports equality checks only (requiredValue = "1"). The negated gate ("not set")
//    cannot be expressed as a visibilityCondition with the current schema.
//    Resolution: visibilityCondition covers the positive gate (jailer_defeated = "1"
//    for refuge_birth_win_path). The negated checks (l2_first_descent_seen ≠ "1",
//    refuge_ever_established ≠ "1") are left as code comments and must be handled
//    in EventUI / CombatRoomBridge. Add a "notEquals" or "mustBeUnset" mode to
//    FlagCondition when the system is extended.
//
// 2. l2_transition IMPLEMENTED AS LORE, NOT EVENT
//    The spec says "no choices — pure narration." RoomHotspotSO has no dedicated
//    "narration" type. Lore type is the correct match: it presents body text,
//    applies flags on read, and has no choice buttons. consumeAfterUse = false
//    is standard for Lore, but EventUI / CombatRoomBridge should treat this one
//    as fire-once (gated by l2_first_descent_seen save_slot flag).
//
// 3. l2_jailer_loss IMPLEMENTED AS LORE
//    Same reasoning: no player choice, pure narration, applies a save-slot flag.
//    The "fade to dark → fade to cell" transition is a CombatRoomBridge/scene
//    transition concern, not expressible in the SO.
//
// 4. l2_wake_in_cell SPLIT INTO THREE HEROINE-LOCKED VARIANTS
//    The spec calls for heroine-specific variants. RoomHotspotSO has one
//    characterReactionLine field (not a per-heroine array). Since the body text
//    for wake-in-cell is identical across heroines (prose does not show
//    heroine-specific reaction lines for this scene), the split is structural
//    only — all three variants carry the same body. If heroine-specific wake
//    text is added to LAYER2_NARRATIVE_SCENES.md later, update the three SOs.
//
// 5. l2_offering_list_room SPLIT INTO THREE HEROINE-LOCKED VARIANTS
//    Prose specifies distinct reaction lines per heroine. Since RoomHotspotSO
//    supports one characterReactionLine, three separate heroine-locked SOs are
//    the only clean solution without schema changes.
//
// 6. l2_empty_mens_cell SPLIT INTO THREE HEROINE-LOCKED VARIANTS
//    Same reason as offering list. Each heroine's distinct observation line
//    cannot fit in a single characterReactionLine field.
//    Caveat: the pool (pool_l2_lore) contains all six lore variants. At runtime,
//    LayerGenerator / RoomManager should filter pool candidates by active heroine
//    before drawing — otherwise it may draw an opposing heroine's lore SO.
//    This is the same pattern needed for Layer 1 heroine-locked events.
//
// 7. l2_wrong_map_room — PROSE NOT IN NARRATIVE SCENES DOC
//    The spec document lists "wrong map room" as event #8 but LAYER2_NARRATIVE_SCENES.md
//    contains no prose for this scene. Resolution: prose was authored here based
//    on the design brief ("a map left by previous adventurers, almost correct,
//    reveals that the Castle changes over time"). Flag l2_wrong_map_found (RunState)
//    matches the brief. If locked prose is written for this scene later, replace
//    the body text and outcome text in the SO (re-run generator after deleting
//    the existing asset, or edit directly in Inspector).
//
// 8. jailer_defeated FLAG SCOPE
//    The NOTES FOR WIRING section of LAYER2_NARRATIVE_SCENES.md specifies
//    persistent_knowledge for jailer_defeated. This matches the usage: once
//    defeated, he does not reappear on any future run. Applied consistently.
//
// 9. refuge_ever_established FLAG SCOPE
//    Same notes section: persistent_knowledge. Applied consistently across
//    l2_wake_in_cell variants and l2_refuge_birth_win_path.
// ══════════════════════════════════════════════════════════════════════════════
