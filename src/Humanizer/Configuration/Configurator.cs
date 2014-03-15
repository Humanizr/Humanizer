using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation;
using Dyna = Humanizer.Localisation.DynamicResourceKeys;

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

        /// <summary>
        /// Providers similar functionality for the DefaultFormatter except the localization, at the moment.
        /// </summary>
        public static Dyna.IFormatter DynamicFormatter
        {
            get
            {
                return new Dyna.DefaultFormatter();
            }
        }
    }
}
