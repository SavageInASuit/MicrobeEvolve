import matplotlib.pyplot as plt
from random import random
import csv, os

def line_graph(path):
    plt.clf()
    max_distances = []
    trav_distances = []
    ave_distances = []
    min_distances = []
    gen = 0
    max_d = 0
    min_d = 9999999
    trav = 0
    ave = 0.0
    pop = 0.0
    g_name = ''
    with open(path) as csvfile:
        reader = csv.DictReader(csvfile)
        g_name = reader.fieldnames[len(reader.fieldnames) - 1]
        for row in reader:
            if int(row["generation"]) != gen:
                gen = int(row["generation"])
                max_distances += [max_d]
                trav_distances += [trav]
                ave_distances += [ave / pop]
                min_distances += [min_d]
                max_d = 0
                min_d = 9999999
                ave = 0
            if float(row["max_distance"]) >= max_d:
                max_d = float(row["max_distance"])
                trav = float(row["distance_travelled"])
            if float(row["max_distance"]) < min_d:
                min_d = float(row["max_distance"])
            ave += float(row["max_distance"])

            if gen == 0:
                pop += 1
            
    plt.plot(max_distances, label="Max Gen Distance")
    plt.plot(ave_distances, label="Average Gen Distance")
    plt.plot(min_distances, label="Minimum Gen Distance")
    plt.legend(loc="upper left")
    plt.title("Microbe Distances (Pop size = " + str(int(pop)) + ")")
    plt.ylabel("Distance")
    plt.xlabel("Generation")
    # plt.show()
    g_name += "_max-ave-min.png"
    g_name = ver_name(g_name, 0)
    plt.savefig("graphs/" + g_name, bbox_inches='tight', dpi=300)

def ver_name(filename, f_type):
    # type==0 is graph, 1 is log
    to_look = ''
    if f_type == 0:
        to_look = "graphs/"
    elif f_type == 1:
        to_look = "processed_logs/"
    
    ver = 0
    filename = filename[:-4]
    for f in os.listdir(to_look):
        if f.startswith(filename):
            ver += 1
    
    if ver > 0:
        if f_type == 0:
            return filename + ' (' + str(ver) + ').png'
        elif f_type == 1:
            return filename + ' (' + str(ver) + ').csv'
    else:
        ext = '.png' if (f_type == 0) else '.csv'
        return filename + ext

def main():
    print("The line_graph(path) method should be imported from this file, rather than ran")
    # cd /usr/local/bin/python3 


if __name__=="__main__":
    main()