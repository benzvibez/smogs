using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRoomLightToggle : MonoBehaviour
{

    public Light breakroomlight;
    public static bool on = true;
    public static bool off;


    public void OnMouseDown()
    {
        if (off)
            return;

        breakroomlight.enabled = !breakroomlight.enabled;
        if (breakroomlight.enabled)
        {
            transform.eulerAngles = new Vector3(-122, 90, 0);
            on = true;
        }
        else
        {
            on = false;
            transform.eulerAngles = new Vector3(-60, 90, 0);
        }

    }

}