using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PowerController : MonoBehaviour
{
    public static PowerController singleton;
    public float powerTimeInSeconds = 2440;
    public float powerRemaining;
    public bool powerIsRunning;
    public TextMeshProUGUI powerText;
    public AudioSource losemusic;
    public RawImage usageImg;
    bool off;
    public List<Texture> usageMeters = new(); // 0-4 (1-5)

    private float flashTimerRemainder = 0.3f;

    public void Start()
    {
        powerRemaining = powerTimeInSeconds;
    }

    private void Awake()
    {
        powerText.color = Color.green;
        singleton = this;
    }

    public float additionalPower = 1;
    public void Update()
    {
        if (GameConsole.cinematic)
            return;

        usageImg.texture = usageImg.texture = usageMeters[(int)UsageLevel];
        if (powerIsRunning)
        {
            flashTimerRemainder -= Time.deltaTime * 1f;
            if (flashTimerRemainder < 0)
            {
                flashTimerRemainder = 0.3f;
                Flash();
            }


            if (powerRemaining > 0)
            {
                GameConsole.singleton.POWERPERCENT.text = "POWER: " + powerRemaining;
                GameConsole.singleton.POWERUSAGE.text = "USAGE: " + UsageLevel;
                GameConsole.singleton.POWERMULTIPLIER.text = "POWER MULTIPLIER: " + additionalPower;
                powerRemaining -= Time.deltaTime * additionalPower;
                DisplayPower(powerRemaining);
            }
            else if (!off)
            {
                off = true;
                Debug.Log("Power ran out");
                powerRemaining = 0;
                powerIsRunning = false;
                ExitMenu.singleton.StopEverything();
                ExitMenu.singleton.enabled = false;
                StartCoroutine(GoToMenu());
            }
        }

    }
    public IEnumerator GoToMenu()
    {
        losemusic.Play();
        OptionLoader.Loader.AllLights.ForEach(light =>
        {
            light.enabled = false;
        });
        yield return new WaitForSeconds(7);

        losemusic.Stop();
        StartCoroutine(EnemyController.singleton.ALG.Jumpscare((done) =>
        {
            if (done)
            {
                //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }
        ));

    }
    public void DisplayPower(float powerToDisplay)
    {

        powerToDisplay += 1;
        float percent = Mathf.FloorToInt(powerToDisplay / powerTimeInSeconds * 100); //basic maths to get a percentage


        if (percent == 0)
        {
            powerText.text = $"0%";
        }
        else
        {
            if (percent < 75 && percent > 50 && percent > 25)
                powerText.color = Color.cyan;

            if (percent < 50 && percent > 25)
                powerText.color = Color.yellow;

            if (percent < 25)
                powerText.color = Color.red;

            powerText.text = $"{percent}%";
        }

    }

    public uint UsageLevel; // 0-4 (1-5)


    public void Flash()
    {
        if (powerText.fontStyle == FontStyles.Underline)
        {
            powerText.fontStyle = FontStyles.Normal;
        }
        else
        {
            powerText.fontStyle = FontStyles.Underline;
        }
    }




}
