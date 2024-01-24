using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeScreenButton : MonoBehaviour
{
    public Button yourButton; // Assign this in the inspector

    void Start()
    {
        // Add a listener to the button's click event
        yourButton.onClick.AddListener(LoadTitleScreen);
    }

    void LoadTitleScreen()
    {
        // Load the "TitleScreen" scene
        SceneManager.LoadScene("TitleScreen");
    }
}