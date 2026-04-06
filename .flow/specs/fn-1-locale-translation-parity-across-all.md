# Locale Translation Parity Across All Shipped Locales

## Overview

Achieve full locale parity for all 62 shipped YAML locale files (52 base + 10 variants). Every shipped locale must have explicit, locale-authored definitions for all 7 canonical surfaces — no fallback to default/English converters. Vocabulary/Vocabularies types are excluded.

**Current state:**
- Core surfaces (list, formatter, phrases, number.words, number.parse, ordinal.numeric, compass) are **complete** for all 62 locales.
- `ordinal.date` / `ordinal.dateOnly`: Only 13 locales define this in YAML. 49 fall back to `DefaultDateToOrdinalWordConverter`.
- `clock`: Only 8 locales define this in YAML (4 use generated engines, 4 use residual handwritten leaves). 54 fall back to `DefaultTimeOnlyToClockNotationConverter`.
- 4 residual handwritten clock converters exist: `GermanTimeOnlyToClockNotationConverter`, `FrenchTimeOnlyToClockNotationConverter`, `LuxembourgishTimeOnlyToClockNotationConverter`, `JapaneseTimeOnlyToClockNotationConverter`. These must be migrated into the consolidated engine.

## Engine Consolidation Strategy

**Guiding principle:** Fewest engines, ALL locale data source-generated from YAML, minimal runtime allocations. No residual handwritten converter classes — migrate existing leaves into the consolidated engines. No spaghetti.

### Clock: Single Generalized Engine — `phrase-clock`

One data-driven engine that handles ALL clock patterns through YAML configuration, replacing both existing engines (`phrase-hour`, `relative-hour`) and all 4 residual leaves (german, french, luxembourgish, japanese). The source generator compiles YAML into a static profile; the runtime converter is a single class with zero per-call allocations.

**Core YAML fields:**
- `hourMode`: `h12` (default), `h24`, or `numeric` (digits not words — for Japanese)
- `hourGender`: gender for `ToWords()` (Masculine/Feminine/Neuter)
- `connector`: word between hour and minute words (empty = direct concat)
- `hourPrefix`/`hourSuffix`: words around hour (e.g., "pukul", "uur", "時")
- `hourSuffixSingular`/`hourSuffixPlural`: for French-style "heure"/"heures"
- `minuteSuffix`: word after minutes (e.g., "perc", "分")
- `zeroFiller`: zero-pad word for minutes < 10 (e.g., "noll", "零")

**Day-period support** (replaces separate `relative-hour` engine):
- `dayPeriods`: { earlyMorning, morning, afternoon, night }
- `dayPeriodPosition`: `prefix` or `suffix`
- `dayPeriodArticle`: optional article word

**Full minute-bucket map** (covers German/Luxembourgish bucketed phrases):
- Explicit templates for each 5-minute increment: `min0` through `min60`
- Templates use placeholders: `{hour}`, `{nextHour}`, `{minutes}`, `{minutesReverse}` (60-min), `{minutesFromHalf}` (min-30)
- Non-bucketed minutes fall to range-based defaults

**Range-based defaults** (for Luxembourgish quadrant patterns):
- `pastHourTemplate`: minutes 1-14 default
- `beforeHalfTemplate`: minutes 16-29 default
- `afterHalfTemplate`: minutes 31-44 default
- `beforeNextTemplate`: minutes 46-59 default
- `defaultTemplate`: catch-all fallback

**Leaf migrations:**
- **French** → `phrase-clock` with `hourMode: h24`, `hourSuffixSingular: 'heure'`, `hourSuffixPlural: 'heures'`, `hourGender: Feminine`
- **German** → `phrase-clock` with `hourMode: h12`, full minute-bucket map (all 13 five-min templates), `hourSuffix: 'Uhr'`
- **Luxembourgish** → `phrase-clock` with `hourMode: h12`, quadrant templates, minute-bucket overrides, `hourGender: Feminine`
- **Japanese** → `phrase-clock` with `hourMode: numeric`, `hourSuffix: '時'`, `minuteSuffix: '分'`
- **English default** → `phrase-clock` with `hourMode: h12` and standard English bucket phrases

After migration, delete: `GermanTimeOnlyToClockNotationConverter.cs`, `FrenchTimeOnlyToClockNotationConverter.cs`, `LuxembourgishTimeOnlyToClockNotationConverter.cs`, `JapaneseTimeOnlyToClockNotationConverter.cs`, `DefaultTimeOnlyToClockNotationConverter.cs`.

### Ordinal Date

Existing `pattern` engine with `OrdinalDatePattern` + `OrdinalDateDayMode` handles all needs. No new engine required — just YAML authoring. Exception: Thai Buddhist calendar may need a small runtime change.

## Scope

- Design and implement the unified `phrase-clock` engine (source generator + runtime converter)
- Migrate all 4 existing residual leaves + default converter into `phrase-clock`
- Add `ordinal.date` + `ordinal.dateOnly` YAML sections to 49 locales
- Add `clock:` YAML sections to 54 locales
- Migrate existing `clock:` YAML from `phrase-hour`/`relative-hour` to `phrase-clock`
- Add 3 missing registry completeness tests (`#if NET6_0_OR_GREATER` guarded)
- Update documentation
- Thai ordinal.date may require runtime changes for Buddhist calendar

## Quick commands

```bash
dotnet build src/Humanizer/Humanizer.csproj -c Release
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --filter "FullyQualifiedName~LocaleRegistrySweepTests"
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --filter "FullyQualifiedName~LocaleTheoryMatrixCompletenessTests"
dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj
```

## Acceptance

- [ ] All 62 shipped locales have explicit `ordinal.date` + `ordinal.dateOnly` YAML definitions
- [ ] All 62 shipped locales have explicit `clock:` YAML definitions using the unified `phrase-clock` engine
- [ ] Single `phrase-clock` engine handles ALL clock patterns — no separate `phrase-hour`, `relative-hour`, or residual leaf converters
- [ ] All 4 residual leaf converter classes deleted (German, French, Luxembourgish, Japanese)
- [ ] `DefaultTimeOnlyToClockNotationConverter` deleted — all locales use YAML-driven converters
- [ ] All clock locale data is YAML-authored and source-generated
- [ ] Registry completeness tests cover all 8 registries (NET6+ guarded)
- [ ] All sweep tests pass
- [ ] Runtime: zero per-call allocations, profile data is static
- [ ] Source generator tests pass
- [ ] Documentation updated
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0` passes

## Early proof point

Task .2 implements the `phrase-clock` engine and validates it by migrating German, French, Japanese + adding Germanic locales. If the engine cannot cleanly express these patterns, re-evaluate before continuing.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1 | Unified phrase-clock engine | .2 | — |
| R2 | Migrate existing leaves into phrase-clock | .2 (de, fr, ja), .3 (lb migration in task covering lb locale) | — |
| R3 | All locales have ordinal.date/dateOnly YAML | .3-.9 | — |
| R4 | All locales have clock YAML | .2-.9 | — |
| R5 | Registry completeness tests | .1 | — |
| R6 | No default/English fallback | .2-.9 | — |
| R7 | Thai Buddhist calendar support | .8 | — |
| R8 | Documentation updated | .10 | — |
| R9 | Delete residual leaf classes + default converter | .2 (de, fr, ja, en default), .3 (lb) | — |
