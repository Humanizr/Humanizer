# fn-1-locale-translation-parity-across-all.5 Add ordinal.date + clock YAML — East and South Slavic locales

## Description
Add `ordinal.date`, `ordinal.dateOnly`, and `clock:` YAML sections to East and South Slavic locales using consolidated engines from task .2.

**Locales:** bg, ru, sr, sr-Latn, uk (all need both surfaces)
Note: sr and sr-Latn are standalone (no `variantOf`).

**Size:** M
**Files:** `src/Humanizer/Locales/bg.yml`, `ru.yml`, `sr.yml`, `sr-Latn.yml`, `uk.yml`

## Approach

**ordinal.date:** Use `pattern` engine. bg needs trailing "г." suffix. sr/sr-Latn need trailing period.

**clock engine assignments:**
- bg: `phrase-clock` with `hourMode: h24`, `hourSuffix`, `connector: 'и'`, `minuteSuffix` ("тринадесет часа и двадесет и три минути")
- ru: `phrase-clock` with `hourMode: h12` — simple concat ("час двадцать три")
- sr: `phrase-clock` with `hourMode: h12`, `connector: 'и'` ("један и двадесет три")
- sr-Latn: same as sr but Latin script ("jedan i dvadeset tri")
- uk: `phrase-clock` with `hourMode: h12` — simple concat ("перша двадцять три")

All values verified against `LocaleCoverageData`.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
## Approach

**For ordinal.date/dateOnly:** Expected patterns from test data:
- bg: `'{day} MMMM yyyy ''г.'''` + `dayMode: 'Numeric'` ("25 януари 2022 г.")
- ru: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 января 2022")
- sr: `'{day} MMMM yyyy''.'` + `dayMode: 'DotSuffix'` ("25. јануар 2022.")
- sr-Latn: `'{day} MMMM yyyy''.'` + `dayMode: 'DotSuffix'` ("25. januar 2022.")
- uk: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 січня 2022")

Note: bg has trailing "г." (abbreviation for "года"), sr/sr-Latn have trailing period. These need embedded literal strings in the pattern.

**For clock:** Expected output forms:
- bg: "тринадесет часа и двадесет и три минути" — structured with "часа и" and "минути"
- ru: "час двадцать три" — 12h spoken form, simple
- sr: "један и двадесет три" — 12h + "и" + minutes
- sr-Latn: "jedan i dvadeset tri" — Latin script equivalent of sr
- uk: "перша двадцять три" — 12h feminine + minutes

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations
- `src/Humanizer/Locales/sr.yml` — verify sr is standalone (no variantOf)
- `src/Humanizer/Locales/sr-Latn.yml` — verify standalone

**Optional:**
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:52` — how pattern literal strings work
- `docs/locale-yaml-how-to.md` — YAML authoring conventions
## Acceptance
- [ ] bg.yml, ru.yml, sr.yml, sr-Latn.yml, uk.yml each have ordinal.date, ordinal.dateOnly, and clock sections
- [ ] bg trailing "г." and sr/sr-Latn trailing period correct
- [ ] No new handwritten C# converter classes
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for bg, ru, sr, sr-Latn, uk
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
