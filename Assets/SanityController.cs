using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SanityController : MonoBehaviour
{
    //public vars
    public List<Texture> bars = new();
    public Texture emptyBar;
    public RawImage bar;
    public AudioSource heartBeat;
    public static SanityController singleton;

    //internally used vars
    public float sanityRemaining = 360f;
    internal bool sanityIsRunning = false;
    public float sanityAdditonalSubtractionSpeed = 3.5f;
    public float sanityAdditonalSpeed = 0f;
    public float flashTimerRemainder = 3f;
    public bool flashed = false;
    public bool flashing = false;
    public bool OFF = false;

    internal float flashTimerRemainder_C;
    internal Texture barOn;
    

    private void Awake()
    {
        barOn = bars[5];
        singleton = this;
        flashTimerRemainder_C = flashTimerRemainder;
    }

    private void Update()
    {
        if (GameConsole.cinematic)
            return;

        if (OFF || !PhoneController.ready)
            return;

        float minutes = 0;
        sanityAdditonalSubtractionSpeed /= sanityAdditonalSubtractionSpeed * sanityRemaining;
        if (!MainRoomLightToggle.on || sanityRemaining <= 360)
            sanityAdditonalSubtractionSpeed += 7.5f;

        if (!MainRoomLightToggle.on)
        {
            flashing = true;
        }
        else
        {
            flashing = false;
        }

        if (sanityIsRunning)
        {
            if (sanityRemaining >= 0 || !MainRoomLightToggle.on)
            {

                if (MainRoomLightToggle.on)
                    sanityRemaining += Time.deltaTime * sanityAdditonalSubtractionSpeed;
                else
                    sanityRemaining -= Time.deltaTime * sanityAdditonalSpeed;


                minutes = Mathf.FloorToInt(sanityRemaining / 60);

                if (!flashing)
                    UpdateBar(minutes);
            }
            if (sanityRemaining <= 0)
            {
                Debug.Log("Sanity has run out!");
                //death

            }
        }


        if (flashing) // this means sanity is regenerating
        {
            flashTimerRemainder -= Time.deltaTime * 1f;
            if (flashTimerRemainder < 0)
            {
                flashed = !flashed;

                flashTimerRemainder = flashTimerRemainder_C;

                if (flashed)
                {


                    if (bars.IndexOf(barOn) != 0 && bars.IndexOf(barOn) != 5)
                        bar.texture = bars[bars.IndexOf(barOn) - 1];
                    else if (bars.IndexOf(barOn) == 0 && bars.IndexOf(barOn) != 5)
                        bar.texture = bars[bars.IndexOf(barOn)];

                }
                else
                {
                    UpdateBar(minutes);
                }
            }
        }
    }

    public void UpdateBar(float minutes)
    {
        switch (minutes)
        {
            case 6:
                heartBeat.pitch = 1f;
                heartBeat.volume = 0.15f;
                bar.texture = bars[5];
                barOn = bars[5];
                break;
            case 5:
                heartBeat.pitch = 1.25f;
                heartBeat.volume = 0.25f;
                bar.texture = bars[4];
                barOn = bars[4];
                break;
            case 4:
                heartBeat.pitch = 1.50f;
                heartBeat.volume = 0.32f;
                bar.texture = bars[3];
                barOn = bars[3];
                break;
            case 3:
                heartBeat.pitch = 1.75f;
                heartBeat.volume = 0.50f;
                bar.texture = bars[2];
                barOn = bars[2];
                break;
            case 2:
                heartBeat.pitch = 2f;
                heartBeat.volume = 0.75f;
                bar.texture = bars[1];
                barOn = bars[1];
                break;
            case 1:
                heartBeat.pitch = 2.25f;
                heartBeat.volume = 1f;
                bar.texture = bars[0];
                barOn = bars[0];
                break;
        }
    }
}
