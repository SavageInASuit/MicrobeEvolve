using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class InstanceCreatorScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private TMP_Text mutTitle;
    [SerializeField]
    private TMP_Text mutDescription;

    public Button startInstanceButton;
    public Button backToMainMenuButton;

    public Slider popSizeSlider;
    public Slider genTimeSlider;
    public Slider mutationRateSlider;

    public TextMeshProUGUI popSizeText;
    public TextMeshProUGUI genTimeText;
    public TextMeshProUGUI mutationRateText;

    [SerializeField]
    private Toggle singleBitToggle;
    [SerializeField]
    private Toggle randomBitToggle;

    public void Start()
    {
        startInstanceButton.onClick.AddListener(StartInstance);
        backToMainMenuButton.onClick.AddListener(GoBackToMainMenu);

        popSizeSlider.onValueChanged.AddListener(UpdatePopulationText);
        genTimeSlider.onValueChanged.AddListener(UpdateTimeText);
        mutationRateSlider.onValueChanged.AddListener(UpdateMutationText);

        singleBitToggle.onValueChanged.AddListener(MutationTypeChanged);

        if (InstanceData.FFAMode)
            titleText.text = "FFA Instance Creator";
        else
            titleText.text = "Sequential Instance Creator";
    }

    public void StartInstance()
    {
        Debug.Log("Popsize: " + popSizeSlider.value.ToString() + ", Gentime: " + genTimeSlider.value.ToString());
        InstanceData.GenerationTime = (int) genTimeSlider.value;
        InstanceData.PopulationSize = (int) popSizeSlider.value;
        InstanceData.MutationRate = mutationRateSlider.value;
        InstanceData.DataCollectionMode = false;

        if (singleBitToggle.isOn)
            InstanceData.SingleMutate = true;
        else if (randomBitToggle.isOn)
            InstanceData.SingleMutate = false;

        if (InstanceData.FFAMode)
            SceneManager.LoadScene("FreeForAllScene");
        else
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

    void MutationTypeChanged(bool val)
    {
        if (singleBitToggle.isOn)
        {
            mutTitle.text = "Single Bit Mutation";
            mutDescription.text = "Only a single bit of the microbe's chromosome is flipped at mutation time.\n\n(with a probability equal to mutation rate)";
        }
        else
        {
            mutTitle.text = "Random Bit Mutation";
            mutDescription.text = "Each bit of the microbe's chromosome is flipped.\n\n(with a probability equal to mutation rate)";
        }
    }
}
