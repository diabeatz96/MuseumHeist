using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class TimeGui : NetworkBehaviour
{
    public GameController gameController; // The GameController instance
    public TextMeshProUGUI gameTimeText; // The TextMeshProUGUI element that displays the game timer
    public GameObject outOfTimeScreen; // The screen to display when the timer ends
    bool spawned = false;

    void Start()
    {
        gameTimeText.text = "Waiting for timer.";
    }

    public override void Spawned()
    {
        spawned = true;
    }

    
    public void FixedUpdate() {

        if(!spawned) {
            return;
        }

         UpdateTimerText();

        if (gameController.opStat == OperationStatus.OutOfTime)
        {
            TimerEnds();
        }
    }
    // public override void FixedUpdateNetwork()
    // {
        
    //     UpdateTimerText();

    //     if (gameController.opStat == OperationStatus.OutOfTime)
    //     {
    //         TimerEnds();
    //     }
    // }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(gameController.gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameController.gameTimer % 60f);
        string formattedTime = minutes == 0 ? $"{seconds:00}" : $"{minutes:0}:{seconds:00}";
        gameTimeText.text = "Time:" + formattedTime;

        // Check if the timer has ended
        if (gameController.gameTimer <= 0 && gameController.opStat != OperationStatus.OutOfTime)
        {
            TimerEnds();
        }
    }

    void TimerEnds()
    {
        // Set canvas to active
        outOfTimeScreen.SetActive(true);
        gameTimeText.text = "Start a Heist?";
        // Set timescale to 0
        //Time.timeScale = 0f;
    }
}