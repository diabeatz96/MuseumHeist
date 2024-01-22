using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public enum OperationStatus
{
    OnGoing,
    Caught,
    KilledInAction,
    OutOfTime,
}

public class GameController : MonoBehaviour
{

    public float gameTimer = 5f;
    public TextMeshProUGUI gameTimeText;
    public OperationStatus opStat;
    public GameObject outOfTimeScreen;

    void Start()
    {
        opStat = OperationStatus.OnGoing;
    }


    void Update()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameTimer % 60f);
        //gameTimeText.text = "Time Remaining: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        string formattedTime = minutes == 0 ? $"{seconds:00}" : $"{minutes:0}:{seconds:00}";
        gameTimeText.text = "Time Remaining: " + formattedTime;
        gameTimer -= Time.deltaTime;
        Debug.Log(gameTimer);
        if (gameTimer <= 0)
        {
            gameTimer = 0;
            opStat = OperationStatus.OutOfTime;
        }

        switch (opStat)
        {
            case OperationStatus.OutOfTime:
                //set canvas to active
                outOfTimeScreen.SetActive(true);
                //set timescale to 0
                Time.timeScale = 0f;
                break;
        }
    }
}
