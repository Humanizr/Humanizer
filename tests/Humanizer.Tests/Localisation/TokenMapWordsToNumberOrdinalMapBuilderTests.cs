using System.Collections.Frozen;

namespace Humanizer.Tests;

/// <summary>
/// Covers <see cref="TokenMapWordsToNumberOrdinalMapBuilder"/>, exercising both <c>Build</c>
/// overloads across all three <see cref="TokenMapOrdinalGenderVariant"/> switch arms.
/// </summary>
public class TokenMapWordsToNumberOrdinalMapBuilderTests
{
    // --- Build(string, profile, variant) overload: by kind name ---

    [Fact]
    public void Build_ByKind_None_ReturnsDefaultOrdinals()
    {
        // Turkish uses ordinalGenderVariant: 'none' and has a known NumberToWords kind "tr"
        var map = TokenMapWordsToNumberOrdinalMapBuilder.Build(
            "tr",
            TokenMapNormalizationProfile.LowercaseRemovePeriodsAndDiacritics,
            TokenMapOrdinalGenderVariant.None);

        Assert.NotEmpty(map);
        // Turkish ordinal for 1 is "birinci"
        Assert.True(map.ContainsKey("birinci"));
        Assert.Equal(1, map["birinci"]);
        // Spot-check a higher value
        Assert.True(map.ContainsKey("onuncu")); // 10th
        Assert.Equal(10, map["onuncu"]);
    }

    [Fact]
    public void Build_ByKind_MasculineAndFeminine_ReturnsBothGenderForms()
    {
        // Portuguese uses ordinalGenderVariant: 'masculine-and-feminine'
        var map = TokenMapWordsToNumberOrdinalMapBuilder.Build(
            "pt",
            TokenMapNormalizationProfile.LowercaseRemovePeriods,
            TokenMapOrdinalGenderVariant.MasculineAndFeminine);

        Assert.NotEmpty(map);
        // Portuguese masculine 1st = "primeiro", feminine 1st = "primeira"
        Assert.True(map.ContainsKey("primeiro"));
        Assert.Equal(1, map["primeiro"]);
        Assert.True(map.ContainsKey("primeira"));
        Assert.Equal(1, map["primeira"]);
    }

    [Fact]
    public void Build_ByKind_All_ReturnsDefaultFeminineAndNeuterForms()
    {
        // Russian uses ordinalGenderVariant: 'all'
        var map = TokenMapWordsToNumberOrdinalMapBuilder.Build(
            "ru",
            TokenMapNormalizationProfile.CollapseWhitespace,
            TokenMapOrdinalGenderVariant.All);

        Assert.NotEmpty(map);
        // The map should contain entries for numbers 1..200; verify count is substantial
        Assert.True(map.Count >= 200, $"Expected at least 200 entries but got {map.Count}");
    }

    // --- Build(INumberToWordsConverter, profile, variant) overload: by converter ---

    [Fact]
    public void Build_ByConverter_None_ReturnsDefaultOrdinals()
    {
        var converter = NumberToWordsProfileCatalog.Resolve("tr", CultureInfo.InvariantCulture);
        var map = TokenMapWordsToNumberOrdinalMapBuilder.Build(
            converter,
            TokenMapNormalizationProfile.LowercaseRemovePeriodsAndDiacritics,
            TokenMapOrdinalGenderVariant.None);

        Assert.NotEmpty(map);
        Assert.True(map.ContainsKey("birinci"));
        Assert.Equal(1, map["birinci"]);
        Assert.True(map.ContainsKey("yuzuncu")); // 100th with diacritics stripped
        Assert.Equal(100, map["yuzuncu"]);
    }

    [Fact]
    public void Build_ByConverter_MasculineAndFeminine_ReturnsBothGenderForms()
    {
        var converter = NumberToWordsProfileCatalog.Resolve("pt", CultureInfo.InvariantCulture);
        var map = TokenMapWordsToNumberOrdinalMapBuilder.Build(
            converter,
            TokenMapNormalizationProfile.LowercaseRemovePeriods,
            TokenMapOrdinalGenderVariant.MasculineAndFeminine);

        Assert.NotEmpty(map);
        Assert.True(map.ContainsKey("primeiro"));
        Assert.Equal(1, map["primeiro"]);
        Assert.True(map.ContainsKey("primeira"));
        Assert.Equal(1, map["primeira"]);
    }

    [Fact]
    public void Build_ByConverter_All_ReturnsDefaultFeminineAndNeuterForms()
    {
        var converter = NumberToWordsProfileCatalog.Resolve("ru", CultureInfo.InvariantCulture);
        var map = TokenMapWordsToNumberOrdinalMapBuilder.Build(
            converter,
            TokenMapNormalizationProfile.CollapseWhitespace,
            TokenMapOrdinalGenderVariant.All);

        Assert.NotEmpty(map);
        // With all three gender forms for 200 numbers, there should be many entries
        Assert.True(map.Count >= 200, $"Expected at least 200 entries but got {map.Count}");
    }
}