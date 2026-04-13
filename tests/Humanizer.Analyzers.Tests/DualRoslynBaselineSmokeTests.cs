using System.Reflection;

using Xunit;

namespace Humanizer.Analyzers.Tests;

/// <summary>
/// Smoke tests that confirm the baseline (Roslyn 3.8) arm of the analyzer is compiled and loaded.
/// The main test project builds the analyzer with the default RoslynVersion=3.8, which does NOT
/// define ROSLYN_4_14_OR_GREATER. The IsNamespaceMatch methods use string parameters in this arm.
/// The companion project Humanizer.Analyzers.Tests.Roslyn414 has matching tests for the 4.14 arm.
/// </summary>
public class DualRoslynBaselineSmokeTests
{
    [Fact]
    public void AnalyzerAssembly_ContainsFallbackStringCode()
    {
        var analyzerType = typeof(NamespaceMigrationAnalyzer);
        var method = analyzerType
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Single(m => m.Name == "IsNamespaceMatch");

        Assert.Equal(typeof(string), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void CodeFixProvider_ContainsFallbackStringCode()
    {
        var codeFixType = typeof(NamespaceMigrationCodeFixProvider);
        var method = codeFixType
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Single(m => m.Name == "IsNamespaceMatch");

        Assert.Equal(typeof(string), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void AnalyzerAssembly_ReferencesBaselineRoslynPackages()
    {
        // Verify the analyzer assembly was compiled against baseline Roslyn 3.8 packages.
        // The Microsoft.CodeAnalysis.CSharp reference should have version < 4.14.0.
        var assembly = typeof(NamespaceMigrationAnalyzer).Assembly;
        var codeAnalysisRef = assembly.GetReferencedAssemblies()
            .SingleOrDefault(r => r.Name == "Microsoft.CodeAnalysis.CSharp");

        Assert.NotNull(codeAnalysisRef);
        Assert.True(codeAnalysisRef.Version < new Version(4, 14, 0),
            $"Expected Microsoft.CodeAnalysis.CSharp < 4.14.0 but found {codeAnalysisRef.Version}");
    }
}
