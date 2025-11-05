# String Transformations

`Transform` lets you apply one or more `IStringTransformer` implementations to a string. The built-in transformers cover the common casing scenarios, and you can add your own for domain-specific formatting.

```csharp
using Humanizer;
using Humanizer.Transforms;

"Sentence casing".Transform(To.LowerCase);            // "sentence casing"
"Sentence casing".Transform(To.SentenceCase);         // "Sentence casing"
"Sentence casing".Transform(To.TitleCase);            // "Sentence Casing"
"Sentence casing".Transform(To.UpperCase);            // "SENTENCE CASING"
"HTML_parser".Transform(To.LowerCase, To.TitleCase);  // "Html Parser"
```

Create custom transformers by implementing `IStringTransformer` and registering them through `Configurator.StringTransformers.Add`. The order of registration determines the execution order when chaining transformations.

## Chaining transformers

`Transform` accepts a params array of transformers, letting you compose multiple operations in a single call. For example, to strip diacritics, title-case the result, and remove spaces:

```csharp
"Département d'Informatique".Transform(
	To.LowerCase,
	To.TitleCase,
	new RemoveDiacriticsTransformer());           // "Departement D'Informatique"

public sealed class RemoveDiacriticsTransformer : IStringTransformer
{
	public string Transform(string input) => input.RemoveDiacritics();
}
```

## Registering global transformers

When you need a reusable pipeline (for example, for SEO-friendly slugs), register it once and reuse it everywhere:

```csharp
Configurator.StringTransformers.Add(To.LowerCase);
Configurator.StringTransformers.Add(new KebabCaseTransformer());

var slug = "Hello, World!".Transform();            // "hello-world"

public sealed class KebabCaseTransformer : IStringTransformer
{
	public string Transform(string input) => input.Kebaberize();
}
```

## Guidelines

- Keep transformers idempotent where possible—calling them multiple times should not change the output.
- Handle `null` and empty inputs gracefully to avoid unexpected exceptions.
- Document the configured transformer list so contributors understand the global text pipeline.
