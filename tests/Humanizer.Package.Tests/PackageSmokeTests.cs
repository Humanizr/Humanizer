using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

using Xunit;
using Xunit.Sdk;

namespace Humanizer.Package.Tests;

public class PackageSmokeTests
{
    [Fact]
    public async Task Net48_consumer_loads_the_namespace_migration_analyzer()
    {
        var packageContext = await PackageTestContext.GetAsync();
        var consumerProject = packageContext.CreateCustomConsoleConsumer(
            "net48-analyzer",
            "net48",
            PackageScenario.MetaPackage,
            """
            using Humanizer.Bytes;

            namespace Humanizer.Bytes
            {
                public class StubFormatter
                {
                }
            }

            class Program
            {
                static void Main()
                {
                }
            }
            """);

        var restoreResult = await ProcessRunner.RunAsync(
            "dotnet",
            ["restore", consumerProject.ProjectPath, "--configfile", packageContext.NuGetConfigPath, "--packages", packageContext.GlobalPackagesFolder],
            consumerProject.WorkingDirectory);
        AssertSucceeded(restoreResult, "restore", "net48", HostScenario.Console, PackageScenario.MetaPackage);

        var buildResult = await ProcessRunner.RunAsync(
            "dotnet",
            ["build", consumerProject.ProjectPath, "--no-restore"],
            consumerProject.WorkingDirectory);

        Assert.NotEqual(0, buildResult.ExitCode);
        Assert.Contains("HUMANIZER001", buildResult.StandardOutput + buildResult.StandardError, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Net8_blazor_consumer_loads_the_namespace_migration_analyzer()
    {
        var packageContext = await PackageTestContext.GetAsync();
        var consumerProject = await packageContext.CreateConsumerAsync("net8.0", HostScenario.Blazor, PackageScenario.MetaPackage, "analyzer");
        File.WriteAllText(
            Path.Combine(consumerProject.WorkingDirectory, "AnalyzerProbe.cs"),
            """
            using Humanizer.Bytes;

            namespace Humanizer.Bytes
            {
                public class StubFormatter
                {
                }
            }
            """);

        var restoreResult = await ProcessRunner.RunAsync(
            "dotnet",
            ["restore", consumerProject.ProjectPath, "--configfile", packageContext.NuGetConfigPath, "--packages", packageContext.GlobalPackagesFolder],
            consumerProject.WorkingDirectory);
        AssertSucceeded(restoreResult, "restore", "net8.0", HostScenario.Blazor, PackageScenario.MetaPackage);

        var buildResult = await ProcessRunner.RunAsync(
            "dotnet",
            ["build", consumerProject.ProjectPath, "--no-restore"],
            consumerProject.WorkingDirectory);

        Assert.NotEqual(0, buildResult.ExitCode);
        Assert.Contains("HUMANIZER001", buildResult.StandardOutput + buildResult.StandardError, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(GetCases))]
    public async Task Packaged_consumer_can_restore_build_and_run_with_packaged_humanizer(
        string targetFramework,
        HostScenario hostScenario,
        PackageScenario packageScenario)
    {
        var packageContext = await PackageTestContext.GetAsync();
        var consumerProject = await packageContext.CreateConsumerAsync(targetFramework, hostScenario, packageScenario);

        var restoreResult = await ProcessRunner.RunAsync(
            "dotnet",
            ["restore", consumerProject.ProjectPath, "--configfile", packageContext.NuGetConfigPath, "--packages", packageContext.GlobalPackagesFolder],
            consumerProject.WorkingDirectory);
        AssertSucceeded(restoreResult, "restore", targetFramework, hostScenario, packageScenario);

        var runResult = targetFramework == "net48"
            ? await BuildAndRunNetFrameworkConsumerAsync(consumerProject, targetFramework)
            : await ProcessRunner.RunAsync(
                "dotnet",
                hostScenario.GetRunArguments(consumerProject.ProjectPath),
                consumerProject.WorkingDirectory);

        AssertSucceeded(runResult, "run", targetFramework, hostScenario, packageScenario);
        Assert.Equal("deux", runResult.StandardOutput.Trim());
    }

    public static TheoryData<string, HostScenario, PackageScenario> GetCases()
    {
        var theoryData = new TheoryData<string, HostScenario, PackageScenario>();

        foreach (var targetFramework in TestMatrix.TargetFrameworks)
        {
            foreach (var hostScenario in TestMatrix.HostScenarios)
            {
                if (!hostScenario.SupportsTargetFramework(targetFramework))
                    continue;

                foreach (var packageScenario in TestMatrix.PackageScenarios)
                    theoryData.Add(targetFramework, hostScenario, packageScenario);
            }
        }

        return theoryData;
    }

    static async Task<ProcessResult> BuildAndRunNetFrameworkConsumerAsync(ConsumerProject consumerProject, string targetFramework)
    {
        var packageContext = await PackageTestContext.GetAsync();
        var buildResult = await ProcessRunner.RunAsync(
            "dotnet",
            ["build", consumerProject.ProjectPath, "--no-restore"],
            consumerProject.WorkingDirectory);

        if (buildResult.ExitCode != 0)
            return buildResult;

        var executablePath = Path.Combine(consumerProject.WorkingDirectory, "bin", "Debug", targetFramework, "Consumer.exe");
        return await ProcessRunner.RunAsync(executablePath, [], consumerProject.WorkingDirectory);
    }

    static void AssertSucceeded(
        ProcessResult result,
        string verb,
        string targetFramework,
        HostScenario hostScenario,
        PackageScenario packageScenario)
    {
        if (result.ExitCode == 0)
            return;

        throw new XunitException(
            string.Create(
                CultureInfo.InvariantCulture,
                $"{hostScenario.ToDisplayName()} / {packageScenario.ToDisplayName()} failed during {verb} for {targetFramework}.{Environment.NewLine}{result.FormatForFailure()}"));
    }
}

sealed class PackageTestContext
{
    static readonly SemaphoreSlim SyncRoot = new(1, 1);
    static PackageTestContext? cachedInstance;

    PackageTestContext(string workingDirectory, string packagesDirectory, string packageVersion, string nuGetConfigPath, string globalPackagesFolder)
    {
        WorkingDirectory = workingDirectory;
        PackagesDirectory = packagesDirectory;
        PackageVersion = packageVersion;
        NuGetConfigPath = nuGetConfigPath;
        GlobalPackagesFolder = globalPackagesFolder;
    }

    public string WorkingDirectory { get; }
    public string PackagesDirectory { get; }
    public string PackageVersion { get; }
    public string NuGetConfigPath { get; }
    public string GlobalPackagesFolder { get; }

    public static async Task<PackageTestContext> GetAsync()
    {
        if (cachedInstance is not null)
            return cachedInstance;

        await SyncRoot.WaitAsync();
        try
        {
            if (cachedInstance is not null)
                return cachedInstance;

            cachedInstance = await CreateAsync();
            return cachedInstance;
        }
        finally
        {
            SyncRoot.Release();
        }
    }

    public async Task<ConsumerProject> CreateConsumerAsync(
        string targetFramework,
        HostScenario hostScenario,
        PackageScenario packageScenario,
        string? folderNameSuffix = null)
    {
        var safeTargetFramework = targetFramework.Replace('.', '_');
        var safeHostName = hostScenario.ToFolderName();
        var safeScenarioName = packageScenario.ToFolderName();
        var suffix = string.IsNullOrWhiteSpace(folderNameSuffix) ? string.Empty : "-" + folderNameSuffix;
        var projectDirectory = Path.Combine(WorkingDirectory, "consumers", safeHostName, safeScenarioName, safeTargetFramework + suffix);

        if (hostScenario == HostScenario.Console)
        {
            Directory.CreateDirectory(projectDirectory);
            CreateConsoleProject(projectDirectory, targetFramework, packageScenario);
        }
        else
            await CreateTemplatedProjectAsync(projectDirectory, targetFramework, hostScenario, packageScenario);

        return new ConsumerProject(projectDirectory, Path.Combine(projectDirectory, "Consumer.csproj"));
    }

    public ConsumerProject CreateCustomConsoleConsumer(string folderName, string targetFramework, PackageScenario packageScenario, string programSource)
    {
        var projectDirectory = Path.Combine(WorkingDirectory, folderName);
        Directory.CreateDirectory(projectDirectory);
        CreateConsoleProject(projectDirectory, targetFramework, packageScenario, programSource);
        return new ConsumerProject(projectDirectory, Path.Combine(projectDirectory, "Consumer.csproj"));
    }

    static async Task<PackageTestContext> CreateAsync()
    {
        var settings = PackageSmokeSettings.Load();
        var workingDirectory = Path.Combine(Path.GetTempPath(), "Humanizer.Package.Tests", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        Directory.CreateDirectory(workingDirectory);

        if (!string.IsNullOrWhiteSpace(settings.PackagesDirectory) &&
            !string.IsNullOrWhiteSpace(settings.PackageVersion))
        {
            var (nuGetConfigPath, configuredGlobalPackagesFolder) = WriteNuGetConfig(workingDirectory, settings.PackagesDirectory);
            return new PackageTestContext(workingDirectory, settings.PackagesDirectory, settings.PackageVersion, nuGetConfigPath, configuredGlobalPackagesFolder);
        }

        var repositoryRoot = FindRepositoryRoot();
        var packageOutputPath = Path.Combine(workingDirectory, "packages");
        Directory.CreateDirectory(packageOutputPath);

        var buildResult = await ProcessRunner.RunAsync(
            "dotnet",
            [
                "build",
                "Humanizer/Humanizer.csproj",
                "-c",
                "Release",
                "/t:PackNuSpecs",
                $"/p:PackageOutputPath={packageOutputPath}"
            ],
            Path.Combine(repositoryRoot, "src"));

        if (buildResult.ExitCode != 0)
            throw new XunitException($"Failed to build test packages.{Environment.NewLine}{buildResult.FormatForFailure()}");

        var builtPackageVersion = GetMetapackageVersion(packageOutputPath);
        var (configPath, globalPackagesFolder) = WriteNuGetConfig(workingDirectory, packageOutputPath);

        return new PackageTestContext(workingDirectory, packageOutputPath, builtPackageVersion, configPath, globalPackagesFolder);
    }

    internal static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "AGENTS.md")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new XunitException("Could not locate the repository root.");
    }

    static string GetMetapackageVersion(string packagesDirectory)
    {
        var regex = new Regex(@"^Humanizer\.(?!Core(?:\.|$))(?<version>.+)\.nupkg$", RegexOptions.CultureInvariant);

        foreach (var packagePath in Directory.EnumerateFiles(packagesDirectory, "Humanizer.*.nupkg"))
        {
            var fileName = Path.GetFileName(packagePath);
            var match = regex.Match(fileName);
            if (match.Success)
                return match.Groups["version"].Value;
        }

        throw new XunitException("Could not determine the packaged Humanizer version from the available package files.");
    }

    void CreateConsoleProject(string projectDirectory, string targetFramework, PackageScenario packageScenario) =>
        CreateConsoleProject(
            projectDirectory,
            targetFramework,
            packageScenario,
            """
            using System.Globalization;
            using Humanizer;

            Console.WriteLine(2.ToWords(new CultureInfo("fr")));
            """);

    void CreateConsoleProject(string projectDirectory, string targetFramework, PackageScenario packageScenario, string programSource)
    {
        var projectPath = Path.Combine(projectDirectory, "Consumer.csproj");
        File.WriteAllText(
            projectPath,
            $$"""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <TargetFramework>{{targetFramework}}</TargetFramework>
                <ImplicitUsings>enable</ImplicitUsings>
                <LangVersion>latest</LangVersion>
                <Nullable>enable</Nullable>
              </PropertyGroup>
              {{packageScenario.GetPackageReferences(PackageVersion)}}
            </Project>
            """);

        File.WriteAllText(
            Path.Combine(projectDirectory, "Program.cs"),
            programSource);
    }

    async Task CreateTemplatedProjectAsync(
        string projectDirectory,
        string targetFramework,
        HostScenario hostScenario,
        PackageScenario packageScenario)
    {
        var createResult = await ProcessRunner.RunAsync(
            "dotnet",
            hostScenario.GetTemplateArguments(targetFramework, projectDirectory),
            WorkingDirectory);

        if (createResult.ExitCode != 0)
            throw new XunitException($"Failed to create {hostScenario.ToDisplayName()} smoke project.{Environment.NewLine}{createResult.FormatForFailure()}");

        var projectPath = Path.Combine(projectDirectory, "Consumer.csproj");
        var projectContents = File.ReadAllText(projectPath);
        projectContents = projectContents.Replace(
            "</Project>",
            $"{Environment.NewLine}{packageScenario.GetPackageReferences(PackageVersion)}{Environment.NewLine}</Project>",
            StringComparison.Ordinal);
        File.WriteAllText(projectPath, projectContents);

        var programPath = Path.Combine(projectDirectory, "Program.cs");
        var programContents = File.ReadAllText(programPath);
        programContents = "using System.Globalization;" + Environment.NewLine +
            "using Humanizer;" + Environment.NewLine + Environment.NewLine +
            programContents;

        var smokeExitGuard =
            """
            if (args.Contains("--humanizer-smoke-exit", StringComparer.Ordinal))
            {
                Console.WriteLine(2.ToWords(new CultureInfo("fr")));
                return;
            }

            """;

        var appRunStatement = "app.Run();";
        var appRunIndex = programContents.IndexOf(appRunStatement, StringComparison.Ordinal);
        if (appRunIndex < 0)
            throw new XunitException($"Could not locate '{appRunStatement}' in {programPath}.");

        programContents = programContents.Insert(appRunIndex, smokeExitGuard);
        File.WriteAllText(programPath, programContents);
    }

    static (string NuGetConfigPath, string GlobalPackagesFolder) WriteNuGetConfig(string workingDirectory, string packagesDirectory)
    {
        var nuGetConfigPath = Path.Combine(workingDirectory, "NuGet.config");
        var globalPackagesFolder = Path.Combine(workingDirectory, ".nuget", "packages");
        File.WriteAllText(
            nuGetConfigPath,
            $$"""
            <?xml version="1.0" encoding="utf-8"?>
            <configuration>
              <config>
                <add key="globalPackagesFolder" value="{{globalPackagesFolder}}" />
              </config>
              <packageSources>
                <clear />
                <add key="local" value="{{packagesDirectory}}" />
                <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
              </packageSources>
            </configuration>
            """);

        return (nuGetConfigPath, globalPackagesFolder);
    }
}

readonly record struct ConsumerProject(string WorkingDirectory, string ProjectPath);

public enum HostScenario
{
    Console,
    WebApi,
    Blazor
}

public enum PackageScenario
{
    MetaPackage,
    CoreAndFrenchLanguage
}

static class HostScenarioExtensions
{
    public static IReadOnlyList<string> GetRunArguments(this HostScenario hostScenario, string projectPath) =>
        hostScenario == HostScenario.Console
            ? ["run", "--project", projectPath, "--no-restore"]
            : ["run", "--project", projectPath, "--no-restore", "--no-launch-profile", "--", "--humanizer-smoke-exit"];

    public static IReadOnlyList<string> GetTemplateArguments(this HostScenario hostScenario, string targetFramework, string projectDirectory) =>
        hostScenario switch
        {
            HostScenario.WebApi => ["new", "webapi", "-n", "Consumer", "-o", projectDirectory, "--framework", targetFramework, "--no-restore", "--force"],
            HostScenario.Blazor => ["new", "blazor", "-n", "Consumer", "-o", projectDirectory, "--framework", targetFramework, "--no-restore", "--force"],
            _ => throw new ArgumentOutOfRangeException(nameof(hostScenario), hostScenario, null)
        };

    public static bool SupportsTargetFramework(this HostScenario hostScenario, string targetFramework) =>
        hostScenario switch
        {
            HostScenario.Console => true,
            HostScenario.WebApi => targetFramework != "net48",
            HostScenario.Blazor => targetFramework != "net48",
            _ => false
        };

    public static string ToDisplayName(this HostScenario hostScenario) =>
        hostScenario switch
        {
            HostScenario.Console => "Console",
            HostScenario.WebApi => "Web API",
            HostScenario.Blazor => "Blazor",
            _ => throw new ArgumentOutOfRangeException(nameof(hostScenario), hostScenario, null)
        };

    public static string ToFolderName(this HostScenario hostScenario) =>
        hostScenario switch
        {
            HostScenario.Console => "console",
            HostScenario.WebApi => "webapi",
            HostScenario.Blazor => "blazor",
            _ => throw new ArgumentOutOfRangeException(nameof(hostScenario), hostScenario, null)
        };
}

static class PackageScenarioExtensions
{
    public static string GetPackageReferences(this PackageScenario scenario, string packageVersion) =>
        scenario switch
        {
            PackageScenario.MetaPackage =>
                $$"""
                  <ItemGroup>
                    <PackageReference Include="Humanizer" Version="{{packageVersion}}" />
                  </ItemGroup>
                """,
            PackageScenario.CoreAndFrenchLanguage =>
                $$"""
                  <ItemGroup>
                    <PackageReference Include="Humanizer.Core" Version="{{packageVersion}}" />
                    <PackageReference Include="Humanizer.Core.fr" Version="{{packageVersion}}" />
                  </ItemGroup>
                """,
            _ => throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null)
        };

    public static string ToDisplayName(this PackageScenario scenario) =>
        scenario switch
        {
            PackageScenario.MetaPackage => "Metapackage",
            PackageScenario.CoreAndFrenchLanguage => "Core + language",
            _ => throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null)
        };

    public static string ToFolderName(this PackageScenario scenario) =>
        scenario switch
        {
            PackageScenario.MetaPackage => "metapackage",
            PackageScenario.CoreAndFrenchLanguage => "core-language",
            _ => throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null)
        };
}

static class TestMatrix
{
    static readonly PackageSmokeSettings settings = PackageSmokeSettings.Load();

    public static IReadOnlyList<HostScenario> HostScenarios { get; } = settings.HostScenarios;
    public static IReadOnlyList<string> TargetFrameworks { get; } = settings.TargetFrameworks;
    public static IReadOnlyList<PackageScenario> PackageScenarios { get; } = settings.PackageScenarios;
}

sealed class PackageSmokeSettings
{
    static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    PackageSmokeSettings(string? packagesDirectory, string? packageVersion, IReadOnlyList<string> targetFrameworks, IReadOnlyList<HostScenario> hostScenarios, IReadOnlyList<PackageScenario> packageScenarios)
    {
        PackagesDirectory = packagesDirectory;
        PackageVersion = packageVersion;
        TargetFrameworks = targetFrameworks;
        HostScenarios = hostScenarios;
        PackageScenarios = packageScenarios;
    }

    public string? PackagesDirectory { get; }
    public string? PackageVersion { get; }
    public IReadOnlyList<string> TargetFrameworks { get; }
    public IReadOnlyList<HostScenario> HostScenarios { get; }
    public IReadOnlyList<PackageScenario> PackageScenarios { get; }

    public static PackageSmokeSettings Load()
    {
        var settingsFilePath = GetSettingsFilePath();
        if (!File.Exists(settingsFilePath))
            return new PackageSmokeSettings(null, null, ["net8.0", "net10.0"], [HostScenario.Console], [PackageScenario.MetaPackage, PackageScenario.CoreAndFrenchLanguage]);

        var json = File.ReadAllText(settingsFilePath);
        var dto = JsonSerializer.Deserialize<PackageSmokeSettingsDto>(json, SerializerOptions) ??
            throw new XunitException($"Could not deserialize package smoke settings from '{settingsFilePath}'.");

        return new PackageSmokeSettings(
            dto.PackagesDirectory,
            dto.PackageVersion,
            ParseTargetFrameworks(dto.TargetFrameworks),
            ParseHosts(dto.Hosts),
            ParseScenarios(dto.Scenarios));
    }

    public static string GetSettingsFilePath() =>
        Path.Combine(PackageTestContext.FindRepositoryRoot(), "artifacts", "package-smoke-settings.json");

    static string[] ParseTargetFrameworks(string[]? targetFrameworks) =>
        targetFrameworks is { Length: > 0 }
            ? targetFrameworks.Distinct(StringComparer.OrdinalIgnoreCase).ToArray()
            : ["net8.0", "net10.0"];

    static HostScenario[] ParseHosts(string[]? hosts) =>
        hosts is { Length: > 0 }
            ? hosts.Select(ParseHostScenario).Distinct().ToArray()
            : [HostScenario.Console];

    static PackageScenario[] ParseScenarios(string[]? scenarios) =>
        scenarios is { Length: > 0 }
            ? scenarios.Select(ParseScenario).Distinct().ToArray()
            : [PackageScenario.MetaPackage, PackageScenario.CoreAndFrenchLanguage];

    static PackageScenario ParseScenario(string value) =>
        value.ToLowerInvariant() switch
        {
            "meta" or "metapackage" => PackageScenario.MetaPackage,
            "core-lang" or "core+lang" or "core-language" => PackageScenario.CoreAndFrenchLanguage,
            _ => throw new XunitException($"Unknown package smoke test scenario '{value}'.")
        };

    static HostScenario ParseHostScenario(string value) =>
        value.ToLowerInvariant() switch
        {
            "console" => HostScenario.Console,
            "webapi" or "web" or "aspnet" => HostScenario.WebApi,
            "blazor" => HostScenario.Blazor,
            _ => throw new XunitException($"Unknown package smoke test host scenario '{value}'.")
        };

    sealed class PackageSmokeSettingsDto
    {
        public string[]? Hosts { get; init; }
        public string? PackageVersion { get; init; }
        public string? PackagesDirectory { get; init; }
        public string[]? Scenarios { get; init; }
        public string[]? TargetFrameworks { get; init; }
    }
}

sealed record ProcessResult(string FileName, string Arguments, int ExitCode, string StandardOutput, string StandardError)
{
    public string FormatForFailure()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Command: {FileName} {Arguments}");
        builder.AppendLine($"ExitCode: {ExitCode}");

        if (!string.IsNullOrWhiteSpace(StandardOutput))
        {
            builder.AppendLine("StandardOutput:");
            builder.AppendLine(StandardOutput.Trim());
        }

        if (!string.IsNullOrWhiteSpace(StandardError))
        {
            builder.AppendLine("StandardError:");
            builder.AppendLine(StandardError.Trim());
        }

        return builder.ToString().TrimEnd();
    }
}

static class ProcessRunner
{
    public static async Task<ProcessResult> RunAsync(
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var argument in arguments)
            process.StartInfo.ArgumentList.Add(argument);

        process.Start();

        var standardOutputTask = process.StandardOutput.ReadToEndAsync();
        var standardErrorTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        return new ProcessResult(
            fileName,
            string.Join(" ", arguments),
            process.ExitCode,
            await standardOutputTask,
            await standardErrorTask);
    }
}
