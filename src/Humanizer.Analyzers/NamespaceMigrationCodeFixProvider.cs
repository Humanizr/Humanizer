using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Humanizer.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NamespaceMigrationCodeFixProvider)), Shared]
public class NamespaceMigrationCodeFixProvider : CodeFixProvider
{
    private const string Title = "Update to Humanizer namespace";

    // Ordered by length (longest first) for optimal matching
    private static readonly FrozenSet<string> OldNamespaces = FrozenSet.ToFrozenSet(
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
        ],
        StringComparer.Ordinal);

    public sealed override ImmutableArray<string> FixableDiagnosticIds => [NamespaceMigrationAnalyzer.DiagnosticId];

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var node = root.FindNode(diagnosticSpan);

        // Handle using directives
        if (node is UsingDirectiveSyntax usingDirective)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: c => ReplaceUsingDirectiveAsync(context.Document, usingDirective, c),
                    equivalenceKey: Title),
                diagnostic);
        }
        // Handle qualified names (e.g., Humanizer.Bytes.ByteSize)
        else if (node is QualifiedNameSyntax qualifiedName)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: c => ReplaceQualifiedNameAsync(context.Document, qualifiedName, c),
                    equivalenceKey: Title),
                diagnostic);
        }
    }

    private static async Task<Document> ReplaceUsingDirectiveAsync(Document document, UsingDirectiveSyntax usingDirective, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        // Check if "using Humanizer;" already exists
        if (root is CompilationUnitSyntax compilationUnit)
        {
            var hasHumanizerUsing = compilationUnit.Usings.Any(static u => 
                u.Name?.ToFullString() == "Humanizer");

            if (hasHumanizerUsing)
            {
                // Just remove the old using directive
                var newRoot = root.RemoveNode(usingDirective, SyntaxRemoveOptions.KeepNoTrivia);
                return document.WithSyntaxRoot(newRoot!);
            }
        }

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
        var fullName = qualifiedName.ToString().AsSpan();
        var foundNs = OldNamespaces.FirstOrDefault(ns => IsNamespaceMatch(fullName, ns.AsSpan()));
        ReadOnlySpan<char> matchedNamespace = foundNs is not null ? foundNs.AsSpan() : default;

        if (matchedNamespace.IsEmpty)
            return document;

        // Replace the old namespace prefix with "Humanizer"
        var newNameText = GetReplacementName(fullName, matchedNamespace);

        var newQualifiedName = SyntaxFactory.ParseName(newNameText)
            .WithLeadingTrivia(qualifiedName.GetLeadingTrivia())
            .WithTrailingTrivia(qualifiedName.GetTrailingTrivia());

        var updatedRoot = root.ReplaceNode(qualifiedName, newQualifiedName);
        return document.WithSyntaxRoot(updatedRoot);
    }

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

    private static string GetReplacementName(ReadOnlySpan<char> fullName, ReadOnlySpan<char> matchedNamespace)
    {
        if (fullName.Length == matchedNamespace.Length)
            return "Humanizer";

        // Skip the matched namespace and the dot
        var startIndex = matchedNamespace.Length + 1;
        
        if (startIndex >= fullName.Length)
            return "Humanizer";

        // Use modern string concatenation with spans
        var remainder = fullName[startIndex..];
        
#if NET10_0_OR_GREATER
        return string.Concat("Humanizer.", remainder);
#else
        return string.Concat("Humanizer.", remainder.ToString());
#endif
    }
}
