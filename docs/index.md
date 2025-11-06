# Humanizer Documentation

Humanizer turns numbers, dates, enums, quantities, and strings into human-friendly text across more than forty locales. The library ships as lightweight extension methods that you can call from any .NET application, plus extensibility points for deeper customization. Use the guides below to install the library, explore each feature area, and operate Humanizer confidently in production.

## Getting Started

- [Installation](installation.md) — Choose the right packages and configure Humanizer for your project.
- [Quick Start Guide](quick-start.md) — Learn the core string and date helpers with short examples.

## Feature Guides

- **Strings**
	- [String Humanization](string-humanization.md)
	- [String Dehumanization](string-dehumanization.md)
	- [String Transformations](string-transformations.md)
	- [String Truncation](string-truncation.md)
	- [Inflector Helpers](inflector-methods.md)
	- [Pluralization & Singularization](pluralization.md)
- **Enumerations**
	- [Enum humanization and dehumanization](enumerations.md)
- **Dates & Times**
	- [Relative time, durations, and clock notation](dates-and-times.md)
- **Numbers & Quantities**
	- [Numeric words, ordinals, metric, and Roman numerals](numbers.md)
	- [ByteSize utilities](bytesize.md)
- **Collections**
	- [Human-friendly lists](collection-humanization.md)
- **Specialized helpers**
	- [Heading conversions](heading.md)
	- [Time unit symbols](time-unit-symbols.md)

## Localization and Extensibility

- [Localization](localization.md) — Manage language-specific resources and behaviors.
- [Custom vocabularies](custom-vocabularies.md) — Override pluralization rules per culture.
- [Configuration](configuration.md) — Register strategies, formatters, and transformers.
- [Extensibility](extensibility.md) — Implement custom transformers, truncators, and converters.

## Operational Guidance

- [Application Integration](application-integration.md) — Apply Humanizer across web, desktop, background, and CLI workloads.
- [Performance and Optimization](performance.md) — Keep humanized output efficient in hot code paths.
- [Testing and Quality Assurance](testing.md) — Lock down expectations with automated tests.
- [Troubleshooting](troubleshooting.md) — Diagnose installation, localization, and configuration issues.

## Migration Support

- [Namespace Migration to v3](v3-namespace-migration.md) — Understand the breaking changes and updates introduced in the 3.x release.

## Versioned Docs

The published site always serves the latest stable release at the root path. Development snapshots live under `/main/`, and prior releases remain under `/{version}/`. See [`/versions.json`](/versions.json) to enumerate the available builds programmatically.

## API Reference

Browse the generated [Humanizer API reference](api/index.md) for XML-documented namespaces, types, and members.

## Contributing

See the [contributing guide](contributing.md) for information about building, testing, and submitting improvements.
