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
}
