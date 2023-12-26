string[] lines = File.ReadAllLines("input.txt");

List<List<ulong>> brokenSprings = new();
List<string> schemes = new();
foreach (string line in lines)
{
    List<ulong> brokenSpring = new();
    string[] split = line.Split(" ");
    schemes.Add(split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0]);
    for (int i = 0; i < 5; i++)
    {
        foreach (string part in split[1].Split(","))
        {
            brokenSpring.Add(ulong.Parse(part));
        }
    }
    brokenSprings.Add(brokenSpring);
}

ulong count = 0;
for (int i = 0; i < schemes.Count; i++)
{
    string input = schemes[i];
    ulong[] groups = brokenSprings[i].ToArray();
    ulong n = (ulong)input.Length;
    ulong m = (ulong)groups.Length;
    ulong?[,,] memo = new ulong?[n + 1, m + 1, 2];
    count += CountCombinations(input, groups, 0, 0, false, memo);
}

Console.WriteLine("Total combinations: " + count);

static ulong CountCombinations(string input, ulong[] groups, ulong i, ulong j, bool prevHash, ulong?[,,] memo)
{
    if (i == (ulong)input.Length)
    {
        return j == (ulong)groups.Length ? 1UL : 0UL;
    }
    if (j == (ulong)groups.Length)
    {
        return input.Substring((int)i).All(c => c != '#') ? 1UL : 0UL;
    }
    if (memo[i, j, prevHash ? 1 : 0].HasValue)
    {
        return memo[i, j, prevHash ? 1 : 0].Value;
    }
    ulong res = 0;
    if (input[(int)i] != '#')
    {
        res += CountCombinations(input, groups, i + 1, j, false, memo);
    }
    if (!prevHash && input[(int)i] != '.' && i + groups[j] <= (ulong)input.Length && input.Substring((int)i, (int)groups[j]).All(c => c != '.'))
    {
        res += CountCombinations(input, groups, i + groups[j], j + 1, true, memo);
    }
    memo[i, j, prevHash ? 1UL : 0UL] = res;
    return res;
}