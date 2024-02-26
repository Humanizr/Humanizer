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
    /// Converts integer to named tuple (e.g. 'single', 'double' etc.).
    /// </summary>
    /// <param name="input">Integer</param>
    /// <returns>Named tuple</returns>
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