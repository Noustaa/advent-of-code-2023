string[] input = File.ReadAllLines(@"./input.txt");

// Part 1

List<long> racesTime = input[0]
    .Split(':')[1]
    .Trim()
    .Split(' ')
    .Where(x => x != "")
    .Select(long.Parse)
    .ToList();
List<long> racesDistance = input[1]
    .Split(':')[1]
    .Trim()
    .Split(' ')
    .Where(x => x != "")
    .Select(long.Parse)
    .ToList();

List<Race> races = new();
for (int i = 0; i < racesTime.Count; i++)
{
    races.Add(new Race(racesTime[i], racesDistance[i]));
}

int countOfPossibleWaysToBeatRecords = 1;
object lockObject = new();

Parallel.ForEach(races, race => 
{
    int recordBreakingHoldingTimesCount = 0;
    for (long buttonHoldingTime = 0; buttonHoldingTime <= race.RaceTime; buttonHoldingTime++)
    {
        long carSpeed = buttonHoldingTime;
        long traveledDistance = carSpeed * (race.RaceTime - buttonHoldingTime);
        if (traveledDistance > race.RaceDistanceToBeat)
            recordBreakingHoldingTimesCount++;
    }
    lock (lockObject)
    {
        countOfPossibleWaysToBeatRecords *= recordBreakingHoldingTimesCount;
    }
});

Console.WriteLine("Part 1: " + countOfPossibleWaysToBeatRecords);

// Part 2

Race race = new(
    long.Parse(input[0]
        .Split(':')[1]
        .Trim()
        .Replace(" ", "")),
    long.Parse(input[1]
        .Split(':')[1]
        .Trim()
        .Replace(" ", ""))
    );

long recordBreakingHoldingTimesCount = 0;
for (long buttonHoldingTime = 0; buttonHoldingTime <= race.RaceTime; buttonHoldingTime++)
{
    long carSpeed = buttonHoldingTime;
    long traveledDistance = carSpeed * (race.RaceTime - buttonHoldingTime);
    if (traveledDistance > race.RaceDistanceToBeat)
        recordBreakingHoldingTimesCount++;
}

Console.WriteLine("Part 2: " + recordBreakingHoldingTimesCount);

public record Race(long RaceTime, long RaceDistanceToBeat);