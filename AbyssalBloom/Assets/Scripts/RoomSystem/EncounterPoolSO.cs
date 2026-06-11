using System;
using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// EncounterPoolSO — weighted enemy group pools for one layer
// ════════════════════════════════════════════════════════════════════════════
// Create one per layer: Assets > Create > AbyssalBloom > Encounter Pool
//
// Three pools — standard (Battle rooms), elite (Elite rooms), boss (Boss room).
// Each pool is a list of EnemyGroups, each with a weight for random selection.
//
// DECISION: Each EnemyGroup holds direct EnemyDataSO refs + a parallel array
// of ability lists.  At encounter time, RoomManager reads these and builds
// RuntimeCharacterStates the same way EncounterBuilder does today.
// ════════════════════════════════════════════════════════════════════════════

[CreateAssetMenu(fileName = "NewEncounterPool", menuName = "AbyssalBloom/Encounter Pool")]
public class EncounterPoolSO : ScriptableObject
{
    // ── Enemy Group ────────────────────────────────────────────────────────
    // One possible encounter.  Weight controls how often it's picked.

    [Serializable]
    public class EnemyGroup
    {
        [Tooltip("Human-readable label, e.g. '2× Hollow Servant'")]
        public string label;

        [Tooltip("Higher weight = more likely to be picked.  Weights are relative, not percentages.")]
        [Min(1)]
        public int weight = 10;

        [Tooltip("The enemies in this group.  Order doesn't matter for combat.")]
        public EnemyDataSO[] enemies;

        [Tooltip("One AbilityList per enemy, matching enemies[] order.")]
        public EnemyAbilityList[] enemyAbilities;
    }

    [Serializable]
    public class EnemyAbilityList
    {
        public CharacterAbilitySO[] abilities;
    }

    // ── Pools ──────────────────────────────────────────────────────────────

    [Header("Standard Pool (Battle rooms)")]
    public EnemyGroup[] standardPool;

    [Header("Elite Pool (Elite rooms)")]
    public EnemyGroup[] elitePool;

    [Header("Boss Pool (Boss room — typically one entry)")]
    public EnemyGroup[] bossPool;

    // ── Weighted random pick ───────────────────────────────────────────────

    /// <summary>
    /// Pick a random group from the given pool using weights.
    /// Returns null if the pool is empty.
    /// </summary>
    public EnemyGroup PickFromPool(EnemyGroup[] pool, System.Random rng)
    {
        if (pool == null || pool.Length == 0) return null;

        int totalWeight = 0;
        foreach (var g in pool) totalWeight += g.weight;

        int roll = rng.Next(0, totalWeight);
        int running = 0;
        foreach (var g in pool)
        {
            running += g.weight;
            if (roll < running) return g;
        }

        // Fallback (shouldn't happen unless weights are all 0)
        return pool[pool.Length - 1];
    }

    public EnemyGroup PickStandard(System.Random rng) => PickFromPool(standardPool, rng);
    public EnemyGroup PickElite(System.Random rng)    => PickFromPool(elitePool, rng);
    public EnemyGroup PickBoss(System.Random rng)     => PickFromPool(bossPool, rng);
}
