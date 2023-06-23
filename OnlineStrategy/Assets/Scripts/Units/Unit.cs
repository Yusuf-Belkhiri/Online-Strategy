using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Unit Selection: This can be only a client side script, server doesn't actually need to know which units are selected,
/// because as soon as we want to move we tell the server to move the selected units.
///
/// Unit box collider (not the nav mesh collider): is used for selection (mouse click raycast) detection & hits (ex: projectiles)
/// </summary>
public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitMovement _unitMovement;
    [SerializeField] private Targeter _targeter; 
        
    // Selection Events
    [SerializeField] private UnityEvent _onSelected;
    [SerializeField] private UnityEvent _onDeselected;

    // Spawn & Despawn Events: Called on server 
    public static event Action<Unit> ServerOnUnitSpawned;       
    public static event Action<Unit> ServerOnUnitDespawned;       // (when unit dies or gets destroyed) 

    // Called on client
    public static event Action<Unit> AuthorityOnUnitSpawned;    
    public static event Action<Unit> AuthorityOnUnitDespawned;
    
    public UnitMovement GetUnitMovement()
    {
        return _unitMovement;
    }
    public Targeter GetTargeter()
    {
        return _targeter;
    }


    [SerializeField] private Health _health;
    
    #region SERVER

    public override void OnStartServer()        // just like Start() (in server/host)
    {
        ServerOnUnitSpawned?.Invoke(this);
        _health.ServerOnDie += ServerHandleOnDie;
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
        _health.ServerOnDie -= ServerHandleOnDie;
    }


    [Server]
    private void ServerHandleOnDie()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion
    
    
    #region CLIENT
    
    // OnStartClient & isOwned are replaced with OnStartAuthority because we want to raise AuthorityOnUnitSpawned in both clients & host
    // public override void OnStartClient()
    // {
    //     if (!isOwned) return;      // if (!isOwned || !isClientOnly) return; because in the case of Host, both OnStartServer & OnStartClient will be called  => _myUnits.Add(unit) twice  (check RTSPlayer)
    //     AuthorityOnUnitSpawned?.Invoke(this);
    // }
    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);   // isClientOnly is removed after the addition of AuthorityHandleUnitDespawned in UnitSelectionHandler
    }

    public override void OnStopClient()     // unlike OnStartClient with isOwned was replaced with OnStartAuthority, OnStopClient can't be replaced by OnStopAuthority (because this one can be called by just loosing the auth (not necessary being destroyed))
    {
        if (!isOwned) return;      // if (!isOwned || !isClientOnly) return;
        AuthorityOnUnitDespawned?.Invoke(this);
    }
    
    
    
    [Client]
    public void Select()
    {
        if(!isOwned) return;        // can be removed ig

        _onSelected?.Invoke();        
    }

    [Client]
    public void Deselect()
    {
        if(!isOwned) return;        // can be removed ig

        _onDeselected?.Invoke();        
    }    

    #endregion
    
}
