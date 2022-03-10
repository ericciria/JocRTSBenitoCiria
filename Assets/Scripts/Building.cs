using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool constructing;
    public bool constructed;

    private int life;
    [SerializeField] int maxLife;
    [SerializeField] ObjectLife health;
    [SerializeField] MeshRenderer renderer;
    //[SerializeField] float duration = 2f;
    
    private float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        constructing = true;
        constructed = false;
        life = 1;
        renderer.material.SetColor("_Color", new Color(0.5f, 0.8f, 0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!constructed)
        {
            
            if (constructing)
            {
                Construct();
            }
        }
    }

    void Construct()
    {
        if (life<maxLife)
        {
            life = health.getHealth();
            life += 1;
            health.setHealth(life);
            t = (float)life / maxLife;
            renderer.material.color = Color.Lerp(new Color(0.5f, 0.8f, 0.5f, 0.5f), new Color(1f, 1f, 1f, 1f), t);
        }
        else
        {
            constructed = true;
        }
    }
}
