// Part 1

// Game settings
Dictionary<string, int> availableCubes = new(){
    {"red", 12},
    {"green", 13},
    {"blue", 14}
};

// Game output
int totalIDs = 0;

foreach (string line in File.ReadAllLines(@"./input.txt"))
{
    int gameID = int.Parse(line.Split(":")[0].Split(" ")[1]);

    string[] subsets = line.Split(":")[1].Split(";");

    bool isGameValid = true;
    foreach (string subset in subsets)
    {
        if (isGameValid)
        {
            foreach (string cubes in subset.Split(","))
            {
                int nbCubes = int.Parse(cubes.Trim().Split(" ")[0]);
                string cubesColor = cubes.Trim().Split(" ")[1];

                if (nbCubes > availableCubes[cubesColor])
                {
                    isGameValid = false;
                    break;
                }
            }
        }
    }
    if (isGameValid)
    {
        totalIDs += gameID;
    }
}

Console.WriteLine("Part 1 : " + totalIDs);

// Part 2

// Game output
int powerSum = 0;

foreach (string line in File.ReadAllLines(@"./input.txt"))
{
    Dictionary<string, int> minCubesPerColor = new() {
        {"red", 0},
        {"green", 0},
        {"blue", 0}
    };

    string[] subsets = line.Split(":")[1].Split(";");

    foreach (string subset in subsets)
    {
        foreach (string cubes in subset.Split(","))
        {
            int nbCubes = int.Parse(cubes.Trim().Split(" ")[0]);
            string cubesColor = cubes.Trim().Split(" ")[1];

            if (nbCubes > minCubesPerColor[cubesColor])
            {
                minCubesPerColor[cubesColor] = nbCubes;
            }
        }
    }

    powerSum += minCubesPerColor["red"] * minCubesPerColor["green"] * minCubesPerColor["blue"];
}

Console.WriteLine("Part 2 : " + powerSum);