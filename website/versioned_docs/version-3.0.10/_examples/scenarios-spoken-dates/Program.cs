using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var date = new DateTime(2022, 1, 25).ToOrdinalWords();
var time = new TimeOnly(13, 23)
    .ToClockNotation(ClockNotationRounding.NearestFiveMinutes);

AssertEqual("January 25th, 2022", date);
AssertEqual("twenty-five past one", time);

Console.WriteLine($"{date}; {time}");

static void AssertEqual(string expected, string actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
