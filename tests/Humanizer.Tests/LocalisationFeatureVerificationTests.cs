using System.Text.Json;
using System.Text.RegularExpressions;

public class LocalisationFeatureVerificationTests
{
    static readonly Regex[] registryPatterns =
    [
        new("Register\\(\"(?<locale>[^\"]+)\"", RegexOptions.Compiled),
        new("RegisterDefaultConverter\\(\"(?<locale>[^\"]+)\"", RegexOptions.Compiled),
        new("RegisterDefaultOrdinalizer\\(\"(?<locale>[^\"]+)\"", RegexOptions.Compiled),
        new("RegisterNumericSuffixOrdinalizer\\(\"(?<locale>[^\"]+)\"", RegexOptions.Compiled),
        new("RegisterCzechSlovakPolishFormatter\\(\"(?<locale>[^\"]+)\"", RegexOptions.Compiled)
    ];

    [Fact]
    public void LocalizedResourceLocalesCanBeEnumeratedFromSource()
    {
        var locales = GetLocalizedResourceLocales();

        Assert.NotEmpty(locales);
        Assert.Contains("af", locales);
        Assert.Contains("zh-Hant", locales);
    }

    [Fact]
    public void RegistryRegistrationsCanBeEnumeratedFromSource()
    {
        var locales = GetRegisteredLocales("FormatterRegistry.cs");

        Assert.Contains("fr", locales);
        Assert.Contains("ru", locales);
    }

    [Fact]
    public void FormatterDefaultAcceptedLocalesNeedNoExplicitRegistration()
    {
        var locales = GetRegisteredLocales("FormatterRegistry.cs");

        Assert.DoesNotContain("af", locales);
        Assert.DoesNotContain("da", locales);
        Assert.DoesNotContain("es", locales);
        Assert.DoesNotContain("fil", locales);
        Assert.DoesNotContain("ku", locales);
        Assert.DoesNotContain("nl", locales);
        Assert.DoesNotContain("pt", locales);
        Assert.DoesNotContain("ta", locales);
    }

    [Fact]
    public void OrdinalizerRegistryCoversAllLocalizedResourceLocales()
    {
        var localizedLocales = GetLocalizedResourceLocales();
        var registeredLocales = GetRegisteredLocales("OrdinalizerRegistry.cs");

        Assert.Empty(localizedLocales.Except(registeredLocales));
    }

    [Fact]
    public void DateToOrdinalWordsRegistryCoversAllLocalizedResourceLocales()
    {
        var localizedLocales = GetLocalizedResourceLocales();
        var registeredLocales = GetRegisteredLocales("DateToOrdinalWordsConverterRegistry.cs");

        Assert.Empty(localizedLocales.Except(registeredLocales));
        Assert.Contains("en-US", registeredLocales);
    }

    [Fact]
    public void CollectionFormatterDefaultUsesAmpersand()
    {
        var formatter = new DefaultCollectionFormatter("&");

        Assert.Equal("a & b", formatter.Humanize(["a", "b"]));
    }

    [Fact]
    public void WordsToNumberDefaultThrowsForNonEnglishCultures()
    {
        var converter = new DefaultWordsToNumberConverter(new("fr"));

        Assert.Throws<NotSupportedException>(() => converter.Convert("un"));
    }

    [Fact]
    public void DateToOrdinalWordDefaultIsEnglishShaped()
    {
        var converter = new DefaultDateToOrdinalWordConverter();

        Assert.Equal("1st January 2026", converter.Convert(new(2026, 1, 1)));
    }

#if NET6_0_OR_GREATER
    [Fact]
    public void DateOnlyToOrdinalWordDefaultIsEnglishShaped()
    {
        var converter = new DefaultDateOnlyToOrdinalWordConverter();

        Assert.Equal("1st January 2026", converter.Convert(new(2026, 1, 1)));
    }

    [Fact]
    public void TimeOnlyClockNotationDefaultIsEnglishShaped()
    {
        var converter = new DefaultTimeOnlyToClockNotationConverter();

        Assert.Equal("five past five", converter.Convert(new(5, 5), ClockNotationRounding.None));
    }

    [Fact]
    public void DateOnlyToOrdinalWordsRegistryCoversAllLocalizedResourceLocales()
    {
        var localizedLocales = GetLocalizedResourceLocales();
        var registeredLocales = GetRegisteredLocales("DateOnlyToOrdinalWordsConverterRegistry.cs");

        Assert.Empty(localizedLocales.Except(registeredLocales));
        Assert.Contains("en-US", registeredLocales);
    }

    [Fact]
    public void TimeOnlyClockNotationRegistryCoversAllLocalizedResourceLocales()
    {
        var localizedLocales = GetLocalizedResourceLocales();
        var registeredLocales = GetRegisteredLocales("TimeOnlyToClockNotationConvertersRegistry.cs");

        Assert.Empty(localizedLocales.Except(registeredLocales));
    }
#endif

    [Fact]
    public void DefaultOrdinalizerReturnsOriginalNumberString()
    {
        var ordinalizer = new DefaultOrdinalizer();

        Assert.Equal("42", ordinalizer.Convert(42, "42"));
    }

    [Fact]
    public void SupportedNonResourceCulturesAreTracked()
    {
        Assert.Equal(["en-IN", "nn", "ta"], SupportedNonResourceCultures);
    }

    [Fact]
    public void SavedLocaleMatrixHasNoPendingCells()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(
            Path.Combine(RepoRoot, "docs", "plans", "2026-03-17-locale-feature-matrix.json")));

        var pendingCells = document.RootElement
            .GetProperty("locales")
            .EnumerateArray()
            .SelectMany(locale => locale.GetProperty("registries").EnumerateArray().Select(registry => new
            {
                Locale = locale.GetProperty("locale").GetString()!,
                Registry = registry.GetProperty("registry").GetString()!,
                CurrentState = registry.GetProperty("currentState").GetString()!,
                VerifiedOutcome = registry.GetProperty("verifiedOutcome").GetString()!
            }))
            .Where(cell => cell.CurrentState.Contains("pending", StringComparison.Ordinal) ||
                           cell.VerifiedOutcome == "pending")
            .Select(cell => $"{cell.Locale}/{cell.Registry}: {cell.CurrentState} -> {cell.VerifiedOutcome}")
            .ToArray();

        Assert.True(
            pendingCells.Length == 0,
            "Pending locale matrix cells remain:" + Environment.NewLine + string.Join(Environment.NewLine, pendingCells));
    }

    internal static string RepoRoot
    {
        get
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "src", "Humanizer", "Properties", "Resources.resx")))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            throw new InvalidOperationException("Could not locate repository root.");
        }
    }

    internal static string[] SupportedNonResourceCultures { get; } = ["en-IN", "nn", "ta"];

    internal static string[] GetLocalizedResourceLocales() =>
        Directory.EnumerateFiles(Path.Combine(RepoRoot, "src", "Humanizer", "Properties"), "Resources.*.resx")
            .Select(Path.GetFileNameWithoutExtension)
            .Select(name => name!["Resources.".Length..])
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

    internal static string[] GetRegisteredLocales(string registryFileName)
    {
        var source = File.ReadAllText(Path.Combine(RepoRoot, "src", "Humanizer", "Configuration", registryFileName));

        return registryPatterns
            .SelectMany(pattern => pattern.Matches(source).Select(match => match.Groups["locale"].Value))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
    }
}
