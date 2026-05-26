namespace Humanizer.Tests.Localisation.fi;

public class FinnishTimeSpanHumanizeTests
{
    [Theory]
    [InlineData("tunti", 1, 0)]
    [InlineData("2 tuntia", 2, 0)]
    [InlineData("minuutti", 0, 1)]
    public void Humanize_UsesFinnishPhrasesForFiFiCulture(string expected, int hours, int minutes)
    {
        var culture = new CultureInfo("fi-FI");
        var timeSpan = new TimeSpan(hours, minutes, 0);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture));
    }
}
