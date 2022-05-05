using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGeneral : MonoBehaviour
{
    private delegate bool Condition();
    private delegate void Action();

    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] buildingsPrefabs;

    [SerializeField] Transform spawnPoint;
    public List<Unit> enemies;
    public List<Unit> constructors;
    public List<Building> buildings;
    public Building enemyBase;
    private Unit enemy;

    public GameObject nearestEnemy;

    [SerializeField] float evaluationRate = 100f;
    private float timeSinceLastEvaluation = Mathf.Infinity;

    [SerializeField] float spawnRate = 10f;
    private float timeSinceLastSpawn = Mathf.Infinity;
    private bool start;
    private float dangerDistance = 50f;

    List<Condition> conditions = new List<Condition>();
    List<Action> actions = new List<Action>();

    [SerializeField] Transform[] points;
    public Unit unit1;
    public Unit unit2;
    public Unit unit3;
    public Unit unit4;

    public int metall;
    public int monedes;
    public int energia;

    // Regla: tupla de condicio-accio

    private void Awake()
    {
        conditions.Add(Condition1);
        conditions.Add(Condition2);
        conditions.Add(Condition3);
        conditions.Add(Condition4);
        conditions.Add(Condition5);
        conditions.Add(Condition6);
        conditions.Add(Condition7);
        conditions.Add(Condition8);
        conditions.Add(Condition9);
        /*conditions.Add(Condition10);
        conditions.Add(Condition11);
        conditions.Add(Condition12);*/
        actions.Add(Action1);
        actions.Add(Action2);
        actions.Add(Action3);
        actions.Add(Action4);
        actions.Add(Action5);
        actions.Add(Action6);
        actions.Add(Action7);
        actions.Add(Action8);
        actions.Add(Action9);
        /*actions.Add(Action10);
        actions.Add(Action11);
        actions.Add(Action12);*/

        enemies = new List<Unit>();
        constructors = new List<Unit>();
        buildings = new List<Building>();

        enemyBase = GameObject.Find("/EnemyBase").GetComponent<Building>();
    }

    void Start()
    {
        
        start = false;

        metall = 500;
        monedes = 3000;
        energia = 0;

        StartCoroutine(startAI());
    }

    void Update()
    {
        timeSinceLastEvaluation += Time.deltaTime;
        if (timeSinceLastEvaluation > evaluationRate && start)
        {
            Evaluate();
            timeSinceLastEvaluation = 0;
        }
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > spawnRate && Input.GetKey(KeyCode.L))
        {
            start = true;
        }
        if (Input.GetKey(KeyCode.C))
        {

        }
    }

    private void Evaluate()
    {
        // cada cop que comença l'avaluació netejo les llistes d'unitats i edificis per evitar que hi haguin nulls
        if (enemies.Count > 0)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i] == null)
                {
                    enemies[i] = enemies[enemies.Count - 1];
                    enemies.RemoveAt(enemies.Count - 1);
                }
            }
        }
        if (constructors.Count > 0)
        {
            for (int i = constructors.Count - 1; i >= 0; i--)
            {
                if (constructors[i] == null)
                {
                    constructors[i] = constructors[constructors.Count - 1];
                    constructors.RemoveAt(constructors.Count - 1);
                }
            }
        }
        if (buildings.Count > 0)
        {
            for (int i = buildings.Count - 1; i >= 0; i--)
            {
                if (buildings[i] == null)
                {
                    buildings[i] = buildings[buildings.Count - 1];
                    buildings.RemoveAt(buildings.Count - 1);
                }
            }
        }

        Debug.Assert(conditions.Count == actions.Count); // Assert: Si no se cumple, el codigo peta y te indica donde

        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i]())
            {
                actions[i]();
                //Debug.LogWarning(i);
                break;
            }
        }
    }

    // Condició 1 - Si hi ha un edifici ferit envia totes les unitats a atacar a la unitat del jugador mes aprop de la base
    private bool Condition1()
    {
        bool ret = false;
        foreach(Building building in buildings)
        {
            if(building.constructed && building.health.getHealth()< building.health.maxHealth)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
    private void Action1()
    {
        GameObject nearestEnemy = FindClosestEnemy();
        foreach(Unit unit in enemies)
        {
            unit.target = nearestEnemy.transform;
        }
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                break;
            }
        }
    }

    // Condició 2 - Si hi menys de 2 constructors i hi han prous recursos spawneja constructors
    private bool Condition2()
    {
        bool canSpawn = false;
        if (enemyBase!=null && enemyBase.canSpawn)
        {
            canSpawn=true;
        }
        
        return constructors.Count<2 && monedes>200 && metall>50 && canSpawn;
    }
    private void Action2()
    {
        unit1 = SpawnEnemy(enemyPrefabs[1], enemyBase.transform.position, enemyBase.transform.position-new Vector3(0,0,10));
        constructors.Add(unit1);
    }

    // Condició 3 - Si hi menys de 2 mines, hi han constructors lliures i hi han prous recursos spawneja mines i envia constructors a construïr
    private bool Condition3()
    {
        int mina = 0;
        foreach(Building building in buildings)
        {
            
            if (building.name.Equals("Mine"))
            {
                mina ++;
            }
        }
        return (mina<2 && monedes > 500 && energia >= 5);
    }
    private void Action3()
    {
        Building building = null;
        Vector3 minePosition = checkClosestUnoccupiedMineral();
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                building = SpawnBuilding(buildingsPrefabs[1], minePosition).GetComponent<Building>();
                constructor.target = building.transform;
                break;
            }
        }
        buildings.Add(building);
    }
    private bool Condition4()
    {
        bool canConstruct = false;
        int petrolPumps = 0;
        foreach (Building building in buildings)
        {
            Debug.Log(building.name);
            if (building.name.Equals("PetrolPump"))
            {
                petrolPumps++;
            }
        }
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                canConstruct = true;
                break;
            }
        }
        //return !petrolPump && monedes > 400 && metall > 100;
        return petrolPumps < 5 && monedes > 400 && metall > 100 && canConstruct && energia >=5;
    }
    private void Action4()
    {
        Building building = null;
        Vector3 position = checkIfCanBuild();
        //Building mina = SpawnBuilding(buildingsPrefabs[1], minePosition);
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                building = SpawnBuilding(buildingsPrefabs[0], position).GetComponent<Building>();
                buildings.Add(building);
                constructor.target = building.transform;
                buildings.Add(building);
                break;
            }
        }  
    }
    private bool Condition5()
    {
        bool canSpawn = false;
        if (enemyBase != null && enemyBase.canSpawn)
        {
            canSpawn = true;
        }
        return constructors.Count < 3 && monedes > 500 && metall > 50 && canSpawn;
    }
    private void Action5()
    {
        unit1 = SpawnEnemy(enemyPrefabs[1], enemyBase.transform.position, enemyBase.transform.position - new Vector3(0, 0, 10));
        constructors.Add(unit1);
    }
    private bool Condition6()
    {
        int mina = 0;
        foreach (Building building in buildings)
        {
            if (building.name.Equals("Mine"))
            {
                mina++;
            }
        }
        return mina<2 && monedes > 500;
    }
    private void Action6()
    {
        Building building = null;
        Vector3 minePosition = checkClosestUnoccupiedMineral();
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                building = SpawnBuilding(buildingsPrefabs[1], minePosition).GetComponent<Building>();
                constructor.target = building.transform;
                buildings.Add(building);
                break;
            }
        }
        
    }
    private bool Condition7()
    {
        return energia<20 && monedes > 200 && metall > 50;
    }
    private void Action7()
    {
        Building building = null;
        Vector3 minePosition = checkIfCanBuild();
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                building = SpawnBuilding(buildingsPrefabs[2], minePosition).GetComponent<Building>();
                constructor.target = building.transform;
                buildings.Add(building);
                break;
            }
        }

    }
    private bool Condition8()
    {
        int WarFactory = 0;
        foreach (Building building in buildings)
        {
            if (building.name.Equals("WarFactory"))
            {
                WarFactory++;
            }
        }
        return WarFactory < 1 && monedes > 800 && metall > 100;
    }
    private void Action8()
    {
        Building building = null;
        Vector3 position = checkIfCanBuild();
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                building = SpawnBuilding(buildingsPrefabs[3], position).GetComponent<Building>();
                buildings.Add(building);
                constructor.target = building.transform;
                buildings.Add(building);
                break;
            }
        }
    }

    private bool Condition9()
    {
        int warFactory = 0;
        foreach (Building building in buildings)
        {
            if (building.name.Equals("WarFactory") && building.constructed)
            {
                warFactory++;
                spawnPoint = building.transform;
                break;
            }
        }
        return enemies.Count < 4 && monedes > 200 && metall > 300 && warFactory>0;
    }
    private void Action9()
    {
        Unit unit = SpawnEnemy(enemyPrefabs[0], spawnPoint.position, points[1].position);
        enemies.Add(unit);
        GameObject target = FindClosestEnemy();
        if (target != null)
        {
            unit.target = target.transform;
        }
        else
        {
            target = FindClosestEnemyBuilding();
            unit.target = target.transform;
        }

    }

    //(int level, int hp)
    public Unit SpawnEnemy(GameObject prefab , Vector3 point, Vector3 destination)
    {
        timeSinceLastSpawn = 0;
        GameObject enemy =
            Instantiate(prefab, point, Quaternion.identity) as GameObject;
        Unit enemyUnit = enemy.GetComponentInChildren<Unit>();
        RestarRecursos(enemyUnit.unitData.MoneyCost, enemyUnit.unitData.MetalCost);

        enemyUnit.team = 2;
        enemyUnit.agent.SetDestination(destination);

        return enemyUnit;
    }
    public Building SpawnBuilding(GameObject prefab, Vector3 point)
    {
        timeSinceLastSpawn = 0;
        GameObject enemy = Instantiate(prefab, point, Quaternion.identity) as GameObject;
        Building enemyBuilding = enemy.GetComponent<Building>();
        enemyBuilding.team = 2;

        RestarRecursos(enemyBuilding.data.MoneyCost, enemyBuilding.data.MetalCost);        

        return enemyBuilding;
    }
    public void RestarRecursos(int money, int metal)
    {
        metall -= metal;
        monedes -= money;
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Unit");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<Unit>().team != 2)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }
    public GameObject FindClosestEnemyBuilding()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Building");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if (go.GetComponentInParent<Building>().team != 2)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    private Vector3 checkClosestUnoccupiedMineral()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Minerals");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            // miro que no hi hahui cap edifici o unitat en aquell mineral
            Collider[] hitColliders = Physics.OverlapSphere(go.transform.position, 3);

            if (hitColliders.Length < 3)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest.transform.position;
    }

    private Vector3 checkIfCanBuild()
    {
        int unstuck = 0;
        bool canbuild = false;
        Vector3 randomPosition = RandomPointOnCircleEdge(dangerDistance/2);
        while (unstuck<20 || canbuild)
        {
            Collider[] hitColliders = Physics.OverlapSphere(randomPosition, 3);
            if (hitColliders.Length > 1)
            {
                Debug.LogWarning(hitColliders.Length);
                randomPosition = RandomPointOnCircleEdge(dangerDistance/2);
            }
            else
            {
                break;
            }
            Debug.LogWarning(hitColliders[0].name);
            
            unstuck++;
        }
        return randomPosition;
    }

    IEnumerator startAI()
    {
        yield return new WaitForSeconds(5);
        start = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        DrawCircle(this.transform.position, this.transform.forward, dangerDistance, 20);
    }

    public static void DrawCircle(Vector3 position, Vector3 dir, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = 360 / maxSteps;
        var angle = srcAngles - 360 / 2;

        var rad = Mathf.Deg2Rad * angle;
        var posB = initialPos;
        posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
        angle += stepAngles;
        posA = posB;

        for (var i = 1; i <= maxSteps; i++)
        {
            rad = Mathf.Deg2Rad * angle;
            posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));

            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
    }

    static float GetAnglesFromDir(Vector3 position, Vector3 dir)
    {
        var forwardLimitPos = position + dir;
        var srcAngles =
            Mathf.Rad2Deg
            * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

        return srcAngles;
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle * radius;
        return (new Vector3(vector2.x, 0, vector2.y) + this.transform.position);
    }
}
