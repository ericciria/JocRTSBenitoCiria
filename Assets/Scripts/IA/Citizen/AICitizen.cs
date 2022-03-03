using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICitizen : MonoBehaviour
{

    public float detectionDistance = 40.0f;
    public Vector3 startingPoint;
    public Transform coverPoint;
    public GameObject nearestEnemy;
    public GameObject nearestPlayer;
    public NavMeshAgent agent;
    private bool cheching;
    private bool covering;

    Vector3 previousCorner;
    Vector3 currentCorner;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPoint = this.transform.position;
        cheching = false;
        covering = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cheching)
        {
            cheching = true;
            if (!covering)
            {
                StartCoroutine(checkFight());
            }
            else
            {
                StartCoroutine(returnToStartingPoint());
            }
        }
    }

    IEnumerator checkFight()
    {
        //Debug.Log("Checking Fight");
        yield return new WaitForSeconds(1);
        if (!covering)
        {
            checkRadius(transform.position, detectionDistance);
        }
        cheching = false;
    }

    IEnumerator returnToStartingPoint()
    {
        yield return new WaitForSeconds(10);
        covering = false;
        cheching = false;
        agent.SetDestination(startingPoint);
        
    }

    void OnDrawGizmos()
    {
        if (covering)
        {
            Gizmos.color = Color.black;
        }
        else
        {
            Gizmos.color = Color.white;
        }
        //Cercle de detecció
        DrawCircle(this.transform.position, this.transform.forward, detectionDistance, 40);
        Gizmos.DrawLine(transform.position, transform.position + this.transform.forward * 2);
        DrawPath(agent.path);

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

    void checkRadius(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var collider in hitColliders)
        {
            checkState(collider.gameObject);
        }
    }
    void checkState(GameObject unit)
    {
        if (unit != null)
        {
            if(unit.tag.Equals("PlayerUnit"))
        {
                if (unit.GetComponent<AIPlayerunit>().currentState.ToString().Equals("PlayerAttackState"))
                {
                    Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    agent.SetDestination(coverPoint.position);
                    covering = true;
                }
            }
        else if (unit.tag.Equals("Attackable"))
            {
                if (unit.GetComponent<PatrollingEnemy>().currentState.ToString().Equals("AttackState"))
                {
                    Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    agent.SetDestination(coverPoint.position);
                    covering = true;
                }
            }
        }
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
