using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] private GameObject _unitSpawnerPrefab;
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);       // Instantiate & Spawn player object
        
        // Instantiate, & Spawn a unit spawner for the client (owned)
        GameObject unitSpawnerInstance = 
            GameObject.Instantiate(_unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawnerInstance, conn);     // Pass the Ownership authority (conn) (this UnitSpawner is owned by this client) 
    }
}
