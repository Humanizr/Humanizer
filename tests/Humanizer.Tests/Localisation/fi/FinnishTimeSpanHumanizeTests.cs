using System.Globalization;

namespace Humanizer.Tests.Localisation.fi;

public class FinnishTimeSpanHumanizeTests
{
    [Theory]
    [InlineData(1, "tunti")]
    [InlineData(2, "2 tuntia")]
    [InlineData(1, "minuutti", 0, 1)]
    public void Humanize_UsesFinnishPhrasesForFiFiCulture(int amount, string expected, int hours = 1, int minutes = 0)
    {
        var culture = new CultureInfo("fi-FI");
        var timeSpan = new TimeSpan(hours, minutes, 0);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture));
    }
}
