using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MicrobeApplication;

public class ScoreListScript : MonoBehaviour {

    List<DistanceEntryScript> distEntries;

    private int distCount;

    public GameObject EntryObject;

    // Use this for initialization
    void Start () {
        distEntries = new List<DistanceEntryScript>();
    }

    public void PlaceMicrobe(int id, int generation, float score, Chromosome chromosome)
    {
        bool inTop = false;
        int placeInd = -1;
        for (int i = 0; i < distCount; i++)
        {
            if (score > distEntries[i].distance)
            {
                inTop = true;
                placeInd = i;
                break;
            }
        }

        int pos = ((placeInd == -1) ? distCount + 1 : placeInd+1);

        if (distCount < 10)
        {
            GameObject entry = Instantiate(EntryObject);
            entry.transform.SetParent(gameObject.transform);
            entry.transform.localScale = new Vector3(1, 1, 1);

            distEntries.Add(entry.GetComponent<DistanceEntryScript>());

            distCount++;
        }

        if (inTop)
        {
            for (int i = distCount - 1; i > placeInd; i--)
            {

                distEntries[i].SetData((i+1), distEntries[i - 1].id, distEntries[i - 1].generation, distEntries[i - 1].distance, distEntries[i - 1].GetChromosome());
            }

            distEntries[placeInd].SetData(pos, id, generation, score, chromosome);
        }
        else if (distCount < 10)
        {
            distEntries[distCount-1].SetData(pos, id, generation, score, chromosome);

        }
    }
}
