// Static class to hold the options the user selects in the new instance menu!
public static class InstanceData {
    private static int populationSize = 10;
    private static float mutationRate = 0.15f;
    private static int generationTime = 10;
    private static string chromosomeString = "000010000010000010000101011001001001001010101001011110101101100110011011010100101101100100011111101101001100001100110001000001101111011100000100110101011011001100011000001001001110010100001100111100001000110110010010101001001101001110";
    private static float poolScale = 50f;
    private static bool singleMutate = false;
    private static float boosterForce = 300f;
    private static bool dataCollectionMode = false;
    private static int runsCompleted = 0;
    private static int runs = 5;
    private static int runGenerations = 5;
    private static float simSpeed = 1f;
    private static bool ffaMode = false;

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

    public static float BoosterForce
    {
        get
        {
            return boosterForce;
        }
        set
        {
            boosterForce = value;
        }
    }

    public static bool DataCollectionMode
    {
        get
        {
            return dataCollectionMode;
        }

        set
        {
            dataCollectionMode = value;
        }
    }

    public static int RunGenerations
    {
        get
        {
            return runGenerations;
        }

        set
        {
            runGenerations = value;
        }
    }

    public static int RunsCompleted
    {
        get
        {
            return runsCompleted;
        }

        set
        {
            runsCompleted = value;
        }
    }

    public static int Runs
    {
        get
        {
            return runs;
        }

        set
        {
            runs = value;
        }
    }

    public static float SimSpeed
    {
        get
        {
            return simSpeed;
        }

        set
        {
            simSpeed = value;
        }
    }

    public static bool FFAMode
    {
        get
        {
            return ffaMode;
        }

        set
        {
            ffaMode = value;
        }
    }
}
