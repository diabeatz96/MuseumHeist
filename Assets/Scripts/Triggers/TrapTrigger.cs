using UnityEngine;
using Fusion;

public class TrapTrigger : NetworkBehaviour
{
    public GameController gameController; // Reference to the GameController

    private float cooldownTime = 1.0f; // 1 second cooldown
    private float nextTriggerTime = 0;

    void Start()
    {
        // Get the GameController instance 
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected");
        if (other.gameObject.CompareTag("Player") && Time.time > nextTriggerTime)
        {
            nextTriggerTime = Time.time + cooldownTime;
            Debug.Log("Trap triggered");

            // Check if we are on the server
            if (Runner.IsServer)
            {
                // Move the player to the base spawner position
                //other.gameObject.transform.position = gameController.baseSpawner.transform.position;

                // Set hasBeenCaught to true and end the timer
                gameController.hasBeenCaught = true;
                gameController.EndTimerIsCaught();
            }
        }
    }
}