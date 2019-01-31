using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceEntryScript : MonoBehaviour {

    public TextMeshProUGUI idText;
    public TextMeshProUGUI distanceText;

    public string idString;
    public float distance;
    public int position;
    public int id;
    public int generation;

    public void SetIdAndDist(int position, int id, int generation, float dist)
    {
        this.position = position;
        this.id = id;
        this.generation = generation;

        idString = position.ToString() + ". #" + id.ToString() + "/gen " + generation.ToString();
        idText.text = idString;

        distanceText.text = dist.ToString("n2") + "m";
        distance = dist;
    }
}
