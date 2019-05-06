using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MicrobeApplication;
using TMPro;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField]
    private Button newInstanceButton;
    [SerializeField]
    private Button newFreeforallInstanceButton;
    [SerializeField]
    private Button loadMicrobeButton;
    [SerializeField]
    private Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        newInstanceButton.onClick.AddListener(GoToNewInstanceScene);
        newFreeforallInstanceButton.onClick.AddListener(GoToFreeforallScene);
        loadMicrobeButton.onClick.AddListener(GoToLoadMicrobeScene);
        exitButton.onClick.AddListener(ExitProgram);
    }

    public void GoToNewInstanceScene()
    {
        InstanceData.FFAMode = false;
        SceneManager.LoadScene("InstanceMenuScene");
    }

    public void GoToFreeforallScene()
    {
        InstanceData.FFAMode = true;
        SceneManager.LoadScene("InstanceMenuScene");
    }

    public void GoToLoadMicrobeScene()
    {
        SceneManager.LoadScene("LoadMicrobeMenuScene");
    }

    public void ExitProgram()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
