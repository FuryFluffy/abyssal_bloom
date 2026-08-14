# Abyssal Bloom — Layer 1 & 2 LoRA Review and Canonical Pose Prompt Pack

**Version:** 1.0  
**Date:** 2026-08-14  
**Scope:** 13 identity LoRAs; Idle, Attack, Hurt, Defeated and Move; front and back variants.

## Executive result

All thirteen LoRAs are usable. None shows a catastrophic checkpoint mismatch or broad style failure. The remaining problems are local and controllable: Mira requires explicit colour anchors; Hollow Servant requires explicit adult/gaunt anchors and VFX suppression; Butler, Knife Footman, Guard and Jailer require equipment-count checks; full-body back poses for the three heroines need reference control. Intrinsic identity light or material—Cell Slime's internal glow, the Jailer's contained cage light, and the Red-Wax Acolyte's wax/blood staining—should remain. External flashes, trails, projectiles, smoke, splashes, loose particles and floor effects should be generated separately.

VFX contamination is not an SD 1.5-only issue. SDXL LoRAs can learn repeated effects and backgrounds too. In these tests the clearest entanglement is Hollow Servant's shoulder flame; it can be suppressed by caption language and negatives. Cell Slime drips and Jailer glow are mostly identity geometry/material rather than accidental scene VFX.

## Locked production settings

| Setting | Value |
|---|---|
| Checkpoint | `animagine-xl-4.0-opt.safetensors` |
| Canvas | 832 × 1216 |
| Sampler / scheduler | Euler a / Normal |
| Steps / CFG | 28 / 5.0 |
| Denoise / CLIP setting | 1.0 / default / no CLIP Set Last Layer node |
| Style LoRA | None during canonical generation |
| First-pass batch | 4 images; Jailer 8 images |
| Background | Solid cyan for masking; remove only after selection |
| VFX | Separate layer; never bake trails, particles, projectiles, rings or floor shadows into the body sprite |

Use the same LoRA strength for both Model and CLIP. Do not use a refiner or hires pass before the pose and identity are approved. Once an Idle Front and Idle Back are accepted, lock one seed family per orientation and use those references for the remaining states.

## Control workflow

1. Generate `Idle Front` and `Idle Back` first. Use batch 4, except Jailer batch 8.
2. Select one front and one back whose proportions, costume and equipment are canonical. Record the seeds.
3. Use OpenPose/DWPose at 0.75–0.85 for humanoid front poses. A skeleton alone does not reliably encode facing direction; use Depth, Lineart or a same-orientation reference for backs.
4. Use IPAdapter only when identity or costume drifts: 0.25–0.35, same-orientation reference, end around 0.80. Never use it to copy a scene background.
5. Use SoftEdge/Depth for Cell Slime and Depth for the Guard and Jailer.
6. Correct one missing/extra prop by inpainting. Do not raise LoRA weight to solve counting; that increases entanglement.
7. Remove the cyan background only after selection. Composite attacks, impacts, blood, smoke, spell light, selection rings and status effects afterward.

## Review matrix

| LoRA | Preferred weight | Decision | Handling |
|---|---:|---|---|
| Blood Nun | 0.75 | Ready with prop checks | Generate four. Keep the halo circular and complete. Reject extra candles/books. The floor-length layered nun robes are canonical; do not add a cape. |
| Corrupted Butler | 0.75 | Conditional: count props | Use 0.75, not 0.85. State exactly one tray and one knife. Reject duplicates; inpaint the hands/props if needed instead of increasing weight. |
| Hollow Servant | 0.70 | Conditional: age and VFX anchors | Always say gaunt adult man, hollow cheeks and exhausted mature face. Keep violet fire/smoke out of the sprite and add it later as VFX if wanted. |
| Knife Footman | 0.80 | Conditional: weapon count | Use exactly two short fighting knives and fitted black trousers in every armed pose. The narrow torn split tailcoat is canonical; do not turn it into a cape or broad cloak. |
| Prayer-Rag Novice | 0.70 | Ready | Use a clear casting gesture for Attack, then composite the ranged/magical projectile separately. Keep the face partly shadowed by the hood but still readable in front views. |
| Red-Wax Acolyte | 0.75 | Ready with prop checks | Red wax and cloth staining are intrinsic costume details, not disposable VFX. Keep swing trails, smoke and loose particles separate. Reject extra censers or malformed halos. |
| Lysandra | 0.75 | Ready; control full-body/back views | Use a same-orientation reference or Depth for back views. Describe the narrow feathered hip drapes explicitly; never add a cape. |
| Mira Voss | 0.80 | Conditional: explicit hair anchor mandatory | Never use the trigger alone. Keep the red-hair and eye-colour anchors in every prompt and negative blue/black hair. Use the dagger for the generic Attack state; reserve flasks for separate ability art. |
| Seraphine | 0.70 | Ready; control full-body/back views | Use an empty-hand casting pose for the generic Light Attack and composite light separately. Describe the long skirt panels as hip drapes, not a cape. |
| Chain Thrall | 0.75 | Ready | Treat wrist, ankle and collar chains as body-attached costume geometry, not VFX. Keep chain lengths inside the canvas and reject boots or sandals. |
| Iron-Masked Guard | 0.80 | Ready with equipment prompt | Always state one rectangular barred tower shield and one spiked iron mace. Use Depth for rear poses so the shield does not migrate or become a second body panel. |
| Cell Slime | 0.70 | Ready; use shape control | Use SoftEdge or Depth rather than DWPose. The internal purple glow and viscous body drips are identity; detached droplets, puddle splashes and attack arcs are separate VFX. |
| Jailer | 0.70 | Conditional: lantern count | Generate eight, select the cleanest four-lantern silhouette, then inpaint the chain rig if needed. Do not solve counting by raising LoRA weight. Keep the small internal purple light; remove external aura and particles. |

## Canonical prompts

Every prompt below is complete and paste-ready. Load only the named identity LoRA at the listed Model and CLIP weight. The negative prompt is repeated deliberately so each state can be copied independently.

### 1. Blood Nun

- **LoRA:** `ABBloodNun.safetensors`
- **Trigger:** `ab_blood_nun`
- **Weight:** `0.75` Model / `0.75` CLIP
- **Test finding:** 0.75 retained the spiked halo, veils, black/white/red palette and adult silhouette without the rigidity of 0.80. Candle and book placement still varies.
- **Production handling:** Generate four. Keep the halo circular and complete. Reject extra candles/books. The floor-length layered nun robes are canonical; do not add a cape.
- **Control:** OpenPose 0.80; use Depth 0.65 for back views; optional same-orientation IPAdapter 0.25.

#### Idle — Front

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one tall red candle in a brass holder in one hand, holding exactly one closed dark prayer book in the other hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Idle — Back

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one tall red candle in a brass holder in one hand, holding exactly one closed dark prayer book in the other hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Attack — Front

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, wide ritual casting stance, one arm extending exactly one tall red candle in a brass holder, other hand presenting one open dark prayer book, sleeves controlled, no spell effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Attack — Back

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, wide ritual casting stance, one arm extending exactly one tall red candle in a brass holder, other hand presenting one open dark prayer book, sleeves controlled, no spell effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Hurt — Front

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one tall red candle in a brass holder in one hand, holding exactly one closed dark prayer book in the other hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Hurt — Back

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one tall red candle in a brass holder in one hand, holding exactly one closed dark prayer book in the other hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Defeated — Front

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Defeated — Back

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Move — Front

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one tall red candle in a brass holder in one hand, holding exactly one closed dark prayer book in the other hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

#### Move — Back

**Positive**

```text
1girl, ab_blood_nun, adult woman, blood nun, pale face, red eyes, black hood, layered white veil, complete spiked circular brass halo behind head, floor-length layered black nun robes, narrow ivory devotional panels, blood-red embroidery, red wax stains, brass chains, black boots, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one tall red candle in a brass holder in one hand, holding exactly one closed dark prayer book in the other hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra candle, multiple candles, extra book, multiple books, weapon, staff, broken halo, incomplete halo, red aura
```

### 2. Corrupted Butler

- **LoRA:** `ABCorruptedButler.safetensors`
- **Trigger:** `ab_corrupted_butler`
- **Weight:** `0.75` Model / `0.75` CLIP
- **Test finding:** Identity is strong at every tested weight. The targeted 0.85 pass produced the intended tray-and-knife language but also duplicated knives. The ragged split tailcoat is learned strongly and is part of the supplied design, not a cape.
- **Production handling:** Use 0.75, not 0.85. State exactly one tray and one knife. Reject duplicates; inpaint the hands/props if needed instead of increasing weight.
- **Control:** OpenPose 0.80; Depth 0.70 for back; IPAdapter 0.30 from approved idle if trouser colour drifts.

#### Idle — Front

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one round silver serving tray in the left hand like a shield, holding exactly one short table knife in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Idle — Back

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one round silver serving tray in the left hand like a shield, holding exactly one short table knife in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Attack — Front

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, combat stance, thrusting exactly one short table knife forward with the right hand, exactly one round silver serving tray raised defensively in the left hand, no second weapon, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Attack — Back

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, combat stance, thrusting exactly one short table knife with the right hand, exactly one round silver serving tray raised defensively in the left hand, no second weapon, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Hurt — Front

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one round silver serving tray in the left hand like a shield, holding exactly one short table knife in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Hurt — Back

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one round silver serving tray in the left hand like a shield, holding exactly one short table knife in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Defeated — Front

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Defeated — Back

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Move — Front

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one round silver serving tray in the left hand like a shield, holding exactly one short table knife in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

#### Move — Back

**Positive**

```text
1boy, ab_corrupted_butler, adult man, corrupted butler, slim elegant build, messy black hair covering one eye, pale cracked face, black formal tailcoat with narrow ragged split coat tails, charcoal waistcoat with gold piping, white shirt, black tie, fitted black trousers, black gloves, polished black shoes, small violet corruption seams, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one round silver serving tray in the left hand like a shield, holding exactly one short table knife in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra knife, two knives, multiple knives, extra tray, multiple trays, sword, round shield, white trousers, beige trousers, long flowing coat, cloak
```

### 3. Hollow Servant

- **LoRA:** `ABHollowServant.safetensors`
- **Trigger:** `ab_hollow_servant`
- **Weight:** `0.70` Model / `0.70` CLIP
- **Test finding:** The clean 0.70 prompt suppresses the learned shoulder flames, but neutral tests drift toward a youthful, healthy face. The clothing silhouette is very stable.
- **Production handling:** Always say gaunt adult man, hollow cheeks and exhausted mature face. Keep violet fire/smoke out of the sprite and add it later as VFX if wanted.
- **Control:** OpenPose 0.82; Depth 0.65 for back; optional IPAdapter 0.30 from the supplied adult portrait.

#### Idle — Front

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, empty hands, long fingers relaxed, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Idle — Back

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, empty hands, long fingers relaxed, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Attack — Front

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, predatory melee lunge, torso leaning forward, one clawed hand reaching toward the target, other clawed hand drawn back, no magic effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Attack — Back

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, predatory melee lunge, torso leaning forward, one clawed hand reaching toward the target, other clawed hand drawn back, no magic effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Hurt — Front

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, empty hands, long fingers relaxed, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Hurt — Back

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, empty hands, long fingers relaxed, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Defeated — Front

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Defeated — Back

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Move — Front

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, empty hands, long fingers relaxed, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

#### Move — Back

**Positive**

```text
1boy, ab_hollow_servant, adult man, gaunt hollow servant, tall slim body, short dishevelled silver-grey hair, hollow cheeks, exhausted mature face, sunken grey eyes, pallid cracked skin, fitted black servant jacket with muted gold piping, black ribbon tie, torn white apron, narrow ragged black coat tails, fitted black trousers, black boots, claw-like pale hands, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, empty hands, long fingers relaxed, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, child, boy, teen, youthful face, healthy face, purple fire, violet flame, smoke, shoulder aura, glowing shoulders, weapon
```

### 4. Knife Footman

- **LoRA:** `ABKnifeFootman.safetensors`
- **Trigger:** `ab_knife_footman`
- **Weight:** `0.80` Model / `0.80` CLIP
- **Test finding:** 0.80 gave the most stable black uniform and the targeted test reliably produced two knives. Lower weights introduced white or beige trousers.
- **Production handling:** Use exactly two short fighting knives and fitted black trousers in every armed pose. The narrow torn split tailcoat is canonical; do not turn it into a cape or broad cloak.
- **Control:** OpenPose 0.82; Depth 0.68 for back; inpaint one hand if the knife count is wrong.

#### Idle — Front

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly two short matching fighting knives, one knife in each hand, blades pointed down, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Idle — Back

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly two short matching fighting knives, one knife in each hand, blades pointed down, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Attack — Front

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, fast low melee lunge, crossing slash with exactly two short matching fighting knives, one knife in each hand, both blades fully visible, no slash trail, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Attack — Back

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, fast low melee lunge, crossing slash with exactly two short matching fighting knives, one knife in each hand, both blades fully visible, no slash trail, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Hurt — Front

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly two short matching fighting knives, one knife in each hand, blades pointed down, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Hurt — Back

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly two short matching fighting knives, one knife in each hand, blades pointed down, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Defeated — Front

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Defeated — Back

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Move — Front

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly two short matching fighting knives, one knife in each hand, blades pointed down, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

#### Move — Back

**Positive**

```text
1boy, ab_knife_footman, adult man, knife footman, lean athletic build, short tousled black hair, stern face, fitted black double-breasted footman jacket with subtle gold piping, narrow torn split tailcoat, black tie, fitted black trousers, black gloves, polished black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly two short matching fighting knives, one knife in each hand, blades pointed down, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, one knife, three knives, extra knife, multiple weapons, sword, long blade, white trousers, beige trousers, long flowing coat, broad coat tails, cloak
```

### 5. Prayer-Rag Novice

- **LoRA:** `ABPrayerRagNovice.safetensors`
- **Trigger:** `ab_prayer_rag_novice`
- **Weight:** `0.70` Model / `0.70` CLIP
- **Test finding:** 0.70 preserved the adult feminine silhouette, rag layers and red thorn crown consistently. The design does not need visible projectile VFX baked into the sprite.
- **Production handling:** Use a clear casting gesture for Attack, then composite the ranged/magical projectile separately. Keep the face partly shadowed by the hood but still readable in front views.
- **Control:** OpenPose 0.80; Depth 0.65 for back; no IPAdapter unless the hood/crown changes.

#### Idle — Front

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, empty hands, fingers slightly curled, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Idle — Back

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, empty hands, fingers slightly curled, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Attack — Front

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible beneath hood, ranged invocation stance, one clawed hand extended toward the target, other hand held near the chest in prayer, no projectile, no magic effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Attack — Back

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, ranged invocation stance, one clawed hand extended toward the target, other hand held near the chest in prayer, no projectile, no magic effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Hurt — Front

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, empty hands, fingers slightly curled, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Hurt — Back

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, empty hands, fingers slightly curled, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Defeated — Front

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Defeated — Back

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Move — Front

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, empty hands, fingers slightly curled, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

#### Move — Back

**Positive**

```text
1girl, ab_prayer_rag_novice, adult woman, prayer-rag novice, slim body, long ash-brown hair partly hidden, shadowed tired eyes, layered bone-white and dirty grey prayer rags, torn hood and face veil, red wax stitches, small red thorn-and-candle crown, wrapped forearms and calves, bare or cloth-wrapped feet, long claw-like fingers, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, empty hands, fingers slightly curled, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, weapon, staff, book, visible spell, magic orb, fire, purple aura, extra crown, missing hood
```

### 6. Red-Wax Acolyte

- **LoRA:** `ABRedWaxAcolyte_v2.safetensors`
- **Trigger:** `ab_red_wax_acolyte`
- **Weight:** `0.75` Model / `0.75` CLIP
- **Test finding:** 0.75 retained the face veil, complete halo, red-white robe design and censer. 0.60 was visibly weaker; 0.80 added rigidity without a useful gain.
- **Production handling:** Red wax and cloth staining are intrinsic costume details, not disposable VFX. Keep swing trails, smoke and loose particles separate. Reject extra censers or malformed halos.
- **Control:** OpenPose 0.80; Depth 0.70 for back; optional IPAdapter 0.25 for halo/veil continuity.

#### Idle — Front

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face veil visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one heavy brass censer on a chain in the left hand, right hand lifting a short devotional chain, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Idle — Back

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one heavy brass censer on a chain in the left hand, right hand lifting a short devotional chain, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Attack — Front

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face veil visible, wide balanced stance, left arm swinging exactly one heavy brass censer outward on its chain, right arm extended for balance, censer fully visible, no smoke, no swing trail, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Attack — Back

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, wide balanced stance, left arm swinging exactly one heavy brass censer outward on its chain, right arm extended for balance, censer fully visible, no smoke, no swing trail, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Hurt — Front

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face veil visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one heavy brass censer on a chain in the left hand, right hand lifting a short devotional chain, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Hurt — Back

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one heavy brass censer on a chain in the left hand, right hand lifting a short devotional chain, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Defeated — Front

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face veil visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Defeated — Back

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Move — Front

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face veil visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one heavy brass censer on a chain in the left hand, right hand lifting a short devotional chain, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

#### Move — Back

**Positive**

```text
1girl, ab_red_wax_acolyte, adult woman, red-wax acolyte, pale skin, long straight black hair under a white hood, eyes covered by a red wax-stained white face veil, complete spiked circular brass halo, layered floor-length white ritual robes soaked and edged with dark red wax, black inner bodice, brass chains and small crosses, black gloves, black shoes, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one heavy brass censer on a chain in the left hand, right hand lifting a short devotional chain, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, extra censer, multiple censers, broken halo, incomplete halo, uncovered eyes, bare face, smoke, incense cloud, floating wax, airborne blood
```

### 7. Lysandra

- **LoRA:** `ABLysandra.safetensors`
- **Trigger:** `ab_lysandra`
- **Weight:** `0.75` Model / `0.75` CLIP
- **Test finding:** Face, black hair, violet-black palette and gold lattice styling were stable at 0.75. The tests were portrait-heavy, so full-body rear poses need orientation control.
- **Production handling:** Use a same-orientation reference or Depth for back views. Describe the narrow feathered hip drapes explicitly; never add a cape.
- **Control:** OpenPose 0.80 for front; Depth or Lineart 0.70 for back; IPAdapter 0.30 from approved same-orientation reference.

#### Idle — Front

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one slender dark longsword lowered at her side, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Idle — Back

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one slender dark longsword lowered at her side, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Attack — Front

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, decisive sword attack stance, lunging forward with exactly one slender dark longsword in a two-handed diagonal slash, blade fully visible, no slash trail, no magic effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Attack — Back

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, decisive sword attack stance, lunging forward with exactly one slender dark longsword in a two-handed diagonal slash, blade fully visible, no slash trail, no magic effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Hurt — Front

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one slender dark longsword lowered at her side, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Hurt — Back

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one slender dark longsword lowered at her side, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Defeated — Front

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Defeated — Back

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Move — Front

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one slender dark longsword lowered at her side, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

#### Move — Back

**Positive**

```text
1girl, ab_lysandra, adult woman, Lysandra the Dreadblade, tall athletic curvy build, very long black hair in a high ponytail, violet eyes, pale skin, stern expression, fitted black and charcoal battle bodysuit, intricate antique-gold chain lattice and filigree, black feathered hip drapes, black thigh-high armored boots, black gloves, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one slender dark longsword lowered at her side, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, extra sword, two swords, shield, purple aura, magic circle
```

### 8. Mira Voss

- **LoRA:** `ABMiraVoss.safetensors`
- **Trigger:** `ab_mira_voss`
- **Weight:** `0.80` Model / `0.80` CLIP
- **Test finding:** Trigger-only tests produced blue hair even at 1.00. At 0.80, explicitly stating dark red hair, high ponytail, green eyes and beauty mark corrected all four targeted samples.
- **Production handling:** Never use the trigger alone. Keep the red-hair and eye-colour anchors in every prompt and negative blue/black hair. Use the dagger for the generic Attack state; reserve flasks for separate ability art.
- **Control:** OpenPose 0.80 for front; Depth 0.70 for back; IPAdapter 0.30 from an approved red-haired reference is recommended.

#### Idle — Front

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one short alchemist dagger lowered at her side, other hand relaxed near the vial belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Idle — Back

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one short alchemist dagger lowered at her side, other hand relaxed near the vial belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Attack — Front

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, agile dagger attack stance, quick forward thrust with exactly one short alchemist dagger, off hand guarding near the vial belt, no poison cloud, no slash trail, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Attack — Back

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, agile dagger attack stance, quick forward thrust with exactly one short alchemist dagger, off hand guarding near the vial belt, no poison cloud, no slash trail, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Hurt — Front

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one short alchemist dagger lowered at her side, other hand relaxed near the vial belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Hurt — Back

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one short alchemist dagger lowered at her side, other hand relaxed near the vial belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Defeated — Front

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Defeated — Back

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Move — Front

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one short alchemist dagger lowered at her side, other hand relaxed near the vial belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

#### Move — Back

**Positive**

```text
1girl, ab_mira_voss, adult woman, Mira Voss the Alchemist, curvy athletic build, dark red hair, messy high ponytail, loose red side locks, green eyes, small beauty mark below one eye, confident expression, fitted black alchemist jacket and corset with antique-gold lattice seams, fitted charcoal trousers, black high-heeled boots, purple glass vials at belt, narrow dark-purple feathered hip drapes, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one short alchemist dagger lowered at her side, other hand relaxed near the vial belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, blue hair, navy hair, black hair, silver hair, short hair, loose hair, extra dagger, two daggers, cape, cloak, wings, poison cloud, purple puddle, magic circle
```

### 9. Seraphine

- **LoRA:** `ABSeraphine.safetensors`
- **Trigger:** `ab_seraphine`
- **Weight:** `0.70` Model / `0.70` CLIP
- **Test finding:** 0.70 was sufficient for stable silver-white hair, pale face and white/black/gold outfit. Higher weight did not materially improve identity.
- **Production handling:** Use an empty-hand casting pose for the generic Light Attack and composite light separately. Describe the long skirt panels as hip drapes, not a cape.
- **Control:** OpenPose 0.80 for front; Depth or Lineart 0.70 for back; optional IPAdapter 0.30 from approved same-orientation reference.

#### Idle — Front

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, empty hands, small purple censer hanging motionless at the belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Idle — Back

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, empty hands, small purple censer hanging motionless at the belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Attack — Front

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled Light casting stance, one open palm extended toward the target, other hand braced near the chest, small belt censer still, no visible spell, no magic circle, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Attack — Back

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled Light casting stance, one open palm extended toward the target, other hand braced near the chest, small belt censer still, no visible spell, no magic circle, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Hurt — Front

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, empty hands, small purple censer hanging motionless at the belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Hurt — Back

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, empty hands, small purple censer hanging motionless at the belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Defeated — Front

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Defeated — Back

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Move — Front

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, empty hands, small purple censer hanging motionless at the belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

#### Move — Back

**Positive**

```text
1girl, ab_seraphine, adult woman, Seraphine the Purifier, tall curvy build, very long silver-white hair, pale grey-violet eyes, serene determined expression, ornate white and black fitted ritual bodice, antique-gold chain lattice and filigree, white split skirt panels with black inner layers, black-and-gold thigh-high armored boots, black gloves, small purple censer hanging at the belt, no cape, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, empty hands, small purple censer hanging motionless at the belt, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, cape, cloak, wings, giant feather mantle, weapon, staff, sword, bright aura, light beam, magic circle, floating particles, extra censer
```

### 10. Chain Thrall

- **LoRA:** `ABChainThrall.safetensors`
- **Trigger:** `ab_chain_thrall`
- **Weight:** `0.75` Model / `0.75` CLIP
- **Test finding:** 0.75 kept the gaunt adult male, torn grey rags, bare feet and restraint system. Higher weight added ornamental spear-like chain ends and rigidity.
- **Production handling:** Treat wrist, ankle and collar chains as body-attached costume geometry, not VFX. Keep chain lengths inside the canvas and reject boots or sandals.
- **Control:** OpenPose 0.82; Depth 0.65 for back; keep all chain endpoints inside the frame.

#### Idle — Front

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, face visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, empty hands, restraint chains hanging with visible slack, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Idle — Back

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, empty hands, restraint chains hanging with visible slack, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Attack — Front

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, desperate reaching melee attack, crouched forward lunge, one open hand reaching toward target, other arm dragged back by wrist chain, ankle chain taut, no weapon, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Attack — Back

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, desperate reaching melee attack, crouched forward lunge, one open hand reaching toward target, other arm dragged back by wrist chain, ankle chain taut, no weapon, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Hurt — Front

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, empty hands, restraint chains hanging with visible slack, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Hurt — Back

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, empty hands, restraint chains hanging with visible slack, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Defeated — Front

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Defeated — Back

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Move — Front

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, face visible, controlled forward walking stride, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, empty hands, restraint chains hanging with visible slack, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

#### Move — Back

**Positive**

```text
1boy, ab_chain_thrall, adult man, chain thrall, gaunt muscular body, messy black hair covering part of face, exhausted hostile eyes, bruised dirty skin, torn sleeveless grey prison rags, wrapped waist, bare feet, heavy iron collar, iron cuffs on both wrists, iron shackles on both ankles, short dragging restraint chains attached to cuffs and shackles, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, controlled forward walking stride away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, empty hands, restraint chains hanging with visible slack, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, boots, shoes, sandals, armor, shirt, weapon, spearhead chain, arrowhead chain, extra chains crossing face, floating chains
```

### 11. Iron-Masked Guard

- **LoRA:** `ABIronMaskedGuard.safetensors`
- **Trigger:** `ab_iron_masked_guard`
- **Weight:** `0.80` Model / `0.80` CLIP
- **Test finding:** 0.80 gave a stable massive black/brass armor silhouette. The targeted equipment prompt produced the rectangular barred tower shield and single spiked mace reliably.
- **Production handling:** Always state one rectangular barred tower shield and one spiked iron mace. Use Depth for rear poses so the shield does not migrate or become a second body panel.
- **Control:** Depth 0.72 is preferred for every pose; OpenPose 0.65 may be added for attacks; inpaint a missing or duplicate item.

#### Idle — Front

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, helmet front visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one tall rectangular barred prison-door tower shield in the left hand, holding exactly one heavy spiked iron mace in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Idle — Back

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, holding exactly one tall rectangular barred prison-door tower shield in the left hand, holding exactly one heavy spiked iron mace in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Attack — Front

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, wide heavy stance, exactly one tall rectangular barred prison-door tower shield braced in the left hand, exactly one heavy spiked iron mace raised in the right hand for a downward strike, no impact effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Attack — Back

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, no head turn, wide heavy stance, exactly one tall rectangular barred prison-door tower shield braced in the left hand, exactly one heavy spiked iron mace raised in the right hand for a downward strike, no impact effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Hurt — Front

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, helmet front visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one tall rectangular barred prison-door tower shield in the left hand, holding exactly one heavy spiked iron mace in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Hurt — Back

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, holding exactly one tall rectangular barred prison-door tower shield in the left hand, holding exactly one heavy spiked iron mace in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Defeated — Front

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, helmet front visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, shield and mace absent from frame, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Defeated — Back

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, empty hands, shield and mace absent from frame, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Move — Front

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, helmet front visible, slow heavy guarded step, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one tall rectangular barred prison-door tower shield in the left hand, holding exactly one heavy spiked iron mace in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

#### Move — Back

**Positive**

```text
1other, ab_iron_masked_guard, monster, iron-masked guard, enormous broad armored humanoid, full blackened iron plate armor with antique-brass edging, cage-like barred helmet with no visible face, massive squared pauldrons, purple tattered waist cloth, heavy sabatons, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, slow heavy guarded step away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, holding exactly one tall rectangular barred prison-door tower shield in the left hand, holding exactly one heavy spiked iron mace in the right hand, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra shield, two shields, round shield, extra mace, two maces, sword, spear, floating weapon, cape
```

### 12. Cell Slime

- **LoRA:** `ABCellSlime.safetensors`
- **Trigger:** `ab_cell_slime`
- **Weight:** `0.70` Model / `0.70` CLIP
- **Test finding:** 0.70 clean tests preserved the translucent violet-black body and barred domed head. Wide attack prompts can turn the arms into wing-like membranes; neutral wording avoids that.
- **Production handling:** Use SoftEdge or Depth rather than DWPose. The internal purple glow and viscous body drips are identity; detached droplets, puddle splashes and attack arcs are separate VFX.
- **Control:** SoftEdge 0.70 or Depth 0.65; do not use DWPose. For back views, reference the supplied back image.

#### Idle — Front

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, head grille visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, arms hanging low, compact puddled base, no detached droplets, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Idle — Back

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, arms hanging low, compact puddled base, no detached droplets, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Attack — Front

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, head grille facing viewer, asymmetric reaching attack, one long viscous forearm extending toward target with four narrow claws, other arm held low, compact torso, no wing membrane, no splash effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Attack — Back

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear of smooth domed head visible, head grille not visible, asymmetric reaching attack, one long viscous forearm extending away from viewer with four narrow claws, other arm held low, compact torso, no wing membrane, no splash effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Hurt — Front

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, head grille visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, arms hanging low, compact puddled base, no detached droplets, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Hurt — Back

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, arms hanging low, compact puddled base, no detached droplets, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Defeated — Front

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, head grille visible, defeated pose, body collapsed into a low broad puddle, domed head and shoulders still discernible, arms slack in the slime, no splash, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Defeated — Back

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, body collapsed into a low broad puddle, domed head and shoulders still discernible, arms slack in the slime, no splash, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Move — Front

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, head grille visible, flowing forward locomotion, compact puddled base stretching into one clear directional surge, torso upright, arms balanced, no detached droplets, no splash, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

#### Move — Back

**Positive**

```text
1other, ab_cell_slime, monster, cell slime, translucent violet-black humanoid slime, tall amorphous body, domed faceless head, vertical prison-bar grille embedded across the front of the head, dim internal purple core glow, two long viscous arms ending in four narrow claws, tapering torso merging into a compact puddled base, no legs, safe, solo, full body, entire body visible, centered, puddled base visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, flowing forward locomotion, compact puddled base stretching into one clear directional surge, torso upright, arms balanced, no detached droplets, no splash, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, human face, eyes, mouth, hair, clothing, armor, legs, feet, wings, wing membrane, detached floating droplets, splash, slime projectile, multiple heads, skulls
```

### 13. Jailer

- **LoRA:** `ABJailer.safetensors`
- **Trigger:** `ab_jailer`
- **Weight:** `0.70` Model / `0.70` CLIP
- **Test finding:** 0.70 preserved the prison-architecture armor and chain-tendril language, but the number and placement of cage lanterns varied. Raising weight increased chain clutter rather than solving counting.
- **Production handling:** Generate eight, select the cleanest four-lantern silhouette, then inpaint the chain rig if needed. Do not solve counting by raising LoRA weight. Keep the small internal purple light; remove external aura and particles.
- **Control:** Depth 0.72 for all poses; OpenPose 0.60 only for arm direction; batch 8; inpaint/count-correct the chain rig.

#### Idle — Front

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, symmetrical camera angle, helmet front visible, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, four chain tendrils spread symmetrically behind the body, four cage lanterns hanging clear of the silhouette, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Idle — Back

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, neutral idle stance, weight evenly distributed, arms relaxed, calm readable posture, four chain tendrils spread symmetrically behind the body, four cage lanterns hanging clear of the silhouette, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Attack — Front

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, wide boss stance, one armored hand reaching toward target, exactly four back-mounted chain tendrils sweeping outward, each ending in one barred cage lantern, four cage lanterns total, no motion trail, no energy effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Attack — Back

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, no head turn, wide boss stance, one armored hand reaching away from viewer, exactly four back-mounted chain tendrils sweeping outward, each ending in one barred cage lantern, four cage lanterns total, no motion trail, no energy effect, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Hurt — Front

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, helmet front visible, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, four chain tendrils spread symmetrically behind the body, four cage lanterns hanging clear of the silhouette, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Hurt — Back

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, hurt recoil pose, torso bent slightly, one shoulder pulled back, one arm guarding the ribs, knees flexed, off balance but still standing, no wound effects, four chain tendrils spread symmetrically behind the body, four cage lanterns hanging clear of the silhouette, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Defeated — Front

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, helmet front visible, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, chain tendrils lowered and cage lanterns resting close to the body, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Defeated — Back

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, defeated pose, collapsed on both knees, head lowered, shoulders slumped, arms hanging slack, stable compact silhouette, not dead, chain tendrils lowered and cage lanterns resting close to the body, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Move — Front

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, front view, facing viewer, helmet front visible, slow heavy marching step, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, four chain tendrils spread symmetrically behind the body, four cage lanterns hanging clear of the silhouette, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, back view, from behind, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

#### Move — Back

**Positive**

```text
1other, ab_jailer, monster, the Jailer, colossal prison-warden construct, blackened iron and antique-brass fortress armor, tall barred tower helmet with no visible face, barred cage built into chest, broad spiked pauldrons, layered black armored skirt plates with narrow tattered purple cloth between plates, heavy armored boots, exactly four long articulated chain tendrils emerging from the back, each tendril ending in exactly one barred cage lantern, four cage lanterns total, dim contained purple light inside helmet chest and lanterns, safe, solo, full body, entire body visible, centered, feet visible, game character sprite, fixed camera distance, orthographic-like view, minimal perspective, readable silhouette, simple background, solid cyan background, back view, from behind, rear view, face not visible, no head turn, slow heavy marching step away from viewer, one foot ahead, balanced arms, upright torso, readable locomotion pose, no running, no motion blur, four chain tendrils spread symmetrically behind the body, four cage lanterns hanging clear of the silhouette, masterpiece, high score, great score, absurdres
```

**Negative**

```text
lowres, worst quality, low quality, low score, bad score, average score, jpeg artifacts, blurry, error, bad anatomy, bad hands, malformed hands, missing finger, extra digits, fewer digits, extra limbs, missing limbs, fused limbs, duplicate character, multiple people, multiple views, character sheet, split screen, text, watermark, username, signature, logo, cropped, out of frame, close-up, upper body, extreme perspective, fisheye, strong foreshortening, motion blur, depth of field, scenery, detailed background, gradient background, floor, cast shadow, reflection, cape, superhero cape, aura, magic circle, particles, sparks, projectile, attack trail, motion trail, impact effect, airborne blood, blood splatter, gore, selection ring, status icon, front view, three-quarter view, face, looking at viewer, visible face, open helmet, extra arm, extra chain tendril, fewer than four lanterns, more than four lanterns, floating unattached lantern, giant cage building, prison room, cape, bright purple aura, lightning
```

## Acceptance checklist

For every selected image verify: one character only; entire silhouette inside frame; fixed scale; correct front/back orientation; no face visible in a back pose; no cape; correct costume palette; correct weapon/prop count; all hands and endpoints visible; no scene floor or cast shadow; no projectile, impact, aura, trail, loose particle or UI element; cyan background clean enough for masking.

For paired front/back assets also verify equivalent camera height, equivalent occupied canvas height, matching garment length, matching equipment side and matching halo/chain count. Back-view failures should be regenerated with Depth/Lineart or repaired, not accepted as three-quarter views.
