string[] lines = File.ReadAllLines(@"./input.txt");

// Part 1 & 2

List<Hand> handsPart1 = new();
List<Hand> handsPart2 = new();

foreach (string line in lines)
{
    string[] split = line.Split(" ");
    string cards = split[0];
    int bid = int.Parse(split[1]);
    
    // PART 1
    handsPart1.Add(new Hand(cards, bid, jokerRuleOn: false));
    // PART 2
    handsPart2.Add(new Hand(cards, bid, jokerRuleOn: true));
}

List<List<Hand>> parts = new() { handsPart1, handsPart2 };

for (int i = 0; i < parts.Count; i++)
{
    List<Hand> hands = parts[i];

    var handsByHandTypes = hands
        .GroupBy(hand => hand.HandType)
        .ToList();

    handsByHandTypes.Sort((x, y) => x.Key.CompareTo(y.Key));

    hands.Clear();
    foreach (var handType in handsByHandTypes)
    {
        List<Hand> localHands = new();
        foreach (var hand in handType)
        {
            localHands.Add(hand);
        }
        localHands.Sort((x, y) =>
        {
            for (int i = 0; i < x.CardValues.Count; i++)
            {
                if (x.CardValues[i] != y.CardValues[i])
                {
                    return x.CardValues[i].CompareTo(y.CardValues[i]);
                }
            }
            return 0;
        });
        hands.AddRange(localHands);
    }

    long totalWinnings = 0;

    for (int j = 0; j < hands.Count; j++)
    {
        totalWinnings += hands[j].Bid * (j + 1);
    }

    Console.WriteLine("Part " + (i + 1) + ": " + totalWinnings);
}
