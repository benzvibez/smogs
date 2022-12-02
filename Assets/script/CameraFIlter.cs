using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFilter : MonoBehaviour
{

    Image image;


    void Start()
    {
        image = GetComponent<Image>();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            image.enabled = !image.enabled;
        }
    }
}