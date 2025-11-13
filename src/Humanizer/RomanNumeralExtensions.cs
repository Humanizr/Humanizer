// Done by Jesse Slicer https://github.com/jslicer

using System.Diagnostics;

namespace Humanizer;

/// <summary>
/// Contains extension methods for changing a number to Roman representation (ToRoman) and from Roman representation back to the number (FromRoman)
/// </summary>
public static partial class RomanNumeralExtensions
{
    static readonly KeyValuePair<string, int>[] RomanNumeralsSequence =
    [
        new("M", 1000),
        new("CM", 900),
        new("D", 500 ),
        new("CD", 400),
        new("C", 100 ),
        new("XC", 90 ),
        new("L", 50  ),
        new("XL", 40 ),
        new("X", 10  ),
        new("IX", 9  ),
        new("V", 5   ),
        new("IV", 4  ),
        new("I", 1   ),
    ];

    static int GetRomanNumeralCharValue(char c)
    {
        Debug.Assert(char.ToUpperInvariant(c) is 'M' or 'D' or 'C' or 'L' or 'X' or 'V' or 'I', "Invalid Roman numeral character");
        return (c & ~0x20) switch
        {
            'M' => 1000,
            'D' => 500,
            'C' => 100,
            'L' => 50,
            'X' => 10,
            'V' => 5,
            _ => 1,
        };
    }

    private const string ValidRomanNumeralPattern = @"^(?i:(?=[MDCLXVI])((M{0,3})((C[DM])|(D?C{0,3}))?((X[LC])|(L?XX{0,2})|L)?((I[VX])|(V?(II{0,2}))|V)?))$";

#if NET7_0_OR_GREATER
    [GeneratedRegex(ValidRomanNumeralPattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ValidRomanNumeralGenerated();
    
    private static Regex ValidRomanNumeral() => ValidRomanNumeralGenerated();
#else
    private static readonly Regex ValidRomanNumeralRegex = new(
        ValidRomanNumeralPattern,
        RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static Regex ValidRomanNumeral() => ValidRomanNumeralRegex;
#endif

    /// <summary>
    /// Converts a Roman numeral string to its integer representation.
    /// </summary>
    /// <param name="input">The Roman numeral string to convert (e.g., "XIV", "MCMXC"). Must not be null.</param>
    /// <returns>
    /// The integer value represented by the Roman numeral.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is empty or contains an invalid Roman numeral format.
    /// </exception>
    /// <remarks>
    /// Valid Roman numerals use the characters M, D, C, L, X, V, and I (case-insensitive).
    /// Supports subtractive notation (e.g., IV = 4, IX = 9, XL = 40, XC = 90, CD = 400, CM = 900).
    /// Valid range is 1 to 3999.
    /// </remarks>
    /// <example>
    /// <code>
    /// "XIV".FromRoman() => 14
    /// "MCMXC".FromRoman() => 1990
    /// "IV".FromRoman() => 4
    /// "MMXXIII".FromRoman() => 2023
    /// </code>
    /// </example>
    public static int FromRoman(this string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return FromRoman(input.AsSpan());
    }

    /// <summary>
    /// Converts a Roman numeral character span to its integer representation.
    /// </summary>
    /// <param name="input">The Roman numeral character span to convert. Must not be empty.</param>
    /// <returns>
    /// The integer value represented by the Roman numeral.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is empty (after trimming) or contains an invalid Roman numeral format.
    /// </exception>
    /// <remarks>
    /// This is a memory-efficient overload that works with character spans to avoid string allocations.
    /// Valid Roman numerals use the characters M, D, C, L, X, V, and I (case-insensitive).
    /// Supports subtractive notation (e.g., IV = 4, IX = 9).
    /// </remarks>
    /// <example>
    /// <code>
    /// "XIV".AsSpan().FromRoman() => 14
    /// "MCMXC".AsSpan().FromRoman() => 1990
    /// </code>
    /// </example>
    public static int FromRoman(CharSpan input)
    {
        input = input.Trim();

        var length = input.Length;

        if (length == 0 || IsInvalidRomanNumeral(input))
        {
            throw new ArgumentException("Empty or invalid Roman numeral string.", nameof(input));
        }

        var total = 0;
        var i = length;

        while (i > 0)
        {
            var digit = GetRomanNumeralCharValue(input[--i]);
            if (i > 0)
            {
                var previousDigit = GetRomanNumeralCharValue(input[i - 1]);
                if (previousDigit < digit)
                {
                    digit -= previousDigit;
                    i--;
                }
            }

            total += digit;
        }

        return total;
    }

    /// <summary>
    /// Converts an integer to its Roman numeral representation.
    /// </summary>
    /// <param name="input">The integer value to convert. Must be between 1 and 3999 inclusive.</param>
    /// <returns>
    /// A string containing the Roman numeral representation of the input value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="input"/> is less than 1 or greater than 3999.
    /// Roman numerals are traditionally limited to this range.
    /// </exception>
    /// <remarks>
    /// Uses standard Roman numeral notation including subtractive notation for 4, 9, 40, 90, 400, and 900.
    /// The implementation is optimized for performance and avoids string allocations where possible.
    /// </remarks>
    /// <example>
    /// <code>
    /// 14.ToRoman() => "XIV"
    /// 1990.ToRoman() => "MCMXC"
    /// 4.ToRoman() => "IV"
    /// 2023.ToRoman() => "MMXXIII"
    /// 3999.ToRoman() => "MMMCMXCIX"
    /// </code>
    /// </example>
    public static string ToRoman(this int input)
    {
        const int minValue = 1;
        const int maxValue = 3999;
        const int maxRomanNumeralLength = 15;

        if (input is < minValue or > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(input));
        }

        Span<char> builder = stackalloc char[maxRomanNumeralLength];
        var pos = 0;

        foreach (var pair in RomanNumeralsSequence)
        {
            while (input >= pair.Value)
            {
                pair.Key.AsSpan().CopyTo(builder[pos..]);
                pos += pair.Key.Length;
                input -= pair.Value;
            }
        }

        return builder[..pos].ToString();
    }

    static bool IsInvalidRomanNumeral(CharSpan input) =>
        !ValidRomanNumeral().IsMatch(input);
}