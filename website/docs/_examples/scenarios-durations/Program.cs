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
var age = TimeSpan.FromDays(750)
    .ToAge(culture, toWords: true);

Console.WriteLine($"Elapsed: {elapsed}");
Console.WriteLine($"Natural list: {compact}");
Console.WriteLine($"Symbols: {symbols}");
Console.WriteLine($"Fractional: {fractionalSeconds}");
Console.WriteLine($"Parsed: {parsedCompact}");
Console.WriteLine($"Age: {age}");
