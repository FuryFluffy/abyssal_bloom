using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// ItemPickupUI
// ════════════════════════════════════════════════════════════════════════════
// Attach to Canvas child: ItemPickupPanel (inactive by default)
//
// Opened by HotspotDisplayUI when an Item hotspot is clicked (non-Key).
// Displays item info and category-appropriate action buttons.
//
// Dialog buttons by ItemCategory:
//   Consumable  → Take / Use Now / Leave
//   CombatTool  → Take / Leave
//   LoreItem    → Examine / Take / Leave  (Examine swaps to flavor text view)
// Key items are auto-taken in HotspotDisplayUI — this panel is not shown.
// ════════════════════════════════════════════════════════════════════════════

public class ItemPickupUI : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────

    [Header("Panel")]
    public GameObject panel;

    [Header("Display")]
    public Text  itemNameText;
    public Text  itemFlavorText;
    public Image itemSpriteImage;

    [Header("Buttons")]
    public Button takeButton;
    public Button useNowButton;
    public Button examineButton;
    public Button leaveButton;

    [Header("Feedback")]
    public Text feedbackText;   // "Inventory full" — shown briefly

    // ── Runtime state ─────────────────────────────────────────────────────────

    private ItemSO            _item;
    private RoomHotspotSO     _hotspot;
    private HotspotDisplayUI  _display;
    private int               _buttonIndex;
    private bool              _examined;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    private void Start()
    {
        panel.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        takeButton.onClick.AddListener(OnTakeClicked);
        useNowButton.onClick.AddListener(OnUseNowClicked);
        examineButton.onClick.AddListener(OnExamineClicked);
        leaveButton.onClick.AddListener(OnLeaveClicked);
    }

    // ── Open ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Called by HotspotDisplayUI. Key items bypass this panel entirely.
    /// </summary>
    public void Open(ItemSO item, RoomHotspotSO hotspot,
                     HotspotDisplayUI display, int buttonIndex)
    {
        _item        = item;
        _hotspot     = hotspot;
        _display     = display;
        _buttonIndex = buttonIndex;
        _examined    = false;

        // Populate display
        itemNameText.text   = item.displayName;
        itemFlavorText.text = hotspot.itemPickupText; // pickup-specific text first
        feedbackText.gameObject.SetActive(false);

        if (itemSpriteImage != null)
        {
            itemSpriteImage.sprite  = item.itemSprite;
            itemSpriteImage.enabled = item.itemSprite != null;
        }

        // Configure buttons based on category
        takeButton.gameObject.SetActive(true);
        leaveButton.gameObject.SetActive(true);

        switch (item.category)
        {
            case ItemCategory.Consumable:
                useNowButton.gameObject.SetActive(item.usableOutOfCombat);
                examineButton.gameObject.SetActive(false);
                break;

            case ItemCategory.CombatTool:
                useNowButton.gameObject.SetActive(false);
                examineButton.gameObject.SetActive(false);
                break;

            case ItemCategory.LoreItem:
                useNowButton.gameObject.SetActive(false);
                examineButton.gameObject.SetActive(true);
                break;

            default:
                useNowButton.gameObject.SetActive(false);
                examineButton.gameObject.SetActive(false);
                break;
        }

        panel.SetActive(true);
    }

    // ── Button handlers ───────────────────────────────────────────────────────

    private void OnTakeClicked()
    {
        if (ItemManager.Instance.IsInventoryFull())
        {
            ShowFeedback("Inventory full.");
            return;
        }

        ItemManager.Instance.AddItem(_item);

        if (_hotspot.consumeAfterUse)
            _display.ConsumeButton(_buttonIndex);

        Close();
    }

    private void OnUseNowClicked()
    {
        // "Use Now" requires the item to be in inventory first, then UseItem removes it
        // Add → use pattern preserves ItemManager.UseItem contract
        if (ItemManager.Instance.IsInventoryFull())
        {
            ShowFeedback("Inventory full — can't use right now.");
            return;
        }

        ItemManager.Instance.AddItem(_item);

        var party = RunStateManager.Instance?.Party;
        RuntimeCharacterState target = (party != null && party.Count > 0) ? party[0] : null;
        if (target != null)
            ItemManager.Instance.UseItem(_item, target);

        // Consumables are removed by UseItem; always consume the hotspot
        _display.ConsumeButton(_buttonIndex);
        Close();
    }

    private void OnExamineClicked()
    {
        if (_examined) return;
        _examined = true;

        // Swap flavor text to item's own flavor text
        itemFlavorText.text = _item.flavorText;

        // Set examined flag in RunState scope
        FlagManager.Instance?.SetFlag(
            FlagManager.Scope.RunState,
            $"examined_{_item.itemId}",
            "1");

        // After examining, show Take/Leave only
        examineButton.gameObject.SetActive(false);
        // Panel stays open
    }

    private void OnLeaveClicked()
    {
        Close();
        // Hotspot remains active (consumeAfterUse determines this in HotspotDisplayUI)
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void Close()
    {
        panel.SetActive(false);
        _item    = null;
        _hotspot = null;
        _display = null;
    }

    private void ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HideFeedbackAfterDelay(2f));
    }

    private IEnumerator HideFeedbackAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        feedbackText.gameObject.SetActive(false);
    }
}
