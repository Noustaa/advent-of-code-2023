// Part 1

// Game output
List<long> seedsLocations = new();

string[] lines = File.ReadAllLines(@"./input.txt")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();

List<long> seedsToBePlanted = lines[0]
                                .Split(':')[1]
                                .Trim()
                                .Split(' ')
                                .Select(x => long.Parse(x))
                                .ToList();
List<List<TransformMap>> maps = new();

List<TransformMap> map = new();
foreach (string line in lines.Skip(2))
{
    if (char.IsLetter(line[0]))
    {
        maps.Add(map);
        map = new();
        continue;
    }

    List<long> values = line.Split(' ').Select(long.Parse).ToList();

    map.Add(new TransformMap
    {
        StartRange = values[1],
        EndRange = values[1] + values[2] - 1,
        Difference = values[0] - values[1]
    });
}
maps.Add(map);

// Seed to be planted
foreach (long seed in seedsToBePlanted)
{
    long mappedSeed = seed;
    // Sections
    foreach (List<TransformMap> tMapList in maps)
    {
        long localMappedSeed = mappedSeed;
        // Lines in Sections
        foreach (TransformMap tMap in tMapList)
        {
            if (mappedSeed >= tMap.StartRange && mappedSeed <= tMap.EndRange)
                localMappedSeed += tMap.Difference;
        }
        mappedSeed = localMappedSeed;
    }
    seedsLocations.Add(mappedSeed);
}

Console.WriteLine("Part 1: " + seedsLocations.Min());

// Part 2

// Game output
List<(long, long)> seedsRangesToBePlanted = new();

lines = File.ReadAllLines(@"./input.txt")
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

List<long> seedNumbers = lines[0]
                    .Split(':')[1]
                    .Trim()
                    .Split(' ')
                    .Select(long.Parse)
                    .ToList();
for (int i = 0; i < seedNumbers.Count; i += 2)
{
    seedsRangesToBePlanted.Add((seedNumbers[i], seedNumbers[i + 1]));
}

maps = new();

map = new();
foreach (string line in lines.Skip(2))
{
    if (char.IsLetter(line[0]))
    {
        maps.Add(map);
        map = new();
        continue;
    }

    List<long> values = line.Split(' ').Select(long.Parse).ToList();

    map.Add(new TransformMap
    {
        StartRange = values[1],
        EndRange = values[1] + values[2] - 1,
        Difference = values[0] - values[1]
    });
}
maps.Add(map);

// Seed to be planted
long lowestLocation = long.MaxValue;
object lockObject = new();
Parallel.ForEach(seedsRangesToBePlanted, seedRange =>
{
    long localLowestLocation = long.MaxValue;
    for (long i = seedRange.Item1; i < seedRange.Item1 + seedRange.Item2; i++)
    {
        long mappedSeed = i;
        // Sections
        foreach (List<TransformMap> tMapList in maps)
        {
            long localMappedSeed = mappedSeed;
            // Lines in Sections
            foreach (TransformMap tMap in tMapList)
            {
                if (mappedSeed >= tMap.StartRange && mappedSeed <= tMap.EndRange)
                    localMappedSeed += tMap.Difference;
            }
            mappedSeed = localMappedSeed;
        }
        if (mappedSeed < localLowestLocation)
            localLowestLocation = mappedSeed;
    }
    lock (lockObject)
    {
        if (localLowestLocation < lowestLocation)
            lowestLocation = localLowestLocation;
    }
});

Console.WriteLine("Part 2: " + lowestLocation);

public class TransformMap
{
    public long StartRange { get; set; }
    public long EndRange { get; set; }
    public long Difference { get; set; }
}
