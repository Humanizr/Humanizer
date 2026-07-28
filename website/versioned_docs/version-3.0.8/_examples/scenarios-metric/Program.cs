using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var symbol = 123_456.ToMetric(decimals: 1);
var name = 1_000d.ToMetric(
    MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName);
var parsed = "1.5k".FromMetric();

AssertEqual("123.5k", symbol);
AssertEqual("1 kilo", name);
AssertEqual(1500d, parsed);

Console.WriteLine($"{symbol}; {name}; {parsed}");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
