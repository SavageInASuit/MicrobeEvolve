using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Application;

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
            Assert.AreEqual(chromosome.ChromosomeString, chromString);
        }

        [Test]
        public void ChromosomeStringSameLengthAfterMutation()
        {
            string chromString = "000010000010000010000101011001001001001010101001011110101101100110011011010100101101100100011111101101001100001100110001000001101111011100000100110101011011001100011000001001001110010100001100111100001000110110010010101001001101001110";
            // Use the Assert class to test conditions
            Chromosome chromosome = new Chromosome(chromString);
            chromosome = Chromosome.Mutate(chromosome, 0.05f);
            Assert.AreEqual(chromosome.ChromosomeString.Length, chromString.Length);
        }

        [Test]
        public void ChromosomeCorrectParsing()
        {
            string chromString = "";
            for (int i = 0; i < Chromosome.CHROMOSOME_LENGTH; i++)
                chromString += '0';
            // Use the Assert class to test conditions
            Chromosome chromosome = new Chromosome(chromString);

            Assert.AreEqual(chromosome.HullId, 0);
            Assert.AreEqual(chromosome.HullId, 0);
            Assert.AreEqual(chromosome.HullMass, 0);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ChromosomeTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
