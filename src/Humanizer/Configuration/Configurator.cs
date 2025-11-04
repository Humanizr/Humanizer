namespace Humanizer;

/// <summary>
/// Provides a configuration point for Humanizer
/// </summary>
public static class Configurator
{
    /// <summary>
    /// A registry of formatters used to format collections based on the current locale
    /// </summary>
    public static LocaliserRegistry<ICollectionFormatter> CollectionFormatters { get; } = new CollectionFormatterRegistry();

    /// <summary>
    /// A registry of formatters used to format strings based on the current locale
    /// </summary>
    public static LocaliserRegistry<IFormatter> Formatters { get; } = new FormatterRegistry();

    /// <summary>
    /// A registry of number to words converters used to localise ToWords and ToOrdinalWords methods
    /// </summary>
    public static LocaliserRegistry<INumberToWordsConverter> NumberToWordsConverters { get; } = new NumberToWordsConverterRegistry();

    /// <summary>
    /// Registry of converters that transform words into numbers for english language
    /// </summary>
    private static LocaliserRegistry<IWordsToNumberConverter> WordsToNumberConverters { get; } = new WordsToNumberConverterRegistry();

    /// <summary>
    /// A registry of ordinalizers used to localise Ordinalize method
    /// </summary>
    public static LocaliserRegistry<IOrdinalizer> Ordinalizers { get; } = new OrdinalizerRegistry();

    /// <summary>
    /// A registry of ordinalizers used to localise Ordinalize method
    /// </summary>
    public static LocaliserRegistry<IDateToOrdinalWordConverter> DateToOrdinalWordsConverters { get; } = new DateToOrdinalWordsConverterRegistry();

#if NET6_0_OR_GREATER
    /// <summary>
    /// A registry of ordinalizers used to localise Ordinalize method
    /// </summary>
    public static LocaliserRegistry<IDateOnlyToOrdinalWordConverter> DateOnlyToOrdinalWordsConverters { get; } = new DateOnlyToOrdinalWordsConverterRegistry();

    /// <summary>
    /// A registry of time to clock notation converters used to localise ToClockNotation methods
    /// </summary>
    public static LocaliserRegistry<ITimeOnlyToClockNotationConverter> TimeOnlyToClockNotationConverters { get; } = new TimeOnlyToClockNotationConvertersRegistry();
#endif

    internal static ICollectionFormatter CollectionFormatter => CollectionFormatters.ResolveForUiCulture();

    /// <summary>
    /// The formatter to be used
    /// </summary>
    /// <param name="culture">The culture to retrieve formatter for. Null means that current thread's UI culture should be used.</param>
    internal static IFormatter GetFormatter(CultureInfo? culture) =>
        Formatters.ResolveForCulture(culture);

    /// <summary>
    /// The converter to be used
    /// </summary>
    /// <param name="culture">The culture to retrieve number to words converter for. Null means that current thread's UI culture should be used.</param>
    internal static INumberToWordsConverter GetNumberToWordsConverter(CultureInfo? culture) =>
        NumberToWordsConverters.ResolveForCulture(culture);

    internal static IWordsToNumberConverter GetWordsToNumberConverter(CultureInfo culture) =>
        WordsToNumberConverters.ResolveForCulture(culture);



    /// <summary>
    /// The ordinalizer to be used
    /// </summary>
    internal static IOrdinalizer Ordinalizer => Ordinalizers.ResolveForUiCulture();

    /// <summary>
    /// The ordinalizer to be used
    /// </summary>
    internal static IDateToOrdinalWordConverter DateToOrdinalWordsConverter => DateToOrdinalWordsConverters.ResolveForUiCulture();

#if NET6_0_OR_GREATER
    /// <summary>
    /// The ordinalizer to be used
    /// </summary>
    internal static IDateOnlyToOrdinalWordConverter DateOnlyToOrdinalWordsConverter => DateOnlyToOrdinalWordsConverters.ResolveForUiCulture();

    internal static ITimeOnlyToClockNotationConverter TimeOnlyToClockNotationConverter => TimeOnlyToClockNotationConverters.ResolveForUiCulture();
#endif

    /// <summary>
    /// The strategy to be used for DateTime.Humanize
    /// </summary>
    /// <remarks>
    /// This property should be set only once during application startup before any humanization operations occur.
    /// For thread-safety, use volatile reads or appropriate synchronization when accessing this property in multi-threaded scenarios.
    /// In production applications, avoid changing this value after the application has started serving requests.
    /// </remarks>
    public static IDateTimeHumanizeStrategy DateTimeHumanizeStrategy { get; set; } = new DefaultDateTimeHumanizeStrategy();

    /// <summary>
    /// The strategy to be used for DateTimeOffset.Humanize
    /// </summary>
    /// <remarks>
    /// This property should be set only once during application startup before any humanization operations occur.
    /// For thread-safety, use volatile reads or appropriate synchronization when accessing this property in multi-threaded scenarios.
    /// In production applications, avoid changing this value after the application has started serving requests.
    /// </remarks>
    public static IDateTimeOffsetHumanizeStrategy DateTimeOffsetHumanizeStrategy { get; set; } = new DefaultDateTimeOffsetHumanizeStrategy();

#if NET6_0_OR_GREATER
    /// <summary>
    /// The strategy to be used for DateOnly.Humanize
    /// </summary>
    /// <remarks>
    /// This property should be set only once during application startup before any humanization operations occur.
    /// For thread-safety, use volatile reads or appropriate synchronization when accessing this property in multi-threaded scenarios.
    /// In production applications, avoid changing this value after the application has started serving requests.
    /// </remarks>
    public static IDateOnlyHumanizeStrategy DateOnlyHumanizeStrategy { get; set; } = new DefaultDateOnlyHumanizeStrategy();

    /// <summary>
    /// The strategy to be used for TimeOnly.Humanize
    /// </summary>
    /// <remarks>
    /// This property should be set only once during application startup before any humanization operations occur.
    /// For thread-safety, use volatile reads or appropriate synchronization when accessing this property in multi-threaded scenarios.
    /// In production applications, avoid changing this value after the application has started serving requests.
    /// </remarks>
    public static ITimeOnlyHumanizeStrategy TimeOnlyHumanizeStrategy { get; set; } = new DefaultTimeOnlyHumanizeStrategy();
#endif

    static readonly Func<PropertyInfo, bool> DefaultEnumDescriptionPropertyLocator = p => p.Name == "Description";
    static readonly object EnumDescriptionPropertyLocatorLock = new();
    static volatile Func<PropertyInfo, bool> enumDescriptionPropertyLocator = DefaultEnumDescriptionPropertyLocator;
    static volatile bool enumDescriptionPropertyLocatorHasBeenUsed;

    internal static Func<PropertyInfo, bool> EnumDescriptionPropertyLocator
    {
        get
        {
            enumDescriptionPropertyLocatorHasBeenUsed = true;
            return enumDescriptionPropertyLocator;
        }
        private set => enumDescriptionPropertyLocator = value;
    }

    /// <summary>
    /// Use a predicate function for description property of attribute to use for Enum.Humanize
    /// </summary>
    public static void UseEnumDescriptionPropertyLocator(Func<PropertyInfo, bool> func)
    {
        lock (EnumDescriptionPropertyLocatorLock)
        {
            if (enumDescriptionPropertyLocatorHasBeenUsed)
            {
                throw new("UseEnumDescriptionPropertyLocator must be called before any Enum.Humanize has already been. Move the call to UseEnumDescriptionPropertyLocator to the app startup or a ModuleInitializer.");
            }

            EnumDescriptionPropertyLocator = func;
        }
    }

    internal static void ResetUseEnumDescriptionPropertyLocator()
    {
        lock (EnumDescriptionPropertyLocatorLock)
        {
            enumDescriptionPropertyLocatorHasBeenUsed = false;
            EnumDescriptionPropertyLocator = DefaultEnumDescriptionPropertyLocator;
        }
    }
}