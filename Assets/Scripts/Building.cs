using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public bool constructing;
    public bool constructed;
    public Material opaqueMat;

    private float life;
    [SerializeField] int maxLife;
    [SerializeField] ObjectLife health;
    [SerializeField] MeshRenderer renderer;
    [SerializeField] float duration = 2f;
    [SerializeField] CameraController player;

    private bool minant;
    private float t = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraController>();
        player.monedes -= 100;
        constructing = true;
        constructed = false;
        life = 1;
        renderer.material.SetColor("_Color", new Color(0.5f, 0.8f, 0.5f, 0.1f));
        minant = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (constructed && !minant)
        {
            StartCoroutine(sumarMonedes());
        }
    }

    private void FixedUpdate()
    {
        if (!constructed)
        {
            if (constructing)
            {
                //Construct();
            }
        }
    }

    public void Construct()
    {
        if (life<maxLife)
        {
            
            Debug.Log("Life: " + life);
            Debug.Log(renderer.material.color);
            life = health.getHealth();
            life += 1/duration;

            health.setHealth(life);
            t = life / maxLife;
            renderer.material.color = Color.Lerp(new Color(0.5f, 0.8f, 0.5f, 0.1f), new Color(1f, 0.5f, 0.5f, 1f), t);
        }
        else
        {
            Debug.Log("Hola");
            constructed = true;
           
        renderer.material = opaqueMat;

        }
      
    }
    IEnumerator sumarMonedes()
    {
        minant = true;
        yield return new WaitForSeconds(2);
        player.monedes += 10;
        Debug.Log(player.monedes);
        minant = false;
    }
}
