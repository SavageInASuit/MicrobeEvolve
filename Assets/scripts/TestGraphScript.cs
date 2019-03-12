using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGraphScript : MonoBehaviour
{
    [SerializeField]
    private GraphScript graph;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            graph.AddPoint(Random.Range(0f, 100f));
    }
}
