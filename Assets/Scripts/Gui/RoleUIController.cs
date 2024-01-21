using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RoleUIController : MonoBehaviour
{
    public GameController gameController; // Reference to the GameController
    public TMP_Dropdown roleDropdown; // Reference to the role dropdown
    public GameObject hackerDetails; // Reference to the hacker details panel
    public GameObject thiefDetails; // Reference to the thief details panel
    public Canvas uiCanvas; // Reference to the UI canvas
    public GameObject hackerButtonDetails; // Reference to the hacker button details panel
    public GameObject thiefButtonDetails; // Reference to the thief button details panel

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

        // Hide the canvas when the 'P' key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            uiCanvas.enabled = !uiCanvas.enabled;
        }
    }

    void OnRoleChanged(int index)
    {
        // Show the hacker details and hacker button details if the selected role is hacker, and hide them otherwise
        hackerDetails.SetActive(index == 0);
        hackerButtonDetails.SetActive(index == 0);

        // Show the thief details and thief button details if the selected role is thief, and hide them otherwise
        thiefDetails.SetActive(index == 1);
        thiefButtonDetails.SetActive(index == 1);
    }
}