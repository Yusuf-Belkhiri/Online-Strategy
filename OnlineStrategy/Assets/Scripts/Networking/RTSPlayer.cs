using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Attached to Player objects in the scene. Each player has a list
/// If we are just a client: we only have our own list of units
/// If we are a server/host: we have a list for everyone 
/// </summary>
public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> _myUnits = new List<Unit>();

    #region SEREVER

    // Subscribe / unsubscribe to events
    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;    // Listener (subscribe to ServerOnUnitSpawned event)
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
    }

    // Listeners
    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;    //  Server can get all the clients connections
        
        _myUnits.Add(unit);
    }
    
    private void ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
        
        _myUnits.Remove(unit);
    }

    #endregion


    #region CLIENT

    public override void OnStartClient()
    {
        if (!isClientOnly) return;      // because in the case of Host, both OnStartServer & OnStartClient will be called => _myUnits.Add(unit) twice

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
    }
    
    public override void OnStopClient()
    {
        if (!isClientOnly) return;      

        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
    }

    
    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        if (!isOwned) return;           
        _myUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        if (!isOwned) return;
        _myUnits.Remove(unit);
    }

    #endregion
}
