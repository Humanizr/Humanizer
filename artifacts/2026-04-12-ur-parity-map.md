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

### Environment (net10.0 on macOS 26.4.1)

```
.NET: 10.0.2
OS: macOS 26.4.1
RID: osx-arm64
Timestamp: 2026-04-12T22:07:09Z
```

### ur (neutral)

| Property | Value |
|---|---|
| DecimalSeparator | `.` (U+002E) |
| GroupSeparator | `,` (U+002C) |
| NegativeSign | `LRM -` (U+200E U+002D) |
| ShortDatePattern | `d/M/yyyy` |
| LongDatePattern | `dddd، d MMMM، yyyy` |
| ShortTimePattern | `LRM h:mm tt` |
| AM | `AM` |
| PM | `PM` |
| Calendar | GregorianCalendar |
| OptionalCalendars | GregorianCalendar, HijriCalendar |
| HijriCalendar assignment | SUCCESS |
| UmAlQuraCalendar assignment | FAILED (not valid for culture) |
| Months | جنوری فروری مارچ اپریل مئی جون جولائی اگست ستمبر اکتوبر نومبر دسمبر |
| MonthsGenitive | (same as nominative) |

### ur-PK

| Property | Value |
|---|---|
| DecimalSeparator | `.` (U+002E) |
| GroupSeparator | `,` (U+002C) |
| NegativeSign | `LRM -` (U+200E U+002D) |
| ShortDatePattern | `d/M/yyyy` |
| LongDatePattern | `dddd، d MMMM، yyyy` |
| Calendar | GregorianCalendar |
| OptionalCalendars | GregorianCalendar, HijriCalendar |
| HijriCalendar assignment | SUCCESS |
| UmAlQuraCalendar assignment | FAILED |
| Months | (identical to ur) |

### ur-IN

| Property | Value |
|---|---|
| DecimalSeparator | `.` (U+002E) |
| GroupSeparator | `,` (U+002C) |
| NegativeSign | `LRM -` (U+200E U+002D) |
| ShortDatePattern | `d/M/yyyy` |
| LongDatePattern | `dddd، d MMMM، yyyy` |
| Calendar | GregorianCalendar |
| OptionalCalendars | GregorianCalendar, HijriCalendar |
| HijriCalendar assignment | SUCCESS |
| UmAlQuraCalendar assignment | FAILED |
| Months | (identical to ur) |

### net48 Probe

net48 probe requires a Windows host. Not available locally on macOS. Will be verified via CI.

---

## Contract Decisions

### Decision 1a -- IOrdinalizer engine strategy for Urdu word ordinals

**Choice**: Extend `word-form-template` engine with an `exactReplacements` table for Urdu.

**Rationale**: The `word-form-template` engine already supports `masculine`, `feminine`, and `neuter` pattern sets with `exactReplacements` dictionaries (see `OrdinalizerProfileCatalogInput.cs:201-208`). Urdu word ordinals (e.g., `پہلا` / `پہلی` for 1st masc/fem, `دوسرا` / `دوسری` for 2nd) follow a pattern where most ordinals are formed by suffix attachment to the cardinal stem, but the low ordinals (1-10) are highly irregular. The `exactReplacements` field maps integer values to exact output strings, which handles the irregular range. For regular ordinals (11+), the `defaultSuffix` field (`واں` masc / `ویں` fem) appended to the cardinal-rendered number achieves the correct output.

However, the `word-form-template` engine today renders `{number}{suffix}` where `{number}` is the input number as a digit string. To produce word ordinals like `پانچواں` (not `5واں`), the engine would need to call through `NumberToWords` at runtime. This is a runtime extension that belongs to task .9.

**Implementation task**: .9 (Wire gendered ordinals end-to-end)

### Decision 1b -- INumberToWordsConverter gendered ordinal output

**Choice**: The `INumberToWordsConverter.ConvertToOrdinal(int)` method is genderless (`GenderlessNumberToWordsConverter` base). Urdu needs gendered ordinals via `ToOrdinalWords(GrammaticalGender)`, which routes through `INumberToWordsConverter` selected by the `number.words` engine.

The current `IndianGroupingNumberToWordsConverter` extends `GenderlessNumberToWordsConverter` and its `ConvertToOrdinal` produces a cardinal+suffix output, not gendered word ordinals. For Urdu, the gendered ordinal output (`پانچواں` vs `پانچویں`) must come from the ordinalizer path (Decision 1a), not from the number-to-words converter.

**Strategy**: The `ToOrdinalWords(GrammaticalGender, CultureInfo)` extension method does NOT exist today -- `ToOrdinalWords` on numbers goes through `NumberToWordsExtension.ToOrdinalWords(int)` which calls `ConvertToOrdinal(int)` on `INumberToWordsConverter`. The gendered ordinal behavior routes through `Ordinalize(GrammaticalGender, CultureInfo)` which calls `IOrdinalizer.Convert(int, string, GrammaticalGender)`.

**Resolution**: Task .9 owns both:
1. Extending the ordinalizer engine to produce word ordinals (calling `NumberToWords` internally for the cardinal stem)
2. Ensuring `5.Ordinalize(GrammaticalGender.Masculine, urCulture)` returns `پانچواں` and `5.ToOrdinalWords(urCulture)` returns a sensible ordinal

### Decision 2 -- Number-to-words engine for Urdu

**Choice**: `indian-grouping` engine.

**Inspection**: The `IndianGroupingNumberToWordsConverter` supports:
- Dense irregular 0-19 via `UnitsMap` (20 entries)
- Tens 20-90 via `TensMap`
- Hundreds via `HundredsMap` (9 entries for 100-900)
- Thousands via `ThousandsMap` (19 entries for 1000-19000)
- Lakh (100,000) and Crore (10,000,000) scales with lakh/crore words
- Quintillion and Quadrillion for extreme ranges
- Suffix-based ordinals via `OrdinalSuffix` and `OrdinalExceptions`

Urdu uses the South Asian number grouping system: ones, tens, hundreds, thousands, then lakhs (1,00,000) and crores (1,00,00,000). This matches the `indian-grouping` engine exactly.

The engine requires dense lookup for 0-19 in `UnitsMap`. Urdu has distinct words for 0-19 plus the unique decade values 20-90. Numbers 21-99 (excluding exact tens) are formed as `{tens}{suffix}{units}` which the engine handles via `GetTensValue`.

For Urdu the tens + units composition uses a space or suffix pattern that the engine provides. The engine contract fields `defaultTensWithRemainderSuffix`, `specialTensWithRemainderSuffix`, etc. control the morphological joining.

**Verification targets** (to be proven by .3 and .6):
- `21` -> `اکیس` (this is actually a dense irregular -- Urdu has unique words for 21-99)
- `99` -> `ننانوے`
- `100` -> `ایک سو`
- `100000` -> `ایک لاکھ`
- `1234567` -> to be reviewer-approved

**Key note**: Urdu, like Hindi, has distinct words for ALL numbers 0-99 (not just 0-19 and then compositional tens+units). This means the `UnitsMap` will need to be extended beyond the standard 20 entries, or the dense 21-99 words must be handled via the tens+suffix+units composition. The `indian-grouping` engine in Tamil uses a dense `UnitsMap` of 20 entries plus compositional tens. For Urdu, the dense irregularity extends through 99.

**Engine adaptation required**: The `UnitsMap` field is currently a `string[]` indexed by value (0-19). To handle Urdu's dense 0-99, the `indian-grouping` engine contract may need a `unitsMap` array of 100 entries (0-99) OR the `tensMap` plus the compositional suffix system can encode the 21-99 range if the composition rules match Urdu morphology. Task .3 owns this investigation.

### Decision 3 -- Hijri calendar contract

**Choice**: **(A) Culture-calendar contract** with `calendarMode: Native`.

**Feasibility proof**:

The probe results confirm:
- All three Urdu cultures (`ur`, `ur-PK`, `ur-IN`) list `HijriCalendar` in `OptionalCalendars`
- `HijriCalendar` assignment to `DateTimeFormat.Calendar` succeeds for all three cultures
- `UmAlQuraCalendar` is NOT valid for any Urdu culture

The existing `OrdinalDateCalendarMode` enum already has `Native` (see `OrdinalDateCalendarMode.cs:9`). The `OrdinalDatePattern.GetPatternCulture()` method already implements this: when `calendarMode == OrdinalDateCalendarMode.Native`, it returns the culture as-is without forcing Gregorian (see `OrdinalDatePattern.cs:258-261`).

**Mechanism**: The caller sets `CurrentCulture` to a culture whose `DateTimeFormat.Calendar` is `HijriCalendar`, then calls `DateTime.ToOrdinalWords()`. The YAML ordinal date profile sets `calendarMode: Native`. The `OrdinalDatePattern` skips Gregorian forcing, and `DateTime.ToString()` uses the Hijri calendar natively. Month names come from the `calendar.months` override array indexed by the Hijri month number.

**Schema extension**: Add `calendar.hijriMonths` as a new 12-element sequence in the `calendar` surface. The `OrdinalDatePattern.SubstituteMonth()` method already handles month override arrays; the generator (`OrdinalDateProfileCatalogInput.cs:33-34`) already extracts `calendar.months` and passes them to the pattern. For Hijri, a separate month array is needed because the Hijri months are completely different from the Gregorian months.

**Alternative considered and deferred**: A `calendar.months: { gregorian, hijri }` calendar-keyed map was considered but adds complexity to the `AddCalendarFeatures` validator (which currently expects `months` to be a flat 12-element sequence). The simpler approach is a new `hijriMonths` key at the same level as `months`.

**Implementation note**: Task .10 owns the implementation. The `AddCalendarFeatures` method in `CanonicalLocaleAuthoring.cs` needs to accept `hijriMonths` as a valid key. The `OrdinalDateProfileCatalogInput` needs to extract and pass Hijri months when the calendarMode is Native and a Hijri calendar is in use. The `OrdinalDatePattern` needs to select the correct month array based on the active calendar.

**Blast radius**: 3 files: `CanonicalLocaleAuthoring.cs` (validator), `OrdinalDateProfileCatalogInput.cs` (generator), `OrdinalDatePattern.cs` (runtime). Under the 5-file threshold. No re-plan needed.

---

## LocaleTheoryMatrixCompletenessTests Failure Output (.6 Driver List)

84 failures for locale `ur` across all `ShippedLocaleRows` datasets. Full list:

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

1. Dense 0-99 handling in `indian-grouping` engine for Urdu -- task .3
2. Exact Hijri month override schema extension -- task .10
3. Word-ordinal engine extension for `word-form-template` -- task .9
4. Regional variant differences between ur-PK and ur-IN -- task .11
