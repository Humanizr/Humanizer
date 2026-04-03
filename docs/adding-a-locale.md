# Adding Or Updating A Locale

This document is the contributor guide for Humanizer's localization pipeline.

If you are adding a new locale or changing how an existing locale behaves, start with [Locale YAML How-To](./locale-yaml-how-to.md), then use this document for pipeline details and [Locale YAML Reference](./locale-yaml-reference.md) for the exhaustive field and strategy inventory.

## Design Goals

The localization system is intentionally opinionated.

1. Locale-owned data lives in exactly one checked-in YAML file per locale under `src/Humanizer/Locales`.
2. Shared algorithms live in runtime C# kernels under `src/Humanizer/Localisation`.
3. The source generator turns locale YAML into typed runtime registrations and typed profile objects.
4. There is no runtime YAML parsing and no runtime JSON parsing on hot paths.
5. Locale-specific leaf converters are a last resort, not the default implementation strategy.
6. Supported number locales should plan `number.words` and `number.parse` together so the locale has one consistent high-range contract.

## Repository Map

These are the files and directories you usually need to understand:

- `src/Humanizer/Locales/<locale>.yml`
  This is the source of truth for locale-owned generated behavior.
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`
  Parses locale YAML, resolves inheritance, validates feature blocks, and exposes a resolved per-locale view to the rest of the generator.
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`
  Describes how a structural engine maps locale data to a typed runtime constructor call.
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/*`
  Build the generated profile catalogs for number-to-words, words-to-number, ordinalizers, date-to-ordinal, formatters, and clock notation.
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`
  Emits the locale-to-implementation wiring.
- `src/Humanizer/Localisation/*`
  Shared runtime kernels and accepted residual locale-specific leaves.
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
   - or a residual handwritten runtime path when the locale is still a deliberate leaf
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

Every locale file does not need every surface. If a `surfaces.<surface>` block is missing, the locale inherits that surface unchanged from its parent, if it has one.

Supported number locales should author `number.words` and `number.parse` together.

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

## Step-By-Step: Add A Brand New Locale

1. Decide whether the locale is neutral or regional.
2. Choose a parent locale if the locale is a regional variant.
3. Create `src/Humanizer/Locales/<locale>.yml`.
4. Add `variantOf` if needed.
5. Add the feature blocks that the locale supports.
6. Reuse existing structural engines wherever possible.
7. Add runtime tests under `tests/Humanizer.Tests/Localisation/<culture>`.
8. Add source-generator assertions if the change alters generated profile wiring.
9. Run the validation commands in this document.

## Step-By-Step: Add A Regional Variant

1. Create `src/Humanizer/Locales/<locale>.yml`.
2. Set `variantOf` to the parent locale.
3. Add only the blocks that differ.
4. Avoid copy-pasting the full parent locale.
5. Add tests that prove the variant-specific behavior instead of retesting the entire parent locale.

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
3. package build verification
4. benchmark coverage when you touch runtime-sensitive shared kernels or registry dispatch

Recommended commands:

```powershell
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/plan-validation
pwsh ./tests/verify-packages.ps1 -PackageVersion <version> -PackagesDirectory ./artifacts/plan-validation
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
- package verification passes
- benchmark comparisons show no regression versus the chosen base

## Related Documents

- [Localization Overview](./localization.md)
- [Locale YAML How-To](./locale-yaml-how-to.md)
- [Locale YAML Reference](./locale-yaml-reference.md)
