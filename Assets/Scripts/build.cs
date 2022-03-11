using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class build : MonoBehaviour
{
    public GameObject build_blue;
    public GameObject build_mina;
    public GameObject btn_Hosue;
    public GameObject btn_Mina;

    public void spawn_blue()
    {   
        Instantiate(build_blue);
    }
    public void spawn_mina()
    {
        Instantiate(build_mina);
    }

    public void Start()
    {
        btn_Hosue.SetActive(false);
        btn_Mina.SetActive(false);
    }
    
}
