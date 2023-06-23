using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler       // for IPointerClickHandler (originally used with UI): add EventSystem object in the scene, & Attach PhysicsRaycaster component to camera
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private Transform _unitSpawnPos;
    [SerializeField] private Health _health;
    #region SERVER

    public override void OnStartServer()
    {
        _health.ServerOnDie += ServerHandleOnDie;
    }

    public override void OnStopServer()
    {
        _health.ServerOnDie -= ServerHandleOnDie;
    }

    [Server]
    private void ServerHandleOnDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        // First, Instantiate
        GameObject unitInstance = GameObject.Instantiate(_unitPrefab, _unitSpawnPos.position, _unitSpawnPos.rotation);
        // Then, Spawn it in the Server
        NetworkServer.Spawn(unitInstance, connectionToClient);      // Specify the client this unit belongs to, or, if no network connection is passed: no ownership (server only object). 
                                                                    // connectionToClient: (pass ownership) to make the owner of the unit the same owner of this UnitSpawner (isOwned)
    }
    #endregion

    #region CLIENT
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)  return;
        
        // It won't work anyways if another non owner client of this object clicks on it, Because CLIENT CAN ONLY CALL COMMANDS if HE OWNS THE OBJECT (safe) 
        // This is just to not even bother telling the server to try to do it if we don't own this unit spawner
        if(!isOwned) return;
        
        CmdSpawnUnit();
    }
    
    #endregion

}
