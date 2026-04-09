#!/usr/bin/env dotnet run
#:property PublishAot=false
#pragma warning disable IDE0007, CA1869, IL2026, IL2075, IL3050
// Compare probe JSON files across platforms and identify values that differ.
// Supports both "before" (pre-override) and "after" (post-override) baselines.
//
// Usage:
//   dotnet run tools/compare-probes.cs                    # compare all available probes
//   dotnet run tools/compare-probes.cs --after             # compare only "after" probes
//   dotnet run tools/compare-probes.cs --before-vs-after   # compare before vs after per platform

using System.Globalization;
using System.Text;
using System.Text.Json;

Console.OutputEncoding = Encoding.UTF8;

var afterOnly = args.Contains("--after");
var beforeVsAfter = args.Contains("--before-vs-after");

// Locales with calendar.months overrides (DateToOrdinalWords / DateOnlyToOrdinalWords)
string[] calendarOverrideLocales = ["bn", "fa", "he", "ku", "ta", "zu-ZA"];

// Locales with number.formatting.decimalSeparator overrides
string[] decimalOverrideLocales = ["ar", "ku", "fr-CH"];

if (beforeVsAfter)
{
    RunBeforeVsAfterComparison();
    return;
}

var files = afterOnly
    ? new (string Label, string Path)[]
    {
        ("macOS-after",  "tools/probe-macos-after.json"),
        ("Linux-after",  "tools/probe-linux-after.json"),
        ("Win10-after",  "tools/probe-windows-net10-after.json"),
        ("Win48-after",  "tools/probe-windows-net48-after.json"),
    }
    : new (string Label, string Path)[]
    {
        ("macOS",    "tools/probe-macos.json"),
        ("Linux",    "tools/probe-linux.json"),
        ("Win10",    "tools/probe-windows-net10.json"),
        ("Win48",    "tools/probe-windows-net48.json"),
    };

// Filter to only files that exist
var existingFiles = files.Where(f => File.Exists(f.Path)).ToArray();
if (existingFiles.Length < 2)
{
    Console.WriteLine($"Need at least 2 probe files to compare. Found {existingFiles.Length}:");
    foreach (var f in existingFiles)
        Console.WriteLine($"  {f.Label}: {f.Path}");
    Console.WriteLine("Missing:");
    foreach (var f in files.Where(f => !File.Exists(f.Path)))
        Console.WriteLine($"  {f.Label}: {f.Path}");
    return;
}

var data = new Dictionary<string, JsonDocument>();
foreach (var (label, path) in existingFiles)
{
    data[label] = JsonDocument.Parse(File.ReadAllText(path));
}

// Print environment info
Console.WriteLine("## Environments");
foreach (var (label, doc) in data)
{
    var env = doc.RootElement.GetProperty("environment");
    var framework = env.GetProperty("framework").GetString();
    var os = env.GetProperty("os").GetString();
    var rid = env.TryGetProperty("rid", out var ridEl) ? ridEl.GetString() : "n/a";
    Console.WriteLine($"  {label}: {framework} on {os} ({rid})");
}
Console.WriteLine();

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

        // Raw month name arrays (added in fn-5.1; not present in all probe files)
        foreach (var arrayField in new[] { "month_names_raw", "month_genitive_names_raw" })
        {
            if (localeEl.TryGetProperty(arrayField, out var arrayEl) && arrayEl.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                var items = arrayEl.EnumerateArray().ToArray();
                for (int i = 0; i < items.Length; i++)
                {
                    var val = items[i].GetString() ?? "";
                    var key = (locale, arrayField, i.ToString("D2"));
                    if (!matrix.ContainsKey(key)) matrix[key] = new();
                    matrix[key][label] = val;
                }
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
Console.WriteLine($"Agreement rate: {100.0 * (matrix.Count - differences.Length) / matrix.Count:F1}%");
Console.WriteLine();

// Override-relevant analysis
Console.WriteLine("## Override-relevant analysis");
Console.WriteLine();

// Calendar overrides: check month_standalone for overridden locales
Console.WriteLine("### Calendar overrides (month_standalone for overridden locales)");
var calendarDiffs = differences
    .Where(kv => calendarOverrideLocales.Contains(kv.Key.Locale) && kv.Key.Category == "date_month_standalone")
    .ToArray();
Console.WriteLine($"  Overridden locales: {string.Join(", ", calendarOverrideLocales)}");
Console.WriteLine($"  Month-standalone differences across platforms: {calendarDiffs.Length}");
if (calendarDiffs.Length == 0)
    Console.WriteLine("  RESULT: 100% platform agreement for overridden calendar locales");
else
{
    Console.WriteLine("  RESULT: Differences found (expected in raw CultureInfo data; Humanizer overrides at runtime):");
    foreach (var (key, vals) in calendarDiffs)
    {
        Console.Write($"    {key.Locale}/{key.Key}:");
        foreach (var (label, _) in existingFiles)
        {
            vals.TryGetValue(label, out var v);
            Console.Write($"  {label}=\"{v ?? "<missing>"}\"");
        }
        Console.WriteLine();
    }
}
Console.WriteLine();

// Decimal separator overrides
Console.WriteLine("### Number formatting overrides (decimal_separator for overridden locales)");
var decimalDiffs = differences
    .Where(kv => decimalOverrideLocales.Contains(kv.Key.Locale) && kv.Key.Category == "meta" && kv.Key.Key == "number_decimal_separator")
    .ToArray();
Console.WriteLine($"  Overridden locales: {string.Join(", ", decimalOverrideLocales)}");
Console.WriteLine($"  Decimal separator differences across platforms: {decimalDiffs.Length}");
if (decimalDiffs.Length == 0)
    Console.WriteLine("  RESULT: 100% platform agreement for overridden decimal separator locales");
else
{
    Console.WriteLine("  RESULT: Differences found (expected in raw CultureInfo data; Humanizer overrides at runtime):");
    foreach (var (key, vals) in decimalDiffs)
    {
        Console.Write($"    {key.Locale}:");
        foreach (var (label, _) in existingFiles)
        {
            vals.TryGetValue(label, out var v);
            Console.Write($"  {label}=\"{v ?? "<missing>"}\"");
        }
        Console.WriteLine();
    }
}
Console.WriteLine();

// Non-overridden locales analysis
var nonOverriddenLocales = allLocales.Except(calendarOverrideLocales.Union(decimalOverrideLocales)).ToArray();
var nonOverriddenDiffs = differences.Where(kv => nonOverriddenLocales.Contains(kv.Key.Locale)).Count();
var nonOverriddenTotal = matrix.Where(kv => nonOverriddenLocales.Contains(kv.Key.Locale)).Count();
Console.WriteLine("### Non-overridden locales");
Console.WriteLine($"  Locale count: {nonOverriddenLocales.Length}");
Console.WriteLine($"  Data points: {nonOverriddenTotal}");
Console.WriteLine($"  Differences: {nonOverriddenDiffs}");
Console.WriteLine($"  Agreement rate: {100.0 * (nonOverriddenTotal - nonOverriddenDiffs) / nonOverriddenTotal:F1}%");
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
    foreach (var (label, _) in existingFiles)
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

var platforms = existingFiles.Select(f => f.Label).ToArray();
Console.Write("         ");
foreach (var p in platforms) Console.Write($"{p,14}");
Console.WriteLine();
foreach (var p1 in platforms)
{
    Console.Write($"{p1,13} ");
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
        Console.Write($" {pct,12:F1}%");
    }
    Console.WriteLine();
}

static void RunBeforeVsAfterComparison()
{
    var pairs = new (string Platform, string Before, string After)[]
    {
        ("macOS",  "tools/probe-macos.json",  "tools/probe-macos-after.json"),
        ("Linux",  "tools/probe-linux.json",  "tools/probe-linux-after.json"),
        ("Win10",  "tools/probe-windows-net10.json",  "tools/probe-windows-net10-after.json"),
        ("Win48",  "tools/probe-windows-net48.json",  "tools/probe-windows-net48-after.json"),
    };

    Console.WriteLine("## Before vs After comparison (per platform)");
    Console.WriteLine();
    Console.WriteLine("NOTE: Probes capture raw CultureInfo data, not Humanizer output.");
    Console.WriteLine("Before/after probes on the same platform should have identical shared fields");
    Console.WriteLine("because Humanizer overrides operate at the runtime layer, not CultureInfo.");
    Console.WriteLine("Schema extensions (e.g. month_names_raw added in fn-5.1) are expected");
    Console.WriteLine("differences and do not indicate data regression.");
    Console.WriteLine();

    // Shared fields used for before/after comparison (the original probe contract).
    // New fields like month_names_raw / month_genitive_names_raw are schema extensions
    // and should not trigger a false "CHANGED" result.
    string[] sharedScalarFields = ["number_decimal_separator", "short_date_pattern", "long_date_pattern", "short_time_pattern", "long_time_pattern", "am_designator", "pm_designator"];
    string[] sharedDateFields = ["ref", "short", "long", "month_standalone"];
    string[] sharedTimeFields = ["ref", "short", "long"];

    foreach (var (platform, beforePath, afterPath) in pairs)
    {
        if (!File.Exists(beforePath) || !File.Exists(afterPath))
        {
            var missing = !File.Exists(beforePath) ? beforePath : afterPath;
            Console.WriteLine($"  {platform}: SKIPPED (missing {missing})");
            continue;
        }

        var beforeDoc = JsonDocument.Parse(File.ReadAllText(beforePath));
        var afterDoc = JsonDocument.Parse(File.ReadAllText(afterPath));

        // Compare only shared fields per locale (ignoring schema extensions)
        var beforeLocales = beforeDoc.RootElement.GetProperty("locales").EnumerateArray().ToArray();
        var afterLocales = afterDoc.RootElement.GetProperty("locales").EnumerateArray().ToArray();

        var dataChanged = false;
        var schemaExtended = false;

        if (beforeLocales.Length != afterLocales.Length)
        {
            dataChanged = true;
        }
        else
        {
            for (int i = 0; i < beforeLocales.Length; i++)
            {
                var bl = beforeLocales[i];
                var al = afterLocales[i];

                // Check shared scalar fields
                foreach (var f in sharedScalarFields)
                {
                    if (bl.GetProperty(f).GetString() != al.GetProperty(f).GetString())
                        dataChanged = true;
                }

                // Check shared date fields
                var bDates = bl.GetProperty("dates").EnumerateArray().ToArray();
                var aDates = al.GetProperty("dates").EnumerateArray().ToArray();
                if (bDates.Length != aDates.Length) dataChanged = true;
                else
                {
                    for (int j = 0; j < bDates.Length; j++)
                        foreach (var f in sharedDateFields)
                            if (bDates[j].GetProperty(f).GetString() != aDates[j].GetProperty(f).GetString())
                                dataChanged = true;
                }

                // Check shared time fields
                var bTimes = bl.GetProperty("times").EnumerateArray().ToArray();
                var aTimes = al.GetProperty("times").EnumerateArray().ToArray();
                if (bTimes.Length != aTimes.Length) dataChanged = true;
                else
                {
                    for (int j = 0; j < bTimes.Length; j++)
                        foreach (var f in sharedTimeFields)
                            if (bTimes[j].GetProperty(f).GetString() != aTimes[j].GetProperty(f).GetString())
                                dataChanged = true;
                }

                // Detect schema extensions (after has fields before doesn't)
                var bProps = bl.EnumerateObject().Select(p => p.Name).ToHashSet();
                var aProps = al.EnumerateObject().Select(p => p.Name).ToHashSet();
                if (!aProps.SetEquals(bProps))
                    schemaExtended = true;
            }
        }

        if (dataChanged)
            Console.WriteLine($"  {platform}: CHANGED (shared field data differs — investigate)");
        else if (schemaExtended)
            Console.WriteLine($"  {platform}: IDENTICAL shared fields (schema extended with new fields — expected)");
        else
            Console.WriteLine($"  {platform}: IDENTICAL (raw CultureInfo data unchanged)");
    }
    Console.WriteLine();
}
