using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NamespaceMigrationAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider()
    {
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null)
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

    private async Task<Document> ReplaceUsingDirectiveAsync(Document document, UsingDirectiveSyntax usingDirective, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null)
            return document;

        // Check if "using Humanizer;" already exists
        var compilationUnit = root as CompilationUnitSyntax;
        if (compilationUnit != null)
        {
            var hasHumanizerUsing = compilationUnit.Usings.Any(u => 
                u.Name?.ToString() == "Humanizer");

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

    private async Task<Document> ReplaceQualifiedNameAsync(Document document, QualifiedNameSyntax qualifiedName, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null)
            return document;

        // Find which old namespace this qualified name starts with
        var fullName = qualifiedName.ToString();
        string? matchedNamespace = null;
        
        var oldNamespaces = new[]
        {
            "Humanizer.Bytes",
            "Humanizer.Localisation.Formatters",
            "Humanizer.Localisation.NumberToWords",
            "Humanizer.Localisation.DateToOrdinalWords",
            "Humanizer.Localisation.Ordinalizers",
            "Humanizer.Localisation.CollectionFormatters",
            "Humanizer.Localisation.TimeToClockNotation",
            "Humanizer.Localisation",
            "Humanizer.DateTimeHumanizeStrategy",
            "Humanizer.Configuration",
            "Humanizer.Inflections"
        };

        foreach (var ns in oldNamespaces)
        {
            if (fullName == ns || fullName.StartsWith(ns + "."))
            {
                matchedNamespace = ns;
                break;
            }
        }

        if (matchedNamespace == null)
            return document;

        // Replace the old namespace prefix with "Humanizer"
        var remainder = fullName.Substring(matchedNamespace.Length);
        var trimmedRemainder = remainder.TrimStart('.');
        var newName = !string.IsNullOrEmpty(trimmedRemainder)
            ? $"Humanizer.{trimmedRemainder}"
            : "Humanizer";

        var newQualifiedName = SyntaxFactory.ParseName(newName)
            .WithLeadingTrivia(qualifiedName.GetLeadingTrivia())
            .WithTrailingTrivia(qualifiedName.GetTrailingTrivia());

        var updatedRoot = root.ReplaceNode(qualifiedName, newQualifiedName);
        return document.WithSyntaxRoot(updatedRoot);
    }
}
