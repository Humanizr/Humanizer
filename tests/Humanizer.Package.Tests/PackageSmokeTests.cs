using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Xunit;
using Xunit.Sdk;

namespace Humanizer.Package.Tests;

public class PackageSmokeTests
{
    [Theory]
    [MemberData(nameof(GetCases))]
    public async Task Console_consumer_can_restore_build_and_run_with_packaged_humanizer(string targetFramework, PackageScenario scenario)
    {
        var packageContext = await PackageTestContext.GetAsync();
        var consumerProject = packageContext.CreateConsoleConsumer(targetFramework, scenario);

        var restoreResult = await ProcessRunner.RunAsync(
            "dotnet",
            ["restore", consumerProject.ProjectPath, "--configfile", packageContext.NuGetConfigPath],
            consumerProject.WorkingDirectory,
            packageContext.ProcessEnvironmentVariables);
        AssertSucceeded(restoreResult, "restore", targetFramework, scenario);

        var runResult = targetFramework == "net48"
            ? await BuildAndRunNetFrameworkConsumerAsync(consumerProject, targetFramework)
            : await ProcessRunner.RunAsync(
                "dotnet",
                ["run", "--project", consumerProject.ProjectPath, "--no-restore"],
                consumerProject.WorkingDirectory,
                packageContext.ProcessEnvironmentVariables);

        AssertSucceeded(runResult, "run", targetFramework, scenario);
        Assert.Equal("deux", runResult.StandardOutput.Trim());
    }

    public static TheoryData<string, PackageScenario> GetCases()
    {
        var theoryData = new TheoryData<string, PackageScenario>();

        foreach (var targetFramework in TestMatrix.TargetFrameworks)
        {
            foreach (var scenario in TestMatrix.Scenarios)
                theoryData.Add(targetFramework, scenario);
        }

        return theoryData;
    }

    static async Task<ProcessResult> BuildAndRunNetFrameworkConsumerAsync(ConsumerProject consumerProject, string targetFramework)
    {
        var packageContext = await PackageTestContext.GetAsync();
        var buildResult = await ProcessRunner.RunAsync(
            "dotnet",
            ["build", consumerProject.ProjectPath, "--no-restore"],
            consumerProject.WorkingDirectory,
            packageContext.ProcessEnvironmentVariables);

        if (buildResult.ExitCode != 0)
            return buildResult;

        var executablePath = Path.Combine(consumerProject.WorkingDirectory, "bin", "Debug", targetFramework, "Consumer.exe");
        return await ProcessRunner.RunAsync(executablePath, [], consumerProject.WorkingDirectory, packageContext.ProcessEnvironmentVariables);
    }

    static void AssertSucceeded(ProcessResult result, string verb, string targetFramework, PackageScenario scenario)
    {
        if (result.ExitCode == 0)
            return;

        throw new XunitException(
            string.Create(
                CultureInfo.InvariantCulture,
                $"{scenario.ToDisplayName()} scenario failed during {verb} for {targetFramework}.{Environment.NewLine}{result.FormatForFailure()}"));
    }
}

sealed class PackageTestContext
{
    const string PackagesDirectoryEnvironmentVariable = "HUMANIZER_PACKAGES_DIR";
    const string PackageVersionEnvironmentVariable = "HUMANIZER_PACKAGE_VERSION";
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
    public IReadOnlyDictionary<string, string> ProcessEnvironmentVariables =>
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["NUGET_PACKAGES"] = GlobalPackagesFolder
        };

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

    public ConsumerProject CreateConsoleConsumer(string targetFramework, PackageScenario scenario)
    {
        var safeTargetFramework = targetFramework.Replace('.', '_');
        var safeScenarioName = scenario.ToFolderName();
        var projectDirectory = Path.Combine(WorkingDirectory, "consumers", safeScenarioName, safeTargetFramework);
        Directory.CreateDirectory(projectDirectory);

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
              {{scenario.GetPackageReferences(PackageVersion)}}
            </Project>
            """);

        File.WriteAllText(
            Path.Combine(projectDirectory, "Program.cs"),
            """
            using System.Globalization;
            using Humanizer;

            Console.WriteLine(2.ToWords(new CultureInfo("fr")));
            """);

        return new ConsumerProject(projectDirectory, projectPath);
    }

    static async Task<PackageTestContext> CreateAsync()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), "Humanizer.Package.Tests", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        Directory.CreateDirectory(workingDirectory);

        var packagesDirectory = Environment.GetEnvironmentVariable(PackagesDirectoryEnvironmentVariable);
        var packageVersion = Environment.GetEnvironmentVariable(PackageVersionEnvironmentVariable);

        if (!string.IsNullOrWhiteSpace(packagesDirectory) &&
            !string.IsNullOrWhiteSpace(packageVersion))
        {
            var (nuGetConfigPath, configuredGlobalPackagesFolder) = WriteNuGetConfig(workingDirectory, packagesDirectory);
            return new PackageTestContext(workingDirectory, packagesDirectory, packageVersion, nuGetConfigPath, configuredGlobalPackagesFolder);
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

        var builtPackageVersion = FindPackageVersion(packageOutputPath);
        var (configPath, globalPackagesFolder) = WriteNuGetConfig(workingDirectory, packageOutputPath);

        return new PackageTestContext(workingDirectory, packageOutputPath, builtPackageVersion, configPath, globalPackagesFolder);
    }

    static string FindRepositoryRoot()
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

    static string FindPackageVersion(string packagesDirectory)
    {
        var regex = new Regex(@"^Humanizer\.(?<version>.+)\.nupkg$", RegexOptions.CultureInvariant);

        foreach (var packagePath in Directory.EnumerateFiles(packagesDirectory, "Humanizer.*.nupkg"))
        {
            var fileName = Path.GetFileName(packagePath);
            var match = regex.Match(fileName);
            if (match.Success)
                return match.Groups["version"].Value;
        }

        throw new XunitException($"Could not determine the packaged Humanizer version from '{packagesDirectory}'.");
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

public enum PackageScenario
{
    MetaPackage,
    CoreAndFrenchLanguage
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
    const string TargetFrameworksEnvironmentVariable = "HUMANIZER_PACKAGE_TEST_TARGET_FRAMEWORKS";
    const string ScenariosEnvironmentVariable = "HUMANIZER_PACKAGE_TEST_SCENARIOS";

    public static IReadOnlyList<string> TargetFrameworks { get; } = GetTargetFrameworks();
    public static IReadOnlyList<PackageScenario> Scenarios { get; } = GetScenarios();

    static string[] GetTargetFrameworks()
    {
        var configured = Environment.GetEnvironmentVariable(TargetFrameworksEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(configured))
            return ["net8.0", "net10.0"];

        return configured
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    static PackageScenario[] GetScenarios()
    {
        var configured = Environment.GetEnvironmentVariable(ScenariosEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(configured))
            return [PackageScenario.MetaPackage, PackageScenario.CoreAndFrenchLanguage];

        return configured
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ParseScenario)
            .Distinct()
            .ToArray();
    }

    static PackageScenario ParseScenario(string value) =>
        value.ToLowerInvariant() switch
        {
            "meta" or "metapackage" => PackageScenario.MetaPackage,
            "core-lang" or "core+lang" or "core-language" => PackageScenario.CoreAndFrenchLanguage,
            _ => throw new XunitException($"Unknown package smoke test scenario '{value}'.")
        };
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
        string workingDirectory,
        IReadOnlyDictionary<string, string>? environmentVariables = null)
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

        if (environmentVariables is not null)
        {
            foreach (var (key, value) in environmentVariables)
                process.StartInfo.Environment[key] = value;
        }

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
