using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveChromosomeScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    public GameObject closeButton;

    private void Start()
    {
        inputField.onSelect.AddListener(OnSelect);
        gameObject.SetActive(false);
    }

    public void OnSelect(string text)
    {
        CopyToClipboard(text);
        inputField.text = "Copied to clipboard!";
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
        inputField.text = text;
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
