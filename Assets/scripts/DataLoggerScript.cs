﻿using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataLoggerScript : MonoBehaviour
{
    private readonly string filePathPrefix = "logs/";
    private string fullPath;
    private string columnTitles = "generation,microbe,max_distance,distance_travelled,parent1,parent2,chromosome";

    private List<string[]> data;

    private bool fileExists;

    // Start is called before the first frame update
    void Start()
    {
        data = new List<string[]>();
        InitialiseLogger();
    }

    public void InitialiseLogger()
    {
        data.Clear();

        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);

        string muteType = InstanceData.SingleMutate ? "single-bit" : "random-bits";
        string muteRate = InstanceData.MutationRate.ToString().Replace('.', '-');
        muteType += '-' + muteRate;
        string poolSize = (InstanceData.PoolScale * 10).ToString();
        string pathPostfix = "pop-" + InstanceData.PopulationSize +
                             "_" + muteType +
                             "_" + poolSize;
        columnTitles += "," + pathPostfix;

        fullPath = filePathPrefix +
                   pathPostfix + "_" +
                   ".csv";

        fileExists = File.Exists(fullPath);
        int ver = 0;
        // if this log file already exists, increment the version num
        while (fileExists)
        {
            ver++;
            fullPath = filePathPrefix +
                       pathPostfix + "_" +
                       "(" + ver + ").csv";
            fileExists = File.Exists(fullPath);
        }
    }

    public void AddData(string gen, string ind, string max_dist, string dist_travelled, string chromosomeString, string p1, string p2)
    {
        data.Add(new string[]{ gen, ind, max_dist, dist_travelled, p1, p2, chromosomeString});
    }

    public void WriteLog()
    {
        using (StreamWriter sw = File.CreateText(fullPath))
        {
            sw.WriteLine(columnTitles);
            foreach (string[] entry in data)
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", entry);
                sw.Flush();
            }
        }
    }

    //private void OnApplicationQuit()
    //{
    //    WriteLog();
    //}
}
