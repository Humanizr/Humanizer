// ReSharper disable IdentifierTypo 
// ReSharper disable StringLiteralTypo
namespace Humanizer
{
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
        public static string Tupleize(this int input)
        {
            switch (input)
            {
                case 1:
                    return "single";
                case 2:
                    return "double";
                case 3:
                    return "triple";
                case 4:
                    return "quadruple";
                case 5:
                    return "quintuple";
                case 6:
                    return "sextuple";
                case 7:
                    return "septuple";
                case 8:
                    return "octuple";
                case 9:
                    return "nonuple";
                case 10:
                    return "decuple";
                case 100:
                    return "centuple";
                case 1000:
                    return "milluple";
                default:
                    return $"{input}-tuple";
            }
        }
    }
}
