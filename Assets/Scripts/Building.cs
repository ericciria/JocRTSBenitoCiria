using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UniqueIdentifier))]

public class Building : MonoBehaviour
{
    public bool constructing, constructed, canSpawn;
    public Material opaqueMat;
    public int team = 1;
    public int moneyCost, metalCost, energy;
    private int attackDamage;
    public string name;

    private float life;
    [SerializeField] int maxLife;
    public ObjectLife health;
    public MeshRenderer renderer;
    [SerializeField] float duration = 2f;
    [SerializeField] CameraController player;
    private AIGeneral aiGeneral;
    public BuildingData data;
    public Material[] materials;
    public Color teamColor;

    private bool minant;
    private float spawnTimer, timerAmount;
    [SerializeField] GameObject[] spawnableUnits;
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
        canSpawn = true;


        moneyCost = data.MoneyCost;
        metalCost = data.MetalCost;
        energy = data.EnergyCost;
        maxLife = data.MaxHealth;
        attackDamage = data.AttackDamage;
        name = data.BuildingName;
        health.setMaxHealth(maxLife);
        spawnTimer = 10f;

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
            aiGeneral = GameObject.Find("/AIGeneral").GetComponent<AIGeneral>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (constructed && !minant && data.BuildingName.Equals("PetrolPump"))
        {
            if(team==1 && player.electricitat >= 0)
            {
                StartCoroutine(SumarFusta());
            }
            else if(team==2 && aiGeneral.energia >= 0)
            {
                StartCoroutine(SumarFusta());
            }
            
        }
        if (constructed && !minant && data.BuildingName.Equals("Mine"))
        {
            if (team == 1 && player.electricitat >= 0)
            {
                StartCoroutine(SumarMonedes());
            }
            else if (team == 2 && aiGeneral.energia >= 0)
            {
                StartCoroutine(SumarMonedes());
            }
        }
    }

    public void Construct()
    {
        if (!constructing)
        {
            if (life < maxLife)
            {
                StartCoroutine(ConstructTimer());
                //Debug.Log("Life: " + life);
            }
            else
            {
                constructed = true;
                renderer.material = opaqueMat;
                if (team == 1)
                {
                    player.electricitat += energy;
                }
                else
                {
                    aiGeneral.energia += energy;
                }
                AdjustMaterials();
                //destroyScafolding();
            }
        }
    }

    private void DestroyScafolding()
    {
        if (transform.childCount>1)
        {
            Destroy(transform.GetChild(1).gameObject);
            Destroy(transform.GetChild(2).gameObject);
            Destroy(transform.GetChild(3).gameObject);
            Destroy(transform.GetChild(4).gameObject);
        }
        
    }

    public void AdjustMaterials()
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

    IEnumerator SumarMonedes()
    {
        minant = true;
        yield return new WaitForSeconds(2);
        if (this.team == 1)
        {
            player.monedes += 10;
            //Debug.Log(player.monedes);
        }
        else
        {
            aiGeneral.monedes += 10;
        }
        minant = false;

    }

    IEnumerator SumarFusta()
    {
        minant = true;
        yield return new WaitForSeconds(2);
        if (this.team == 1)
        {
            player.fusta += 10;
            //Debug.Log(player.monedes);
        }
        else
        {
            aiGeneral.metall += 10;
        }
        minant = false;
    }
    IEnumerator ConstructTimer()
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

    public void SpawnUnitActivator(GameObject unit)
    {
        StartCoroutine(SpawnUnit(unit));
    }

    IEnumerator SpawnUnit(GameObject unit)
    {
        canSpawn = false;
        yield return new WaitForSeconds(1f);
        GameObject spawnedUnit = Instantiate(unit, this.transform.position, Quaternion.identity) as GameObject;
        Unit unitComponent = spawnedUnit.GetComponentInChildren<Unit>();
        unitComponent.agent.SetDestination(this.transform.position - new Vector3(0, 0, +5));
        canSpawn = true;
    }

}
