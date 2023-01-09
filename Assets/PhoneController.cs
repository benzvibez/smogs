using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public static bool ready;
    public AudioSource ringtone;
    public AudioSource hangup;
    public AudioSource phoneguy;
    public GameObject phone;
    public AudioSource joinedCall;
    public GameObject callBTN;
    public static PhoneController singleton;

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        Radio.singleton.AuditabilityLevelText.gameObject.SetActive(false);
        Clock.timerIsRunning = false;
        PowerController.singleton.powerIsRunning = false;
        CamHub.singleton.quickBar.SetActive(false);
        CamHub.singleton.Hidebar.SetActive(false);
        PowerController.singleton.gameObject.SetActive(false);
        StartCoroutine(StartCall());
    }

    public IEnumerator StartCall()
    {
        yield return new WaitForSeconds(2);
        ringtone.Play();
        yield return new WaitForSeconds(2.5f);
        startPhoneAnimUp = true;
    }

    public bool startPhoneAnimUp;
    public bool startPhoneAnimDown;

    void Update()
    {
        if (startPhoneAnimUp)
        {
            if (phone.transform.localPosition.y >= -0.0659f)
                return;
            phone.transform.position = new Vector3(phone.transform.position.x, phone.transform.position.y+0.001f, phone.transform.position.z);
        }

        if (startPhoneAnimDown)
        {
            if (phone.transform.localPosition.y <= -0.23f)
                return;
            phone.transform.position = new Vector3(phone.transform.position.x, phone.transform.position.y - 0.001f, phone.transform.position.z);
        }
    }

    public void DeclineCall()
    {
        startPhoneAnimUp = false;
        ringtone.Stop();
        hangup.Play();
        phoneguy.Stop();
        startPhoneAnimDown = true;
        StartGame();
    }

    public void AcceptCall()
    {
        callBTN.SetActive(false);
        ringtone.Stop();
        joinedCall.Play();
        StartCoroutine(Call());
        StartGame();
    }

    public IEnumerator Call()
    {
        phoneguy.Play();
        yield return new WaitForSeconds(phoneguy.clip.length+1);
        startPhoneAnimDown = true;
    }

    public void StartGame()
    {
        Radio.singleton.AuditabilityLevelText.gameObject.SetActive(true);
        Clock.timerIsRunning = true;
        PowerController.singleton.powerIsRunning = true;
        CamHub.singleton.quickBar.SetActive(true);
        CamHub.singleton.Hidebar.SetActive(true);
        PowerController.singleton.gameObject.SetActive(true);
        ready = true;
    }
}
