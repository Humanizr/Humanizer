using System.Globalization;
using Humanizer;
#if HUMANIZER_V2
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
#endif

var culture = CultureInfo.GetCultureInfo("en-US");
var comparison = new DateTime(2025, 1, 20, 12, 0, 0, DateTimeKind.Utc);

Configurator.DateTimeHumanizeStrategy =
    new PrecisionDateTimeHumanizeStrategy(precision: 0.75);

var result = comparison.AddMinutes(-45).Humanize(
    utcDate: true,
    dateToCompareAgainst: comparison,
    culture: culture);

if (result != "an hour ago")
    throw new InvalidOperationException($"Unexpected result: {result}");

Console.WriteLine(result);
