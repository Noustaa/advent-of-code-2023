string[] lines = File.ReadAllLines("input.txt");

// Part 1

List<int> interpolations = new();
lines.ToList()
    .ForEach(
        line =>
        {
            List<List<int>> localInterpolation = new() {
                line.Split(' ').Select(int.Parse).ToList()
            };
            while (localInterpolation.Last().Sum() != 0)
            {
                List<int> currentRow = localInterpolation.Last();
                List<int> lowerInterpolation = new();
                for (int i = 0; i < currentRow.Count - 1; i++)
                {
                    lowerInterpolation.Add(currentRow[i + 1] - currentRow[i]);
                }
                localInterpolation.Add(lowerInterpolation);
            }
            for (int i = localInterpolation.Count - 2; i >= 0; i--)
            {
                localInterpolation[i].Add(localInterpolation[i].Last() + localInterpolation[i + 1].Last());
            }
            interpolations.Add(localInterpolation[0].Last());
        }
    );

Console.WriteLine("Part 1: " + interpolations.Sum());

// Part 2

interpolations.Clear();
lines.ToList()
    .ForEach(
        line =>
        {
            List<List<int>> localInterpolation = new() {
                line.Split(' ').Select(int.Parse).ToList()
            };
            while (localInterpolation.Last().Sum() != 0)
            {
                List<int> currentRow = localInterpolation.Last();
                List<int> lowerInterpolation = new();
                for (int i = 0; i < currentRow.Count - 1; i++)
                {
                    lowerInterpolation.Add(currentRow[i + 1] - currentRow[i]);
                }
                localInterpolation.Add(lowerInterpolation);
            }
            for (int i = localInterpolation.Count - 2; i >= 0; i--)
            {
                localInterpolation[i].Insert(0, localInterpolation[i].First() - localInterpolation[i + 1].First());
            }
            interpolations.Insert(0, localInterpolation[0].First());
        }
    );

Console.WriteLine("Part 2: " + interpolations.Sum());