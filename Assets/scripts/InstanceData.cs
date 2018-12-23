// Static class to hold the options the user selects in the new instance menu!
public static class InstanceData {
    private static int populationSize = 20;
    private static float mutationRate = 0.05f;

    public static float MutationRate
    {
        get
        {
            return mutationRate;
        }

        set
        {
            mutationRate = value;
        }
    }

    public static int PopulationSize
    {
        get
        {
            return populationSize;
        }

        set
        {
            populationSize = value;
        }
    }
}
