# fn-1-locale-translation-parity-across-all.9 Add ordinal.date + clock YAML — Austronesian, Semitic, and other locales

## Description
Add `ordinal.date`, `ordinal.dateOnly`, and `clock:` YAML sections to Austronesian, Semitic, and remaining locales.

**Locales:** he, id, ms, fil, mt, uz-Cyrl-UZ, uz-Latn-UZ, zu-ZA (all need both surfaces)

**Size:** M
**Files:** `src/Humanizer/Locales/he.yml`, `id.yml`, `ms.yml`, `fil.yml`, `mt.yml`, `uz-Cyrl-UZ.yml`, `uz-Latn-UZ.yml`, `zu-ZA.yml`

## Approach

**ordinal.date:** Use `pattern` engine.
- Filipino: month-first format `'MMMM {day}, yyyy'` (like English US)
- Maltese: `ta'` apostrophe needs careful YAML quoting — `'{day} ''ta'''' MMMM yyyy'`
- Hebrew: RTL — no directionality marks in output
- Others: derive from `LocaleCoverageData`

**clock:** Use `phrase-clock` engine.
- he: RTL — no directional marks
- id: `hourMode: h12`, `hourPrefix: 'pukul'`
- ms: similar to id
- Others: derive from `LocaleCoverageData`

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:425-441` — RTL validation
## Approach

**For ordinal.date/dateOnly:** Expected patterns:
- he: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 בינואר 2022")
- id: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Januari 2022")
- ms: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Januari 2022")
- fil: `'MMMM {day}, yyyy'` + `dayMode: 'Numeric'` ("Enero 25, 2022") — month-first like en-US
- mt: pattern with embedded `ta'` ("25 ta' Jannar 2022") — needs careful YAML quoting
- uz-Cyrl-UZ: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 январ 2022")
- uz-Latn-UZ: pattern with hyphen ("25-yanvar 2022")
- zu-ZA: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Januwari 2022")

Note: Maltese (mt) has embedded `ta'` with apostrophe — needs careful YAML quoting.
Note: Hebrew (he) is RTL — verify no directionality marks in output.

**For clock:** Derive ALL expected values directly from `LocaleCoverageData.TimeOnlyToClockNotation*ExpectationTheoryData`. Do not approximate — read exact strings:
- uz-Cyrl-UZ: "бирдан йигирма уч ўтди" (all Cyrillic script)
- uz-Latn-UZ: "birdan yigirma uch o'tdi" (all Latin script)
- All other locales: read exact values from LocaleCoverageData

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations (exact source of truth)
- `src/Humanizer/Locales/fil.yml` — existing Filipino YAML structure
- `docs/locale-yaml-reference.md:671-710` — clock engine field reference

**Optional:**
- `src/Humanizer/Locales/en-US.yml:5-13` — month-first ordinal.date pattern (for fil reference)
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:425-441` — RTL directionality checks
## Approach

**For ordinal.date/dateOnly:** Expected patterns:
- he: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 בינואר 2022")
- id: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Januari 2022")
- ms: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Januari 2022")
- fil: `'MMMM {day}, yyyy'` + `dayMode: 'Numeric'` ("Enero 25, 2022") — month-first like en-US
- mt: `'{day} ''ta'''' MMMM yyyy'` + `dayMode: 'Numeric'` ("25 ta' Jannar 2022")
- uz-Cyrl-UZ: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 январ 2022")
- uz-Latn-UZ: `'{day}''-''MMMM yyyy'` + `dayMode: 'Numeric'` ("25-yanvar 2022") — hyphen separator
- zu-ZA: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 Januwari 2022")

Note: Maltese (mt) has embedded `ta'` with apostrophe — needs careful YAML quoting.
Note: Hebrew (he) is RTL — verify no directionality marks in output.

**For clock:** Expected output:
- he: "אחת עשרים ושלוש" — 12h + minutes
- id: "pukul satu lewat dua puluh tiga menit" — "pukul" + 12h + "lewat" + minutes + "menit"
- ms: "pukul satu dua puluh tiga petang" — "pukul" + 12h + minutes + day-period
- fil: "ala una beinte-tres ng hapon" — "ala" + 12h + Spanish-influenced minutes + "ng" + day-period
- mt: "is-siegħa waħda u tlieta u għoxrin" — "is-siegħa" + 12h + "u" + minutes
- uz-Cyrl-UZ: "бирдан йигирма уч ўтди" — 12h + minutes + "ўтdi"
- uz-Latn-UZ: "birdan yigirma uch o'tdi" — Latin script equivalent
- zu-ZA: "ihora lokuqala namashumi amabili nantathu ntambama" — complex Zulu structure

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations
- `src/Humanizer/Locales/fil.yml` — existing Filipino YAML structure
- `docs/locale-yaml-reference.md:671-710` — clock engine field reference

**Optional:**
- `src/Humanizer/Locales/en-US.yml:5-13` — month-first ordinal.date pattern (for fil reference)
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:425-441` — RTL directionality checks
## Acceptance
- [ ] he.yml, id.yml, ms.yml, fil.yml, mt.yml, uz-Cyrl-UZ.yml, uz-Latn-UZ.yml, zu-ZA.yml each have ordinal.date, ordinal.dateOnly, and clock sections
- [ ] Maltese `ta'` apostrophe quoting correct
- [ ] Filipino month-first format correct
- [ ] Hebrew RTL output has no directional marks
- [ ] No new handwritten C# converter classes
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for he, id, ms, fil, mt, uz-Cyrl-UZ, uz-Latn-UZ, zu-ZA
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
