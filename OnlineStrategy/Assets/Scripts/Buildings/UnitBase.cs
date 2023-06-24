using System;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class UnitBase : NetworkBehaviour
{
    [SerializeField] private Health _health;

    public static event Action<UnitBase> ServerOnBaseSpawned; 
    public static event Action<UnitBase> ServerOnBaseDespawned;     // a player has lost, remove this base form the list

    #region SERVER

    public override void OnStartServer()
    {
        _health.ServerOnDie += ServerHandleDie;
        
        ServerOnBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        _health.ServerOnDie -= ServerHandleDie;
        
        ServerOnBaseDespawned?.Invoke(this);
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    
    #region CLIENT



    #endregion
}
