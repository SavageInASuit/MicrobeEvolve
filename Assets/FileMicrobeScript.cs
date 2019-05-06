using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileMicrobeScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text microbeFileText;

    public string ChromosomeString { get; set; }
    public string FileName { get; set; }

    public void SetMicrobeFileText(string fileName)
    {
        microbeFileText.text = fileName;
        FileName = fileName;
    }
}
