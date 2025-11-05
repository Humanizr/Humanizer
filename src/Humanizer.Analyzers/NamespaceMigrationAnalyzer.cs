using System.Collections.Frozen;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Humanizer.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NamespaceMigrationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "HUMANIZER001";
    private const string Category = "Usage";

    private static readonly LocalizableString Title = "Old Humanizer namespace usage";
    private static readonly LocalizableString MessageFormat = "The namespace '{0}' has been consolidated into 'Humanizer' in v3. Update your using directive.";
    private static readonly LocalizableString Description = "Humanizer v3 consolidates sub-namespaces into the root Humanizer namespace. This using directive should be updated to 'using Humanizer;'.";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: Description);

    // All the old namespaces that were consolidated in v3
    // Using FrozenSet for optimal lookup performance
    private static readonly FrozenSet<string> OldNamespaces = FrozenSet.ToFrozenSet(
        [
            "Humanizer.Bytes",
            "Humanizer.Localisation",
            "Humanizer.Localisation.Formatters",
            "Humanizer.Localisation.NumberToWords",
            "Humanizer.DateTimeHumanizeStrategy",
            "Humanizer.Configuration",
            "Humanizer.Localisation.DateToOrdinalWords",
            "Humanizer.Localisation.Ordinalizers",
            "Humanizer.Inflections",
            "Humanizer.Localisation.CollectionFormatters",
            "Humanizer.Localisation.TimeToClockNotation"
        ],
        StringComparer.Ordinal);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeUsingDirective, SyntaxKind.UsingDirective);
    }

    private static void AnalyzeUsingDirective(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not UsingDirectiveSyntax usingDirective)
            return;

        if (usingDirective.Name is null)
            return;

        var namespaceName = usingDirective.Name.ToString();

        if (OldNamespaces.Contains(namespaceName))
        {
            var diagnostic = Diagnostic.Create(Rule, usingDirective.Name.GetLocation(), namespaceName);
            context.ReportDiagnostic(diagnostic);
        }
    }


}
