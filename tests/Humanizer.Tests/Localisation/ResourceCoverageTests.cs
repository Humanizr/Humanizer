namespace Humanizer.Tests.Localisation;

public class ResourceCoverageTests
{
    static readonly double[] HeadingAngles = Enumerable.Range(0, 16)
        .Select(index => index * 22.5)
        .ToArray();

    [Fact]
    public void LocalizedResourceFilesDoNotIntroduceKeysMissingFromNeutralResources()
    {
        var neutralKeys = LocaleCoverageData.NeutralResourceKeys.ToHashSet(StringComparer.Ordinal);

        foreach (var (locale, resourceKeys) in LocaleCoverageData.LocalizedResourceKeysByLocale)
        {
            var extraKeys = resourceKeys
                .Where(key => !neutralKeys.Contains(key))
                .OrderBy(key => key, StringComparer.Ordinal)
                .ToArray();

            Assert.True(extraKeys.Length == 0, $"{locale} introduces keys missing from neutral Resources.resx: {string.Join(", ", extraKeys)}");
        }
    }

    [Fact]
    public void NeutralResourcesContainAllHeadingKeys()
    {
        var missingHeadingKeys = LocaleCoverageData.HeadingKeys
            .Where(key => !LocaleCoverageData.NeutralResourceKeys.Contains(key, StringComparer.Ordinal))
            .OrderBy(key => key, StringComparer.Ordinal)
            .ToArray();

        Assert.Empty(missingHeadingKeys);
    }

    [Fact]
    public void LocalizedResourceFilesDoNotContainBlankValues()
    {
        foreach (var (locale, entries) in LocaleCoverageData.LocalizedResourceValuesByLocale)
        {
            var blankKeys = entries
                .Where(static pair => string.IsNullOrWhiteSpace(pair.Value))
                .Select(static pair => pair.Key)
                .OrderBy(static key => key, StringComparer.Ordinal)
                .ToArray();

            Assert.True(blankKeys.Length == 0, $"{locale} contains blank resource values: {string.Join(", ", blankKeys)}");
        }
    }

    [Fact]
    public void HeadingResourcesRenderLocalizedStringsWhereLocalesShipHeadingKeys()
    {
        Assert.NotEmpty(LocaleCoverageData.LocalesWithCompleteHeadingResources);

        foreach (var locale in LocaleCoverageData.LocalesWithCompleteHeadingResources)
        {
            var culture = new CultureInfo(locale);
            var resourceKeys = LocaleCoverageData.LocalizedResourceKeysByLocale[locale];

            Assert.True(
                LocaleCoverageData.HeadingKeys.All(key => resourceKeys.Contains(key, StringComparer.Ordinal)),
                $"{locale} must ship the full heading resource set.");

            foreach (var heading in HeadingAngles)
            {
                var abbreviatedHeading = heading.ToHeading(culture: culture);
                Assert.False(string.IsNullOrWhiteSpace(abbreviatedHeading));

                var fullHeading = heading.ToHeading(HeadingStyle.Full, culture);
                Assert.False(string.IsNullOrWhiteSpace(fullHeading));
            }
        }
    }

    [Fact]
    public void HeadingResourcesRoundTripForLocalesWithExistingHeadingCoverage()
    {
        foreach (var locale in new[] { "de", "is", "ru" })
        {
            var culture = new CultureInfo(locale);

            foreach (var heading in HeadingAngles)
            {
                var abbreviatedHeading = heading.ToHeading(culture: culture);
                Assert.Equal(heading, abbreviatedHeading.FromAbbreviatedHeading(culture));
            }
        }
    }
}
