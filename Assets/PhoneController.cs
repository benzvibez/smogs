using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneController : MonoBehaviour
{
    public static bool ready;
    public AudioSource ringtone;
    public AudioSource hangup;
    public AudioSource phoneguy;
    public GameObject phone;
    public AudioSource joinedCall;
    public GameObject callBTN;
    public Texture onCall;
    public Texture onHome;
    public Material main;
    private Texture old;
    public static PhoneController singleton;
    public TextMeshProUGUI info;

    private void Awake()
    {
        old = main.GetTexture("_BaseMap");
        singleton = this;
    }

    void Start()
    {
        SanityController.singleton.gameObject.SetActive(false);
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
            {
                main.SetTexture("_BaseMap", old);
                return;
            }
            phone.transform.position = new Vector3(phone.transform.position.x, phone.transform.position.y - 0.001f, phone.transform.position.z);
        }
    }

    public void DeclineCall()
    {
        main.SetTexture("_BaseMap", onHome);
        startPhoneAnimUp = false;
        ringtone.Stop();
        hangup.Play();
        phoneguy.Stop();
        startPhoneAnimDown = true;
        StartGame();
        CamHub.singleton.quickBar.SetActive(true);
        CamHub.singleton.Hidebar.SetActive(true);
        ready = true;
        Radio.singleton.OFF = false;
        startPhoneAnimDown = true;
    }

    public void AcceptCall()
    {
        callBTN.SetActive(false);
        ringtone.Stop();
        joinedCall.Play();
        main.SetTexture("_BaseMap", onCall);
        StartCoroutine(Call());
        
    }

    private void OnApplicationQuit()
    {
        main.SetTexture("_BaseMap", old);
    }

    public IEnumerator Call()
    {
        phoneguy.Play();
        CamHub.singleton.quickBar.SetActive(true);
        CamHub.singleton.Hidebar.SetActive(true);
        yield return new WaitForSeconds(phoneguy.clip.length+1);
        main.SetTexture("_BaseMap", onHome);
        ready = true;
        Radio.singleton.OFF = false;
        startPhoneAnimDown = true;
        info.text = "night started";
        info.GetComponent<AudioSource>().Play();
        SanityController.singleton.gameObject.SetActive(true);
        SanityController.singleton.sanityIsRunning = true;
        Radio.singleton.AuditabilityLevelText.gameObject.SetActive(true);
        Clock.timerIsRunning = true;
        PowerController.singleton.powerIsRunning = true;
        PowerController.singleton.gameObject.SetActive(true);
        StartCoroutine(HideInfo());
    }

    public void StartGame()
    {
        info.text = "night started";
        info.GetComponent<AudioSource>().Play();
        SanityController.singleton.gameObject.SetActive(true);
        SanityController.singleton.sanityIsRunning = true;
        Radio.singleton.AuditabilityLevelText.gameObject.SetActive(true);
        Clock.timerIsRunning = true;
        PowerController.singleton.powerIsRunning = true;
        PowerController.singleton.gameObject.SetActive(true);
        StartCoroutine(HideInfo());
    }

    internal IEnumerator HideInfo()
    {
        yield return new WaitForSeconds(5);
        info.gameObject.SetActive(false);
    }
}
