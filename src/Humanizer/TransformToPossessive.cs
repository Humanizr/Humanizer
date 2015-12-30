using System.Linq;
using System.Text;

namespace Humanizer
{
    public static class TransformToPossessive
    {
        private static readonly string[] _possessivePronouns = { "ITS", "MY", "MINE", "YOUR", "HER", "HERS", "HIS", "OUR", "OURS", "THEIR", "THEIRS", "YOURS" };
        private static readonly string[] _nounsEndingInSilentS = { "ILLINOIS", "ARKANSAS", "CORPS" };
        public static string ToPossessive(this string input)
        {
            if (IsPossessivePronoun(input))
                return input;
            var possessiveBuilder = new StringBuilder();
            possessiveBuilder.Append(input);
            var result = (EndsInS(input) ? 
                (EndsInSilentS(input) ? 
                    possessiveBuilder.Append("'s") : possessiveBuilder.Append("'")) : 
                possessiveBuilder.Append("'s"));
            return result.ToString();
        }

        private static bool EndsInS(string input)
        {
            var len = input.Length;
            var lastChar = input[len - 1];
            return (char.ToUpper(lastChar) == 'S');
        }

        private static bool EndsInSilentS(string input)
        {
            var uppercaseInput = input.ToUpper().Trim();
            return (_nounsEndingInSilentS.Contains(uppercaseInput));
        }

        private static bool IsPossessivePronoun(string input)
        {
            var uppercaseInput = input.ToUpper().Trim();
            return (_possessivePronouns.Contains(uppercaseInput));
        }
    }
}
