using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamHub : MonoBehaviour

{
    public AudioSource cameraChangeSoundFX;
    public Camera mainCamera; //the main player camera

    public int cameraAmount; //amount of swithable cameras
    public List<Camera> Cameras = new List<Camera>();//list of switchable cameras
    public static bool isLookingAtCams = false; //start with cameras disabled
    public int CurrentlyOn = 0; //start at camera 1
    public float mainCameraRotationSpeed; //the rotation speed of the main camera L/R
    public static CamHub singleton;

    private void Start()
    {
        singleton = this;
        Cursor.lockState = CursorLockMode.Confined;//start the game with the cursor confined to game screen
    }

    private bool camerasInverted;
    void Update()
    {
        if (OptionDataContainer.STOREDinvertedMode && !camerasInverted)
        {
            foreach (var c in Cameras)
            {
                c.transform.Rotate(new Vector3(0, 0, 180));
            }
            camerasInverted = true;
        }
        

        if (Input.GetKeyDown(KeyCode.Escape))// if esc is pressed, allow user to leave confined cursor mode, if re-pressed, re-enter confined cursor mode
        {
            if (Cursor.lockState == CursorLockMode.Confined) // if cursor is current confined
                Cursor.lockState = CursorLockMode.None; //un-confine the cursor
            else //if the cursor is NOT confined
                Cursor.lockState = CursorLockMode.Confined; //confine the cursor
        }

        if (Input.GetKeyDown(KeyCode.F)) //detect if f is pressed
        {
            cameraChangeSoundFX.Play();
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
                cameraChangeSoundFX.Play();
                isLookingAtCams = true;// tell the system the player is looking at the cameras
                mainCamera.enabled = false;//disabled the main camera
                Cameras[CurrentlyOn].enabled = true;//enable the fist camera in the cameras list
            }

        }
        if (Input.GetKeyDown(KeyCode.Space))//player switches camera
        {
            if (!isLookingAtCams)//detect if player is looking at cameras
                return;//if not end script execution   

            cameraChangeSoundFX.Play();
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

        if (!isLookingAtCams && mainCamera.enabled) // true if plaer is not looking at cams and the main cmaera is enabled
        {
            Vector3 mousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            /*The above is used to transform the mosue position vector, into a vecotr usable in
             the viewport, this allows us to determin if the mouse is in the bounds of
             the game, or if the mouse is out of it, you can also reverse this, and user it
            for telling if the mouse is on the far left, or far right side of the screen,
            which we do below, to move the cameras, left, and right*/


            /*This if statement determins if the mouse is within bounds of the screen
             * and if the mouse is on the left or right far side,
             whichever side it may be on, move that direction.*/
            if (mousePosition.x < 0.15f && mousePosition.y < 1.0f && mousePosition.y > 0f && mousePosition.x > 0f && mousePosition.x < 1.0f)
            {
                //this will rotate the main camera in the direction of the mouse
                //using Time.delaTime will allow us to slowly, each frame,
                //turn the camera in the mouses direction. (time.deltaTime is the time it takes for each frame, usually arround 3 milliseconds (3.0) or 2 milliseconds (2.0), nearly never one
                mainCamera.transform.Rotate(new Vector3(0, -mainCameraRotationSpeed, 0 * 2));
            }
            else if (mousePosition.x > 0.85f && mousePosition.y < 1.0f && mousePosition.y > 0f && mousePosition.x > 0f && mousePosition.x < 1.0f)
            {
                mainCamera.transform.Rotate(new Vector3(0, mainCameraRotationSpeed, 0 * 2));
            }
        }

    }
}