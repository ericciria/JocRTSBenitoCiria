using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UniqueIdentifier))]

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
    public string name;

    private float life;
    [SerializeField] int maxLife;
    [SerializeField] ObjectLife health;
    [SerializeField] MeshRenderer renderer;
    [SerializeField] float duration = 2f;
    [SerializeField] CameraController player;
    public BuildingData data;
    public Material[] materials;
    private Color teamColor;

    private bool minant;
    private float t = 0;

    public string id;


    private void Awake()
    {
        materials = renderer.materials;
        id = GetComponent<UniqueIdentifier>().id;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraController>();
        //player.monedes -= 100;
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
        name = data.BuildingName;
        health.setMaxHealth(maxLife);

        foreach (Material material in materials)
        {
            material.color = new Color(0.5f, 0.5f, 0.8f, 0.3f);
        }
        if (team == 1)
        {
            teamColor = new Color(0.1F, 0.1F, 0.7F, 1F);
        }
        else if (team == 2)
        {
            teamColor = new Color(0.7F, 0.1F, 0.1F, 1F);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (constructed && !minant && data.BuildingName.Equals("PetrolPump"))
        {
            StartCoroutine(sumarFusta());
        }
        if (constructed && !minant && data.BuildingName.Equals("Mine"))
        {
            StartCoroutine(sumarMonedes());
        }
        if (constructed && !minant && data.BuildingName.Equals("EnergyPlant"))
        {
            minant = true;
            player.electricitat += energy;
        }

    }

    public void Construct()
    {
        if (!constructing)
        {
            if (life < maxLife)
            {
                StartCoroutine(constructTimer());
                //Debug.Log("Life: " + life);
                
            }
            else
            {
                constructed = true;
                renderer.material = opaqueMat;
                adjustMaterials();
                destroyScafolding();

            }
        }
    }

    private void destroyScafolding()
    {
        if (transform.childCount>1)
        {
            Destroy(transform.GetChild(1).gameObject);
            Destroy(transform.GetChild(2).gameObject);
            Destroy(transform.GetChild(3).gameObject);
            Destroy(transform.GetChild(4).gameObject);
        }
        
    }

    private void adjustMaterials()
    {
        if (name.Equals("Mine"))
        {
            materials[0].color = Color.yellow;
            materials[1].color = Color.white;
            materials[2].color = teamColor;
            materials[3].color = teamColor;
            materials[4].color = new Color(0.5f, 0.4f, 0.3f);
            materials[5].color = new Color(0.4f, 0.5f, 0.4f);
            materials[6].color = Color.gray;
            
        }
        /*else if (name.Equals(""))
        {

        }*/
    }

    IEnumerator sumarMonedes()
    {
        minant = true;
        yield return new WaitForSeconds(2);
        player.monedes += 10;
        //Debug.Log(player.monedes);
        minant = false;
    }

    IEnumerator sumarFusta()
    {
        minant = true;
        yield return new WaitForSeconds(2);
        player.fusta += 10;
        //Debug.Log(player.monedes);
        minant = false;
    }
    IEnumerator constructTimer()
    {
        constructing = true;

        life = health.getHealth();
        yield return new WaitForSeconds(0.4f);

        life += maxLife/ 10 / duration;
        health.setHealth(life);

        t = life / maxLife;
        foreach(Material material in materials)
        {
            material.color = Color.Lerp(new Color(0.5f, 0.5f, 0.8f, 0.3f), new Color(1f, 1f, 1f, 1f), t);
        }
        //renderer.material.color = Color.Lerp(new Color(0.5f, 0.8f, 0.5f, 0.3f), new Color(1f, 0.5f, 0.5f, 1f), t);

        constructing = false;
    }

}
