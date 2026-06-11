using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// CombatRoomBridge
// ════════════════════════════════════════════════════════════════════════════
// Listens to CombatManager.OnEncounterEnd and drives the room system.
//
// Attach to the same _Managers GameObject as CombatManager and RoomManager.
// Wire both references in the Inspector.
// ════════════════════════════════════════════════════════════════════════════

public class CombatRoomBridge : MonoBehaviour
{
    [Header("References")]
    public CombatManager combatManager;
    public RoomManager   roomManager;

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

    private void HandleEncounterEnd(bool victory)
    {
        if (victory)
            HandleVictory();
        else
            HandleDefeat();
    }

    private void HandleVictory()
    {
        FlagManager.Instance?.SetFlag(FlagManager.Scope.RunState, "last_combat_result", "victory");

        var room = roomManager?.CurrentRoom;
        if (room != null)
        {
            int bloom = BloomForRoomType(room.roomType);
            if (bloom > 0)
                RunStateManager.Instance?.AddBloom(bloom);
        }

        roomManager?.CompleteRoom(room);
    }

    private void HandleDefeat()
    {
        FlagManager.Instance?.SetFlag(FlagManager.Scope.RunState, "last_combat_result", "defeat");
        roomManager?.HandleRunEnd();
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
