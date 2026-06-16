using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RoomHotspotPoolSO — pool of hotspot variants for randomized room spawning
// ════════════════════════════════════════════════════════════════════════════
// Each room anchor references one pool.  When the player enters the room,
// RoomManager.GenerateHotspots() draws from each anchor's pool using a
// seeded RNG so results are deterministic per run-seed.
//
// Example: Pool_WineCellar_Items holds 6 item hotspot SOs; pickCount = 2
// means 2 different items will appear in that room each run.
// ════════════════════════════════════════════════════════════════════════════

[CreateAssetMenu(menuName = "AbyssalBloom/Room Hotspot Pool")]
public class RoomHotspotPoolSO : ScriptableObject
{
    [Header("Pool Identity")]
    [Tooltip("Unique pool ID, e.g. 'wine_cellar_items'")]
    public string poolId;

    [Tooltip("The type of hotspots in this pool (all variants should match)")]
    public RoomHotspotSO.HotspotType type;

    [Header("Pool Contents")]
    [Tooltip("All possible hotspot variants to draw from")]
    public RoomHotspotSO[] hotspotVariants;

    [Header("Selection Rules")]
    [Tooltip("How many distinct hotspots to pick per room spawn")]
    [Min(1)]
    public int pickCount = 1;

    [Tooltip("Allow the same hotspot to be selected more than once? " +
             "Usually false — each variant appears at most once.")]
    public bool allowDuplicates = false;
}
