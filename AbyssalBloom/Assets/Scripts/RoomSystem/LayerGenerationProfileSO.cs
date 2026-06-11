using System;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// LayerGenerationProfileSO — generation parameters for one layer
// ════════════════════════════════════════════════════════════════════════════
// Create one per layer: Assets > Create > AbyssalBloom > Layer Generation Profile
//
// Controls how many rooms, which room types appear and how often,
// how many rooms must be cleared before the boss unlocks, and
// which encounter pool to draw enemies from.
// ════════════════════════════════════════════════════════════════════════════

[CreateAssetMenu(fileName = "NewLayerProfile", menuName = "AbyssalBloom/Layer Generation Profile")]
public class LayerGenerationProfileSO : ScriptableObject
{
    [Header("Layer Identity")]
    [Tooltip("Layer number (1–10).  Informational — used for debug logs.")]
    [Range(1, 10)]
    public int layerNumber = 1;

    // ── Map Shape ──────────────────────────────────────────────────────────

    [Header("Map Shape")]
    [Tooltip("Minimum total rooms (excluding boss).")]
    [Min(4)]
    public int minNodes = 8;

    [Tooltip("Maximum total rooms (excluding boss).")]
    [Min(4)]
    public int maxNodes = 12;

    [Tooltip("Number of starting paths from the entrance.  More = wider map.")]
    [Range(2, 4)]
    public int startingPaths = 2;

    [Tooltip("Minimum depth columns before the boss node.  "
           + "More depth = longer run, fewer rooms per column.")]
    [Range(3, 10)]
    public int minDepth = 4;

    // ── Boss Unlock ────────────────────────────────────────────────────────

    [Header("Boss Unlock")]
    [Tooltip("Player must complete at least this many rooms before the boss unlocks.")]
    [Min(1)]
    public int minRoomsBeforeBoss = 4;

    // ── Room Type Weights ──────────────────────────────────────────────────
    // Higher weight = more likely.  Zero = never appears on this layer.
    // Boss room is always placed as the final node — not weighted.

    [Header("Room Type Weights")]
    public int weightBattle      = 30;
    public int weightElite       = 10;
    public int weightEvent       = 15;
    public int weightLoreDiscovery = 5;
    public int weightRiskReward  = 10;
    public int weightFalseRest   = 5;
    public int weightKeyMechanism = 5;

    // ── Encounter Pool ─────────────────────────────────────────────────────

    [Header("Encounter Pool")]
    [Tooltip("Drag the EncounterPoolSO for this layer here.")]
    public EncounterPoolSO encounterPool;

    // ── Helpers ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns a weighted-random RoomType (never Boss — that's placed manually).
    /// </summary>
    public RoomType PickRoomType(System.Random rng)
    {
        int total = weightBattle + weightElite + weightEvent
                  + weightLoreDiscovery + weightRiskReward
                  + weightFalseRest + weightKeyMechanism;

        if (total <= 0) return RoomType.Battle; // safety fallback

        int roll = rng.Next(0, total);
        int running = 0;

        running += weightBattle;       if (roll < running) return RoomType.Battle;
        running += weightElite;        if (roll < running) return RoomType.Elite;
        running += weightEvent;        if (roll < running) return RoomType.Event;
        running += weightLoreDiscovery;if (roll < running) return RoomType.LoreDiscovery;
        running += weightRiskReward;   if (roll < running) return RoomType.RiskReward;
        running += weightFalseRest;    if (roll < running) return RoomType.FalseRest;
        running += weightKeyMechanism; if (roll < running) return RoomType.KeyMechanism;

        return RoomType.Battle;
    }
}
