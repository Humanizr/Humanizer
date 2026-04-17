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

    [Fact]
    public void ExplicitNumericOrdinalLocalesAreRegistered()
    {
        var locales = new OrdinalizerRegistry().GetRegisteredLocaleCodes();

        Assert.Contains("ar", locales);
        Assert.Contains("he", locales);
        Assert.Contains("mt", locales);
        Assert.Contains("zu-ZA", locales);
    }

    [Theory]
    [InlineData("eo", 1, "1")]
    public void LocalesWithoutOrdinalizerEntriesUseTheRegistryDefault(string cultureName, int number, string expected) =>
        Assert.Equal(expected, number.Ordinalize(new CultureInfo(cultureName)));
}