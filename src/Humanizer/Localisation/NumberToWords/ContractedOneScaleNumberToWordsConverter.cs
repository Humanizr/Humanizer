using System.Collections.Generic;

namespace Humanizer;

/// <summary>
/// Shared renderer for Malay-style systems where "one + scale" may collapse into a dedicated
/// lexical item such as "seratus" or "seribu".
///
/// The algorithm is straightforward:
/// - walk the descending scale rows
/// - if the count is one and the scale provides a dedicated one-word contraction, emit that token
/// - otherwise render the count under one thousand and append the scale name
/// - finish by rendering the terminal under-thousand remainder
///
/// The expected result is a natural contracted scale phrase where scale-level one-forms come from
/// generated data instead of hardcoded thousand-specific branches.
/// </summary>
class ContractedOneScaleNumberToWordsConverter(ContractedOneScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the contracted scale lexicon.
    /// </summary>
    readonly ContractedOneScaleNumberToWordsProfile profile = profile;

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number)}";
        }

        return ConvertPositive(number);
    }

    // Positive rendering is a simple descending scale walk. The only family-specific decision is
    // whether a scale row has a dedicated contracted one-word form.
    string ConvertPositive(long number)
    {
        var parts = new List<string>();

        foreach (var scale in profile.Scales)
        {
            if (number < scale.Value)
            {
                continue;
            }

            var part = number / scale.Value;
            number %= scale.Value;

            if (part == 0)
            {
                continue;
            }

            // Some Malay-family locales contract "one + scale" into a dedicated word such as "seribu".
            if (part == 1 && scale.OneWord is not null)
            {
                parts.Add(scale.OneWord);
            }
            else
            {
                parts.Add($"{ConvertUnderThousand(part)} {scale.Name}");
            }
        }

        if (number > 0)
        {
            parts.Add(ConvertUnderThousand(number));
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Gets the hundreds phrase for the current count. Derived types may override this when a
    /// locale needs a slightly different hundred contraction while still fitting the family.
    /// </summary>
    protected virtual string GetHundredsWord(long count) =>
        count == 1 ? profile.HundredWord : $"{profile.Units[count]} {profile.HundredUnitWord}";

    // Hundreds remain shared; only the singular one-word form and scale words vary by generated data.
    string ConvertUnderThousand(long number)
    {
        if (number < 100)
        {
            return ConvertUnderHundred(number);
        }

        var hundreds = number / 100;
        var remainder = number % 100;
        var parts = new List<string> { GetHundredsWord(hundreds) };

        if (remainder > 0)
        {
            parts.Add(ConvertUnderHundred(remainder));
        }

        return string.Join(" ", parts);
    }

    // Under one hundred, the family is regular: look up direct units for 0-19, then combine a
    // tens word with the unit remainder.
    string ConvertUnderHundred(long number)
    {
        if (number < profile.Units.Length)
        {
            return profile.Units[number];
        }

        var tens = (int)(number / 10);
        var remainder = number % 10;
        var tensWord = profile.Tens[tens];

        return remainder == 0
            ? tensWord
            : $"{tensWord} {profile.Units[remainder]}";
    }

    public override string ConvertToOrdinal(int number) => Convert(number);

    internal readonly struct Scale
    {
        public long Value { get; }
        public string Name { get; }
        public string? OneWord { get; }

        /// <summary>
        /// Initializes one descending scale row.
        /// </summary>
        public Scale(long value, string name, string? oneWord = null)
        {
            Value = value;
            Name = name;
            OneWord = oneWord;
        }
    }
}

/// <summary>
/// Immutable generated profile for <see cref="ContractedOneScaleNumberToWordsConverter"/>.
/// </summary>
sealed class ContractedOneScaleNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string hundredWord,
    string hundredUnitWord,
    string[] units,
    string[] tens,
    ContractedOneScaleNumberToWordsConverter.Scale[] scales)
{
    /// <summary>
    /// Gets the cardinal zero word.
    /// </summary>
    public string ZeroWord { get; } = zeroWord;

    /// <summary>
    /// Gets the word used to prefix negative values.
    /// </summary>
    public string MinusWord { get; } = minusWord;

    /// <summary>
    /// Gets the dedicated one-hundred word.
    /// </summary>
    public string HundredWord { get; } = hundredWord;

    /// <summary>
    /// Gets the word used when a non-one count precedes the hundred scale.
    /// </summary>
    public string HundredUnitWord { get; } = hundredUnitWord;

    /// <summary>
    /// Gets the units lexicon used under one hundred.
    /// </summary>
    public string[] Units { get; } = units;

    /// <summary>
    /// Gets the tens lexicon used under one hundred.
    /// </summary>
    public string[] Tens { get; } = tens;

    /// <summary>
    /// Gets the descending scale rows used for positive rendering.
    /// </summary>
    public ContractedOneScaleNumberToWordsConverter.Scale[] Scales { get; } = scales;
}
