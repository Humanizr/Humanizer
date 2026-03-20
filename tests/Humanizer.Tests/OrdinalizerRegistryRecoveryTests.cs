using System.Globalization;
using System.Reflection;

public class OrdinalizerRegistryRecoveryTests
{
    [Fact]
    public void OrdinalizerRegistryContainsRecoveredLocaleRegistrations()
    {
        var registry = new OrdinalizerRegistry();
        var registrations = GetRegistrations(registry);

        Assert.Contains("af", registrations.Keys);
        Assert.Contains("bg", registrations.Keys);
        Assert.Contains("ja", registrations.Keys);
        Assert.Contains("pt-BR", registrations.Keys);
        Assert.Contains("sr-Latn", registrations.Keys);
        Assert.Contains("sv", registrations.Keys);
        Assert.Contains("uz-Cyrl-UZ", registrations.Keys);
        Assert.Contains("uz-Latn-UZ", registrations.Keys);
    }

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

    static Dictionary<string, Func<CultureInfo, IOrdinalizer>> GetRegistrations(OrdinalizerRegistry registry)
    {
        var field = typeof(LocaliserRegistry<IOrdinalizer>).GetField("localisersBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);

        var registrations = field.GetValue(registry) as Dictionary<string, Func<CultureInfo, IOrdinalizer>>;
        Assert.NotNull(registrations);

        return registrations;
    }
}
