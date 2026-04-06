using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

using Xunit;

namespace Humanizer.Analyzers.Tests;

public class WordsToNumberMigrationCodeFixTests
{
    [Fact]
    public async Task FixesToNumberAssignedToInt()
    {
        var test = @"
using System.Globalization;
using Humanizer;

class TestClass
{
    void Method(string words, CultureInfo culture)
    {
        int value = {|CS0266:words.ToNumber(culture)|};
    }
}
";

        var fixedCode = @"
using System.Globalization;
using Humanizer;

class TestClass
{
    void Method(string words, CultureInfo culture)
    {
        int value = checked(((int)words.ToNumber(culture)));
    }
}
";

        await VerifyCodeFixAsync(test, fixedCode);
    }

    static async Task VerifyCodeFixAsync(string source, string fixedSource)
    {
        var test = new CSharpCodeFixTest<NoopAnalyzer, WordsToNumberMigrationCodeFixProvider, DefaultVerifier>
        {
            TestCode = source,
            FixedCode = fixedSource,
            CompilerDiagnostics = CompilerDiagnostics.Errors,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net100
        };

        test.TestState.AdditionalReferences.Add(typeof(Humanizer.ByteSize).Assembly);
        test.FixedState.AdditionalReferences.Add(typeof(Humanizer.ByteSize).Assembly);
        await test.RunAsync();
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    sealed class NoopAnalyzer : DiagnosticAnalyzer
    {
        public override System.Collections.Immutable.ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            System.Collections.Immutable.ImmutableArray<DiagnosticDescriptor>.Empty;

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        }
    }
}