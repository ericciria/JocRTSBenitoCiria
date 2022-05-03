using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] BuildingData data;
    private GameObject dataCanvas;
    private string name;
    private int money;
    private int metal;
    private string description;
    private CameraController player;
    private Button button;
    // Start is called before the first frame update

    private void Awake()
    {
        dataCanvas = GameObject.Find("/Canvas/Info");
    }

    void Start()
    {
        player = GameObject.Find("/Camera").GetComponent<CameraController>();
        button = GetComponent<Button>() ;
        name = data.name;
        money = data.MoneyCost;
        metal = data.MetalCost;
        description = data.Description;

        // se que aizò s'executarà varies vegades, pero no he trobat cap altra forma de fer-ho sense que doni algun error
        dataCanvas.SetActive(false);    
    }
    private void Update()
    {
        if (player.fusta >= metal)
        {
            if(player.monedes >= money)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
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

        dataCanvas.transform.position = new Vector3(this.transform.position.x-50, dataCanvas.transform.position.y, dataCanvas.transform.position.z);

        dataCanvas.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dataCanvas.SetActive(false);
    }
}
