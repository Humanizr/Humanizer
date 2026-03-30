namespace Humanizer;

class IndianGroupingNumberToWordsConverter(IndianGroupingNumberToWordsConverter.Profile profile) : GenderlessNumberToWordsConverter
{
    public override string Convert(long number) =>
        ConvertImpl(number, isOrdinal: false);

    public override string ConvertToOrdinal(int number) =>
        ConvertImpl(number, isOrdinal: true);

    string ConvertImpl(long number, bool isOrdinal)
    {
        if (number == 0)
        {
            return GetUnitValue(0, isOrdinal);
        }

        if (number < 0)
        {
            return profile.NegativeWord + " " + Convert(-number);
        }

        var parts = new List<string>();

        if (number / 1_000_000_000_000_000_000 > 0)
        {
            parts.Add(Convert(number / 1_000_000_000_000_000_000) + " " + profile.QuintillionWord);
            number %= 1_000_000_000_000_000_000;
        }

        if (number / 1_000_000_000_000_000 > 0)
        {
            parts.Add(Convert(number / 1_000_000_000_000_000) + " " + profile.QuadrillionWord);
            number %= 1_000_000_000_000_000;
        }

        if (number / 10_000_000 > 0)
        {
            parts.Add(GetCroresValue(ref number));
        }

        if (number / 100_000 > 0)
        {
            parts.Add(GetLakhsValue(ref number, isOrdinal));
        }

        if (number / 1000 > 0)
        {
            parts.Add(GetThousandsValue(ref number));
        }

        if (number / 100 > 0)
        {
            parts.Add(GetHundredsValue(ref number));
        }

        if (number > 0)
        {
            parts.Add(GetTensValue(number, isOrdinal));
        }
        else if (isOrdinal)
        {
            parts[^1] += profile.OrdinalSuffix;
        }

        return string.Join(" ", parts);
    }

    string GetUnitValue(long number, bool isOrdinal)
    {
        if (isOrdinal)
        {
            if (profile.OrdinalExceptions.TryGetValue(checked((int)number), out var exceptionString))
            {
                return exceptionString;
            }

            return profile.UnitsMap[number] + profile.OrdinalSuffix;
        }

        return profile.UnitsMap[number];
    }

    string GetTensValue(long number, bool isOrdinal, bool isThousand = false)
    {
        if (number < 20)
        {
            return GetUnitValue(number, isOrdinal);
        }

        var quotient = number / 10;
        var lastPart = profile.TensMap[quotient];

        if (number % 10 > 0)
        {
            if (quotient == 9)
            {
                lastPart += profile.NineTensWithRemainderSuffix;
            }
            else if (quotient is 4 or 7 or 8)
            {
                lastPart += profile.SpecialTensWithRemainderSuffix;
            }
            else
            {
                lastPart += profile.DefaultTensWithRemainderSuffix;
            }

            if (!isThousand)
            {
                lastPart += GetUnitValue(number % 10, isOrdinal);
            }
        }
        else if (isThousand)
        {
            lastPart += quotient == 9
                ? profile.ExactThousandsNineSuffix
                : profile.ExactThousandsDefaultSuffix;
        }
        else
        {
            lastPart += quotient == 9
                ? profile.ExactTensNineSuffix
                : profile.ExactTensDefaultSuffix;
        }

        return lastPart;
    }

    string GetLakhsValue(ref long number, bool isOrdinal)
    {
        var numberAboveTen = number / 100_000;
        string localWord;
        if (numberAboveTen >= 20)
        {
            localWord = GetTensValue(numberAboveTen, isOrdinal) + " " + profile.LakhWord;
        }
        else if (numberAboveTen == 1)
        {
            localWord = profile.SingleLakhWord + " " + profile.LakhWord;
        }
        else
        {
            localWord = GetTensValue(numberAboveTen, isOrdinal) + " " + profile.LakhWord;
        }

        localWord += number % 1_000_000 == 0 || number % 100_000 == 0
            ? profile.LakhExactSuffix
            : profile.LakhContinuingSuffix;

        number %= 100_000;
        return localWord;
    }

    string GetCroresValue(ref long number)
    {
        var localWord = string.Empty;
        var numberAboveTen = number / 10_000_000;

        if (numberAboveTen is > 99_999 and <= 9_999_999)
        {
            localWord = GetLakhsValue(ref numberAboveTen, false) + " ";
        }

        if (numberAboveTen is > 999 and <= 99_999)
        {
            localWord += GetThousandsValue(ref numberAboveTen) + " ";
        }

        if (numberAboveTen is > 99 and <= 999)
        {
            localWord += GetHundredsValue(ref numberAboveTen) + " ";
        }

        if (numberAboveTen >= 20)
        {
            localWord += GetTensValue(numberAboveTen, false) + " ";
        }
        else if (numberAboveTen == 1)
        {
            localWord = profile.SingleLakhWord + " ";
        }
        else if (numberAboveTen > 0)
        {
            localWord += GetTensValue(numberAboveTen, false) + " ";
        }

        localWord = localWord.TrimEnd() + " " + profile.CroreWord;
        if (number % 10_000_000 != 0 && number % 100_000_000 != 0)
        {
            localWord += profile.CroreContinuingSuffix;
        }

        number %= 10_000_000;
        return localWord;
    }

    string GetThousandsValue(ref long number)
    {
        var numberAboveTen = number / 1000;
        var localWord = string.Empty;

        if (numberAboveTen >= 20)
        {
            localWord = GetTensValue(numberAboveTen, false, true);

            if (numberAboveTen % 10 == 1)
            {
                localWord += profile.ThousandsOneBridge;
            }
            else if (numberAboveTen % 10 > 1)
            {
                localWord += profile.ThousandsMap[numberAboveTen % 10 - 1];
            }
        }
        else
        {
            localWord += profile.ThousandsMap[number / 1000 - 1];
        }

        number %= 1000;
        localWord += number > 0
            ? profile.ThousandContinuingSuffix
            : profile.ThousandExactSuffix;

        return localWord;
    }

    string GetHundredsValue(ref long number)
    {
        var localWord = profile.HundredsMap[number / 100 - 1];
        if (number / 100 == 9)
        {
            localWord += number % 100 == 0
                ? profile.HundredsNineExactSuffix
                : profile.HundredsNineContinuingSuffix;
        }
        else
        {
            localWord += number % 100 >= 1
                ? profile.HundredsContinuingSuffix
                : profile.HundredsExactSuffix;
        }

        number %= 100;
        return localWord;
    }

    public sealed record Profile(
        string ZeroWord,
        string NegativeWord,
        string OrdinalSuffix,
        string QuintillionWord,
        string QuadrillionWord,
        string LakhWord,
        string SingleLakhWord,
        string CroreWord,
        string DefaultTensWithRemainderSuffix,
        string SpecialTensWithRemainderSuffix,
        string NineTensWithRemainderSuffix,
        string ExactTensDefaultSuffix,
        string ExactTensNineSuffix,
        string ExactThousandsDefaultSuffix,
        string ExactThousandsNineSuffix,
        string ThousandContinuingSuffix,
        string ThousandExactSuffix,
        string ThousandsOneBridge,
        string LakhContinuingSuffix,
        string LakhExactSuffix,
        string CroreContinuingSuffix,
        string HundredsContinuingSuffix,
        string HundredsExactSuffix,
        string HundredsNineContinuingSuffix,
        string HundredsNineExactSuffix,
        string[] UnitsMap,
        string[] TensMap,
        string[] HundredsMap,
        string[] ThousandsMap,
        FrozenDictionary<int, string> OrdinalExceptions);
}
