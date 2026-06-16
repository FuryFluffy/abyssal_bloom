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
The gate is open.

It has been open since before you arrived. The iron is warm to the touch — not from sun, there is no sun here — but from something underneath the metal, something patient and slow. The courtyard beyond is lit by candles in every window. Dozens of them. Hundreds. All burning at the same height.

Someone has been expecting company.

**[LYSANDRA]**
I came this far. No door gets credit for opening.

*→ She steps through. The gate swings shut behind her. She does not look back.*

**[NARRATION]**
The sound it makes is not a lock. It is something softer. A sigh, perhaps. Or a welcome.

---

## SCENE: L1_MIRA_RECRUITMENT
**Trigger:** Player enters the Butler's Corridor room. Encounter with Corrupted Butler fires first. After combat resolves —
**Speakers:** NARRATION, MIRA, LYSANDRA

---

**[NARRATION]**
The butler is on the floor. Whatever it was before the Castle found it, it is quiet now — folded into stillness, hands arranged neatly at its sides as if it lay down by choice.

The woman by the far wall lowers her arm.

She is not surprised to see you. She is assessing you, which is different.

**[MIRA]**
You are either very brave or very lost. In this place, I suppose both count as qualifications.

*→ She straightens. Three vials at her belt, two already used. She notes where your eyes go.*

**[MIRA]**
Mira Voss. I've been inside for six days, I think. The light doesn't change here, which I have decided to find professionally irritating rather than personally alarming.

*→ She crosses the room without being invited. Examines the corridor ahead.*

**[MIRA]**
You're going deeper. I can tell because you're moving like someone with a destination rather than someone who's lost their nerve. I'll come with you.

**[LYSANDRA]**
I didn't offer.

**[MIRA]**
No. I'm telling you my decision. That's different from asking permission.

*→ Beat.*

**[LYSANDRA]**
Can you keep up?

**[MIRA]**
I've survived six days alone in a Castle that rearranges itself when you're not looking. I'll manage.

*→ Flag trigger: `mira_recruited` → "1" (run_state)*

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

**Heroine lock logic:**
- L1_MIRA_RECRUITMENT: always fires (Mira always found in Butler's Corridor, Layer 1).
- L1_SERAPHINE_RECRUITMENT: always fires (Seraphine always found in Ruined Chapel, Layer 1).
- Optional post-boss lines: check active heroine ID, display matching line only.

**Scene triggers all use `run_state` scope** — they clear on run end. If persistent knowledge of "I met Mira in the butler's corridor" is needed for later dialogue, set a separate `save_slot` flag at recruitment.

**Blood Nun phase 2 trigger:** Fires from CombatManager's `OnPhaseTransition` or equivalent — the same HP-threshold check already implemented (Decision X in master reference: below 50%, `_bloodNunHealedPhase2` reset in StartEncounter).

**Post-boss choice:** This is a `RoomEventSO` modal, not a hotspot. Wire to `EventUI` using the same choice/outcome pattern as existing events. Flag effects applied on choice selection before scene transition.
