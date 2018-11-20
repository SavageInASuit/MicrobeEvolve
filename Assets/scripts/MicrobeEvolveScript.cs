using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrobeEvolveScript : MonoBehaviour {
    /*
     * This script takes settings defined in the start instance menu then
     * populates, manages and evolves the microbe generations
     * 
     * General Idea:
     * Each microbe has its own chromosome which all follow the same format.
     * The chromosome will encode: 
     * hull_id, hull_mass, hull_buoyancy, component_1_id, component_1_vertex_id...
     * And so on for the number of components wanted (defined in init options)
     * 
     * When the scene first loads, a population should be generated at the
     * specified size (defined in init options)
     */

    [Range(0.01f, 1f)] [SerializeField] private float mutationRate = 0.05f;   // Probability of each bit in chromosome flipping
    // Should probably have a min value of 3 for the algorithm to work? 
    [Range(1, 100)] [SerializeField] private int populationSize;    // Size of the population throughout the evolution process

    MicrobeBuilderScript microbeBuilder;

    void GenerateInitialPopulation(){
        for (int i = 0; i < populationSize; i++){
            microbeBuilder.CreateInitialMicrobe();
        }
    }

    // Use this for initialization
    void Start () {
        microbeBuilder = GetComponent<MicrobeBuilderScript>();

        GenerateInitialPopulation();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
