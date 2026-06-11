using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// EncounterBuilder — test scaffolding, not the final roguelite room spawner.
// Attach to a GameObject in the combat scene alongside CombatManager.
// Populate Inspector fields, press Play, and the encounter starts immediately.
// ════════════════════════════════════════════════════════════════════════════

public class EncounterBuilder : MonoBehaviour
{
    // ── Inspector ──────────────────────────────────────────────────────────

    [Header("Combat Manager (same GameObject or drag reference)")]
    public CombatManager combatManager;

    [Header("Heroines — index 0 = starting Active heroine")]
    public CharacterDataSO[] heroineData = new CharacterDataSO[3];

    [Tooltip("One AbilityList per heroine. Each AbilityList holds that heroine's abilities.")]
    public HeroineAbilityList[] heroineAbilities = new HeroineAbilityList[3];

    [Header("Enemies")]
    public EnemyDataSO[] enemyData;

    [Tooltip("One AbilityList per enemy, matching enemyData order.")]
    public EnemyAbilityList[] enemyAbilities;

    // ── Unity serialisable wrapper types ──────────────────────────────────
    // Unity can't serialise jagged arrays (T[][]) in the Inspector.
    // Wrapping in a [Serializable] class is the standard workaround.

    [System.Serializable]
    public class HeroineAbilityList
    {
        public CharacterAbilitySO[] abilities;
    }

    [System.Serializable]
    public class EnemyAbilityList
    {
        public CharacterAbilitySO[] abilities;
    }

    // ── Lifecycle ──────────────────────────────────────────────────────────

    private void Start()
    {
        StartEncounter();
    }

    // Public so a menu button or test harness can also call it.
    public void StartEncounter()
    {
        if (combatManager == null)
        {
            Debug.LogError("[EncounterBuilder] CombatManager reference is missing.");
            return;
        }

        var heroines = BuildHeroines();
        var enemies  = BuildEnemies();

        if (heroines == null || enemies == null) return;

        combatManager.StartEncounter(heroines, enemies);
    }

    // ── Builders ───────────────────────────────────────────────────────────

    private List<RuntimeCharacterState> BuildHeroines()
    {
        if (heroineData == null || heroineData.Length != 3)
        {
            Debug.LogError("[EncounterBuilder] Exactly 3 heroineData slots required.");
            return null;
        }

        var list = new List<RuntimeCharacterState>(3);

        for (int i = 0; i < 3; i++)
        {
            var data = heroineData[i];
            if (data == null)
            {
                Debug.LogError($"[EncounterBuilder] heroineData[{i}] is null.");
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

            // Wire abilities
            state.abilities = new List<CharacterAbilitySO>();
            if (heroineAbilities != null && i < heroineAbilities.Length
                && heroineAbilities[i]?.abilities != null)
            {
                foreach (var ability in heroineAbilities[i].abilities)
                    if (ability != null)
                        state.abilities.Add(ability);
            }

            list.Add(state);
        }

        return list;
    }

    private List<RuntimeCharacterState> BuildEnemies()
    {
        if (enemyData == null || enemyData.Length == 0)
        {
            Debug.LogError("[EncounterBuilder] No enemyData provided.");
            return null;
        }

        var list = new List<RuntimeCharacterState>(enemyData.Length);

        for (int i = 0; i < enemyData.Length; i++)
        {
            var data = enemyData[i];
            if (data == null)
            {
                Debug.LogError($"[EncounterBuilder] enemyData[{i}] is null.");
                return null;
            }

            // Use EnemyDataSO.Final* properties — these apply the three-table
            // formula (layer base × rank multiplier × role modifier) or honour
            // any hand-tuned overrides set in the SO.
            var state = new RuntimeCharacterState
            {
                characterId  = data.enemyId,
                displayName  = data.displayName,
                isHeroine    = false,
                maxHP        = data.FinalHP,
                currentHP    = data.FinalHP,
                maxMP        = data.FinalMP,
                currentMP    = data.FinalMP,
                maxResolve   = 0,   // enemies have no Resolve track
                resolve      = 0,
                maxCorruption = 0,  // enemies have no Corruption track
                corruption   = 0,
                ATK          = data.FinalATK,
                MAG          = data.FinalMAG,
                DEF          = data.FinalDEF,
                RES          = data.FinalRES,
                SPD          = data.FinalSPD,
            };

            state.abilities = new List<CharacterAbilitySO>();
            if (enemyAbilities != null && i < enemyAbilities.Length
                && enemyAbilities[i]?.abilities != null)
            {
                foreach (var ability in enemyAbilities[i].abilities)
                    if (ability != null)
                        state.abilities.Add(ability);
            }

            list.Add(state);
        }

        return list;
    }
}
