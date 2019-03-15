using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrobeApplication;

public class DistanceEntryScript : MonoBehaviour
{

    public TextMeshProUGUI idText;
    public TextMeshProUGUI distanceText;

    public string idString;
    public float distance;
    public int position;
    public int id;
    public int generation;

    private Chromosome chromosome;

    public void SetData(int position, int id, int generation, float dist, Chromosome chromosome)
    {
        this.position = position;
        this.id = id;
        this.generation = generation;

        this.chromosome = chromosome;

        idString = position.ToString() + ". #" + id.ToString() + "/gen " + generation.ToString();
        idText.text = idString;

        distanceText.text = dist.ToString("n2") + "m";
        distance = dist;
    }

    public Chromosome GetChromosome()
    {
        return chromosome;
    }

    public void OnMouseOver()
    {
        Debug.Log(position.ToString() + ". #" + id.ToString() + "/gen " + generation.ToString());
    }

    public void OnMouseEnter()
    {
        Debug.Log(position.ToString() + ". #" + id.ToString() + "/gen " + generation.ToString());
    }
}
