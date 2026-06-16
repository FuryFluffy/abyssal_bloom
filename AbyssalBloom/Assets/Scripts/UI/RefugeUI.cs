using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════════════════════════
// RefugeUI — drives the Refuge hub menus.  No logic lives here.
// ════════════════════════════════════════════════════════════════════════════
// Attach to the root Refuge panel GameObject.
// Wire every Inspector field listed at the bottom of this file.
//
// Section GameObjects are toggled active/inactive by SwitchSection().
// All per-heroine rows are generated at runtime from the Party list —
// you do NOT need one row prefab per heroine; just provide the three
// slots below (rows 0-2 map to Party[0-2]).
// ════════════════════════════════════════════════════════════════════════════

public class RefugeUI : MonoBehaviour
{
    // ════════════════════════════════════════════════════════════════════════
    // Inspector fields — wire every one of these in the Inspector
    // ════════════════════════════════════════════════════════════════════════

    [Header("Manager References")]
    public RefugeManager   refugeManager;
    public RunStateManager runStateManager;

    [Header("Root Panel (this is shown/hidden as a whole)")]
    public GameObject refugePanel;

    // ── Always-visible ─────────────────────────────────────────────────────

    [Header("Always Visible — Bloom display")]
    public Text bloomText;          // "Bloom: {n}"

    [Header("Always Visible — Tab buttons")]
    public Button tabRecovery;
    public Button tabParty;
    public Button tabUpgrades;
    public Button tabGallery;
    public Button tabKnowledge;

    [Header("Always Visible — Start Run button")]
    public Button startRunButton;   // label set to "Start Run (Bloom resets)"

    // ── Sections ───────────────────────────────────────────────────────────

    [Header("Section Root GameObjects (toggled active/inactive)")]
    public GameObject recoverySection;
    public GameObject partySection;
    public GameObject upgradesSection;
    public GameObject gallerySection;
    public GameObject knowledgeSection;

    // ── Recovery section ───────────────────────────────────────────────────
    // One HeroineRecoveryRow per party slot (index matches Party[i]).

    [Header("Recovery Section — one row per party slot (0-2)")]
    public HeroineRecoveryRow[] recoveryRows = new HeroineRecoveryRow[3];

    [Header("Recovery Section — Full Party button")]
    public Button fullPartyButton;  // "Recover All (20 Bloom)"

    // ── Party section ──────────────────────────────────────────────────────

    [Header("Party Section — one row per party slot (0-2)")]
    public HeroinePartyRow[] partyRows = new HeroinePartyRow[3];

    // ── Upgrades section ───────────────────────────────────────────────────
    // One HeroineUpgradeRow per party slot.

    [Header("Upgrades Section — one row per party slot (0-2)")]
    public HeroineUpgradeRow[] upgradeRows = new HeroineUpgradeRow[3];

    // ════════════════════════════════════════════════════════════════════════
    // Serialisable row types (set up in the Inspector as child GameObjects)
    // ════════════════════════════════════════════════════════════════════════

    [System.Serializable]
    public class HeroineRecoveryRow
    {
        public Text   nameLabel;
        public Button restoreHPButton;      // "Restore HP (5 Bloom)"
        public Button restoreMPButton;      // "Restore MP (3 Bloom)"
        public Button restoreResolveButton; // "Restore Resolve (4 Bloom)"
        public Button reduceCorruptButton;  // "Reduce Corruption (6 Bloom)"
    }

    [System.Serializable]
    public class HeroinePartyRow
    {
        public Text nameLabel;
        public Text hpLabel;         // "HP: {cur}/{max}"
        public Text mpLabel;         // "MP: {cur}/{max}"
        public Text resolveLabel;    // "Resolve: {cur}/{max}"
        public Text corruptionLabel; // "Corruption: {cur}/100"
    }

    [System.Serializable]
    public class HeroineUpgradeRow
    {
        public Text   nameLabel;
        // Ability slot 0
        public Text   ability0Label;         // ability name + tier
        public Button ability0Tier1Button;   // "Upgrade (15 Bloom)" — shown when tier == "0"
        public Button ability0Tier2aButton;  // "Branch A (25 Bloom)" — shown when tier == "1"
        public Button ability0Tier2bButton;  // "Branch B (25 Bloom)" — shown when tier == "1"
        // Ability slot 1
        public Text   ability1Label;
        public Button ability1Tier1Button;
        public Button ability1Tier2aButton;
        public Button ability1Tier2bButton;

        // ── Upgrade ability references (drag the SOs in the Inspector) ──
        [Header("Ability SOs for this heroine (drag from Project)")]
        public CharacterAbilitySO ability0Base;
        public CharacterAbilitySO ability0Tier1;
        public CharacterAbilitySO ability0Tier2a;
        public CharacterAbilitySO ability0Tier2b;
        public CharacterAbilitySO ability1Base;
        public CharacterAbilitySO ability1Tier1;
        public CharacterAbilitySO ability1Tier2a;
        public CharacterAbilitySO ability1Tier2b;
    }

    // ════════════════════════════════════════════════════════════════════════
    // Lifecycle
    // ════════════════════════════════════════════════════════════════════════

    private void OnEnable()
    {
        refugeManager.OnBloomChanged      += RefreshBloom;
        refugeManager.OnPartyStateChanged += RefreshAll;

        WireButtons();
        RefreshAll();
        SwitchSection(recoverySection);
    }

    private void OnDisable()
    {
        refugeManager.OnBloomChanged      -= RefreshBloom;
        refugeManager.OnPartyStateChanged -= RefreshAll;
    }

    // ════════════════════════════════════════════════════════════════════════
    // Button wiring — done once in OnEnable
    // ════════════════════════════════════════════════════════════════════════

    private void WireButtons()
    {
        // Tab buttons
        tabRecovery.onClick.AddListener(() => SwitchSection(recoverySection));
        tabParty.onClick.AddListener(()     => SwitchSection(partySection));
        tabUpgrades.onClick.AddListener(()  => SwitchSection(upgradesSection));
        tabGallery.onClick.AddListener(()   => SwitchSection(gallerySection));
        tabKnowledge.onClick.AddListener(() => SwitchSection(knowledgeSection));

        // Start run
        startRunButton.GetComponentInChildren<Text>().text = "Start Run (Bloom resets)";
        startRunButton.onClick.AddListener(OnStartRunClicked);

        // Full party recover
        fullPartyButton.GetComponentInChildren<Text>().text =
            $"Recover All ({RefugeManager.COST_FULL_PARTY} Bloom)";
        fullPartyButton.onClick.AddListener(() => refugeManager.RestoreFullParty());

        // Per-heroine recovery rows
        var party = runStateManager.Party;
        for (int i = 0; i < recoveryRows.Length && i < party.Count; i++)
        {
            var row    = recoveryRows[i];
            var heroine = party[i]; // capture for lambda

            row.restoreHPButton.GetComponentInChildren<Text>().text =
                $"Restore HP ({RefugeManager.COST_RESTORE_HP} Bloom)";
            row.restoreMPButton.GetComponentInChildren<Text>().text =
                $"Restore MP ({RefugeManager.COST_RESTORE_MP} Bloom)";
            row.restoreResolveButton.GetComponentInChildren<Text>().text =
                $"Restore Resolve ({RefugeManager.COST_RESTORE_RESOLVE} Bloom)";
            row.reduceCorruptButton.GetComponentInChildren<Text>().text =
                $"Reduce Corruption ({RefugeManager.COST_REDUCE_CORRUPT} Bloom)";

            row.restoreHPButton.onClick.AddListener(()     => refugeManager.RestoreHeroineHP(heroine));
            row.restoreMPButton.onClick.AddListener(()     => refugeManager.RestoreHeroineMP(heroine));
            row.restoreResolveButton.onClick.AddListener(() => refugeManager.RestoreHeroineResolve(heroine));
            row.reduceCorruptButton.onClick.AddListener(() => refugeManager.ReduceHeroineCorruption(heroine));
        }

        // Per-heroine upgrade rows
        for (int i = 0; i < upgradeRows.Length && i < party.Count; i++)
        {
            var row    = upgradeRows[i];
            var heroine = party[i];

            row.ability0Tier1Button.GetComponentInChildren<Text>().text =
                $"Upgrade ({RefugeManager.COST_UPGRADE_TIER1} Bloom)";
            row.ability0Tier2aButton.GetComponentInChildren<Text>().text =
                $"Branch A ({RefugeManager.COST_UPGRADE_TIER2} Bloom)";
            row.ability0Tier2bButton.GetComponentInChildren<Text>().text =
                $"Branch B ({RefugeManager.COST_UPGRADE_TIER2} Bloom)";
            row.ability1Tier1Button.GetComponentInChildren<Text>().text =
                $"Upgrade ({RefugeManager.COST_UPGRADE_TIER1} Bloom)";
            row.ability1Tier2aButton.GetComponentInChildren<Text>().text =
                $"Branch A ({RefugeManager.COST_UPGRADE_TIER2} Bloom)";
            row.ability1Tier2bButton.GetComponentInChildren<Text>().text =
                $"Branch B ({RefugeManager.COST_UPGRADE_TIER2} Bloom)";

            // Ability 0 upgrade buttons
            row.ability0Tier1Button.onClick.AddListener(() =>
                refugeManager.PurchaseUpgrade(heroine, row.ability0Base, row.ability0Tier1, "1"));
            row.ability0Tier2aButton.onClick.AddListener(() =>
                refugeManager.PurchaseUpgrade(heroine, row.ability0Tier1, row.ability0Tier2a, "2a"));
            row.ability0Tier2bButton.onClick.AddListener(() =>
                refugeManager.PurchaseUpgrade(heroine, row.ability0Tier1, row.ability0Tier2b, "2b"));

            // Ability 1 upgrade buttons
            row.ability1Tier1Button.onClick.AddListener(() =>
                refugeManager.PurchaseUpgrade(heroine, row.ability1Base, row.ability1Tier1, "1"));
            row.ability1Tier2aButton.onClick.AddListener(() =>
                refugeManager.PurchaseUpgrade(heroine, row.ability1Tier1, row.ability1Tier2a, "2a"));
            row.ability1Tier2bButton.onClick.AddListener(() =>
                refugeManager.PurchaseUpgrade(heroine, row.ability1Tier1, row.ability1Tier2b, "2b"));
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // Section switching
    // ════════════════════════════════════════════════════════════════════════

    private void SwitchSection(GameObject active)
    {
        recoverySection.SetActive(active == recoverySection);
        partySection.SetActive(active    == partySection);
        upgradesSection.SetActive(active == upgradesSection);
        gallerySection.SetActive(active  == gallerySection);
        knowledgeSection.SetActive(active == knowledgeSection);
    }

    // ════════════════════════════════════════════════════════════════════════
    // Refresh helpers
    // ════════════════════════════════════════════════════════════════════════

    private void RefreshAll()
    {
        RefreshBloom();
        RefreshRecovery();
        RefreshParty();
        RefreshUpgrades();
    }

    private void RefreshBloom()
    {
        bloomText.text = $"Bloom: {refugeManager.CurrentBloom}";
    }

    private void RefreshRecovery()
    {
        var party = runStateManager.Party;
        for (int i = 0; i < recoveryRows.Length && i < party.Count; i++)
        {
            var row    = recoveryRows[i];
            var heroine = party[i];

            row.nameLabel.text = heroine.displayName;

            row.restoreHPButton.interactable     = refugeManager.CanAfford(RefugeManager.COST_RESTORE_HP);
            row.restoreMPButton.interactable     = refugeManager.CanAfford(RefugeManager.COST_RESTORE_MP);
            row.restoreResolveButton.interactable = refugeManager.CanAfford(RefugeManager.COST_RESTORE_RESOLVE);
            row.reduceCorruptButton.interactable  = refugeManager.CanAfford(RefugeManager.COST_REDUCE_CORRUPT);
        }

        fullPartyButton.interactable = refugeManager.CanAfford(RefugeManager.COST_FULL_PARTY);
    }

    private void RefreshParty()
    {
        var party = runStateManager.Party;
        for (int i = 0; i < partyRows.Length && i < party.Count; i++)
        {
            var row    = partyRows[i];
            var h      = party[i];
            row.nameLabel.text       = h.displayName;
            row.hpLabel.text         = $"HP: {h.currentHP}/{h.maxHP}";
            row.mpLabel.text         = $"MP: {h.currentMP}/{h.maxMP}";
            row.resolveLabel.text    = $"Resolve: {h.resolve}/{h.maxResolve}";
            row.corruptionLabel.text = $"Corruption: {h.corruption}/100";
        }
    }

    private void RefreshUpgrades()
    {
        var party = runStateManager.Party;
        for (int i = 0; i < upgradeRows.Length && i < party.Count; i++)
        {
            var row    = upgradeRows[i];
            var heroine = party[i];

            row.nameLabel.text = heroine.displayName;

            RefreshAbilityUpgrade(heroine, 0,
                row.ability0Base, row.ability0Label,
                row.ability0Tier1Button, row.ability0Tier2aButton, row.ability0Tier2bButton);

            RefreshAbilityUpgrade(heroine, 1,
                row.ability1Base, row.ability1Label,
                row.ability1Tier1Button, row.ability1Tier2aButton, row.ability1Tier2bButton);
        }
    }

    // Refreshes one ability slot's label and button visibility/interactivity.
    private void RefreshAbilityUpgrade(RuntimeCharacterState heroine, int slot,
                                       CharacterAbilitySO baseAbility,
                                       Text label,
                                       Button tier1Btn, Button tier2aBtn, Button tier2bBtn)
    {
        if (baseAbility == null) { label.text = "(no ability)"; return; }

        string tier = refugeManager.GetUpgradeTier(heroine.characterId, baseAbility.abilityId);

        // Label shows current ability display name and tier tag
        string tierTag = tier switch { "1" => " [+]", "2a" => " [A]", "2b" => " [B]", _ => "" };
        label.text = baseAbility.displayName + tierTag;

        // Tier 1 button: visible and interactable only at tier 0
        tier1Btn.gameObject.SetActive(tier == "0");
        if (tier == "0")
            tier1Btn.interactable = refugeManager.CanUpgrade(heroine.characterId, baseAbility.abilityId, "1");

        // Tier 2 branch buttons: visible only at tier 1
        bool showBranches = tier == "1";
        tier2aBtn.gameObject.SetActive(showBranches);
        tier2bBtn.gameObject.SetActive(showBranches);
        if (showBranches)
        {
            tier2aBtn.interactable = refugeManager.CanUpgrade(heroine.characterId, baseAbility.abilityId, "2a");
            tier2bBtn.interactable = refugeManager.CanUpgrade(heroine.characterId, baseAbility.abilityId, "2b");
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // Start run
    // ════════════════════════════════════════════════════════════════════════

    private void OnStartRunClicked()
    {
        refugeManager.StartRun();
        refugePanel.SetActive(false);
    }
}
