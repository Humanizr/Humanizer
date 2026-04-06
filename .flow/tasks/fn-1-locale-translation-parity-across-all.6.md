# fn-1-locale-translation-parity-across-all.6 Add ordinal.date + clock YAML — Finno-Ugric, Turkic, Greek, Armenian locales

## Description
Add `ordinal.date`, `ordinal.dateOnly`, and `clock:` YAML sections to Finno-Ugric, Turkic, Greek, and Armenian locales.

**Locales:** fi, hu, az, tr, el, hy (all need both surfaces)

**Size:** M
**Files:**
- `src/Humanizer/Locales/fi.yml`
- `src/Humanizer/Locales/hu.yml`
- `src/Humanizer/Locales/az.yml`
- `src/Humanizer/Locales/tr.yml`
- `src/Humanizer/Locales/el.yml`
- `src/Humanizer/Locales/hy.yml`

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
- [ ] fi.yml, hu.yml, az.yml, tr.yml, el.yml, hy.yml each have `ordinal.date`, `ordinal.dateOnly`, and `clock:` sections
- [ ] Hungarian date format correctly produces year-first output ("2022. január 25.")
- [ ] All clock YAML phrases verified against exact LocaleCoverageData expectations
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for fi, hu, az, tr, el, hy
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
