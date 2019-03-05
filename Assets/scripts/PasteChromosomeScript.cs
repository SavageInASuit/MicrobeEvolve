using UnityEngine;
using TMPro;

public class PasteChromosomeScript : MonoBehaviour
{
    private TMP_InputField input;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<TMP_InputField>();

        input.onSelect.AddListener(pasteText);
    }

    public void pasteText(string text)
    {
        TextEditor te = new TextEditor();
        te.Paste();
        input.text = te.text;
    }
}
