using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Enemy;
    public List<GameObject> spawners = new List<GameObject>();

    public static EnemyController singleton;

    public int MoveRangeMin = 13;
    public int MoveRangeMax = 28;

    public int RangeInflate = 3;
    public int RangeDefalte = 5;

    public int RangeRandomizerAttempts = 30;

    public Algorithm ALG;


    private void Awake()
    {
        singleton = this;
        ALG = new Algorithm(spawners, MoveRangeMin, MoveRangeMax, Enemy, RangeInflate, RangeDefalte, RangeRandomizerAttempts, this);
    }

    public void ReRegister()
    {
        ALG.ReRegister(spawners, MoveRangeMin, MoveRangeMax, RangeInflate, RangeDefalte, RangeRandomizerAttempts);
    }

    private void Update()
    {
        ALG.Run();
    }

}


public class Algorithm
{
    public GameObject CurrentSpawner;
    public GameObject NextSpawner;
    public GameObject PastSpawner;

    public GameObject Enemy;

    public int Min;
    public int Max;

    public bool Cooldown;

    public List<GameObject> RegisteredSpawners = new List<GameObject>();

    public int DedicatedCurrentTimeRange;
    public int PreviousDedicatedTimeRange;
    public int PreviousPreviousDedicatedTimeRange;

    public int RangeInflate;
    public int RangeDefalte;

    public int RangeRandomizerAttempts;

    public EnemyController enemyController;

    public bool Run()
    {
        if (!Cooldown)
        {
            DedicatedCurrentTimeRange = GenerateDedicatedTimeRange();
            Cooldown = true;
            return Start();
        }
        else
        {
            return false;
        }
    }


    public void SetDebugger()
    {
        GameConsole.singleton.ALGSPAWN.text = "NEXT ALGORITHM SPAWN: " + NextSpawner.name;

        GameConsole.singleton.ALGDEDSECS.text = "ALGORITHM DEDICATED SECONDS: " + DedicatedCurrentTimeRange;

        if (PastSpawner != null)
            GameConsole.singleton.ALGPASTSPAWNER.text = "PAST ALGORITHM SPAWNER: " + PastSpawner.name;

        if (CurrentSpawner != null)
            GameConsole.singleton.ALGCURRENTSPAWNER.text = "CURRENT ALGORITHM SPAWNER: " + CurrentSpawner.name;

        GameConsole.singleton.ALGLASTDEDSEC.text = "PAST ALGORITHM DEDICATED SECONDS: " + PreviousDedicatedTimeRange;

        GameConsole.singleton.ALGLASTLASTDEDSEC.text = "PAST-PAST ALGORITHM DEDICATED SECONDS: " + PreviousPreviousDedicatedTimeRange;

        GameConsole.singleton.ALGINFLATE.text = "ALGORITHM INFLATE: " + RangeInflate;

        GameConsole.singleton.ALGDEFLATE.text = "ALGORITHM DEFLATE: " + RangeDefalte;
    }

    public bool Start()
    {
        if (NextSpawner == null)
            NextSpawner = GenerateNextSpawnerIndex();
        enemyController.StartCoroutine(WaitOnDTRForReInit());
        return false;
    }


    public void Move()
    {
        GameObject cached = null;
        if (CurrentSpawner != null)
            cached = CurrentSpawner;

        CurrentSpawner = NextSpawner;
        Enemy.transform.position = CurrentSpawner.transform.position;
        PastSpawner = cached;
        NextSpawner = GenerateNextSpawnerIndex();
        SetDebugger();
    }

    public IEnumerator WaitOnDTRForReInit()
    {
        yield return new WaitForSeconds(DedicatedCurrentTimeRange);
        Move();
        Cooldown = false;
    }

    public int GenerateDedicatedTimeRange()
    {
        PreviousPreviousDedicatedTimeRange = PreviousDedicatedTimeRange;
        PreviousDedicatedTimeRange = DedicatedCurrentTimeRange;

        for (var i = 0; i != RangeRandomizerAttempts; i++)
        {
            var _DedicatedCurrentTimeRange = Mathf.RoundToInt(Random.Range(Min, Max));

            if (_DedicatedCurrentTimeRange == PreviousDedicatedTimeRange + RangeInflate || _DedicatedCurrentTimeRange == PreviousDedicatedTimeRange - RangeDefalte || _DedicatedCurrentTimeRange == PreviousPreviousDedicatedTimeRange || _DedicatedCurrentTimeRange == PreviousDedicatedTimeRange)
                continue;
            else
                return _DedicatedCurrentTimeRange;
        }
        Debug.LogWarning("ALG: failed to randomize next teleport time range. defaulting to the same time range as previously applied.");
        return DedicatedCurrentTimeRange;
    }


    public GameObject GenerateNextSpawnerIndex()
    {
        

        for (var i = 0; i != RangeRandomizerAttempts; i++)
        {
            GameObject _NextSpawner = RegisteredSpawners[Mathf.RoundToInt(Random.Range(0, RegisteredSpawners.Count))];
            if (_NextSpawner == PastSpawner || _NextSpawner == CurrentSpawner)
                continue;
            else
                return _NextSpawner;
        }
        Debug.LogWarning("ALG: failed to randomize next position. defaulting to the same position as we are in now.");
        return CurrentSpawner;
    }


    public Algorithm(List<GameObject> spawners, int min, int max, GameObject enemy, int rangeInflate, int rangeDeflate, int rangeRandomizerAttempts, EnemyController EnemyController)
    {
        RegisteredSpawners = spawners;
        Min = min;
        Max = max;
        Enemy = enemy;
        RangeInflate = rangeInflate;
        RangeDefalte = rangeDeflate;
        RangeRandomizerAttempts = rangeRandomizerAttempts;
        enemyController = EnemyController;
    }

    public void ReRegister(List<GameObject> Spawners, int min, int max, int rangeInflate, int rangeDeflate, int rangeRandomizerAttempts)
    {
        RegisteredSpawners = Spawners;
        Min = min;
        Max = max;
        RangeInflate = rangeInflate;
        RangeDefalte = rangeDeflate;
        RangeRandomizerAttempts = rangeRandomizerAttempts;
    }

}
