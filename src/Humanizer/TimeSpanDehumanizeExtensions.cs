namespace Humanizer;

/// <summary>
/// Parses compact, human-friendly duration strings into <see cref="TimeSpan"/> values.
/// </summary>
public static class TimeSpanDehumanizeExtensions
{
    /// <summary>
    /// Parses a duration string into a <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="input">The duration text to parse.</param>
    /// <param name="options">Optional parsing options.</param>
    /// <returns>The parsed duration.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when <paramref name="input"/> cannot be parsed.</exception>
    public static TimeSpan DehumanizeTimeSpan(this string input, TimeSpanDehumanizeOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (!input.TryDehumanizeTimeSpan(out var result, options))
        {
            throw new FormatException($"Could not parse '{input}' as a duration.");
        }

        return result;
    }

    /// <summary>
    /// Attempts to parse a duration string into a <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="input">The duration text to parse.</param>
    /// <param name="result">The parsed duration when successful.</param>
    /// <param name="options">Optional parsing options.</param>
    /// <returns><c>true</c> when parsing succeeds; otherwise <c>false</c>.</returns>
    public static bool TryDehumanizeTimeSpan(
        this string input,
        out TimeSpan result,
        TimeSpanDehumanizeOptions? options = null)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var text = input.Trim();
        options ??= TimeSpanDehumanizeOptions.Default;

        try
        {
            return TryParseUnitTokens(text, out result)
                   || TryParseColonDuration(text, options.ColonFormat, out result)
                   || TimeSpan.TryParse(text, CultureInfo.InvariantCulture, out result);
        }
        catch (OverflowException)
        {
            result = default;
            return false;
        }
    }

    static bool TryParseUnitTokens(string input, out TimeSpan result)
    {
        result = default;
        var index = 0;
        var parsedAny = false;

        while (index < input.Length)
        {
            SkipTokenSeparators(input, ref index);

            if (index >= input.Length)
            {
                break;
            }

            var numberStart = index;
            while (index < input.Length && (char.IsDigit(input[index]) || input[index] is '.' or '-'))
            {
                index++;
            }

            if (numberStart == index
                || !double.TryParse(
                    input.AsSpan(numberStart, index - numberStart),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var amount))
            {
                return false;
            }

            SkipTokenSeparators(input, ref index);

            if (!TryReadUnit(input, ref index, out var unit))
            {
                return false;
            }

            if (!TryConvertUnit(amount, unit, out var contribution))
            {
                return false;
            }

            result += contribution;
            parsedAny = true;
        }

        return parsedAny;
    }

    static void SkipTokenSeparators(string input, ref int index)
    {
        while (index < input.Length)
        {
            while (index < input.Length && char.IsWhiteSpace(input[index]))
            {
                index++;
            }

            if (index < input.Length && input[index] == ',')
            {
                index++;
                continue;
            }

            if (TrySkipCaseInsensitiveWord(input, ref index, "and"))
            {
                continue;
            }

            break;
        }
    }

    static bool TrySkipCaseInsensitiveWord(string input, ref int index, string word)
    {
        if (index + word.Length > input.Length
            || !input.AsSpan(index, word.Length).Equals(word.AsSpan(), StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var after = index + word.Length;
        if (after < input.Length && char.IsLetter(input[after]))
        {
            return false;
        }

        index = after;
        return true;
    }

    static bool TryReadUnit(string input, ref int index, out ReadOnlyMemory<char> unit)
    {
        unit = default;
        if (index >= input.Length || !char.IsLetter(input[index]))
        {
            return false;
        }

        var start = index;
        while (index < input.Length && char.IsLetter(input[index]))
        {
            index++;
        }

        unit = input.AsMemory(start, index - start);
        return true;
    }

    static bool TryConvertUnit(double amount, ReadOnlyMemory<char> unitMemory, out TimeSpan result)
    {
        result = default;
        var unit = unitMemory.Span;

        if (UnitEquals(unit, "ms") || UnitEquals(unit, "millis") || UnitEquals(unit, "millisecond") || UnitEquals(unit, "milliseconds"))
        {
            result = TimeSpan.FromMilliseconds(amount);
            return true;
        }

        if (UnitEquals(unit, "s") || UnitEquals(unit, "sec") || UnitEquals(unit, "secs") || UnitEquals(unit, "second") || UnitEquals(unit, "seconds"))
        {
            result = TimeSpan.FromSeconds(amount);
            return true;
        }

        if (UnitEquals(unit, "m") || UnitEquals(unit, "min") || UnitEquals(unit, "mins") || UnitEquals(unit, "minute") || UnitEquals(unit, "minutes"))
        {
            result = TimeSpan.FromMinutes(amount);
            return true;
        }

        if (UnitEquals(unit, "h") || UnitEquals(unit, "hr") || UnitEquals(unit, "hrs") || UnitEquals(unit, "hour") || UnitEquals(unit, "hours"))
        {
            result = TimeSpan.FromHours(amount);
            return true;
        }

        if (UnitEquals(unit, "d") || UnitEquals(unit, "day") || UnitEquals(unit, "days"))
        {
            result = TimeSpan.FromDays(amount);
            return true;
        }

        if (UnitEquals(unit, "w") || UnitEquals(unit, "week") || UnitEquals(unit, "weeks"))
        {
            result = TimeSpan.FromDays(7 * amount);
            return true;
        }

        return false;
    }

    static bool UnitEquals(ReadOnlySpan<char> unit, string expected)
    {
        return unit.Length == expected.Length
               && unit.Equals(expected.AsSpan(), StringComparison.OrdinalIgnoreCase);
    }

    static bool TryParseColonDuration(string input, TimeSpanDehumanizeColonFormat colonFormat, out TimeSpan result)
    {
        result = default;
        var parts = input.Split(':');
        if (parts.Length is not (2 or 3) || parts.Any(string.IsNullOrWhiteSpace))
        {
            return false;
        }

        if (parts.Length == 3
            && TryParseInvariantInt(parts[0], out var hours)
            && TryParseInvariantInt(parts[1], out var minutes)
            && TryParseInvariantDouble(parts[2], out var seconds)
            && hours >= 0 && minutes >= 0 && seconds >= 0)
        {
            result = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
            return true;
        }

        if (parts.Length == 2)
        {
            if (colonFormat == TimeSpanDehumanizeColonFormat.MinutesSeconds
                && TryParseInvariantInt(parts[0], out var minutesOnly)
                && TryParseInvariantDouble(parts[1], out var secondsOnly)
                && minutesOnly >= 0 && secondsOnly >= 0)
            {
                result = TimeSpan.FromMinutes(minutesOnly) + TimeSpan.FromSeconds(secondsOnly);
                return true;
            }

            if (TryParseInvariantInt(parts[0], out var hoursOnly)
                && TryParseInvariantDouble(parts[1], out var minutesOnlyFromHours)
                && hoursOnly >= 0 && minutesOnlyFromHours >= 0)
            {
                result = TimeSpan.FromHours(hoursOnly) + TimeSpan.FromMinutes(minutesOnlyFromHours);
                return true;
            }
        }

        return false;
    }

    static bool TryParseInvariantInt(string value, out int result) =>
        int.TryParse(value.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result);

    static bool TryParseInvariantDouble(string value, out double result) =>
        double.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
}
