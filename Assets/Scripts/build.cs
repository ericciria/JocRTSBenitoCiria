using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class build : MonoBehaviour
{
    public GameObject build_blue;
    public GameObject build_mina;
    public GameObject build_torre;
    public GameObject btn_Hosue;
    public GameObject btn_Mina;
    public GameObject constructor;
    public GameObject tank;

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

    public void spawn_constructor()
    {
        GameObject unit = Instantiate(constructor, player.building.transform.position + new Vector3(0, 0, -2), Quaternion.identity);
        Unit playerUnit = unit.GetComponentInChildren<Unit>();
        playerUnit.agent.SetDestination(player.building.transform.position + new Vector3(0,0,-5));
    }
    public void spawn_tank()
    {
        GameObject unit = Instantiate(tank, player.building.transform.position + new Vector3(0, 0, -2), Quaternion.identity);
        Unit playerUnit = unit.GetComponentInChildren<Unit>();
        playerUnit.agent.SetDestination(player.building.transform.position + new Vector3(0, 0, -5));
    }

}
