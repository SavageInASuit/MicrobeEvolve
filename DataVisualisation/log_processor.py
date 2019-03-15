from graphing import line_graph, ver_name
from scatter import scatter_graph
import sched, time, os

# graph file name format
#   - pop-n_ = population size n
#   - mutation-method-mutation-rate_ = how chromosomes were evolved single/random bits at specified rate
#   - graph-type = max-ave-min shows max, average, and minimum for each gen
#                  scatter shows a point for the max dist for each microbe in each gen + 
#                                a line linking the max distances for each gen

def process_logs(log_path):
    to_process = []

    if not log_path.endswith('/'):
        log_path += '/'
    
    for f in os.listdir(log_path):
        if f.endswith('.csv'):
            to_process.append(f)

    print(str(len(to_process)) + " logs to process")
    
    ind = 0
    for f in to_process:
        print("processing log " + str(ind) + ": " + f)
        os.rename(log_path + f, f)
        line_graph(f)
        scatter_graph(f)
        scatter_graph(f, True)
        name = ver_name(f, 1)
        os.rename(f, 'processed_logs/' + name)
        ind += 1


def sched_process_logs(scheder):
    print("scheduling process logs")
    log_path = "../logs"
    
    process_logs(log_path)

    scheder.enter(10, 1, process_logs, (scheder,))

def main():
    # TODO: Loop every 5 minutes and process any new log files
    print("starting scheduler")
    s = sched.scheduler(time.time, time.sleep)
    s.enter(10, 1, sched_process_logs, (s,))
    s.run()

if __name__ == "__main__":
    main()