using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] private GameObject _unitSpawnerPrefab;

    [SerializeField] private GameOverHandler _gameOverHandlerPrefab;
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);       // Instantiate & Spawn player object
        
        // Instantiate, & Spawn a unit spawner for the client (owned)
        GameObject unitSpawnerInstance = 
            GameObject.Instantiate(_unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawnerInstance, conn);     // Pass the Ownership authority (conn) (this UnitSpawner is owned by this client) 
    }

    
    public override void OnServerSceneChanged(string sceneName)
    {
        // Check if scene changed into gameplay scene to spawn the game over handler (unlike NetworkManager game object, GameOverHandler game obj must be only in gameplay scenes) 
        if (SceneManager.GetActiveScene().name.StartsWith("Level"))
        {
            GameOverHandler gameOverHandlerInstance = Instantiate(_gameOverHandlerPrefab);
            
            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);
        }
    }
}
