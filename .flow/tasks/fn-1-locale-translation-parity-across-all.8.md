# fn-1-locale-translation-parity-across-all.8 Add ordinal.date + clock YAML — East Asian locales

## Description
Add `ordinal.date`, `ordinal.dateOnly`, and `clock:` YAML sections to East Asian locales. Includes runtime investigation for Thai Buddhist calendar support.

**Locales needing both:** ko, zh-Hans, zh-Hant, bn, ta, th, vi
**Variant that auto-inherits:** zh-CN from zh-Hans
**Already complete:** ja (has both surfaces)

**Size:** M
**Files:**
- `src/Humanizer/Locales/ko.yml`
- `src/Humanizer/Locales/zh-Hans.yml`
- `src/Humanizer/Locales/zh-Hant.yml`
- `src/Humanizer/Locales/bn.yml`
- `src/Humanizer/Locales/ta.yml`
- `src/Humanizer/Locales/th.yml`
- `src/Humanizer/Locales/vi.yml`
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs` (if Thai needs runtime fix)

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
- [ ] ko.yml, zh-Hans.yml, zh-Hant.yml, bn.yml, ta.yml, th.yml, vi.yml each have `ordinal.date`, `ordinal.dateOnly`, and `clock:` sections
- [ ] zh-CN correctly inherits both surfaces from zh-Hans
- [ ] Thai ordinal date correctly produces Buddhist era year (2565 for 2022) — runtime changes made if needed
- [ ] CJK ordinal date patterns produce year-month-day order
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
- [ ] Sweep tests pass for ko, zh-Hans, zh-Hant, zh-CN, bn, ta, th, vi
- [ ] Source generator tests pass if OrdinalDatePattern was modified
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
