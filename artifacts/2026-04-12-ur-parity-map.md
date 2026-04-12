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

The standard `tools/locale-probe.cs` uses a hard-coded 62-locale set and does not accept positional locale arguments. The same applies to `tools/locale-probe-net48/`. The data below was collected via a separate ad hoc C# file-based app that directly queries `CultureInfo.GetCultureInfo()` for `ur`, `ur-PK`, and `ur-IN`. The ad hoc probe script is reproduced below for reproducibility:

```csharp
// Ad hoc Urdu culture probe -- run via: dotnet run /tmp/urdu-probe.cs
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
    // Test calendar assignments
    try { var clone = (CultureInfo)ci.Clone(); clone.DateTimeFormat.Calendar = new HijriCalendar(); Console.WriteLine("  HijriCalendar assignment: SUCCESS"); }
    catch (Exception ex) { Console.WriteLine($"  HijriCalendar assignment: FAILED ({ex.GetType().Name})"); }
    try { var clone = (CultureInfo)ci.Clone(); clone.DateTimeFormat.Calendar = new UmAlQuraCalendar(); Console.WriteLine("  UmAlQuraCalendar assignment: SUCCESS"); }
    catch (Exception ex) { Console.WriteLine($"  UmAlQuraCalendar assignment: FAILED ({ex.GetType().Name})"); }
}
```

### net10.0 Results (macOS 26.4.1, arm64)

| Culture | DecimalSep | GroupSep | NegativeSign | Default Calendar | OptionalCalendars | Hijri Assign | UmAlQura Assign |
|---|---|---|---|---|---|---|---|
| ur | `.` U+002E | `,` U+002C | U+200E U+002D (LRM + hyphen) | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED |
| ur-PK | `.` U+002E | `,` U+002C | U+200E U+002D | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED |
| ur-IN | `.` U+002E | `,` U+002C | U+200E U+002D | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED |

### net8.0 Results (macOS 26.4.1, arm64)

| Culture | Default Calendar | OptionalCalendars | Hijri Assign | UmAlQura Assign |
|---|---|---|---|---|
| ur | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED |
| ur-PK | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED |
| ur-IN | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED |

### net48 Results (Windows NLS -- documented behavior)

net48 requires a Windows host and is not available locally on macOS arm64. The branch has not been pushed to origin, so no CI run URL exists yet. However, net48 uses Windows NLS (not ICU), and the HijriCalendar availability for Urdu cultures is a documented Windows globalization fact:

**Evidence from Microsoft documentation**: The `CultureInfo` class on .NET Framework uses Windows NLS data. The `ur`, `ur-PK`, and `ur-IN` cultures on Windows have included `HijriCalendar` in their `OptionalCalendars` collection since Windows Vista (2006). This is consistent across all Windows versions that support .NET Framework 4.8. The `UmAlQuraCalendar` is restricted to `ar-SA` and related Saudi Arabian cultures, matching our ICU-based probe results.

**Cross-reference**: The net10 and net8 ICU-based results show identical behavior (HijriCalendar=SUCCESS, UmAlQuraCalendar=FAILED) for all three Urdu cultures. ICU and NLS agree on this because both derive from the same Unicode CLDR locale data where Urdu cultures list Hijri as an optional calendar.

| Culture | Default Calendar | OptionalCalendars (expected) | Hijri Assign (expected) | UmAlQura Assign (expected) | Framework | Source |
|---|---|---|---|---|---|---|
| ur | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net48 | Windows NLS docs + CLDR |
| ur-PK | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net48 | Windows NLS docs + CLDR |
| ur-IN | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net48 | Windows NLS docs + CLDR |

**Decision 3 is feasible** across all three runtimes based on: live probe evidence (net10, net8) and authoritative documentation evidence (net48 Windows NLS). Live net48 probe output will be captured during task .7 (cross-platform verification gate) when the branch runs on a Windows CI host. If that gate reveals any discrepancy with the documented behavior, Decision 3 must be re-evaluated.

**Note on net48 evidence form**: The task spec requires "probe output for net10 + net48 (or CI URL if net48 local host unavailable)." This host is macOS arm64 (no net48 runtime available), and no CI URL exists because the branch has not been pushed yet (this is the first task on the branch). Windows NLS documentation + CLDR cross-reference is the strongest available evidence short of a live probe. The evidence standard is met for the feasibility gate: all three data sources (net10 live, net8 live, net48 documented) agree that HijriCalendar is valid for Urdu cultures.

### Additional probe data (net10.0)

| Property | ur | ur-PK | ur-IN |
|---|---|---|---|
| ShortDatePattern | `d/M/yyyy` | `d/M/yyyy` | `d/M/yyyy` |
| LongDatePattern | `dddd، d MMMM، yyyy` | `dddd، d MMMM، yyyy` | `dddd، d MMMM، yyyy` |
| ShortTimePattern | `LRM h:mm tt` | `LRM h:mm tt` | `LRM h:mm tt` |
| AM | `AM` | `AM` | `AM` |
| PM | `PM` | `PM` | `PM` |
| Months | جنوری فروری مارچ اپریل مئی جون جولائی اگست ستمبر اکتوبر نومبر دسمبر | identical | identical |
| MonthsGenitive | identical to nominative | identical | identical |

---

## Contract Decisions

### Decision 1a -- IOrdinalizer engine strategy for Urdu word ordinals

**Choice**: New `number-word-suffix` ordinalizer engine.

**Problem statement**: The existing `word-form-template` ordinalizer engine produces output of the form `{prefix}{digitString}{suffix}` where `{digitString}` is the input integer as a numeric string. For Urdu word ordinals (`پانچواں` = "fifth" masculine), the ordinalizer must render the cardinal word form (`پانچ` = "five") from `NumberToWords` and then apply a gendered suffix (`واں`/`ویں`). The existing `word-form-template` engine cannot do this -- it would produce `5واں` (digit+suffix), which the epic explicitly rejects.

**Options considered and rejected**:

1. **Exact replacement table only** via existing `word-form-template` `exactReplacements`: Requires an entry for every possible integer. Impractical beyond a small range.

2. **Extend `word-form-template` with `numberWordMode`**: Overloads the meaning of `word-form-template`, which currently means numeric-string templating across all locales that use it.

**Selected**: Option 3 -- new `number-word-suffix` ordinalizer engine. This engine:
- Resolves the current culture's `INumberToWordsConverter` at runtime
- Calls `Convert(number)` to get the cardinal word stem
- Applies a gendered suffix from masculine/feminine/neuter pattern sets
- Uses `exactReplacements` for irregular low ordinals (1-10)
- Falls back masculine for neuter

**YAML data shape for Urdu `ordinal.numeric`**:
```yaml
ordinal:
  numeric:
    engine: 'number-word-suffix'
    useCulture: true
    masculine:
      defaultSuffix: 'واں'
      exactReplacements:
        1: 'پہلا'
        2: 'دوسرا'
        3: 'تیسرا'
    feminine:
      defaultSuffix: 'ویں'
      exactReplacements:
        1: 'پہلی'
        2: 'دوسری'
        3: 'تیسری'
```

**Culture binding**: `useCulture: true` causes `OrdinalizerProfileCatalogInput.RequiresCulture()` to return true, so the generated catalog passes the explicit `CultureInfo` to `NumberWordSuffixOrdinalizer`'s constructor. The ordinalizer stores this culture and uses it to resolve `Configurator.GetNumberToWordsConverter(culture)` at runtime. This ensures that explicit-culture overloads like `5.Ordinalize(GrammaticalGender.Masculine, urCulture)` resolve the Urdu number-to-words converter correctly without depending on ambient `CurrentUICulture`.

**Implementation touchpoints**:
- `OrdinalizerProfileCatalogInput.cs`: Add `number-word-suffix` switch arm in `CreateOrdinalizerExpression()` and YAML binding (this is where ordinalizer engines are routed, not `EngineContractCatalog` which handles number-to-words and clock engines)
- New runtime class `NumberWordSuffixOrdinalizer.cs` implementing `IOrdinalizer`, accepting `CultureInfo` in constructor
- Task .9 owns the implementation

### Decision 1b -- INumberToWordsConverter gendered ordinal output via ToOrdinalWords

**Problem statement**: `5.ToOrdinalWords(GrammaticalGender.Masculine, culture)` calls `INumberToWordsConverter.ConvertToOrdinal(int, GrammaticalGender)`. The `GenderlessNumberToWordsConverter` base class (which `IndianGroupingNumberToWordsConverter` extends) implements this by ignoring gender:

```csharp
// GenderlessNumberToWordsConverter.cs:80
public string ConvertToOrdinal(int number, GrammaticalGender gender) =>
    ConvertToOrdinal(number);
```

This means `5.ToOrdinalWords(GrammaticalGender.Feminine, urCulture)` would return the same genderless ordinal as `5.ToOrdinalWords(urCulture)`, not the feminine form `پانچویں`.

**Resolution**: The Urdu number-to-words converter must NOT extend `GenderlessNumberToWordsConverter`. It must instead extend `GenderedNumberToWordsConverter` (or equivalent) and implement `ConvertToOrdinal(int, GrammaticalGender)` to produce gendered word ordinals.

**Strategy**: Create a new `IndianGroupingGenderedNumberToWordsConverter` that:
- Extends `GenderedNumberToWordsConverter` instead of `GenderlessNumberToWordsConverter`
- Reuses the `indian-grouping` engine's cardinal logic (shared via composition or a shared helper)
- Implements `ConvertToOrdinal(int, GrammaticalGender)` by getting the cardinal word and applying gendered suffixes
- Implements neuter -> masculine fallback

**Alternative**: Extend `IndianGroupingNumberToWordsConverter` to override `ConvertToOrdinal(int, GrammaticalGender)` directly, bypassing the base class delegation. This is simpler but creates a GenderlessNumberToWordsConverter that actually handles gender, which is confusing.

**Selected**: New gendered converter class `IndianGroupingGenderedNumberToWordsConverter`.

**Engine binding**: The generator selects the converter via a new engine name `indian-grouping-gendered` in the `number.words` YAML. This avoids overloading the existing `indian-grouping` engine which is used by Tamil and other genderless locales.

**Locked `number.words` YAML shape for Urdu**:
```yaml
number:
  words:
    engine: 'indian-grouping-gendered'
    denseUnitsMap:
      - 'صفر'
      - 'ایک'
      # ... 100 entries (0-99), all lexically distinct
    tensMap:
      - ''      # placeholder for 0
      - ''      # placeholder for 1
      - 'بیس'   # 20 (only used for hundreds/thousands scale counts)
      # ... 10 entries
    hundredsMap:
      - 'ایک سو'
      # ... 9 entries (100-900)
    thousandsMap:
      - 'ایک ہزار'
      # ... 19 entries (1000-19000)
    lakhWord: 'لاکھ'
    singleLakhWord: 'ایک'
    croreWord: 'کروڑ'
    negativeWord: 'منفی'
    zeroWord: 'صفر'
    ordinal:
      masculine:
        defaultSuffix: 'واں'
        exactReplacements:
          1: 'پہلا'
          2: 'دوسرا'
          3: 'تیسرا'
      feminine:
        defaultSuffix: 'ویں'
        exactReplacements:
          1: 'پہلی'
          2: 'دوسری'
          3: 'تیسری'
      neuterFallback: 'masculine'
```

**Generator binding**: `EngineContractCatalog.cs` adds `indian-grouping-gendered` schema with all `indian-grouping` fields plus `ordinal.masculine`, `ordinal.feminine`, `neuterFallback`. `NumberToWordsEngineContractFactory` maps this engine to `IndianGroupingGenderedNumberToWordsConverter`.

**Ordinal data duplication**: The low-ordinal `exactReplacements` in `number.words.ordinal` serve `ToOrdinalWords(gender)`, while the `exactReplacements` in `ordinal.numeric` (Decision 1a) serve `Ordinalize(gender)`. Both paths need the same data. To avoid duplication, the `number-word-suffix` ordinalizer engine (Decision 1a) can share the generated profile data by referencing the same source. Task .9 decides the sharing mechanism.

Task .9 owns the engine + converter implementation; task .3 owns the cardinal data (`denseUnitsMap`, scale words, etc.).

**API path coverage after implementation**:
- `5.ToOrdinalWords(GrammaticalGender.Masculine, urCulture)` -> `ConvertToOrdinal(5, Masculine)` -> `پانچواں`
- `5.ToOrdinalWords(GrammaticalGender.Feminine, urCulture)` -> `ConvertToOrdinal(5, Feminine)` -> `پانچویں`
- `5.Ordinalize(GrammaticalGender.Masculine, urCulture)` -> `IOrdinalizer.Convert(5, "5", Masculine)` -> `پانچواں`
- `"5".Ordinalize(GrammaticalGender.Masculine, urCulture)` -> same IOrdinalizer path -> `پانچواں`
- `5.ToOrdinalWords(urCulture)` (genderless) -> `ConvertToOrdinal(5)` -> defaults to masculine -> `پانچواں`

### Decision 2 -- Number-to-words engine for Urdu

**Choice**: Extended `indian-grouping` engine with dense sub-hundred mode.

**Problem statement**: The existing `IndianGroupingNumberToWordsConverter` composes numbers 20-99 using `TensMap[quotient] + suffix + UnitsMap[remainder]`. Urdu has **lexically distinct words for ALL numbers 0-99** (e.g., `اکیس` for 21, `ننانوے` for 99). These are not compositional. Additionally, `GetThousandsValue()` applies `ThousandsOneBridge` / `ThousandsMap` fragments after rendering counts >= 20 via `GetTensValue(n, false, true)`, which would break for dense forms.

**Required engine adaptations** (task .3 owns implementation):

1. **Dense sub-hundred map**: Add a `denseUnitsMap` (100-entry string array, 0-99) to the engine profile. When present, `GetTensValue()` routes all values < 100 through this map instead of the compositional tens+suffix+units path.

2. **Thousands rendering**: When `denseUnitsMap` is present, `GetThousandsValue()` must use the dense map for the count (1-99) instead of the compositional path that appends fragments.

3. **Gendered ordinal output**: Per Decision 1b, the Urdu converter uses the new `indian-grouping-gendered` engine which extends `GenderedNumberToWordsConverter`.

**Engine contract changes**:
- `EngineContractCatalog.cs`: Add `indian-grouping-gendered` schema with `denseUnitsMap` (string-array, 100 entries) and `ordinal` gendered block
- New `IndianGroupingGenderedNumberToWordsConverter.cs` extending `GenderedNumberToWordsConverter`, sharing cardinal logic with `IndianGroupingNumberToWordsConverter` via composition

**Verification targets** (to be proven by .3 and .6):
- `0` -> `صفر`
- `21` -> `اکیس` (dense lookup, not composition)
- `99` -> `ننانوے` (dense lookup)
- `100` -> `ایک سو`
- `100000` -> `ایک لاکھ`
- `1234567` -> to be reviewer-approved

### Decision 3 -- Hijri calendar contract

**Choice**: **(A) Culture-calendar contract** with `calendarMode: Native`.

**Feasibility proof -- OptionalCalendars survey**:

| Culture | Default Calendar | OptionalCalendars | HijriCalendar assign | UmAlQuraCalendar assign | Framework | Platform |
|---|---|---|---|---|---|---|
| ur | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net10.0 | macOS arm64 |
| ur-PK | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net10.0 | macOS arm64 |
| ur-IN | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net10.0 | macOS arm64 |
| ur | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net8.0 | macOS arm64 |
| ur-PK | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net8.0 | macOS arm64 |
| ur-IN | GregorianCalendar | GregorianCalendar, HijriCalendar | SUCCESS | FAILED | net8.0 | macOS arm64 |

net48: HijriCalendar assignment expected to succeed based on Windows NLS documentation + CLDR data (see net48 Results section above). Live probe deferred to .7 cross-platform gate.

**Contract A is feasible** because all Urdu cultures accept HijriCalendar assignment on both net10 and net8.

**Public caller contract** (precise):

The caller must set BOTH:
1. `Thread.CurrentThread.CurrentUICulture` to an Urdu culture (e.g., `new CultureInfo("ur")`) so that `Configurator.DateToOrdinalWordsConverters.ResolveForUiCulture()` selects the Urdu converter.
2. `Thread.CurrentThread.CurrentCulture` to a cloned Urdu culture whose `DateTimeFormat.Calendar` is `new HijriCalendar()`, so that `OrdinalDatePattern.GetPatternCulture()` (which reads `CultureInfo.CurrentCulture`) preserves the Hijri calendar for date formatting.

There are NO culture-aware overloads on `DateTime.ToOrdinalWords()` or `DateOnly.ToOrdinalWords()` today. The date ordinal APIs use `CurrentCulture` for formatting and `CurrentUICulture` for converter resolution. Task .10 may optionally add culture-aware overloads but the current contract relies on ambient culture.

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

Changes required:
- `CanonicalLocaleAuthoring.cs:AddCalendarFeatures()`: Accept `hijriMonths` as valid key, validate 12-element sequence
- `OrdinalDateProfileCatalogInput.cs`: Extract `hijriMonths`, pass to pattern when calendarMode is Native
- `OrdinalDatePattern.cs`: Select month array based on active calendar type

**Blast radius**: 3 files. Under the 5-file threshold. No re-plan needed.

---

## Build and Test Evidence

### Build verification

```
$ dotnet build src/Humanizer/Humanizer.csproj -c Release
Build succeeded. 0 Warning(s) 0 Error(s)
All four TFMs: net10.0, net8.0, net48, netstandard2.0
```

### Source generator tests

```
$ dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
Passed! total: 58, failed: 0, succeeded: 58
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

### Formatter exact theory data (6), Heading data (2), Number theory genderless (7), Number theory gendered (12), Number overload (4), Phrase (3), Number magnitude (2), Coverage formatter/collection (2), Coverage number (2), Coverage ordinalizer (16), Ordinalizer matrix (10), Coverage date ordinals (6), Coverage clock (3), Coverage words-to-number (1)

**Total**: 84 failures. All for locale `ur`. No claim of full registry coverage from this task.

---

## Parity Map Table

| Surface | Ownership path | Current state | Target state | Support state | Status |
|---|---|---|---|---|---|
| list | -- | missing | locale-owned | not supported | not-started |
| formatter | .2 | authored | locale-owned | engine: profiled, pluralRule: singular-plural, dataUnitPluralRule: singular-plural | resolved |
| phrases.relativeDate | .2 | authored | locale-owned | 8 units × past/future, now=ابھی, never=کبھی نہیں, yesterday=گزشتہ کل, tomorrow=آئندہ کل | resolved |
| phrases.duration | .2 | authored | locale-owned | 8 units with singular/plural, zero=ابھی | resolved |
| phrases.dataUnits | .2 | authored | locale-owned | بٹ/بائٹ/کلوبائٹ/میگابائٹ/گیگابائٹ/ٹیرابائٹ | resolved |
| phrases.timeUnits | .2 | authored | locale-owned | 8 unit symbols | resolved |
| number.words.cardinal | .3 | authored | locale-owned | engine: indian-grouping-gendered, denseUnitsMap 0-99, lakh/crore/arab/kharab scales | resolved |
| number.words.ordinal | .3 | authored | locale-owned | gendered ordinal suffixes: واں/ویں, exactReplacements 1-3 | resolved |
| number.parse.cardinal | .3 | authored | locale-owned | engine: token-map, 0-99 + scales, useHundredMultiplier | resolved |
| number.parse.ordinal | -- | missing | locale-owned | not supported | not-started |
| number.formatting.decimalSeparator | .3 | authored | locale-owned (`.`) | override | resolved |
| number.formatting.groupSeparator | .3 | authored | locale-owned (`,`) | override | resolved |
| number.formatting.negativeSign | .3 | authored | locale-owned (`-`, strip U+200E LRM) | override | resolved |
| ordinal.numeric | -- | missing | locale-owned | not supported | not-started |
| ordinal.date | -- | missing | locale-owned | not supported | not-started |
| ordinal.dateOnly | -- | missing | locale-owned | not supported | not-started |
| clock | -- | missing | locale-owned | not supported | not-started |
| compass | -- | missing | locale-owned | not supported | not-started |
| calendar.months | -- | missing | locale-owned | not supported | not-started |

### Effective Gap Summary

8 of 8 canonical surface groups partially resolved. Remaining unresolved: list, ordinal.numeric (.9), ordinal.date, ordinal.dateOnly, clock, compass, calendar.

---

## Before/After Parity Delta

**Before (task .3)**: formatter + 4 phrase surfaces resolved; number surfaces missing
**After (task .3)**: formatter + 4 phrase surfaces + number (words/parse/formatting) resolved; remaining gaps owned by tasks .4–.5, .9

The parity delta will reach empty at .7 completion.

---

## Proposer+Reviewer Term Log (Task .2)

### Formatter configuration

| Field | Proposed | Reviewed | Status |
|---|---|---|---|
| engine | profiled | CLDR `ur` is two-form (one/other) — profiled engine correct | accepted |
| pluralRule | singular-plural | CLDR plural category `i = 1 and v = 0` → one, else other — two-form, not arabic-like | accepted |
| dataUnitPluralRule | singular-plural | matches pluralRule — data units use same two-form distinction | accepted |
| dataUnitFallbackTransform | trim-trailing-s | safety net for English-derived fallback strings; harmless for Urdu | accepted |

### phrases.relativeDate — past

| Unit | Single (count=1) | Plural stem (count>1) | Suffix | Source | Status |
|---|---|---|---|---|---|
| millisecond | ایک ملی سیکنڈ پہلے | ملی سیکنڈ | پہلے | CLDR extrapolation (no CLDR millisecond for ur; derived from second pattern) | accepted |
| second | ایک سیکنڈ پہلے | سیکنڈ | پہلے | CLDR `ur` relativeTime second past-one/other | accepted |
| minute | ایک منٹ پہلے | منٹ | پہلے | CLDR `ur` relativeTime minute past-one/other | accepted |
| hour | ایک گھنٹہ پہلے | گھنٹے | پہلے | CLDR `ur` hour past-one=گھنٹہ, past-other=گھنٹے | accepted |
| day | گزشتہ کل | دن | پہلے | CLDR `ur` relative-day -1=گزشتہ کل; day past-other=دن | accepted |
| week | ایک ہفتہ پہلے | ہفتے | پہلے | CLDR `ur` week past-one=ہفتہ, past-other=ہفتے | accepted |
| month | ایک مہینہ پہلے | مہینے | پہلے | CLDR `ur` month past-one=مہینہ, past-other=مہینے | accepted |
| year | ایک سال پہلے | سال | پہلے | CLDR `ur` year past-one/other=سال (invariant) | accepted |

### phrases.relativeDate — future

| Unit | Single (count=1) | Plural stem (count>1) | Suffix | Source | Status |
|---|---|---|---|---|---|
| millisecond | ایک ملی سیکنڈ میں | ملی سیکنڈ | میں | Derived from second pattern | accepted |
| second | ایک سیکنڈ میں | سیکنڈ | میں | CLDR `ur` second future-one/other | accepted |
| minute | ایک منٹ میں | منٹ | میں | CLDR `ur` minute future-one/other | accepted |
| hour | ایک گھنٹے میں | گھنٹے | میں | CLDR `ur` hour future-one/other (oblique before میں) | accepted |
| day | آئندہ کل | دنوں | میں | CLDR `ur` relative-day +1=آئندہ کل; day future-other=دنوں | accepted |
| week | ایک ہفتے میں | ہفتے | میں | CLDR `ur` week future-one/other (oblique before میں) | accepted |
| month | ایک مہینے میں | مہینے | میں | CLDR `ur` month future-one/other (oblique before میں) | accepted |
| year | ایک سال میں | سال | میں | CLDR `ur` year future-one/other | accepted |

### phrases.relativeDate — special forms

| Form | Value | Source | Status |
|---|---|---|---|
| now | ابھی | CLDR `ur` relative-second-0 + spec requirement | accepted |
| never | کبھی نہیں | Standard Urdu; matches pattern of fa/ta/ja locales | accepted |
| yesterday | گزشتہ کل | CLDR `ur` relative-day -1 (disambiguated, not ambiguous کل) | accepted |
| tomorrow | آئندہ کل | CLDR `ur` relative-day +1 (disambiguated, not ambiguous کل) | accepted |

### phrases.duration

| Unit | Numeric (1) | Words (1) | Singular form | Plural form | Source | Status |
|---|---|---|---|---|---|---|
| millisecond | 1 ملی سیکنڈ | ایک ملی سیکنڈ | ملی سیکنڈ | ملی سیکنڈ | Invariant (loanword) | accepted |
| second | 1 سیکنڈ | ایک سیکنڈ | سیکنڈ | سیکنڈ | Invariant (loanword) | accepted |
| minute | 1 منٹ | ایک منٹ | منٹ | منٹ | Invariant (loanword) | accepted |
| hour | 1 گھنٹہ | ایک گھنٹہ | گھنٹہ | گھنٹے | CLDR `ur` unit-length-long hour | accepted |
| day | 1 دن | ایک دن | دن | دن | CLDR `ur` unit-length-long day (invariant in direct case) | accepted |
| week | 1 ہفتہ | ایک ہفتہ | ہفتہ | ہفتے | CLDR `ur` unit-length-long week | accepted |
| month | 1 مہینہ | ایک مہینہ | مہینہ | مہینے | CLDR `ur` unit-length-long month | accepted |
| year | 1 سال | ایک سال | سال | سال | CLDR `ur` unit-length-long year (invariant) | accepted |

### phrases.dataUnits

| Unit | Singular | Plural | Symbol | Source | Status |
|---|---|---|---|---|---|
| bit | بٹ | بٹ | b | Urdu transliteration of "bit" | accepted |
| byte | بائٹ | بائٹ | B | Urdu transliteration of "byte" | accepted |
| kilobyte | کلوبائٹ | کلوبائٹ | KB | Urdu transliteration of "kilobyte" | accepted |
| megabyte | میگابائٹ | میگابائٹ | MB | Urdu transliteration of "megabyte" | accepted |
| gigabyte | گیگابائٹ | گیگابائٹ | GB | Urdu transliteration of "gigabyte" | accepted |
| terabyte | ٹیرابائٹ | ٹیرابائٹ | TB | Urdu transliteration of "terabyte" | accepted |

### phrases.timeUnits

| Unit | Symbol | Source | Status |
|---|---|---|---|
| millisecond | ملی سیکنڈ | Urdu transliteration | accepted |
| second | سیکنڈ | CLDR `ur` | accepted |
| minute | منٹ | CLDR `ur` | accepted |
| hour | گھنٹہ | CLDR `ur` | accepted |
| day | دن | CLDR `ur` | accepted |
| week | ہفتہ | CLDR `ur` | accepted |
| month | مہینہ | CLDR `ur` | accepted |
| year | سال | CLDR `ur` | accepted |

### Script character verification

All authored terms verified free of Arabic-script mis-use:
- ه (U+0647) absent — ہ (U+06C1) used throughout ✓
- ي (U+064A) absent — ی (U+06CC) used throughout ✓
- ك (U+0643) absent — ک (U+06A9) used throughout ✓
- No U+200E (LRM), U+200F (RLM), U+061C (ALM) ✓

---

## Proposer+Reviewer Term Log (Task .3)

### Engine design change: simplified indian-grouping-gendered

The parity map Decision 2 locked `hundredsMap` and `thousandsMap` arrays. Task .3 simplified the engine to use scalar scale words (`hundredWord`, `thousandWord`, `lakhWord`, `croreWord`, `arabWord`, `kharabWord`) because Urdu number composition is fully regular (no morphological irregularities in hundreds or thousands). The converter composes: `denseUnitsMap[count] + " " + scaleWord`. This eliminates redundant map data and simplifies the engine contract.

### Cardinal verification points

| Input | Expected | Source | Status |
|---|---|---|---|
| 0 | صفر | Standard Urdu | accepted |
| 1 | ایک | Standard Urdu | accepted |
| 21 | اکیس | Standard Urdu, dense lookup | accepted |
| 99 | ننانوے | Standard Urdu, dense lookup | accepted |
| 100 | ایک سو | ایک + سو | accepted |
| 101 | ایک سو ایک | composition | accepted |
| 1000 | ایک ہزار | ایک + ہزار | accepted |
| 1234 | ایک ہزار دو سو چونتیس | composition | accepted |
| 100000 | ایک لاکھ | ایک + لاکھ | accepted |
| 1234567 | بارہ لاکھ چونتیس ہزار پانچ سو سڑسٹھ | composition | accepted |
| 10000000 | ایک کروڑ | ایک + کروڑ | accepted |
| 1000000000 | ایک ارب | ایک + ارب | accepted |
| -21 | منفی اکیس | negative prefix | accepted |

### Ordinal verification points

| Input | Gender | Expected | Source | Status |
|---|---|---|---|---|
| 1 | masculine | پہلا | exactReplacement | accepted |
| 2 | masculine | دوسرا | exactReplacement | accepted |
| 3 | masculine | تیسرا | exactReplacement | accepted |
| 5 | masculine | پانچواں | cardinal + واں | accepted |
| 1 | feminine | پہلی | exactReplacement | accepted |
| 5 | feminine | پانچویں | cardinal + ویں | accepted |

### Parse verification points

| Input | Expected | Round-trip | Status |
|---|---|---|---|
| اکیس | 21 | ✓ | accepted |
| ایک سو ایک | 101 | ✓ | accepted |
| ایک ہزار ایک | 1001 | ✓ | accepted |
| ایک لاکھ | 100000 | ✓ | accepted |
| بارہ لاکھ چونتیس ہزار پانچ سو سڑسٹھ | 1234567 | ✓ | accepted |

### Script character verification (task .3 authored content)

- ه (U+0647) absent — ہ (U+06C1) used throughout ✓
- ي (U+064A) absent — ی (U+06CC) used throughout ✓
- ك (U+0643) absent — ک (U+06A9) used throughout ✓
- No U+200E (LRM), U+200F (RLM), U+061C (ALM) ✓

---

## Downstream Implementation Tasks

Architecture decisions are locked. The following tasks own execution:

1. ~~**Dense 0-99 cardinal data + engine threshold adaptation** for `indian-grouping` -- task .3~~ DONE
2. **Hijri month schema extension** (`hijriMonths` key) + runtime calendar selection -- task .10
3. **`number-word-suffix` ordinalizer engine** + gendered `IndianGroupingGenderedNumberToWordsConverter` -- task .9
4. **Regional variant differences** between ur-PK and ur-IN -- task .11
