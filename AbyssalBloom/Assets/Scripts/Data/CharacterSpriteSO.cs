using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// CharacterSpriteSO — sprite set for one heroine or enemy.
// ════════════════════════════════════════════════════════════════════════════
// Create via: Assets → Create → AbyssalBloom → Character Sprites
//
// One asset per character. Link from CharacterDataSO or EnemyDataSO, or
// look up at runtime by characterId via a registry.
//
// CombatUI uses this to swap sprites as combat state changes:
//   idle       — default stance, shown at turn start
//   attack     — shown briefly when the character acts
//   grappled_* — shown during grapple scene stages 1/2/3
//   defeated   — shown at 0 HP
//
// Stage 4+ sprites are reserved for future adult content passes.
// Leave null for any sprite not yet imported — CombatUI will skip
// the swap silently if the field is null.
// ════════════════════════════════════════════════════════════════════════════

[CreateAssetMenu(fileName = "NewCharacterSprites", menuName = "AbyssalBloom/Character Sprites")]
public class CharacterSpriteSO : ScriptableObject
{
    // ── Identity ───────────────────────────────────────────────────────────

    [Header("Identity")]
    [Tooltip("Must match the characterId on CharacterDataSO or EnemyDataSO.")]
    public string characterId;

    // ── Combat Sprites ─────────────────────────────────────────────────────

    [Header("Combat")]
    [Tooltip("Default standing pose. Shown at turn start and when idle.")]
    public Sprite idle;

    [Tooltip("Attack or casting pose. Shown briefly when the character acts.")]
    public Sprite attack;

    [Tooltip("Shown when HP drops to 0.")]
    public Sprite defeated;

    // ── Grapple / H-Scene Stages ───────────────────────────────────────────

    [Header("Grapple Stages")]
    [Tooltip("Stage 1 — initial grapple.")]
    public Sprite grappled_stage1;

    [Tooltip("Stage 2 — escalated grapple.")]
    public Sprite grappled_stage2;

    [Tooltip("Stage 3 — climax.")]
    public Sprite grappled_stage3;

    // Stage 4+ reserved — requires Corruption ≥ 40. Populated in adult content pass.

    // ── Helper ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns the correct grapple sprite for a given scene stage.
    /// Falls back to the previous stage if the requested one is null.
    /// Returns null if no grapple sprites are assigned at all.
    /// </summary>
    public Sprite GetGrappleSprite(int stage)
    {
        Sprite result = stage switch
        {
            1 => grappled_stage1,
            2 => grappled_stage2 != null ? grappled_stage2 : grappled_stage1,
            _ => grappled_stage3 != null ? grappled_stage3
               : grappled_stage2 != null ? grappled_stage2
               : grappled_stage1,
        };
        return result;
    }
}
