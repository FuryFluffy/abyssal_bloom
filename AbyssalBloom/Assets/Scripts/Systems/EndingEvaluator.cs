using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// EndingEvaluator — determines which of the 7 endings fires at Castle Heart.
// ════════════════════════════════════════════════════════════════════════════
// Pure static class. No MonoBehaviour. No state.
//
// Call EvaluateEnding() after the Castle Heart (Layer 10 boss) is defeated,
// before the final scene plays.
//
// Endings are checked in strict priority order (highest priority first).
// The first condition that passes wins. If no condition passes, the fallback
// ending fires.
//
// Priority order and ending IDs sourced from:
//   ending_check_implementation_v0_1.csv (godot data export)
//
// ── Ending IDs (priority order) ───────────────────────────────────────────
//   1. battery_paradise        — max Bloom / comfort investment
//   2. hidden_captivity        — apparent escape, Castle retained
//   3. living_keep             — Castle accepted as home
//   4. escaped_path            — genuine escape with knowledge
//   5. carrying_the_castle     — Castle Heart carried out
//   6. unfed_bloom             — Castle weakened, not destroyed
//   7. breaking_castle_understanding — true ending, hardest conditions
//
// ── Implementation Status ─────────────────────────────────────────────────
// STUB — condition logic is not yet authored. Each check method currently
// returns false. Fill in each method when ending conditions are finalised
// in a Narrative chat.
//
// The structure (priority loop, flag access, return value) is locked and
// must not be changed when conditions are filled in. Only the bodies of
// the Check* methods should be modified.
// ════════════════════════════════════════════════════════════════════════════

public static class EndingEvaluator
{
    // ── Public API ─────────────────────────────────────────────────────────

    /// <summary>
    /// Evaluates all ending conditions in priority order and returns the
    /// ending ID that should play. Never returns null — falls back to
    /// "breaking_castle_understanding" if no other condition passes.
    /// </summary>
    public static string EvaluateEnding()
    {
        if (FlagManager.Instance == null)
        {
            Debug.LogError("[EndingEvaluator] FlagManager not available.");
            return "breaking_castle_understanding";
        }

        // Check in strict priority order — first match wins.
        if (CheckBatteryParadise())       return "battery_paradise";
        if (CheckHiddenCaptivity())       return "hidden_captivity";
        if (CheckLivingKeep())            return "living_keep";
        if (CheckEscapedPath())           return "escaped_path";
        if (CheckCarryingTheCastle())     return "carrying_the_castle";
        if (CheckUnfedBloom())            return "unfed_bloom";

        // Fallback — hardest conditions, or no other ending matched.
        return "breaking_castle_understanding";
    }

    // ── Condition Checks (STUB — fill in when narrative is finalised) ──────

    /// <summary>
    /// battery_paradise — Priority 1
    /// Highest Bloom investment / comfort route fully accepted.
    /// TODO: Define exact flag conditions in Narrative chat.
    /// </summary>
    private static bool CheckBatteryParadise()
    {
        // STUB
        return false;
    }

    /// <summary>
    /// hidden_captivity — Priority 2
    /// Run ends in apparent escape but Castle was retained / heroines re-captured.
    /// TODO: Define exact flag conditions in Narrative chat.
    /// </summary>
    private static bool CheckHiddenCaptivity()
    {
        // STUB
        return false;
    }

    /// <summary>
    /// living_keep — Priority 3
    /// Castle accepted as permanent home. Heroines remain willingly.
    /// TODO: Define exact flag conditions in Narrative chat.
    /// </summary>
    private static bool CheckLivingKeep()
    {
        // STUB
        return false;
    }

    /// <summary>
    /// escaped_path — Priority 4
    /// Genuine escape. Requires specific knowledge flags to be set.
    /// TODO: Define exact flag conditions in Narrative chat.
    /// </summary>
    private static bool CheckEscapedPath()
    {
        // STUB
        return false;
    }

    /// <summary>
    /// carrying_the_castle — Priority 5
    /// Castle Heart carried out of the pocket dimension.
    /// TODO: Define exact flag conditions in Narrative chat.
    /// </summary>
    private static bool CheckCarryingTheCastle()
    {
        // STUB
        return false;
    }

    /// <summary>
    /// unfed_bloom — Priority 6
    /// Castle weakened but not destroyed. Bloom withheld throughout.
    /// TODO: Define exact flag conditions in Narrative chat.
    /// </summary>
    private static bool CheckUnfedBloom()
    {
        // STUB
        return false;
    }

    // breaking_castle_understanding is the fallback in EvaluateEnding() —
    // no separate check method needed. It fires when nothing else matches.

    // ── Flag Access Helper ─────────────────────────────────────────────────

    /// <summary>
    /// Shorthand for reading a PersistentKnowledge flag.
    /// Returns true if the flag is set to "1".
    /// </summary>
    private static bool KnowsFlag(string key)
        => FlagManager.Instance.GetFlag(
               FlagManager.Scope.PersistentKnowledge, key, "0") == "1";

    /// <summary>
    /// Shorthand for reading a SaveSlot flag.
    /// Returns true if the flag is set to "1".
    /// </summary>
    private static bool SlotFlag(string key)
        => FlagManager.Instance.GetFlag(
               FlagManager.Scope.SaveSlot, key, "0") == "1";

    /// <summary>
    /// Shorthand for reading a RunState flag.
    /// Returns true if the flag is set to "1".
    /// </summary>
    private static bool RunFlag(string key)
        => FlagManager.Instance.GetFlag(
               FlagManager.Scope.RunState, key, "0") == "1";
}
