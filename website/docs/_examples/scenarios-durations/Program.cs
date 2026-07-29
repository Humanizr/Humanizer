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
var fractionalSeconds = TimeSpan.FromMilliseconds(1500)
    .HumanizeWithFractionalSeconds(
        precision: 1,
        maxFractionalDigits: 3,
        roundingMode: MidpointRounding.ToEven,
        culture: culture,
        maxUnit: TimeUnit.Second);
var parsedCompact = "1h 30m 15.25s".DehumanizeTimeSpan();
var parsedStandard = "1.02:03:04.0050060".DehumanizeTimeSpan();
var parsedInvalid = "1,5h".TryDehumanizeTimeSpan(out var invalid);
var age = TimeSpan.FromDays(750)
    .ToAge(culture, toWords: true);

AssertEqual("2 hours, 5 minutes", elapsed);
AssertEqual("1 minute and 2 seconds", compact);
AssertEqual("1h, 3s, 1ms", symbols);
AssertEqual("1.5 seconds", fractionalSeconds);
AssertEqual(new TimeSpan(0, 1, 30, 15, 250), parsedCompact);
AssertEqual(TimeSpan.FromTicks(937_840_050_060), parsedStandard);
AssertEqual(false, parsedInvalid);
AssertEqual(TimeSpan.Zero, invalid);
AssertEqual("two years old", age);

Console.WriteLine($"{elapsed}; {fractionalSeconds}; {symbols}; {age}");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
