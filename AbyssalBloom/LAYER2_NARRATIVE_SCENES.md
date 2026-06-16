# Abyssal Bloom — Layer 2 Scripted Narrative Scenes
**Status:** Locked prose. Ready for dialogue engine wiring.
**Tone:** Present tense, second person where narration addresses player. Gothic, spare. No explanations given.
**Format per scene:** [SCENE ID] → speaker tags → text blocks → flag triggers noted inline.

---

## SCENE: L2_TRANSITION
**Trigger:** Player enters Layer 2 for the first time, before the first room loads.
**Speaker:** NARRATION

---

**[NARRATION]**
The stairs go down longer than they should.

You count them at first. You stop counting somewhere after forty. The stone changes — lighter-coloured, older, laid by different hands than the ones above. The air is cooler here, and the smell changes: iron, stone, and something underneath both that you recognise without being able to name. Something clean. Deliberately clean.

The first cell door you pass is open. The bed inside is made.

---

## SCENE: L2_OFFERING_LIST_ROOM
**Trigger:** Player interacts with the Offering List Room hotspot.
**Type:** Lore discovery
**Speaker:** NARRATION, heroine reaction lines

---

**[NARRATION — body text]**
A records room. Shelves of identical ledgers, spines labelled by year. The most recent is open on the table, its entries current to within the last few months.

The columns read: *Name. Origin. Reason for placement. Duration.*

You read through several entries. The names are ordinary. The origins are ordinary — villages, cities, trades, a merchant, a soldier, a herbalist's apprentice. The *reason for placement* column is what stops you. It does not read: *theft, violence, debt, crime.*

It reads: *distress. isolation. insufficient shelter. grief. fear of the outside. repeated harm.*

People were not imprisoned here because they were dangerous. They were imprisoned here because someone decided they were in danger.

The *duration* column, for every entry, reads the same two words: *ongoing care.*

*→ Flag: `l2_offering_list_read` → "1" (run_state)*
*→ Flag: `missing_men_placement_logic_known` → "1" (persistent_knowledge)*

---

**[Optional reaction — LYSANDRA:]**
Ongoing care. As if it was doing them a favour.

**[Optional reaction — MIRA:]**
*→ She closes the ledger.*
I've seen magistrates use kinder language for life sentences.

**[Optional reaction — SERAPHINE:]**
The doctrine of necessary suffering. I know this logic. It never ends with the suffering becoming unnecessary.

---

## SCENE: L2_RUSTED_KEY_CELL
**Trigger:** Player interacts with the Rusted Key Cell hotspot.
**Type:** Risk/reward event
**Speaker:** NARRATION

---

**[NARRATION — body text]**
The cell is unlocked — door standing open, which is wrong, because every other door on this corridor is sealed shut. Inside: a cot, a bucket, a small table. Under the table, half-hidden by a folded blanket, a ring of keys. Old iron, four keys on the loop, each one labelled with a number rather than a name.

The cell has no other exits. There is no reason the keys should be here. There is no obvious reason the door was left open.

---

**[CHOICE: Take the keys]**
*→ outcome text:*
The iron is cold and heavier than it looks. The moment you pocket the ring, a sound from somewhere deeper in the corridor — a lock turning. Something checking. You hold very still until it stops.

*→ healHP: 0 | restoreMP: 0 | resolveChange: -3*
*→ Flag: `l2_rusted_keys_taken` → "1" (run_state)*
*→ itemReward: item_l02_rusted_key_ring [wire when ItemSO exists]*

**[CHOICE: Search the cell first]**
*→ outcome text:*
Under the cot: a scratch in the stone floor, half-worn away. Letters. You make out four of them before the light gives out. A name, maybe. Or the start of one.

*→ healHP: 0 | restoreMP: 0 | resolveChange: 0*
*→ Flag: `l2_scratched_name_seen` → "1" (run_state)*

**[CHOICE: Leave it]**
*→ outcome text:*
You step back out. The door behind you swings slowly closed. The latch catches with a sound like a held breath releasing.

*→ no effects*

---

## SCENE: L2_FALSE_SAFE_CELL
**Trigger:** Player interacts with the False Safe Cell hotspot.
**Type:** False rest / hidden feeding
**Speaker:** NARRATION, CASTLE (voice — no attribution given in-game)

---

**[NARRATION — body text]**
At the end of a short side passage, a cell unlike the others. The door is heavy but stands open. The bed inside has been made with care — not the mechanical precision of the dormitory above, but something closer to attention. A pillow, slightly adjusted. A blanket folded back at the corner. A small lamp burning on the floor, the flame the right height for reading.

On the table: water, bread, a cloth for cleaning wounds.

The room is warm. The room should not be warm — you are deep underground and the stone everywhere else is cold.

---

**[CHOICE: Rest and eat]**
*→ outcome text:*
The bread is plain and the water is clean. You eat. The warmth settles over you like a decision being made on your behalf. When you close your eyes, the room does not feel like a cell. When you open them, a few minutes have passed and you feel — not better, exactly. Held.

The door is still open. You are fairly certain it would close if you asked it to.

You didn't ask.

*→ healHP: 25 | restoreMP: 12 | resolveChange: 0*
*→ Flag: `hidden_feeding_flag_minor` → "1" (run_state)*
*→ Flag: `l2_false_cell_accepted` → "1" (run_state)*

**[CHOICE: Take the supplies and go]**
*→ outcome text:*
You take the water and the cloth. You leave the bread. It feels important to leave something. You don't examine why.

*→ healHP: 8 | restoreMP: 5 | resolveChange: 0*
*→ Flag: `l2_false_cell_partial` → "1" (run_state)*

**[CHOICE: Don't touch anything]**
*→ outcome text:*
You stand in the doorway for a moment. The room waits. The warmth presses at you from inside — not threatening, not demanding. Just present. An offer that will not be withdrawn.

You back away. The lamp stays lit behind you.

*→ resolveChange: +4*
*→ Flag: `l2_false_cell_refused` → "1" (run_state)*

---

## SCENE: L2_QUIET_SHACKLE
**Trigger:** Player interacts with The Quiet Shackle hotspot.
**Type:** Lysandra-locked character event
**Speaker:** NARRATION, CASTLE (voice — no attribution), LYSANDRA
**heroineLock:** lysandra

---

**[NARRATION — body text]**
A single manacle, mounted to the wall at wrist height. Not rusted — maintained, the iron clean, the hinge oiled. A short chain, only a few links, padded at the cuff with dark leather worn soft from use.

Beside it, at eye level, scratched into the stone in very small letters: *you don't have to keep carrying it.*

---

**[CHOICE: Read the inscription again]**
*→ outcome text:*
You read it twice. The words are ordinary. They are also precisely the words you have not said to yourself, not once, in however long you've been in here — because saying them would mean something about the weight you've been carrying, and you haven't decided yet whether you're ready to name it.

The manacle does not move. It does not need to.

*→ resolveChange: -6*
*→ Flag: `l2_shackle_inscription_read` → "1" (run_state)*
*→ Flag: `lysandra_offered_relief` → "1" (persistent_knowledge)*

**[CHOICE: Turn away]**
*→ outcome text:*

**[LYSANDRA]**
*→ Under her breath. Not for anyone else.*
I carry it because I chose to. That's the difference.

*→ She walks away from the wall. She doesn't look back.*

*→ resolveChange: +5*
*→ Flag: `l2_shackle_refused` → "1" (run_state)*
*→ Flag: `lysandra_refused_relief` → "1" (persistent_knowledge)*

---

## SCENE: L2_EMPTY_MENS_CELL
**Trigger:** Player interacts with the Empty Men's Cell hotspot.
**Type:** Lore discovery / Missing Men thread
**Speaker:** NARRATION, heroine reaction lines

---

**[NARRATION — body text]**
The largest cell on the level. Built for many — ten beds bolted to the walls, five to a side, each one made. Ten sets of folded clothes at the foot of each bed. Ten cups on the shelf above the washbasin.

No one here. No sign of struggle. No sign of departure — no missing belongings, nothing taken, no note. The clothes are the right size for men of different builds and heights, as if measured carefully.

The door lock is on the outside. The door has no handle on the inside.

*→ Flag: `l2_empty_cell_seen` → "1" (run_state)*
*→ Flag: `missing_men_no_exit_known` → "1" (persistent_knowledge)*

---

**[Optional reaction — LYSANDRA:]**
Ten beds. Ten sets of clothes. No bodies.
*→ A beat.*
They didn't leave.

**[Optional reaction — MIRA:]**
*→ She checks the door mechanism. Studies it for a moment.*
The lock is on the wrong side. Whoever designed this wasn't trying to keep things out.

**[Optional reaction — SERAPHINE:]**
*→ Quietly.*
The Castle made room for them. That's the part that should frighten us. It was expecting this many.

---

## SCENE: L2_JAILER_PREAMBLE
**Trigger:** Player enters the Jailer's chamber. Pre-combat. No player input.
**Speakers:** NARRATION, THE JAILER

---

**[NARRATION]**
The chamber at the bottom of the dungeon is circular. The ceiling is vaulted — high enough that the details are lost in darkness above. The walls are lined with doors: cell doors, each one sealed, each one with a small barred window at face height. Behind most of them, darkness. Behind a few, a dim light, as if someone left a lamp burning.

He is at the centre.

Large is not the right word for what he is. He fills the space differently than a large thing would — not by taking up room, but by being the room's purpose. He was built here. He has always been here. His body is part stone and part iron and part something that moves too slowly to be mechanical and too deliberately to be alive in the ordinary sense. His face, if it can be called that, is turned toward you with an expression you take a moment to identify.

It is concern.

**[THE JAILER]**
*→ His voice is deep and careful, like something that learned to speak from listening very closely.*
You are hurt.

*→ He takes a step toward you. It shakes the floor.*

**[THE JAILER]**
You are frightened. You keep walking into doors that bite.

*→ He stops. He looks at the party with the patient attention of something that has waited a very long time and does not mind waiting longer.*

**[THE JAILER]**
That is why doors must close.

*→ He raises his arms — not aggressively. The gesture is almost gentle. The chains along the walls begin to move.*

**[THE JAILER]**
A kept thing cannot be lost.

*→ Combat begins.*

---

## SCENE: L2_JAILER_PHASE2
**Trigger:** Jailer HP drops below 50%. Brief combat pause.
**Speaker:** THE JAILER

---

**[THE JAILER]**
*→ He does not roar. He does not rage. His voice becomes quieter, which is worse.*
You keep — moving. Away.

*→ He looks at his hands. At the chains.*

**[THE JAILER]**
Away means hurt. I have — explained. Away means hurt.

*→ Combat resumes. His movements become less controlled — not faster, but more distressed, like someone who cannot understand why the thing they're doing isn't working.*

---

## SCENE: L2_JAILER_DEFEAT
**Trigger:** Jailer HP reaches 0.
**Speakers:** NARRATION, THE JAILER, heroine optional lines

---

**[NARRATION]**
He goes down piece by piece — not broken, but folded. The chain mechanisms along the walls go slack. The cell doors remain sealed.

He settles onto the floor with the slow inevitability of a wall deciding to become rubble. His hands are open. His face is still turned toward you.

*→ A long moment.*

**[THE JAILER]**
Door... opening.

*→ The sound of a mechanism somewhere above. A door, somewhere in the dungeon, unlocking.*

**[THE JAILER]**
No.

*→ Very quietly.*

**[THE JAILER]**
Open means gone. Gone means hurt.

*→ His eyes — whatever serves as his eyes — begin to dim.*

**[THE JAILER]**
Mother...

*→ A breath.*

**[THE JAILER]**
I kept them.

*→ He is still. The chamber is very quiet. The few lamps behind the cell windows continue to burn.*

---

**[Optional — if LYSANDRA is active heroine:]**
**[LYSANDRA]**
*→ Looking at the sealed cell doors.*
He thought he was protecting them.

*→ Pause.*

He might have been right about what he was doing. Just not about whether it was his choice to make.

---

**[Optional — if MIRA is active heroine:]**
**[MIRA]**
*→ She does not look at the Jailer. She looks at the cell doors.*
I've met people like him. They always mean it. That's what makes it so difficult.

---

**[Optional — if SERAPHINE is active heroine:]**
**[SERAPHINE]**
*→ She kneels briefly. Stands again.*
The tragedy isn't that he was wrong. It's that the thing that made him was right enough that he could never tell the difference.

---

## SCENE: L2_REFUGE_BIRTH
**Trigger:** After Jailer defeat, player reaches the end of the dungeon passage — the farthest cell.
**This is the `refuge_ever_established` trigger moment.**
**Speakers:** NARRATION, CASTLE (text inscription only — no voice, no attribution)

---

**[NARRATION]**
The passage past the Jailer's chamber narrows. The ceiling drops. The doors on either side are smaller here — not cells for containment, just alcoves, storage spaces, forgotten rooms. The stone is older. The lamps have run out.

At the very end of the passage, a door.

It is the worst door you have seen in this place — warped in its frame, the iron fixtures rusted, the wood dark with age. A door that has been here longer than the chains above, longer than the copperplate ledgers, longer than the Blood Nun's registers. A door that predates the Castle's decision to become what it became.

You push it open.

*→ The room inside is small. A cell — but barely. Stone walls, a floor, a ceiling low enough to feel the weight of the dungeon above. There is nothing in it. There is no lamp. There is no bed.*

*→ And then the walls change.*

*→ It is not dramatic. It is slow — the way warmth returns to a room when someone finally lights a fire. The stone doesn't shift or reshape. It simply becomes — less cold. Less indifferent. The air settles. The dark at the ceiling softens from hostile to merely dark.*

*→ In the corner of the room, from a crack in the floor, something grows.*

*→ A crystal. Pale and blue-white, the size of a closed fist. It grows the way crystals grow — too slowly to watch, too steadily to doubt. It is already here. It has been here, waiting for the right conditions.*

*→ Its light is quiet. Not warm, not cold. Just present.*

*→ At the base of the crystal, in the stone, scratched by something that took its time:*

**[INSCRIPTION]**
Keep pushing through.

*→ The door behind you closes. The latch catches from the inside.*

*→ You are alone in the smallest room in the Castle. The warmest room in the Castle. The only room where the door locks the way you choose.*

**[NARRATION]**
You believe you have found something.

You are not wrong about the crystal. You are not wrong about the door. You are not wrong about the warmth.

What you cannot know yet is who prepared this room. What you cannot know yet is what the crystal is for, and what feeds it, and what grows larger the more you return.

The Castle does not leave things unsorted.

It has simply learned to sort them more carefully.

*→ Flag: `refuge_ever_established` → "1" (persistent_knowledge)*
*→ Flag: `l2_refuge_birth_seen` → "1" (save_slot)*
*→ Bloom crystal UI unlocked. Refuge hub becomes accessible between runs.*

---

## NOTES FOR WIRING

**Layer 2 transition scene:** Fires once on first entry to Layer 2, then never again. Gate with `l2_first_entry_seen` run_state flag.

**Room event scenes (Offering List, Rusted Key, False Safe Cell, Empty Men's Cell):** Wire to hotspot system same as Layer 1. Lore entries use `LoreReaderUI`. Events use `EventUI` with choice/outcome pattern.

**Quiet Shackle:** `heroineLock = "lysandra"`. If Lysandra is not in the party, this hotspot does not appear. Both choices set `persistent_knowledge` flags — these feed into later-layer dialogue and potentially ending conditions.

**Jailer phase 2 trigger:** Same HP-threshold pattern as Blood Nun (below 50%). Brief pause, one dialogue exchange, combat resumes.

**Jailer defeat optional lines:** Check active heroine ID, display matching line only. All three lines are designed to reveal character through the same event — Lysandra focuses on agency, Mira on intent, Seraphine on origin.

**Refuge birth scene:** This is the most important scene in Layer 2. It must fire after the Jailer is defeated and before Layer 3 access is granted. The inscription *"Keep pushing through"* must display as diegetic text in the world — scratched in stone — not as UI narration. The Castle never speaks aloud here. The comfort is environmental, not voiced.

**`refuge_ever_established` flag scope:** `persistent_knowledge` — it never clears. This is the gate for the `unfed_bloom` ending. If this flag is never set across all runs on a save slot, `unfed_bloom` becomes eligible at Castle Heart.

**`l2_refuge_birth_seen` flag scope:** `save_slot` — persists between runs but not across new saves. Used for "you've been here before" dialogue variants in later runs.
