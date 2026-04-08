# fn-1-locale-translation-parity-across-all.8 Add ordinal.date + clock YAML — East Asian locales

## Description
Add `OrdinalDateCalendarMode` enum for non-Gregorian calendar support. Add ordinal.date/dateOnly + clock YAML for East Asian locales. Update existing ar/fa ordinal.date YAML and add he ordinal.date with `calendarMode: 'Native'` where test expectations require it.

**Locales (new YAML):** ko, zh-Hans, zh-Hant, bn, ta, th, vi (ordinal.date + clock)
**Locales (calendarMode update):** ar, fa (update existing ordinal.date YAML), he (add ordinal.date)
**Variants:** zh-CN auto-inherits from zh-Hans.

**Size:** M
**Files:**
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDateCalendarMode.cs` — NEW enum (`Gregorian` | `Native`)
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs` — add `calendarMode` parameter
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs` — add `calendarMode` field to ordinal.date engine contract
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` — emit calendarMode
- `src/Humanizer/Locales/ko.yml`, `zh-Hans.yml`, `zh-Hant.yml`, `bn.yml`, `ta.yml`, `th.yml`, `vi.yml` — add ordinal.date/dateOnly + clock
- `src/Humanizer/Locales/ar.yml`, `fa.yml` — update existing ordinal.date to add `calendarMode: 'Native'` if test expects native year
- `src/Humanizer/Locales/he.yml` — add ordinal.date/dateOnly with `calendarMode: 'Native'` if test expects Hebrew year

## Approach

### OrdinalDateCalendarMode enum

```
enum OrdinalDateCalendarMode { Gregorian, Native }
```

- `Gregorian` (default): forces `GregorianCalendar` — preserves current behavior for all existing locales
- `Native`: uses the culture's default calendar (e.g., ThaiBuddhistCalendar for th-TH, HebrewCalendar for he-IL, UmAlQuraCalendar for ar-SA, PersianCalendar for fa-IR)

Update `OrdinalDatePattern` to accept this mode. When `Native`, skip the Gregorian override in `GetPatternCulture()`. Add `calendarMode` as an optional YAML field — defaults to `Gregorian` when omitted.

### Non-Gregorian calendar locales

Check `LocaleCoverageData` test expectations for each. If test expects native calendar year, use `calendarMode: 'Native'`:
- `th` — Thai Buddhist (2565 for 2022) → likely `Native`
- `he` — Hebrew (5783, different months) → likely `Native`
- `ar` — Um Al Qura / Hijri (1444 for 2022) → likely `Native`
- `fa` — Persian / Solar Hijri (1401 for 2022) → likely `Native`
- `ja`, `ko`, `zh-Hant` — have optional non-Gregorian calendars but typically use Gregorian → likely `Gregorian` (omit field)

### East Asian ordinal.date + clock

Standard authoring using `pattern` engine + `phrase-clock` engine. All values from `LocaleCoverageData`.

**zh-Hant TFM conditionals:** `LocaleCoverageData.cs:11-30` has TFM-conditional expectations. YAML-driven clock output should be TFM-consistent.

## Investigation targets

**Required:**
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:59-83` — GetPatternCulture() to make conditional
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs` — ordinal.date engine contract
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations (check th, he, ar, fa years)
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- `src/Humanizer/Locales/ar.yml` — existing ordinal.date section to update
- `src/Humanizer/Locales/fa.yml` — existing ordinal.date section to update

**Optional:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:11-30` — zh-Hant TFM conditionals
- `src/Humanizer/Locales/zh-CN.yml` — verify variantOf zh-Hans
## Approach

### OrdinalDateCalendarMode enum

Add a new enum to control calendar behavior in ordinal date formatting:
```
enum OrdinalDateCalendarMode { Gregorian, Native }
```

- `Gregorian` (default): forces `GregorianCalendar` as today — preserves existing behavior for all current locales
- `Native`: uses the culture's default calendar (e.g., `ThaiBuddhistCalendar` for `th-TH`, producing year 2565 for 2022)

Update `OrdinalDatePattern` to accept this mode. When `Native`, skip the Gregorian override in `GetPatternCulture()`. Add `calendarMode` as an optional YAML field in the ordinal.date engine contract — defaults to `Gregorian` when omitted.

### Thai YAML

Thai uses: `calendarMode: 'Native'` to get Buddhist Era year in output.

### Other locales

Standard `pattern` engine with `calendarMode: 'Gregorian'` (or omitted, since it's the default).

**ordinal.date patterns from test expectations:**
- ko: `'yyyy''년 ''M''월 ''{day}''일'''` + `dayMode: 'Numeric'`
- zh-Hans/zh-Hant: `'yyyy''年''M''月''{day}''日'''` + `dayMode: 'Numeric'`
- bn: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'`
- ta: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'`
- th: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` + `calendarMode: 'Native'`
- vi: `'{day} ''tháng'' M ''năm'' yyyy'` + `dayMode: 'Numeric'`

**clock using `phrase-clock`:**
- ko: dayPeriods + h12 + "시" / "분"
- zh-Hans: dayPeriods + h12 + "点" / "分"
- zh-Hant: dayPeriods + h12 + "點" / "分"
- bn: dayPeriods + h12
- ta: dayPeriods + h12 + "மணி" / "நிமிடம்"
- th: dayPeriods + h12 + "นาที"
- vi: h12 + "giờ" / "phút" + dayPeriod suffix

All values MUST match `LocaleCoverageData` expectations.

**zh-Hant TFM conditionals:** `LocaleCoverageData.cs:11-30` has TFM-conditional expectations for short time format. YAML-driven clock output should be TFM-consistent since it uses spoken words.

## Investigation targets

**Required:**
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:59-83` — GetPatternCulture() Gregorian override to make conditional
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs` — ordinal.date engine contract (add calendarMode)
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` — ordinal.date profile generation
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations (verify Thai year)
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations

**Optional:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:11-30` — zh-Hant TFM conditionals
- `src/Humanizer/Locales/zh-CN.yml` — verify variantOf zh-Hans
- `src/Humanizer/Locales/ja.yml:290-296` — Japanese ordinal.date pattern reference
## Approach

**ordinal.date:** Use `pattern` engine.

**Thai Buddhist calendar blocker:** `OrdinalDatePattern.GetPatternCulture()` at line 59-83 forces Gregorian calendar. If Thai ordinal.date test expects Buddhist year (2565 for 2022), this needs a runtime change — either remove the Gregorian override for `th-TH` or add a `useNativeCalendar` flag to the pattern engine. Investigate actual test expectations in `LocaleCoverageData` before making changes.

**zh-Hant TFM conditionals:** `LocaleCoverageData.cs:11-30` has TFM-conditional expectations for zh-Hant. Clock output (spoken words) should be TFM-consistent. Verify.

**clock:** Use `phrase-clock` engine.
- ko: `hourMode: h12`, `hourSuffixSingular: '시'`/`hourSuffixPlural: '시'`, `minuteSuffixSingular: '분'`/`minuteSuffixPlural: '분'`
- zh-Hans/zh-Hant: `hourMode: h12`, numeric-style or word-style depending on test expectations
- bn, ta: check test expectations for format
- th: investigate — Thai may use numeric format or spoken words
- vi: `hourMode: h12`, `hourSuffixSingular: 'giờ'`/`hourSuffixPlural: 'giờ'`, `minuteSuffixSingular: 'phút'`/`minuteSuffixPlural: 'phút'`
<!-- Updated by plan-sync: fn-1.5 used hourSuffixSingular/hourSuffixPlural and minuteSuffixSingular/minuteSuffixPlural, not hourSuffix/minuteSuffix -->

All values MUST match `LocaleCoverageData` expectations.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations (check Thai year)
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:11-30` — TFM-conditional zh-Hant data
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1263` — clock expectations
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:59-83` — Gregorian calendar forcing
- `src/Humanizer/Locales/zh-CN.yml` — verify variantOf zh-Hans

**Optional:**
- `src/Humanizer/Locales/th.yml` — current Thai locale state
## Approach

**For ordinal.date/dateOnly:** Expected patterns:
- ko: year-month-day with Korean suffixes ("2022년 1월 25일")
- zh-Hans/zh-Hant: year-month-day with CJK suffixes ("2022年1月25日")
- bn: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 জানুয়ারী 2022")
- ta: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 ஜனவரி 2022")
- vi: day + "tháng" + month + "năm" + year ("25 tháng 1 năm 2022")
- th: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 มกราคม 2565") — **Buddhist era year**

**Thai Buddhist calendar investigation (REQUIRED before YAML authoring):**
The test expects "2565" for Gregorian year 2022. `OrdinalDatePattern.GetPatternCulture()` at line 59-83 forces a Gregorian calendar via `GregorianCalendar(GregorianCalendarTypes.Localized)`. This will produce "2022", not "2565". Options:
1. Thai needs a dedicated `dayMode` or engine that preserves the Thai Buddhist calendar
2. The pattern engine needs a `useNativeCalendar: true` flag that skips the Gregorian override
3. Thai may need a residual-leaf converter instead of the pattern engine
Investigate which approach fits, implement the minimal runtime change, then author the YAML.

**For clock:** Derive exact expectations from `LocaleCoverageData.TimeOnlyToClockNotation*ExpectationTheoryData`.

## Key context

The `zh-Hant` locale has TFM-conditional expectations in `LocaleCoverageData.cs:11-30`. YAML-driven clock output should be consistent across TFMs since it uses spoken words, not `ToString("t")`.

## Investigation targets

**Required:**
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:59-83` — GetPatternCulture() Gregorian override (Thai blocker)
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations (th expects 2565)
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations
- `src/Humanizer/Locales/ja.yml:290-296` — Japanese ordinal.date and clock pattern

**Optional:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:11-30` — zh-Hant TFM conditionals
- `src/Humanizer/Locales/zh-CN.yml` — zh-CN variant file
## Approach

**For ordinal.date/dateOnly:** Expected patterns:
- ko: `'yyyy''년 ''M''월 ''{day}''일'''` + `dayMode: 'Numeric'` ("2022년 1월 25일")
- zh-Hans: `'yyyy''年''M''月''{day}''日'''` + `dayMode: 'Numeric'` ("2022年1月25日")
- zh-Hant: same as zh-Hans ("2022年1月25日")
- bn: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 জানুয়ারী 2022")
- ta: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 ஜனவரி 2022")
- th: `'{day} MMMM yyyy'` + `dayMode: 'Numeric'` ("25 มกราคม 2565") — Thai Buddhist calendar!
- vi: `'{day} ''tháng'' M ''năm'' yyyy'` + `dayMode: 'Numeric'` ("25 tháng 1 năm 2022")

**Thai Buddhist calendar:** The `OrdinalDatePattern.GetPatternCulture()` forces a Gregorian calendar, but Thai test expects 2565 (Buddhist era). Investigate how the existing test expectation was authored — the pattern engine may need `useNativeCalendar: true` or the Thai output may need special handling.

**For clock:** Expected output:
- ko: "오후 한 시 이십삼 분" — day-period + 12h + "시" + minutes + "분"
- zh-Hans: "下午一点二十三分" — day-period + 12h + "点" + minutes + "分"
- zh-Hant: "下午一點二十三分" — Traditional Chinese variant
- bn: "দুপুর একটা তেইশ" — day-period + 12h + minutes
- ta: "பிற்பகல் ஒரு மணி இருபத்து மூன்று நிமிடம்" — day-period + 12h + "மணி" + minutes + "நிமிடம்"
- th: "บ่ายหนึ่งยี่สิบสามนาที" — day-period + 12h + minutes + "นาที"
- vi: "một giờ hai mươi ba phút chiều" — 12h + "giờ" + minutes + "phút" + day-period

## Key context

The `zh-Hant` locale has TFM-conditional expectations in `LocaleCoverageData.cs:11-30` — short time format differs between .NET 8 and .NET 10. YAML-driven clock output should be consistent across TFMs since it uses spoken words, not `ToString("t")`.

The `zh-CN` → `zh-Hans` variant should auto-inherit. Verify the CultureInfo.Parent chain works correctly on both Windows and Unix for `zh-CN`.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-99` — ordinal.date expectations
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:1065-1129` — clock expectations
- `src/Humanizer/Locales/ja.yml:290-296` — Japanese ordinal.date and clock pattern
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:59-83` — GetPatternCulture() Gregorian override

**Optional:**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:11-30` — zh-Hant TFM conditionals
- `src/Humanizer/Locales/zh-CN.yml` — zh-CN variant file
## Acceptance
- [ ] `OrdinalDateCalendarMode` enum added (`Gregorian` | `Native`)
- [ ] `OrdinalDatePattern` updated to respect `calendarMode` — `Native` skips Gregorian override
- [ ] `calendarMode` field added to ordinal.date engine contract in source generator
- [ ] ko.yml, zh-Hans.yml, zh-Hant.yml, bn.yml, ta.yml, th.yml, vi.yml each have ordinal.date, ordinal.dateOnly, and clock sections
- [ ] th.yml uses `calendarMode: 'Native'` — produces Buddhist Era year
- [ ] ar.yml, fa.yml ordinal.date updated with `calendarMode: 'Native'` if test expects native year
- [ ] he.yml has ordinal.date/dateOnly with `calendarMode: 'Native'` if test expects Hebrew year
- [ ] zh-CN correctly inherits from zh-Hans
- [ ] zh-Hant clock output is TFM-consistent
- [ ] Existing locales without calendarMode unaffected (defaults to Gregorian)
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for ko, zh-Hans, zh-Hant, zh-CN, bn, ta, th, vi, ar (ordinal.date), fa (ordinal.date), he (ordinal.date)
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
