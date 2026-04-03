namespace Humanizer.Tests.Localisation;

public class DefaultFormatterTests
{
    [Fact]
    [UseCulture("iv")]
    public void FallsBackToDefaultCollectionFormatterForUnsupportedCulture()
    {
        var dates = new[] { DateTime.UtcNow, DateTime.UtcNow.AddDays(10) };
        var humanized = dates.Humanize();

        Assert.Equal(dates[0] + " & " + dates[1], humanized);
    }
}
