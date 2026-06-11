using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// MapNodeView — attached to each instantiated node GameObject on the map.
// Hierarchy expected:
//   [NodeRoot]  — MapNodeView, Button, Image  (the background panel)
//     [Label]   — Text
//     [Tick]    — Image  (white ✓, hidden until completed)
//     [Lock]    — Image  (lock icon tint, hidden once boss unlocked)
// ════════════════════════════════════════════════════════════════════════════

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class MapNodeView : MonoBehaviour
{
    // ── Inspector ───────────────────────────────────────────────────────────
    public Text  label;
    public Image tickOverlay;   // shown when completed
    public Image lockOverlay;   // shown on boss node when locked

    // ── Runtime ─────────────────────────────────────────────────────────────
    public RoomNode Node { get; private set; }

    Image  _bg;
    Button _btn;

    // ── Colors ──────────────────────────────────────────────────────────────
    static readonly Color ColBattle       = HexColor("C0392B");
    static readonly Color ColElite        = HexColor("8E44AD");
    static readonly Color ColEvent        = HexColor("2980B9");
    static readonly Color ColLoreDiscovery= HexColor("27AE60");
    static readonly Color ColRiskReward   = HexColor("E67E22");
    static readonly Color ColBossLocked   = HexColor("2C3E50");
    static readonly Color ColBossUnlocked = HexColor("4A6FA5");   // lightened
    static readonly Color ColKeyMechanism = HexColor("7F8C8D");
    static readonly Color ColCurrentBorder= HexColor("F1C40F");   // gold

    void Awake()
    {
        _bg  = GetComponent<Image>();
        _btn = GetComponent<Button>();
    }

    // ── Public API ──────────────────────────────────────────────────────────

    /// <summary>
    /// Initialise the node view.  Call once after instantiation.
    /// </summary>
    public void Initialise(RoomNode node, System.Action<RoomNode> onClick)
    {
        Node = node;
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(() => onClick?.Invoke(Node));
        Refresh(node, false, false);
    }

    /// <summary>
    /// Update all visual state.  Call whenever RoomManager state changes.
    /// </summary>
    /// <param name="node">The RoomNode this view represents.</param>
    /// <param name="isCurrent">True if the player is currently in this room.</param>
    /// <param name="bossUnlocked">True after OnBossRoomUnlocked has fired.</param>
    public void Refresh(RoomNode node, bool isCurrent, bool bossUnlocked = false)
    {
        Node = node;

        // ── Determine display type ───────────────────────────────────────
        // One-step lookahead: apparentRoomType used unless already revealed.
        RoomType displayType = node.isRevealed ? node.roomType : node.apparentRoomType;

        // ── Label ────────────────────────────────────────────────────────
        if (label != null)
            label.text = RoomTypeLabel(displayType);

        // ── Background colour ────────────────────────────────────────────
        bool isLockedBoss = (node.roomType == RoomType.Boss && !bossUnlocked);
        Color baseColor   = NodeColor(displayType, isLockedBoss);

        // ── Alpha ────────────────────────────────────────────────────────
        float alpha;
        if (node.isCompleted)         alpha = 0.50f;
        else if (!node.isAccessible)  alpha = 0.30f;
        else                          alpha = 1.00f;

        _bg.color = WithAlpha(baseColor, alpha);

        // ── Current-room gold tint ───────────────────────────────────────
        if (isCurrent)
            _bg.color = WithAlpha(Color.Lerp(baseColor, ColCurrentBorder, 0.45f), 1f);

        // ── Overlays ─────────────────────────────────────────────────────
        if (tickOverlay != null)
            tickOverlay.gameObject.SetActive(node.isCompleted);

        if (lockOverlay != null)
            lockOverlay.gameObject.SetActive(isLockedBoss);

        // ── Button interactability ───────────────────────────────────────
        // Clickable only when accessible AND not completed AND not locked boss.
        _btn.interactable = node.isAccessible && !node.isCompleted && !isLockedBoss;
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    static Color NodeColor(RoomType type, bool isLockedBoss)
    {
        if (isLockedBoss) return ColBossLocked;
        return type switch
        {
            RoomType.Battle        => ColBattle,
            RoomType.Elite         => ColElite,
            RoomType.Event         => ColEvent,
            RoomType.LoreDiscovery => ColLoreDiscovery,
            RoomType.RiskReward    => ColRiskReward,
            RoomType.FalseRest     => ColEvent,          // disguised as Event
            RoomType.Boss          => ColBossUnlocked,
            RoomType.KeyMechanism  => ColKeyMechanism,
            _                      => Color.white,
        };
    }

    static string RoomTypeLabel(RoomType type) => type switch
    {
        RoomType.Battle        => "Battle",
        RoomType.Elite         => "Elite",
        RoomType.Event         => "Event",
        RoomType.LoreDiscovery => "Lore",
        RoomType.RiskReward    => "Risk",
        RoomType.FalseRest     => "Event",   // shown as Event until revealed
        RoomType.Boss          => "Boss",
        RoomType.KeyMechanism  => "Key",
        _                      => "?",
    };

    static Color WithAlpha(Color c, float a) => new Color(c.r, c.g, c.b, a);

    static Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }
}
