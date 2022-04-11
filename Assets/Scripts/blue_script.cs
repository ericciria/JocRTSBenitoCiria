using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blue_script : MonoBehaviour
{

    RaycastHit hit;
    public GameObject prefab;
    private GameObject building;
    bool canConstruct;
    bool canConstruct2;
    bool canConstruct3;
    Vector3 position;
    [SerializeField] MeshRenderer renderer;
    Renderer rend; 
    private float largestSide;
    CameraController player;
    private List<Unit> playerUnits;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("/Camera").GetComponent<CameraController>();
        playerUnits = player.selectedUnits;

        rend = GetComponentInChildren<Renderer>();
        if (rend.bounds.size.x > rend.bounds.size.z)
        {
            largestSide = rend.bounds.size.x;
        }
        else
        {
            largestSide = rend.bounds.size.z;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 10)))
        {
            transform.position = hit.point;
        }
        canConstruct = true;
        canConstruct2 = true;
        canConstruct3 = true;
        renderer.material.SetColor("_Color", new Color(0.5f, 0.8f, 0.5f, 0.5f));
    }

    void Update()
    {
        GetCollidersInRadius();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 10)))
        {
            if (hit.transform.gameObject.tag.Equals("Terrain"))
            {
                transform.position = hit.point;

            }
            else
            {
                transform.position = hit.transform.position;
            }
            
        }

        if (Input.GetMouseButton(0) && canConstruct && UnityEngine.EventSystems.EventSystem.current != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {

            building = Instantiate(prefab, position, transform.rotation);
            foreach (Unit unit in playerUnits)
            {
                if (unit.constructor)
                {
                    unit.target = building.GetComponentInChildren<ObjectLife>().gameObject.transform;
                    break;
                }
            }
            Destroy(gameObject);

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    private void GetCollidersInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, largestSide*0.6f);

        canConstruct2 = false;
        canConstruct3 = true;
        if (prefab.GetComponent<Building>().name.Equals("Mine"))
        {
            foreach(Collider col in hitColliders)
            {
                if (col.gameObject.tag.Equals("Minerals"))
                {
                    canConstruct2 = true;
                    position = col.transform.position;
                    //Debug.Log("Minerals");
                }
                else if (col.gameObject.tag.Equals("Building"))
                {
                    canConstruct3 = false;
                }
            }
            if (!canConstruct2 || !canConstruct3)
            {
                canConstruct = false;
                renderer.material.SetColor("_Color", new Color(0.8f, 0.5f, 0.5f, 0.5f));
            }
            else
            {
                canConstruct = true;
                renderer.material.SetColor("_Color", new Color(0.5f, 0.8f, 0.5f, 0.5f));
            }
        }
        else
        {
            if (hitColliders.Length > 2)
            {
                canConstruct = false;
                renderer.material.SetColor("_Color", new Color(0.8f, 0.5f, 0.5f, 0.5f));
            }
            else
            {
                position = transform.position;
                canConstruct = true;
                renderer.material.SetColor("_Color", new Color(0.5f, 0.8f, 0.5f, 0.5f));
            }
        }
    }
}

  
        
