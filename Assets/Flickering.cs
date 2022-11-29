using UnityEngine;

public class Flickering : MonoBehaviour
{
    #region Inspector-facing variables

    [Tooltip("Base intensity of the light, adjust as necessary in the inspector")]
    public float baseIntensity = 1f;

    [Tooltip("Range by which flicker varies (proportinal to base intensity)")]
    public float intensityVariance;

    [Tooltip("Speed (in seconds) in which intensity changes occur")]
    public float flickerSpeed;

    public bool stopFlickering = false;
    #endregion


    private Light _lightSource;


    public void Awake()
    {
        _lightSource = GetComponent<Light>();
        if (_lightSource == null)
        {
            Debug.LogError("Flicker script is not attached to a GameObject with a Light component");
            return;
        }
    }


    void Update()
    {
        if (!stopFlickering)
        {
            FlickerUsingPerlinNoise();
        }
    }


    private void FlickerUsingPerlinNoise()
    {
        float intensity = baseIntensity + (intensityVariance * baseIntensity) * Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);

        // avoid going into negative intensity 
        if (intensity <= 0)
            intensity = baseIntensity * 0.01f;

        _lightSource.intensity = intensity;
    }

}