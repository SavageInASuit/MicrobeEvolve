using System.Collections;
using System.Collections.Generic;
using Application;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(MicrobeBuilderScript))]
[RequireComponent(typeof(MicrobeEvolveScript))]
public class PopulationManagerScript : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI generationText;
    [SerializeField]
    private TextMeshProUGUI microbeText;
    [SerializeField]
    private TextMeshProUGUI distanceText;
    [SerializeField]
    private TextMeshProUGUI simSpeedText;

    [SerializeField]
    private float roundTime;

    private int generation;
    private int chromosomeInd;

    private float startTime;

    private Chromosome[] population;
    private GameObject currentMicrobe;

    MicrobeBuilderScript microbeBuilder;
    MicrobeEvolveScript microbeEvolver;

    public ScoreListScript listScript;

    // Use this for initialization
    void Start () {
        microbeBuilder = GetComponent<MicrobeBuilderScript>();
        microbeEvolver = GetComponent<MicrobeEvolveScript>();

        generation = 0;
        chromosomeInd = 0;

        generationText.text = "Gen" + (generation + 1);
        microbeText.text = "microbe: " + (chromosomeInd+1) + "/" + population.Length;
    }

    // Update is called once per frame
    void Update() {
        if ((population != null) && currentMicrobe != null && Time.time - startTime > roundTime)
        {
            population[chromosomeInd - 1].Fitness = GetFitness(currentMicrobe);
            Destroy(currentMicrobe);
            // If we have processed the final microbe, we can start the next gen
            if (chromosomeInd == population.Length)
            {
                // exit from function and start the evolution process
                chromosomeInd = 0;
                microbeEvolver.EvolveNextGeneration();

                return;
            }

            StartMicrobe();

        }
        else if (currentMicrobe == null && microbeBuilder != null)
        {
            StartMicrobe();
        }

        if (currentMicrobe != null)
        {
            distanceText.text = "Distance: " + GetFitness(currentMicrobe).ToString("n2") + "m";
        }

        CheckSpeed();

        MeshRenderer[] rends = currentMicrobe.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < rends.Length; i++)
        {
            Color c = rends[i].material.color;
            c.g -= 1f / roundTime * Time.deltaTime;
            c.b -= 1f / roundTime * Time.deltaTime;
            rends[i].material.color = c;
        }
    }

    void StartMicrobe()
    {
        microbeText.text = "Microbe: " + (chromosomeInd + 1) + " of " + population.Length;

        if (microbeBuilder != null)
        {
            if (currentMicrobe != null)
            {
                Debug.Log("Calling PlaceMicrobe");
                listScript.PlaceMicrobe(chromosomeInd, (generation + 1), GetFitness(currentMicrobe));
            }

            currentMicrobe = microbeBuilder.CreateInitialMicrobe(population[chromosomeInd]);
            //Debug.Log("Starting microbe: " + microbeCount);
            // Start timer? after specified time, kill microbe and set fitness
            startTime = Time.time;


            chromosomeInd++;
        }
    }

    public void SetGenerationChromosomes(Chromosome[] chromosomes)
    {
        population = chromosomes;

        generation++;
        generationText.text = "Gen" + (generation+1);

        StartMicrobe();
    }

    private float GetFitness(GameObject microbe)
    {
        Vector2 pos = new Vector2(microbe.transform.position.x, microbe.transform.position.z);

        if(pos.magnitude > 115.0f)
        {
            return 0.1f;
        }

        return pos.magnitude;
    }

    private void CheckSpeed()
    {
        if (Input.GetKeyDown("space"))
        {
            Time.timeScale = 1;
            simSpeedText.text = "Sim Speed: " + Time.timeScale + "x";
        }else if (Input.GetKeyDown("up"))
        {
            Time.timeScale += 1;
            simSpeedText.text = "Sim Speed: " + Time.timeScale + "x";
        }
        else if (Input.GetKeyDown("down"))
        {
            Time.timeScale -= 1;
            simSpeedText.text = "Sim Speed: " + Time.timeScale + "x";
        }
    }
}
