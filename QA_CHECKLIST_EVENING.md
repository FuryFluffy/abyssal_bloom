# Abyssal Bloom — Evening QA Checklist

**Goal:** Test the full game loop from startup to combat and back.
**Method:** Press Play, click through each step, note every console error.
**Rule:** Don't fix anything mid-session. Log errors, finish the list, report back.

---

## PHASE 0 — Setup (Before Play)

- [ ] All generators run (if not already done from last session):
  - AbyssalBloom → Generate All Status Effect SOs
  - AbyssalBloom → Generate Core Layer 1 Assets
  - AbyssalBloom → Generate All Layer 1 Items
  - AbyssalBloom → Generate Layer 1 Hotspot SOs
- [ ] Game scene built: AbyssalBloom → Build Game Scene
- [ ] Open `Assets/Scenes/game.unity`
- [ ] 0 compile errors in Console before pressing Play

---

## PHASE 1 — Startup

Press Play.

- [ ] **No crash on startup**
- [ ] **RefugePanel is visible** (black panel with "Bloom: 0" and "Start Run" button)
- [ ] **Other panels hidden** (MapPanel, CombatPanel, EventPanel not visible)
- [ ] Console: note any errors that fire on startup

**What to look for:**
- `[GameBootstrap]` errors → heroine SOs not found or party not built
- `[RunStateManager]` errors → singleton not initialized
- `[RefugeUI]` errors → missing field references
- NullReferenceException → something not wired in Inspector

---

## PHASE 2 — Refuge Hub

While on the Refuge screen:

- [ ] **Bloom counter shows "Bloom: 0"**
- [ ] **Tab buttons clickable** (Recovery, Party, Upgrades, Gallery, Knowledge)
- [ ] **Click Recovery tab** → heroine rows visible with restore buttons
- [ ] **Click Party tab** → heroine stats visible (HP/MP/Resolve/Corruption)
- [ ] **Click Upgrades tab** → upgrade buttons visible (greyed out at 0 Bloom is fine)
- [ ] **Restore HP button** on a heroine → no crash (may say "not enough Bloom")
- [ ] Console: note any errors

---

## PHASE 3 — Start Run

- [ ] **Click "Start Run" button**
- [ ] **RefugePanel hides**
- [ ] **MapPanel shows** with node graph
- [ ] Console: note any errors

**What to look for:**
- `[RoomManager]` errors → layer profile not assigned or LayerGenerator failed
- `[LayerGenerator]` errors → profile missing encounter pool or templates
- `[MapUI]` errors → node prefab not assigned
- Blank MapPanel with no nodes → GenerateMap not called or OnMapGenerated not firing

---

## PHASE 4 — Map Navigation

On the map:

- [ ] **Node graph visible** with multiple room nodes
- [ ] **Nodes are clickable** (cursor changes on hover)
- [ ] **Click an accessible node** → room entered, some panel changes
- [ ] **Room type routes correctly:**
  - Battle room → CombatPanel shows, combat starts
  - Event/Lore room → HotspotPanel shows OR EventPanel shows
- [ ] Console: note any errors

**What to look for:**
- `[RoomManager]` errors → CurrentRoom not set
- `[MapNodeView]` errors → node view not finding its data
- Clicking nodes does nothing → OnNodeClicked not wired

---

## PHASE 5 — Combat Room

If you entered a Battle room:

- [ ] **CombatPanel visible** with heroine and enemy panels
- [ ] **Heroine stats correct** (HP/MP/Resolve/Corruption bars filled)
- [ ] **Enemy stats correct** (HP bar filled, name shown)
- [ ] **Action buttons visible** (Attack, Defend, Skill, Item, Run)
- [ ] **Click Attack** → enemy takes damage, turn advances
- [ ] **Click Skill** → skill submenu opens
- [ ] **Click Item** → inventory panel opens (may be empty)
- [ ] **Click Run** → flee attempt fires
- [ ] **Win combat** → CombatPanel hides, MapPanel returns
- [ ] Console: note any errors

---

## PHASE 6 — Event/Hotspot Room

If you entered a non-combat room:

- [ ] **Hotspot buttons visible** on room background
- [ ] **Click an Item hotspot** → ItemPickupPanel opens
  - Take button → item added to inventory
  - Leave button → panel closes
- [ ] **Click an Event hotspot** → EventChoicePanel opens
  - Choice buttons visible with correct text
  - Click a choice → outcome text shows
  - Continue button → panel closes
- [ ] **Click a Lore hotspot** → LoreReaderPanel opens
  - Title and body text visible
  - Close button → panel closes
- [ ] **Click the Door hotspot** → room completes, map returns
- [ ] Console: note any errors

---

## PHASE 7 — Return to Refuge

After completing enough rooms (or dying):

- [ ] **MapPanel hides on death/completion**
- [ ] **RefugePanel shows** on Refuge return
- [ ] **Bloom counter updated** (should show Bloom earned this run)
- [ ] **Heroine stats reflect damage taken** (HP not fully restored)
- [ ] **Restore HP button works** at correct Bloom cost
- [ ] Console: note any errors

---

## PHASE 8 — Save/Load

- [ ] **Exit Play mode**
- [ ] **Enter Play mode again** → game should resume or start fresh (no crash)
- [ ] Console: note any errors

---

## PHASE 9 — Edge Cases (If Time Allows)

- [ ] **Grapple:** Let an enemy grab a heroine, test Struggle/Submit/Intervene
- [ ] **Forced swap:** Let a heroine reach 0 HP mid-combat, verify swap prompt fires
- [ ] **Empty inventory:** Click Item button in combat with no items → graceful (no crash)
- [ ] **Full inventory:** Try picking up a 7th item → "Inventory full" message

---

## HOW TO REPORT BACK

For each phase that fails, note:
1. **Which phase** (e.g. "Phase 3 — Start Run")
2. **What happened** (e.g. "MapPanel shows but no nodes")
3. **Console error** (copy the full error message)

Don't fix anything during the session — just collect the list. One batch fix is more efficient than piecemeal repairs.

---

## KNOWN ISSUES TO EXPECT (Don't Panic)

These are already known and non-critical:

- Input System spam in Console → fix Active Input Handling to "Both" in Project Settings
- `itemMenuRoot` not wired on CombatUI → item menu won't open but no crash
- Hotspot anchor positions at (0.5, 0.5) → all hotspot buttons appear in center of screen (expected until art exists)
- `CurrentRoomTemplate` not on RunStateManager → background sprite won't swap (Debug.Log only)
- Gallery/Knowledge tabs in Refuge → placeholder, no content

Good luck. Report back what breaks.
