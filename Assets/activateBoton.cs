using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateBoton : MonoBehaviour
{
    public GameObject btn_house;
    public GameObject btn_mina;
    public void ActivarContrucio()
    {
        btn_house.SetActive(true);
        btn_mina.SetActive(true);
    }
}
