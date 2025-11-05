using Xunit;
using VerifyCS = Humanizer.Analyzers.Tests.CSharpCodeFixVerifier<
    Humanizer.Analyzers.NamespaceMigrationAnalyzer,
    Humanizer.Analyzers.NamespaceMigrationCodeFixProvider>;

namespace Humanizer.Analyzers.Tests;

public class NamespaceMigrationAnalyzerTests
{
    [Fact]
    public async Task EmptyCode_NoDiagnostic()
    {
        var test = "";
        await VerifyCS.VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task UsingHumanizer_NoDiagnostic()
    {
        var test = @"
using Humanizer;

class TestClass { }
";
        await VerifyCS.VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task UsingHumanizerBytes_Diagnostic()
    {
        var test = @"
using {|#0:Humanizer.Bytes|};

class TestClass { }
";
        await VerifyCS.VerifyAnalyzerAsync(test,
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(0)
                .WithArguments("Humanizer.Bytes"));
    }

    [Fact]
    public async Task UsingHumanizerLocalisation_Diagnostic()
    {
        var test = @"
using {|#0:Humanizer.Localisation|};

class TestClass { }
";
        await VerifyCS.VerifyAnalyzerAsync(test,
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(0)
                .WithArguments("Humanizer.Localisation"));
    }

    [Fact]
    public async Task UsingHumanizerLocalisationFormatters_Diagnostic()
    {
        var test = @"
using {|#0:Humanizer.Localisation.Formatters|};

class TestClass { }
";
        await VerifyCS.VerifyAnalyzerAsync(test,
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(0)
                .WithArguments("Humanizer.Localisation.Formatters"));
    }

    [Fact]
    public async Task UsingHumanizerConfiguration_Diagnostic()
    {
        var test = @"
using {|#0:Humanizer.Configuration|};

class TestClass { }
";
        await VerifyCS.VerifyAnalyzerAsync(test,
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(0)
                .WithArguments("Humanizer.Configuration"));
    }

    [Fact]
    public async Task UsingHumanizerInflections_Diagnostic()
    {
        var test = @"
using {|#0:Humanizer.Inflections|};

class TestClass { }
";
        await VerifyCS.VerifyAnalyzerAsync(test,
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(0)
                .WithArguments("Humanizer.Inflections"));
    }

    [Fact]
    public async Task MultipleOldUsings_MultipleDiagnostics()
    {
        var test = @"
using {|#0:Humanizer.Bytes|};
using {|#1:Humanizer.Localisation|};
using {|#2:Humanizer.Configuration|};

class TestClass { }
";
        await VerifyCS.VerifyAnalyzerAsync(test,
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(0)
                .WithArguments("Humanizer.Bytes"),
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(1)
                .WithArguments("Humanizer.Localisation"),
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(2)
                .WithArguments("Humanizer.Configuration"));
    }

    [Fact]
    public async Task QualifiedNameUsage_Diagnostic()
    {
        var test = @"
namespace TestNamespace
{
    class TestClass 
    {
        void Method()
        {
            var typeName = typeof({|#0:Humanizer.Bytes.IFormatter|}).Name;
        }
    }
}
";
        await VerifyCS.VerifyAnalyzerAsync(test,
            VerifyCS.Diagnostic(NamespaceMigrationAnalyzer.DiagnosticId)
                .WithLocation(0)
                .WithArguments("Humanizer.Bytes"));
    }

}
