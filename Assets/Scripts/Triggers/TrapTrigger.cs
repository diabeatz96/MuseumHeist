using UnityEngine;
using Fusion;

public class TrapTrigger : NetworkBehaviour
{
    public GameController gameController; // Reference to the GameController

    private bool playerInTrigger = false;

    void Start()
    {
        // Get the GameController instance 
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if we are on the server
        if (Runner.IsServer && other.gameObject.CompareTag("Player") && !playerInTrigger)
        {
            playerInTrigger = true;
            Debug.Log("Trigger detected");
            Debug.Log("Trap triggered");

            // Move the player to the base spawner position
            other.gameObject.transform.position = gameController.baseSpawner.transform.position;

            Debug.Log("Player moved to base spawner");
            Debug.Log("Player position: " + other.gameObject.transform.position);
            
            // Set hasBeenCaught to true and end the timer
            gameController.hasBeenCaught = true;
            gameController.EndTimerIsCaught();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if we are on the server
        if (Runner.IsServer && other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}