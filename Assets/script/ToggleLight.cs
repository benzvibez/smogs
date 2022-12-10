using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{

    Light bossroomlight;
    public bool isOn;

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

            bossroomlight.enabled = !bossroomlight.enabled;
            isOn = bossroomlight.enabled;

            if (isOn)
            {
                PowerController.singleton.UsageLevel += 1;
                PowerController.singleton.additionalPower += 2;
                Clock.singleton.clockAdditionalSpeed += 1;
            }
            else
            {
                PowerController.singleton.UsageLevel -= 1;
                Clock.singleton.clockAdditionalSpeed -= 1;
                PowerController.singleton.additionalPower -= 2;
            }
        }
    }
}