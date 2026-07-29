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

AssertEqual("yesterday", relative);
AssertEqual("yesterday", offsetRelative);
AssertEqual("yesterday", dateOnlyRelative);
AssertEqual("a minute from now", timeOnlyRelative);
AssertEqual("2 hours, 5 minutes", duration);

Console.WriteLine($"{relative}; {offsetRelative}; {dateOnlyRelative}; {timeOnlyRelative}; {duration}");

static void AssertEqual(string expected, string actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
