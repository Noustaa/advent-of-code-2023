using System.Drawing;

string[] lines = File.ReadAllLines("input.txt");
char[,] map = new char[lines.Length, lines[0].Length];
List<Point> rocks = new();
Point start = new();

for (int y = 0; y < lines.Length; y++)
    for (int x = 0; x < lines[y].Length; x++)
    {
        map[y, x] = lines[y][x];
        if (map[y, x] == 'S')
            start = new Point(x, y);
        if (map[y, x] == '#')
            rocks.Add(new Point(x, y));
    }

DijkstraAlgorithm dijkstra = new();
Dictionary<Point, int> distances = dijkstra.Dijkstra(map, start);

int stepsToDo = 131;
List<Point> inRange = distances
    .Where(x => x.Value <= stepsToDo && x.Value % 2 == stepsToDo % 2)
    .Select(x => x.Key)
    .ToList();

Console.WriteLine($"Part 1: {inRange.Count}");

public class DijkstraAlgorithm
{
    public Dictionary<Point, int> Dijkstra(char[,] map, Point start)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        Dictionary<Point, int> distances = new();
        PriorityQueue<Point> queue = new();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Point point = new(x, y);
                distances[point] = int.MaxValue;
            }
        }

        distances[start] = 0;
        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            Point current = queue.Dequeue();

            List<Point> neighbors = GetNeighbors(current, rows, cols, map);

            foreach (Point neighbor in neighbors)
            {
                int distance = distances[current] + 1;

                if (distance < distances[neighbor])
                {
                    distances[neighbor] = distance;
                    queue.Enqueue(neighbor, distance);
                }
            }
        }

        return distances;
    }

    private List<Point> GetNeighbors(Point point, int rows, int cols, char[,] map)
    {
        List<Point> neighbors = new();

        int x = point.X;
        int y = point.Y;

        if (x > 0 && map[x - 1, y] != '#')
            neighbors.Add(new Point(x - 1, y));
        if (x < cols - 1 && map[x + 1, y] != '#')
            neighbors.Add(new Point(x + 1, y));
        if (y > 0 && map[x, y - 1] != '#')
            neighbors.Add(new Point(x, y - 1));
        if (y < rows - 1 && map[x, y + 1] != '#')
            neighbors.Add(new Point(x, y + 1));

        return neighbors;
    }
}

public class PriorityQueue<T>
{
    private SortedDictionary<int, Queue<T>> _dictionary = new();

    public int Count { get; private set; }

    public void Enqueue(T item, int priority)
    {
        if (!_dictionary.ContainsKey(priority))
            _dictionary[priority] = new Queue<T>();

        _dictionary[priority].Enqueue(item);
        Count++;
    }

    public T Dequeue()
    {
        var item = _dictionary.First().Value.Dequeue();
        if (_dictionary.First().Value.Count == 0)
            _dictionary.Remove(_dictionary.First().Key);

        Count--;
        return item;
    }
}
