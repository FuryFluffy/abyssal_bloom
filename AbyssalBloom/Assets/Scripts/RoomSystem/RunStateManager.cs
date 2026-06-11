using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RunStateManager — roguelite run progress tracker
// ════════════════════════════════════════════════════════════════════════════
// Attach to the same "_Managers" GameObject as FlagManager.
// Singleton — persists across scenes via DontDestroyOnLoad.
//
// This is DISTINCT from FlagManager:
//   FlagManager  = string key/value store for narrative flags
//   RunStateManager = structured run progress (current layer, room, seed, etc.)
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

    /// <summary>Master seed for this run.  All layer seeds derive from this.</summary>
    public int RunSeed { get; private set; }

    /// <summary>Current layer number (1-based).</summary>
    public int CurrentLayer { get; private set; } = 1;

    /// <summary>Rooms completed on the current layer.</summary>
    public int RoomsCompletedThisLayer { get; private set; }

    /// <summary>Total Bloom (meta-currency) earned this run.</summary>
    public int BloomEarned { get; private set; }

    /// <summary>The node ID of the room the player is currently in (null if between rooms).</summary>
    public string CurrentRoomId { get; private set; }

    // ── Party (heroine RuntimeCharacterStates that persist between rooms) ──
    // Set once at run start, updated after each encounter.
    // Index 0 = active heroine, 1-2 = support.

    public List<RuntimeCharacterState> Party { get; private set; } = new(3);

    // ── Run lifecycle ──────────────────────────────────────────────────────

    /// <summary>
    /// Start a new run.  Call from the Refuge hub or a "New Run" button.
    /// </summary>
    /// <param name="party">The three heroines in play order.</param>
    /// <param name="seed">Optional explicit seed.  -1 = random.</param>
    public void StartNewRun(List<RuntimeCharacterState> party, int seed = -1)
    {
        IsRunActive  = true;
        RunSeed      = seed >= 0 ? seed : Random.Range(int.MinValue, int.MaxValue);
        CurrentLayer = 1;
        RoomsCompletedThisLayer = 0;
        BloomEarned  = 0;
        CurrentRoomId = null;

        Party.Clear();
        Party.AddRange(party);

        // Clear transient run flags
        FlagManager.Instance?.ClearRunFlags();

        Debug.Log($"[RunStateManager] New run started.  Seed={RunSeed}");
    }

    /// <summary>End the current run (wipe or voluntary retreat).</summary>
    public void EndRun()
    {
        IsRunActive = false;
        CurrentRoomId = null;
        FlagManager.Instance?.ClearRunFlags();
        Debug.Log($"[RunStateManager] Run ended.  Bloom earned: {BloomEarned}");
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
    /// Advance to the next layer.  Called by RoomManager after the boss is beaten.
    /// </summary>
    public void AdvanceLayer()
    {
        CurrentLayer++;
        RoomsCompletedThisLayer = 0;
        CurrentRoomId = null;
        Debug.Log($"[RunStateManager] Advanced to layer {CurrentLayer}");
    }

    // ── Bloom ──────────────────────────────────────────────────────────────

    public void AddBloom(int amount)
    {
        if (amount <= 0) return;
        BloomEarned += amount;
    }

    // ── Seed helper (wraps LayerGenerator's derivation) ────────────────────

    /// <summary>
    /// Get the generation seed for the current layer.
    /// </summary>
    public int GetCurrentLayerSeed()
    {
        return LayerGenerator.DeriveLayerSeed(RunSeed, CurrentLayer);
    }
}
