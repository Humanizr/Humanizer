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
- lv: `phrase-clock` with `hourMode: h24`, connector 'un' embedded in `defaultTemplate` (e.g., `defaultTemplate: '{hour} un {minutes}'`) ("trīspadsmit un divdesmit trīs")
<!-- Updated by plan-sync: fn-1-locale-translation-parity-across-all.6 — connector is not a valid phrase-clock field; embed in defaultTemplate -->
- ar: `phrase-clock` with day-periods (`morning`/`afternoon`/`night`/`earlyMorning`), `minuteSuffixSingular: 'دقيقة'`/`minuteSuffixPlural: 'دقائق'` — RTL
- fa: `phrase-clock` with day-periods (`morning`/`afternoon`/`night`/`earlyMorning`), connector 'و' embedded in `defaultTemplate` (not a separate field) — RTL
<!-- Updated by plan-sync: fn-1-locale-translation-parity-across-all.6 — connector is not a valid phrase-clock field; embed in defaultTemplate like '{hour} و {minutes}' -->
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

**Key phrase-clock patterns from task .6 implementation:** Every clock profile needs at minimum `defaultTemplate` (the main format string with `{hour}`, `{minutes}`, `{minuteSuffix}` placeholders). Use `min0` for the zero-minutes case. Connectors go inside `defaultTemplate`, not as a separate field. See `EngineContractCatalog.cs:678-722` for the full field list.
<!-- Updated by plan-sync: fn-1-locale-translation-parity-across-all.6 used defaultTemplate/min0 patterns extensively; connector is not a standalone field -->

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
Added clock YAML sections for 5 Baltic and Indo-Iranian locales (lt, lv, ar, fa, ku) using the phrase-clock engine, with template-aware day-period resolution and {dayPeriod} placeholder support for Kurdish inline day-period placement. All clock tests pass for these locales and RTL output contains no directional marks.
## Evidence
- Commits: fd01c7bb, 22da334e, 3876eca5, be4f8a91, 75190de1
- Tests: dotnet build src/Humanizer/Humanizer.csproj -c Release, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
- PRs: