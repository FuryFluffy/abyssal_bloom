using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// SaveTrigger — subscribes to game events and calls SaveSystem at the
// correct moments. No game logic lives here.
// ════════════════════════════════════════════════════════════════════════════
// Attach to the _Managers GameObject.
// Drag in the required references in the Inspector.
//
// Save moments:
//   • Room completed     → save run progress
//   • Bloom spent        → save run progress (upgrade flags changed)
//   • Return to Refuge   → save run + meta
//   • Run ended (wipe)   → delete run file, save meta
//   • Application quit   → save run + meta as a safety net
// ════════════════════════════════════════════════════════════════════════════

public class SaveTrigger : MonoBehaviour
{
    // ── Inspector ──────────────────────────────────────────────────────────

    [Header("References")]
    public RoomManager   roomManager;
    public RefugeManager refugeManager;

    [Tooltip("Save slot index — 0 for single-slot games.")]
    public int slotIndex = 0;

    // ── Lifecycle ──────────────────────────────────────────────────────────

    private void OnEnable()
    {
        if (roomManager != null)
        {
            roomManager.OnRoomCompleted  += HandleRoomCompleted;
            roomManager.OnLayerCompleted += HandleLayerCompleted;
        }

        if (refugeManager != null)
        {
            refugeManager.OnBloomChanged += HandleBloomChanged;
        }
    }

    private void OnDisable()
    {
        if (roomManager != null)
        {
            roomManager.OnRoomCompleted  -= HandleRoomCompleted;
            roomManager.OnLayerCompleted -= HandleLayerCompleted;
        }

        if (refugeManager != null)
        {
            refugeManager.OnBloomChanged -= HandleBloomChanged;
        }
    }

    private void OnApplicationQuit()
    {
        // Safety net — save everything on quit regardless of state
        SaveRun();
        SaveSystem.SaveMeta();
    }

    // ── Handlers ──────────────────────────────────────────────────────────

    private void HandleRoomCompleted(RoomNode room)
    {
        SaveRun();
    }

    private void HandleLayerCompleted(int layerNumber)
    {
        // Layer clear is a meaningful checkpoint — save run + meta
        SaveRun();
        SaveSystem.SaveMeta();
    }

    private void HandleBloomChanged()
    {
        // Bloom spent or gained — upgrade flags may have changed
        SaveRun();
    }

    // ── Save helpers ───────────────────────────────────────────────────────

    /// <summary>
    /// Build a RunSaveData snapshot from current RunStateManager state
    /// and write it to disk.
    /// </summary>
    public void SaveRun()
    {
        var rsm = RunStateManager.Instance;
        if (rsm == null || !rsm.IsRunActive) return;

        var party = rsm.Party;
        if (party == null || party.Count == 0) return;

        var data = new RunSaveData
        {
            slotIndex     = slotIndex,
            runSeed       = rsm.RunSeed,
            currentLayer  = rsm.CurrentLayer,
            currentRoomId = rsm.CurrentRoomId,
            partyIds      = new string[3],
            currentHP     = new int[3],
            currentMP     = new int[3],
            currentResolve= new int[3],
            currentCorrupt= new int[3],
        };

        for (int i = 0; i < 3 && i < party.Count; i++)
        {
            var h = party[i];
            data.partyIds[i]       = h.characterId;
            data.currentHP[i]      = h.currentHP;
            data.currentMP[i]      = h.currentMP;
            data.currentResolve[i] = h.resolve;
            data.currentCorrupt[i] = h.corruption;
        }

        SaveSystem.SaveRun(data);
    }

    /// <summary>
    /// Call when a run ends in a wipe — deletes the run file so the
    /// player starts fresh next session, but preserves meta/knowledge.
    /// </summary>
    public void DeleteRunAndSaveMeta()
    {
        SaveSystem.DeleteRun(slotIndex);
        SaveSystem.SaveMeta();
    }
}
