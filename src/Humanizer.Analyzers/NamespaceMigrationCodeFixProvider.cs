using System.Collections.Immutable;
using System.Composition;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Humanizer.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NamespaceMigrationCodeFixProvider)), Shared]
public class NamespaceMigrationCodeFixProvider : CodeFixProvider
{
    private const string Title = "Update to Humanizer namespace";

    // Ordered by length (longest first) for optimal matching
    private static readonly string[] OldNamespaces =
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

    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NamespaceMigrationAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var node = root.FindNode(diagnosticSpan);

        // The diagnostic is reported on the namespace name, so navigate up to the using directive
        var usingDirective = node.FirstAncestorOrSelf<UsingDirectiveSyntax>();
        if (usingDirective is not null)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: c => ReplaceUsingDirectiveAsync(context.Document, usingDirective, c),
                    equivalenceKey: Title),
                diagnostic);
            return;
        }

        // Handle qualified names (e.g., Humanizer.Bytes.ByteSize)
        if (node is QualifiedNameSyntax qualifiedName)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: c => ReplaceQualifiedNameAsync(context.Document, qualifiedName, c),
                    equivalenceKey: Title),
                diagnostic);
        }
    }

    private static async Task<Document> ReplaceUsingDirectiveAsync(
        Document document,
        UsingDirectiveSyntax usingDirective, 
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        // Replace the namespace with "Humanizer"
        var newName = SyntaxFactory.IdentifierName("Humanizer");
        var newUsingDirective = usingDirective.WithName(newName);

        var updatedRoot = root.ReplaceNode(usingDirective, newUsingDirective);
        return document.WithSyntaxRoot(updatedRoot);
    }

    private static async Task<Document> ReplaceQualifiedNameAsync(Document document, QualifiedNameSyntax qualifiedName, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        // Find which old namespace this qualified name starts with
        var fullName = qualifiedName.ToString();
        if (!TryGetMatchingNamespace(fullName, out var matchedNamespace))
            return document;

        // Replace the old namespace prefix with "Humanizer"
        var newNameText = GetReplacementName(fullName, matchedNamespace!);

        var newQualifiedName = SyntaxFactory.ParseName(newNameText)
            .WithLeadingTrivia(qualifiedName.GetLeadingTrivia())
            .WithTrailingTrivia(qualifiedName.GetTrailingTrivia());

        var updatedRoot = root.ReplaceNode(qualifiedName, newQualifiedName);
        return document.WithSyntaxRoot(updatedRoot);
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

    private static string GetReplacementName(string fullName, string matchedNamespace)
    {
        if (fullName.Length == matchedNamespace.Length)
            return "Humanizer";

        // Skip the matched namespace and the dot
        var startIndex = matchedNamespace.Length + 1;

        if (startIndex >= fullName.Length)
            return "Humanizer";

#if NET10_0_OR_GREATER
        return string.Concat("Humanizer.", fullName.AsSpan(startIndex));
#else
        return string.Concat("Humanizer.", fullName.Substring(startIndex));
#endif
    }

    private static bool TryGetMatchingNamespace(string fullName, out string? matchedNamespace)
    {
#if ROSLYN_4_14_OR_GREATER
        var fullNameSpan = fullName.AsSpan();
        foreach (var ns in OldNamespaces)
        {
            if (!IsNamespaceMatch(fullNameSpan, ns))
                continue;

            matchedNamespace = ns;
            return true;
        }
#else
        foreach (var ns in OldNamespaces)
        {
            if (!IsNamespaceMatch(fullName, ns))
                continue;

            matchedNamespace = ns;
            return true;
        }
#endif

        matchedNamespace = null;
        return false;
    }
}
