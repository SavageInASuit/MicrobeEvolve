using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseInputBoxScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject inputBox;
    [SerializeField]
    private GameObject goButton;

    public void OnPointerClick(PointerEventData data)
    {
        inputBox.SetActive(false);
        gameObject.SetActive(false);
        goButton.SetActive(false);
    }

    public void ShowBox()
    {
        inputBox.SetActive(true);
        gameObject.SetActive(true);
        goButton.SetActive(true);
    }
}
