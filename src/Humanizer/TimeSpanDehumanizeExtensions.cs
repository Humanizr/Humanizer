using System.Numerics;

namespace Humanizer;

/// <summary>
/// Contains extension methods for parsing invariant duration text into a <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanDehumanizeExtensions
{
    static readonly BigInteger NegativeTickLimit = BigInteger.One << 63;

    /// <summary>
    /// Parses a standard invariant <see cref="TimeSpan"/> value or compact duration tokens using
    /// <c>ms</c>, <c>s</c>, <c>m</c>, <c>h</c>, <c>d</c>, and <c>w</c>.
    /// </summary>
    /// <param name="input">The duration text to parse.</param>
    /// <returns>The parsed duration.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when <paramref name="input"/> is not a supported duration.</exception>
    /// <remarks>
    /// Compact tokens are culture-invariant, may be separated by whitespace, and may have one leading sign.
    /// Units may repeat and appear in any order; their values are added. Each token must resolve to whole ticks.
    /// A week is seven days.
    /// Colon-separated values use the standard invariant <see cref="TimeSpan"/> interpretation.
    /// </remarks>
    public static TimeSpan DehumanizeTimeSpan(this string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (TryDehumanizeTimeSpan(input, out var result))
        {
            return result;
        }

        throw new FormatException("Value is not a supported TimeSpan format.");
    }

    /// <summary>
    /// Tries to parse a standard invariant <see cref="TimeSpan"/> value or compact duration tokens using
    /// <c>ms</c>, <c>s</c>, <c>m</c>, <c>h</c>, <c>d</c>, and <c>w</c>.
    /// </summary>
    /// <param name="input">The duration text to parse.</param>
    /// <param name="result">The parsed duration, or <see cref="TimeSpan.Zero"/> when parsing fails.</param>
    /// <returns><see langword="true"/> when parsing succeeds; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Compact tokens are culture-invariant, may be separated by whitespace, and may have one leading sign.
    /// Units may repeat and appear in any order; their values are added. Each token must resolve to whole ticks.
    /// A week is seven days.
    /// Colon-separated values use the standard invariant <see cref="TimeSpan"/> interpretation.
    /// </remarks>
    public static bool TryDehumanizeTimeSpan(this string? input, out TimeSpan result)
    {
        result = default;
        if (input is null)
        {
            return false;
        }

        if (TimeSpan.TryParse(input, CultureInfo.InvariantCulture, out result))
        {
            return true;
        }

        return TryParseCompact(input, out result);
    }

    static bool TryParseCompact(string input, out TimeSpan result)
    {
        result = default;
        var index = 0;
        SkipWhitespace(input, ref index);

        var negative = false;
        if (index < input.Length && input[index] is '+' or '-')
        {
            negative = input[index++] == '-';
            SkipWhitespace(input, ref index);
        }

        var parsedToken = false;
        BigInteger ticks = 0;
        while (index < input.Length)
        {
            var numberStart = index;
            var digitCount = 0;
            var fractionStart = -1;
            while (index < input.Length && IsAsciiDigit(input[index]))
            {
                index++;
                digitCount++;
            }

            if (index < input.Length && input[index] == '.')
            {
                index++;
                fractionStart = index;
                while (index < input.Length && IsAsciiDigit(input[index]))
                {
                    index++;
                    digitCount++;
                }

                if (fractionStart == index)
                    return false;
            }

            if (digitCount == 0)
            {
                return false;
            }

            var numberEnd = index;
            SkipWhitespace(input, ref index);
            if (!TryReadUnit(input, ref index, out var ticksPerUnit))
            {
                return false;
            }

            if (!TryGetExactTicks(input, numberStart, numberEnd, fractionStart, ticksPerUnit, out var tokenTicks))
            {
                return false;
            }

            ticks += tokenTicks;
            parsedToken = true;
            SkipWhitespace(input, ref index);
        }

        var limit = negative ? NegativeTickLimit : new BigInteger(long.MaxValue);
        if (!parsedToken || ticks > limit)
        {
            return false;
        }

        if (negative && ticks == NegativeTickLimit)
        {
            result = TimeSpan.MinValue;
            return true;
        }

        var signedTicks = (long)ticks;
        result = TimeSpan.FromTicks(negative ? -signedTicks : signedTicks);
        return true;
    }

    static bool TryReadUnit(string input, ref int index, out long ticksPerUnit)
    {
        ticksPerUnit = 0;
        if (index >= input.Length)
        {
            return false;
        }

        switch (input[index++])
        {
            case 'm' when index < input.Length && input[index] == 's':
                index++;
                ticksPerUnit = TimeSpan.TicksPerMillisecond;
                return true;
            case 's':
                ticksPerUnit = TimeSpan.TicksPerSecond;
                return true;
            case 'm':
                ticksPerUnit = TimeSpan.TicksPerMinute;
                return true;
            case 'h':
                ticksPerUnit = TimeSpan.TicksPerHour;
                return true;
            case 'd':
                ticksPerUnit = TimeSpan.TicksPerDay;
                return true;
            case 'w':
                ticksPerUnit = 7 * TimeSpan.TicksPerDay;
                return true;
            default:
                return false;
        }
    }

    static void SkipWhitespace(string input, ref int index)
    {
        while (index < input.Length && char.IsWhiteSpace(input[index]))
        {
            index++;
        }
    }

    static bool IsAsciiDigit(char value) =>
        value is >= '0' and <= '9';

    static bool TryGetExactTicks(
        string input,
        int start,
        int end,
        int fractionStart,
        long ticksPerUnit,
        out BigInteger ticks)
    {
        ticks = 0;
        var normalizedEnd = end;
        while (fractionStart >= 0 && normalizedEnd > fractionStart && input[normalizedEnd - 1] == '0')
        {
            normalizedEnd--;
        }

        if (fractionStart >= 0 && normalizedEnd - fractionStart > 28)
        {
            return false;
        }

        BigInteger coefficient = 0;
        var significantDigits = 0;
        var foundNonZero = false;
        for (var index = start; index < normalizedEnd; index++)
        {
            var value = input[index];
            if (value == '.')
            {
                continue;
            }

            foundNonZero |= value != '0';
            if (foundNonZero && ++significantDigits > 28)
            {
                return false;
            }

            coefficient = coefficient * 10 + (value - '0');
        }

        var scale = fractionStart < 0 ? 0 : normalizedEnd - fractionStart;
        ticks = BigInteger.DivRem(
            coefficient * ticksPerUnit,
            BigInteger.Pow(10, scale),
            out var remainder);
        return remainder.IsZero;
    }
}