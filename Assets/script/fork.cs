using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fork : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 7 * Time.deltaTime); //rotates 50 degrees per second around z axis
    }
}
