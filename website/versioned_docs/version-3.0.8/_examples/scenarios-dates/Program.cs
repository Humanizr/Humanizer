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
var duration = TimeSpan.FromMinutes(125).Humanize(precision: 2, culture: culture);

AssertEqual("yesterday", relative);
AssertEqual("2 hours, 5 minutes", duration);

Console.WriteLine($"{relative}; {duration}");

static void AssertEqual(string expected, string actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
