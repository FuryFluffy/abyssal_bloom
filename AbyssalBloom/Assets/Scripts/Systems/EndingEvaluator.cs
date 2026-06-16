using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// EndingEvaluator — determines which of the 7 endings fires at the end
// of Layer 10 (or Layer 9 for the Neverending Cycle).
// ════════════════════════════════════════════════════════════════════════════
// Pure static class. No MonoBehaviour. No state.
//
// Call EvaluateEnding() after the final layer boss is defeated,
// before the final scene plays.
//
// Endings are checked in strict priority order (highest priority first).
// The first condition that passes wins.
//
// ── Ending IDs (priority order) ───────────────────────────────────────────
//
//   1. unfed_bloom
//      Refugee never established. No Bloom spent. No h-scenes triggered.
//      Theme: refusal of interaction = stagnation.
//
//   2. neverending_cycle
//      Layer 9 end only. Blood Nun dead. No Castle truths known.
//      Theme: victory without revelation.
//
//   3. battery_truth
//      All characters escaped. No roster remains. Escape = consumption.
//      Theme: exploitation disguised as freedom.
//
//   4. escape_selective
//      3 characters escaped Layer 10. Remaining roster stays behind.
//      Theme: survival as abandonment.
//
//   5. kill_the_castle
//      Castle destroyed. Everyone dies with it.
//      Theme: liberation through annihilation.
//
//   6. become_the_castle_reign
//      Heroine becomes Castle, rules through lust and desire.
//      Theme: power without restraint.
//
//   7. become_the_castle_free (FALLBACK)
//      Heroine becomes Castle, lets everyone else leave.
//      Theme: sacrifice for others' freedom.
//
// ════════════════════════════════════════════════════════════════════════════

public static class EndingEvaluator
{
    /// <summary>
    /// Evaluates all ending conditions in priority order.
    /// Pass currentLayer so Layer 9 vs Layer 10 endings are distinguished.
    /// Never returns null — falls back to "become_the_castle_free".
    /// </summary>
    public static string EvaluateEnding(int currentLayer = 10)
    {
        if (FlagManager.Instance == null)
        {
            Debug.LogError("[EndingEvaluator] FlagManager not available.");
            return "become_the_castle_free";
        }

        if (CheckUnfedBloom())                  return "unfed_bloom";
        if (currentLayer == 9
            && CheckNeverendingCycle())          return "neverending_cycle";
        if (CheckBatteryTruth())                return "battery_truth";
        if (CheckEscapeSelective())             return "escape_selective";
        if (CheckKillTheCastle())               return "kill_the_castle";
        if (CheckBecomeTheCastleReign())        return "become_the_castle_reign";

        return "become_the_castle_free";
    }

    // ── Condition Checks (STUB) ────────────────────────────────────────────

    /// <summary>
    /// unfed_bloom — Priority 1
    /// Refuge never established. No Bloom spent. No h-scenes triggered.
    /// </summary>
    private static bool CheckUnfedBloom()
    {
        // STUB
        // refuge_ever_established = "0"
        // total_bloom_spent = "0"
        // no h_scene_triggered flags set
        return false;
    }

    /// <summary>
    /// neverending_cycle — Priority 2 (Layer 9 only)
    /// Blood Nun dead but no Castle truths known. Party stops at Layer 9.
    /// </summary>
    private static bool CheckNeverendingCycle()
    {
        // STUB
        // blood_nun_defeated = "1"
        // castle_sentience_known = "0"
        // castle_reinterpretation_flag = "0"
        return false;
    }

    /// <summary>
    /// battery_truth — Priority 3
    /// All characters took Escape route. No roster remains.
    /// Truth: Escape was always consumption.
    /// </summary>
    private static bool CheckBatteryTruth()
    {
        // STUB
        // all_characters_escaped = "1"
        // remaining_roster_count = "0"
        return false;
    }

    /// <summary>
    /// escape_selective — Priority 4
    /// 3 characters escaped. Remaining roster stays behind.
    /// </summary>
    private static bool CheckEscapeSelective()
    {
        // STUB
        // escape_route_taken = "1"
        // remaining_roster_count > "0"
        // castle_exit_accepted = "1"
        return false;
    }

    /// <summary>
    /// kill_the_castle — Priority 5
    /// Castle destroyed. Everyone inside dies.
    /// </summary>
    private static bool CheckKillTheCastle()
    {
        // STUB
        // castle_destruction_chosen = "1"
        // castle_heart_destroyed = "1"
        return false;
    }

    /// <summary>
    /// become_the_castle_reign — Priority 6
    /// Heroine becomes Castle, rules through lust and desire.
    /// High corruption route.
    /// </summary>
    private static bool CheckBecomeTheCastleReign()
    {
        // STUB
        // heroine_became_castle = "1"
        // deep_corruption_flag = "1"
        // desire_reign_chosen = "1"
        return false;
    }

    // become_the_castle_free is the fallback — no Check method needed.
    // Fires when heroine becomes Castle but frees everyone else.

    // ── Flag Helpers ───────────────────────────────────────────────────────

    private static bool KnowsFlag(string key)
        => FlagManager.Instance.GetFlag(
               FlagManager.Scope.PersistentKnowledge, key, "0") == "1";

    private static bool SlotFlag(string key)
        => FlagManager.Instance.GetFlag(
               FlagManager.Scope.SaveSlot, key, "0") == "1";

    private static bool RunFlag(string key)
        => FlagManager.Instance.GetFlag(
               FlagManager.Scope.RunState, key, "0") == "1";

    private static int SlotInt(string key)
    {
        string val = FlagManager.Instance.GetFlag(
            FlagManager.Scope.SaveSlot, key, "0");
        return int.TryParse(val, out int result) ? result : 0;
    }
}
