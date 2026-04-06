# fn-1-locale-translation-parity-across-all.3 Add ordinal.date + clock YAML — Nordic and Romance locales

## Description
Add `ordinal.date`, `ordinal.dateOnly`, and `clock:` YAML to Nordic and Romance locales. Migrate Luxembourgish residual leaf into `phrase-clock` engine.

**Locales needing both ordinal.date + clock:** nb, it, ro
**Locales needing ordinal.date only:** pt (clock already migrated in task .2)
**Luxembourgish migration:** lb already has clock YAML with `engine: 'luxembourgish'` — migrate to `phrase-clock` with quadrant templates and delete `LuxembourgishTimeOnlyToClockNotationConverter.cs`
**Variants that auto-inherit:** nn from nb; fr-BE, fr-CH from fr

**Size:** M
**Files:**
- `src/Humanizer/Locales/nb.yml`, `it.yml`, `pt.yml`, `ro.yml`
- `src/Humanizer/Locales/lb.yml` — migrate clock from `luxembourgish` to `phrase-clock`
- `src/Humanizer/Localisation/TimeToClockNotation/LuxembourgishTimeOnlyToClockNotationConverter.cs` — DELETE after migration

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
- [ ] lb.yml clock migrated from `luxembourgish` engine to `phrase-clock` with quadrant templates
- [ ] `LuxembourgishTimeOnlyToClockNotationConverter.cs` deleted
- [ ] nn correctly inherits from nb
- [ ] No handwritten converter classes remain (all 4 leaves + default now deleted)
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for nb, nn, it, pt, ro, lb, fr-BE, fr-CH
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
