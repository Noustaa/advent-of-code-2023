string[] input = File.ReadAllLines("input.txt");
List<List<string>> sections = new();

List<string> localSection = new();
foreach (string line in input)
{
    if (line == "")
    {
        sections.Add(localSection);
        localSection = new();
    }
    else
    {
        localSection.Add(line);
    }
}
sections.Add(localSection);

IEnumerable<int> MirroredRowsIdx(List<string> input) =>
    from rowIdx in Enumerable.Range(0, input.Count - 1)
    where input[rowIdx]
       .SequenceEqual(input[rowIdx + 1])
    select rowIdx;

IEnumerable<int> MirroredColsIdx(List<string> input) =>
    from colIdx in Enumerable.Range(0, input[0].Length - 1)
    where input.Select(row => row[colIdx])
       .SequenceEqual(input.Select(row => row[colIdx + 1]))
    select colIdx;

bool IsPerfectMirror(List<string> input, int index, MirrorMode mode, bool smudgeDetected = false)
{
    List<string> localInput = input.ToList();
    int mirrorDistance = 1;
    if (mode == MirrorMode.Row)
    {
        for (int i = index; i >= 0; i--)
        {
            if (i + mirrorDistance >= localInput.Count)
                break;
            if (localInput[i] != localInput[i + mirrorDistance])
            {
                if (!smudgeDetected)
                {
                    if (GetDifference(localInput[i], localInput[i + mirrorDistance]) == 1)
                    {
                        string smudgeFixed = FixDifference(localInput[i], localInput[i + mirrorDistance]);
                        localInput[i] = smudgeFixed;
                        localInput[i + mirrorDistance] = smudgeFixed;
                        i++;
                        smudgeDetected = true;
                        continue;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                mirrorDistance += 2;
        }
    }
    else
    {
        for (int i = index; i >= 0; i--)
        {
            if (i + mirrorDistance >= localInput[0].Length)
                break;
            if (!localInput.Select(row => row[i]).SequenceEqual(localInput.Select(row => row[i + mirrorDistance])))
            {
                if (!smudgeDetected)
                {
                    if (GetDifference(string.Concat(localInput.Select(row => row[i])),
                        string.Concat(localInput.Select(row => row[i + mirrorDistance]))) == 1)
                    {
                        string smudgeFixed = FixDifference(
                            string.Concat(localInput.Select(row => row[i])),
                            string.Concat(localInput.Select(row => row[i + mirrorDistance])));
                        for (int j = 0; j < localInput.Count; j++)
                        {
                            localInput[j] = localInput[j][..i] + smudgeFixed[j] + localInput[j][(i + 1)..];
                        }
                        i++;
                        smudgeDetected = true;
                        continue;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                mirrorDistance += 2;
        }
    }
    if (smudgeDetected) return true;
    else return false;
}

int colTotal = 0;
int rowTotal = 0;
foreach (List<string> section in sections)
{
    bool smudgeDetected = false;

    List<int> rowMirrors = MirroredRowsIdx(section).ToList();
    List<int> colMirrors = MirroredColsIdx(section).ToList();
    foreach (int rowIdx in rowMirrors)
    {
        if (IsPerfectMirror(section, rowIdx, MirrorMode.Row))
        {
            rowTotal += rowIdx + 1;
            smudgeDetected = true;
            break;
        }
    }
    if (!smudgeDetected)
    {
        foreach (int colIdx in colMirrors)
        {
            if (IsPerfectMirror(section, colIdx, MirrorMode.Col))
            {
                colTotal += colIdx + 1;
                smudgeDetected = true;
                break;
            }
        }
    }

    if (smudgeDetected)
        continue;

    int index = 0;
    for (int i = 0; i < section.Count - 1; i++)
    {
        if (GetDifference(section[i], section[i + 1]) == 1)
        {
            string smudgeFixed = FixDifference(section[i], section[i + 1]);
            List<string> sectionCopy = section.ToList();
            sectionCopy[i] = smudgeFixed;
            sectionCopy[i + 1] = smudgeFixed;
            if (IsPerfectMirror(sectionCopy, i, MirrorMode.Row, true))
            {
                smudgeDetected = true;
                index = i;
                break;
            }
        }
    }
    if (smudgeDetected)
    {
        rowTotal += index + 1;
        continue;
    }

    if (!smudgeDetected)
    {
        for (int i = 0; i < section[0].Length - 1; i++)
        {
            if (GetDifference(string.Concat(section.Select(row => row[i])),
                string.Concat(section.Select(row => row[i + 1]))) == 1)
            {
                string smudgeFixed = FixDifference(
                    string.Concat(section.Select(row => row[i])),
                    string.Concat(section.Select(row => row[i + 1])));

                List<string> sectionCopy = section.ToList();
                for (int j = 0; j < sectionCopy.Count; j++)
                {
                    sectionCopy[j] = sectionCopy[j][..i] + smudgeFixed[j] + sectionCopy[j][(i + 1)..];
                }

                if (IsPerfectMirror(sectionCopy, i, MirrorMode.Col, true))
                {
                    smudgeDetected = true;
                    index = i;
                    break;
                }
            }
        }
    }
    if (smudgeDetected)
    {
        colTotal += index + 1;
        continue;
    }
}

Console.WriteLine("Part 1: " + (colTotal + (100 * rowTotal)));

int GetDifference(string str1, string str2)
{
    int difference = 0;
    for (int i = 0; i < str1.Length; i++)
    {
        if (str1[i] != str2[i])
            difference++;
    }
    return difference;
}

string FixDifference(string str1, string str2)
{
    for (int i = 0; i < str1.Length; i++)
    {
        if (str1[i] != str2[i])
        {
            return str1[..i] + str2[i] + str1[(i + 1)..];
        }
    }
    throw new Exception("No difference found");
}

enum MirrorMode
{
    Row,
    Col
}