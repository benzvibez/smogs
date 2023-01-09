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
    internal TMP_InputField input;
    public GameObject DEBUG;


    [Serializable]
    public struct Command
    {
        public string invoker;
        public EventTrigger.TriggerEvent callback;
    }

    public static GameConsole singleton;

    public static string cachedParam;

    public Command[] CommandsInfos;

    internal TextMeshProUGUI fps;
    internal TextMeshProUGUI MODE;
    internal TextMeshProUGUI NIGHT;
    internal TextMeshProUGUI TIMESINCESTART;
    internal TextMeshProUGUI DISCORDUSER;
    internal TextMeshProUGUI DISCORDUSERID;
    internal TextMeshProUGUI MAINCAMPOS;
    internal TextMeshProUGUI MAINCAMSPEED;
    internal TextMeshProUGUI TIMELEFT;
    internal TextMeshProUGUI NOCLIP;
    internal TextMeshProUGUI CINEMATIC;
    internal TextMeshProUGUI ALGSPAWN;
    internal TextMeshProUGUI ALGDEDSECS;
    internal TextMeshProUGUI ALGPASTSPAWNER;
    internal TextMeshProUGUI ALGCURRENTSPAWNER;
    internal TextMeshProUGUI ALGLASTDEDSEC;
    internal TextMeshProUGUI ALGLASTLASTDEDSEC;
    internal TextMeshProUGUI ALGINFLATE;
    internal TextMeshProUGUI ALGDEFLATE;
    internal TextMeshProUGUI MAINCAMROT;
    internal TextMeshProUGUI ALGATTACKING;
    internal TextMeshProUGUI POWERPERCENT;
    internal TextMeshProUGUI POWERUSAGE;
    internal TextMeshProUGUI POWERMULTIPLIER;


    private float timeleft = 360;
    private float deltaTime;
    public void SetDebugValues()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fpss = 1.0f / deltaTime;
        fps.text = "FPS: " + Mathf.Ceil(fpss).ToString();

        if (OptionDataContainer.STOREDinvertedMode && !OptionDataContainer.STOREDrayTracing)
            MODE.text = "MODE: Inverted";
        else if (OptionDataContainer.STOREDinvertedMode && OptionDataContainer.STOREDrayTracing)
            MODE.text = "MODE: Inverted & Raytracing";
        else if (!OptionDataContainer.STOREDinvertedMode && OptionDataContainer.STOREDrayTracing)
            MODE.text = "MODE: Raytracing";
        else if (!OptionDataContainer.STOREDinvertedMode && !OptionDataContainer.STOREDrayTracing)
            MODE.text = "MODE: Normal";

        float minutes = Mathf.FloorToInt(Time.unscaledTime / 60);
        float seconds = Mathf.FloorToInt(Time.unscaledTime % 60);
        TIMESINCESTART.text = "TIME SINCE GAME START: " + minutes + ":" + seconds;

        if (Camera.main != null)
        {
            MAINCAMPOS.text = "MAIN CAM POS: " + Camera.main.transform.position;
        }
        else
        {
            MAINCAMPOS.text = "MAIN CAM POS: Not showing";
        }

        if (Camera.main != null)
        {
            MAINCAMROT.text = "MAIN CAM ROT: " + Camera.main.transform.eulerAngles;
        }
        else
        {
            MAINCAMROT.text = "MAIN CAM ROT: Not showing";
        }

        MAINCAMSPEED.text = "MAIN CAM SPEED: " + CamHub.singleton.mainCameraRotationSpeed;

        if (!Clock.singleton.stoppedTimer)
        {
            var t = (timeleft -= Time.deltaTime * 1f);
            float minutes1 = Mathf.FloorToInt(t / 60);
            float seconds1 = Mathf.FloorToInt(t % 60);


            TIMELEFT.text = "TIME LEFT: " + minutes1 + ":" + seconds1;
        }

        NOCLIP.text = "NOCLIP: " + noclipping;

        CINEMATIC.text = "CINEMATIC " + cinematic;
    }

    private void Awake()
    {
        singleton = this;
        foreach (var g in DEBUG.GetComponentsInChildren<Transform>())
        {
            if (g.name == "DEBUG")
                continue;

            TextMeshProUGUI t = g.GetComponent<TextMeshProUGUI>();
            if (t.name == "FPS")
            {
                fps = t;
            }
            else if (t.name == "Mode")
            {
                MODE = t;
            }
            else if (t.name == "Night")
            {
                NIGHT = t;
            }
            else if (t.name == "TIME SINCE START")
            {
                TIMESINCESTART = t;
            }
            else if (t.name == "DISCORD USER")
            {
                DISCORDUSER = t;
            }
            else if (t.name == "DISCORD USER ID")
            {
                DISCORDUSERID = t;
            }
            else if (t.name == "MAIN CAM POS")
            {
                MAINCAMPOS = t;
            }
            else if (t.name == "MAIN CAM SPEED")
            {
                MAINCAMSPEED = t;
            }
            else if (t.name == "TIME LEFT")
            {
                TIMELEFT = t;
            }
            else if (t.name == "Noclip")
            {
                NOCLIP = t;
            }
            else if (t.name == "Cinematic")
            {
                CINEMATIC = t;
            }
            else if (t.name == "ALG SPAWN")
            {
                ALGSPAWN = t;
            }
            else if (t.name == "ALG DED SECS")
            {
                ALGDEDSECS = t;
            }
            else if (t.name == "ALG PAST SPAWNER")
            {
                ALGPASTSPAWNER = t;
            }
            else if (t.name == "ALG CURRENT SPAWNER")
            {
                ALGCURRENTSPAWNER = t;
            }
            else if (t.name == "ALG LAST DED SEC")
            {
                ALGLASTDEDSEC = t;
            }
            else if (t.name == "ALG LAST LAST DED SEC")
            {
                ALGLASTLASTDEDSEC = t;
            }
            else if (t.name == "ALG INFLATE")
            {
                ALGINFLATE = t;
            }
            else if (t.name == "ALG DEFLATE")
            {
                ALGDEFLATE = t;
            }
            else if (t.name == "MAIN CAM ROT")
            {
                MAINCAMROT = t;
            }
            else if (t.name == "attacking")
            {
                ALGATTACKING = t;
            }
            else if (t.name == "POWER")
            {
                POWERPERCENT = t;
            }
            else if (t.name == "USAGE")
            {
                POWERUSAGE = t;
            }
            else if (t.name == "POWER MUlTIPLIER")
            {
                POWERMULTIPLIER = t;
            }
        }

        DEBUG.SetActive(false);

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
        SetDebugValues();

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
                Clock.timerIsRunning = true;
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
        }
        else
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
        }
        else
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
        CamHub.singleton.mainCameraRotationSpeed = float.Parse("0." + param);
    }


    public void Attack()
    {
        bool.TryParse(cachedParam, out var yesno);
        if (yesno)
        {
            EnemyController.singleton.ALG.canAttack = 0;
            EnemyController.singleton.ALG.Move();
        } else
        {
            EnemyController.singleton.ALG.canAttack = 0;
            EnemyController.singleton.ALG.Move();
        }

    }


    public void SetPowerP()
    {
        PowerController.singleton.powerRemaining = Mathf.FloorToInt(float.Parse(cachedParam) / PowerController.singleton.powerTimeInSeconds * 100);
    }

    public void SetPowerT()
    {
        PowerController.singleton.powerRemaining = float.Parse(cachedParam);
    }

    public void SetPowerM()
    {
        PowerController.singleton.additionalPower = float.Parse(cachedParam);
    }

    public void SetPowerU()
    {
        if (uint.Parse(cachedParam) > 4)
            throw new Exception("you cannot try to set the usage level above 5 or below 0!");

        PowerController.singleton.UsageLevel = uint.Parse(cachedParam);
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
            Clock.timerIsRunning = false;
            Clock.singleton.timeText.gameObject.SetActive(false);
            CamHub.singleton.quickBar.SetActive(false);
            input.text = "";
        }
        else
        {
            input.text = "start noclip first";
        }
    }


    public void reload()
    {
        input.text = "reloading game";
        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadScene("Game");
    }

    public void cinematicsens()
    {
        input.text = "set cine sens";
        sensitivity = float.Parse("0." + cachedParam);
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

    public void reloadAlg()
    {
        EnemyController.singleton.ReRegister();
        input.text = "reloaded ALG";
    }

    public void DEVELOPERMODE()
    {
        if (bool.Parse(cachedParam))
        {
            input.text = "Dev Mode On";
            DEBUG.SetActive(true);
        }
        else
        {
            DEBUG.SetActive(false);
            input.text = "Dev Mode Off";
        }
    }
}
