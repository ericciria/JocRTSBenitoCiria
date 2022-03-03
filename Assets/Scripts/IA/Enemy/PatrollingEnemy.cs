using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform target;
    public float pursueDistance = 20.0f;
    public float atackDistance = 2.0f;
    public float atackCooldown = 10.0f;
    public NavMeshAgent agent;

    public IState currentState = new IdleState();

    public Transform[] points;
    public Transform currentPoint;
    public int currentPointInt;
    float distanceToPoint;

    public GameObject nearestEnemy;
    private bool cheching;

    public int attack = 1;

    Vector3 previousCorner;
    Vector3 currentCorner;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        nearestEnemy = null;
        currentPointInt = 0;
        currentPoint = points[currentPointInt];
        distanceToPoint = Vector3.Distance(currentPoint.position, transform.position);
        cheching = false;
    }

    // Update is called once per frame
    void Update()
    {
        IState nextState = currentState.OnUpdate(this);

        if (nextState != null)
        {
            currentState.OnExitState(this);
            currentState = nextState;
            currentState.OnEnterState(this);
        }

        if (!cheching)
        {
            cheching = true;
            StartCoroutine(checkClosestEnemy());
        }

        distanceToPoint = Vector3.Distance(currentPoint.position, transform.position);
        if (distanceToPoint < 1f)
        {
            currentPointInt += 1;
            if (currentPointInt > 7)
            {
                currentPointInt = 0;
            }
            currentPoint = points[currentPointInt];
        }
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
        target = nearestEnemy.transform;
        cheching = false;
    }

    void OnDrawGizmos()
    {
        if (currentState.ToString().Equals("IdleState"))
        {
            Gizmos.color = Color.blue;
        }
        else if (currentState.ToString().Equals("PursueState"))
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        DrawWireArc(this.transform.position, this.transform.forward, 10, atackDistance); // width,length

        //Cercle de detecci�
        DrawCircle(this.transform.position, this.transform.forward, pursueDistance, 40);
        DrawPath(agent.path);

    }

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
