using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{

    Light bossroomlight;
    public bool isOn;
    public static bool off;
    // Use this for initialization
    void Start()
    {
        bossroomlight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle light on/off when L key is pressed.
        if (Input.GetKeyUp(KeyCode.L))
        {
            if (off)
                return;

            bossroomlight.enabled = !bossroomlight.enabled;
            isOn = bossroomlight.enabled;

            if (isOn)
            {
                PowerController.singleton.UsageLevel += 1;
                PowerController.singleton.additionalPower += 4;
            }
            else
            {
                PowerController.singleton.UsageLevel -= 1;
                PowerController.singleton.additionalPower -= 4;
            }
        }
    }
}