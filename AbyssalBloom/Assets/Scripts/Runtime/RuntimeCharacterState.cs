using System.Collections.Generic;
using UnityEngine;

// ── Enums used by RuntimeCharacterState and CombatManager ─────────────────

public enum ResolveBand { Steady, Strong, Strained, Fragile, Broken }
public enum CorruptionBand { Clear, Low, Tempted, High, FullyCorrupted }
public enum PowerBand { VeryLow, Low, Medium, High, VeryHigh, Severe }

// ── RuntimeCharacterState ──────────────────────────────────────────────────
// Plain C# class — NOT a ScriptableObject.
// Holds all mutable combat state for one heroine or enemy during a run.
// Created by EncounterBuilder from SO data at encounter start.

public class RuntimeCharacterState
{
    // ── Identity ───────────────────────────────────────────────────────────
    public string characterId;
    public string displayName;
    public bool   isHeroine;

    // ── Base Stats (set at spawn, modified by permanent buffs) ─────────────
    public int maxHP;
    public int maxMP;
    public int maxResolve;
    public int maxCorruption;

    // ── Combat Stats (may be temporarily modified by statuses) ────────────
    // Use these properties in formulas — they fold in active stat modifiers.
    private int _atk, _mag, _def, _res, _spd;

    public int ATK => _atk + GetStatMod("atk");
    public int MAG => _mag + GetStatMod("mag");
    public int DEF => _def + GetStatMod("def");
    public int RES => _res + GetStatMod("res");
    public int SPD => _spd + GetStatMod("spd");

    // Setters used by EncounterBuilder
    public int BaseATK { set => _atk = value; }
    public int BaseMAG { set => _mag = value; }
    public int BaseDEF { set => _def = value; }
    public int BaseRES { set => _res = value; }
    public int BaseSPD { set => _spd = value; }

    // ── Current Values ─────────────────────────────────────────────────────
    public int currentHP;
    public int currentMP;
    public int resolve;
    public int corruption;

    // ── Derived ───────────────────────────────────────────────────────────
    public bool  IsAlive    => currentHP > 0;
    public float HPPercent  => maxHP > 0 ? (float)currentHP / maxHP : 0f;
    public int   MaxGrapplers => GetMaxGrapplers();

    // ── Turn state ────────────────────────────────────────────────────────
    public int  initiative;
    public bool isDefending;

    // ── Abilities ─────────────────────────────────────────────────────────
    public List<CharacterAbilitySO> abilities = new();

    // ── Active Statuses ───────────────────────────────────────────────────
    public List<StatusEffectInstance> activeStatuses = new();

    // ── Passive cooldowns (keyed by passiveId, value = turns remaining) ───
    private Dictionary<string, int> _passiveCooldowns = new();

    // ════════════════════════════════════════════════════════════════════════
    #region Stat Modification
    // ════════════════════════════════════════════════════════════════════════

    // Sum all flat stat modifiers from active statuses
    private int GetStatMod(string stat)
    {
        int total = 0;
        foreach (var inst in activeStatuses)
        {
            if (inst.definition == null) continue;
            switch (stat)
            {
                case "atk": total += inst.definition.modATK; break;
                case "mag": total += inst.definition.modMAG; break;
                case "def": total += inst.definition.modDEF; break;
                case "res": total += inst.definition.modRES; break;
                case "spd": total += inst.definition.modSPD; break;
            }
        }
        return total;
    }

    /// <summary>Permanent stat boost (e.g. enemy Frenzy stacks).</summary>
    public void PermanentStatBoost(int atkBonus, int magBonus)
    {
        _atk += atkBonus;
        _mag += magBonus;
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region HP / MP / Resolve / Corruption
    // ════════════════════════════════════════════════════════════════════════

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }

    public void SpendMP(int amount)
    {
        currentMP = Mathf.Max(0, currentMP - amount);
    }

    public void RestoreMP(int amount)
    {
        currentMP = Mathf.Min(maxMP, currentMP + amount);
    }

    public void LoseResolve(int amount)
    {
        resolve = Mathf.Max(0, resolve - amount);
    }

    public void RestoreResolve(int amount)
    {
        resolve = Mathf.Min(maxResolve, resolve + amount);
    }

    public void GainCorruption(int amount)
    {
        corruption = Mathf.Min(maxCorruption, corruption + amount);
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Resolve & Corruption Bands
    // ════════════════════════════════════════════════════════════════════════

    public ResolveBand GetResolveBand()
    {
        if (resolve == 100) return ResolveBand.Steady;
        if (resolve >= 70)  return ResolveBand.Strong;
        if (resolve >= 40)  return ResolveBand.Strained;
        if (resolve >= 1)   return ResolveBand.Fragile;
        return ResolveBand.Broken;
    }

    public CorruptionBand GetCorruptionBand()
    {
        if (corruption == 0)   return CorruptionBand.Clear;
        if (corruption <= 39)  return CorruptionBand.Low;
        if (corruption <= 69)  return CorruptionBand.Tempted;
        if (corruption <= 99)  return CorruptionBand.High;
        return CorruptionBand.FullyCorrupted;
    }

    private int GetMaxGrapplers()
    {
        switch (GetCorruptionBand())
        {
            case CorruptionBand.Clear:
            case CorruptionBand.Low:          return 1;
            case CorruptionBand.Tempted:       return 2;
            case CorruptionBand.High:          return 3;
            case CorruptionBand.FullyCorrupted: return 99; // effectively unlimited
            default:                           return 1;
        }
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Passive Cooldowns
    // ════════════════════════════════════════════════════════════════════════

    public bool IsPassiveReady(string passiveId)
    {
        return !_passiveCooldowns.ContainsKey(passiveId) ||
               _passiveCooldowns[passiveId] <= 0;
    }

    public void StartPassiveCooldown(string passiveId, int turns)
    {
        _passiveCooldowns[passiveId] = turns;
    }

    /// <summary>
    /// Called at the start of this unit's turn.
    /// Decrements all cooldowns by 1.
    /// </summary>
    public void TickPassiveCooldowns()
    {
        var keys = new List<string>(_passiveCooldowns.Keys);
        foreach (var key in keys)
        {
            _passiveCooldowns[key] = Mathf.Max(0, _passiveCooldowns[key] - 1);
        }
    }

    #endregion
}
