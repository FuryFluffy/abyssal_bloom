using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RoomType enum — shared by RoomNode, LayerGenerationProfileSO, RoomManager
// ════════════════════════════════════════════════════════════════════════════

public enum RoomType
{
    Battle,
    Elite,
    Event,
    LoreDiscovery,
    RiskReward,
    FalseRest,
    Boss,
    KeyMechanism
}

// ════════════════════════════════════════════════════════════════════════════
// RoomNode — one room in the generated layer map
// ════════════════════════════════════════════════════════════════════════════
// Plain C# class (not a MonoBehaviour, not a ScriptableObject).
// Created by LayerGenerator, owned by RoomManager at runtime.
//
// DECISION (flagged #2): RoomNode stores resolved EnemyDataSO[] directly,
// not an index into the pool.  Once the map is generated the pool SO is
// not needed again.
//
// DECISION (flagged #3): False Rest rooms carry apparentRoomType = Event
// (or whatever the designer sets).  The player sees apparentRoomType in
// the one-step lookahead; the true roomType is revealed on entry.
//
// DECISION (hotspot system): hotspotAnchors/fixedHotspots are populated by
// LayerGenerator from a room template SO.  GenerateHotspots() in RoomManager
// fills spawnedHotspots at entry time using a seeded RNG.
// eventData is retained for backward compatibility but is deprecated —
// new rooms use spawnedHotspots instead.
// ════════════════════════════════════════════════════════════════════════════

public class RoomNode
{
    // ── Identity ───────────────────────────────────────────────────────────

    /// <summary>Unique id within this layer map, e.g. "room_03".</summary>
    public string nodeId;

    /// <summary>Depth column in the DAG (0 = first rooms after entrance).</summary>
    public int depth;

    // ── Room Type ──────────────────────────────────────────────────────────

    /// <summary>The true room type.  Drives actual behaviour on entry.</summary>
    public RoomType roomType;

    /// <summary>
    /// What the player sees in the lookahead.  Normally equal to roomType.
    /// For FalseRest, this is set to a decoy type (e.g. Event).
    /// </summary>
    public RoomType apparentRoomType;

    // ── Encounter data (Battle / Elite / Boss) ─────────────────────────────

    /// <summary>
    /// Resolved enemy references for this room.  Null for non-combat rooms.
    /// </summary>
    public EnemyDataSO[] encounterEnemies;

    /// <summary>
    /// Ability lists for encounterEnemies (parallel array).
    /// </summary>
    public EncounterPoolSO.EnemyAbilityList[] encounterAbilities;

    // ── Event data (Event / LoreDiscovery / RiskReward / FalseRest) ────────

    /// <summary>
    /// DEPRECATED — kept for backward compatibility with old event rooms.
    /// New rooms use spawnedHotspots instead.  Do not populate for new content.
    /// </summary>
    public RoomEventSO eventData;

    // ── Hotspot system (replaces eventData for new rooms) ──────────────────

    /// <summary>
    /// Anchor definitions from the room template: position on screen + which pool
    /// to draw from.  Populated by LayerGenerator when the room is created.
    /// Null or empty = no anchor-based hotspots (use fixedHotspots only).
    /// </summary>
    [System.NonSerialized]
    public HotspotAnchor[] hotspotAnchors;

    /// <summary>
    /// Hotspots randomly chosen from each anchor's pool when the player
    /// enters the room.  Populated by RoomManager.GenerateHotspots().
    /// Combined result of all anchor pools + fixedHotspots.
    /// </summary>
    [System.NonSerialized]
    public RoomHotspotSO[] spawnedHotspots;

    /// <summary>
    /// Hotspots that always appear in this room with no randomization
    /// (typically the exit door).  Populated by LayerGenerator.
    /// </summary>
    [System.NonSerialized]
    public RoomHotspotSO[] fixedHotspots;

    // ── HotspotAnchor inner class ──────────────────────────────────────────

    /// <summary>
    /// One anchor slot on the room background.
    /// Defines WHERE a hotspot appears and WHICH pool it draws from.
    /// </summary>
    public class HotspotAnchor
    {
        /// <summary>Designer-assigned slot name, e.g. "item_slot_1".</summary>
        public string anchorId;

        /// <summary>
        /// Anchored position in screen/canvas space for UI placement.
        /// Set by LayerGenerator from the room template SO.
        /// </summary>
        public Vector2 screenPosition;

        /// <summary>
        /// The pool to pick from for this anchor.
        /// Null = skip this anchor during generation.
        /// </summary>
        public RoomHotspotPoolSO pool;
    }

    // ── Graph connections ──────────────────────────────────────────────────

    /// <summary>Rooms the player can move TO from here (forward edges).</summary>
    public List<RoomNode> exits = new();

    /// <summary>Rooms that lead TO this room (back edges, for pathfinding).</summary>
    public List<RoomNode> entrances = new();

    // ── Runtime state ──────────────────────────────────────────────────────

    /// <summary>True once the player has completed this room.</summary>
    public bool isCompleted;

    /// <summary>True if this room is currently accessible to the player.</summary>
    public bool isAccessible;

    /// <summary>True once revealed (entered or scouted).  False Rest rooms
    /// start unrevealed — apparentRoomType is shown instead.</summary>
    public bool isRevealed;

    // ── Helpers ─────────────────────────────────────────────────────────────

    public bool IsCombatRoom =>
        roomType == RoomType.Battle ||
        roomType == RoomType.Elite  ||
        roomType == RoomType.Boss;

    /// <summary>True if this room uses the hotspot system (has anchors or fixed hotspots).</summary>
    public bool UsesHotspotSystem =>
        (hotspotAnchors != null && hotspotAnchors.Length > 0) ||
        (fixedHotspots  != null && fixedHotspots.Length > 0);

    public override string ToString() =>
        $"[{nodeId} depth={depth} type={roomType}" +
        (roomType != apparentRoomType ? $" (appears={apparentRoomType})" : "") +
        (isCompleted ? " ✓" : "") + "]";
}
