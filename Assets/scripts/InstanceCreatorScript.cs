﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class InstanceCreatorScript : MonoBehaviour
{
    public Button startInstanceButton;
    public Button backToMainMenuButton;

    public Slider popSizeSlider;
    public Slider genTimeSlider;
    public Slider mutationRateSlider;

    public TextMeshProUGUI popSizeText;
    public TextMeshProUGUI genTimeText;
    public TextMeshProUGUI mutationRateText;

    public void Start()
    {
        startInstanceButton.onClick.AddListener(StartInstance);
        backToMainMenuButton.onClick.AddListener(GoBackToMainMenu);

        popSizeSlider.onValueChanged.AddListener(UpdatePopulationText);
        genTimeSlider.onValueChanged.AddListener(UpdateTimeText);
        mutationRateSlider.onValueChanged.AddListener(UpdateMutationText);
    }

    public void StartInstance()
    {
        Debug.Log("Popsize: " + popSizeSlider.value.ToString() + ", Gentime: " + genTimeSlider.value.ToString());
        InstanceData.GenerationTime = (int) genTimeSlider.value;
        InstanceData.PopulationSize = (int) popSizeSlider.value;
        InstanceData.MutationRate = mutationRateSlider.value;
        InstanceData.DataCollectionMode = false;

        SceneManager.LoadScene("MainScene");
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    void UpdatePopulationText(float value)
    {
        popSizeText.text = value.ToString();
    }

    void UpdateMutationText(float value)
    {
        mutationRateText.text = value.ToString("n2");
    }

    void UpdateTimeText(float value)
    {
        genTimeText.text = value.ToString() + " seconds";
    }
}
