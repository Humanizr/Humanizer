using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Humanizer.Tests.Localisation;

static class LocaleCoverageData
{
    static readonly Lazy<string> repositoryRoot = new(FindRepositoryRoot);
    static readonly Lazy<IReadOnlyList<string>> neutralResourceKeys = new(() => ReadResourceKeys(NeutralResourceFilePath));
    static readonly Lazy<IReadOnlyList<string>> localizedResourceFilePaths = new(() =>
        Directory.GetFiles(ResourceDirectoryPath, "Resources.*.resx", SearchOption.TopDirectoryOnly)
            .Where(path => !path.EndsWith("Resources.resx", StringComparison.OrdinalIgnoreCase))
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray());

    static readonly Lazy<IReadOnlyDictionary<string, IReadOnlyList<string>>> localizedResourceKeysByLocale = new(() =>
        LocalizedResourceFilePaths.ToDictionary(
            GetLocaleFromResourceFilePath,
            ReadResourceKeys,
            StringComparer.OrdinalIgnoreCase));
    static readonly Lazy<IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>>> localizedResourceValuesByLocale = new(() =>
        LocalizedResourceFilePaths.ToDictionary(
            GetLocaleFromResourceFilePath,
            ReadResourceEntries,
            StringComparer.OrdinalIgnoreCase));

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

    public static string NeutralResourceFilePath => Path.Combine(ResourceDirectoryPath, "Resources.resx");

    public static string ResourceDirectoryPath => Path.Combine(RepositoryRoot, "src", "Humanizer", "Properties");

    public static string RepositoryRoot => repositoryRoot.Value;

    public static IReadOnlyList<string> NeutralResourceKeys => neutralResourceKeys.Value;

    public static IReadOnlyList<string> LocalizedResourceFilePaths => localizedResourceFilePaths.Value;

    public static IReadOnlyDictionary<string, IReadOnlyList<string>> LocalizedResourceKeysByLocale => localizedResourceKeysByLocale.Value;

    public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> LocalizedResourceValuesByLocale => localizedResourceValuesByLocale.Value;

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

    public static IReadOnlyList<string> HeadingKeys =>
        HeadingExtensions.Headings
            .Concat(HeadingExtensions.HeadingsShort)
            .ToArray();

    public static IReadOnlyList<string> LocalesWithCompleteHeadingResources =>
        LocalizedResourceKeysByLocale
            .Where(static pair => HeadingKeys.All(key => pair.Value.Contains(key, StringComparer.Ordinal)))
            .Select(static pair => pair.Key)
            .OrderBy(static locale => locale, StringComparer.Ordinal)
            .ToArray();

    public static TheoryData<string> FormatterLocaleTheoryData => CreateLocaleTheoryData(FormatterLocales);

    public static TheoryData<string> LocalizedResourceLocaleTheoryData => CreateLocaleTheoryData(LocalizedResourceKeysByLocale.Keys);

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
            { "dk", "1 og 2", "1, 2 og 3" },
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

    public static TheoryData<string, string> FormatterFallbackTheoryData =>
        new()
        {
            { "de-CH", "gestern" },
            { "de-LI", "gestern" },
            { "en-IN", "yesterday" },
            { "fr-CH", "hier" },
            { "ta", "yesterday" },
            { "zu-ZA", "yesterday" }
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

    public static TheoryData<string, long, string> NumberToWordsCardinalExpectationTheoryData =>
        new()
        {
            { "de-CH", 30, "dreissig" },
            { "de-LI", 30, "dreissig" },
            { "en-IN", 100000, "one lakh" },
            { "fr-CH", 80, "octante" },
            { "ta", 100, "நூறு" }
        };

    public static TheoryData<string, int, string> NumberToWordsOrdinalExpectationTheoryData =>
        new()
        {
            { "pt-BR", 1, "primeira" }
        };

    public static TheoryData<string, int, string> OrdinalizerExpectationTheoryData =>
        new()
        {
            { "de", 1, "1." },
            { "en", 1, "1st" },
            { "fr", 1, "1er" },
            { "pt", 1, "1º" },
            { "ro", 1, "primul" },
            { "tr", 1, "1." }
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

    public static TheoryData<string, string> UnsupportedWordsToNumberCultureTheoryData =>
        new()
        {
            { "es-ES", "veinte" },
            { "fr-FR", "vingt" },
            { "zu-ZA", "one" }
        };

    public static string GetLocaleFromResourceFilePath(string resourceFilePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(resourceFilePath);
        const string prefix = "Resources.";
        return fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? fileName[prefix.Length..]
            : string.Empty;
    }

    public static IReadOnlyList<string> ReadResourceKeys(string resourceFilePath) =>
        ReadResourceEntries(resourceFilePath)
            .Keys
            .OrderBy(static name => name, StringComparer.Ordinal)
            .ToArray();

    public static IReadOnlyDictionary<string, string> ReadResourceEntries(string resourceFilePath) =>
        XDocument.Load(resourceFilePath)
            .Root!
            .Elements("data")
            .Select(static element => new
            {
                Name = (string?)element.Attribute("name"),
                Value = (string?)element.Element("value") ?? string.Empty
            })
            .Where(static element => !string.IsNullOrWhiteSpace(element.Name))
            .ToDictionary(static element => element.Name!, static element => element.Value, StringComparer.Ordinal);

    public static CultureSwap UseCulture(string cultureName) => new(new(cultureName));

    static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "src", "Humanizer", "Humanizer.csproj")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new Xunit.Sdk.XunitException("Could not locate the repository root.");
    }

    static TheoryData<string> CreateLocaleTheoryData(IEnumerable<string> locales)
    {
        var data = new TheoryData<string>();
        foreach (var locale in locales)
        {
            data.Add(locale);
        }

        return data;
    }

    static string[] GetRegisteredLocales<TRegistry, TLocaliser>()
        where TRegistry : LocaliserRegistry<TLocaliser>, new()
        where TLocaliser : class
    {
        var registry = new TRegistry();
        var field = typeof(LocaliserRegistry<TLocaliser>).GetField("localisersBuilder", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new Xunit.Sdk.XunitException($"Could not find localiser registry field for {typeof(TRegistry).Name}.");
        var registrations = (Dictionary<string, Func<CultureInfo, TLocaliser>>)field.GetValue(registry)!;

        return registrations.Keys
            .OrderBy(static locale => locale, StringComparer.Ordinal)
            .ToArray();
    }
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
