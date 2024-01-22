using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private bool _mouseButton0;
    private string _roomName = "TestRoom"; // Default room name
    public PlayerCamera playerCamera;
    public GameObject defaultSpawner;
    public GameObject thiefSpawner;
    private bool first_player = true;
    public GameController gameController; // The GameController instance
    private GameMode _gameMode; // Game mode will be loaded from PlayerPrefs

public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
{
    if (runner.IsServer)
    {
        gameController.myRole = "Hacker";
        // Create a unique position for the player
        Vector3 spawnPosition;
        if(first_player) {
            spawnPosition = defaultSpawner.transform.position;
            first_player = false;
        } else {
            spawnPosition = defaultSpawner.transform.position;
        }
        
        NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

        // Assign the player role based on the ownership of the network object
        var playerComponent = networkPlayerObject.GetComponent<Player>();
        playerComponent.Role = networkPlayerObject.HasInputAuthority ? PlayerRole.Hacker : PlayerRole.Thief;

        // // If this client has input authority over the network object, set the camera target
        // if (networkPlayerObject.HasInputAuthority)
        // {
        //     Debug.Log("Setting camera target");
        //     Debug.Log("Player: " + player);
        //     playerCamera.SetCameraTarget(networkPlayerObject.transform);
        // }

        _spawnedCharacters.Add(player, networkPlayerObject);
    } else {
        Debug.Log("Not server");
        gameController.myRole = "Thief";
    }
}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

public void OnInput(NetworkRunner runner, NetworkInput input)
{
    var data = new NetworkInputData();

    if (Input.GetKey(KeyCode.W))
        data.direction += Vector3.left; //right 

    if (Input.GetKey(KeyCode.S))
        data.direction += Vector3.right; //  left

    if (Input.GetKey(KeyCode.A))
        data.direction += Vector3.back; // forward

    if (Input.GetKey(KeyCode.D))
        data.direction += Vector3.forward; // back

    data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _mouseButton0);
    _mouseButton0 = false;

    input.Set(data);
    
}
  public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
  public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
  public void OnConnectedToServer(NetworkRunner runner) { }
  public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
  public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
  public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
  public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
  public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
  public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
  public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
  public void OnSceneLoadDone(NetworkRunner runner) {
    
   }
  public void OnSceneLoadStart(NetworkRunner runner) { }
  public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
  public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
  public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }

private void Update()
{
  _mouseButton0 = _mouseButton0 | Input.GetMouseButton(0);
  
}

 private void Start()
    {
        // Retrieve room name and game mode from PlayerPrefs
        _roomName = PlayerPrefs.GetString("RoomName");
        string gameMode = PlayerPrefs.GetString("GameMode");
        _gameMode = gameMode == "Host" ? GameMode.Host : GameMode.Client;

        // Start the game with the retrieved game mode
        StartGame(_gameMode);
    }

async void StartGame(GameMode mode)
{
    // Create the Fusion runner and let it know that we will be providing user input
    // _runner = gameObject.AddComponent<NetworkRunner>();
    _runner = gameObject.GetComponent<NetworkRunner>();
    _runner.ProvideInput = true;

    // Create the NetworkSceneInfo from the current scene
    var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
    var sceneInfo = new NetworkSceneInfo();
    if (scene.IsValid) {
        sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
    }

    // Start or join (depends on gamemode) a session with a specific name
    await _runner.StartGame(new StartGameArgs()
    {
        GameMode = mode,
        SessionName = _roomName, // Use the room name from the GUI input
        Scene = scene,
        SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
    });
}

    

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

//     private void OnGUI()
// {
//   if (_runner == null)
//   {
//     _roomName = GUI.TextField(new Rect(0, 80, 200, 20), _roomName);
    
//     if (GUI.Button(new Rect(0,0,200,40), "Host"))
//     {
//         StartGame(GameMode.Host);
//     }
//     if (GUI.Button(new Rect(0,40,200,40), "Join"))
//     {
//         StartGame(GameMode.Client);
//     }
//   }

//   if (_runner != null)
//         {
//             GUILayout.Label("Fusion Network Info:");
//             GUILayout.Label("Cam Spawn?: " + _runner.CanSpawn);
//             GUILayout.Label("Connected: " + (_runner.IsRunning ? "Yes" : "No"));
//             GUILayout.Label("In Client or Host?: " + (_runner.IsClient ? "Client" : "Host"));            
//             GUILayout.Label("Room Name: " + _runner.SessionInfo.Name);
//             GUILayout.Label("Players in Room: " + _runner.ActivePlayers);
//         }
        
// }

}



