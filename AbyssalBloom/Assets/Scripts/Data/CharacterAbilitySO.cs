using UnityEngine;

// Create one asset per ability (Assets > Create > AbyssalBloom > Character Ability).
// Heroines and enemies both use this SO for their abilities.
[CreateAssetMenu(fileName = "NewAbility", menuName = "AbyssalBloom/Character Ability")]
public class CharacterAbilitySO : ScriptableObject
{
    public enum AbilityType
    {
        Physical,
        Magic,
        Healing,
        ResolveAttack,
        CorruptionAttack,
        Grapple
    }

    [Header("Identity")]
    public string abilityId;
    public string displayName;
    [TextArea] public string description;

    [Header("Type & Cost")]
    public AbilityType abilityType = AbilityType.Physical;
    public int mpCost = 0;

    [Header("Damage")]
    public PowerBand powerBand = PowerBand.Medium;
    [Tooltip("Used by CombatFormulas.PowerMultiplier(). Read-only computed value shown for reference.")]
    public float Power => CombatFormulas.PowerMultiplier(powerBand);

    [Header("Accuracy (Physical only — magic auto-hits)")]
    [Range(0, 100)]
    public int baseHitChance = 90;

    [Header("Status Effect")]
    public StatusEffectSO statusEffect;
    [Range(0, 100)]
    [Tooltip("Base chance before MAG vs RES modifier. 0 = no status.")]
    public int statusBaseChance = 0;
    [Tooltip("Override status duration. -1 = use status SO default.")]
    public int statusDurationOverride = -1;

    [Header("Healing (Healing type only)")]
    [Tooltip("Base heal value before MAG × power modifier.")]
    public int baseHeal = 0;

    [Header("Resolve / Corruption (ResolveAttack / CorruptionAttack types)")]
    public int baseResolveDamage = 0;
    public int baseCorruptionGain = 0;

    [Header("Grapple")]
    [Tooltip("True = this ability initiates a grapple on hit.")]
    public bool isGrappleInitiator = false;
}
