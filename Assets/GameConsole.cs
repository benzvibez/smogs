using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Linq;

public class GameConsole : MonoBehaviour
{
    public GameObject noclipandcinematichelp;
    private TMP_InputField input;

    [Serializable]
    public struct Command
    {
        public string invoker;
        public EventTrigger.TriggerEvent callback;
    }

    public static string cachedParam;

    public Command[] CommandsInfos;

    private void Awake()
    {
        spawnEulers = Camera.main.transform.eulerAngles;
        spawnPOS = Camera.main.transform.position;

        foreach (var c in CommandsInfos)
        {
            cmds.Add(c.invoker, c.callback);
        }
        input = GetComponent<TMP_InputField>();
    }

    public float sensitivity = 0.3f;
    float xR = 0f;
    public void Update()
    {

        if (noclipping)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            Camera current = Camera.main;
            current.gameObject.transform.Translate(current.gameObject.transform.TransformDirection(movement) * noclipSpeed * Time.deltaTime, Space.World);


            if (Input.GetKey(KeyCode.Q))
            {
                current.transform.Rotate(new Vector3(0, -CamHub.singleton.mainCameraRotationSpeed, 0 * 3), Space.World);
            }

            if (Input.GetKey(KeyCode.E))
            {
                current.transform.Rotate(new Vector3(0, CamHub.singleton.mainCameraRotationSpeed, 0 * 3), Space.World);
            }

            if (Input.GetKey(KeyCode.R))
            {
                current.gameObject.transform.Translate(Vector3.up * noclipSpeed * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.F))
            {
                current.gameObject.transform.Translate(Vector3.down * noclipSpeed * Time.deltaTime, Space.World);
            }
        }

        if (cinematic && noclipping)
        {
            float mX = Input.GetAxis("Mouse X") * sensitivity;
            float mY = Input.GetAxis("Mouse Y") * sensitivity;

            xR -= mY;
            MovePalyersCamera(xR, mX, 0);

            if (Input.GetKeyDown(KeyCode.L))
            {
                CamHub.singleton.mainCameraRotationSpeed = 0.15f;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                CamHub.singleton.camOverlay.SetActive(false);
                Clock.singleton.timerIsRunning = true;
                Clock.singleton.timeText.gameObject.SetActive(true);
                CamHub.singleton.quickBar.SetActive(true);
                input.text = "Resumed";
                cinematic = false;
                noclipping = false;
                Camera.main.transform.eulerAngles = spawnEulers;
                Camera.main.transform.position = spawnPOS;
            }
        }

        if (Input.GetKeyDown(KeyCode.BackQuote) || cinematic)
        {
            if (!input.interactable && !cinematic)
            {
                input.placeholder.GetComponent<TextMeshProUGUI>().text = "CMD Here";
                input.interactable = true;
                input.Select();
            }
            else
            {
                input.placeholder.GetComponent<TextMeshProUGUI>().text = "";
                input.text = "";
                input.interactable = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            RunCommand(input.text.ToLower());
            input.Select();
        }
    }

    public void MovePalyersCamera(float xR, float mX, float z)
    {
        Camera.main.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0);
        Camera.main.transform.transform.Rotate(xR, mX * 4.5f, z);
    }

    public void InternalRun(string param, string _cmd)
    {
        cachedParam = param;
        foreach (var cmd in cmds)
        {
            if (cmd.Key == _cmd)
            {
                cmd.Value.Invoke(null);
                return;
            }
        }
    }




    public void RunCommand(string invoke)
    {
        CheckForCommand(invoke, out EventTrigger.TriggerEvent Cmd, out string param, out string cmdName);

        if (Cmd != null)
        {
            InternalRun(param, cmdName);
        }
        else
            input.text = "Failed - 404 @invoke";
    }

    public Dictionary<string, EventTrigger.TriggerEvent> cmds = new Dictionary<string, EventTrigger.TriggerEvent>();

    internal void CheckForCommand(string checkFor, out EventTrigger.TriggerEvent output, out string param, out string cmdf)
    {
        string cachedCheckfor;
        cachedCheckfor = checkFor;
        checkFor += "~";

        char[] charArr = new char[checkFor.Length];
        for (int i = 0; i < checkFor.Length; i++)
            charArr[i] = checkFor[i];

        string FoundParam = "";
        foreach (char c in charArr)
        {
            if (c == '=')
            {
                for (int i = charArr.ToList().FindIndex(ch => ch == c) + 1; charArr[i] != '~'; i++)
                {
                    FoundParam += charArr[i];
                    continue;
                }
                break;
            }
        }

        if (cachedCheckfor.Contains(".") && !cachedCheckfor.Contains("="))
        {
            param = "";
            cmdf = cachedCheckfor;

            foreach (var cmd in cmds)
            {
                if (cmd.Key == cachedCheckfor)
                {
                    output = cmd.Value;
                    return;
                }
            }
            output = null;
        }
        else if (cachedCheckfor.Contains("=") && !cachedCheckfor.Contains("."))
        {

            string FoundCmd = "";
            foreach (char c in charArr)
            {
                if (c == '=')
                    break;
                else
                    FoundCmd += c;
            }

            param = FoundParam;
            checkFor = cachedCheckfor;
            cmdf = FoundCmd;
            foreach (var cmd in cmds)
            {
                if (cmd.Key == cmdf)
                {
                    output = cmd.Value;
                    return;
                }
            }
            output = null;
        } else
        {
            param = null;
            checkFor = null;
            cmdf = null;
            output = null;
        }
    }



    //COMMANDS BELOW


    public void mode()
    {
        string mode = cachedParam;
        if (mode == "invert")
        {
            ModeInvert();
        }
        else if (mode == "normal")
        {
            ModeNormal();
        }
        else if (mode == "raytracing")
        {
            ModeRaytracing();
        }
        else if (mode == "cinematic")
        {
            cinematicMode();
        } else
            input.text = "" + mode + " is not valid";

        cachedParam = "";
    }

    public static bool cinematic;
    public static bool noclipping = false;
    public float noclipSpeed = 0.5f;
    public Vector3 spawnPOS;
    public Vector3 spawnEulers;

    public void noclip()
    {
        
        string param = cachedParam;
        if (bool.Parse(param))
        {
            noclipandcinematichelp.SetActive(true);
            input.text = "entered noclip";
            noclipping = bool.Parse(param);
        }
        else
        {
            noclipandcinematichelp.SetActive(false);
            Camera.main.transform.position = spawnPOS;
        }
    }

    public void setNoclipSpeed()
    {
        input.text = "set nclip speed";
        string param = cachedParam;
        noclipSpeed = float.Parse(param);
    }

    public void setNoclipRSpeed()
    {
        input.text = "set R speed";
        string param = cachedParam;
        CamHub.singleton.mainCameraRotationSpeed = float.Parse("0."+param);
    }

    public void cinematicMode()
    {
        if (noclipping)
        {
            cinematic = true;
            sensitivity = 0.10f;
            noclipSpeed = 3.5f;
            CamHub.singleton.mainCameraRotationSpeed = 0.45f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CamHub.singleton.camOverlay.SetActive(false);
            Clock.singleton.timerIsRunning = false;
            Clock.singleton.timeText.gameObject.SetActive(false);
            CamHub.singleton.quickBar.SetActive(false);
            input.text = "";
        } else
        {
            input.text = "start noclip first";
        }
    }


    public void reload()
    {
        input.text = "reloading game";
        SceneManager.LoadScene("Game");
    }

    public void cinematicsens()
    {
        input.text = "set cine sens";
        sensitivity = float.Parse("0."+cachedParam);
    }


    public void ModeInvert()
    {
        if (!OptionDataContainer.STOREDinvertedMode)
        {
            OptionDataContainer.STOREDinvertedMode = true;
            OptionLoader.Loader.SetValues();
            input.text = "Completed";
        }
        else
            input.text = "Failed - A_A";
    }

    public void ModeNormal()
    {
        if (OptionDataContainer.STOREDinvertedMode || OptionDataContainer.STOREDrayTracing)
        {
            OptionDataContainer.STOREDrayTracing = false;
            OptionDataContainer.STOREDinvertedMode = false;
            OptionLoader.Loader.SetValues();
            input.text = "Reloading Scene";
            SceneManager.LoadScene("Game");
        }
        else
            input.text = "Failed - A_A";
    }

    public void ModeRaytracing()
    {
        if (!OptionDataContainer.STOREDrayTracing)
        {
            OptionDataContainer.STOREDrayTracing = true;
            OptionLoader.Loader.SetValues();
            input.text = "Completed";
        }
        else
            input.text = "Failed - A_A";
    }
}
