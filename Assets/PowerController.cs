using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PowerController : MonoBehaviour
{
    public static PowerController singleton;
    public float powerTimeInSeconds = 1440;
    public float powerRemaining;
    public bool powerIsRunning;
    public TextMeshProUGUI powerText;
    public AudioSource losemusic;

    private float flashTimer = 1;
    private float flashTimerRemainder;

    public void Start()
    {
        powerRemaining = powerTimeInSeconds;
        powerIsRunning = true;
    }

    private void Awake()
    {
        singleton = this;
    }

    public float additionalPower = 1;
    public void Update()
    {


        if (powerIsRunning)
        {
            flashTimerRemainder -= Time.deltaTime * 1f;
            if (flashTimerRemainder < 0)
            {
                flashTimerRemainder = 1;
                Flash();
            }


            if (powerRemaining > 0)
            {
                powerRemaining -= Time.deltaTime * 1f * additionalPower;
                DisplayPower(powerRemaining);
            }
            else
            {
                Debug.Log("Power ran out");
                powerRemaining = 0;
                powerIsRunning = false;
                ExitMenu.singleton.StopEverything();
                //play loose msuic and shit here and jumpscare
                losemusic.Play(); 
            }
        }
    }

    public void DisplayPower(float powerToDisplay)
    {
        powerToDisplay += 1;
        float percent = Mathf.FloorToInt(powerToDisplay / powerTimeInSeconds * 100); //basic maths to get a percentage


        if (percent == 0)
        {
            powerText.text = $"Power 0%, poutinee man got u :(";
        }
        else
        {
            powerText.text = $"Power: {percent}%";
        }

    }

    public void Flash()
    {
        if (powerText.fontStyle == FontStyles.Underline)
            powerText.fontStyle = FontStyles.Normal;
        else
            powerText.fontStyle = FontStyles.Underline;
    }




}
