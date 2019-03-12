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
    private TextMeshProUGUI lifeTimeText;
    [SerializeField]
    private GameObject pausedText;


    [SerializeField]
    private float roundTime;

    [SerializeField]
    private GraphScript graph;

    private int generation;
    private int chromosomeInd;

    private float startTime;

    private float distLimit;

    private float maxGenDist;

    private float simSpeed = 1;
    private bool paused;

    private Vector2 prevPos;
    private Vector2 curPos;
    private float distTravelled;

    private Chromosome[] population;
    private GameObject currentMicrobe;

    MicrobeBuilderScript microbeBuilder;
    MicrobeEvolveScript microbeEvolver;
    DataLoggerScript dataLogger;

    public ScoreListScript listScript;

    // Use this for initialization
    void Start () {
        microbeBuilder = GetComponent<MicrobeBuilderScript>();
        microbeEvolver = GetComponent<MicrobeEvolveScript>();
        dataLogger = GetComponent<DataLoggerScript>();

        // Limit on distance travelled for fitness is the distance from the 
        // center point to the corner, otherwise microbe is out of pool
        distLimit = Mathf.Sqrt(Mathf.Pow(InstanceData.PoolScale * 10f, 2f) + Mathf.Pow(InstanceData.PoolScale * 10f, 2f)) / 2f;

        generation = 0;
        chromosomeInd = -1;

        roundTime = InstanceData.GenerationTime;
        Time.timeScale = 1f;

        generationText.text = "Gen: " + (generation + 1);
    }

    // Update is called once per frame
    void Update() {
        float curTime = Time.time - startTime;
        lifeTimeText.text = "Time: " + curTime.ToString("n1") + "/" + roundTime.ToString() + "s";
        if (currentMicrobe != null && curTime > roundTime)
        {
            population[chromosomeInd].Fitness = GetFitness(currentMicrobe);
            if (chromosomeInd == 0)
                graph.AddPoint(population[chromosomeInd].Fitness);
            if (population[chromosomeInd].Fitness > maxGenDist)
            {
                maxGenDist = population[chromosomeInd].Fitness;
                graph.ChangeLastPoint(maxGenDist);
            }

            float fitness = GetFitness(currentMicrobe);
            listScript.PlaceMicrobe(chromosomeInd + 1, generation + 1, fitness, population[chromosomeInd]);
            dataLogger.AddData((generation + 1).ToString(), (chromosomeInd + 1).ToString(), fitness.ToString(), distTravelled.ToString(), population[chromosomeInd].ChromosomeString);
            
            Destroy(currentMicrobe);
            // If we have processed the final microbe, we can start the next gen
            if (chromosomeInd == population.Length - 1)
            {
                graph.ChangeLastPoint(maxGenDist);
                maxGenDist = 0f;
                // exit from function and start the evolution process
                microbeEvolver.EvolveNextGeneration();

                return;
            }

            distTravelled = 0f;
            StartMicrobe();

        }
        else if (currentMicrobe == null && microbeBuilder != null)
        {
            StartMicrobe();
        }

        if (currentMicrobe != null)
        {
            distanceText.text = "Distance: " + GetFitness(currentMicrobe).ToString("n2") + "m";

            curPos = new Vector2(currentMicrobe.transform.position.x, currentMicrobe.transform.position.z);
            if (prevPos != null)
            {
                distTravelled += Vector2.Distance(prevPos, curPos);
            }
            prevPos = curPos;

            MeshRenderer[] rends = currentMicrobe.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < rends.Length; i++)
            {
                Color c = rends[i].material.color;
                c.g -= 1f / roundTime * Time.deltaTime;
                c.b -= 1f / roundTime * Time.deltaTime;
                rends[i].material.color = c;
            }
        }

        CheckSpeed();
    }

    void StartMicrobe()
    {
        if (microbeBuilder != null)
        {
            chromosomeInd++;
            microbeText.text = "Microbe: " + (chromosomeInd + 1) + " of " + population.Length;

            if (chromosomeInd < population.Length)
            {
                currentMicrobe = microbeBuilder.CreateInitialMicrobe(population[chromosomeInd]);
                currentMicrobe.GetComponent<MicrobeDataScript>().SetStartingPosition(new Vector3(0, 0, 0));
                //Debug.Log("Starting microbe: " + microbeCount);
                // Start timer? after specified time, kill microbe and set fitness
                startTime = Time.time;
            }

        }
    }

    public void SetGenerationChromosomes(Chromosome[] chromosomes)
    {
        population = chromosomes;

        generation++;
        chromosomeInd = -1;
        generationText.text = "Gen: " + (generation + 1);

        StartMicrobe();
    }

    private float GetFitness(GameObject microbe)
    {
        Vector2 pos = new Vector2(microbe.transform.position.x, microbe.transform.position.z);

        if(pos.magnitude > distLimit)
        {
            return 0.1f;
        }

        return pos.magnitude;
    }

    private void CheckSpeed()
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown("space"))
            {
                simSpeed = 1;
            }
            else if (Input.GetKeyDown("up"))
            {
                simSpeed += 1;
            }
            else if (Input.GetKeyDown("down") && Time.timeScale > 1)
            {
                simSpeed -= 1;
            }else if (Input.GetKeyDown(KeyCode.P))
            {
                if (Time.timeScale == 0) {
                    Time.timeScale = simSpeed;
                    paused = false;
                }
                else
                {
                    Time.timeScale = 0;
                    paused = true;
                }

                pausedText.SetActive(paused);
            }

            if (!paused)
                Time.timeScale = simSpeed;

            simSpeedText.text = "Sim Speed: " + Time.timeScale + "x";
        }
    }
}
