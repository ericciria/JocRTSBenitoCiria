using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitsButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] GameObject unitPrefab;
    UnitData data;
    CameraController player;
    private Button button;
    private GameObject dataCanvas;
    private string name;
    private int money, metal;
    private string description;

    private void Awake()
    {
        dataCanvas = GameObject.Find("/Canvas/Info");
    }
    void Start()
    {
        data = unitPrefab.GetComponentInChildren<Unit>().unitData;
        player = GameObject.Find("/Camera").GetComponent<CameraController>();
        button = GetComponent<Button>();
        name = data.name;
        money = data.MoneyCost;
        metal = data.MetalCost;
        description = data.Description;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.building.canSpawn && player.fusta >= metal && player.monedes >= money && player.electricitat>=0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        } 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dataCanvas.transform.Find("Name").GetComponent<Text>().text = name;
        dataCanvas.transform.Find("MonedesQ").GetComponent<Text>().text = money.ToString();
        dataCanvas.transform.Find("MetallQ").GetComponent<Text>().text = metal.ToString();
        dataCanvas.transform.Find("Description").GetComponent<Text>().text = description;

        dataCanvas.transform.position = new Vector3(this.transform.position.x - 50, dataCanvas.transform.position.y, dataCanvas.transform.position.z);

        dataCanvas.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dataCanvas.SetActive(false);
    }
}
