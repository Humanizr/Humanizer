# fn-1-locale-translation-parity-across-all.7 Add clock YAML — Baltic and Indo-Iranian locales

## Description
Add `clock:` YAML sections to Baltic and Indo-Iranian locales (ordinal.date already done for lt, lv, ar, fa, ku).

**Locales:** lt, lv, ar, fa, ku (clock only — ordinal.date already exists)

**Size:** S
**Files:** `src/Humanizer/Locales/lt.yml`, `lv.yml`, `ar.yml`, `fa.yml`, `ku.yml`

## Approach

**clock:** Use `phrase-clock` engine. RTL locales (ar, fa, ku) must NOT produce directionality marks (LRM/RLM/ALM) in output.

All values from `LocaleCoverageData` expectations. RTL validation: sweep tests at `LocaleRegistrySweepTests.cs:425-441` check for directional marks.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:425-441` — RTL validation pattern
## Approach

**clock engine assignments:**
- lt: `phrase-clock` with `hourMode: h24`, `hourSuffixSingular`/`hourSuffixPlural` ("valanda"/"valandų"), `minuteSuffixSingular`/`minuteSuffixPlural` ("minutė"/"minutės")
- lv: `phrase-clock` with `hourMode: h24`, `connector: 'un'` ("trīspadsmit un divdesmit trīs")
- ar: `phrase-clock` with day-periods (`morning`/`afternoon`/`night`/`earlyMorning`), `minuteSuffixSingular: 'دقيقة'`/`minuteSuffixPlural: 'دقائق'` — RTL
- fa: `phrase-clock` with day-periods (`morning`/`afternoon`/`night`/`earlyMorning`), `connector: 'و'` — RTL
- ku: `phrase-clock` with day-periods (`morning`/`afternoon`/`night`/`earlyMorning`) — RTL
<!-- Updated by plan-sync: fn-1.5 confirmed only phrase-clock engine exists (no relative-clock). Used hourSuffixSingular/hourSuffixPlural and minuteSuffixSingular/minuteSuffixPlural, not hourSuffix/minuteSuffix. Day periods use earlyMorning/morning/afternoon/night fields (see es.yml). -->

RTL locales (ar, fa, ku): verify output has no invisible directionality marks (LRM/RLM/ALM).

All values verified against `LocaleCoverageData`.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:425-441` — RTL mark assertions
- Task .2 `phrase-clock` engine schema (only engine that exists; no `relative-clock`)
## Approach

**For clock:** Expected output:
- lt: "trylika valandų dvidešimt trys minutės" — 24h + "valandų" + minutes + "minutės"
- lv: "trīspadsmit un divdesmit trīs" — 24h + "un" + minutes
- ar: "الواحدة وثلاث وعشرون دقيقة بعد الظهر" — 12h + minutes + day-period (relative-hour style)
- fa: "یک و بیست و سه بعدازظهر" — 12h + "و" + minutes + day-period
- ku: "یەک و بیست و سێی ئێوارە" — 12h + "و" + minutes + day-period

Arabic (ar), Farsi (fa), and Kurdish (ku) are RTL locales. Ensure clock output does not include invisible directionality control characters (LRM, RLM, ALM) — follow the existing pattern at `LocaleRegistrySweepTests` that checks Arabic ordinal.date output for these marks.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1132-1200` — rounded clock expectations
- `src/Humanizer/Locales/ar.yml` — existing ar YAML structure
- `src/Humanizer/Locales/fa.yml` — existing fa YAML structure

**Optional:**
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:425-441` — Arabic directionality mark assertions
- `src/Humanizer/Locales/es.yml:980-1000` — phrase-clock with day-period fields (earlyMorning/morning/afternoon/night) for RTL adaptation
## Acceptance
- [ ] lt.yml, lv.yml, ar.yml, fa.yml, ku.yml each have clock sections
- [ ] RTL locales (ar, fa, ku) produce no directional marks
- [ ] No new handwritten C# converter classes
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for lt, lv, ar, fa, ku
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
