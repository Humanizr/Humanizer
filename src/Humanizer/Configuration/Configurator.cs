using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation;
using Humanizer.TimeSpanLocalisation;

namespace Humanizer.Configuration
{
    /// <summary>
    /// Provides a configuration point for Humanizer
    /// </summary>
    public static class Configurator
    {
        private static readonly IDictionary<string, Func<IFormatter>> FormatterFactories = 
            new Dictionary<string, Func<IFormatter>>(StringComparer.OrdinalIgnoreCase)
        {
            { "ro", () => new RomanianFormatter() },
            { "ru", () => new RussianFormatter() }
        };

        /// <summary>
        /// The formatter to be used
        /// </summary>
        public static IFormatter Formatter
        {
            get
            {
                Func<IFormatter> formatterFactory;
                if (FormatterFactories.TryGetValue(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, out formatterFactory))
                {
                    return formatterFactory();
                }
                return new DefaultFormatter();
            }
        }

        private static readonly IDictionary<string, Func<ITimeSpanFormatter>> TimeSpanFormatterFactories =
            new Dictionary<string, Func<ITimeSpanFormatter>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets a formatter that knows how to format various parts of a TimeSpan object
        /// </summary>
        public static ITimeSpanFormatter TimeSpanFormatter
        {
            get
            {
                var languageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                Func<ITimeSpanFormatter> formatterFactory;
                return TimeSpanFormatterFactories.TryGetValue(languageName, out formatterFactory)
                    ? formatterFactory()
                    : new DefaultTimeSpanFormatter();
            }
        }
    }
}
