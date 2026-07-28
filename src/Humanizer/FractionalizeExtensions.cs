using System.Numerics;

namespace Humanizer;

/// <summary>
/// Contains extension methods for converting decimals to common fractions.
/// </summary>
public static class FractionalizeExtensions
{
    /// <summary>
    /// Converts a decimal to the closest reduced fraction whose denominator does not exceed the specified maximum.
    /// </summary>
    /// <param name="input">The decimal to convert.</param>
    /// <param name="maxDenominator">The largest denominator that may be used.</param>
    /// <param name="tolerance">The maximum absolute difference allowed between the input and the fraction.</param>
    /// <param name="useUnicode">Whether to use a Unicode vulgar-fraction character when an exact character exists.</param>
    /// <returns>
    /// A whole number, proper fraction, or mixed fraction when the closest fraction is within
    /// <paramref name="tolerance"/>; otherwise, the culture-formatted input.
    /// </returns>
    /// <remarks>
    /// The tolerance boundary is inclusive. Equidistant fractions prefer the smaller denominator,
    /// then the value farther from zero. Fraction components use invariant digits and slash notation.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="maxDenominator"/> is less than one or <paramref name="tolerance"/> is negative.
    /// </exception>
    /// <example>
    /// <code>
    /// 1.25m.Fractionalize(5, 0m) => "1 1/4"
    /// 0.34m.Fractionalize(5, 0.01m) => "1/3"
    /// 0.75m.Fractionalize(4, 0m, useUnicode: true) => "¾"
    /// </code>
    /// </example>
    public static string Fractionalize(
        this decimal input,
        int maxDenominator,
        decimal tolerance,
        bool useUnicode = false)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfLessThan(maxDenominator, 1);
        ArgumentOutOfRangeException.ThrowIfNegative(tolerance);
#else
        if (maxDenominator < 1)
            throw new ArgumentOutOfRangeException(nameof(maxDenominator));
        if (tolerance < 0)
            throw new ArgumentOutOfRangeException(nameof(tolerance));
#endif

        if (decimal.Truncate(input) == input)
            return input.ToString("0", CultureInfo.InvariantCulture);

        var value = ToFraction(input);
        var approximation = LimitDenominator(BigInteger.Abs(value.Numerator), value.Denominator, maxDenominator);
        if (value.Numerator.Sign < 0)
            approximation.Numerator = -approximation.Numerator;

        if (!IsWithinTolerance(value, approximation, ToFraction(tolerance)))
            return input.ToString(LocaleNumberFormattingOverrides.GetFormattingNumberFormat(CultureInfo.CurrentCulture));

        return Format(approximation, useUnicode);
    }

    static (BigInteger Numerator, BigInteger Denominator) ToFraction(decimal value)
    {
        var bits = decimal.GetBits(value);
        var numerator =
            (BigInteger)(uint)bits[0] |
            (BigInteger)(uint)bits[1] << 32 |
            (BigInteger)(uint)bits[2] << 64;
        var denominator = BigInteger.Pow(10, (bits[3] >> 16) & 0x7F);
        var divisor = BigInteger.GreatestCommonDivisor(numerator, denominator);

        if (bits[3] < 0)
            numerator = -numerator;

        return (numerator / divisor, denominator / divisor);
    }

    static (BigInteger Numerator, BigInteger Denominator) LimitDenominator(
        BigInteger numerator,
        BigInteger denominator,
        int maxDenominator)
    {
        var originalNumerator = numerator;
        var originalDenominator = denominator;
        BigInteger previousNumerator = 0;
        BigInteger previousDenominator = 1;
        BigInteger currentNumerator = 1;
        BigInteger currentDenominator = 0;

        while (!denominator.IsZero)
        {
            var quotient = numerator / denominator;
            var nextDenominator = previousDenominator + quotient * currentDenominator;
            if (nextDenominator > maxDenominator)
                break;

            (previousNumerator, currentNumerator) =
                (currentNumerator, previousNumerator + quotient * currentNumerator);
            (previousDenominator, currentDenominator) = (currentDenominator, nextDenominator);
            (numerator, denominator) = (denominator, numerator - quotient * denominator);
        }

        if (denominator.IsZero)
            return (currentNumerator, currentDenominator);

        var multiplier = (maxDenominator - previousDenominator) / currentDenominator;
        var first = (
            previousNumerator + multiplier * currentNumerator,
            previousDenominator + multiplier * currentDenominator);
        var second = (currentNumerator, currentDenominator);

        var firstError = BigInteger.Abs(
            first.Item1 * originalDenominator - originalNumerator * first.Item2);
        var secondError = BigInteger.Abs(
            second.Item1 * originalDenominator - originalNumerator * second.Item2);
        var comparison = (firstError * second.Item2).CompareTo(secondError * first.Item2);

        if (comparison != 0)
            return comparison < 0 ? first : second;

        if (first.Item2 != second.Item2)
            return first.Item2 < second.Item2 ? first : second;

        return first.Item1 > second.Item1 ? first : second;
    }

    static bool IsWithinTolerance(
        (BigInteger Numerator, BigInteger Denominator) value,
        (BigInteger Numerator, BigInteger Denominator) approximation,
        (BigInteger Numerator, BigInteger Denominator) tolerance)
    {
        var difference = BigInteger.Abs(
            approximation.Numerator * value.Denominator -
            value.Numerator * approximation.Denominator);

        return difference * tolerance.Denominator <=
               tolerance.Numerator * value.Denominator * approximation.Denominator;
    }

    static string Format((BigInteger Numerator, BigInteger Denominator) fraction, bool useUnicode)
    {
        var sign = fraction.Numerator.Sign < 0 ? "-" : "";
        var numerator = BigInteger.Abs(fraction.Numerator);
        var whole = BigInteger.DivRem(numerator, fraction.Denominator, out var remainder);
        var wholeText = whole.ToString(CultureInfo.InvariantCulture);

        if (remainder.IsZero)
            return sign + wholeText;

        if (useUnicode && TryGetVulgarFraction(remainder, fraction.Denominator, out var vulgarFraction))
            return sign + (whole.IsZero ? "" : wholeText) + vulgarFraction;

        var fractionText =
            remainder.ToString(CultureInfo.InvariantCulture) +
            "/" +
            fraction.Denominator.ToString(CultureInfo.InvariantCulture);

        return sign + (whole.IsZero ? "" : wholeText + " ") + fractionText;
    }

    static bool TryGetVulgarFraction(BigInteger numerator, BigInteger denominator, out char fraction)
    {
        fraction = ((int)numerator, (int)denominator) switch
        {
            (1, 2) => '½',
            (1, 3) => '⅓',
            (2, 3) => '⅔',
            (1, 4) => '¼',
            (3, 4) => '¾',
            (1, 5) => '⅕',
            (2, 5) => '⅖',
            (3, 5) => '⅗',
            (4, 5) => '⅘',
            (1, 6) => '⅙',
            (5, 6) => '⅚',
            (1, 7) => '⅐',
            (1, 8) => '⅛',
            (3, 8) => '⅜',
            (5, 8) => '⅝',
            (7, 8) => '⅞',
            (1, 9) => '⅑',
            (1, 10) => '⅒',
            _ => default,
        };

        return fraction != default;
    }
}