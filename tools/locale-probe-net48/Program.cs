// Locale probe for .NET Framework 4.8 (uses NLS on Windows, not ICU).
// Build and run on Windows:
//   dotnet build tools/locale-probe-net48/locale-probe-net48.csproj -c Release
//   tools/locale-probe-net48/bin/Release/net48/locale-probe-net48.exe --json > tools/probe-windows-net48.json
//   tools/locale-probe-net48/bin/Release/net48/locale-probe-net48.exe > tools/probe-windows-net48.tsv
//
// Note: .NET Framework 4.8 does NOT have TimeOnly. We use DateTime instead.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace LocaleProbeNet48;

internal static class Program
{
    private static readonly string[] AllLocales =
    {
        "af", "ar", "az", "bg", "bn", "ca", "cs", "da", "de", "de-CH", "de-LI",
        "el", "en", "en-GB", "en-IN", "en-US", "es", "fa", "fi", "fil", "fr",
        "fr-BE", "fr-CH", "he", "hr", "hu", "hy", "id", "is", "it", "ja", "ko",
        "ku", "lb", "lt", "lv", "ms", "mt", "nb", "nl", "nn", "pl", "pt", "pt-BR",
        "ro", "ru", "sk", "sl", "sr", "sr-Latn", "sv", "ta", "th", "tr", "uk",
        "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "zh-CN", "zh-Hans", "zh-Hant", "zu-ZA"
    };

    private static readonly string[] FailingLocales =
    {
        "ar", "bg", "bn", "ca", "el", "en", "en-IN", "en-US", "fa", "fr-CH",
        "he", "hr", "ja", "ku", "sk", "sl", "sr", "sr-Latn", "ta", "vi",
        "zh-CN", "zh-Hans", "zu-ZA"
    };

    private static readonly (string Label, int Y, int M, int D)[] Dates =
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

    private static readonly (string Label, int H, int M)[] Times =
    {
        ("01:05", 1, 5),
        ("13:23", 13, 23),
        ("13:25", 13, 25),
    };

    private static string ToVisibleText(string value) =>
        new string(value.Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.Format).ToArray());

    private static int Main(string[] args)
    {
        var jsonMode = args.Contains("--json");
        var onlyFailing = args.Contains("--failing");

        var locales = onlyFailing ? FailingLocales : AllLocales;

        Console.OutputEncoding = Encoding.UTF8;

        var framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        var osDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        if (jsonMode)
        {
            WriteJson(locales, framework, osDescription);
        }
        else
        {
            WriteTsv(locales, framework, osDescription);
        }

        return 0;
    }

    private static void WriteJson(string[] locales, string framework, string osDescription)
    {
        var result = new Dictionary<string, object>
        {
            ["environment"] = new Dictionary<string, string>
            {
                ["dotnet_version"] = Environment.Version.ToString(),
                ["framework"] = framework,
                ["os"] = osDescription,
                ["globalization"] = "NLS", // .NET Framework always uses NLS on Windows
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
                    ["dates"] = Dates.Select(d =>
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
                    ["times"] = Times.Select(t =>
                    {
                        // net48 has no TimeOnly — use DateTime with today's date
                        var dt = new DateTime(2024, 1, 1, t.H, t.M, 0);
                        return new Dictionary<string, string>
                        {
                            ["ref"] = t.Label,
                            ["short"] = ToVisibleText(dt.ToString("t", culture)),
                            ["long"] = ToVisibleText(dt.ToString("T", culture)),
                        };
                    }).ToArray(),
                };
            }).ToArray(),
        };

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static void WriteTsv(string[] locales, string framework, string osDescription)
    {
        Console.WriteLine("# Locale Probe");
        Console.WriteLine("# .NET: " + framework);
        Console.WriteLine("# OS:   " + osDescription);
        Console.WriteLine("# Glob: NLS (net48 uses Windows NLS, not ICU)");
        Console.WriteLine("# Time: " + DateTime.UtcNow.ToString("O"));
        Console.WriteLine();

        Console.WriteLine("## Date Formatting (ToString \"d\" and \"D\")");
        Console.WriteLine("locale\tdate\tshort_date\tlong_date");
        foreach (var loc in locales)
        {
            var culture = CultureInfo.GetCultureInfo(loc);
            foreach (var d in Dates)
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
            foreach (var t in Times)
            {
                var dt = new DateTime(2024, 1, 1, t.H, t.M, 0);
                Console.WriteLine($"{loc}\t{t.Label}\t{ToVisibleText(dt.ToString("t", culture))}\t{ToVisibleText(dt.ToString("T", culture))}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("## Month Names (used by ToOrdinalWords via \" MMMM yyyy\")");
        Console.WriteLine("locale\tdate\tmonth_phrase");
        foreach (var loc in locales)
        {
            var culture = CultureInfo.GetCultureInfo(loc);
            foreach (var d in Dates)
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
            var hex = string.Join(" ", sep.Select(c => $"U+{((int)c):X4}"));
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
}
