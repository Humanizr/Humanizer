using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
var comparison = new DateTime(2025, 1, 20, 12, 0, 0, DateTimeKind.Utc);

Configurator.DateTimeHumanizeStrategy =
    new PrecisionDateTimeHumanizeStrategy(precision: 0.75);
Configurator.TimeSpanHumanizeStrategy =
    new SingleUnitTimeSpanHumanizeStrategy();

var result = comparison.AddMinutes(-45).Humanize(
    utcDate: true,
    dateToCompareAgainst: comparison,
    culture: culture);
var duration = TimeSpan.FromMinutes(62).Humanize(
    precision: 2,
    culture: culture);

Console.WriteLine($"{result}; {duration}");

sealed class SingleUnitTimeSpanHumanizeStrategy : ITimeSpanHumanizeStrategy
{
    readonly DefaultTimeSpanHumanizeStrategy defaultStrategy = new();

    public string Humanize(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        string? collectionSeparator,
        bool toWords,
        bool toSymbols) =>
        defaultStrategy.Humanize(
            timeSpan,
            Math.Min(precision, 1),
            countEmptyUnits,
            culture,
            maxUnit,
            minUnit,
            collectionSeparator,
            toWords,
            toSymbols);
}
