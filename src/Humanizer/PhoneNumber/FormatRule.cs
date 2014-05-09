using System.Text.RegularExpressions;

namespace Humanizer.PhoneNumber
{
    public class FormatRule
    {
        private readonly Regex _pattern;
        private readonly string _outFormat;

        public FormatRule(Regex pattern, string outFormat)
        {
            _pattern = pattern;
            _outFormat = outFormat;
        }

        public bool IsMatch(string input)
        {
            return _pattern.IsMatch(input);
        }

        public string Format(string input)
        {
            return _pattern.Replace(input, _outFormat);
        }
    }
}