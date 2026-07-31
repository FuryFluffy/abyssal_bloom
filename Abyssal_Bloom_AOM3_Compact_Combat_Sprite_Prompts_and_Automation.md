# Abyssal Bloom — Compact AOM3 Combat Sprite Prompt Library
These are shortened **AOM3 / SD1.5-friendly** prompts designed to reduce prompt dilution and stop the model from collapsing into default idle poses.
Use the **full positive** and **full negative** blocks exactly as written, then tune only LoRA strength / seed / CFG if needed.
## Workflow notes
- Recommended canvas: **576×832** for base generation.
- For sprite batches, **disable FaceDetailer and hand Detailer in the first pass**. Use them only later if a chosen sprite really needs cleanup.
- In your current workflow, set the **style LoRA node (id 3)** to **0.0 / 0.0** or bypass it entirely, because you said you do **not** want to use a style LoRA.
- Suggested character LoRA strength range: **0.70–0.85**.
- If a pose still collapses into idle, increase the main pose token weight slightly or reduce LoRA strength.
## Subject list
- **LYS** — Lysandra — **LoRA:** `abLysandraSD.safetensors` with trigger `ablysandra`
- **MIR** — Mira Voss — **LoRA:** `abMiraSD.safetensors` with trigger `abmira`
- **SER** — Seraphine — **LoRA:** `abSeraphineSD.safetensors` with trigger `abseraphine`
- **L1_HS** — Hollow Servant — **LoRA:** none
- **L1_KF** — Knife Footman — **LoRA:** none
- **L1_PRN** — Prayer-Rag Novice — **LoRA:** none
- **L1_CB** — Corrupted Butler — **LoRA:** none
- **L1_RWA** — Red-Wax Acolyte — **LoRA:** none
- **L1_BN** — Blood Nun — **LoRA:** none
- **L2_CT** — Chain Thrall — **LoRA:** none
- **L2_IMG** — Iron-Masked Guard — **LoRA:** none
- **L2_CS** — Cell Slime — **LoRA:** none
- **L2_CW** — Chain Warden — **LoRA:** none
- **L2_JAILER** — The Jailer — **LoRA:** none

# Lysandra

**Code:** `LYS`  
**Character LoRA:** `abLysandraSD.safetensors`  
**Trigger tag:** `ablysandra`

## Front view

### Idle — `LYS_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, ablysandra, adult woman, long dark hair in one heavy high ponytail, dreadblade duelist, fitted black leather and cloth armor, restrained dark-fantasy outfit, long black waist curtain, black thigh boots, one-handed steel sword, practical combat gear, no cape, no glow, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, sword held ready in one hand
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `LYS_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, ablysandra, adult woman, long dark hair in one heavy high ponytail, dreadblade duelist, fitted black leather and cloth armor, restrained dark-fantasy outfit, long black waist curtain, black thigh boots, one-handed steel sword, practical combat gear, no cape, no glow, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, mid sword slash or thrust
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `LYS_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, ablysandra, adult woman, long dark hair in one heavy high ponytail, dreadblade duelist, fitted black leather and cloth armor, restrained dark-fantasy outfit, long black waist curtain, black thigh boots, one-handed steel sword, practical combat gear, no cape, no glow, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, sword lowered or pulled back
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `LYS_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, ablysandra, adult woman, long dark hair in one heavy high ponytail, dreadblade duelist, fitted black leather and cloth armor, restrained dark-fantasy outfit, long black waist curtain, black thigh boots, one-handed steel sword, practical combat gear, no cape, no glow, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, sword inaccessible or displaced
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `LYS_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, ablysandra, adult woman, long dark hair in one heavy high ponytail, dreadblade duelist, fitted black leather and cloth armor, restrained dark-fantasy outfit, long black waist curtain, black thigh boots, one-handed steel sword, practical combat gear, no cape, no glow, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, sword held ready in one hand
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `LYS_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, ablysandra, adult woman, long dark hair in one heavy high ponytail, dreadblade duelist, fitted black leather and cloth armor, restrained dark-fantasy outfit, long black waist curtain, black thigh boots, one-handed steel sword, practical combat gear, no cape, no glow, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, mid sword slash or thrust
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `LYS_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, ablysandra, adult woman, long dark hair in one heavy high ponytail, dreadblade duelist, fitted black leather and cloth armor, restrained dark-fantasy outfit, long black waist curtain, black thigh boots, one-handed steel sword, practical combat gear, no cape, no glow, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, sword lowered or pulled back
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `LYS_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, ablysandra, adult woman, long dark hair in one heavy high ponytail, dreadblade duelist, fitted black leather and cloth armor, restrained dark-fantasy outfit, long black waist curtain, black thigh boots, one-handed steel sword, practical combat gear, no cape, no glow, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, sword inaccessible or displaced
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Mira Voss

**Code:** `MIR`  
**Character LoRA:** `abMiraSD.safetensors`  
**Trigger tag:** `abmira`

## Front view

### Idle — `MIR_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abmira, adult woman, vivid red hair in a messy high ponytail, rogue alchemist, pear-shaped figure, fitted dark leather rogue outfit, short layered cloth panels, utility belt, small alchemy pouches and sealed vials, dual steel daggers, practical boots, no cape, no glow, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, dual daggers ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `MIR_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abmira, adult woman, vivid red hair in a messy high ponytail, rogue alchemist, pear-shaped figure, fitted dark leather rogue outfit, short layered cloth panels, utility belt, small alchemy pouches and sealed vials, dual steel daggers, practical boots, no cape, no glow, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, dual dagger strike
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `MIR_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abmira, adult woman, vivid red hair in a messy high ponytail, rogue alchemist, pear-shaped figure, fitted dark leather rogue outfit, short layered cloth panels, utility belt, small alchemy pouches and sealed vials, dual steel daggers, practical boots, no cape, no glow, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, daggers lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `MIR_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abmira, adult woman, vivid red hair in a messy high ponytail, rogue alchemist, pear-shaped figure, fitted dark leather rogue outfit, short layered cloth panels, utility belt, small alchemy pouches and sealed vials, dual steel daggers, practical boots, no cape, no glow, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, daggers inaccessible or lowered
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `MIR_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abmira, adult woman, vivid red hair in a messy high ponytail, rogue alchemist, pear-shaped figure, fitted dark leather rogue outfit, short layered cloth panels, utility belt, small alchemy pouches and sealed vials, dual steel daggers, practical boots, no cape, no glow, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, dual daggers ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `MIR_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abmira, adult woman, vivid red hair in a messy high ponytail, rogue alchemist, pear-shaped figure, fitted dark leather rogue outfit, short layered cloth panels, utility belt, small alchemy pouches and sealed vials, dual steel daggers, practical boots, no cape, no glow, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, dual dagger strike
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `MIR_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abmira, adult woman, vivid red hair in a messy high ponytail, rogue alchemist, pear-shaped figure, fitted dark leather rogue outfit, short layered cloth panels, utility belt, small alchemy pouches and sealed vials, dual steel daggers, practical boots, no cape, no glow, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, daggers lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `MIR_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abmira, adult woman, vivid red hair in a messy high ponytail, rogue alchemist, pear-shaped figure, fitted dark leather rogue outfit, short layered cloth panels, utility belt, small alchemy pouches and sealed vials, dual steel daggers, practical boots, no cape, no glow, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, daggers inaccessible or lowered
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Seraphine

**Code:** `SER`  
**Character LoRA:** `abSeraphineSD.safetensors`  
**Trigger tag:** `abseraphine`

## Front view

### Idle — `SER_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abseraphine, adult woman, tall woman, long white hair as one smooth heavy hair mass, cleric exorcist, white and dark navy layered cleric robes, modest gold trim, fitted bodice, white boots, largest build of the trio, long non-purple wooden and pale-metal staff, simple holy insignia, no cape, no glow, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, staff held ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `SER_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abseraphine, adult woman, tall woman, long white hair as one smooth heavy hair mass, cleric exorcist, white and dark navy layered cleric robes, modest gold trim, fitted bodice, white boots, largest build of the trio, long non-purple wooden and pale-metal staff, simple holy insignia, no cape, no glow, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, staff strike or casting-start motion without projectile
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `SER_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abseraphine, adult woman, tall woman, long white hair as one smooth heavy hair mass, cleric exorcist, white and dark navy layered cleric robes, modest gold trim, fitted bodice, white boots, largest build of the trio, long non-purple wooden and pale-metal staff, simple holy insignia, no cape, no glow, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, staff lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `SER_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abseraphine, adult woman, tall woman, long white hair as one smooth heavy hair mass, cleric exorcist, white and dark navy layered cleric robes, modest gold trim, fitted bodice, white boots, largest build of the trio, long non-purple wooden and pale-metal staff, simple holy insignia, no cape, no glow, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, staff trapped or inaccessible
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `SER_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abseraphine, adult woman, tall woman, long white hair as one smooth heavy hair mass, cleric exorcist, white and dark navy layered cleric robes, modest gold trim, fitted bodice, white boots, largest build of the trio, long non-purple wooden and pale-metal staff, simple holy insignia, no cape, no glow, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, staff held ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `SER_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abseraphine, adult woman, tall woman, long white hair as one smooth heavy hair mass, cleric exorcist, white and dark navy layered cleric robes, modest gold trim, fitted bodice, white boots, largest build of the trio, long non-purple wooden and pale-metal staff, simple holy insignia, no cape, no glow, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, staff strike or casting-start motion without projectile
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `SER_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abseraphine, adult woman, tall woman, long white hair as one smooth heavy hair mass, cleric exorcist, white and dark navy layered cleric robes, modest gold trim, fitted bodice, white boots, largest build of the trio, long non-purple wooden and pale-metal staff, simple holy insignia, no cape, no glow, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, staff lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `SER_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, abseraphine, adult woman, tall woman, long white hair as one smooth heavy hair mass, cleric exorcist, white and dark navy layered cleric robes, modest gold trim, fitted bodice, white boots, largest build of the trio, long non-purple wooden and pale-metal staff, simple holy insignia, no cape, no glow, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, staff trapped or inaccessible
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Hollow Servant

**Code:** `L1_HS`  
**Character LoRA:** none

## Front view

### Idle — `L1_HS_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, hollow servant, gaunt emaciated castle servant, pallid grey skin, sunken eyes, torn dark servant uniform, frayed shirt and trousers, bent spine, long thin arms, dirty bare hands, worn shoes, no weapon, restrained dark-fantasy design, no gore, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, empty hands ready to lunge
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_HS_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, hollow servant, gaunt emaciated castle servant, pallid grey skin, sunken eyes, torn dark servant uniform, frayed shirt and trousers, bent spine, long thin arms, dirty bare hands, worn shoes, no weapon, restrained dark-fantasy design, no gore, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, lunging claw swipe
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_HS_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, hollow servant, gaunt emaciated castle servant, pallid grey skin, sunken eyes, torn dark servant uniform, frayed shirt and trousers, bent spine, long thin arms, dirty bare hands, worn shoes, no weapon, restrained dark-fantasy design, no gore, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, recoiling with empty hands
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_HS_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, hollow servant, gaunt emaciated castle servant, pallid grey skin, sunken eyes, torn dark servant uniform, frayed shirt and trousers, bent spine, long thin arms, dirty bare hands, worn shoes, no weapon, restrained dark-fantasy design, no gore, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, limbs pinned by shadow restraint
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L1_HS_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, hollow servant, gaunt emaciated castle servant, pallid grey skin, sunken eyes, torn dark servant uniform, frayed shirt and trousers, bent spine, long thin arms, dirty bare hands, worn shoes, no weapon, restrained dark-fantasy design, no gore, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, empty hands ready to lunge
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_HS_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, hollow servant, gaunt emaciated castle servant, pallid grey skin, sunken eyes, torn dark servant uniform, frayed shirt and trousers, bent spine, long thin arms, dirty bare hands, worn shoes, no weapon, restrained dark-fantasy design, no gore, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, lunging claw swipe
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_HS_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, hollow servant, gaunt emaciated castle servant, pallid grey skin, sunken eyes, torn dark servant uniform, frayed shirt and trousers, bent spine, long thin arms, dirty bare hands, worn shoes, no weapon, restrained dark-fantasy design, no gore, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, recoiling with empty hands
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_HS_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, hollow servant, gaunt emaciated castle servant, pallid grey skin, sunken eyes, torn dark servant uniform, frayed shirt and trousers, bent spine, long thin arms, dirty bare hands, worn shoes, no weapon, restrained dark-fantasy design, no gore, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, limbs pinned by shadow restraint
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Knife Footman

**Code:** `L1_KF`  
**Character LoRA:** none

## Front view

### Idle — `L1_KF_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted castle footman, lean fast fighter, pale tired face, short dark hair, worn burgundy and black footman coat, cracked dark leather armor, one steel dagger, one small battered round buckler, practical trousers and boots, restrained dark-fantasy design, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, dagger and buckler ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_KF_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted castle footman, lean fast fighter, pale tired face, short dark hair, worn burgundy and black footman coat, cracked dark leather armor, one steel dagger, one small battered round buckler, practical trousers and boots, restrained dark-fantasy design, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, quick dagger strike with buckler forward
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_KF_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted castle footman, lean fast fighter, pale tired face, short dark hair, worn burgundy and black footman coat, cracked dark leather armor, one steel dagger, one small battered round buckler, practical trousers and boots, restrained dark-fantasy design, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, dagger and buckler lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_KF_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted castle footman, lean fast fighter, pale tired face, short dark hair, worn burgundy and black footman coat, cracked dark leather armor, one steel dagger, one small battered round buckler, practical trousers and boots, restrained dark-fantasy design, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, dagger arm restrained, buckler compressed inward
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L1_KF_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted castle footman, lean fast fighter, pale tired face, short dark hair, worn burgundy and black footman coat, cracked dark leather armor, one steel dagger, one small battered round buckler, practical trousers and boots, restrained dark-fantasy design, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, dagger and buckler ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_KF_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted castle footman, lean fast fighter, pale tired face, short dark hair, worn burgundy and black footman coat, cracked dark leather armor, one steel dagger, one small battered round buckler, practical trousers and boots, restrained dark-fantasy design, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, quick dagger strike with buckler forward
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_KF_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted castle footman, lean fast fighter, pale tired face, short dark hair, worn burgundy and black footman coat, cracked dark leather armor, one steel dagger, one small battered round buckler, practical trousers and boots, restrained dark-fantasy design, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, dagger and buckler lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_KF_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted castle footman, lean fast fighter, pale tired face, short dark hair, worn burgundy and black footman coat, cracked dark leather armor, one steel dagger, one small battered round buckler, practical trousers and boots, restrained dark-fantasy design, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, dagger arm restrained, buckler compressed inward
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Prayer-Rag Novice

**Code:** `L1_PRN`  
**Character LoRA:** none

## Front view

### Idle — `L1_PRN_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, prayer-rag novice, thin corrupted religious caster, face partly hidden by layered dirty prayer cloth, loose faded burgundy and grey robes made from tied prayer rags, simple cord belt, long plain wooden staff, cloth strips hanging from sleeves, no readable writing, no glow, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, staff ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_PRN_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, prayer-rag novice, thin corrupted religious caster, face partly hidden by layered dirty prayer cloth, loose faded burgundy and grey robes made from tied prayer rags, simple cord belt, long plain wooden staff, cloth strips hanging from sleeves, no readable writing, no glow, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, staff jab or restrained casting motion without projectile
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_PRN_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, prayer-rag novice, thin corrupted religious caster, face partly hidden by layered dirty prayer cloth, loose faded burgundy and grey robes made from tied prayer rags, simple cord belt, long plain wooden staff, cloth strips hanging from sleeves, no readable writing, no glow, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, staff lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_PRN_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, prayer-rag novice, thin corrupted religious caster, face partly hidden by layered dirty prayer cloth, loose faded burgundy and grey robes made from tied prayer rags, simple cord belt, long plain wooden staff, cloth strips hanging from sleeves, no readable writing, no glow, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, staff trapped or inaccessible
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L1_PRN_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, prayer-rag novice, thin corrupted religious caster, face partly hidden by layered dirty prayer cloth, loose faded burgundy and grey robes made from tied prayer rags, simple cord belt, long plain wooden staff, cloth strips hanging from sleeves, no readable writing, no glow, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, staff ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_PRN_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, prayer-rag novice, thin corrupted religious caster, face partly hidden by layered dirty prayer cloth, loose faded burgundy and grey robes made from tied prayer rags, simple cord belt, long plain wooden staff, cloth strips hanging from sleeves, no readable writing, no glow, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, staff jab or restrained casting motion without projectile
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_PRN_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, prayer-rag novice, thin corrupted religious caster, face partly hidden by layered dirty prayer cloth, loose faded burgundy and grey robes made from tied prayer rags, simple cord belt, long plain wooden staff, cloth strips hanging from sleeves, no readable writing, no glow, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, staff lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_PRN_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, prayer-rag novice, thin corrupted religious caster, face partly hidden by layered dirty prayer cloth, loose faded burgundy and grey robes made from tied prayer rags, simple cord belt, long plain wooden staff, cloth strips hanging from sleeves, no readable writing, no glow, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, staff trapped or inaccessible
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Corrupted Butler

**Code:** `L1_CB`  
**Character LoRA:** none

## Front view

### Idle — `L1_CB_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted butler, tall elegant but unsettling castle servant, slick dark hair, pale courteous face, fitted black tailcoat, dark burgundy waistcoat, white gloves, narrow trousers, polished shoes, one heavy silver serving tray used as a weapon, immaculate silhouette with subtle wear, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, silver tray held ready like a weapon
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_CB_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted butler, tall elegant but unsettling castle servant, slick dark hair, pale courteous face, fitted black tailcoat, dark burgundy waistcoat, white gloves, narrow trousers, polished shoes, one heavy silver serving tray used as a weapon, immaculate silhouette with subtle wear, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, swinging the silver tray
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_CB_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted butler, tall elegant but unsettling castle servant, slick dark hair, pale courteous face, fitted black tailcoat, dark burgundy waistcoat, white gloves, narrow trousers, polished shoes, one heavy silver serving tray used as a weapon, immaculate silhouette with subtle wear, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, tray lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_CB_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted butler, tall elegant but unsettling castle servant, slick dark hair, pale courteous face, fitted black tailcoat, dark burgundy waistcoat, white gloves, narrow trousers, polished shoes, one heavy silver serving tray used as a weapon, immaculate silhouette with subtle wear, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, tray trapped close to the body
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L1_CB_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted butler, tall elegant but unsettling castle servant, slick dark hair, pale courteous face, fitted black tailcoat, dark burgundy waistcoat, white gloves, narrow trousers, polished shoes, one heavy silver serving tray used as a weapon, immaculate silhouette with subtle wear, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, silver tray held ready like a weapon
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_CB_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted butler, tall elegant but unsettling castle servant, slick dark hair, pale courteous face, fitted black tailcoat, dark burgundy waistcoat, white gloves, narrow trousers, polished shoes, one heavy silver serving tray used as a weapon, immaculate silhouette with subtle wear, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, swinging the silver tray
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_CB_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted butler, tall elegant but unsettling castle servant, slick dark hair, pale courteous face, fitted black tailcoat, dark burgundy waistcoat, white gloves, narrow trousers, polished shoes, one heavy silver serving tray used as a weapon, immaculate silhouette with subtle wear, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, tray lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_CB_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, corrupted butler, tall elegant but unsettling castle servant, slick dark hair, pale courteous face, fitted black tailcoat, dark burgundy waistcoat, white gloves, narrow trousers, polished shoes, one heavy silver serving tray used as a weapon, immaculate silhouette with subtle wear, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, tray trapped close to the body
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Red-Wax Acolyte

**Code:** `L1_RWA`  
**Character LoRA:** none

## Front view

### Idle — `L1_RWA_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, red-wax acolyte, pale support caster, dark hair beneath a simple fitted hood, layered charcoal and deep burgundy acolyte robes, hardened red wax seals and wax drips on cuffs and belt, plain wooden staff capped with dull brass, cloth armor under robes, no readable symbols, no glow, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, staff ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_RWA_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, red-wax acolyte, pale support caster, dark hair beneath a simple fitted hood, layered charcoal and deep burgundy acolyte robes, hardened red wax seals and wax drips on cuffs and belt, plain wooden staff capped with dull brass, cloth armor under robes, no readable symbols, no glow, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, staff casting motion without projectile
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_RWA_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, red-wax acolyte, pale support caster, dark hair beneath a simple fitted hood, layered charcoal and deep burgundy acolyte robes, hardened red wax seals and wax drips on cuffs and belt, plain wooden staff capped with dull brass, cloth armor under robes, no readable symbols, no glow, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, staff lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_RWA_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, red-wax acolyte, pale support caster, dark hair beneath a simple fitted hood, layered charcoal and deep burgundy acolyte robes, hardened red wax seals and wax drips on cuffs and belt, plain wooden staff capped with dull brass, cloth armor under robes, no readable symbols, no glow, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, staff trapped or inaccessible
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L1_RWA_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, red-wax acolyte, pale support caster, dark hair beneath a simple fitted hood, layered charcoal and deep burgundy acolyte robes, hardened red wax seals and wax drips on cuffs and belt, plain wooden staff capped with dull brass, cloth armor under robes, no readable symbols, no glow, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, staff ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_RWA_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, red-wax acolyte, pale support caster, dark hair beneath a simple fitted hood, layered charcoal and deep burgundy acolyte robes, hardened red wax seals and wax drips on cuffs and belt, plain wooden staff capped with dull brass, cloth armor under robes, no readable symbols, no glow, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, staff casting motion without projectile
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_RWA_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, red-wax acolyte, pale support caster, dark hair beneath a simple fitted hood, layered charcoal and deep burgundy acolyte robes, hardened red wax seals and wax drips on cuffs and belt, plain wooden staff capped with dull brass, cloth armor under robes, no readable symbols, no glow, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, staff lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_RWA_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, red-wax acolyte, pale support caster, dark hair beneath a simple fitted hood, layered charcoal and deep burgundy acolyte robes, hardened red wax seals and wax drips on cuffs and belt, plain wooden staff capped with dull brass, cloth armor under robes, no readable symbols, no glow, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, staff trapped or inaccessible
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Blood Nun

**Code:** `L1_BN`  
**Character LoRA:** none

## Front view

### Idle — `L1_BN_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, blood nun boss, tall imposing corrupted nun, severe pale face, dark hair mostly hidden beneath a black and blood-red habit, layered black chain armor beneath religious robes, faded red veil panels, heavy segmented flagellant lash in one hand, reinforced boots, restrained sacramental detail, no gore, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, flagellant lash ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_BN_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, blood nun boss, tall imposing corrupted nun, severe pale face, dark hair mostly hidden beneath a black and blood-red habit, layered black chain armor beneath religious robes, faded red veil panels, heavy segmented flagellant lash in one hand, reinforced boots, restrained sacramental detail, no gore, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, lash strike windup or downswing
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_BN_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, blood nun boss, tall imposing corrupted nun, severe pale face, dark hair mostly hidden beneath a black and blood-red habit, layered black chain armor beneath religious robes, faded red veil panels, heavy segmented flagellant lash in one hand, reinforced boots, restrained sacramental detail, no gore, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, lash lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_BN_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, blood nun boss, tall imposing corrupted nun, severe pale face, dark hair mostly hidden beneath a black and blood-red habit, layered black chain armor beneath religious robes, faded red veil panels, heavy segmented flagellant lash in one hand, reinforced boots, restrained sacramental detail, no gore, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, lash arm restrained
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L1_BN_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, blood nun boss, tall imposing corrupted nun, severe pale face, dark hair mostly hidden beneath a black and blood-red habit, layered black chain armor beneath religious robes, faded red veil panels, heavy segmented flagellant lash in one hand, reinforced boots, restrained sacramental detail, no gore, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, flagellant lash ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L1_BN_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, blood nun boss, tall imposing corrupted nun, severe pale face, dark hair mostly hidden beneath a black and blood-red habit, layered black chain armor beneath religious robes, faded red veil panels, heavy segmented flagellant lash in one hand, reinforced boots, restrained sacramental detail, no gore, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, lash strike windup or downswing
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L1_BN_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, blood nun boss, tall imposing corrupted nun, severe pale face, dark hair mostly hidden beneath a black and blood-red habit, layered black chain armor beneath religious robes, faded red veil panels, heavy segmented flagellant lash in one hand, reinforced boots, restrained sacramental detail, no gore, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, lash lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L1_BN_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1girl, adult woman, blood nun boss, tall imposing corrupted nun, severe pale face, dark hair mostly hidden beneath a black and blood-red habit, layered black chain armor beneath religious robes, faded red veil panels, heavy segmented flagellant lash in one hand, reinforced boots, restrained sacramental detail, no gore, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, lash arm restrained
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Chain Thrall

**Code:** `L2_CT`  
**Character LoRA:** none

## Front view

### Idle — `L2_CT_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, chain thrall, thin bent former prisoner, pale bruised skin without gore, shaved or ragged short hair, torn grey prison tunic and trousers, iron collar, wrist shackles and ankle shackles, several dragging chains, one loose chain used as a lash, bare or wrapped feet, cold dungeon palette, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, loose chain ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_CT_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, chain thrall, thin bent former prisoner, pale bruised skin without gore, shaved or ragged short hair, torn grey prison tunic and trousers, iron collar, wrist shackles and ankle shackles, several dragging chains, one loose chain used as a lash, bare or wrapped feet, cold dungeon palette, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, whipping chain strike
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_CT_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, chain thrall, thin bent former prisoner, pale bruised skin without gore, shaved or ragged short hair, torn grey prison tunic and trousers, iron collar, wrist shackles and ankle shackles, several dragging chains, one loose chain used as a lash, bare or wrapped feet, cold dungeon palette, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, chain lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_CT_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, chain thrall, thin bent former prisoner, pale bruised skin without gore, shaved or ragged short hair, torn grey prison tunic and trousers, iron collar, wrist shackles and ankle shackles, several dragging chains, one loose chain used as a lash, bare or wrapped feet, cold dungeon palette, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, chains tangled and body pinned
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L2_CT_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, chain thrall, thin bent former prisoner, pale bruised skin without gore, shaved or ragged short hair, torn grey prison tunic and trousers, iron collar, wrist shackles and ankle shackles, several dragging chains, one loose chain used as a lash, bare or wrapped feet, cold dungeon palette, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, loose chain ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_CT_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, chain thrall, thin bent former prisoner, pale bruised skin without gore, shaved or ragged short hair, torn grey prison tunic and trousers, iron collar, wrist shackles and ankle shackles, several dragging chains, one loose chain used as a lash, bare or wrapped feet, cold dungeon palette, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, whipping chain strike
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_CT_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, chain thrall, thin bent former prisoner, pale bruised skin without gore, shaved or ragged short hair, torn grey prison tunic and trousers, iron collar, wrist shackles and ankle shackles, several dragging chains, one loose chain used as a lash, bare or wrapped feet, cold dungeon palette, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, chain lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_CT_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult male monster, chain thrall, thin bent former prisoner, pale bruised skin without gore, shaved or ragged short hair, torn grey prison tunic and trousers, iron collar, wrist shackles and ankle shackles, several dragging chains, one loose chain used as a lash, bare or wrapped feet, cold dungeon palette, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, chains tangled and body pinned
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Iron-Masked Guard

**Code:** `L2_IMG`  
**Character LoRA:** none

## Front view

### Idle — `L2_IMG_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, iron-masked guard, oversized dungeon enforcer, sealed featureless iron mask, broad heavy body, blackened cell-door plate armor, riveted leather underlayer, ring of keys and blank prison tags at the belt, heavy iron baton, rectangular door-like shield, thick boots, cold blue-grey dungeon detail, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, baton and shield ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_IMG_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, iron-masked guard, oversized dungeon enforcer, sealed featureless iron mask, broad heavy body, blackened cell-door plate armor, riveted leather underlayer, ring of keys and blank prison tags at the belt, heavy iron baton, rectangular door-like shield, thick boots, cold blue-grey dungeon detail, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, baton strike with shield braced
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_IMG_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, iron-masked guard, oversized dungeon enforcer, sealed featureless iron mask, broad heavy body, blackened cell-door plate armor, riveted leather underlayer, ring of keys and blank prison tags at the belt, heavy iron baton, rectangular door-like shield, thick boots, cold blue-grey dungeon detail, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, baton lowered with shield sagging
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_IMG_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, iron-masked guard, oversized dungeon enforcer, sealed featureless iron mask, broad heavy body, blackened cell-door plate armor, riveted leather underlayer, ring of keys and blank prison tags at the belt, heavy iron baton, rectangular door-like shield, thick boots, cold blue-grey dungeon detail, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, baton arm restrained, shield pinned inward
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L2_IMG_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, iron-masked guard, oversized dungeon enforcer, sealed featureless iron mask, broad heavy body, blackened cell-door plate armor, riveted leather underlayer, ring of keys and blank prison tags at the belt, heavy iron baton, rectangular door-like shield, thick boots, cold blue-grey dungeon detail, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, baton and shield ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_IMG_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, iron-masked guard, oversized dungeon enforcer, sealed featureless iron mask, broad heavy body, blackened cell-door plate armor, riveted leather underlayer, ring of keys and blank prison tags at the belt, heavy iron baton, rectangular door-like shield, thick boots, cold blue-grey dungeon detail, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, baton strike with shield braced
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_IMG_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, iron-masked guard, oversized dungeon enforcer, sealed featureless iron mask, broad heavy body, blackened cell-door plate armor, riveted leather underlayer, ring of keys and blank prison tags at the belt, heavy iron baton, rectangular door-like shield, thick boots, cold blue-grey dungeon detail, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, baton lowered with shield sagging
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_IMG_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, iron-masked guard, oversized dungeon enforcer, sealed featureless iron mask, broad heavy body, blackened cell-door plate armor, riveted leather underlayer, ring of keys and blank prison tags at the belt, heavy iron baton, rectangular door-like shield, thick boots, cold blue-grey dungeon detail, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, baton arm restrained, shield pinned inward
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Cell Slime

**Code:** `L2_CS`  
**Character LoRA:** none

## Front view

### Idle — `L2_CS_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, cell slime monster, low broad translucent prison-residue mass, dirty blue-grey gelatinous body, embedded grit, rust flakes, broken nail fragments and old hair, faint muted violet motes inside, one unstable pseudo-face near the leading edge, thick dragging base, damp gloss but not 3d-rendered, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, low ready mass, subtle forward creep
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, bad anatomy, messy silhouette, 3d render, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_CS_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, cell slime monster, low broad translucent prison-residue mass, dirty blue-grey gelatinous body, embedded grit, rust flakes, broken nail fragments and old hair, faint muted violet motes inside, one unstable pseudo-face near the leading edge, thick dragging base, damp gloss but not 3d-rendered, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, surging pseudopod strike
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, bad anatomy, messy silhouette, 3d render, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_CS_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, cell slime monster, low broad translucent prison-residue mass, dirty blue-grey gelatinous body, embedded grit, rust flakes, broken nail fragments and old hair, faint muted violet motes inside, one unstable pseudo-face near the leading edge, thick dragging base, damp gloss but not 3d-rendered, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, distorted recoiling mass
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, bad anatomy, messy silhouette, 3d render, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_CS_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, cell slime monster, low broad translucent prison-residue mass, dirty blue-grey gelatinous body, embedded grit, rust flakes, broken nail fragments and old hair, faint muted violet motes inside, one unstable pseudo-face near the leading edge, thick dragging base, damp gloss but not 3d-rendered, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, compressed by shadow restraint or containment silhouette
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, bad anatomy, messy silhouette, 3d render, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L2_CS_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, cell slime monster, low broad translucent prison-residue mass, dirty blue-grey gelatinous body, embedded grit, rust flakes, broken nail fragments and old hair, faint muted violet motes inside, one unstable pseudo-face near the leading edge, thick dragging base, damp gloss but not 3d-rendered, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, low ready mass, subtle forward creep
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, bad anatomy, messy silhouette, 3d render, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_CS_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, cell slime monster, low broad translucent prison-residue mass, dirty blue-grey gelatinous body, embedded grit, rust flakes, broken nail fragments and old hair, faint muted violet motes inside, one unstable pseudo-face near the leading edge, thick dragging base, damp gloss but not 3d-rendered, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, surging pseudopod strike
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, bad anatomy, messy silhouette, 3d render, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_CS_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, cell slime monster, low broad translucent prison-residue mass, dirty blue-grey gelatinous body, embedded grit, rust flakes, broken nail fragments and old hair, faint muted violet motes inside, one unstable pseudo-face near the leading edge, thick dragging base, damp gloss but not 3d-rendered, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, distorted recoiling mass
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, bad anatomy, messy silhouette, 3d render, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_CS_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, cell slime monster, low broad translucent prison-residue mass, dirty blue-grey gelatinous body, embedded grit, rust flakes, broken nail fragments and old hair, faint muted violet motes inside, one unstable pseudo-face near the leading edge, thick dragging base, damp gloss but not 3d-rendered, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, compressed by shadow restraint or containment silhouette
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, bad anatomy, messy silhouette, 3d render, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# Chain Warden

**Code:** `L2_CW`  
**Character LoRA:** none

## Front view

### Idle — `L2_CW_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, chain warden elite, tall lean dungeon officer, narrow iron cage helmet with face hidden in darkness, long black leather warden coat over dark chainmail, layered belts with chain reels and lock mechanisms, reinforced gloves, long hooked chain-glaive, controlled chains fixed to the back rig, disciplined silhouette, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, chain-glaive ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_CW_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, chain warden elite, tall lean dungeon officer, narrow iron cage helmet with face hidden in darkness, long black leather warden coat over dark chainmail, layered belts with chain reels and lock mechanisms, reinforced gloves, long hooked chain-glaive, controlled chains fixed to the back rig, disciplined silhouette, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, hooked chain-glaive swing or thrust
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_CW_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, chain warden elite, tall lean dungeon officer, narrow iron cage helmet with face hidden in darkness, long black leather warden coat over dark chainmail, layered belts with chain reels and lock mechanisms, reinforced gloves, long hooked chain-glaive, controlled chains fixed to the back rig, disciplined silhouette, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, glaive lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_CW_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, chain warden elite, tall lean dungeon officer, narrow iron cage helmet with face hidden in darkness, long black leather warden coat over dark chainmail, layered belts with chain reels and lock mechanisms, reinforced gloves, long hooked chain-glaive, controlled chains fixed to the back rig, disciplined silhouette, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, weapon arm and back chains restrained
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L2_CW_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, chain warden elite, tall lean dungeon officer, narrow iron cage helmet with face hidden in darkness, long black leather warden coat over dark chainmail, layered belts with chain reels and lock mechanisms, reinforced gloves, long hooked chain-glaive, controlled chains fixed to the back rig, disciplined silhouette, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, chain-glaive ready
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_CW_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, chain warden elite, tall lean dungeon officer, narrow iron cage helmet with face hidden in darkness, long black leather warden coat over dark chainmail, layered belts with chain reels and lock mechanisms, reinforced gloves, long hooked chain-glaive, controlled chains fixed to the back rig, disciplined silhouette, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, hooked chain-glaive swing or thrust
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_CW_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, chain warden elite, tall lean dungeon officer, narrow iron cage helmet with face hidden in darkness, long black leather warden coat over dark chainmail, layered belts with chain reels and lock mechanisms, reinforced gloves, long hooked chain-glaive, controlled chains fixed to the back rig, disciplined silhouette, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, glaive lowered while recoiling
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_CW_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1man, adult man, chain warden elite, tall lean dungeon officer, narrow iron cage helmet with face hidden in darkness, long black leather warden coat over dark chainmail, layered belts with chain reels and lock mechanisms, reinforced gloves, long hooked chain-glaive, controlled chains fixed to the back rig, disciplined silhouette, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, weapon arm and back chains restrained
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# The Jailer

**Code:** `L2_JAILER`  
**Character LoRA:** none

## Front view

### Idle — `L2_JAILER_front_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, the jailer major boss, massive constructed guardian, part prison architecture and part living creature, enormous humanoid stone-and-iron body, chest like a barred cell door, black iron ribs and lock plates, heavy chains linking shoulders and wrists, oversized hands, broad rounded childlike mask, monumental but readable silhouette, front view, facing viewer, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, massive ready stance
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_JAILER_front_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, the jailer major boss, massive constructed guardian, part prison architecture and part living creature, enormous humanoid stone-and-iron body, chest like a barred cell door, black iron ribs and lock plates, heavy chains linking shoulders and wrists, oversized hands, broad rounded childlike mask, monumental but readable silhouette, front view, facing viewer, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, heavy arm swing or downward smash windup
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_JAILER_front_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, the jailer major boss, massive constructed guardian, part prison architecture and part living creature, enormous humanoid stone-and-iron body, chest like a barred cell door, black iron ribs and lock plates, heavy chains linking shoulders and wrists, oversized hands, broad rounded childlike mask, monumental but readable silhouette, front view, facing viewer, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, recoiling monumental body
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_JAILER_front_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, the jailer major boss, massive constructed guardian, part prison architecture and part living creature, enormous humanoid stone-and-iron body, chest like a barred cell door, black iron ribs and lock plates, heavy chains linking shoulders and wrists, oversized hands, broad rounded childlike mask, monumental but readable silhouette, front view, facing viewer, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, bound by many shadow chains or containment silhouette
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, rear view, back view, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

## Back view

### Idle — `L2_JAILER_back_idle`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, the jailer major boss, massive constructed guardian, part prison architecture and part living creature, enormous humanoid stone-and-iron body, chest like a barred cell door, black iron ribs and lock plates, heavy chains linking shoulders and wrists, oversized hands, broad rounded childlike mask, monumental but readable silhouette, rear view, back view, combat idle, neutral ready stance, compact pose footprint, balanced stance, readable silhouette, weapon ready but lowered, massive ready stance
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, attack pose, hurt, injured, staggering, grappled, restrained, kneeling, sitting
```

### Attack — `L2_JAILER_back_attack`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, the jailer major boss, massive constructed guardian, part prison architecture and part living creature, enormous humanoid stone-and-iron body, chest like a barred cell door, black iron ribs and lock plates, heavy chains linking shoulders and wrists, oversized hands, broad rounded childlike mask, monumental but readable silhouette, rear view, back view, (attack pose:1.35), compact combat attack, readable mid-action silhouette, controlled step, action suited for turn-based combat sprite, no huge leap, no oversized swing trail, heavy arm swing or downward smash windup
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, neutral pose, standing still, calm pose, resting
```

### Hurt — `L2_JAILER_back_hurt`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, the jailer major boss, massive constructed guardian, part prison architecture and part living creature, enormous humanoid stone-and-iron body, chest like a barred cell door, black iron ribs and lock plates, heavy chains linking shoulders and wrists, oversized hands, broad rounded childlike mask, monumental but readable silhouette, rear view, back view, (hurt pose:1.35), (staggering:1.25), recoiling from a hit, off-balance but still standing, readable pain reaction, compact sprite footprint, recoiling monumental body
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, calm, smiling, triumphant, attack pose, relaxed
```

### Grappled — `L2_JAILER_back_grappled`

**Positive**
```text
masterpiece, best quality, anime game character sprite, full body, entire body visible, centered composition, feet fully visible, clean silhouette, vertical sprite framing, slightly elevated tactical-game camera, mild orthographic feeling, minimal foreshortening, plain bright cyan background, no environment, no scenery, no floor, no cast shadow, 1monster, the jailer major boss, massive constructed guardian, part prison architecture and part living creature, enormous humanoid stone-and-iron body, chest like a barred cell door, black iron ribs and lock plates, heavy chains linking shoulders and wrists, oversized hands, broad rounded childlike mask, monumental but readable silhouette, rear view, back view, (grappled:1.35), (restrained:1.3), struggling posture, body tension, arms partially pinned or pulled, compact captive pose, simple shadowy grappler silhouette behind or beside the subject, grappler secondary and indistinct, bound by many shadow chains or containment silhouette
```

**Negative**
```text
nsfw, (worst quality, low quality:1.4), (realistic, lip, nose, tooth, rouge, lipstick, eyeshadow:1.0), (depth of field, bokeh, blurry:1.4), motion blur, film grain, chromatic aberration, lens flare, text, title, logo, signature, watermark, scenery, environment, floor, cast shadow, cropped, close-up, portrait, multiple subjects, extra limbs, extra digits, bad anatomy, front view, facing viewer, looking at viewer, idle, free standing, calm, relaxed, attack pose, triumphant, explicit, nude, sexual
```

# ComfyUI automation for this workflow

Your uploaded workflow is a **GUI workflow JSON**, but ComfyUI batch queue automation works best with an **API-format workflow JSON**.

## Best method

1. Open the workflow in ComfyUI.
2. Make a **sprite-batch variant**:
   - set **style LoRA node 3** to **0.0 / 0.0** or bypass it
   - set **character LoRA node 4** to the heroine LoRA you want, or leave it ready to be replaced by script
   - set **EmptyLatentImage node 8** to **576×832**
   - temporarily **bypass FaceDetailer nodes 18 and 20** for first-pass pose generation
   - keep the final **SaveImage node 15**
3. In ComfyUI, save that workflow as **API format JSON**.
4. Use the CSV batch file below plus the Python submission script below to queue every sprite automatically.

## Node IDs the script patches in your workflow

- **3** — Style LoRA (`strength_model`, `strength_clip`) → set to 0 when style LoRA is disabled
- **4** — Character LoRA (`lora_name`, `strength_model`, `strength_clip`)
- **6** — Positive CLIP text
- **7** — Negative CLIP text
- **8** — Width / Height / batch size
- **10** — First KSampler seed / steps / cfg
- **12** — Second KSampler seed / steps / cfg
- **15** — filename prefix

## Why API queueing is better than pasting prompts manually

- every prompt is preserved exactly
- you can generate all front/back/idle/attack/hurt/grappled states in one queue
- you can batch by subject or by enemy layer
- you can keep seeds reproducible and output names organized

