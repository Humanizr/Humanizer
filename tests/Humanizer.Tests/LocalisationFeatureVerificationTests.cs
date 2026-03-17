using System.Text.RegularExpressions;

public class LocalisationFeatureVerificationTests
{
    static readonly Regex registryPattern = new("Register\\(\"(?<locale>[^\"]+)\"", RegexOptions.Compiled);

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

        return registryPattern.Matches(source)
            .Select(match => match.Groups["locale"].Value)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
    }
}
