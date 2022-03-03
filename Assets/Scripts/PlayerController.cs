using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform targetTransform;
    NavMeshAgent agent;

    [System.Serializable]
    struct CursorMapping
    {
        public CursorTypes type;
        public Texture2D texture;
    }

    [SerializeField] CursorMapping[] cursorMappings;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = GetCameraRay();
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            if (IsPointValid(hit.point))
            {
                ChangeCursor(CursorTypes.MOVEMENT);
                if (Input.GetMouseButtonDown(1))
                {
                    agent.SetDestination(hit.point);
                    Debug.Log("Path: " + GetPathLenght(transform.position, hit.point));
                }
            }
            else
            {
                ChangeCursor(CursorTypes.INVALID_MOVEMENT);
            }
        }
        else
        {
            ChangeCursor(CursorTypes.INVALID);
        }
    }

    private Ray GetCameraRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private float GetPathLenght(Vector3 origin, Vector3 destination)
    {
        float pathLenght = 0.0f;

        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(origin, destination, NavMesh.AllAreas, path);

        if (path.corners.Length < 2)
        {
            return pathLenght;
        }

        for(int i = 0; i <  path.corners.Length -1; i++)
        {
            pathLenght += Vector3.Distance(path.corners[i], path.corners[1 - 1]);
        }

        return pathLenght;
    }

    private bool IsPointValid(Vector3 point)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(point, out hit, 3.0f, NavMesh.AllAreas);

    }

    private void ChangeCursor(CursorTypes type)
    {
        foreach(CursorMapping mapping in cursorMappings)
        {
            if(type == mapping.type)
            {
                Cursor.SetCursor(mapping.texture, new Vector2(0.0f, 0.0f), CursorMode.Auto);
            }
        }
    }
}
