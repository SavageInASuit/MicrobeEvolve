using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        image.color = new Color(0.6f, 1.0f, 0.6f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        image.color = new Color(0.8f, 1.0f, 0.8f);
    }
}
