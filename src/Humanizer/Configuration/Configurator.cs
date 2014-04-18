using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.DateTimeHumanizeStrategy;
using Humanizer.Localisation.Formatters;

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
            { "ru", () => new RussianFormatter() },
            { "ar", () => new ArabicFormatter() },
            { "he", () => new HebrewFormatter() },
            { "sk", () => new CzechSlovakPolishFormatter() },
            { "cs", () => new CzechSlovakPolishFormatter() },
            { "pl", () => new CzechSlovakPolishFormatter() }
        };

        private static IDateTimeHumanizeStrategy _dateTimeHumanizeStrategy = new DefaultDateTimeHumanizeStrategy();

        /// <summary>
        /// The formatter to be used
        /// </summary>
        public static IFormatter Formatter
        {
            get
            {
                Func<IFormatter> formatterFactory;
                if (FormatterFactories.TryGetValue(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, out formatterFactory))
                    return formatterFactory();
                
                return new DefaultFormatter();
            }
        }

        /// <summary>
        /// The strategy to be used for DateTime.Humanize
        /// </summary>
        public static IDateTimeHumanizeStrategy DateTimeHumanizeStrategy
        {
            get { return _dateTimeHumanizeStrategy; }
            set { _dateTimeHumanizeStrategy = value; }
        }
    }
}
