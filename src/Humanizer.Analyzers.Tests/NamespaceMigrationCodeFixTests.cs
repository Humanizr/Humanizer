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
            .WithSpan(2, 7, 2, 23)
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
            .WithSpan(2, 7, 2, 30)
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
            .WithSpan(2, 7, 2, 31)
            .WithArguments("Humanizer.Configuration");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public async Task FixRemovesRedundantUsing_WhenHumanizerAlreadyExists()
    {
        var test = @"
using Humanizer;
using Humanizer.Bytes;

class TestClass { }
";

        var fixedCode = @"
using Humanizer;

class TestClass { }
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(3, 7, 3, 23)
            .WithArguments("Humanizer.Bytes");

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public async Task FixMultipleUsings()
    {
        var test = @"
using Humanizer.Bytes;
using Humanizer.Localisation;
using Humanizer.Configuration;

class TestClass { }
";

        var fixedCode = @"
using Humanizer;

class TestClass { }
";

        var expected = new[]
        {
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithSpan(2, 7, 2, 23)
                .WithArguments("Humanizer.Bytes"),
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithSpan(3, 7, 3, 30)
                .WithArguments("Humanizer.Localisation"),
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithSpan(4, 7, 4, 31)
                .WithArguments("Humanizer.Configuration")
        };

        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }

    [Fact]
    public async Task FixQualifiedNameUsage()
    {
        var test = @"
class TestClass 
{
    void Method()
    {
        var size = Humanizer.Bytes.ByteSize.FromKilobytes(10);
    }
}
";

        var fixedCode = @"
class TestClass 
{
    void Method()
    {
        var size = Humanizer.ByteSize.FromKilobytes(10);
    }
}
";

        var expected = VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
            .WithSpan(6, 20, 6, 43)
            .WithArguments("Humanizer.Bytes");

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
            .WithSpan(2, 7, 2, 41)
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
}
