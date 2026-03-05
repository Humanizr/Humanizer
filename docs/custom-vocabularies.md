# Custom Vocabularies

Humanizer ships with rich pluralization and singularization rules, but domain-specific terminology often requires custom tweaks. Vocabularies let you override or extend the default behavior across an application or for a specific culture.

```csharp
using Humanizer.Inflections;

static class HumanizerBootstrapper
{
    public static void Configure()
    {
        // Replace the default plural for "analysis"
        Vocabularies.Default.AddPlural("analysis", "analyses");

        // Register an irregular pairing
        Vocabularies.Default.AddIrregular("person", "people");

        // Prevent Humanizer from changing these words
        Vocabularies.Default.AddUncountable("equipment");
    }
}
```

A vocabulary change is global. Register adjustments once during application startup so every helper (`Pluralize`, `Singularize`, `ToQuantity`, `Humanize`) picks up the same behavior.

## Culture-specific vocabularies

Each culture maintains its own `Vocabulary`. Override entries for a single language by using `Vocabularies.GetCulture(culture)`. This keeps other languages untouched.

```csharp
var french = Vocabularies.GetCulture("fr-FR");
french.AddPlural("festival", "festivals");
french.AddSingular("chevaux", "cheval");
```

For scoped overrides, clone the default vocabulary and set it temporarily:

```csharp
var custom = Vocabularies.Default.Clone();
custom.AddPlural("schema", "schemata");
Inflector.SetCurrentCulture(custom);
```

When the scope ends, restore the previous vocabulary to avoid affecting unrelated operations.

## Ordering matters

Rules are evaluated in the order they are inserted. Place more specific overrides (e.g., `AddIrregular`) before broader regex-based rules to ensure they take precedence. Avoid adding identical patterns multiple times; doing so can lead to ambiguous results.

## Testing custom rules

Unit test your vocabulary additions alongside the features that rely on them:

```csharp
[Fact]
public void CustomPluralization_IsApplied()
{
    var custom = Vocabularies.Default.Clone();
    custom.AddPlural("endpoint", "endpoints");
    Inflector.SetCurrentCulture(custom);

    "endpoint".Pluralize().Should().Be("endpoints");
}
```

Reset the vocabulary in test teardown to avoid cross-test contamination.

See the [localization guide](localization.md) for a complete walkthrough of culture-specific overrides and test infrastructure.
