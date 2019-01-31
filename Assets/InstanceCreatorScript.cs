using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class InstanceCreatorScript : MonoBehaviour
{
    public Button startInstanceButton;
    public Slider popSizeSlider;
    public Slider genTimeSlider;

    public TextMeshProUGUI popSizeText;
    public TextMeshProUGUI genTimeText;



    public void Start()
    {
        startInstanceButton.onClick.AddListener(StartInstance);

        popSizeSlider.onValueChanged.AddListener(UpdatePopulationText);
        genTimeSlider.onValueChanged.AddListener(UpdateTimeText);
    }

    public void StartInstance()
    {
        Debug.Log("Popsize: " + popSizeSlider.value.ToString() + ", Gentime: " + genTimeSlider.value.ToString());
        InstanceData.GenerationTime = (int) genTimeSlider.value;
        InstanceData.PopulationSize = (int) popSizeSlider.value;

        SceneManager.LoadScene("MainScene");
    }

    void UpdatePopulationText(float value)
    {
        popSizeText.text = value.ToString();
    }

    void UpdateTimeText(float value)
    {
        genTimeText.text = value.ToString() + " seconds";
    }
}
