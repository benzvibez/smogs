using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{

    Canvas canvas;

    
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

   
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            canvas.enabled = !canvas.enabled;
        }
    }
}