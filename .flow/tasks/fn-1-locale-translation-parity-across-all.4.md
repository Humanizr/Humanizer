# fn-1-locale-translation-parity-across-all.4 Add ordinal.date + clock YAML — West Slavic locales

## Description
Add `ordinal.date`, `ordinal.dateOnly`, and `clock:` YAML sections to West Slavic locales using `phrase-clock` engine from task .2.

**Locales:** cs, sk, pl, hr, sl (all need both surfaces)

**Size:** M
**Files:** `src/Humanizer/Locales/cs.yml`, `sk.yml`, `pl.yml`, `hr.yml`, `sl.yml`

## Approach

**ordinal.date:** Use `pattern` engine. DotSuffix dayMode for cs, sk, hr, sl. Numeric for pl. hr trailing period needs embedded literal `'.'` in template.

**clock:** Use `phrase-clock` engine. Expected patterns from `LocaleCoverageData`:
- cs: "třináct hodin dvacet tři minut" — `hourMode: h24`, `hourSuffix`, `minuteSuffix` (plural-sensitive)
- sk: "trinásť hodín dvadsaťtri minút" — similar to Czech
- pl: "trzynasta dwadzieścia trzy" — `hourMode: h24`, simple concat
- hr: "trinaest sati i dvadeset tri minute" — `hourMode: h24`, `hourSuffix`, `connector: 'i'`, `minuteSuffix`
- sl: "trinajst triindvajset" — `hourMode: h24`, simple concat

**Note:** cs, sk, hr have plural-sensitive unit words. Investigate if `phrase-clock` engine handles this via `hourSuffixSingular`/`hourSuffixPlural` + `minuteSuffixSingular`/`minuteSuffixPlural`, or if bucket overrides are needed.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- Task .2 `phrase-clock` engine — verify plural suffix handling

**Optional:**
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:52` — pattern literal syntax
## Approach

**ordinal.date:** Use `pattern` engine. DotSuffix dayMode for cs, sk, hr, sl. Numeric for pl. hr/sr trailing period needs embedded literal.

**clock engine assignments:**
- cs: `phrase-clock` with `hourMode: h24`, `hourSuffix` (plural-sensitive: "hodin"/"hodina"), `minuteSuffix` (plural-sensitive: "minut")
- sk: `phrase-clock` with `hourMode: h24`, `hourSuffix` ("hodín"/"hodina"), `minuteSuffix` ("minút")
- pl: `phrase-clock` with `hourMode: h24` — simple concat ("trzynasta dwadzieścia trzy")
- hr: `phrase-clock` with `hourMode: h24`, `hourSuffix` ("sati"/"sat"), `connector: 'i'`, `minuteSuffix` ("minute"/"minuta")
- sl: `phrase-clock` with `hourMode: h24` — simple concat ("trinajst triindvajset")

Note: cs, sk, hr have plural-sensitive unit words (hour/minute change form based on the number). Investigate if `phrase-clock` engine needs singular/plural fields for units, or if the YAML bucketed overrides can handle this.

All values verified against `LocaleCoverageData`.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- Task .2 `phrase-clock` engine schema — verify unit-word plural handling
## Approach

**For ordinal.date/dateOnly:** Expected patterns from test data:
- cs: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` ("25. ledna 2022")
- sk: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` ("25. januára 2022")
- pl: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 stycznia 2022")
- hr: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` ("25. siječnja 2022." — note trailing dot)
- sl: `'{day} MMMM yyyy'` + `dayMode: 'DotSuffix'` ("25. januar 2022")

Note: hr and sr use trailing period after year. The pattern template may need `'.'` suffix: `'{day} MMMM yyyy''.'`

**For clock:** Expected output forms:
- cs: "třináct hodin dvacet tři minut" — 24h format with "hodin" and "minut" connectors
- sk: "trinásť hodín dvadsaťtri minút" — similar to Czech
- pl: "trzynasta dwadzieścia trzy" — 24h format, simpler
- hr: "trinaest sati i dvadeset tri minute" — "sati i" + minutes + "minute"
- sl: "trinajst triindvajset" — simple concatenation

Use `phrase-hour` engine with appropriate phrase templates. Each locale's clock conventions are distinct enough to need separate YAML entries.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations (13:23)
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1132-1200` — clock expectations (13:23 rounded)
- `docs/locale-yaml-reference.md:671-710` — phrase-hour and relative-hour engine fields

**Optional:**
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseHourClockNotationConverter.cs` — phrase-hour implementation
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs` — dayMode rendering
## Acceptance
- [ ] cs.yml, sk.yml, pl.yml, hr.yml, sl.yml each have ordinal.date, ordinal.dateOnly, and clock sections
- [ ] Plural-sensitive hour/minute unit words handled correctly (cs, sk, hr)
- [ ] hr trailing period in ordinal.date output correct
- [ ] No new handwritten C# converter classes
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for cs, sk, pl, hr, sl
## Done summary
Added ordinal.date, ordinal.dateOnly, and clock YAML sections for all 5 West Slavic locales (cs, sk, pl, hr, sl) with phrase-clock engine extensions. Implemented Slavic paucal suffix support with a paucalLowOnly mode to correctly distinguish West Slavic (cs, sk: paucal only for 2-4) from South Slavic (hr: paucal for units 2-4 excluding teens) grammatical rules.
## Evidence
- Commits: df406c1d, e8f25eab
- Tests: dotnet build src/Humanizer/Humanizer.csproj -c Release, dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj (58 passed), dotnet exec tests/Humanizer.Tests/bin/Debug/net10.0/Humanizer.Tests.dll (cs/sk/pl/hr/sl ordinal+clock all pass)
- PRs: