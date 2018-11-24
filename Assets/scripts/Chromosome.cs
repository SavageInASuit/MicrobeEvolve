using UnityEngine;
using System;

namespace Application
{
    public class Chromosome
    {
        private string chromosomeString;
        private int hullId, hullScale, hullMass, hullBuoyancy;
        private int componentCount;
        private ComponentData[] componentData;

        private const int HULL_ID_BITS = 3;
        private const int HULL_SCALE_BITS = 4;
        private const int HULL_MASS_BITS = 4;
        private const int HULL_BUOYANCY_BITS = 4;
        private const int COMPONENT_COUNT_BITS = 4; // 16 options
        private const int COMPONENT_ID_BITS = 4; // 16 options
        private const int COMPONENT_MESHV_BITS = 10; // 1024 options
        private const int COMPONENT_SCALE_BITS = 4; // 16 options 0.25, 0.5, 0.75, 1.0...
        private const int COMPONENT_ROTATION_BITS = 9; // x: 0-359, y: 0-359, z: 0-359

        private static readonly int CHROMOSOME_LENGTH = HULL_ID_BITS +
                                                        HULL_SCALE_BITS +
                                                        HULL_MASS_BITS +
                                                        HULL_BUOYANCY_BITS +
                                                        COMPONENT_COUNT_BITS +
                                                        ((COMPONENT_ID_BITS +
                                                        COMPONENT_MESHV_BITS +
                                                        COMPONENT_SCALE_BITS +
                                                          COMPONENT_ROTATION_BITS) * (int)Math.Pow(2, COMPONENT_COUNT_BITS));

        public Chromosome(string chromosome)
        {
            this.chromosomeString = chromosome;
            ParseChromosome();
        }

        public static Chromosome RandomChromosome(){
            string chromosome = "";
            for (int i = 0; i < CHROMOSOME_LENGTH; i++){
                chromosome += UnityEngine.Random.value >= 0.5 ? '1' : '0';
            }
            return new Chromosome(chromosome);
        }

        private void ParseChromosome(){
            int cur = 0;
            hullId = BinToDec(chromosomeString.Substring(0, HULL_ID_BITS));
            cur += HULL_ID_BITS;
            hullScale = BinToDec(chromosomeString.Substring(cur, HULL_SCALE_BITS));
            cur += HULL_SCALE_BITS;
            hullMass = BinToDec(chromosomeString.Substring(cur, HULL_MASS_BITS));
            cur += HULL_MASS_BITS;
            hullBuoyancy = BinToDec(chromosomeString.Substring(cur, HULL_BUOYANCY_BITS));
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
                int componentScale = BinToDec(chromosomeString.Substring(cur, COMPONENT_SCALE_BITS));
                cur += COMPONENT_SCALE_BITS;
                Vector3 rotation = new Vector3(BinToDec(chromosomeString.Substring(cur, COMPONENT_ROTATION_BITS)) % 360,
                                               BinToDec(chromosomeString.Substring(cur+COMPONENT_ROTATION_BITS, COMPONENT_ROTATION_BITS)) % 360,
                                               BinToDec(chromosomeString.Substring(cur+(COMPONENT_ROTATION_BITS*2), COMPONENT_ROTATION_BITS)) % 360);
                componentData[i] = new ComponentData(componentId, componentMeshV, componentScale, rotation);
            }
        }

        private int BinToDec(string gene){
            return (int)Convert.ToUInt32(gene, 2);
        }

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

        public int HullScale
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

        public int HullMass
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

        public int HullBuoyancy
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
    }
}