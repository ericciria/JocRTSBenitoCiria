using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public bool constructing;
    public bool constructed;
    public Material opaqueMat;
    public int team = 1;
    public int moneyCost;
    public int metalCost;
    public int energy;
    private int attackDamage;

    private float life;
    [SerializeField] int maxLife;
    [SerializeField] ObjectLife health;
    [SerializeField] MeshRenderer renderer;
    [SerializeField] float duration = 2f;
    [SerializeField] CameraController player;
    [SerializeField] BuildingData data;

    private bool minant;
    private float t = 0;

    
   

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraController>();
        player.monedes -= 100;
        constructing = false;
        constructed = false;
        life = 1;
        renderer.material.SetColor("_Color", new Color(0.5f, 0.8f, 0.5f, 0.3f));
        minant = false;

        moneyCost = data.MoneyCost;
        metalCost = data.MetalCost;
        energy = data.EnergyCost;
        maxLife = data.MaxHealth;
        attackDamage = data.AttackDamage;
        health.setMaxHealth(maxLife);
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
        if (!constructing)
        {
            if (life < maxLife)
            {
                StartCoroutine(constructTimer());
                Debug.Log("Life: " + life);
                
            }
            else
            {
                constructed = true;
                renderer.material = opaqueMat;

            }
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
    IEnumerator constructTimer()
    {
        constructing = true;

        life = health.getHealth();
        yield return new WaitForSeconds(0.4f);

        life += maxLife/ 20 / duration;
        health.setHealth(life);

        t = life / maxLife;
        renderer.material.color = Color.Lerp(new Color(0.5f, 0.8f, 0.5f, 0.3f), new Color(1f, 0.5f, 0.5f, 1f), t);

        constructing = false;
    }
}
