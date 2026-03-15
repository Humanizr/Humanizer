using System.Collections;
using System.Globalization;
using System.Reflection;

namespace Humanizer.Tests;

public class LocalisationRegistryCoverageTests
{
    static readonly string[] LocalizedCultures =
    [
        "af", "ar", "az", "bg", "bn", "ca", "cs", "da", "de", "el", "es", "fa", "fi", "fil", "fr",
        "he", "hr", "hu", "hy", "id", "is", "it", "ja", "ko", "ku", "lb", "lt", "lv", "ms", "mt",
        "nb", "nl", "pl", "pt", "pt-BR", "ro", "ru", "sk", "sl", "sr", "sr-Latn", "sv", "th", "tr",
        "uk", "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "zh-CN", "zh-Hans", "zh-Hant"
    ];

    [Fact]
    public void CollectionFormatterRegistry_has_explicit_entries_for_all_localized_cultures() =>
        AssertRegistryContainsAllLocalizedCultures(new CollectionFormatterRegistry(), "en", "nn");

    [Fact]
    public void NumberToWordsConverterRegistry_has_explicit_entries_for_all_localized_cultures() =>
        AssertRegistryContainsAllLocalizedCultures(
            new NumberToWordsConverterRegistry(),
            "de-CH",
            "de-LI",
            "en",
            "en-IN",
            "fr-BE",
            "fr-CH",
            "ta");

    [Fact]
    public void OrdinalizerRegistry_has_explicit_entries_for_all_localized_cultures() =>
        AssertRegistryContainsAllLocalizedCultures(new OrdinalizerRegistry(), "en");

    [Fact]
    public void DateToOrdinalWordsConverterRegistry_has_explicit_entries_for_all_localized_cultures() =>
        AssertRegistryContainsAllLocalizedCultures(new DateToOrdinalWordsConverterRegistry(), "en-US");

#if NET6_0_OR_GREATER
    [Fact]
    public void DateOnlyToOrdinalWordsConverterRegistry_has_explicit_entries_for_all_localized_cultures() =>
        AssertRegistryContainsAllLocalizedCultures(new DateOnlyToOrdinalWordsConverterRegistry(), "en-US");

    [Fact]
    public void TimeOnlyToClockNotationConvertersRegistry_has_explicit_entries_for_all_localized_cultures() =>
        AssertRegistryContainsAllLocalizedCultures(new TimeOnlyToClockNotationConvertersRegistry(), "en");
#endif

    [Fact]
    public void WordsToNumberConverterRegistry_has_explicit_entries_for_all_localized_cultures() =>
        AssertRegistryContainsAllLocalizedCultures(
            new WordsToNumberConverterRegistry(),
            "de-CH",
            "de-LI",
            "en",
            "en-US",
            "en-GB",
            "fr-BE",
            "fr-CH",
            "ta");

    static void AssertRegistryContainsAllLocalizedCultures<TLocaliser>(
        LocaliserRegistry<TLocaliser> registry,
        params string[] allowedNonLocalizedCultures)
        where TLocaliser : class
    {
        var registeredCultures = GetRegisteredCultureNames(registry);
        var unexpectedMissing = LocalizedCultures.Except(registeredCultures).ToArray();

        Assert.True(
            unexpectedMissing.Length == 0,
            $"Missing explicit registrations: {string.Join(", ", unexpectedMissing)}");

        Assert.All(registeredCultures, cultureName =>
        {
            Assert.True(
                LocalizedCultures.Contains(cultureName) || allowedNonLocalizedCultures.Contains(cultureName),
                $"Unexpected registration '{cultureName}' in {registry.GetType().Name}");
        });
    }

    static string[] GetRegisteredCultureNames<TLocaliser>(LocaliserRegistry<TLocaliser> registry)
        where TLocaliser : class
    {
        var field = typeof(LocaliserRegistry<TLocaliser>).GetField("localisersBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);

        var registrations = Assert.IsAssignableFrom<IDictionary>(field!.GetValue(registry));
        return registrations.Keys
            .Cast<string>()
            .OrderBy(static cultureName => cultureName, StringComparer.Ordinal)
            .ToArray();
    }
}
