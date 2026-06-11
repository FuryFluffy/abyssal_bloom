using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// ── Data containers (serialised to JSON via JsonUtility) ───────────────────
// JsonUtility cannot serialise Dictionary directly.
// We use flat key/value pair lists and convert on the way in and out.

[Serializable]
public class FlagEntry
{
    public string key;
    public string value;
}

[Serializable]
public class RunSaveData
{
    // Which save slot this belongs to (0, 1, 2 …)
    public int slotIndex;

    // Party: characterIds of the three heroines in order [active, support1, support2]
    public string[] partyIds = new string[3];

    // Current room / layer progress
    public int currentLayer = 1;
    public string currentRoomId;

    // Per-heroine runtime state (parallel arrays — index matches partyIds)
    public int[]   currentHP      = new int[3];
    public int[]   currentMP      = new int[3];
    public int[]   currentResolve = new int[3];
    public int[]   currentCorrupt = new int[3];

    // Run-time flags are NOT saved here — they're intentionally transient.
    // (run_state is always reconstructed fresh each run.)

    // Save-slot flags
    public List<FlagEntry> saveSlotFlags = new();

    public string timestamp;
}

[Serializable]
public class MetaSaveData
{
    // persistent_knowledge flags survive all runs and save-slot resets.
    public List<FlagEntry> persistentFlags = new();

    // Any other cross-slot meta (unlocked heroines, etc.) can go here later.
}

// ── SaveSystem ─────────────────────────────────────────────────────────────
// Static helper — no MonoBehaviour needed.
// Files land in Application.persistentDataPath (per-user, writable on all platforms).
//
// DECISION POINT: Meta-progression (persistent_knowledge) is stored in a
// separate JSON file rather than PlayerPrefs. Reasons:
//   • No 1 MB PlayerPrefs size limit risk as flags accumulate.
//   • Easier to inspect/debug the file directly.
//   • If you prefer PlayerPrefs for meta, swap out SaveMeta / LoadMeta below.
public static class SaveSystem
{
    private const string RunFilePrefix  = "run_slot_";   // e.g. run_slot_0.json
    private const string MetaFile       = "meta.json";

    // ── Paths ──────────────────────────────────────────────────────────────
    private static string RunPath(int slotIndex) =>
        Path.Combine(Application.persistentDataPath, $"{RunFilePrefix}{slotIndex}.json");

    private static string MetaPath =>
        Path.Combine(Application.persistentDataPath, MetaFile);

    // ── Save run state ─────────────────────────────────────────────────────
    public static void SaveRun(RunSaveData data)
    {
        if (FlagManager.Instance != null)
            data.saveSlotFlags = DictToList(FlagManager.Instance.GetSaveSlotDict());

        data.timestamp = DateTime.UtcNow.ToString("o");

        try
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(RunPath(data.slotIndex), json);
            Debug.Log($"[SaveSystem] Run saved → slot {data.slotIndex}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveSystem] SaveRun failed: {e.Message}");
        }
    }

    // ── Load run state ─────────────────────────────────────────────────────
    public static RunSaveData LoadRun(int slotIndex)
    {
        string path = RunPath(slotIndex);
        if (!File.Exists(path))
        {
            Debug.Log($"[SaveSystem] No run file for slot {slotIndex}.");
            return null;
        }

        try
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<RunSaveData>(json);

            if (FlagManager.Instance != null)
                FlagManager.Instance.LoadSaveSlotDict(ListToDict(data.saveSlotFlags));

            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveSystem] LoadRun failed: {e.Message}");
            return null;
        }
    }

    // ── Delete run (on party wipe / new run) ───────────────────────────────
    public static void DeleteRun(int slotIndex)
    {
        string path = RunPath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[SaveSystem] Run file deleted for slot {slotIndex}.");
        }

        FlagManager.Instance?.ClearSaveSlotFlags();
    }

    // ── Save meta-progression ──────────────────────────────────────────────
    public static void SaveMeta()
    {
        var data = new MetaSaveData();

        if (FlagManager.Instance != null)
            data.persistentFlags = DictToList(FlagManager.Instance.GetPersistentKnowledgeDict());

        try
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(MetaPath, json);
            Debug.Log("[SaveSystem] Meta saved.");
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveSystem] SaveMeta failed: {e.Message}");
        }
    }

    // ── Load meta-progression ──────────────────────────────────────────────
    public static void LoadMeta()
    {
        if (!File.Exists(MetaPath))
        {
            Debug.Log("[SaveSystem] No meta file found — fresh start.");
            return;
        }

        try
        {
            string json = File.ReadAllText(MetaPath);
            var data = JsonUtility.FromJson<MetaSaveData>(json);

            if (FlagManager.Instance != null)
                FlagManager.Instance.LoadPersistentKnowledgeDict(ListToDict(data.persistentFlags));

            Debug.Log("[SaveSystem] Meta loaded.");
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveSystem] LoadMeta failed: {e.Message}");
        }
    }

    // ── Helpers ────────────────────────────────────────────────────────────
    private static List<FlagEntry> DictToList(Dictionary<string, string> dict)
    {
        var list = new List<FlagEntry>(dict.Count);
        foreach (var kvp in dict)
            list.Add(new FlagEntry { key = kvp.Key, value = kvp.Value });
        return list;
    }

    private static Dictionary<string, string> ListToDict(List<FlagEntry> list)
    {
        var dict = new Dictionary<string, string>(list.Count);
        foreach (var entry in list)
            dict[entry.key] = entry.value;
        return dict;
    }
}
