using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

    [Fact]
    public async Task FixesWordsToNumberInterfaceConvertAssignedToInt()
    {
        var test = @"
using Humanizer;

class TestClass
{
    void Method(IWordsToNumberConverter converter, string words)
    {
        int value = {|CS0266:converter.Convert(words)|};
    }
}
";

        var fixedCode = @"
using Humanizer;

class TestClass
{
    void Method(IWordsToNumberConverter converter, string words)
    {
        int value = checked(((int)converter.Convert(words)));
    }
}
";

        await VerifyCodeFixAsync(test, fixedCode);
    }

    [Fact]
    public async Task FixesWordsToNumberImplementationConvertAssignedToInt()
    {
        var test = @"
using Humanizer;

class TestClass
{
    void Method(LocalConverter converter, string words)
    {
        int value = {|CS0266:converter.Convert(words)|};
    }
}

class LocalConverter : IWordsToNumberConverter
{
    public bool TryConvert(string words, out long parsedValue)
    {
        parsedValue = 0;
        return true;
    }

    public bool TryConvert(string words, out long parsedValue, out string? unrecognizedNumber)
    {
        parsedValue = 0;
        unrecognizedNumber = null;
        return true;
    }

    public long Convert(string words) => 0;
}
";

        var fixedCode = @"
using Humanizer;

class TestClass
{
    void Method(LocalConverter converter, string words)
    {
        int value = checked(((int)converter.Convert(words)));
    }
}

class LocalConverter : IWordsToNumberConverter
{
    public bool TryConvert(string words, out long parsedValue)
    {
        parsedValue = 0;
        return true;
    }

    public bool TryConvert(string words, out long parsedValue, out string? unrecognizedNumber)
    {
        parsedValue = 0;
        unrecognizedNumber = null;
        return true;
    }

    public long Convert(string words) => 0;
}
";

        await VerifyCodeFixAsync(test, fixedCode);
    }

    [Fact]
    public async Task DoesNotOfferFixForUnrelatedLongInvocation()
    {
        var test = @"
class TestClass
{
    void Method()
    {
        int value = {|CS0266:GetLong()|};
    }

    long GetLong() => 0;
}
";

        await VerifyNoCodeFixAsync(test);
    }

    [Fact]
    public void MethodClassifierRejectsNonMatchingInvocationShapes()
    {
        var getLongMethod = GetDeclaredMethodSymbol(
            """
class TestClass
{
    long GetLong() => 0;
}
""",
            "GetLong");
        var localConvertMethod = GetDeclaredMethodSymbol(
            """
namespace Sample
{
    public interface IWordsToNumberConverter
    {
        long Convert(string words);
    }

    public class LocalConverter : IWordsToNumberConverter
    {
        public long Convert(string words) => 0;
    }
}
""",
            "Convert");

        Assert.False(InvokeIsLongWordsToNumberMethod(getLongMethod));
        Assert.False(InvokeIsLongWordsToNumberMethod(localConvertMethod));
        Assert.False(InvokeIsLongWordsToNumberResult(
            """
class TestClass
{
    void Method()
    {
        Missing();
    }
}
"""));
        Assert.False(InvokeTryFindWordsToNumberInvocation(null));
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

    static async Task VerifyNoCodeFixAsync(string source)
    {
        var test = new CSharpCodeFixTest<NoopAnalyzer, WordsToNumberMigrationCodeFixProvider, DefaultVerifier>
        {
            TestCode = source,
            CompilerDiagnostics = CompilerDiagnostics.Errors,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net100
        };

        test.TestState.AdditionalReferences.Add(typeof(Humanizer.ByteSize).Assembly);
        await test.RunAsync();
    }

    static IMethodSymbol GetDeclaredMethodSymbol(string source, string methodName)
    {
        var tree = CSharpSyntaxTree.ParseText(source);
        var compilation = CSharpCompilation.Create(
            "AnalyzerCoverage",
            [tree],
            [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var method = tree.GetRoot()
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .Last(method => method.Identifier.ValueText == methodName);

        return compilation.GetSemanticModel(tree).GetDeclaredSymbol(method)!;
    }

    static bool InvokeIsLongWordsToNumberMethod(IMethodSymbol method) =>
        (bool)GetPrivateProviderMethod(nameof(InvokeIsLongWordsToNumberMethod), "IsLongWordsToNumberMethod")
            .Invoke(null, [method])!;

    static bool InvokeIsLongWordsToNumberResult(string source)
    {
        var tree = CSharpSyntaxTree.ParseText(source);
        var compilation = CSharpCompilation.Create(
            "AnalyzerCoverage",
            [tree],
            [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var invocation = tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>().Single();

        return (bool)GetPrivateProviderMethod(nameof(InvokeIsLongWordsToNumberResult), "IsLongWordsToNumberResult")
            .Invoke(null, [invocation, compilation.GetSemanticModel(tree), CancellationToken.None])!;
    }

    static bool InvokeTryFindWordsToNumberInvocation(SyntaxNode? node)
    {
        var args = new object?[] { node, null };
        return (bool)GetPrivateProviderMethod(nameof(InvokeTryFindWordsToNumberInvocation), "TryFindWordsToNumberInvocation")
            .Invoke(null, args)!;
    }

    static MethodInfo GetPrivateProviderMethod(string callerName, string methodName) =>
        typeof(WordsToNumberMigrationCodeFixProvider).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException($"{callerName} could not find {methodName}.");

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