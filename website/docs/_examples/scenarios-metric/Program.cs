using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");

var symbol = 123_456.ToMetric(decimals: 1);
var fixedWidth = 1_000d.ToMetric(
    MetricNumeralFormats.KeepTrailingZeros,
    decimals: 2);
var name = 1_000d.ToMetric(
    MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName);
var scaleWord = 1.5E9.ToMetric(
    MetricNumeralFormats.KeepTrailingZeros |
    MetricNumeralFormats.WithSpace |
    MetricNumeralFormats.UseScaleWord,
    decimals: 2);
var parsed = "1.5k".FromMetric();

Console.WriteLine($"Symbol: {symbol}");
Console.WriteLine($"Fixed width: {fixedWidth}");
Console.WriteLine($"Prefix name: {name}");
Console.WriteLine($"Scale word: {scaleWord}");
Console.WriteLine($"Parsed: {parsed}");
