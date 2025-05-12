// Done by Jesse Slicer https://github.com/jslicer

using System.Diagnostics;

namespace Humanizer;

/// <summary>
/// Contains extension methods for changing a number to Roman representation (ToRoman) and from Roman representation back to the number (FromRoman)
/// </summary>
public static class RomanNumeralExtensions
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

    static readonly Regex ValidRomanNumeral =
        new(
            "^(?i:(?=[MDCLXVI])((M{0,3})((C[DM])|(D?C{0,3}))?((X[LC])|(L?XX{0,2})|L)?((I[VX])|(V?(II{0,2}))|V)?))$",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    /// <summary>
    /// Converts Roman numbers into integer
    /// </summary>
    /// <param name="input">Roman number</param>
    /// <returns>Human-readable number</returns>
    public static int FromRoman(this string input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        return FromRoman(input.AsSpan());
    }

    /// <summary>
    /// Converts Roman numbers into integer
    /// </summary>
    /// <param name="input">Roman number</param>
    /// <returns>Human-readable number</returns>
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
    /// Converts the input to Roman number
    /// </summary>
    /// <param name="input">Integer input</param>
    /// <returns>Roman number</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the input is smaller than 1 or larger than 3999</exception>
    public static string ToRoman(this int input)
    {
        const int minValue = 1;
        const int maxValue = 3999;
        const int maxRomanNumeralLength = 15;

        if (input is < minValue or > maxValue)
        {
            throw new ArgumentOutOfRangeException();
        }

        Span<char> builder = stackalloc char[maxRomanNumeralLength];
        var pos = 0;

        foreach (var pair in RomanNumeralsSequence)
        {
            while (input >= pair.Value)
            {
                pair.Key.AsSpan().CopyTo(builder.Slice(pos));
                pos += pair.Key.Length;
                input -= pair.Value;
            }
        }

        return builder.Slice(0, pos).ToString();
    }

    static bool IsInvalidRomanNumeral(CharSpan input) =>
        !ValidRomanNumeral.IsMatch(input);
}