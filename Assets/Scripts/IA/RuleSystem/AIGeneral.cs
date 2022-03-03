using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGeneral : MonoBehaviour
{
    private delegate bool Condition();
    private delegate void Action();

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] Transform spawnPoint;
    private List<AIEnemyUnit> enemies;
    private AIEnemyUnit enemy;

    public GameObject nearestEnemy;

    [SerializeField] float evaluationRate = 1f;
    private float timeSinceLastEvaluation = Mathf.Infinity;

    [SerializeField] float spawnRate = 10f;
    private float timeSinceLastSpawn = Mathf.Infinity;
    private bool start;
    private float dangerDistance = 50f;

    List<Condition> conditions = new List<Condition>();
    List<Action> actions = new List<Action>();

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
        enemies = new List<AIEnemyUnit>();
        start = false;
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
    }

    private void Evaluate()
    {
        Debug.Assert(conditions.Count == actions.Count); // Assert: Si no se cumple, el codigo peta y te indica donde
        if (nearestEnemy != null)
        {
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
    }

    // Llista funcions-condicio i llista funcions-accio
    private bool Condition1()
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
        foreach (AIEnemyUnit enemy in enemies)
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
    }

    //(int level, int hp)
    public AIEnemyUnit SpawnEnemy()
    {
        timeSinceLastSpawn = 0;
        GameObject enemy =
            Instantiate(enemyPrefab, transform.position, Quaternion.identity) as GameObject;
        AIEnemyUnit enemyUnit = enemy.GetComponentInChildren<AIEnemyUnit>();
        enemyUnit.agent.SetDestination(spawnPoint.position);

        //enemy.GetComponent<ActorController>().level = level;
        //enemy.GetComponent<HealthComponent>().value = hp;

        return enemyUnit;
    }

    public GameObject FindClosestEnemy()
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
    }

    IEnumerator checkClosestEnemy()
    {
        yield return new WaitForSeconds(2);
        nearestEnemy = FindClosestEnemy();
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
