// ── StatusEffectAssetGenerator.cs ─────────────────────────────────────────
// EDITOR ONLY — lives in Assets/Editor/
// Usage: Unity menu → AbyssalBloom → Generate All Status Effect SOs
//
// Creates all 45 StatusEffectSO assets in Assets/ScriptableObjects/Statuses/
// organised into subfolders by group.
// Safe to run multiple times — skips assets that already exist.
// ──────────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class StatusEffectAssetGenerator
{
    private const string ROOT = "Assets/ScriptableObjects/Statuses";

    [MenuItem("AbyssalBloom/Generate All Status Effect SOs")]
    public static void GenerateAll()
    {
        // ── Physical ──────────────────────────────────────────────────────
        Create("Physical", "bleed", "Bleed",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            dot: 3, dotResolve: false,
            effects:    "Deals 3 HP damage per turn at turn start. Blood Nun variant also uses 3/t; Knife Footman uses 2/t (set per ability SO).");

        Create("Physical", "burning", "Burning",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            dot: 2, dotResolve: false,
            effects:    "Deals 2 HP fire damage per turn. Extra pressure on root/slime/Bloom/rope/vine enemies.");

        Create("Physical", "off_balance", "Off-Balance",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modDef: -2,
            effects:    "Reduces evasion/defense; makes restraint follow-ups more likely.");

        Create("Physical", "overcommitted", "Overcommitted",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Heroine overextended — next physical action has reduced hit chance.");

        // ── Generic Setup ─────────────────────────────────────────────────
        Create("GenericSetup", "marked", "Marked",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Target easier to hit or apply setup effects against.");

        Create("GenericSetup", "guarded", "Guarded",
            category:   StatusEffectSO.Category.Buff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modDef: 3,
            effects:    "Reduces incoming damage from next hit. Core defensive status.");

        // ── Restraint (tiered — ReplaceWeaker) ───────────────────────────
        Create("Restraint", "snared", "Snared",
            category:   StatusEffectSO.Category.Restraint,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.ReplaceWeaker,
            modSpd: -2,
            effects:    "Light restraint. Reduces movement/evasion; weakens Escape chance.");

        Create("Restraint", "restrained", "Restrained",
            category:   StatusEffectSO.Category.Restraint,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.ReplaceWeaker,
            modSpd: -3,
            effects:    "Medium restraint. Restricts physical skills and Escape.");

        Create("Restraint", "bound", "Bound",
            category:   StatusEffectSO.Category.Restraint,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.ReplaceWeaker,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Restricts action list; blocks Escape. Enables rescue/struggle/item choices.");

        Create("Restraint", "held", "Held",
            category:   StatusEffectSO.Category.Restraint,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.ReplaceWeaker,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Strong restraint. Limits attacks and escape.");

        Create("Restraint", "roped", "Roped",
            category:   StatusEffectSO.Category.Restraint,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Bell/rope restraint. Interacts with bell timing or Kneeling.");

        Create("Restraint", "wrapped", "Wrapped",
            category:   StatusEffectSO.Category.Restraint,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Cloth/cradle restraint. Increases grief/silence pressure.");

        // ── Mental / Social ───────────────────────────────────────────────
        Create("Mental", "uneasy", "Uneasy",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.NextPressure, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Slightly increases next Resolve/social/comfort pressure check.");

        Create("Mental", "shaken", "Shaken",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modRes: -2,
            effects:    "Increases Resolve damage taken or reduces Resolve recovery.");

        Create("Mental", "wavering", "Wavering",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modRes: -3,
            effects:    "Reduces Resolve resistance; weakens truth-response options in combat.");

        Create("Mental", "self_conscious", "Self-Conscious",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Reduces social/composure resistance; empowers Court/appraisal follow-ups.");

        Create("Mental", "suppressed", "Suppressed",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.NextPressure, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Emotional narrowing. Reduces special options or Resolve recovery.");

        Create("Mental", "hushed", "Hushed",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Weakens or disables speech/prayer/call support actions.");

        Create("Mental", "clinical_strain", "Clinical Strain",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.NextAction, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Next analysis/tool action may cost more MP or Resolve. Mira pressure.");

        Create("Mental", "filed", "Filed",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Castle/Archive classification pressure. Restricts identity/choice options. Does NOT set castle_reinterpretation_flag.");

        // ── Comfort / Bloom ───────────────────────────────────────────────
        Create("ComfortBloom", "comforted", "Comforted",
            category:   StatusEffectSO.Category.Comfort,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modDef: 2,
            effects:    "Reduces immediate pressure but increases comfort/Bloom/Castle vulnerability. Does not set hidden feeding flags.");

        Create("ComfortBloom", "sheltered", "Sheltered",
            category:   StatusEffectSO.Category.Comfort,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            modDef: 2,
            effects:    "Reduces incoming pressure. May strengthen Castle safety arguments.");

        Create("ComfortBloom", "soothing_contact", "Soothing Contact",
            category:   StatusEffectSO.Category.Comfort,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Minor recovery/pressure reduction. Increases vulnerability to Bloom drowsing or Yielding.");

        Create("ComfortBloom", "drowsy_bloom", "Drowsy Bloom",
            category:   StatusEffectSO.Category.Comfort,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.ReplaceWeaker,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            modSpd: -2,
            effects:    "Light Bloom drowsing. Reduces action priority or weakens choices.");

        Create("ComfortBloom", "bloom_drowsed", "Bloom-Drowsed",
            category:   StatusEffectSO.Category.Comfort,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.ReplaceWeaker,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            modSpd: -4,
            effects:    "Strong Bloom drowsing. Restricts strongest actions or delays initiative.");

        Create("ComfortBloom", "petal_mark", "Petal Mark",
            category:   StatusEffectSO.Category.Comfort,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Bloom enemies prioritize or empower effects against target.");

        Create("ComfortBloom", "yielding", "Yielding",
            category:   StatusEffectSO.Category.Comfort,
            durationType: StatusEffectSO.DurationType.Room, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Reduces resistance to comfort/safety offers. Weakens refusal unless truth context strong. Temporary — not a persistent route flag.");

        Create("ComfortBloom", "kept", "Kept",
            category:   StatusEffectSO.Category.Comfort,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Final Castle containment pressure. Restricts escape/refusal options temporarily. Status only — does NOT trigger an ending.");

        // ── Court ─────────────────────────────────────────────────────────
        Create("Court", "appraised", "Appraised",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Court enemies exploit target role or vulnerability. Increases social pressure.");

        Create("Court", "court_role", "Court Role",
            category:   StatusEffectSO.Category.Buff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 3,
            stacking:   StatusEffectSO.StackingRule.ExclusiveGroup,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Grants role-specific benefit AND role-specific vulnerability. Useful but dangerous.");

        Create("Court", "audience_favor", "Audience Favor",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Court enemies gain bonus from crowd attention.");

        Create("Court", "applause", "Applause",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Applause stacks fuel Velvet Regent's threshold abilities.");

        // ── Mirror / Archive ──────────────────────────────────────────────
        Create("MirrorArchive", "echo_delay", "Echo Delay",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modSpd: -2,
            effects:    "Timing disruption from mirror reflection. Reduces initiative.");

        // ── Blessed ───────────────────────────────────────────────────────
        Create("Blessed", "stained", "Stained",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Marked as impure by Blessed enemies. Increases judgment pressure.");

        Create("Blessed", "excised", "Excised",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Forcibly purged. Corruption removed at a cost — suppresses special options.");

        Create("Blessed", "kneeling", "Kneeling",
            category:   StatusEffectSO.Category.Restraint,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Forced supplication. Restricts movement actions.");

        Create("Blessed", "sealed", "Sealed",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Route denial. Blocks specific passage or ability.");

        Create("Blessed", "weight_of_purity", "Weight of Purity",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            modSpd: -2, modDef: -2,
            effects:    "Purity burden. Reduces speed and defense under judgment pressure.");

        // ── Blood / Wax / Bell ────────────────────────────────────────────
        Create("BloodWaxBell", "blood_wax_resonance", "Blood-Wax Resonance",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Blood-wax mark setup. Enables blood-wax payoff moves.");

        Create("BloodWaxBell", "blood_wax_sentence", "Blood-Wax Sentence",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            dot: 4, dotResolve: false,
            effects:    "Delayed punishment. Deals 4 HP damage per turn while active.");

        Create("BloodWaxBell", "revenge_lure", "Revenge Lure",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Tempts heroine toward revenge route. Increases Corruption pressure.");

        Create("BloodWaxBell", "fervor", "Fervor",
            category:   StatusEffectSO.Category.Buff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modAtk: 2,
            targetHeroine: false,
            effects:    "Enemy fervor buff. Increases ATK when denial/threshold triggers fire.");

        Create("BloodWaxBell", "bell_sleepers_timing", "Bell Sleeper's Timing",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            modSpd: -3,
            effects:    "Bell timing disruption. Severely reduces initiative.");

        // ── Sky / Route ───────────────────────────────────────────────────
        Create("SkyRoute", "misrouted", "Misrouted",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "False-route confusion. Reduces ability to find correct path options.");

        Create("SkyRoute", "sky_worn", "Sky-Worn",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modSpd: -1,
            effects:    "Fatigue from rooftop exposure. Minor SPD penalty.");

        Create("SkyRoute", "weightless", "Weightless",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            modDef: -3,
            effects:    "Unmoored from ground. Dangerous DEF reduction from ascetic wind prayer.");

        Create("SkyRoute", "perched", "Perched",
            category:   StatusEffectSO.Category.Buff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            targetHeroine: false,
            modAtk: 2,
            effects:    "Enemy on high ground. ATK bonus while perched.");

        Create("SkyRoute", "loop_momentum", "Loop Momentum",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Caught in Horizon-Fold loop. Cumulative loop pressure.");

        // ── Final Argument ────────────────────────────────────────────────
        Create("FinalArgument", "mercy_charge", "Mercy Charge",
            category:   StatusEffectSO.Category.Buff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            targetHeroine: false,
            modAtk: 3, modMag: 3,
            effects:    "Castle final system buff. Strengthens the Mercy Engine Avatar's composite attacks.");

        Create("FinalArgument", "argument_strength", "Argument Strength",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            visibility: StatusEffectSO.Visibility.VisibleWarning,
            effects:    "Castle Heart's argument gaining weight. Weakens final refusal options.");

        // ── Battlefield ───────────────────────────────────────────────────
        Create("Battlefield", "obscured", "Obscured",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 1,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            effects:    "Target partially hidden. Reduces hit chance against them.");

        // ── Debuffs (stat-only, used by abilities) ────────────────────────
        Create("Debuff", "def_down", "DEF Down",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 3,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            modDef: -3,
            effects:    "Reduces target DEF by 3 for 3 turns. Applied by Mira's Acid Flask.");

        Create("Debuff", "poison", "Poison",
            category:   StatusEffectSO.Category.Debuff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 3,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            dot: 2, dotResolve: false,
            effects:    "Deals 2 HP damage per turn for 3 turns. Applied by Mira's Poisoned Dart.");

        // ── Enemy Buffs ───────────────────────────────────────────────────
        Create("EnemyBuff", "def_up_enemy", "DEF Up",
            category:   StatusEffectSO.Category.Buff,
            durationType: StatusEffectSO.DurationType.Turns, duration: 2,
            stacking:   StatusEffectSO.StackingRule.RefreshDuration,
            targetHeroine: false,
            modDef: 3,
            effects:    "Increases enemy DEF by 3 for 2 turns. Applied by Red-Wax Acolyte's Ember Ward.");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[StatusEffectAssetGenerator] All status effect SOs created successfully.");
    }

    // ── Factory ───────────────────────────────────────────────────────────────

    private static void Create(
        string subfolder,
        string id,
        string displayName,
        StatusEffectSO.Category category         = StatusEffectSO.Category.Debuff,
        StatusEffectSO.DurationType durationType = StatusEffectSO.DurationType.Turns,
        int duration                             = 2,
        StatusEffectSO.StackingRule stacking     = StatusEffectSO.StackingRule.RefreshDuration,
        StatusEffectSO.Visibility visibility     = StatusEffectSO.Visibility.VisibleUI,
        bool targetHeroine                       = true,
        int dot                                  = 0,
        bool dotResolve                          = false,
        int modAtk = 0, int modDef = 0, int modRes = 0, int modSpd = 0, int modMag = 0,
        string effects                           = "")
    {
        string folder = $"{ROOT}/{subfolder}";
        string path   = $"{folder}/{id}.asset";

        // Skip if already exists
        if (AssetDatabase.LoadAssetAtPath<StatusEffectSO>(path) != null)
        {
            Debug.Log($"[StatusEffectAssetGenerator] Skipped (already exists): {path}");
            return;
        }

        // Ensure folder exists
        if (!AssetDatabase.IsValidFolder(folder))
        {
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

        var so = ScriptableObject.CreateInstance<StatusEffectSO>();
        so.statusId          = id;
        so.displayName       = displayName;
        so.group             = subfolder;
        so.category          = category;
        so.targetHeroine     = targetHeroine;
        so.visibility        = visibility;
        so.durationType      = durationType;
        so.defaultDuration   = duration;
        so.stackingRule      = stacking;
        so.maxStacks         = 1;
        so.removable         = true;
        so.mechanicalEffects = effects;
        so.dotAmountPerTick  = dot;
        so.dotTargetsResolve = dotResolve;
        so.modATK            = modAtk;
        so.modDEF            = modDef;
        so.modRES            = modRes;
        so.modSPD            = modSpd;
        so.modMAG            = modMag;

        AssetDatabase.CreateAsset(so, path);
        Debug.Log($"[StatusEffectAssetGenerator] Created: {path}");
    }
}
#endif
