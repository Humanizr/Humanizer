using System.Collections.Generic;
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

    // All the old namespaces that were consolidated in v3.
    private static readonly string[] OldNamespaceValues =
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
    ];

    private static readonly string[] PrefixMatchingNamespaces =
    [
        "Humanizer.Localisation.CollectionFormatters",
        "Humanizer.Localisation.TimeToClockNotation",
        "Humanizer.Localisation.DateToOrdinalWords",
        "Humanizer.Localisation.NumberToWords",
        "Humanizer.Localisation.Formatters",
        "Humanizer.Localisation.Ordinalizers",
        "Humanizer.DateTimeHumanizeStrategy",
        "Humanizer.Configuration",
        "Humanizer.Localisation",
        "Humanizer.Inflections",
        "Humanizer.Bytes"
    ];

    private static readonly HashSet<string> OldNamespaces = new(OldNamespaceValues, StringComparer.Ordinal);

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

    private static void AnalyzeQualifiedName(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not QualifiedNameSyntax qualifiedName)
            return;

        // Skip if this is part of a using directive (already handled above)
        // Check all ancestors, not just immediate parent, because nested qualified names
        // will have another qualified name as parent
        if (qualifiedName.Ancestors().OfType<UsingDirectiveSyntax>().Any())
            return;

        // Skip if this qualified name has a parent qualified name that also matches
        // We only want to report the outermost qualified name to avoid duplicate diagnostics
        if (qualifiedName.Parent is QualifiedNameSyntax parentQualified)
        {
            var parentName = parentQualified.ToString();
            if (HasMatchingNamespace(parentName))
                return;
        }

        var fullName = qualifiedName.ToString();
        if (!TryGetMatchingNamespace(fullName, out var matchingNamespace))
            return;

        var diagnostic = Diagnostic.Create(Rule, qualifiedName.GetLocation(), matchingNamespace);
        context.ReportDiagnostic(diagnostic);
    }

#if ROSLYN_4_14_OR_GREATER
    private static bool IsNamespaceMatch(ReadOnlySpan<char> fullName, ReadOnlySpan<char> oldNamespace)
    {
        // Exact match
        if (fullName.Length == oldNamespace.Length)
            return fullName.Equals(oldNamespace, StringComparison.Ordinal);

        // Prefix match with dot separator
        return fullName.Length > oldNamespace.Length
            && fullName[oldNamespace.Length] == '.'
            && fullName.StartsWith(oldNamespace, StringComparison.Ordinal);
    }
#else
    private static bool IsNamespaceMatch(string fullName, string oldNamespace)
    {
        if (fullName.Length == oldNamespace.Length)
            return string.Equals(fullName, oldNamespace, StringComparison.Ordinal);

        return fullName.Length > oldNamespace.Length
            && fullName[oldNamespace.Length] == '.'
            && fullName.StartsWith(oldNamespace, StringComparison.Ordinal);
    }
#endif

    private static bool HasMatchingNamespace(string namespaceName)
    {
#if ROSLYN_4_14_OR_GREATER
        var nameSpan = namespaceName.AsSpan();
        foreach (var ns in PrefixMatchingNamespaces)
        {
            if (IsNamespaceMatch(nameSpan, ns))
                return true;
        }
#else
        foreach (var ns in PrefixMatchingNamespaces)
        {
            if (IsNamespaceMatch(namespaceName, ns))
                return true;
        }
#endif

        return false;
    }

    private static bool TryGetMatchingNamespace(string fullName, out string? matchingNamespace)
    {
#if ROSLYN_4_14_OR_GREATER
        var fullNameSpan = fullName.AsSpan();
        foreach (var ns in PrefixMatchingNamespaces)
        {
            if (!IsNamespaceMatch(fullNameSpan, ns))
                continue;

            matchingNamespace = ns;
            return true;
        }
#else
        foreach (var ns in PrefixMatchingNamespaces)
        {
            if (!IsNamespaceMatch(fullName, ns))
                continue;

            matchingNamespace = ns;
            return true;
        }
#endif

        matchingNamespace = null;
        return false;
    }
}
