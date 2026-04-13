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

    [Fact]
    public async Task FixesConvertViaInterfaceAssignedToInt()
    {
        var test = @"
using Humanizer;

class TestClass
{
    void Method(IWordsToNumberConverter converter)
    {
        int result = {|CS0266:converter.Convert(""five"")|};
    }
}
";

        var fixedCode = @"
using Humanizer;

class TestClass
{
    void Method(IWordsToNumberConverter converter)
    {
        int result = checked(((int)converter.Convert(""five"")));
    }
}
";

        await VerifyCodeFixAsync(test, fixedCode);
    }

    [Fact]
    public async Task FixesConvertViaImplementorAssignedToInt()
    {
        var test = @"
using Humanizer;

class MyConverter : IWordsToNumberConverter
{
    public bool TryConvert(string words, out long parsedValue) { parsedValue = 0; return false; }
    public bool TryConvert(string words, out long parsedValue, out string? unrecognizedNumber) { parsedValue = 0; unrecognizedNumber = null; return false; }
    public long Convert(string words) => 0;
}

class TestClass
{
    void Method()
    {
        var converter = new MyConverter();
        int result = {|CS0266:converter.Convert(""five"")|};
    }
}
";

        var fixedCode = @"
using Humanizer;

class MyConverter : IWordsToNumberConverter
{
    public bool TryConvert(string words, out long parsedValue) { parsedValue = 0; return false; }
    public bool TryConvert(string words, out long parsedValue, out string? unrecognizedNumber) { parsedValue = 0; unrecognizedNumber = null; return false; }
    public long Convert(string words) => 0;
}

class TestClass
{
    void Method()
    {
        var converter = new MyConverter();
        int result = checked(((int)converter.Convert(""five"")));
    }
}
";

        await VerifyCodeFixAsync(test, fixedCode);
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