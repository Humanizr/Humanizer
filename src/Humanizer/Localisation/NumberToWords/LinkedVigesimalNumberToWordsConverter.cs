namespace Humanizer;

/// <summary>
/// Renders linked-vigesimal number systems that have lexicalized lower numbers and scale nouns
/// that lead their count words.
/// </summary>
class LinkedVigesimalNumberToWordsConverter(LinkedVigesimalNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly LinkedVigesimalNumberToWordsProfile profile = profile;

    /// <inheritdoc />
    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            var magnitude = number == long.MinValue
                ? (ulong)long.MaxValue + 1UL
                : (ulong)-number;
            return profile.NegativeWord + profile.NegativeJoiner + ConvertPositive(magnitude);
        }

        return ConvertPositive((ulong)number);
    }

    /// <inheritdoc />
    public override string ConvertToOrdinal(int number)
    {
        if (number < 0)
        {
            var magnitude = number == int.MinValue
                ? (ulong)int.MaxValue + 1UL
                : (ulong)-number;
            return profile.NegativeWord + profile.NegativeJoiner + ConvertPositiveToOrdinal(magnitude);
        }

        return ConvertPositiveToOrdinal((ulong)number);
    }

    string ConvertPositiveToOrdinal(ulong number)
    {
        if (number <= int.MaxValue && profile.OrdinalExceptions.TryGetValue((int)number, out var ordinal))
        {
            return ordinal;
        }

        return ConvertPositive(number) + profile.OrdinalSuffix;
    }

    string ConvertPositive(ulong number)
    {
        if (number < (ulong)profile.Words.Length)
        {
            return profile.Words[(int)number];
        }

        var parts = new List<string>();
        var remainder = number;
        foreach (var scale in profile.Scales)
        {
            var scaleValue = (ulong)scale.Value;
            var count = remainder / scaleValue;
            if (count == 0)
            {
                continue;
            }

            remainder %= scaleValue;
            parts.Add(FormatScale(count, scale, remainder > 0));
        }

        if (parts.Count == 0)
        {
            throw new InvalidOperationException("Linked-vigesimal number profiles require a lexical word table or scale row covering the requested value.");
        }

        if (remainder > 0)
        {
            var renderedRemainder = ConvertPositive(remainder);
            if (remainder < (ulong)profile.TerminalRemainderThreshold)
            {
                parts.Add(profile.GetTerminalRemainderJoiner(renderedRemainder));
            }

            parts.Add(renderedRemainder);
        }

        return string.Join(profile.PartJoiner, parts);
    }

    string FormatScale(ulong count, LinkedVigesimalScale scale, bool hasRemainder)
    {
        if (count == 1)
        {
            return hasRemainder ? scale.OneWithRemainder : scale.One;
        }

        if (count <= int.MaxValue)
        {
            var intCount = (int)count;
            if (hasRemainder && scale.CountOverridesWithRemainder.TryGetValue(intCount, out var overrideWithRemainder))
            {
                return overrideWithRemainder;
            }

            if (scale.CountOverrides.TryGetValue(intCount, out var overrideWord))
            {
                return overrideWord;
            }
        }

        var name = hasRemainder && scale.NameWithRemainder.Length > 0 ? scale.NameWithRemainder : scale.Name;
        return name + scale.CountJoiner + ConvertPositive(count);
    }
}

/// <summary>
/// Immutable generated profile for <see cref="LinkedVigesimalNumberToWordsConverter"/>.
/// </summary>
sealed class LinkedVigesimalNumberToWordsProfile(
    string zeroWord,
    string negativeWord,
    string negativeJoiner,
    string partJoiner,
    string terminalRemainderJoiner,
    int terminalRemainderThreshold,
    string ordinalSuffix,
    string[] words,
    LinkedVigesimalScale[] scales,
    FrozenDictionary<int, string>? ordinalExceptions = null,
    string terminalRemainderAlternateJoiner = "",
    string terminalRemainderAlternateJoinerInitials = "")
{
    /// <summary>Gets the word used for zero.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used for negative numbers.</summary>
    public string NegativeWord { get; } = negativeWord;
    /// <summary>Gets the joiner after the negative word.</summary>
    public string NegativeJoiner { get; } = negativeJoiner;
    /// <summary>Gets the joiner inserted between rendered parts.</summary>
    public string PartJoiner { get; } = partJoiner;
    /// <summary>Gets the linker inserted before terminal low remainders.</summary>
    public string TerminalRemainderJoiner { get; } = terminalRemainderJoiner;
    /// <summary>Gets the alternate linker inserted before matching terminal low remainders.</summary>
    public string TerminalRemainderAlternateJoiner { get; } = terminalRemainderAlternateJoiner;
    /// <summary>Gets the terminal low-remainder initials that use the alternate linker.</summary>
    public string TerminalRemainderAlternateJoinerInitials { get; } = terminalRemainderAlternateJoinerInitials;
    /// <summary>Gets the threshold below which terminal remainders receive the linker.</summary>
    public int TerminalRemainderThreshold { get; } = terminalRemainderThreshold;
    /// <summary>Gets the fallback ordinal suffix.</summary>
    public string OrdinalSuffix { get; } = ordinalSuffix;
    /// <summary>Gets lexicalized natural words from zero upward.</summary>
    public string[] Words { get; } = ValidateWords(words);
    /// <summary>Gets descending scale rows.</summary>
    public LinkedVigesimalScale[] Scales { get; } = ValidateScales(scales);
    /// <summary>Gets exact ordinal words keyed by numeric value.</summary>
    public FrozenDictionary<int, string> OrdinalExceptions { get; } = ordinalExceptions ?? FrozenDictionary<int, string>.Empty;

    /// <summary>Gets the terminal low-remainder linker for the rendered remainder.</summary>
    public string GetTerminalRemainderJoiner(string renderedRemainder)
    {
        if (TerminalRemainderAlternateJoiner.Length > 0 &&
            renderedRemainder.Length > 0 &&
            TerminalRemainderAlternateJoinerInitials.Contains(renderedRemainder[0]))
        {
            return TerminalRemainderAlternateJoiner;
        }

        return TerminalRemainderJoiner;
    }

    static string[] ValidateWords(string[] value)
    {
        if (value.Length == 0)
        {
            throw new InvalidOperationException("Linked-vigesimal number profiles require at least a zero word.");
        }

        return value;
    }

    static LinkedVigesimalScale[] ValidateScales(LinkedVigesimalScale[] value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            if (value[i].Value <= 0)
            {
                throw new InvalidOperationException("Linked-vigesimal number profiles require positive scale values.");
            }

            if (i > 0 && value[i - 1].Value <= value[i].Value)
            {
                throw new InvalidOperationException("Linked-vigesimal number profiles require descending scales.");
            }
        }

        return value;
    }
}

/// <summary>
/// One descending scale row for linked-vigesimal renderers and parsers.
/// </summary>
/// <param name="Value">The numeric scale value.</param>
/// <param name="One">The exact singular scale phrase.</param>
/// <param name="OneWithRemainder">The singular scale phrase when followed by a lower-order remainder.</param>
/// <param name="Name">The scale noun used with generated counts.</param>
/// <param name="NameWithRemainder">The scale noun used with generated counts when followed by a lower-order remainder.</param>
/// <param name="CountJoiner">The joiner between scale noun and generated count.</param>
/// <param name="CountOverrides">Exact scale-count phrases keyed by count.</param>
/// <param name="CountOverridesWithRemainder">Exact scale-count phrases keyed by count when followed by a lower-order remainder.</param>
readonly record struct LinkedVigesimalScale(
    long Value,
    string One,
    string OneWithRemainder,
    string Name,
    string NameWithRemainder,
    string CountJoiner,
    FrozenDictionary<int, string> CountOverrides,
    FrozenDictionary<int, string> CountOverridesWithRemainder);