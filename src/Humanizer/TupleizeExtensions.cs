// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
namespace Humanizer;

/// <summary>
/// Convert int to named tuple strings (1 -> 'single', 2-> 'double' etc.).
/// Only values 1-10, 100, and 1000 have specific names. All others will return 'n-tuple'.
/// </summary>
public static class TupleizeExtensions
{
    /// <summary>
    /// Converts an integer to its corresponding tuple name (e.g., 'single', 'double', 'triple').
    /// </summary>
    /// <param name="input">The integer value to convert to a tuple name.</param>
    /// <returns>
    /// A string representing the tuple name:
    /// - 1 returns "single"
    /// - 2 returns "double"
    /// - 3 returns "triple"
    /// - 4 returns "quadruple"
    /// - 5 returns "quintuple"
    /// - 6 returns "sextuple"
    /// - 7 returns "septuple"
    /// - 8 returns "octuple"
    /// - 9 returns "nonuple"
    /// - 10 returns "decuple"
    /// - 100 returns "centuple"
    /// - 1000 returns "milluple"
    /// - Any other value returns "{value}-tuple" (e.g., "42-tuple")
    /// </returns>
    /// <remarks>
    /// Only values 1-10, 100, and 1000 have specific named tuples. All other values return 
    /// a generic n-tuple format. Negative values and zero will return in the format "{value}-tuple".
    /// </remarks>
    /// <example>
    /// <code>
    /// 1.Tupleize() => "single"
    /// 2.Tupleize() => "double"
    /// 3.Tupleize() => "triple"
    /// 10.Tupleize() => "decuple"
    /// 100.Tupleize() => "centuple"
    /// 42.Tupleize() => "42-tuple"
    /// (-5).Tupleize() => "-5-tuple"
    /// </code>
    /// </example>
    public static string Tupleize(this int input) =>
        input switch
        {
            1 => "single",
            2 => "double",
            3 => "triple",
            4 => "quadruple",
            5 => "quintuple",
            6 => "sextuple",
            7 => "septuple",
            8 => "octuple",
            9 => "nonuple",
            10 => "decuple",
            100 => "centuple",
            1000 => "milluple",
            _ => $"{input}-tuple"
        };
}