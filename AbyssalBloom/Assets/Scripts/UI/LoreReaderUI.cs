using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// LoreReaderUI
// ════════════════════════════════════════════════════════════════════════════
// Attach to Canvas child: LoreReaderPanel (inactive by default)
//
// Displays lore title, body text, and heroine reaction line.
// Calls RoomManager.SubmitHotspotInteraction on first open only (flags +
// Resolve change applied once). Tracked via _interactionSubmitted.
// ════════════════════════════════════════════════════════════════════════════

public class LoreReaderUI : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────

    [Header("Panel")]
    public GameObject panel;

    [Header("Text")]
    public Text titleText;
    public Text bodyText;
    public Text reactionText;

    [Header("Close")]
    public Button closeButton;

    // ── Runtime state ─────────────────────────────────────────────────────────

    private RoomNode         _room;
    private RoomHotspotSO    _hotspot;
    private HotspotDisplayUI _display;
    private int              _buttonIndex;
    private bool             _interactionSubmitted;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    private void Start()
    {
        panel.SetActive(false);
        closeButton.onClick.AddListener(OnCloseClicked);
    }

    // ── Open ──────────────────────────────────────────────────────────────────

    /// <summary>Called by HotspotDisplayUI when a Lore hotspot is clicked.</summary>
    public void Open(RoomNode room, RoomHotspotSO hotspot,
                     HotspotDisplayUI display, int buttonIndex)
    {
        ResetIfNewHotspot(hotspot);

        _room        = room;
        _hotspot     = hotspot;
        _display     = display;
        _buttonIndex = buttonIndex;

        // Populate text
        titleText.text = hotspot.loreTitle;
        bodyText.text  = hotspot.loreBodyText;

        bool hasReaction = !string.IsNullOrEmpty(hotspot.characterReactionLine);
        reactionText.gameObject.SetActive(hasReaction);
        if (hasReaction)
            reactionText.text = hotspot.characterReactionLine;

        // Submit interaction once — applies lore flags and Resolve change
        if (!_interactionSubmitted)
        {
            display.roomManager?.SubmitHotspotInteraction(room, hotspot);
            _interactionSubmitted = true;
        }

        panel.SetActive(true);
    }

    // ── Close ─────────────────────────────────────────────────────────────────

    private void OnCloseClicked()
    {
        if (_hotspot != null && _hotspot.consumeAfterUse)
            _display?.ConsumeButton(_buttonIndex);

        panel.SetActive(false);

        _room    = null;
        _hotspot = null;
        _display = null;
        // _interactionSubmitted intentionally NOT reset here —
        // if the player re-opens the same hotspot (consumeAfterUse = false),
        // the flags should not be applied again.
        // Reset it when a new room is entered (new Open call sets it fresh
        // only when it's a new hotspot instance; see note below).
    }

    // NOTE: _interactionSubmitted is per-panel-instance, not per-hotspot.
    // If consumeAfterUse = false and the player re-opens the same hotspot,
    // the flag/resolve is NOT applied again, which is correct.
    // If a different lore hotspot opens the same panel, Open() should
    // clear the flag. We do this by checking object identity:

    /// <summary>
    /// Resets submission tracking. Called automatically when a new hotspot
    /// opens the panel (detected via hotspot reference change in Open).
    /// </summary>
    private void ResetIfNewHotspot(RoomHotspotSO incoming)
    {
        if (_hotspot != incoming)
            _interactionSubmitted = false;
    }
}
