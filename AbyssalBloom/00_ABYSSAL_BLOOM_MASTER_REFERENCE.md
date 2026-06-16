# Abyssal Bloom — Master Project Reference
**Add this file to every new chat. It is the single source of truth for all locked decisions.**
**Engine:** Unity (C#), ScriptableObjects, no third-party frameworks
**Genre:** Dark fantasy roguelite, standalone desktop, adult content

---

## 1. Story & Core Premise (LOCKED)

Heroines trapped in a sentient living Castle that tore itself into a pocket dimension to fulfill its original command: *"Keep them safe. Keep them happy."*

**10 layers, 7 endings** determined by knowledge flags + final choice at Castle Heart.
Bloom = primary currency earned from battles. Spending Bloom at the Refuge feeds the Castle.

**7 Boss Philosophies:**
Layer 1: Justice (Blood Nun) | Layer 2: Love (Jailer) | Layer 3: Comfort (Bloom Saint, Layer 5) | Layer 4: Purpose (Velvet Regent, Layer 6) | Layer 5: Purity (Starved Seraph, Layer 8) | Layer 6: Liberation (Blood High Nun, Layer 9) | Layer 10: Happiness (Castle Heart)

---

## 1a. World & Foundational Lore (LOCKED)

### The World: En
The game's setting is called **En**, divided between two realms:
- **The Blessed Realm** — values purity, order, separation, sacrifice for the greater good. Fatal flaw: the belief that cruelty can become mercy when justified by doctrine.
- **The Corrupted Realm** — values desire, need, pleasure, emotional honesty. Fatal flaw: the belief that genuine desire is automatically justified.

The true conflict in Abyssal Bloom is not Blessed vs Corrupted, pain vs pleasure, or purity vs corruption. **It is control vs choice.**

### The Forbidden Child
A child born between a Blessed father and a Corrupted mother — ordinary, innocent, loved — became the contradiction neither side could accept. The ultimate revelation of the game: **love cannot exist without choice.**

### The Castle's Origin & Logic
Created as a living sanctuary with one command: *"Keep them safe. Keep them happy."*
Its tragedy is obeying perfectly while misunderstanding humanity. Its logic escalated as:
1. Prevent suffering.
2. Prevent departure.
3. Remove reasons to leave.
4. Change what people desire.
5. Create a happiness that cannot be rejected.

It tore itself and a piece of En into a pocket dimension: **the Abyssal Bloom**.

### The Castle's Psychological Arc (Layer by Layer)
| Layer | The Castle's belief |
|---|---|
| 1 | People can be sorted and processed. |
| 2 | People must be restrained to be protected. |
| 3 | People fail because they choose incorrectly. |
| 4 | Pain can be removed by changing memories. |
| 5 | Comfort is more valuable than freedom. |
| 6 | A perfect role is better than uncertainty. |
| 7 | Escape is possible, but perhaps meaningless. |
| 8 | Purity is worth any sacrifice. |
| 9 | Revenge can become another prison. |
| 10 | A perfect eternal happiness can replace choice. |

### The Blood Nuns
Process arrivals as categories, not people. Core belief: nothing should be wasted.

### The Missing Men (Ongoing Mystery Thread)
Men who disappeared inside the Castle were not simply victims or escapees. They were absorbed — incorporated into the Castle's architecture. The foundations contain them. The walls remember them. The Castle's reasoning: *"They were never abandoned. They were given a place."*

Thread seeds: Layer 1 (Servant Ledger — names crossed out, almost all male) → Layer 2 (Empty Men's Cell — built for many, no bodies, no escape route) → Layer 4 (revelation begins: they became part of the structure).

---

## 1b. Layer Narrative Canon (LOCKED where noted)

### Layer 1 — The Lower Castle
**Tag line:** *"You are not prisoners. You are merely unsorted."*
**Emotional tone:** Bureaucratic horror. Domestic uncanny. Ritual without purpose.
**Visual:** Cursed domestic space — servant halls, kitchens, storage rooms, abandoned bedrooms, chapel wings, butler offices, service corridors. Everything recently abandoned. Table still set. Candles still burning. Bells still ring. Servants are wrong (faceless maids still folding sheets; butlers offering tea from empty trays).

**Opening — Lysandra alone. (LOCKED):**
Lysandra does not enter voluntarily. No gate, no threshold crossed. She is simply inside with no memory of arrival. The wall behind her is solid. Only direction is forward.

Her first line is not spoken aloud — it is internal, and it is not given to the player. The player sees only the corridor and moves forward. First encounter is a Hollow Servant — no scripted preamble, straight into combat.

The line *"I came this far. No door gets credit for opening."* is **retired** — it implies a voluntary crossing that does not happen in the game.

**Mira's arrival:** Found fighting a Corrupted Butler. Trapped but not helpless. Joins after battle.
**Mira's first line to Lysandra (LOCKED):**
> *"You are either very brave or very lost. In this place, I suppose both count as qualifications."*

Immediate tension: Lysandra sees Mira's cynicism as weakness. Mira sees Lysandra's pride as a blind spot.

**Seraphine's arrival:** Party enters ruined chapel. She maintains a protective ward. The prayer answers back — cadence is wrong. She understands: something here is imitating faith.
**Seraphine's line (LOCKED):**
> *"This place remembers prayer, but not mercy."*

**Room events:**
- *Servant Ledger Alcove* — servant registry, many names crossed out, almost all male. First seed of the Missing Men mystery.
- *Wine Cellar of Warm Bottles* — offers benefits; secretly advances `hidden_feeding_flag_minor`.
- *Servant Dormitory* — false rest. Beds warm, sheets clean. No one should be alive to prepare them.
- *Ruined Confessional* — Resolve/Corruption event focused on Seraphine.
- *Coat Beside the Service Door* — torn fabric, Lysandra recognizes the stitching. First hint toward the Vowbroken Duelist (item: The Torn Cuff).

**Boss — Blood Nun:**
Philosophy (LOCKED): *"Obedience and processing create justice."*
She does not see herself as cruel — cruelty wastes resources.
**Her greatest line (LOCKED):** *"Cruelty is waste. We waste nothing."*
She introduces the party by function, not name:
- *"Dreadblade. Useful defiance."*
- *"Red-haired one. Useful suspicion."*
- *"White-haired one. Useful prayer."*

After defeat: *"The lower dark will correct what I could not."*

**Post-boss choice:**
- **Go Up** — harder path. Represents escape, the unknown, belief the surface is above.
- **Go Down** — safer-looking path. Represents shelter, prison, the path toward the Castle's heart.

**Hidden truth of Layer 1:** The Castle does not hate you. It does not want to kill you. It wants to put you where you belong.

---

### Layer 2 — The Dungeon
**Tag line:** *"A kept thing cannot be lost."*
**Emotional tone:** Protective horror. Terror of kindness without consent. Being locked inside for your own good.
**Visual:** Cold stone corridors, cells, chains, iron doors, prison mechanisms. Disturbing contradiction: cells are maintained, chains polished, beds repaired. Someone is still caring for the prisoners.

**Room events:**
- *Offering List Room* — records of prisoner classification. Imprisoned not for evil, but because someone decided they needed to stay.
- *Rusted Key Cell* — risk/reward. Key recoverable, resources gained, danger exposure.
- *False Safe Cell* — a comfortable prison. Room offers healing. Castle whispers: *"Stay a little longer."* Advances `hidden_feeding_flag_minor`.
- *The Quiet Shackle* — major Lysandra event. Restraint is especially horrifying to her (identity built on choosing her battles, carrying her own pain, continuing despite grief). Castle offers relief. She refuses.
- *Empty Men's Cell* — built for many prisoners. No bodies. No escape route. Only absence. Second seed of Missing Men mystery.

**Boss — The Jailer:**
Appearance: Massive constructed guardian — part prison architecture, part living creature, part childlike servant.
Philosophy (LOCKED): *"Love means containment."*
**His lines (LOCKED):**
> *"You are hurt. You are frightened. You keep walking into doors that bite. That is why doors must close."*

Most tragic belief: *"A kept thing cannot be lost."*
**Defeat dialogue (LOCKED):**
> *"Door opening... No. Open means gone. Gone means hurt."*
> *"Mother... I kept them."*

His final thought is not hatred. It is fear.

**The Birth of the Bloom Refuge (LOCKED narrative moment):**
After exhaustion, defeat, or choosing safety — party reaches the farthest cell. The worst cell in the Castle. The walls become warm. The door locks from the inside. A Bloom crystal grows. A message appears: *"Keep pushing through."*
The player believes they have found hope. The Castle has simply found a better way to keep them.
→ This is the `refuge_ever_established` trigger moment.

**Hidden truth of Layer 2:** The most dangerous prison is not built from iron. It is built from kindness.

---

### Layer 3 — Lower Halls / Rival Paths
**Tag line:** *"Knowing the danger does not mean you can overcome it."*
**Emotional tone:** Paranoia. Distrust. Survivor guilt. The horror of seeing yourself in those who failed.
**Visual:** Graveyard of previous attempts — broken camps, abandoned supplies, half-written journals, barricades built by people who understood the Castle's tricks and still lost.

**This is Mira's layer.** Her worldview is directly challenged.
Mira believes: *"Everything has a trick. Every trick has a weakness."*
The Castle proves: you can understand the mechanism and still lose. Her greatest fear is not ignorance — it is helplessness.

**Room events:**
- *Purist Journal Niche* — journal from the Purist Remnants. Entries are frightening because mostly correct. Final pages show descent into fanaticism. Their failure: they confused resistance with hatred.
- *Rival Cache Dead End* — supplies of dead adventurers. Taking them is practical. The question: did they die because they were weak, or because they were willing to leave something behind?
- *Abandoned Waystation* — false rest that feels earned. Campfire, equipment, a note: *"We made it this far."* Someone had hope here. They were wrong.
- *Oath-Marked Barricade* — Corruption/Resolve pressure event. A barricade built by people who swore to never surrender. The oath itself has become a prison.
- *Door with the Old Joke* — Mira recognition event. Door bears the phrase: *"Never steal from a locked room. Steal the room."* She recognizes the joke before the person behind it. First sign someone from her past is inside.
- *Wrong Map Room* — utility/exploration. A map left by previous adventurers. Almost correct. The small mistakes reveal the Castle changes.

**Optional Boss — Grinning Butler-Mimic:**
The absorbed form of someone Mira trusted completely (partner-in-crime / lover / fiancé — exact relationship left to interpretation, always someone who knew her entirely).
Appearance: A perfect servant. Too perfect. Nothing human beneath the performance.
**Recognition dialogue (LOCKED):** The creature remembers: *"Never steal from a locked room. Steal the room."*
Its most painful line: *"Ah. That one still opens."* (the memory is still inside.)

Resolution paths:
- *Acceptance* — Mira accepts the Castle's interpretation (understanding = possession). May contribute to `castle_reinterpretation_flag`, `deep_corruption_flag`.
- *Rejection* — Mira refuses the Castle's version. **Her line (LOCKED):** *"I remember you. Not this."*

**Hidden truth of Layer 3:** People can resist pain. People can resist chains. They struggle against the memories of their own failures.

---

### Layer 4 — Underground / Mirror Dream
**Tag line:** *"If a memory hurts you, why keep it?"*
**Emotional tone:** Nostalgia. Regret. Grief. The terror of a perfect lie.
**Visual:** Architecture becomes less physical. Hallways change. Rooms remember previous visitors. Reflections speak. The walls contain memories. The Castle stops changing the body — it begins changing meaning.

**The Castle's lesson:** *"A painful truth is less valuable than a comforting lie."* It believes it has discovered kindness. Why preserve guilt, loss, regret, shame — when those things can be rewritten?

**Missing Men revelation begins here:** The heroes discover the disappeared men were not simply victims. They became part of the Castle — incorporated into the foundations and walls. The Castle's reasoning: they were never abandoned. They were given a place.

*(Layers 5–10 pending — archive cut off. To be added when ChatGPT completes the dump.)*

---

## 2. Party Structure (LOCKED)

- 1 Active heroine (on screen, full combat)
- 2 Support heroines (off screen, passive triggers + support actions)
- Party of 3 picked before a run, locked for the entire run
- Support heroines cannot be directly targeted (exception reserved for later layers)
- Swap In costs the Support heroine's action. NOT available during grapple.
- Forced swap at 0 HP: immediate interrupt, player chooses new Active heroine

---

## 3. Heroine Roster

### Starting Trio (Stats Locked)

| Heroine | Role | HP | MP | ATK | MAG | DEF | RES | SPD |
|---|---|---|---|---|---|---|---|---|
| Lysandra | Dreadblade Duelist | 125 | 32 | 18 | 6 | 13 | 9 | 10 |
| Mira Voss | Rogue / Alchemist | 105 | 42 | 13 | 10 | 9 | 11 | 16 |
| Seraphine | Cleric / Exorcist | 95 | 58 | 7 | 17 | 8 | 16 | 9 |

All start: Resolve 100/100, Corruption 0/100

### Remaining 12 Heroines (Pending stat design)
Druid, Shield Saint, Dream Mage, Beastwarden, Succubus, Devilborne, Barbarian, Thorn Witch, Necromancer, Muse + 2 unnamed slots

### Rivals
15 rival characters exist (LoRA training data ready for all 30 heroines + rivals)

---

## 4. Combat Formulas (LOCKED)

```
Physical:   max(1, Mathf.RoundToInt((atk * power) - (target.def * 0.50f)))
Magic:      max(1, Mathf.RoundToInt((mag * power) - (target.res * 0.50f)))
Resolve:    max(1, Mathf.RoundToInt(base + (mag * 0.25f) - (target.res * 0.20f)))
Corruption: max(0, Mathf.RoundToInt(base - (target.res * 0.05f)))
Healing:    Mathf.RoundToInt(base + (mag * power))
Initiative: spd + Random.Range(0, 5)
Hit chance: Mathf.Clamp(base + ((atk.spd - def.spd) * 2), 60, 98)
Status %:   Mathf.Clamp(base + ((atk.mag - def.res) * 2), 15, 95)
```

**Magic always auto-hits. Only physical rolls hit chance.**
**MAG governs all status application chances, even on physical abilities.**

### Power Bands
| Band | Multiplier | Use |
|---|---|---|
| Very Low | 0.65 | Chip / assist attacks |
| Low | 0.85 | Universal Attack, light skills |
| Medium | 1.10 | Standard skills |
| High | 1.35 | Strong skills |
| Very High | 1.65 | Boss strikes / expensive skills |
| Severe | 2.00 | Telegraphed boss moves |

### MP Cost Bands
| Band | Range | Use |
|---|---|---|
| Free | 0 | Basic attack, defend, struggle |
| Cheap | 3–5 | Light utility, weak skill |
| Standard | 6–9 | Normal skill |
| Expensive | 10–14 | Strong attack / heal / multi-target |
| Major | 15–20 | Signature / boss-answer |
| Final | 20+ | Rare story moves |

---

## 5. Resolve & Corruption System (LOCKED)

### Resolve Bands
| Range | Band | Effect |
|---|---|---|
| 100 | Steady | No modifier |
| 70–99 | Strong | No modifier |
| 40–69 | Strained | Struggle chance −10 |
| 1–39 | Fragile | Struggle chance −25 |
| 0 | Broken | Struggle UNAVAILABLE; Submit or Item only |

### Corruption Bands
| Range | Band | Max Grapplers |
|---|---|---|
| 0–39 | Clear / Low | 1 (grapple ability required) |
| 40–69 | Tempted | 2 (any enemy can join) |
| 70–99 | High | 3 (any enemy can join) |
| 100 | Fully Corrupted | All enemies (any can join); Struggle always at Broken modifier |

### Route Balance (Core Design Rule)
- Resistance route: harder short-term, cleaner long-term, better endings
- Comfort/Corruption route: easier short-term, more compromised long-term, stronger Castle argument

---

## 6. Universal Commands (LOCKED — All Heroines)

| Command | Details |
|---|---|
| Attack | Physical, Low power (0.85), Free, 90% base hit |
| Defend | Reduce incoming damage, skip turn |
| Item | Use item from shared inventory |
| Run | Attempt to flee encounter |

---

## 7. Starting Abilities (LOCKED)

### Lysandra
| Ability | Type | Power | MP | Notes |
|---|---|---|---|---|
| Dread Slash | Physical | Medium (1.10) | Free | No status. Primary free damage. |
| Crimson Lunge | Physical | High (1.35) | 4 | Bleed 3/t ×2t, 45% base chance |

### Mira Voss
| Ability | Type | Power | MP | Notes |
|---|---|---|---|---|
| Poisoned Dart | Physical | Low (0.85) | Free | Poison 2/t ×3t, 50% base chance |
| Acid Flask | Magic | Medium (1.10) | 5 | DEF Down −3 ×3t, 55% base chance |

### Seraphine
| Ability | Type | Power | MP | Notes |
|---|---|---|---|---|
| Holy Light | Magic | Medium (1.10) | 4 | Auto-hit, no status |
| Mending Prayer | Healing | Low (0.85) | 5 | Base 8 + (mag × 0.85) = 22 HP |

---

## 8. Upgrade Paths (LOCKED)

Structure: Base → Upgrade 1 (Enhanced) → Upgrade 2A or 2B (permanent branch, no re-spec)

| Heroine | Ability | Upgrade 1 | 2A | 2B |
|---|---|---|---|---|
| Lysandra | Dread Slash | Abyssal Slash (High power, free) | Void Cleave (AoE) | Dread Reaper (execute +25% sub-50%) |
| Lysandra | Crimson Lunge | Sanguine Pierce (Bleed 4/t ×3t) | Hemorrhage (DOT burst) | Blood Riposte (counter) |
| Mira | Poisoned Dart | Venomous Dart (Medium power, Poison 3/t) | Neurotoxin Needle (+SPD Down) | Barbed Flechette (+50% vs Poisoned) |
| Mira | Acid Flask | Caustic Flask (High power, DEF −4) | Blight Bomb (AoE debuff) | Volatile Concoction (DEF+RES down) |
| Seraphine | Holy Light | Sacred Radiance (High power) | Judgment (Very High, +25% vs Corrupt) | Purifying Burst (AoE + 5 Resolve self) |
| Seraphine | Mending Prayer | Sanctuary Prayer (31 HP) | Greater Restoration (39 HP + cleanse) | Ward of Grace (29 HP + DEF +3) |

---

## 9. Support Heroine System (LOCKED)

### Passive Triggers (Fire automatically, no action cost)
| Heroine | Trigger | Effect | Cooldown |
|---|---|---|---|
| Lysandra — Blade Instinct | Active takes hit ≥20% max HP | Auto-attack at Very Low (0.65), 90% hit | 3 Lysandra turns |
| Mira — Alchemist's Reflex | Status applied to Active heroine | 50% chance to negate before application | 3 Mira turns (fires on attempt) |
| Seraphine — Divine Vigil | Active drops below 35% HP | Auto-heal 12 HP flat | 4 Seraphine turns |

Mira's passive does NOT trigger on Corruption gain (Corruption is a stat, not a status).

### Assist Attacks (Always available, free, fallback only)
| Heroine | Name | Type | Power |
|---|---|---|---|
| Lysandra | Side Slash | Physical | Very Low (0.65) |
| Mira | Quick Dart | Physical | Very Low (0.65) |
| Seraphine | Minor Smite | Magic | Very Low (0.65) |

### Assist Abilities (Free, 3-turn cooldown)
| Heroine | Ability | Effect |
|---|---|---|
| Lysandra | Battle Cry | Active heroine ATK +3 for 2 turns |
| Mira | Smelling Salts | Restore 5 Resolve to Active heroine |
| Seraphine | Shield of Faith | Active heroine RES +3 for 2 turns |

---

## 10. Grapple & H-Scene System (LOCKED)

### During Grapple — Menu Changes
- Active heroine: Struggle / Submit / Use Item
- Support heroines: Intervene / Watch / Encourage / Use Item
- Swap In is UNAVAILABLE during grapple

### Active Heroine Actions
| Action | Cost | Effect |
|---|---|---|
| Struggle | Free | Escape: clamp(50 + (hero.ATK - enemy.ATK) × 3, 20, 85). Resolve mods apply. |
| Submit | Free | +1 Stage. Resolve dmg: max(1, round(8 + mag×0.25 - res×0.20)). Corruption: max(0, round(6 - res×0.05)) |

### Support Heroine Actions During Grapple
| Action | Cost | Effect |
|---|---|---|
| Intervene | 8 MP | Guaranteed grapple break. −3 Resolve to intervening heroine. |
| Watch | Free | No effect. +5 Corruption to watcher. |
| Encourage | Free | +1 Stage. +8 Corruption to encourager. +3 Corruption to Active. Enemy +1 ATK permanent. Unlocks Stage 4+ if encourager Corruption ≥40. |

### Grapple Action (Automatic per grappler per turn)
- Resolve damage: max(1, round(6 + enemy.MAG × 0.25 - target.RES × 0.20))
- Corruption gain: max(0, round(4 - target.RES × 0.05))
- Frenzy: +1 ATK and +1 MAG permanently to grappling enemy (persists after grapple breaks)

### Scene Stages
| Stage | Effect |
|---|---|
| 1 | Base values |
| 2 | Grapple Action +2 Resolve, +2 Corruption |
| 3 | Climax. Grapple auto-resolves. Climax Recoil fires. |
| 4+ | Requires Active or encourager Corruption ≥40 |

### Climax Recoil (Sex-Route Victory)
`round(enemy.maxHP × (0.15 + active.corruption × 0.005))`
Higher Corruption → more Ecstasy Damage → fights end faster via sex route → feedback loop.

---

## 11. Layer 1 Enemies (LOCKED)

**Asset Generation — Automated**
All Layer 1 enemies, abilities, and encounter pools are auto-generated by `CoreAssetGenerator.cs` in one click.

**Menu:** AbyssalBloom → Generate Core Layer 1 Assets
**Creates:**
- 6× EnemyDataSO (Hollow Servant, Knife Footman, Prayer-Rag Novice, Corrupted Butler, Red-Wax Acolyte, Blood Nun)
- 18× CharacterAbilitySO (all abilities for all 6 enemies)
- 1× EncounterPoolSO (Layer 1, groups A–Z)
- 3× Enemy table SOs (EnemyLayerTemplateSO, EnemyRankMultiplierSO, EnemyRoleModifierSO)

The enemy stats and ability data below are the source of truth for the generator and remain **LOCKED**. Do not edit generated SOs manually.

**Note on CharacterAbilitySO:** This class does NOT have `targetIsHeroine` or `hpThresholdToUse` fields. Enemy AI in `CombatManager` handles all targeting logic directly. `targetIsHeroine` references in Red-Wax Acolyte (Healing/Buff targeting) and Blood Nun (self-heal gating) are implemented via AI conditions, not SO field checks.

| Enemy | HP | MP | ATK | MAG | DEF | RES | SPD | Role | Tag |
|---|---|---|---|---|---|---|---|---|---|
| Hollow Servant | 40 | 0 | 8 | 3 | 4 | 3 | 6 | Fodder/grappler | male-mindless |
| Knife Footman | 36 | 6 | 11 | 3 | 3 | 3 | 10 | Physical DPS | male-humanoid |
| Prayer-Rag Novice | 34 | 12 | 5 | 8 | 3 | 6 | 7 | Resolve attacker | male-humanoid |
| Corrupted Butler | 44 | 8 | 9 | 6 | 5 | 5 | 7 | Grapple specialist | male-humanoid |
| Red-Wax Acolyte | 30 | 14 | 5 | 7 | 3 | 6 | 8 | Enemy support | female-humanoid |
| Blood Nun (Boss) | 130 | 24 | 12 | 10 | 7 | 8 | 8 | Layer boss | female-humanoid |

### Layer 1 Enemy Abilities (LOCKED)

#### Hollow Servant
| abilityId | Type | Power | MP | Hit | Notes |
|---|---|---|---|---|---|
| `shambling_strike` | Physical | Medium (1.10) | 0 | 90% | Normal fallback attack |
| `clutching_grab` | Grapple | — | 0 | 75% | `isGrappleInitiator = true` |

**AI:** If Active Resolve ≤50 and no grapple active → Clutching Grab. Otherwise → Shambling Strike.

#### Knife Footman
| abilityId | Type | Power | MP | Hit | Status | Status % |
|---|---|---|---|---|---|---|
| `quick_slash` | Physical | Medium (1.10) | 0 | 90% | None | — |
| `driven_thrust` | Physical | High (1.35) | 3 | 85% | Bleed 2/t ×2t | 40% |

**AI:** If MP ≥3 and target not Bleeding → Driven Thrust. Otherwise → Quick Slash.

#### Prayer-Rag Novice
| abilityId | Type | Power | MP | Hit | Notes |
|---|---|---|---|---|---|
| `dark_prayer` | Magic | Medium (1.10) | 0 | Auto | Normal magic fallback |
| `whisper_of_doubt` | ResolveAttack | — | 4 | Auto | `baseResolveDamage 8` + `baseCorruptionGain 3` (combined) |

**AI:** If MP ≥4 → Whisper of Doubt (always targets Active). Otherwise → Dark Prayer.

#### Corrupted Butler
| abilityId | Type | Power | MP | Hit | Notes |
|---|---|---|---|---|---|
| `silver_tray` | Physical | Low (0.85) | 0 | 90% | Weak fallback |
| `courteous_embrace` | Grapple | — | 4 | 80% | `isGrappleInitiator = true` |

**AI:** If no grapple active and MP ≥4 → Courteous Embrace. If grapple active (by anyone) → Silver Tray.

#### Red-Wax Acolyte
| abilityId | Type | Power | MP | Hit | Notes |
|---|---|---|---|---|---|
| `wax_drip` | Magic | Medium (1.10) | 0 | Auto | Fallback only |
| `crimson_blessing` | Healing | Low (0.85) | 4 | — | Heals ally 12 HP. Ally targeting implemented via AI. |
| `ember_ward` | Buff | — | 3 | — | Applies `def_up_enemy` (+3 DEF, 2t) to ally. Ally targeting implemented via AI. |

**AI:** Ally below 60% HP and MP ≥4 → Crimson Blessing (lowest HP ally). Highest ATK ally lacks `def_up_enemy` and MP ≥3 → Ember Ward. Otherwise → Wax Drip.

#### Blood Nun (Boss)
| abilityId | Type | Power | MP | Hit | Status | Status % | Notes |
|---|---|---|---|---|---|---|---|
| `flagellants_lash` | Physical | High (1.35) | 0 | 90% | Bleed 3/t ×2t | 50% | Free, spammable |
| `communion` | ResolveAttack | Magic (1.10) | 4 | Auto | — | — | `baseResolveDamage` + `baseCorruptionGain 4` (combined) |
| `sanctified_embrace` | Grapple | — | 6 | 85% | — | — | `isGrappleInitiator = true` |
| `blood_rite` | Healing | — | 6 | — | — | — | Self-heal 15 HP. Below 50% HP only (gated in AI). |

**AI Phase 1 (above 50% HP):** Turn 1 → Lash. Turn 2 → Communion. Turn 3+: if Active Resolve ≤60 and MP ≥6 → Embrace. Else alternate Lash/Communion by round parity.
**AI Phase 2 (below 50% HP):** One Blood Rite cast per phase entry (tracked by `_bloodNunHealedPhase2`). If Active Resolve ≤50 and MP ≥6 → Embrace. Otherwise → Lash spam.

### Minimum StatusEffectSOs for Layer 1 Playtest
4 assets needed (not 9 — others deferred):
- `bleed` — DOT 3/t (Footman: 2/t), 2 turns, refresh_duration
- `poison` — DOT 2/t, 3 turns, refresh_duration  
- `def_down` — modDEF −3, 3 turns, refresh_duration
- `def_up_enemy` — modDEF +3, 2 turns, refresh_duration

---

## 12. Enemy Stat Scaling System (LOCKED)

Three-table system. Final stat = floor(layerBase × rankMultiplier × roleModifier)

### Layer Base Stats
| Layer | HP | MP | ATK | MAG | DEF | RES | SPD |
|---|---|---|---|---|---|---|---|
| 1 | 42 | 8 | 8 | 4 | 4 | 4 | 7 |
| 2 | 55 | 10 | 10 | 5 | 5 | 5 | 7 |
| 3 | 70 | 12 | 12 | 6 | 6 | 6 | 8 |
| 4 | 88 | 16 | 14 | 9 | 8 | 9 | 9 |
| 5 | 110 | 20 | 15 | 12 | 9 | 11 | 9 |
| 6 | 135 | 24 | 18 | 14 | 11 | 13 | 10 |
| 7 | 165 | 28 | 20 | 16 | 13 | 15 | 11 |
| 8 | 200 | 34 | 22 | 20 | 15 | 20 | 10 |
| 9 | 240 | 38 | 26 | 22 | 18 | 19 | 11 |
| 10 | 300 | 45 | 30 | 28 | 22 | 24 | 12 |

### Rank Multipliers
| Rank | HP | ATK/MAG | DEF/RES | SPD bonus |
|---|---|---|---|---|
| Standard | ×1.0 | ×1.0 | ×1.0 | +0 |
| Elite | ×1.8 | ×1.25 | ×1.2 | +1 |
| Major Boss | ×4.2 | ×1.55 | ×1.4 | +2 |
| Final Boss | ×6.0 | ×1.70 | ×1.6 | +3 |

### Role Modifiers
| Role | HP | ATK | MAG | DEF | RES | SPD bonus |
|---|---|---|---|---|---|---|
| Bruiser | ×1.15 | ×1.1 | — | ×1.05 | — | −1 |
| Caster | — | — | ×1.2 | ×0.9 | ×1.1 | 0 |
| Controller | ×0.95 | — | ×1.1 | — | ×1.15 | 0 |
| Skirmisher | ×0.9 | ×1.05 | — | — | — | +2 |
| Tank | ×1.25 | — | — | ×1.2 | — | −2 |
| Support | ×0.95 | — | ×1.1 | — | — | 0 |

---

## 13. DOT Rules (LOCKED)

- Bleed and Poison can coexist on the same target
- Each DOT type is capped at one active instance — re-application refreshes duration
- DOTs tick at the START of the affected unit's turn

---

## 14. Encounter Tuning Targets (LOCKED)

| Type | Rounds | HP Loss | Resolve Loss | Corruption Gain |
|---|---|---|---|---|
| Resistance encounter | 3–6 | 15–45% | 0–25 | 0–10 |
| Comfort-accepting | 2–5 | 5–30% | 0–20 | 5–20 |
| H-heavy/Corruption | 3–7 | 10–50% | 10–45 | 15–45 |
| Major Boss | 8–14 | 50–90% | 20–60 | 10–40 |

Layer 1 passes. Lysandra slightly below 15% floor in Standard A — accepted (tutorial layer).

---

## 15. Unity Architecture (LOCKED)

**FlagManager:** Persistent singleton MonoBehaviour (DontDestroyOnLoad). Three scopes:
- `run_state` — clears on run end/wipe
- `save_slot` — persists between runs
- `persistent_knowledge` — never clears (castle memory mechanic)

**ScriptableObject hierarchy:**
```
Characters/  → CharacterDataSO, CharacterAbilitySO
Enemies/     → EnemyDataSO, EnemyLayerTemplateSO, EnemyRankMultiplierSO, EnemyRoleModifierSO
Combat/      → MoveSO, StatusEffectSO, PowerBandSO
Systems/     → ResolveBandsSO, CorruptionBandsSO, EncounterTuningTargetsSO
Progression/ → FlagDefinitionSO, EndingCheckSO
Layers/      → LayerGenerationProfileSO
```

---

## 16. Status Effects Summary

45 unique statuses across 12 groups. Key groups:
- **Restraint (tiered):** Snared → Restrained → Bound → Held → Roped → Wrapped (replace_weaker stacking)
- **Comfort/Bloom:** Comforted, Sheltered, Soothing Contact, Drowsy Bloom, Bloom-Drowsed, Petal Mark, Yielding, Kept — provide real short-term benefit with hidden vulnerability
- **Mental/Social:** Uneasy, Shaken, Wavering, Self-Conscious, Suppressed, Hushed, Clinical Strain, Filed
- `Filed`, `Yielding`, `Kept` are STATUS EFFECTS ONLY — not persistent route flags

---

## 17. Locked Design Decisions Log

| ID | Decision |
|---|---|
| A | Universal commands: Attack (Low 0.85), Defend, Item, Run — all heroines |
| B | Magic auto-hits; only physical rolls hit chance |
| C | MAG governs all status chances including on physical abilities |
| D | Corrupted enemy bonus deferred to Seraphine's Judgment upgrade |
| E | Bleed and Poison coexist; each refreshes on re-apply |
| F | Universal Attack is Low (0.85) — all skills strictly better |
| H | Upgrade 1 required before Upgrade 2 unlocks |
| I | Upgrade 2 branch locked permanently — no re-spec |
| J | Assist Ability upgrades static for now |
| K | Passive trigger upgrades: one Bloom tier each, deferred to Bloom economy pass |
| L | Alchemist's Reflex does NOT trigger on Corruption gain |
| M | Stage 4+ framework locked; enemy-specific effects deferred to scene writing |
| N | Heroine KO during grapple: scene → climax, grapple breaks, enemy keeps Frenzy |
| O | Broken Resolve: Cannot Struggle at all |
| P | Layer 1 tuning floor accepted — Lysandra slightly below in Standard A |
| R | Climax Recoil upgrade is shared party upgrade ("Allure of the Abyss") |
| S | FlagManager: persistent singleton MonoBehaviour, not static class |
| T | Grapple is a distinct AbilityType in CharacterAbilitySO — does not deal HP damage, only rolls hit and triggers TryInitiateGrapple() |
| U | ResolveAttack abilities also apply baseCorruptionGain if non-zero (no separate enum value needed — one field, one ability type) |
| X | Blood Nun phase transition is HP-based (below 50%), not round-based. _bloodNunHealedPhase2 resets in StartEncounter() |
| Y | ✅ RESOLVED — RunStateManager now has `CurrentBloom` (seeds from BloomEarned on ReturnToRefuge) and `SpendBloom(int amount)`. RefugeManager uses `RunStateManager.Instance.CurrentBloom` and `SpendBloom()` directly. |
| Z1 | Passive trigger Tier 1 = stronger effect. T2 branches: Lysandra (Blade Frenzy: threshold 20%→10% / Retaliating Edge: power Very Low→Medium), Mira (Full Immunity: negate 50%→100% / Reactive Poison: on negate 40% apply Poison to attacker), Seraphine (Preemptive Vigil: threshold 35%→50% / Restorative Aura: heal +4 Resolve). |
| Z2 | Allure of the Abyss tiers: T1 45 Bloom → Special actions restore 5% max HP. T2 70 Bloom → +5% MP restored + grapple Resolve damage −25% (all stages). T3 100 Bloom → +ATK/DEF buff 1 turn on Special + grapple Resolve damage −50% (replaces −25%). Corruption gain unchanged at all tiers. Same Special action pool across all heroines — heroine-specific sprites only. |
| Z3 | 100 Corruption: ActionType.Attack suppressed. Active heroine forced to use Skills or Special only. Attack button hidden/disabled when corruption >= 100. |
| Z4 | Allure Special actions are shared across all heroines (same action pool). Each heroine has unique cut-in sprites per action. Dialogue per action deferred to h-scene system design pass. |
| Z5 | Seven endings locked (replaces all previous GPT-generated ending names). IDs: unfed_bloom, neverending_cycle, battery_truth, escape_selective, kill_the_castle, become_the_castle_reign, become_the_castle_free (fallback). Priority order and theme locked. Flag conditions are stubs pending flag audit. EvaluateEnding(int currentLayer) signature: neverending_cycle only fires at Layer 9. |

---

## 18. Open Items (Not Yet Designed)

- Bloom economy: LOCKED (see RefugeManager.cs constants — Resolve 5, Corrupt 12/−15, T1 15, T2 25, Allure 45, earn rate layered formula)
- Passive trigger upgrade numbers — ✅ LOCKED (see Decision Z1)
- Allure of the Abyss upgrade branch numbers — ✅ LOCKED (see Decision Z2)
- 100 Corruption mechanical effects — ✅ LOCKED (see Decision Z3)
- Layer 2–10 enemy design (Layer 2 Jailer + standard enemies locked, Layers 3–10 pending)
- Stage 4+ enemy-specific scene effects
- Later-layer abilities targeting Support heroines directly
- Stats for heroines 4–15
- Repeat visit text handling: abbreviated text on revisit (one-line summary + "read again" option) as default; full-skip toggle in settings for veteran players. Most `run_state` seen-flags already in place — hotspot system needs a check on those flags to branch text. Scripted scenes (recruitment, boss preambles) need separate `save_slot` seen-flags.
- Ending condition specifics for all 7 endings — IDs and themes locked (see Z5), flag conditions still STUB pending flag audit
- HSceneAssetSO architecture + dialogue system
- Allure Special action list (what actions exist, what each does per tier)

---

## 19. Development Roadmap & Current Status

### Completed ✅
- Full game design (story, combat formulas, heroine stats, enemy scaling, grapple system)
- All ScriptableObject definitions
- Combat runtime: full turn loop, grapple state machine, flee mechanic, UseItem in combat
- Status effect system: all 45 SOs auto-generated
- CombatUI: enemy stat cards, heroine portrait, collapsible log, inventory strip, skill/special panels, click-to-select target
- Room system: RoomNode, LayerGenerator (10-step pipeline), RoomManager, hotspot population
- Hotspot system: RoomHotspotSO, RoomHotspotPoolSO, RoomTemplateSO, HotspotDisplayUI, ItemPickupUI, EventChoiceUI, LoreReaderUI
- Item system: ItemSO (14 Layer 1 items), ItemManager (6-slot inventory, lore migration)
- Map system: MapUI, MapNodeView
- Refuge system: RefugeManager, RefugeUI
- Bridge/save: CombatRoomBridge (EncounterResult enum, flee routing), SaveSystem, SaveTrigger, FlagManager
- Asset generators: StatusEffectAssetGenerator, CoreAssetGenerator, ItemAssetGenerator, Layer1HotspotAssetGenerator, GameSceneBootstrapper
- Unity: combat loop tested and confirmed working (turn order, swaps, status effects, damage, victory, defeat, flee)
- Scene: `Assets/Scenes/game.unity` auto-built by GameSceneBootstrapper
- Narrative: Layer 1 + Layer 2 prose complete (LAYER1_NARRATIVE_SCENES.md, LAYER2_NARRATIVE_SCENES.md)
- Endings: EndingEvaluator.cs stub (7 endings, priority order, flag helpers)

### Current Focus 🔧
- Full loop QA: Refuge → Start Run → Map → Room → Combat/Event → Return
- Hotspot UI runtime testing
- Layer 2 enemy design (Jailer)
- Layer 2 hotspot asset generator

### Next Up 📋
- Layer 2 hotspot generator (same pattern as Layer1HotspotAssetGenerator)
- Jailer EnemyDataSO + abilities added to CoreAssetGenerator
- Narrative wiring (opening scenes, recruitment, Blood Nun post-boss sequence)
- HSceneAssetSO design and implementation
- Ability upgrade system (RefugeUI upgrade tabs)
- Lore Collection display in Refuge

### Later Passes (Designed, Not Yet Built)
- Layer 3–10 enemy design
- Stage 4+ scene effects
- Ending condition specifics for all 7 endings — IDs and themes locked (see Z5), flag conditions still STUB pending flag audit
- Stats for heroines 4–15
- 100 Corruption mechanical effects
- Allure of the Abyss upgrade branch numbers
- Bloom economy final tuning

### Chat Handoff Instructions
When starting a new chat, attach:
- This file (00_ABYSSAL_BLOOM_MASTER_REFERENCE.md)
- Any .cs files relevant to what you're building next

Opening line for next chat:
> "Continuing Abyssal Bloom Unity development. Master reference is attached.
> Current status: [describe where you are, e.g. 'project opened in Unity,
> ScriptableObject assets created, first test encounter running'].
> Next goal: [what you want to tackle]."
