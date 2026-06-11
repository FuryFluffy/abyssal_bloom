using System.Collections.Generic;

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
    /// Reference to the event SO for narrative rooms.  Null for pure combat.
    /// </summary>
    public RoomEventSO eventData;

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

    public override string ToString() =>
        $"[{nodeId} depth={depth} type={roomType}" +
        (roomType != apparentRoomType ? $" (appears={apparentRoomType})" : "") +
        (isCompleted ? " ✓" : "") + "]";
}
