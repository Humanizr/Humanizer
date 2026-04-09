#!/usr/bin/env dotnet run
#:property PublishAot=false
#pragma warning disable IDE0007, CA1869, IL2026, IL2075, IL3050
// Compare probe JSON files across platforms and identify values that differ.
// Usage:
//   dotnet run tools/compare-probes.cs

using System.Globalization;
using System.Text;
using System.Text.Json;

var files = new (string Label, string Path)[]
{
    ("macOS",    "tools/probe-macos.json"),
    ("Linux",    "tools/probe-linux.json"),
    ("Win10",    "tools/probe-windows-net10.json"),
    ("Win48",    "tools/probe-windows-net48.json"),
};

Console.OutputEncoding = Encoding.UTF8;

var data = new Dictionary<string, JsonDocument>();
foreach (var (label, path) in files)
{
    if (!File.Exists(path))
    {
        Console.WriteLine($"MISSING: {path}");
        return;
    }
    data[label] = JsonDocument.Parse(File.ReadAllText(path));
}

// Build a lookup: (field, locale, key) -> platform -> value
var allLocales = data.First().Value.RootElement.GetProperty("locales")
    .EnumerateArray().Select(l => l.GetProperty("locale").GetString()!).ToArray();

// Collect all values into a flat table: (locale, category, ref, platform) -> value
var matrix = new Dictionary<(string Locale, string Category, string Key), Dictionary<string, string>>();

foreach (var (label, doc) in data)
{
    foreach (var localeEl in doc.RootElement.GetProperty("locales").EnumerateArray())
    {
        var locale = localeEl.GetProperty("locale").GetString()!;

        // Scalar properties
        foreach (var propName in new[] { "number_decimal_separator", "short_date_pattern", "long_date_pattern", "short_time_pattern", "long_time_pattern", "am_designator", "pm_designator" })
        {
            var val = localeEl.GetProperty(propName).GetString() ?? "";
            var key = (locale, "meta", propName);
            if (!matrix.ContainsKey(key)) matrix[key] = new();
            matrix[key][label] = val;
        }

        // Dates
        foreach (var dateEl in localeEl.GetProperty("dates").EnumerateArray())
        {
            var dateRef = dateEl.GetProperty("ref").GetString()!;
            foreach (var field in new[] { "short", "long", "month_standalone" })
            {
                var val = dateEl.GetProperty(field).GetString() ?? "";
                var key = (locale, "date_" + field, dateRef);
                if (!matrix.ContainsKey(key)) matrix[key] = new();
                matrix[key][label] = val;
            }
        }

        // Times
        foreach (var timeEl in localeEl.GetProperty("times").EnumerateArray())
        {
            var timeRef = timeEl.GetProperty("ref").GetString()!;
            foreach (var field in new[] { "short", "long" })
            {
                var val = timeEl.GetProperty(field).GetString() ?? "";
                var key = (locale, "time_" + field, timeRef);
                if (!matrix.ContainsKey(key)) matrix[key] = new();
                matrix[key][label] = val;
            }
        }
    }
}

// Find entries where not all platforms agree
var differences = matrix
    .Where(kv => kv.Value.Values.Distinct().Count() > 1)
    .OrderBy(kv => kv.Key.Category)
    .ThenBy(kv => kv.Key.Locale)
    .ThenBy(kv => kv.Key.Key)
    .ToArray();

Console.WriteLine($"Total data points: {matrix.Count}");
Console.WriteLine($"Differing data points: {differences.Length}");
Console.WriteLine();

// Group by (category, locale) to show which locales have differences
Console.WriteLine("## Difference count by (category, locale)");
var grouped = differences
    .GroupBy(kv => (kv.Key.Category, kv.Key.Locale))
    .OrderByDescending(g => g.Count())
    .ThenBy(g => g.Key.Category)
    .ThenBy(g => g.Key.Locale);

Console.WriteLine("category\tlocale\tdiff_count");
foreach (var g in grouped)
{
    Console.WriteLine($"{g.Key.Category}\t{g.Key.Locale}\t{g.Count()}");
}

Console.WriteLine();
Console.WriteLine("## Detailed differences");
Console.WriteLine();

string? lastCategory = null;
foreach (var (key, vals) in differences)
{
    if (key.Category != lastCategory)
    {
        Console.WriteLine();
        Console.WriteLine($"### {key.Category}");
        lastCategory = key.Category;
    }
    Console.Write($"  {key.Locale}/{key.Key}:");
    foreach (var (label, _) in files)
    {
        vals.TryGetValue(label, out var v);
        Console.Write($"  {label}=\"{v ?? "<missing>"}\"");
    }
    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine("## Platform similarity matrix");
Console.WriteLine("How often each pair of platforms produces the same value:");
Console.WriteLine();

var platforms = files.Select(f => f.Label).ToArray();
Console.Write("         ");
foreach (var p in platforms) Console.Write($"{p,8}");
Console.WriteLine();
foreach (var p1 in platforms)
{
    Console.Write($"{p1,8} ");
    foreach (var p2 in platforms)
    {
        var agree = 0;
        var total = 0;
        foreach (var (_, vals) in matrix)
        {
            if (vals.TryGetValue(p1, out var v1) && vals.TryGetValue(p2, out var v2))
            {
                total++;
                if (v1 == v2) agree++;
            }
        }
        var pct = total == 0 ? 0.0 : 100.0 * agree / total;
        Console.Write($" {pct,6:F1}%");
    }
    Console.WriteLine();
}
