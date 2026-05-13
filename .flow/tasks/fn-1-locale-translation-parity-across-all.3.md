# fn-1-locale-translation-parity-across-all.3 Add ordinal.date + clock YAML — Nordic and Romance locales

## Description
Add `ordinal.date`, `ordinal.dateOnly`, and `clock:` YAML sections to Nordic and Romance locales using `phrase-clock` engine.

**Locales needing both ordinal.date + clock:** nb, it, ro
**Locales needing ordinal.date only:** pt (clock already migrated in task .2)
**Variants that auto-inherit:** nn from nb

**Size:** M
**Files:**
- `src/Humanizer/Locales/nb.yml`, `it.yml`, `ro.yml` — add ordinal.date/dateOnly + clock
- `src/Humanizer/Locales/pt.yml` — add ordinal.date/dateOnly only

## Approach

**ordinal.date:** Use `pattern` engine. Patterns from `LocaleCoverageData`:
- nb: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'`
- it: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'`
- pt: `'{day} ''de'' MMMM ''de'' yyyy'` + `dayMode: 'Numeric'`
- ro: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'`

**clock:** Use `phrase-clock` engine. All values from `LocaleCoverageData`:
- nb: `hourMode: h24`, simple concat ("tretten tjuetre")
- it: `hourMode: h12`, `connector: 'e'` ("una e ventitré")
- ro: `hourMode: h12`, `hourPrefix: 'ora'`, `connector: 'și'` ("ora unu și douăzeci și trei")
- nn auto-inherits from nb

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
## Approach

### Leaf migrations

Migrate each leaf by authoring `phrase-clock` YAML that produces identical output to the handwritten converter. Verify against `LocaleCoverageData` expectations.

**German:** `hourMode: h12`, `hourSuffix: 'Uhr'`, full minute buckets. Templates use `{hour}`, `{nextHour}` placeholders: `min5: 'fünf nach {hour}'`, `min25: 'fünf vor halb {nextHour}'`, `min30: 'halb {nextHour}'`, etc.

**French:** `hourMode: h24`, `hourGender: Feminine`, `hourSuffixSingular: 'heure'`, `hourSuffixPlural: 'heures'`. No buckets — uses `defaultTemplate`.

**Japanese:** `hourMode: numeric`, `hourSuffix: '時'`, `minuteSuffix: '分'`. Simplest migration.

**Luxembourgish:** Most complex. `hourMode: h12`, `hourGender: Feminine`, `applyEifelerRule: true`, `minuteSuffixSingular: 'Minutt'`, `minuteSuffixPlural: 'Minutten'`. Quadrant templates in range defaults + minute bucket overrides for 0, 15, 25, 30, 35, 45. If `applyEifelerRule` cannot fully handle the Eifeler Rule edge cases, document limitations and keep lb as a known exception.

### Engine migrations

**Portuguese (pt, pt-BR):** Migrate from `phrase-hour` fields to `phrase-clock` fields. Most fields map directly (midnight, midday, oclockFormat, etc.). Add any missing `phrase-clock` fields.

**Catalan, Spanish (ca, es):** Migrate from `relative-hour` fields to `phrase-clock`. Map day-period fields and article/connector fields.

### Nordic/Romance ordinal.date + clock

Add ordinal.date, ordinal.dateOnly, and clock for nb, it, ro. Use `pattern` engine for ordinal.date. Use `phrase-clock` for clock.

### Cleanup

After ALL migrations verified green, delete the 7 old converter classes, 2 old profile classes, and remove `phrase-hour`/`relative-hour` from `EngineContractCatalog`. Delete `DefaultTimeOnlyToClockNotationConverter`. Update `GenerationHelpers.cs` to remove the default converter fallback.

## Investigation targets

**Required:**
- `src/Humanizer/Localisation/TimeToClockNotation/GermanTimeOnlyToClockNotationConverter.cs` — exact German logic
- `src/Humanizer/Localisation/TimeToClockNotation/FrenchTimeOnlyToClockNotationConverter.cs` — exact French logic
- `src/Humanizer/Localisation/TimeToClockNotation/LuxembourgishTimeOnlyToClockNotationConverter.cs` — exact lb logic + EifelerRule usage
- `src/Humanizer/Localisation/TimeToClockNotation/JapaneseTimeOnlyToClockNotationConverter.cs` — exact Japanese logic
- `src/Humanizer/Localisation/EifelerRule.cs` — Eifeler Rule implementation
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1500` — all clock expectations

**Optional:**
- `src/Humanizer/Locales/pt.yml:813-830` — current phrase-hour YAML
- `src/Humanizer/Locales/es.yml:980-1000` — current relative-hour YAML
- `src/Humanizer.SourceGenerators/Common/GenerationHelpers.cs:79-82` — default converter fallback to remove
## Approach

**Luxembourgish migration** — the most complex leaf. Express its patterns in `phrase-clock` YAML:
- `hourMode: h12`, `hourGender: Feminine`
- Full minute-bucket map for 0, 15, 25, 30, 35, 45
- Range templates for non-bucketed minutes: `pastHourTemplate`, `beforeHalfTemplate`, `afterHalfTemplate`, `beforeNextTemplate`
- Eifeler rule handling: investigate whether the engine needs a dedicated field or if bucketed overrides can handle the 7-specific forms
- Singular/plural minute noun: "Minutt" (singular for 1/59) vs "Minutten" (plural)

Reference: `LuxembourgishTimeOnlyToClockNotationConverter.cs` — study the exact patterns, then express in YAML.

**Other locales:** Use `phrase-clock` engine as established in task .2.

## Investigation targets

**Required:**
- `src/Humanizer/Localisation/TimeToClockNotation/LuxembourgishTimeOnlyToClockNotationConverter.cs` — full Luxembourgish logic to migrate
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- Task .2 `phrase-clock` engine schema

**Optional:**
- `src/Humanizer/Locales/lb.yml:527-531` — current lb clock YAML
## Approach

**ordinal.date:** Use `pattern` engine with appropriate `dayMode` (derive from LocaleCoverageData expectations).

**clock engine assignments** (use engines from task .2):
- nb: `phrase-clock` with `hourMode: h24`, `zeroFiller` for zero-pad ("tretten tjuetre")
- it: `phrase-clock` with `hourMode: h12`, `connector: 'e'` ("una e ventitré")
- ro: `phrase-clock` with `hourMode: h12`, `hourPrefix: 'ora'`, `connector: 'și'` ("ora unu și douăzeci și trei")
- nn inherits from nb

All clock values MUST be verified against exact `LocaleCoverageData.TimeOnlyToClockNotation*ExpectationTheoryData` expectations.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations (exact source of truth)
- Task .2 engine implementation — `phrase-clock` and `relative-clock` YAML schema
## Approach

**For ordinal.date/dateOnly:** Use the `pattern` engine. Expected patterns from test data:
- nb: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` ("25. januar 2022")
- it: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 gennaio 2022")
- pt: `'{day} ''de'' MMMM ''de'' yyyy'` + `dayMode: 'Numeric'` ("25 de janeiro de 2022")
- ro: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 ianuarie 2022")

**For clock:** Use `phrase-hour` or `relative-hour` engine. Reference test expectations:
- nb: "tretten tjuetre" — simple hour+minutes pattern
- it: "una e ventitré" — hour + connector + minutes
- ro: "ora unu și douăzeci și trei" — "ora" prefix + hour + connector + minutes
- nn inherits from nb

Follow existing `pt.yml:813-830` for phrase-hour and `es.yml:980-1000` for relative-hour.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations
- `src/Humanizer/Locales/pt.yml:813-830` — existing phrase-hour clock YAML
- `src/Humanizer/Locales/fr.yml:367-373` — existing ordinal.date + clock for Romance

**Optional:**
- `src/Humanizer/Locales/es.yml:975-1000` — relative-hour engine example
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseHourClockNotationConverter.cs` — phrase-hour engine
## Acceptance
- [ ] nb.yml, it.yml, ro.yml have ordinal.date, ordinal.dateOnly, and clock sections
- [ ] pt.yml has ordinal.date and ordinal.dateOnly sections
- [ ] nn correctly inherits from nb
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for nb, nn, it, pt, ro
## Done summary
Added ordinal.date/dateOnly and clock YAML sections for Nordic (nb, nn) and Romance (it, ro) locales. nb uses pattern engine with DotSuffix dayMode and phrase-clock with h24/hourOneWord; it uses Numeric dayMode and h12 phrase-clock with connector; ro uses Numeric dayMode and h12 phrase-clock with prefix. nn inherits ordinal from nb and overrides hourOneWord for Nynorsk. pt already had both sections from task .2.
## Evidence
- Commits: 021003d2, 072bf6fa, 470328ba
- Tests: dotnet build src/Humanizer/Humanizer.csproj -c Release, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
- PRs: