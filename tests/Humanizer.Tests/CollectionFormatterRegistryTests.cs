public class CollectionFormatterRegistryTests
{
    [Theory]
    [InlineData("af", "1 en 2", "1, 2 en 3")]
    [InlineData("az", "1 və 2", "1, 2 və 3")]
    [InlineData("bg", "1 и 2", "1, 2 и 3")]
    [InlineData("bn", "1 ও 2", "1, 2 ও 3")]
    [InlineData("cs", "1 a 2", "1, 2 a 3")]
    [InlineData("el", "1 και 2", "1, 2 και 3")]
    [InlineData("fi", "1 ja 2", "1, 2 ja 3")]
    [InlineData("fil", "1 at 2", "1, 2 at 3")]
    [InlineData("fr", "1 et 2", "1, 2 et 3")]
    [InlineData("hr", "1 i 2", "1, 2 i 3")]
    [InlineData("hu", "1 és 2", "1, 2 és 3")]
    [InlineData("hy", "1 և 2", "1, 2 և 3")]
    [InlineData("id", "1 dan 2", "1, 2 dan 3")]
    [InlineData("ko", "1 및 2", "1, 2 및 3")]
    [InlineData("ku", "1 û 2", "1, 2 û 3")]
    [InlineData("lt", "1 ir 2", "1, 2 ir 3")]
    [InlineData("lv", "1 un 2", "1, 2 un 3")]
    [InlineData("ms", "1 dan 2", "1, 2 dan 3")]
    [InlineData("mt", "1 u 2", "1, 2 u 3")]
    [InlineData("pl", "1 i 2", "1, 2 i 3")]
    [InlineData("pt-BR", "1 e 2", "1, 2 e 3")]
    [InlineData("ru", "1 и 2", "1, 2 и 3")]
    [InlineData("sk", "1 a 2", "1, 2 a 3")]
    [InlineData("sl", "1 in 2", "1, 2 in 3")]
    [InlineData("sr", "1 и 2", "1, 2 и 3")]
    [InlineData("sr-Latn", "1 i 2", "1, 2 i 3")]
    [InlineData("th", "1 และ 2", "1, 2 และ 3")]
    [InlineData("tr", "1 ve 2", "1, 2 ve 3")]
    [InlineData("uk", "1 і 2", "1, 2 і 3")]
    [InlineData("uz-Cyrl-UZ", "1 ва 2", "1, 2 ва 3")]
    [InlineData("uz-Latn-UZ", "1 va 2", "1, 2 va 3")]
    [InlineData("vi", "1 và 2", "1, 2 và 3")]
    public void RegisteredCollectionFormatterProducesExpectedConjunction(string locale, string expectedTwo, string expectedThree)
    {
        var formatter = Configurator.CollectionFormatters.ResolveForCulture(new(locale));

        Assert.Equal(expectedTwo, formatter.Humanize([1, 2]));
        Assert.Equal(expectedThree, formatter.Humanize([1, 2, 3]));
    }
}
