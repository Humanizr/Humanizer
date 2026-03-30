namespace Humanizer;

class AppendedGroupNumberToWordsConverter(AppendedGroupNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return profile.NegativeWord + " " + Convert(-number, gender);
        }

        var result = string.Empty;
        var groupLevel = 0;

        while (number >= 1)
        {
            var groupNumber = number % 1000;
            number /= 1000;

            var tens = groupNumber % 100;
            var hundreds = groupNumber / 100;
            var process = string.Empty;

            if (hundreds > 0)
            {
                process = tens == 0 && hundreds == 2
                    ? profile.AppendedTwos[0]
                    : profile.HundredsGroup[hundreds];
            }

            if (tens > 0)
            {
                if (tens < 20)
                {
                    if (tens == 2 && hundreds == 0 && groupLevel > 0)
                    {
                        process = number switch
                        {
                            2000 or 2000000 or 2000000000 => profile.AppendedTwos[groupLevel],
                            _ => profile.Twos[groupLevel]
                        };
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(process))
                        {
                            process += " " + profile.ConjunctionWord + " ";
                        }

                        if (tens == 1 && groupLevel > 0 && hundreds == 0)
                        {
                            process += " ";
                        }
                        else
                        {
                            process += gender == GrammaticalGender.Feminine && groupLevel == 0
                                ? profile.FeminineOnesGroup[tens]
                                : profile.OnesGroup[tens];
                        }
                    }
                }
                else
                {
                    var ones = tens % 10;
                    tens /= 10;

                    if (ones > 0)
                    {
                        if (!string.IsNullOrEmpty(process))
                        {
                            process += " " + profile.ConjunctionWord + " ";
                        }

                        process += gender == GrammaticalGender.Feminine
                            ? profile.FeminineOnesGroup[ones]
                            : profile.OnesGroup[ones];
                    }

                    if (!string.IsNullOrEmpty(process))
                    {
                        process += " " + profile.ConjunctionWord + " ";
                    }

                    process += profile.TensGroup[tens];
                }
            }

            if (!string.IsNullOrEmpty(process))
            {
                if (groupLevel > 0)
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result = profile.ConjunctionWord + " " + result;
                    }

                    if (groupNumber != 2)
                    {
                        if (groupNumber % 100 != 1)
                        {
                            result = groupNumber is >= 3 and <= 10
                                ? profile.PluralGroups[groupLevel] + " " + result
                                : (string.IsNullOrEmpty(result) ? profile.Groups[groupLevel] : profile.AppendedGroups[groupLevel]) + " " + result;
                        }
                        else
                        {
                            result = profile.Groups[groupLevel] + " " + result;
                        }
                    }
                }

                result = process + " " + result;
            }

            groupLevel++;
        }

        return result.Trim();
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.OrdinalZeroWord;
        }

        var beforeOneHundredNumber = number % 100;
        var overTensPart = number / 100 * 100;
        var beforeOneHundredWord = string.Empty;
        var overTensWord = string.Empty;

        if (beforeOneHundredNumber > 0)
        {
            beforeOneHundredWord = ParseNumber(Convert(beforeOneHundredNumber, gender), beforeOneHundredNumber, gender);
        }

        if (overTensPart > 0)
        {
            overTensWord = ParseNumber(Convert(overTensPart), overTensPart, gender);
        }

        return (beforeOneHundredWord +
                (overTensPart > 0
                    ? (string.IsNullOrWhiteSpace(beforeOneHundredWord) ? string.Empty : " " + profile.OrdinalSeparatorWord + " ") + overTensWord
                    : string.Empty))
            .Trim();
    }

    string ParseNumber(string word, int number, GrammaticalGender gender)
    {
        if (number == 1)
        {
            return gender == GrammaticalGender.Feminine ? profile.FirstOrdinalFeminine : profile.FirstOrdinalMasculine;
        }

        var ordinals = gender == GrammaticalGender.Feminine
            ? profile.FeminineOrdinalExceptions
            : profile.OrdinalExceptions;

        if (number <= 10)
        {
            foreach (var kv in ordinals.Where(kv => word.EndsWith(kv.Key, StringComparison.Ordinal)))
            {
                return StringHumanizeExtensions.Concat(
                    word.AsSpan(0, word.Length - kv.Key.Length),
                    kv.Value.AsSpan());
            }

            return word;
        }

        if (number is > 10 and < 100)
        {
            var parts = word.Split(' ');
            var newParts = new string[parts.Length];

            for (var index = 0; index < parts.Length; index++)
            {
                var oldPart = parts[index];
                var newPart = oldPart;

                foreach (var kv in ordinals.Where(kv => oldPart.EndsWith(kv.Key, StringComparison.Ordinal)))
                {
                    newPart = StringHumanizeExtensions.Concat(
                        oldPart.AsSpan(0, oldPart.Length - kv.Key.Length),
                        kv.Value.AsSpan());
                }

                if (number > 19 && newPart == oldPart && oldPart.Length > 1)
                {
                    newPart = profile.OrdinalPrefix + oldPart;
                }

                newParts[index] = newPart;
            }

            return string.Join(" ", newParts);
        }

        return profile.OrdinalPrefix + word;
    }

    public sealed record Profile(
        string ZeroWord,
        string NegativeWord,
        string ConjunctionWord,
        string OrdinalZeroWord,
        string OrdinalSeparatorWord,
        string OrdinalPrefix,
        string FirstOrdinalMasculine,
        string FirstOrdinalFeminine,
        string[] Groups,
        string[] AppendedGroups,
        string[] PluralGroups,
        string[] OnesGroup,
        string[] TensGroup,
        string[] HundredsGroup,
        string[] AppendedTwos,
        string[] Twos,
        string[] FeminineOnesGroup,
        FrozenDictionary<string, string> OrdinalExceptions,
        FrozenDictionary<string, string> FeminineOrdinalExceptions);
}
