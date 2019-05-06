using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private MicrobeStorageScript microbeStorage;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        microbeStorage.Close();
    }
}
