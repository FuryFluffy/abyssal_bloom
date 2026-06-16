using System;
using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// LayerGenerator — builds a room map (DAG) from a LayerGenerationProfileSO
// ════════════════════════════════════════════════════════════════════════════
// Pure static class.  No MonoBehaviour, no state between calls.
// Deterministic given a seed — call with the same seed and profile to get
// the exact same map.
//
// DECISION (flagged #1): The map is a DAG (directed acyclic graph), not a
// tree.  Nodes are arranged in depth columns; edges only go forward
// (depth d → depth d+1).  Two paths can converge on the same node.
// No backward edges, so no cycles.
//
// DECISION (flagged #4): Seed is derived per-layer from the run seed.
// RunStateManager passes Hash(runSeed, layerNumber) as the seed here.
// ════════════════════════════════════════════════════════════════════════════

public static class LayerGenerator
{
    // ── Public entry point ─────────────────────────────────────────────────

    /// <summary>
    /// Generate a complete layer map.
    /// Returns (allNodes, startNodes) — startNodes are depth-0 entry points.
    /// </summary>
    public static (List<RoomNode> allNodes, List<RoomNode> startNodes)
        Generate(LayerGenerationProfileSO profile, int seed)
    {
        var rng = new System.Random(seed);

        // 1. Decide dimensions
        int totalNonBoss = rng.Next(profile.minNodes, profile.maxNodes + 1);
        int depthCount   = Mathf.Clamp(profile.minDepth,
                                        3,
                                        totalNonBoss - 1); // need at least 1 node per depth on average

        // 2. Distribute nodes across depth columns
        int[] nodesPerDepth = DistributeNodes(totalNonBoss, depthCount,
                                               profile.startingPaths, rng);

        // 3. Create all RoomNodes (excluding boss)
        var columns = new List<RoomNode>[depthCount];
        var allNodes = new List<RoomNode>(totalNonBoss + 1); // +1 for boss
        int nodeCounter = 0;

        for (int d = 0; d < depthCount; d++)
        {
            columns[d] = new List<RoomNode>();
            for (int i = 0; i < nodesPerDepth[d]; i++)
            {
                var node = new RoomNode
                {
                    nodeId = $"room_{nodeCounter:D2}",
                    depth  = d
                };
                columns[d].Add(node);
                allNodes.Add(node);
                nodeCounter++;
            }
        }

        // 4. Wire forward edges between adjacent columns
        for (int d = 0; d < depthCount - 1; d++)
        {
            WireColumns(columns[d], columns[d + 1], rng);
        }

        // 5. Create boss node and connect from the last column
        var bossNode = new RoomNode
        {
            nodeId          = $"room_{nodeCounter:D2}",
            depth           = depthCount,
            roomType        = RoomType.Boss,
            apparentRoomType = RoomType.Boss,
            isRevealed      = true // boss is always shown on the map
        };
        allNodes.Add(bossNode);

        foreach (var lastNode in columns[depthCount - 1])
        {
            lastNode.exits.Add(bossNode);
            bossNode.entrances.Add(lastNode);
        }

        // 6. Assign room types (non-boss nodes)
        AssignRoomTypes(allNodes, profile, rng);

        // 7. Guarantee at least 1 Battle per branch from each start node
        var startNodes = columns[0];
        GuaranteeBattles(startNodes, rng);

        // 8. Populate encounter data for combat rooms
        PopulateEncounters(allNodes, profile.encounterPool, rng);

        // 8b. Assign room templates to non-combat rooms
        // Template defines: background sprite + anchor positions + fixed hotspots.
        // Must run before PopulateHotspots so anchors are set when pools are drawn.
        AssignRoomTemplates(allNodes, profile, rng);

        // 9. Populate hotspots for non-combat rooms
        // Each room's spawnedHotspots are rolled once here and never changed again.
        // The same seed always produces the same hotspots for the same room instance.
        // RoomManager.EnterRoom() reads spawnedHotspots directly — it does NOT regenerate.
        PopulateHotspots(allNodes, seed);

        // 10. Mark start nodes as accessible
        foreach (var s in startNodes)
            s.isAccessible = true;

        return (allNodes, startNodes);
    }

    // ── Node distribution ──────────────────────────────────────────────────
    // Ensures depth 0 has exactly startingPaths nodes and every other depth
    // has at least 1 node.

    private static int[] DistributeNodes(int total, int depthCount,
                                          int startingPaths, System.Random rng)
    {
        int[] counts = new int[depthCount];

        // Pin first column
        counts[0] = Mathf.Min(startingPaths, total - (depthCount - 1));

        // Give every other column at least 1
        int remaining = total - counts[0];
        for (int d = 1; d < depthCount; d++)
        {
            counts[d] = 1;
            remaining--;
        }

        // Spread leftover randomly across columns 1+
        while (remaining > 0)
        {
            int d = rng.Next(1, depthCount);
            counts[d]++;
            remaining--;
        }

        return counts;
    }

    // ── Column wiring ──────────────────────────────────────────────────────
    // Every node in the current column gets 1–3 exits in the next column.
    // Every node in the next column must have at least 1 entrance.

    private static void WireColumns(List<RoomNode> current, List<RoomNode> next,
                                     System.Random rng)
    {
        // Phase 1: give every current node at least 1 exit
        foreach (var node in current)
        {
            int exitCount = Mathf.Min(rng.Next(1, 4), next.Count); // 1–3
            var chosen = PickRandom(next, exitCount, rng);
            foreach (var target in chosen)
            {
                if (!node.exits.Contains(target))
                {
                    node.exits.Add(target);
                    target.entrances.Add(node);
                }
            }
        }

        // Phase 2: ensure every next node has at least 1 entrance
        foreach (var target in next)
        {
            if (target.entrances.Count == 0)
            {
                var source = current[rng.Next(current.Count)];
                source.exits.Add(target);
                target.entrances.Add(source);
            }
        }
    }

    // ── Room type assignment ───────────────────────────────────────────────

    private static void AssignRoomTypes(List<RoomNode> allNodes,
                                         LayerGenerationProfileSO profile,
                                         System.Random rng)
    {
        foreach (var node in allNodes)
        {
            if (node.roomType == RoomType.Boss) continue; // already set

            node.roomType = profile.PickRoomType(rng);

            // False Rest: set apparent type to Event (player sees "Event")
            if (node.roomType == RoomType.FalseRest)
            {
                node.apparentRoomType = RoomType.Event;
                node.isRevealed = false;
            }
            else
            {
                node.apparentRoomType = node.roomType;
                node.isRevealed = true;
            }
        }
    }

    // ── Battle guarantee ───────────────────────────────────────────────────
    // Walk one forward path from each start node.  If the path has no Battle
    // room, convert the first non-special room to Battle.

    private static void GuaranteeBattles(List<RoomNode> startNodes, System.Random rng)
    {
        foreach (var start in startNodes)
        {
            // Walk a random path to the boss
            var path = new List<RoomNode>();
            var current = start;
            while (current != null && current.roomType != RoomType.Boss)
            {
                path.Add(current);
                if (current.exits.Count > 0)
                    current = current.exits[rng.Next(current.exits.Count)];
                else
                    break;
            }

            bool hasBattle = false;
            foreach (var node in path)
            {
                if (node.roomType == RoomType.Battle || node.roomType == RoomType.Elite)
                {
                    hasBattle = true;
                    break;
                }
            }

            if (!hasBattle && path.Count > 0)
            {
                // Convert the first convertible node
                foreach (var node in path)
                {
                    if (node.roomType != RoomType.Boss &&
                        node.roomType != RoomType.KeyMechanism)
                    {
                        node.roomType = RoomType.Battle;
                        node.apparentRoomType = RoomType.Battle;
                        node.isRevealed = true;
                        break;
                    }
                }
            }
        }
    }

    // ── Room template assignment ──────────────────────────────────────────
    //
    // Assigns a RoomTemplateSO to each non-combat room from the profile's
    // roomTemplates array. Copies anchor definitions and fixed hotspots
    // directly into the RoomNode so PopulateHotspots can read them.
    // If no matching template exists the node gets no anchors (hotspots
    // will not spawn — add a matching template to the profile to fix).

    private static void AssignRoomTemplates(List<RoomNode> allNodes,
                                             LayerGenerationProfileSO profile,
                                             System.Random rng)
    {
        if (profile.roomTemplates == null || profile.roomTemplates.Length == 0)
        {
            Debug.LogWarning("[LayerGenerator] No room templates in profile — non-combat rooms will have no hotspots.");
            return;
        }

        foreach (var node in allNodes)
        {
            if (node.IsCombatRoom) continue;

            var template = profile.PickTemplate(node.roomType, rng);
            if (template == null)
            {
                Debug.LogWarning($"[LayerGenerator] No template for RoomType.{node.roomType} in layer {profile.layerNumber}. Node {node.nodeId} will have no hotspots.");
                continue;
            }

            node.hotspotAnchors = template.BuildAnchors();
            node.fixedHotspots  = template.fixedHotspots;
        }
    }

    // ── Encounter population ───────────────────────────────────────────────

    private static void PopulateEncounters(List<RoomNode> allNodes,
                                            EncounterPoolSO pool,
                                            System.Random rng)
    {
        if (pool == null) return;

        foreach (var node in allNodes)
        {
            EncounterPoolSO.EnemyGroup group = null;

            switch (node.roomType)
            {
                case RoomType.Battle:
                    group = pool.PickStandard(rng);
                    break;
                case RoomType.Elite:
                    group = pool.PickElite(rng);
                    break;
                case RoomType.Boss:
                    group = pool.PickBoss(rng);
                    break;
            }

            if (group != null)
            {
                node.encounterEnemies   = group.enemies;
                node.encounterAbilities = group.enemyAbilities;
            }
        }
    }

    // ── Utility ────────────────────────────────────────────────────────────

    /// <summary>Pick up to count distinct random elements from the list.</summary>
    private static List<T> PickRandom<T>(List<T> source, int count, System.Random rng)
    {
        var result = new List<T>(count);
        var indices = new List<int>(source.Count);
        for (int i = 0; i < source.Count; i++) indices.Add(i);

        int picks = Mathf.Min(count, source.Count);
        for (int i = 0; i < picks; i++)
        {
            int idx = rng.Next(indices.Count);
            result.Add(source[indices[idx]]);
            indices.RemoveAt(idx);
        }

        return result;
    }

    // ── Hotspot population ─────────────────────────────────────────────────
    // Mirrors PopulateEncounters: called once during Generate(), writes
    // directly into RoomNode.spawnedHotspots, never touched again.
    //
    // Seed per room = layerSeed XOR nodeId.GetHashCode()
    // This ensures each room has its own independent RNG sequence while
    // remaining fully deterministic from the layer seed alone.

    private static void PopulateHotspots(List<RoomNode> allNodes, int layerSeed)
    {
        foreach (var node in allNodes)
        {
            // Combat rooms and boss rooms don't use hotspots
            if (node.IsCombatRoom) continue;

            // No anchors and no fixed hotspots — nothing to do
            bool hasAnchors = node.hotspotAnchors != null && node.hotspotAnchors.Length > 0;
            bool hasFixed   = node.fixedHotspots  != null && node.fixedHotspots.Length  > 0;
            if (!hasAnchors && !hasFixed) continue;

            // Per-room seed: XOR layer seed with nodeId hashcode
            int roomSeed = layerSeed ^ (node.nodeId?.GetHashCode() ?? 0);
            var rng = new System.Random(roomSeed);

            var generated = new List<RoomHotspotSO>();

            // Fixed hotspots always appear first (typically the exit door)
            if (hasFixed)
                generated.AddRange(node.fixedHotspots);

            // Draw from each anchor's pool
            if (hasAnchors)
            {
                foreach (var anchor in node.hotspotAnchors)
                {
                    if (anchor?.pool == null) continue;
                    var picked = PickFromHotspotPool(anchor.pool, rng);
                    generated.AddRange(picked);
                }
            }

            node.spawnedHotspots = generated.ToArray();
        }
    }

    /// <summary>
    /// Fisher-Yates shuffle then take the first pickCount entries.
    /// Respects pool.allowDuplicates — if false, clamps to pool size.
    /// </summary>
    private static RoomHotspotSO[] PickFromHotspotPool(RoomHotspotPoolSO pool, System.Random rng)
    {
        if (pool.hotspotVariants == null || pool.hotspotVariants.Length == 0)
            return Array.Empty<RoomHotspotSO>();

        // Shallow copy to shuffle without mutating the SO asset
        var shuffled = new RoomHotspotSO[pool.hotspotVariants.Length];
        Array.Copy(pool.hotspotVariants, shuffled, pool.hotspotVariants.Length);

        // Fisher-Yates
        for (int i = shuffled.Length - 1; i > 0; i--)
        {
            int j    = rng.Next(i + 1);
            var temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        int count = pool.allowDuplicates
            ? pool.pickCount
            : Mathf.Min(pool.pickCount, shuffled.Length);

        var result = new RoomHotspotSO[count];
        Array.Copy(shuffled, result, count);
        return result;
    }

    // ── Seed derivation helper ─────────────────────────────────────────────

    /// <summary>
    /// Derive a per-layer seed from the run seed.
    /// Uses a simple hash so each layer gets a different but deterministic seed.
    /// </summary>
    public static int DeriveLayerSeed(int runSeed, int layerNumber)
    {
        // Knuth multiplicative hash variant — fast, good distribution
        unchecked
        {
            int h = runSeed;
            h = h * (int)2654435761 + layerNumber * (int)2246822519;
            h ^= h >> 16;
            return h;
        }
    }
}
