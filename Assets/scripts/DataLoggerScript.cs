using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataLoggerScript : MonoBehaviour
{
    private readonly string filePathPrefix = "logs/microbe-data_";
    private string fullPath;
    private readonly string columnTitles = "generation,microbe,max_distance,distance_travelled,chromosome";

    private List<string[]> data;

    private bool fileExists;

    // Start is called before the first frame update
    void Start()
    {
        data = new List<string[]>();

        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
        fullPath = filePathPrefix + ts.TotalSeconds + ".txt";

        fileExists = File.Exists(fullPath);
    }

    public void AddData(string gen, string ind, string max_dist, string dist_travelled, string chromosomeString)
    {
        data.Add(new string[]{ gen, ind, max_dist, dist_travelled, chromosomeString});
    }

    private void OnApplicationQuit()
    {
        using (StreamWriter sw = fileExists ? new StreamWriter(fullPath) : File.CreateText(fullPath))
        {
            sw.WriteLine(columnTitles);
            foreach (string[] entry in data)
            {
                sw.WriteLine("{0},{1},{2},{3},{4}", entry);
                sw.Flush();
            }
        }
    }
}
