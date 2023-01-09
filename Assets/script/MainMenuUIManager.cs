using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{

    public GameObject options;
    public GameObject quit;

    public Toggle vysnc;
    public Toggle rayTracing;
    public Toggle invertedMode;

    public TMP_Dropdown quality;
    public TMP_Dropdown audioMode;

    private void Update()
    {
        Vector2 resTarget = new Vector2(1920f, 1080f);
        Vector2 resViewport = new Vector2(Screen.width, Screen.height);
        Vector2 resNormalized = resTarget / resViewport; // target res in viewport space
        Vector2 size = resNormalized / Mathf.Max(resNormalized.x, resNormalized.y);
        Camera.current.rect = new Rect(default, size) { center = new Vector2(0.5f, 0.5f) };
    }

    private void Start()
    {
        OptionDataContainer.SetAllValues(this);
        QualitySettings.vSyncCount = 0;
    }

    public void Quit()
    {
        quit.SetActive(!quit.activeInHierarchy);
    }

    public void QuitReal()
    {
        Application.Quit();
    }

    public void StopQuit()
    {
        quit.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void ShowOptions()
    {
        options.SetActive(!options.activeInHierarchy);
    }
    
    public void ChangeQuality(TMP_Dropdown drpdwn)
    {
        OptionDataContainer.STOREDquality = drpdwn.value;
        OptionDataContainer.STOREDquality = drpdwn.value;
        QualitySettings.SetQualityLevel(drpdwn.value); 
    }

    public void Vsync()
    {
        OptionDataContainer.STOREDvsync = vysnc.isOn;
        if (QualitySettings.vSyncCount == 1)
        {
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
        }
    }

    public void InvertMode()
    {
        OptionDataContainer.STOREDinvertedMode = invertedMode.isOn;
    }

    public void RayTracing()
    {
            OptionDataContainer.STOREDrayTracing = rayTracing.isOn;
    }

    public void ChangeAudioMode()
    {
        OptionDataContainer.STOREDaudioMode = audioMode.value;
    }

}
