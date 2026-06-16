using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// EventUI
// ════════════════════════════════════════════════════════════════════════════
// Subscribes to RoomManager.OnEventRoomEntered, displays event text and
// choice buttons, then calls RoomManager.SubmitEventChoice() on Continue.
//
// Hierarchy expected (all wired in Inspector):
//   [EventPanel]           ← panelRoot  (toggled active/inactive)
//     [TitleText]          ← titleText
//     [BodyText]           ← bodyText
//     [OutcomeText]        ← outcomeText  (hidden until choice made)
//     [Button0..3]         ← choiceButtons[0..3]  (pool of 4)
//       [Label]            ← choiceLabels[0..3]
//     [ContinueButton]     ← continueButton  (hidden until outcome shown)
// ════════════════════════════════════════════════════════════════════════════

public class EventUI : MonoBehaviour
{
    // ── Inspector ───────────────────────────────────────────────────────────

    [Header("References")]
    public RoomManager roomManager;

    [Header("Panel")]
    public GameObject panelRoot;

    [Header("Text Fields")]
    public Text titleText;
    public Text bodyText;
    public Text outcomeText;

    [Header("Choice Button Pool (4 slots)")]
    public Button[] choiceButtons = new Button[4];
    public Text[]   choiceLabels  = new Text[4];

    [Header("Continue Button")]
    public Button continueButton;

    // ── Runtime state ───────────────────────────────────────────────────────

    private RoomNode                  _currentRoom;
    private RoomEventSO.EventChoice   _pendingChoice;

    // ── Lifecycle ────────────────────────────────────────────────────────────

    private void OnEnable()
    {
        if (roomManager != null)
            roomManager.OnEventRoomEntered += HandleEventRoomEntered;
    }

    private void OnDisable()
    {
        if (roomManager != null)
            roomManager.OnEventRoomEntered -= HandleEventRoomEntered;
    }

    private void Start()
    {
        // Hidden between events
        panelRoot.SetActive(false);
        outcomeText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        continueButton.onClick.AddListener(OnContinueClicked);
    }

    // ── Event handler ────────────────────────────────────────────────────────

    private void HandleEventRoomEntered(RoomNode room, List<RoomEventSO.EventChoice> choices)
    {
        _currentRoom   = room;
        _pendingChoice = null;

        panelRoot.SetActive(true);
        outcomeText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        // Title
        string title = (room.eventData != null && !string.IsNullOrEmpty(room.eventData.displayTitle))
            ? room.eventData.displayTitle
            : room.roomType.ToString();
        titleText.text = title;

        // Body — prepend False Rest reveal line if needed
        string body = room.eventData != null ? room.eventData.bodyText : string.Empty;
        if (room.roomType == RoomType.FalseRest && room.apparentRoomType != RoomType.FalseRest)
            body = "This was no safe haven.\n\n" + body;
        bodyText.text = body;

        // Buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Count)
            {
                RoomEventSO.EventChoice choice = choices[i]; // capture for lambda
                choiceLabels[i].text  = BuildChoiceLabel(choice);
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].interactable = true;

                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceClicked(choice));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // ── Button handlers ──────────────────────────────────────────────────────

    private void OnChoiceClicked(RoomEventSO.EventChoice choice)
    {
        _pendingChoice = choice;

        // Hide choice buttons
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);

        // Show outcome
        outcomeText.text = choice.outcomeText;
        outcomeText.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
    }

    private void OnContinueClicked()
    {
        panelRoot.SetActive(false);
        roomManager.SubmitEventChoice(_currentRoom, _pendingChoice);
    }

    // ── Label builder ─────────────────────────────────────────────────────────

    private static string BuildChoiceLabel(RoomEventSO.EventChoice choice)
    {
        var effects = new List<string>();

        if (choice.healHP > 0)
            effects.Add($"+{choice.healHP} HP");

        if (choice.restoreMP > 0)
            effects.Add($"+{choice.restoreMP} MP");

        if (choice.resolveChange > 0)
            effects.Add($"+{choice.resolveChange} Resolve");
        else if (choice.resolveChange < 0)
            effects.Add($"{choice.resolveChange} Resolve");

        if (choice.corruptionChange != 0)
            effects.Add($"+{choice.corruptionChange} Corrupt");

        if (choice.encounterEnemies != null && choice.encounterEnemies.Length > 0)
            effects.Add("⚔ Combat");

        if (effects.Count == 0)
            return choice.choiceText;

        return $"{choice.choiceText} [{string.Join(", ", effects)}]";
    }
}
