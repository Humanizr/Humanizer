using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CodeFixes;

namespace Humanizer.Analyzers.Tests;

public static class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFix : CodeFixProvider, new()
{
    public static DiagnosticResult Diagnostic(string diagnosticId)
        => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic(diagnosticId);

    public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
        {
            TestCode = source,
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        test.TestState.AdditionalReferences.Add(typeof(Humanizer.ByteSize).Assembly);
        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync();
    }

    public static async Task VerifyCodeFixAsync(string source, string fixedSource)
    {
        await VerifyCodeFixAsync(source, DiagnosticResult.EmptyDiagnosticResults, fixedSource);
    }

    public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource)
    {
        await VerifyCodeFixAsync(source, new[] { expected }, fixedSource);
    }

    public static async Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource)
    {
        var test = new CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
        {
            TestCode = source,
            FixedCode = fixedSource,
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        test.TestState.AdditionalReferences.Add(typeof(Humanizer.ByteSize).Assembly);
        test.FixedState.AdditionalReferences.Add(typeof(Humanizer.ByteSize).Assembly);
        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync();
    }
}
