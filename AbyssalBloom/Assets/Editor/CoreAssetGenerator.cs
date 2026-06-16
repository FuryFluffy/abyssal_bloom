// ── CoreAssetGenerator.cs ─────────────────────────────────────────────────
// EDITOR ONLY — place in Assets/Editor/
//
// Generates all core Layer 1 ScriptableObject assets in one click:
//   AbyssalBloom → Generate Core Layer 1 Assets
//
// Creates (skips if already exists):
//   3  CharacterDataSO         (Lysandra, Mira Voss, Seraphine)
//   1  EnemyLayerTemplateSO    (all 10 layer base stats)
//   1  EnemyRankMultiplierSO   (Standard / Elite / Major Boss / Final Boss)
//   1  EnemyRoleModifierSO     (6 roles)
//   6  EnemyDataSO             (Layer 1 enemies + Blood Nun boss)
//  18  CharacterAbilitySO      (6 heroine abilities + 12 enemy abilities)
//   1  EncounterPoolSO         (Layer 1 standard / elite / boss pools)
//   1  LayerGenerationProfileSO(Layer 1 map shape + room weights)
//
// All data sourced directly from 00_ABYSSAL_BLOOM_MASTER_REFERENCE.md.
// Stats are LOCKED — do not modify without updating the master reference.
// ──────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CoreAssetGenerator
{
    // ── Output roots ───────────────────────────────────────────────────────
    private const string ROOT         = "Assets/ScriptableObjects";
    private const string HEROINES     = ROOT + "/Heroines";
    private const string ENEMIES      = ROOT + "/Enemies/Layer1";
    private const string ABILITIES_H  = ROOT + "/Abilities/Heroines";
    private const string ABILITIES_E  = ROOT + "/Abilities/Enemies/Layer1";

    // Layer 2 folders
    private const string ENEMIES_L2   = ROOT + "/Enemies/Layer2";
    private const string ABILITIES_E2 = ROOT + "/Abilities/Enemies/Layer2";
    private const string TABLES       = ROOT + "/EnemyTables";
    private const string ENCOUNTERS   = ROOT + "/Encounters";
    private const string LAYERS       = ROOT + "/LayerProfiles";

    // ── Entry point ────────────────────────────────────────────────────────

    [MenuItem("AbyssalBloom/Generate Core Layer 1 Assets")]
    public static void GenerateAll()
    {
        EnsureFolders();

        GenerateEnemyTables();      // Must be first — enemies reference these
        GenerateHeroines();
        GenerateHeroineAbilities();
        GenerateEnemies();          // Must be before encounter pool
        GenerateEnemyAbilities();   // Must be before encounter pool
        GenerateEncounterPool();
        GenerateLayerProfile();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[CoreAssetGenerator] All Layer 1 core assets created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // ENEMY TABLES
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateEnemyTables()
    {
        // ── Layer Template (all 10 layers) ────────────────────────────────
        var tmpl = GetOrCreate<EnemyLayerTemplateSO>(TABLES, "EnemyLayerTemplate");
        tmpl.layers = new EnemyLayerTemplateSO.LayerBaseStats[]
        {
            new() { label="Layer 1",  hp=42,  mp=8,  atk=8,  mag=4,  def=4,  res=4,  spd=7  },
            new() { label="Layer 2",  hp=55,  mp=10, atk=10, mag=5,  def=5,  res=5,  spd=7  },
            new() { label="Layer 3",  hp=70,  mp=12, atk=12, mag=6,  def=6,  res=6,  spd=8  },
            new() { label="Layer 4",  hp=88,  mp=16, atk=14, mag=9,  def=8,  res=9,  spd=9  },
            new() { label="Layer 5",  hp=110, mp=20, atk=15, mag=12, def=9,  res=11, spd=9  },
            new() { label="Layer 6",  hp=135, mp=24, atk=18, mag=14, def=11, res=13, spd=10 },
            new() { label="Layer 7",  hp=165, mp=28, atk=20, mag=16, def=13, res=15, spd=11 },
            new() { label="Layer 8",  hp=200, mp=34, atk=22, mag=20, def=15, res=20, spd=10 },
            new() { label="Layer 9",  hp=240, mp=38, atk=26, mag=22, def=18, res=19, spd=11 },
            new() { label="Layer 10", hp=300, mp=45, atk=30, mag=28, def=22, res=24, spd=12 },
        };
        EditorUtility.SetDirty(tmpl);

        // ── Rank Multipliers ──────────────────────────────────────────────
        var rank = GetOrCreate<EnemyRankMultiplierSO>(TABLES, "EnemyRankMultiplier");
        rank.ranks = new EnemyRankMultiplierSO.RankMultiplier[]
        {
            new() { rank=EnemyRankMultiplierSO.Rank.Standard,  hpMult=1.0f, atkMagMult=1.00f, defResMult=1.0f, spdBonus=0 },
            new() { rank=EnemyRankMultiplierSO.Rank.Elite,     hpMult=1.8f, atkMagMult=1.25f, defResMult=1.2f, spdBonus=1 },
            new() { rank=EnemyRankMultiplierSO.Rank.MajorBoss, hpMult=4.2f, atkMagMult=1.55f, defResMult=1.4f, spdBonus=2 },
            new() { rank=EnemyRankMultiplierSO.Rank.FinalBoss, hpMult=6.0f, atkMagMult=1.70f, defResMult=1.6f, spdBonus=3 },
        };
        EditorUtility.SetDirty(rank);

        // ── Role Modifiers ────────────────────────────────────────────────
        // "—" in master ref = 1.0 (no change). SPD bonus 0 unless specified.
        var role = GetOrCreate<EnemyRoleModifierSO>(TABLES, "EnemyRoleModifier");
        role.roles = new EnemyRoleModifierSO.RoleModifier[]
        {
            new() { role=EnemyRoleModifierSO.Role.Bruiser,    hpMult=1.15f, atkMult=1.10f, magMult=1.00f, defMult=1.05f, resMult=1.00f, spdBonus=-1 },
            new() { role=EnemyRoleModifierSO.Role.Caster,     hpMult=1.00f, atkMult=1.00f, magMult=1.20f, defMult=0.90f, resMult=1.10f, spdBonus=0  },
            new() { role=EnemyRoleModifierSO.Role.Controller, hpMult=0.95f, atkMult=1.00f, magMult=1.10f, defMult=1.00f, resMult=1.15f, spdBonus=0  },
            new() { role=EnemyRoleModifierSO.Role.Skirmisher, hpMult=0.90f, atkMult=1.05f, magMult=1.00f, defMult=1.00f, resMult=1.00f, spdBonus=2  },
            new() { role=EnemyRoleModifierSO.Role.Tank,       hpMult=1.25f, atkMult=1.00f, magMult=1.00f, defMult=1.20f, resMult=1.00f, spdBonus=-2 },
            new() { role=EnemyRoleModifierSO.Role.Support,    hpMult=0.95f, atkMult=1.00f, magMult=1.10f, defMult=1.00f, resMult=1.00f, spdBonus=0  },
        };
        EditorUtility.SetDirty(role);

        Debug.Log("[CoreAssetGenerator] Enemy tables created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // HEROINES  (Section 3 of master reference)
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateHeroines()
    {
        CreateHeroine("lysandra",  "Lysandra",  "Dreadblade Duelist",
            hp:125, mp:32, atk:18, mag:6,  def:13, res:9,  spd:10);

        CreateHeroine("mira_voss", "Mira Voss", "Rogue / Alchemist",
            hp:105, mp:42, atk:13, mag:10, def:9,  res:11, spd:16);

        CreateHeroine("seraphine", "Seraphine", "Cleric / Exorcist",
            hp:95,  mp:58, atk:7,  mag:17, def:8,  res:16, spd:9);

        Debug.Log("[CoreAssetGenerator] Heroines created.");
    }

    private static void CreateHeroine(
        string id, string displayName, string description,
        int hp, int mp, int atk, int mag, int def, int res, int spd)
    {
        var so = GetOrCreate<CharacterDataSO>(HEROINES, id);
        so.characterId   = id;
        so.displayName   = displayName;
        so.description   = description;
        so.maxHP         = hp;
        so.maxMP         = mp;
        so.atk           = atk;
        so.mag           = mag;
        so.def           = def;
        so.res           = res;
        so.spd           = spd;
        so.maxResolve    = 100;
        so.maxCorruption = 100;
        EditorUtility.SetDirty(so);
    }

    // ══════════════════════════════════════════════════════════════════════
    // HEROINE ABILITIES  (Sections 7 + 9 of master reference)
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateHeroineAbilities()
    {
        // ── Lysandra — active abilities ───────────────────────────────────
        CreateAbility(ABILITIES_H, "dread_slash", "Dread Slash",
            "Primary free physical attack. No status.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.Medium, mpCost: 0, hitChance: 90);

        CreateAbility(ABILITIES_H, "crimson_lunge", "Crimson Lunge",
            "High-power physical. Applies Bleed.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.High, mpCost: 4, hitChance: 85,
            statusId: "bleed", statusChance: 45);

        // ── Lysandra — passives / assists ─────────────────────────────────
        CreateAbility(ABILITIES_H, "side_slash", "Side Slash",
            "Lysandra's assist attack. Very Low power.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.VeryLow, mpCost: 0, hitChance: 90);

        CreateAbility(ABILITIES_H, "battle_cry", "Battle Cry",
            "Lysandra's assist ability. Buffs active heroine ATK +3 for 2 turns.",
            type: CharacterAbilitySO.AbilityType.Buff,
            power: PowerBand.Medium, mpCost: 0,
            statusId: "atk_up", statusChance: 100,
            targetIsHeroine: true);

        // ── Mira Voss — active abilities ──────────────────────────────────
        CreateAbility(ABILITIES_H, "poisoned_dart", "Poisoned Dart",
            "Free physical. Applies Poison.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.Low, mpCost: 0, hitChance: 90,
            statusId: "poison", statusChance: 50);

        CreateAbility(ABILITIES_H, "acid_flask", "Acid Flask",
            "Magic attack. Applies DEF Down.",
            type: CharacterAbilitySO.AbilityType.Magic,
            power: PowerBand.Medium, mpCost: 5,
            statusId: "def_down", statusChance: 55);

        // ── Mira Voss — passives / assists ────────────────────────────────
        CreateAbility(ABILITIES_H, "quick_dart", "Quick Dart",
            "Mira's assist attack. Very Low power.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.VeryLow, mpCost: 0, hitChance: 90);

        CreateAbility(ABILITIES_H, "smelling_salts_assist", "Smelling Salts",
            "Mira's assist ability. Restores 5 Resolve to Active heroine.",
            type: CharacterAbilitySO.AbilityType.Buff,
            power: PowerBand.Medium, mpCost: 0,
            statusId: "resolve_restore_5", statusChance: 100,
            targetIsHeroine: true);

        // ── Seraphine — active abilities ──────────────────────────────────
        CreateAbility(ABILITIES_H, "holy_light", "Holy Light",
            "Auto-hit magic attack. No status.",
            type: CharacterAbilitySO.AbilityType.Magic,
            power: PowerBand.Medium, mpCost: 4);

        CreateAbility(ABILITIES_H, "mending_prayer", "Mending Prayer",
            "Healing ability. Base 8 + MAG × power.",
            type: CharacterAbilitySO.AbilityType.Healing,
            power: PowerBand.Low, mpCost: 5,
            baseHeal: 8);

        // ── Seraphine — passives / assists ────────────────────────────────
        CreateAbility(ABILITIES_H, "minor_smite", "Minor Smite",
            "Seraphine's assist attack. Very Low magic.",
            type: CharacterAbilitySO.AbilityType.Magic,
            power: PowerBand.VeryLow, mpCost: 0);

        CreateAbility(ABILITIES_H, "shield_of_faith", "Shield of Faith",
            "Seraphine's assist ability. Buffs active heroine RES +3 for 2 turns.",
            type: CharacterAbilitySO.AbilityType.Buff,
            power: PowerBand.Medium, mpCost: 0,
            statusId: "res_up", statusChance: 100,
            targetIsHeroine: true);

        Debug.Log("[CoreAssetGenerator] Heroine abilities created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // LAYER 1 ENEMIES  (Section 11 of master reference)
    // All use override stats — Layer 1 enemies are hand-tuned.
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateEnemies()
    {
        var tmpl = LoadAsset<EnemyLayerTemplateSO>(TABLES, "EnemyLayerTemplate");
        var rank = LoadAsset<EnemyRankMultiplierSO>(TABLES, "EnemyRankMultiplier");
        var role = LoadAsset<EnemyRoleModifierSO>  (TABLES, "EnemyRoleModifier");

        if (tmpl == null || rank == null || role == null)
        {
            Debug.LogError("[CoreAssetGenerator] Enemy table SOs missing — run GenerateEnemyTables first.");
            return;
        }

        CreateEnemy("hollow_servant",   "Hollow Servant",  "Mindless castle servant. Grappler.",
            tmpl, rank, role, layer:1,
            rankEnum: EnemyRankMultiplierSO.Rank.Standard,
            roleEnum: EnemyRoleModifierSO.Role.Bruiser,
            tag: "male-mindless",
            oHP:40, oMP:0, oATK:8, oMAG:3, oDEF:4, oRES:3, oSPD:6);

        CreateEnemy("knife_footman",    "Knife Footman",   "Fast physical DPS. Bleeds targets.",
            tmpl, rank, role, layer:1,
            rankEnum: EnemyRankMultiplierSO.Rank.Standard,
            roleEnum: EnemyRoleModifierSO.Role.Skirmisher,
            tag: "male-humanoid",
            oHP:36, oMP:6, oATK:11, oMAG:3, oDEF:3, oRES:3, oSPD:10);

        CreateEnemy("prayer_rag_novice","Prayer-Rag Novice","Resolve attacker. Whispers of doubt.",
            tmpl, rank, role, layer:1,
            rankEnum: EnemyRankMultiplierSO.Rank.Standard,
            roleEnum: EnemyRoleModifierSO.Role.Caster,
            tag: "male-humanoid",
            oHP:34, oMP:12, oATK:5, oMAG:8, oDEF:3, oRES:6, oSPD:7);

        CreateEnemy("corrupted_butler", "Corrupted Butler","Grapple specialist. Courteous threat.",
            tmpl, rank, role, layer:1,
            rankEnum: EnemyRankMultiplierSO.Rank.Standard,
            roleEnum: EnemyRoleModifierSO.Role.Controller,
            tag: "male-humanoid",
            oHP:44, oMP:8, oATK:9, oMAG:6, oDEF:5, oRES:5, oSPD:7);

        CreateEnemy("red_wax_acolyte",  "Red-Wax Acolyte", "Enemy support. Heals and buffs allies.",
            tmpl, rank, role, layer:1,
            rankEnum: EnemyRankMultiplierSO.Rank.Standard,
            roleEnum: EnemyRoleModifierSO.Role.Support,
            tag: "female-humanoid",
            oHP:30, oMP:14, oATK:5, oMAG:7, oDEF:3, oRES:6, oSPD:8);

        CreateEnemy("blood_nun",        "Blood Nun",        "Layer 1 boss. Justice incarnate.",
            tmpl, rank, role, layer:1,
            rankEnum: EnemyRankMultiplierSO.Rank.MajorBoss,
            roleEnum: EnemyRoleModifierSO.Role.Bruiser,
            tag: "female-humanoid",
            oHP:130, oMP:24, oATK:12, oMAG:10, oDEF:7, oRES:8, oSPD:8);

        Debug.Log("[CoreAssetGenerator] Layer 1 enemies created.");
    }

    private static void CreateEnemy(
        string id, string displayName, string description,
        EnemyLayerTemplateSO tmpl, EnemyRankMultiplierSO rank, EnemyRoleModifierSO role,
        int layer,
        EnemyRankMultiplierSO.Rank rankEnum,
        EnemyRoleModifierSO.Role  roleEnum,
        string tag,
        int oHP=0, int oMP=0, int oATK=0, int oMAG=0,
        int oDEF=0, int oRES=0, int oSPD=0)
    {
        var so = GetOrCreate<EnemyDataSO>(ENEMIES, id);
        so.enemyId       = id;
        so.displayName   = displayName;
        so.description   = description;
        so.tag           = tag;
        so.layerTemplate = tmpl;
        so.rankTable     = rank;
        so.roleTable     = role;
        so.layer         = layer;
        so.rank          = rankEnum;
        so.role          = roleEnum;
        so.overrideHP    = oHP;
        so.overrideMP    = oMP;
        so.overrideATK   = oATK;
        so.overrideMAG   = oMAG;
        so.overrideDEF   = oDEF;
        so.overrideRES   = oRES;
        so.overrideSPD   = oSPD;
        EditorUtility.SetDirty(so);
    }

    // ══════════════════════════════════════════════════════════════════════
    // ENEMY ABILITIES  (Section 11 of master reference)
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateEnemyAbilities()
    {
        // ── Hollow Servant ────────────────────────────────────────────────
        CreateAbility(ABILITIES_E, "shambling_strike", "Shambling Strike",
            "Hollow Servant normal attack. Medium physical.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.Medium, mpCost: 0, hitChance: 90,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E, "clutching_grab", "Clutching Grab",
            "Hollow Servant grapple attempt. 75% hit.",
            type: CharacterAbilitySO.AbilityType.Grapple,
            power: PowerBand.Medium, mpCost: 0, hitChance: 75,
            isGrappleInitiator: true, targetIsHeroine: true);

        // ── Knife Footman ─────────────────────────────────────────────────
        CreateAbility(ABILITIES_E, "quick_slash", "Quick Slash",
            "Knife Footman fallback attack.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.Medium, mpCost: 0, hitChance: 90,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E, "driven_thrust", "Driven Thrust",
            "Knife Footman high-power attack. Applies Bleed.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.High, mpCost: 3, hitChance: 85,
            statusId: "bleed", statusChance: 40,
            targetIsHeroine: true);

        // ── Prayer-Rag Novice ─────────────────────────────────────────────
        CreateAbility(ABILITIES_E, "dark_prayer", "Dark Prayer",
            "Novice magic fallback. Auto-hit.",
            type: CharacterAbilitySO.AbilityType.Magic,
            power: PowerBand.Medium, mpCost: 0,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E, "whisper_of_doubt", "Whisper of Doubt",
            "Combined Resolve + Corruption attack. Base 8 resolve dmg, 3 corruption.",
            type: CharacterAbilitySO.AbilityType.ResolveAttack,
            power: PowerBand.Medium, mpCost: 4,
            baseResolveDamage: 8, baseCorruptionGain: 3,
            targetIsHeroine: true);

        // ── Corrupted Butler ──────────────────────────────────────────────
        CreateAbility(ABILITIES_E, "silver_tray", "Silver Tray",
            "Butler weak fallback. Low physical.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.Low, mpCost: 0, hitChance: 90,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E, "courteous_embrace", "Courteous Embrace",
            "Butler grapple attempt. 80% hit.",
            type: CharacterAbilitySO.AbilityType.Grapple,
            power: PowerBand.Medium, mpCost: 4, hitChance: 80,
            isGrappleInitiator: true, targetIsHeroine: true);

        // ── Red-Wax Acolyte ───────────────────────────────────────────────
        CreateAbility(ABILITIES_E, "wax_drip", "Wax Drip",
            "Acolyte magic fallback. Auto-hit.",
            type: CharacterAbilitySO.AbilityType.Magic,
            power: PowerBand.Medium, mpCost: 0,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E, "crimson_blessing", "Crimson Blessing",
            "Acolyte ally heal. Heals 12 HP. Targets enemy ally.",
            type: CharacterAbilitySO.AbilityType.Healing,
            power: PowerBand.Low, mpCost: 4,
            baseHeal: 12, targetIsHeroine: false);

        CreateAbility(ABILITIES_E, "ember_ward", "Ember Ward",
            "Acolyte ally buff. Applies def_up_enemy (+3 DEF, 2t) to ally.",
            type: CharacterAbilitySO.AbilityType.Buff,
            power: PowerBand.Medium, mpCost: 3,
            statusId: "def_up_enemy", statusChance: 100,
            targetIsHeroine: false);

        // ── Blood Nun (Boss) ──────────────────────────────────────────────
        CreateAbility(ABILITIES_E, "flagellants_lash", "Flagellant's Lash",
            "Blood Nun free spammable physical. Applies Bleed.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.High, mpCost: 0, hitChance: 90,
            statusId: "bleed", statusChance: 50,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E, "communion", "Communion",
            "Blood Nun combined Resolve + Corruption auto-hit. Base 10 resolve, 4 corruption.",
            type: CharacterAbilitySO.AbilityType.ResolveAttack,
            power: PowerBand.Medium, mpCost: 4,
            baseResolveDamage: 10, baseCorruptionGain: 4,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E, "sanctified_embrace", "Sanctified Embrace",
            "Blood Nun grapple. 85% hit.",
            type: CharacterAbilitySO.AbilityType.Grapple,
            power: PowerBand.Medium, mpCost: 6, hitChance: 85,
            isGrappleInitiator: true, targetIsHeroine: true);

        CreateAbility(ABILITIES_E, "blood_rite", "Blood Rite",
            "Blood Nun self-heal 15 HP. Only usable below 50% HP.",
            type: CharacterAbilitySO.AbilityType.Healing,
            power: PowerBand.Medium, mpCost: 6,
            baseHeal: 15, hpThreshold: 0.5f,
            targetIsHeroine: false);   // targets self (enemy)

        Debug.Log("[CoreAssetGenerator] Enemy abilities created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // ENCOUNTER POOL — Layer 1
    // Standard: Battle rooms. Elite: Elite rooms. Boss: Blood Nun.
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateEncounterPool()
    {
        var pool = GetOrCreate<EncounterPoolSO>(ENCOUNTERS, "EncounterPool_Layer1");

        // Load enemy SOs
        var hollowServant   = LoadAsset<EnemyDataSO>(ENEMIES, "hollow_servant");
        var knifeFootman    = LoadAsset<EnemyDataSO>(ENEMIES, "knife_footman");
        var novice          = LoadAsset<EnemyDataSO>(ENEMIES, "prayer_rag_novice");
        var butler          = LoadAsset<EnemyDataSO>(ENEMIES, "corrupted_butler");
        var acolyte         = LoadAsset<EnemyDataSO>(ENEMIES, "red_wax_acolyte");
        var bloodNun        = LoadAsset<EnemyDataSO>(ENEMIES, "blood_nun");

        // Load ability SOs
        var shambling       = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "shambling_strike");
        var clutching       = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "clutching_grab");
        var quickSlash      = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "quick_slash");
        var drivenThrust    = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "driven_thrust");
        var darkPrayer      = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "dark_prayer");
        var whisper         = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "whisper_of_doubt");
        var silverTray      = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "silver_tray");
        var courteousEmbr   = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "courteous_embrace");
        var waxDrip         = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "wax_drip");
        var crimsonBless    = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "crimson_blessing");
        var emberWard       = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "ember_ward");
        var lash            = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "flagellants_lash");
        var communion       = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "communion");
        var embrace         = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "sanctified_embrace");
        var bloodRite       = LoadAsset<CharacterAbilitySO>(ABILITIES_E, "blood_rite");

        // ── Standard Pool ─────────────────────────────────────────────────
        // Group A: 2× Hollow Servant
        // Group B: 1× Knife Footman
        // Group C: 1× Hollow Servant + 1× Knife Footman
        // Group D: 1× Prayer-Rag Novice
        // Group E: 1× Hollow Servant + 1× Prayer-Rag Novice
        // Group F: 1× Corrupted Butler

        pool.standardPool = new EncounterPoolSO.EnemyGroup[]
        {
            MakeGroup("2× Hollow Servant", weight:10,
                new[]{hollowServant, hollowServant},
                new[]{ new[]{shambling,clutching}, new[]{shambling,clutching} }),

            MakeGroup("1× Knife Footman", weight:10,
                new[]{knifeFootman},
                new[]{ new[]{quickSlash,drivenThrust} }),

            MakeGroup("Hollow Servant + Knife Footman", weight:10,
                new[]{hollowServant, knifeFootman},
                new[]{ new[]{shambling,clutching}, new[]{quickSlash,drivenThrust} }),

            MakeGroup("1× Prayer-Rag Novice", weight:8,
                new[]{novice},
                new[]{ new[]{darkPrayer,whisper} }),

            MakeGroup("Hollow Servant + Novice", weight:8,
                new[]{hollowServant, novice},
                new[]{ new[]{shambling,clutching}, new[]{darkPrayer,whisper} }),

            MakeGroup("1× Corrupted Butler", weight:7,
                new[]{butler},
                new[]{ new[]{silverTray,courteousEmbr} }),
        };

        // ── Elite Pool ────────────────────────────────────────────────────
        // Group A: 1× Corrupted Butler + 1× Red-Wax Acolyte
        // Group B: 2× Knife Footman
        // Group C: 1× Corrupted Butler + 1× Prayer-Rag Novice

        pool.elitePool = new EncounterPoolSO.EnemyGroup[]
        {
            MakeGroup("Butler + Red-Wax Acolyte", weight:10,
                new[]{butler, acolyte},
                new[]{ new[]{silverTray,courteousEmbr}, new[]{waxDrip,crimsonBless,emberWard} }),

            MakeGroup("2× Knife Footman", weight:8,
                new[]{knifeFootman, knifeFootman},
                new[]{ new[]{quickSlash,drivenThrust}, new[]{quickSlash,drivenThrust} }),

            MakeGroup("Butler + Novice", weight:8,
                new[]{butler, novice},
                new[]{ new[]{silverTray,courteousEmbr}, new[]{darkPrayer,whisper} }),
        };

        // ── Boss Pool ─────────────────────────────────────────────────────
        pool.bossPool = new EncounterPoolSO.EnemyGroup[]
        {
            MakeGroup("Blood Nun", weight:10,
                new[]{bloodNun},
                new[]{ new[]{lash,communion,embrace,bloodRite} }),
        };

        EditorUtility.SetDirty(pool);
        Debug.Log("[CoreAssetGenerator] Encounter pool created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // LAYER GENERATION PROFILE — Layer 1
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateLayerProfile()
    {
        var profile = GetOrCreate<LayerGenerationProfileSO>(LAYERS, "LayerProfile_Layer1");

        profile.layerNumber       = 1;
        profile.minNodes          = 8;
        profile.maxNodes          = 12;
        profile.startingPaths     = 2;
        profile.minDepth          = 4;
        profile.minRoomsBeforeBoss = 4;

        // Room type weights — Layer 1 is tutorial-paced
        profile.weightBattle        = 30;
        profile.weightElite         = 10;
        profile.weightEvent         = 15;
        profile.weightLoreDiscovery = 5;
        profile.weightRiskReward    = 10;
        profile.weightFalseRest     = 5;
        profile.weightKeyMechanism  = 5;

        // Wire encounter pool
        var pool = LoadAsset<EncounterPoolSO>(ENCOUNTERS, "EncounterPool_Layer1");
        if (pool != null) profile.encounterPool = pool;

        EditorUtility.SetDirty(profile);
        Debug.Log("[CoreAssetGenerator] Layer 1 profile created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // SHARED ABILITY FACTORY
    // ══════════════════════════════════════════════════════════════════════

    private static void CreateAbility(
        string folder,
        string id,
        string displayName,
        string description,
        CharacterAbilitySO.AbilityType type   = CharacterAbilitySO.AbilityType.Physical,
        PowerBand power                        = PowerBand.Medium,
        int mpCost                             = 0,
        int hitChance                          = 90,
        string statusId                        = null,
        int statusChance                       = 0,
        int baseHeal                           = 0,
        int baseResolveDamage                  = 0,
        int baseCorruptionGain                 = 0,
        bool isGrappleInitiator                = false,
        bool targetIsHeroine                   = true,
        float hpThreshold                      = 0f)
    {
        var so = GetOrCreate<CharacterAbilitySO>(folder, id);
        so.abilityId         = id;
        so.displayName       = displayName;
        so.description       = description;
        so.abilityType       = type;
        so.powerBand         = power;
        so.mpCost            = mpCost;
        so.baseHitChance     = hitChance;
        so.baseHeal          = baseHeal;
        so.baseResolveDamage = baseResolveDamage;
        so.baseCorruptionGain= baseCorruptionGain;
        so.isGrappleInitiator= isGrappleInitiator;
        so.targetIsHeroine   = targetIsHeroine;
        so.hpThresholdToUse  = hpThreshold;

        // Wire status SO if provided
        if (!string.IsNullOrEmpty(statusId))
        {
            so.statusBaseChance = statusChance;
            // Attempt to load the status SO from StatusEffects resource path
            var statusSO = Resources.Load<StatusEffectSO>($"StatusEffects/{statusId}");
            if (statusSO != null)
                so.statusEffect = statusSO;
            else
            {
                // Try AssetDatabase search (editor-only fallback)
                string[] guids = AssetDatabase.FindAssets($"t:StatusEffectSO {statusId}");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    so.statusEffect = AssetDatabase.LoadAssetAtPath<StatusEffectSO>(path);
                }
                else
                {
                    Debug.LogWarning($"[CoreAssetGenerator] StatusEffectSO '{statusId}' not found. " +
                                     $"Run StatusEffectAssetGenerator first, then re-run this generator.");
                }
            }
        }

        EditorUtility.SetDirty(so);
    }

    // ══════════════════════════════════════════════════════════════════════
    // HELPERS
    // ══════════════════════════════════════════════════════════════════════

    /// <summary>Load or create a ScriptableObject at folder/filename.asset</summary>
    private static T GetOrCreate<T>(string folder, string filename) where T : ScriptableObject
    {
        string path = $"{folder}/{filename}.asset";
        var existing = AssetDatabase.LoadAssetAtPath<T>(path);
        if (existing != null) return existing;

        var so = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(so, path);
        return so;
    }

    /// <summary>Load an existing asset — returns null if not found.</summary>
    private static T LoadAsset<T>(string folder, string filename) where T : ScriptableObject
    {
        string path = $"{folder}/{filename}.asset";
        var asset = AssetDatabase.LoadAssetAtPath<T>(path);
        if (asset == null)
            Debug.LogWarning($"[CoreAssetGenerator] Could not load {path}");
        return asset;
    }

    /// <summary>Build an EncounterPoolSO.EnemyGroup from parallel arrays.</summary>
    private static EncounterPoolSO.EnemyGroup MakeGroup(
        string label,
        int weight,
        EnemyDataSO[] enemies,
        CharacterAbilitySO[][] abilitySets)
    {
        var abilityLists = new EncounterPoolSO.EnemyAbilityList[abilitySets.Length];
        for (int i = 0; i < abilitySets.Length; i++)
            abilityLists[i] = new EncounterPoolSO.EnemyAbilityList { abilities = abilitySets[i] };

        return new EncounterPoolSO.EnemyGroup
        {
            label          = label,
            weight         = weight,
            enemies        = enemies,
            enemyAbilities = abilityLists,
        };
    }

    /// <summary>Ensure all output folders exist.</summary>
    private static void EnsureFolders()
    {
        string[] folders =
        {
            "Assets/ScriptableObjects",
            HEROINES,
            ENEMIES,
            ABILITIES_H,
            "Assets/ScriptableObjects/Abilities",
            "Assets/ScriptableObjects/Abilities/Enemies",
            ABILITIES_E,
            TABLES,
            ENCOUNTERS,
            LAYERS,
        };

        foreach (var folder in folders)
        {
            if (AssetDatabase.IsValidFolder(folder)) continue;
            string[] parts  = folder.Split('/');
            string current  = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
    // ══════════════════════════════════════════════════════════════════════
    // LAYER 2 ENTRY POINT
    // ══════════════════════════════════════════════════════════════════════

    [MenuItem("AbyssalBloom/Generate Core Layer 2 Assets")]
    public static void GenerateLayer2()
    {
        EnsureFolders();
        EnsureLayer2Folders();

        GenerateLayer2Enemies();
        GenerateLayer2EnemyAbilities();
        GenerateLayer2EncounterPool();
        GenerateLayer2Profile();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[CoreAssetGenerator] All Layer 2 core assets created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // LAYER 2 ENEMIES
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateLayer2Enemies()
    {
        var tmpl = LoadAsset<EnemyLayerTemplateSO>(TABLES, "EnemyLayerTemplate");
        var rank = LoadAsset<EnemyRankMultiplierSO>(TABLES, "EnemyRankMultiplier");
        var role = LoadAsset<EnemyRoleModifierSO>  (TABLES, "EnemyRoleModifier");

        if (tmpl == null || rank == null || role == null)
        {
            Debug.LogError("[CoreAssetGenerator] Enemy table SOs missing — run GenerateAll first.");
            return;
        }

        // ── Standard enemies ──────────────────────────────────────────────
        CreateEnemyL2("dungeon_warden",   "Dungeon Warden",
            "An older model. Moves in patterns. Doesn't improvise.",
            tmpl, rank, role,
            rankEnum: EnemyRankMultiplierSO.Rank.Standard,
            roleEnum: EnemyRoleModifierSO.Role.Controller,
            tag: "male-construct",
            oHP:50, oMP:6, oATK:9, oMAG:4, oDEF:6, oRES:5, oSPD:6);

        CreateEnemyL2("pale_attendant",   "Pale Attendant",
            "Maintains the dungeon. Attentive. Disturbingly cheerful.",
            tmpl, rank, role,
            rankEnum: EnemyRankMultiplierSO.Rank.Standard,
            roleEnum: EnemyRoleModifierSO.Role.Support,
            tag: "male-humanoid",
            oHP:38, oMP:16, oATK:4, oMAG:9, oDEF:4, oRES:7, oSPD:8);

        CreateEnemyL2("sealed_thing",     "Sealed Thing",
            "Whatever it was, it's been here longer than the Castle's current purpose.",
            tmpl, rank, role,
            rankEnum: EnemyRankMultiplierSO.Rank.Standard,
            roleEnum: EnemyRoleModifierSO.Role.Bruiser,
            tag: "undead",
            oHP:62, oMP:0, oATK:13, oMAG:2, oDEF:5, oRES:3, oSPD:5);

        // ── Boss ──────────────────────────────────────────────────────────
        CreateEnemyL2("the_jailer",       "The Jailer",
            "Layer 2 boss. Controller/Tank. He was built here.",
            tmpl, rank, role,
            rankEnum: EnemyRankMultiplierSO.Rank.MajorBoss,
            roleEnum: EnemyRoleModifierSO.Role.Tank,
            tag: "male-construct",
            oHP:220, oMP:28, oATK:14, oMAG:8, oDEF:16, oRES:10, oSPD:5);

        Debug.Log("[CoreAssetGenerator] Layer 2 enemies created.");
    }

    /// <summary>Creates an EnemyDataSO in the Layer 2 folder.</summary>
    private static void CreateEnemyL2(
        string id, string displayName, string description,
        EnemyLayerTemplateSO tmpl, EnemyRankMultiplierSO rank, EnemyRoleModifierSO role,
        EnemyRankMultiplierSO.Rank rankEnum,
        EnemyRoleModifierSO.Role  roleEnum,
        string tag,
        int oHP=0, int oMP=0, int oATK=0, int oMAG=0,
        int oDEF=0, int oRES=0, int oSPD=0)
    {
        var so = GetOrCreate<EnemyDataSO>(ENEMIES_L2, id);
        so.enemyId       = id;
        so.displayName   = displayName;
        so.description   = description;
        so.tag           = tag;
        so.layerTemplate = tmpl;
        so.rankTable     = rank;
        so.roleTable     = role;
        so.layer         = 2;
        so.rank          = rankEnum;
        so.role          = roleEnum;
        so.overrideHP    = oHP;
        so.overrideMP    = oMP;
        so.overrideATK   = oATK;
        so.overrideMAG   = oMAG;
        so.overrideDEF   = oDEF;
        so.overrideRES   = oRES;
        so.overrideSPD   = oSPD;
        EditorUtility.SetDirty(so);
    }

    // ══════════════════════════════════════════════════════════════════════
    // LAYER 2 ENEMY ABILITIES
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateLayer2EnemyAbilities()
    {
        // ── Dungeon Warden ────────────────────────────────────────────────
        CreateAbility(ABILITIES_E2, "warden_strike", "Warden Strike",
            "Dungeon Warden normal attack. Medium physical.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.Medium, mpCost: 0, hitChance: 90,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E2, "chain_throw", "Chain Throw",
            "Dungeon Warden grapple attempt. 70% hit.",
            type: CharacterAbilitySO.AbilityType.Grapple,
            power: PowerBand.Medium, mpCost: 0, hitChance: 70,
            isGrappleInitiator: true, targetIsHeroine: true);

        // ── Pale Attendant ────────────────────────────────────────────────
        CreateAbility(ABILITIES_E2, "pale_touch", "Pale Touch",
            "Pale Attendant magic attack. Auto-hit.",
            type: CharacterAbilitySO.AbilityType.Magic,
            power: PowerBand.Low, mpCost: 0,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E2, "attendant_mending", "Attendant Mending",
            "Pale Attendant ally heal. Heals 14 HP. Targets ally.",
            type: CharacterAbilitySO.AbilityType.Healing,
            power: PowerBand.Low, mpCost: 4,
            baseHeal: 14, targetIsHeroine: false);

        CreateAbility(ABILITIES_E2, "soothing_words", "Soothing Words",
            "Pale Attendant Resolve attack. Patiently explains why you should stop resisting.",
            type: CharacterAbilitySO.AbilityType.ResolveAttack,
            power: PowerBand.Medium, mpCost: 5,
            baseResolveDamage: 8, baseCorruptionGain: 2,
            targetIsHeroine: true);

        // ── Sealed Thing ──────────────────────────────────────────────────
        CreateAbility(ABILITIES_E2, "thrashing_blow", "Thrashing Blow",
            "Sealed Thing heavy physical. It has been waiting a long time.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.High, mpCost: 0, hitChance: 80,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E2, "desperate_grab", "Desperate Grab",
            "Sealed Thing grapple. Low power, 65% hit.",
            type: CharacterAbilitySO.AbilityType.Grapple,
            power: PowerBand.Low, mpCost: 0, hitChance: 65,
            isGrappleInitiator: true, targetIsHeroine: true);

        // ── The Jailer (Boss) ─────────────────────────────────────────────
        CreateAbility(ABILITIES_E2, "iron_admonishment", "Iron Admonishment",
            "A restrained blow. He doesn't want to hurt you.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.Low, mpCost: 0, hitChance: 85,
            statusId: "restrained", statusChance: 40,
            targetIsHeroine: true);

        CreateAbility(ABILITIES_E2, "gentle_hold", "Gentle Hold",
            "His hands close carefully. He is trying not to break anything.",
            type: CharacterAbilitySO.AbilityType.Grapple,
            power: PowerBand.Medium, mpCost: 0, hitChance: 80,
            isGrappleInitiator: true, targetIsHeroine: true);

        CreateAbility(ABILITIES_E2, "weight_of_order", "Weight of Order",
            "He explains, patiently, why this is necessary. The logic is impeccable and wrong.",
            type: CharacterAbilitySO.AbilityType.ResolveAttack,
            power: PowerBand.Medium, mpCost: 6,
            baseResolveDamage: 12, baseCorruptionGain: 3,
            targetIsHeroine: true);

        // Phase 2 ability — CombatManager AI checks HP threshold before selecting this.
        CreateAbility(ABILITIES_E2, "i_did_not_want_this", "I Did Not Want This",
            "Phase 2. He stops holding back. His voice doesn't change.",
            type: CharacterAbilitySO.AbilityType.Physical,
            power: PowerBand.High, mpCost: 8, hitChance: 90,
            baseResolveDamage: 8,
            targetIsHeroine: true);

        Debug.Log("[CoreAssetGenerator] Layer 2 enemy abilities created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // ENCOUNTER POOL — Layer 2
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateLayer2EncounterPool()
    {
        var pool = GetOrCreate<EncounterPoolSO>(ENCOUNTERS, "EncounterPool_Layer2");

        // Load enemy SOs
        var warden      = LoadAsset<EnemyDataSO>(ENEMIES_L2, "dungeon_warden");
        var attendant   = LoadAsset<EnemyDataSO>(ENEMIES_L2, "pale_attendant");
        var sealedThing = LoadAsset<EnemyDataSO>(ENEMIES_L2, "sealed_thing");
        var jailer      = LoadAsset<EnemyDataSO>(ENEMIES_L2, "the_jailer");

        // Load ability SOs
        var wardenStrike    = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "warden_strike");
        var chainThrow      = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "chain_throw");
        var paleTouch       = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "pale_touch");
        var attMending      = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "attendant_mending");
        var soothingWords   = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "soothing_words");
        var thrashingBlow   = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "thrashing_blow");
        var desperateGrab   = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "desperate_grab");
        var ironAdmon       = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "iron_admonishment");
        var gentleHold      = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "gentle_hold");
        var weightOfOrder   = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "weight_of_order");
        var didNotWant      = LoadAsset<CharacterAbilitySO>(ABILITIES_E2, "i_did_not_want_this");

        // ── Standard Pool ─────────────────────────────────────────────────
        pool.standardPool = new EncounterPoolSO.EnemyGroup[]
        {
            MakeGroup("1× Dungeon Warden", weight:10,
                new[]{ warden },
                new[]{ new[]{ wardenStrike, chainThrow } }),

            MakeGroup("1× Pale Attendant", weight:8,
                new[]{ attendant },
                new[]{ new[]{ paleTouch, attMending, soothingWords } }),

            MakeGroup("Dungeon Warden + Pale Attendant", weight:9,
                new[]{ warden, attendant },
                new[]{ new[]{ wardenStrike, chainThrow }, new[]{ paleTouch, attMending, soothingWords } }),

            MakeGroup("1× Sealed Thing", weight:7,
                new[]{ sealedThing },
                new[]{ new[]{ thrashingBlow, desperateGrab } }),
        };

        // ── Elite Pool ────────────────────────────────────────────────────
        pool.elitePool = new EncounterPoolSO.EnemyGroup[]
        {
            MakeGroup("Warden + Attendant + Sealed Thing", weight:10,
                new[]{ warden, attendant, sealedThing },
                new[]{ new[]{ wardenStrike, chainThrow }, new[]{ paleTouch, attMending, soothingWords }, new[]{ thrashingBlow, desperateGrab } }),

            MakeGroup("2× Sealed Thing", weight:8,
                new[]{ sealedThing, sealedThing },
                new[]{ new[]{ thrashingBlow, desperateGrab }, new[]{ thrashingBlow, desperateGrab } }),
        };

        // ── Boss Pool ─────────────────────────────────────────────────────
        pool.bossPool = new EncounterPoolSO.EnemyGroup[]
        {
            MakeGroup("The Jailer", weight:10,
                new[]{ jailer },
                new[]{ new[]{ ironAdmon, gentleHold, weightOfOrder, didNotWant } }),
        };

        EditorUtility.SetDirty(pool);
        Debug.Log("[CoreAssetGenerator] Layer 2 encounter pool created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // LAYER GENERATION PROFILE — Layer 2
    // ══════════════════════════════════════════════════════════════════════

    private static void GenerateLayer2Profile()
    {
        var profile = GetOrCreate<LayerGenerationProfileSO>(LAYERS, "LayerProfile_Layer2");

        profile.layerNumber        = 2;
        profile.minNodes           = 7;
        profile.maxNodes           = 10;
        profile.startingPaths      = 1;   // single main corridor
        profile.minDepth           = 3;
        profile.minRoomsBeforeBoss = 3;

        profile.weightBattle        = 30;
        profile.weightElite         = 12;
        profile.weightEvent         = 18;
        profile.weightLoreDiscovery = 8;
        profile.weightRiskReward    = 10;
        profile.weightFalseRest     = 7;
        profile.weightKeyMechanism  = 5;

        var pool = LoadAsset<EncounterPoolSO>(ENCOUNTERS, "EncounterPool_Layer2");
        if (pool != null) profile.encounterPool = pool;

        EditorUtility.SetDirty(profile);
        Debug.Log("[CoreAssetGenerator] Layer 2 profile created.");
    }

    // ══════════════════════════════════════════════════════════════════════
    // LAYER 2 FOLDER SETUP
    // ══════════════════════════════════════════════════════════════════════

    private static void EnsureLayer2Folders()
    {
        string[] folders =
        {
            ENEMIES_L2,
            ABILITIES_E2,
        };

        foreach (var folder in folders)
        {
            if (AssetDatabase.IsValidFolder(folder)) continue;
            string[] parts = folder.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }

}
#endif
