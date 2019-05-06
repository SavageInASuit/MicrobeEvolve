using UnityEngine;
using System;
using System.Collections.Generic;

namespace MicrobeApplication
{
    public class Chromosome
    {
        private string chromosomeString;
        private int hullId;
        private float hullScale, hullMass, hullBuoyancy;
        private int componentCount;
        private float fitness = 0f;
        private string[] parents;
        private ComponentData[] componentData;

        public const int HULL_ID_BITS = 3;
        public const int HULL_SCALE_BITS = 4;
        public const int HULL_MASS_BITS = 4;
        public const int HULL_BUOYANCY_BITS = 4;
        public const int COMPONENT_COUNT_BITS = 3; // 8 options 0-7
        public const int COMPONENT_ID_BITS = 4; // 16 options
        public const int COMPONENT_MESHV_BITS = 10; // 1024 options
        public const int COMPONENT_SCALE_BITS = 4; // 16 options 1/16, 1/16, 2/16...
        public const int COMPONENT_ROTATION_BITS = 9; // x: 0-359, y: 0-359, z: 0-359
        public const int TOTAL_HULL_BITS = HULL_ID_BITS + HULL_SCALE_BITS + HULL_MASS_BITS + HULL_BUOYANCY_BITS + COMPONENT_COUNT_BITS; // 18
        public const int TOTAL_COMPONENT_BITS = COMPONENT_ID_BITS + COMPONENT_MESHV_BITS + COMPONENT_SCALE_BITS + (COMPONENT_ROTATION_BITS * 3); // 45

        public static readonly int CHROMOSOME_LENGTH = TOTAL_HULL_BITS +
                                                        (TOTAL_COMPONENT_BITS * ((int)Math.Pow(2, COMPONENT_COUNT_BITS) - 1));

        public Chromosome(string chromosome)
        {
            // provided chromosome should be the correct length
            // Debug.Assert(chromosome.Length == CHROMOSOME_LENGTH, "Passed string is not the correct length for a chromosome string");

            chromosomeString = chromosome;
            ParseChromosome();
        }

        public static Chromosome RandomChromosome(){
            string chromosome = "";
            for (int i = 0; i < CHROMOSOME_LENGTH; i++){
                chromosome += UnityEngine.Random.value >= 0.5 ? '1' : '0';
            }
            return new Chromosome(chromosome);
        }

        // Flip bits in the passed chromosome using the mutationRate as a probability
        public static Chromosome Mutate(Chromosome toMutate, float mutationRate){
            string newChrom = "";
            char[] oldChrom = toMutate.ChromosomeString.ToCharArray();

            for (int i = 0; i < oldChrom.Length; i++){
                char curChar = oldChrom[i];
                if(UnityEngine.Random.value < mutationRate){
                    if (curChar == '1')
                        newChrom += '0';
                    else
                        newChrom += '1';
                }else{
                    newChrom += curChar;
                }
            }

            return new Chromosome(newChrom);
        }

        public void SetParents(int first, int second)
        {
            parents = new string[] { first.ToString(), second.ToString() };
        }

        public string[] GetParents()
        {
            if (parents == null)
                return new string[] { "-1", "-1" };

            return parents;
        }

        // Flip bits in the passed chromosome using the mutationRate as a probability
        public static Chromosome MutateSingle(Chromosome toMutate, float mutationRate)
        {
            char[] oldChrom = toMutate.ChromosomeString.ToCharArray();

            if (UnityEngine.Random.value < mutationRate)
            {
                float activeBits = TOTAL_HULL_BITS + (toMutate.componentCount * TOTAL_COMPONENT_BITS);
                int charInd = (int) Mathf.Floor(UnityEngine.Random.value * activeBits);
                if (oldChrom[charInd] == '1')
                    oldChrom[charInd] = '0';
                else
                    oldChrom[charInd] = '1';
            }

            return new Chromosome(new string(oldChrom));
        }

        public static Chromosome Crossover(Chromosome a, Chromosome b){
            string newChrom = "";
            string chromA = a.ChromosomeString;
            string chromB = b.ChromosomeString;

            // Chromosomes should be the same length
            Debug.Assert(chromA.Length == chromB.Length);

            int crossInd = (int)Math.Round(UnityEngine.Random.value * chromA.Length);
            newChrom += chromA.Substring(0, crossInd);
            newChrom += chromB.Substring(crossInd, chromB.Length - crossInd);

            return new Chromosome(newChrom);
        }

        private void ParseChromosome(){
            int cur = 0;
            hullId = BinToDec(chromosomeString.Substring(0, HULL_ID_BITS));
            cur += HULL_ID_BITS;
            hullScale = (BinToDec(chromosomeString.Substring(cur, HULL_SCALE_BITS)) + 10) / 7.0f;
            cur += HULL_SCALE_BITS;
            hullMass = (BinToDec(chromosomeString.Substring(cur, HULL_MASS_BITS)) + 1) / 7.0f;
            cur += HULL_MASS_BITS;
            hullBuoyancy = BinToDec(chromosomeString.Substring(cur, HULL_BUOYANCY_BITS)) / 5.0f;
            cur += HULL_BUOYANCY_BITS;

            componentCount = BinToDec(chromosomeString.Substring(cur, COMPONENT_COUNT_BITS));
            cur += COMPONENT_COUNT_BITS;

            componentData = new ComponentData[componentCount];
            // Parse the right amount of data for the defined component count and add to array
            for (int i = 0; i < componentCount; i++)
            {
                int componentId = BinToDec(chromosomeString.Substring(cur, COMPONENT_ID_BITS));
                cur += COMPONENT_ID_BITS;
                int componentMeshV = BinToDec(chromosomeString.Substring(cur, COMPONENT_MESHV_BITS));
                cur += COMPONENT_MESHV_BITS;
                float componentScale = (BinToDec(chromosomeString.Substring(cur, COMPONENT_SCALE_BITS)) + 1) / 5.0f;
                cur += COMPONENT_SCALE_BITS;
                Vector3 rotation = new Vector3(BinToDec(chromosomeString.Substring(cur, COMPONENT_ROTATION_BITS)) % 360,
                                               BinToDec(chromosomeString.Substring(cur+COMPONENT_ROTATION_BITS, COMPONENT_ROTATION_BITS)) % 360,
                                               BinToDec(chromosomeString.Substring(cur+(COMPONENT_ROTATION_BITS*2), COMPONENT_ROTATION_BITS)) % 360);
                componentData[i] = new ComponentData(componentId, componentMeshV, componentScale, rotation);
            }
        }

        public ComponentData[] GetComponents()
        {
            return componentData;
        }

        public string GetChromosomeAsHex()
        {
            string output = "";
            for(int ind = 0; ind < chromosomeString.Length; ind += 4)
            {
                int h_val = 0;
                for(int off = 0; off < 4; off++)
                {
                    if (ind + off < chromosomeString.Length)
                        h_val += (int)Math.Pow(2, off) * (int)char.GetNumericValue(chromosomeString[ind + off]);
                }
                output += h_val.ToString("X");
            }
            return output;
        }

        public static bool IsValidChromosome(string chromosomeString)
        {
            // Check chromosome is the correct length
            if (chromosomeString.Length != CHROMOSOME_LENGTH)
            {
                return false;
            }
            // Check chromosome only contains 0s and 1s
            int count = 0;
            foreach (char c in chromosomeString)
            {
                if (c == '0' || c == '1')
                    count++;
            }

            return count == CHROMOSOME_LENGTH;
        }

        private int BinToDec(string gene){
            return (int)Convert.ToUInt32(gene, 2);
        }

        // ------------ Not sure if all of this is needed... or if they should just be public members?
        public int HullId
        {
            get
            {
                return hullId;
            }

            set
            {
                hullId = value;
            }
        }

        public float HullScale
        {
            get
            {
                return hullScale;
            }

            set
            {
                hullScale = value;
            }
        }

        public float HullMass
        {
            get
            {
                return hullMass;
            }

            set
            {
                hullMass = value;
            }
        }

        public float HullBuoyancy
        {
            get
            {
                return hullBuoyancy;
            }

            set
            {
                hullBuoyancy = value;
            }
        }

        public int ComponentCount
        {
            get
            {
                return componentCount;
            }

            set
            {
                componentCount = value;
            }
        }

        public ComponentData[] ComponentData
        {
            get
            {
                return componentData;
            }

            set
            {
                componentData = value;
            }
        }

        public string ChromosomeString
        {
            get
            {
                return chromosomeString;
            }

            set
            {
                chromosomeString = value;
            }
        }

        public float Fitness
        {
            get
            {
                return fitness;
            }

            set
            {
                fitness = value;
            }
        }
    }
}