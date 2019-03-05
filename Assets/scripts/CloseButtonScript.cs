using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject textBox;

    private SaveChromosomeScript scs;

    private void Start()
    {
        gameObject.SetActive(false);

        scs = textBox.GetComponent<SaveChromosomeScript>();
        if (scs == null)
            Debug.Log("Couldnt find save script!!!!!!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        scs.Close();
    }
}
