using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// HotspotDisplayUI
// ════════════════════════════════════════════════════════════════════════════
// Attach to Canvas child: HotspotDisplayRoot
//
// Subscribes to RoomManager.OnHotspotRoomEntered and OnRoomCompleted.
// Spawns pooled hotspot buttons at anchor positions converted to canvas space.
// Routes clicks to the correct sub-panel or directly to RoomManager.
// ════════════════════════════════════════════════════════════════════════════

public class HotspotDisplayUI : MonoBehaviour
{
    // ── Inspector ────────────────────────────────────────────────────────────

    [Header("References")]
    public RoomManager roomManager;
    public RectTransform backgroundRect;       // room background Image RectTransform
    public Image backgroundImage;             // room background Image component
    public GameObject hotspotButtonPrefab;    // Button + Text child + Icon Image child

    [Header("Sub-Panels")]
    public ItemPickupUI  itemPickupUI;
    public EventChoiceUI eventChoiceUI;
    public LoreReaderUI  loreReaderUI;

    // ── Pool ─────────────────────────────────────────────────────────────────

    private const int POOL_SIZE = 12;

    private Button[]   _poolButtons;
    private Text[]     _poolLabels;
    private Image[]    _poolIcons;

    // Parallel arrays: which hotspot each active button represents
    private RoomHotspotSO[] _activeHotspots;
    private RoomNode        _currentRoom;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    private void Awake()
    {
        BuildPool();
    }

    private void OnEnable()
    {
        if (roomManager != null)
        {
            roomManager.OnHotspotRoomEntered += HandleHotspotRoomEntered;
            roomManager.OnRoomCompleted      += HandleRoomCompleted;
        }
    }

    private void OnDisable()
    {
        if (roomManager != null)
        {
            roomManager.OnHotspotRoomEntered -= HandleHotspotRoomEntered;
            roomManager.OnRoomCompleted      -= HandleRoomCompleted;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // ── Pool construction ────────────────────────────────────────────────────

    private void BuildPool()
    {
        _poolButtons    = new Button[POOL_SIZE];
        _poolLabels     = new Text[POOL_SIZE];
        _poolIcons      = new Image[POOL_SIZE];
        _activeHotspots = new RoomHotspotSO[POOL_SIZE];

        Transform poolParent = transform.Find("HotspotButtonPool") ?? transform;

        for (int i = 0; i < POOL_SIZE; i++)
        {
            GameObject go = Instantiate(hotspotButtonPrefab, poolParent);
            go.name = $"HotspotButton_{i:D2}";
            go.SetActive(false);

            _poolButtons[i] = go.GetComponent<Button>();
            _poolLabels[i]  = go.GetComponentInChildren<Text>();

            // Expect a child named "Icon" with an Image component
            Transform iconT = go.transform.Find("Icon");
            _poolIcons[i]   = iconT != null ? iconT.GetComponent<Image>() : null;

            int idx = i; // capture for lambda
            _poolButtons[i].onClick.AddListener(() => OnHotspotClicked(idx));
        }
    }

    // ── Room entered ─────────────────────────────────────────────────────────

    private void HandleHotspotRoomEntered(RoomNode room, RoomHotspotSO[] hotspots)
    {
        _currentRoom = room;

        // Background sprite
        // TODO: CurrentRoomTemplate not yet available on RunStateManager.
        Debug.Log("[HotspotDisplayUI] Background sprite swap pending CurrentRoomTemplate on RunStateManager.");

        gameObject.SetActive(true);
        DeactivatePool();

        if (hotspots == null || hotspots.Length == 0)
            return;

        // Active heroine — needed for heroine-lock checks
        var party = RunStateManager.Instance?.Party;
        string activeHeroineId = (party != null && party.Count > 0)
            ? party[0].characterId
            : string.Empty;

        int buttonIndex = 0;
        for (int h = 0; h < hotspots.Length && buttonIndex < POOL_SIZE; h++)
        {
            RoomHotspotSO hotspot = hotspots[h];
            if (hotspot == null) continue;

            // Visibility check
            if (!hotspot.IsVisible()) continue;

            // Find anchor position for this hotspot
            Vector2 canvasPos = FindAnchorPosition(room, hotspot, h);

            // Configure button
            _activeHotspots[buttonIndex] = hotspot;

            RectTransform rt = _poolButtons[buttonIndex].GetComponent<RectTransform>();
            rt.anchoredPosition = canvasPos;

            if (_poolLabels[buttonIndex] != null)
                _poolLabels[buttonIndex].text = hotspot.displayName;

            if (_poolIcons[buttonIndex] != null && hotspot.hotspotSprite != null)
            {
                _poolIcons[buttonIndex].sprite  = hotspot.hotspotSprite;
                _poolIcons[buttonIndex].enabled = true;
            }

            // Heroine lock
            bool locked = IsHeroineLocked(hotspot, activeHeroineId);
            _poolButtons[buttonIndex].interactable = !locked;

            // Tooltip for locked buttons is handled in OnHotspotClicked

            _poolButtons[buttonIndex].gameObject.SetActive(true);
            buttonIndex++;
        }
    }

    // ── Anchor position helper ────────────────────────────────────────────────

    /// <summary>
    /// Returns the canvas anchored position for a hotspot.
    /// Tries to match by index against hotspotAnchors; falls back to centre.
    /// </summary>
    private Vector2 FindAnchorPosition(RoomNode room, RoomHotspotSO hotspot, int hotspotIndex)
    {
        // spawnedHotspots[0..anchors-1] correspond to anchor slots;
        // fixed hotspots appended after have no anchor position — place at centre.
        if (room.hotspotAnchors != null && hotspotIndex < room.hotspotAnchors.Length)
        {
            return ToCanvasPos(room.hotspotAnchors[hotspotIndex].screenPosition, backgroundRect);
        }
        return Vector2.zero; // fixed hotspots default to centre
    }

    /// <summary>Converts normalised position to canvas anchored position.</summary>
    private static Vector2 ToCanvasPos(Vector2 normalised, RectTransform rect)
    {
        return new Vector2(
            normalised.x * rect.rect.width  - rect.rect.width  * 0.5f,
            normalised.y * rect.rect.height - rect.rect.height * 0.5f
        );
    }

    // ── Heroine lock check ───────────────────────────────────────────────────

    private static bool IsHeroineLocked(RoomHotspotSO hotspot, string activeHeroineId)
    {
        if (hotspot.heroineLock == null || hotspot.heroineLock.Length == 0)
            return false;

        foreach (string id in hotspot.heroineLock)
            if (id == activeHeroineId) return false;

        return true; // active heroine is not in the allowed list
    }

    // ── Button click ─────────────────────────────────────────────────────────

    private void OnHotspotClicked(int idx)
    {
        RoomHotspotSO hotspot = _activeHotspots[idx];
        if (hotspot == null) return;

        // Locked button feedback (should not happen since interactable = false, but guard anyway)
        if (IsHeroineLocked(hotspot, GetActiveHeroineId()))
            return;

        switch (hotspot.type)
        {
            case RoomHotspotSO.HotspotType.Item:
                HandleItemHotspotClick(idx, hotspot);
                break;

            case RoomHotspotSO.HotspotType.Door:
                roomManager.SubmitHotspotInteraction(_currentRoom, hotspot);
                // Door hotspots are never consumed; room completion handled by RoomManager
                break;

            case RoomHotspotSO.HotspotType.Event:
                eventChoiceUI?.Open(_currentRoom, hotspot, this, idx);
                break;

            case RoomHotspotSO.HotspotType.Lore:
                loreReaderUI?.Open(_currentRoom, hotspot, this, idx);
                break;
        }
    }

    private void HandleItemHotspotClick(int idx, RoomHotspotSO hotspot)
    {
        if (hotspot.itemReward == null)
        {
            Debug.LogWarning($"[HotspotDisplayUI] Item hotspot {hotspot.hotspotId} has no itemReward.");
            return;
        }

        // Key items: auto-take, no dialog
        if (hotspot.itemReward.category == ItemCategory.Key)
        {
            bool added = ItemManager.Instance.AddItem(hotspot.itemReward);
            if (added && hotspot.consumeAfterUse)
                ConsumeButton(idx);
            return;
        }

        itemPickupUI?.Open(hotspot.itemReward, hotspot, this, idx);
    }

    // ── Called by sub-panels after interaction ────────────────────────────────

    /// <summary>
    /// Greys out a hotspot button after it has been consumed.
    /// Called by sub-panels (ItemPickupUI, EventChoiceUI, LoreReaderUI).
    /// </summary>
    public void ConsumeButton(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= POOL_SIZE) return;
        _poolButtons[buttonIndex].interactable = false;

        // Visually dim the button
        var colors = _poolButtons[buttonIndex].colors;
        colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);
        _poolButtons[buttonIndex].colors = colors;
    }

    // ── Room completed ────────────────────────────────────────────────────────

    private void HandleRoomCompleted(RoomNode room)
    {
        if (room != _currentRoom) return;
        DeactivatePool();
        _currentRoom = null;
        gameObject.SetActive(false);
    }

    // ── Pool helpers ──────────────────────────────────────────────────────────

    private void DeactivatePool()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            _poolButtons[i].gameObject.SetActive(false);
            _activeHotspots[i] = null;
        }
    }

    // ── Utility ───────────────────────────────────────────────────────────────

    private static string GetActiveHeroineId()
    {
        var party = RunStateManager.Instance?.Party;
        return (party != null && party.Count > 0) ? party[0].characterId : string.Empty;
    }
}
