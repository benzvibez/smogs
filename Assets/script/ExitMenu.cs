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
        Menu.SetActive(!Menu.activeInHierarchy);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StopEverything()
    {
        GameConsole.singleton.DEBUG.SetActive(false);
        GameConsole.singleton.noclipandcinematichelp.SetActive(false);
        GameConsole.singleton.input.gameObject.SetActive(false);
        CamHub.singleton.quickBar.gameObject.SetActive(false);
        Clock.singleton.enabled = false;
        foreach (var c in CamHub.singleton.Cameras)
            c.camera.enabled = false;
        CamHub.singleton.mainCamera.enabled = true;
        CamHub.singleton.enabled = false;
    }

    public void StartEverything()
    {
        GameConsole.singleton.input.gameObject.SetActive(true);
        CamHub.singleton.quickBar.gameObject.SetActive(true);
        Clock.singleton.enabled = true;
        CamHub.singleton.enabled = true;
    }
}
