using System.Collections;
using System.Collections.Generic;
using MicrobeApplication;
using UnityEngine;

public class FFAMicrobeEvolveScript : MonoBehaviour {
    /*
     * This script takes settings defined in the start instance menu then
     * populates, manages and evolves the microbe generations
     * 
     * General Idea:
     * Each microbe has its own chromosome which all follow the same format.
     * The chromosome will encode: 
     * hull_id, hull_scale, hull_mass, hull_buoyancy, component_1_id, component_1_vertex_id...
     * And so on for the number of components wanted (defined in init options)
     * 
     * When the scene first loads, a population should be generated at the
     * specified size (defined in init options)
     */
    // Probability of each bit in chromosome flipping
    private float mutationRate;
    // Should probably have a min value of 3 for the algorithm to work? 
    private int populationSize;    // Size of the population throughout the evolution process

    Chromosome[] population;

    FFAPopulationManagerScript popManager;

    public void GenerateInitialPopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Chromosome chromosome = Chromosome.RandomChromosome();
            population[i] = chromosome;
        }

        popManager.SetGenerationChromosomes(population);
    }

    // Use this for initialization
    void Start()
    {
        popManager = GetComponent<FFAPopulationManagerScript>();

        populationSize = InstanceData.PopulationSize;
        mutationRate = InstanceData.MutationRate;

        population = new Chromosome[populationSize];
        GenerateInitialPopulation();
    }

    public void EvolveNextGeneration()
    {
        Chromosome[] nextGen = new Chromosome[populationSize];

        // Find the max to use for normalisation when selecting
        float maxFitness = 0;
        foreach (Chromosome c in population)
        {
            if (c.Fitness > maxFitness)
                maxFitness = c.Fitness;
        }

        // Build up the next generation using roulette wheel selection
        // More likely to select chromosomes with higher fitness values
        int indFirst = Random.Range(0, populationSize);
        int indSecond = Random.Range(0, populationSize);
        for (int i = 0; i < populationSize; i++)
        {
            // Find the first parent
            float r = Random.value;
            //Debug.Log("first r value: " + r);
            //foreach (Chromosome c in population)
            //{
            //    Debug.Log(c.Fitness / maxFitness);
            //}

            float cur = 0;
            while (cur <= r)
            {
                // Add the normalised value
                cur += population[indFirst].Fitness / maxFitness;
                indFirst++;
                indFirst %= populationSize;
            }
            indFirst -= 1;
            if (indFirst == -1) indFirst = populationSize - 1;
            Chromosome first = population[indFirst];

            // Find the second parent
            r = Random.value;
            cur = 0;
            while (cur <= r)
            {
                if (indSecond == indFirst)
                    indSecond++;
                indSecond %= populationSize;
                // Add the normalised value
                cur += population[indSecond].Fitness / maxFitness;
                indSecond++;
            }
            indSecond -= 1;
            if (indSecond == -1) indSecond = populationSize - 1;
            Chromosome second = population[indSecond];

            Chromosome child = Chromosome.Crossover(first, second);

            if (InstanceData.SingleMutate)
                child = Chromosome.MutateSingle(child, mutationRate);
            else
                child = Chromosome.Mutate(child, mutationRate);

            child.SetParents(indFirst, indSecond);
            nextGen[i] = child;
        }

        population = nextGen;

        popManager.SetGenerationChromosomes(population);
    }
}
