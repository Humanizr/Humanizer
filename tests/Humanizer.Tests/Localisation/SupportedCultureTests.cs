namespace Humanizer.Tests.Localisation;

public class SupportedCultureTests
{
    [Fact]
    public void IsCultureSupportedReturnsTrueForGeneratedLocale()
    {
        Assert.True(Configurator.IsCultureSupported(new CultureInfo("fr")));
    }

    [Fact]
    public void IsCultureSupportedReturnsTrueWhenParentHasGeneratedLocaleSupport()
    {
        Assert.True(Configurator.IsCultureSupported(new CultureInfo("fr-FR")));
    }

    [Theory]
    [MemberData(nameof(UnsupportedCultures))]
    public void IsCultureSupportedReturnsFalseWithoutGeneratedLocaleSupport(CultureInfo culture)
    {
        Assert.False(Configurator.IsCultureSupported(culture));
    }

    [Fact]
    public void IsCultureSupportedThrowsForNullCulture()
    {
        Assert.Throws<ArgumentNullException>(() => Configurator.IsCultureSupported(null!));
    }

    public static TheoryData<CultureInfo> UnsupportedCultures =>
        new()
        {
            CultureInfo.InvariantCulture,
            new CultureInfo("eo")
        };
}