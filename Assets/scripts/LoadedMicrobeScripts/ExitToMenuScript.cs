using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenuScript : MonoBehaviour
{
    public void ExitToMenu()
    {
        InstanceData.SimSpeed = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
