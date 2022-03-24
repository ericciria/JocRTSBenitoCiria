using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class activateBoton : MonoBehaviour
{
    public GameObject btn_house;
    public GameObject btn_mina;
    private int contador = 0;
    private Button button;

    public void Start()
    {
        button = btn_mina.GetComponent<Button>();
    }
    public void ActivarContrucio()
    {
        contador++;
        if (contador == 2)
        {
            button.interactable = false;
        }
        btn_house.SetActive(true);
        btn_mina.SetActive(true);
    }
}
