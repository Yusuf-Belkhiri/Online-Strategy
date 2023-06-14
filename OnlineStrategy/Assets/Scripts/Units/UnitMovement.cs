using System;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent _agent = null;

    [SerializeField] private Targeter _targeter;
    
    #region SERVER

    // Optimize movement (clear path when reaching stop distance in order to stop pushing other units)
    [ServerCallback]
    private void Update()
    {
        if (!_agent.hasPath) return;        // to prevent it from clearing the path while calculating it within the same frame
        if (_agent.remainingDistance > _agent.stoppingDistance) return;
        
        _agent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        _targeter.ClearTarget();

        // Check if the destination position is valid 
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;      
 
        _agent.SetDestination(hit.position);
    }

    #endregion

    
    /*#region CLIENT

    public override void OnStartAuthority()     // just like Start method, for the person (client) that owns this object
    {
        _mainCamera = Camera.main;
    }

    [ClientCallback]        // prevent the server from running this callback
    private void Update()
    {
        if (!isOwned) return;   // prevent "all clients" from accessing (only allow the client that has authority)    (the same as .hasAuthority)
        
        // Movement Input 
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());      // where we actually clicked in the world

        if(!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) return;   // cast the ray (check physics to see what it hits)
        
        CmdMove(hit.point);     // the impact point in world space where the ray hit the collider
    }

    #endregion*/
}
