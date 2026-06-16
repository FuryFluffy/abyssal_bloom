using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// GameBootstrap — startup sequencing
// ════════════════════════════════════════════════════════════════════════════
// Attach to a GameObject in the first scene (e.g. "_Bootstrap").
// Runs once on Start() in the correct dependency order:
//   1. SaveSystem loads persistent data (meta + run)
//   2. FlagManager is ready (singleton, already initialised in its Awake)
//   3. If a saved run exists: rebuild party from save data, resume map
//   4. If no save: build starter party, show Refuge
// ════════════════════════════════════════════════════════════════════════════

public class GameBootstrap : MonoBehaviour
{
    // ── Inspector ──────────────────────────────────────────────────────────

    [Header("Manager References")]
    public RunStateManager runStateManager;
    public RefugeManager   refugeManager;
    public RoomManager     roomManager;

    [Header("Starter Party (assign CharacterDataSOs for the 3 starting heroines)")]
    [Tooltip("Lysandra, Mira Voss, Seraphine — in that order.")]
    public CharacterDataSO[] starterHeroineData = new CharacterDataSO[3];

    [Header("Starter Abilities (2 per heroine, index matches starterHeroineData)")]
    public CharacterAbilitySO[] lysandraStartAbilities = new CharacterAbilitySO[2];
    public CharacterAbilitySO[] miraStartAbilities      = new CharacterAbilitySO[2];
    public CharacterAbilitySO[] seraphineStartAbilities = new CharacterAbilitySO[2];

    [Header("UI Root Panels")]
    public GameObject refugePanel;
    public GameObject combatPanel;
    public GameObject mapPanel;
    public GameObject eventPanel;

    // ── Startup ────────────────────────────────────────────────────────────

    private void Start()
    {
        // Step 1 — hide all panels
        SetPanelVisibility(refugePanel, false);
        SetPanelVisibility(combatPanel, false);
        SetPanelVisibility(mapPanel,    false);
        SetPanelVisibility(eventPanel,  false);

        // Step 2 — load save data (returns null if no save exists)
        var saveData = SaveSystem.Load();

        // Step 3 — resume or start fresh
        if (saveData != null && saveData.partyIds != null && saveData.partyIds.Length == 3)
            ResumeRun(saveData);
        else
            StartFresh();
    }

    // ── Fresh start ────────────────────────────────────────────────────────

    private void StartFresh()
    {
        var party = BuildStarterParty();
        if (party == null)
        {
            Debug.LogError("[GameBootstrap] Starter heroine data not assigned — " +
                           "drag CharacterDataSOs into the Inspector.");
            return;
        }

        // StartNewRun sets IsRunActive = true; ReturnToRefuge seeds Bloom
        // and calls RefugeManager.InitialiseFromRunState().
        // We then immediately call ReturnToRefuge so the player starts
        // in the Refuge before their first run.
        runStateManager?.StartNewRun(party);
        runStateManager?.ReturnToRefuge();

        SetPanelVisibility(refugePanel, true);
        Debug.Log("[GameBootstrap] Fresh start — showing Refuge.");
    }

    // ── Resume saved run ───────────────────────────────────────────────────

    private void ResumeRun(RunSaveData saveData)
    {
        // FIX 3: Rebuild RuntimeCharacterState party from saved data +
        // SO definitions. SaveSystem only serialises IDs and stat snapshots —
        // we must reconstruct the full runtime objects here.

        var party = RebuildPartyFromSave(saveData);
        if (party == null)
        {
            Debug.LogWarning("[GameBootstrap] Could not rebuild party from save — " +
                             "falling back to fresh start.");
            StartFresh();
            return;
        }

        // Rehydrate RunStateManager with the rebuilt party and saved layer
        // without resetting Bloom (the run is already in progress).
        runStateManager?.StartNewRun(party, saveData.runSeed);

        // Restore layer (StartNewRun sets it to 1)
        // We call AdvanceLayer the right number of times to reach the saved layer.
        if (runStateManager != null)
        {
            int targetLayer = Mathf.Max(1, saveData.currentLayer);
            while (runStateManager.CurrentLayer < targetLayer)
                runStateManager.AdvanceLayer();
        }

        // Regenerate map from the saved seed
        if (roomManager != null)
        {
            roomManager.GenerateCurrentLayer();
            SetPanelVisibility(mapPanel, true);
            Debug.Log($"[GameBootstrap] Run resumed — layer {saveData.currentLayer}, " +
                      $"showing map.");
        }
        else
        {
            Debug.LogWarning("[GameBootstrap] RoomManager not assigned — showing Refuge.");
            SetPanelVisibility(refugePanel, true);
        }
    }

    // ── Party rebuild from save data ───────────────────────────────────────

    private List<RuntimeCharacterState> RebuildPartyFromSave(RunSaveData saveData)
    {
        var abilitySets = new[]
        {
            lysandraStartAbilities,
            miraStartAbilities,
            seraphineStartAbilities
        };

        var party = new List<RuntimeCharacterState>(3);

        for (int i = 0; i < 3; i++)
        {
            // Find the matching SO by characterId
            CharacterDataSO data = FindHeroineData(saveData.partyIds[i]);
            if (data == null)
            {
                Debug.LogError($"[GameBootstrap] No CharacterDataSO found for " +
                               $"'{saveData.partyIds[i]}'. Cannot resume.");
                return null;
            }

            var state = new RuntimeCharacterState
            {
                characterId   = data.characterId,
                displayName   = data.displayName,
                isHeroine     = true,
                maxHP         = data.maxHP,
                maxMP         = data.maxMP,
                maxResolve    = data.maxResolve,
                maxCorruption = data.maxCorruption,
                // Restore saved runtime values
                currentHP  = saveData.currentHP[i],
                currentMP  = saveData.currentMP[i],
                resolve    = saveData.currentResolve[i],
                corruption = saveData.currentCorrupt[i],
            };
            state.BaseATK = data.atk;
            state.BaseMAG = data.mag;
            state.BaseDEF = data.def;
            state.BaseRES = data.res;
            state.BaseSPD = data.spd;

            // Wire abilities — use the starter set for now.
            // TODO: When upgrade tier flags are loaded, resolve upgraded ability SOs.
            // For now the starter abilities are always wired; upgrade effects are
            // tracked in FlagManager SaveSlot but the actual SO swap needs to be
            // re-applied here once ability upgrade SOs are authored.
            state.abilities = new List<CharacterAbilitySO>();
            var abilitySet  = (i < abilitySets.Length) ? abilitySets[i] : null;
            if (abilitySet != null)
                foreach (var ab in abilitySet)
                    if (ab != null) state.abilities.Add(ab);

            party.Add(state);
        }

        return party;
    }

    // Find a CharacterDataSO from starterHeroineData by characterId
    private CharacterDataSO FindHeroineData(string characterId)
    {
        if (starterHeroineData == null) return null;
        foreach (var data in starterHeroineData)
            if (data != null && data.characterId == characterId)
                return data;
        return null;
    }

    // ── Starter party builder (fresh start only) ───────────────────────────

    private List<RuntimeCharacterState> BuildStarterParty()
    {
        if (starterHeroineData == null || starterHeroineData.Length < 3)
            return null;

        var abilitySets = new[]
        {
            lysandraStartAbilities,
            miraStartAbilities,
            seraphineStartAbilities
        };

        var party = new List<RuntimeCharacterState>(3);

        for (int i = 0; i < 3; i++)
        {
            var data = starterHeroineData[i];
            if (data == null)
            {
                Debug.LogError($"[GameBootstrap] starterHeroineData[{i}] is null.");
                return null;
            }

            var state = new RuntimeCharacterState
            {
                characterId   = data.characterId,
                displayName   = data.displayName,
                isHeroine     = true,
                maxHP         = data.maxHP,
                currentHP     = data.maxHP,
                maxMP         = data.maxMP,
                currentMP     = data.maxMP,
                maxResolve    = data.maxResolve,
                resolve       = data.maxResolve,
                maxCorruption = data.maxCorruption,
                corruption    = 0,
            };
            state.BaseATK = data.atk;
            state.BaseMAG = data.mag;
            state.BaseDEF = data.def;
            state.BaseRES = data.res;
            state.BaseSPD = data.spd;

            state.abilities = new List<CharacterAbilitySO>();
            var abilitySet  = abilitySets[i];
            if (abilitySet != null)
                foreach (var ab in abilitySet)
                    if (ab != null) state.abilities.Add(ab);

            party.Add(state);
        }

        return party;
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    private static void SetPanelVisibility(GameObject panel, bool visible)
    {
        if (panel != null) panel.SetActive(visible);
    }
}
