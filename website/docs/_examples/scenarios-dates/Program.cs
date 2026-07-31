using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var comparison = new DateTime(2025, 1, 20, 12, 0, 0, DateTimeKind.Utc);
var relative = comparison.AddDays(-1).Humanize(
    utcDate: true,
    dateToCompareAgainst: comparison,
    culture: culture);
var offsetComparison = new DateTimeOffset(comparison);
var offsetRelative = offsetComparison.AddDays(-1).Humanize(offsetComparison, culture);
var dateOnlyComparison = new DateOnly(2025, 1, 20);
var dateOnlyRelative = dateOnlyComparison.AddDays(-1).Humanize(dateOnlyComparison, culture);
var timeOnlyComparison = new TimeOnly(12, 0);
var timeOnlyRelative = timeOnlyComparison.AddMinutes(1).Humanize(timeOnlyComparison, culture: culture);
var duration = TimeSpan.FromMinutes(125).Humanize(precision: 2, culture: culture);

Console.WriteLine($"DateTime: {relative}");
Console.WriteLine($"DateTimeOffset: {offsetRelative}");
Console.WriteLine($"DateOnly: {dateOnlyRelative}");
Console.WriteLine($"TimeOnly: {timeOnlyRelative}");
Console.WriteLine($"Duration: {duration}");
