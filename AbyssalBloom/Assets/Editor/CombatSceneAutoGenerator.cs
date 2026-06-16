using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

// ════════════════════════════════════════════════════════════════════════════
// CombatSceneAutoGenerator — Editor-only utility
// Place in Assets/Editor/
//
// Menu: AbyssalBloom > Generate Combat Test Scene
//
// Layout (1920×1080 reference, ScaleWithScreenSize):
//   Top-left:       Enemy stat cards (horizontal row, up to 6)
//   Right side:     Active heroine portrait placeholder (full height)
//   Center-bottom:  Battle log (3-4 lines, collapsible)
//   Bottom bar:     Party strip | Inventory (6 slots) | Action buttons (5+3)
//   Floating:       Skill panel / Special panel (hidden, toggle on click)
//   Hidden:         Grapple panel (center, shown during grapple)
// ════════════════════════════════════════════════════════════════════════════

#if UNITY_EDITOR

public class CombatSceneAutoGenerator
{
    // ── Layout constants (1920×1080) ────────────────────────────────────────

    const float BottomBarH = 140f;

    // Party strip
    const float PartyStripW     = 535f;
    const float HeroCardW       = 172f;
    const float HeroCardGap     = 4f;
    const float PortraitW_Small = 50f;
    const float PortraitH_Small = 66f;

    // Inventory strip
    const float InvStripW  = 380f;
    const float InvSlotW   = 54f;
    const float InvIconSize = 44f;
    const float InvSlotGap = 6f;

    // Action buttons
    const float ActBtnW   = 100f;
    const float ActBtnH   = 52f;
    const float ActBtnGap = 6f;

    // Enemy cards
    const float ECardW      = 195f;
    const float ECardH      = 80f;
    const float ECardGap    = 8f;
    const float ECardMargin = 12f;

    // Active heroine portrait
    const float ActivePortraitW = 380f;

    // Battle log
    const float LogW       = 500f;
    const float LogBodyH   = 66f;
    const float LogHeaderH = 22f;

    // Skill / Special popup panels
    const float MenuW     = 190f;
    const float MenuRowH  = 22f;
    const int   MenuSlots = 6;

    // Grapple panel
    const float GrappleW = 320f;
    const float GrappleH = 50f;

    // Stat bars
    const float SBarH      = 5f;
    const float SBarRowH   = 16f;
    const float SBarLabelW = 22f;
    const float SBarValueW = 42f;

    // ── Colors ──────────────────────────────────────────────────────────────

    static readonly Color ColPanelBg     = new Color(0.02f, 0.01f, 0.05f, 0.92f);
    static readonly Color ColCardBg      = new Color(0.04f, 0.02f, 0.08f, 0.88f);
    static readonly Color ColEnemyCardBg = new Color(0.05f, 0.03f, 0.10f, 0.85f);
    static readonly Color ColBarTrack    = new Color(1f, 1f, 1f, 0.07f);
    static readonly Color ColHP          = new Color(0.61f, 0.14f, 0.21f);
    static readonly Color ColMP          = new Color(0.10f, 0.37f, 0.61f);
    static readonly Color ColRES         = new Color(0.15f, 0.68f, 0.37f);
    static readonly Color ColCOR         = new Color(0.42f, 0.18f, 0.55f);
    static readonly Color ColTextBright  = new Color(0.85f, 0.78f, 1f, 0.9f);
    static readonly Color ColTextDim     = new Color(1f, 1f, 1f, 0.35f);
    static readonly Color ColBtnBg       = new Color(0.08f, 0.05f, 0.16f, 0.9f);
    static readonly Color ColBtnDanger   = new Color(0.25f, 0.08f, 0.08f, 0.9f);
    static readonly Color ColActiveGlow  = new Color(0.36f, 0.20f, 0.63f, 0.20f);
    static readonly Color ColPortrait    = new Color(0.12f, 0.06f, 0.20f, 0.6f);
    static readonly Color ColLogBg       = new Color(0.02f, 0.01f, 0.06f, 0.85f);
    static readonly Color ColMenuBg      = new Color(0.03f, 0.02f, 0.08f, 0.92f);
    static readonly Color ColDivider     = new Color(0.47f, 0.31f, 0.71f, 0.25f);

    // ════════════════════════════════════════════════════════════════════════
    // Entry point
    // ════════════════════════════════════════════════════════════════════════

    [MenuItem("AbyssalBloom/Generate Combat Test Scene")]
    public static void GenerateCombatTestScene()
    {
        Canvas canvas = FindOrCreateCanvas();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("Error", "Could not create Canvas.", "OK");
            return;
        }

        EnsureEventSystem();

        if (EditorUtility.DisplayDialog("Clear Existing?",
            "Clear all child GameObjects in Canvas?", "Yes", "No"))
        {
            for (int i = canvas.transform.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(canvas.transform.GetChild(i).gameObject);
        }

        GenerateEnemyCards(canvas);
        GenerateActivePortrait(canvas);
        GenerateBattleLog(canvas);
        GenerateGrapplePanel(canvas);
        GenerateBottomBar(canvas);
        GenerateSubMenu(canvas, "SkillMenuRoot", "Skills", "SkillButton", -10f);
        GenerateSubMenu(canvas, "SpecialMenuRoot", "Specials", "SpecialButton",
            -(10f + MenuW + 8f));

        Transform mgrs = FindOrCreateManagers();
        var cm = mgrs.GetComponent<CombatManager>();
        if (cm == null) cm = mgrs.gameObject.AddComponent<CombatManager>();

        var ui = canvas.GetComponent<CombatUI>();
        if (ui == null) ui = canvas.gameObject.AddComponent<CombatUI>();

        WireCombatUI(canvas, ui, cm);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorUtility.DisplayDialog("Done",
            "Combat scene generated!\n\n" +
            "Manual steps:\n" +
            "1. Add EncounterBuilder to _Managers\n" +
            "2. Wire CombatManager -> EncounterBuilder\n" +
            "3. Assign CharacterDataSOs and EnemyDataSOs\n" +
            "4. Press Play",
            "OK");
    }

    // ════════════════════════════════════════════════════════════════════════
    // Infrastructure
    // ════════════════════════════════════════════════════════════════════════

    static Canvas FindOrCreateCanvas()
    {
        Canvas c = Object.FindObjectOfType<Canvas>();
        if (c != null) { ConfigureScaler(c.gameObject); return c; }

        var go = new GameObject("Canvas",
            typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        c = go.GetComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        ConfigureScaler(go);
        return c;
    }

    static void ConfigureScaler(GameObject go)
    {
        var s = go.GetComponent<CanvasScaler>();
        if (s == null) return;
        s.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        s.referenceResolution = new Vector2(1920f, 1080f);
        s.matchWidthOrHeight = 0.5f;
    }

    static void EnsureEventSystem()
    {
        if (Object.FindObjectOfType<EventSystem>() != null) return;
        var go = new GameObject("EventSystem",
            typeof(EventSystem), typeof(StandaloneInputModule));
        Undo.RegisterCreatedObjectUndo(go, "Create EventSystem");
    }

    static Transform FindOrCreateManagers()
    {
        var go = GameObject.Find("_Managers");
        return go != null ? go.transform : new GameObject("_Managers").transform;
    }

    // ════════════════════════════════════════════════════════════════════════
    // RectTransform helpers
    // ════════════════════════════════════════════════════════════════════════

    static void StretchFull(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    static void AnchorFixed(RectTransform rt, Vector2 anchorPivot,
        float w, float h, Vector2 offset)
    {
        rt.anchorMin = anchorPivot;
        rt.anchorMax = anchorPivot;
        rt.pivot = anchorPivot;
        rt.sizeDelta = new Vector2(w, h);
        rt.anchoredPosition = offset;
    }

    static void PlaceInside(RectTransform rt,
        float x, float yDown, float w, float h)
    {
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(x, -yDown);
        rt.sizeDelta = new Vector2(w, h);
    }

    // ════════════════════════════════════════════════════════════════════════
    // Element creation helpers
    // ════════════════════════════════════════════════════════════════════════

    static Font _font;
    static Font GetFont()
    {
        if (_font == null)
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return _font;
    }

    static GameObject MakeText(GameObject parent, string name, string text,
        int fontSize = 11, Color? color = null)
    {
        var go = new GameObject(name, typeof(Text));
        go.transform.SetParent(parent.transform, false);
        var t = go.GetComponent<Text>();
        t.text = text;
        t.font = GetFont();
        t.fontSize = fontSize;
        t.color = color ?? Color.white;
        t.horizontalOverflow = HorizontalWrapMode.Wrap;
        t.verticalOverflow = VerticalWrapMode.Truncate;
        t.raycastTarget = false;
        return go;
    }

    static GameObject MakePanel(Transform parent, string name, Color bg)
    {
        var go = new GameObject(name, typeof(Image));
        go.transform.SetParent(parent, false);
        go.GetComponent<Image>().color = bg;
        return go;
    }

    /// <summary>
    /// Track + Fill bar. Returns the Fill Image (wired to CombatUI via fillAmount).
    /// </summary>
    static Image MakeBar(GameObject parent, string baseName, Color fillColor,
        float x, float yDown, float w, float h)
    {
        var track = MakePanel(parent.transform, baseName + "_Track", ColBarTrack);
        PlaceInside(track.GetComponent<RectTransform>(), x, yDown, w, h);

        var fill = MakePanel(track.transform, baseName + "Bar", fillColor);
        var img = fill.GetComponent<Image>();
        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Horizontal;
        img.fillAmount = 1f;
        StretchFull(fill.GetComponent<RectTransform>());

        return img;
    }

    /// <summary>
    /// Full stat row: label + bar track/fill + numeric value text.
    /// Returns (barFillImage, valueText) for wiring.
    /// </summary>
    static (Image bar, Text val) MakeStatRow(GameObject parent,
        string stat, Color barColor, float x, float yDown, float barW)
    {
        var lbl = MakeText(parent, stat + "Label", stat, 9, ColTextDim);
        PlaceInside(lbl.GetComponent<RectTransform>(), x, yDown, SBarLabelW, SBarRowH);

        float bx = x + SBarLabelW + 2f;
        float by = yDown + (SBarRowH - SBarH) * 0.5f;
        var barImg = MakeBar(parent, stat, barColor, bx, by, barW, SBarH);

        float vx = bx + barW + 2f;
        var val = MakeText(parent, stat + "Text", "0/0", 9, ColTextDim);
        PlaceInside(val.GetComponent<RectTransform>(), vx, yDown, SBarValueW, SBarRowH);
        val.GetComponent<Text>().alignment = TextAnchor.MiddleRight;

        return (barImg, val.GetComponent<Text>());
    }

    // ════════════════════════════════════════════════════════════════════════
    // Enemy Cards — top-left horizontal row
    // ════════════════════════════════════════════════════════════════════════

    static void GenerateEnemyCards(Canvas canvas)
    {
        for (int i = 0; i < 6; i++)
        {
            var card = MakePanel(canvas.transform, $"EnemyRow_{i}", ColEnemyCardBg);
            var rt = card.GetComponent<RectTransform>();

            float xOff = ECardMargin + i * (ECardW + ECardGap);
            AnchorFixed(rt, new Vector2(0f, 1f), ECardW, ECardH,
                new Vector2(xOff, -ECardMargin));

            var nameGO = MakeText(card, "EnemyNameText", "Enemy", 10, ColTextBright);
            PlaceInside(nameGO.GetComponent<RectTransform>(), 0f, 3f, ECardW, 16f);
            nameGO.GetComponent<Text>().alignment = TextAnchor.UpperCenter;

            float rowX = 6f;
            float barW = ECardW - SBarLabelW - SBarValueW - 18f;
            MakeStatRow(card, "HP",  ColHP,  rowX, 22f, barW);
            MakeStatRow(card, "MP",  ColMP,  rowX, 38f, barW);
            MakeStatRow(card, "COR", ColCOR, rowX, 54f, barW);

            if (i > 0) card.SetActive(false);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // Active Heroine Portrait — right side, above bottom bar
    // ════════════════════════════════════════════════════════════════════════

    static void GenerateActivePortrait(Canvas canvas)
    {
        var panel = MakePanel(canvas.transform, "ActiveHeroinePortrait", ColPortrait);
        var rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(1f, 0f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(1f, 0.5f);
        rt.offsetMin = new Vector2(-ActivePortraitW, BottomBarH);
        rt.offsetMax = Vector2.zero;

        var label = MakeText(panel, "PortraitLabel",
            "ACTIVE HEROINE\n\n(placeholder for CharacterSpriteSO)",
            14, new Color(0.6f, 0.5f, 1f, 0.3f));
        StretchFull(label.GetComponent<RectTransform>());
        label.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
    }

    // ════════════════════════════════════════════════════════════════════════
    // Battle Log — centered above bottom bar, collapsible
    // ════════════════════════════════════════════════════════════════════════

    static void GenerateBattleLog(Canvas canvas)
    {
        float totalH = LogHeaderH + LogBodyH;
        var root = new GameObject("BattleLogRoot", typeof(RectTransform));
        root.transform.SetParent(canvas.transform, false);
        var rootRT = root.GetComponent<RectTransform>();
        AnchorFixed(rootRT, new Vector2(0.5f, 0f), LogW, totalH,
            new Vector2(0f, BottomBarH + 6f));
        rootRT.pivot = new Vector2(0.5f, 0f);

        // ── Header bar ──
        var header = MakePanel(root.transform, "LogHeader", ColLogBg);
        PlaceInside(header.GetComponent<RectTransform>(), 0f, 0f, LogW, LogHeaderH);

        var roundGO = MakeText(header, "RoundText", "Round 1", 10, ColTextBright);
        PlaceInside(roundGO.GetComponent<RectTransform>(), 8f, 2f, 100f, 18f);

        var title = MakeText(header, "LogTitle", "Battle Log", 9, ColTextDim);
        var titleRT = title.GetComponent<RectTransform>();
        titleRT.anchorMin = new Vector2(0.5f, 0f);
        titleRT.anchorMax = new Vector2(0.5f, 1f);
        titleRT.pivot = new Vector2(0.5f, 0.5f);
        titleRT.sizeDelta = new Vector2(100f, 0f);
        title.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        // Collapse/expand button
        var toggleGO = new GameObject("LogToggleButton", typeof(Button), typeof(Image));
        toggleGO.transform.SetParent(header.transform, false);
        toggleGO.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.05f);
        var toggleRT = toggleGO.GetComponent<RectTransform>();
        toggleRT.anchorMin = new Vector2(1f, 0f);
        toggleRT.anchorMax = new Vector2(1f, 1f);
        toggleRT.pivot = new Vector2(1f, 0.5f);
        toggleRT.sizeDelta = new Vector2(50f, 0f);
        toggleRT.anchoredPosition = Vector2.zero;
        var toggleLabel = MakeText(toggleGO, "Text", "▼", 10, ColTextDim);
        StretchFull(toggleLabel.GetComponent<RectTransform>());
        toggleLabel.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        // ── Log body (collapsible) ──
        var body = MakePanel(root.transform, "LogBodyPanel", ColLogBg);
        PlaceInside(body.GetComponent<RectTransform>(), 0f, LogHeaderH, LogW, LogBodyH);

        // ScrollRect
        var scrollGO = new GameObject("LogScrollRect", typeof(ScrollRect), typeof(Image));
        scrollGO.transform.SetParent(body.transform, false);
        scrollGO.GetComponent<Image>().color = Color.clear;
        StretchFull(scrollGO.GetComponent<RectTransform>());

        // Viewport
        var viewport = new GameObject("Viewport", typeof(Image), typeof(Mask));
        viewport.transform.SetParent(scrollGO.transform, false);
        viewport.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.01f);
        viewport.GetComponent<Mask>().showMaskGraphic = false;
        StretchFull(viewport.GetComponent<RectTransform>());

        // Log text content
        var logTextGO = MakeText(viewport, "LogText", "", 11,
            new Color(0.78f, 0.72f, 0.86f, 0.7f));
        var logTextRT = logTextGO.GetComponent<RectTransform>();
        logTextRT.anchorMin = new Vector2(0f, 1f);
        logTextRT.anchorMax = new Vector2(1f, 1f);
        logTextRT.pivot = new Vector2(0f, 1f);
        logTextRT.offsetMin = new Vector2(6f, 0f);
        logTextRT.offsetMax = new Vector2(-6f, 0f);
        logTextRT.sizeDelta = new Vector2(0f, 0f);
        logTextGO.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;

        var fitter = logTextGO.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var sr = scrollGO.GetComponent<ScrollRect>();
        sr.content = logTextRT;
        sr.viewport = viewport.GetComponent<RectTransform>();
        sr.horizontal = false;
        sr.vertical = true;
        sr.movementType = ScrollRect.MovementType.Clamped;

        // Scrollbar
        float sbW = 10f;
        var sbGO = new GameObject("Scrollbar", typeof(Scrollbar), typeof(Image));
        sbGO.transform.SetParent(body.transform, false);
        sbGO.GetComponent<Image>().color = new Color(0.15f, 0.12f, 0.2f);
        var sbRT = sbGO.GetComponent<RectTransform>();
        sbRT.anchorMin = new Vector2(1f, 0f);
        sbRT.anchorMax = new Vector2(1f, 1f);
        sbRT.pivot = new Vector2(1f, 0.5f);
        sbRT.sizeDelta = new Vector2(sbW, 0f);
        sbRT.anchoredPosition = Vector2.zero;

        var slidingArea = new GameObject("Sliding Area", typeof(RectTransform));
        slidingArea.transform.SetParent(sbGO.transform, false);
        StretchFull(slidingArea.GetComponent<RectTransform>());

        var handle = MakePanel(slidingArea.transform, "Handle",
            new Color(0.4f, 0.35f, 0.5f));
        StretchFull(handle.GetComponent<RectTransform>());

        var scrollbar = sbGO.GetComponent<Scrollbar>();
        scrollbar.handleRect = handle.GetComponent<RectTransform>();
        scrollbar.direction = Scrollbar.Direction.BottomToTop;
        scrollbar.targetGraphic = handle.GetComponent<Image>();

        sr.verticalScrollbar = scrollbar;
        sr.verticalScrollbarVisibility =
            ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
    }

    // ════════════════════════════════════════════════════════════════════════
    // Grapple Panel — center screen, hidden
    // ════════════════════════════════════════════════════════════════════════

    static void GenerateGrapplePanel(Canvas canvas)
    {
        var panel = MakePanel(canvas.transform, "GrapplePanel",
            new Color(0.35f, 0.28f, 0.06f, 0.92f));
        panel.SetActive(false);
        AnchorFixed(panel.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f), GrappleW, GrappleH, Vector2.zero);

        var text = MakeText(panel, "GrappleStageText", "GRAPPLE — Stage 0",
            16, Color.white);
        StretchFull(text.GetComponent<RectTransform>());
        text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
    }

    // ════════════════════════════════════════════════════════════════════════
    // Bottom Bar — full-width strip at screen bottom
    // ════════════════════════════════════════════════════════════════════════

    static void GenerateBottomBar(Canvas canvas)
    {
        var bar = MakePanel(canvas.transform, "BottomBar", ColPanelBg);
        var barRT = bar.GetComponent<RectTransform>();
        barRT.anchorMin = Vector2.zero;
        barRT.anchorMax = new Vector2(1f, 0f);
        barRT.pivot = new Vector2(0f, 0f);
        barRT.sizeDelta = new Vector2(0f, BottomBarH);
        barRT.anchoredPosition = Vector2.zero;

        GeneratePartyStrip(bar);
        GenerateInventoryStrip(bar);
        GenerateActionArea(bar);
    }

    // ── Party Strip ─────────────────────────────────────────────────────

    static void GeneratePartyStrip(GameObject bottomBar)
    {
        var strip = MakePanel(bottomBar.transform, "PartyStrip", Color.clear);
        var srt = strip.GetComponent<RectTransform>();
        srt.anchorMin = new Vector2(0f, 0f);
        srt.anchorMax = new Vector2(0f, 1f);
        srt.pivot = new Vector2(0f, 0.5f);
        srt.sizeDelta = new Vector2(PartyStripW, 0f);
        srt.anchoredPosition = Vector2.zero;

        // Right-edge divider
        var div = MakePanel(strip.transform, "Divider", ColDivider);
        var drt = div.GetComponent<RectTransform>();
        drt.anchorMin = new Vector2(1f, 0f);
        drt.anchorMax = new Vector2(1f, 1f);
        drt.pivot = new Vector2(1f, 0.5f);
        drt.sizeDelta = new Vector2(1f, 0f);

        for (int i = 0; i < 3; i++)
            CreateHeroineCard(strip, i);
    }

    static void CreateHeroineCard(GameObject strip, int index)
    {
        float cardH = BottomBarH - 8f;
        float xPos = 6f + index * (HeroCardW + HeroCardGap);

        var card = MakePanel(strip.transform, $"HeroinePanel_{index}", ColCardBg);
        var crt = card.GetComponent<RectTransform>();
        crt.anchorMin = new Vector2(0f, 0.5f);
        crt.anchorMax = new Vector2(0f, 0.5f);
        crt.pivot = new Vector2(0f, 0.5f);
        crt.sizeDelta = new Vector2(HeroCardW, cardH);
        crt.anchoredPosition = new Vector2(xPos, 0f);

        // Active indicator overlay
        var indicator = MakePanel(card.transform, "ActiveIndicator", ColActiveGlow);
        StretchFull(indicator.GetComponent<RectTransform>());

        // Portrait placeholder
        var portrait = MakePanel(card.transform, "Portrait", ColPortrait);
        PlaceInside(portrait.GetComponent<RectTransform>(),
            4f, 4f, PortraitW_Small, PortraitH_Small);

        // Stats column layout
        float sx = PortraitW_Small + 10f;
        float sw = HeroCardW - sx - 4f;
        float barW = sw - SBarLabelW - SBarValueW - 6f;

        // Name
        var nameGO = MakeText(card, "NameText", "", 10, ColTextBright);
        PlaceInside(nameGO.GetComponent<RectTransform>(), sx, 4f, sw, 14f);
        nameGO.GetComponent<Text>().fontStyle = FontStyle.Bold;

        // Stat rows: HP, MP, RES, COR
        float y0 = 22f;
        MakeStatRow(card, "HP",  ColHP,  sx, y0,                barW);
        MakeStatRow(card, "MP",  ColMP,  sx, y0 + SBarRowH,     barW);
        MakeStatRow(card, "RES", ColRES, sx, y0 + SBarRowH * 2, barW);
        MakeStatRow(card, "COR", ColCOR, sx, y0 + SBarRowH * 3, barW);
    }

    // ── Inventory Strip ─────────────────────────────────────────────────

    static void GenerateInventoryStrip(GameObject bottomBar)
    {
        var strip = MakePanel(bottomBar.transform, "InventoryStrip", Color.clear);
        var srt = strip.GetComponent<RectTransform>();
        srt.anchorMin = new Vector2(0f, 0f);
        srt.anchorMax = new Vector2(0f, 1f);
        srt.pivot = new Vector2(0f, 0.5f);
        srt.sizeDelta = new Vector2(InvStripW, 0f);
        srt.anchoredPosition = new Vector2(PartyStripW, 0f);

        // Right-edge divider
        var div = MakePanel(strip.transform, "Divider", ColDivider);
        var drt = div.GetComponent<RectTransform>();
        drt.anchorMin = new Vector2(1f, 0f);
        drt.anchorMax = new Vector2(1f, 1f);
        drt.pivot = new Vector2(1f, 0.5f);
        drt.sizeDelta = new Vector2(1f, 0f);

        var label = MakeText(strip, "InvLabel", "Inventory", 9, ColTextDim);
        PlaceInside(label.GetComponent<RectTransform>(), 8f, 4f, 80f, 16f);

        float slotY = 24f;
        float startX = 8f;
        for (int i = 0; i < 6; i++)
        {
            float slotX = startX + i * (InvSlotW + InvSlotGap);
            var slot = new GameObject($"InventorySlot_{i}",
                typeof(Button), typeof(Image));
            slot.transform.SetParent(strip.transform, false);
            slot.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.04f);
            slot.GetComponent<Button>().interactable = false;
            PlaceInside(slot.GetComponent<RectTransform>(),
                slotX, slotY, InvSlotW, InvIconSize + 28f);

            var icon = MakePanel(slot.transform, "ItemIcon",
                new Color(1f, 1f, 1f, 0.03f));
            PlaceInside(icon.GetComponent<RectTransform>(),
                (InvSlotW - InvIconSize) * 0.5f, 2f, InvIconSize, InvIconSize);

            var nameGO = MakeText(slot, "ItemName", "", 8,
                new Color(0.78f, 0.68f, 0.40f, 0.7f));
            PlaceInside(nameGO.GetComponent<RectTransform>(),
                0f, InvIconSize + 4f, InvSlotW, 12f);
            nameGO.GetComponent<Text>().alignment = TextAnchor.UpperCenter;

            var qtyGO = MakeText(slot, "ItemQty", "", 8,
                new Color(0.78f, 0.68f, 0.40f, 0.5f));
            PlaceInside(qtyGO.GetComponent<RectTransform>(),
                0f, InvIconSize + 16f, InvSlotW, 12f);
            qtyGO.GetComponent<Text>().alignment = TextAnchor.UpperCenter;
        }
    }

    // ── Action Area (5 main + 3 pool slots) ─────────────────────────────

    static void GenerateActionArea(GameObject bottomBar)
    {
        var area = MakePanel(bottomBar.transform, "ActionArea", Color.clear);
        var art = area.GetComponent<RectTransform>();
        art.anchorMin = new Vector2(0f, 0f);
        art.anchorMax = new Vector2(1f, 1f);
        art.offsetMin = new Vector2(PartyStripW + InvStripW, 0f);
        art.offsetMax = Vector2.zero;

        var group = new GameObject("ActionButtonGroup", typeof(HorizontalLayoutGroup));
        group.transform.SetParent(area.transform, false);
        var grt = group.GetComponent<RectTransform>();
        grt.anchorMin = new Vector2(0.5f, 0.5f);
        grt.anchorMax = new Vector2(0.5f, 0.5f);
        grt.pivot = new Vector2(0.5f, 0.5f);
        float totalW = 8 * ActBtnW + 7 * ActBtnGap;
        grt.sizeDelta = new Vector2(totalW, ActBtnH);

        var hlg = group.GetComponent<HorizontalLayoutGroup>();
        hlg.spacing = ActBtnGap;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;
        hlg.childControlWidth = false;
        hlg.childControlHeight = true;
        hlg.childAlignment = TextAnchor.MiddleCenter;

        string[] labels = {
            "Attack", "Skill", "Special", "Defend", "Run",
            "", "", ""
        };

        for (int i = 0; i < 8; i++)
        {
            bool isDanger = (i == 4);
            var btn = new GameObject($"ActionButton_{i}",
                typeof(Button), typeof(Image));
            btn.transform.SetParent(group.transform, false);
            btn.GetComponent<Image>().color = isDanger ? ColBtnDanger : ColBtnBg;
            btn.GetComponent<Button>().interactable = false;
            btn.GetComponent<RectTransform>().sizeDelta =
                new Vector2(ActBtnW, ActBtnH);

            var le = btn.AddComponent<LayoutElement>();
            le.preferredWidth = ActBtnW;

            var lbl = MakeText(btn, "Text", labels[i], 11, ColTextBright);
            StretchFull(lbl.GetComponent<RectTransform>());
            lbl.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            if (i >= 5) btn.SetActive(false);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // Skill / Special Sub-Menu — floating above action area, hidden
    // ════════════════════════════════════════════════════════════════════════

    static void GenerateSubMenu(Canvas canvas, string rootName, string title,
        string btnPrefix, float xFromRight)
    {
        float panelH = MenuSlots * MenuRowH + 28f;

        var panel = MakePanel(canvas.transform, rootName, ColMenuBg);
        panel.SetActive(false);
        AnchorFixed(panel.GetComponent<RectTransform>(),
            new Vector2(1f, 0f), MenuW, panelH,
            new Vector2(xFromRight, BottomBarH + 4f));

        var titleGO = MakeText(panel, "MenuTitle", title, 10, ColTextDim);
        PlaceInside(titleGO.GetComponent<RectTransform>(), 8f, 4f, MenuW - 16f, 18f);

        var div = MakePanel(panel.transform, "TitleDiv", ColDivider);
        PlaceInside(div.GetComponent<RectTransform>(), 6f, 22f, MenuW - 12f, 1f);

        for (int i = 0; i < MenuSlots; i++)
        {
            float rowY = 26f + i * MenuRowH;
            var btn = new GameObject($"{btnPrefix}_{i}",
                typeof(Button), typeof(Image));
            btn.transform.SetParent(panel.transform, false);
            btn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.03f);
            btn.GetComponent<Button>().interactable = false;
            PlaceInside(btn.GetComponent<RectTransform>(),
                4f, rowY, MenuW - 8f, MenuRowH - 2f);

            var lbl = MakeText(btn, "Text", "", 10, ColTextBright);
            StretchFull(lbl.GetComponent<RectTransform>());
            lbl.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            var lrt = lbl.GetComponent<RectTransform>();
            lrt.offsetMin = new Vector2(6f, 0f);
            lrt.offsetMax = new Vector2(-6f, 0f);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // Auto-wiring → CombatUI fields
    // ════════════════════════════════════════════════════════════════════════

    static void WireCombatUI(Canvas canvas, CombatUI ui, CombatManager cm)
    {
        ui.combatManager = cm;
        var ct = canvas.transform;

        // ── Heroine panels ──
        var strip = ct.Find("BottomBar/PartyStrip");
        for (int i = 0; i < 3; i++)
        {
            var card = strip?.Find($"HeroinePanel_{i}");
            if (card == null) continue;

            ui.heroineNameText[i]    = card.Find("NameText")?.GetComponent<Text>();
            ui.heroineHPText[i]      = card.Find("HPText")?.GetComponent<Text>();
            ui.heroineMPText[i]      = card.Find("MPText")?.GetComponent<Text>();
            ui.heroineResolveText[i] = card.Find("RESText")?.GetComponent<Text>();
            ui.heroineCorruptText[i] = card.Find("CORText")?.GetComponent<Text>();

            ui.heroineHPBar[i]      = card.Find("HP_Track/HPBar")?.GetComponent<Image>();
            ui.heroineResolveBar[i] = card.Find("RES_Track/RESBar")?.GetComponent<Image>();
            ui.heroineMPBar[i]      = card.Find("MP_Track/MPBar")?.GetComponent<Image>();
            ui.heroineCorruptBar[i] = card.Find("COR_Track/CORBar")?.GetComponent<Image>();
        }

        if (strip != null)
            ui.activeIndicator = strip.Find("HeroinePanel_0/ActiveIndicator")
                ?.GetComponent<Image>();

        ui.activeHeroinePortrait = ct.Find("ActiveHeroinePortrait")
            ?.GetComponent<Image>();

        // ── Enemy cards ──
        for (int i = 0; i < 6; i++)
        {
            var card = ct.Find($"EnemyRow_{i}");
            if (card == null) continue;

            ui.enemyRowRoot[i]  = card.gameObject;
            ui.enemyNameText[i] = card.Find("EnemyNameText")?.GetComponent<Text>();
            ui.enemyHPText[i]   = card.Find("HPText")?.GetComponent<Text>();
            ui.enemyHPBar[i]    = card.Find("HP_Track/HPBar")?.GetComponent<Image>();
            ui.enemyMPText[i]   = card.Find("MPText")?.GetComponent<Text>();
            ui.enemyMPBar[i]    = card.Find("MP_Track/MPBar")?.GetComponent<Image>();
            ui.enemyCORText[i]  = card.Find("CORText")?.GetComponent<Text>();
            ui.enemyCORBar[i]   = card.Find("COR_Track/CORBar")?.GetComponent<Image>();
        }

        // ── Grapple ──
        ui.grapplePanel     = ct.Find("GrapplePanel")?.gameObject;
        ui.grappleStageText = ct.Find("GrapplePanel/GrappleStageText")
            ?.GetComponent<Text>();

        // ── Round text (inside log header) ──
        ui.roundText = ct.Find("BattleLogRoot/LogHeader/RoundText")
            ?.GetComponent<Text>();

        // ── Action buttons (pool of 8) ──
        var btnGroup = ct.Find("BottomBar/ActionArea/ActionButtonGroup");
        if (btnGroup != null)
        {
            for (int i = 0; i < 8; i++)
            {
                var btn = btnGroup.Find($"ActionButton_{i}");
                if (btn == null) continue;
                ui.actionButtons[i]      = btn.GetComponent<Button>();
                ui.actionButtonLabels[i] = btn.Find("Text")?.GetComponent<Text>();
            }
        }

        // ── Skill sub-menu ──
        ui.skillMenuRoot = ct.Find("SkillMenuRoot")?.gameObject;
        if (ui.skillMenuRoot != null)
        {
            for (int i = 0; i < 6; i++)
            {
                var btn = ui.skillMenuRoot.transform.Find($"SkillButton_{i}");
                if (btn == null) continue;
                ui.skillButtons[i]      = btn.GetComponent<Button>();
                ui.skillButtonLabels[i] = btn.Find("Text")?.GetComponent<Text>();
            }
        }

        // ── Special sub-menu ──
        ui.specialMenuRoot = ct.Find("SpecialMenuRoot")?.gameObject;
        if (ui.specialMenuRoot != null)
        {
            for (int i = 0; i < 6; i++)
            {
                var btn = ui.specialMenuRoot.transform.Find($"SpecialButton_{i}");
                if (btn == null) continue;
                ui.specialButtons[i]      = btn.GetComponent<Button>();
                ui.specialButtonLabels[i] = btn.Find("Text")?.GetComponent<Text>();
            }
        }

        // ── Combat log ──
        ui.logScrollRect = ct.Find("BattleLogRoot/LogBodyPanel/LogScrollRect")
            ?.GetComponent<ScrollRect>();
        ui.logText = ct.Find(
            "BattleLogRoot/LogBodyPanel/LogScrollRect/Viewport/LogText")
            ?.GetComponent<Text>();
        ui.logToggleButton = ct.Find("BattleLogRoot/LogHeader/LogToggleButton")
            ?.GetComponent<Button>();
        ui.logBodyPanel = ct.Find("BattleLogRoot/LogBodyPanel")?.gameObject;

        // ── Inventory slots ──
        var inv = ct.Find("BottomBar/InventoryStrip");
        if (inv != null)
        {
            for (int i = 0; i < 6; i++)
            {
                var slot = inv.Find($"InventorySlot_{i}");
                if (slot == null) continue;
                ui.inventorySlots[i]     = slot.gameObject;
                ui.inventoryIcons[i]     = slot.Find("ItemIcon")?.GetComponent<Image>();
                ui.inventoryNameTexts[i] = slot.Find("ItemName")?.GetComponent<Text>();
                ui.inventoryQtyTexts[i]  = slot.Find("ItemQty")?.GetComponent<Text>();
            }
        }

        EditorUtility.SetDirty(ui);
    }
}

#endif
