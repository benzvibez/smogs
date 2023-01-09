using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Radio : MonoBehaviour
{
    public float timeToCatchRadio;
    public AudioSource radioMusic;
    public Light radioLight;
    public bool radioOff = true;
    public static Radio singleton;
    public int AuditabilityLevel = 0;
    public TextMeshProUGUI AuditabilityLevelText;

    private void Awake()
    {
        AuditabilityLevelText.text = "Radio Auditability: <color=green>" + AuditabilityLevel + " - Minimal</color>";
        radioMusic.Play();
        singleton = this;
    }

    public void Hidden()
    {
        StartCoroutine(TimesUpHider());
    }

    public IEnumerator TimesUpHider()
    {

        yield return new WaitForSeconds(timeToCatchRadio - 4);

        if (CamHub.singleton.hidden && !EnemyController.singleton.ALG.attacking && radioOff)
            StartCoroutine(StartRadio());

        yield return new WaitForSeconds(2);
        if (CamHub.singleton.hidden && !radioOff)
            AuidoLevel();

        yield return new WaitForSeconds(2);
        if (CamHub.singleton.hidden && !radioOff)
            AuidoLevel();

        yield return new WaitForSeconds(2);
        if (CamHub.singleton.hidden && !radioOff)
            AuidoLevel();
    }

    public void AuidoLevel()
    {
        AuditabilityLevel++;

        if (AuditabilityLevel == 1)
        {
            AuditabilityLevelText.text = "Radio Auditability: <color=yellow>" + AuditabilityLevel + " - WARN</color>";
        }
        else if (AuditabilityLevel == 2)
        {
            AuditabilityLevelText.text = "Radio Auditability: <color=red>" + AuditabilityLevel + " - RUN</color>";
        }
        else if (AuditabilityLevel == 3)
        {
            AuditabilityLevelText.text = "Radio Auditability: <color=blue>" + AuditabilityLevel + " - HE KNOWS.</color>";

        }
        else
        {
            StartCoroutine(EnemyController.singleton.ALG.Jumpscare());
        }
    }

    public IEnumerator StartRadio(bool ovr = false)
    {
        if (!radioOff && !ovr)
            yield return null;

        radioOff = false;
        radioLight.enabled = true;
        radioMusic.volume = 1f;

        yield return new WaitForSeconds(timeToCatchRadio);

        if (!radioOff)
        {
            AuidoLevel();
            EnemyController.singleton.ALG.overrideAttack = true;
            if (!CamHub.singleton.hidden && EnemyController.singleton.ALG.attacking)
                radioOff = true;
        }
    }

    public void StopRadio()
    {
        radioOff = true;
        radioLight.enabled = false;
        radioMusic.volume = 0.00f;
    }

}
