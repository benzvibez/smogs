using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Call : MonoBehaviour
{
    public bool declineCall;
    bool dc;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !dc)
        {
            dc = true;
            PhoneController.singleton.DeclineCall();
        }
    }
    public void OnMouseDown()
    {
        if (declineCall)
        {
            PhoneController.singleton.DeclineCall();
        }
        else
        {
            PhoneController.singleton.AcceptCall();
        }
    }
}
