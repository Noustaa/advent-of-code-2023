using System.Drawing;

string[] lines = File.ReadAllLines("input.txt");
int[][] paths = lines.Select(x => x.Select(c => (int)c).ToArray()).ToArray();
List<char> pipesChars = new() { '|', 'L', 'J', '7', 'F', '-' };
for (int i = 0; i < paths.Length; i++)
{
    for (int j = 0; j < paths[i].Length; j++)
    {
        if (pipesChars.Contains((char)paths[i][j]))
        {
            paths[i][j] = -1;
        }
        else
        {
            paths[i][j] = -2;
        }
    }
}

// Part 1

// Position of S character
Point sPosition = new(
    x: Array.IndexOf(lines, lines.Where(x => x.Contains('S')).First()),
    y: lines.Where(x => x.Contains('S')).First().IndexOf('S')
);
Point currentPosition = sPosition;
paths[currentPosition.X][currentPosition.Y] = 0;

List<Point> startingPositions = GetStartingPositions(currentPosition);
int max = 0;
int distance = 1;
foreach (Point position in startingPositions.Skip(1))
{
    currentPosition = position;
    distance = 1;
    do
    {
        paths[currentPosition.X][currentPosition.Y] = distance;
        distance++;
    } while ((currentPosition = Move(currentPosition)) != Point.Empty);
    if (distance > max)
        max = distance;
}

Dictionary<int, List<int>> surround = new();
for (int i = 0; i < paths.Length; i++)
{
    List<int> list = new();
    for (int j = 0; j < paths[i].Length; j++)
    {
        if (paths[i][j] >= 0)
        {
            list.Add(j);
        }
    }
    if (list.Count > 0)
        surround.Add(i, list);
}

Dictionary<int, List<(int, int)>> surround2 = new();
List<(int, int)> indexIntervals;
for (int i = 0; i < paths.Length; i++)
{
    int start = -1;
    int end = -1;
    indexIntervals = new();
    for (int j = 0; j < paths[i].Length; j++)
    {
        if (paths[i][j] >= 0)
        {
            if (start == -1)
            {
                start = j;
            }
            end = j;
        }
        else if (start != -1 && end != -1)
        {
            indexIntervals.Add((start, end));
            start = -1;
            end = -1;
        }
    }
    if (start != -1 && end != -1)
    {
        indexIntervals.Add((start, end));
    }
    if (indexIntervals.Count > 0)
        surround2.Add(i, indexIntervals);
}

int total = 0;
List<Point> countedPoints = new();
for (int i = 0; i < paths.Length; i++)
{
    if (surround2.ContainsKey(i))
    {
        for (int j = 0; j < paths[i].Length; j++)
        {
            if (i < paths.Length / 2)
            {
                foreach ((int, int) interval in surround2[i])
                {
                    if (IsNumberBetween(j, interval.Item1, interval.Item2))
                    {
                        if (surround2.ContainsKey(i - 1))
                        {
                            if (paths[i][j] == -2 || paths[i][j] == -1)
                            {
                                total++;
                                countedPoints.Add(new Point(i, j));
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                if (j > surround[i].First() && j < surround[i].Last())
                {
                    if (surround.ContainsKey(i + 1))
                    {
                        if (surround[i + 1].Contains(j) && surround[i + 1].Contains(j - 1) && surround[i + 1].Contains(j + 1))
                        {
                            if (paths[i][j] == -2 || paths[i][j] == -1)
                            {
                                total++;
                                countedPoints.Add(new Point(i, j));
                            }
                        }
                    }
                }
            }
        }
    }
}

// Change all value in paths that are 0 or more to -3
for (int i = 0; i < paths.Length; i++)
{
    for (int j = 0; j < paths[i].Length; j++)
    {
        if (paths[i][j] >= 0)
        {
            paths[i][j] = -3;
        }
    }
}

// Print paths to console
for (int i = 0; i < paths.Length; i++)
{
    for (int j = 0; j < paths[i].Length; j++)
    {
        Console.Write(paths[i][j] + " ");
    }
    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine("Part 1: " + max / 2);

Point Move(Point position)
{
    switch (lines[position.X][position.Y])
    {
        case '|':
            if (paths[position.X - 1][position.Y] == -1)
                return new Point(position.X - 1, position.Y);
            else if (paths[position.X + 1][position.Y] == -1)
                return new Point(position.X + 1, position.Y);
            break;
        case '-':
            if (paths[position.X][position.Y - 1] == -1)
                return new Point(position.X, position.Y - 1);
            else if (paths[position.X][position.Y + 1] == -1)
                return new Point(position.X, position.Y + 1);
            break;
        case 'L':
            if (paths[position.X - 1][position.Y] == -1)
                return new Point(position.X - 1, position.Y);
            else if (paths[position.X][position.Y + 1] == -1)
                return new Point(position.X, position.Y + 1);
            break;
        case 'J':
            if (paths[position.X - 1][position.Y] == -1)
                return new Point(position.X - 1, position.Y);
            else if (paths[position.X][position.Y - 1] == -1)
                return new Point(position.X, position.Y - 1);
            break;
        case '7':
            if (paths[position.X + 1][position.Y] == -1)
                return new Point(position.X + 1, position.Y);
            else if (paths[position.X][position.Y - 1] == -1)
                return new Point(position.X, position.Y - 1);
            break;
        case 'F':
            if (paths[position.X + 1][position.Y] == -1)
                return new Point(position.X + 1, position.Y);
            else if (paths[position.X][position.Y + 1] == -1)
                return new Point(position.X, position.Y + 1);
            break;
        default:
            return Point.Empty;
    }
    return Point.Empty;
}

List<Point> GetStartingPositions(Point point)
{
    List<Point> points = new();
    try
    {
        if (lines[point.X - 1][point.Y] == '|' || lines[point.X - 1][point.Y] == '7' || lines[point.X - 1][point.Y] == 'F')
            points.Add(new Point(point.X - 1, point.Y));
    }
    catch { }
    try
    {
        if (lines[point.X + 1][point.Y] == '|' || lines[point.X + 1][point.Y] == 'L' || lines[point.X + 1][point.Y] == 'J')
            points.Add(new Point(point.X + 1, point.Y));
    }
    catch { }
    try
    {
        if (lines[point.X][point.Y - 1] == '-' || lines[point.X][point.Y - 1] == 'L' || lines[point.X][point.Y - 1] == 'F')
            points.Add(new Point(point.X, point.Y - 1));
    }
    catch { }
    try
    {
        if (lines[point.X][point.Y + 1] == '-' || lines[point.X][point.Y + 1] == '7' || lines[point.X][point.Y + 1] == 'J')
            points.Add(new Point(point.X, point.Y + 1));
    }
    catch { }
    return points;
}

bool IsNumberBetween(int number, int bound1, int bound2)
{
    int lowerBound = Math.Min(bound1, bound2);
    int upperBound = Math.Max(bound1, bound2);
    return number > lowerBound && number < upperBound;
}
