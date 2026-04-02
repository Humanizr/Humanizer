using System.Globalization;

namespace Humanizer.Tests.Localisation;

public static class LocaleCoverageData
{
    static readonly Lazy<Dictionary<string, string>> localeFiles = new(LoadLocaleFiles);

    public static TheoryData<string> NumberToWordsLocaleTheoryData => CreateNestedSurfaceTheoryData("number", "words");

    public static TheoryData<string> OrdinalizerLocaleTheoryData => CreateNestedSurfaceTheoryData("ordinal", "numeric");

    public static TheoryData<string> DateToOrdinalWordsLocaleTheoryData => CreateOrdinalDateTheoryData("date");

    public static TheoryData<string, string> DateToOrdinalWordsExpectationTheoryData => new()
    {
        { "ca", "1 de gener de 2015" },
        { "de", "1. Januar 2015" },
        { "es", "1 de enero de 2015" },
        { "fr", "1er janvier 2015" },
        { "lb", "1. Januar 2015" },
        { "lv", "1. janvāris 2015" },
        { "pt-BR", "1º de janeiro de 2015" }
    };

    public static TheoryData<string> DateOnlyToOrdinalWordsLocaleTheoryData => CreateOrdinalDateTheoryData("dateOnly");

    public static TheoryData<string, string> DateOnlyToOrdinalWordsExpectationTheoryData => new()
    {
        { "ca", "1 de gener de 2015" },
        { "de", "1. Januar 2015" },
        { "es", "1 de enero de 2015" },
        { "fr", "1er janvier 2015" },
        { "lb", "1. Januar 2015" },
        { "lv", "1. janvāris 2015" },
        { "pt-BR", "1º de janeiro de 2015" }
    };

    public static TheoryData<string> TimeOnlyToClockNotationLocaleTheoryData => CreateSurfaceTheoryData("clock");

    public static TheoryData<string, int, int, string> TimeOnlyToClockNotationExpectationTheoryData => new()
    {
        { "ca", 13, 23, "la una i vint-i-cinc de la tarda" },
        { "de", 13, 23, "fünf vor halb zwei" },
        { "es", 13, 23, "la una y veinticinco de la tarde" },
        { "fr", 13, 23, "treize heures vingt-cinq" },
        { "lb", 13, 23, "fënnef vir hallwer zwou" },
        { "pt", 13, 23, "uma e vinte e cinco" },
        { "pt-BR", 0, 0, "meia-noite" }
    };

    public static TheoryData<string> WordsToNumberLocaleTheoryData => new()
    {
        "en",
        "en-US",
        "en-GB",
        "en-IN"
    };

    public static TheoryData<string, string> WordsToNumberFallbackLocaleTheoryData => new()
    {
        { "nn", "one hundred and five" },
        { "ta", "one hundred and five" }
    };

    public static IDisposable UseCulture(string cultureName) => new CultureScope(new CultureInfo(cultureName));

    public static bool SupportsSurface(string localeName, string surfaceName) =>
        localeFiles.Value.TryGetValue(localeName, out var text) &&
        (HasSurface(text, surfaceName) || surfaceName switch
        {
            "number" => HasNestedSurface(text, "number", "words"),
            "ordinal" => HasNestedSurface(text, "ordinal", "numeric"),
            _ => false
        });

    static TheoryData<string> CreateSurfaceTheoryData(string surfaceName)
    {
        var data = new TheoryData<string>();

        foreach (var locale in localeFiles.Value.Keys
                     .Where(locale => HasSurface(localeFiles.Value[locale], surfaceName))
                     .OrderBy(static locale => locale, StringComparer.Ordinal))
        {
            data.Add(locale);
        }

        return data;
    }

    static TheoryData<string> CreateNestedSurfaceTheoryData(string surfaceName, string memberName)
    {
        var data = new TheoryData<string>();

        foreach (var locale in localeFiles.Value.Keys
                     .Where(locale => HasNestedSurface(localeFiles.Value[locale], surfaceName, memberName))
                     .OrderBy(static locale => locale, StringComparer.Ordinal))
        {
            data.Add(locale);
        }

        return data;
    }

    static TheoryData<string> CreateOrdinalDateTheoryData(string memberName)
    {
        var data = new TheoryData<string>();

        foreach (var locale in localeFiles.Value.Keys
                     .Where(locale => HasNestedSurface(localeFiles.Value[locale], "ordinal", memberName))
                     .OrderBy(static locale => locale, StringComparer.Ordinal))
        {
            data.Add(locale);
        }

        return data;
    }

    static bool HasSurface(string text, string surfaceName) =>
        text.Contains($"\n  {surfaceName}:", StringComparison.Ordinal);

    static bool HasNestedSurface(string text, string surfaceName, string memberName)
    {
        var surfaceIndex = text.IndexOf($"\n  {surfaceName}:", StringComparison.Ordinal);
        if (surfaceIndex < 0)
        {
            return false;
        }

        return text.IndexOf($"\n    {memberName}:", surfaceIndex, StringComparison.Ordinal) >= 0;
    }

    static Dictionary<string, string> LoadLocaleFiles()
    {
        var localesDirectory = FindLocalesDirectory();
        return Directory.EnumerateFiles(localesDirectory, "*.yml")
            .OrderBy(static path => path, StringComparer.Ordinal)
            .ToDictionary(
                static path => Path.GetFileNameWithoutExtension(path),
                static path => File.ReadAllText(path).Replace("\r\n", "\n"),
                StringComparer.Ordinal);
    }

    static string FindLocalesDirectory()
    {
        for (var current = new DirectoryInfo(AppContext.BaseDirectory); current is not null; current = current.Parent)
        {
            var localesDirectory = Path.Combine(current.FullName, "src", "Humanizer", "Locales");
            if (Directory.Exists(localesDirectory))
            {
                return localesDirectory;
            }
        }

        throw new DirectoryNotFoundException("Could not find src/Humanizer/Locales from the test output directory.");
    }

    sealed class CultureScope : IDisposable
    {
        readonly CultureInfo originalCulture = CultureInfo.CurrentCulture;
        readonly CultureInfo originalUiCulture = CultureInfo.CurrentUICulture;

        public CultureScope(CultureInfo culture)
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }
}
