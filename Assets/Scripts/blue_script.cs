using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blue_script : MonoBehaviour
{

    RaycastHit hit;
    Vector3 movePoint;
    public GameObject prefab;
    Collider col;
    int canConstruct;
    [SerializeField] MeshRenderer renderer;
    

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 10)))
        {
            transform.position = hit.point;
        }
        canConstruct = 0;
        renderer.material.SetColor("_Color", new Color(0.5f, 0.8f, 0.5f, 0.5f));
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 10)))
        {
            transform.position = hit.point;
        }
        if (Input.GetMouseButton(0) && canConstruct == 0 && UnityEngine.EventSystems.EventSystem.current != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Instantiate(prefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag != "Terrain")
        {
            //Debug.Log("Enter: ", other);
            canConstruct +=1;
            renderer.material.SetColor("_Color", new Color(0.8f, 0.5f, 0.5f, 0.5f));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Terrain")
        {
            //Debug.Log("Exit: ", other);
            canConstruct -= 1;
            if (canConstruct == 0)
            {
                renderer.material.SetColor("_Color", new Color(0.5f, 0.8f, 0.5f, 0.5f));
            }
        }
    }
}

  
        
