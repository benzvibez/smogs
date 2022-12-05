using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CamHub : MonoBehaviour
{
    [Header("UI & Audio")]
    public GameObject camOverlay;
    public TextMeshProUGUI currentCam;
    public TextMeshProUGUI currentCamRoom;
    public GameObject quickBar;
    public AudioSource cameraChangeSoundFX;


    [Header("Camera Settings")]
    public Camera mainCamera; //the main player camera
    internal int cameraAmount; //amount of swithable cameras
    public bool isLookingAtCams = false; //start with cameras disabled
    public int CurrentlyOn = 0; //start at camera 1
    public float mainCameraRotationSpeed; //the rotation speed of the main camera L/R

    [Serializable]
    public struct CameraStructure
    {
        public Camera camera;
        public string room;
    }

    [Tooltip("Camera object, then the camera name")]
    public CameraStructure[] Cameras;//list of switchable cameras
    public static CamHub singleton;
    public Image prevBtn;
    public Image btn;

    [Header("Hover L/R Detection Scale (0.0 > 1.0)")]
    public float min = 0.15f;
    [Tooltip("allows you to pass a min left, and min right, if enabled. will ignore the 'min' variable")]
    public bool overrideMins;
    public float minL = -1f;
    public float minR = -1f;

    private void Start()
    {
        cameraAmount = Cameras.Length - 1;
        singleton = this;
        Cursor.lockState = CursorLockMode.Confined;//start the game with the cursor confined to game screen
    }

    internal bool camerasInverted;
    void Update()
    {

        if (!overrideMins)
        {
            minL = min;
            minR = 1.0f - min;
        }

        if (OptionDataContainer.STOREDinvertedMode && !camerasInverted)
        {
            foreach (var c in Cameras)
            {
                c.camera.transform.Rotate(new Vector3(0, 0, 180));
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

        if (Input.GetKeyDown(KeyCode.Space)) //detect if space is pressed
        {
            cameraChangeSoundFX.Play();
            if (isLookingAtCams)//if player is looking at cameras, then set back to main camera
            {
                camOverlay.SetActive(false);
                isLookingAtCams = false;//tell the sytem player is no longer looking at cameras
                foreach (var c in Cameras)//for each camera in the list, disable them
                {
                    c.camera.enabled = false;//disables the camerassss
                }
                mainCamera.enabled = true;//enabled the main camera, after the foreach loop is doneeee
            }
            else //this runs when the player request to look at the cameras
            {
                currentCam.text = Cameras[CurrentlyOn].camera.gameObject.name;
                camOverlay.SetActive(true);
                cameraChangeSoundFX.Play();
                isLookingAtCams = true;// tell the system the player is looking at the cameras
                mainCamera.enabled = false;//disabled the main camera
                currentCamRoom.text = Cameras[CurrentlyOn].room;
                Cameras[CurrentlyOn].camera.enabled = true;//enable the fist camera in the cameras list
            }

        }

        if (!isLookingAtCams && mainCamera.enabled && !GameConsole.cinematic) // true if plaer is not looking at cams and the main cmaera is enabled
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
            if (mousePosition.x < minL && mousePosition.y < 1.0f && mousePosition.y > 0f && mousePosition.x > 0f && mousePosition.x < 1.0f)
            {
                mainCamera.transform.Rotate(new Vector3(0, -mainCameraRotationSpeed, 0 * 3));
            }
            else if (mousePosition.x > minR && mousePosition.y < 1.0f && mousePosition.y > 0f && mousePosition.x > 0f && mousePosition.x < 1.0f)
            {
                mainCamera.transform.Rotate(new Vector3(0, mainCameraRotationSpeed, 0 * 3));
            }
        } else
        {

        }

    }


    public void SetCurrentButton(Image b)
    {
        prevBtn = btn;
        btn = b;
    }

    public void SwitchToCamera(int cam)
    {
        if (!isLookingAtCams)
            return;
        if (cam == -1)
        {
            cameraChangeSoundFX.Play();
            camOverlay.SetActive(false);
            isLookingAtCams = false;
            foreach (var c in Cameras)
            {
                c.camera.enabled = false;
            }
            mainCamera.enabled = true;
        }
        else
        {
            if (prevBtn)
                prevBtn.color = new Color(btn.color.r, btn.color.g, btn.color.b, 0);

            btn.color = new Color(btn.color.r, btn.color.g, btn.color.b, 0.5f);

            cameraChangeSoundFX.Play();

            CurrentlyOn = cam;

            foreach (var c in Cameras)
            {
                c.camera.enabled = false;
            }
            currentCamRoom.text = Cameras[CurrentlyOn].room;
            currentCam.text = Cameras[CurrentlyOn].camera.name;
            Cameras[CurrentlyOn].camera.enabled = true;
        }
    }

    public void QuickClose()
    {
        if (isLookingAtCams)
        {
            cameraChangeSoundFX.Play();
            camOverlay.SetActive(false);
            isLookingAtCams = false;
            foreach (var c in Cameras)
            {
                c.camera.enabled = false;
            }
            mainCamera.enabled = true;
        } else
        {
            currentCam.text = Cameras[CurrentlyOn].camera.gameObject.name;
            camOverlay.SetActive(true);
            cameraChangeSoundFX.Play();
            isLookingAtCams = true;// tell the system the player is looking at the cameras
            mainCamera.enabled = false;//disabled the main camera
            currentCamRoom.text = Cameras[CurrentlyOn].room;
            Cameras[CurrentlyOn].camera.enabled = true;//enable the fist camera in the cameras list
        }
    }
}