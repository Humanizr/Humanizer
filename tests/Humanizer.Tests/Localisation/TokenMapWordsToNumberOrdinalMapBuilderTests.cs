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
        // With three gender forms per number, there should be many entries
        Assert.True(map.Count >= 200, $"Expected at least 200 entries but got {map.Count}");

        // Verify all three gender forms for Russian 1st: default, feminine, neuter
        Assert.Equal(1, map["\u043f\u0435\u0440\u0432\u044b\u0439"]);   // первый (default/masculine)
        Assert.Equal(1, map["\u043f\u0435\u0440\u0432\u0430\u044f"]);   // первая (feminine)
        Assert.Equal(1, map["\u043f\u0435\u0440\u0432\u043e\u0435"]);   // первое (neuter)

        // Verify all three gender forms for Russian 3rd
        Assert.Equal(3, map["\u0442\u0440\u0435\u0442\u0438\u0439"]);   // третий (default/masculine)
        Assert.Equal(3, map["\u0442\u0440\u0435\u0442\u044c\u044f"]);   // третья (feminine)
        Assert.Equal(3, map["\u0442\u0440\u0435\u0442\u044c\u0435"]);   // третье (neuter)
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
        // With three gender forms per number, there should be many entries
        Assert.True(map.Count >= 200, $"Expected at least 200 entries but got {map.Count}");

        // Verify all three gender forms for Russian 10th
        Assert.Equal(10, map["\u0434\u0435\u0441\u044f\u0442\u044b\u0439"]);   // десятый (default/masculine)
        Assert.Equal(10, map["\u0434\u0435\u0441\u044f\u0442\u0430\u044f"]);   // десятая (feminine)
        Assert.Equal(10, map["\u0434\u0435\u0441\u044f\u0442\u043e\u0435"]);   // десятое (neuter)
    }
}