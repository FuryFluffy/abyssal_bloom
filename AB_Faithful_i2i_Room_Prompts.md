# Abyssal Bloom — Faithful i2i Room Prompt Pack

These prompts are intended for the **AB_01B Faithful Room Restyle SD1.5** workflow. The source image is authoritative: the prompt asks AOM3 and the style LoRA to repaint the existing composition, not redesign it.

## Test settings

```text
Recommended first-pass settings for AB_01B Faithful Room Restyle:
- Denoise: 0.48
- Depth ControlNet: 0.95
- Canny ControlNet: 0.45
- LoRA model/CLIP strength: 0.20
- CFG: 6.0
- Steps: 28
- Sampler: DPM++ 2M
- Scheduler: Karras

Adjustments:
- Layout drifting: denoise 0.42–0.45, Depth 1.00, Canny 0.50.
- Style too weak: denoise 0.52–0.55 or LoRA 0.25.
- Repeated beds/doors merging: lower denoise before increasing Canny.
```

## L1-01 — Wine Cellar

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, medieval castle wine cellar, preserve the large left-side wooden wine rack with crossed bottle slots, preserve the central recessed stone alcove, preserve the central rectangular wooden table with empty surface, preserve the right-side arched wooden door and its iron hardware, preserve both wall-mounted flame sconces, preserve the vaulted stone ceiling, rough stone walls and open stone floor, same object positions and proportions, empty wine rack, empty table, no barrels, no bottles, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, barrels, wine bottles, extra racks, extra shelves, cabinet, changed table shape, missing alcove
```

## L1-02 — Servant Corridor

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, medieval castle servant corridor, preserve the long centered hallway and exact vanishing point, preserve the repeated arched wooden doors on both walls, preserve the existing door count and spacing, preserve the symmetrical stone columns, vaulted ceiling and broad unobstructed central walking path, preserve the existing wall-torch positions and the dark corridor opening at the far end, slightly damp reflective stone floor, maintained but recently abandoned servant wing, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, extra doors, missing doors, door at far end, portcullis, side passage, crooked corridor, asymmetrical arches
```

## L1-03 — Servant Dormitory

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, medieval castle servant dormitory, preserve the rectangular room and centered central aisle, preserve the two rows of wooden single beds, preserve every existing bed position, blanket and pillow, preserve the small empty wall shelves above the beds, preserve the rear wooden door, preserve the two hanging lanterns and the open doorway at the front right, mechanically tidy prepared beds, orderly abandoned living quarters, warm even lighting, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, extra beds, missing beds, merged beds, bunk beds, metal hospital beds, extra pillows, personal belongings, cluttered shelves
```

## L1-04 — Ruined Confessional

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, ruined medieval chapel interior, preserve the exact source composition and camera position, preserve the intact dark wooden confessional booth as the primary focal point, preserve its carved wood, screen, slightly open penitent door and worn kneeler, preserve the existing collapsed pew, stone debris, partial arch and damaged walls, preserve the open floor around the confessional and the existing chapel exit, single focused shaft of light illuminating the confessional against a cold ruined chapel, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, missing confessional, extra confessional, intact pristine chapel, restored walls, extra pews, altar replacing confessional, stained glass
```

## L1-05 — Servant Ledger Alcove

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, small medieval stone writing alcove, preserve the close front-facing composition, preserve the wooden writing desk, chair and drawer arrangement, preserve the clean empty desk surface, preserve the central candle sconce above the desk, preserve the empty iron hooks on the left wall, preserve the small empty shallow shelf on the right wall, worn wood with faint old ink stains, intimate warm candlelight, clear prepared writing workspace, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, paper, ledger, book, quill, ink bottle, scroll, extra candle, extra shelves, missing desk, cluttered desk
```

## L1-06 — Bell-Pull Pantry

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, medieval castle service pantry, preserve the tall empty wooden shelving unit on the left, preserve the existing ceramic jars on the lowest shelf, preserve the wooden door beside the shelving, preserve the exact row of hanging bell-pull mechanisms, every pulley, braided cord, wooden handle and blank tag, preserve the overhead hanging lantern and open empty stone floor, organized functional servant-service room, clear wall space around the bell pulls, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, extra bell pulls, missing bell pulls, merged cords, tangled ropes, readable labels, filled shelves, extra jars, pantry food, clutter
```

## L1-07 — Coat Beside Service Door

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, close medieval service-door view, preserve the heavy wooden door occupying the right side, preserve its iron bands, handle, ring and surrounding stone frame, preserve the wall-mounted flame sconce on the left, preserve the single empty iron coat hook beside it and its distinct cast shadow, preserve the warm line of light beneath or beyond the door and the narrow visible passage at the right edge, minimal intimate environmental-storytelling composition, the empty hook remains the focal detail, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, coat, cloak, clothing, multiple hooks, extra doors, open wide doorway, missing hook, missing sconce, character silhouette
```

## L1-08 — Blood Nun Chapel

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, grand ruined medieval gothic chapel used as a ritual processing chamber, preserve the exact symmetrical nave composition, preserve the tall ribbed vaulted ceiling, stone columns and large open central floor, preserve the circular worn floor marking in the foreground and the raised empty altar platform at the rear, preserve all displaced wooden pew positions, iron candle stands, red candles and hanging chandeliers, preserve the existing side doorway, restrained blood-red and amber candlelight, functional maintained ritual chamber prepared for confrontation, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, character on altar, statue, corpse, gore, blood pool, extra pews in center aisle, bright daylight, white candles, missing altar
```

## L2-01 — Dungeon Corridor

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, medieval castle dungeon corridor, preserve the long centered corridor and exact vanishing point, preserve the heavy stone arches, low ceiling and broad central stone path, preserve the solid iron-banded cell doors with small barred windows on both walls, preserve the existing door count, spacing, outside bolts and wall-torch positions, preserve the raised portcullis and descending passage at the far end, cold blue-amber dungeon lighting, wet maintained stone and polished iron, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, extra cell doors, missing cell doors, open cells, wooden servant doors, missing portcullis, prison bars across entire corridor
```

## L2-02 — Jailer's Circular Chamber

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, large circular dungeon chamber, preserve the exact wide symmetrical composition and domed ceiling, preserve the circular stone floor rings and empty worn center, preserve the central massive unlit iron chandelier, preserve the identical inward-facing cell doors evenly spaced around the curved wall, preserve the small barred windows and faint warm light behind selected doors, preserve the larger central rear entrance door and its steps, cold dim observation chamber designed around the empty center, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, boss creature, figure in center, altar, arena props, extra chandelier, missing doors, uneven door spacing, bright center light
```

## L2-03 — Farthest Cell / Wake Scene

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, single medieval dungeon cell, preserve the intimate source composition, preserve the narrow iron bed against the left wall and its neatly prepared bare bedding, preserve the small wooden table on the right, preserve the wall candle or sconce above it, preserve the open iron shackle mounted on the right wall, preserve the small high barred window and its cold grey light, preserve the open barred cell door and dark corridor beyond, warm candlelight against cold stone, prepared cell that feels like an offering rather than an abandoned prison, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, extra bed, extra table, chair, prisoner, closed cell door, locked shackle, multiple shackles, large window, daylight
```

## L2-04 — Empty Men's Cell

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, large communal medieval dungeon cell, preserve the exact source composition and centered aisle, preserve the two orderly rows of narrow iron beds, preserve every existing bed position and identical dark bedding, preserve the small wall shelves above the beds and all existing centered cups, preserve the clean iron wash basin at the far end, preserve the open barred entrance at the near edge, cold even institutional lighting, oppressive mechanical tidiness, no evidence of occupants, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, missing beds, extra beds, merged beds, wooden beds, pillows, personal belongings, displaced cups, people, messy bedding
```

## L2-05 — Offering List Room

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, small dungeon classification and records room, preserve the exact three-quarter source composition, preserve the wooden writing desk and stool on the left, preserve the clean empty desk surface, preserve the organized grid of iron hooks above the desk, preserve the existing blank hanging tags and empty hooks, preserve the tall open wooden cabinet on the right with deliberate empty shelves and faint dust outlines, preserve the open barred door at the left edge and the wall-mounted flame sconce, functional cold bureaucratic room prepared to categorize people, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, readable writing, ledger, books, scrolls, filled cabinet, extra tags, random papers, missing desk, closed cabinet
```

## L2-06 — False Safe Cell

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, comfortable medieval dungeon cell, preserve the exact intimate source composition, preserve the wider iron bed with neatly folded warm blanket on the left, preserve the small wooden table and proper backed chair in the center-right, preserve the empty shelf at comfortable reading height on the right wall, preserve the small iron brazier or heater at the far wall, preserve the existing wall sconces and open barred entrance, noticeably warmer amber lighting than other dungeon rooms, comfort prepared too deliberately to feel safe, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, luxury bedroom, carpet, curtains, extra furniture, food, books, flowers, fireplace, missing chair, missing brazier, cold blue lighting
```

## REFUGE — Bloom Refuge Hub

### Positive

```text
(masterpiece, best quality:1.1), abbloomstyle, dark fantasy anime game background, 2d hand-painted environment, controlled painterly brushwork, simplified material detail, clean readable silhouettes, restrained dark palette, (faithful source-image composition:1.3), (same camera angle:1.3), (same framing:1.3), (same perspective:1.3), (same room layout:1.3), preserve all major architecture, permanent fixtures, furniture positions and lighting placement, the same farthest dungeon cell now used as a modest refuge, preserve the exact source-room geometry and camera, preserve the iron bed, small wooden table, high barred window, wall shackle and open cell door, preserve subtle signs of repeated use without adding loose clutter, a small cloth tied around one rough bed-frame joint, the shackle pushed aside and tied flat against the wall, a faint nonverbal scratch mark on the stone, a second small candle near the original light source, slightly warmer corridor light outside the open door, still clearly a prison cell, but maintained as a temporary foothold rather than a home, warm or cold localized lighting matching the source image, controlled shadows, readable dark areas, quiet oppressive atmosphere, no characters, no loose collectible props, no random clutter, no unrequested objects on tables, shelves, beds or floors
```

### Negative

```text
alternate composition, changed layout, changed camera angle, changed perspective, changed framing, missing architecture, extra room, extra doorway, extra furniture, duplicated fixtures, shifted furniture, warped walls, distorted arches, bent shelves, malformed doors, malformed furniture, inconsistent scale, photorealistic, hyperrealistic, 3d render, CGI, PBR, unreal engine screenshot, ray tracing, glossy stone, procedural stone texture, repetitive pebble texture, overly embossed surfaces, excessive microdetail, oversharpened, high frequency texture noise, crushed blacks, excessive bloom, excessive orange glow, characters, people, monsters, enemies, animals, weapons, armor, text, readable letters, watermark, logo, ui, blurry, low quality, cozy cottage, luxurious room, excessive decorations, supplies piled everywhere, books, food, weapons, banners, readable writing, bloom crystal unless intentionally added later
```
