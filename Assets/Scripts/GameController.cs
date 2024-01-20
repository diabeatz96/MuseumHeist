using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public enum OperationStatus
{
    Start,
    OnGoing,
    Caught,
    KilledInAction,
    OutOfTime,
    Finished
}

public class GameController : NetworkBehaviour
{
    public GameObject thiefPrefab; // The thief player prefab
    public GameObject hackerPrefab; // The hacker player prefab
    [Networked] public float gameTimer{ get; set;} // The game timer
    public TextMeshProUGUI gameTimeText; // The TextMeshProUGUI element that displays the game timer
    [Networked] public string gameTimerText { get; set; } // The string that displays the game timer
    [Networked] public OperationStatus opStat { get; set; } // The operation status
    public GameObject outOfTimeScreen;
    [Networked] public bool timerStarted { get; set; } // Whether the timer has started
    [Networked] public bool startTimerSignal { get; set; } // The signal to start the timer
    public string myRole;
    private ChangeDetector _changeDetector;
    public Light globalLight; // The global light
    public GameObject baseSpawner; // The base spawner
    public GameObject theifSpawner; // The thief spawner

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        opStat = OperationStatus.Start;
    }
    
    void Start()
    {
            // Don't start the timer immediately
            // StartTimer();

        //     // Display "Waiting for heist to begin."
        //     gameTimerText = "Waiting for heist to begin.";

        //     // Register an OnChanged callback for the gameTimer variable
        //    this.OnChanged(gameTimer, newTimer =>
        // {
        //     // This code will be executed whenever gameTimer changes
        //     UpdateTimerText();
        // });

        globalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        
        }

public override void FixedUpdateNetwork()
{
    UpdateTimer();

     foreach (var change in _changeDetector.DetectChanges(this))
    {
        switch (change)
        {
            case nameof(startTimerSignal):
                if (startTimerSignal) // If the signal is true
                {
                    StartTimer(); // Start the timer
                    startTimerSignal = false; // Reset the signal
                }
                break;
        }
    }

    switch (opStat)
    {
        case OperationStatus.OutOfTime:
            TimerEnds();
            break;
    }
}

public void TriggerStartTimer()
{
    // Check if the current instance is the server
    // Set the signal to true
    startTimerSignal = true;
}

public void StartTimer()
    {
        // Reset the game timer
        gameTimer = 60f;

        // Start the timer
        timerStarted = true;

        opStat = OperationStatus.OnGoing;

        // UpdateTimerText();
    }

    void UpdateTimer()
    {
        if (!timerStarted)
        {
            return;
        }

        gameTimer -= Time.deltaTime;

        if (gameTimer <= 0)
        {
            gameTimer = 0;
            opStat = OperationStatus.OutOfTime;
        }
    }

    void TimerEnds()
    {
        opStat = OperationStatus.Finished;
    }

    public void Update() {
        if(myRole == "Hacker") {
            globalLight.intensity = 1f;
        } else {
            globalLight.intensity = 0f;
        }
    }

}