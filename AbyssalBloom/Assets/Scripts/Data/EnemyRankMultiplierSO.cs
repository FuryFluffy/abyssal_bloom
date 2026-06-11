using UnityEngine;

// Create ONE asset (Assets > Create > AbyssalBloom > Enemy Rank Multiplier).
// Fill in all four rank rows in the Inspector.
[CreateAssetMenu(fileName = "EnemyRankMultiplier", menuName = "AbyssalBloom/Enemy Rank Multiplier")]
public class EnemyRankMultiplierSO : ScriptableObject
{
    public enum Rank { Standard, Elite, MajorBoss, FinalBoss }

    [System.Serializable]
    public struct RankMultiplier
    {
        public Rank rank;
        [Tooltip("Multiplier applied to base HP")]
        public float hpMult;
        [Tooltip("Multiplier applied to base ATK and MAG")]
        public float atkMagMult;
        [Tooltip("Multiplier applied to base DEF and RES")]
        public float defResMult;
        [Tooltip("Flat bonus added to SPD after all multipliers")]
        public int spdBonus;
    }

    public RankMultiplier[] ranks;

    public RankMultiplier GetRank(Rank rank)
    {
        foreach (var r in ranks)
            if (r.rank == rank) return r;

        Debug.LogWarning($"[EnemyRankMultiplierSO] Rank '{rank}' not found. Returning default.");
        return new RankMultiplier { hpMult = 1f, atkMagMult = 1f, defResMult = 1f, spdBonus = 0 };
    }
}
