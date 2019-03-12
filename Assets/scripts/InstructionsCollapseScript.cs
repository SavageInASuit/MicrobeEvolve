using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsCollapseScript : MonoBehaviour
{
    private bool collapsed;

    private RectTransform rt;
    private float panelWidth;

    [SerializeField]
    private RectTransform collapseButton;

    private Vector3 origPos;

    public void Start()
    {
        rt = GetComponent<RectTransform>();
        panelWidth = rt.rect.width;

        origPos = rt.anchoredPosition;
    }

    public void OnCollapseButtonClick()
    {
        if (collapsed)
        {
            // rt.Translate(panelWidth, 0, 0);

            rt.anchoredPosition = origPos;
            collapseButton.rotation = Quaternion.Euler(0, 0, 0);

            collapsed = false;
        }
        else
        {
            // rt.Translate(-panelWidth, 0, 0);

            rt.anchoredPosition = origPos - new Vector3(panelWidth, 0, 0);
            collapseButton.rotation = Quaternion.Euler(0, 0, 180);

            collapsed = true;
        }
    }
}
