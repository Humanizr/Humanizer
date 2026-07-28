using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var elapsed = TimeSpan.FromMinutes(125)
    .Humanize(precision: 2, culture: culture);
var compact = TimeSpan.FromSeconds(62)
    .Humanize(precision: 2, culture: culture, collectionSeparator: null);
var symbols = TimeSpan.FromMilliseconds(3_603_001)
    .HumanizeToSymbols(precision: 3, culture: culture);
var age = TimeSpan.FromDays(750)
    .ToAge(culture, toWords: true);

AssertEqual("2 hours, 5 minutes", elapsed);
AssertEqual("1 minute and 2 seconds", compact);
AssertEqual("1h, 3s, 1ms", symbols);
AssertEqual("two years old", age);

Console.WriteLine($"{elapsed}; {symbols}; {age}");

static void AssertEqual(string expected, string actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
