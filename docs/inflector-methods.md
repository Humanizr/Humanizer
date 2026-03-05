# Inflector Helpers

Inflector extensions provide naming conversions such as PascalCase, camelCase, snake_case, and kebab-case. Apply them when generating identifiers, slugs, file names, or test data.

```csharp
using Humanizer;

"welcome_message".Pascalize();          // "WelcomeMessage"
"OrderNumber".Underscore();             // "order_number"
"CustomerAddress".Kebaberize();         // "customer-address"
"some title".Titleize();                // "Some Title"
"product-id".Camelize();                // "productId"
"feature_flag".Dasherize();             // "feature-flag"
"Heading for docs".Kebaberize();        // "heading-for-docs"
```

## Common helpers

- `Pascalize()` and `Camelize()` remove separators and adjust casing.
- `Underscore()` introduces `_` separators and lowers casing.
- `Dasherize()` and `Hyphenate()` convert underscores to `-`.
- `Kebaberize()` performs both lowercase conversion and hyphenation, ideal for URLs.
- Combine `Transform()` helpers (for example, `To.LowerCase` plus a custom diacritic stripper) when you need additional slug rules.
- `Titleize()` converts text to title case while respecting word boundaries.

## Combining helpers

Chain methods to move between storage formats and display formats:

```csharp
"UserDisplayName".Humanize()            // "User display name"
	.Titleize();                        // "User Display Name"

"ProductName".Underscore()              // "product_name"
	.Dasherize()                        // "product-name"
	.Transform(To.LowerCase);           // "product-name"
```

## Recommendations

- **Normalize before storing:** Convert inputs to a consistent format (for example, `Kebaberize` or a custom `Transform` pipeline) to avoid duplicates.
- **Preserve acronyms when needed:** `Humanize()` leaves all-caps strings intact; combine with `Transform` for custom casing rules.
- **Localize where appropriate:** Some helpers accept `CultureInfo` to respect language-specific title casing rules.

See [String Transformations](string-transformations.md) for composing custom `IStringTransformer` pipelines.
