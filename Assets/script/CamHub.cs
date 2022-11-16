using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHub : MonoBehaviour
{
    public Camera mainCamera; //the main player camera

    public int cameraAmount; //amount of swithable cameras
    public List<Camera> Cameras = new List<Camera>();//list of cameras
    public bool isLookingAtCams = false; //start with cameras disabled
    public int CurrentlyOn = 0; //start at camera 1
    public float mainCameraRotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Confined)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Confined;
        }

        if (Input.GetKeyDown(KeyCode.F)) //detect if f is pressed
        {
            if (isLookingAtCams)//if player is looking at cameras, then set back to main camera
            {
                isLookingAtCams = false;//tell the sytem player is no longer looking at cameras
                foreach (var c in Cameras)//for each camera in the list, disable them
                {
                    c.enabled = false;//disables the camerassss
                }
                mainCamera.enabled = true;//enabled the main camera, after the foreach loop is doneeee
            }
            else //this runs when the player request to look at the cameras
            {
                isLookingAtCams = true;// tell the system the player is looking at the cameras
                mainCamera.enabled = false;//disabled the main camera
                Cameras[CurrentlyOn].enabled = true;//enable the fist camera in the cameras list
            }

        }


        if (Input.GetKeyDown(KeyCode.Space))//player switches camera
        {
            if (!isLookingAtCams)//detect if player is looking at cameras
                return;//if not end script execution


            if (CurrentlyOn == cameraAmount)//since we only have 3 cameras, only make sure it sorts through three cameras
                CurrentlyOn = 0;//if we are over the camera limit we have, reset the camera to camera 1
            else//if have not hit the last camera, go to the next camera
                CurrentlyOn++;//change camera

            foreach (var c in Cameras)//foreach camera disable each camera
            {
                c.enabled = false;//disable camera
            }
            Cameras[CurrentlyOn].enabled = true;//enable the next camera
        }

        if (!isLookingAtCams && mainCamera.enabled)
        {
            Vector3 mousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            mousePosition.z = mainCamera.nearClipPlane;

            if (mousePosition.x < 0.15f && mousePosition.y < 1.0f && mousePosition.y > 0f && mousePosition.x > 0f && mousePosition.x < 1.0f)
            {
                mainCamera.transform.Rotate(new Vector3(0, -mainCameraRotationSpeed, 0 * Time.deltaTime));
            }
            else if (mousePosition.x > 0.85f && mousePosition.y < 1.0f && mousePosition.y > 0f && mousePosition.x > 0f && mousePosition.x < 1.0f)
            {
                mainCamera.transform.Rotate(new Vector3(0, mainCameraRotationSpeed, 0 * Time.deltaTime));
            }

            //c.Rotate(0, 0, -Input.GetAxis("QandE") * 90 * Time.deltaTime);
        }

    }
}