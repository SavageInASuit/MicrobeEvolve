// Static class to hold the options the user selects in the new instance menu!
public static class InstanceData {
    private static int populationSize = 30;
    private static float mutationRate = 0.15f;
    private static int generationTime = 10;
    private static string chromosomeString = "";
    private static float poolScale = 20f;
    private static bool singleMutate = true;

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

    public static int GenerationTime
    {
        get
        {
            return generationTime;
        }
        set
        {
            generationTime = value;
        }
    }

    public static string ChromosomeString
    {
        get
        {
            return chromosomeString;
        }
        set
        {
            chromosomeString = value;
        }
    }

    public static float PoolScale
    {
        get
        {
            return poolScale;
        }
        set
        {
            poolScale = value;
        }
    }

    public static bool SingleMutate
    {
        get
        {
            return singleMutate;
        }
        set
        {
            singleMutate = value;
        }
    }
}
