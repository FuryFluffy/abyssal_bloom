using System.Collections.Generic;
using UnityEngine;

// Attach this to a GameObject in your first scene (e.g. "_Managers").
// FlagManager.Instance is available from any script after Awake().
//
// Three scopes:
//   run_state          — cleared when the run ends or party wipes
//   save_slot          — persists across runs on the same save file
//   persistent_knowledge — never cleared (castle memory mechanic)
//
// Flags are string keys mapped to string values.
// Use "1" / "0" for booleans, or any string for richer state.
public class FlagManager : MonoBehaviour
{
    // ── Singleton ──────────────────────────────────────────────────────────
    public static FlagManager Instance { get; private set; }

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

    // ── Storage ────────────────────────────────────────────────────────────
    // Each scope is a plain Dictionary<string, string>.
    // SaveSystem serialises save_slot and persistent_knowledge to disk.
    // run_state is never saved — it vanishes when the process ends or ClearRunFlags() is called.

    private readonly Dictionary<string, string> _runState            = new();
    private readonly Dictionary<string, string> _saveSlot            = new();
    private readonly Dictionary<string, string> _persistentKnowledge = new();

    // ── Scope enum ─────────────────────────────────────────────────────────
    public enum Scope { RunState, SaveSlot, PersistentKnowledge }

    private Dictionary<string, string> GetDict(Scope scope) => scope switch
    {
        Scope.RunState            => _runState,
        Scope.SaveSlot            => _saveSlot,
        Scope.PersistentKnowledge => _persistentKnowledge,
        _                         => _runState
    };

    // ── Public API ─────────────────────────────────────────────────────────

    /// <summary>Set a flag. Pass value="1" for a simple boolean flag.</summary>
    public void SetFlag(Scope scope, string key, string value = "1")
    {
        GetDict(scope)[key] = value;
    }

    /// <summary>Get a flag value. Returns defaultValue if not set.</summary>
    public string GetFlag(Scope scope, string key, string defaultValue = "0")
    {
        return GetDict(scope).TryGetValue(key, out var val) ? val : defaultValue;
    }

    /// <summary>Convenience: returns true if flag == "1".</summary>
    public bool IsFlagSet(Scope scope, string key)
    {
        return GetFlag(scope, key) == "1";
    }

    /// <summary>Remove a single flag.</summary>
    public void RemoveFlag(Scope scope, string key)
    {
        GetDict(scope).Remove(key);
    }

    // ── Bulk clear methods ─────────────────────────────────────────────────

    /// <summary>Call on run end or party wipe. Clears run_state only.</summary>
    public void ClearRunFlags()
    {
        _runState.Clear();
        Debug.Log("[FlagManager] run_state cleared.");
    }

    /// <summary>
    /// Call when the player wipes a save slot.
    /// Clears run_state AND save_slot. persistent_knowledge survives.
    /// </summary>
    public void ClearSaveSlotFlags()
    {
        _runState.Clear();
        _saveSlot.Clear();
        Debug.Log("[FlagManager] run_state + save_slot cleared.");
    }

    /// <summary>Nuclear: wipes everything including persistent_knowledge.</summary>
    public void ClearAllFlags()
    {
        _runState.Clear();
        _saveSlot.Clear();
        _persistentKnowledge.Clear();
        Debug.Log("[FlagManager] All flags cleared.");
    }

    // ── Internal accessors for SaveSystem ─────────────────────────────────
    // SaveSystem calls these to serialise and restore flag data.

    internal Dictionary<string, string> GetSaveSlotDict()            => _saveSlot;
    internal Dictionary<string, string> GetPersistentKnowledgeDict() => _persistentKnowledge;

    internal void LoadSaveSlotDict(Dictionary<string, string> data)
    {
        _saveSlot.Clear();
        foreach (var kvp in data) _saveSlot[kvp.Key] = kvp.Value;
    }

    internal void LoadPersistentKnowledgeDict(Dictionary<string, string> data)
    {
        _persistentKnowledge.Clear();
        foreach (var kvp in data) _persistentKnowledge[kvp.Key] = kvp.Value;
    }
}
