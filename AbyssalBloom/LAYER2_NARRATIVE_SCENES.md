# Abyssal Bloom — Layer 2 Scripted Narrative Scenes
**Status:** Locked prose. Ready for dialogue engine wiring.
**Tone:** Present tense, second person where narration addresses player. Gothic, spare. No explanations given.
**Format per scene:** [SCENE ID] → speaker tags → text blocks → flag triggers noted inline.

---

## STRUCTURAL NOTE

Layer 2 is the dungeon beneath Layer 1. It is not explored before the Jailer — he is the first and only thing encountered on descent. The dungeon proper (all room events) opens only after the Jailer encounter resolves, regardless of outcome.

**Three paths into the farthest cell:**
- **Go Down + lose Jailer** → wake in farthest cell (Castle places you there)
- **Go Down + beat Jailer** → dungeon opens, player navigates manually, chooses to rest
- **Wipe during any layer (first time)** → wake in farthest cell (same as loss path, Jailer never met)

All three paths establish the Refuge. The Jailer is not present on subsequent runs if previously defeated.

---

## SCENE: L2_TRANSITION
**Trigger:** Player chooses Go Down after Blood Nun defeat. Fires before Jailer room loads.
**Speaker:** NARRATION
**Fires:** Once per save slot (gate with `l2_first_descent_seen` save_slot flag)

---

**[NARRATION]**
The stairs go down longer than they should.

You count them at first. You stop counting somewhere after forty. The stone changes — lighter-coloured, older, laid by different hands than the ones above. The air is cooler here, and the smell changes: iron, stone, and something underneath both that you recognise without being able to name. Something clean. Deliberately clean.

The first cell door you pass is open. The bed inside is made.

At the bottom of the stairs, a single door. It is not locked.

It opens before you reach it.

---

## SCENE: L2_JAILER_PREAMBLE
**Trigger:** Player enters the Jailer's chamber. First room of Layer 2 on the Go Down path.
**Speakers:** NARRATION, THE JAILER
**Note:** This is the first thing encountered on descent — no exploration precedes it.

---

**[NARRATION]**
The chamber is circular. The ceiling is vaulted — high enough that the details are lost in darkness above. The walls are lined with doors: cell doors, each one sealed, each one with a small barred window at face height. Behind most of them, darkness. Behind a few, a dim light, as if someone left a lamp burning.

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
He goes down piece by piece — not broken, but folded. The chain mechanisms along the walls go slack. The cell doors along the far corridor unlock with a sound like a long exhale.

He settles onto the floor with the slow inevitability of a wall deciding to become rubble. His hands are open. His face is still turned toward you.

*→ A long moment.*

**[THE JAILER]**
Door... opening.

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
*→ Looking at the now-unlocked corridor beyond.*
He thought he was protecting them.

*→ Pause.*

He might have been right about what he was doing. Just not about whether it was his choice to make.

---

**[Optional — if MIRA is active heroine:]**
**[MIRA]**
*→ She does not look at the Jailer. She looks at the open corridor.*
I've met people like him. They always mean it. That's what makes it so difficult.

---

**[Optional — if SERAPHINE is active heroine:]**
**[SERAPHINE]**
*→ She kneels briefly. Stands again.*
The tragedy isn't that he was wrong. It's that the thing that made him was right enough that he could never tell the difference.

---

*→ Dungeon corridor unlocks. Room events become accessible. Player navigates toward the farthest cell.*
*→ Flag: `jailer_defeated` → "1" (persistent_knowledge)*

---

## SCENE: L2_JAILER_LOSS
**Trigger:** Party HP reaches 0 during Jailer fight.
**Speaker:** NARRATION
**Note:** Standard wipe screen does NOT fire here. This scene replaces it.

---

**[NARRATION]**
The last thing you see is his hands — open, careful, lowering you to the floor with more gentleness than anything in this place has shown you yet.

He does not look like he has won. He looks like he has finally been understood.

*→ Fade to dark.*

*→ Fade in: the farthest cell.*

---

## SCENE: L2_WAKE_IN_CELL
**Trigger:** After L2_JAILER_LOSS, OR after wipe during Layer 1 tutorial (first run only).
**Speaker:** NARRATION
**Fires:** First time only — gate with `refuge_ever_established` (if already set, skip directly to Refuge UI)

---

**[NARRATION]**
You are on the floor of a cell.

Not the Jailer's chamber — somewhere further in, somewhere the corridor narrows and the ceiling drops and the stone is older than everything above. The cell is small. There is no lamp. There is no bed.

You are not bound. The door is closed but you can feel, without trying it, that it would open if you pushed.

*→ The walls are cold. And then, slowly, they are less cold.*

*→ It is not dramatic. It is the way warmth returns to a room — gradually, without announcement, until the absence of cold is simply the new fact of the air.*

*→ In the corner, from a crack in the floor, something grows.*

*→ A crystal. Pale, blue-white, the size of a closed fist. Its light is quiet. Not warm, not cold. Just present.*

*→ At its base, scratched into the stone:*

**[INSCRIPTION]**
Keep pushing through.

*→ You read it. You are not sure what it means yet — whether someone left it for you, whether someone left it for themselves, whether the Castle put it there because it knows exactly what those words do to a person who has just lost.*

*→ The door at your back is still closed. The crystal is still growing.*

**[NARRATION]**
You stay.

Not because you have decided to. Because you are here, and here is warm, and the corridor beyond the door is dark, and the crystal is the only light.

You tell yourself it is temporary.

*→ Flag: `refuge_ever_established` → "1" (persistent_knowledge)*
*→ Flag: `l2_refuge_birth_seen` → "1" (save_slot)*
*→ Refuge hub unlocked. Run ends. Player returns to Refuge UI.*

---

## SCENE: L2_FIRST_WIPE_NOTE
**Trigger:** Player opens Refuge for the first time after `l2_refuge_birth_seen` is set.
**Type:** Environmental text — note found in the cell, readable as a hotspot.
**Fires:** Once (gate with `l2_first_wipe_note_read` save_slot flag)

---

**[NARRATION — note text, displayed as found object]**
*Written on a small folded card, propped against the crystal:*

You have to go forward.

There is support waiting. Keep moving.

*→ No signature. The handwriting is careful and unhurried.*

**[NARRATION]**
You fold the card. You put it in your pocket, or you leave it where it is. Either way, you remember it.

*→ Flag: `l2_first_wipe_note_read` → "1" (save_slot)*

---

## SCENE: L2_REFUGE_BIRTH_WIN_PATH
**Trigger:** Player navigates manually to the farthest cell after defeating the Jailer (win path only).
**Condition:** `jailer_defeated` = "1" AND `refuge_ever_established` ≠ "1"
**Speaker:** NARRATION

---

**[NARRATION]**
The passage past the Jailer's chamber narrows. The ceiling drops. The doors on either side are smaller here — alcoves, storage spaces, forgotten rooms. The stone is older. The lamps have run out.

At the very end of the passage, a door.

Warped in its frame, the iron fixtures rusted, the wood dark with age. The worst door in the dungeon. You push it open.

*→ The room inside is small. Stone walls, low ceiling, no lamp, no bed. Nothing in it.*

*→ And then the walls change.*

*→ Slowly. Without announcement. The stone becomes less cold. The air settles. The dark at the ceiling softens.*

*→ In the corner, from a crack in the floor, something grows. A crystal — pale, blue-white, quiet.*

*→ At its base, scratched into the stone:*

**[INSCRIPTION]**
Keep pushing through.

*→ Choice prompt displays:*

**[CHOICE: Rest here]**
*→ outcome text:*
You sit down. The warmth is real — you are certain of that. The crystal is real. The door, which has locked itself from the inside, is real.

What you cannot account for is how the room knew to do this. What you cannot account for is whether you found this place or whether it was waiting to be found.

You rest. The Castle does not leave things unsorted. You understand this now, somewhere beneath the part of you that is still calling it a victory.

*→ Flag: `refuge_ever_established` → "1" (persistent_knowledge)*
*→ Flag: `l2_refuge_birth_seen` → "1" (save_slot)*
*→ Flag: `l2_refuge_found_by_choice` → "1" (save_slot)*
*→ Refuge hub unlocked. Run ends. Player returns to Refuge UI.*

**[CHOICE: Note it. Keep moving.]**
*→ outcome text:*
You look at the crystal for a long moment. You look at the inscription. You leave without touching anything.

The door closes behind you. The warmth stays inside.

*→ resolveChange: +5*
*→ No Refuge established. Player continues the run.*
*→ Note: Player can return to this cell on a future run to establish Refuge then.*

---

## SCENE: L2_OFFERING_LIST_ROOM
**Trigger:** Player interacts with the Offering List Room hotspot (post-Jailer dungeon).
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

*→ Flag: `l2_scratched_name_seen` → "1" (run_state)*

**[CHOICE: Leave it]**
*→ outcome text:*
You step back out. The door behind you swings slowly closed. The latch catches with a sound like a held breath releasing.

---

## SCENE: L2_FALSE_SAFE_CELL
**Trigger:** Player interacts with the False Safe Cell hotspot.
**Type:** False rest / hidden feeding

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
*→ She checks the door mechanism.*
The lock is on the wrong side. Whoever designed this wasn't trying to keep things out.

**[Optional reaction — SERAPHINE:]**
*→ Quietly.*
The Castle made room for them. That's the part that should frighten us. It was expecting this many.

---

## NOTES FOR WIRING

**Spatial structure:** Layer 2 entry → Jailer chamber (first room, always) → dungeon corridor unlocks on any Jailer outcome → room events accessible → farthest cell at the end.

**L2_TRANSITION:** Fires once per save slot on first Go Down choice. Gate with `l2_first_descent_seen` (save_slot). On subsequent runs the descent is silent — no transition narration.

**Jailer loss path:** Standard wipe screen suppressed. L2_JAILER_LOSS fires instead, fading directly into L2_WAKE_IN_CELL. No run summary, no "you died" — just the Jailer's hands and then the cell.

**Any-layer first wipe path:** If the party wipes on any layer before the Refuge is established, skip directly to L2_WAKE_IN_CELL. The Jailer is never seen on this path — the Castle simply delivers them to the farthest cell by other means, unexplained.

**Win path Refuge:** L2_REFUGE_BIRTH_WIN_PATH only fires if `jailer_defeated` = "1" AND `refuge_ever_established` ≠ "1". The second choice ("Note it. Keep moving.") allows the player to defer Refuge establishment — they can return to this cell on any future run. The cell persists as an interactable hotspot until the Refuge is established.

**First wipe note (L2_FIRST_WIPE_NOTE):** Appears as an interactable object in the Refuge cell on the first visit after `l2_refuge_birth_seen` is set. It is not voiced, not a UI popup — it is a found note, read like a lore hotspot. It fires once, then disappears. The Castle never acknowledges having written it.

**Room events:** All five events (Offering List, Rusted Key, False Safe Cell, Quiet Shackle, Empty Men's Cell) sit in the dungeon corridor between the Jailer chamber and the farthest cell. They are accessible on any run where the Jailer has been defeated (past or present run). On runs where the player lost to the Jailer and woke in the cell, they access these rooms by pushing back out through the dungeon on their way up to Layer 1.

**`jailer_defeated` flag scope:** `persistent_knowledge` — once he is defeated, he is gone from all future runs. The chamber still exists on revisit but is empty.

**`refuge_ever_established` flag scope:** `persistent_knowledge` — never clears. Gate for `unfed_bloom` ending.

**`l2_refuge_birth_seen` flag scope:** `save_slot` — persists between runs, resets on new save. Used for return-visit dialogue variants.

**`l2_refuge_found_by_choice` flag scope:** `save_slot` — set only on win path. Reserved for potential Castle dialogue acknowledging that you walked there yourself.
