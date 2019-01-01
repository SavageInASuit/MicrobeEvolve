using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceEntryScript : MonoBehaviour {

    public TextMeshProUGUI idText;
    public TextMeshProUGUI distanceText;

    public string idString;
    public float distance;

    public void SetIdAndDist(string id, float dist)
    {
        idText.text = id;
        string[] parts = id.Split(' ');
        idString = parts[1];
        distanceText.text = dist.ToString("n2") + "m";
        distance = dist;
    }
}
