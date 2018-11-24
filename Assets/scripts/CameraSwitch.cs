using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraSwitch : MonoBehaviour
{
    public GameObject[] objects;
    public TextMeshProUGUI text;
    public Button switchButton;

    int m_CurrentActiveObject;
    Text buttonText;

    public void Start()
    {
        switchButton.onClick.AddListener(NextCamera);
        buttonText = switchButton.GetComponentInChildren<Text>();

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == m_CurrentActiveObject);
            if (i == m_CurrentActiveObject)
                buttonText.text = objects[(i + 1) % objects.Length].name;
        }
    }

    private void OnEnable()
    {
        text.text = objects[m_CurrentActiveObject].name;
    }


    public void NextCamera()
    {
        int nextactiveobject = m_CurrentActiveObject + 1 >= objects.Length ? 0 : m_CurrentActiveObject + 1;

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == nextactiveobject);
            if(i == nextactiveobject)
                buttonText.text = objects[(i + 1) % objects.Length].name;
        }

        m_CurrentActiveObject = nextactiveobject;
        text.text = objects[m_CurrentActiveObject].name;
    }
}
