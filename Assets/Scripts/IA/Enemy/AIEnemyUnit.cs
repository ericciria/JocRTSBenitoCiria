using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemyUnit : MonoBehaviour
{

    public NavMeshAgent agent;
    public int attack = 1;
    public Transform target;

    public IState currentState = new IdleState();

    public float movementspeed = 10f;
    public float attackDistance = 10.0f;
    public float attackCooldown = 4.0f;
    public float timeSinceLastAttack = Mathf.Infinity;
    public GameObject nearestEnemy;

    private bool checking;

    Vector3 previousCorner;
    Vector3 currentCorner;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        checking = false;
    }


    void Update()
    {
        IState nextState = currentState.OnUpdate(this);
        timeSinceLastAttack += Time.deltaTime;

        if (nextState != null)
        {
            currentState.OnExitState(this);
            currentState = nextState;
            currentState.OnEnterState(this);
        }
        if (!checking && currentState.ToString().Equals("IdleState"))
        {
            checking = true;
            StartCoroutine(checkClosestEnemy());
        }

        //Debug.Log(target);
    }
    
    void OnDrawGizmos()
    {
        if (currentState.ToString().Equals("IdleState"))
        {
            Gizmos.color = Color.blue;
            //Debug.Log("AAAAAAAAAAAAAAA");
        }
        else if (currentState.ToString().Equals("PursueState"))
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        DrawWireArc(this.transform.position, this.transform.forward, 10, attackDistance); // width,length
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        DrawCircle(this.transform.position, this.transform.forward, attackDistance, 20);
        DrawPath(agent.path);

    }

    // Aixo ho he buscat, ja que nomes se fer lo basic, linies, esferes i el que es fa de base
    public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = anglesRange / maxSteps;
        var angle = srcAngles - anglesRange / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));

            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, initialPos);
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
        var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

        return srcAngles;
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
        checking = false;
    }

    void DrawPath(NavMeshPath path)
    {
        //yield return new WaitForEndOfFrame();
        path = agent.path;
        if (path.corners.Length < 2)
        {
            return;
        }

        previousCorner = path.corners[0];

        int i = 1;
        while (i < path.corners.Length)
        {
            currentCorner = path.corners[i];
            Gizmos.DrawLine(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }

    }
}
