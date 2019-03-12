import matplotlib.pyplot as plt
from random import random
import csv

# line1 = plt.plot([i for i in range(10)], [random() * 10 for _ in range(10)], label="First things")
# line2 = plt.plot([i for i in range(10)], [random() * 10 for _ in range(10)], label="Other things")
max_distances = []
trav_distances = []
ave_distances = []
min_distances = []
with open("data.csv") as csvfile:
    reader = csv.DictReader(csvfile)
    gen = 0
    max_d = 0
    min_d = 9999999
    trav = 0
    ave = 0.0
    pop = 0.0
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
# plt.plot(trav_distances, label="Travelled Distance")
plt.plot(ave_distances, label="Average Gen Distance")
plt.plot(min_distances, label="Minimum Gen Distance")
plt.legend(loc="upper left")
plt.title("Microbe Distances (Pop size = " + str(int(pop)) + ")")
plt.ylabel("Distance")
plt.xlabel("Generation")
plt.show()