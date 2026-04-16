using System.Collections.Immutable;
using System.Composition;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Humanizer.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(WordsToNumberMigrationCodeFixProvider)), Shared]
public class WordsToNumberMigrationCodeFixProvider : CodeFixProvider
{
    const string CastTitle = "Wrap with checked int cast";
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create("CS0029", "CS0266");

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return;
        }

        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        if (semanticModel is null)
        {
            return;
        }

        foreach (var diagnostic in context.Diagnostics)
        {
            var node = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);
            switch (diagnostic.Id)
            {
                case "CS0029":
                case "CS0266":
                    if (TryFindWordsToNumberInvocation(node, out var narrowingInvocation) &&
                        IsLongWordsToNumberResult(narrowingInvocation, semanticModel, context.CancellationToken))
                    {
                        context.RegisterCodeFix(
                            CodeAction.Create(
                                CastTitle,
                                cancellationToken => WrapInvocationWithCheckedCastAsync(context.Document, narrowingInvocation, cancellationToken),
                                equivalenceKey: CastTitle),
                            diagnostic);
                    }

                    break;
                default:
                    break;
            }
        }
    }

    static bool TryFindWordsToNumberInvocation(SyntaxNode? node, out InvocationExpressionSyntax invocation)
    {
        invocation = node?.FirstAncestorOrSelf<InvocationExpressionSyntax>()!;
        return invocation is not null;
    }

    static bool IsLongWordsToNumberResult(InvocationExpressionSyntax invocation, SemanticModel semanticModel, CancellationToken cancellationToken)
    {
        return semanticModel.GetSymbolInfo(invocation, cancellationToken).Symbol is not IMethodSymbol symbol
            ? false
            : IsLongWordsToNumberMethod(symbol);
    }

    static bool IsLongWordsToNumberMethod(IMethodSymbol method) =>
        method.Name switch
        {
            "ToNumber" when method.ContainingType.Name == "WordsToNumberExtension" && method.ContainingNamespace.ToDisplayString() == "Humanizer" => method.ReturnType.SpecialType == SpecialType.System_Int64,
            "Convert" when ImplementsWordsToNumberInterface(method.ContainingType, "IWordsToNumberConverter") => method.ReturnType.SpecialType == SpecialType.System_Int64,
            _ => false
        };

    static bool ImplementsWordsToNumberInterface(INamedTypeSymbol containingType, string interfaceName) =>
        containingType.Name == interfaceName && containingType.ContainingNamespace.ToDisplayString() == "Humanizer" ||
        containingType.AllInterfaces.Any(interfaceType => interfaceType.Name == interfaceName && interfaceType.ContainingNamespace.ToDisplayString() == "Humanizer");

    static async Task<Document> WrapInvocationWithCheckedCastAsync(Document document, InvocationExpressionSyntax invocation, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        var castExpression = SyntaxFactory.CheckedExpression(
            SyntaxKind.CheckedExpression,
            SyntaxFactory.ParenthesizedExpression(
                SyntaxFactory.CastExpression(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                    invocation.WithoutTrivia())))
            .WithTriviaFrom(invocation);

        var updatedRoot = root.ReplaceNode(invocation, castExpression);
        return document.WithSyntaxRoot(updatedRoot);
    }
}