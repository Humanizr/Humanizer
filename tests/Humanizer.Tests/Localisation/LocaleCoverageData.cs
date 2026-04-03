using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Tests.Localisation;

static class LocaleCoverageData
{
    static readonly Lazy<IReadOnlyList<string>> formatterLocales = new(() => GetRegisteredLocales<FormatterRegistry, IFormatter>());
    static readonly Lazy<IReadOnlyList<string>> collectionFormatterLocales = new(() => GetRegisteredLocales<CollectionFormatterRegistry, ICollectionFormatter>());
    static readonly Lazy<IReadOnlyList<string>> numberToWordsLocales = new(() => GetRegisteredLocales<NumberToWordsConverterRegistry, INumberToWordsConverter>());
    static readonly Lazy<IReadOnlyList<string>> ordinalizerLocales = new(() => GetRegisteredLocales<OrdinalizerRegistry, IOrdinalizer>());
    static readonly Lazy<IReadOnlyList<string>> dateToOrdinalWordsLocales = new(() => GetRegisteredLocales<DateToOrdinalWordsConverterRegistry, IDateToOrdinalWordConverter>());
    static readonly Lazy<IReadOnlyList<string>> wordsToNumberLocales = new(() => GetRegisteredLocales<WordsToNumberConverterRegistry, IWordsToNumberConverter>());
#if NET6_0_OR_GREATER
    static readonly Lazy<IReadOnlyList<string>> dateOnlyToOrdinalWordsLocales = new(() => GetRegisteredLocales<DateOnlyToOrdinalWordsConverterRegistry, IDateOnlyToOrdinalWordConverter>());
    static readonly Lazy<IReadOnlyList<string>> timeOnlyToClockNotationLocales = new(() => GetRegisteredLocales<TimeOnlyToClockNotationConvertersRegistry, ITimeOnlyToClockNotationConverter>());
#endif

    public static IReadOnlyList<string> FormatterLocales => formatterLocales.Value;

    public static IReadOnlyList<string> CollectionFormatterLocales => collectionFormatterLocales.Value;

    public static IReadOnlyList<string> NumberToWordsLocales => numberToWordsLocales.Value;

    public static IReadOnlyList<string> OrdinalizerLocales => ordinalizerLocales.Value;

    public static IReadOnlyList<string> DateToOrdinalWordsLocales => dateToOrdinalWordsLocales.Value;

    public static IReadOnlyList<string> WordsToNumberLocales => wordsToNumberLocales.Value;

#if NET6_0_OR_GREATER
    public static IReadOnlyList<string> DateOnlyToOrdinalWordsLocales => dateOnlyToOrdinalWordsLocales.Value;

    public static IReadOnlyList<string> TimeOnlyToClockNotationLocales => timeOnlyToClockNotationLocales.Value;
#endif

    public static TheoryData<string> FormatterLocaleTheoryData => CreateLocaleTheoryData(FormatterLocales);

    public static TheoryData<string> CollectionFormatterLocaleTheoryData => CreateLocaleTheoryData(CollectionFormatterLocales);

    public static TheoryData<string> NumberToWordsLocaleTheoryData => CreateLocaleTheoryData(NumberToWordsLocales);

    public static TheoryData<string> OrdinalizerLocaleTheoryData => CreateLocaleTheoryData(OrdinalizerLocales);

    public static TheoryData<string> DateToOrdinalWordsLocaleTheoryData => CreateLocaleTheoryData(DateToOrdinalWordsLocales);

    public static TheoryData<string> WordsToNumberLocaleTheoryData => CreateLocaleTheoryData(WordsToNumberLocales);

#if NET6_0_OR_GREATER
    public static TheoryData<string> DateOnlyToOrdinalWordsLocaleTheoryData => CreateLocaleTheoryData(DateOnlyToOrdinalWordsLocales);

    public static TheoryData<string> TimeOnlyToClockNotationLocaleTheoryData => CreateLocaleTheoryData(TimeOnlyToClockNotationLocales);
#endif

    public static TheoryData<string, string, string> CollectionFormatterExpectationTheoryData =>
        new()
        {
            { "ca", "1 i 2", "1, 2 i 3" },
            { "de", "1 und 2", "1, 2 und 3" },
            { "en", "1 and 2", "1, 2, and 3" },
            { "es", "1 y 2", "1, 2 y 3" },
            { "is", "1 og 2", "1, 2 og 3" },
            { "it", "1 e 2", "1, 2 e 3" },
            { "lb", "1 an 2", "1, 2 an 3" },
            { "nb", "1 og 2", "1, 2 og 3" },
            { "nl", "1 en 2", "1, 2 en 3" },
            { "nn", "1 og 2", "1, 2 og 3" },
            { "pt", "1 e 2", "1, 2 e 3" },
            { "ro", "1 și 2", "1, 2 și 3" },
            { "sv", "1 och 2", "1, 2 och 3" }
        };

    public static TheoryData<string, string, string> FormatterFallbackExpectationTheoryData =>
        new()
        {
            { "de-CH", "gestern", "2 Tage" },
            { "de-LI", "gestern", "2 Tage" },
            { "en-IN", "yesterday", "2 days" },
            { "fr-CH", "hier", "2 jours" },
            { "ta", "yesterday", "2 days" },
            { "zu-ZA", "yesterday", "2 days" }
        };

    public static TheoryData<string, int, string> NumberToWordsOrdinalExpectationTheoryData =>
        new()
        {
            { "pt-BR", 1, "primeira" }
        };

    public static TheoryData<string, string> DateToOrdinalWordsExpectationTheoryData =>
        new()
        {
            { "ca", "1 de gener de 2015" },
            { "en-US", "January 1st, 2015" },
            { "es", "1 de enero de 2015" },
            { "fr", "1er janvier 2015" },
            { "lt", "2015 m. sausio 1 d." }
        };

#if NET6_0_OR_GREATER
    public static TheoryData<string, string> DateOnlyToOrdinalWordsExpectationTheoryData =>
        new()
        {
            { "ca", "1 de gener de 2015" },
            { "en-US", "January 1st, 2015" },
            { "es", "1 de enero de 2015" },
            { "fr", "1er janvier 2015" },
            { "lt", "2015 m. sausio 1 d." }
        };

    public static TheoryData<string, int, int, string> TimeOnlyToClockNotationExpectationTheoryData =>
        new()
        {
            { "ca", 15, 40, "les quatre menys vint de la tarda" },
            { "de", 13, 23, "fünf vor halb zwei" },
            { "es", 10, 25, "las diez y veinticinco de la mañana" },
            { "fr", 13, 23, "treize heures vingt-cinq" },
            { "lb", 13, 23, "fënnef vir hallwer zwou" },
            { "pt", 13, 23, "uma e vinte e cinco" },
            { "pt-BR", 13, 23, "uma e vinte e cinco" }
        };
#endif

    public static TheoryData<string, string> UnsupportedWordsToNumberCultureTheoryData => CreateUnsupportedWordsToNumberCultureTheoryData();

    public static CultureSwap UseCulture(string cultureName) => new(new(cultureName));

    static TheoryData<string> CreateLocaleTheoryData(IEnumerable<string> locales)
    {
        var data = new TheoryData<string>();
        foreach (var locale in locales)
        {
            data.Add(locale);
        }

        return data;
    }

    static TheoryData<string, string> CreateUnsupportedWordsToNumberCultureTheoryData()
    {
        var data = new TheoryData<string, string>();

        foreach (var locale in NumberToWordsLocales.Where(static locale => CultureInfo.GetCultureInfo(locale).TwoLetterISOLanguageName != "en"))
        {
            data.Add(locale, "one");
        }

        data.Add("zu-ZA", "one");
        return data;
    }

    static string[] GetRegisteredLocales<TRegistry, TLocaliser>()
        where TRegistry : LocaliserRegistry<TLocaliser>, new()
        where TLocaliser : class
        => new TRegistry().GetRegisteredLocaleCodes();
}

sealed class CultureSwap : IDisposable
{
    readonly CultureInfo originalCulture = CultureInfo.CurrentCulture;
    readonly CultureInfo originalUICulture = CultureInfo.CurrentUICulture;

    public CultureSwap(CultureInfo culture)
    {
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    public void Dispose()
    {
        CultureInfo.CurrentCulture = originalCulture;
        CultureInfo.CurrentUICulture = originalUICulture;
    }
}
