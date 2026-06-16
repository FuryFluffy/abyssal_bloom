using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// CombatRoomBridge
// ════════════════════════════════════════════════════════════════════════════
// Listens to CombatManager.OnEncounterEnd and drives the room system.
//
// Attach to the same _Managers GameObject as CombatManager and RoomManager.
// Wire all references in the Inspector.
// ════════════════════════════════════════════════════════════════════════════

public class CombatRoomBridge : MonoBehaviour
{
    [Header("References")]
    public CombatManager combatManager;
    public RoomManager   roomManager;
    public SaveTrigger   saveTrigger;

    [Header("UI Panels")]
    [Tooltip("Shown after a run ends (wipe). Hide combat/map, show refuge.")]
    public GameObject refugePanel;
    public GameObject combatPanel;
    public GameObject mapPanel;

    // ── Lifecycle ──────────────────────────────────────────────────────────

    private void OnEnable()
    {
        if (combatManager != null)
            combatManager.OnEncounterEnd += HandleEncounterEnd;
    }

    private void OnDisable()
    {
        if (combatManager != null)
            combatManager.OnEncounterEnd -= HandleEncounterEnd;
    }

    // ── Handler ────────────────────────────────────────────────────────────

    private void HandleEncounterEnd(EncounterResult result)
    {
        switch (result)
        {
            case EncounterResult.Victory:
                HandleVictory();
                break;
            case EncounterResult.Fled:
                HandleFled();
                break;
            case EncounterResult.Defeated:
                HandleDefeat();
                break;
        }
    }

    private void HandleVictory()
    {
        FlagManager.Instance?.SetFlag(
            FlagManager.Scope.RunState, "last_combat_result", "victory");

        var room = roomManager?.CurrentRoom;
        if (room != null)
        {
            int bloom = BloomForRoomType(room.roomType);
            if (bloom > 0)
                RunStateManager.Instance?.AddBloom(bloom);
        }

        roomManager?.CompleteRoom(room);
        // SaveTrigger.OnRoomCompleted fires automatically via RoomManager event
    }

    private void HandleFled()
    {
        FlagManager.Instance?.SetFlag(
            FlagManager.Scope.RunState, "last_combat_result", "fled");

        // Mark the room completed so the player can move on, but grant no Bloom.
        var room = roomManager?.CurrentRoom;
        roomManager?.CompleteRoom(room);
        // SaveTrigger.OnRoomCompleted fires automatically via RoomManager event
    }

    private void HandleDefeat()
    {
        FlagManager.Instance?.SetFlag(
            FlagManager.Scope.RunState, "last_combat_result", "defeat");

        // FIX 1: End the run properly — delete save file, save meta,
        // call ReturnToRefuge so RefugeManager initialises correctly.
        roomManager?.HandleRunEnd();
        saveTrigger?.DeleteRunAndSaveMeta();
        RunStateManager.Instance?.ReturnToRefuge();

        // Switch panels: hide combat and map, show refuge
        if (combatPanel != null) combatPanel.SetActive(false);
        if (mapPanel    != null) mapPanel.SetActive(false);
        if (refugePanel != null) refugePanel.SetActive(true);
    }

    // ── Bloom lookup ───────────────────────────────────────────────────────

    /// <summary>Returns the Bloom awarded for completing a room of the given type.</summary>
    public static int BloomForRoomType(RoomType type) => type switch
    {
        RoomType.Battle => 8,
        RoomType.Elite  => 18,
        RoomType.Boss   => 40,
        _               => 0
    };
}
