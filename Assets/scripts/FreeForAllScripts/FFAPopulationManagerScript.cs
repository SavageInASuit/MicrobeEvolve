using System.Collections;
using System.Collections.Generic;
using Application;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(MicrobeBuilderScript))]
[RequireComponent(typeof(FFAMicrobeEvolveScript))]
public class FFAPopulationManagerScript : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI generationText;
    [SerializeField]
    private TextMeshProUGUI microbeText;
    [SerializeField]
    private TextMeshProUGUI distanceText;
    [SerializeField]
    private TextMeshProUGUI simSpeedText;
    [SerializeField]
    private TextMeshProUGUI lifeTimeText;

    [SerializeField]
    private float roundTime;

    private int generation;
    private int chromosomeInd;

    private float startTime;

    private Chromosome[] population;
    // TODO: change to being current best? Have the best be highlighted in the scene
    private GameObject currentMicrobe;
    private GameObject[] currentMicrobes;
    private bool needToSpawn = true;

    MicrobeBuilderScript microbeBuilder;
    FFAMicrobeEvolveScript microbeEvolver;

    public ScoreListScript listScript;

    // Use this for initialization
    void Start () {
        microbeBuilder = GetComponent<MicrobeBuilderScript>();
        microbeEvolver = GetComponent<FFAMicrobeEvolveScript>();

        currentMicrobes = new GameObject[InstanceData.PopulationSize];

        generation = 0;
        chromosomeInd = 0;

        roundTime = InstanceData.GenerationTime;

        generationText.text = "Gen: " + (generation + 1);
        // microbeText.text = "microbe: " + (chromosomeInd+1) + "/" + population.Length;
    }

    // TODO: Fix fitness mechanics to handle the whole generation spawning at the same time
    void Update() {
        float curTime = Time.time - startTime;
        lifeTimeText.text = "Time: " + curTime.ToString("n1") + "/" + roundTime.ToString() + "s";
        if (curTime > roundTime && needToSpawn == false)
        {
            DestroyGeneration();
            needToSpawn = true;
        }
        if (population != null && needToSpawn == true && microbeBuilder != null)
        {
            microbeEvolver.EvolveNextGeneration();

            StartMicrobes();
        }

        //if (needToSpawn == false)
        //{
        //    distanceText.text = "Distance: " + GetFitness(currentMicrobe).ToString("n2") + "m";
        //}

        CheckSpeed();

        //MeshRenderer[] rends = currentMicrobe.GetComponentsInChildren<MeshRenderer>();
        //for (int i = 0; i < rends.Length; i++)
        //{
        //    Color c = rends[i].material.color;
        //    c.g -= 1f / roundTime * Time.deltaTime;
        //    c.b -= 1f / roundTime * Time.deltaTime;
        //    rends[i].material.color = c;
        //}
    }

    void StartMicrobes()
    {
        Debug.Log("Called StartMicrobes");

        if (needToSpawn)
        {
            Debug.Log("Needed to spawn microbes!");
            microbeText.text = "Microbe: " + (chromosomeInd + 1) + " of " + population.Length;

            if (microbeBuilder != null)
            {
                // TODO: Leaderboards need to be worked out for free for all
                //if (currentMicrobe != null)
                //{
                //    listScript.PlaceMicrobe(chromosomeInd, (generation + 1), GetFitness(currentMicrobe), population[chromosomeInd]);
                //}

                // Get the size of the population
                // Use this to workout the spacing between microbes at spawn time
                float xRows = Mathf.Ceil(Mathf.Sqrt(population.Length));
                float yRows = Mathf.Ceil(population.Length / xRows);
                Debug.Log("xRows: " + xRows);
                Debug.Log("yRows: " + yRows);
                // 160 is the width of the pool - giving 20 units each side = 140
                float width = 140f;
                float spacing = width / xRows;
                int ind = 0;
                Vector3 pos = new Vector3(-(width / 2), 8, -(width / 2));
                for (int y = 0; y < yRows; y++)
                {
                    for (int x = 0; x < xRows && ind < population.Length; x++)
                    {
                        currentMicrobes[ind] = microbeBuilder.CreateMicrobeAtPosition(population[ind], pos);
                        currentMicrobes[ind].GetComponent<MicrobeDataScript>().SetStartingPosition(pos);
                        pos = new Vector3(pos.x + spacing, pos.y, pos.z);
                        ind++;
                    }
                    pos = new Vector3(-(width / 2), pos.y, pos.z + spacing);
                }

                // currentMicrobe = microbeBuilder.CreateInitialMicrobe(population[chromosomeInd]);
                //Debug.Log("Starting microbe: " + microbeCount);
                // Start timer? after specified time, kill microbe and set fitness
                startTime = Time.time;


                chromosomeInd++;
                needToSpawn = false;
            }
        }
        else
        {
            Debug.Log("Did not need to spawn microbes");
        }
    }

    public void SetGenerationChromosomes(Chromosome[] chromosomes)
    {
        population = chromosomes;

        generation++;
        generationText.text = "Gen" + (generation+1);

        StartMicrobes();
    }

    private float GetFitness(GameObject microbe)
    {
        Vector3 start = microbe.GetComponent<MicrobeDataScript>().GetStartingPosition();
        Vector2 pos = new Vector2(microbe.transform.position.x - start.x, microbe.transform.position.z - start.z);

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
        else if (Input.GetKeyDown("down") && Time.timeScale > 0)
        {
            Time.timeScale -= 1;
            simSpeedText.text = "Sim Speed: " + Time.timeScale + "x";
        }
    }

    private void DestroyGeneration()
    {
        // Place microbes in leaderboard before destroying here
        for (int i = 0; i < currentMicrobes.Length; i++)
        {
            Destroy(currentMicrobes[i]);
        }
    }
}
