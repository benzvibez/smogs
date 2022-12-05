using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Clock : MonoBehaviour
{
    public bool stoppedTimer;

    public void stopTimer(bool yesstart)
    {
        stoppedTimer = yesstart;
    }

    public bool showingSeconds;

    public void ShowSeconds(bool yesno)
    {
        showingSeconds = yesno;
    }

    private void Awake()
    {
        singleton = this;
    }

    public static Clock singleton;
    public float timeRemaining = 0;
    public bool timerIsRunning;
    public TextMeshProUGUI timeText;

    public void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
    public void Update()
    {
        if (timerIsRunning && !stoppedTimer)
        {
            if (timeRemaining <= 360)
            {
                timeRemaining += Time.deltaTime * 1f;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        } else
        {
            timeText.text = $"time paused";
        }
    }
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (showingSeconds)
        {
            if (minutes == 0)
                timeText.text = $"12:{seconds} AM ";
            else
                timeText.text = $"{minutes}:{seconds} AM ";
        } else
        {
            if (minutes == 0)
                timeText.text = $"12 AM ";
            else
                timeText.text = $"{minutes} AM ";
        }

    }
}