using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// MapUI — subscribes to RoomManager events and renders the DAG map.
//
// EDGE DRAWING DECISION:
//   Two options considered:
//   (A) GL lines in OnRenderObject — simple but renders outside the Canvas
//       layer stack, making depth/sorting with UI impossible.
//   (B) Thin UI Image panels rotated to span two points — pure uGUI,
//       correct Canvas depth, no extra camera setup.
//   → Chose (B).  Each edge is a 2px-tall Image stretched and rotated to
//     connect the centres of two node RectTransforms.
// ════════════════════════════════════════════════════════════════════════════

public class MapUI : MonoBehaviour
{
    // ── Inspector ───────────────────────────────────────────────────────────
    [Header("References")]
    public RoomManager roomManager;

    [Header("Map Panel — nodes spawn inside this")]
    public RectTransform mapPanel;

    [Header("Edge Container — behind nodes")]
    public RectTransform edgeContainer;

    [Header("Node Prefab (has MapNodeView, Image, Button, Text child)")]
    public GameObject nodePrefab;

    // ── Runtime ─────────────────────────────────────────────────────────────
    readonly Dictionary<string, MapNodeView> _views = new();
    readonly List<GameObject>                _edges = new();
    bool _bossUnlocked;

    // Node visual size
    const float NODE_W = 80f;
    const float NODE_H = 50f;
    const float EDGE_THICKNESS = 2f;

    // ════════════════════════════════════════════════════════════════════════
    // Enable / Disable — subscribe / unsubscribe
    // ════════════════════════════════════════════════════════════════════════

    void OnEnable()
    {
        if (roomManager == null) return;
        roomManager.OnMapGenerated    += HandleMapGenerated;
        roomManager.OnRoomEntered     += HandleRoomEntered;
        roomManager.OnRoomCompleted   += HandleRoomCompleted;
        roomManager.OnBossRoomUnlocked+= HandleBossRoomUnlocked;
    }

    void OnDisable()
    {
        if (roomManager == null) return;
        roomManager.OnMapGenerated    -= HandleMapGenerated;
        roomManager.OnRoomEntered     -= HandleRoomEntered;
        roomManager.OnRoomCompleted   -= HandleRoomCompleted;
        roomManager.OnBossRoomUnlocked-= HandleBossRoomUnlocked;
    }

    // ════════════════════════════════════════════════════════════════════════
    // Event handlers
    // ════════════════════════════════════════════════════════════════════════

    void HandleMapGenerated(List<RoomNode> allNodes, List<RoomNode> startNodes)
    {
        ClearMap();
        _bossUnlocked = false;

        // ── Layout ───────────────────────────────────────────────────────
        // Group nodes by depth column.
        var columns = new Dictionary<int, List<RoomNode>>();
        foreach (var node in allNodes)
        {
            if (!columns.TryGetValue(node.depth, out var col))
            {
                col = new List<RoomNode>();
                columns[node.depth] = col;
            }
            col.Add(node);
        }

        int maxDepth = 0;
        foreach (var d in columns.Keys)
            if (d > maxDepth) maxDepth = d;

        int colCount = maxDepth + 1; // inclusive

        // Panel dimensions
        float panelW = mapPanel.rect.width;
        float panelH = mapPanel.rect.height;

        // Column spacing — evenly spread, with half-column padding each side.
        float colStep = colCount > 1 ? panelW / colCount : panelW;

        // ── Instantiate nodes ─────────────────────────────────────────────
        foreach (var kvp in columns)
        {
            int depth     = kvp.Key;
            var col       = kvp.Value;
            int rowCount  = col.Count;

            // X centre of this column
            float x = (depth + 0.5f) * colStep - panelW * 0.5f;

            // Y positions: evenly spaced within panel height
            float rowStep = rowCount > 1 ? panelH / rowCount : panelH;

            for (int i = 0; i < rowCount; i++)
            {
                float y = (i + 0.5f) * rowStep - panelH * 0.5f;
                // Flip so row 0 is near top
                y = -y;

                RoomNode node = col[i];
                SpawnNode(node, new Vector2(x, y));
            }
        }

        // ── Draw edges ────────────────────────────────────────────────────
        foreach (var node in allNodes)
        {
            foreach (var exit in node.exits)
            {
                if (_views.TryGetValue(node.nodeId, out var fromView) &&
                    _views.TryGetValue(exit.nodeId,  out var toView))
                {
                    DrawEdge(fromView.GetComponent<RectTransform>(),
                             toView.GetComponent<RectTransform>());
                }
            }
        }
    }

    void HandleRoomEntered(RoomNode room)
    {
        RefreshAll();
    }

    void HandleRoomCompleted(RoomNode room)
    {
        if (_views.TryGetValue(room.nodeId, out var view))
            view.Refresh(room, false, _bossUnlocked);

        // Refresh exits — they may now be accessible.
        foreach (var exit in room.exits)
        {
            if (_views.TryGetValue(exit.nodeId, out var exitView))
                exitView.Refresh(exit, false, _bossUnlocked);
        }
    }

    void HandleBossRoomUnlocked()
    {
        _bossUnlocked = true;
        RefreshAll();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Node click
    // ════════════════════════════════════════════════════════════════════════

    void OnNodeClicked(RoomNode node)
    {
        // Guard — button interactability handles most cases, but double-check.
        if (!node.isAccessible || node.isCompleted) return;
        if (node.roomType == RoomType.Boss && !_bossUnlocked) return;

        roomManager.MoveToRoom(node);
    }

    // ════════════════════════════════════════════════════════════════════════
    // Helpers
    // ════════════════════════════════════════════════════════════════════════

    void SpawnNode(RoomNode node, Vector2 anchoredPos)
    {
        var go  = Instantiate(nodePrefab, mapPanel);
        var rt  = go.GetComponent<RectTransform>();
        var view= go.GetComponent<MapNodeView>();

        rt.sizeDelta       = new Vector2(NODE_W, NODE_H);
        rt.anchorMin       = new Vector2(0.5f, 0.5f);
        rt.anchorMax       = new Vector2(0.5f, 0.5f);
        rt.pivot           = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition= anchoredPos;

        view.Initialise(node, OnNodeClicked);
        _views[node.nodeId] = view;
    }

    // ── Edge drawing (rotated UI Image panels) ─────────────────────────────

    void DrawEdge(RectTransform from, RectTransform to)
    {
        Vector2 a = from.anchoredPosition;
        Vector2 b = to.anchoredPosition;

        Vector2 dir    = b - a;
        float   length = dir.magnitude;
        float   angle  = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var go = new GameObject("Edge", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(edgeContainer, false);

        var img = go.GetComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.25f);  // dim white

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0.5f, 0.5f);
        rt.anchorMax        = new Vector2(0.5f, 0.5f);
        rt.pivot            = new Vector2(0f, 0.5f);   // pivot at left = origin point
        rt.sizeDelta        = new Vector2(length, EDGE_THICKNESS);
        rt.anchoredPosition = a;
        rt.localRotation    = Quaternion.Euler(0f, 0f, angle);

        _edges.Add(go);
    }

    // ── Refresh all views ─────────────────────────────────────────────────

    void RefreshAll()
    {
        RoomNode current = roomManager.CurrentRoom;
        foreach (var kvp in _views)
        {
            bool isCurrent = (current != null && kvp.Key == current.nodeId);
            kvp.Value.Refresh(kvp.Value.Node, isCurrent, _bossUnlocked);
        }
    }

    // ── Teardown ──────────────────────────────────────────────────────────

    void ClearMap()
    {
        foreach (var kvp in _views)
            if (kvp.Value != null)
                Destroy(kvp.Value.gameObject);
        _views.Clear();

        foreach (var edge in _edges)
            if (edge != null)
                Destroy(edge);
        _edges.Clear();
    }
}
