import matplotlib.pyplot as plt
import numpy as np
from graphing import ver_name

import csv

def scatter_graph(path, trend=False):
    plt.clf()
    distances = []
    max_distances = []
    gens = []
    pop = 0.0
    max_d = 0
    gen = 0
    overall_max = 0
    g_name = ''
    with open(path) as csvfile:
        reader = csv.DictReader(csvfile)
        g_name = reader.fieldnames[len(reader.fieldnames) - 1]
        for row in reader:
            if int(row["generation"]) != gen:
                gen = int(row["generation"])
                max_distances += [max_d]
                if max_d > overall_max:
                    overall_max = max_d
                max_d = 0
            
            if float(row["max_distance"]) > max_d:
                max_d = float(row["max_distance"])

            distances += [float(row["max_distance"])]
            gens += [int(row["generation"])]
        
            if int(row["generation"]) == 0:
                pop += 1
        
        max_distances += [max_d]
        if max_d > overall_max:
            overall_max = max_d
            
    # Scatter plot
    plt.scatter(gens, distances, s=4, c=[[0,0,0,0.2]], label="Individual Max Distances")

    if trend:
        # trend line for scatter
        z = np.polyfit(gens, distances, 1)
        p = np.poly1d(z)
        plt.plot(gens,p(gens),"r--", label="Overall Trend")
        g_name += "_scatter-trend.png"
    else:
        # Line connecting max dists for each gen
        plt.plot(max_distances, label="Max Distance per Generation")
        g_name += "_scatter-max.png"


    plt.ylim(top=overall_max + 10)
    plt.legend(loc="upper left")
    plt.title("Microbe Distances (Pop size = " + str(int(pop)) + ")")
    plt.ylabel("Distance")
    plt.xlabel("Generation")

    g_name = ver_name(g_name, 0)
    plt.savefig("graphs/" + g_name, bbox_inches='tight', dpi=300)

def main():
    # print("The scatter_graph(path) method should be imported from this file, rather than ran")
    scatter_graph("../logs/pop-50_single-bit-0-15_500", True)
    


if __name__=="__main__":
    main()