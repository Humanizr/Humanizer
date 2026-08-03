using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Nodes;

using Humanizer.SourceGenerators;

if (args.Length is < 2 or > 3 || args.Length == 3 && args[2] != "--check")
{
    Console.Error.WriteLine("Usage: Humanizer.Docs.LanguageCoverage <input.json> <output.json> [--check]");
    return 2;
}

var input = JsonSerializer.Deserialize<GeneratorInput>(
    File.ReadAllText(args[0]),
    SerializerOptions.Default)
    ?? throw new InvalidOperationException("Language coverage input is empty.");
var output = CreateOutput(input);
var expected = output.ToJsonString(SerializerOptions.Indented).Replace("\r\n", "\n") + "\n";

if (args.Length == 3)
{
    if (!File.Exists(args[1]) || File.ReadAllText(args[1]) != expected)
    {
        throw new InvalidOperationException(
            $"Generated language coverage is stale. Run tools/docs/generate-language-coverage.ps1 -Version {input.VersionKey}.");
    }

    Console.WriteLine($"Language coverage is current for {input.GeneratedFor}.");
    return 0;
}

Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(args[1]))!);
File.WriteAllText(args[1], expected, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
Console.WriteLine($"Generated language coverage for {input.GeneratedFor}.");
return 0;

static JsonObject CreateOutput(GeneratorInput input)
{
    var root = new JsonObject
    {
        ["schemaVersion"] = 2,
        ["coverageKind"] = input.CoverageKind,
        ["version"] = input.VersionKey,
        ["generatedFor"] = input.GeneratedFor,
        ["source"] = input.Source,
        ["package"] = JsonSerializer.SerializeToNode(input.Package, SerializerOptions.Default)
    };

    switch (input.CoverageKind)
    {
        case "yaml-capabilities":
            if (string.IsNullOrWhiteSpace(input.LocalesPath))
            {
                throw new InvalidOperationException("YAML capability coverage requires localesPath.");
            }

            var localeFiles = Directory
                .GetFiles(input.LocalesPath, "*.yml", SearchOption.TopDirectoryOnly)
                .OrderBy(static path => path, StringComparer.Ordinal)
                .Select(static path => (HumanizerSourceGenerator.LocaleDefinitionFile?)new HumanizerSourceGenerator.LocaleDefinitionFile(
                    Path.GetFileNameWithoutExtension(path),
                    File.ReadAllText(path)));
            var catalog = HumanizerSourceGenerator.LocaleCatalogInput.Create(ImmutableArray.CreateRange(localeFiles));
            var coverage = HumanizerSourceGenerator.LocaleCoverageInput.Create(catalog);
            root["capabilities"] = JsonSerializer.SerializeToNode(coverage.Capabilities, SerializerOptions.Default);
            var localeRows = new JsonArray();
            foreach (var locale in coverage.Locales)
            {
                var capabilityValues = new JsonObject();
                foreach (var capability in coverage.Capabilities)
                {
                    capabilityValues[capability.Key] = JsonSerializer.SerializeToNode(
                        locale.Capabilities[capability.Key],
                        SerializerOptions.Default);
                }

                localeRows.Add(new JsonObject
                {
                    ["locale"] = locale.Locale,
                    ["parent"] = locale.Parent,
                    ["capabilities"] = capabilityValues
                });
            }
            root["locales"] = localeRows;

            var localeProfileOwners = coverage.Locales
                .Select(static locale => locale.Locale)
                .ToHashSet(StringComparer.Ordinal);
            var acceptedCultureRows = catalog.AcceptedCultures
                .OrderBy(static culture => culture.Name, StringComparer.Ordinal)
                .ToArray();
            if (acceptedCultureRows
                    .Select(static culture => culture.Name)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count() != acceptedCultureRows.Length)
            {
                throw new InvalidOperationException(
                    "Accepted culture names must be unique ignoring case.");
            }

            var unknownOwners = acceptedCultureRows
                .Select(static culture => culture.LocaleProfileOwner)
                .Where(owner => !localeProfileOwners.Contains(owner))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(static owner => owner, StringComparer.Ordinal)
                .ToArray();
            if (unknownOwners.Length != 0)
            {
                throw new InvalidOperationException(
                    $"Accepted cultures reference unknown locale profile owners: {string.Join(", ", unknownOwners)}.");
            }

            var unrepresentedOwners = localeProfileOwners
                .Where(owner => !acceptedCultureRows.Any(culture =>
                    string.Equals(culture.LocaleProfileOwner, owner, StringComparison.Ordinal)))
                .OrderBy(static owner => owner, StringComparer.Ordinal)
                .ToArray();
            if (unrepresentedOwners.Length != 0)
            {
                throw new InvalidOperationException(
                    $"Locale profile owners are absent from the accepted-culture inventory: {string.Join(", ", unrepresentedOwners)}.");
            }

            var acceptedCultures = new JsonArray();
            foreach (var acceptedCulture in acceptedCultureRows)
            {
                acceptedCultures.Add(new JsonObject
                {
                    ["name"] = acceptedCulture.Name,
                    ["localeProfileOwner"] = acceptedCulture.LocaleProfileOwner
                });
            }

            root["acceptedCultureInventory"] = new JsonObject
            {
                ["source"] = "the generated accepted-culture catalog used by Humanizer's built-in registries",
                ["acceptedNameCount"] = acceptedCultureRows.Length,
                ["localeProfileOwnerCount"] = localeProfileOwners.Count,
                ["mappings"] = acceptedCultures
            };
            break;

        case "published-package-cultures":
            if (input.Cultures is null || input.Cultures.Length == 0)
            {
                throw new InvalidOperationException("Published package coverage requires cultures.");
            }
            if (input.Cultures.Distinct(StringComparer.OrdinalIgnoreCase).Count() != input.Cultures.Length)
            {
                throw new InvalidOperationException("Published package cultures must be unique.");
            }

            root["cultures"] = JsonSerializer.SerializeToNode(
                input.Cultures.OrderBy(static culture => culture, StringComparer.Ordinal),
                SerializerOptions.Default);
            break;

        default:
            throw new InvalidOperationException($"Unsupported coverage kind '{input.CoverageKind}'.");
    }

    return root;
}

sealed class GeneratorInput
{
    public required string CoverageKind { get; init; }
    public required string VersionKey { get; init; }
    public required string GeneratedFor { get; init; }
    public required string Source { get; init; }
    public required PackageEvidence Package { get; init; }
    public string? LocalesPath { get; init; }
    public string[]? Cultures { get; init; }
}

sealed class PackageEvidence
{
    public required string Id { get; init; }
    public string? Version { get; init; }
    public string? ApiPackage { get; init; }
    public required string[] TargetFrameworks { get; init; }
    public required string LocaleData { get; init; }
    public required ApiAvailability ApiAvailability { get; init; }
}

sealed class ApiAvailability
{
    public required string[] DateOnlyOrdinals { get; init; }
    public required string[] ClockNotation { get; init; }
}

static class SerializerOptions
{
    public static JsonSerializerOptions Default { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static JsonSerializerOptions Indented { get; } = new(Default)
    {
        WriteIndented = true
    };
}