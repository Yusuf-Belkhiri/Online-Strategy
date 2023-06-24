using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Server only
/// Attached in NetworkManager 
/// </summary>
public class GameOverHandler : NetworkBehaviour
{
    private List<UnitBase> _bases = new List<UnitBase>();

    #region SERVER

    public override void OnStartServer()
    {
        UnitBase.ServerOnBaseSpawned += ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseDespawned += ServerHandleBaseDespawned;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnBaseSpawned -= ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseDespawned -= ServerHandleBaseDespawned;
    }

    [Server]
    private void ServerHandleBaseSpawned(UnitBase unitBase)
    {
        _bases.Add(unitBase);        
    }
    
    [Server]
    private void ServerHandleBaseDespawned(UnitBase unitBase)
    {
        _bases.Remove(unitBase);
        
        // Check for game over if only one player remains
        if (_bases.Count != 1) return;
        
        Debug.Log("Game Over");
    }

    #endregion
}
