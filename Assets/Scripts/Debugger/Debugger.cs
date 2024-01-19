using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

public class NetworkDebug : MonoBehaviour
{
    private NetworkRunner runner;

    private void Start()
    {
        runner = NetworkRunner.GetRunnerForScene(SceneManager.GetActiveScene());
    }

    private void OnGUI()
    {
        if (runner != null)
        {
            GUILayout.Label("Fusion Network Info:");
            GUILayout.Label("Cam Spawn?: " + runner.CanSpawn);
            GUILayout.Label("Connected: " + (runner.IsRunning ? "Yes" : "No"));
            GUILayout.Label("In Client or Host?: " + (runner.IsClient ? "Client" : "Host"));            
            GUILayout.Label("Room Name: " + runner.LobbyInfo.Name);
            GUILayout.Label("Players in Room: " + runner.ActivePlayers);
        }
    }
}