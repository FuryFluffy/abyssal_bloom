using System;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RoomTemplateSO — background art + hotspot anchor layout for one room.
// ════════════════════════════════════════════════════════════════════════════
// Create via: Assets → Create → AbyssalBloom → Room Template
//
// One asset per named room (e.g. WineCellar, ServantDormitory, BloodNunChapel).
// LayerGenerator reads this SO when building RoomNodes — it copies anchor
// definitions into RoomNode.hotspotAnchors and fixed hotspots into
// RoomNode.fixedHotspots.
//
// HotspotDisplayUI reads anchor positions from the RoomNode at runtime
// and renders clickable buttons at those screen positions over the background.
//
// ── Anchor positions ──────────────────────────────────────────────────────
// screenPosition is in canvas/viewport space (0,0 = bottom-left, 1,1 = top-right
// of the room background rect). HotspotDisplayUI converts to pixel position
// using the background RectTransform.
//
// ── Room type association ─────────────────────────────────────────────────
// A template is associated with one or more RoomTypes via roomTypes[].
// LayerGenerator picks from the matching template pool when assigning
// a template to a newly-typed node.
// ════════════════════════════════════════════════════════════════════════════

[CreateAssetMenu(fileName = "NewRoomTemplate", menuName = "AbyssalBloom/Room Template")]
public class RoomTemplateSO : ScriptableObject
{
    // ── Identity ───────────────────────────────────────────────────────────

    [Header("Identity")]
    [Tooltip("Unique ID, e.g. 'wine_cellar', 'blood_nun_chapel'.")]
    public string templateId;

    [Tooltip("Display name shown in debug UI.")]
    public string displayName;

    [Tooltip("Which room types this template can be assigned to.")]
    public RoomType[] roomTypes;

    // ── Visuals ────────────────────────────────────────────────────────────

    [Header("Background")]
    [Tooltip("Full-screen background sprite for this room.")]
    public Sprite backgroundSprite;

    // ── Anchor Layout ──────────────────────────────────────────────────────

    [Header("Hotspot Anchors")]
    [Tooltip(
        "One entry per interactive slot in this room. " +
        "Position is in normalised canvas space (0,0 = bottom-left, 1,1 = top-right). " +
        "Each anchor references one RoomHotspotPoolSO — LayerGenerator draws " +
        "spawnedHotspots from that pool when building the map.")]
    public AnchorDefinition[] anchors;

    [Header("Fixed Hotspots")]
    [Tooltip(
        "Hotspots that ALWAYS appear in this room with no randomisation. " +
        "Typically the exit door. Populated into RoomNode.fixedHotspots directly.")]
    public RoomHotspotSO[] fixedHotspots;

    // ── AnchorDefinition ──────────────────────────────────────────────────

    [Serializable]
    public class AnchorDefinition
    {
        [Tooltip("Designer-assigned slot name, e.g. 'item_slot_1', 'event_slot_1'.")]
        public string anchorId;

        [Tooltip(
            "Position in normalised canvas space (0,0 = bottom-left, 1,1 = top-right). " +
            "Set these once background art is imported and you can see where objects sit.")]
        public Vector2 screenPosition;

        [Tooltip(
            "Pool to draw from for this anchor. " +
            "Null = anchor is inactive (skipped during generation).")]
        public RoomHotspotPoolSO pool;

        [Tooltip(
            "Optional icon shown at this anchor position before the player interacts. " +
            "Null = use the hotspot's own hotspotSprite.")]
        public Sprite anchorIcon;
    }

    // ── Helper ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Converts this template's AnchorDefinitions into the RoomNode.HotspotAnchor
    /// format used at runtime. Called by LayerGenerator when building a RoomNode.
    /// </summary>
    public RoomNode.HotspotAnchor[] BuildAnchors()
    {
        if (anchors == null || anchors.Length == 0)
            return new RoomNode.HotspotAnchor[0];

        var result = new RoomNode.HotspotAnchor[anchors.Length];
        for (int i = 0; i < anchors.Length; i++)
        {
            result[i] = new RoomNode.HotspotAnchor
            {
                anchorId       = anchors[i].anchorId,
                screenPosition = anchors[i].screenPosition,
                pool           = anchors[i].pool,
            };
        }
        return result;
    }

    /// <summary>
    /// Returns true if this template supports the given room type.
    /// </summary>
    public bool SupportsRoomType(RoomType type)
    {
        if (roomTypes == null) return false;
        foreach (var t in roomTypes)
            if (t == type) return true;
        return false;
    }
}
