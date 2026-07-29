namespace Humanizer.Tests.Localisation;

public class FractionalTimeSpanLocaleSweepTests
{
    public static IEnumerable<object?[]> ShippedLocaleRows =>
        LocaleCoverageData.ShippedLocales.Select(static localeName => new object?[] { localeName });

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void EveryLocaleFormatsCommonAndPluralBoundaryValues(string localeName)
    {
        var culture = new CultureInfo(localeName);
        decimal[] seconds = [0m, 0.5m, 1m, 1.5m, 2m, 2.1m, 3m, 6m, 11m, 21m, 101m];

        foreach (var value in seconds)
        {
            var timeSpan = TimeSpan.FromTicks(decimal.ToInt64(value * TimeSpan.TicksPerSecond));
            var words = timeSpan.HumanizeWithFractionalSeconds(
                1,
                7,
                MidpointRounding.ToEven,
                culture,
                TimeUnit.Second);
            var symbols = timeSpan.HumanizeToSymbolsWithFractionalSeconds(
                1,
                7,
                MidpointRounding.ToEven,
                culture,
                TimeUnit.Second);

            Assert.False(string.IsNullOrWhiteSpace(words));
            Assert.False(string.IsNullOrWhiteSpace(symbols));
        }
    }
}