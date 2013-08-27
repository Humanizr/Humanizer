namespace Humanizer.Configuration
{
    /// <summary>
    /// Allows different locales to override how string.Format should work when numbers are involved
    /// This was added to provide better localization support for Romanian where for example, "5 days" is "5 zile", while "24 days" is "24 de zile".
    /// </summary>
    public interface ILocaleAwareFormatter
    {
        /// <summary>
        /// Is used instead of String.Format anywhere a number is to be injected in a string to allow for customization of string based on the number
        /// </summary>
        /// <param name="format"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        string FormatNumberInString(string format, object number);
    }
}
