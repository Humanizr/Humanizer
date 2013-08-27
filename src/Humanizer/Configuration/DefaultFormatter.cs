namespace Humanizer.Configuration
{
    class DefaultFormatter : ILocaleAwareFormatter
    {
        /// <summary>
        /// String.Format is the default formatter which seems to work for majority of locales
        /// </summary>
        /// <param name="format"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public string FormatNumberInString(string format, object number)
        {
            return string.Format(format, number);
        }
    }
}