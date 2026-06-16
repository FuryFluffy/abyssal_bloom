using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RunStateManager — roguelite run progress tracker
// ════════════════════════════════════════════════════════════════════════════
// Attach to the same "_Managers" GameObject as FlagManager.
// Singleton — persists across scenes via DontDestroyOnLoad.
//
// This is DISTINCT from FlagManager:
//   FlagManager     = string key/value store for narrative flags
//   RunStateManager = structured run progress (layer, room, Bloom, seed, etc.)
//
// RunStateManager does NOT own the map — RoomManager does.
// RunStateManager owns the numbers that survive between rooms.
// ════════════════════════════════════════════════════════════════════════════

public class RunStateManager : MonoBehaviour
{
    // ── Singleton ──────────────────────────────────────────────────────────

    public static RunStateManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ── Run State ──────────────────────────────────────────────────────────

    /// <summary>True while a run is in progress.</summary>
    public bool IsRunActive { get; private set; }

    /// <summary>Master seed for this run. All layer seeds derive from this.</summary>
    public int RunSeed { get; private set; }

    /// <summary>Current layer number (1-based).</summary>
    public int CurrentLayer { get; private set; } = 1;

    /// <summary>Rooms completed on the current layer.</summary>
    public int RoomsCompletedThisLayer { get; private set; }

    /// <summary>Total Bloom earned this run (cumulative, never decremented).</summary>
    public int BloomEarned { get; private set; }

    /// <summary>
    /// Bloom currently available to spend at the Refuge.
    /// Seeds from BloomEarned on ReturnToRefuge; decrements as spent.
    /// </summary>
    public int CurrentBloom { get; private set; }

    /// <summary>Bloom accumulated across all runs (for stats / future gallery unlocks).</summary>
    public int TotalBloomEver { get; private set; }

    /// <summary>The node ID of the room the player is currently in (null if between rooms).</summary>
    public string CurrentRoomId { get; private set; }

    // ── Party (heroine RuntimeCharacterStates that persist between rooms) ──
    // Set once at run start, updated after each encounter.
    // Index 0 = active heroine, 1-2 = support.

    public List<RuntimeCharacterState> Party { get; private set; } = new(3);

    // ── Run lifecycle ──────────────────────────────────────────────────────

    /// <summary>
    /// Start a new run. Call from the Refuge hub or a "New Run" button.
    /// </summary>
    /// <param name="party">The three heroines in play order.</param>
    /// <param name="seed">Optional explicit seed. -1 = random.</param>
    public void StartNewRun(List<RuntimeCharacterState> party, int seed = -1)
    {
        IsRunActive             = true;
        RunSeed                 = seed >= 0 ? seed : Random.Range(int.MinValue, int.MaxValue);
        CurrentLayer            = 1;
        RoomsCompletedThisLayer = 0;
        BloomEarned             = 0;
        CurrentBloom            = 0;
        CurrentRoomId           = null;

        Party.Clear();
        Party.AddRange(party);

        // Clear transient run flags
        FlagManager.Instance?.ClearRunFlags();

        Debug.Log($"[RunStateManager] New run started. Seed={RunSeed}");
    }

    /// <summary>End the current run (wipe or voluntary retreat).</summary>
    public void EndRun()
    {
        IsRunActive   = false;
        CurrentRoomId = null;
        FlagManager.Instance?.ClearRunFlags();
        Debug.Log($"[RunStateManager] Run ended. Bloom earned: {BloomEarned}");
    }

    /// <summary>
    /// Call when the player returns to the Refuge after a run ends.
    /// Seeds CurrentBloom from BloomEarned so the Refuge can spend it.
    /// Also triggers RefugeManager to initialise if present.
    /// </summary>
    public void ReturnToRefuge()
    {
        CurrentBloom = BloomEarned;
        IsRunActive  = false;
        CurrentRoomId = null;
        FlagManager.Instance?.ClearRunFlags();

        // Notify RefugeManager so it seeds its local counter
        RefugeManager.Instance?.InitialiseFromRunState();

        Debug.Log($"[RunStateManager] Returned to Refuge. Bloom available: {CurrentBloom}");
    }

    // ── Room tracking ──────────────────────────────────────────────────────

    public void SetCurrentRoom(string roomNodeId)
    {
        CurrentRoomId = roomNodeId;
    }

    public void MarkRoomCompleted()
    {
        RoomsCompletedThisLayer++;
    }

    // ── Layer progression ──────────────────────────────────────────────────

    /// <summary>
    /// Advance to the next layer. Called by RoomManager after the boss is beaten.
    /// </summary>
    public void AdvanceLayer()
    {
        CurrentLayer++;
        RoomsCompletedThisLayer = 0;
        CurrentRoomId           = null;
        Debug.Log($"[RunStateManager] Advanced to layer {CurrentLayer}");
    }

    // ── Bloom ──────────────────────────────────────────────────────────────

    /// <summary>Earn Bloom during a run. Increments both BloomEarned and CurrentBloom.</summary>
    public void AddBloom(int amount)
    {
        if (amount <= 0) return;
        BloomEarned  += amount;
        CurrentBloom += amount;
        TotalBloomEver += amount;
    }

    /// <summary>
    /// Spend Bloom at the Refuge. Decrements CurrentBloom only —
    /// BloomEarned and TotalBloomEver are never reduced.
    /// RefugeManager calls this; do not call directly from UI.
    /// </summary>
    public void SpendBloom(int amount)
    {
        if (amount <= 0) return;
        CurrentBloom = Mathf.Max(0, CurrentBloom - amount);
    }

    // ── Seed helper ────────────────────────────────────────────────────────

    /// <summary>Get the generation seed for the current layer.</summary>
    public int GetCurrentLayerSeed()
    {
        return LayerGenerator.DeriveLayerSeed(RunSeed, CurrentLayer);
    }
}
