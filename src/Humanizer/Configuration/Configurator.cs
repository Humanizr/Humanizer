using System;
using System.Globalization;

namespace Humanizer.Configuration
{
    /// <summary>
    /// Provides a configuration point for Humanizer
    /// </summary>
    public static class Configurator
    {
        /// <summary>
        /// The formatter to be used 
        /// </summary>
        public static IFormatter Formatter 
        {
            get
            {
                //TODO: find proper way to get partial cultures. We want to match all xx-RU cultures.
                if (string.Equals(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, "ru", StringComparison.OrdinalIgnoreCase))
                {
                    return new RussianFormatter();
                }

                // ToDo: when other formatters are created we should change this implementation to resolve the Formatter based on the current culture
                return new DefaultFormatter();
            }
        }
    }
}
