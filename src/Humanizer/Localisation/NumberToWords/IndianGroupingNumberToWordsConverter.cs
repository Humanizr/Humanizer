namespace Humanizer;

/// <summary>
/// Renders numbers using the Indian grouping system with crores, lakhs, thousands, hundreds, and
/// locale-specific ordinal suffixes.
/// </summary>
/// <remarks>
/// The converter walks the value from the largest supported grouping down to units, applying the
/// generated suffix tables at each step so the locale can decide when a scale is exact or when it
/// continues into a remainder.
/// </remarks>
class IndianGroupingNumberToWordsConverter(IndianGroupingNumberToWordsConverter.Profile profile) : GenderlessNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the Indian-grouping lexicon and suffix rules.
    /// </summary>
    /// <param name="ZeroWord">The cardinal zero word.</param>
    /// <param name="NegativeWord">The word used to prefix negative values.</param>
    /// <param name="OrdinalSuffix">The suffix appended when an ordinal does not use an exact exception.</param>
    /// <param name="QuintillionWord">The quintillion scale word.</param>
    /// <param name="QuadrillionWord">The quadrillion scale word.</param>
    /// <param name="LakhWord">The lakh scale word used after rendered counts.</param>
    /// <param name="SingleLakhWord">The dedicated one-count word used before lakh-based scales.</param>
    /// <param name="CroreWord">The crore scale word.</param>
    /// <param name="DefaultTensWithRemainderSuffix">The default suffix applied to tens that continue into a unit remainder.</param>
    /// <param name="SpecialTensWithRemainderSuffix">The alternate suffix used by the locale's special tens values when they continue into a unit remainder.</param>
    /// <param name="NineTensWithRemainderSuffix">The suffix used by the nine-tens value when it continues into a unit remainder.</param>
    /// <param name="ExactTensDefaultSuffix">The suffix applied to exact tens values other than nine tens.</param>
    /// <param name="ExactTensNineSuffix">The suffix applied to exact nine-tens values.</param>
    /// <param name="ExactThousandsDefaultSuffix">The suffix applied when an exact thousands count greater than twenty terminates the phrase.</param>
    /// <param name="ExactThousandsNineSuffix">The suffix applied when an exact nine-thousands count terminates the phrase.</param>
    /// <param name="ThousandContinuingSuffix">The suffix applied when the thousands segment is followed by a remainder.</param>
    /// <param name="ThousandExactSuffix">The suffix applied when the thousands segment ends the phrase.</param>
    /// <param name="ThousandsOneBridge">The bridge inserted between a terminal one and the thousands lexicon.</param>
    /// <param name="LakhContinuingSuffix">The suffix applied when the lakh segment is followed by a remainder.</param>
    /// <param name="LakhExactSuffix">The suffix applied when the lakh segment ends the phrase.</param>
    /// <param name="CroreContinuingSuffix">The suffix applied when the crore segment is followed by a remainder.</param>
    /// <param name="HundredsContinuingSuffix">The default suffix applied when a hundreds segment is followed by a remainder.</param>
    /// <param name="HundredsExactSuffix">The default suffix applied when a hundreds segment ends the phrase.</param>
    /// <param name="HundredsNineContinuingSuffix">The suffix applied when the nine-hundreds segment is followed by a remainder.</param>
    /// <param name="HundredsNineExactSuffix">The suffix applied when the nine-hundreds segment ends the phrase.</param>
    /// <param name="UnitsMap">The units lexicon keyed by absolute value.</param>
    /// <param name="TensMap">The tens lexicon keyed by decade value.</param>
    /// <param name="HundredsMap">The hundreds lexicon keyed by digit value.</param>
    /// <param name="ThousandsMap">The exact thousands forms keyed by one-through-nineteen counts.</param>
    /// <param name="OrdinalExceptions">The exact ordinal words keyed by value.</param>
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

    readonly Profile profile = profile;

    /// <summary>
    /// Converts the number using the locale's Indian-grouping cardinal rules.
    /// </summary>
    /// <inheritdoc />
    public override string Convert(long number) =>
        ConvertImpl(number, isOrdinal: false);

    /// <summary>
    /// Converts the number using the locale's Indian-grouping ordinal rules.
    /// </summary>
    /// <inheritdoc />
    public override string ConvertToOrdinal(int number) =>
        ConvertImpl(number, isOrdinal: true);

    /// <summary>
    /// Shared rendering path for cardinal and ordinal values in the Indian-grouping family.
    /// </summary>
    string ConvertImpl(long number, bool isOrdinal)
    {
        if (number == 0)
        {
            return GetUnitValue(0, isOrdinal);
        }

        if (number < 0)
        {
            // Preserve the cardinal/ordinal mode through the negative branch so negative ordinals do
            // not silently fall back to the cardinal renderer.
            return profile.NegativeWord + " " + ConvertImpl(-number, isOrdinal);
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

    /// <summary>
    /// Returns the locale-specific unit word, applying ordinal exceptions when required.
    /// </summary>
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

    /// <summary>
    /// Returns the tens word, including suffixes for remainders, exact forms, and thousand forms.
    /// </summary>
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
            // The family distinguishes three remainder cases: nine-tens, a small set of special
            // tens, and the default remainder suffix. Those choices come from generated grammar data
            // rather than a generic "tens + unit" concatenation.
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
            // Exact thousands use a different suffix table because the rendered tens phrase is
            // about to feed directly into the thousands bridge logic below.
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

    /// <summary>
    /// Builds the lakhs segment and consumes the part of <paramref name="number"/> that it covers.
    /// </summary>
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

    /// <summary>
    /// Builds the crore segment and consumes the part of <paramref name="number"/> that it covers.
    /// </summary>
    string GetCroresValue(ref long number)
    {
        var localWord = string.Empty;
        var numberAboveTen = number / 10_000_000;

        if (numberAboveTen is > 99_999 and <= 9_999_999)
        {
            // Very large crore counts recurse through the lakh helper first because the same Indian
            // grouping grammar repeats inside the count that precedes "crore".
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

    /// <summary>
    /// Builds the thousands segment and consumes the part of <paramref name="number"/> that it covers.
    /// </summary>
    string GetThousandsValue(ref long number)
    {
        var numberAboveTen = number / 1000;
        var localWord = string.Empty;

        if (numberAboveTen >= 20)
        {
            localWord = GetTensValue(numberAboveTen, false, true);

            if (numberAboveTen % 10 == 1)
            {
                // Exact one after a higher tens-thousands phrase uses a generated bridge instead of
                // the normal thousands table so the final morphology stays natural for the locale.
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

    /// <summary>
    /// Builds the hundreds segment and consumes the part of <paramref name="number"/> that it covers.
    /// </summary>
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
}