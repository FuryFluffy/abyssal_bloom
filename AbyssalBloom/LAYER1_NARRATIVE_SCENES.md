# Abyssal Bloom — Layer 1 Scripted Narrative Scenes
**Status:** Locked prose. Ready for dialogue engine wiring.
**Tone:** Present tense, second person where narration addresses player. Gothic, spare. No explanations given.
**Format per scene:** [SCENE ID] → speaker tags → text blocks → flag triggers noted inline.

---

## SCENE: L1_OPENING
**Trigger:** Game start, before first room.
**Speaker:** NARRATION, then LYSANDRA

---

**[NARRATION]**
You don't remember the door.

That is the first thing. You are inside — corridor, stone, candles burning at intervals along the wall — and there is no memory of crossing a threshold. No decision to enter. No moment of stepping through. Just: outside, and then not.

The corridor behind you ends in a wall. Solid stone, no seam, no handle, no hinge. As if it was always a wall. As if there was never anything else.

**[LYSANDRA]**
*→ She turns from the wall. Examines it once. Puts her hand flat against the stone.*

*→ Nothing.*

*→ She turns back to face the corridor ahead. The candles burn at the same height in both directions.*

There's no going back through something that isn't there.

*→ She moves forward. The first room opens ahead of her.*

**[NARRATION]**
The Castle does not explain how you arrived. It doesn't need to. You are here, and here has only one direction.

*→ First encounter loads: Hollow Servant.*

---

## SCENE: L1_MIRA_PREAMBLE
**Trigger:** Player enters the Butler's Corridor room. Butler encounter has not yet fired.
**Speaker:** NARRATION

---

**[NARRATION]**
The corridor opens into a wider service room — shelves along the walls, a long table, doorways to the kitchen beyond. A woman is backed against the far shelves, a vial in each hand, circling a Corrupted Butler that is between her and every exit.

She is not losing. She is managing. There is a difference, and she knows it.

She hasn't seen you yet.

*→ Choice prompt displays.*

**[CHOICE A — Help]**
> *Step in.*

**[CHOICE B — Watch]**
> *Wait. See how she handles it.*

*→ Flag trigger on choice:*
- Help: `l1_mira_choice_helped` → "1" (run_state)
- Watch: `l1_mira_choice_watched` → "1" (run_state)

---

## SCENE: L1_MIRA_HELP_PATH
**Trigger:** Player chose Help. Battle starts immediately with Lysandra joining.
**Speakers:** MIRA, NARRATION (mid-combat line fires once during battle)

---

*→ Lysandra steps in. The Butler turns. Combat begins with both Lysandra and Mira against the Butler.*

**[MIRA]**
*→ Mid-combat, while fighting:*
I can brag and fight at the same time — ask me how I know. Mira Voss, by the way. We can do introductions after.

*→ Combat resolves. Butler down.*

---

## SCENE: L1_MIRA_WATCH_PATH
**Trigger:** Player chose Watch. No combat yet — this plays out before the fight.
**Speakers:** NARRATION, MIRA

---

**[NARRATION]**
The Butler presses her into the shelf. She ducks — fast, practiced — comes up behind it, goes for the flank. She's good. She's also running out of vials.

*→ The Butler catches her coat as she pivots. A sharp sound — fabric tearing.*

*→ Mira freezes for exactly one second. Looks down.*

**[MIRA]**
*→ Flatly, to herself.*
Absolutely not.

*→ She turns and throws both remaining vials. The Butler goes down in a cloud of acrid smoke, choking.*

*→ She stands very still for a moment, then reaches back to confirm what she already knows.*

**[MIRA]**
*→ Under her breath, with great feeling.*
Perfect.

*→ She notices Lysandra in the doorway.*

**[MIRA]**
*→ A beat. Then, with complete composure:*
You've been there the whole time.

*→ It is not a question.*

*→ Combat fires now — Butler still standing, weakened.*

---

## SCENE: L1_MIRA_RECRUITMENT
**Trigger:** Butler is defeated (either path). Recruitment dialogue.
**Speakers:** NARRATION, MIRA, LYSANDRA

---

**[NARRATION]**
The butler is on the floor. Whatever it was before the Castle found it, it is quiet now.

*→ [HELP PATH only:]*
The woman catches her breath. She looks at you — not with gratitude, exactly. With assessment.

*→ [WATCH PATH only:]*
The woman does not look at you immediately. She looks at the butler, then at the ceiling, then at you. She has made a decision about something.

---

**[MIRA]**
Mira Voss. Six days inside, I think — the light doesn't change, which I have decided to find professionally irritating rather than personally alarming.

*→ She crosses the room without being invited. Examines the corridor ahead.*

**[MIRA]**
You're going deeper. I'll come with you.

**[LYSANDRA]**
I didn't offer.

**[MIRA]**
No. I'm telling you my decision. That's different from asking permission.

*→ [HELP PATH only — LYSANDRA:]*
**[LYSANDRA]**
Can you keep up?

**[MIRA]**
Six days alone in a place that rearranges itself. I'll manage.

*→ [WATCH PATH only — beat of silence. Mira doesn't ask. Lysandra doesn't offer. They both know.]*

*→ Flag trigger: `mira_recruited` → "1" (run_state)*

---

## SCENE: L1_TRAP_ROOM
**Trigger:** Next scripted room after Mira recruitment. Outcome branches on `l1_mira_choice_helped` / `l1_mira_choice_watched`.
**Speakers:** NARRATION, MIRA

---

**[NARRATION]**
The next corridor is longer than the others. Halfway down, the floor changes texture — slightly too smooth, the stone fitted differently. Something underneath.

*→ [HELP PATH — Mira warns:]*

**[MIRA]**
*→ She stops. Studies the floor without touching it.*
Middle section. Pressure plate, I'd guess — the grout lines are wrong. Go along the left wall.

*→ Lysandra goes left. Nothing happens. They continue.*

**[MIRA]**
*→ Almost to herself:*
You saved me a vial back there. Fair's fair.

---

*→ [WATCH PATH — Mira stays quiet:]*

**[NARRATION]**
Mira says nothing. She walks slightly behind Lysandra, to the left, close to the wall.

*→ Lysandra hits the centre of the corridor.*

*→ The floor panel drops. Tentacles — thin, pale, deeply wrong — come up from the gap, wrap around her ankles, her wrists. A lesser trap, not designed to hold long, but designed to hold.*

*→ Combat encounter: Lesser Tentacle Trap [weakened enemy, short fight].*

*→ After combat resolves — Lysandra pulls free. Trap retracts.*

**[MIRA]**
*→ From the wall, where she has been standing perfectly safe.*
I noticed it about four steps back, actually.

*→ A beat.*

**[MIRA]**
Eye for an eye, right? We're good now.

*→ She walks past. She pats Lysandra once on the shoulder as she goes.*

**[LYSANDRA]**
*→ After a moment.*
We are absolutely not good.

**[MIRA]**
*→ Already ahead, not looking back.*
Give it a room or two.

---

## SCENE: L1_SERAPHINE_RECRUITMENT
**Trigger:** Player enters the Ruined Chapel room. No combat precedes this.
**Speakers:** NARRATION, SERAPHINE, LYSANDRA, MIRA

---

**[NARRATION]**
The chapel has been broken for a long time. The roof is open to the dark above — not sky, not ceiling, just dark, the same dark that fills all the spaces between rooms here. The pews are intact. The altar is intact. The candles are lit, every one of them, in perfect rows.

Someone is kneeling at the altar.

She is not praying. She is doing something more precise than prayer — a ward, her hands moving in small deliberate arcs, a word repeated under her breath at intervals. The air around her is different. Cooler. Quieter.

*→ As the party enters, the prayer answers.*

*→ It comes back wrong — the cadence off by half a beat, the words slightly transposed, like a room's echo playing the sound back slightly too slow.*

**[SERAPHINE]**
*→ She stands. She does not look startled. She looks certain of something.*

This place remembers prayer, but not mercy.

*→ She turns. She takes in Lysandra first, then Mira. Her expression does not change.*

**[SERAPHINE]**
You came down from the upper floors. I've been holding this room since I lost the others — three days, perhaps four. The ward keeps most of them out.

**[MIRA]**
Most.

**[SERAPHINE]**
Most. The older ones don't have the same relationship with thresholds.

*→ She gathers her things — a single satchel, a book with a cracked spine. Efficient. She's been ready to leave for some time.*

**[SERAPHINE]**
I'll come with you.

**[LYSANDRA]**
What makes you think we're going in the same direction?

**[SERAPHINE]**
I've been listening to this place for four days. It wants you to go down. Which means down is the direction that costs something. That's the direction I'm interested in.

*→ Mira glances at Lysandra.*

**[MIRA]**
She's not wrong about the direction.

**[LYSANDRA]**
*→ A pause.*

Keep the ward active as long as you can.

**[SERAPHINE]**
That was always the plan.

*→ Flag trigger: `seraphine_recruited` → "1" (run_state)*

---

## SCENE: L1_BLOOD_NUN_PREAMBLE
**Trigger:** Player enters the Blood Nun's chamber. Pre-combat. No player input during this scene.
**Speakers:** NARRATION, BLOOD NUN

---

**[NARRATION]**
The room at the end of the lower wing is larger than it should be. The ceiling is lost in darkness. The floor is clean — kept, maintained, scrubbed. The walls hold registers: columns of names in the same copperplate hand you saw in the Ledger Alcove, but these go floor to ceiling and the columns are full.

She is standing at the centre. She has been standing here, waiting, since before you entered the wing.

*→ She does not turn immediately. She finishes writing. She caps her pen. She turns.*

**[BLOOD NUN]**
You have taken longer than most. That is not a criticism — the longer a thing takes to arrive, the more carefully it must have been considered. Processing may proceed.

*→ She regards the party. Her gaze moves from person to person, not with curiosity, but with the efficiency of sorting.*

**[BLOOD NUN]**
Dreadblade. Useful defiance.

*→ Her eyes move to Mira.*

**[BLOOD NUN]**
Red-haired one. Useful suspicion.

*→ Her eyes move to Seraphine.*

**[BLOOD NUN]**
White-haired one. Useful prayer.

*→ She opens a ledger.*

**[BLOOD NUN]**
You will resist. They all resist. Resistance is not inefficiency — it stress-tests the assigned role. Those who resist longest are typically the most precisely placed.

*→ She closes the ledger.*

**[BLOOD NUN]**
Cruelty is waste. We waste nothing.

*→ Combat begins.*

---

## SCENE: L1_BLOOD_NUN_PHASE2
**Trigger:** Blood Nun HP drops below 50%. Combat pauses briefly — one dialogue line, then resumes.
**Speaker:** BLOOD NUN

---

**[BLOOD NUN]**
*→ She does not stagger. She adjusts her grip on the implement. Her voice is unchanged.*

Imprecise. I will need to recalibrate.

*→ Combat resumes. She begins Phase 2 (Binding Rites).*

---

## SCENE: L1_BLOOD_NUN_DEFEAT
**Trigger:** Blood Nun HP reaches 0. Combat ends.
**Speakers:** NARRATION, BLOOD NUN, [optional heroine lines]

---

**[NARRATION]**
She goes down slowly. Not with violence — there is nothing dramatic in it. She reaches the floor with the same controlled economy she brought to everything else, and she lies still.

Her pen is still in her hand.

*→ A moment.*

**[BLOOD NUN]**
*→ From the floor. Her voice is quieter but completely level.*

This outcome was within predicted parameters.

*→ She exhales.*

**[BLOOD NUN]**
The lower dark will correct what I could not. You are unsorted. The Castle does not leave things unsorted.

*→ Her eyes close. The registers on the wall don't change.*

---

**[Optional — if LYSANDRA is active heroine:]**
**[LYSANDRA]**
*→ Quietly, to the room.*
She never asked our names.

---

**[Optional — if MIRA is active heroine:]**
**[MIRA]**
*→ She looks at the registers.*
That's a lot of names to cross out.

---

**[Optional — if SERAPHINE is active heroine:]**
**[SERAPHINE]**
*→ She does not move toward the body.*
She believed every word of it. That's the part that should concern us.

---

## SCENE: L1_POST_BOSS_CHOICE
**Trigger:** Immediately after Blood Nun defeat scene. The two-path choice modal.
**Speakers:** NARRATION, then choice prompt

---

**[NARRATION]**
Two passages open beyond the Blood Nun's chamber — one climbing, one descending.

The climbing passage is cold. Air moves through it, actual air, with the texture of the world outside, or the memory of it. It smells like distance.

The descending passage is warm. The stone is smooth, the kind of smooth that comes from many hands touching it over many years. The candles down there are brighter than they should be. Something has maintained them.

*→ Choice prompt displays:*

**[CHOICE A — GO UP]**
> *The cold passage. Upward, toward whatever the surface means here.*

**[CHOICE B — GO DOWN]**
> *The warm passage. Downward, toward whatever has been keeping the candles lit.*

*→ Flag trigger on choice:*
- Up: `l1_chose_ascent` → "1" (run_state)
- Down: `l1_chose_descent` → "1" (run_state)

---

## NOTES FOR WIRING

**Opening scene:** Lysandra does not enter voluntarily. No gate, no threshold crossed. She is simply inside with no memory of arrival. The wall behind her is solid. Only direction is forward. First encounter after the opening monologue is a Hollow Servant — no scripted preamble, straight into combat.

**Mira recruitment choice (`l1_mira_choice_helped` / `l1_mira_choice_watched`):** Fires as a modal before the Butler combat. Both paths resolve into the same recruitment dialogue but with different pre-text and different mid-scene tone. The Watch path delays combat until after the wardrobe malfunction beat. Both flags are `run_state` scope.

**Trap room:** Is a fixed scripted room, always present, always the next room after Mira recruitment. The outcome branches purely on which flag is set — Mira warns (Help) or Mira watches Lysandra get caught (Watch). The Watch path fires a combat encounter (Lesser Tentacle Trap, weakened stats). The Help path has no combat. Both paths converge after the room.

**Mira's Watch path reconciliation:** "Eye for an eye, right? We're good now." / "We are absolutely not good." / "Give it a room or two." — this exchange is the full close of the bit. No further flag needed; the relationship tension is character texture, not a tracked state.

**Heroine lock logic:**
- L1_MIRA scenes: always fire (Mira always found Layer 1).
- L1_SERAPHINE scenes: always fire (Seraphine always found Layer 1).
- Optional post-boss lines: check active heroine ID, display matching line only.

**Blood Nun phase 2 trigger:** HP-threshold below 50%, same pattern as Decision X in master reference.

**Post-boss choice:** Wire to `EventUI` as a `RoomEventSO` modal. Flag effects applied on choice selection before scene transition.
