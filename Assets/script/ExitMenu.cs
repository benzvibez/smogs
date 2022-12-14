using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ExitMenu : MonoBehaviour
{
    public static ExitMenu singleton;
    public GameObject Menu;

    private void Awake()
    {
        Menu.SetActive(false);
        singleton = this;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(!Menu.activeInHierarchy);
        }

    }

    public void MenuShow()
    {
        StartEverything();
        Menu.SetActive(!Menu.activeInHierarchy);
    }

    public void MainMenu()
    {
        StartEverything();
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StopCanvas()
    {
        GameConsole.singleton.DEBUG.SetActive(false);
        GameConsole.singleton.noclipandcinematichelp.SetActive(false);
        GameConsole.singleton.input.gameObject.SetActive(false);
        CamHub.singleton.quickBar.gameObject.SetActive(false);
        PowerController.singleton.gameObject.SetActive(false);
    }

    public void StopEverything(string but = "")
    {
        if (!PhoneController.ready)
            return;

        if (but != "GAME_CONSOLE")
        {
            GameConsole.singleton.DEBUG.SetActive(false);
            GameConsole.singleton.noclipandcinematichelp.SetActive(false);
            GameConsole.singleton.input.gameObject.SetActive(false);
        }
        SanityController.singleton.OFF = true;
        SanityController.singleton.gameObject.SetActive(false);
        Radio.singleton.AuditabilityLevelText.gameObject.SetActive(false);
        CamHub.singleton.quickBar.gameObject.SetActive(false);
        Clock.singleton.enabled = false;
        foreach (var c in CamHub.singleton.Cameras)
            c.camera.enabled = false;
        CamHub.singleton.mainCamera.enabled = true;
        CamHub.singleton.enabled = false;
    }

    public void StartEverything()
    {

        MainRoomLightToggle.off = false;
        ToggleLight.off = false;
        SanityController.singleton.gameObject.SetActive(true);
        SanityController.singleton.sanityIsRunning = true;
        Radio.singleton.AuditabilityLevelText.gameObject.SetActive(true);
        Clock.timerIsRunning = true;
        PowerController.singleton.powerIsRunning = true;
        CamHub.singleton.quickBar.SetActive(true);
        CamHub.singleton.Hidebar.SetActive(true);
        PowerController.singleton.gameObject.SetActive(true);
    }
}
