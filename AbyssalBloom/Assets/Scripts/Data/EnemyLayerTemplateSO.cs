using UnityEngine;

// Create ONE asset for the entire game (Assets > Create > AbyssalBloom > Enemy Layer Template).
// Fill in all 10 rows in the Inspector.
[CreateAssetMenu(fileName = "EnemyLayerTemplate", menuName = "AbyssalBloom/Enemy Layer Template")]
public class EnemyLayerTemplateSO : ScriptableObject
{
    [System.Serializable]
    public struct LayerBaseStats
    {
        [Tooltip("Layer number 1–10 (informational only — index in the array is authoritative)")]
        public string label;        // e.g. "Layer 1"
        public int hp;
        public int mp;
        public int atk;
        public int mag;
        public int def;
        public int res;
        public int spd;
    }

    [Tooltip("Index 0 = Layer 1, index 9 = Layer 10")]
    public LayerBaseStats[] layers = new LayerBaseStats[10];

    // Convenience accessor — clamps so a bad layer value doesn't throw.
    public LayerBaseStats GetLayer(int layerNumber)
    {
        int idx = Mathf.Clamp(layerNumber - 1, 0, layers.Length - 1);
        return layers[idx];
    }
}
