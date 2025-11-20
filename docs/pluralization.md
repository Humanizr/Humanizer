# Pluralization and Singularization

Humanizer ships with comprehensive English pluralization and singularization rules. Irregular nouns, words ending in `-y`, `-us`, or `-is`, and uncountable nouns are handled out of the box. Call the extension methods directly on strings to transform them.

```csharp
using Humanizer;

"person".Pluralize();                      // "people"
"people".Singularize();                    // "person"
"analysis".Pluralize();                    // "analyses"
"status".Pluralize(inputIsKnownToBeSingular: false); // "status"
```

## Handling uncertain plurality

If you are unsure whether an input is already plural or singular, use the `inputIsKnownToBeSingular` or `inputIsKnownToBePlural` flags. When set to `false`, Humanizer inspects the word before transforming it, preventing double transformations.

```csharp
"men".Pluralize(inputIsKnownToBeSingular: false);       // "men"
"series".Singularize(inputIsKnownToBePlural: false);   // "series"
```

## Converting quantities

Combine pluralization with `ToQuantity` to generate grammatically correct phrases that include the count.

```csharp
"case".ToQuantity(0);                 // "0 cases"
"case".ToQuantity(1);                 // "1 case"
"case".ToQuantity(2, ShowQuantityAs.Words); // "two cases"
```

## Culture-specific behavior

Most languages beyond English require a culture-specific vocabulary. Humanizer ships with many built-in vocabularies—for example, `Humanizer.Core.fr` for French. Ensure you set `CultureInfo.CurrentUICulture` or pass a culture explicitly when working across locales.

```csharp
CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
"cheval".Pluralize();   // "chevaux"
```

## Extending the vocabulary

When the defaults do not match your requirements, add custom rules with `Vocabularies.Default`. See [custom vocabularies](custom-vocabularies.md) for guidance on registering irregular forms, uncountables, and culture-specific adjustments.

## Testing recommendations

- Cover domain-specific terms with unit tests to guard against regressions during upgrades.
- Verify both pluralization and singularization to ensure round-tripping works as expected.
- Use the `[UseCulture]` test attribute (see the test suite) when validating non-English vocabularies.
