using System.Collections;
using System.Collections.Specialized;

string[] inputs = File.ReadAllText("input.txt").Split(",");
OrderedDictionary[] boxes = new OrderedDictionary[256];
for (int i = 0; i < boxes.Length; i++)
    boxes[i] = new OrderedDictionary();

int ComputeHash(string input)
{
    int currentValue = 0;
    foreach (var c in input)
    {
        currentValue += c;
        currentValue *= 17;
        currentValue %= 256;
    }
    return currentValue;
}

foreach (var str in inputs)
{
    if (str.Contains('-'))
    {
        string boxTag = str.Split('-')[0];
        int boxId = ComputeHash(boxTag);
        if (boxes[boxId].Contains(boxTag))
            boxes[boxId].Remove(boxTag);
    }
    else if (str.Contains('='))
    {
        string boxTag = str.Split('=')[0];
        int lensId = int.Parse(str.Split('=')[1]);
        int boxId = ComputeHash(boxTag);
        if (boxes[boxId].Contains(boxTag))
            boxes[boxId][boxTag] = lensId;
        else
            boxes[boxId].Add(boxTag, lensId);
    }
    else
    {
        throw new Exception("Invalid input");
    }
}

long total = 0;
for (int i = 0; i < boxes.Length; i++)
{
    if (boxes[i].Count > 0)
    {
        long boxTotal = 0;
        int lensesIdx = 0;
        foreach (DictionaryEntry lens in boxes[i])
        {
            boxTotal += (i + 1) * (lensesIdx + 1) * (int)lens.Value;
            lensesIdx++;
        }
        total += boxTotal;
    }
}

Console.WriteLine(total);