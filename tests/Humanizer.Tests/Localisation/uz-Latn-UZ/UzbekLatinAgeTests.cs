namespace Humanizer.Tests.Localisation.uzLatn;

[UseCulture("uz-Latn-UZ")]
public class UzbekLatinAgeTests
{
    static readonly CultureInfo UzLatn = new("uz-Latn-UZ");

    [Theory]
    [InlineData(14, "2 hafta")]
    [InlineData(366, "1 yil")]
    [InlineData(800, "2 yil")]
    public void ToAge_DoesNotAppendYosh(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).ToAge(UzLatn));
}