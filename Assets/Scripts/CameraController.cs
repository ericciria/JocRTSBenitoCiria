using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CameraController : MonoBehaviour, IsSaveable
{

    [SerializeField] float moveSpeed = 15;
    [SerializeField] float zoomSpeed = 25;
    [SerializeField] float minZoomDist = 5;
    [SerializeField] float maxZoomDist = 50;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] GameObject unitPrefab;
    public int team;
    private Camera cam;
    public int monedes;
    public int fusta;
    public NavMeshAgent agent;
    private Button buttonmina;
    private GameObject buttonminaOcultar;
    private Button buttonFabrica;
    private GameObject buttonFabricaOcultar;
    private Button buttonCentral;
    private GameObject buttonCentralOcultar;
    private Button buttonMillora;
    private GameObject buttonMilloraOcultar;
    private Button buttonConstructor;
    private GameObject buttonConstructorOcultar;
    private Button buttonTank;
    private GameObject buttonTankOcultar;
    public Building building;

    public List<GameObject> buildings = new List<GameObject>();

    GameObject seleccio = null;
    GameObject playerUnit = null;

    private float timeSinceLastSpawn = Mathf.Infinity;
    float spawnRate = 1f;

    public RectTransform selectionBox;
    public LayerMask unitLayerMask;

    //coses per seleccionar unitats
    private List<Unit> units = new List<Unit>();
    public List<Unit> selectedUnits = new List<Unit>();

    private Vector2 startPos;
    private bool selection;
    private Text textMonedes;
    private Text textFusta;
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
        


        buttonminaOcultar = GameObject.Find("/Canvas/Mina");
        buttonFabricaOcultar = GameObject.Find("/Canvas/House");
        buttonCentralOcultar = GameObject.Find("/Canvas/Central");
        buttonMilloraOcultar = GameObject.Find("/Canvas/milloraHouse");
        buttonConstructorOcultar = GameObject.Find("/Canvas/SpawnConstructor");
        buttonTankOcultar = GameObject.Find("/Canvas/SpawnTank");

        buttonmina = buttonminaOcultar.GetComponent<Button>();
        buttonFabrica = buttonFabricaOcultar.GetComponent<Button>();
        buttonCentral = buttonCentralOcultar.GetComponent<Button>();
        buttonMillora = buttonMilloraOcultar.GetComponent<Button>();
        buttonConstructor = buttonConstructorOcultar.GetComponent<Button>();
        buttonTank = buttonTankOcultar.GetComponent<Button>();
    } 

    private void Start()
    {
        hideButtons();
        selection = false;
        textMonedes = GameObject.Find("/Canvas/monedes").GetComponent<Text>();
        textFusta = GameObject.Find("/Canvas/fusta").GetComponent<Text>();

        monedes = 1000;
        fusta = 500;
        
        team = 1;
    }

    void Update()
    {
        Move();
        Zoom();
        actualitzarRecursos();

        Ray ray = GetCameraRay();
        RaycastHit hit;

        //&& UnityEngine.EventSystems.EventSystem.current != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
        timeSinceLastSpawn += Time.deltaTime;
        if (Physics.Raycast(ray, out hit, 1000.0f) && UnityEngine.EventSystems.EventSystem.current != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
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
            //SpawnUnit();
        }

        if (Input.GetMouseButton(0) && selection)
        {
            updateSelectionBox(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            releaseSelectionBox();
            selection = false;
        }
    }

    private void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * zInput + transform.right * xInput;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (Input.GetMouseButton(2))
        {
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * 0, Input.GetAxis("Mouse X") * 1, 0));
            float mouseX = transform.rotation.eulerAngles.x;
            float mouseY = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(mouseX, mouseY, 0);
        }
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
        //Debug.Log(hit);
        if (Input.GetMouseButtonDown(0))
        {
            hideButtons();

            startPos = Input.mousePosition;
            selection = true;

            ///////////////////////// Prova per la selecci� d'unitats definitiva ////////////////////////
            foreach (Unit unit in units)
            {
                if (unit != null)
                {
                    seleccio = unit.transform.Find("Selection").gameObject;
                    seleccio.SetActive(false);
                }
            }
            selectedUnits = new List<Unit>();
            startPos = Input.mousePosition;
            selection = true;

            //Debug.Log(hit.collider.gameObject.tag);
            if (hit.collider.gameObject.tag.Equals("Unit") && hit.collider.gameObject.GetComponent<Unit>().team == 1)
            {
               if( hit.collider.gameObject.GetComponent<Unit>().constructor)
                {
                    buttonminaOcultar.SetActive(true);
                    buttonFabricaOcultar.SetActive(true);
                    buttonCentralOcultar.SetActive(true);
                }
                selectUnit(hit.collider.gameObject);

            }
            else if(hit.collider.gameObject.tag.Equals("Building") && hit.collider.gameObject.GetComponentInParent<Building>().team == 1)
            {
                building = hit.collider.gameObject.GetComponentInParent<Building>();
                /*if (building.buildingName.Equals("Mina"))
                {

                }*/
                buttonMilloraOcultar.SetActive(true);
                buttonConstructorOcultar.SetActive(true);
                buttonTankOcultar.SetActive(true);
            }
        }
        if (Input.GetMouseButton(0) && selection)
        {
            //Debug.Log("press");
            updateSelectionBox(Input.mousePosition);
        }
       
        //hit.collider.gameObject.SetActive(false);
        if ( selectedUnits.Count != 0)
        {
            if (IsPointValid(hit.point))
            {
                //if (hit.collider.gameObject.tag.Equals("Attackable") || hit.collider.gameObject.tag.Equals("NPC"))
                if (hit.collider.gameObject.tag.Equals("Unit"))
                {
                    if(hit.collider.gameObject.GetComponent<Unit>().team != team)
                    {
                        ChangeCursor(CursorTypes.ATTACK);
                    }
                    else
                    {
                        ChangeCursor(CursorTypes.SELECT);
                    }
                    
                }
                else if (hit.collider.gameObject.tag.Equals("Building"))
                {
                    if(hit.collider.gameObject.GetComponentInParent<Building>().team != team)
                    {
                        ChangeCursor(CursorTypes.ATTACK);
                    }
                    else
                    {
                        ChangeCursor(CursorTypes.SELECT);
                    }
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
                float asd = 0 - selectedUnits.Count * 1.95f;
                foreach (Unit unit in selectedUnits)
                {
                    asd += 2;
                    if (unit != null)
                    {
                        unit.setObjective(hit, asd);
                    }
                }
            }
        }
        else if (hit.collider.gameObject.tag.Equals("Unit") && hit.collider.gameObject.GetComponent<Unit>().team == team)
        {
            ChangeCursor(CursorTypes.SELECT);
        }
        else if (hit.collider.gameObject.tag.Equals("Building") && hit.collider.gameObject.GetComponentInParent<Building>().team == team)
        {
            ChangeCursor(CursorTypes.SELECT);
        }
        else
        {
            ChangeCursor(CursorTypes.DEFAULT);
        }
    }

    /*public AIPlayerunit SpawnUnit()
    {
        timeSinceLastSpawn = 0;
        GameObject unit =
            Instantiate(unitPrefab, spawnPoint1.position, Quaternion.identity) as GameObject;
        AIPlayerunit playerUnit = unit.GetComponentInChildren<AIPlayerunit>();
        playerUnit.agent.SetDestination(spawnPoint2.position);

        return playerUnit;
    }*/

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

        GameObject[] playerUnits2 = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in playerUnits2)
        {
            if (unit.GetComponent<Unit>() != null)
            {
                units.Add(unit.GetComponent<Unit>());

            }
        }

        foreach (Unit unit in units)
        {
            if (unit != null)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
                if (screenPos.x > minPosition.x && screenPos.x < maxPosition.x && screenPos.y > minPosition.y && screenPos.y < maxPosition.y)
                {
                    selectedUnits.Add(unit);
                    seleccio = unit.transform.Find("Selection").gameObject;
                    seleccio.SetActive(true);
                    if (unit.constructor)
                    {
                        hideButtons();
                        buttonminaOcultar.SetActive(true);
                        buttonFabricaOcultar.SetActive(true);
                        buttonCentralOcultar.SetActive(true);
                    }
                }
            }
        }
    }

    void actualitzarRecursos()
    {
        textMonedes.text = monedes.ToString();
        textFusta.text = fusta.ToString();

        /*if (monedes >= 100)
        {
            buttonmina.interactable = true;
            buttonFabrica.interactable = true;
        }
        else
        {
            buttonmina.interactable = false;
            buttonFabrica.interactable = false;
        }
        if (monedes >= 10)
        {
            buttonMilloraOcultar.GetComponent<Button>().interactable = true;
        }
        else
        {
            buttonMilloraOcultar.GetComponent<Button>().interactable = false;
        }*/
    }
    void selectUnit(GameObject unit)
    {
        playerUnit = unit;
        selectedUnits.Add(playerUnit.GetComponent<Unit>());
        seleccio = playerUnit.transform.Find("Selection").gameObject;
        seleccio.SetActive(true);
    }

    void hideButtons()
    {
        buttonminaOcultar.SetActive(false);
        buttonFabricaOcultar.SetActive(false);
        buttonCentralOcultar.SetActive(false);
        buttonMilloraOcultar.SetActive(false);
        buttonConstructorOcultar.SetActive(false);
        buttonTankOcultar.SetActive(false);
    }

    public object CaptureState()
    {
        throw new System.NotImplementedException();
    }

    public void RestoreState(object data)
    {
        throw new System.NotImplementedException();
    }
}
