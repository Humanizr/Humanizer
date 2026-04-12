#!/usr/bin/env dotnet run
#:property PublishAot=false
#pragma warning disable IDE0007, CA1869, IL2026, IL2075, IL3050
// Locale probe: outputs the exact ICU/NLS-dependent values that Humanizer tests assert.
// Run on each target OS to capture platform-specific expected values.
//
// Usage:
//   dotnet run tools/locale-probe.cs > locale-probe-macos.tsv
//   dotnet run tools/locale-probe.cs --json > locale-probe-macos.json
//   dotnet run tools/locale-probe.cs --failing   (only show known-failing locales)

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var jsonMode = args.Contains("--json");
var onlyFailing = args.Contains("--failing");

var dotnetVersion = Environment.Version.ToString();
var osDescription = RuntimeInformation.OSDescription;
var rid = RuntimeInformation.RuntimeIdentifier;
var framework = RuntimeInformation.FrameworkDescription;

// Strip Unicode format characters (matches test ToVisibleText)
static string ToVisibleText(string value) =>
    new(value.Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.Format).ToArray());

// All 62 shipped locales
string[] allLocales = [
    "af", "ar", "az", "bg", "bn", "ca", "cs", "da", "de", "de-CH", "de-LI",
    "el", "en", "en-GB", "en-IN", "en-US", "es", "fa", "fi", "fil", "fr",
    "fr-BE", "fr-CH", "he", "hr", "hu", "hy", "id", "is", "it", "ja", "ko",
    "ku", "lb", "lt", "lv", "ms", "mt", "nb", "nl", "nn", "pl", "pt", "pt-BR",
    "ro", "ru", "sk", "sl", "sr", "sr-Latn", "sv", "ta", "th", "tr", "uk",
    "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "zh-CN", "zh-Hans", "zh-Hant", "zu-ZA"
];

// Locales known to fail on macOS 26.4 / .NET 10.0.102
string[] failingLocales = [
    "ar", "bg", "bn", "ca", "el", "en", "en-IN", "en-US", "fa", "fr-CH",
    "he", "hr", "ja", "ku", "sk", "sl", "sr", "sr-Latn", "ta", "vi",
    "zh-CN", "zh-Hans", "zu-ZA"
];

var locales = onlyFailing ? failingLocales : allLocales;

// Reference dates (matching test data in LocaleCoverageData.cs)
var dates = new (string Label, int Y, int M, int D)[]
{
    ("2015-01-01", 2015, 1, 1),
    ("2015-02-03", 2015, 2, 3),
    ("2022-01-25", 2022, 1, 25),
    ("2020-02-29", 2020, 2, 29),
    ("2015-09-04", 2015, 9, 4),
    ("1979-11-07", 1979, 11, 7),
    ("2020-03-02", 2020, 3, 2),
    ("2021-10-31", 2021, 10, 31),
    ("2024-12-31", 2024, 12, 31),
};

// Reference times (matching test data)
var times = new (string Label, int H, int M)[]
{
    ("01:05", 1, 5),
    ("13:23", 13, 23),
    ("13:25", 13, 25),
};

Console.OutputEncoding = Encoding.UTF8;

if (jsonMode)
{
    var result = new Dictionary<string, object>
    {
        ["environment"] = new Dictionary<string, string>
        {
            ["dotnet_version"] = dotnetVersion,
            ["framework"] = framework,
            ["os"] = osDescription,
            ["rid"] = rid,
            ["timestamp"] = DateTime.UtcNow.ToString("O"),
        },
        ["locales"] = locales.Select(localeName =>
        {
            var culture = CultureInfo.GetCultureInfo(localeName);
            var dtf = culture.DateTimeFormat;
            return new Dictionary<string, object>
            {
                ["locale"] = localeName,
                ["number_decimal_separator"] = culture.NumberFormat.NumberDecimalSeparator,
                ["short_date_pattern"] = dtf.ShortDatePattern,
                ["long_date_pattern"] = dtf.LongDatePattern,
                ["short_time_pattern"] = dtf.ShortTimePattern,
                ["long_time_pattern"] = dtf.LongTimePattern,
                ["am_designator"] = ToVisibleText(dtf.AMDesignator),
                ["pm_designator"] = ToVisibleText(dtf.PMDesignator),
                ["month_names_raw"] = dtf.MonthNames.Take(12).ToArray(),
                ["month_genitive_names_raw"] = dtf.MonthGenitiveNames.Take(12).ToArray(),
                ["dates"] = dates.Select(d =>
                {
                    var dt = new DateTime(d.Y, d.M, d.D);
                    return new Dictionary<string, string>
                    {
                        ["ref"] = d.Label,
                        ["short"] = ToVisibleText(dt.ToString("d", culture)),
                        ["long"] = ToVisibleText(dt.ToString("D", culture)),
                        ["month_standalone"] = ToVisibleText(dt.ToString(" MMMM yyyy", culture).Trim()),
                    };
                }).ToArray(),
                ["times"] = times.Select(t =>
                {
                    var to = new TimeOnly(t.H, t.M);
                    return new Dictionary<string, string>
                    {
                        ["ref"] = t.Label,
                        ["short"] = ToVisibleText(to.ToString("t", culture)),
                        ["long"] = ToVisibleText(to.ToString("T", culture)),
                    };
                }).ToArray(),
            };
        }).ToArray(),
    };

    Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
}
else
{
    Console.WriteLine($"# Locale Probe");
    Console.WriteLine($"# .NET: {framework}");
    Console.WriteLine($"# OS:   {osDescription}");
    Console.WriteLine($"# RID:  {rid}");
    Console.WriteLine($"# Time: {DateTime.UtcNow:O}");
    Console.WriteLine();

    Console.WriteLine("## Date Formatting (ToString \"d\" and \"D\")");
    Console.WriteLine("locale\tdate\tshort_date\tlong_date");
    foreach (var loc in locales)
    {
        var culture = CultureInfo.GetCultureInfo(loc);
        foreach (var d in dates)
        {
            var dt = new DateTime(d.Y, d.M, d.D);
            Console.WriteLine($"{loc}\t{d.Label}\t{ToVisibleText(dt.ToString("d", culture))}\t{ToVisibleText(dt.ToString("D", culture))}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("## Time Formatting (ToString \"t\" and \"T\")");
    Console.WriteLine("locale\ttime\tshort_time\tlong_time");
    foreach (var loc in locales)
    {
        var culture = CultureInfo.GetCultureInfo(loc);
        foreach (var t in times)
        {
            var to = new TimeOnly(t.H, t.M);
            Console.WriteLine($"{loc}\t{t.Label}\t{ToVisibleText(to.ToString("t", culture))}\t{ToVisibleText(to.ToString("T", culture))}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("## Month Names (used by ToOrdinalWords via \" MMMM yyyy\")");
    Console.WriteLine("locale\tdate\tmonth_phrase");
    foreach (var loc in locales)
    {
        var culture = CultureInfo.GetCultureInfo(loc);
        foreach (var d in dates)
        {
            var dt = new DateTime(d.Y, d.M, d.D);
            Console.WriteLine($"{loc}\t{d.Label}\t{ToVisibleText(dt.ToString(" MMMM yyyy", culture).Trim())}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("## Number Decimal Separator");
    Console.WriteLine("locale\tdecimal_separator\thex");
    foreach (var loc in locales)
    {
        var culture = CultureInfo.GetCultureInfo(loc);
        var sep = culture.NumberFormat.NumberDecimalSeparator;
        var hex = string.Join(" ", sep.Select(c => $"U+{(int)c:X4}"));
        Console.WriteLine($"{loc}\t{sep}\t{hex}");
    }

    Console.WriteLine();
    Console.WriteLine("## Culture Patterns");
    Console.WriteLine("locale\tshort_date_pattern\tlong_date_pattern\tshort_time_pattern\tlong_time_pattern\tam\tpm");
    foreach (var loc in locales)
    {
        var culture = CultureInfo.GetCultureInfo(loc);
        var dtf = culture.DateTimeFormat;
        Console.WriteLine($"{loc}\t{dtf.ShortDatePattern}\t{dtf.LongDatePattern}\t{dtf.ShortTimePattern}\t{dtf.LongTimePattern}\t{ToVisibleText(dtf.AMDesignator)}\t{ToVisibleText(dtf.PMDesignator)}");
    }
}
