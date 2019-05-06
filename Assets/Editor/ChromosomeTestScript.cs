using NUnit.Framework;
using MicrobeApplication;
using System;
using UnityEngine;

namespace Tests
{
    public class ChromosomeTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ChromosomeStringInstanceEqual()
        {
            string chromString = "000010000010000010000101011001001001001010101001011110101101100110011011010100101101100100011111101101001100001100110001000001101111011100000100110101011011001100011000001001001110010100001100111100001000110110010010101001001101001110";
            // Use the Assert class to test conditions
            Chromosome chromosome = new Chromosome(chromString);
            Assert.AreEqual(chromosome.ChromosomeString, chromString, "Chromosome string length equal to string passed in");
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ChromosomeBitsEqualLength()
        {
            int totalBits = Chromosome.HULL_ID_BITS + Chromosome.HULL_SCALE_BITS + 
                        Chromosome.HULL_MASS_BITS + Chromosome.HULL_BUOYANCY_BITS + 
                        Chromosome.COMPONENT_COUNT_BITS + 
                        (Chromosome.COMPONENT_ID_BITS + Chromosome.COMPONENT_MESHV_BITS + 
                        Chromosome.COMPONENT_SCALE_BITS + (Chromosome.COMPONENT_ROTATION_BITS * 3)) *
                        ((int)Math.Pow(2, Chromosome.COMPONENT_COUNT_BITS) - 1);


            Assert.AreEqual(Chromosome.CHROMOSOME_LENGTH, totalBits, "Chromosome string length not equal to sum of defined bit counts");
        }

        [Test]
        public void ChromosomeStringSameLengthAfterMutation()
        {
            string chromString = "000010000010000010000101011001001001001010101001011110101101100110011011010100101101100100011111101101001100001100110001000001101111011100000100110101011011001100011000001001001110010100001100111100001000110110010010101001001101001110";
            // Use the Assert class to test conditions
            Chromosome chromosome = new Chromosome(chromString);
            chromosome = Chromosome.Mutate(chromosome, 0.05f);
            Assert.AreEqual(chromosome.ChromosomeString.Length, chromString.Length, "Chromosome string length not the same after mutation");
        }

        [Test]
        public void ChromosomeParsingAllZeros()
        {
            string chromString = "";
            for (int i = 0; i < Chromosome.CHROMOSOME_LENGTH; i++)
                chromString += '0';
            // Use the Assert class to test conditions
            Chromosome chromosome = new Chromosome(chromString);

            Assert.AreEqual(chromosome.HullId, 0, "Hull ID non-zero");
            // Hull scale is 10/7 even when all bits are zero - scale of 0
            Assert.AreEqual(chromosome.HullScale, 10f / 7f, "Hull scale non-zero");
            // Hull mass is 1/7 even when all bits are zero - prevents 0 mass objects
            Assert.AreEqual(chromosome.HullMass, 1f / 7f, "Hull mass non-zero");
            Assert.AreEqual(chromosome.HullBuoyancy, 0, "Hull buoyancy non-zero");
            Assert.AreEqual(chromosome.ComponentCount, 0, "Component count non-zero");
            // there is no component data to test...
        }

        [Test]
        public void ChromosomeParsingAllOnes()
        {
            string chromString = "";
            for (int i = 0; i < Chromosome.CHROMOSOME_LENGTH; i++)
                chromString += '1';
            // Use the Assert class to test conditions
            Chromosome chromosome = new Chromosome(chromString);

            Assert.AreEqual(chromosome.HullId, (float) Math.Pow(2, Chromosome.HULL_ID_BITS) - 1f, "Hull ID incorrect considering bit count");
            Assert.AreEqual(chromosome.HullScale, (float) (Math.Pow(2, Chromosome.HULL_SCALE_BITS) + 9f) / 7f, "Hull scale incorrect considering bit count");
            Assert.AreEqual(chromosome.HullMass, (float)Math.Pow(2, Chromosome.HULL_MASS_BITS) / 7f, "Hull mass incorrect considering bit count");
            Assert.AreEqual(chromosome.HullBuoyancy, (float) (Math.Pow(2, Chromosome.HULL_BUOYANCY_BITS) - 1f) / 5f, "Hull buoyancy incorrect considering bit count");
            float componentCount = (float)Math.Pow(2, Chromosome.COMPONENT_COUNT_BITS) - 1f;
            Assert.AreEqual(chromosome.ComponentCount, componentCount, "Component count incorrect considering bit count");
            ComponentData[] components = chromosome.GetComponents();
            for (int i = 0; i < componentCount; i++)
            {
                ComponentData cd = components[i];
                Assert.AreEqual(cd.Id, (float)Math.Pow(2, Chromosome.COMPONENT_ID_BITS) - 1f, "Component ID incorrect considering bit count");
                Assert.AreEqual(cd.MeshVertex, (float)Math.Pow(2, Chromosome.COMPONENT_MESHV_BITS) - 1f, "Component mesh vertex incorrect considering bit count");
                Assert.AreEqual(cd.Scale, (float)Math.Pow(2, Chromosome.COMPONENT_SCALE_BITS) / 5f, "Component scale incorrect considering bit count");
                Vector3 rot = cd.Rotation;
                Assert.AreEqual(rot.x, ((float)Math.Pow(2, Chromosome.COMPONENT_ROTATION_BITS) - 1f) % 360, "Component rot.x incorrect considering bit count");
                Assert.AreEqual(rot.x, rot.y, "Component rot.x not equal to rot.y");
                Assert.AreEqual(rot.x, rot.z, "Component rot.x not equal to rot.z");
            }
        }

        [Test]
        public void ChromosomeIsValidChromosome()
        {
            Assert.IsFalse(Chromosome.IsValidChromosome(""), "Blank string accepted as valid");
            Assert.IsFalse(Chromosome.IsValidChromosome("abcdefghijklmnopqrstuvwxyz"), "String containing alpabetic characters accepted");
            Assert.IsFalse(Chromosome.IsValidChromosome("10101010101010101010101001"), "String of incorrect length accepted");

            string chromString = "";
            for (int i = 0; i < Chromosome.CHROMOSOME_LENGTH; i++)
            {
                chromString += (UnityEngine.Random.value < 0.5) ? "1" : "0";
            }

            Assert.IsTrue(Chromosome.IsValidChromosome(chromString), "String of correct length not accepted");
        }
    }
}