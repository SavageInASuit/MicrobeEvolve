using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenuScript : MonoBehaviour
{
    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
