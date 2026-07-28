using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");

var parsed = "two hundred five".ToNumber(culture);
var succeeded = "negative forty-two".TryToNumber(
    out var negative,
    culture,
    out var unrecognized);
var rejected = "two otters".TryToNumber(
    out var invalid,
    culture,
    out var invalidWord);

AssertEqual(205, parsed);
AssertEqual(true, succeeded);
AssertEqual(-42, negative);
AssertEqual(null, unrecognized);
AssertEqual(false, rejected);
AssertEqual(0, invalid);
AssertEqual("otters", invalidWord);

Console.WriteLine("205; -42; rejected at otters");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
