# String Transformations

`Transform` lets you apply one or more `IStringTransformer` implementations to a string. The built-in transformers cover the common casing scenarios, and you can add your own for domain-specific formatting.

```csharp
using Humanizer;

"Sentence casing".Transform(To.LowerCase);            // "sentence casing"
"Sentence casing".Transform(To.SentenceCase);         // "Sentence casing"
"Sentence casing".Transform(To.TitleCase);            // "Sentence Casing"
"Sentence casing".Transform(To.UpperCase);            // "SENTENCE CASING"
"HTML_parser".Transform(To.LowerCase, To.TitleCase);  // "Html Parser"
```

Create custom transformers by implementing `IStringTransformer`. You can then pass them to `Transform` alongside the built-in helpers.

## Chaining transformers

`Transform` accepts a params array of transformers, letting you compose multiple operations in a single call. For example, to strip diacritics, title-case the result, and remove spaces:

```csharp
using System.Globalization;
using System.Text;

"Département d'Informatique".Transform(
	To.LowerCase,
	new RemoveDiacriticsTransformer(),
	To.TitleCase);                                // "Departement D'Informatique"

public sealed class RemoveDiacriticsTransformer : IStringTransformer
{
	public string Transform(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}

		var normalized = input.Normalize(NormalizationForm.FormD);
		var builder = new StringBuilder(normalized.Length);

		foreach (var character in normalized)
		{
			if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
			{
				builder.Append(character);
			}
		}

		return builder.ToString().Normalize(NormalizationForm.FormC);
	}
}
```

## Reusable pipelines

Wrap common sequences in helper methods so you can apply them consistently.

```csharp
using System.Text.RegularExpressions;

public static class SlugTransform
{
	static readonly Regex NonSlugCharacters = new("[^a-z0-9]+", RegexOptions.IgnoreCase);

	public static string ToSlug(this string phrase)
	{
		var lower = phrase.Transform(To.LowerCase);
		var collapsed = NonSlugCharacters.Replace(lower, "-");
		return collapsed.Trim('-');
	}
}

"Hello, World!".ToSlug();                       // "hello-world"
```

## Guidelines

- Keep transformers idempotent where possible—calling them multiple times should not change the output.
- Handle `null` and empty inputs gracefully to avoid unexpected exceptions.
- Prefer helpers or extension methods for reuse; Humanizer does not expose a global transformer registry.
