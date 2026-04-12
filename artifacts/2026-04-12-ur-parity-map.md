# Urdu (ur) Locale Parity Map

**Created**: 2026-04-12
**Branch**: `feat/urdu-locale`
**Epic**: fn-8-add-urdu-ur-locale-with-full-language

---

## Preflight Gap Report

| Surface | Status |
|---|---|
| list | missing |
| formatter | missing |
| phrases | missing |
| number.words | missing |
| number.parse | missing |
| number.formatting | missing (override needed) |
| ordinal.numeric | missing |
| ordinal.date | missing |
| ordinal.dateOnly | missing |
| clock | missing |
| compass | missing |
| calendar | missing (Hijri extension needed) |

All 8 canonical surfaces (+ nested members) are unresolved.

---

## Probe Output

### Probe method

The standard `tools/locale-probe.cs` does not accept positional locale arguments (it probes a hard-coded 62-locale set that does not include `ur`). The probe data below was collected via a separate ad hoc C# file-based app (`/tmp/urdu-probe.cs`) that directly queries `CultureInfo.GetCultureInfo()` for `ur`, `ur-PK`, and `ur-IN`. The ad hoc probe script is reproduced below for reproducibility.

```csharp
// /tmp/urdu-probe.cs -- ad hoc probe for Urdu cultures
using System;
using System.Globalization;
using System.Linq;

var cultures = new[] { "ur", "ur-PK", "ur-IN" };
foreach (var c in cultures) {
    var ci = CultureInfo.GetCultureInfo(c);
    var dtf = ci.DateTimeFormat;
    var nfi = ci.NumberFormat;
    Console.WriteLine($"=== {c} ===");
    Console.WriteLine($"  DecimalSeparator: [{nfi.NumberDecimalSeparator}] (U+{(int)nfi.NumberDecimalSeparator[0]:X4})");
    Console.WriteLine($"  GroupSeparator: [{nfi.NumberGroupSeparator}] (U+{(int)nfi.NumberGroupSeparator[0]:X4})");
    Console.WriteLine($"  NegativeSign: [{nfi.NegativeSign}] ({string.Join(" ", nfi.NegativeSign.Select(ch => $"U+{(int)ch:X4}"))})");
    Console.WriteLine($"  ShortDatePattern: {dtf.ShortDatePattern}");
    Console.WriteLine($"  LongDatePattern: {dtf.LongDatePattern}");
    Console.WriteLine($"  ShortTimePattern: {dtf.ShortTimePattern}");
    Console.WriteLine($"  AM: {dtf.AMDesignator}");
    Console.WriteLine($"  PM: {dtf.PMDesignator}");
    Console.WriteLine($"  Calendar: {dtf.Calendar.GetType().Name}");
    Console.Write("  OptionalCalendars:");
    foreach (var cal in ci.OptionalCalendars) Console.Write($" {cal.GetType().Name}");
    Console.WriteLine();
    Console.Write("  Months:");
    for (int m = 1; m <= 12; m++) Console.Write($" {dtf.MonthNames[m-1]}");
    Console.WriteLine();
    Console.Write("  MonthsGenitive:");
    for (int m = 1; m <= 12; m++) Console.Write($" {dtf.MonthGenitiveNames[m-1]}");
    Console.WriteLine();
    // Test HijriCalendar assignment
    try {
        var clone = (CultureInfo)ci.Clone();
        clone.DateTimeFormat.Calendar = new HijriCalendar();
        Console.WriteLine("  HijriCalendar assignment: SUCCESS");
    } catch (Exception ex) {
        Console.WriteLine($"  HijriCalendar assignment: FAILED ({ex.GetType().Name})");
    }
    // Test UmAlQuraCalendar assignment
    try {
        var clone = (CultureInfo)ci.Clone();
        clone.DateTimeFormat.Calendar = new UmAlQuraCalendar();
        Console.WriteLine("  UmAlQuraCalendar assignment: SUCCESS");
    } catch (Exception ex) {
        Console.WriteLine($"  UmAlQuraCalendar assignment: FAILED ({ex.GetType().Name})");
    }
}
```

### Environment

```
.NET: 10.0.2
Framework: .NET 10.0.2
OS: macOS 26.4.1
RID: osx-arm64
Timestamp: 2026-04-12T22:07:09Z
Command: dotnet run /tmp/urdu-probe.cs
```

### ur (neutral)

| Property | Value |
|---|---|
| DecimalSeparator | `.` (U+002E) |
| GroupSeparator | `,` (U+002C) |
| NegativeSign | `LRM -` (U+200E U+002D) -- contains invisible U+200E LRM prefix |
| ShortDatePattern | `d/M/yyyy` |
| LongDatePattern | `dddd، d MMMM، yyyy` |
| ShortTimePattern | `LRM h:mm tt` -- contains invisible U+200E LRM prefix |
| AM | `AM` |
| PM | `PM` |
| Default Calendar | GregorianCalendar |
| OptionalCalendars | GregorianCalendar, HijriCalendar |
| HijriCalendar assignment | SUCCESS (cloned culture accepts `new HijriCalendar()` on `DateTimeFormat.Calendar`) |
| UmAlQuraCalendar assignment | FAILED (ArgumentOutOfRangeException: not valid for culture) |
| Months (nominative) | جنوری فروری مارچ اپریل مئی جون جولائی اگست ستمبر اکتوبر نومبر دسمبر |
| MonthsGenitive | identical to nominative |

### ur-PK

| Property | Value |
|---|---|
| DecimalSeparator | `.` (U+002E) |
| GroupSeparator | `,` (U+002C) |
| NegativeSign | `LRM -` (U+200E U+002D) |
| ShortDatePattern | `d/M/yyyy` |
| LongDatePattern | `dddd، d MMMM، yyyy` |
| Default Calendar | GregorianCalendar |
| OptionalCalendars | GregorianCalendar, HijriCalendar |
| HijriCalendar assignment | SUCCESS |
| UmAlQuraCalendar assignment | FAILED |
| Months | identical to `ur` |

### ur-IN

| Property | Value |
|---|---|
| DecimalSeparator | `.` (U+002E) |
| GroupSeparator | `,` (U+002C) |
| NegativeSign | `LRM -` (U+200E U+002D) |
| ShortDatePattern | `d/M/yyyy` |
| LongDatePattern | `dddd، d MMMM، yyyy` |
| Default Calendar | GregorianCalendar |
| OptionalCalendars | GregorianCalendar, HijriCalendar |
| HijriCalendar assignment | SUCCESS |
| UmAlQuraCalendar assignment | FAILED |
| Months | identical to `ur` |

### net48 Probe

net48 probe requires a Windows host. Not available locally on macOS arm64. Will be verified via CI. The net48 `tools/locale-probe-net48/` tool also does not accept positional locale arguments and would need the same ad hoc approach.

---

## Contract Decisions

### Decision 1a -- IOrdinalizer engine strategy for Urdu word ordinals

**Choice**: New `number-word-ordinalizer` engine (or extend `word-form-template` to call `NumberToWords` internally).

**Problem statement**: The existing `word-form-template` ordinalizer engine (`OrdinalizerProfileCatalogInput.cs:180-198`) produces output of the form `{prefix}{number}{suffix}` where `{number}` is the input integer rendered as a digit string. For Urdu word ordinals (`پانچواں` = "fifth" masculine, `پانچویں` = "fifth" feminine), the ordinalizer must render the cardinal word form (`پانچ` = "five") and then apply a gendered suffix (`واں`/`ویں`). The existing engine cannot do this -- it would produce `5واں` (digit+suffix), which the epic explicitly rejects.

**Options considered**:

1. **Exact replacement table only**: Use the existing `exactReplacements` dictionary on `word-form-template` to map every integer to its word ordinal. Rejected because the replacement table would need entries for every possible integer, which is impractical beyond a small range.

2. **Extend `word-form-template` to call `NumberToWords` at runtime**: The engine would resolve the current culture's `INumberToWordsConverter`, call `Convert(number)` to get the cardinal word, then apply the gendered suffix. This preserves the existing YAML profile shape (masculine/feminine pattern sets) while adding a `numberWordMode: true` flag or similar.

3. **New `number-word-ordinalizer` engine**: A new dedicated ordinalizer that takes the cardinal word from `NumberToWords` and applies gendered suffixes. Similar to option 2 but cleaner separation.

**Selected**: Option 2 or 3 (final engine shape decided in task .9). Both produce the same output; the difference is whether the extension lives inside `WordFormTemplateOrdinalizer` or in a new class. Task .9 owns the implementation. The key requirement is that the runtime ordinalizer calls `NumberToWords` to get the cardinal stem before applying suffixes.

**YAML data shape for Urdu `ordinal.numeric`**:
```yaml
ordinal:
  numeric:
    engine: 'word-form-template'   # or new engine name
    numberWordMode: true           # signals: render cardinal word, then suffix
    masculine:
      defaultSuffix: 'واں'
      exactReplacements:
        1: 'پہلا'
        2: 'دوسرا'
        3: 'تیسرا'
        # ... irregular forms through ~10
    feminine:
      defaultSuffix: 'ویں'
      exactReplacements:
        1: 'پہلی'
        2: 'دوسری'
        3: 'تیسری'
```

**Implementation task**: .9 (Wire gendered ordinals end-to-end)

### Decision 1b -- INumberToWordsConverter gendered ordinal output

**Problem statement**: `INumberToWordsConverter.ConvertToOrdinal(int)` is genderless (the `IndianGroupingNumberToWordsConverter` extends `GenderlessNumberToWordsConverter`). The epic requires gendered ordinal output:
- `5.ToOrdinalWords(GrammaticalGender.Masculine, "ur")` == `پانچواں`
- `5.ToOrdinalWords(GrammaticalGender.Feminine, "ur")` == `پانچویں`

**API path analysis**:
- `int.ToOrdinalWords()` calls `NumberToWordsExtension.ToOrdinalWords(int)` which calls `INumberToWordsConverter.ConvertToOrdinal(int)` -- genderless, no gender parameter.
- `int.Ordinalize(GrammaticalGender, CultureInfo)` calls `IOrdinalizer.Convert(int, string, GrammaticalGender)` -- this IS gendered.
- `"5".Ordinalize(GrammaticalGender, CultureInfo)` -- same path through IOrdinalizer.

**Resolution**: The gendered word-ordinal output for Urdu routes through the `Ordinalize` API (IOrdinalizer), NOT through `ToOrdinalWords` (INumberToWordsConverter). The ordinalizer engine extension from Decision 1a handles the gendered dispatch. The `ToOrdinalWords` path will produce genderless ordinals using the `indian-grouping` engine's `OrdinalSuffix` + `OrdinalExceptions` mechanism -- these will be cardinal+suffix forms (acceptable for the genderless API).

**Task ownership**: .9 owns the ordinalizer engine extension. .3 owns the `indian-grouping` cardinal/genderless-ordinal data.

### Decision 2 -- Number-to-words engine for Urdu

**Choice**: `indian-grouping` engine, with required adaptation for dense 0-99 forms.

**Inspection of existing engine**: `IndianGroupingNumberToWordsConverter` (`IndianGroupingNumberToWordsConverter.cs`) supports:
- `UnitsMap`: string[] indexed by value, currently 0-19 in Tamil
- `TensMap`: string[] indexed by decade (2-9), currently 10 entries
- Compositional tens+units via `GetTensValue()` which concatenates `TensMap[quotient]` + suffix + `UnitsMap[remainder]`
- `HundredsMap` (9 entries), `ThousandsMap` (19 entries)
- Lakh/Crore scales with continuing/exact suffixes
- Genderless ordinals via `OrdinalSuffix` + `OrdinalExceptions`

**Urdu requirement**: Urdu (like Hindi) has **distinct words for ALL numbers 0-99**, not just 0-19. For example: `اکیس` (21), `بائیس` (22), ... `ننانوے` (99). These are NOT compositional (tens+units) -- they are lexically distinct.

**Adaptation options**:

1. **Extend `UnitsMap` to 100 entries** (0-99): The `indian-grouping` engine's `GetTensValue()` method checks `if (number < 20)` then uses `GetUnitValue(number)`. If `UnitsMap` has 100 entries, change the threshold to `if (number < 100)` or route all sub-100 values through `UnitsMap`. This is a small engine change.

2. **Empty `TensMap` + encode all 21-99 in suffix composition**: The engine's suffix system (`defaultTensWithRemainderSuffix`, `specialTensWithRemainderSuffix`, etc.) cannot encode arbitrary irregular forms -- it concatenates a tens stem with a suffix and then a unit word, which is fundamentally compositional.

**Selected**: Option 1 -- extend `UnitsMap` to 100 entries. The engine change is minimal: one threshold comparison in `GetTensValue()`. The `indian-grouping` engine contract in `EngineContractCatalog.cs` already defines `unitsMap` as a `string-array` with no fixed-length constraint. Task .3 owns the YAML data authoring and any engine threshold adjustment.

**Engine suitability confirmed**:
- Lakh/Crore scale decomposition matches Urdu grouping rules
- Hundreds map (9 entries) matches Urdu hundreds (ایک سو through نو سو)
- Thousands map (19 entries) matches Urdu thousands
- Genderless ordinal output via `OrdinalSuffix` is adequate for the `ToOrdinalWords` API path

**Verification targets** (to be proven by .3 and .6):
- `21` -> `اکیس` (dense lookup, not composition)
- `99` -> `ننانوے` (dense lookup)
- `100` -> `ایک سو`
- `100000` -> `ایک لاکھ`
- `1234567` -> to be reviewer-approved

### Decision 3 -- Hijri calendar contract

**Choice**: **(A) Culture-calendar contract** with `calendarMode: Native`.

**Feasibility proof -- OptionalCalendars survey**:

| Culture | Default Calendar | OptionalCalendars | HijriCalendar assign | UmAlQuraCalendar assign | Platform |
|---|---|---|---|---|---|
| ur | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED (not valid) | net10.0 macOS |
| ur-PK | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED (not valid) | net10.0 macOS |
| ur-IN | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED (not valid) | net10.0 macOS |

All three Urdu cultures accept `HijriCalendar` assignment to `DateTimeFormat.Calendar`. `UmAlQuraCalendar` is rejected by all three. net48/net8 verification requires Windows CI host.

**Existing runtime support**: The `OrdinalDateCalendarMode` enum (`OrdinalDateCalendarMode.cs`) has two values:
- `Gregorian` -- forces Gregorian calendar
- `Native` -- uses `CultureInfo.CurrentCulture.DateTimeFormat.Calendar` as-is

`OrdinalDatePattern.GetPatternCulture()` (`OrdinalDatePattern.cs:254-286`):
```csharp
if (calendarMode == OrdinalDateCalendarMode.Native)
{
    return culture;  // preserves whatever calendar is on the culture
}
// Otherwise forces GregorianCalendar
```

**Public caller contract**: The caller must:
1. Clone the Urdu CultureInfo
2. Assign `HijriCalendar` to the clone's `DateTimeFormat.Calendar`
3. Set `Thread.CurrentCulture` to the clone (or pass it via culture-aware overloads)
4. Call `DateTime.ToOrdinalWords()`

This works because:
- `OrdinalDatePattern.Format()` clones `CurrentCulture` and reads `Calendar` from the clone
- `DateToOrdinalWordsExtensions` resolves the converter via `Configurator.DateToOrdinalWordsConverters.ResolveForCulture(culture)` which walks `CultureInfo.Parent` -- the Urdu converter resolves via `ur` regardless of which calendar is active
- The `OrdinalDatePattern` then uses the culture's calendar for day/month extraction

**Schema extension for Hijri month names**: New `hijriMonths` key in the `calendar` surface:
```yaml
calendar:
  months:
    - جنوری
    - فروری
    # ... 12 Gregorian month names
  hijriMonths:
    - محرم
    - صفر
    # ... 12 Hijri month names
```

The `AddCalendarFeatures` validator in `CanonicalLocaleAuthoring.cs:300-358` must be extended to accept `hijriMonths` as a valid key (currently only `months` and `monthsGenitive`). The `OrdinalDateProfileCatalogInput` must extract `hijriMonths` and pass them to the pattern when the active calendar is Hijri. The `OrdinalDatePattern.SubstituteMonth()` must select the correct month array based on the active calendar.

**Blast radius**: 3 files: `CanonicalLocaleAuthoring.cs`, `OrdinalDateProfileCatalogInput.cs`, `OrdinalDatePattern.cs`. Under the 5-file threshold. No re-plan needed.

---

## Build and Test Evidence

### Build verification

```
$ dotnet build src/Humanizer/Humanizer.csproj -c Release
Build succeeded.
    0 Warning(s)
    0 Error(s)
Time Elapsed 00:00:07.78
```

All four TFMs built: net10.0, net8.0, net48, netstandard2.0. Zero diagnostics attributable to `ur.yml`.

### Source generator tests

```
$ dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
Test run summary: Passed!
  total: 58
  failed: 0
  succeeded: 58
  skipped: 0
  duration: 2s 284ms
```

---

## LocaleTheoryMatrixCompletenessTests Failure Output (.6 Driver List)

84 failures for locale `ur` across all `ShippedLocaleRows` datasets.

### Registry coverage (8 failures)
- FormatterRegistryCoversYamlLocale
- CollectionFormatterRegistryCoversYamlLocale
- NumberToWordsConverterRegistryCoversYamlLocale
- OrdinalizerRegistryCoversYamlLocale
- WordsToNumberConverterRegistryCoversYamlLocale
- DateToOrdinalWordsConverterRegistryCoversYamlLocale
- DateOnlyToOrdinalWordsConverterRegistryCoversYamlLocale
- TimeOnlyToClockNotationConvertersRegistryCoversYamlLocale

### Formatter exact theory data (6 failures)
- DateDayPluralCases
- MultiPartTimeSpanCases
- TimeUnitSymbolCases
- ByteSizeSymbolCases
- ByteSizeFullWordCases
- CollectionHumanizeCases

### Heading data (2 failures)
- HeadingCases
- HeadingAbbreviatedCardinalCases

### Number theory data -- genderless (7 failures)
- CardinalCases
- CardinalAddAndCases
- CardinalWordFormCases
- OrdinalCases
- OrdinalWordFormCases
- TupleCases
- WordsToNumberCases

### Number theory data -- gendered (12 failures, 3 genders x 4 datasets)
- CardinalGenderCases (M/F/N)
- CardinalWordFormGenderCases (M/F/N)
- OrdinalGenderCases (M/F/N)
- OrdinalWordFormGenderCases (M/F/N)

### Number overload theory data (4 failures)
- AddAndCases
- WordFormCases
- GenderCases
- WordFormGenderCases

### Phrase theory data (3 failures)
- DateHumanizeCases
- NullDateHumanizeCases
- TimeSpanHumanizeCases

### Number magnitude theory data (2 failures)
- MagnitudeCardinalCases
- ExtendedMagnitudeCardinalCases

### Coverage data -- formatter and collection (2 failures)
- FormatterExpectationTheoryData
- CollectionFormatterExpectationTheoryData

### Coverage data -- number to words (2 failures)
- NumberToWordsOrdinalExpectationTheoryData
- NumberToWordsCardinalExpectationTheoryData

### Coverage data -- ordinalizer (16 failures: 4 genderless + 12 gendered)
- OrdinalizerExpectationTheoryData
- OrdinalizerDefaultExpectationTheoryData
- OrdinalizerNegativeExpectationTheoryData
- OrdinalizerWordFormExpectationTheoryData
- OrdinalizerGenderExpectationTheoryData (M/F/N)
- OrdinalizerWordFormGenderExpectationTheoryData (M/F/N)
- OrdinalizerStringExactExpectationTheoryData (M/F/N)
- OrdinalizerNumberExactExpectationTheoryData (M/F/N)

### Ordinalizer matrix data (10 failures: 4 genderless + 6 gendered)
- OrdinalizerExpectationTheoryData
- OrdinalizerDefaultExpectationTheoryData
- OrdinalizerNegativeExpectationTheoryData
- OrdinalizerWordFormExpectationTheoryData
- OrdinalizerGenderExpectationTheoryData (M/F/N)
- OrdinalizerWordFormGenderExpectationTheoryData (M/F/N)

### Coverage data -- date ordinals (6 failures)
- DateToOrdinalWords2022January25ExpectationTheoryData
- DateToOrdinalWords2015January1ExpectationTheoryData
- DateToOrdinalWords2015February3ExpectationTheoryData
- DateOnlyToOrdinalWords2022January25ExpectationTheoryData
- DateOnlyToOrdinalWords2015January1ExpectationTheoryData
- DateOnlyToOrdinalWords2015February3ExpectationTheoryData

### Coverage data -- clock (3 failures)
- TimeOnlyToClockNotation1323ExpectationTheoryData
- TimeOnlyToClockNotation1323RoundedExpectationTheoryData
- TimeOnlyToClockNotation0105ExpectationTheoryData

### Coverage data -- words to number (1 failure)
- WordsToNumberExpectationTheoryData

**Total**: 84 failures. All for locale `ur`. No claim of full registry coverage from this task.

---

## Parity Map Table

| Surface | Ownership path | Current state | Target state | Support state | Status |
|---|---|---|---|---|---|
| list | -- | missing | locale-owned | not supported | not-started |
| formatter | -- | missing | locale-owned | not supported | not-started |
| phrases.relativeDate | -- | missing | locale-owned | not supported | not-started |
| phrases.duration | -- | missing | locale-owned | not supported | not-started |
| phrases.dataUnits | -- | missing | locale-owned | not supported | not-started |
| phrases.timeUnits | -- | missing | locale-owned | not supported | not-started |
| number.words.cardinal | -- | missing | locale-owned | not supported | not-started |
| number.words.ordinal | -- | missing | locale-owned | not supported | not-started |
| number.parse.cardinal | -- | missing | locale-owned | not supported | not-started |
| number.parse.ordinal | -- | missing | locale-owned | not supported | not-started |
| number.formatting.decimalSeparator | -- | missing | locale-owned | not supported | not-started |
| ordinal.numeric | -- | missing | locale-owned | not supported | not-started |
| ordinal.date | -- | missing | locale-owned | not supported | not-started |
| ordinal.dateOnly | -- | missing | locale-owned | not supported | not-started |
| clock | -- | missing | locale-owned | not supported | not-started |
| compass | -- | missing | locale-owned | not supported | not-started |
| calendar.months | -- | missing | locale-owned | not supported | not-started |

### Effective Gap Summary

All 8 canonical surfaces remain unresolved. Implementation begins with task .2.

---

## Before/After Parity Delta

**Before (this task)**: 8 canonical surfaces unresolved (all missing)
**After (this task)**: 8 canonical surfaces still unresolved (scaffolded, not implemented)

The parity delta will reach empty at .7 completion.

---

## Unresolved Questions

(To be resolved by downstream tasks .2 through .14)

1. Dense 0-99 handling in `indian-grouping` engine for Urdu (extend `UnitsMap` to 100 entries + adjust `GetTensValue` threshold) -- task .3
2. Exact Hijri month override schema extension (`hijriMonths` key in calendar surface) -- task .10
3. Word-ordinal ordinalizer engine (extend `word-form-template` with `numberWordMode` or new engine) -- task .9
4. Regional variant differences between ur-PK and ur-IN -- task .11
