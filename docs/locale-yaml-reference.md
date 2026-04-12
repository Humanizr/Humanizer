# Locale YAML Reference

This document is the authoritative authoring reference for `src/Humanizer/Locales/*.yml`.

If you are adding or changing locale-owned generated behavior, read this document together with [Locale YAML How-To](./locale-yaml-how-to.md) and [Adding Or Updating A Locale](./adding-a-locale.md).

Taken together, those three documents are intended to fully describe the checked-in locale contract without consulting generator source code.

## Scope

These YAML files are the single checked-in authoring surface for locale-owned generated behavior.

They are consumed only at build time by `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`.

They are not parsed at runtime.

## File-Level Rules

1. There is exactly one locale YAML file per locale code.
2. The file name is the locale code, for example `en.yml`, `en-US.yml`, `pt-BR.yml`.
3. Top-level properties are limited to:
   - `locale`
   - `variantOf`
   - `surfaces`
4. Canonical surface names under `surfaces` are limited to:
   - `list`
   - `formatter`
   - `phrases`
   - `number`
   - `ordinal`
   - `clock`
   - `compass`
   - `calendar`
5. Unknown top-level keys are rejected by the generator.
6. Locale words, switches, tables, and strategy choices belong here.
7. Generator implementation contracts do not belong here.
8. Shipped locales are expected to resolve both `number.words` and `number.parse`, either locale-owned or same-language inherited with proof.
9. `number.formatting` is a nested member under `number`, alongside `number.words` and `number.parse`.

## Merge Rules

Locale inheritance is resolved during code generation.

The rules are:

1. Omitting a `surfaces.<surface>` block inherits the parent surface unchanged.
2. A scalar child value replaces the parent scalar value.
3. A sequence child value replaces the parent sequence value.
4. A mapping child value merges recursively with the parent mapping.
5. If a child mapping changes `engine`, that mapping replaces the parent mapping entirely.
6. Do not use empty mappings to request built-in behavior. If a surface would only say "use the default", omit the block instead. Exception: `engine: 'default'` is permitted on `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, and `clock` surfaces to explicitly opt into the built-in engine.

That means regional variants can now override only the fields they actually differ on.
It does not mean inherited surfaces are automatically proved for parity purposes.

Example:

```yaml
locale: 'en-IN'
variantOf: 'en'

surfaces:
  number:
    words:
      engine: 'conjunctional-scale'
      tensUnitsSeparator: ' '
      scales:
        -
          value: 10000000
          name: 'crore'
          ordinalName: 'crore'
        -
          value: 100000
          name: 'lakh'
          ordinalName: 'lakh'
        -
          value: 1000
          name: 'thousand'
          ordinalName: 'thousand'
```

In that example:

1. The locale still inherits the parent `number.words.andWord`, `number.words.unitsMap`, and `ordinal` surface.
2. The `tensUnitsSeparator` scalar overrides only that one field.
3. The `scales` sequence intentionally replaces the parent scale list.

## Lexical Table Shapes

Most `*Map` fields are lexical tables. There are two supported authoring shapes.

1. Dense sequence when every slot is meaningful:

```yaml
digitWords:
  - 'zero'
  - 'one'
  - 'two'
```

2. Sparse numeric-slot mapping when the table starts at an offset or has intentional holes:

```yaml
tensMap:
  2: 'twenty'
  3: 'thirty'
```

```yaml
unitsOrdinalPrefixes:
  0: 'zeroth'
  1: 'first'
  3: 'third'
```

Rules:

1. Numeric keys are emitted as array indices.
2. Missing numeric slots emit as empty strings.
3. If the locale needs a real value at index `0`, declare `0:` explicitly.
4. Do not use blank-string padding, `null: 0`, or wrapper DSLs like `firstIndex` and `fillValue`.
5. When a literal token is also a YAML keyword, quote it. For example, use `'null': 0` in a `cardinalMap` when the locale word is literally `"null"`.

## Common Authoring Patterns

These field-name patterns repeat across engines.

| Pattern | Meaning |
| --- | --- |
| `*Word` | A literal locale word or phrase used verbatim by the runtime kernel. |
| `*Prefix` | A literal prefix added before a stem, number, or token. |
| `*Suffix` | A literal suffix appended after a stem, number, or token. |
| `*Joiner` | A literal token inserted between number parts. |
| `*Separator` | A literal separator used when joining composed pieces. |
| `*Map` | A lexical table authored either as a dense sequence or as a sparse numeric-slot mapping. |
| `scales` | A sequence of scale metadata records used for thousands, millions, crores, and similar groups. |
| `ordinal*` | Fields used only for ordinal generation or ordinal parsing. |
| `negative*` | Fields used only for negative number rendering or parsing. |
| `useCulture` | Instructs the generated runtime path to pass the current culture into the shared kernel. |

## Canonical Surface Sections

The sections below answer two things for every block:

1. what values are valid
2. what the block is responsible for in the runtime pipeline

If you need step-by-step authoring guidance, use this document together with [Locale YAML How-To](./locale-yaml-how-to.md).

### `variantOf`

- Type: scalar locale code
- Purpose: declares the parent locale whose surfaces and fields should be inherited.
- Examples in the repo:
  - `'en'`
  - `'de'`
  - `'fr'`
  - `'pt'`
  - `'zh-Hans'`

### `list`

Purpose:

- owns list-joining words or delimiters only
- feeds the collection-humanization registration path
- should stay small; if the block starts accumulating general grammar, the data belongs elsewhere

Supported forms:

1. Scalar shorthand:
   - `'oxford'`
2. Mapping engines:
   - `engine: 'clitic'`
   - `engine: 'conjunction'`
   - `engine: 'delimited'`

Fields:

- Scalar `'oxford'`: uses the built-in Oxford-comma formatter.
- `value`: the locale-owned conjunction or delimiter token consumed by the selected engine.

### `formatter`

Purpose:

- owns formatter-specific grammar and phrase/form selection metadata
- feeds `ProfiledFormatter`
- should contain only information used while humanizing TimeSpan, dates, quantities, or data units through formatter resources

Supported engine:

- `engine: 'profiled'`

Fields:

- `resourceKeyDetector`
- `exactDateForms`
- `exactTimeSpanForms`
- `resourceKeySuffixes`
- `timeUnitGenders`
- `prepositionMode`
- `dataUnitDetector`
- `dataUnitSuffixes`
- `dataUnitFallbackTransform`
- `dataUnitNonIntegralForm`
- `secondaryPlaceholderMode`

Notes:

- `resourceKeyDetector` selects the pluralization/resource-shape family.
- `exactDateForms` maps exact date counts to a specific grammatical form for selected units and tenses.
- `exactTimeSpanForms` maps exact time-span counts to a specific grammatical form for selected units.
- `resourceKeySuffixes` supplies suffix fragments appended during resource-key construction.
- `timeUnitGenders` carries grammatical gender metadata for time-unit resources.
- `prepositionMode` selects locale-specific preposition handling.
- `dataUnitDetector` selects file-size/data-size plural behavior.
- `dataUnitSuffixes` supplies unit-specific suffix overrides for data units.
- `dataUnitFallbackTransform` rewrites fallback data-unit strings when direct resources do not exist.
- `dataUnitNonIntegralForm` tells the formatter which grammatical number to use for non-integral values.
- `secondaryPlaceholderMode` enables special placeholder behavior for locales with secondary article/consonant rules.

### `phrases`

Purpose:

- owns locale phrase tables that are not formatter profile metadata
- feeds the generated phrase-table catalogs used by relative-date, duration, data-unit, and time-unit humanization
- should contain direct locale phrases, not runtime strategy enums

Members:

- `relativeDate`
- `duration`
- `dataUnits`
- `timeUnits`

Notes:

- `relativeDate` owns phrases such as "yesterday", "tomorrow", and tense-sensitive relative-date templates.
- `duration` owns phrase tables used by duration humanization outside formatter profile selection metadata.
- `dataUnits` owns localized data-unit phrases.
- `timeUnits` owns localized time-unit phrases.
- `phrases` is a distinct canonical surface from `formatter`. Do not hide phrase-table ownership under `formatter` just because both influence humanized output.

### `ordinal`

Purpose:

- owns numeric ordinalization plus date-specific ordinal day rendering
- feeds the numeric ordinalizer and generated date-to-ordinal converters
- keeps all ordinal behavior under one conceptual surface

Members:

- `numeric`
- `date`
- `dateOnly`

`numeric` owns numeric ordinalization rules when the runtime output stays in numeric form.
`date` and `dateOnly` own date-specific day placement and day rendering mode.

Supported engines:

- `modulo-suffix`
- `suffix`
- `template`
- `word-form-template`

Date-specific fields:

- `date.pattern`: output format string using `{day}` for the day component.
- `date.dayMode`: day rendering strategy.
- `dateOnly.pattern`
- `dateOnly.dayMode`

### `number.words`

Purpose:

- owns render-side number composition data
- feeds the generated number-to-words profile catalog
- should contain words, lexical tables, scale rows, and structural strategy choices used while producing text from numbers

For locale parity work, `number.words` is the writer half of the number contract and should be planned together with `number.parse`.

Supported engines in current checked-in YAML:

- `agglutinative-ordinal-scale`
- `appended-group`
- `billion-strategy`
- `conjoined-gendered-scale`
- `conjunctional-scale`
- `construct-state-scale`
- `contextual-decimal`
- `contracted-one-scale`
- `east-asian-grouped`
- `east-slavic`
- `gendered-scale-ordinal`
- `harmony-ordinal`
- `hyphenated-scale`
- `hyphenated-ordinal`
- `indian-grouping`
- `inverted-tens`
- `joined-scale`
- `linking-scale`
- `long-scale-stem-ordinal`
- `dual-form-scale`
- `ordinal-prefix-scale`
- `pluralized-scale`
- `scale-strategy`
- `segmented-scale`
- `south-slavic-cardinal`
- `terminal-ordinal-scale`
- `triad-scale`
- `unit-leading-compound`
- `variant-decade`
- `west-slavic-gendered`

### `number.parse`

Purpose:

- owns parse-side number lexicons and normalization settings
- feeds the generated words-to-number profile catalog or token-map lexicon generator
- should contain only input-facing tokens and parser behavior, not render-only vocabulary that users never type or speak

For locale parity work, `number.parse` is the parser half of the same number contract and should accept the same natural forms that `number.words` emits.

Supported engines in current checked-in YAML:

- `compound-scale`
- `contracted-scale`
- `east-asian-positional`
- `greedy-compound`
- `inverted-tens`
- `linking-affix`
- `prefixed-tens-scale`
- `suffix-scale`
- `token-map`
- `vigesimal-compound`

### `clock`

Purpose:

- owns clock-phrase templates and phrase-family choices for `TimeOnly` clock notation
- feeds the single generated `phrase-clock` engine
- should stay separate from general time humanization and formatter resource behavior

Notes:

- `clock` is the canonical YAML authoring surface.
- The generator emits this surface into the runtime `timeOnlyToClockNotation` feature slot.
- All 62 shipped locales use the unified `phrase-clock` engine. There are no residual handwritten clock leaves.

Supported engine:

- `engine: 'phrase-clock'`

### `compass`

Purpose:

- owns localized 16-point compass heading tables
- feeds the generated heading-table catalog used by `HeadingExtensions`
- should contain only the heading strings for each style, not extra navigation logic

Fields:

- `full`
- `short`

Notes:

- Both `full` and `short` must contain exactly 16 entries.
- `compass` is the canonical YAML authoring surface.
- The generator currently emits this surface into the runtime `headings` feature slot.

### `calendar`

Purpose:

- owns locale-specific temporal data that overrides `CultureInfo.DateTimeFormat` when platform globalization data (ICU or NLS) differs across platforms, target frameworks, or globalization sources, or is incorrect
- feeds the generated `OrdinalDatePattern` profiles with month-name arrays
- should contain only data that differs from the platform-supplied `CultureInfo`; omit the block entirely when `CultureInfo` is correct

Members:

- `months`
- `monthsGenitive`

Fields:

- `months`: array of exactly 12 strings, indexed by Gregorian month (index 0 = January). Overrides `DateTimeFormatInfo.MonthNames` in `DateToOrdinalWords` and `DateOnlyToOrdinalWords` output.
- `monthsGenitive`: optional parallel array of 12 strings for locales that distinguish nominative and genitive month forms (e.g., Czech, Polish, Russian). When present, the genitive form is used in date patterns where the month follows a day number. When absent, `months` is used in all positions.

Notes:

- Both arrays must contain exactly 12 entries if present. The generator validates this at build time.
- Empty or absent = no override; the runtime falls through to `CultureInfo.DateTimeFormat`.
- Inherits via `variantOf`: a child locale inherits the parent's `calendar.months` unless it authors its own.
- Only `MMMM` (full month name) substitution is supported. If an ordinal-date pattern uses `MMM` (abbreviated month) while `calendar.months` is active, the generator emits a diagnostic error.

Example:

```yaml
surfaces:
  calendar:
    months:
      - 'জানুয়ারি'
      - 'ফেব্রুয়ারি'
      - 'মার্চ'
      - 'এপ্রিল'
      - 'মে'
      - 'জুন'
      - 'জুলাই'
      - 'আগস্ট'
      - 'সেপ্টেম্বর'
      - 'অক্টোবর'
      - 'নভেম্বর'
      - 'ডিসেম্বর'
```

Future fields: `monthsAbbreviated`, `days`, `daysAbbreviated`, `dayPeriods`, `amDesignator`, `pmDesignator`, `eraNames`. These are reserved but not yet implemented.

### `number.formatting`

Purpose:

- owns locale-specific number formatting data that overrides `NumberFormatInfo` when platform globalization data (ICU or NLS) differs across platforms, target frameworks, or globalization sources, or is incorrect
- feeds culture-aware `Ordinalize` overloads, byte-size string formatting (`ByteSize.ToString` / `ToFullWords`), and `MetricNumeralExtensions` with stable decimal separator, negative sign, and group separator values
- symmetric with `number.words` (output as words) and `number.parse` (input); `number.formatting` is "output as digits"

Fields:

- `decimalSeparator`: single character (or multi-character string for locales like Persian). Overrides `NumberFormatInfo.NumberDecimalSeparator` in Humanizer formatting call sites. Caller-supplied custom `NumberFormatInfo` / `IFormatProvider` is never overridden.
- `negativeSign`: single character (or multi-character string). Overrides `NumberFormatInfo.NegativeSign` in Humanizer formatting call sites. Used when the platform's NLS data returns U+002D (hyphen-minus) but the locale-correct character is U+2212 (minus sign), as is the case for several Nordic and European locales. Caller-supplied custom `NumberFormatInfo` / `IFormatProvider` is never overridden.
- `groupSeparator`: single character (or multi-character string). Overrides `NumberFormatInfo.NumberGroupSeparator` in Humanizer formatting call sites. Used when the platform's NLS data returns a different thousands separator than the locale-correct one (e.g., lb-LU where NLS returns a space but CLDR specifies a period). Caller-supplied custom `NumberFormatInfo` / `IFormatProvider` is never overridden.

Notes:

- Empty or absent = no override; the runtime uses the platform's `NumberFormatInfo` value for each respective property.
- Inherits via `variantOf`: a child locale inherits the parent's `number.formatting` fields unless it authors its own. For example, `nn.yml` uses `variantOf: 'nb'` and inherits `nb`'s `negativeSign` override automatically.
- The generated `LocaleNumberFormattingOverrides` registry walks `CultureInfo.Parent` at runtime (same fallback semantics as `LocaliserRegistry.FindLocaliser`), so unlisted child cultures fall back to the parent override.
- Override fields are consumed by culture-aware `Ordinalize` int overloads (formatting only), byte-size string formatting (`ByteSize.ToString` and `ByteSize.ToFullWords`), and `MetricNumeralExtensions`. String `Ordinalize` overloads parse with the culture's native `NumberFormatInfo` (no overrides). `ByteSize.TryParse` applies only the decimal separator override, and only when an explicit `CultureInfo` is passed as the format provider; it does not use `negativeSign` or `groupSeparator` overrides.

Example (decimal separator):

```yaml
surfaces:
  number:
    formatting:
      decimalSeparator: '.'
```

Example (negative sign for Nordic/European locales):

```yaml
surfaces:
  number:
    formatting:
      negativeSign: '−'   # U+2212 minus sign
```

Example (group separator):

```yaml
surfaces:
  number:
    formatting:
      groupSeparator: '.'
```

Future fields: `digitSubstitution`, `percentSymbol`. These are reserved but not yet implemented.

## Shared Strategy Values

These are the non-lexical option values that currently appear in checked-in locale YAML.

### Clock Hour Modes

- `h12` — 12-hour clock with words via `ToWords()` (default)
- `h24` — 24-hour clock with words via `ToWords()`
- `numeric` — digits instead of words (e.g., Japanese "3時")

### Clock Day Period Positions

- `prefix` — day period appears before the clock phrase
- `suffix` — day period appears after the clock phrase (default)

### Ordinal Date Calendar Modes

- `Gregorian` — forces the Gregorian calendar regardless of the culture's default calendar (default)
- `Native` — uses the culture's default calendar (e.g., Thai Buddhist year + 543, Hebrew calendar)

### Date Day Modes

- `DotSuffix`
- `MasculineOrdinalWhenDayIsOne`
- `Numeric`
- `Ordinal`
- `OrdinalWhenDayIsOne`

### Formatter Strategy Values

`resourceKeyDetector`

- `arabic-like`
- `between2-and4-paucal`
- `lithuanian`
- `russian`
- `slovenian`
- `south-slavic`

`dataUnitDetector`

- `arabic-like`
- `between2-and4-paucal`
- `lithuanian`
- `russian`
- `singular-plural`
- `slovenian`
- `south-slavic`

`dataUnitFallbackTransform`

- `latvian`
- `trim-trailing-s`

`dataUnitNonIntegralForm`

- `plural`

`prepositionMode`

- `romanian-de`

`secondaryPlaceholderMode`

- `luxembourgish-eifeler-n`

### Ordinalizer Strategy Values

`negativeNumberMode`

- `absolute-culture`
- `absolute-invariant`

### Number-To-Words Strategy Values

`addAndMode`

- `always-default`
- `use-caller-flag`

`andStrategy`

- `within-group-and-after-scale-sub-hundred-remainder`
- `within-group-only`

`ordinalMode`

- `cardinal`
- `english`
- `lithuanian`
- `numeric-culture`
- `numeric-string`
- `suffix`

`ordinalLeadingOneStrategy`

- `keep-leading-one`
- `omit-leading-one`

`hundredStrategy`

- `allow-explicit-one-in-composite`
- `omit-one-when-singular`

`ordinalSuffixStrategy`

- `final-character-membership`
- `last-vowel-map`

`cardinalStrategy`

- `norwegian-bokmal`
- `swedish`

`ordinalStrategy`

- `norwegian-bokmal`
- `swedish`

`formDetector`

- `lithuanian`
- `polish`

`unitVariantStrategy`

- `lithuanian`
- `polish`

`seventyStrategy`

- `regular`
- `sixty-plus-teens`

`ninetyStrategy`

- `eighty-plus-teens`
- `regular`

`tensJoinerTransform`

- `eifeler`

`scaleFormDetector`

- `slovenian`

`numberComposition`

- `inverted-tens-with-linker`

`defaultGender`

- `feminine`
- `masculine`
- `neuter`

`hundredTailPrefixMode`

- `low-or-exact-tens`
- `none`

`scaleTailPrefixMode`

- `low-or-exact-tens`
- `none`
- `sub-hundred`

### Words-To-Number Strategy Values

`normalizationProfile`

- `collapse-whitespace`
- `CollapseWhitespace`
- `LowercaseRemovePeriods`
- `LowercaseRemovePeriodsAndDiacritics`
- `LowercaseReplacePeriodsWithSpaces`
- `Persian`
- `punctuation-to-spaces-remove-diacritics`

`ordinalGenderVariant`

- `all`
- `masculine-and-feminine`
- `none`

## Date And Ordinal Blocks

### `ordinal.date` and `ordinal.dateOnly`

Fields:

- `pattern`
- `dayMode`
- `calendarMode`

Notes:

- `pattern` is a normal output template that must include `{day}` where the formatted day should appear.
- `dayMode` controls whether the day is numeric, ordinal, or conditionally ordinal.
- `calendarMode` controls how the calendar is resolved when formatting ordinal dates. Values are `Gregorian` (default, forces the Gregorian calendar regardless of culture) and `Native` (uses the culture's default calendar, e.g., Thai Buddhist, Hebrew, Persian). When omitted, `Gregorian` is used.
- `pattern` is the owning block's complete output template; the generator does not infer extra punctuation or month placement for you.
- if a locale has both `ordinal.date` and `ordinal.dateOnly`, document the reason in the locale file comments because most locales either share the same pattern family or omit one of the two blocks.

### `ordinalizer: modulo-suffix`

Fields:

- `defaultSuffix`
- `useAbsoluteValue`
- `absoluteAtLeast`
- `exactSuffixes`
- `lastTwoDigitsRange`
- `lastDigitSuffixes`

Notes:

- This engine computes suffixes from the numeric value.
- `defaultSuffix` is the fallback suffix when no special rule matches.
- `exactSuffixes` overrides exact numeric values.
- `lastTwoDigitsRange` handles ranges like 11 through 20.
- `lastDigitSuffixes` handles trailing-digit special cases such as 1, 2, and 3.
- `absoluteAtLeast` and `useAbsoluteValue` control whether negative numbers are normalized before suffix selection.

### `ordinalizer: suffix`

Fields:

- `masculineSuffix`
- `feminineSuffix`
- `neuterSuffix`
- `zeroAsPlainNumber`

Notes:

- This engine appends one of a small set of suffixes to the numeric form.
- `zeroAsPlainNumber` leaves zero unmodified instead of suffixing it.

### `ordinalizer: template`

Fields:

- `masculine`
- `feminine`
- `neuter`
- `negativeNumberMode`
- `minValueAsPlainNumber`
- `zeroAsPlainNumber`

Notes:

- The gendered template fields are numeric-string templates.
- `negativeNumberMode` controls whether negatives are normalized through invariant or culture-aware absolute-value handling.
- `minValueAsPlainNumber` leaves values below the threshold untemplated.

### `ordinalizer: word-form-template`

Fields:

- `masculine`
- `feminine`
- `neuter`
- `negativeNumberMode`
- `minValueAsPlainNumber`
- `zeroAsPlainNumber`
- `useCulture`

Notes:

- This is the same general family as `template`, but the runtime kernel also receives the current culture.

## Formatter Block

### `formatter: profiled`

Fields:

- `resourceKeyDetector`
- `exactDateForms`
- `exactTimeSpanForms`
- `resourceKeySuffixes`
- `timeUnitGenders`
- `prepositionMode`
- `dataUnitDetector`
- `dataUnitSuffixes`
- `dataUnitFallbackTransform`
- `dataUnitNonIntegralForm`
- `secondaryPlaceholderMode`

Authoring notes:

- `resourceKeyDetector` and `dataUnitDetector` choose the plural/resource family; these are structural strategy values, not locale display strings.
- `exactDateForms`, `exactTimeSpanForms`, and `resourceKeySuffixes` are the escape hatches for locales that need exact-count or unit-specific form selection beyond the default detector.
- `timeUnitGenders` belongs here because formatter resources often inflect by unit gender even when `number.words` does not.
- `secondaryPlaceholderMode` is rare; if you add it, leave a YAML comment in the locale file explaining the grammatical rule being modeled.

## Time-Only Clock Notation

### `clock: phrase-clock`

The unified `phrase-clock` engine handles all clock notation patterns through YAML configuration. It replaces the former `phrase-hour`, `relative-hour`, and residual leaf engines (French, German, Luxembourgish, Japanese).

**Core fields:**

- `hourMode`: hour display strategy (see Shared Strategy Values below)
- `hourGender`: grammatical gender for `ToWords()` hour rendering (`masculine`, `feminine`, `neuter`; default `masculine`)
- `minuteGender`: grammatical gender for `ToWords()` minute rendering (`masculine`, `feminine`, `neuter`; default `masculine`)
- `midnight`: display string for 00:00
- `midday`: display string for 12:00

**Minute-bucket templates:**

Explicit templates for each 5-minute increment. Templates use placeholders `{hour}`, `{nextHour}`, `{minutes}`, `{minutesReverse}` (60 minus minutes), `{minutesFromHalf}` (minutes minus 30), `{nextArticle}`, and `{dayPeriod}` (inline day-period placement).

- `min0` through `min55`: templates for minutes 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55
- `defaultTemplate`: catch-all fallback for non-bucketed minutes

**Range-based default templates** (fill gaps between 5-minute bucket positions):

- `pastHourTemplate`: minutes 1-24 (excluding any explicit bucket templates at 5, 10, 15, 20)
- `beforeHalfTemplate`: minutes 26-29
- `afterHalfTemplate`: minutes 31-34
- `beforeNextTemplate`: minutes 36-59 (excluding any explicit bucket templates at 40, 45, 50, 55)

**Zero filler:**

- `zeroFiller`: zero-pad word for minutes less than 10 (e.g., "noll", "零")

**Day-period support:**

- `earlyMorning`: display string for the early morning period
- `morning`: display string for the morning period
- `afternoon`: display string for the afternoon period
- `night`: display string for the night period
- `dayPeriodPosition`: `prefix` or `suffix` (default `suffix`)

The `{dayPeriod}` placeholder allows inline day-period placement within bucket templates. When a template contains `{dayPeriod}`, the engine expands it inline and does not append or prepend the day period again. This is useful for languages like Kurdish where the day period appears mid-phrase.

**Hour and minute word overrides:**

- `hourZeroWord`: override word for hour zero
- `hourOneWord`: override word for hour one
- `hourTwelveWord`: override word for hour twelve
- `hourWordsMap`: optional dense sequence of locale-specific hour words indexed by the resolved hour value, used instead of `ToWords()` when the locale needs pre-declined or article-attached forms (e.g., Arabic). The required length depends on `hourMode`: 13 entries (indices 0-12) for `h12`, or 24 entries (indices 0-23) for `h24`/`numeric`.
- `minuteWordsMap`: optional dense sequence of locale-specific minute words

**Hour suffixes:**

- `hourSuffixSingular`: singular hour suffix (e.g., French "heure")
- `hourSuffixPlural`: plural hour suffix (e.g., French "heures")
- `hourSuffixPaucal`: paucal hour suffix (for Slavic-family locales)

**Minute suffixes:**

- `minuteSuffixSingular`: singular minute suffix (e.g., Luxembourgish "Minutt")
- `minuteSuffixPlural`: plural minute suffix (e.g., Luxembourgish "Minutten")
- `minuteSuffixPaucal`: paucal minute suffix (for Slavic-family locales)

**Articles:**

- `singularArticle`: article for singular hours
- `pluralArticle`: article for plural hours

**Compact mode:**

- `compactMinuteWords`: boolean (default `false`) — when true, minute words use a compact conjunction form
- `compactConjunction`: conjunction word used in compact mode (e.g., Arabic "و")

**Paucal control:**

- `paucalLowOnly`: boolean (default `false`) — restricts paucal suffixes to low values only

**Eifeler Rule support** (Luxembourgish morphology):

- `applyEifelerRule`: boolean (default `false`) — applies `EifelerRule.DoesApply()` post-processing to trim trailing 'n' from number words when the following word blocks it

Notes:

- All 62 shipped locales use `phrase-clock`. There are no residual handwritten clock leaves.
- Non-bucketed minutes fall to range-based defaults or `defaultTemplate`.
- Day-period hour resolution is template-aware: templates that reference `{nextHour}` or `{nextArticle}` base the day period on hour+1 (since the phrasing is relative to the next hour), while templates using `{hour}` base the period on the current hour.

## Words-To-Number Engines

### `compound-scale`

Fields:

- `cardinalMap`
- `tens`
- `largeScales`
- `ignoredToken`
- `negativePrefixes`
- `ordinalNumberToWordsKind`
- `sequenceMultiplierThreshold`

Notes:

- `cardinalMap` maps plain tokens to values.
- `tens` is the ordered tens vocabulary used by the shared parser.
- `largeScales` is the ordered scale-token list.
- `ignoredToken` is a single token skipped during parsing.
- `ordinalNumberToWordsKind` links to a `number.words` profile whose ordinal outputs can be mined for parsing support.
- `sequenceMultiplierThreshold` changes how adjacent values are multiplied versus added.
- `ordinalNumberToWordsKind` may use `self` when ordinal parsing should be derived from the locale's own `number.words` block at generation time.

### `contracted-scale`

Fields:

- `minusWord`
- `cardinalMap`

### `east-asian-positional`

Fields:

- `digits`
- `smallUnits`
- `largeUnits`
- `negativePrefixes`
- `ordinalPrefix`
- `ordinalSuffix`
- `ordinalMap`

### `greedy-compound`

Fields:

- `cardinalMap`
- `negativePrefixes`
- `ordinalAbbreviationSuffixes`
- `ordinalNumberToWordsKind`
- `charactersToRemove`
- `charactersToReplaceWithSpace`
- `textReplacements`
- `lowercase`
- `removeDiacritics`

Notes:

- This engine normalizes aggressively, then greedily matches compound pieces.
- `textReplacements` is an ordered replacement table applied before tokenization.
- `charactersToRemove` and `charactersToReplaceWithSpace` should stay narrowly scoped to normalization, not spelling correction.

### `inverted-tens`

Fields:

- `cardinalMap`
- `unitMap`
- `tensTokens`
- `tensLinker`
- `scaleTokens`
- `ordinalMap`
- `ordinalNumberToWordsKind`
- `negativePrefixes`
- `ignoredTokens`
- `ordinalSuffixes`
- `unitPartReplacements`
- `allowInvariantIntegerInput`

### `linking-affix`

Fields:

- `cardinalMap`
- `teenPrefix`
- `teenBaseValue`
- `linkedSuffixes`
- `ignoredTokens`
- `negativePrefixes`

### `prefixed-tens-scale`

Fields:

- `cardinalMap`
- `tensMap`
- `scales`
- `prefixedTens`
- `negativePrefixes`

### `suffix-scale`

Fields:

- `cardinalMap`
- `bareScaleMap`
- `scales`
- `hundredSingularToken`
- `hundredPluralToken`
- `tensSuffixToken`
- `teenSuffixToken`
- `negativePrefixes`

### `token-map`

Fields:

- `normalizationProfile`
- `cardinalMap`
- `ordinalMap`
- `ordinalScaleMap`
- `gluedOrdinalScaleSuffixes`
- `compositeScaleMap`
- `negativePrefixes`
- `negativeSuffixes`
- `ordinalPrefixes`
- `ignoredTokens`
- `leadingTokenPrefixesToTrim`
- `multiplierTokens`
- `tokenSuffixesToStrip`
- `ordinalAbbreviationSuffixes`
- `teenSuffixTokens`
- `hundredSuffixTokens`
- `allowTerminalOrdinalToken`
- `useHundredMultiplier`
- `allowInvariantIntegerInput`
- `teenBaseValue`
- `hundredSuffixValue`
- `unitTokenMinValue`
- `unitTokenMaxValue`
- `hundredSuffixMinValue`
- `hundredSuffixMaxValue`
- `scaleThreshold`
- `ordinalGenderVariant`
- `ordinalNumberToWordsKind`

Notes:

- This is the most declarative parsing engine.
- `ordinalMap` holds exact ordinal tokens.
- `ordinalScaleMap` holds ordinal scale words such as "millionth".
- `gluedOrdinalScaleSuffixes` supports glued forms where the scale is expressed as a suffix.
- `compositeScaleMap` supports composite multi-token scales.
- `ordinalGenderVariant` tells the token-map generator which gender variants are already present in the YAML data.
- `normalizationProfile` is the first field to pick for a new locale because it determines how aggressively the generated parser cleans incoming text before tokenization.
- `cardinalMap` is the authoritative literal-token dictionary for this engine. If a token should parse, it must appear here or in one of the explicit ordinal/scale maps.

### `vigesimal-compound`

Fields:

- `cardinalMap`
- `ordinalMap`
- `negativePrefixes`
- `ignoredTokens`
- `vigesimalLeadingToken`
- `vigesimalFollowerTokens`
- `vigesimalValue`
- `teenLeaderToken`
- `teenLeaderBases`

## Number-To-Words Engines

For every engine below, scalar `*Word`, `*Prefix`, `*Suffix`, `*Joiner`, and `*Separator` fields are literal locale-owned strings. Array `*Map` fields are lexical tables. `scales` is always ordered from larger to smaller scale values unless the engine documentation says otherwise.

When an engine has nested objects such as `cardinal`, `ordinal`, or `endings`, keep those groupings intact in YAML. They exist to keep one locale file readable by semantic area instead of flattening unrelated values into one giant list.

### `agglutinative-ordinal-scale`

Fields:

- `minusWord`
- `tensSuffix`
- `teenSuffix`
- `ordinalTensSuffix`
- `unitsMap`
- `ordinalUnitsMap`
- `scales`
- `ordinalExceptions`

### `appended-group`

Fields:

- `zeroWord`
- `negativeWord`
- `conjunctionWord`
- `ordinalZeroWord`
- `ordinalSeparatorWord`
- `ordinalPrefix`
- `firstOrdinalMasculine`
- `firstOrdinalFeminine`
- `groups`
- `appendedGroups`
- `pluralGroups`
- `onesGroup`
- `tensGroup`
- `hundredsGroup`
- `appendedTwos`
- `twos`
- `feminineOnesGroup`
- `ordinalExceptions`
- `feminineOrdinalExceptions`

### `billion-strategy`

Fields:

- `minusWord`
- `andWord`
- `cardinal`
- `ordinal`

Nested `cardinal` fields:

- `hundredExactWord`
- `thousandWord`
- `millionSingularWord`
- `millionPluralWord`
- `billionStrategy`
- `billionSingularWord`
- `billionPluralWord`
- `unitsMap`
- `tensMap`
- `hundredsMap`

Nested `ordinal` fields:

- `billionStrategy`
- `thousandWord`
- `millionWord`
- `billionWord`
- `millionSeparator`
- `unitsMap`
- `tensMap`
- `hundredsMap`

Notes:

- Use this engine when a locale's main variability is how it names the billion scale and how the cardinal and ordinal branches differ at higher scales.
- Keep `cardinal` and `ordinal` words separate even when many values happen to match; they model two distinct runtime sub-profiles.

### `conjoined-gendered-scale`

Fields:

- `minusWord`
- `conjunction`
- `unitsMap`
- `tensMap`
- `hundredsMap`
- `hundredsOrdinalMap`
- `unitsOrdinal`
- `scales`

### `conjunctional-scale`

Fields:

- `minusWord`
- `andWord`
- `hundredWord`
- `hundredOrdinalWord`
- `tensUnitsSeparator`
- `defaultAddAnd`
- `addAndMode`
- `andStrategy`
- `tupleSuffix`
- `ordinalLeadingOneStrategy`
- `ordinalMode`
- `unitsMap`
- `ordinalUnitsMap`
- `tensMap`
- `ordinalTensMap`
- `scales`
- `namedTuples`

Notes:

- `defaultAddAnd`, `addAndMode`, and `andStrategy` together define when conjunctions appear. Do not try to encode that behavior with ad hoc words or duplicated tables.
- `ordinalMode` decides whether ordinals come from dedicated lexical tables, numeric suffixes, or other structural strategies within the shared kernel.
- `namedTuples` is for irregular compact tuple-style forms; keep it sparse and comment unusual values in the locale file.

### `construct-state-scale`

Fields:

- `defaultGender`
- `zeroWord`
- `minusWord`
- `andPrefix`
- `teenMasculineWord`
- `teenFeminineWord`
- `teenNormalizationOld`
- `teenNormalizationNew`
- `unitsFeminine`
- `unitsMasculine`
- `tensMap`
- `oneHundredWord`
- `twoHundredsWord`
- `hundredsPluralSuffix`
- `thousandsPluralSuffix`
- `thousandsSingularSuffix`
- `thousandsSpecialCases`
- `scales`
- `useCulture`

### `contextual-decimal`

Fields:

- `zeroWord`
- `minusWord`
- `ordinalPrefix`
- `tenWord`
- `tensWord`
- `zeroTensWord`
- `digitWords`
- `scales`
- `teenUnitExceptions`
- `postTensUnitExceptions`
- `exactOrdinals`

### `contracted-one-scale`

Fields:

- `zeroWord`
- `minusWord`
- `hundredWord`
- `hundredUnitWord`
- `units`
- `tens`
- `scales`

### `east-asian-grouped`

Fields:

- `zeroWord`
- `negativePrefix`
- `ordinalPrefix`
- `ordinalSuffix`
- `digitWords`
- `smallUnitWords`
- `largeUnits`
- `omitOneBeforeTen`
- `omitOneBeforeTenOnlyWhenLeading`
- `omitOneBeforeHundred`
- `omitOneBeforeThousand`
- `insertZeroInGroup`
- `insertZeroBetweenGroups`
- `ordinalMap`

Notes:

- Prefer sparse numeric-slot mappings for `smallUnitWords` or `largeUnits` whenever the locale only defines words from a non-zero starting slot.
- The `omitOneBefore*` flags are composition rules, not lexical exceptions; if you find yourself adding many one-off words instead, the locale may belong on a different engine.

### `east-slavic`

Fields:

- `zeroWord`
- `minusWord`
- `zeroOrdinalStem`
- `hundredsMap`
- `tensMap`
- `unitsMap`
- `unitsOrdinalPrefixes`
- `tensOrdinalPrefixes`
- `tensOrdinal`
- `unitsOrdinal`
- `feminineOne`
- `neuterOne`
- `feminineTwo`
- `oneOrdinalPrefix`
- `scales`
- `endings`

Nested `endings` fields:

- `masculine`
- `feminine`
- `neuter`

### `gendered-scale-ordinal`

Fields:

- `zeroWord`
- `minusWord`
- `feminineSingular`
- `masculineOrdinalPrefix`
- `feminineOrdinalPrefix`
- `masculineOrdinalSuffix`
- `feminineOrdinalSuffix`
- `joinGroups`
- `joinAboveTwenty`
- `unitsVariants`
- `teensVariants`
- `tensMap`
- `ordinalUnderTenVariants`
- `scales`

### `harmony-ordinal`

Fields:

- `minimumValue`
- `maximumValue`
- `minusWord`
- `unitsMap`
- `tensMap`
- `scales`
- `tupleSuffixes`
- `namedTuples`
- `hundredWord`
- `hundredStrategy`
- `ordinalSuffixes`
- `ordinalSuffixPair`
- `ordinalSuffixStrategy`
- `secondOrdinalSuffixCharacters`
- `softenTerminalTBeforeSuffix`
- `dropTerminalVowelBeforeHarmonySuffix`

Notes:

- Use this engine only for suffix systems driven by vowel harmony or final-character membership. If the locale needs wholly different tens/hundreds composition, start by checking another render-side engine first.
- `ordinalSuffixes`, `ordinalSuffixPair`, and `secondOrdinalSuffixCharacters` work together; document unusual combinations with a YAML comment in the locale file.

### `hyphenated-scale`

Fields:

- `zeroWord`
- `zeroOrdinalWord`
- `minusWord`
- `unitsMap`
- `tensMap`
- `hundredsMap`
- `ordinalUnitsMap`
- `ordinalUnitsExceptions`
- `ordinalTensMap`
- `scales`
- `thousandScale`
- `tupleMap`
- `twoPrefixWord`

### `hyphenated-ordinal`

Fields:

- `zeroWord`
- `negativeWord`
- `unitsMasculine`
- `unitsFeminine`
- `teens`
- `tens`
- `hundredsMasculine`
- `hundredsFeminine`
- `thousandWord`
- `millionSingular`
- `millionPlural`
- `millionSingularPrefix`
- `masculineCompoundOne`
- `feminineCompoundOne`
- `masculineOrdinalOne`
- `feminineOrdinalOne`
- `ordinalMasculine`
- `ordinalFeminine`
- `ordinalUnitComponents`
- `ordinalTensStems`
- `masculineOrdinalAppender`
- `masculineTensOrdinalSuffix`
- `feminineTensOrdinalSuffix`
- `defaultOrdinalAbbreviationMasculine`
- `defaultOrdinalAbbreviationFeminine`
- `ordinalAbbreviations`
- `defaultTensJoiner`
- `specialTensJoiner`
- `specialJoinerTensValue`
- `tupleMap`
- `tupleFallbackWord`

### `indian-grouping`

Fields:

- `zeroWord`
- `negativeWord`
- `unitsMap`
- `tensMap`
- `hundredsMap`
- `thousandsMap`
- `singleLakhWord`
- `lakhWord`
- `croreWord`
- `quadrillionWord`
- `quintillionWord`
- `thousandsOneBridge`
- `hundredsContinuingSuffix`
- `hundredsExactSuffix`
- `hundredsNineContinuingSuffix`
- `hundredsNineExactSuffix`
- `defaultTensWithRemainderSuffix`
- `nineTensWithRemainderSuffix`
- `specialTensWithRemainderSuffix`
- `exactTensDefaultSuffix`
- `exactTensNineSuffix`
- `thousandContinuingSuffix`
- `thousandExactSuffix`
- `lakhContinuingSuffix`
- `lakhExactSuffix`
- `croreContinuingSuffix`
- `ordinalSuffix`
- `ordinalExceptions`

### `inverted-tens`

Fields:

- `minusWord`
- `unitsMap`
- `tensMap`
- `hundredWord`
- `oneHundredWord`
- `hundredsPrefixMap`
- `unitTensJoiner`
- `unitTensAlternateJoiner`
- `unitTensAlternateJoinerUnitEnding`
- `hundredJoiner`
- `hundredRemainderSeparator`
- `hundredTailPrefix`
- `hundredTailPrefixMode`
- `scaleTailPrefix`
- `scaleTailPrefixMode`
- `leadingOneWord`
- `leadingOnePreservePrefix`
- `scales`
- `ordinalMode`
- `ordinalDefaultSuffix`
- `ordinalSteSuffix`
- `ordinalSteSuffixEndingChars`
- `ordinalExceptions`
- `removeLeadingOneInOrdinal`

### `joined-scale`

Fields:

- `zeroWord`
- `minusWord`
- `joinWord`
- `negativeJoinMode`
- `underHundredJoinMode`
- `scaleCountJoinMode`
- `compoundOrdinalWord`
- `compoundOrdinalRemainder`
- `defaultOrdinalSuffix`
- `matchingOrdinalSuffix`
- `ordinalSuffixMatchCharacters`
- `maximumValue`
- `omitOneWhenSingularAlways`
- `subHundredMap`
- `unitsMap`
- `tensMap`
- `hundredsMap`
- `scales`
- `ordinalExceptions`
- `compoundOrdinalExcludedValues`

Separator modes:

- `negativeJoinMode`
- `underHundredJoinMode`
- `scaleCountJoinMode`

Allowed values:

- `none`
- `space`
- `inherit-join-word`

### `linking-scale`

Fields:

- `zeroWord`
- `minusWord`
- `unitsMap`
- `tensMap`
- `hundredWords`
- `conjunctionWord`
- `tensRemainderJoiner`
- `scalePrefixes`
- `scaleLinkRules`
- `scales`

### `long-scale-stem-ordinal`

Fields:

- `zeroWord`
- `negativeWord`
- `unitsMasculine`
- `unitsFeminine`
- `unitsMasculineAbbreviation`
- `tensMap`
- `tensJoiner`
- `hundredthsRoots`
- `thousandthsRoots`
- `ordinalUnitRoots`
- `tenthsRoots`
- `exactHundredWord`
- `minLongValueWord`
- `thousandWord`
- `thousandOrdinalStem`
- `highestScaleValue`
- `highestScaleOrdinalSource`
- `highestScaleOrdinalTarget`
- `roundHigherScaleCompactValue`
- `masculineOrdinalEnding`
- `feminineOrdinalEnding`
- `tupleMap`
- `tupleFallbackSuffix`
- `hundredsMasculine`
- `hundredsFeminine`
- `largeScales`

### `dual-form-scale`

Fields:

- `prefixMap`
- `unitsMap`
- `tensMap`
- `conjunction`
- `hundredWord`
- `hundredPrefixWord`
- `dualHundredsWord`
- `feminineOneWord`
- `thousandScale`
- `millionScale`
- `billionScale`
- `exactOrdinals`
- `minusSuffix`

### `ordinal-prefix-scale`

Fields:

- `minusWord`
- `andWord`
- `defaultGender`
- `unitsMap`
- `masculineUnitsMap`
- `feminineUnitsMap`
- `neuterUnitsMap`
- `tensMap`
- `unitsOrdinalPrefixes`
- `tensOrdinalPrefixes`
- `hundredSingular`
- `hundredPlural`
- `ordinalTwoMasculine`
- `ordinalTwoFeminine`
- `ordinalTwoNeuter`
- `masculineOrdinalEnding`
- `nonMasculineOrdinalEnding`
- `scales`

### `pluralized-scale`

Fields:

- `zeroWord`
- `minusWord`
- `zeroOrdinalStem`
- `unitsMap`
- `tensMap`
- `hundredsMap`
- `scales`
- `formDetector`
- `unitVariantStrategy`
- `ordinalMode`
- `supportsNeuter`
- `masculineOrdinalSuffix`
- `feminineOrdinalSuffix`
- `ordinalUnitsMap`
- `ordinalTensMap`
- `ordinalHundredsMap`
- `useCulture`

### `scale-strategy`

Fields:

- `cardinalStrategy`
- `ordinalStrategy`
- `maximumValue`
- `defaultGender`
- `zeroWord`
- `minusWord`
- `oneDefault`
- `oneMasculine`
- `oneFeminine`
- `oneNeuter`
- `tensLinker`
- `largeScaleRemainderJoiner`
- `exactLargeScaleOrdinalSuffix`
- `exactDefaultOrdinalSuffix`
- `tensOrdinalTrimEndCharacters`
- `tensOrdinalSuffix`
- `shortOrdinalUpperBoundExclusive`
- `shortOrdinalTrimEndCharacters`
- `shortOrdinalTrimmedSuffix`
- `shortOrdinalSuffix`
- `hundredWord`
- `hundredCompositeSingularWord`
- `thousandWord`
- `thousandSingularWord`
- `thousandCompositeSingularWord`
- `unitsMap`
- `tensMap`
- `hundredUnitMap`
- `scales`
- `ordinalExceptions`

Notes:

- This engine is for locales where the same broad scale decomposition works, but the exact cardinal and ordinal policies vary by strategy enum.
- Use `cardinalStrategy` and `ordinalStrategy` before adding extra booleans. If a new distinction cannot be named cleanly as a strategy, the kernel may need redesign instead of more flags.

### `segmented-scale`

Fields:

- `maximumValue`
- `zeroWord`
- `minusWord`
- `teenPrefix`
- `exactOneHundredWord`
- `unitsDefault`
- `unitsPluralized`
- `tensMap`
- `hundredsDefault`
- `hundredsPluralized`
- `scales`
- `maximumOrdinal`
- `ordinalMap`

### `south-slavic-cardinal`

Fields:

- `maximumValue`
- `allowLongMin`
- `zeroWord`
- `minusWord`
- `scaleFormDetector`
- `numberComposition`
- `invertedTensLinker`
- `unitsMap`
- `tensMap`
- `hundredsMap`
- `feminineOne`
- `feminineTwo`
- `scales`
- `useCulture`

### `terminal-ordinal-scale`

Fields:

- `zeroWord`
- `minusWord`
- `unitStems`
- `ordinalUnitStems`
- `tensMap`
- `hundredsExactStems`
- `exactOneHundredCardinal`
- `exactOneHundredAfterHigherScale`
- `oneHundredWithRemainder`
- `hundredsPluralWord`
- `masculineOrdinalSuffix`
- `feminineOrdinalSuffix`
- `scales`

### `triad-scale`

Fields:

- `zeroWord`
- `minusWord`
- `feminineOneWord`
- `leadingOneWord`
- `tenWord`
- `tenOrdinalStem`
- `commonOrdinalStem`
- `masculineOrdinalSuffix`
- `feminineOrdinalSuffix`
- `ordinalUnit3RestoredVowel`
- `ordinalUnit6RestoredVowel`
- `unitsMap`
- `unitsFinalAccent`
- `tensMap`
- `teensMap`
- `hundredsMap`
- `ordinalUnderTen`
- `scales`

### `unit-leading-compound`

Fields:

- `zeroWord`
- `minusWord`
- `masculineOne`
- `feminineOne`
- `neuterOne`
- `feminineTwo`
- `tensJoiner`
- `tensJoinerTransform`
- `ordinalStemSuffix`
- `masculineOrdinalEnding`
- `feminineOrdinalEnding`
- `neuterOrdinalEnding`
- `supportsEifelerRule`
- `unitsMap`
- `compoundUnitsMap`
- `tensMap`
- `unitsOrdinal`
- `scales`

### `variant-decade`

Fields:

- `minusWord`
- `seventyStrategy`
- `ninetyStrategy`
- `specialSeventyOneWord`
- `pluralizeExactEighty`
- `tensUsingEtWhenUnitIsOne`
- `tensMap`

### `west-slavic-gendered`

Fields:

- `minusWord`
- `unitsMap`
- `tensMap`
- `hundredsMap`
- `unitsMasculineForms`
- `unitsFeminineForms`
- `unitsNeuterForms`
- `unitsInvariantForms`
- `thousands`
- `millions`
- `billions`
- `useCulture`

## Generator Pipeline Touchpoints

These are the main files that turn locale YAML into runtime behavior:

1. `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`
   - reads YAML
   - resolves locale inheritance
   - merges child mappings with parent mappings
   - normalizes author-facing references such as `self`
2. `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`
   - defines the typed generator-side structural contracts for shared engines
3. `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/*.cs`
   - emit typed profile catalogs for the supported surfaces
4. `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`
   - emits the locale-to-implementation registry wiring
5. `src/Humanizer/Localisation/*`
   - shared runtime kernels

## Regional Variant Checklist

Before creating a new regional variant file:

1. Do not use plain culture fallback as parity proof for a shipped locale.
2. If the variant has no locale-owned generated overrides, do not create a YAML file.
3. If the variant differs only in a few fields inside a feature block, declare `variantOf` and override only those fields.
4. If the variant needs a different `engine`, replace the whole mapping intentionally.
5. For parity work, inherited surfaces still need locale-specific proving assertions and a recorded inheritance chain to the terminal owner.

A locale parity claim is invalid unless every canonical surface is explicitly accounted for as locale-owned or same-language inherited with proof. There is no shipped-locale exemption list in this repo.

Examples in the current repo:

- `en-US.yml` exists because U.S. ordinal date formatting differs from `en`.
- `ru-RU.yml` does not need to exist when `ru` already covers the generated behavior.
- `en-IN.yml` declares `variantOf: 'en'` and overrides only the `number.words` fields that genuinely differ.


