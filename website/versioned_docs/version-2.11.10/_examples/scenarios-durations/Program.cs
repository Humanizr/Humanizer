using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var elapsed = TimeSpan.FromMinutes(125)
    .Humanize(precision: 2, culture: culture);
var compact = TimeSpan.FromSeconds(62)
    .Humanize(precision: 2, culture: culture, collectionSeparator: null);
AssertEqual("2 hours, 5 minutes", elapsed);
AssertEqual("1 minute and 2 seconds", compact);

Console.WriteLine($"{elapsed}; {compact}");

static void AssertEqual(string expected, string actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
