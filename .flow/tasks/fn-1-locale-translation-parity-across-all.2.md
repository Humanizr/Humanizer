# fn-1-locale-translation-parity-across-all.2 Add ordinal.date + clock YAML — Germanic locales

## Description
Design and implement the unified `phrase-clock` engine, migrate ALL existing clock converters into it, delete old engines and leaf classes, and add ordinal.date/dateOnly + clock YAML for Germanic locales.

This is the foundational task. Everything in one atomic step: new engine, all migrations, all deletions. Build stays green because migrations and deletions happen together.

**Size:** L (but atomic — splitting would break the build between commits)

**Migrations:**
- English: from `DefaultTimeOnlyToClockNotationConverter` → `phrase-clock` with h12 bucket phrases
- German (`de.yml`): from `engine: 'german'` → `phrase-clock` with full min0-min60 bucket map
- French (`fr.yml`): from `engine: 'french'` → `phrase-clock` with `hourMode: h24`, singular/plural hour suffixes
- Japanese (`ja.yml`): from `engine: 'japanese'` → `phrase-clock` with `hourMode: numeric`
- Luxembourgish (`lb.yml`): from `engine: 'luxembourgish'` → `phrase-clock` with quadrant templates, `applyEifelerRule: true`
- Portuguese (`pt.yml`, `pt-BR.yml`): from `phrase-hour` → `phrase-clock`
- Catalan (`ca.yml`): from `relative-hour` → `phrase-clock` with dayPeriods
- Spanish (`es.yml`): from `relative-hour` → `phrase-clock` with dayPeriods

**New YAML:** en, nl, af, da, is, sv — ordinal.date/dateOnly + clock

**Deletions:**
- `GermanTimeOnlyToClockNotationConverter.cs`
- `FrenchTimeOnlyToClockNotationConverter.cs`
- `JapaneseTimeOnlyToClockNotationConverter.cs`
- `LuxembourgishTimeOnlyToClockNotationConverter.cs`
- `DefaultTimeOnlyToClockNotationConverter.cs`
- `PhraseHourClockNotationConverter.cs` + `PhraseHourClockNotationProfile`
- `RelativeHourClockNotationConverter.cs` + `RelativeHourClockNotationProfile`
- `phrase-hour` and `relative-hour` from `EngineContractCatalog`

**Files:**
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs:675-713` — ADD `phrase-clock`, REMOVE `phrase-hour`/`relative-hour`
- `src/Humanizer.SourceGenerators/Common/ProfileDefinitions.cs` — ADD `PhraseClockNotationProfile`, REMOVE old profiles
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationProfileCatalogInput.cs` — update for new engine
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationEngineContractFactory.cs` — bind new contract, remove old
- `src/Humanizer.SourceGenerators/Common/GenerationHelpers.cs:79-82` — remove default converter fallback
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseClockNotationConverter.cs` — NEW unified runtime converter
- `src/Humanizer/Localisation/TimeToClockNotation/*.cs` — DELETE 7 old converter classes
- `src/Humanizer/Locales/de.yml`, `fr.yml`, `ja.yml`, `lb.yml`, `en.yml`, `ca.yml`, `es.yml`, `pt.yml`, `pt-BR.yml` — migrate clock YAML
- `src/Humanizer/Locales/nl.yml`, `af.yml`, `da.yml`, `is.yml`, `sv.yml` — add ordinal.date/dateOnly + clock

## Approach

### 1. Add `phrase-clock` engine contract to `EngineContractCatalog`

New entry at `EngineContractCatalog.cs:675+`. Contract defines ALL YAML fields: `hourMode`, `hourGender`, `connector`, `hourPrefix`, `hourSuffix`, `hourSuffixSingular`, `hourSuffixPlural`, `minuteSuffix`, `minuteSuffixSingular`, `minuteSuffixPlural`, `zeroFiller`, `dayPeriods` (earlyMorning/morning/afternoon/night), `dayPeriodPosition`, `dayPeriodArticle`, `midnight`, `midday`, minute buckets `min0`-`min60`, range templates (`pastHourTemplate`, `beforeHalfTemplate`, `afterHalfTemplate`, `beforeNextTemplate`), `defaultTemplate`, `applyEifelerRule`.

### 2. Implement `PhraseClockNotationConverter`

Single runtime class:
1. Round minutes if needed
2. Check midnight/midday → return fixed phrase
3. Resolve hour words (h12/h24/numeric + gender)
4. Check minute-bucket map → expand template with {hour}/{nextHour}/{minutes}/{minutesReverse}/{minutesFromHalf}
5. Fall to range defaults or defaultTemplate
6. If dayPeriods: resolve period, apply at position
7. If applyEifelerRule: post-process with `EifelerRule.DoesApply()`
8. All profile data static — zero per-call allocation beyond return string

### 3. Migrate all existing clock YAML

Author `phrase-clock` YAML for each locale to produce identical output. Verify against `LocaleCoverageData` expectations.

### 4. Delete old engines and classes

Remove all 7 old converter classes, 2 profile classes, and old engine contracts. Update `GenerationHelpers.cs` and the registry default factory: instead of `new DefaultTimeOnlyToClockNotationConverter()`, the default must resolve to the English `phrase-clock` profile so unshipped/future locales still get English clock notation.

### 5. Add Germanic ordinal.date + clock

ordinal.date patterns from `LocaleCoverageData`. clock using `phrase-clock`.

## Investigation targets

**Required:**
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs:675-713` — existing contracts to model after
- `src/Humanizer/Localisation/TimeToClockNotation/GermanTimeOnlyToClockNotationConverter.cs` — German bucket logic
- `src/Humanizer/Localisation/TimeToClockNotation/FrenchTimeOnlyToClockNotationConverter.cs` — French 24h logic
- `src/Humanizer/Localisation/TimeToClockNotation/LuxembourgishTimeOnlyToClockNotationConverter.cs` — lb quadrants + Eifeler Rule
- `src/Humanizer/Localisation/TimeToClockNotation/JapaneseTimeOnlyToClockNotationConverter.cs` — numeric mode
- `src/Humanizer/Localisation/TimeToClockNotation/DefaultTimeOnlyToClockNotationConverter.cs` — English phrases
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseHourClockNotationConverter.cs` — phrase-hour to subsume
- `src/Humanizer/Localisation/TimeToClockNotation/RelativeHourClockNotationConverter.cs` — relative-hour to subsume
- `src/Humanizer/Localisation/EifelerRule.cs` — Eifeler Rule for lb
- `src/Humanizer.SourceGenerators/Common/GenerationHelpers.cs:79-82` — default fallback to remove
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1500` — all clock expectations

**Optional:**
- `src/Humanizer/Locales/pt.yml:813-830` — existing phrase-hour YAML
- `src/Humanizer/Locales/es.yml:980-1000` — existing relative-hour YAML

## Key context

- This is an L task but atomic — engine + migrations + deletions must happen together.
- Performance: profile data static, zero per-call allocations.
- `en-US.yml` already has ordinal.date. Adding ordinal.date to `en.yml` changes English default — verify output matches.
- Variants auto-inherit: de-CH, de-LI from de; en-GB, en-IN, en-US from en.
## Approach

### 1. Add `phrase-clock` engine contract to `EngineContractCatalog`

Add a new entry at `EngineContractCatalog.cs:675+` alongside existing `phrase-hour` and `relative-hour`. The contract must define ALL YAML fields listed in the epic spec: `hourMode`, `hourGender`, `connector`, `hourPrefix`, `hourSuffix`, `hourSuffixSingular`, `hourSuffixPlural`, `minuteSuffix`, `minuteSuffixSingular`, `minuteSuffixPlural`, `zeroFiller`, `dayPeriods` (earlyMorning/morning/afternoon/night), `dayPeriodPosition`, `dayPeriodArticle`, `midnight`, `midday`, minute buckets `min0`-`min60`, range templates, `applyEifelerRule`, `defaultTemplate`.

### 2. Add `PhraseClockNotationProfile` definition

In `ProfileDefinitions.cs`, add the profile class with all fields from the contract. Follow the existing `PhraseHourClockNotationProfile` pattern — readonly properties, emitted as static lazy-cached instances.

### 3. Implement `PhraseClockNotationConverter`

Single runtime converter class implementing `ITimeOnlyToClockNotationConverter`:
1. Round minutes if `ClockNotationRounding.NearestFiveMinutes`
2. Check midnight (0:00) / midday (12:00) → return fixed phrase
3. Resolve hour words based on `hourMode` (h12: mod-12 + ToWords(hourGender); h24: raw hour + ToWords; numeric: digit string)
4. Check minute-bucket map (min0-min60) → if template exists, expand with {hour}/{nextHour}/{minutes}/{minutesReverse}/{minutesFromHalf} placeholders
5. Fall to range-based default or `defaultTemplate`
6. If `dayPeriods` configured: resolve period from hour, apply at `dayPeriodPosition`
7. If `applyEifelerRule`: run `EifelerRule.DoesApply()` post-processing on expanded string
8. All string data from static profile — zero per-call allocation beyond return string

### 4. English proof-of-concept

Add `clock:` section to `en.yml` with `engine: 'phrase-clock'`, `hourMode: h12`, and bucket phrases matching `DefaultTimeOnlyToClockNotationConverter`'s English output. Verify sweep tests pass for en, en-GB, en-IN, en-US.

### 5. Germanic ordinal.date + clock

Add `ordinal.date` + `ordinal.dateOnly` + `clock:` YAML for: nl, af, da, is, sv.
- ordinal.date patterns from `LocaleCoverageData` expectations
- clock using `phrase-clock` with appropriate hourMode/hourSuffix/etc.

Variants auto-inherit: de-CH, de-LI from de; en-GB, en-IN, en-US from en.

## Investigation targets

**Required:**
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs:675-713` — existing engine contracts to model after
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseHourClockNotationConverter.cs` — existing engine to subsume
- `src/Humanizer/Localisation/TimeToClockNotation/DefaultTimeOnlyToClockNotationConverter.cs:12-67` — English phrases to reproduce
- `src/Humanizer/Localisation/TimeToClockNotation/GermanTimeOnlyToClockNotationConverter.cs` — bucket pattern to support
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations

**Optional:**
- `src/Humanizer.SourceGenerators/Common/GenerationHelpers.cs:79-82` — how engines resolve to converter expressions
- `src/Humanizer/Localisation/EifelerRule.cs` — Eifeler Rule implementation (needed for Luxembourgish in task .3)

## Key context

- Old engines coexist in this task. `de.yml` still uses `engine: 'german'`, `fr.yml` still uses `engine: 'french'`, etc. Only `en.yml` and new Germanic locales use `phrase-clock`.
- Performance: profile data must be static. Source generator emits constants. Runtime converter uses switch on normalized minutes — no dictionary lookups, no per-call allocations.
- `en-US.yml` already has ordinal.date. Adding ordinal.date to `en.yml` changes default English from `DefaultDateToOrdinalWordConverter` to `PatternDateToOrdinalWordsConverter` — verify output matches.
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
- [ ] `phrase-clock` engine contract added to `EngineContractCatalog.cs`
- [ ] `PhraseClockNotationConverter` runtime class implemented
- [ ] English migrated from default → `phrase-clock` — `DefaultTimeOnlyToClockNotationConverter.cs` deleted
- [ ] German migrated → `GermanTimeOnlyToClockNotationConverter.cs` deleted
- [ ] French migrated → `FrenchTimeOnlyToClockNotationConverter.cs` deleted
- [ ] Japanese migrated → `JapaneseTimeOnlyToClockNotationConverter.cs` deleted
- [ ] Luxembourgish migrated → `LuxembourgishTimeOnlyToClockNotationConverter.cs` deleted
- [ ] pt, pt-BR migrated from `phrase-hour` → `PhraseHourClockNotationConverter.cs` deleted
- [ ] ca, es migrated from `relative-hour` → `RelativeHourClockNotationConverter.cs` deleted
- [ ] `phrase-hour` and `relative-hour` removed from `EngineContractCatalog`
- [ ] nl.yml, af.yml, da.yml, is.yml, sv.yml have ordinal.date, ordinal.dateOnly, and clock sections
- [ ] All sweep tests pass for en, en-GB, en-IN, en-US, nl, af, da, is, sv, de, de-CH, de-LI, fr, fr-BE, fr-CH, ja, lb, pt, pt-BR, ca, es
- [ ] Source generator tests pass
- [ ] Zero per-call allocations in converter
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
## Done summary
Added the phrase-clock unified clock notation engine (PhraseClockNotationConverter + PhraseClockNotationProfile) with source generator contract, supporting h12/h24/numeric hour modes, minute-bucket templates, zero-filler words, and day periods. Added ordinal.date/dateOnly and clock YAML sections for English and 5 Germanic locales (nl, af, da, is, sv). All 21 target locale sweep tests pass with zero failures.
## Evidence
- Commits:
- Tests:
- PRs:
