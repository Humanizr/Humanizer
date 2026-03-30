namespace Humanizer;

internal class EastAsianPositionalWordsToNumberConverter(EastAsianPositionalWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly EastAsianPositionalWordsToNumberProfile profile = profile;

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return parsedValue;
    }

    public override bool TryConvert(string words, out int parsedValue) =>
        TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
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

        if (profile.OrdinalMap?.TryGetValue(normalized, out parsedValue) == true)
        {
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

        if (TryParse(normalized.AsSpan(), out parsedValue, out unrecognizedWord))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            return true;
        }

        parsedValue = default;
        return false;
    }

    bool TryParse(ReadOnlySpan<char> text, out int value, out string? unrecognizedWord)
    {
        if (profile.HasSingleCharacterTokens)
        {
            return TryParseSingleCharacter(text, out value, out unrecognizedWord);
        }

        return TryParseMultiCharacter(text, out value, out unrecognizedWord);
    }

    bool TryParseSingleCharacter(ReadOnlySpan<char> text, out int value, out string? unrecognizedWord)
    {
        var total = 0;
        var section = 0;
        var number = 0;
        var parsedAnyToken = false;
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
                    number = 1;
                }

                section += number * smallUnit;
                number = 0;
                parsedAnyToken = true;
                continue;
            }

            if (profile.SingleCharacterLargeUnits!.TryGetValue(character, out var largeUnit))
            {
                section += number;
                if (section == 0)
                {
                    section = 1;
                }

                total += section * largeUnit;
                section = 0;
                number = 0;
                parsedAnyToken = true;
                continue;
            }

            value = default;
            unrecognizedWord = parsedAnyToken ? firstToken : character.ToString();
            return false;
        }

        value = total + section + number;
        unrecognizedWord = null;
        return true;
    }

    bool TryParseMultiCharacter(ReadOnlySpan<char> text, out int value, out string? unrecognizedWord)
    {
        var total = 0;
        var section = 0;
        var number = 0;
        var position = 0;
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
                        number = 1;
                    }

                    section += number * tokenValue;
                    number = 0;
                    break;
                case EastAsianPositionalTokenKind.LargeUnit:
                    section += number;
                    if (section == 0)
                    {
                        section = 1;
                    }

                    total += section * tokenValue;
                    section = 0;
                    number = 0;
                    break;
                default:
                    throw new InvalidOperationException("Unsupported positional token kind.");
            }
        }

        value = total + section + number;
        unrecognizedWord = null;
        return true;
    }

    bool TryReadToken(ReadOnlySpan<char> remaining, out EastAsianPositionalTokenKind kind, out int value, out int length)
    {
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

internal sealed class EastAsianPositionalWordsToNumberProfile
{
    public EastAsianPositionalWordsToNumberProfile(
        FrozenDictionary<string, int> digits,
        FrozenDictionary<string, int> smallUnits,
        FrozenDictionary<string, int> largeUnits,
        string[] negativePrefixes,
        string ordinalPrefix,
        string ordinalSuffix,
        FrozenDictionary<string, int>? ordinalMap = null)
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

    public FrozenDictionary<string, int> Digits { get; }
    public FrozenDictionary<string, int> SmallUnits { get; }
    public FrozenDictionary<string, int> LargeUnits { get; }
    public string[] NegativePrefixes { get; }
    public string OrdinalPrefix { get; }
    public string OrdinalSuffix { get; }
    public FrozenDictionary<string, int>? OrdinalMap { get; }
    public bool HasSingleCharacterTokens { get; }
    public FrozenDictionary<char, int>? SingleCharacterDigits { get; }
    public FrozenDictionary<char, int>? SingleCharacterSmallUnits { get; }
    public FrozenDictionary<char, int>? SingleCharacterLargeUnits { get; }
    public EastAsianPositionalToken[] Tokens { get; }
}

internal enum EastAsianPositionalTokenKind
{
    Digit,
    SmallUnit,
    LargeUnit
}

internal readonly record struct EastAsianPositionalToken(string Text, int Value, EastAsianPositionalTokenKind Kind);
