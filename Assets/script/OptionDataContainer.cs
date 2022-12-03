using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionDataContainer : MonoBehaviour
{
    public static bool STOREDrayTracing = false;
    public static bool STOREDinvertedMode = false;
    public static int STOREDquality = 0;
    public static int STOREDaudioMode = 0; // 0 = normal mode, 1 = anime mode, 2 = breaking bad
    public static bool STOREDvsync = false;
    public static OptionDataContainer singleton;

    private void Start()
    {
        singleton = this;
        DontDestroyOnLoad(this);
    }

    public static void SetAllValues(MainMenuUIManager Manager)
    {
        Manager.rayTracing.isOn = STOREDrayTracing;
        Manager.invertedMode.isOn = STOREDinvertedMode;
        Manager.vysnc.isOn = STOREDvsync;

        Manager.quality.value = STOREDquality;
        Manager.audioMode.value = STOREDaudioMode;
    }

}
