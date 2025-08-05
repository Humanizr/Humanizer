using System.Globalization;

namespace Humanizer.Tests.Localisation;

public class CompassDirectionTests
{
    [Theory]
    [InlineData("N", "bg", "С")]
    [InlineData("S", "bg", "Ю")]
    [InlineData("E", "bg", "И")]
    [InlineData("W", "bg", "З")]
    [InlineData("N", "ru", "С")]
    [InlineData("S", "ru", "Ю")]
    [InlineData("E", "ru", "В")]
    [InlineData("W", "ru", "З")]
    [InlineData("N", "de", "N")]
    [InlineData("E", "de", "O")]
    [InlineData("W", "de", "W")]
    [InlineData("N", "fr", "N")]
    [InlineData("W", "fr", "O")]
    public void CompassDirectionShortFormsShouldBeTranslated(string direction, string cultureName, string expected)
    {
        var culture = new CultureInfo(cultureName);
        var actual = Resources.GetResource(direction, culture);
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData("N_Short", "bg", "С")]
    [InlineData("S_Short", "bg", "Ю")]
    [InlineData("E_Short", "fr", "E")]
    [InlineData("W_Short", "fr", "O")]
    public void CompassDirectionShortResourcesShouldBeTranslated(string key, string cultureName, string expected)
    {
        var culture = new CultureInfo(cultureName);
        var actual = Resources.GetResource(key, culture);
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData("north", "bg", "север")]
    [InlineData("south", "bg", "юг")]
    [InlineData("east", "bg", "изток")]
    [InlineData("west", "bg", "запад")]
    [InlineData("north", "fr", "nord")]
    [InlineData("south", "fr", "sud")]
    [InlineData("east", "fr", "est")]
    [InlineData("west", "fr", "ouest")]
    public void CompassDirectionFullWordsShouldBeTranslated(string direction, string cultureName, string expected)
    {
        var culture = new CultureInfo(cultureName);
        var actual = Resources.GetResource(direction, culture);
        Assert.Equal(expected, actual);
    }
}