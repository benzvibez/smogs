using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    public AudioSource a;
    public Light bossroomlight;
    public GameObject BTN;
    public Light BTN_Light;
    public static bool isOn;
    public static bool off;



    void OnMouseDown()
    {
        if (off)
        {
            BTN_Light.enabled = false;
            a.Stop();
            return;
        }

        bossroomlight.enabled = !bossroomlight.enabled;
        isOn = bossroomlight.enabled;

        if (isOn)
        {
            a.Play();
            BTN_Light.enabled = true;
            PowerController.singleton.UsageLevel += 1;
            PowerController.singleton.additionalPower += 4;
        }
        else
        {
            a.Stop();
            BTN_Light.enabled = false;
            PowerController.singleton.UsageLevel -= 1;
            PowerController.singleton.additionalPower -= 4;
        }
    }
}