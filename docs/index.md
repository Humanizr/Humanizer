# Humanizer Documentation

Humanizer meets all your .NET needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities.

## Getting Started

- [Installation](installation.md) - How to install and configure Humanizer
- [Quick Start Guide](quick-start.md) - Get up and running quickly

## Core Features

Detailed examples for most features live in the [project README](https://github.com/Humanizr/Humanizer#features). Topic-specific guides that ship in this folder are linked below.

### String Manipulation

- [String Humanization](string-humanization.md) - Transform computerized strings to human-readable text
- [String Dehumanization](string-dehumanization.md) - Convert back to PascalCase
- [String Truncation](string-truncation.md) - Intelligent truncation strategies
- [String Transformations](https://github.com/Humanizr/Humanizer#transform-string) - Apply custom transformations with `IStringTransformer`

### Enumerations

- [Enum Humanization](https://github.com/Humanizr/Humanizer#humanize-enums) - Make enums readable
- [Enum Dehumanization](https://github.com/Humanizr/Humanizer#dehumanize-enums) - Parse strings back to enums

### Date and Time

- [DateTime Humanization](https://github.com/Humanizr/Humanizer#humanize-datetime) - Relative time ("2 hours ago", "tomorrow")
- [TimeSpan Humanization](https://github.com/Humanizr/Humanizer#humanize-timespan) - Human-readable durations
- [Fluent Date API](https://github.com/Humanizr/Humanizer#fluent-date) - Readable date/time construction and manipulation
- [DateTime to Ordinal Words](https://github.com/Humanizr/Humanizer#datetime-to-ordinal-words) - "1st of January 2020"
- [TimeOnly to Clock Notation](https://github.com/Humanizr/Humanizer#timeonly-to-clock-notation) - "half past two" (.NET 6+)

### Numbers

- [Number to Words](https://github.com/Humanizr/Humanizer#number-to-words) - "123" → "one hundred twenty-three"
- [Number to Ordinal Words](https://github.com/Humanizr/Humanizer#number-to-ordinal-words) - "1" → "first"
- [Words to Number](https://github.com/Humanizr/Humanizer#words-to-number-conversion) - "forty-two" → 42
- [Ordinalization](https://github.com/Humanizr/Humanizer#ordinalize) - "1" → "1st"
- [Roman Numerals](https://github.com/Humanizr/Humanizer#roman-numerals) - Convert to/from Roman numerals
- [Metric Numerals](https://github.com/Humanizr/Humanizer#metric-numerals) - "1230" → "1.23k"
- [Number to Numbers](https://github.com/Humanizr/Humanizer#number-to-numbers) - Fluent API for large numbers
- [Tupleize](https://github.com/Humanizr/Humanizer#tupleize) - "2" → "double"

### Collections

- [Collection Humanization](https://github.com/Humanizr/Humanizer#humanize-collections) - Turn lists into "item1, item2, and item3"
- [ToQuantity](https://github.com/Humanizr/Humanizer#toquantity) - "5 cases", "1 man", "2 men"

### Word Manipulation

- [Pluralization](https://github.com/Humanizr/Humanizer#pluralize) - Handle singular/plural forms
- [Singularization](https://github.com/Humanizr/Humanizer#singularize) - Convert plurals to singular
- [Inflector Methods](https://github.com/Humanizr/Humanizer#inflector-methods) - Pascalize, Camelize, Underscore, Kebaberize, etc.

### Specialized Features

- [ByteSize](https://github.com/Humanizr/Humanizer#bytesize) - Human-readable byte sizes
- [Heading](https://github.com/Humanizr/Humanizer#heading-to-words) - Convert headings to text
- [Time Unit Symbols](https://github.com/Humanizr/Humanizer#time-unit-to-symbol) - "ms", "s", "min", etc.

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

- [Contributing Guide](https://github.com/Humanizr/Humanizer/blob/main/.github/CONTRIBUTING.md) - How to contribute to Humanizer
