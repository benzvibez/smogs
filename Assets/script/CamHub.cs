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
    public GameObject Hidebar;
    public AudioSource cameraChangeSoundFX;


    [Header("Camera Settings")]
    public Camera mainCamera; //the main player camera
    internal int cameraAmount; //amount of swithable cameras
    public bool isLookingAtCams = false; //start with cameras disabled
    public int CurrentlyOn = 0; //start at camera 1
    public float mainCameraRotationSpeed; //the rotation speed of the main camera L/R
    public float ClampLeft;
    public float ClampRight;

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

    private void Awake()
    {
        singleton = this;
        Application.targetFrameRate = 100;
    }

    private void Start()
    {
        cameraAmount = Cameras.Length - 1;
        Cursor.lockState = CursorLockMode.Confined;//start the game with the cursor confined to game screen
    }
    internal static bool off;
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

        if (!isLookingAtCams && mainCamera.enabled && !GameConsole.cinematic && !off) // true if plaer is not looking at cams and the main cmaera is enabled
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
            if (mousePosition.x <= minL && mousePosition.y <= 1.0f && mousePosition.y >= 0f && mousePosition.x >= 0f && mousePosition.x <= 1.0f)
            {
                if (mainCamera.transform.eulerAngles.y <= ClampLeft)
                    return;

                mainCamera.transform.Rotate(new Vector3(0, -mainCameraRotationSpeed, 0 * 3));
            }
            else if (mousePosition.x > minR && mousePosition.y < 1.0f && mousePosition.y > 0f && mousePosition.x > 0f && mousePosition.x < 1.0f)
            {
                if (mainCamera.transform.eulerAngles.y >= ClampRight)
                    return;

                mainCamera.transform.Rotate(new Vector3(0, mainCameraRotationSpeed, 0 * 3));
            }
        }
    }


    public bool hidden;
    public bool hideOff;
    public void Hide(TextMeshProUGUI txt = null)
    {
        if (hideOff)
            return;

        if (hidden)
        {
            off = false;
            quickBar.SetActive(true);
            if (txt)
                txt.text = "HIDE";
            hidden = false;
            mainCamera.transform.position = new Vector3(-70.195f, 4.334f, 1.916f);
            mainCamera.transform.eulerAngles = new Vector3(0, 147.73f, 0);
        }
        else
        {
            hidden = true;
            Radio.singleton.Hidden();
            quickBar.SetActive(false);
            if (txt)
                txt.text = "UNHIDE";
            if (!mainCamera.enabled)
                CamerasED();

            mainCamera.enabled = true;
            mainCamera.transform.position = new Vector3(-69.054f, 2.76f, 1.916f);
            mainCamera.transform.eulerAngles = new Vector3(0, 170.726f, 0);
            off = true;
        }
    }


    public void SetCurrentButton(Image b)
    {
        if (off)
            return;

        prevBtn = btn;
        btn = b;
    }

    public void SwitchToCamera(int cam)
    {
        if (off)
            return;

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
        if (!hidden && !off)
            CamerasED();
    }

    public void CamerasED()
    {
        if (off)
            return;

        cameraChangeSoundFX.Play();
        if (isLookingAtCams)
        {
            hideOff = false;
            PowerController.singleton.UsageLevel -= 3;
            PowerController.singleton.additionalPower -= 5;
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
            hideOff = true;
            PowerController.singleton.UsageLevel += 3;
            PowerController.singleton.additionalPower += 5;
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