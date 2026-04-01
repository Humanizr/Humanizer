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

    /// <summary>
    /// Converts the given value using the locale's contracted-one scale rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
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

    /// <summary>
    /// Converts a positive number using the descending scale walk.
    ///
    /// The only family-specific decision is whether a scale row has a dedicated contracted
    /// one-word form.
    /// </summary>
    string ConvertPositive(long number)
    {
        var parts = new List<string>();

        // Scale rows are processed largest to smallest so a singular contraction such as
        // "seribu" can be emitted only when the generated profile says the locale has one.
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
                // Non-singular counts keep the recursive under-thousand form plus the scale name.
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
    /// Gets the hundreds phrase for the current count.
    /// </summary>
    /// <param name="count">The hundreds count within the current triad.</param>
    /// <returns>The localized hundreds phrase for <paramref name="count"/>.</returns>
    protected virtual string GetHundredsWord(long count) =>
        count == 1 ? profile.HundredWord : $"{profile.Units[count]} {profile.HundredUnitWord}";

    /// <summary>
    /// Converts a number below one thousand.
    /// </summary>
    string ConvertUnderThousand(long number)
    {
        if (number < 100)
        {
            return ConvertUnderHundred(number);
        }

        // Hundreds are the only place where the family needs a special dedicated one-word form.
        var hundreds = number / 100;
        var remainder = number % 100;
        var parts = new List<string> { GetHundredsWord(hundreds) };

        if (remainder > 0)
        {
            parts.Add(ConvertUnderHundred(remainder));
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Converts a number below one hundred.
    /// </summary>
    string ConvertUnderHundred(long number)
    {
        if (number < profile.Units.Length)
        {
            // Units and teens are stored as exact table lookups so the common path stays simple.
            return profile.Units[number];
        }

        var tens = (int)(number / 10);
        var remainder = number % 10;
        var tensWord = profile.Tens[tens];

        // Tens are a straight tens-word + unit-word combination when the number is not a direct
        // table lookup.
        return remainder == 0
            ? tensWord
            : $"{tensWord} {profile.Units[remainder]}";
    }

    /// <summary>
    /// Converts the given value to its locale-specific ordinal form.
    /// </summary>
    /// <remarks>
    /// This family uses the same rendering for cardinals and ordinals.
    /// </remarks>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number) => Convert(number);

    /// <summary>
    /// One descending scale row for <see cref="ContractedOneScaleNumberToWordsConverter"/>.
    /// </summary>
    internal readonly struct Scale
    {
        /// <summary>
        /// Gets the scale value.
        /// </summary>
        public long Value { get; }
        /// <summary>
        /// Gets the scale name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the optional one-word contraction for a singular scale.
        /// </summary>
        public string? OneWord { get; }

        /// <summary>
        /// Initializes one descending scale row.
        /// </summary>
        /// <param name="value">The numeric value represented by the scale.</param>
        /// <param name="name">The scale word used when the count is not contracted.</param>
        /// <param name="oneWord">The optional dedicated contraction used when the count is exactly one.</param>
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
/// <param name="zeroWord">The cardinal zero word.</param>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="hundredWord">The dedicated one-hundred word.</param>
/// <param name="hundredUnitWord">The hundred scale word used after non-one counts.</param>
/// <param name="units">The units and teens lexicon.</param>
/// <param name="tens">The tens lexicon.</param>
/// <param name="scales">The descending scale rows used during positive rendering.</param>
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
