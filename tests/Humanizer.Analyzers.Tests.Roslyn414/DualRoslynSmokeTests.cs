using System.Reflection;

using Xunit;

namespace Humanizer.Analyzers.Tests;

/// <summary>
/// Smoke tests that confirm the Roslyn 4.14 arm of the analyzer is compiled and loaded.
/// The companion project builds the analyzer with RoslynVersion=4.14, which defines
/// ROSLYN_4_14_OR_GREATER and compiles Span-based overloads of IsNamespaceMatch,
/// HasMatchingNamespace, and TryGetMatchingNamespace.
/// </summary>
public class DualRoslynSmokeTests
{
    [Fact]
    public void AnalyzerAssembly_ContainsRoslyn414Code()
    {
        // When built with RoslynVersion=4.14, the analyzer has ReadOnlySpan<char> overloads
        // of IsNamespaceMatch. We verify this by checking that the analyzer type has a
        // private static method IsNamespaceMatch that accepts ReadOnlySpan<char> parameters.
        var analyzerType = typeof(NamespaceMigrationAnalyzer);
        var methods = analyzerType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

        var method = Assert.Single(methods, m => m.Name == "IsNamespaceMatch");

        // With ROSLYN_4_14_OR_GREATER, the first parameter should be ReadOnlySpan<char>
        var firstParam = method.GetParameters()[0];
        Assert.Equal(typeof(ReadOnlySpan<char>), firstParam.ParameterType);
    }

    [Fact]
    public void CodeFixProvider_ContainsRoslyn414Code()
    {
        // Same check for the code fix provider's IsNamespaceMatch
        var codeFixType = typeof(NamespaceMigrationCodeFixProvider);
        var methods = codeFixType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

        var method = Assert.Single(methods, m => m.Name == "IsNamespaceMatch");

        var firstParam = method.GetParameters()[0];
        Assert.Equal(typeof(ReadOnlySpan<char>), firstParam.ParameterType);
    }
}
