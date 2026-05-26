# Humanizer Documentation

Humanizer meets all your .NET needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities.

## Getting Started

- [Installation](installation.md) - How to install and configure Humanizer
- [Quick Start Guide](quick-start.md) - Get up and running quickly

## Core Features

Detailed examples for most features live in the [project README](../readme.md#features). Topic-specific guides that ship in this folder are linked below.

### String Manipulation

- [String Humanization](string-humanization.md) - Transform computerized strings to human-readable text
- [String Dehumanization](string-dehumanization.md) - Convert back to PascalCase
- [String Truncation](string-truncation.md) - Intelligent truncation strategies
- [String Transformations](../readme.md#transform-string) - Apply custom transformations with `IStringTransformer`

### Enumerations

- [Enum Humanization](../readme.md#humanize-enums) - Make enums readable
- [Enum Dehumanization](../readme.md#dehumanize-enums) - Parse strings back to enums

### Date and Time

- [DateTime Humanization](../readme.md#humanize-datetime) - Relative time ("2 hours ago", "tomorrow")
- [TimeSpan Humanization](../readme.md#humanize-timespan) - Human-readable durations
- [Fluent Date API](../readme.md#fluent-date) - Readable date/time construction and manipulation
- [DateTime to Ordinal Words](../readme.md#datetime-to-ordinal-words) - "1st of January 2020"
- [TimeOnly to Clock Notation](../readme.md#timeonly-to-clock-notation) - "half past two" (.NET 6+)

### Numbers

- [Number to Words](../readme.md#number-to-words) - "123" → "one hundred twenty-three"
- [Number to Ordinal Words](../readme.md#number-to-ordinal-words) - "1" → "first"
- [Words to Number](../readme.md#words-to-number-conversion) - "forty-two" → 42
- [Ordinalization](../readme.md#ordinalize) - "1" → "1st"
- [Roman Numerals](../readme.md#roman-numerals) - Convert to/from Roman numerals
- [Metric Numerals](../readme.md#metric-numerals) - "1230" → "1.23k"
- [Number to Numbers](../readme.md#number-to-numbers) - Fluent API for large numbers
- [Tupleize](../readme.md#tupleize) - "2" → "double"

### Collections

- [Collection Humanization](../readme.md#humanize-collections) - Turn lists into "item1, item2, and item3"
- [ToQuantity](../readme.md#toquantity) - "5 cases", "1 man", "2 men"

### Word Manipulation

- [Pluralization](../readme.md#pluralize) - Handle singular/plural forms
- [Singularization](../readme.md#singularize) - Convert plurals to singular
- [Inflector Methods](../readme.md#inflector-methods) - Pascalize, Camelize, Underscore, Kebaberize, etc.

### Specialized Features

- [ByteSize](../readme.md#bytesize) - Human-readable byte sizes
- [Heading](../readme.md#heading-to-words) - Convert headings to text
- [Time Unit Symbols](../readme.md#time-unit-to-symbol) - "ms", "s", "min", etc.

## Advanced Topics

- [Localization](localization.md) - Multi-language support, YAML locale data, and inheritance
- [Adding a Locale](adding-a-locale.md) - Contributor workflow for new locales
- [Locale YAML How-To](locale-yaml-how-to.md) - Practical YAML authoring guide
- [Locale YAML Reference](locale-yaml-reference.md) - Schema reference for locale files
- [Extensibility](extensibility.md) - Implement custom transformers and truncators

## Migration Guides

- [Migrating from 2.14.1 to 3.0.8](migration-v3.md) - Comprehensive breaking changes, patch-line fixes, and known regressions
- [Namespace migration details](v3-namespace-migration.md) - Namespace-only migration guidance and analyzer usage

## Contributing

- [Contributing Guide](../.github/CONTRIBUTING.md) - How to contribute to Humanizer
