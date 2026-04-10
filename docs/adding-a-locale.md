# Adding Or Updating A Locale

This document is the contributor guide for Humanizer's localization pipeline.

Together with [Locale YAML How-To](./locale-yaml-how-to.md), this document is intended to be sufficient for contributors to understand what a locale file may contain, how locale data flows through the generator, and what must be verified before locale work is complete. Contributors should not need to inspect C# source files just to discover the allowed locale shape or the required parity workflow.

Use [Locale YAML Reference](./locale-yaml-reference.md) as the field-by-field inventory after you understand the contract described here.

## Design Goals

The localization system is intentionally opinionated.

1. Locale-owned data lives in exactly one checked-in YAML file per locale under `src/Humanizer/Locales`.
2. Shared algorithms live in runtime C# kernels under `src/Humanizer/Localisation`.
3. The source generator turns locale YAML into typed runtime registrations and typed profile objects.
4. There is no runtime YAML parsing and no runtime JSON parsing on hot paths.
5. Locale-specific leaf converters are a last resort, not the default implementation strategy.
6. Shipped locales are expected to resolve every canonical surface intentionally; there is no shipped-locale exemption list in this repo.

## Repository Map

These are the files and directories you usually need to understand:

- `src/Humanizer/Locales/<locale>.yml`
  This is the source of truth for locale-owned generated behavior.
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs`
  Defines the canonical locale YAML surface: allowed top-level keys, canonical surface names, and nested canonical members such as `number.words`, `number.parse`, `number.formatting`, `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, `calendar.months`, and `calendar.monthsGenitive`.
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`
  Parses locale YAML, resolves inheritance, validates feature blocks, and exposes a resolved per-locale view to the rest of the generator.
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`
  Describes how a structural engine maps locale data to a typed runtime constructor call.
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/*`
  Build the generated profile catalogs and tables for number-to-words, words-to-number, ordinalizers, date-to-ordinal, formatters, locale phrases, compass headings, and clock notation.
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`
  Emits the locale-to-implementation wiring.
- `src/Humanizer/Localisation/*`
  Shared runtime kernels.
- `tests/Humanizer.SourceGenerators.Tests`
  Verifies generator behavior and generated source structure.
- `tests/Humanizer.Tests`
  Verifies runtime behavior for real cultures.

## The High-Level Flow

The pipeline works like this:

1. A locale YAML file declares `locale`, optional `variantOf`, and the locale's `surfaces`.
2. `LocaleYamlCatalog` reads every locale file and resolves `variantOf` inheritance.
3. Each feature block selects either:
   - a generated profile path
   - a token-map generation path
   - or (rarely) a handwritten runtime path when the locale behavior is still genuinely procedural
4. `EngineContractCatalog` tells the generator how to map structured locale data into constructor arguments for a shared runtime kernel.
5. The profile catalog generators emit typed cached profile instances.
6. `LocaleRegistryInput` emits the locale-to-implementation registrations.
7. At runtime, Humanizer resolves the locale through generated registries and shared kernels with no data parsing on the hot path.

## What Goes In The Locale YAML

A locale YAML file owns locale-specific words, switches, lists, mappings, and feature selection.

Canonical top-level keys are:

- `locale`
- `variantOf`
- `surfaces`

Supported canonical surfaces are:

- `list`
- `formatter`
- `phrases`
- `number`
- `ordinal`
- `clock`
- `compass`
- `calendar`

Every locale file does not need to author every surface directly. If a `surfaces.<surface>` block is missing, that surface must still resolve intentionally through same-language inheritance with proof. If it does not, the locale is incomplete.

Do not keep a surface block only to indicate default behavior. If a locale does not need locale-specific data for a surface or nested block, omit it entirely.

If a locale claims parity, it must explicitly account for both `number.words` and `number.parse`, either locale-owned or same-language inherited with proof.

## Canonical Locale Contract

The canonical locale shape is exact. A locale file contains:

1. `locale`
2. optional `variantOf`
3. `surfaces`

Under `surfaces`, the canonical members are exactly:

1. `list`
2. `formatter`
3. `phrases`
4. `number`
5. `ordinal`
6. `clock`
7. `compass`
8. `calendar`

The nested canonical members are:

1. `number.words`
2. `number.parse`
3. `number.formatting`
4. `ordinal.numeric`
5. `ordinal.date`
6. `ordinal.dateOnly`
7. `calendar.months`
8. `calendar.monthsGenitive`

The `phrases` surface is also structured, with these canonical members:

1. `relativeDate`
2. `duration`
3. `dataUnits`
4. `timeUnits`

If you are authoring or reviewing a locale, treat that as the full allowed locale contract. Do not invent alternate top-level keys, alternate surface names, or alternate nested names.

A locale is not complete unless every canonical surface is explicitly accounted for as locale-owned or same-language inherited with proof. There is no shipped-locale exemption list in this repo.

## Canonical Surface Responsibilities

This is what each canonical surface owns:

| Surface | Owns |
| --- | --- |
| `list` | Collection conjunction/delimiter behavior only |
| `formatter` | Formatter grammar/resource-selection metadata such as detectors, gender metadata, preposition rules, and fallback transforms |
| `phrases` | Relative date phrases, duration phrases, data-unit phrases, and time-unit phrases |
| `number.words` | Render-side number composition data |
| `number.parse` | Parse-side lexicons and normalization behavior |
| `ordinal.numeric` | Numeric ordinalization |
| `ordinal.date` | `DateTime.ToOrdinalWords` day placement/day rendering rules |
| `ordinal.dateOnly` | `DateOnly.ToOrdinalWords` day placement/day rendering rules |
| `clock` | `TimeOnly.ToClockNotation` phrase templates or clock engine selection |
| `compass` | Full and abbreviated heading/compass labels |
| `calendar` | Month-name overrides (nominative and genitive) for stable cross-platform date output |
| `number.formatting` | Decimal separator, negative sign, and group separator overrides for stable cross-platform numeric output |

Two boundaries matter:

1. `formatter` and `phrases` are separate surfaces. Formatter metadata is not a substitute for authored phrase tables.
2. `clock` is the canonical locale surface name even though generator and runtime code often refer to the emitted runtime feature as `timeOnlyToClockNotation`.

## Locale File Example

This is the shape of a typical locale file:

```yaml
locale: 'en-US'
variantOf: 'en'

surfaces:
  list:
    engine: 'oxford'

  number:
    words:
      engine: 'conjunctional-scale'
      minusWord: 'minus'
      andWord: 'and'
      unitsMap:
        - 'zero'
        - 'one'
        - 'two'
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
        two: 2
        hundred: 100
```

Rules for authoring:

1. Put locale words in YAML, not in the generator.
2. Prefer inheritance over duplication.
3. Prefer explicit structural engine names over generic flags.
4. Keep the block structural. If the values describe reusable rules, that locale likely belongs on a shared kernel.
5. If you find yourself wanting imperative hooks, stop and decide whether the locale truly needs a residual leaf.

## Canonical Locale Skeleton

Use this as the complete structural skeleton for a locale file. Every block is optional except `locale` and `surfaces`, but no other top-level or surface names are valid.

```yaml
locale: '<locale>'
variantOf: '<parent-locale>'

surfaces:
  list:
    engine: '<list-engine>'

  formatter:
    engine: 'profiled'

  phrases:
    relativeDate:
      now: '<text>'
      never: '<text>'
      past: {}
      future: {}
    duration:
      zero: '<text>'
      age:
        template: '{value}'
    dataUnits: {}
    timeUnits: {}

  number:
    words:
      engine: '<number-to-words-engine>'
    parse:
      engine: '<words-to-number-engine>'
    formatting:
      decimalSeparator: '<separator>'
      negativeSign: '<sign>'
      groupSeparator: '<separator>'

  ordinal:
    numeric:
      engine: '<ordinal-engine>'
    date:
      pattern: '<pattern-with-{day}>'
      dayMode: '<day-mode>'
    dateOnly:
      pattern: '<pattern-with-{day}>'
      dayMode: '<day-mode>'

  clock:
    engine: '<clock-engine>'

  calendar:
    months:
      - '<January>'
      - '<February>'
      - '<March>'
      - '<April>'
      - '<May>'
      - '<June>'
      - '<July>'
      - '<August>'
      - '<September>'
      - '<October>'
      - '<November>'
      - '<December>'
    monthsGenitive:
      - '<January-genitive>'
      - '<February-genitive>'
      - '<March-genitive>'
      - '<April-genitive>'
      - '<May-genitive>'
      - '<June-genitive>'
      - '<July-genitive>'
      - '<August-genitive>'
      - '<September-genitive>'
      - '<October-genitive>'
      - '<November-genitive>'
      - '<December-genitive>'

  compass:
    full: []
    short: []
```

## Inheritance Rules

Inheritance is resolved per locale file, not per feature file, because there is only one file per locale.

Rules:

1. `variantOf` points to the parent locale.
2. Omitting a `surfaces.<surface>` block inherits it unchanged from the parent locale.
3. Scalar overrides replace the inherited scalar.
4. Sequence overrides replace the inherited sequence.
5. Mapping overrides merge recursively with the inherited mapping.
6. Changing `engine` inside a child mapping replaces that mapping instead of merging it.
7. Regional variants should usually inherit from the neutral locale unless the repo already uses a different established parent.
8. Locale YAML should not reference internal generated profile identifiers. Use locale-facing values like `self` when the generator supports them.
9. Inheritance is not self-proving. A parity claim still needs at least one locale-specific proving assertion for every inherited canonical surface, and the proof must record the full inheritance chain to the terminal owner.

Examples:

- `en-US` uses `variantOf: 'en'`
- `fr-BE` uses `variantOf: 'fr'`
- `de-CH` uses `variantOf: 'de'`

## When To Reuse An Existing Engine

Reuse an existing engine contract when:

1. The runtime algorithm already matches the new locale's composition rules.
2. The differences are lexical, list-based, or strategy-based.
3. The existing runtime kernel does not need locale-specific hard-coded branches to support the new locale.

## When To Add A New Engine Contract

Add a new engine contract only when:

1. The behavior is structural and reusable.
2. It clearly applies to at least two locales, or to one locale plus an obvious second target already exists.
3. The runtime kernel can stay generic once the locale-owned words and switches are passed in.
4. The data shape is still coherent and not an exception bucket.

When you add a new engine contract, you usually touch:

1. `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`
2. the relevant profile catalog generator in `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs`
3. a shared runtime kernel under `src/Humanizer/Localisation`
4. source-generator tests
5. runtime tests
6. benchmark coverage if the runtime path is hot

## When A Residual Locale Leaf Is Acceptable

A residual locale-specific converter is acceptable only when at least one of these is true:

1. The logic is still genuinely procedural rather than declarative.
2. The morphology would force imperative hooks into a shared kernel.
3. There is no second locale that can share the abstraction yet.
4. Generalizing it would add more complexity than value.

If a runtime kernel still hard-codes language-specific behavior, do not pretend it is generic. Either make it structurally generic for real or keep the locale name.

Note: as of the locale parity completion, no clock residual leaves remain. All 62 shipped locales use the unified `phrase-clock` engine for clock notation.

## Step-By-Step: Add A Brand New Locale

1. Decide whether the locale is neutral or regional.
2. Choose a parent locale if the locale is a regional variant.
3. Produce a preflight gap report covering every canonical surface.
4. Create `src/Humanizer/Locales/<locale>.yml`.
5. Add `variantOf` if needed.
6. Add or prove every canonical surface through locale ownership or same-language inheritance with proof.
7. Reuse existing structural engines wherever possible.
8. Add runtime tests under `tests/Humanizer.Tests/Localisation/<culture>`.
9. Add source-generator assertions if the change alters generated profile wiring.
10. Maintain a parity map artifact until the unresolved set is empty.
11. Run the validation commands in this document.

## Step-By-Step: Add A Regional Variant

1. Create `src/Humanizer/Locales/<locale>.yml`.
2. Set `variantOf` to the parent locale.
3. Add only the blocks that differ.
4. Avoid copy-pasting the full parent locale.
5. Add proving assertions for every inherited canonical surface, not just the fields that differ.
6. Record the full inheritance chain to the terminal owner in the parity artifact.

## Step-By-Step: Migrate A Locale Off A Leaf Converter

1. Study the current converter and isolate the actual rule family.
2. Check whether another locale already shares that behavior.
3. Move the locale-owned words, switches, and mappings into the locale YAML.
4. Add or extend the shared runtime kernel if necessary.
5. Add or extend the engine contract in `EngineContractCatalog.cs`.
6. Update the profile generator to emit the shared kernel path.
7. Remove the locale leaf only after tests and benchmarks prove parity.

## Testing Requirements

Every functional localization change should include:

1. generator coverage in `tests/Humanizer.SourceGenerators.Tests`
2. runtime behavior coverage in `tests/Humanizer.Tests`
3. `dotnet pack` verification for the main package
4. benchmark coverage when you touch runtime-sensitive shared kernels or registry dispatch

## Mandatory Parity Workflow

If the task is locale parity work, the workflow is stricter than "edit YAML and run tests."

You must:

1. produce a preflight gap report for every canonical surface
2. maintain a parity map artifact under `artifacts/`
3. keep an effective-gap summary and drive it to empty
4. record a before/after parity delta and drive the final unresolved set to empty
5. add a closeout line for every canonical surface with ownership path and proof

If you cannot produce an empty unresolved set for the locale, you must report `parity not complete`.

Recommended commands:

```powershell
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/plan-validation
```

When you change hot-path shared kernels, also run the relevant benchmark filters in `src/Benchmarks`.

## Performance Rules

These are non-negotiable:

1. No runtime YAML parsing.
2. No runtime JSON parsing.
3. No reflection-based locale dispatch.
4. Generated registries should resolve directly to cached objects or shared kernels.
5. New shared kernels must benchmark at parity or better against the baseline they replace.

## Contributor Checklist

Before you call the work done, verify all of these:

- the locale is represented by exactly one YAML file
- inheritance is correct
- no locale-owned words were left hard-coded in the generator
- any new shared kernel uses a structural name
- any remaining locale leaf is explicitly justified
- source-generator tests pass
- runtime tests pass on `net10.0` and `net8.0`
- registry completeness tests pass (`LocaleRegistrySweepTests` and `LocaleTheoryMatrixCompletenessTests`)
- `dotnet pack` passes
- benchmark comparisons show no regression versus the chosen base
- verify that `DateTime.ToOrdinalWords()`, `DateOnly.ToOrdinalWords()`, `ByteSize`, `Ordinalize`, and `MetricNumeralExtensions` output for your locale is byte-identical across platforms and target frameworks (including net48/NLS vs net8+/ICU on Windows); if platform globalization data (month names, decimal separators, negative signs, group separators) disagrees, author explicit overrides in `calendar:` and/or `number.formatting:` rather than relying on `CultureInfo`

## Related Documents

- [Localization Overview](./localization.md)
- [Locale YAML How-To](./locale-yaml-how-to.md)
- [Locale YAML Reference](./locale-yaml-reference.md)
