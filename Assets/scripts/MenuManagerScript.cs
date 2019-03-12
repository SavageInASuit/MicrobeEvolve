using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    [SerializeField]
    private TMP_InputField chromosomeInput;

    // Start is called before the first frame update
    void Start()
    {
        newInstanceButton.onClick.AddListener(GoToNewInstanceScene);
        newFreeforallInstanceButton.onClick.AddListener(GoToFreeforallScene);
        loadMicrobeButton.onClick.AddListener(GoToLoadMicrobeScene);
        exitButton.onClick.AddListener(GoToNewInstanceScene);
    }

    public void GoToNewInstanceScene()
    {
        SceneManager.LoadScene("InstanceMenuScene");
    }

    public void GoToFreeforallScene()
    {
        SceneManager.LoadScene("FreeForAllScene");
    }

    public void GoToLoadMicrobeScene()
    {
        // Put the chromosome code into the instance data object
        InstanceData.ChromosomeString = chromosomeInput.text;
        SceneManager.LoadScene("LoadedMicrobeScene");
    }

    public void ExitProgram()
    {

    }
}
