namespace Humanizer;

/// <summary>
/// Shared parser for Malay-style contracted scale systems where dedicated tokens encode
/// "one hundred" or "one thousand" and the rest of the phrase is composed by multiplying or
/// adding token values in sequence.
///
/// The parser normalizes punctuation and casing, removes the configured negative prefix, then
/// walks the token stream from left to right. Contracted tens and teens such as <c>puluh</c> and
/// <c>belas</c> are interpreted as structural operators over the current accumulated value rather
/// than as plain additive words. Locale data can therefore express native high-range phrases and
/// parse them back through the same token map.
/// </summary>
class ContractedScaleWordsToNumberConverter(ContractedScaleWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    /// <summary>
    /// Immutable parser profile emitted from locale YAML. It owns the literal minus word and the
    /// cardinal token dictionary used by the state machine below.
    /// </summary>
    readonly ContractedScaleWordsToNumberProfile profile = profile;

    /// <inheritdoc />
    public override long Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return parsedValue;
    }

    /// <inheritdoc />
    public override bool TryConvert(string words, out long parsedValue) =>
        TryConvert(words, out parsedValue, out _);

    /// <inheritdoc />
    public override bool TryConvert(string words, out long parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        var normalized = Normalize(words);
        var negative = false;

        // Negative parsing is kept outside the token state machine so the main loop can stay
        // focused on contracted scale composition.
        if (normalized.StartsWith(profile.MinusWord + " ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized[(profile.MinusWord.Length + 1)..].Trim();
        }

        if (TryParseCardinal(normalized, out var value))
        {
            parsedValue = value;
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = WordsToNumberTokenizer.GetLastTokenOrSelf(normalized);
        parsedValue = default;
        return false;
    }

    /// <summary>
    /// Normalizes punctuation, hyphenation, casing, and repeated whitespace before token parsing.
    /// </summary>
    /// <remarks>
    /// This keeps the parser semantics tied to lexical meaning rather than to user-specific input
    /// formatting.
    /// </remarks>
    protected virtual string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    /// <summary>
    /// Parses a normalized cardinal phrase that may contain contracted tens, teens, and scale
    /// tokens.
    /// </summary>
    /// <param name="words">A normalized phrase ready for token-by-token parsing.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCardinal(string words, out long value)
    {
        if (profile.Cardinals.TryGetValue(words, out value))
        {
            return true;
        }

        long total = 0;
        long current = 0;

        // The main parsing loop is a tiny state machine:
        // - additive tokens increase the current local group
        // - contracted teen/tens tokens reshape the current local group
        // - hundred tokens multiply the current group by 100
        // - large scales flush the current group into the running total
        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

            if (token == "dan")
            {
                continue;
            }

            if (!profile.Cardinals.TryGetValue(token, out var tokenValue))
            {
                value = default;
                return false;
            }

            if (token == "belas")
            {
                current = checked((current == 0 ? 1 : current) + 10);
            }
            else if (token == "puluh")
            {
                current = checked((current == 0 ? 1 : current) * 10);
            }
            else if (tokenValue >= 1000)
            {
                total = checked(total + checked((current == 0 ? 1 : current) * tokenValue));
                current = 0;
            }
            else if (tokenValue == 100)
            {
                current = checked((current == 0 ? 1 : current) * tokenValue);
            }
            else
            {
                current = checked(current + tokenValue);
            }
        }

        value = checked(total + current);
        return true;
    }
}

/// <summary>
/// Immutable locale data for <see cref="ContractedScaleWordsToNumberConverter"/>.
/// </summary>
sealed class ContractedScaleWordsToNumberProfile(string minusWord, FrozenDictionary<string, long> cardinals)
{
    /// <summary>
    /// Gets the literal word that marks a negative number phrase.
    /// </summary>
    public string MinusWord { get; } = minusWord;

    /// <summary>
    /// Gets the token-to-value dictionary used by the contracted scale parser.
    /// </summary>
public FrozenDictionary<string, long> Cardinals { get; } = cardinals;
}
