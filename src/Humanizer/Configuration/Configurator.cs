using System;
using System.Globalization;
using System.Reflection;
using Humanizer.DateTimeHumanizeStrategy;
using Humanizer.Localisation.CollectionFormatters;
using Humanizer.Localisation.DateToOrdinalWords;
using Humanizer.Localisation.Formatters;
using Humanizer.Localisation.NumberToWords;
using Humanizer.Localisation.Ordinalizers;
#if NET6_0_OR_GREATER
using Humanizer.Localisation.TimeToClockNotation;
#endif

namespace Humanizer.Configuration
{
    /// <summary>
    /// Provides a configuration point for Humanizer
    /// </summary>
    public static class Configurator
    {
        private static readonly LocaliserRegistry<ICollectionFormatter> _collectionFormatters = new CollectionFormatterRegistry();

        /// <summary>
        /// A registry of formatters used to format collections based on the current locale
        /// </summary>
        public static LocaliserRegistry<ICollectionFormatter> CollectionFormatters
        {
            get { return _collectionFormatters; }
        }

        private static readonly LocaliserRegistry<IFormatter> _formatters = new FormatterRegistry();
        /// <summary>
        /// A registry of formatters used to format strings based on the current locale
        /// </summary>
        public static LocaliserRegistry<IFormatter> Formatters
        {
            get { return _formatters; }
        }

        private static readonly LocaliserRegistry<INumberToWordsConverter> _numberToWordsConverters = new NumberToWordsConverterRegistry();
        /// <summary>
        /// A registry of number to words converters used to localise ToWords and ToOrdinalWords methods
        /// </summary>
        public static LocaliserRegistry<INumberToWordsConverter> NumberToWordsConverters
        {
            get { return _numberToWordsConverters; }
        }

        private static readonly LocaliserRegistry<IOrdinalizer> _ordinalizers = new OrdinalizerRegistry();
        /// <summary>
        /// A registry of ordinalizers used to localise Ordinalize method
        /// </summary>
        public static LocaliserRegistry<IOrdinalizer> Ordinalizers
        {
            get { return _ordinalizers; }
        }

        private static readonly LocaliserRegistry<IDateToOrdinalWordConverter> _dateToOrdinalWordConverters = new DateToOrdinalWordsConverterRegistry();
        /// <summary>
        /// A registry of ordinalizers used to localise Ordinalize method
        /// </summary>
        public static LocaliserRegistry<IDateToOrdinalWordConverter> DateToOrdinalWordsConverters
        {
            get { return _dateToOrdinalWordConverters; }
        }

#if NET6_0_OR_GREATER
        private static readonly LocaliserRegistry<IDateOnlyToOrdinalWordConverter> _dateOnlyToOrdinalWordConverters = new DateOnlyToOrdinalWordsConverterRegistry();
        /// <summary>
        /// A registry of ordinalizers used to localise Ordinalize method
        /// </summary>
        public static LocaliserRegistry<IDateOnlyToOrdinalWordConverter> DateOnlyToOrdinalWordsConverters
        {
            get { return _dateOnlyToOrdinalWordConverters; }
        }

        private static readonly LocaliserRegistry<ITimeOnlyToClockNotationConverter> _timeOnlyToClockNotationConverters = new TimeOnlyToClockNotationConvertersRegistry();
        /// <summary>
        /// A registry of time to clock notation converters used to localise ToClockNotation methods
        /// </summary>
        public static LocaliserRegistry<ITimeOnlyToClockNotationConverter> TimeOnlyToClockNotationConverters
        {
            get { return _timeOnlyToClockNotationConverters; }
        }
#endif

        internal static ICollectionFormatter CollectionFormatter
        {
            get
            {
                return CollectionFormatters.ResolveForUiCulture();
            }
        }

        /// <summary>
        /// The formatter to be used
        /// </summary>
        /// <param name="culture">The culture to retrieve formatter for. Null means that current thread's UI culture should be used.</param>
        internal static IFormatter GetFormatter(CultureInfo culture)
        {
            return Formatters.ResolveForCulture(culture);
        }

        /// <summary>
        /// The converter to be used
        /// </summary>
        /// <param name="culture">The culture to retrieve number to words converter for. Null means that current thread's UI culture should be used.</param>
        internal static INumberToWordsConverter GetNumberToWordsConverter(CultureInfo culture)
        {
            return NumberToWordsConverters.ResolveForCulture(culture);
        }

        /// <summary>
        /// The ordinalizer to be used
        /// </summary>
        internal static IOrdinalizer Ordinalizer
        {
            get
            {
                return Ordinalizers.ResolveForUiCulture();
            }
        }

        /// <summary>
        /// The ordinalizer to be used
        /// </summary>
        internal static IDateToOrdinalWordConverter DateToOrdinalWordsConverter
        {
            get
            {
                return DateToOrdinalWordsConverters.ResolveForUiCulture();
            }
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// The ordinalizer to be used
        /// </summary>
        internal static IDateOnlyToOrdinalWordConverter DateOnlyToOrdinalWordsConverter
        {
            get
            {
                return DateOnlyToOrdinalWordsConverters.ResolveForUiCulture();
            }
        }

        internal static ITimeOnlyToClockNotationConverter TimeOnlyToClockNotationConverter
        {
            get
            {
                return TimeOnlyToClockNotationConverters.ResolveForUiCulture();
            }
        }
#endif

        private static IDateTimeHumanizeStrategy _dateTimeHumanizeStrategy = new DefaultDateTimeHumanizeStrategy();
        /// <summary>
        /// The strategy to be used for DateTime.Humanize
        /// </summary>
        public static IDateTimeHumanizeStrategy DateTimeHumanizeStrategy
        {
            get { return _dateTimeHumanizeStrategy; }
            set { _dateTimeHumanizeStrategy = value; }
        }

        private static IDateTimeOffsetHumanizeStrategy _dateTimeOffsetHumanizeStrategy = new DefaultDateTimeOffsetHumanizeStrategy();
        /// <summary>
        /// The strategy to be used for DateTimeOffset.Humanize
        /// </summary>
        public static IDateTimeOffsetHumanizeStrategy DateTimeOffsetHumanizeStrategy
        {
            get { return _dateTimeOffsetHumanizeStrategy; }
            set { _dateTimeOffsetHumanizeStrategy = value; }
        }

#if NET6_0_OR_GREATER
        private static IDateOnlyHumanizeStrategy _dateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();
        /// <summary>
        /// The strategy to be used for DateOnly.Humanize
        /// </summary>
        public static IDateOnlyHumanizeStrategy DateOnlyHumanizeStrategy
        {
            get { return _dateOnlyHumanizeStrategy; }
            set { _dateOnlyHumanizeStrategy = value; }
        }

        private static ITimeOnlyHumanizeStrategy _timeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();
        /// <summary>
        /// The strategy to be used for TimeOnly.Humanize
        /// </summary>
        public static ITimeOnlyHumanizeStrategy TimeOnlyHumanizeStrategy
        {
            get { return _timeOnlyHumanizeStrategy; }
            set { _timeOnlyHumanizeStrategy = value; }
        }
#endif

        private static readonly Func<PropertyInfo, bool> DefaultEnumDescriptionPropertyLocator = p => p.Name == "Description";
        private static Func<PropertyInfo, bool> _enumDescriptionPropertyLocator = DefaultEnumDescriptionPropertyLocator;
        /// <summary>
        /// A predicate function for description property of attribute to use for Enum.Humanize
        /// </summary>
        public static Func<PropertyInfo, bool> EnumDescriptionPropertyLocator
        {
            get { return _enumDescriptionPropertyLocator; }
            set { _enumDescriptionPropertyLocator = value ?? DefaultEnumDescriptionPropertyLocator; }
        }
    }
}
