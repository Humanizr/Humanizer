using System;

namespace Humanizer.PhoneNumber
{
    public static class StringPhoneNumberFormatExtensions
    {
        private static readonly Lazy<PhoneNumberFormatter> Formatter = new Lazy<PhoneNumberFormatter>(() => new PhoneNumberFormatter());

        public static string FormatPhone(this string phoneNumber)
        {
            return Formatter.Value.Format(phoneNumber);
        }
    }
}