using System.Collections;
using UnityEngine;
using TMPro;

public class StartHeistTrigger : MonoBehaviour
{
    public GameController gameController; // The GameController instance
    public GameObject promptTextObject; // The GameObject of the TextMeshPro element
    private bool eKeyPressed = false; // Track whether the E key has been pressed
    public SwitchCameras switchCameras; // The SwitchCameras instance
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the other collider is the player and the E key has not been pressed
        if (other.CompareTag("Player") && !eKeyPressed)
        {
            // Activate the TextMeshPro object
            promptTextObject.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Check if the other collider is the player
        if (other.CompareTag("Player"))
        {
            // Check if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Start the timer
                gameController.TriggerStartTimer();

                // Deactivate the TextMeshPro object
                promptTextObject.SetActive(false);

                // Set eKeyPressed to true
                eKeyPressed = true;

                // Switch the camera
                switchCameras.SwitchCamera(1);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the other collider is the player
        if (other.CompareTag("Player"))
        {
            // Deactivate the TextMeshPro object after a small delay
            StartCoroutine(DeactivatePromptTextAfterDelay(0.1f));

            // Reset eKeyPressed to false
            eKeyPressed = false;
        }
    }

    IEnumerator DeactivatePromptTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        promptTextObject.SetActive(false);
    }
}