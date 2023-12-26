using rowIdx = System.Int32;
using colIdx = System.Int32;

string[] inputs = File.ReadAllLines("input.txt");
int rows = inputs.Length;
int cols = inputs[0].Length;

Dictionary<rowIdx, List<colIdx>> squaredRockPositionsByRows = new();
Dictionary<colIdx, List<rowIdx>> squaredRockPositionsByCols = new();
List<(int, int)> roundedRockPositions = new();

for (int i = 0; i < inputs.Length; i++)
{
    for (int j = 0; j < inputs[i].Length; j++)
    {
        if (inputs[i][j] == '#')
        {
            if (!squaredRockPositionsByRows.ContainsKey(i))
            {
                squaredRockPositionsByRows.Add(i, new List<colIdx>());
            }
            squaredRockPositionsByRows[i].Add(j);
            if (!squaredRockPositionsByCols.ContainsKey(j))
            {
                squaredRockPositionsByCols.Add(j, new List<rowIdx>());
            }
            squaredRockPositionsByCols[j].Add(i);
        }
        if (inputs[i][j] == 'O')
        {
            roundedRockPositions.Add((i, j));
        }
    }
}

List<string> mapHistory = new();

for (int i = 0; i < 1_000_000_000; i++)
{
    roundedRockPositions = TiltPlatform(
        roundedRockPositions,
        RollDirection.North
    );
    roundedRockPositions = TiltPlatform(
        roundedRockPositions,
        RollDirection.West
    );
    roundedRockPositions = TiltPlatform(
        roundedRockPositions,
        RollDirection.South
    );
    roundedRockPositions = TiltPlatform(
        roundedRockPositions,
        RollDirection.East
    );
    var currentMap = new List<string>();
    for (int j = 0; j < rows; j++)
    {
        string row = "";
        for (int k = 0; k < cols; k++)
        {
            if (squaredRockPositionsByRows.ContainsKey(j) && squaredRockPositionsByRows[j].Contains(k))
            {
                row += "#";
            }
            else if (roundedRockPositions.Contains((j, k)))
            {
                row += "O";
            }
            else
            {
                row += ".";
            }
        }
        currentMap.Add(row);
    }
    if (mapHistory.Contains(string.Join("", currentMap)))
    {
        int totatestlWeight = 0;
        foreach (var (row, col) in roundedRockPositions)
        {
            totatestlWeight += rows - row;
        }

        Console.WriteLine(totatestlWeight);
    }
    mapHistory.Add(string.Join("", currentMap));
}

List<(rowIdx, colIdx)> TiltPlatform(
    List<(rowIdx, colIdx)> roundedRockPositions,
    RollDirection direction
    )
{
    List<(int, int)> newRoundedRocksPositions = new List<(int, int)>(roundedRockPositions.Count);
    Dictionary<int, Dictionary<int, int>> distanceFromEdgeByCols = new();
    Dictionary<int, Dictionary<int, int>> distanceFromEdgeByRows = new();
    if (direction is RollDirection.North)
    {
        int nbRocks = roundedRockPositions.Count;
        for (int i = 0; i < nbRocks; i++)
        {
            var (row, col) = roundedRockPositions[i];

            if (!distanceFromEdgeByCols.TryGetValue(col, out var segments))
            {
                segments = new Dictionary<int, int>();
                distanceFromEdgeByCols[col] = segments;
            }

            int segment = 0;
            if (!segments.ContainsKey(segment))
                segments[segment] = -1;

            if (squaredRockPositionsByCols.TryGetValue(col, out var squareRows))
            {
                int nbSquares = squareRows.Count;
                for (int j = 0; j < nbSquares; j++)
                {
                    int squareRow = squareRows[j];
                    int nextSquareRow = j < nbSquares - 1 ? squareRows[j + 1] : -1;
                    if (row < squareRow)
                        break;
                    else if ((row > squareRow && row < nextSquareRow) || nextSquareRow == -1)
                    {
                        if (!segments.ContainsKey(++segment))
                            segments[segment] = squareRow;
                        break;
                    }
                    else
                        segment++;
                }
            }
            segments[segment]++;
            newRoundedRocksPositions.Add((segments[segment], col));
        }
    }
    if (direction is RollDirection.West)
    {
        int nbRocks = roundedRockPositions.Count;
        for (int i = 0; i < nbRocks; i++)
        {
            var (row, col) = roundedRockPositions[i];

            if (!distanceFromEdgeByRows.TryGetValue(row, out var segments))
            {
                segments = new Dictionary<int, int>();
                distanceFromEdgeByRows[row] = segments;
            }

            int segment = 0;
            if (!segments.ContainsKey(segment))
                segments[segment] = -1;

            if (squaredRockPositionsByRows.TryGetValue(row, out var squareCols))
            {
                int nbSquares = squareCols.Count;
                for (int j = 0; j < nbSquares; j++)
                {
                    int squareCol = squareCols[j];
                    int nextSquareCol = j < nbSquares - 1 ? squareCols[j + 1] : -1;
                    if (col < squareCol)
                        break;
                    else if ((col > squareCol && col < nextSquareCol) || nextSquareCol == -1)
                    {
                        if (!segments.ContainsKey(++segment))
                            segments[segment] = squareCol;
                        break;
                    }
                    else
                        segment++;
                }
            }
            segments[segment]++;
            newRoundedRocksPositions.Add((row, segments[segment]));
        }
    }
    if (direction is RollDirection.South)
    {
        int nbRocks = roundedRockPositions.Count;
        for (int i = nbRocks - 1; i >= 0; i--)
        {
            var (row, col) = roundedRockPositions[i];

            if (!distanceFromEdgeByCols.TryGetValue(col, out var segments))
            {
                segments = new Dictionary<int, int>();
                distanceFromEdgeByCols[col] = segments;
            }

            if (squaredRockPositionsByCols.TryGetValue(col, out var squareRows))
            {
                int nbSquares = squareRows.Count;
                int segment = nbSquares;
                if (!segments.ContainsKey(segment))
                    segments[segment] = rows;
                for (int j = nbSquares - 1; j >= 0; j--)
                {
                    int squareRow = squareRows[j];
                    int previousSquareRow = j > 0 ? squaredRockPositionsByCols[col][j - 1] : -1;
                    if (row > squareRow)
                        break;
                    else if ((row < squareRow && row > previousSquareRow) || previousSquareRow == -1)
                    {
                        if (!segments.ContainsKey(--segment))
                            segments[segment] = squareRow;
                        break;
                    }
                    else
                        segment--;
                }
                segments[segment]--;
                newRoundedRocksPositions.Add((segments[segment], col));
            }
            else
            {
                int segment = 0;
                if (!distanceFromEdgeByCols[col].ContainsKey(segment))
                    distanceFromEdgeByCols[col].Add(segment, rows);
                segments[segment]--;
                newRoundedRocksPositions.Add((segments[segment], col));
            }
        }
    }
    if (direction is RollDirection.East)
    {
        int nbRocks = roundedRockPositions.Count;
        for (int i = nbRocks - 1; i >= 0; i--)
        {
            var (row, col) = roundedRockPositions[i];

            if (!distanceFromEdgeByRows.TryGetValue(row, out var segments))
            {
                segments = new Dictionary<int, int>();
                distanceFromEdgeByRows[row] = segments;
            }

            if (squaredRockPositionsByRows.TryGetValue(row, out var squareCols))
            {
                int nbSquares = squareCols.Count;
                int segment = nbSquares;
                if (!segments.ContainsKey(segment))
                    segments[segment] = cols;
                for (int j = nbSquares - 1; j >= 0; j--)
                {
                    int squareCol = squareCols[j];
                    int previousSquareCol = j > 0 ? squaredRockPositionsByRows[row][j - 1] : -1;
                    if (col > squareCol)
                        break;
                    else if ((col < squareCol && col > previousSquareCol) || previousSquareCol == -1)
                    {
                        if (!segments.ContainsKey(--segment))
                            segments[segment] = squareCol;
                        break;
                    }
                    else
                        segment--;
                }
                segments[segment]--;
                newRoundedRocksPositions.Add((row, segments[segment]));
            }
            else
            {
                int segment = 0;
                if (!distanceFromEdgeByRows[row].ContainsKey(segment))
                    distanceFromEdgeByRows[row].Add(segment, cols);
                segments[segment]--;
                newRoundedRocksPositions.Add((row, segments[segment]));
            }
        }
    }
    return newRoundedRocksPositions;
}

int totalWeight = 0;
foreach (var (row, col) in roundedRockPositions)
{
    totalWeight += rows - row;
}

Console.WriteLine(totalWeight);

record Segment((rowIdx, colIdx) Start, (rowIdx, colIdx) End);
enum ParsingMode
{
    Row,
    Column
}

enum RollDirection
{
    North,
    West,
    South,
    East
}

enum RockType
{
    NoRock,
    SquaredRock,
    RoundedRock
}