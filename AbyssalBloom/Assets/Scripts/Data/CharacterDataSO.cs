using UnityEngine;

// Create via: Assets > Create > AbyssalBloom > Character Data
// One asset per heroine. Drag into party roster or ability references.
[CreateAssetMenu(fileName = "NewCharacter", menuName = "AbyssalBloom/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [Header("Identity")]
    public string characterId;          // e.g. "lysandra" — used by FlagManager and SaveSystem
    public string displayName;
    [TextArea] public string description;

    [Header("Base Combat Stats")]
    public int maxHP      = 100;
    public int maxMP      = 30;
    public int atk        = 10;
    public int mag        = 10;
    public int def        = 8;
    public int res        = 8;
    public int spd        = 8;

    [Header("Resolve & Corruption")]
    public int maxResolve    = 100;     // Always 100 per design doc
    public int maxCorruption = 100;     // Always 100 per design doc

    // Runtime state is NOT stored here — this SO is read-only reference data.
    // RuntimeCharacterState.cs (not a SO) holds current HP/MP/Resolve/Corruption mid-run.
}
