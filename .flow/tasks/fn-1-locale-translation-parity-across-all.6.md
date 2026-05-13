# fn-1-locale-translation-parity-across-all.6 Add ordinal.date + clock YAML — Finno-Ugric, Turkic, Greek, Armenian locales

## Description
Add `ordinal.date`, `ordinal.dateOnly`, and `clock:` YAML sections to Finno-Ugric, Turkic, Greek, and Armenian locales.

**Locales:** fi, hu, az, tr, el, hy (all need both surfaces)

**Size:** M
**Files:** `src/Humanizer/Locales/fi.yml`, `hu.yml`, `az.yml`, `tr.yml`, `el.yml`, `hy.yml`

## Approach

**ordinal.date:** Use `pattern` engine. Hungarian is unique (year-first with dots).
- fi: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'`
- hu: year-first format + `dayMode: 'Numeric'` (e.g., "2022. január 25.")
- az: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'`
- tr: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'`
- el: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'`
- hy: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'`

**clock:** Use `phrase-clock` engine. el and hy may need dayPeriods (relative-hour style).
- fi: `hourMode: h24`, simple concat
- hu: `hourMode: h24`, `hourSuffixSingular: 'óra'`/`hourSuffixPlural: 'óra'`, `minuteSuffixSingular: 'perc'`/`minuteSuffixPlural: 'perc'`
- az: `hourMode: h24`, `hourSuffixSingular: 'saat'`/`hourSuffixPlural: 'saat'`, `minuteSuffixSingular: 'dəqiqə'`/`minuteSuffixPlural: 'dəqiqə'`
<!-- Updated by plan-sync: fn-1.5 used hourSuffixSingular/hourSuffixPlural and minuteSuffixSingular/minuteSuffixPlural, not hourSuffix/minuteSuffix -->
- tr: `hourMode: h24`, simple concat
- el: `hourMode: h12`, dayPeriods (το πρωί/το απόγευμα/etc.), `connector: 'και'`
- hy: `hourMode: h12`, connector + minutes pattern

All values MUST match `LocaleCoverageData` expectations exactly.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- `src/Humanizer/Locales/ja.yml:290-296` — Japanese year-first ordinal.date as reference for Hungarian
## Approach

**For ordinal.date/dateOnly:** Expected patterns:
- fi: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` ("25. tammikuuta 2022")
- hu: year-first with dots + `dayMode: 'Numeric'` ("2022. január 25.") — unique Hungarian format
- az: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 yanvar 2022")
- tr: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Ocak 2022")
- el: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Ιανουαρίου 2022")
- hy: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 հունվարի 2022")

Note: Hungarian has a unique date format with year-first and trailing dots. The pattern needs careful construction.

**For clock:** Derive exact expectations from `LocaleCoverageData.TimeOnlyToClockNotation*ExpectationTheoryData`. All expected values MUST be sourced directly from the test data, not approximated. Key patterns:
- fi: "kolmetoista kaksikymmentäkolme" — simple 24h word concatenation
- hu: "tizenhárom óra huszonhárom perc" — 24h + "óra" + minutes + "perc"
- az: "on üç saat iyirmi üç dəqiqə" — 24h + "saat" + minutes + "dəqiqə"
- tr: "on üç yirmi üç" — simple concatenation
- el: "μία και είκοσι τρία το απόγευμα" — 12h + "και" + minutes + day-period (relative-hour)
- hy: "մեկն անց քսաներեք" — 12h + "անց" (past) + minutes (read directly from LocaleCoverageData)

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations (exact source of truth)
- `docs/locale-yaml-reference.md:671-710` — clock engine field reference

**Optional:**
- `src/Humanizer/Locales/ja.yml:290-296` — Japanese year-first ordinal.date pattern reference
- `src/Humanizer/Locales/es.yml:980-1000` — relative-hour clock example for el/hy
## Approach

**For ordinal.date/dateOnly:** Expected patterns:
- fi: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` ("25. tammikuuta 2022")
- hu: `'yyyy''. ''MMMM'' ''{day}''.'` + `dayMode: 'Numeric'` ("2022. január 25.") — year-first with dots
- az: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 yanvar 2022")
- tr: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Ocak 2022")
- el: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Ιανουαρίου 2022")
- hy: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 հունվարի 2022")

Note: Hungarian has a unique date format with year-first and trailing dots. The pattern needs careful construction.

**For clock:** Expected output:
- fi: "kolmetoista kaksikymmentäkolme" — simple 24h word concatenation
- hu: "tizenhárom óra huszonhárom perc" — 24h + "óra" + minutes + "perc"
- az: "on üç saat iyirmi üç dəqiqə" — 24h + "saat" + minutes + "dəqiqə"
- tr: "on üç yirmi üç" — simple concatenation
- el: "μία και είκοσι τρία το απόγευμα" — 12h + "και" + minutes + day-period (relative-hour)
- hy: "մեկն անdelays քdelays" — uses "անpoints" pattern ("past" + minutes)

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations
- `docs/locale-yaml-reference.md:671-710` — clock engine field reference

**Optional:**
- `src/Humanizer/Locales/ja.yml:290-296` — Japanese year-first ordinal.date pattern reference
- `src/Humanizer/Locales/es.yml:980-1000` — relative-hour clock example for el/hy
## Acceptance
- [ ] fi.yml, hu.yml, az.yml, tr.yml, el.yml, hy.yml each have ordinal.date, ordinal.dateOnly, and clock sections
- [ ] Hungarian year-first ordinal.date format correct
- [ ] el/hy day-period clock notation correct
- [ ] No new handwritten C# converter classes
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for fi, hu, az, tr, el, hy
## Done summary
Added ordinal.date, ordinal.dateOnly, and clock YAML sections for Finnish (fi), Hungarian (hu), Azerbaijani (az), Turkish (tr), Greek (el), and Armenian (hy) locales using the phrase-clock engine with locale-appropriate configurations (h24/h12 modes, dayPeriods, connectors, suffixes, zeroFillers).
## Evidence
- Commits: 4ca971d87d39e94a8f2bbe4e6e1f8b08f6ccc344, 52d67860e10a00e2e23e9e3c89e5ddcf2e18cdb2, 613061a5efd1b29af2dda33efc579051658fcadb
- Tests: dotnet build src/Humanizer/Humanizer.csproj -c Release, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (ordinal/clock tests pass for fi, hu, az, tr, el, hy)
- PRs: