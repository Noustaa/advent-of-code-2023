// PART 1

int total = 0;

foreach (string line in File.ReadAllLines(@"./input.txt"))
{
    char firstDigit = line.FirstOrDefault(x => char.IsDigit(x));
    char lastDigit = line.LastOrDefault(x => char.IsDigit(x));
    string bothDigits = firstDigit.ToString() + lastDigit.ToString();
    total += int.Parse(bothDigits);
}

Console.WriteLine("Part1 : " + total);

// PART 2

total = 0;
List<string> digitsAsWords = new() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

foreach (string line in File.ReadAllLines(@"./input.txt"))
{
    int firstWordDigitIdx = -1;
    int lastWordDigitIdx = -1;
    string firstWordDigit = "";
    string lastWordDigit = "";
    char firstDigit, lastDigit;

    foreach (string wordDigit in digitsAsWords)
    {
        if (line.Contains(wordDigit))
        {
            int wordIdx = line.IndexOf(wordDigit);
            int lastWordIdx = line.LastIndexOf(wordDigit);
            if ((firstWordDigitIdx == -1) || (wordIdx < firstWordDigitIdx))
            {
                firstWordDigitIdx = wordIdx;
                firstWordDigit = wordDigit;
            }
            if ((lastWordDigitIdx == -1) || (lastWordIdx > lastWordDigitIdx))
            {
                lastWordDigitIdx = lastWordIdx;
                lastWordDigit = wordDigit;
            }
            continue;
        }
    }
    int firstDigitIdx = line.IndexOf(line.FirstOrDefault(x => char.IsDigit(x)));
    int lastDigitIdx = line.LastIndexOf(line.LastOrDefault(x => char.IsDigit(x)));

    if (firstWordDigitIdx < firstDigitIdx && firstWordDigitIdx != -1)
    {
        // Add 48 to get the ASCII equivalent of the digit
        firstDigit = (char)(digitsAsWords.IndexOf(firstWordDigit) + 1 + 48);
    }
    else
    {
        firstDigit = line.FirstOrDefault(x => char.IsDigit(x));
    }

    if (lastWordDigitIdx > lastDigitIdx && lastWordDigitIdx != -1)
    {
        // Add 48 to get the ASCII equivalent of the digit
        lastDigit = (char)(digitsAsWords.IndexOf(lastWordDigit) + 1 + 48);
    }
    else
    {
        lastDigit = line.LastOrDefault(x => char.IsDigit(x));
    }

    string bothDigits = firstDigit.ToString() + lastDigit.ToString();
    total += int.Parse(bothDigits);
}

Console.WriteLine("Part2 : " + total);
