using System.Drawing;

string[] lines = File.ReadAllLines("input.txt");

List<int> linesWitoutPlanets = new();
List<int> columnsWitoutPlanets = new();

for (int i = 0; i < lines.Length; i++)
{
    bool hasPlanet = false;
    for (int j = 0; j < lines[i].Length; j++)
    {
        if (lines[i][j] == '#')
        {
            hasPlanet = true;
            break;
        }
    }
    if (!hasPlanet)
    {
        linesWitoutPlanets.Add(i);
    }
}

for (int i = 0; i < lines[0].Length; i++)
{
    bool hasPlanet = false;
    for (int j = 0; j < lines.Length; j++)
    {
        if (lines[j][i] == '#')
        {
            hasPlanet = true;
            break;
        }
    }
    if (!hasPlanet)
    {
        columnsWitoutPlanets.Add(i);
    }
}

List<Point> planetCoordinates = new();
int planetCount = 0;
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        if (lines[i][j] == '#')
        {
            planetCoordinates.Add(new Point(j, i));
            planetCount++;
        }
    }
}

// Part 1
long totalDistance = 0;
int expensionModifier = 1;
for (int i = 0; i < planetCount; i++)
{
    for (int j = i + 1; j < planetCount; j++)
    {
        int linesExpended = 0;
        int columnsExpended = 0;
        foreach (int lineIndex in linesWitoutPlanets)
        {
            if (IsNumberBetween(lineIndex, planetCoordinates[i].Y, planetCoordinates[j].Y))
            {
                linesExpended += expensionModifier;
            }
        }
        foreach (int columnIndex in columnsWitoutPlanets)
        {
            if (IsNumberBetween(columnIndex, planetCoordinates[i].X, planetCoordinates[j].X))
            {
                columnsExpended += expensionModifier;
            }
        }
        totalDistance += Math.Abs(Math.Max(planetCoordinates[i].X, planetCoordinates[j].X) + columnsExpended -
            Math.Min(planetCoordinates[i].X, planetCoordinates[j].X))
            + Math.Abs(Math.Max(planetCoordinates[i].Y, planetCoordinates[j].Y) + linesExpended -
            Math.Min(planetCoordinates[i].Y, planetCoordinates[j].Y));
    }
}

Console.WriteLine("Part 1: " + totalDistance);

// Part 2
totalDistance = 0;
expensionModifier = 999_999;
for (int i = 0; i < planetCount; i++)
{
    for (int j = i + 1; j < planetCount; j++)
    {
        int linesExpended = 0;
        int columnsExpended = 0;
        foreach (int lineIndex in linesWitoutPlanets)
        {
            if (IsNumberBetween(lineIndex, planetCoordinates[i].Y, planetCoordinates[j].Y))
            {
                linesExpended += expensionModifier;
            }
        }
        foreach (int columnIndex in columnsWitoutPlanets)
        {
            if (IsNumberBetween(columnIndex, planetCoordinates[i].X, planetCoordinates[j].X))
            {
                columnsExpended += expensionModifier;
            }
        }
        totalDistance += 
            Math.Max(planetCoordinates[i].X, planetCoordinates[j].X) + columnsExpended -
            Math.Min(planetCoordinates[i].X, planetCoordinates[j].X)
            + 
            Math.Max(planetCoordinates[i].Y, planetCoordinates[j].Y) + linesExpended -
            Math.Min(planetCoordinates[i].Y, planetCoordinates[j].Y);
    }
}

Console.WriteLine("Part 2: " + totalDistance);

bool IsNumberBetween(int number, int bound1, int bound2)
{
    int lowerBound = Math.Min(bound1, bound2);
    int upperBound = Math.Max(bound1, bound2);
    return number > lowerBound && number < upperBound;
}
