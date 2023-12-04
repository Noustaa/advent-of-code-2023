// Part 1

// Game output
double total = 0;

foreach (string line in File.ReadAllLines(@"./input.txt"))
{
    string[] parts = line.Split(':', '|');
    List<int> winningNumbers = parts[1]
                                .Trim()
                                .Split(' ')
                                .Where(x => x != "")
                                .Select(int.Parse)
                                .ToList();
    List<int> myNumbers = parts[2]
                            .Trim()
                            .Split(' ')
                            .Where(x => x != "")
                            .Select(int.Parse)
                            .ToList();

    int count = winningNumbers.Intersect(myNumbers).Count();
    if (count > 0)
        total += Math.Pow(2, count) / 2;
}

Console.WriteLine("Part 1: " + total);

// Part 2

// Game output
total = 0;

Dictionary<int, int> cards = new();
string[] lines = File.ReadAllLines(@"./input.txt");

// Default value 1 represents the original card
for (int i = 0; i < lines.Length; i++)
    cards.Add(i, 1);


for (int i = 0; i < lines.Length; i++)
{
    string[] parts = lines[i].Split(':', '|');
    List<int> winningNumbers = parts[1]
                                .Trim()
                                .Split(' ')
                                .Where(x => x != "")
                                .Select(int.Parse)
                                .ToList();
    List<int> myNumbers = parts[2]
                            .Trim()
                            .Split(' ')
                            .Where(x => x != "")
                            .Select(int.Parse)
                            .ToList();
    int count = winningNumbers.Intersect(myNumbers).Count();

    for (int j = 0; j < count; j++)
    {
        cards[i + j + 1] += cards[i];
    }
}

total = cards.Values.Sum();

Console.WriteLine("Part 2: " + total);