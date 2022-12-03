using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticFX : MonoBehaviour
{
    public RawImage staticness;
    private bool running;
    void Update()
    {
        if (!running && CamHub.singleton.isLookingAtCams)
            StartCoroutine(StartStaticness());
        if (CamHub.singleton.isLookingAtCams)
            staticness.gameObject.SetActive(true);
        else
            staticness.gameObject.SetActive(false);
    }

    public IEnumerator StartStaticness()
    {
        running = true;
        yield return new WaitForSeconds(0.1f);
        staticness.uvRect = new Rect(0, 0, Random.Range(-29, 47), Random.Range(-29, 47));
        running = false;
    }
}
