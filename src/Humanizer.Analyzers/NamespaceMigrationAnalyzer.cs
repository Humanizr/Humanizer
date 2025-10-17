using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Humanizer.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NamespaceMigrationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "HUMANIZER001";
    private const string Category = "Usage";

    private static readonly LocalizableString Title = "Old Humanizer namespace usage";
    private static readonly LocalizableString MessageFormat = "The namespace '{0}' has been consolidated into 'Humanizer' in v3. Update your using directive.";
    private static readonly LocalizableString Description = "Humanizer v3 consolidates sub-namespaces into the root Humanizer namespace. This using directive should be updated to 'using Humanizer;'.";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    // All the old namespaces that were consolidated in v3
    // Using StringComparer.Ordinal for optimal performance
    private static readonly ImmutableHashSet<string> OldNamespaces = ImmutableHashSet.Create(
        StringComparer.Ordinal,
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
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeUsingDirective, SyntaxKind.UsingDirective);
        context.RegisterSyntaxNodeAction(AnalyzeQualifiedName, SyntaxKind.QualifiedName);
    }

    private static void AnalyzeUsingDirective(SyntaxNodeAnalysisContext context)
    {
        var usingDirective = (UsingDirectiveSyntax)context.Node;
        
        if (usingDirective.Name == null)
            return;

        var namespaceName = usingDirective.Name.ToString();
        
        if (OldNamespaces.Contains(namespaceName))
        {
            var diagnostic = Diagnostic.Create(Rule, usingDirective.Name.GetLocation(), namespaceName);
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static void AnalyzeQualifiedName(SyntaxNodeAnalysisContext context)
    {
        var qualifiedName = (QualifiedNameSyntax)context.Node;
        
        // Skip if this is part of a using directive (already handled above)
        if (qualifiedName.Parent is UsingDirectiveSyntax)
            return;

        var fullName = qualifiedName.ToString();
        
        // Check if any old namespace is used as a prefix
        // Iterate in reverse order to match longer namespaces first (more specific)
        foreach (var oldNamespace in OldNamespaces)
        {
            if (IsNamespaceMatch(fullName, oldNamespace))
            {
                var diagnostic = Diagnostic.Create(Rule, qualifiedName.GetLocation(), oldNamespace);
                context.ReportDiagnostic(diagnostic);
                break;
            }
        }
    }

    private static bool IsNamespaceMatch(string fullName, string oldNamespace)
    {
        // Exact match
        if (fullName.Length == oldNamespace.Length)
            return string.Equals(fullName, oldNamespace, StringComparison.Ordinal);

        // Prefix match with dot separator (avoid allocating with StartsWith + concatenation)
        if (fullName.Length > oldNamespace.Length && 
            fullName[oldNamespace.Length] == '.' &&
            fullName.AsSpan().StartsWith(oldNamespace.AsSpan(), StringComparison.Ordinal))
        {
            return true;
        }

        return false;
    }
}
