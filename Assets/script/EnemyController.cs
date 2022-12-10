using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    public GameObject Enemy;

    public List<GameObject> routeToRoom = new List<GameObject>();

    public List<GameObject> spawners = new List<GameObject>();

    public static EnemyController singleton;

    public int MoveRangeMin = 13;
    public int MoveRangeMax = 28;

    public int RangeInflate = 3;
    public int RangeDeflate = 5;

    public int RangeRandomizerAttempts = 30;

    public Algorithm ALG;


    private void Awake()
    {
        singleton = this;
        ALG = new Algorithm(spawners, routeToRoom, MoveRangeMin, MoveRangeMax, Enemy, RangeInflate, RangeDeflate, RangeRandomizerAttempts, this);
    }

    public void ReRegister()
    {
        ALG.ReRegister(spawners, routeToRoom, MoveRangeMin, MoveRangeMax, RangeInflate, RangeDeflate, RangeRandomizerAttempts);
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
    public int RangeDeflate;

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
        if (NextSpawner != null)
            GameConsole.singleton.ALGSPAWN.text = "NEXT ALGORITHM SPAWN: " + NextSpawner.name;

        GameConsole.singleton.ALGDEDSECS.text = "ALGORITHM DEDICATED SECONDS: " + DedicatedCurrentTimeRange;

        if (PastSpawner != null)
            GameConsole.singleton.ALGPASTSPAWNER.text = "PAST ALGORITHM SPAWNER: " + PastSpawner.name;

        if (CurrentSpawner != null)
            GameConsole.singleton.ALGCURRENTSPAWNER.text = "CURRENT ALGORITHM SPAWNER: " + CurrentSpawner.name;

        GameConsole.singleton.ALGLASTDEDSEC.text = "PAST ALGORITHM DEDICATED SECONDS: " + PreviousDedicatedTimeRange;

        GameConsole.singleton.ALGLASTLASTDEDSEC.text = "PAST-PAST ALGORITHM DEDICATED SECONDS: " + PreviousPreviousDedicatedTimeRange;

        GameConsole.singleton.ALGINFLATE.text = "ALGORITHM INFLATE: " + RangeInflate;

        GameConsole.singleton.ALGDEFLATE.text = "ALGORITHM DEFLATE: " + RangeDeflate;
        GameConsole.singleton.ALGATTACKING.text = "ALG ATTACKING: " + goingToRoom + "(" + canAttack + ")";
    }

    public bool Start()
    {
        if (NextSpawner == null)
            NextSpawner = GenerateNextSpawnerIndex();
        MonoBehaviour.print(NextSpawner);
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

            if (_DedicatedCurrentTimeRange == PreviousDedicatedTimeRange + RangeInflate || _DedicatedCurrentTimeRange == PreviousDedicatedTimeRange - RangeDeflate || _DedicatedCurrentTimeRange == PreviousPreviousDedicatedTimeRange || _DedicatedCurrentTimeRange == PreviousDedicatedTimeRange)
                continue;
            else
                return _DedicatedCurrentTimeRange;
        }
        Debug.LogWarning("ALG: failed to randomize next teleport time range. defaulting to the same time range as previously applied.");
        return DedicatedCurrentTimeRange;
    }

    GameObject GetClosestSpawner()
    {
        Debug.Log("going to cloests spawner within regions");
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = enemyController.Enemy.transform.position;
        foreach (GameObject t in RegisteredSpawners)
        {

            if (t.gameObject == CurrentSpawner || t.gameObject == PastSpawner)
                continue;

            float dist = Vector3.Distance(t.transform.position, currentPos);

            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin.gameObject;
    }

    public List<GameObject> RouteToRoom = new();
    public bool goingToRoom;
    public bool LeavingRoom;
    public int goingToRoomNextIndex = 0;
    public GameObject goingToRoomCurrentRoom;

    public int canAttack = 3;

    public GameObject GenerateNextSpawnerIndex()
    {
        var type = Random.Range(0, 32);
        if (type > 26)
        {
            return RegisteredSpawners[Random.Range(RangeDeflate - RangeInflate + 1, RegisteredSpawners.Count)];
        }
        else if (type > 0 && type < 21 && !goingToRoom && !LeavingRoom) //high chance that it will go on approach to office
        {
            if (canAttack != 0)
            {
                canAttack--;
                return GetClosestSpawner();
            }
            else
            {
                canAttack = 3;
                Debug.Log("Room approach started");
                goingToRoom = true;
                goingToRoomCurrentRoom = RouteToRoom[goingToRoomNextIndex];
                goingToRoomNextIndex++;

                return goingToRoomCurrentRoom;
            }
        }
        else if (goingToRoom && !LeavingRoom)
        {
            Debug.Log("Room approach moving");
            goingToRoomNextIndex++;
            if (goingToRoomNextIndex == 3)
            {
                var idx = Random.Range(3, 5); // if he goes here plauer dead NOT IMPLIMENTED YET
                goingToRoom = false;
                LeavingRoom = true;
                goingToRoomCurrentRoom = null;
                return RouteToRoom[goingToRoomNextIndex];
            }
            else
            {
                goingToRoomCurrentRoom = RouteToRoom[goingToRoomNextIndex];
                return goingToRoomCurrentRoom;
            }
        }
        else if (LeavingRoom && !goingToRoom)
        {
            goingToRoomNextIndex--;
            if (goingToRoomNextIndex == 0)
            {
                Debug.Log("Room approach leaving");
                goingToRoom = false;
                LeavingRoom = false;
                goingToRoomNextIndex = 0;
                goingToRoomCurrentRoom = null;
                return GetClosestSpawner();
            }
            else
            {
                goingToRoomCurrentRoom = RouteToRoom[goingToRoomNextIndex];
                return goingToRoomCurrentRoom;
            }
        }
        else if (type > 0 && type < 25 && !goingToRoom) // high chance that it will go to nearest spawner regions
            return GetClosestSpawner();

        return GetClosestSpawner();
    }

    public Algorithm(List<GameObject> spawners, List<GameObject> RTR, int min, int max, GameObject enemy, int rangeInflate, int rangeDeflate, int rangeRandomizerAttempts, EnemyController EnemyController)
    {
        RegisteredSpawners = spawners;
        Min = min;
        Max = max;
        Enemy = enemy;
        RangeInflate = rangeInflate;
        RangeDeflate = rangeDeflate;
        RangeRandomizerAttempts = rangeRandomizerAttempts;
        enemyController = EnemyController;
        RouteToRoom = RTR;
    }

    public void ReRegister(List<GameObject> Spawners, List<GameObject> RTR, int min, int max, int rangeInflate, int rangeDeflate, int rangeRandomizerAttempts)
    {
        RegisteredSpawners = Spawners;
        Min = min;
        Max = max;
        RangeInflate = rangeInflate;
        RangeDeflate = rangeDeflate;
        RangeRandomizerAttempts = rangeRandomizerAttempts;
        RouteToRoom = RTR;
    }

}
