using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 15;
    [SerializeField] float zoomSpeed = 25;
    [SerializeField] float minZoomDist = 10;
    [SerializeField] float maxZoomDist = 50;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] GameObject unitPrefab;
    private Camera cam;
    public NavMeshAgent agent;

    GameObject seleccio = null;
    GameObject playerUnit = null;

    private float timeSinceLastSpawn = Mathf.Infinity;
    float spawnRate = 1f;

    public RectTransform selectionBox;
    public LayerMask unitLayerMask;

    //coses per seleccionar unitats
    private List<AIPlayerunit> units = new List<AIPlayerunit>();
    private List<AIPlayerunit> selectedUnits = new List<AIPlayerunit>();
    private Vector2 startPos;

    [System.Serializable]
    struct CursorMapping
    {
        public CursorTypes type;
        public Texture2D texture;
    }

    [SerializeField] CursorMapping[] cursorMappings;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Move();
        Zoom();

        Ray ray = GetCameraRay();
        RaycastHit hit;

        timeSinceLastSpawn += Time.deltaTime;
        if (Physics.Raycast(ray, out hit, 1000.0f) && UnityEngine.EventSystems.EventSystem.current != null &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            checkCameraRay(hit);
        }
        else
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                ChangeCursor(CursorTypes.DEFAULT);
            }
            else
            {
                ChangeCursor(CursorTypes.INVALID);
            }
            
        }
        if(timeSinceLastSpawn > spawnRate && Input.GetKey(KeyCode.E))
        {
            SpawnUnit();
        }
    }

    private void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * zInput + transform.right * xInput;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    private void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, cam.transform.position);

        if (dist < minZoomDist && scrollInput > 0.0f)
        {
            return;
        }
            
        else if (dist > maxZoomDist && scrollInput < 0.0f)
        {
            return;
        }
            
        cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
    }

    private Ray GetCameraRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private bool IsPointValid(Vector3 point)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(point, out hit, 3.0f, NavMesh.AllAreas);
    }

    private void ChangeCursor(CursorTypes type)
    {
        foreach (CursorMapping mapping in cursorMappings)
        {
            if (type == mapping.type)
            {
                Cursor.SetCursor(mapping.texture, new Vector2(0.0f, 0.0f), CursorMode.Auto);
            }
        }
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

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            pathLenght += Vector3.Distance(path.corners[i], path.corners[1 - 1]);
        }

        return pathLenght;
    }

    private void checkCameraRay(RaycastHit  hit)
    {
        Debug.Log(hit);
        if (Input.GetMouseButtonDown(0))
        {
            foreach (AIPlayerunit unit in units)
            {
                if (unit != null)
                {
                    seleccio = unit.transform.Find("Selection").gameObject;
                    seleccio.SetActive(false);
                }
            }
            selectedUnits = new List<AIPlayerunit>();
            startPos = Input.mousePosition;
            Debug.Log(hit.collider.gameObject.tag); ;
            if (hit.collider.gameObject.tag.Equals("PlayerUnit"))
            {
                playerUnit = hit.collider.gameObject;
                selectedUnits.Add(playerUnit.GetComponent<AIPlayerunit>());
                seleccio = playerUnit.transform.Find("Selection").gameObject;
                seleccio.SetActive(true);
            }
        }
        if (Input.GetMouseButton(0))
        {
            updateSelectionBox(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            releaseSelectionBox();
        }
        //hit.collider.gameObject.SetActive(false);
        if (selectedUnits.Count!=0)
        {
            if (IsPointValid(hit.point))
            {
                if (hit.collider.gameObject.tag.Equals("Attackable") || hit.collider.gameObject.tag.Equals("NPC"))
                {
                    ChangeCursor(CursorTypes.ATTACK);
                }
                else if (hit.collider.gameObject.tag.Equals("PlayerUnit"))
                {
                    ChangeCursor(CursorTypes.SELECT);
                }
                else
                {
                    ChangeCursor(CursorTypes.MOVEMENT);
                }
            }
            else
            {
                ChangeCursor(CursorTypes.INVALID_MOVEMENT);
            }
            if (Input.GetMouseButtonDown(1))
            {
                float asd = 0 - selectedUnits.Count*1.95f;
                foreach(AIPlayerunit unit in selectedUnits)
                {
                    asd += 2;
                    if (unit != null)
                    {
                        unit.setObjective(hit,asd);
                    }
                }
            }
        }
        else if (hit.collider.gameObject.tag.Equals("PlayerUnit"))
        {
            ChangeCursor(CursorTypes.SELECT);
        }
        else
        {
            ChangeCursor(CursorTypes.DEFAULT);
        }
        

    }

    public AIPlayerunit SpawnUnit()
    {
        timeSinceLastSpawn = 0;
        GameObject unit =
            Instantiate(unitPrefab, spawnPoint1.position, Quaternion.identity) as GameObject;
        AIPlayerunit playerUnit = unit.GetComponentInChildren<AIPlayerunit>();
        playerUnit.agent.SetDestination(spawnPoint2.position);

        //enemy.GetComponent<ActorController>().level = level;
        //enemy.GetComponent<HealthComponent>().value = hp;

        return playerUnit;
    }

    void updateSelectionBox(Vector2 cursor)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }
        float width = cursor.x - startPos.x;
        float height = cursor.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }
    void releaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);
        Vector2 minPosition = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 maxPosition = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        foreach (GameObject unit in playerUnits)
        {
            if (unit.GetComponent<AIPlayerunit>() != null)
            {
                units.Add(unit.GetComponent<AIPlayerunit>());
            }
        }
        
        foreach (AIPlayerunit unit in units)
        {
            if (unit != null)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
                if (screenPos.x > minPosition.x && screenPos.x < maxPosition.x && screenPos.y > minPosition.y && screenPos.y < maxPosition.y)
                {
                    selectedUnits.Add(unit);
                    seleccio = unit.transform.Find("Selection").gameObject;
                    seleccio.SetActive(true);
                }
            }
        }
        //Debug.LogWarning(selectedUnits.Count);
    }
}
