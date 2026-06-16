#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// ════════════════════════════════════════════════════════════════════════════
// GameSceneBootstrapper
// ════════════════════════════════════════════════════════════════════════════
// Menu: AbyssalBloom → Build Game Scene
//
// Creates a fully-wired game scene with:
//   - All manager GameObjects (_Managers)
//   - All UI panels (Canvas children)
//   - All cross-references wired in Inspector
//   - Layer 1 profile assigned to RoomManager
//   - Starter heroines and abilities assigned to GameBootstrap
//
// Run ONCE on an empty scene. Safe to re-run — checks for existing objects.
// After running, verify in Inspector and press Play.
// ════════════════════════════════════════════════════════════════════════════

public static class GameSceneBootstrapper
{
    // ── Asset paths ────────────────────────────────────────────────────────

    private const string SO_ROOT       = "Assets/ScriptableObjects";
    private const string HEROINES_PATH = SO_ROOT + "/Heroines";
    private const string ABILITIES_PATH= SO_ROOT + "/Abilities/Heroines";
    private const string LAYERS_PATH   = SO_ROOT + "/LayerProfiles";
    private const string FONT_NAME     = "LegacyRuntime";

    // ── Entry point ────────────────────────────────────────────────────────

    [MenuItem("AbyssalBloom/Build Game Scene")]
    public static void BuildGameScene()
    {
        // Create a new scene or work in the current one
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            if (!EditorUtility.DisplayDialog("Build Game Scene",
                "Current scene has unsaved changes. Continue and lose changes?",
                "Continue", "Cancel"))
                return;
        }

        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // ── Camera ─────────────────────────────────────────────────────────
        var cam = GameObject.Find("Main Camera") ?? new GameObject("Main Camera");
        if (!cam.GetComponent<Camera>()) cam.AddComponent<Camera>();
        cam.tag = "MainCamera";

        // ── Managers ───────────────────────────────────────────────────────
        var managers = GetOrCreate("_Managers");
        managers.isStatic = true;

        var flagManager      = AddComponent<FlagManager>(managers);
        var runStateManager  = AddComponent<RunStateManager>(managers);
        var refugeManager    = AddComponent<RefugeManager>(managers);
        var itemManager      = AddComponent<ItemManager>(managers);
        var roomManager      = AddComponent<RoomManager>(managers);
        var combatManager    = AddComponent<CombatManager>(managers);
        var encounterBuilder = AddComponent<EncounterBuilder>(managers);
        var combatBridge     = AddComponent<CombatRoomBridge>(managers);
        var saveTrigger      = AddComponent<SaveTrigger>(managers);
        var gameBootstrap    = AddComponent<GameBootstrap>(managers);

        // ── Canvas ─────────────────────────────────────────────────────────
        var canvasGO = GetOrCreate("Canvas");
        var canvas = canvasGO.GetComponent<Canvas>() ?? canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;
        if (!canvasGO.GetComponent<CanvasScaler>())
        {
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
        }
        if (!canvasGO.GetComponent<GraphicRaycaster>())
            canvasGO.AddComponent<GraphicRaycaster>();

        // ── EventSystem ────────────────────────────────────────────────────
        var eventSystemGO = GetOrCreate("EventSystem");
        if (!eventSystemGO.GetComponent<UnityEngine.EventSystems.EventSystem>())
            eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
        if (!eventSystemGO.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>())
            eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // ── UI Panels ──────────────────────────────────────────────────────
        var refugePanel  = CreatePanel(canvasGO, "RefugePanel",  Color.black);
        var mapPanel     = CreatePanel(canvasGO, "MapPanel",     new Color(0.05f, 0.05f, 0.1f));
        var combatPanel  = CreatePanel(canvasGO, "CombatPanel",  Color.black);
        var eventPanel   = CreatePanel(canvasGO, "EventPanel",   new Color(0.1f, 0.05f, 0.05f));
        var hotspotPanel = CreatePanel(canvasGO, "HotspotPanel", Color.clear);

        // Start with refuge visible, others hidden
        refugePanel.SetActive(true);
        mapPanel.SetActive(false);
        combatPanel.SetActive(false);
        eventPanel.SetActive(false);
        hotspotPanel.SetActive(false);

        // ── Refuge Panel contents ──────────────────────────────────────────
        var refugeUI  = refugePanel.AddComponent<RefugeUI>();
        var bloomText = CreateText(refugePanel, "BloomText", "Bloom: 0",
            new Vector2(0, 0.92f), new Vector2(1, 1f));

        var startRunBtn = CreateButton(refugePanel, "StartRunButton", "▶ Start Run",
            new Vector2(0.35f, 0.02f), new Vector2(0.65f, 0.1f));

        var fullPartyBtn = CreateButton(refugePanel, "FullPartyButton",
            "Recover All (20 Bloom)",
            new Vector2(0.35f, 0.12f), new Vector2(0.65f, 0.18f));

        // Tab buttons
        string[] tabNames  = { "Recovery", "Party", "Upgrades", "Gallery", "Knowledge" };
        Button[] tabBtns   = new Button[5];
        for (int i = 0; i < 5; i++)
        {
            float x0 = 0.02f + i * 0.196f;
            tabBtns[i] = CreateButton(refugePanel, $"Tab{tabNames[i]}", tabNames[i],
                new Vector2(x0, 0.82f), new Vector2(x0 + 0.18f, 0.9f));
        }

        // Heroine rows — 3 heroines × compact row
        var recoveryRows = new RefugeUI.HeroineRecoveryRow[3];
        var partyRows    = new RefugeUI.HeroineStatsRow[3];
        var upgradeRows  = new RefugeUI.HeroineUpgradeRow[3];

        string[] heroineNames = { "Lysandra", "Mira Voss", "Seraphine" };
        for (int i = 0; i < 3; i++)
        {
            float y0 = 0.55f - i * 0.18f;
            float y1 = y0 + 0.16f;

            // Recovery row
            recoveryRows[i] = new RefugeUI.HeroineRecoveryRow
            {
                nameLabel          = CreateText(refugePanel, $"RecovName_{i}", heroineNames[i],
                                        new Vector2(0.02f, y0), new Vector2(0.2f, y1)),
                restoreHPButton    = CreateButton(refugePanel, $"RecovHP_{i}",   "HP (5)",  new Vector2(0.22f, y0), new Vector2(0.38f, y1)),
                restoreMPButton    = CreateButton(refugePanel, $"RecovMP_{i}",   "MP (3)",  new Vector2(0.40f, y0), new Vector2(0.56f, y1)),
                restoreResolveButton= CreateButton(refugePanel, $"RecovRes_{i}", "Res (4)", new Vector2(0.58f, y0), new Vector2(0.74f, y1)),
                reduceCorruptButton= CreateButton(refugePanel, $"RecovCor_{i}",  "Cor (6)", new Vector2(0.76f, y0), new Vector2(0.92f, y1)),
            };

            // Party stats row (same area — will be toggled by tabs)
            partyRows[i] = new RefugeUI.HeroineStatsRow
            {
                nameLabel      = CreateText(refugePanel, $"PartyName_{i}",    heroineNames[i], new Vector2(0.02f, y0), new Vector2(0.2f, y1)),
                hpLabel        = CreateText(refugePanel, $"PartyHP_{i}",      "HP: -/-",       new Vector2(0.22f, y0), new Vector2(0.4f, y1)),
                mpLabel        = CreateText(refugePanel, $"PartyMP_{i}",      "MP: -/-",       new Vector2(0.42f, y0), new Vector2(0.6f, y1)),
                resolveLabel   = CreateText(refugePanel, $"PartyResolve_{i}", "Res: -/-",      new Vector2(0.62f, y0), new Vector2(0.8f, y1)),
                corruptionLabel= CreateText(refugePanel, $"PartyCor_{i}",     "Cor: -",        new Vector2(0.82f, y0), new Vector2(1f,  y1)),
            };

            // Upgrade row
            upgradeRows[i] = new RefugeUI.HeroineUpgradeRow
            {
                nameLabel          = CreateText(refugePanel, $"UpgName_{i}",   heroineNames[i], new Vector2(0.02f, y0), new Vector2(0.15f, y1)),
                ability0Label      = CreateText(refugePanel, $"Upg0Label_{i}", "Ability 0",     new Vector2(0.17f, y0), new Vector2(0.32f, y1)),
                ability0Tier1Button= CreateButton(refugePanel, $"Upg0T1_{i}",  "T1 (15)",       new Vector2(0.34f, y0), new Vector2(0.45f, y1)),
                ability0Tier2aButton=CreateButton(refugePanel, $"Upg0T2a_{i}", "T2a (25)",      new Vector2(0.47f, y0), new Vector2(0.58f, y1)),
                ability0Tier2bButton=CreateButton(refugePanel, $"Upg0T2b_{i}", "T2b (25)",      new Vector2(0.60f, y0), new Vector2(0.71f, y1)),
                ability1Label      = CreateText(refugePanel, $"Upg1Label_{i}", "Ability 1",     new Vector2(0.73f, y0), new Vector2(0.85f, y1)),
                ability1Tier1Button= CreateButton(refugePanel, $"Upg1T1_{i}",  "T1 (15)",       new Vector2(0.87f, y0), new Vector2(0.95f, y1)),
                ability1Tier2aButton=CreateButton(refugePanel, $"Upg1T2a_{i}", "T2a",           new Vector2(0.87f, y0), new Vector2(0.91f, y1)),
                ability1Tier2bButton=CreateButton(refugePanel, $"Upg1T2b_{i}", "T2b",           new Vector2(0.92f, y0), new Vector2(0.96f, y1)),
            };
        }

        // ── Map Panel contents ─────────────────────────────────────────────
        var mapUI   = mapPanel.AddComponent<MapUI>();
        var mapRect = mapPanel.GetComponent<RectTransform>();

        // Node prefab — simple circle button
        var nodePrefabGO = new GameObject("NodePrefab");
        nodePrefabGO.transform.SetParent(canvasGO.transform, false);
        nodePrefabGO.SetActive(false);
        var nodeRect = nodePrefabGO.AddComponent<RectTransform>();
        nodeRect.sizeDelta = new Vector2(60, 60);
        var nodeImg = nodePrefabGO.AddComponent<Image>();
        nodeImg.color = Color.white;
        nodePrefabGO.AddComponent<Button>();
        var nodeText = new GameObject("Label");
        nodeText.transform.SetParent(nodePrefabGO.transform, false);
        var nodeLabelRect = nodeText.AddComponent<RectTransform>();
        nodeLabelRect.anchorMin = Vector2.zero;
        nodeLabelRect.anchorMax = Vector2.one;
        nodeLabelRect.offsetMin = nodeLabelRect.offsetMax = Vector2.zero;
        var nodeLabel = nodeText.AddComponent<Text>();
        nodeLabel.alignment = TextAnchor.MiddleCenter;
        nodeLabel.fontSize = 11;
        nodeLabel.color = Color.black;
        SetFont(nodeLabel);

        // ── Event Panel contents ───────────────────────────────────────────
        var eventUI      = eventPanel.AddComponent<EventUI>();
        var evTitleText  = CreateText(eventPanel, "EventTitle",   "Event",       new Vector2(0.1f, 0.8f), new Vector2(0.9f, 0.9f));
        var evBodyText   = CreateText(eventPanel, "EventBody",    "",            new Vector2(0.1f, 0.4f), new Vector2(0.9f, 0.78f));
        var evOutcomeText= CreateText(eventPanel, "EventOutcome", "",            new Vector2(0.1f, 0.25f), new Vector2(0.9f, 0.38f));
        var evContBtn    = CreateButton(eventPanel, "EventContinue", "Continue", new Vector2(0.35f, 0.1f), new Vector2(0.65f, 0.22f));

        Button[] evChoiceBtns  = new Button[4];
        Text[]   evChoiceLabels= new Text[4];
        for (int i = 0; i < 4; i++)
        {
            float y0 = 0.6f - i * 0.1f;
            evChoiceBtns[i]   = CreateButton(eventPanel, $"EventChoice_{i}", $"Choice {i}", new Vector2(0.15f, y0), new Vector2(0.85f, y0 + 0.09f));
            evChoiceLabels[i] = evChoiceBtns[i].GetComponentInChildren<Text>();
        }

        // ── Hotspot Panel contents ─────────────────────────────────────────
        // Background image for room art
        var bgGO   = new GameObject("RoomBackground");
        bgGO.transform.SetParent(hotspotPanel.transform, false);
        var bgRect = bgGO.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = bgRect.offsetMax = Vector2.zero;
        var bgImg  = bgGO.AddComponent<Image>();
        bgImg.color = new Color(0.15f, 0.1f, 0.12f);

        // Hotspot button pool parent
        var hotspotPoolGO = new GameObject("HotspotButtonPool");
        hotspotPoolGO.transform.SetParent(hotspotPanel.transform, false);
        hotspotPoolGO.AddComponent<RectTransform>();

        // Hotspot button prefab
        var hotspotBtnPrefab = new GameObject("HotspotButtonPrefab");
        hotspotBtnPrefab.transform.SetParent(canvasGO.transform, false);
        hotspotBtnPrefab.SetActive(false);
        var hbRect = hotspotBtnPrefab.AddComponent<RectTransform>();
        hbRect.sizeDelta = new Vector2(100, 40);
        hotspotBtnPrefab.AddComponent<Image>().color = new Color(1,1,1,0.8f);
        hotspotBtnPrefab.AddComponent<Button>();
        var hbLabelGO = new GameObject("Label");
        hbLabelGO.transform.SetParent(hotspotBtnPrefab.transform, false);
        var hbLabelRect = hbLabelGO.AddComponent<RectTransform>();
        hbLabelRect.anchorMin = Vector2.zero;
        hbLabelRect.anchorMax = Vector2.one;
        hbLabelRect.offsetMin = hbLabelRect.offsetMax = Vector2.zero;
        var hbLabel = hbLabelGO.AddComponent<Text>();
        hbLabel.alignment = TextAnchor.MiddleCenter;
        hbLabel.fontSize = 12;
        hbLabel.color = Color.black;
        SetFont(hbLabel);
        var hbIconGO = new GameObject("Icon");
        hbIconGO.transform.SetParent(hotspotBtnPrefab.transform, false);
        var hbIconRect = hbIconGO.AddComponent<RectTransform>();
        hbIconRect.anchorMin = new Vector2(0, 0.5f);
        hbIconRect.anchorMax = new Vector2(0, 0.5f);
        hbIconRect.sizeDelta = new Vector2(32, 32);
        hbIconRect.anchoredPosition = new Vector2(16, 0);
        hbIconGO.AddComponent<Image>();

        // ItemPickupPanel
        var itemPickupPanel = CreatePanel(canvasGO, "ItemPickupPanel", new Color(0.1f, 0.08f, 0.12f, 0.95f));
        itemPickupPanel.SetActive(false);
        var itemPickupUI   = itemPickupPanel.AddComponent<ItemPickupUI>();
        var ipNameText     = CreateText(itemPickupPanel, "ItemName",    "Item Name",  new Vector2(0.1f, 0.75f), new Vector2(0.9f, 0.88f));
        var ipFlavorText   = CreateText(itemPickupPanel, "ItemFlavor",  "",           new Vector2(0.1f, 0.45f), new Vector2(0.9f, 0.73f));
        var ipSpriteGO     = new GameObject("ItemSprite");
        ipSpriteGO.transform.SetParent(itemPickupPanel.transform, false);
        var ipSpriteRect   = ipSpriteGO.AddComponent<RectTransform>();
        ipSpriteRect.anchorMin = new Vector2(0.38f, 0.52f);
        ipSpriteRect.anchorMax = new Vector2(0.62f, 0.72f);
        ipSpriteRect.offsetMin = ipSpriteRect.offsetMax = Vector2.zero;
        var ipSpriteImg    = ipSpriteGO.AddComponent<Image>();
        var ipTakeBtn      = CreateButton(itemPickupPanel, "TakeButton",    "Take",     new Vector2(0.1f, 0.28f), new Vector2(0.3f, 0.38f));
        var ipUseNowBtn    = CreateButton(itemPickupPanel, "UseNowButton",  "Use Now",  new Vector2(0.4f, 0.28f), new Vector2(0.6f, 0.38f));
        var ipExamineBtn   = CreateButton(itemPickupPanel, "ExamineButton", "Examine",  new Vector2(0.35f, 0.28f), new Vector2(0.65f, 0.38f));
        var ipLeaveBtn     = CreateButton(itemPickupPanel, "LeaveButton",   "Leave",    new Vector2(0.7f, 0.28f), new Vector2(0.9f, 0.38f));
        var ipFeedbackText = CreateText(itemPickupPanel, "FeedbackText",  "",           new Vector2(0.1f, 0.18f), new Vector2(0.9f, 0.26f));

        // EventChoicePanel
        var eventChoicePanel = CreatePanel(canvasGO, "EventChoicePanel", new Color(0.08f, 0.06f, 0.1f, 0.95f));
        eventChoicePanel.SetActive(false);
        var eventChoiceUI = eventChoicePanel.AddComponent<EventChoiceUI>();
        var ecBodyText    = CreateText(eventChoicePanel, "BodyText",    "", new Vector2(0.05f, 0.6f), new Vector2(0.95f, 0.9f));
        var ecOutcomeText = CreateText(eventChoicePanel, "OutcomeText", "", new Vector2(0.05f, 0.25f), new Vector2(0.95f, 0.55f));
        var ecChoiceGroup = new GameObject("ChoiceButtonGroup");
        ecChoiceGroup.transform.SetParent(eventChoicePanel.transform, false);
        ecChoiceGroup.AddComponent<RectTransform>();
        Button[] ecChoiceBtns  = new Button[4];
        Text[]   ecChoiceLabels= new Text[4];
        for (int i = 0; i < 4; i++)
        {
            float y0 = 0.46f - i * 0.11f;
            ecChoiceBtns[i] = CreateButton(ecChoiceGroup, $"ChoiceBtn_{i}", $"Choice {i}",
                new Vector2(0.1f, y0), new Vector2(0.9f, y0 + 0.1f));
            ecChoiceLabels[i] = ecChoiceBtns[i].GetComponentInChildren<Text>();
        }
        var ecContinueBtn = CreateButton(eventChoicePanel, "ContinueButton", "Continue",
            new Vector2(0.35f, 0.05f), new Vector2(0.65f, 0.14f));

        // LoreReaderPanel
        var lorePanel   = CreatePanel(canvasGO, "LoreReaderPanel", new Color(0.06f, 0.05f, 0.08f, 0.97f));
        lorePanel.SetActive(false);
        var loreReaderUI  = lorePanel.AddComponent<LoreReaderUI>();
        var loreTitleText = CreateText(lorePanel, "TitleText",    "Title",   new Vector2(0.1f, 0.82f), new Vector2(0.9f, 0.93f));
        var loreBodyText  = CreateText(lorePanel, "BodyText",     "",        new Vector2(0.05f, 0.25f), new Vector2(0.95f, 0.8f));
        var loreReactText = CreateText(lorePanel, "ReactionText", "",        new Vector2(0.1f, 0.12f), new Vector2(0.9f, 0.23f));
        var loreCloseBtn  = CreateButton(lorePanel, "CloseButton", "Close",  new Vector2(0.35f, 0.02f), new Vector2(0.65f, 0.1f));

        // HotspotDisplayUI
        var hotspotDisplayUI = hotspotPanel.AddComponent<HotspotDisplayUI>();

        // ── Wire: RoomManager ──────────────────────────────────────────────
        var layerProfile = AssetDatabase.LoadAssetAtPath<LayerGenerationProfileSO>(
            LAYERS_PATH + "/LayerProfile_Layer1.asset");
        if (layerProfile != null)
            roomManager.layerProfiles = new[] { layerProfile };
        else
            Debug.LogWarning("[GameSceneBootstrapper] LayerProfile_Layer1 not found — run CoreAssetGenerator first.");

        roomManager.combatManager = combatManager;

        // ── Wire: MapUI ────────────────────────────────────────────────────
        mapUI.roomManager = roomManager;
        mapUI.mapPanel    = mapPanel.GetComponent<RectTransform>();
        mapUI.nodePrefab  = nodePrefabGO;

        // ── Wire: EncounterBuilder ─────────────────────────────────────────
        encounterBuilder.combatManager = combatManager;

        // ── Wire: CombatRoomBridge ─────────────────────────────────────────
        combatBridge.combatManager = combatManager;
        combatBridge.roomManager   = roomManager;
        combatBridge.saveTrigger   = saveTrigger;
        combatBridge.combatPanel   = combatPanel;
        combatBridge.mapPanel      = mapPanel;
        combatBridge.refugePanel   = refugePanel;

        // ── Wire: SaveTrigger ──────────────────────────────────────────────
        saveTrigger.roomManager   = roomManager;
        saveTrigger.refugeManager = refugeManager;

        // ── Wire: EventUI ──────────────────────────────────────────────────
        eventUI.roomManager   = roomManager;
        eventUI.titleText     = evTitleText;
        eventUI.bodyText      = evBodyText;
        eventUI.outcomeText   = evOutcomeText;
        eventUI.continueButton= evContBtn;
        eventUI.choiceButtons = evChoiceBtns;
        eventUI.choiceLabels  = evChoiceLabels;

        // ── Wire: RefugeUI ─────────────────────────────────────────────────
        refugeUI.refugeManager    = refugeManager;
        refugeUI.runStateManager  = runStateManager;
        refugeUI.refugePanel      = refugePanel;
        refugeUI.bloomText        = bloomText;
        refugeUI.startRunButton   = startRunBtn;
        refugeUI.fullPartyButton  = fullPartyBtn;
        refugeUI.tabRecovery      = tabBtns[0];
        refugeUI.tabParty         = tabBtns[1];
        refugeUI.tabUpgrades      = tabBtns[2];
        refugeUI.tabGallery       = tabBtns[3];
        refugeUI.tabKnowledge     = tabBtns[4];
        refugeUI.heroineRecoveryRows = recoveryRows;
        refugeUI.heroineStatsRows    = partyRows;
        refugeUI.heroineUpgradeRows  = upgradeRows;

        // ── Wire: HotspotDisplayUI ─────────────────────────────────────────
        hotspotDisplayUI.roomManager        = roomManager;
        hotspotDisplayUI.backgroundRect     = bgRect;
        hotspotDisplayUI.backgroundImage    = bgImg;
        hotspotDisplayUI.hotspotButtonPrefab= hotspotBtnPrefab;
        hotspotDisplayUI.itemPickupUI       = itemPickupUI;
        hotspotDisplayUI.eventChoiceUI      = eventChoiceUI;
        hotspotDisplayUI.loreReaderUI       = loreReaderUI;

        // ── Wire: ItemPickupUI ─────────────────────────────────────────────
        itemPickupUI.panel          = itemPickupPanel;
        itemPickupUI.itemNameText   = ipNameText;
        itemPickupUI.itemFlavorText = ipFlavorText;
        itemPickupUI.itemSpriteImage= ipSpriteImg;
        itemPickupUI.takeButton     = ipTakeBtn;
        itemPickupUI.useNowButton   = ipUseNowBtn;
        itemPickupUI.examineButton  = ipExamineBtn;
        itemPickupUI.leaveButton    = ipLeaveBtn;
        itemPickupUI.feedbackText   = ipFeedbackText;

        // ── Wire: EventChoiceUI ────────────────────────────────────────────
        eventChoiceUI.panel             = eventChoicePanel;
        eventChoiceUI.bodyText          = ecBodyText;
        eventChoiceUI.outcomeText       = ecOutcomeText;
        eventChoiceUI.choiceButtonGroup = ecChoiceGroup;
        eventChoiceUI.choiceButtons     = ecChoiceBtns;
        eventChoiceUI.choiceButtonLabels= ecChoiceLabels;
        eventChoiceUI.continueButton    = ecContinueBtn;

        // ── Wire: LoreReaderUI ─────────────────────────────────────────────
        loreReaderUI.panel        = lorePanel;
        loreReaderUI.titleText    = loreTitleText;
        loreReaderUI.bodyText     = loreBodyText;
        loreReaderUI.reactionText = loreReactText;
        loreReaderUI.closeButton  = loreCloseBtn;

        // ── Wire: GameBootstrap ────────────────────────────────────────────
        gameBootstrap.runStateManager = runStateManager;
        gameBootstrap.refugeManager   = refugeManager;
        gameBootstrap.roomManager     = roomManager;
        gameBootstrap.refugePanel     = refugePanel;
        gameBootstrap.combatPanel     = combatPanel;
        gameBootstrap.mapPanel        = mapPanel;
        gameBootstrap.eventPanel      = eventPanel;

        // Load starter heroine data SOs
        var lysandra  = AssetDatabase.LoadAssetAtPath<CharacterDataSO>(HEROINES_PATH + "/lysandra.asset");
        var miraVoss  = AssetDatabase.LoadAssetAtPath<CharacterDataSO>(HEROINES_PATH + "/mira_voss.asset");
        var seraphine = AssetDatabase.LoadAssetAtPath<CharacterDataSO>(HEROINES_PATH + "/seraphine.asset");

        if (lysandra && miraVoss && seraphine)
            gameBootstrap.starterHeroineData = new[] { lysandra, miraVoss, seraphine };
        else
            Debug.LogWarning("[GameSceneBootstrapper] Heroine SOs not found — run CoreAssetGenerator first.");

        // Load starter abilities
        gameBootstrap.lysandraStartAbilities = LoadAbilities("dread_slash", "crimson_lunge");
        gameBootstrap.miraStartAbilities     = LoadAbilities("poisoned_dart", "acid_flask");
        gameBootstrap.seraphineStartAbilities= LoadAbilities("holy_light", "mending_prayer");

        // ── Save scene ─────────────────────────────────────────────────────
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(),
            "Assets/Scenes/game.unity");

        Debug.Log("[GameSceneBootstrapper] ✅ Game scene built and saved to Assets/Scenes/game.unity");
        Debug.Log("[GameSceneBootstrapper] Press Play to test. Check console for wiring warnings.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // Helpers
    // ══════════════════════════════════════════════════════════════════════

    private static GameObject GetOrCreate(string name)
    {
        var go = GameObject.Find(name);
        if (go == null) go = new GameObject(name);
        return go;
    }

    private static T AddComponent<T>(GameObject go) where T : Component
    {
        return go.GetComponent<T>() ?? go.AddComponent<T>();
    }

    private static GameObject CreatePanel(GameObject parent, string name, Color bgColor)
    {
        var go   = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        var img  = go.AddComponent<Image>();
        img.color = bgColor;
        return go;
    }

    private static Text CreateText(GameObject parent, string name, string content,
        Vector2 anchorMin, Vector2 anchorMax)
    {
        var go   = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        var txt  = go.AddComponent<Text>();
        txt.text      = content;
        txt.color     = Color.white;
        txt.fontSize  = 18;
        txt.alignment = TextAnchor.MiddleCenter;
        SetFont(txt);
        return txt;
    }

    private static Button CreateButton(GameObject parent, string name, string label,
        Vector2 anchorMin, Vector2 anchorMax)
    {
        var go   = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        go.AddComponent<Image>().color = new Color(0.25f, 0.22f, 0.3f);
        var btn  = go.AddComponent<Button>();
        var lblGO= new GameObject("Label");
        lblGO.transform.SetParent(go.transform, false);
        var lblRect = lblGO.AddComponent<RectTransform>();
        lblRect.anchorMin = Vector2.zero;
        lblRect.anchorMax = Vector2.one;
        lblRect.offsetMin = lblRect.offsetMax = Vector2.zero;
        var txt  = lblGO.AddComponent<Text>();
        txt.text      = label;
        txt.color     = Color.white;
        txt.fontSize  = 16;
        txt.alignment = TextAnchor.MiddleCenter;
        SetFont(txt);
        return btn;
    }

    private static void SetFont(Text txt)
    {
        var font = Resources.GetBuiltinResource<Font>($"{FONT_NAME}.ttf");
        if (font != null) txt.font = font;
    }

    private static CharacterAbilitySO[] LoadAbilities(string id0, string id1)
    {
        var a0 = AssetDatabase.LoadAssetAtPath<CharacterAbilitySO>(
            $"{ABILITIES_PATH}/{id0}.asset");
        var a1 = AssetDatabase.LoadAssetAtPath<CharacterAbilitySO>(
            $"{ABILITIES_PATH}/{id1}.asset");
        if (a0 == null) Debug.LogWarning($"[GameSceneBootstrapper] Ability not found: {id0}");
        if (a1 == null) Debug.LogWarning($"[GameSceneBootstrapper] Ability not found: {id1}");
        return new[] { a0, a1 };
    }
}
#endif
