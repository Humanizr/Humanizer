# Custom Vocabularies

Humanizer's pluralization and singularization engine is driven by a vocabulary of rules, irregular words, and uncountable nouns. The default vocabulary covers standard US English, but you can extend it to handle domain-specific terms, jargon, or words that the built-in rules get wrong.

## Basic Usage

```csharp
using Humanizer;

// Add an irregular word pair
Vocabularies.Default.AddIrregular("person", "people");

// Add an uncountable word
Vocabularies.Default.AddUncountable("equipment");

// Add a custom plural rule
Vocabularies.Default.AddPlural("bus", "buses");

// Add a custom singular rule
Vocabularies.Default.AddSingular("(vert|ind)ices$", "$1ex");
```

After adding rules, all calls to `.Pluralize()` and `.Singularize()` will pick them up automatically.

## The Default Vocabulary

`Vocabularies.Default` is the single built-in vocabulary instance. It is lazily initialized and comes pre-loaded with a comprehensive set of English rules covering:

- Common plural patterns (e.g., adding "s", "es", "ies")
- Common singular patterns (e.g., removing "s", "es", "ies")
- Irregular words (e.g., "person"/"people", "child"/"children", "goose"/"geese")
- Uncountable words (e.g., "equipment", "information", "fish", "sheep")

You extend this vocabulary by calling its methods at application startup, before any pluralization or singularization occurs.

## AddIrregular

Use `AddIrregular` for words that do not follow standard English pluralization patterns.

```csharp
Vocabularies.Default.AddIrregular("octopus", "octopi");

"octopus".Pluralize()   // => "octopi"
"octopi".Singularize()  // => "octopus"
```

A single call registers both the plural and singular forms. You do not need to call `AddPlural` and `AddSingular` separately.

### The matchEnding Parameter

By default, `matchEnding` is `true`. This means the rule also applies when the word appears as the ending of a longer word.

```csharp
// matchEnding: true (default) — matches "person" and "salesperson"
Vocabularies.Default.AddIrregular("person", "people");

"person".Pluralize()       // => "people"
"salesperson".Pluralize()  // => "salespeople"
```

Set `matchEnding: false` to match only the exact word.

```csharp
// matchEnding: false — matches only "person", not "salesperson"
Vocabularies.Default.AddIrregular("person", "people", matchEnding: false);

"person".Pluralize()       // => "people"
"salesperson".Pluralize()  // => "salespersons" (default "s" rule applies)
```

This is important for short words that commonly appear as suffixes. Several built-in irregulars use `matchEnding: false` to avoid unintended matches, including "is"/"are", "this"/"these", "bus"/"buses", and "die"/"dice".

## AddPlural

Use `AddPlural` when a word follows a pattern that the default rules miss. The first argument is a regex pattern and the second is a regex replacement string.

```csharp
Vocabularies.Default.AddPlural("(quiz)$", "$1zes");

"quiz".Pluralize()  // => "quizzes"
```

Patterns are matched case-insensitively. Later rules take priority over earlier ones, so your custom rules override the defaults.

## AddSingular

Use `AddSingular` the same way as `AddPlural`, but for converting plural forms back to singular.

```csharp
Vocabularies.Default.AddSingular("(vert|ind)ices$", "$1ex");

"vertices".Singularize()  // => "vertex"
"indices".Singularize()   // => "index"
```

## AddUncountable

Use `AddUncountable` for words that should remain unchanged regardless of plurality. These words are returned as-is by both `Pluralize` and `Singularize`.

```csharp
Vocabularies.Default.AddUncountable("metadata");

"metadata".Pluralize()    // => "metadata"
"metadata".Singularize()  // => "metadata"
```

The default vocabulary already includes common uncountable words like "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer", and "aircraft".

## Practical Examples

### Domain-Specific Terminology

```csharp
// Medical terms
Vocabularies.Default.AddIrregular("diagnosis", "diagnoses");
Vocabularies.Default.AddIrregular("appendix", "appendices");
Vocabularies.Default.AddUncountable("plasma");

// Gaming terms
Vocabularies.Default.AddIrregular("die", "dice", matchEnding: false);
Vocabularies.Default.AddUncountable("ammo");
```

### Application Startup Configuration

Register all custom vocabulary rules once at startup, before any humanization calls.

```csharp
// In Program.cs or a startup initializer
Vocabularies.Default.AddIrregular("stadium", "stadia");
Vocabularies.Default.AddIrregular("antenna", "antennae");
Vocabularies.Default.AddUncountable("feedback");

// Now safe to use throughout the application
"stadium".Pluralize()  // => "stadia"
"feedback".Pluralize() // => "feedback"
```

## Important Notes

- Custom rules are added to the end of the rule list and take priority over earlier rules.
- The vocabulary comparison is case-insensitive. Adding a rule for "person" also handles "Person" and "PERSON".
- Only a single vocabulary (`Vocabularies.Default`) is supported. Multiple independent vocabularies and removing existing rules are not available.
- Vocabulary rules should be added at application startup before any calls to `Pluralize` or `Singularize`.

## Related Topics

- [Pluralization](pluralization.md) - Singular to plural conversion
- [Singularization](singularization.md) - Plural to singular conversion
- [ToQuantity](to-quantity.md) - Combine numbers with correctly pluralized words
- [Extensibility](extensibility.md) - Other ways to customize Humanizer
- [Configuration](configuration.md) - Global configuration options
