# fn-1-locale-translation-parity-across-all.2 Add ordinal.date + clock YAML — Germanic locales

## Description
Design and implement the unified `phrase-clock` engine, migrate existing residual leaves (German, French, Japanese) + default English converter into it, then validate with Germanic locales. Also add ordinal.date/dateOnly YAML for Germanic locales.

This is the foundational task — the single engine must absorb all existing clock patterns before other locales adopt it.

**Size:** M (engine design is the bulk; locale YAML is formulaic once engine works)
**Files:**
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationProfileCatalogInput.cs` — extend for new YAML fields
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationEngineContractFactory.cs` — engine contract
- `src/Humanizer.SourceGenerators/Common/ProfileDefinitions.cs` — profile definition
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseClockNotationConverter.cs` — new unified converter (replaces PhraseHourClockNotationConverter + RelativeHourClockNotationConverter)
- `src/Humanizer/Localisation/TimeToClockNotation/GermanTimeOnlyToClockNotationConverter.cs` — DELETE after migration
- `src/Humanizer/Localisation/TimeToClockNotation/FrenchTimeOnlyToClockNotationConverter.cs` — DELETE after migration
- `src/Humanizer/Localisation/TimeToClockNotation/JapaneseTimeOnlyToClockNotationConverter.cs` — DELETE after migration
- `src/Humanizer/Localisation/TimeToClockNotation/DefaultTimeOnlyToClockNotationConverter.cs` — DELETE after migration
- `src/Humanizer/Locales/de.yml`, `fr.yml`, `ja.yml` — migrate clock YAML to `phrase-clock` engine
- `src/Humanizer/Locales/en.yml`, `nl.yml`, `af.yml`, `da.yml`, `is.yml`, `sv.yml` — add ordinal.date/dateOnly + clock
- `src/Humanizer/Locales/ca.yml`, `es.yml`, `pt.yml`, `pt-BR.yml` — migrate from `relative-hour`/`phrase-hour` to `phrase-clock`

## Approach

### Unified `phrase-clock` engine design

One engine, one runtime converter class, all locale data in YAML compiled to static profiles.

**YAML schema** (source generator compiles these to a profile struct):
```yaml
clock:
  engine: 'phrase-clock'
  hourMode: 'h12'        # h12 | h24 | numeric
  hourGender: 'Feminine'  # for ToWords()
  # Structural words (all optional, empty = omitted)
  connector: ''           # between hour and minutes
  hourPrefix: ''          # before hour (e.g., "pukul", "ora")
  hourSuffix: ''          # after hour (e.g., "uur", "Uhr", "時")
  hourSuffixSingular: ''  # for hour == 1 (e.g., French "heure")
  hourSuffixPlural: ''    # for hour > 1 (e.g., French "heures")
  minuteSuffix: ''        # after minutes (e.g., "perc", "分")
  zeroFiller: ''          # for minutes < 10 (e.g., "noll", "零")
  # Day periods (optional — enables relative-hour behavior)
  dayPeriods:
    earlyMorning: ''
    morning: ''
    afternoon: ''
    night: ''
  dayPeriodPosition: 'suffix'  # prefix | suffix
  # Fixed phrases
  midnight: ''
  midday: ''
  # Minute-bucket overrides (template strings, {hour}/{nextHour}/{minutes} placeholders)
  min0: ''       # on the hour
  min5: ''       # five past
  min10: ''
  min15: ''      # quarter past
  min20: ''
  min25: ''      # five before half (German)
  min30: ''      # half past
  min35: ''      # five after half (German)
  min40: ''      # twenty to
  min45: ''      # quarter to
  min50: ''      # ten to
  min55: ''      # five to
  min60: ''      # next hour (rounding rollover)
  # Range defaults (for non-bucketed minutes)
  defaultTemplate: '{hour} {minutes}'
```

**Runtime converter** — single `PhraseClockNotationConverter` class:
1. Round minutes if needed
2. Check midnight/noon → return fixed phrase
3. Resolve hour words (h12/h24/numeric mode + gender)
4. Check minute-bucket map → return formatted template
5. Fall to defaultTemplate
6. If dayPeriods configured: resolve period and apply position
7. All string data from static profile — zero per-call allocation

**Leaf migrations:**
- **German** (`de.yml`): `hourMode: h12`, `hourSuffix: 'Uhr'`, full min0-min60 bucket map with "fünf nach {hour}", "fünf vor halb {nextHour}", etc.
- **French** (`fr.yml`): `hourMode: h24`, `hourSuffixSingular: 'heure'`, `hourSuffixPlural: 'heures'`, `hourGender: Feminine`
- **Japanese** (`ja.yml`): `hourMode: numeric`, `hourSuffix: '時'`, `minuteSuffix: '分'`
- **English** (`en.yml`): `hourMode: h12`, standard bucket phrases ("five past {hour}", "twenty-five past {hour}", etc.)
- **Catalan** (`ca.yml`): migrate from `relative-hour` → `phrase-clock` with dayPeriods
- **Spanish** (`es.yml`): migrate from `relative-hour` → `phrase-clock` with dayPeriods
- **Portuguese** (`pt.yml`, `pt-BR.yml`): migrate from `phrase-hour` → `phrase-clock`

After migration: delete `GermanTimeOnlyToClockNotationConverter.cs`, `FrenchTimeOnlyToClockNotationConverter.cs`, `JapaneseTimeOnlyToClockNotationConverter.cs`, `DefaultTimeOnlyToClockNotationConverter.cs`, `PhraseHourClockNotationConverter.cs`, `RelativeHourClockNotationConverter.cs` and their profile classes.

### Germanic ordinal.date/dateOnly

Add `ordinal.date` + `ordinal.dateOnly` YAML using existing `pattern` engine for: en, nl, af, da, is, sv.

## Investigation targets

**Required:**
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseHourClockNotationConverter.cs` — existing engine to replace
- `src/Humanizer/Localisation/TimeToClockNotation/RelativeHourClockNotationConverter.cs` — existing engine to replace
- `src/Humanizer/Localisation/TimeToClockNotation/GermanTimeOnlyToClockNotationConverter.cs` — migrate to YAML
- `src/Humanizer/Localisation/TimeToClockNotation/FrenchTimeOnlyToClockNotationConverter.cs` — migrate to YAML
- `src/Humanizer/Localisation/TimeToClockNotation/JapaneseTimeOnlyToClockNotationConverter.cs` — migrate to YAML
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationEngineContractFactory.cs` — engine contracts
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations

**Optional:**
- `src/Humanizer/Localisation/TimeToClockNotation/LuxembourgishTimeOnlyToClockNotationConverter.cs` — reference for task .3 lb migration
- `src/Humanizer/Locales/pt.yml:813-830` — existing phrase-hour YAML
- `src/Humanizer/Locales/es.yml:980-1000` — existing relative-hour YAML

## Key context

Performance: profile data must be static. The source generator should emit profile fields as constants. The runtime converter switch should be branchless where possible — no dictionary lookups, no per-call string building beyond the final interpolation.
## Approach

### Engine A: `phrase-clock`

Generalize the existing `phrase-hour` engine to support ALL non-day-period clock patterns through YAML config. Key additions:
- `hourMode`: `h12` (default) or `h24` — controls whether hour is mod-12 or raw 24h
- `hourGender`: gender for `ToWords()` (default: Feminine)
- `connector`: word between hour-words and minute-words (empty = direct concatenation)
- `hourPrefix`: word before hour-words (e.g., "ora")
- `hourSuffix`: word after hour-words (e.g., "uur", "saat")
- `minuteSuffix`: word after minute-words (e.g., "perc", "menit")
- `zeroFiller`: word inserted when minutes < 10 instead of plain zero (e.g., "noll", "null")
- Existing bucketed templates (midnight, noon, onHour, quarterPast, halfPast, etc.) remain

The source generator compiles YAML fields into a profile struct. The runtime converter is a single class with a switch on normalized minutes, then string interpolation from profile fields. All profile data is static — zero per-call allocations.

Follow existing pattern at `PhraseHourClockNotationConverter.cs` and `PhraseHourClockNotationProfile`. Evolve rather than replace — backward compatibility with existing `phrase-hour` YAML (pt, pt-BR) must be maintained.

### Engine B: `relative-clock`

Extend existing `relative-hour` engine with:
- `hourPrefix`: optional word before hour (e.g., "pukul", "ala")
- `hourSuffix`: optional word after hour (e.g., "시", "giờ")
- `minuteSuffix`: optional word after minutes (e.g., "분", "phút")
- `dayPeriodPosition`: `prefix` or `suffix` (e.g., Korean/Chinese = prefix, Vietnamese = suffix)
- Existing fields remain backward-compatible with ca, es

### Germanic locales (proof-of-concept)

Add ordinal.date/dateOnly + clock YAML for: en, nl, af, da, is, sv
- Variants (en-GB, en-IN, en-US, de-CH, de-LI) auto-inherit
- en clock uses `phrase-clock` with `hourMode: h12` (matches existing DefaultTimeOnlyToClockNotationConverter English behavior)
- da, sv use `phrase-clock` with `hourMode: h24`, `zeroFiller` for zero-pad
- nl, af use `phrase-clock` with `hourMode: h12`, `hourSuffix: 'uur'`
- is uses `phrase-clock` with `hourMode: h24`, `connector: 'og'`

## Investigation targets

**Required:**
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseHourClockNotationConverter.cs` — existing engine to extend
- `src/Humanizer/Localisation/TimeToClockNotation/RelativeHourClockNotationConverter.cs` — existing engine to extend
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationEngineContractFactory.cs` — how engines map to YAML fields
- `src/Humanizer.SourceGenerators/Common/ProfileDefinitions.cs` — profile definition classes
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — exact clock expectations for validation

**Optional:**
- `src/Humanizer/Locales/pt.yml:813-830` — existing phrase-hour YAML for backward compat reference
- `src/Humanizer/Locales/es.yml:980-1000` — existing relative-hour YAML

## Key context

Runtime performance is critical. Profile data must be static (allocated once, cached). The converter switch should be a simple normalized-minutes match with no per-call allocations. Use `string.Create` or pre-computed format strings where possible. The source generator should emit all locale-specific data as constants in the generated profile class.
## Approach

**For ordinal.date/dateOnly:** Use the `pattern` engine with appropriate `dayMode`. Derive the pattern and dayMode by matching the expected test output in `LocaleCoverageData`:
- en: `'{day} MMMM yyyy'` + `dayMode: 'Ordinal'` (produces "25th January 2022")
- nl: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` (produces "25 januari 2022")
- af: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` (produces "25 Januarie 2022")
- da: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` (produces "25. januar 2022")
- is: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` (produces "25. janúar 2022")
- sv: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` (produces "25 januari 2022")

Follow the existing pattern at `de.yml:495-500` or `fr.yml:367-370`.

**For clock:** Use `phrase-hour` engine for most Germanic locales. The test expectations in `LocaleCoverageData.TimeOnlyToClockNotation*` define the exact spoken-word output each locale must produce. Author YAML phrases to match those expectations.

Reference existing clock engines:
- `de.yml:502-503` (residual `german` engine)
- `pt.yml:813-830` (`phrase-hour` engine example)
- `es.yml:980-1000` (`relative-hour` engine example)

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — DateToOrdinalWords expected output for all locales
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — Clock notation expected output for all locales
- `src/Humanizer/Locales/de.yml:495-503` — existing ordinal.date + clock pattern for de
- `docs/locale-yaml-reference.md:671-710` — phrase-hour and relative-hour engine field reference

**Optional:**
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseHourClockNotationConverter.cs` — how phrase-hour engine works
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs` — dayMode rendering logic
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:444-487` — clock sweep test methods

## Key context

The test expectations are the source of truth for what each locale's output must be. The YAML must be authored to produce output that exactly matches the TheoryData expectations. Build the project and run sweep tests after each locale to verify.

English (`en`) is special: it currently uses `DefaultTimeOnlyToClockNotationConverter` which has English-specific spoken-word phrases hardcoded. Adding a `clock:` YAML section means the generated converter will replace the default. The YAML phrases must produce identical output to the existing default English phrases ("one twenty-three", "twenty-five past one", etc.).
## Acceptance
- [ ] Single `phrase-clock` engine implemented (source generator + runtime converter)
- [ ] German leaf migrated to `phrase-clock` YAML — `GermanTimeOnlyToClockNotationConverter.cs` deleted
- [ ] French leaf migrated to `phrase-clock` YAML — `FrenchTimeOnlyToClockNotationConverter.cs` deleted
- [ ] Japanese leaf migrated to `phrase-clock` YAML — `JapaneseTimeOnlyToClockNotationConverter.cs` deleted
- [ ] English default migrated to `phrase-clock` YAML — `DefaultTimeOnlyToClockNotationConverter.cs` deleted
- [ ] Existing `phrase-hour` locales (pt, pt-BR) migrated to `phrase-clock`
- [ ] Existing `relative-hour` locales (ca, es) migrated to `phrase-clock`
- [ ] Old engine classes deleted: `PhraseHourClockNotationConverter.cs`, `RelativeHourClockNotationConverter.cs` + profile classes
- [ ] en.yml, nl.yml, af.yml, da.yml, is.yml, sv.yml have ordinal.date, ordinal.dateOnly, and clock sections
- [ ] All sweep tests pass for en, en-GB, en-IN, en-US, nl, af, da, is, sv, de, de-CH, de-LI, fr, fr-BE, fr-CH, ja, ca, es, pt, pt-BR
- [ ] Source generator tests pass
- [ ] Zero per-call allocations in converter
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
