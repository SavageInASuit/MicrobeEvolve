using System.IO;
using UnityEngine;
using MicrobeApplication;
using TMPro;

public class MicrobeStorageScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInput;

    [SerializeField]
    private TMP_InputField chromosomeInput;

    public string ChromosomeString { get; set; }

    public GameObject closeButton;

    public void OnSaveButtonClicked()
    {
        string filename = nameInput.text;
        if (IsValidFilename(filename))
        {
            SaveMicrobeLocalScript.SaveMicrobe(filename, ChromosomeString);
            nameInput.text = "Saved Microbe!";
        }
    }

    private bool IsValidFilename(string filename)
    {
        if (filename.Length == 0)
        {
            nameInput.text = "Your microbe needs a name";
            return false;
        }
        if (filename.Length > 20)
        {
            nameInput.text = "Shorter name needed";
            return false;
        }
        char[] invalids = Path.GetInvalidFileNameChars();
        foreach (char c in invalids)
        {
            if (filename.Contains(new string(c, 1)))
            {
                nameInput.text = "Name can't contain '" + c + "'s";
                return false;
            }
        }

        return true;
    }

    private void Start()
    {
        chromosomeInput.onSelect.AddListener(OnSelect);
        gameObject.SetActive(false);
    }

    public void OnSelect(string text)
    {
        CopyToClipboard(text);
        chromosomeInput.text = "Copied to clipboard!";
    }

    private void CopyToClipboard(string text)
    {
        TextEditor te = new TextEditor();
        te.text = text;
        te.SelectAll();
        te.Copy();
    }

    public void SetTextboxText(string text)
    {
        chromosomeInput.text = text;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        closeButton.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        closeButton.SetActive(true);
    }
}
