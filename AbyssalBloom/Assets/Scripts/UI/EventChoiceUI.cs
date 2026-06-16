using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// EventChoiceUI
// ════════════════════════════════════════════════════════════════════════════
// Attach to Canvas child: EventChoicePanel (inactive by default)
//
// Phase 1 — Choices: shows eventBodyText and filtered choice buttons.
// Phase 2 — Outcome: shows outcomeText and a Continue button.
//
// Pool: 4 choice buttons (same pattern as CombatUI / EventUI).
// ════════════════════════════════════════════════════════════════════════════

public class EventChoiceUI : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────

    [Header("Panel")]
    public GameObject panel;

    [Header("Text")]
    public Text bodyText;
    public Text outcomeText;

    [Header("Choice Button Pool (4 slots)")]
    public GameObject choiceButtonGroup;
    public Button[]   choiceButtons      = new Button[4];
    public Text[]     choiceButtonLabels = new Text[4];

    [Header("Continue")]
    public Button continueButton;

    // ── Runtime state ─────────────────────────────────────────────────────────

    private RoomNode         _room;
    private RoomHotspotSO    _hotspot;
    private HotspotDisplayUI _display;
    private int              _buttonIndex;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    private void Start()
    {
        panel.SetActive(false);
        continueButton.onClick.AddListener(OnContinueClicked);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int idx = i; // capture
            choiceButtons[i].onClick.AddListener(() => OnChoiceClicked(idx));
        }
    }

    // ── Open ──────────────────────────────────────────────────────────────────

    /// <summary>Called by HotspotDisplayUI when an Event hotspot is clicked.</summary>
    public void Open(RoomNode room, RoomHotspotSO hotspot,
                     HotspotDisplayUI display, int buttonIndex)
    {
        _room        = room;
        _hotspot     = hotspot;
        _display     = display;
        _buttonIndex = buttonIndex;

        bodyText.text = hotspot.eventBodyText;
        outcomeText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        choiceButtonGroup.SetActive(true);

        // Populate choice buttons
        int shown = 0;
        if (hotspot.eventChoices != null)
        {
            for (int i = 0; i < hotspot.eventChoices.Length && shown < choiceButtons.Length; i++)
            {
                var choice = hotspot.eventChoices[i];
                if (!RoomHotspotSO.AreConditionsMet(choice)) continue;

                choiceButtonLabels[shown].text = choice.choiceText;
                choiceButtons[shown].gameObject.SetActive(true);
                choiceButtons[shown].interactable = true;
                shown++;
            }
        }

        // Hide unused slots
        for (int i = shown; i < choiceButtons.Length; i++)
            choiceButtons[i].gameObject.SetActive(false);

        panel.SetActive(true);
    }

    // ── Choice clicked ────────────────────────────────────────────────────────

    private void OnChoiceClicked(int poolIdx)
    {
        // Map pool index back to the matching EventChoice (skip filtered choices)
        RoomHotspotSO.EventChoice selected = null;
        int shown = 0;
        if (_hotspot.eventChoices != null)
        {
            foreach (var choice in _hotspot.eventChoices)
            {
                if (!RoomHotspotSO.AreConditionsMet(choice)) continue;
                if (shown == poolIdx)
                {
                    selected = choice;
                    break;
                }
                shown++;
            }
        }

        if (selected == null)
        {
            Debug.LogWarning("[EventChoiceUI] Could not map pool index to EventChoice.");
            return;
        }

        // Submit to RoomManager — applies stat changes, flags, optional combat
        RoomManager rm = _display.roomManager;
        rm?.SubmitHotspotInteraction(_room, _hotspot, selected);

        // Transition to outcome phase
        choiceButtonGroup.SetActive(false);
        outcomeText.text = selected.outcomeText;
        outcomeText.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);

        if (_hotspot.consumeAfterUse)
            _display.ConsumeButton(_buttonIndex);
    }

    // ── Continue clicked ──────────────────────────────────────────────────────

    private void OnContinueClicked()
    {
        panel.SetActive(false);
        _room    = null;
        _hotspot = null;
        _display = null;
        // If the choice triggered combat, RoomManager already launched it.
        // HotspotDisplayUI remains visible so other hotspots stay available.
    }
}
