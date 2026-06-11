using System;
using System.Collections.Generic;
using UnityEngine;

// ════════════════════════════════════════════════════════════════════════════
// RoomManager — runtime map owner and room flow controller
// ════════════════════════════════════════════════════════════════════════════
// Attach to the same "_Managers" GameObject.
// Owns the generated map, tracks the current room, and orchestrates
// room entry → encounter/event → completion → movement.
//
// UI subscribes to events and calls public methods (EnterRoom, CompleteRoom,
// MoveToRoom) in response to player input.
//
// RoomManager does NOT run combat itself — it builds the encounter from
// the RoomNode's data and hands it to CombatManager via EncounterBuilder
// pattern (builds RuntimeCharacterStates and calls CombatManager.StartEncounter).
// ════════════════════════════════════════════════════════════════════════════

public class RoomManager : MonoBehaviour
{
    // ── Inspector refs ─────────────────────────────────────────────────────

    [Header("References")]
    [Tooltip("Drag the CombatManager from the scene.")]
    public CombatManager combatManager;

    [Header("Layer Profiles (index 0 = Layer 1)")]
    [Tooltip("One profile per layer.  Array length = number of layers in the game.")]
    public LayerGenerationProfileSO[] layerProfiles;

    // ── Events (UI subscribes to these) ────────────────────────────────────

    /// <summary>Fired after a new layer map is generated.</summary>
    public event Action<List<RoomNode>, List<RoomNode>> OnMapGenerated;
    //                   allNodes         startNodes

    /// <summary>Fired when the player enters a room (before encounter starts).</summary>
    public event Action<RoomNode> OnRoomEntered;

    /// <summary>Fired when a room is completed (encounter won, event resolved).</summary>
    public event Action<RoomNode> OnRoomCompleted;

    /// <summary>Fired when the boss room becomes accessible.</summary>
    public event Action OnBossRoomUnlocked;

    /// <summary>Fired after the boss is beaten and the layer is done.</summary>
    public event Action<int> OnLayerCompleted; // layerNumber

    /// <summary>Fired when an event room is entered — UI should display choices.</summary>
    public event Action<RoomNode, List<RoomEventSO.EventChoice>> OnEventRoomEntered;
    //                   room       availableChoices (conditions already filtered)

    // ── Runtime state ──────────────────────────────────────────────────────

    private List<RoomNode> _allNodes;
    private List<RoomNode> _startNodes;
    private RoomNode       _currentRoom;
    private RoomNode       _bossNode;
    private bool           _bossUnlocked;

    // ── Public read access ─────────────────────────────────────────────────

    public RoomNode          CurrentRoom  => _currentRoom;
    public List<RoomNode>    AllNodes     => _allNodes;
    public bool              BossUnlocked => _bossUnlocked;

    // ════════════════════════════════════════════════════════════════════════
    // Map generation
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Generate the map for the current layer.
    /// Call this when a layer begins (after StartNewRun or AdvanceLayer).
    /// </summary>
    public void GenerateCurrentLayer()
    {
        var runState = RunStateManager.Instance;
        if (runState == null)
        {
            Debug.LogError("[RoomManager] RunStateManager.Instance is null.");
            return;
        }

        int layerIndex = runState.CurrentLayer - 1;
        if (layerProfiles == null || layerIndex >= layerProfiles.Length)
        {
            Debug.LogError($"[RoomManager] No profile for layer {runState.CurrentLayer}.");
            return;
        }

        var profile = layerProfiles[layerIndex];
        int seed    = runState.GetCurrentLayerSeed();

        var (allNodes, startNodes) = LayerGenerator.Generate(profile, seed);

        _allNodes    = allNodes;
        _startNodes  = startNodes;
        _currentRoom = null;
        _bossUnlocked = false;

        // Find the boss node
        _bossNode = null;
        foreach (var node in _allNodes)
        {
            if (node.roomType == RoomType.Boss)
            {
                _bossNode = node;
                break;
            }
        }

        Debug.Log($"[RoomManager] Layer {runState.CurrentLayer} generated: " +
                  $"{_allNodes.Count} rooms, seed={seed}");

        OnMapGenerated?.Invoke(_allNodes, _startNodes);
    }

    // ════════════════════════════════════════════════════════════════════════
    // Room movement
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Move to a room.  Call from UI when the player taps a node.
    /// Validates accessibility, then triggers room entry logic.
    /// </summary>
    public void MoveToRoom(RoomNode target)
    {
        if (target == null)
        {
            Debug.LogWarning("[RoomManager] MoveToRoom called with null.");
            return;
        }

        if (!target.isAccessible)
        {
            Debug.LogWarning($"[RoomManager] {target.nodeId} is not accessible.");
            return;
        }

        if (target.roomType == RoomType.Boss && !_bossUnlocked)
        {
            Debug.LogWarning("[RoomManager] Boss room is still locked.");
            return;
        }

        _currentRoom = target;
        RunStateManager.Instance?.SetCurrentRoom(target.nodeId);

        EnterRoom(target);
    }

    // ════════════════════════════════════════════════════════════════════════
    // Room entry
    // ════════════════════════════════════════════════════════════════════════

    private void EnterRoom(RoomNode room)
    {
        // Reveal the true type (important for False Rest)
        room.isRevealed = true;

        // Set dynamic band flags so event conditions can check them
        SetBandFlags();

        // Fire the general entry event
        OnRoomEntered?.Invoke(room);

        // Route to the correct handler
        switch (room.roomType)
        {
            case RoomType.Battle:
            case RoomType.Elite:
            case RoomType.Boss:
                StartCombatEncounter(room);
                break;

            case RoomType.Event:
            case RoomType.LoreDiscovery:
            case RoomType.RiskReward:
                PresentEvent(room);
                break;

            case RoomType.FalseRest:
                HandleFalseRest(room);
                break;

            case RoomType.KeyMechanism:
                PresentEvent(room); // Key rooms use event SO for their puzzle/gate
                break;
        }
    }

    // ── Combat ─────────────────────────────────────────────────────────────

    private void StartCombatEncounter(RoomNode room)
    {
        if (combatManager == null)
        {
            Debug.LogError("[RoomManager] CombatManager reference missing.");
            return;
        }

        if (room.encounterEnemies == null || room.encounterEnemies.Length == 0)
        {
            Debug.LogWarning($"[RoomManager] {room.nodeId} has no encounter data. Auto-completing.");
            CompleteRoom(room);
            return;
        }

        // Build enemy RuntimeCharacterStates from EnemyDataSO refs
        var enemies = new List<RuntimeCharacterState>(room.encounterEnemies.Length);
        for (int i = 0; i < room.encounterEnemies.Length; i++)
        {
            var data = room.encounterEnemies[i];
            if (data == null) continue;

            var state = new RuntimeCharacterState
            {
                characterId  = data.enemyId,
                displayName  = data.displayName,
                isHeroine    = false,
                maxHP        = data.FinalHP,
                currentHP    = data.FinalHP,
                maxMP        = data.FinalMP,
                currentMP    = data.FinalMP,
                maxResolve   = 0,
                resolve      = 0,
                maxCorruption = 0,
                corruption   = 0,
            };

            // Base stat setters
            state.BaseATK = data.FinalATK;
            state.BaseMAG = data.FinalMAG;
            state.BaseDEF = data.FinalDEF;
            state.BaseRES = data.FinalRES;
            state.BaseSPD = data.FinalSPD;

            // Wire abilities
            state.abilities = new List<CharacterAbilitySO>();
            if (room.encounterAbilities != null && i < room.encounterAbilities.Length
                && room.encounterAbilities[i]?.abilities != null)
            {
                foreach (var ability in room.encounterAbilities[i].abilities)
                    if (ability != null)
                        state.abilities.Add(ability);
            }

            enemies.Add(state);
        }

        // Party comes from RunStateManager (persists between rooms)
        var party = RunStateManager.Instance?.Party;
        if (party == null || party.Count < 3)
        {
            Debug.LogError("[RoomManager] Party not available from RunStateManager.");
            return;
        }

        combatManager.StartEncounter(party, enemies);

        // NOTE: Combat completion is handled by subscribing to CombatManager's
        // OnEncounterWon / OnEncounterLost events elsewhere.
        // When combat ends, the subscriber should call RoomManager.CompleteRoom()
        // or RoomManager.HandleRunEnd() as appropriate.
    }

    // ── Events ─────────────────────────────────────────────────────────────

    private void PresentEvent(RoomNode room)
    {
        if (room.eventData == null)
        {
            Debug.LogWarning($"[RoomManager] {room.nodeId} has no event data. Auto-completing.");
            CompleteRoom(room);
            return;
        }

        // Filter choices by flag conditions
        var available = new List<RoomEventSO.EventChoice>();
        foreach (var choice in room.eventData.choices)
        {
            if (RoomEventSO.AreConditionsMet(choice))
                available.Add(choice);
        }

        if (available.Count == 0)
        {
            Debug.LogWarning($"[RoomManager] No valid choices for event {room.eventData.eventId}. Auto-completing.");
            CompleteRoom(room);
            return;
        }

        OnEventRoomEntered?.Invoke(room, available);
        // UI will display choices, then call SubmitEventChoice()
    }

    /// <summary>
    /// Called by UI when the player picks an event choice.
    /// </summary>
    public void SubmitEventChoice(RoomNode room, RoomEventSO.EventChoice choice)
    {
        // Apply flag effects
        RoomEventSO.ApplyEffects(choice);

        // Apply stat changes to the active heroine
        var party = RunStateManager.Instance?.Party;
        if (party != null && party.Count > 0)
        {
            var active = party[0];
            if (choice.healHP > 0)         active.Heal(choice.healHP);
            if (choice.restoreMP > 0)      active.RestoreMP(choice.restoreMP);
            if (choice.resolveChange > 0)  active.RestoreResolve(choice.resolveChange);
            if (choice.resolveChange < 0)  active.LoseResolve(-choice.resolveChange);
            if (choice.corruptionChange > 0) active.GainCorruption(choice.corruptionChange);
        }

        // If the choice triggers combat, start it
        if (choice.encounterEnemies != null && choice.encounterEnemies.Length > 0)
        {
            // Temporarily graft encounter onto the room node for StartCombatEncounter
            room.encounterEnemies   = choice.encounterEnemies;
            room.encounterAbilities = choice.encounterAbilities;
            StartCombatEncounter(room);
            return; // room completion handled after combat
        }

        // Otherwise, complete immediately
        CompleteRoom(room);
    }

    // ── False Rest ─────────────────────────────────────────────────────────

    private void HandleFalseRest(RoomNode room)
    {
        // False Rest rooms use an event SO for their trap content.
        // The "false" part is that the player thought it was a safe Event room.
        // Now that isRevealed = true, the UI shows the real type.
        // Content is driven by the event SO — different choices may appear
        // based on Resolve/Corruption band flags (set above in EnterRoom).
        PresentEvent(room);
    }

    // ════════════════════════════════════════════════════════════════════════
    // Room completion
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Mark a room as completed.  Call after combat is won or event resolved.
    /// </summary>
    public void CompleteRoom(RoomNode room)
    {
        if (room.isCompleted) return;

        room.isCompleted = true;
        RunStateManager.Instance?.MarkRoomCompleted();

        // Unlock exits
        foreach (var exit in room.exits)
            exit.isAccessible = true;

        OnRoomCompleted?.Invoke(room);

        // Check boss unlock
        CheckBossUnlock();

        // Check layer completion
        if (room.roomType == RoomType.Boss)
        {
            int layer = RunStateManager.Instance?.CurrentLayer ?? 0;
            OnLayerCompleted?.Invoke(layer);
            Debug.Log($"[RoomManager] Layer {layer} completed.");
        }
    }

    // ── Boss unlock check ──────────────────────────────────────────────────

    private void CheckBossUnlock()
    {
        if (_bossUnlocked || _bossNode == null) return;

        var runState = RunStateManager.Instance;
        if (runState == null) return;

        int layerIndex = runState.CurrentLayer - 1;
        if (layerIndex >= layerProfiles.Length) return;

        int required = layerProfiles[layerIndex].minRoomsBeforeBoss;

        if (runState.RoomsCompletedThisLayer >= required)
        {
            _bossUnlocked = true;
            _bossNode.isAccessible = true;
            OnBossRoomUnlocked?.Invoke();
            Debug.Log("[RoomManager] Boss room unlocked.");
        }
    }

    // ── Band flag helpers ──────────────────────────────────────────────────
    // Sets temporary run_state flags for the active heroine's current
    // Resolve and Corruption bands.  Event SOs check these flags in their
    // choice conditions, keeping gating data-driven.

    private void SetBandFlags()
    {
        var fm = FlagManager.Instance;
        var party = RunStateManager.Instance?.Party;
        if (fm == null || party == null || party.Count == 0) return;

        var active = party[0];

        // Clear old band flags
        fm.RemoveFlag(FlagManager.Scope.RunState, "resolve_steady");
        fm.RemoveFlag(FlagManager.Scope.RunState, "resolve_strong");
        fm.RemoveFlag(FlagManager.Scope.RunState, "resolve_strained");
        fm.RemoveFlag(FlagManager.Scope.RunState, "resolve_fragile");
        fm.RemoveFlag(FlagManager.Scope.RunState, "resolve_broken");
        fm.RemoveFlag(FlagManager.Scope.RunState, "corruption_clear");
        fm.RemoveFlag(FlagManager.Scope.RunState, "corruption_low");
        fm.RemoveFlag(FlagManager.Scope.RunState, "corruption_tempted");
        fm.RemoveFlag(FlagManager.Scope.RunState, "corruption_high");
        fm.RemoveFlag(FlagManager.Scope.RunState, "corruption_fullycorrupted");

        // Set current bands
        string rBand = active.GetResolveBand().ToString().ToLowerInvariant();
        fm.SetFlag(FlagManager.Scope.RunState, $"resolve_{rBand}");

        string cBand = active.GetCorruptionBand().ToString().ToLowerInvariant();
        fm.SetFlag(FlagManager.Scope.RunState, $"corruption_{cBand}");
    }

    // ════════════════════════════════════════════════════════════════════════
    // Run end (called externally when party wipes)
    // ════════════════════════════════════════════════════════════════════════

    public void HandleRunEnd()
    {
        _allNodes    = null;
        _startNodes  = null;
        _currentRoom = null;
        _bossNode    = null;
        _bossUnlocked = false;

        RunStateManager.Instance?.EndRun();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Convenience: get nodes the player can currently move to
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Returns rooms the player can move to right now.
    /// If no current room (start of layer), returns start nodes.
    /// Otherwise returns accessible, uncompleted exits of the current room.
    /// </summary>
    public List<RoomNode> GetAvailableMoves()
    {
        var moves = new List<RoomNode>();

        if (_currentRoom == null)
        {
            // Layer start — can pick any start node
            if (_startNodes != null)
            {
                foreach (var s in _startNodes)
                    if (!s.isCompleted) moves.Add(s);
            }
            return moves;
        }

        foreach (var exit in _currentRoom.exits)
        {
            if (exit.isAccessible)
            {
                if (exit.roomType == RoomType.Boss && !_bossUnlocked)
                    continue;
                moves.Add(exit);
            }
        }

        return moves;
    }

    /// <summary>
    /// Returns what the player should see for a node's room type
    /// (respects False Rest disguise until revealed).
    /// </summary>
    public RoomType GetVisibleRoomType(RoomNode node)
    {
        return node.isRevealed ? node.roomType : node.apparentRoomType;
    }
}
