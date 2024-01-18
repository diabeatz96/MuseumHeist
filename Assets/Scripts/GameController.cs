using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    public float gameTimer = 5f;
    public TextMeshProUGUI gameTimeText;

    void Start()
    {

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
            Debug.Log("game is over");
        }
    }
}
