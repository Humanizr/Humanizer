using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.DateTimeHumanizeStrategy;
using Humanizer.Localisation.Formatters;
using Humanizer.Localisation.NumberToWords;
using Humanizer.Localisation.Ordinalizers;

namespace Humanizer.Configuration
{
    /// <summary>
    /// Provides a configuration point for Humanizer
    /// </summary>
    public static class Configurator
    {

        private static FactoryManager<IFormatter> _formatterFactoryManager = new FormatterFactoryManager();

        public static FactoryManager<IFormatter> FormatterFactoryManager
        {
            get { return _formatterFactoryManager; }
        }

        private static FactoryManager<INumberToWordsConverter> _numberToWordsConverterFactoryManager = new NumberToWordsConverterFactoryManager();

        public static FactoryManager<INumberToWordsConverter> NumberToWordsConverterFactoryManager
        {
            get { return _numberToWordsConverterFactoryManager; }
        }

        private static FactoryManager<IOrdinalizer> _ordinalizerFactoryManager = new OrdinalizerFactoryManager();

        public static FactoryManager<IOrdinalizer> OrdinalizerFactoryManager
        {
            get { return _ordinalizerFactoryManager; }
        }

        private static IDateTimeHumanizeStrategy _dateTimeHumanizeStrategy = new DefaultDateTimeHumanizeStrategy();

        /// <summary>
        /// The formatter to be used
        /// </summary>
        internal static IFormatter Formatter
        {
            get
            {
                return FormatterFactoryManager.GetFactory()();
            }
        }

        /// <summary>
        /// The converter to be used
        /// </summary>
        internal static INumberToWordsConverter NumberToWordsConverter
        {
            get
            {
                return NumberToWordsConverterFactoryManager.GetFactory()();
            }
        }

        /// <summary>
        /// The ordinalizer to be used
        /// </summary>
        internal static IOrdinalizer Ordinalizer
        {
            get
            {
                return OrdinalizerFactoryManager.GetFactory()();
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
