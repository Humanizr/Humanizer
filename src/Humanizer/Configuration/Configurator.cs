using Humanizer.Localisation;
using System;
using System.Collections.Generic;
using System.Globalization;

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
            { "sk", () => new CzechSlovakFormatter() },
            { "cs", () => new CzechSlovakFormatter() },
            { "pl", () => new PolishFormatter() }
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
    }
}
