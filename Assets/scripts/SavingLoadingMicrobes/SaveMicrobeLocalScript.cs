using System.IO;
using UnityEngine;

namespace MicrobeApplication
{
    public static class SaveMicrobeLocalScript
    {
        public static void SaveMicrobe(string fileName, string chromosomeString)
        {
            string savePath = Path.Combine(Application.persistentDataPath, fileName + ".txt");
            // Check whether a file has already been saved with this name, if so,
            // increment version
            bool fileExists = File.Exists(savePath);
            int ver = 0;
            while (fileExists)
            {
                ver++;
                savePath = Path.Combine(Application.persistentDataPath, fileName +
                           " (" + ver + ").txt");
                fileExists = File.Exists(savePath);
            }

            Debug.Log("Saved in: " + savePath);

            using (StreamWriter sw = File.CreateText(savePath))
            {
                sw.Write(chromosomeString);
                sw.Flush();
            }
        }
    }
}