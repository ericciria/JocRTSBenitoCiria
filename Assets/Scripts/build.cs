using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class build : MonoBehaviour
{
    public GameObject build_blue;
    public GameObject build_mina;
    public GameObject build_torre;
    public GameObject build_millores;
    public GameObject build_tanko;
    public GameObject build_paleta;
    public GameObject btn_Hosue;
    public GameObject btn_Mina;
    public GameObject[] unitPrefabs;

    public CameraController player;
    private Vector3 position;

    void Start()
    {
        btn_Hosue.SetActive(false);
        btn_Mina.SetActive(false);

        player = GameObject.Find("/Camera").GetComponent<CameraController>();
    }

    public void spawn_blue()
    {
        GameObject blueprint = GameObject.FindGameObjectWithTag("Blueprint");
        if (blueprint != null)
        {
            Destroy(blueprint);
        }
        Instantiate(build_blue);
    }
    public void spawn_torre()
    {
        GameObject blueprint = GameObject.FindGameObjectWithTag("Blueprint");
        if (blueprint != null)
        {
            Destroy(blueprint);
        }
        Instantiate(build_torre);
    }
    public void spawn_mina()
    {
        GameObject blueprint = GameObject.FindGameObjectWithTag("Blueprint");
        if (blueprint != null)
        {
            Destroy(blueprint);
        }
        Instantiate(build_mina);
    }

    public void spawn_millores()
    {
        GameObject blueprint = GameObject.FindGameObjectWithTag("Blueprint");
        if (blueprint != null)
        {
            Destroy(blueprint);
        }
        Instantiate(build_millores);
    }

    public void spawn_tanko()
    {
        GameObject blueprint = GameObject.FindGameObjectWithTag("Blueprint");
        if (blueprint != null)
        {
            Destroy(blueprint);
        }
        Instantiate(build_tanko);
    }

    public void spawn_paletas()
    {
        GameObject blueprint = GameObject.FindGameObjectWithTag("Blueprint");
        if (blueprint != null)
        {
            Destroy(blueprint);
        }
        Instantiate(build_paleta);
    }

    public void spawnUnit(int prefab)
    {
        UnitData unitData = unitPrefabs[prefab].GetComponentInChildren<Unit>().unitData;
        player.building.SpawnUnitActivator(unitPrefabs[prefab]);
        player.monedes -= unitData.MoneyCost;
        player.fusta -= unitData.MetalCost;
    }


}
