using System.Collections.Generic;
using UnityEngine;

// ── StatusEffectManager ────────────────────────────────────────────────────
// Static helper. Applies, ticks, and removes statuses on RuntimeCharacterStates.
// CombatManager calls these methods; this class has no MonoBehaviour lifecycle.

public static class StatusEffectManager
{
    // ── Apply ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Apply a status to a target. Handles stacking rules.
    /// Returns true if the status was applied or refreshed, false if blocked.
    /// </summary>
    public static bool Apply(
        RuntimeCharacterState target,
        StatusEffectSO statusDef,
        RuntimeCharacterState source,
        int durationOverride = -1)
    {
        if (statusDef == null) return false;

        int duration = durationOverride > 0
            ? durationOverride
            : statusDef.defaultDuration;

        switch (statusDef.stackingRule)
        {
            case StatusEffectSO.StackingRule.RefreshDuration:
                return ApplyRefresh(target, statusDef, source, duration);

            case StatusEffectSO.StackingRule.ReplaceWeaker:
                return ApplyReplaceWeaker(target, statusDef, source, duration);

            case StatusEffectSO.StackingRule.ExclusiveGroup:
                return ApplyExclusiveGroup(target, statusDef, source, duration);

            default:
                return ApplyRefresh(target, statusDef, source, duration);
        }
    }

    // ── Refresh (default) ──────────────────────────────────────────────────

    private static bool ApplyRefresh(
        RuntimeCharacterState target,
        StatusEffectSO statusDef,
        RuntimeCharacterState source,
        int duration)
    {
        // Find existing instance of same status
        var existing = FindByStatusId(target, statusDef.statusId);
        if (existing != null)
        {
            existing.remainingDuration = Mathf.Max(existing.remainingDuration, duration);
            return true;
        }

        target.activeStatuses.Add(new StatusEffectInstance(statusDef, duration, source));
        return true;
    }

    // ── Replace Weaker (Restraint tier system) ────────────────────────────
    // Within the Restraint group, a stronger tier replaces a weaker one.
    // Weakness = lower index in the tier list below.
    // Applying a weaker tier when a stronger one is active: no effect.

    private static readonly List<string> RestraintTier = new List<string>
    {
        "snared", "restrained", "bound", "held", "roped", "wrapped"
    };

    private static bool ApplyReplaceWeaker(
        RuntimeCharacterState target,
        StatusEffectSO statusDef,
        RuntimeCharacterState source,
        int duration)
    {
        // Find any status in the same group
        var existing = FindByGroup(target, statusDef.group);

        if (existing != null)
        {
            // For Restraint group: compare tier ranks
            if (statusDef.group == "Restraint" || statusDef.group == "restraint")
            {
                int incomingRank = RestraintTier.IndexOf(statusDef.statusId);
                int existingRank = RestraintTier.IndexOf(existing.definition.statusId);

                if (incomingRank > existingRank)
                {
                    // Incoming is stronger — replace
                    target.activeStatuses.Remove(existing);
                    target.activeStatuses.Add(new StatusEffectInstance(statusDef, duration, source));
                    return true;
                }
                else
                {
                    // Incoming is same tier or weaker — no effect
                    return false;
                }
            }

            // Non-restraint replace_weaker: just replace
            target.activeStatuses.Remove(existing);
            target.activeStatuses.Add(new StatusEffectInstance(statusDef, duration, source));
            return true;
        }

        target.activeStatuses.Add(new StatusEffectInstance(statusDef, duration, source));
        return true;
    }

    // ── Exclusive Group ────────────────────────────────────────────────────
    // Only one status from this group can be active at a time.
    // New application replaces the old one.

    private static bool ApplyExclusiveGroup(
        RuntimeCharacterState target,
        StatusEffectSO statusDef,
        RuntimeCharacterState source,
        int duration)
    {
        var existing = FindByGroup(target, statusDef.group);
        if (existing != null)
            target.activeStatuses.Remove(existing);

        target.activeStatuses.Add(new StatusEffectInstance(statusDef, duration, source));
        return true;
    }

    // ── Tick ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Tick all statuses on a unit at the start of its turn.
    /// Fires DOT damage. Removes expired statuses.
    /// Returns list of DOT results (unit, amount, isResolveDot).
    /// CombatManager reads these to fire events and apply values.
    /// </summary>
    public static List<DotResult> Tick(RuntimeCharacterState unit)
    {
        var dotResults = new List<DotResult>();
        var toRemove   = new List<StatusEffectInstance>();

        foreach (var inst in unit.activeStatuses)
        {
            if (inst.definition == null) continue;

            // Apply DOT
            if (inst.definition.dotAmountPerTick > 0)
            {
                int amount = inst.definition.dotAmountPerTick;

                if (inst.definition.dotTargetsResolve)
                    unit.LoseResolve(amount);
                else
                    unit.TakeDamage(amount);

                dotResults.Add(new DotResult
                {
                    amount           = amount,
                    targetsResolve   = inst.definition.dotTargetsResolve
                });
            }

            // Tick duration (Turns type only)
            bool expired = inst.Tick();
            if (expired) toRemove.Add(inst);
        }

        foreach (var inst in toRemove)
            unit.activeStatuses.Remove(inst);

        return dotResults;
    }

    // ── Remove ─────────────────────────────────────────────────────────────

    /// <summary>Remove a status by statusId. Returns true if found and removed.</summary>
    public static bool Remove(RuntimeCharacterState target, string statusId)
    {
        var inst = FindByStatusId(target, statusId);
        if (inst == null) return false;
        target.activeStatuses.Remove(inst);
        return true;
    }

    /// <summary>Remove all statuses on a target.</summary>
    public static void RemoveAll(RuntimeCharacterState target)
    {
        target.activeStatuses.Clear();
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    public static StatusEffectInstance FindByStatusId(
        RuntimeCharacterState target, string statusId)
    {
        foreach (var inst in target.activeStatuses)
            if (inst.definition != null && inst.definition.statusId == statusId)
                return inst;
        return null;
    }

    public static StatusEffectInstance FindByGroup(
        RuntimeCharacterState target, string group)
    {
        foreach (var inst in target.activeStatuses)
            if (inst.definition != null &&
                string.Equals(inst.definition.group, group,
                    System.StringComparison.OrdinalIgnoreCase))
                return inst;
        return null;
    }

    public static bool HasStatus(RuntimeCharacterState target, string statusId)
    {
        return FindByStatusId(target, statusId) != null;
    }
}

// ── DotResult ──────────────────────────────────────────────────────────────
// Returned by Tick() so CombatManager can fire the correct events.

public struct DotResult
{
    public int  amount;
    public bool targetsResolve;
}
