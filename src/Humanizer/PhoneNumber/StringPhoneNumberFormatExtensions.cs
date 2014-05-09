using System;

namespace Humanizer.PhoneNumber
{
    /// <summary>
    /// Extension methods for String type.
    /// </summary>
    public static class StringPhoneNumberFormatExtensions
    {
        private static readonly Lazy<PhoneNumberFormatter> Formatter = new Lazy<PhoneNumberFormatter>(() => new PhoneNumberFormatter());

        /// <summary>
        /// Extension method to format phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <returns>Formatted phone number</returns>
        public static string FormatPhone(this string phoneNumber)
        {
            return Formatter.Value.Format(phoneNumber);
        }
    }
}