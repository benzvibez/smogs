using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Clock : MonoBehaviour

{
    [Header("Audio % UI")]
    public AudioSource winmusic;
    public RawImage winimage;
    public UnityEngine.Video.VideoPlayer dingdong;
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
    public static bool timerIsRunning;
    public TextMeshProUGUI timeText;
    public float clockAdditionalSpeed = 2f;

    public void Update()
    {

        if (timerIsRunning && !stoppedTimer)
        {
            if (timeRemaining <= 600)
            {
                timeRemaining += Time.deltaTime * clockAdditionalSpeed;
                DisplayTime(timeRemaining);
            }
            else if (timerIsRunning)
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                winmusic.Play();
                winimage.enabled = true;
                dingdong.enabled = true;
                dingdong.Play();
            }
        }
        else if (timerIsRunning)
        {
            timeText.text = $"fr ong";
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
        }
        else
        {
            if (minutes == 0)
                timeText.text = $"12 AM ";
            else
                timeText.text = $"{minutes} AM ";
        }

    }
}