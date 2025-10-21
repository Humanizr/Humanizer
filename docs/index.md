# Humanizer Documentation

Humanizer meets all your .NET needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities.

## Getting Started

- [Installation](installation.md) - How to install and configure Humanizer
- [Quick Start Guide](quick-start.md) - Get up and running quickly

## Core Features

### String Manipulation
- [String Humanization](string-humanization.md) - Transform computerized strings to human-readable text
- [String Dehumanization](string-dehumanization.md) - Convert back to PascalCase
- [String Transformations](string-transformations.md) - Apply custom transformations with IStringTransformer
- [String Truncation](string-truncation.md) - Intelligent truncation strategies

### Enumerations
- [Enum Humanization](enum-humanization.md) - Make enums readable
- [Enum Dehumanization](enum-dehumanization.md) - Parse strings back to enums

### Date and Time
- [DateTime Humanization](datetime-humanization.md) - Relative time ("2 hours ago", "tomorrow")
- [TimeSpan Humanization](timespan-humanization.md) - Human-readable durations
- [Fluent Date API](fluent-date.md) - Readable date/time construction and manipulation
- [DateTime to Ordinal Words](datetime-ordinal-words.md) - "1st of January 2020"
- [TimeOnly to Clock Notation](timeonly-clock-notation.md) - "half past two" (.NET 6+)

### Numbers
- [Number to Words](number-to-words.md) - "123" → "one hundred twenty-three"
- [Number to Ordinal Words](number-to-ordinal-words.md) - "1" → "first"
- [Words to Number](words-to-number.md) - "forty-two" → 42
- [Ordinalization](ordinalization.md) - "1" → "1st"
- [Roman Numerals](roman-numerals.md) - Convert to/from Roman numerals
- [Metric Numerals](metric-numerals.md) - "1230" → "1.23k"
- [Number to Numbers](number-to-numbers.md) - Fluent API for large numbers
- [Tupleize](tupleize.md) - "2" → "double"

### Collections
- [Collection Humanization](collection-humanization.md) - Turn lists into "item1, item2, and item3"
- [ToQuantity](to-quantity.md) - "5 cases", "1 man", "2 men"

### Word Manipulation
- [Pluralization](pluralization.md) - Handle singular/plural forms
- [Singularization](singularization.md) - Convert plurals to singular
- [Inflector Methods](inflector-methods.md) - Pascalize, Camelize, Underscore, Kebaberize, etc.

### Specialized Features
- [ByteSize](bytesize.md) - Human-readable byte sizes
- [Heading](heading.md) - Convert headings to text
- [Time Unit Symbols](time-unit-symbols.md) - "ms", "s", "min", etc.

## Advanced Topics

- [Localization](localization.md) - Multi-language support
- [Custom Vocabularies](custom-vocabularies.md) - Add custom pluralization rules
- [Extensibility](extensibility.md) - Implement custom transformers and truncators
- [Configuration](configuration.md) - Customize Humanizer behavior

## Migration Guides

- [Migrating to v3.0](migration-v3.md) - Breaking changes and new features

## API Reference

- [Complete API Reference](api-reference.md) - Full API documentation

## Contributing

- [Contributing Guide](../CONTRIBUTING.md) - How to contribute to Humanizer
