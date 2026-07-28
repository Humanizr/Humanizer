namespace Humanizer;

/// <summary>
/// Parses East Asian positional number words made up of digits, small units, and large units.
/// </summary>
internal class EastAsianPositionalWordsToNumberConverter(EastAsianPositionalWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly EastAsianPositionalWordsToNumberProfile profile = profile;

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

        var normalized = words.Replace(" ", string.Empty).Trim();
        var negative = false;

        foreach (var negativePrefix in profile.NegativePrefixes)
        {
            if (!normalized.StartsWith(negativePrefix, StringComparison.Ordinal))
            {
                continue;
            }

            normalized = normalized[negativePrefix.Length..];
            negative = true;
            break;
        }

        // Exact ordinals are checked before stripping ordinal affixes because some locales encode
        // the full ordinal spelling as a standalone token that should win over the positional
        // parser.
        if (profile.OrdinalMap?.TryGetValue(normalized, out var ordinalValue) == true)
        {
            parsedValue = ordinalValue;
            unrecognizedWord = null;
            return true;
        }

        if (!string.IsNullOrEmpty(profile.OrdinalPrefix) &&
            normalized.StartsWith(profile.OrdinalPrefix, StringComparison.Ordinal))
        {
            normalized = normalized[profile.OrdinalPrefix.Length..];
        }

        if (!string.IsNullOrEmpty(profile.OrdinalSuffix) &&
            normalized.EndsWith(profile.OrdinalSuffix, StringComparison.Ordinal) &&
            normalized.Length > profile.OrdinalSuffix.Length)
        {
            normalized = normalized[..^profile.OrdinalSuffix.Length];
        }

        if (TryParse(normalized.AsSpan(), out var value, out unrecognizedWord))
        {
            parsedValue = value;
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            return true;
        }

        parsedValue = default;
        return false;
    }

    /// <summary>
    /// Parses a positional number after the input has been normalized and ordinal markers have
    /// been stripped.
    /// </summary>
    /// <param name="text">The normalized text to parse.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token or fragment that was not recognized.</param>
    /// <returns><c>true</c> if the text was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParse(ReadOnlySpan<char> text, out long value, out string? unrecognizedWord)
    {
        // Single-character locales can use a tighter parser, but only when every digit and unit
        // token is one rune wide. That keeps multi-character locales on the more general tokenizer.
        if (profile.HasSingleCharacterTokens)
        {
            return TryParseSingleCharacter(text, out value, out unrecognizedWord);
        }

        return TryParseMultiCharacter(text, out value, out unrecognizedWord);
    }

    /// <summary>
    /// Parses a positional number when every token is a single character.
    /// </summary>
    /// <param name="text">The normalized text to parse.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token or fragment that was not recognized.</param>
    /// <returns><c>true</c> if the text was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseSingleCharacter(ReadOnlySpan<char> text, out long value, out string? unrecognizedWord)
    {
        long total = 0;
        long section = 0;
        long number = 0;
        var parsedAnyToken = false;
        // Keep the first character around so a later failure can report the leading token the
        // caller likely recognizes, not just the exact character that tripped the parse.
        var firstToken = text[..1].ToString();

        foreach (var character in text)
        {
            if (profile.SingleCharacterDigits!.TryGetValue(character, out var digit))
            {
                number = digit;
                parsedAnyToken = true;
                continue;
            }

            if (profile.SingleCharacterSmallUnits!.TryGetValue(character, out var smallUnit))
            {
                if (number == 0)
                {
                    // Positional languages omit an explicit one before a unit, so "十" still means
                    // ten rather than zero tens.
                    number = 1;
                }

                section = checked(section + checked(number * smallUnit));
                number = 0;
                parsedAnyToken = true;
                continue;
            }

            if (profile.SingleCharacterLargeUnits!.TryGetValue(character, out var largeUnit))
            {
                section += number;
                if (section == 0)
                {
                    // Large units behave like "one * unit" when no smaller digit was seen in the
                    // current section.
                    section = 1;
                }

                total = checked(total + checked(section * largeUnit));
                section = 0;
                number = 0;
                parsedAnyToken = true;
                continue;
            }

            value = default;
            unrecognizedWord = parsedAnyToken ? firstToken : character.ToString();
            return false;
        }

        value = checked(total + section + number);
        unrecognizedWord = null;
        return true;
    }

    /// <summary>
    /// Parses a positional number when tokens can span multiple characters.
    /// </summary>
    /// <param name="text">The normalized text to parse.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token or fragment that was not recognized.</param>
    /// <returns><c>true</c> if the text was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseMultiCharacter(ReadOnlySpan<char> text, out long value, out string? unrecognizedWord)
    {
        long total = 0;
        long section = 0;
        long number = 0;
        var position = 0;
        // Track the first token length for the same reason as the single-character path: useful
        // failure diagnostics should point at the first recognizable token in the phrase.
        var firstTokenLength = 0;

        while (position < text.Length)
        {
            if (!TryReadToken(text[position..], out var kind, out var tokenValue, out var tokenLength))
            {
                value = default;
                unrecognizedWord = firstTokenLength > 0
                    ? text[..firstTokenLength].ToString()
                    : text[position..].ToString();
                return false;
            }

            if (firstTokenLength == 0)
            {
                firstTokenLength = tokenLength;
            }

            position += tokenLength;

            switch (kind)
            {
                case EastAsianPositionalTokenKind.Digit:
                    number = tokenValue;
                    break;
                case EastAsianPositionalTokenKind.SmallUnit:
                    if (number == 0)
                    {
                        // The unit is implied, so a bare small unit still contributes one group.
                        number = 1;
                    }

                    section = checked(section + checked(number * tokenValue));
                    number = 0;
                    break;
                case EastAsianPositionalTokenKind.LargeUnit:
                    section += number;
                    if (section == 0)
                    {
                        // Large units also imply an omitted one when they appear without a leading
                        // digit or small unit.
                        section = 1;
                    }

                    total = checked(total + checked(section * tokenValue));
                    section = 0;
                    number = 0;
                    break;
                default:
                    throw new InvalidOperationException("Unsupported positional token kind.");
            }
        }

        value = checked(total + section + number);
        unrecognizedWord = null;
        return true;
    }

    /// <summary>
    /// Tries to match the next token from the front of the remaining input.
    /// </summary>
    /// <param name="remaining">The unparsed tail of the input.</param>
    /// <param name="kind">When this method returns, the token kind.</param>
    /// <param name="value">When this method returns, the token value.</param>
    /// <param name="length">When this method returns, the matched token length.</param>
    /// <returns><c>true</c> if a token was matched; otherwise, <c>false</c>.</returns>
    bool TryReadToken(ReadOnlySpan<char> remaining, out EastAsianPositionalTokenKind kind, out long value, out int length)
    {
        // Tokens are pre-sorted longest-first, so the first match is the greedy match the locale
        // expects.
        foreach (var token in profile.Tokens)
        {
            if (!remaining.StartsWith(token.Text, StringComparison.Ordinal))
            {
                continue;
            }

            kind = token.Kind;
            value = token.Value;
            length = token.Text.Length;
            return true;
        }

        kind = default;
        value = default;
        length = default;
        return false;
    }
}

/// <summary>
/// Immutable locale data used by <see cref="EastAsianPositionalWordsToNumberConverter"/>.
/// </summary>
internal sealed class EastAsianPositionalWordsToNumberProfile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EastAsianPositionalWordsToNumberProfile"/> class.
    /// </summary>
    /// <param name="digits">The digit tokens recognized by the parser.</param>
    /// <param name="smallUnits">The small-unit tokens such as tens or hundreds.</param>
    /// <param name="largeUnits">The large-unit tokens such as thousands or ten-thousands.</param>
    /// <param name="negativePrefixes">The prefixes that mark a negative number phrase.</param>
    /// <param name="ordinalPrefix">The prefix that marks ordinal forms, if any.</param>
    /// <param name="ordinalSuffix">The suffix that marks ordinal forms, if any.</param>
    /// <param name="ordinalMap">An optional map of exact ordinal spellings.</param>
    public EastAsianPositionalWordsToNumberProfile(
        FrozenDictionary<string, long> digits,
        FrozenDictionary<string, long> smallUnits,
        FrozenDictionary<string, long> largeUnits,
        string[] negativePrefixes,
        string ordinalPrefix,
        string ordinalSuffix,
        FrozenDictionary<string, long>? ordinalMap = null)
    {
        Digits = digits;
        SmallUnits = smallUnits;
        LargeUnits = largeUnits;
        NegativePrefixes = negativePrefixes;
        OrdinalPrefix = ordinalPrefix;
        OrdinalSuffix = ordinalSuffix;
        OrdinalMap = ordinalMap;

        HasSingleCharacterTokens = Digits.Keys.All(static key => key.Length == 1) &&
            SmallUnits.Keys.All(static key => key.Length == 1) &&
            LargeUnits.Keys.All(static key => key.Length == 1);

        if (HasSingleCharacterTokens)
        {
            SingleCharacterDigits = Digits.ToFrozenDictionary(static pair => pair.Key[0], static pair => pair.Value);
            SingleCharacterSmallUnits = SmallUnits.ToFrozenDictionary(static pair => pair.Key[0], static pair => pair.Value);
            SingleCharacterLargeUnits = LargeUnits.ToFrozenDictionary(static pair => pair.Key[0], static pair => pair.Value);
        }

        Tokens = Digits.Select(static pair => new EastAsianPositionalToken(pair.Key, pair.Value, EastAsianPositionalTokenKind.Digit))
        .Concat(SmallUnits.Select(static pair => new EastAsianPositionalToken(pair.Key, pair.Value, EastAsianPositionalTokenKind.SmallUnit)))
        .Concat(LargeUnits.Select(static pair => new EastAsianPositionalToken(pair.Key, pair.Value, EastAsianPositionalTokenKind.LargeUnit)))
        .OrderByDescending(static token => token.Text.Length)
        .ThenBy(static token => token.Kind)
        .ToArray();
    }

    /// <summary>
    /// Gets the digit tokens recognized by the parser.
    /// </summary>
    public FrozenDictionary<string, long> Digits { get; }
    /// <summary>
    /// Gets the small-unit tokens recognized by the parser.
    /// </summary>
    public FrozenDictionary<string, long> SmallUnits { get; }
    /// <summary>
    /// Gets the large-unit tokens recognized by the parser.
    /// </summary>
    public FrozenDictionary<string, long> LargeUnits { get; }
    /// <summary>
    /// Gets the prefixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativePrefixes { get; }
    /// <summary>
    /// Gets the prefix that marks ordinal forms.
    /// </summary>
    public string OrdinalPrefix { get; }
    /// <summary>
    /// Gets the suffix that marks ordinal forms.
    /// </summary>
    public string OrdinalSuffix { get; }
    /// <summary>
    /// Gets the optional exact ordinal map.
    /// </summary>
    public FrozenDictionary<string, long>? OrdinalMap { get; }
    /// <summary>
    /// Gets a value indicating whether the locale uses single-character tokens.
    /// </summary>
    public bool HasSingleCharacterTokens { get; }
    /// <summary>
    /// Gets the single-character digit lookup when the locale supports it.
    /// </summary>
    public FrozenDictionary<char, long>? SingleCharacterDigits { get; }
    /// <summary>
    /// Gets the single-character small-unit lookup when the locale supports it.
    /// </summary>
    public FrozenDictionary<char, long>? SingleCharacterSmallUnits { get; }
    /// <summary>
    /// Gets the single-character large-unit lookup when the locale supports it.
    /// </summary>
    public FrozenDictionary<char, long>? SingleCharacterLargeUnits { get; }
    /// <summary>
    /// Gets the tokens ordered from longest to shortest for token matching.
    /// </summary>
    public EastAsianPositionalToken[] Tokens { get; }
}

/// <summary>
/// Describes one positional token and its role in the parser.
/// </summary>
internal enum EastAsianPositionalTokenKind
{
    /// <summary>
    /// A digit token.
    /// </summary>
    Digit,
    /// <summary>
    /// A small-unit token such as tens or hundreds.
    /// </summary>
    SmallUnit,
    /// <summary>
    /// A large-unit token such as thousands or ten-thousands.
    /// </summary>
    LargeUnit
}

/// <summary>
/// Represents one positional token, its numeric value, and its parser role.
/// </summary>
internal readonly record struct EastAsianPositionalToken(string Text, long Value, EastAsianPositionalTokenKind Kind);