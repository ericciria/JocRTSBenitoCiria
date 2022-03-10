using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blue_script : MonoBehaviour
{

    RaycastHit hit;
    Vector3 movePoint;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 10)))
        {
            transform.position = hit.point;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 10)))
        {
            transform.position = hit.point;
        }
        if (Input.GetMouseButton(0))
        {
          Instantiate(prefab, transform.position, transform.rotation);    
          Destroy(gameObject);
        }
    }
}

  
        
