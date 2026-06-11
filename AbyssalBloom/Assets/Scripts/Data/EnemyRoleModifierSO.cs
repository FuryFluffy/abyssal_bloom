using UnityEngine;

// Create ONE asset (Assets > Create > AbyssalBloom > Enemy Role Modifier).
// Fill in all six role rows in the Inspector.
[CreateAssetMenu(fileName = "EnemyRoleModifier", menuName = "AbyssalBloom/Enemy Role Modifier")]
public class EnemyRoleModifierSO : ScriptableObject
{
    public enum Role { Bruiser, Caster, Controller, Skirmisher, Tank, Support }

    [System.Serializable]
    public struct RoleModifier
    {
        public Role role;
        [Tooltip("Multiplier on HP (use 1.0 if no change)")]
        public float hpMult;
        [Tooltip("Multiplier on ATK (use 1.0 if no change)")]
        public float atkMult;
        [Tooltip("Multiplier on MAG (use 1.0 if no change)")]
        public float magMult;
        [Tooltip("Multiplier on DEF (use 1.0 if no change)")]
        public float defMult;
        [Tooltip("Multiplier on RES (use 1.0 if no change)")]
        public float resMult;
        [Tooltip("Flat bonus added to SPD after layer base + rank bonus")]
        public int spdBonus;
    }

    public RoleModifier[] roles;

    public RoleModifier GetRole(Role role)
    {
        foreach (var r in roles)
            if (r.role == role) return r;

        Debug.LogWarning($"[EnemyRoleModifierSO] Role '{role}' not found. Returning default.");
        return new RoleModifier
        {
            hpMult = 1f, atkMult = 1f, magMult = 1f,
            defMult = 1f, resMult = 1f, spdBonus = 0
        };
    }
}
