using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Humanizer.PhoneNumber
{
    public class PhoneNumberFormatter
    {
        private readonly Regex _phoneNumber = new Regex(@"^\+?[\d-. ]+$");
        private readonly Regex _clearRegex = new Regex(@"[^+\d]");

        private readonly IEnumerable<FormatRule> _rules = new List<FormatRule>()
        {
            new FormatRule(new Regex(@"(\+7|8)(\d{3})(\d{3})(\d{2})(\d{2})"), @"$1 $2 $3-$4-$5"),
        };

        public string Format(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return phoneNumber;

            if (!_phoneNumber.IsMatch(phoneNumber))
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
}