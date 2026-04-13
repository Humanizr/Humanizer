using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using Xunit;

namespace Humanizer.SourceGenerators.Tests;

/// <summary>
/// Shared helper that reads YAML fixture files from disk and wraps them as
/// <see cref="AdditionalText"/> for the source generator driver. Task .9 reuses
/// this helper unmodified for its own fixture directory.
/// </summary>
internal static class FixtureLoader
{
    static readonly string RepositoryRoot = FindRepositoryRoot();

    /// <summary>
    /// Returns the absolute path to a fixture directory under the source generator test project.
    /// </summary>
    internal static string GetFixtureDirectory(params string[] relativeSegments) =>
        Path.Combine(
            new[] { RepositoryRoot, "tests", "Humanizer.SourceGenerators.Tests", "Fixtures" }
                .Concat(relativeSegments)
                .ToArray());

    /// <summary>
    /// Enumerates all <c>.yml</c> files in the given fixture directory and returns theory data
    /// where each entry is (fixture file name without extension, fixture file absolute path).
    /// </summary>
    internal static TheoryData<string, string> EnumerateFixtures(params string[] relativeSegments)
    {
        var directory = GetFixtureDirectory(relativeSegments);
        var data = new TheoryData<string, string>();

        foreach (var file in Directory.GetFiles(directory, "*.yml", SearchOption.TopDirectoryOnly)
                     .OrderBy(static path => path, StringComparer.OrdinalIgnoreCase))
        {
            data.Add(Path.GetFileNameWithoutExtension(file), file);
        }

        return data;
    }

    /// <summary>
    /// Reads a YAML fixture file and wraps it as an <see cref="AdditionalText"/> using the
    /// conventional locale path that the source generator expects.
    /// </summary>
    internal static AdditionalText LoadAsAdditionalText(string filePath, string? localeCodeOverride = null)
    {
        var localeCode = localeCodeOverride ?? Path.GetFileNameWithoutExtension(filePath);
        var text = File.ReadAllText(filePath);
        return new InMemoryAdditionalText(
            $@"E:\Dev\Humanizer\src\Humanizer\Locales\{localeCode}.yml",
            text);
    }

    /// <summary>
    /// Reads a YAML fixture from a string and wraps it as an <see cref="AdditionalText"/>.
    /// </summary>
    internal static AdditionalText FromString(string localeCode, string yamlContent) =>
        new InMemoryAdditionalText(
            $@"E:\Dev\Humanizer\src\Humanizer\Locales\{localeCode}.yml",
            yamlContent);

    static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "src", "Humanizer", "Humanizer.csproj")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new Xunit.Sdk.XunitException("Could not locate the repository root.");
    }

    sealed class InMemoryAdditionalText(string path, string text) : AdditionalText
    {
        public override string Path => path;

        public override SourceText GetText(CancellationToken cancellationToken = default) =>
            SourceText.From(text, Encoding.UTF8);
    }
}
