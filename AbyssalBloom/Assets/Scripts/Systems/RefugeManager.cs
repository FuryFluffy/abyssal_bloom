using System;
using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RefugeManager — owns all Refuge hub logic. No UI code here.
// ════════════════════════════════════════════════════════════════════════════
// Attach to the _Managers GameObject alongside RunStateManager and FlagManager.
// Bloom spending delegates to RunStateManager.SpendBloom().
//
// Refuge access is gated on the persistent_knowledge flag "refuge_ever_established".
// That flag is set by narrative triggers — RefugeManager only reads it.
//
// On arrival, HP and MP are restored for free (the Castle provides shelter).
// Resolve and Corruption require deliberate Bloom expenditure.
// Unspent Bloom is consumed by the Castle when StartRun() is called.
//
// Allure of the Abyss is a shared party upgrade, purchasable once per save slot.
// Its flag key is "upgrade_shared_allure_of_the_abyss_tier", value "1" when bought.
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

    // ── Flag keys ──────────────────────────────────────────────────────────

    private const string FLAG_REFUGE_ESTABLISHED = "refuge_ever_established";
    private const string FLAG_ALLURE_PURCHASED   = "upgrade_shared_allure_of_the_abyss_tier";

    // ── Bloom costs (edit here to tune) ───────────────────────────────────
    //
    // Free on Refuge arrival (no constants):
    //   HP fully restored to max
    //   MP fully restored to max
    //
    // Bloom earn rate reference (set on EnemyDataSO / encounter data):
    //   Standard enemy:   2 + Mathf.FloorToInt((layer - 1) / 2f)
    //   Elite enemy:      standard + 3
    //   Miniboss:         9  (flat)
    //   Layer boss:       15 + (layer - 1) * 2
    //   Hotspot event:    1–3  (flat, no scaling)
    //   Lore discovery:   1  (flat)

    public const int COST_RESTORE_RESOLVE  = 5;    // per heroine, full Resolve
    public const int COST_REDUCE_CORRUPT   = 12;   // per heroine, flat reduction
    public const int CORRUPTION_REDUCTION  = 15;   // Corruption removed per purchase

    public const int COST_UPGRADE_TIER1    = 15;   // Tier 0 → Tier 1, per ability
    public const int COST_UPGRADE_TIER2    = 25;   // Tier 1 → Tier 2a or 2b, per ability
    public const int COST_UPGRADE_ALLURE   = 45;   // Allure of the Abyss — once per save slot

    // ── Events ────────────────────────────────────────────────────────────

    public event Action OnBloomChanged;
    public event Action OnPartyStateChanged;

    // ── Refuge gate ───────────────────────────────────────────────────────

    /// <summary>
    /// True when the Refuge has been established (narrative trigger sets the flag).
    /// All Refuge services are unavailable until this is true.
    /// </summary>
    public bool IsRefugeEstablished =>
        FlagManager.Instance != null &&
        FlagManager.Instance.GetFlag(
            FlagManager.Scope.PersistentKnowledge,
            FLAG_REFUGE_ESTABLISHED,
            "false") == "true";

    // ── Bloom passthrough ─────────────────────────────────────────────────

    /// <summary>Bloom available to spend — delegates to RunStateManager.</summary>
    public int CurrentBloom => RunStateManager.Instance?.CurrentBloom ?? 0;

    // ── Initialise ────────────────────────────────────────────────────────

    /// <summary>
    /// Called by RunStateManager.ReturnToRefuge() — or manually if needed.
    /// If the Refuge is established, restores HP and MP for free to all party members.
    /// Fires events so UI refreshes on arrival.
    /// </summary>
    public void InitialiseFromRunState()
    {
        if (IsRefugeEstablished)
        {
            var party = RunStateManager.Instance?.Party;
            if (party != null)
            {
                foreach (var h in party)
                {
                    RestoreHP(h);
                    RestoreMP(h);
                }
            }
        }

        OnBloomChanged?.Invoke();
        OnPartyStateChanged?.Invoke();
    }

    // ── Free restore utilities (private — called only from InitialiseFromRunState) ──

    private static void RestoreHP(RuntimeCharacterState heroine)
        => heroine.Heal(heroine.maxHP);

    private static void RestoreMP(RuntimeCharacterState heroine)
        => heroine.RestoreMP(heroine.maxMP);

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
    // Recovery services (Bloom-gated)
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Restores full Resolve to one heroine. Costs COST_RESTORE_RESOLVE Bloom.</summary>
    public void RestoreHeroineResolve(RuntimeCharacterState heroine)
    {
        if (!IsRefugeEstablished) return;
        if (!CanAfford(COST_RESTORE_RESOLVE)) return;
        heroine.RestoreResolve(heroine.maxResolve);
        SpendBloom(COST_RESTORE_RESOLVE);
        OnPartyStateChanged?.Invoke();
    }

    /// <summary>
    /// Reduces one heroine's Corruption by CORRUPTION_REDUCTION (flat).
    /// Costs COST_REDUCE_CORRUPT Bloom.
    /// </summary>
    public void ReduceHeroineCorruption(RuntimeCharacterState heroine)
    {
        if (!IsRefugeEstablished) return;
        if (!CanAfford(COST_REDUCE_CORRUPT)) return;
        heroine.corruption = Mathf.Max(0, heroine.corruption - CORRUPTION_REDUCTION);
        SpendBloom(COST_REDUCE_CORRUPT);
        OnPartyStateChanged?.Invoke();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Ability upgrades
    // ════════════════════════════════════════════════════════════════════════

    // Flag key: "upgrade_{heroineId}_{abilityId}_tier"
    // Values:   "0" base | "1" upgraded | "2a" branch A | "2b" branch B
    //
    // Allure of the Abyss uses heroineId "shared" and abilityId "allure_of_the_abyss".
    // Its flag is FLAG_ALLURE_PURCHASED in SaveSlot scope, value "1" when bought.

    private static string TierKey(string heroineId, string abilityId)
        => $"upgrade_{heroineId}_{abilityId}_tier";

    public string GetUpgradeTier(string heroineId, string abilityId)
        => FlagManager.Instance.GetFlag(
               FlagManager.Scope.SaveSlot,
               TierKey(heroineId, abilityId),
               "0");

    /// <summary>
    /// Returns true if the Refuge is established, the upgrade is affordable,
    /// and the progression is valid.
    /// targetTier: "1" requires current == "0"; "2a"/"2b" require current == "1".
    /// </summary>
    public bool CanUpgrade(string heroineId, string abilityId, string targetTier)
    {
        if (!IsRefugeEstablished) return false;

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

    // ── Allure of the Abyss ───────────────────────────────────────────────

    /// <summary>True if Allure of the Abyss has been purchased this save slot.</summary>
    public bool IsAllurePurchased =>
        FlagManager.Instance != null &&
        FlagManager.Instance.GetFlag(
            FlagManager.Scope.SaveSlot,
            FLAG_ALLURE_PURCHASED,
            "0") == "1";

    /// <summary>
    /// Returns true if the Refuge is established, Allure has not yet been purchased,
    /// and the player can afford COST_UPGRADE_ALLURE.
    /// </summary>
    public bool CanPurchaseAllure =>
        IsRefugeEstablished && !IsAllurePurchased && CanAfford(COST_UPGRADE_ALLURE);

    /// <summary>
    /// Purchases Allure of the Abyss. Records the flag in SaveSlot scope and spends Bloom.
    /// No ability swap is performed here — the caller is responsible for applying the
    /// passive upgrade effect to all party members.
    /// </summary>
    public void PurchaseAllure()
    {
        if (!CanPurchaseAllure) return;

        FlagManager.Instance.SetFlag(
            FlagManager.Scope.SaveSlot,
            FLAG_ALLURE_PURCHASED,
            "1");

        SpendBloom(COST_UPGRADE_ALLURE);
        OnPartyStateChanged?.Invoke();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Run start
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Discards all remaining Bloom (Castle consumes it), then begins a new run.
    /// Unspent Bloom is lost regardless of whether it was deliberately withheld.
    /// </summary>
    public void StartRun()
    {
        var party = RunStateManager.Instance?.Party;
        if (party == null || party.Count == 0)
        {
            Debug.LogError("[RefugeManager] StartRun called but Party is empty.");
            return;
        }

        // Bloom resets to 0 inside StartNewRun — no explicit spend needed here.
        RunStateManager.Instance.StartNewRun(party);
    }
}
