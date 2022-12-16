using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRoomLightToggle : MonoBehaviour
{

    public Light breakroomlight;

    public void OnMouseDown()
    {
        breakroomlight.enabled = !breakroomlight.enabled;
        if (breakroomlight.enabled)
        {
            transform.eulerAngles = new Vector3(-122, 90, 0);

        }
        else
        {
            transform.eulerAngles = new Vector3(-60, 90, 0);
        }

    }

}