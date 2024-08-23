using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static Hover Event_instance;
    public bool isHoverDown = false;
    public bool isClick = false;
    public bool isEnter = false;

    void Start()
    {
        Event_instance = this;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHoverDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHoverDown = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClick = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }
}
