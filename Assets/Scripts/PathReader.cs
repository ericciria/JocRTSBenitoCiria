using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathReader : MonoBehaviour
{
    private NavMeshAgent agent;
    private Color c = Color.blue;

    Vector3 previousCorner;
    Vector3 currentCorner;
    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        DrawPath(agent.path);
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
