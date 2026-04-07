# Locale Translation Parity Across All Shipped Locales

## Overview

Achieve full locale parity for all 62 shipped YAML locale files (52 base + 10 variants). Every shipped locale must have explicit, locale-authored definitions for all 7 canonical surfaces — no fallback to default/English converters. Vocabulary/Vocabularies types are excluded.

**Scope boundary:** This epic covers only the 62 shipped locales. Unshipped/future locales must still fall back to English clock notation and English ordinal dates. When `DefaultTimeOnlyToClockNotationConverter` is deleted, the registry's default factory must be updated to use the English `phrase-clock` profile so unregistered locales still get English output.

**Current state:**
- Core surfaces (list, formatter, phrases, number.words, number.parse, ordinal.numeric, compass) are **complete** for all 62 locales.
- `ordinal.date` / `ordinal.dateOnly`: Only 13 locales define this in YAML. 49 fall back to `DefaultDateToOrdinalWordConverter`.
- `clock`: Only 8 locales define this in YAML (4 use generated engines, 4 use residual handwritten leaves). 54 fall back to `DefaultTimeOnlyToClockNotationConverter`.
- 4 residual handwritten clock converters exist: `GermanTimeOnlyToClockNotationConverter`, `FrenchTimeOnlyToClockNotationConverter`, `LuxembourgishTimeOnlyToClockNotationConverter`, `JapaneseTimeOnlyToClockNotationConverter`. These must be migrated into the consolidated engine.

## Engine Consolidation Strategy

**Guiding principle:** Fewest engines, ALL locale data source-generated from YAML, minimal runtime allocations. No residual handwritten converter classes — migrate existing leaves into the consolidated engine. No spaghetti.

### Clock: Single Generalized Engine — `phrase-clock`

One data-driven engine that handles ALL clock patterns through YAML configuration. Replaces:
- `phrase-hour` engine (currently used by pt, pt-BR)
- `relative-hour` engine (currently used by ca, es)
- All 4 residual leaf converters (German, French, Luxembourgish, Japanese)
- `DefaultTimeOnlyToClockNotationConverter`

The source generator compiles YAML into a static profile; the runtime converter is a single class.

**Implementation approach:** Add `phrase-clock` as a NEW engine alongside existing engines first. Migrate locales gradually. Delete old engines only after ALL locales are migrated. This keeps the build green between tasks.

**Core YAML fields (compiled to static profile):**
- `hourMode`: `h12` (default), `h24`, or `numeric` (digits not words — for Japanese)
- `hourGender`: gender for `ToWords()` (Masculine/Feminine/Neuter)
- `connector`: word between hour and minute words (empty = direct concat)
- `hourPrefix`/`hourSuffix`: words around hour (e.g., "pukul", "uur", "時")
- `hourSuffixSingular`/`hourSuffixPlural`: for French-style "heure"/"heures"
- `minuteSuffix`: word after minutes (e.g., "perc", "分")
- `minuteSuffixSingular`/`minuteSuffixPlural`: for Luxembourgish "Minutt"/"Minutten"
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

**Eifeler Rule support** (Luxembourgish morphology):
- `applyEifelerRule`: boolean flag — runtime engine applies `EifelerRule.DoesApply()` post-processing to trim trailing 'n' from number words when the following word blocks it
- This is a runtime computation that cannot be expressed in YAML templates alone

**Leaf migrations:**
- **French** → `phrase-clock` with `hourMode: h24`, `hourSuffixSingular: 'heure'`, `hourSuffixPlural: 'heures'`, `hourGender: Feminine`
- **German** → `phrase-clock` with `hourMode: h12`, full minute-bucket map (all 13 five-min templates), `hourSuffix: 'Uhr'`
- **Luxembourgish** → `phrase-clock` with `hourMode: h12`, quadrant templates, minute-bucket overrides, `hourGender: Feminine`, `applyEifelerRule: true`, `minuteSuffixSingular: 'Minutt'`, `minuteSuffixPlural: 'Minutten'`
- **Japanese** → `phrase-clock` with `hourMode: numeric`, `hourSuffix: '時'`, `minuteSuffix: '分'`
- **English default** → `phrase-clock` with `hourMode: h12` and standard English bucket phrases

After migration, delete: `GermanTimeOnlyToClockNotationConverter.cs`, `FrenchTimeOnlyToClockNotationConverter.cs`, `LuxembourgishTimeOnlyToClockNotationConverter.cs`, `JapaneseTimeOnlyToClockNotationConverter.cs`, `DefaultTimeOnlyToClockNotationConverter.cs`, `PhraseHourClockNotationConverter.cs`, `RelativeHourClockNotationConverter.cs`.

### Ordinal Date

Existing `pattern` engine with `OrdinalDatePattern` + `OrdinalDateDayMode` handles all needs. No new engine required — just YAML authoring.

**Calendar mode:** Add an `OrdinalDateCalendarMode` enum (`Gregorian` | `Native`) and a new `calendarMode` YAML field. Default is `Gregorian` (preserving current behavior). The `OrdinalDatePattern` gains an overload/parameter that respects this mode — `Gregorian` forces `GregorianCalendar` as today, `Native` uses the culture's default calendar.

**Shipped locales with non-Gregorian default calendars** (check `LocaleCoverageData` test expectations to confirm which need `Native`):
- `th` — Thai Buddhist (year + 543) → likely `Native`
- `he` — Hebrew (different year + months) → likely `Native`
- `ar` — Um Al Qura / Hijri (Islamic calendar) → likely `Native`
- `fa` — Persian / Solar Hijri → likely `Native`
- `ja`, `ko`, `zh-Hant` — have non-Gregorian calendars available but typically default to Gregorian in practice → likely `Gregorian`

## Scope

- Design and implement the unified `phrase-clock` engine (source generator contract + runtime converter)
- Migrate all 4 existing residual leaves + default converter + 2 old engines into `phrase-clock`
- Add `ordinal.date` + `ordinal.dateOnly` YAML sections to 49 locales
- Add `clock:` YAML sections to 54 locales
- Add 3 missing registry completeness tests (`#if NET6_0_OR_GREATER` guarded)
- Update documentation (4 files: locale-yaml-reference.md, locale-yaml-how-to.md, adding-a-locale.md, ARCHITECTURE.md)
- Add `OrdinalDateCalendarMode` enum (`Gregorian` | `Native`) + `calendarMode` YAML field for Thai Buddhist calendar support

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
- [ ] Old engine classes deleted: `PhraseHourClockNotationConverter`, `RelativeHourClockNotationConverter` + profiles
- [ ] All clock locale data is YAML-authored and source-generated
- [ ] Registry completeness tests cover all 8 registries (NET6+ guarded)
- [ ] All sweep tests pass
- [ ] Runtime: zero per-call allocations beyond return string, profile data is static
- [ ] Source generator tests pass
- [ ] Documentation updated (locale-yaml-reference.md, locale-yaml-how-to.md, adding-a-locale.md, ARCHITECTURE.md)
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0` passes

## Early proof point

Task .2 implements the `phrase-clock` engine and validates it with English (bucket phrases) and German (ToWords() placeholders + half-hour bucketing). If the engine cannot cleanly express these patterns, re-evaluate before continuing.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1 | Unified phrase-clock engine | .2 | — |
| R2 | Migrate existing leaves into phrase-clock | .3 (de, fr, ja, lb) | — |
| R3 | Migrate old engines (phrase-hour, relative-hour) | .3 (pt, pt-BR, ca, es) | — |
| R4 | All locales have ordinal.date/dateOnly YAML | .2 (Germanic), .3-.9 (rest) | — |
| R5 | All locales have clock YAML | .2 (en, Germanic), .3-.9 (rest) | — |
| R6 | Registry completeness tests | .1 (after all locales done) | — |
| R7 | No default/English fallback | .2-.9 collectively | — |
| R8 | Thai Buddhist calendar support | .8 (investigate + fix) | — |
| R9 | Delete residual leaf classes + old engines | .3 (after all migrations in .2-.3) | — |
| R10 | Delete DefaultTimeOnlyToClockNotationConverter | .3 | — |
| R11 | Documentation updated | .10 | — |
