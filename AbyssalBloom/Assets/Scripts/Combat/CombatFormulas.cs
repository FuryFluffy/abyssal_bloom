using UnityEngine;

// ── CombatFormulas ─────────────────────────────────────────────────────────
// Static class. All locked combat math lives here.
// Never instantiated — call everything as CombatFormulas.Method().

public static class CombatFormulas
{
    // ── Constants ──────────────────────────────────────────────────────────
    public const float DefendDamageMultiplier = 0.5f;
    public const int   UniversalAttackBaseHit = 90;
    public const PowerBand UniversalAttackPower = PowerBand.Low;

    // ── Power Band Lookup ──────────────────────────────────────────────────
    public static float PowerMultiplier(PowerBand band)
    {
        switch (band)
        {
            case PowerBand.VeryLow:  return 0.65f;
            case PowerBand.Low:      return 0.85f;
            case PowerBand.Medium:   return 1.10f;
            case PowerBand.High:     return 1.35f;
            case PowerBand.VeryHigh: return 1.65f;
            case PowerBand.Severe:   return 2.00f;
            default:                 return 1.00f;
        }
    }

    // ── Damage ─────────────────────────────────────────────────────────────

    /// <summary>Physical damage. max(1, round((atk × power) - (def × 0.5)))</summary>
    public static int PhysicalDamage(int atk, float power, int def)
    {
        return Mathf.Max(1, Mathf.RoundToInt((atk * power) - (def * 0.50f)));
    }

    /// <summary>Magic damage. max(1, round((mag × power) - (res × 0.5))). Auto-hits.</summary>
    public static int MagicDamage(int mag, float power, int res)
    {
        return Mathf.Max(1, Mathf.RoundToInt((mag * power) - (res * 0.50f)));
    }

    /// <summary>Healing. round(base + (mag × power))</summary>
    public static int Healing(int baseHeal, int mag, float power)
    {
        return Mathf.RoundToInt(baseHeal + (mag * power));
    }

    // ── Resolve & Corruption ───────────────────────────────────────────────

    /// <summary>Resolve damage from abilities. max(1, round(base + (mag × 0.25) - (res × 0.20)))</summary>
    public static int ResolveDamage(int baseDamage, int attackerMag, int targetRes)
    {
        return Mathf.Max(1, Mathf.RoundToInt(baseDamage + (attackerMag * 0.25f) - (targetRes * 0.20f)));
    }

    /// <summary>Corruption gain from abilities. max(0, round(base - (res × 0.05)))</summary>
    public static int CorruptionGain(int baseGain, int targetRes)
    {
        return Mathf.Max(0, Mathf.RoundToInt(baseGain - (targetRes * 0.05f)));
    }

    /// <summary>Grapple Action resolve damage. max(1, round(6 + (mag × 0.25) - (res × 0.20)))</summary>
    public static int GrappleResolveDamage(int grappleMag, int targetRes)
    {
        return Mathf.Max(1, Mathf.RoundToInt(6 + (grappleMag * 0.25f) - (targetRes * 0.20f)));
    }

    /// <summary>Grapple Action corruption gain. max(0, round(4 - (res × 0.05)))</summary>
    public static int GrappleCorruptionGain(int targetRes)
    {
        return Mathf.Max(0, Mathf.RoundToInt(4 - (targetRes * 0.05f)));
    }

    /// <summary>Submit resolve damage. max(1, round(8 + (mag × 0.25) - (res × 0.20)))</summary>
    public static int SubmitResolveDamage(int grappleMag, int targetRes)
    {
        return Mathf.Max(1, Mathf.RoundToInt(8 + (grappleMag * 0.25f) - (targetRes * 0.20f)));
    }

    /// <summary>Submit corruption gain. max(0, round(6 - (res × 0.05)))</summary>
    public static int SubmitCorruptionGain(int targetRes)
    {
        return Mathf.Max(0, Mathf.RoundToInt(6 - (targetRes * 0.05f)));
    }

    // ── Hit & Status Chance ────────────────────────────────────────────────

    /// <summary>Hit chance. clamp(base + ((atkSpd - defSpd) × 2), 60, 98)</summary>
    public static int HitChance(int baseChance, int attackerSpd, int defenderSpd)
    {
        return Mathf.Clamp(baseChance + ((attackerSpd - defenderSpd) * 2), 60, 98);
    }

    /// <summary>
    /// Status application chance. MAG governs all status chances.
    /// clamp(base + ((atkMag - defRes) × 2), 15, 95)
    /// </summary>
    public static int StatusChance(int baseChance, int attackerMag, int targetRes)
    {
        return Mathf.Clamp(baseChance + ((attackerMag - targetRes) * 2), 15, 95);
    }

    /// <summary>Initiative roll. spd + Random.Range(0, 5)</summary>
    public static int RollInitiative(int spd)
    {
        return spd + Random.Range(0, 5);
    }

    /// <summary>Roll against a percentage chance (0–100). Returns true if hit.</summary>
    public static bool RollHit(int chancePercent)
    {
        return Random.Range(0, 100) < chancePercent;
    }

    /// <summary>Roll against a percentage status chance. Returns true if status applies.</summary>
    public static bool RollStatus(int chancePercent)
    {
        return Random.Range(0, 100) < chancePercent;
    }

    // ── Grapple ────────────────────────────────────────────────────────────

    /// <summary>
    /// Struggle escape chance.
    /// clamp(50 + (heroAtk - enemyAtk) × 3, 20, 85)
    /// Returns -1 if Resolve is Broken (Struggle unavailable).
    /// Applies Resolve band modifier before clamping.
    /// </summary>
    public static int StruggleChance(int heroAtk, int enemyAtk, ResolveBand resolveBand)
    {
        if (resolveBand == ResolveBand.Broken) return -1;

        int modifier = 0;
        if (resolveBand == ResolveBand.Strained) modifier = -10;
        if (resolveBand == ResolveBand.Fragile)  modifier = -25;

        int raw = 50 + ((heroAtk - enemyAtk) * 3) + modifier;
        return Mathf.Clamp(raw, 20, 85);
    }

    // ── Climax Recoil ──────────────────────────────────────────────────────

    /// <summary>
    /// Ecstasy Damage dealt to a grappling enemy at Climax.
    /// round(enemy.maxHP × (0.15 + active.corruption × 0.005))
    /// </summary>
    public static int ClimaxRecoilDamage(int enemyMaxHP, int activeCorruption)
    {
        float multiplier = 0.15f + (activeCorruption * 0.005f);
        return Mathf.RoundToInt(enemyMaxHP * multiplier);
    }
}
