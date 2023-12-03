// Part 1

// Game output
int total = 0;

char[] symbols = @".$*#+-%/\:;,?&@=".ToCharArray();
char[] symbolsWithoutDot = symbols.Where(x => x != '.').ToArray();
char[] numbers = "0123456789".ToCharArray();

string[] lines = File.ReadAllLines(@"./input.txt");
for (int i = 0; i < lines.Length; i++)
{
    List<(int, int)> integersGates = GetIntegersGates(lines[i]);
    foreach (var gate in integersGates)
    {
        if (IsIntegerSymbolAdjacent(gate, i))
        {
            total += int.Parse(lines[i][gate.Item1..(gate.Item2 + 1)]);
        }
    }
}

Console.WriteLine("Part1 : " + total);


bool IsIntegerSymbolAdjacent((int, int) integerGates, int lineIndex)
{
    int safeStartGate = integerGates.Item1 == 0 ? 0 : integerGates.Item1 - 1;
    int safeEndGate = integerGates.Item2 == lines[lineIndex].Length - 1 ? integerGates.Item2 : integerGates.Item2 + 1;

    if (symbolsWithoutDot.Contains(lines[lineIndex][safeStartGate]) || symbolsWithoutDot.Contains(lines[lineIndex][safeEndGate]))
    {
        return true;
    }
    if (lineIndex > 0)
    {
        foreach (char c in lines[lineIndex - 1].Substring(safeStartGate, safeEndGate - safeStartGate + 1))
        {
            if (symbolsWithoutDot.Contains(c))
            {
                return true;
            }
        }
    }
    if (lineIndex < lines.Length - 1)
    {
        foreach (char c in lines[lineIndex + 1].Substring(safeStartGate, safeEndGate - safeStartGate + 1))
        {
            if (symbolsWithoutDot.Contains(c))
            {
                return true;
            }
        }
    }
    return false;
}

List<(int, int)> GetIntegersGates(string line)
{
    List<(int, int)> integersGates = new();
    int cursor = 0;
    do
    {
        int startIndex = line[cursor..].IndexOfAny(numbers) + cursor;
        if (startIndex - cursor == -1) break;
        int endIndex = GetIntegerEndIndex(line, startIndex);
        cursor = endIndex + 1;
        integersGates.Add((startIndex, endIndex));
    } while (cursor < line.Length);
    return integersGates;
}

int GetIntegerEndIndex(string line, int startIndex)
{
    string substring = line[startIndex..];
    int endIndex = substring.IndexOfAny(symbols);
    if (endIndex == -1)
    {
        return endIndex = startIndex - 1 + substring.Length;
    }
    else
    {
        return endIndex - 1 + startIndex;
    }
}

// Part 2

// Game output
int totalGearRatio = 0;

char gearSymbol = '*';
int gearID = 0;
List<(int lineIndex, int indexInLine, int ID)> gearsIDs = GetGearsIDs(lines);
// gearID is -1 if there is no adjacent gear
List<(int number, int gearID)> integersWithGearID = new();

for (int i = 0; i < lines.Length; i++)
{

    List<(int, int)> integersGates = GetIntegersGates(lines[i]);
    foreach (var gates in integersGates)
    {
        integersWithGearID.Add(GetNumberWithAdjacentGearID(gates, i));
    }
}

List<(int number, int gearID)> integersGatesWithValidGearID = integersWithGearID
                                                                .Where(x => x.gearID != -1)
                                                                .ToList();
for (int i = 0; i < gearID; i++)
{
    List<int> partNumbers = integersGatesWithValidGearID
                            .Where(x => x.gearID == i)
                            .Select(x => x.number)
                            .ToList();
    if (partNumbers.Count > 1)
    {
        totalGearRatio += partNumbers.Aggregate((a, b) => a * b);
    }
}

Console.WriteLine("Part2 : " + totalGearRatio);

List<(int, int, int)> GetGearsIDs(string[] lines)
{
    List<(int, int, int)> gearsIDs = new();
    for (int i = 0; i < lines.Length; i++)
    {
        int cursor = 0;
        do
        {
            int gearIndex = lines[i][cursor..].IndexOf(gearSymbol) + cursor;
            if (gearIndex - cursor == -1) break;
            cursor = gearIndex + 1;
            gearsIDs.Add((i, gearIndex, gearID));
            gearID++;
        } while (cursor < lines[i].Length);
    }
    return gearsIDs;
}


// Returns -1 if there is no adjacent gear
(int, int) GetNumberWithAdjacentGearID((int, int) integerGates, int lineIndex)
{
    int safeStartGate = integerGates.Item1 == 0 ? 0 : integerGates.Item1 - 1;
    int safeEndGate = integerGates.Item2 == lines[lineIndex].Length - 1 ? integerGates.Item2 : integerGates.Item2 + 1;

    int number = int.Parse(lines[lineIndex][integerGates.Item1..(integerGates.Item2 + 1)]);

    if (gearSymbol == lines[lineIndex][safeStartGate])
    {
        return (number, gearsIDs
                        .Where(x => x.lineIndex == lineIndex && x.indexInLine == safeStartGate)
                        .Select(x => x.ID)
                        .FirstOrDefault());
    }
    if (gearSymbol == lines[lineIndex][safeEndGate])
    {
        return (number, gearsIDs
            .Where(x => x.lineIndex == lineIndex && x.indexInLine == safeEndGate)
            .Select(x => x.ID)
            .FirstOrDefault());
    }
    if (lineIndex > 0)
    {
        string substring = lines[lineIndex - 1].Substring(safeStartGate, safeEndGate - safeStartGate + 1);
        for (int i = 0; i < substring.Length; i++)
        {
            if (gearSymbol == substring[i])
            {
                return (number, gearsIDs
                    .Where(x => x.lineIndex == (lineIndex - 1) && x.indexInLine == i + safeStartGate)
                    .Select(x => x.ID)
                    .FirstOrDefault());
            }
        }
    }
    if (lineIndex < lines.Length - 1)
    {
        string substring = lines[lineIndex + 1].Substring(safeStartGate, safeEndGate - safeStartGate + 1);
        for (int i = 0; i < substring.Length; i++)
        {
            if (gearSymbol == substring[i])
            {
                return (number, gearsIDs
                    .Where(x => x.lineIndex == (lineIndex + 1) && x.indexInLine == i + safeStartGate)
                    .Select(x => x.ID)
                    .FirstOrDefault());
            }
        }
    }
    return (number, -1);
}