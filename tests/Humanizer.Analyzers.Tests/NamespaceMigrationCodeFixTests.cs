using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Xunit;
using VerifyCS = Humanizer.Analyzers.Tests.CSharpCodeFixVerifier<
    Humanizer.Analyzers.NamespaceMigrationAnalyzer,
    Humanizer.Analyzers.NamespaceMigrationCodeFixProvider>;

namespace Humanizer.Analyzers.Tests;

public class NamespaceMigrationCodeFixTests
{
    [Fact]
    public async Task FixUsingHumanizerBytes()
    {
        var test = @"
using Humanizer.Bytes;

class TestClass { }
";

        var fixedCode = @"
using Humanizer;

class TestClass { }
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(2, 7, 2, 22)
            .WithArguments("Humanizer.Bytes");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public async Task FixUsingHumanizerLocalisation()
    {
        var test = @"
using Humanizer.Localisation;

class TestClass { }
";

        var fixedCode = @"
using Humanizer;

class TestClass { }
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(2, 7, 2, 29)
            .WithArguments("Humanizer.Localisation");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public async Task FixUsingHumanizerConfiguration()
    {
        var test = @"
using Humanizer.Configuration;

class TestClass { }
";

        var fixedCode = @"
using Humanizer;

class TestClass { }
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(2, 7, 2, 30)
            .WithArguments("Humanizer.Configuration");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }



    [Fact]
    public async Task FixUsingHumanizerLocalisationFormatters()
    {
        var test = @"
using Humanizer.Localisation.Formatters;

class TestClass { }
";

        var fixedCode = @"
using Humanizer;

class TestClass { }
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(2, 7, 2, 40)
            .WithArguments("Humanizer.Localisation.Formatters");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public async Task FixUsingHumanizerInflections()
    {
        var test = @"
using Humanizer.Inflections;

class TestClass { }
";

        var fixedCode = @"
using Humanizer;

class TestClass { }
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(2, 7, 2, 28)
            .WithArguments("Humanizer.Inflections");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public async Task FixQualifiedNameUsage()
    {
        var test = @"
namespace TestNamespace
{
    class TestClass 
    {
        void Method()
        {
            var typeName = typeof(Humanizer.Bytes.IFormatter).Name;
        }
    }
}
";

        var fixedCode = @"
namespace TestNamespace
{
    class TestClass 
    {
        void Method()
        {
            var typeName = typeof(Humanizer.IFormatter).Name;
        }
    }
}
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(8, 35, 8, 61)
            .WithArguments("Humanizer.Bytes");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public async Task FixExactQualifiedNameUsage()
    {
        var test = @"
class Humanizer
{
    public class Bytes { }
}

class TestClass
{
    void Method()
    {
        var typeName = typeof(Humanizer.Bytes).Name;
    }
}
";

        var fixedCode = @"
class Humanizer
{
    public class Bytes { }
}

class TestClass
{
    void Method()
    {
        var typeName = typeof(Humanizer).Name;
    }
}
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(11, 31, 11, 46)
            .WithArguments("Humanizer.Bytes");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public void ReplacementHelpersHandleNonMatchingAndTrailingNamespaceEdges()
    {
        Assert.Equal("Humanizer", InvokeGetReplacementName("Humanizer.Bytes.", "Humanizer.Bytes"));
        Assert.False(InvokeTryGetMatchingNamespace("System.Text.StringBuilder", out var matchedNamespace));
        Assert.Null(matchedNamespace);
    }

    [Fact]
    public async Task ReplaceQualifiedNameLeavesNonMatchingNamesUnchanged()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var document = CreateDocument(
            """
class TestClass
{
    void Method()
    {
        var typeName = typeof(System.Text.StringBuilder).Name;
    }
}
""");
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        var qualifiedName = root!
            .DescendantNodes()
            .OfType<QualifiedNameSyntax>()
            .Single(name => name.ToString() == "System.Text.StringBuilder");

        var updatedDocument = await InvokeReplaceQualifiedNameAsync(document, qualifiedName, cancellationToken);
        var updatedText = await updatedDocument.GetTextAsync(cancellationToken);

        Assert.Contains("System.Text.StringBuilder", updatedText.ToString(), StringComparison.Ordinal);
    }

    static Document CreateDocument(string source)
    {
        var workspace = new AdhocWorkspace();
        var project = workspace.AddProject("AnalyzerCoverage", LanguageNames.CSharp);
        return workspace.AddDocument(project.Id, "Test.cs", SourceText.From(source));
    }

    static string InvokeGetReplacementName(string fullName, string matchedNamespace) =>
        (string)GetPrivateProviderMethod(nameof(InvokeGetReplacementName), "GetReplacementName")
            .Invoke(null, [fullName, matchedNamespace])!;

    static bool InvokeTryGetMatchingNamespace(string fullName, out string? matchedNamespace)
    {
        var args = new object?[] { fullName, null };
        var result = (bool)GetPrivateProviderMethod(nameof(InvokeTryGetMatchingNamespace), "TryGetMatchingNamespace")
            .Invoke(null, args)!;
        matchedNamespace = (string?)args[1];
        return result;
    }

    static async Task<Document> InvokeReplaceQualifiedNameAsync(
        Document document,
        QualifiedNameSyntax qualifiedName,
        CancellationToken cancellationToken)
    {
        var task = (Task<Document>)GetPrivateProviderMethod(nameof(InvokeReplaceQualifiedNameAsync), "ReplaceQualifiedNameAsync")
            .Invoke(null, [document, qualifiedName, cancellationToken])!;

        return await task;
    }

    static MethodInfo GetPrivateProviderMethod(string callerName, string methodName) =>
        typeof(NamespaceMigrationCodeFixProvider).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException($"{callerName} could not find {methodName}.");
}