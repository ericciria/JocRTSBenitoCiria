using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scriptMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject eric;

    public void Start()
    {
        eric.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eric.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eric.SetActive(false);
    }

}
