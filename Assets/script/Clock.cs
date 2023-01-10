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

        if (GameConsole.cinematic)
            return;

        if (timerIsRunning && !stoppedTimer)
        {
            if (timeRemaining <= 360)
            {
                timeRemaining += clockAdditionalSpeed;
                DisplayTime(timeRemaining);
            }
            else if (timerIsRunning)
            {
                Debug.Log("Time has run out!");
                if (!CamHub.singleton.mainCamera.enabled)
                    CamHub.singleton.CamerasED();
                SanityController.singleton.gameObject.SetActive(false);
                SanityController.singleton.sanityIsRunning = false;
                Radio.singleton.AuditabilityLevelText.gameObject.SetActive(false);
                timeText.text = "";
                PhoneController.ready = false;
                PowerController.singleton.powerIsRunning = false;
                PowerController.singleton.gameObject.SetActive(false);
                CamHub.singleton.quickBar.SetActive(false);
                CamHub.singleton.Hidebar.SetActive(false);
                ExitMenu.singleton.StopEverything();
                timeRemaining = 0;
                timerIsRunning = false;
                winmusic.Play();
                winimage.enabled = true;
                dingdong.enabled = true;
                dingdong.Play();
                OptionDataContainer.CompletedNights.Add(1, true);
                StartCoroutine(End());
            }
        }
        else if (timerIsRunning)
        {
            timeText.text = $"fr ong";
        }
    }

    internal IEnumerator End()
    {
        yield return new WaitForSeconds(4);
            ExitMenu.singleton.MainMenu();
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