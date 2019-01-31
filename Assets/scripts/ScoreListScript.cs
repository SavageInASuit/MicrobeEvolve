using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreListScript : MonoBehaviour {

    List<DistanceEntryScript> distEntries;

    private int distCount;

    public GameObject EntryObject;

    // Use this for initialization
    void Start () {
        distEntries = new List<DistanceEntryScript>();
    }

    public void PlaceMicrobe(int id, int generation, float score)
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

        // TODO: Fix issue here affecting the proper display of the gen number

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
                distEntries[i].SetIdAndDist((i+1), distEntries[i - 1].id, distEntries[i - 1].generation, distEntries[i - 1].distance);
            }

            distEntries[placeInd].SetIdAndDist(pos, id, generation, score);
        }
        else if (distCount < 10)
        {
            distEntries[distCount-1].SetIdAndDist(pos, id, generation, score);

        }
    }
}
