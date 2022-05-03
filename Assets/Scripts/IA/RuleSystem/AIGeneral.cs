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
    private List<Unit> enemies;
    private List<Unit> constructors;
    private List<Building> buildings;
    private Unit enemy;

    public GameObject nearestEnemy;

    [SerializeField] float evaluationRate = 1f;
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
        actions.Add(Action1);
        actions.Add(Action2);
        actions.Add(Action3);
        actions.Add(Action4);
        actions.Add(Action5);
        actions.Add(Action6);
    }

    void Start()
    {
        enemies = new List<Unit>();
        constructors = new List<Unit>();
        buildings = new List<Building>();
        start = false;
        unit1 = SpawnEnemy(enemyPrefabs[0], this.transform.position, points[1].position);
        unit2 = SpawnEnemy(enemyPrefabs[0], this.transform.position, points[2].position);
        unit3 = SpawnEnemy(enemyPrefabs[0], this.transform.position, points[3].position);
        unit4 = SpawnEnemy(enemyPrefabs[0], this.transform.position, points[4].position);

        metall = 500;
        monedes = 3000;

        StartCoroutine(startAI());
    }

    void Update()
    {
        timeSinceLastEvaluation += Time.deltaTime;
        if (timeSinceLastEvaluation > evaluationRate && start)
        {
            Evaluate();
            StartCoroutine(checkClosestEnemy());
            timeSinceLastEvaluation = 0;
        }
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > spawnRate && Input.GetKey(KeyCode.L))
        {
            start = true;
            //enemy = SpawnEnemy();
            //enemies.Add(enemy);
            //Debug.Log(enemies.Count);
            //timeSinceLastSpawn = 0;
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
                break;
            }
        }
    }

    private bool Condition1()
    {
        return constructors.Count<2 && monedes>200 && metall>50;
    }
    private void Action1()
    {
        unit1 = SpawnEnemy(enemyPrefabs[1], this.transform.position, this.transform.position);
        constructors.Add(unit1);
        monedes -= 200;
        metall -= 50;
    }
    private bool Condition2()
    {
        int mina = 0;
        foreach(Building building in buildings)
        {
            if (building.name.Equals("Mine"))
            {
                mina ++;
            }
        }
        return mina<2 && monedes > 500;
    }
    private void Action2()
    {
        Building building = null;
        Vector3 minePosition = checkClosestUnoccupiedMineral();
        //Building mina = SpawnBuilding(buildingsPrefabs[1], minePosition);
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                building = SpawnBuilding(buildingsPrefabs[1], minePosition).GetComponent<Building>();
                monedes -= 500;
                constructor.target = building.transform;
                break;
            }
        }
        buildings.Add(building);
    }
    private bool Condition3()
    {
        bool petrolPump = false;
        bool canConstruct = false;
        int petrolPumps = 0;
        foreach (Building building in buildings)
        {
            if (building.name.Equals("Mine"))
            {
                petrolPump = true;
                petrolPumps++;
                break;
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
        return petrolPumps < 5 && monedes > 400 && metall > 100 && canConstruct;
    }
    private void Action3()
    {
        Building building = null;
        Vector3 position = checkIfCanBuild();
        bool canConstruct = false;
        //Building mina = SpawnBuilding(buildingsPrefabs[1], minePosition);
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                building = SpawnBuilding(buildingsPrefabs[0], position).GetComponent<Building>();
                monedes -= 500;
                constructor.target = building.transform;
                buildings.Add(building);
                break;
            }
        }  
    }
    private bool Condition4()
    {
        return constructors.Count < 3 && monedes > 500 && metall > 50;
    }
    private void Action4()
    {
        unit1 = SpawnEnemy(enemyPrefabs[1], this.transform.position, this.transform.position);
        constructors.Add(unit1);
        monedes -= 500;
        metall -= 50;
    }
    private bool Condition5()
    {
        int mina = 0;
        foreach (Building building in buildings)
        {
            if (building.name.Equals("PetrolPump"))
            {
                mina++;
            }
        }
        return mina<2 && monedes > 500;
    }
    private void Action5()
    {
        Building building = null;
        Vector3 minePosition = checkClosestUnoccupiedMineral();
        //Building mina = SpawnBuilding(buildingsPrefabs[1], minePosition);
        foreach (Unit constructor in constructors)
        {
            if (constructor.currentState.ToString().Equals("UnitIdleState"))
            {
                building = SpawnBuilding(buildingsPrefabs[1], minePosition).GetComponent<Building>();
                monedes -= 500;
                constructor.target = building.transform;
                buildings.Add(building);
                break;
            }
        }
        
    }
    private bool Condition6()
    {
        return enemies.Count < 4 && monedes > 200 && metall > 300;
    }
    private void Action6()
    {
        Unit unit = SpawnEnemy(enemyPrefabs[0], this.transform.position, points[1].position);
        enemies.Add(unit);
        monedes -= 200;
        metall -= 300;
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
        
        enemyUnit.team = 2;
        enemyUnit.agent.SetDestination(destination);

        return enemyUnit;
    }
    public Building SpawnBuilding(GameObject prefab, Vector3 point)
    {
        timeSinceLastSpawn = 0;
        GameObject enemy =
            Instantiate(prefab, point, Quaternion.identity) as GameObject;
        Building enemyBuilding = enemy.GetComponent<Building>();
        enemyBuilding.team = 2;

        return enemyBuilding;
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
        Vector3 randomPosition = RandomPointOnCircleEdge(dangerDistance);
        while (unstuck<20 || canbuild)
        {
            Collider[] hitColliders = Physics.OverlapSphere(randomPosition, 3);
            if (hitColliders.Length > 1)
            {
                Debug.LogWarning(hitColliders.Length);
                randomPosition = RandomPointOnCircleEdge(dangerDistance);
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

    IEnumerator checkClosestEnemy()
    {
        yield return new WaitForSeconds(2);
        //nearestEnemy = FindClosestEnemy();
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
