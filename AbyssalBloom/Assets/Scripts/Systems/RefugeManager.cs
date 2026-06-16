using System;
using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RefugeManager — owns all Refuge hub logic. No UI code here.
// ════════════════════════════════════════════════════════════════════════════
// Attach to the _Managers GameObject alongside RunStateManager and FlagManager.
// Bloom spending now delegates to RunStateManager.SpendBloom() — the local
// _currentBloom workaround has been removed now that RunStateManager exposes
// CurrentBloom, SpendBloom(), and ReturnToRefuge().
// ════════════════════════════════════════════════════════════════════════════

public class RefugeManager : MonoBehaviour
{
    // ── Singleton ──────────────────────────────────────────────────────────

    public static RefugeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ── Bloom costs (edit here to tune) ───────────────────────────────────

    public const int COST_RESTORE_HP      = 5;
    public const int COST_RESTORE_MP      = 3;
    public const int COST_RESTORE_RESOLVE = 4;
    public const int COST_REDUCE_CORRUPT  = 6;
    public const int COST_FULL_PARTY      = 20;
    public const int COST_UPGRADE_TIER1   = 15;
    public const int COST_UPGRADE_TIER2   = 25;
    public const int CORRUPTION_REDUCTION = 20;

    // ── Events ────────────────────────────────────────────────────────────

    public event Action OnBloomChanged;
    public event Action OnPartyStateChanged;

    // ── Bloom passthrough ─────────────────────────────────────────────────

    /// <summary>Bloom available to spend — delegates to RunStateManager.</summary>
    public int CurrentBloom => RunStateManager.Instance?.CurrentBloom ?? 0;

    // ── Initialise ────────────────────────────────────────────────────────

    /// <summary>
    /// Called by RunStateManager.ReturnToRefuge() — or manually if needed.
    /// Fires OnBloomChanged so UI refreshes on arrival.
    /// </summary>
    public void InitialiseFromRunState()
    {
        OnBloomChanged?.Invoke();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Bloom helpers
    // ════════════════════════════════════════════════════════════════════════

    public bool CanAfford(int cost) => CurrentBloom >= cost;

    private void SpendBloom(int cost)
    {
        RunStateManager.Instance?.SpendBloom(cost);
        OnBloomChanged?.Invoke();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Recovery
    // ════════════════════════════════════════════════════════════════════════

    public void RestoreHeroineHP(RuntimeCharacterState heroine)
    {
        if (!CanAfford(COST_RESTORE_HP)) return;
        heroine.Heal(heroine.maxHP);
        SpendBloom(COST_RESTORE_HP);
        OnPartyStateChanged?.Invoke();
    }

    public void RestoreHeroineMP(RuntimeCharacterState heroine)
    {
        if (!CanAfford(COST_RESTORE_MP)) return;
        heroine.RestoreMP(heroine.maxMP);
        SpendBloom(COST_RESTORE_MP);
        OnPartyStateChanged?.Invoke();
    }

    public void RestoreHeroineResolve(RuntimeCharacterState heroine)
    {
        if (!CanAfford(COST_RESTORE_RESOLVE)) return;
        heroine.RestoreResolve(heroine.maxResolve);
        SpendBloom(COST_RESTORE_RESOLVE);
        OnPartyStateChanged?.Invoke();
    }

    public void ReduceHeroineCorruption(RuntimeCharacterState heroine)
    {
        if (!CanAfford(COST_REDUCE_CORRUPT)) return;
        heroine.corruption = Mathf.Max(0, heroine.corruption - CORRUPTION_REDUCTION);
        SpendBloom(COST_REDUCE_CORRUPT);
        OnPartyStateChanged?.Invoke();
    }

    public void RestoreFullParty()
    {
        if (!CanAfford(COST_FULL_PARTY)) return;

        var party = RunStateManager.Instance?.Party;
        if (party == null) return;

        foreach (var h in party)
        {
            h.Heal(h.maxHP);
            h.RestoreMP(h.maxMP);
            h.RestoreResolve(h.maxResolve);
            h.corruption = Mathf.Max(0, h.corruption - CORRUPTION_REDUCTION);
        }

        SpendBloom(COST_FULL_PARTY);
        OnPartyStateChanged?.Invoke();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Ability upgrades
    // ════════════════════════════════════════════════════════════════════════

    // Flag key: "upgrade_{heroineId}_{abilityId}_tier"
    // Values:   "0" base | "1" upgraded | "2a" branch A | "2b" branch B

    private static string TierKey(string heroineId, string abilityId)
        => $"upgrade_{heroineId}_{abilityId}_tier";

    public string GetUpgradeTier(string heroineId, string abilityId)
        => FlagManager.Instance.GetFlag(FlagManager.Scope.SaveSlot, TierKey(heroineId, abilityId), "0");

    /// <summary>
    /// Returns true if the upgrade is affordable and the progression is valid.
    /// targetTier must be "1", "2a", or "2b".
    /// "1" requires current tier == "0".
    /// "2a"/"2b" require current tier == "1".
    /// </summary>
    public bool CanUpgrade(string heroineId, string abilityId, string targetTier)
    {
        string current = GetUpgradeTier(heroineId, abilityId);

        bool progressionValid = targetTier switch
        {
            "1"  => current == "0",
            "2a" => current == "1",
            "2b" => current == "1",
            _    => false
        };

        if (!progressionValid) return false;

        int cost = targetTier == "1" ? COST_UPGRADE_TIER1 : COST_UPGRADE_TIER2;
        return CanAfford(cost);
    }

    /// <summary>
    /// Swaps oldAbility for newAbility on the heroine's ability list,
    /// records the new tier in FlagManager SaveSlot, and spends Bloom.
    /// </summary>
    public void PurchaseUpgrade(RuntimeCharacterState heroine,
                                CharacterAbilitySO    oldAbility,
                                CharacterAbilitySO    newAbility,
                                string                newTier)
    {
        if (!CanUpgrade(heroine.characterId, oldAbility.abilityId, newTier)) return;

        int idx = heroine.abilities.IndexOf(oldAbility);
        if (idx >= 0)
            heroine.abilities[idx] = newAbility;
        else
            heroine.abilities.Add(newAbility);

        FlagManager.Instance.SetFlag(
            FlagManager.Scope.SaveSlot,
            TierKey(heroine.characterId, oldAbility.abilityId),
            newTier);

        int cost = newTier == "1" ? COST_UPGRADE_TIER1 : COST_UPGRADE_TIER2;
        SpendBloom(cost);
        OnPartyStateChanged?.Invoke();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Run start
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Discards remaining Bloom (feeds the Castle), then begins a new run
    /// with the current party.
    /// </summary>
    public void StartRun()
    {
        // Bloom resets to 0 in StartNewRun — no explicit spend needed
        var party = RunStateManager.Instance?.Party;
        if (party == null || party.Count == 0)
        {
            Debug.LogError("[RefugeManager] StartRun called but Party is empty.");
            return;
        }

        RunStateManager.Instance.StartNewRun(party);
    }
}
