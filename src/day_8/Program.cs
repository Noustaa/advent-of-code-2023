string[] lines = File.ReadAllLines("input.txt");
char[] directions = lines[0].ToCharArray();
lines = lines.Skip(2).ToArray();

// Part 1

Dictionary<string, (string left, string right)> mapIndications = new();

foreach (string line in lines)
{
    string position = line[0..3];
    string left = line[7..10];
    string right = line[12..15];
    mapIndications.Add(position, (left, right));
}

int index = 0;
ulong nbOfSteps = 0;
string currentPos = "AAA";
do
{
    if (directions[index] == 'L')
        currentPos = mapIndications[currentPos].left;
    else
        currentPos = mapIndications[currentPos].right;

    nbOfSteps++;

    if (index >= directions.Length - 1)
        index = 0;
    else
        index++;
} while (currentPos != "ZZZ");

Console.WriteLine("Part 1: " + nbOfSteps);

// Part 2

lines = File.ReadAllLines("input.txt");
directions = lines[0].ToCharArray();
lines = lines.Skip(2).ToArray();

mapIndications = new();

foreach (string line in lines)
{
    string position = line[0..3];
    string left = line[7..10];
    string right = line[12..15];
    mapIndications.Add(position, (left, right));
}

Dictionary<ulong, List<string>> arrivalsPerNbOfSteps = new();

object lockObj = new();
List<string> positions = mapIndications.Keys.Where(x => x.EndsWith('A')).ToList();

Parallel.ForEach(positions,
    position =>
    {
        int index = 0;
        ulong nbOfSteps = 0;
        string currentPos = position;
        while (true)
        {
            currentPos = directions[index] == 'L' ? mapIndications[currentPos].left : mapIndications[currentPos].right;
            nbOfSteps++;

            if (currentPos.EndsWith('Z'))
            {
                lock (lockObj)
                {
                    if (arrivalsPerNbOfSteps.ContainsKey(nbOfSteps))
                        arrivalsPerNbOfSteps[nbOfSteps].Add(currentPos);
                    else
                        arrivalsPerNbOfSteps.Add(nbOfSteps, new List<string> { currentPos });

                    if (arrivalsPerNbOfSteps[nbOfSteps].Count == positions.Count)
                    {
                        Console.WriteLine("Part 2: " + nbOfSteps);
                        return;
                    }
                }
            }

            if (index >= directions.Length - 1)
                index = 0;
            else
                index++;
        }
    }
);