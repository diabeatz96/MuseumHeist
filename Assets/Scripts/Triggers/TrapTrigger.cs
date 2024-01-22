using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public GameController gameController; // Reference to the GameController

    void Start()
    {
        // Get the GameController instance
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trap triggered");
            // Move the player to the base spawner position
            //other.gameObject.transform.position = gameController.baseSpawner.transform.position;

            // Set hasBeenCaught to true and end the timer
            gameController.hasBeenCaught = true;
            gameController.EndTimerIsCaught();
        }
    }
}