# Configuration

Humanizer provides a centralized configuration system through the `Configurator` class. It exposes locale-aware registries for formatters, converters, and ordinalizers, as well as strategy properties for customizing date/time humanization behavior.

## Basic Usage

```csharp
using Humanizer;

// Register a custom number-to-words converter for a specific locale
Configurator.NumberToWordsConverters.Register("my-locale",
    new MyNumberToWordsConverter());

// Replace the DateTime humanization strategy
Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(0.75);

// Customize which attribute property is used for enum humanization
Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
```

All configuration should happen once at application startup, before any humanization calls.

## Registries Overview

The `Configurator` class exposes several `LocaliserRegistry<T>` properties. Each registry maps locale codes (e.g., "en", "fr", "pt-BR") to a factory that creates the appropriate implementation for that culture. When no exact match is found, the registry walks up the culture hierarchy (e.g., "fr-CA" falls back to "fr") and ultimately falls back to a default implementation.

| Registry | Interface | Purpose |
|----------|-----------|---------|
| `CollectionFormatters` | `ICollectionFormatter` | Formats lists with locale-appropriate conjunctions ("and", "und", "et") |
| `Formatters` | `IFormatter` | Formats DateTime and TimeSpan humanization output |
| `NumberToWordsConverters` | `INumberToWordsConverter` | Converts numbers to localized word representations |
| `Ordinalizers` | `IOrdinalizer` | Produces ordinal suffixes ("1st", "2nd", "3.") |
| `DateToOrdinalWordsConverters` | `IDateToOrdinalWordConverter` | Converts dates to ordinal word phrases |
| `DateOnlyToOrdinalWordsConverters` | `IDateOnlyToOrdinalWordConverter` | DateOnly variant (.NET 6+) |
| `TimeOnlyToClockNotationConverters` | `ITimeOnlyToClockNotationConverter` | Converts TimeOnly to clock notation (.NET 6+) |

## Registering Custom Implementations

Each registry supports two `Register` overloads: one that accepts an instance directly, and one that accepts a factory function receiving a `CultureInfo`.

### Instance Registration

```csharp
Configurator.CollectionFormatters.Register("pt",
    new DefaultCollectionFormatter("e"));
```

### Factory Registration

Use this when the implementation needs access to the resolved culture.

```csharp
Configurator.Formatters.Register("de",
    culture => new GermanFormatter(culture));
```

### Resolution Behavior

When Humanizer needs a localized component, the registry resolves it by:

1. Checking for an exact match on the full culture name (e.g., "pt-BR")
2. Walking up to the parent culture (e.g., "pt")
3. Falling back to the default implementation

Results are cached per culture name, so the factory runs only once per locale.

## CollectionFormatters

Controls how lists of items are joined into human-readable text. The default uses "&" as the conjunction. English uses the Oxford comma style ("item1, item2, and item3").

```csharp
// Use "y" for Spanish lists
Configurator.CollectionFormatters.Register("es",
    new DefaultCollectionFormatter("y"));
```

## Formatters

Controls the output of `DateTime.Humanize()` and `TimeSpan.Humanize()`. The default formatter handles most languages. Specialized formatters exist for languages with complex pluralization or grammatical rules (Arabic, German, Russian, etc.).

```csharp
Configurator.Formatters.Register("my-locale",
    culture => new MyCustomFormatter(culture));
```

## NumberToWordsConverters

Controls the output of `.ToWords()` and `.ToOrdinalWords()`. Each language typically has its own converter to handle grammatical gender, conjunction rules, and number grouping conventions.

```csharp
Configurator.NumberToWordsConverters.Register("my-locale",
    new MyNumberToWordsConverter());
```

## Ordinalizers

Controls how `.Ordinalize()` formats ordinal numbers. For example, English produces "1st", "2nd", "3rd", while German produces "1.", and French produces "1er", "2e".

```csharp
Configurator.Ordinalizers.Register("my-locale",
    new MyOrdinalizer());
```

## DateToOrdinalWordsConverters

Controls the output of `DateTime.ToOrdinalWords()`, which produces locale-specific date phrases.

```csharp
Configurator.DateToOrdinalWordsConverters.Register("my-locale",
    new MyDateToOrdinalWordConverter());
```

On .NET 6+, a parallel registry `DateOnlyToOrdinalWordsConverters` handles the `DateOnly` type.

## DateTime Humanize Strategies

The `DateTimeHumanizeStrategy` and `DateTimeOffsetHumanizeStrategy` properties control how `DateTime.Humanize()` and `DateTimeOffset.Humanize()` calculate relative time descriptions.

### DefaultDateTimeHumanizeStrategy

The built-in default. It uses simple threshold-based rounding to produce descriptions like "an hour ago" or "3 days ago".

### PrecisionDateTimeHumanizeStrategy

A precision-based alternative. Set a precision threshold (0.0 to 1.0) that controls when the humanizer rounds up to the next time unit.

```csharp
// With 0.75 precision, 45 minutes shows as "an hour ago"
// instead of "45 minutes ago"
Configurator.DateTimeHumanizeStrategy =
    new PrecisionDateTimeHumanizeStrategy(0.75);

Configurator.DateTimeOffsetHumanizeStrategy =
    new PrecisionDateTimeOffsetHumanizeStrategy(0.75);
```

On .NET 6+, equivalent properties exist for `DateOnly` and `TimeOnly`:

```csharp
Configurator.DateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();
Configurator.TimeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();
```

## EnumDescriptionPropertyLocator

Controls which attribute property is read when humanizing enums via `[Display]` or `[Description]` attributes. By default, Humanizer looks for a property named "Description".

```csharp
// Use the "Info" property instead of "Description"
Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
```

This must be called before any `Enum.Humanize()` call. Calling it after the locator has already been used throws an exception to prevent inconsistent behavior.

## Startup Configuration Example

Configure everything once at the start of your application.

```csharp
// Program.cs or a ModuleInitializer
Configurator.DateTimeHumanizeStrategy =
    new PrecisionDateTimeHumanizeStrategy(0.75);

Configurator.DateTimeOffsetHumanizeStrategy =
    new PrecisionDateTimeOffsetHumanizeStrategy(0.75);

Configurator.CollectionFormatters.Register("my-locale",
    new DefaultCollectionFormatter("and"));

Configurator.Formatters.Register("my-locale",
    culture => new DefaultFormatter(culture));

Configurator.NumberToWordsConverters.Register("my-locale",
    new MyNumberToWordsConverter());

Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
```

## Important Notes

- All registry registrations must happen before the registry is first used. Attempting to register after the first resolution throws an `InvalidOperationException`. This is because the registry freezes its internal dictionary on first use for thread-safe read performance.
- Strategy properties (`DateTimeHumanizeStrategy`, `DateTimeOffsetHumanizeStrategy`, etc.) can technically be reassigned at any time, but should only be set once at startup. Setting them after the application starts serving requests may produce inconsistent results in multi-threaded scenarios.
- `UseEnumDescriptionPropertyLocator` must be called before the first `Enum.Humanize()` call. Move it to your app startup or a `ModuleInitializer`.

## Related Topics

- [Extensibility](extensibility.md) - Implement custom transformers, truncators, and formatters
- [Localization](localization.md) - Multi-language support and culture handling
- [Custom Vocabularies](custom-vocabularies.md) - Extend pluralization and singularization rules
- [Enum Humanization](enum-humanization.md) - How enum description attributes are resolved
- [Number to Words](number-to-words.md) - Language-specific number formatting
- [DateTime to Ordinal Words](datetime-ordinal-words.md) - Date ordinal word conversion
