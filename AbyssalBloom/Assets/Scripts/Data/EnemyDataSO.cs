using UnityEngine;

// Create one asset per enemy type (Assets > Create > AbyssalBloom > Enemy Data).
// Wire up the three shared table SOs, set this enemy's layer/rank/role, and read
// calculated stats via the Final* properties at runtime.
[CreateAssetMenu(fileName = "NewEnemy", menuName = "AbyssalBloom/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Identity")]
    public string enemyId;          // e.g. "hollow_servant"
    public string displayName;
    [TextArea] public string description;

    // ── Tags (match your design doc strings) ──────────────────────────────
    [Tooltip("e.g. male-mindless, female-humanoid")]
    public string tag;

    // ── Shared stat tables ─────────────────────────────────────────────────
    [Header("Stat Tables (shared assets — drag the same SO for every enemy)")]
    public EnemyLayerTemplateSO    layerTemplate;
    public EnemyRankMultiplierSO   rankTable;
    public EnemyRoleModifierSO     roleTable;

    // ── This enemy's position in those tables ─────────────────────────────
    [Header("This Enemy's Classification")]
    [Range(1, 10)]
    public int layer = 1;
    public EnemyRankMultiplierSO.Rank rank = EnemyRankMultiplierSO.Rank.Standard;
    public EnemyRoleModifierSO.Role   role = EnemyRoleModifierSO.Role.Bruiser;

    // ── Optional flat overrides (leave 0 to use formula result) ───────────
    // DECISION POINT: You can either leave these out and always trust the formula,
    // or keep them as "pin this stat to an exact value" escape hatches.
    // The Layer 1 enemies in your design doc were hand-tuned, so overrides are useful.
    [Header("Stat Overrides (0 = use formula; non-zero = exact value)")]
    public int overrideHP;
    public int overrideMP;     // MP has no rank/role modifier — always set via override or formula below
    public int overrideATK;
    public int overrideMAG;
    public int overrideDEF;
    public int overrideRES;
    public int overrideSPD;

    // ── Calculated Properties ──────────────────────────────────────────────
    // Call these at encounter-spawn time to get final values.

    private EnemyLayerTemplateSO.LayerBaseStats Base =>
        layerTemplate != null ? layerTemplate.GetLayer(layer)
                              : default;

    private EnemyRankMultiplierSO.RankMultiplier Rank =>
        rankTable != null ? rankTable.GetRank(rank)
                          : new EnemyRankMultiplierSO.RankMultiplier
                            { hpMult = 1f, atkMagMult = 1f, defResMult = 1f };

    private EnemyRoleModifierSO.RoleModifier Role =>
        roleTable != null ? roleTable.GetRole(role)
                          : new EnemyRoleModifierSO.RoleModifier
                            { hpMult = 1f, atkMult = 1f, magMult = 1f,
                              defMult = 1f, resMult = 1f };

    // Formula: floor(base * rankMult * roleMult), or override if set.
    public int FinalHP  => overrideHP  > 0 ? overrideHP  : Mathf.FloorToInt(Base.hp  * Rank.hpMult     * Role.hpMult);
    public int FinalMP  => overrideMP  > 0 ? overrideMP  : Base.mp;   // MP has no rank/role multiplier in the doc
    public int FinalATK => overrideATK > 0 ? overrideATK : Mathf.FloorToInt(Base.atk * Rank.atkMagMult * Role.atkMult);
    public int FinalMAG => overrideMAG > 0 ? overrideMAG : Mathf.FloorToInt(Base.mag * Rank.atkMagMult * Role.magMult);
    public int FinalDEF => overrideDEF > 0 ? overrideDEF : Mathf.FloorToInt(Base.def * Rank.defResMult * Role.defMult);
    public int FinalRES => overrideRES > 0 ? overrideRES : Mathf.FloorToInt(Base.res * Rank.defResMult * Role.resMult);

    // SPD = layerBase + rankBonus + roleBonus (no multiplication)
    public int FinalSPD => overrideSPD > 0 ? overrideSPD : Base.spd + Rank.spdBonus + Role.spdBonus;

#if UNITY_EDITOR
    // Utility: log calculated stats to Console from the Inspector context menu.
    [ContextMenu("Log Calculated Stats")]
    private void LogStats()
    {
        Debug.Log($"[{displayName}] Layer {layer} | {rank} | {role}\n" +
                  $"HP:{FinalHP}  MP:{FinalMP}  ATK:{FinalATK}  MAG:{FinalMAG}  " +
                  $"DEF:{FinalDEF}  RES:{FinalRES}  SPD:{FinalSPD}");
    }
#endif
}
