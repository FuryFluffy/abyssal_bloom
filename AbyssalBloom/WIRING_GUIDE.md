# Abyssal Bloom — Combat Test Scene Wiring Guide

## What you're building

A single Unity scene that runs a test encounter on Play.  
Three SOs drive the heroines; one or more EnemyDataSOs drive the enemies.

---

## Step 1 — Scene GameObjects

Create these GameObjects in the Hierarchy (order doesn't matter):

```
_Managers
  └─ (attach: FlagManager, CombatManager, EncounterBuilder)

Canvas  ← standard Unity UI Canvas (Screen Space – Overlay)
  ├─ HeroinePanel_0          ← Lysandra (or whichever heroine is in slot 0)
  │     ├─ NameText
  │     ├─ HPText
  │     ├─ MPText
  │     ├─ ResolveText
  │     ├─ CorruptText
  │     ├─ HPBar              ← Image component, Image Type = Filled, Fill Method = Horizontal
  │     └─ ResolveBar         ← same setup
  ├─ HeroinePanel_1          ← Mira
  │     └─ (same children)
  ├─ HeroinePanel_2          ← Seraphine
  │     └─ (same children)
  │
  ├─ EnemyRow_0 … EnemyRow_5  ← one per potential enemy
  │     ├─ EnemyNameText
  │     ├─ EnemyHPText
  │     └─ EnemyHPBar
  │
  ├─ GrapplePanel             ← inactive by default
  │     └─ GrappleStagText
  │
  ├─ RoundText
  │
  ├─ ActionButtonGroup
  │     ├─ ActionButton_0 … ActionButton_7   ← 8 Buttons, each with a child Text
  │
  ├─ SkillMenuRoot            ← inactive by default
  │     ├─ SkillButton_0 … SkillButton_5     ← 6 Buttons
  │
  └─ LogPanel
        ├─ ScrollRect         ← attach ScrollRect component
        │     └─ Viewport
        │           └─ LogText   ← Text component; set overflow to overflow, don't auto-size
        └─ Scrollbar Vertical
```

**Tip:** Use the default Unity UI right-click menu (GameObject > UI > ...) to create
these.  Exact positions don't matter for a debug UI — stack panels top-to-bottom.

---

## Step 2 — CombatManager

On `_Managers`, the `CombatManager` component needs no Inspector fields —
it receives everything via `StartEncounter()`.

---

## Step 3 — EncounterBuilder Inspector

On `_Managers`, fill in `EncounterBuilder`:

| Field | What to drag in |
|---|---|
| **Combat Manager** | The CombatManager component on _Managers |
| **Heroine Data [0]** | LysandraData SO |
| **Heroine Data [1]** | MiraVossData SO |
| **Heroine Data [2]** | SeraphineData SO |
| **Heroine Abilities [0].abilities** | [DreadSlash SO, CrimsonLunge SO] |
| **Heroine Abilities [1].abilities** | [PoisonedDart SO, AcidFlask SO] |
| **Heroine Abilities [2].abilities** | [HolyLight SO, MendingPrayer SO] |
| **Enemy Data [0]** | e.g. HollowServant SO |
| **Enemy Abilities [0].abilities** | That enemy's ability SOs (or leave empty) |

> For a first test with one enemy and no abilities, just set Enemy Data [0]
> and leave all ability lists empty. The AI falls back to universal Attack.

---

## Step 4 — CombatUI Inspector

On `Canvas` (or a child GameObject), attach `CombatUI`.  
Then drag every UI element into its matching slot:

**Heroine panels** — drag in index-matched Text and Image components:
- `heroineNameText[0]` → HeroinePanel_0/NameText
- `heroineHPText[0]` → HeroinePanel_0/HPText  
- *(repeat for [1] and [2], and for all other stat texts)*
- `heroineHPBar[0]` → HeroinePanel_0/HPBar *(the Image component)*
- `heroineResolveBar[0]` → HeroinePanel_0/ResolveBar

**Enemy rows:**
- `enemyRowRoot[0]` → EnemyRow_0 *(the root GameObject, not a child)*
- `enemyNameText[0]` → EnemyRow_0/EnemyNameText
- `enemyHPText[0]` → EnemyRow_0/EnemyHPText
- `enemyHPBar[0]` → EnemyRow_0/EnemyHPBar
- *(repeat for rows 1–5; unused rows are hidden automatically)*

**Grapple panel:**
- `grapplePanel` → GrapplePanel GameObject
- `grappleStageText` → GrapplePanel/GrappleStageText

**Round counter:** `roundText` → RoundText

**Action buttons:**
- `actionButtons[0..7]` → ActionButton_0 … ActionButton_7
- `actionButtonLabels[0..7]` → the Text child of each button

**Skill sub-menu:**
- `skillMenuRoot` → SkillMenuRoot
- `skillButtons[0..5]` → SkillButton_0 … SkillButton_5
- `skillButtonLabels[0..5]` → each button's Text child

**Log:**
- `logScrollRect` → LogPanel/ScrollRect
- `logText` → LogPanel/ScrollRect/Viewport/LogText

---

## Step 5 — Press Play

`EncounterBuilder.Start()` fires `StartEncounter()` immediately.  
`CombatUI.OnEnable()` has already subscribed to all events.

You should see:
- Round 1 logged
- A heroine or enemy's turn logged
- Action buttons enabled when it's a heroine's turn
- Clicking a button advances the turn

---

## Known TODOs in the code

| Location | Issue |
|---|---|
| `CombatUI.PickDefaultTarget()` | Always targets first alive enemy. Replace with click-to-select for multi-enemy encounters. |
| `CombatManager.HandleKnockout()` | Auto-picks first alive support on forced swap. Spec says player chooses — fire an event and block until UI responds. |
| `CombatManager.ResolveAction(Run)` | Not yet implemented; always fails silently. |
| `CombatManager.ResolveAction(UseItem)` | Item system not yet designed. |

---

## Decision notes

**Why `HeroineAbilityList` wrapper class?**  
Unity's Inspector cannot serialise `T[][]` (jagged arrays). Wrapping in a
`[Serializable]` class with a single array field is the standard Unity workaround.
The alternative (a flat `CharacterAbilitySO[]` with a stride of 2) is fragile.

**Why 8 action buttons pooled rather than instantiated dynamically?**  
Dynamic Instantiate/Destroy per turn adds garbage and layout recalculation cost.
8 covers the largest action set (active grapple: Struggle/Submit/Item = 3;
normal combat: Attack/Defend/Skill/Item/Run = 5; support grapple: 4).
Pooling is simpler and zero-GC.

**Why party-slot order for heroine panels (not active-first)?**  
Reordering UI panels on every swap requires either anchored transforms or
a layout rebuild. Showing panels in fixed slots 0/1/2 and marking the active
one with a `►` prefix (and optionally a tint via `activeIndicator`) is simpler
and equally clear for a debug UI.
