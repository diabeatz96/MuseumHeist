using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using System;

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
    [Networked] public bool timerEndsSignal { get; set; } // The signal for the timer ends
    [Networked] public bool hasArtifact { get; set; } // Whether the thief has the artifact
    [Networked] public bool hasBeenCaught { get; set; } // Whether the thief has been caught
    [Networked] public bool hasEscaped { get; set; } // Whether the thief has escaped

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

        // globalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        
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
            case nameof(timerEndsSignal):
                if (timerEndsSignal) // If the signal is true
                {
                    TimerEnds(); // Call TimerEnds
                    timerEndsSignal = false; // Reset the signal
                }
                break;
        }
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

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.GetComponent<Player>().Role == PlayerRole.Hacker)
            {
                Debug.Log("Hacker 1");
            }
            else if (player.GetComponent<Player>().Role == PlayerRole.Thief)
            {
                Debug.Log("Thief is spawned");
                player.transform.position = theifSpawner.transform.position;
            }
        }
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
            timerEndsSignal = true; // Set the signal to true when the timer reaches zero
        }
    }

public void Escaped()
{
    opStat = OperationStatus.Finished;
    timerStarted = false; // Reset timerStarted to false

    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    foreach (GameObject player in players)
    {
        if (player.GetComponent<Player>().Role == PlayerRole.Hacker)
        {
            Debug.Log("Hacker 1");
        }
        else if (player.GetComponent<Player>().Role == PlayerRole.Thief)
        {
            Debug.Log("Thief Move");
            player.transform.position = baseSpawner.transform.position;
        }
    }

    if(Runner.IsSceneAuthority) {
        Runner.LoadScene("WinScreen");
        Runner.UnloadScene(SceneRef.FromIndex(1));
    }
}

public void EndTimerIsCaught ()
{
    gameTimer = 60f;
    opStat = OperationStatus.Caught;
    timerEndsSignal = true; // Set the signal to true when the timer reaches zero
}

void TimerEnds()
{
    opStat = OperationStatus.Finished;
    timerStarted = false; // Reset timerStarted to false

    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    foreach (GameObject player in players)
    {
        if (player.GetComponent<Player>().Role == PlayerRole.Hacker)
        {
            Debug.Log("Hacker 1");
        }
        else if (player.GetComponent<Player>().Role == PlayerRole.Thief)
        {
            Debug.Log("Thief Move");
            player.transform.position = baseSpawner.transform.position;
        }
    }
}

    public void Update() {
        if(myRole == "Hacker") {
            globalLight.intensity = 1f;
        } else {
            globalLight.intensity = 0.005f;
        }
    }

}