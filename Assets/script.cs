using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    private AudioSource a;
    private bool clicked;

    private void Awake()
    {
        a = GetComponent<AudioSource>();
        a.mute = false;
        a.playOnAwake = false;
        a.Stop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !clicked)
        {
            clicked = true;
            a.Play();
        }
        else if (Input.GetKeyDown(KeyCode.L) && clicked)
        {
            clicked = false;
            a.Stop();
        }

    }
}
