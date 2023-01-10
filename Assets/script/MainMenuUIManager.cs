using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject options;
    public GameObject quit;

    public Toggle vysnc;
    public Toggle rayTracing;
    public Toggle invertedMode;

    public TMP_Dropdown quality;
    public TMP_Dropdown audioMode;

    public TextMeshProUGUI thankyou;

    private void Start()
    {
        OptionDataContainer.SetAllValues(this);
        QualitySettings.vSyncCount = 0;
        foreach (var n in OptionDataContainer.CompletedNights)
        {
            if (n.Value == true && n.Key == 1)
            {
                thankyou.gameObject.SetActive(true);
            }
        }
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
        StartCoroutine(StartGame());
    }

    public Image img;
    public bool started;
    public float LerpFade;
    internal float add;

    private void Update()
    {
        if (started)
        {
            LerpFade = Mathf.Lerp(0, 1, add += 0.007f);
            Color c = img.color;
            c.a = LerpFade;
            img.color = c;
            loading.transform.Rotate(new Vector3(1,1,1) * 1f);
        }
    }

    public RawImage loading;

    public IEnumerator StartGame()
    {
        img.gameObject.SetActive(true);
        started = true;
        yield return new WaitForSeconds(7);
        loading.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
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
