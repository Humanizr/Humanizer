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
}
