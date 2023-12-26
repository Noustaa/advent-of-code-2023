string[] lines = File.ReadAllLines("input.txt");
char[,] map = new char[lines.Length, lines[0].Length];
char[,] energizedMap = new char[lines.Length, lines[0].Length];

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];
    for (int j = 0; j < line.Length; j++)
    {
        map[i, j] = line[j];
        energizedMap[i, j] = line[j];
    }
}

HashSet<(int, int, Direction)> visitedTiles = new();
void BrowseMap(int row, int column, Direction direction)
{
    while (true)
    {
        if (row < 0 || row >= map.GetLength(0) || column < 0 || column >= map.GetLength(1))
            return;

        if (energizedMap[row, column] != '#')
            energizedMap[row, column] = '#';

        switch (map[row, column])
        {
            case '|':
                if (direction is Direction.Left or Direction.Right)
                {
                    if (visitedTiles.Contains((row, column, Direction.Up)) && visitedTiles.Contains((row, column, Direction.Down)))
                        return;
                    if (!visitedTiles.Contains((row, column, Direction.Up)))
                    {
                        direction = Direction.Up;
                        visitedTiles.Add((row, column, Direction.Up));
                    }
                    if (!visitedTiles.Contains((row, column, Direction.Down)))
                    {
                        visitedTiles.Add((row, column, Direction.Down));
                        BrowseMap(row + 1, column, Direction.Down);
                    }
                }
                break;
            case '-':
                if (direction is Direction.Up or Direction.Down)
                {
                    if (visitedTiles.Contains((row, column, Direction.Left)) && visitedTiles.Contains((row, column, Direction.Right)))
                        return;
                    if (!visitedTiles.Contains((row, column, Direction.Left)))
                    {
                        direction = Direction.Left;
                        visitedTiles.Add((row, column, Direction.Left));
                    }
                    if (!visitedTiles.Contains((row, column, Direction.Right)))
                    {
                        visitedTiles.Add((row, column, Direction.Right));
                        BrowseMap(row, column + 1, Direction.Right);
                    }
                }
                break;
            case '/':
                if (direction is Direction.Right)
                    direction = Direction.Up;
                else if (direction is Direction.Left)
                    direction = Direction.Down;
                else if (direction is Direction.Up)
                    direction = Direction.Right;
                else if (direction is Direction.Down)
                    direction = Direction.Left;
                break;
            case '\\':
                if (direction is Direction.Right)
                    direction = Direction.Down;
                else if (direction is Direction.Left)
                    direction = Direction.Up;
                else if (direction is Direction.Up)
                    direction = Direction.Left;
                else if (direction is Direction.Down)
                    direction = Direction.Right;
                break;
        }
        if (direction is Direction.Right)
            column++;
        else if (direction is Direction.Left)
            column--;
        else if (direction is Direction.Up)
            row--;
        else if (direction is Direction.Down)
            row++;

    }
}

// Part 1

BrowseMap(0, 0, Direction.Right);
long total = CountEnergizedTiles(energizedMap);

Console.WriteLine("Part 1: " + total);

// Part 2
total = 0;

// Iterate over the first row
for (int j = 0; j < map.GetLength(1); j++)
{
    energizedMap = (char[,])map.Clone();
    visitedTiles.Clear();
    BrowseMap(0, j, Direction.Down);
    long localTotal = CountEnergizedTiles(energizedMap);
    if (localTotal > total)
        total = localTotal;
}

// Iterate over the last row
for (int j = 0; j < map.GetLength(1); j++)
{
    energizedMap = (char[,])map.Clone();
    visitedTiles.Clear();
    BrowseMap(map.GetLength(0) - 1, j, Direction.Up);
    long localTotal = CountEnergizedTiles(energizedMap);
    if (localTotal > total)
        total = localTotal;
}

// Iterate over the first column
for (int i = 1; i < map.GetLength(0) - 1; i++)
{
    energizedMap = (char[,])map.Clone();
    visitedTiles.Clear();
    BrowseMap(i, 0, Direction.Right);
    long localTotal = CountEnergizedTiles(energizedMap);
    if (localTotal > total)
        total = localTotal;
}

// Iterate over the last column
for (int i = 1; i < map.GetLength(0) - 1; i++)
{
    energizedMap = (char[,])map.Clone();
    visitedTiles.Clear();
    BrowseMap(i, map.GetLength(1) - 1, Direction.Left);
    long localTotal = CountEnergizedTiles(energizedMap);
    if (localTotal > total)
        total = localTotal;
}

Console.WriteLine("Part 2: " + total);

long CountEnergizedTiles(char[,] map)
{
    long total = 0;
    foreach (char c in map)
    {
        if (c == '#')
            total++;
    }
    return total;
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}