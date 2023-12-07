public class Hand
{
    public readonly HandType HandType;
    public readonly int Bid;
    public readonly List<int> CardValues = new();
    private readonly bool _jokerRuleOn;

    public Hand(string cards, int bid, bool jokerRuleOn = false)
    {
        if (jokerRuleOn)
            _jokerRuleOn = true;

        HandType = GetHandType(cards);
        Bid = bid;
        foreach (char card in cards)
        {
            CardValues.Add(GetCardValue(card));
        }
    }

    private HandType GetHandType(string cards)
    {
        int pairCount = 0;
        int threeOfAKindCount = 0;
        int fourOfAKindCount = 0;
        int jokersCount = 0;

        var cardOccurrences = cards
            .GroupBy(card => card)
            .ToDictionary(group => group.Key, group => group.Count());

        foreach (var card in cardOccurrences)
        {
            if (card.Key == 'J')
            {
                jokersCount = card.Value;
                if (_jokerRuleOn)
                    continue;
            }
            switch (card.Value)
            {
                case 2:
                    pairCount++;
                    break;
                case 3:
                    threeOfAKindCount++;
                    break;
                case 4:
                    fourOfAKindCount++;
                    break;
                case 5:
                    return HandType.FiveOfAKind;
                default:
                    break;
            }
        }
        if (pairCount == 0 && threeOfAKindCount == 0 && fourOfAKindCount == 0)
        {
            if (_jokerRuleOn && jokersCount > 0)
            {
                return jokersCount switch
                {
                    1 => HandType.OnePair,
                    2 => HandType.ThreeOfAKind,
                    3 => HandType.FourOfAKind,
                    4 => HandType.FiveOfAKind,
                    5 => HandType.FiveOfAKind,
                    _ => HandType.HighCard,
                };
            }
            return HandType.HighCard;
        }
        else if (pairCount == 1 && threeOfAKindCount == 0)
        {
            if (_jokerRuleOn && jokersCount > 0)
            {
                return jokersCount switch
                {
                    1 => HandType.ThreeOfAKind,
                    2 => HandType.FourOfAKind,
                    3 => HandType.FiveOfAKind,
                    _ => HandType.OnePair,
                };
            }
            else
                return HandType.OnePair;
        }
        else if (pairCount == 2 && threeOfAKindCount == 0)
        {
            if (_jokerRuleOn && jokersCount == 1)
                return HandType.FullHouse;
            else
                return HandType.TwoPair;
        }
        else if (threeOfAKindCount == 1 && pairCount == 0)
        {
            if (_jokerRuleOn && jokersCount > 0)
            {
                return jokersCount switch
                {
                    1 => HandType.FourOfAKind,
                    2 => HandType.FiveOfAKind,
                    _ => HandType.ThreeOfAKind,
                };
            }
            else
                return HandType.ThreeOfAKind;
        }
        else if (pairCount == 1 && threeOfAKindCount == 1)
        {
            return HandType.FullHouse;
        }
        else if (fourOfAKindCount == 1)
        {
            if (_jokerRuleOn && jokersCount == 1)
                return HandType.FiveOfAKind;
            else
                return HandType.FourOfAKind;
        }
        else
        {
            throw new Exception("Invalid hand");
        }
    }

    private int GetCardValue(int card)
    {
        if (!_jokerRuleOn)
        {
            return card switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                '9' => 9,
                '8' => 8,
                '7' => 7,
                '6' => 6,
                '5' => 5,
                '4' => 4,
                '3' => 3,
                '2' => 2,
                _ => 0,
            };
        }
        else
        {
            return card switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'T' => 10,
                '9' => 9,
                '8' => 8,
                '7' => 7,
                '6' => 6,
                '5' => 5,
                '4' => 4,
                '3' => 3,
                '2' => 2,
                'J' => 1,
                _ => 0,
            };
        }
    }
}