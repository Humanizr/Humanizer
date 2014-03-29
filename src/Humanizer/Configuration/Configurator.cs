using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.DistanceOfTimeCalculators;
using Humanizer.Localisation;

namespace Humanizer.Configuration
{
    /// <summary>
    /// Provides a configuration point for Humanizer
    /// </summary>
    public static class Configurator
    {
        static Configurator()
        {
            DistanceOfTimeInWords = new DefaultDistanceOfTime();
        }

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
        /// The distance of time in words (DOTIW) calculator
        /// </summary>
        public static IDistanceOfTimeInWords DistanceOfTimeInWords { get; set; }

        /// <summary>
        /// Default precision used to calculate DOTIW.
        /// </summary>
        public static double DefaultPrecision
        {
            get { return 0.75; }
        }
    }
}
