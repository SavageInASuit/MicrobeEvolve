using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;
using MicrobeApplication;

public class LoadMicrobeScript : MonoBehaviour
{
    [SerializeField]
    private GameObject microbeFilePrefab;
    [SerializeField]
    private Transform fileContainer;
    [SerializeField]
    private TMP_InputField chromosomeInput;
    [SerializeField]
    private Button loadChromosomeStringButton;


    public void Start()
    {
        string folder = Application.persistentDataPath;
        string[] paths = Directory.GetFiles(folder);
        foreach (string pathname in paths)
        {
            if (Path.GetExtension(pathname) == ".txt")
            {
                using (StreamReader sr = File.OpenText(pathname))
                {
                    GameObject microbeButton = Instantiate(microbeFilePrefab);
                    FileMicrobeScript fms = microbeButton.GetComponent<FileMicrobeScript>();
                    string fileName = Path.GetFileNameWithoutExtension(pathname);
                    fms.SetMicrobeFileText(fileName);
                    string chromosomeString = sr.ReadLine();
                    fms.ChromosomeString = chromosomeString;

                    microbeButton.transform.SetParent(fileContainer);
                    Button button = microbeButton.GetComponent<Button>();
                    button.onClick.AddListener(() => OnMicrobeFileButtonClick(fms));
                }
            }
        }
        chromosomeInput.onSelect.AddListener(PasteText);
        loadChromosomeStringButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        string chromString = chromosomeInput.text;
        if (Chromosome.IsValidChromosome(chromString))
        {
            InstanceData.ChromosomeString = chromString;
            SceneManager.LoadScene("MicrobeViewScene");
        }
        else
        {
            chromosomeInput.text = "Invalid or corrupt chromosome!";
        }
    }

    public void PasteText(string text)
    {
        TextEditor te = new TextEditor();
        te.Paste();
        chromosomeInput.text = te.text;
    }

    public void OnMicrobeFileButtonClick(FileMicrobeScript fms)
    {
        InstanceData.ChromosomeString = fms.ChromosomeString;
        SceneManager.LoadScene("MicrobeViewScene");
    }
}
