using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class OptionLoader : MonoBehaviour
{
    public static OptionLoader Loader;
    public List<Material> AllMaterials = new List<Material>();
    public List<Light> AllLights = new List<Light>();
    public Volume Processing;
    private ColorAdjustments colors;
    private VolumeParameter<float> hueShift = new VolumeParameter<float>();
    private VolumeParameter<float> HueShiftDef = new VolumeParameter<float>();
    private void Start()
    {
        Loader = this;
        SetValues();
        HueShiftDef.value = 0f;
    }

    public void SetValues()
    {
        if (OptionDataContainer.STOREDrayTracing)
        {//enable the epicness of raytracing

            foreach (var m in AllMaterials)
                m.SetFloat("_Metallic Map", 7);

            foreach (var l in AllLights)
                l.intensity = 8000;

        }
        if (OptionDataContainer.STOREDinvertedMode)
        {
            Processing.profile.TryGet<LiftGammaGain>(out var LGG);
            LGG.active = true;
            Camera.main.transform.Rotate(new Vector3(0, 0, 180));
        }
    }

    private float PreviousHue = 0;
    public bool negativeOrPositive = false;
    private void Update()
    {
        if (OptionDataContainer.STOREDinvertedMode)
        {
            Processing.profile.TryGet<ColorAdjustments>(out colors);
            colors.active = true;

            if (PreviousHue > 180)
                negativeOrPositive = true; // negative

            if (PreviousHue < -180)
                negativeOrPositive = false; // positive

            if (negativeOrPositive)
                PreviousHue -= Time.deltaTime * 50;
            if (!negativeOrPositive)
                PreviousHue += Time.deltaTime * 50;

            hueShift.value = PreviousHue;
            colors.hueShift.SetValue(hueShift);
            PreviousHue = hueShift.value;
        }
    }
    private void OnApplicationQuit()
    {
        foreach (var m in AllMaterials)
        {
            m.SetFloat("_Metallic", 0);
        }
        foreach (var l in AllLights)
        {
            l.intensity = 60;
        }
        Thread.Sleep(2000);
    }

}
