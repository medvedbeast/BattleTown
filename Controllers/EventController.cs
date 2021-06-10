using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    // events

    public event System.Action GameStateChange;

    public event System.Action<Unit> UnitSpawn;
    public event System.Action<Unit> UnitDestruction;

    public event System.Action<Unit> BaseHealthChange;
    
    public event System.Action<Unit> UnitHealthChange;
    public event System.Action<Unit> UnitEquipmentChange;

    public event System.Action<Unit, Interactive> UnitInteractionStart;
    public event System.Action<Unit, Interactive> UnitInteractionEnd;

    // forwarders

    public void OnGameStateChange() { GameStateChange?.Invoke(); }

    public void OnUnitSpawn(Unit u) { UnitSpawn?.Invoke(u); }
    public void OnUnitDestruction(Unit u){ UnitDestruction?.Invoke(u); }

    public void OnBaseHealthChange(Unit u) { BaseHealthChange?.Invoke(u); }

    public void OnUnitHealthChange(Unit u) { UnitHealthChange?.Invoke(u); }
    public void OnUnitEquipmentChange(Unit u) { UnitEquipmentChange?.Invoke(u); }

    public void OnUnitInteractionStart(Unit u, Interactive i) { UnitInteractionStart?.Invoke(u, i); }
    public void OnUnitInteractionEnd(Unit u, Interactive i) { UnitInteractionEnd?.Invoke(u, i); }
}
