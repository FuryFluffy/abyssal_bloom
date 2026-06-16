using System;
using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// ItemManager — runtime inventory manager.
//
// Attach to the _Managers GameObject alongside FlagManager.
// Holds the shared 6-slot item inventory for one run.
// Survives scene loads via DontDestroyOnLoad.
//
// Public API used by CombatManager, RefugeManager, and UI:
//   AddItem    / RemoveItem    / GetInventory / IsInventoryFull
//   UseItem(item, target)      — resolves all effects defined on ItemSO
//   ClearInventory             — called by RunStateManager on run end
//
// Events:
//   OnInventoryChanged         — fired after any add / remove / use
//   OnItemUsed(item, target)   — fired after UseItem resolves
// ════════════════════════════════════════════════════════════════════════════

public class ItemManager : MonoBehaviour
{
    // ── Singleton ──────────────────────────────────────────────────────────

    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ── Constants ──────────────────────────────────────────────────────────

    public const int MAX_INVENTORY = 6;

    // ── State ──────────────────────────────────────────────────────────────

    private readonly List<ItemSO> _inventory = new();

    // ── Events ─────────────────────────────────────────────────────────────

    /// <summary>Fired whenever the inventory contents change.</summary>
    public event Action OnInventoryChanged;

    /// <summary>Fired after UseItem resolves all effects.</summary>
    public event Action<ItemSO, RuntimeCharacterState> OnItemUsed;

    // ════════════════════════════════════════════════════════════════════════
    #region Inventory Access
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Returns a shallow copy so callers cannot mutate the internal list.</summary>
    public List<ItemSO> GetInventory() => new(_inventory);

    public int  GetInventoryCount() => _inventory.Count;
    public bool IsInventoryFull()   => _inventory.Count >= MAX_INVENTORY;
    public bool Contains(ItemSO item) => _inventory.Contains(item);

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Add / Remove
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Adds an item to the inventory.
    /// Returns true on success, false if the inventory is already full or item is null.
    /// </summary>
    public bool AddItem(ItemSO item)
    {
        if (item == null)
        {
            Debug.LogWarning("[ItemManager] AddItem: item is null.");
            return false;
        }
        if (IsInventoryFull())
        {
            Debug.Log($"[ItemManager] Inventory full — could not add {item.displayName}.");
            return false;
        }

        _inventory.Add(item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// Removes an item from the inventory.
    /// Returns true if found and removed, false if not present.
    /// </summary>
    public bool RemoveItem(ItemSO item)
    {
        if (!_inventory.Contains(item))
        {
            Debug.LogWarning($"[ItemManager] RemoveItem: {item?.displayName} not in inventory.");
            return false;
        }

        _inventory.Remove(item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>Empties the inventory. Call on run end / game over.</summary>
    public void ClearInventory()
    {
        _inventory.Clear();
        OnInventoryChanged?.Invoke();
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Use Item
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Resolves all effects of an item on a target RuntimeCharacterState.
    /// The item must already be in the inventory.
    /// Consumables are removed after use; CombatTools and Keys are not.
    ///
    /// Call from:
    ///   CombatManager.ResolveAction(UseItem) during combat
    ///   RefugeUI / EventUI for out-of-combat use
    /// </summary>
    public void UseItem(ItemSO item, RuntimeCharacterState target)
    {
        if (item == null)
        {
            Debug.LogWarning("[ItemManager] UseItem: item is null.");
            return;
        }
        if (!_inventory.Contains(item))
        {
            Debug.LogWarning($"[ItemManager] UseItem: {item.displayName} is not in inventory.");
            return;
        }
        if (target == null)
        {
            Debug.LogWarning($"[ItemManager] UseItem: target is null for {item.displayName}.");
            return;
        }

        // ── Numeric effects ────────────────────────────────────────────────

        if (item.healHP != 0)
            target.Heal(item.healHP);

        if (item.restoreMP != 0)
            target.RestoreMP(item.restoreMP);

        if (item.resolveChange > 0)
            target.RestoreResolve(item.resolveChange);
        else if (item.resolveChange < 0)
            target.LoseResolve(-item.resolveChange);

        if (item.corruptionChange > 0)
            target.GainCorruption(item.corruptionChange);
        // Negative corruptionChange (corruption reduction) is deferred — stub here.

        // ── Remove statuses ────────────────────────────────────────────────
        // Supports prefix wildcard: "Restrain*" removes all ids starting with "Restrain".

        if (item.statusesToRemove != null)
        {
            foreach (string pattern in item.statusesToRemove)
            {
                if (string.IsNullOrEmpty(pattern)) continue;

                if (pattern.EndsWith("*"))
                {
                    // Wildcard prefix removal
                    string prefix = pattern.Substring(0, pattern.Length - 1);
                    RemoveStatusesByPrefix(target, prefix);
                }
                else
                {
                    StatusEffectManager.Remove(target, pattern);
                }
            }
        }

        // ── Add statuses ───────────────────────────────────────────────────
        // StatusEffectSOs must be loaded from Resources/StatusEffects/<statusId>.asset
        // or via a registry (future). Currently loads via Resources.Load as a fallback.

        if (item.statusesToAdd != null)
        {
            foreach (string statusId in item.statusesToAdd)
            {
                if (string.IsNullOrEmpty(statusId)) continue;

                StatusEffectSO def = LoadStatusById(statusId);
                if (def != null)
                    StatusEffectManager.Apply(target, def, source: null);
                else
                    Debug.LogWarning($"[ItemManager] Could not find StatusEffectSO for id '{statusId}'.");
            }
        }

        // ── Grapple breaking ───────────────────────────────────────────────
        // Full implementation deferred until grapple-breaking item design pass.
        // Stub: CombatManager will need a BreakGrapple() method.

        if (item.canBreakGrapple)
        {
            // TODO: wire to CombatManager when grapple-break API is added.
            Debug.Log("[ItemManager] Grapple break triggered — BreakGrapple() not yet implemented.");
        }

        // ── Flag effects ───────────────────────────────────────────────────

        if (item.onUseFlags != null)
        {
            var fm = FlagManager.Instance;
            if (fm != null)
            {
                foreach (var effect in item.onUseFlags)
                {
                    if (!string.IsNullOrEmpty(effect.key))
                        fm.SetFlag(effect.scope, effect.key, effect.value);
                }
            }
        }

        // ── Consume ────────────────────────────────────────────────────────
        // Consumables are single-use. CombatTools and Keys persist.

        if (item.category == ItemCategory.Consumable)
        {
            _inventory.Remove(item);
            // OnInventoryChanged fired below via OnItemUsed path
        }

        OnItemUsed?.Invoke(item, target);
        OnInventoryChanged?.Invoke();
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Private Helpers
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Removes all active statuses on target whose statusId starts with prefix.
    /// Used for wildcard patterns like "Restrain*".
    /// </summary>
    private static void RemoveStatusesByPrefix(RuntimeCharacterState target, string prefix)
    {
        // Iterate backwards so removal is safe mid-loop.
        for (int i = target.activeStatuses.Count - 1; i >= 0; i--)
        {
            var inst = target.activeStatuses[i];
            if (inst?.definition != null &&
                inst.definition.statusId.StartsWith(
                    prefix, StringComparison.OrdinalIgnoreCase))
            {
                target.activeStatuses.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Attempts to load a StatusEffectSO by statusId from Resources.
    /// Expects assets at:  Resources/StatusEffects/<statusId>.asset
    ///
    /// A status registry / asset database lookup would be preferable for
    /// production — swap this method's body when that is built.
    /// </summary>
    private static StatusEffectSO LoadStatusById(string statusId)
    {
        return Resources.Load<StatusEffectSO>($"StatusEffects/{statusId}");
    }

    #endregion

    // ════════════════════════════════════════════════════════════════════════
    #region Save / Load Support
    // ════════════════════════════════════════════════════════════════════════

    // SaveSystem must call GetInventoryIds() before saving,
    // then RestoreInventory(ids) after loading, using a master item registry.

    /// <summary>Returns the itemId of every item in inventory (for serialisation).</summary>
    public List<string> GetInventoryIds()
    {
        var ids = new List<string>(_inventory.Count);
        foreach (var item in _inventory)
            ids.Add(item.itemId);
        return ids;
    }

    /// <summary>
    /// Restores inventory from a list of itemIds after a load.
    /// itemRegistry must map itemId → ItemSO (populate from all ItemSO assets).
    /// </summary>
    public void RestoreInventory(List<string> ids, Dictionary<string, ItemSO> itemRegistry)
    {
        _inventory.Clear();
        foreach (string id in ids)
        {
            if (itemRegistry.TryGetValue(id, out ItemSO item))
                _inventory.Add(item);
            else
                Debug.LogWarning($"[ItemManager] RestoreInventory: unknown itemId '{id}'.");
        }
        OnInventoryChanged?.Invoke();
    }


    // ── Lore Collection ────────────────────────────────────────────────────────
    // LoreItems accumulate in the run inventory and are transferred here
    // on Refuge return. Persistent across runs (saved with the save slot).

    private readonly List<ItemSO> _loreCollection = new();

    /// <summary>
    /// Moves all LoreItems from the run inventory to the persistent lore
    /// collection. Call on Refuge return (RunStateManager.ReturnToRefuge).
    /// Fires OnInventoryChanged after migration.
    /// </summary>
    public void MigrateLoreItems()
    {
        for (int i = _inventory.Count - 1; i >= 0; i--)
        {
            if (_inventory[i].category == ItemCategory.LoreItem)
            {
                _loreCollection.Add(_inventory[i]);
                _inventory.RemoveAt(i);
            }
        }
        OnInventoryChanged?.Invoke();
    }

    /// <summary>Returns the persistent lore collection (for Knowledge/Gallery UI).</summary>
    public List<ItemSO> GetLoreCollection() => new(_loreCollection);

    #endregion
}
