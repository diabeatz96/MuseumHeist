using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class RoleUIController : MonoBehaviour
{
    public GameController gameController; // Reference to the GameController
    public TMP_Dropdown roleDropdown; // Reference to the role dropdown
    public GameObject buttonDetailsPanel; // Reference to the panel that shows the button details
    public Image buttonImage; // Reference to the image that shows which button to click
    public Sprite hackerButtonSprite; // The sprite for the hacker button
    public Sprite thiefButtonSprite; // The sprite for the thief button

    void Start()
    {
        // Initialize the dropdown with the possible roles
        roleDropdown.options = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("Hacker"),
            new TMP_Dropdown.OptionData("Thief")
        };

        // Register a listener for the dropdown value changed event
        roleDropdown.onValueChanged.AddListener(OnRoleChanged);
    }

    void Update()
    {
        // Update the selected dropdown value based on the player's role
        roleDropdown.value = gameController.myRole == "Hacker" ? 0 : 1;
    }

    void OnRoleChanged(int index)
    {
        // Update the button details panel and button image based on the selected role
        buttonDetailsPanel.SetActive(true);
        if(gameController.myRole == "Hacker") {
            buttonDetailsPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Click the button to start the heist";
        } else {
            buttonDetailsPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Click the button to stop the heist";
        }
        buttonImage.sprite = index == 0 ? hackerButtonSprite : thiefButtonSprite;
    }
}