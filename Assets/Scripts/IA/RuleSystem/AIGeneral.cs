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
        actions.Add(Action1);
        actions.Add(Action2);
        actions.Add(Action3);
        actions.Add(Action4);
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
        monedes = 2000;

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
            //checkClosestUnoccupiedMineral();
            foreach (Unit constructor in constructors)
            {
                Debug.LogWarning(constructor.currentState);

            }
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
            Debug.Log(enemies);
            if (conditions[i]())
            {
                actions[i]();
                break;
            }
        }
    }

    private bool Condition1()
    {
        return constructors.Count<1 && monedes>500 && metall>50;
    }
    private void Action1()
    {
        unit1 = SpawnEnemy(enemyPrefabs[1], this.transform.position, points[1].position);
        constructors.Add(unit1);
        monedes -= 500;
        metall -= 50;
    }
    private bool Condition2()
    {
        bool mina = false;
        foreach(Building building in buildings)
        {
            if (building.name.Equals("Mine"))
            {
                mina = true;
                break;
            }
        }
        return !mina;
    }
    private void Action2()
    {
        Vector3 minePosition = checkClosestUnoccupiedMineral();
        //Building mina = SpawnBuilding(buildingsPrefabs[1], minePosition);
        foreach (Unit constructor in constructors)
        {
            /*if (constructor.currentState.Equals()
            {
                mina = true;
                break;
            }*/
        }
        //buildings.Add();
    }
    private bool Condition3()
    {
        return unit3 == null;
    }
    private void Action3()
    {
        unit3 = SpawnEnemy(enemyPrefabs[0], this.transform.position, points[1].position);
    }
    private bool Condition4()
    {
        return unit4 == null;
    }
    private void Action4()
    {
        unit4 = SpawnEnemy(enemyPrefabs[0], this.transform.position, points[1].position);
    }

    // Llista funcions-condicio i llista funcions-accio
    /*private bool Condition1()
    {
        float distanceToTarget = Vector3.Distance(
            nearestEnemy.transform.position,
            transform.position
        );
        Debug.Log("Condition1");
        return distanceToTarget < dangerDistance;
    }

    private bool Condition2()
    {
        Debug.Log("Condition2");
        //return Random.Range(0f, 1f) >= 0.5;     //50% - 1/2
        return enemies.Count < 5 && timeSinceLastSpawn > spawnRate;
    }

    private bool Condition3()
    {
        Debug.Log("Condition3");
        //return Random.Range(0f, 1f) >= 0.66;    // 33% - 1/3
        return enemies.Count >= 5 && nearestEnemy != null ;
    }

    private bool Condition4()
    {
        Debug.Log("Condition4");
        return nearestEnemy == null;
        //return Random.Range(0f, 1f) >= 0.75;    // 25% - 1/4
    }

    //Si hi ha una unitat del jugador en la base l'ataca amb el que te
    private void Action1()
    {
        if (timeSinceLastSpawn > spawnRate)
        {
            enemy = SpawnEnemy();
            enemies.Add(enemy);
            timeSinceLastSpawn = 0;
        }
        foreach (Unit enemy in enemies)
        {
            if (nearestEnemy != null)
            {
                enemy.target = nearestEnemy.transform;
            }
        }
        Debug.Log("Action1");
    }

    // Si te menys de 5 unitats en crea un altra
    private void Action2()
    {
        enemy = SpawnEnemy();
        enemies.Add(enemy);
        Debug.Log("Action2");
    }

    // si te 5 o mes unitats ataca a la unitat del jugador mes propera i neteja la 
    //  llista de nulls
    private void Action3()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies[i] = enemies[enemies.Count - 1];
                enemies.RemoveAt(enemies.Count - 1);
            }
            else
            {
                if (nearestEnemy != null)
                {
                    enemies[i].target = nearestEnemy.transform;
                    Debug.Log("Action3AA");
                }
            }
        }
        Debug.Log("Action3");
    }

    // quan no te res mes a fer neteja la llista d'unitats i elimina
    // les unitats que s'han destruit i a la llista son nulls
    private void Action4()
    {
        Debug.Log("Action4");

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null)
            {
                enemies[i].agent.SetDestination(spawnPoint.position);
            }
            else
            {
                enemies[i] = enemies[enemies.Count - 1];
                enemies.RemoveAt(enemies.Count - 1);
            }
        }
    }*/

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

    /*public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("PlayerUnit");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }*/

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
}
