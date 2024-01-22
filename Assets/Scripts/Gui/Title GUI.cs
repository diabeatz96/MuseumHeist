using UnityEngine;
using UnityEngine.UI;
using Fusion;
using UnityEngine.SceneManagement;
using TMPro; // Import the TextMeshPro namespace

public class GameSetup : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public TMP_InputField roomNameInput; // Change this to TMP_InputField

    private void Start()
    {
        // Add listeners to the buttons
        hostButton.onClick.AddListener(() => SaveGameInfo("Host"));
        joinButton.onClick.AddListener(() => SaveGameInfo("Join"));
    }

    private void SaveGameInfo(string gameMode)
    {
        string roomName = roomNameInput.text;

        // Save game mode and room name to PlayerPrefs
        PlayerPrefs.SetString("GameMode", gameMode);
        PlayerPrefs.SetString("RoomName", roomName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Level1");
        // Load the new scene here
        // UnityEngine.SceneManagement.SceneManager.LoadScene("YourSceneName");
    }
}