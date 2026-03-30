using System.Globalization;
public class OrdinalizerRegistryRecoveryTests
{
    [Theory]
    [InlineData("af-ZA", 1, "1ste")]
    [InlineData("bg-BG", 12, "12.")]
    [InlineData("sr-Latn-RS", 4, "4.")]
    [InlineData("sv-SE", 1, "1:a")]
    [InlineData("sv-SE", 11, "11:e")]
    [InlineData("uz-Cyrl-UZ", 7, "7-чи")]
    [InlineData("uz-Latn-UZ", 7, "7-chi")]
    public void RecoveredLocalesOrdinalizeUsingTheirLocaleSpecificRules(string cultureName, int number, string expected) =>
        Assert.Equal(expected, number.Ordinalize(new CultureInfo(cultureName)));

    [Theory]
    [InlineData("ar", 1, "1")]
    [InlineData("ja-JP", 2, "2")]
    [InlineData("zh-Hans", 3, "3")]
    public void LocalesWithoutOrdinalizerOverridesUseTheRegistryDefault(string cultureName, int number, string expected) =>
        Assert.Equal(expected, number.Ordinalize(new CultureInfo(cultureName)));
}
