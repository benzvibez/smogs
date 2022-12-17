// Algorithm designed by Aiden C. Desjarlais

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject DoorToRoom;
    public GameObject DeadImage;

    public List<GameObject> AttackWanderPoints = new List<GameObject>();

    public List<GameObject> routeToRoom = new List<GameObject>();

    public List<GameObject> spawners = new List<GameObject>();

    public static EnemyController singleton;

    public int MoveRangeMin = 13;
    public int MoveRangeMax = 28;

    public int RangeInflate = 3;
    public int RangeDeflate = 5;

    public int RangeRandomizerAttempts = 30;
    public float AttackTime;
    public float AttackRealizationTime;
    public Algorithm ALG;


    private void Awake()
    {
        singleton = this;
        ALG = new Algorithm(AttackTime, spawners, routeToRoom, MoveRangeMin, MoveRangeMax, Enemy, RangeInflate, RangeDeflate, RangeRandomizerAttempts, this);
    }

    public void ReRegister()
    {
        ALG.ReRegister(AttackTime, spawners, routeToRoom, MoveRangeMin, MoveRangeMax, RangeInflate, RangeDeflate, RangeRandomizerAttempts);
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
    public float attackTime;
    public EnemyController enemyController;
    public bool move;
    public bool Run()
    {
        

        if (preAttack)
            DoorOpenAnim();
        else
            DoorCloseAnim();

        if (attacking)
        {
            CamHub.singleton.hideOff = true;
        } else
            CamHub.singleton.hideOff = false;

        if (attacking)
        {
            if (!CamHub.singleton.hidden)
            {//dead
                enemyController.StartCoroutine(Jumpscare());
            }
        }

        Debug.Log(enemyController.DoorToRoom.transform.eulerAngles.y);

        if (move)
            CheckForNextPoint();

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
        if (NextSpawner == null && !attacking && !preAttack)
            NextSpawner = GenerateNextSpawnerIndex();

        MonoBehaviour.print(NextSpawner);
        enemyController.StartCoroutine(WaitOnDTRForReInit());
        return false;
    }
    public Color cachedStaticAlpha;
    public void Move(bool Override = false)
    {
        if (attacking || preAttack && !Override)
            return;

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

        if (!attacking || !preAttack)
        {
            Move();
            cachedStaticAlpha = StaticFX.singleton.staticness.color;

            if (!CamHub.singleton.mainCamera.enabled)
                StaticFX.singleton.staticness.color = new Color(StaticFX.singleton.staticness.color.r, StaticFX.singleton.staticness.color.b, StaticFX.singleton.staticness.color.g, 1);

            yield return new WaitForSeconds(1.3f);

            StaticFX.singleton.staticness.color = cachedStaticAlpha;

            Cooldown = false;
        }    
        else
            yield return false;
        
    }

    public int GenerateDedicatedTimeRange()
    {
        if (attacking || preAttack)
            return 0;

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
        if (attacking || preAttack)
            return null;

        Debug.Log("going to closest spawner within regions");
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
    public bool attacking;
    public int canAttack = 3;

    public GameObject GenerateNextSpawnerIndex()
    {
        if (attacking || preAttack)
            return null;

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
                goingToRoomCurrentRoom = null;
                return RouteToRoom[goingToRoomNextIndex];
            }
            else if (goingToRoomNextIndex == 4 && goingToRoom)
            {
                Debug.Log("Attacking player");
                enemyController.StartCoroutine(AttackPlayer());
                NextSpawner = RouteToRoom[goingToRoomNextIndex-1];
                Move(true);
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

    public void DoorOpenAnim()
    {
        if (enemyController.DoorToRoom.transform.eulerAngles.y <= 340)
            enemyController.DoorToRoom.transform.Rotate(new Vector3(0, 0, -2f) * 0.2f);
    }

    public void DoorCloseAnim()
    {
        if (enemyController.DoorToRoom.transform.eulerAngles.y < 90)
            enemyController.DoorToRoom.transform.Rotate(new Vector3(0, 0, 2f) * 0.2f);
    }


    bool movingToPoint;
    GameObject point;

    public void CheckForNextPoint()
    {
        if (movingToPoint)
        {
            Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, point.transform.position, 0.2f);
        } else
        {
            enemyController.StartCoroutine(AttackWonderAnim());
        }
    }

    public IEnumerator Jumpscare()
    {
        if (!CamHub.singleton.mainCamera.enabled)
            CamHub.singleton.CamerasED();
        CamHub.off = true;
        CamHub.singleton.hideOff = true;
        StopAttacking();
        CamHub.singleton.mainCamera.transform.LookAt(new Vector3(Enemy.transform.position.x, CamHub.singleton.mainCamera.transform.position.y, Enemy.transform.position.z));
        Enemy.transform.position = new Vector3(CamHub.singleton.mainCamera.transform.position.x, Enemy.transform.position.y, CamHub.singleton.mainCamera.transform.position.z-1.5f); ;
        //play jumpscare sound here
        
        yield return new WaitForSeconds(2.5f);
        enemyController.DeadImage.SetActive(true);
        ExitMenu.singleton.StopEverything();
    }

    public List<Light> cachedLightData = new();

    public bool preAttack;
    public IEnumerator AttackPlayer()
    {
        preAttack = true;
        yield return new WaitForSeconds(enemyController.AttackRealizationTime);
        move = true;
        attacking = true;
        //OFFICALLY IN ROOM ATTACKING
        NextSpawner = RouteToRoom[4];
        Move(true);



        if (!CamHub.singleton.hidden)
        {//dead
            enemyController.StartCoroutine(Jumpscare());
        } else {


            enemyController.StartCoroutine(AttackWonderAnim());

            yield return new WaitForSeconds(attackTime);
            preAttack = false;
            move = false;
            attacking = false;
            point = null;
            enteredRoom = false;
            movingToPoint = false;
            goingToRoom = false;
            enemyController.DoorToRoom.transform.Rotate(new Vector3(0, 0, 4));
            LeavingRoom = true;
            Start();
        }
    }

    public void StopAttacking()
    {
        preAttack = false;
        move = false;
        attacking = false;
        point = null;
        enteredRoom = false;
        movingToPoint = false;
        goingToRoom = false;
    }
    
    public bool enteredRoom;

    public IEnumerator AttackWonderAnim()
    {
        if (!enteredRoom)
        {
            point = enemyController.AttackWanderPoints[0];
            movingToPoint = true;
            yield return new WaitForSeconds(2);
            movingToPoint = false;
            enteredRoom = true;
        }
        else
        {
            movingToPoint = true;
            var r = Random.Range(1, enemyController.AttackWanderPoints.Count);
            point = enemyController.AttackWanderPoints[r];
            yield return new WaitForSeconds(4);
            movingToPoint = false;
        }
    }


    public Algorithm(float at, List<GameObject> spawners, List<GameObject> RTR, int min, int max, GameObject enemy, int rangeInflate, int rangeDeflate, int rangeRandomizerAttempts, EnemyController EnemyController)
    {
        attackTime = at;
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

    public void ReRegister(float at, List<GameObject> Spawners, List<GameObject> RTR, int min, int max, int rangeInflate, int rangeDeflate, int rangeRandomizerAttempts)
    {
        attackTime = at;
        RegisteredSpawners = Spawners;
        Min = min;
        Max = max;
        RangeInflate = rangeInflate;
        RangeDeflate = rangeDeflate;
        RangeRandomizerAttempts = rangeRandomizerAttempts;
        RouteToRoom = RTR;
    }

}
