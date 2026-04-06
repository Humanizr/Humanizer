# Architecture

## Overview

Humanizer is a multi-targeted .NET library. Locale data is defined in YAML and transformed into C# at build time by Roslyn source generators. The shipped NuGet package contains the compiled library and Roslyn analyzers that guide consumers through API migrations.

```
                        ┌─── Library Build Time ───┐
YAML Locale Files ──> Source Generators ──> Generated C# Tables ──> Humanizer Library
                      (not shipped)                                      │
                                                    Analyzers ──> Code fixes for consumers
                                                    (shipped in NuGet package)
```

## Projects

### Humanizer (`src/Humanizer/`)

The main library. Multi-targets `net10.0`, `net8.0`, `net48`, and `netstandard2.0`. Contains:

- **Extension methods** — The public API surface (`StringHumanizeExtensions`, `DateHumanizeExtensions`, `NumberToWordsExtensions`, etc.)
- **Formatters** — Culture-specific formatting logic registered via `FormatterRegistry`
- **Converters** — Number-to-words, ordinalizer, and other converters with per-locale implementations
- **Configuration** — `Configurator` class with global settings and strategy selection

The library ships as a NuGet package with embedded analyzers (but not the source generators — those are only used at library build time).

### Source Generators (`src/Humanizer.SourceGenerators/`)

Roslyn incremental source generators used **only during the Humanizer library build** to transform YAML locale definitions into C# code. These are not shipped in the NuGet package and are not used by consumers of the library.

**Pipeline:**

1. `HumanizerSourceGenerator.Initialize()` sets up the incremental pipeline
2. YAML files from `src/Humanizer/Locales/` are read as `AdditionalText` items
3. `LocaleDefinitionFile.Create()` parses each YAML file into a structured model
4. `LocaleCatalogInput.Create()` collects all locale definitions into a catalog
5. Profile catalog generators produce C# source for each feature area:

| Generator | Output |
|-----------|--------|
| `LocalePhraseTableCatalogInput` | Phrase lookup tables (date/time humanization strings) |
| `HeadingTableCatalogInput` | Heading/compass direction tables |
| `FormatterProfileCatalogInput` | Formatter configuration per locale |
| `NumberToWordsProfileCatalogInput` | Number-to-words engine selection per locale |
| `OrdinalizerProfileCatalogInput` | Ordinalizer rules per locale |
| `OrdinalDateProfileCatalogInput` | Ordinal date formatting per locale |
| `TimeOnlyToClockNotationProfileCatalogInput` | Clock notation rules per locale |
| `WordsToNumberProfileCatalogInput` | Words-to-number parsing per locale |
| `LocaleRegistryInput` | Master locale registry (maps culture names to profiles) |
| `TokenMapWordsToNumberInput` | Token maps for words-to-number parsing |

Additionally, engine contract factories generate the wiring between locale profiles and their implementations:
- `NumberToWordsEngineContractFactory`
- `WordsToNumberEngineContractFactory`
- `TimeOnlyToClockNotationEngineContractFactory`

### Analyzers (`src/Humanizer.Analyzers/`)

Roslyn analyzers shipped inside the Humanizer NuGet package. They guide consumers through API changes:

- `NamespaceMigrationAnalyzer` — Detects old `Humanizer` namespace usage and suggests migration to v3 namespaces
- `NamespaceMigrationCodeFixProvider` — Auto-fixes namespace imports
- `WordsToNumberMigrationCodeFixProvider` — Migrates deprecated words-to-number API calls

### Benchmarks (`src/Benchmarks/`)

BenchmarkDotNet project for performance regression testing. A GitHub Actions workflow runs baseline-vs-current comparisons on schedule.

## Locale Data Flow

```
src/Humanizer/Locales/*.yml           # 62 YAML locale definitions
        │
        ▼
Source Generator (build time)          # Parses YAML, generates C# tables
        │
        ▼
Generated C# lookup tables            # In-memory at compile time (no runtime I/O)
        │
        ▼
FormatterRegistry / Converters         # Runtime dispatch by CultureInfo
        │
        ▼
Extension methods                      # Public API (e.g., "3.ToWords()" → "three")
```

Each YAML file defines a locale's capabilities: phrase tables for date/time humanization, number-to-words rules, ordinalizer patterns, heading labels, and clock notation preferences. See `docs/locale-yaml-reference.md` for the complete schema.

## Multi-Targeting Strategy

| Target | Purpose |
|--------|---------|
| `net10.0` | Latest .NET with full feature set |
| `net8.0` | LTS release support |
| `net48` | .NET Framework compatibility (no `TimeOnly`, `DateOnly`) |
| `netstandard2.0` | Broad compatibility baseline |

Conditional compilation (`#if`) handles API differences across targets. The `Polyfill` package backports newer APIs to older targets where possible.

## Build & Packaging

- **Versioning**: Nerdbank.GitVersioning (`version.json`) generates SemVer2 versions from git height
- **Strong naming**: Assembly signed with `Humanizer.snk`
- **Central package management**: `Directory.Packages.props` pins all dependency versions
- **NuGet packaging**: Multi-locale `.nuspec` files generate language-specific satellite packages
- **Code signing**: Azure Key Vault signing in CI for released packages
- **CI**: Azure Pipelines (build/test/pack/sign) + GitHub Actions (CodeQL, DevSkim, benchmarks)
