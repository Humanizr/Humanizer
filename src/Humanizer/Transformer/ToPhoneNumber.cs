using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Humanizer
{
    class ToPhoneNumber : IStringTransformer
    {
        private static readonly Lazy<PhoneNumberFormatter> Formatter = new Lazy<PhoneNumberFormatter>(() => new PhoneNumberFormatter());

        public string Transform(string input)
        {
            return Formatter.Value.Format(input);
        }
    }

    internal class PhoneNumberFormatter
    {
        private readonly Regex _phoneNumberRegex = new Regex(@"^\+?[\d-. ]+$");
        private readonly Regex _clearRegex = new Regex(@"[^+\d]");
        private readonly IEnumerable<FormatRule> _rules = new List<FormatRule>
        {
            new FormatRule(new Regex(@"(\+7|8)(\d{3})(\d{3})(\d{2})(\d{2})"), @"$1 $2 $3-$4-$5"),  // russian mobile phone numbers
        };

        public string Format(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return phoneNumber;

            if (!_phoneNumberRegex.IsMatch(phoneNumber))
                return phoneNumber;

            var clearedNumber = _clearRegex.Replace(phoneNumber, "");
            foreach (var rule in _rules)
            {
                if (rule.IsMatch(clearedNumber))
                    return rule.Format(clearedNumber);
            }

            return phoneNumber;
        }
    }

    internal class FormatRule
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