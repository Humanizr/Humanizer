# Locale YAML How-To

This is the practical authoring guide for `src/Humanizer/Locales/*.yml`.

Read this first when you need to add a locale, change a locale, or migrate a locale off a residual runtime leaf. Together with [Adding Or Updating A Locale](./adding-a-locale.md), this document is intended to fully describe the allowed locale shape and authoring workflow without requiring contributors to inspect generator source code just to discover what belongs in a locale file.

Read [Locale YAML Reference](./locale-yaml-reference.md) beside it when you need the exhaustive field and strategy inventory for a specific engine.

## Mental Model

Each locale YAML file answers one question:

What locale-owned words, switches, lexical tables, and feature choices should the generator compile into runtime code for this locale?

Keep these boundaries strict:

1. Locale YAML owns locale data.
2. Shared runtime kernels own reusable algorithms.
3. Generator C# owns the structural mapping from YAML to runtime constructors.
4. Runtime never parses YAML or JSON.

If a value is a word, phrase, token, scale row, or strategy choice, it probably belongs in locale YAML.

If a value is describing generator plumbing, constructor shape, or how to walk the YAML tree, it does not belong in locale YAML.

## Before You Create A Locale File

Work through these questions in order.

1. Is runtime culture fallback currently masking missing locale behavior?
   If yes, treat that as parity debt, not success. Fallback never counts as parity proof for a shipped locale.
2. Is this a regional variant of an existing neutral locale?
   If yes, create a child file with `variantOf` and override only the differences.
3. Does the locale fit an existing shared engine?
   If yes, reuse that engine and provide locale-owned data.
4. Does the locale need a new shared structural engine?
   Only add one if the behavior is actually reusable.
5. Is the locale still genuinely procedural?
   Only then keep or add a residual locale leaf.
6. Does your locale need month names, decimal separators, negative signs, or group separators that differ from what `CultureInfo.DateTimeFormat` / `NumberFormatInfo` returns on the user's platform?
   If yes, author them in `calendar:` or `number.formatting:` so output is stable across .NET globalization modes and operating systems.

## File Shape

Each locale gets exactly one YAML file:

```yaml
locale: 'en-US'
variantOf: 'en'

surfaces:
  list:
    engine: 'conjunction'
    value: 'and'

  number:
    words:
      engine: 'conjunctional-scale'
      minusWord: 'minus'
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
```

Supported top-level blocks are:

- `locale`
- `variantOf`
- `surfaces`

Supported `surfaces` members are:

- `list`
- `formatter`
- `phrases`
- `number`
- `ordinal`
- `clock`
- `compass`
- `calendar`

Do not invent new top-level keys.

## Canonical Authoring Skeleton

This skeleton shows the complete canonical locale shape. Every locale file must stay within this structure.

```yaml
locale: '<locale>'
variantOf: '<parent-locale>'

surfaces:
  list:
    engine: '<list-engine>'

  formatter:
    engine: 'profiled'

  phrases:
    relativeDate:
      now: '<text>'
      never: '<text>'
      past: {}
      future: {}
    duration:
      zero: '<text>'
      age:
        template: '{value}'
    dataUnits: {}
    timeUnits: {}

  number:
    words:
      engine: '<number-to-words-engine>'
    parse:
      engine: '<words-to-number-engine>'
    formatting:
      decimalSeparator: '<separator>'
      negativeSign: '<sign>'
      groupSeparator: '<separator>'

  ordinal:
    numeric:
      engine: '<ordinal-engine>'
    date:
      pattern: '<pattern-with-{day}>'
      dayMode: '<day-mode>'
    dateOnly:
      pattern: '<pattern-with-{day}>'
      dayMode: '<day-mode>'

  clock:
    engine: '<clock-engine>'

  compass:
    full: []
    short: []

  calendar:
    months: []
    monthsGenitive: []
```

Notes:

1. `formatter` and `phrases` are separate surfaces.
2. `clock` is the canonical locale surface name even though the emitted runtime feature name is `timeOnlyToClockNotation`.
3. `number` and `ordinal` are container surfaces; the actual owned blocks are `number.words`, `number.parse`, `number.formatting`, `ordinal.numeric`, `ordinal.date`, and `ordinal.dateOnly`. Similarly, `calendar` contains `calendar.months` and `calendar.monthsGenitive`.
4. A locale parity claim is invalid unless every canonical surface is explicitly accounted for as locale-owned or same-language inherited with proof. There is no shipped-locale exemption list in this repo.
5. Do not add a block just to say "use the default behavior". If a surface or nested block does not carry locale-specific behavior, omit it.

## Inheritance

Use `variantOf` when a locale is a true variant of another locale.

```yaml
variantOf: 'en'
```

Rules:

1. Omitting a `surfaces.<surface>` block inherits the whole surface from the parent locale.
2. Inside a mapped surface block, omitted scalar fields inherit from the parent mapping.
3. Child sequences replace parent sequences.
4. Child mappings merge with parent mappings.
5. If the child changes `engine`, the whole mapped surface is treated as a new block.
6. Inheritance is not self-proving. A parity claim still needs at least one locale-specific proving assertion for every inherited canonical surface.
7. For parity work, do not rely on English fallback for any canonical surface.
8. Do not use empty mappings as inheritance sentinels. Omission is the only default-behavior signal. Exception: `engine: 'default'` is allowed on `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, and `clock` surfaces to explicitly opt into the built-in engine.

Use inheritance to express real parent-child relationships. Do not use it to hide unrelated locale behavior.

## Lexical Tables

Most `*Map` fields are lexical tables. There are two valid authoring shapes.

### Dense sequence

Use a YAML sequence when every slot from zero upward is meaningful and you actually want to author all of them.

```yaml
digitWords:
  - 'zero'
  - 'one'
  - 'two'
```

### Sparse numeric-slot mapping

Use a numeric-slot mapping when the table starts at an offset, has intentional holes, or would otherwise require blank padding.

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

1. Numeric keys are array indices.
2. Missing slots compile to empty strings.
3. Use a numeric key only when that index is intentionally meaningful.
4. If the locale really needs a word at index `0`, declare `0:` explicitly.

Do not author lexical tables with placeholder padding.

## Top-Level Block Guide

### `list`

Use this block when the locale needs a generated collection joiner.

Put here:

- the collection formatter engine
- the locale-owned conjunction or delimiter token

### `formatter`

Use this block for resource-key selection, time-unit gender metadata, data-unit fallback rules, and similar formatter-only grammar.

Put here:

- formatter strategy selectors
- resource key overrides
- grammatical metadata for units
- data-unit fallback handling

Do not put authored phrase tables here. Those belong under `phrases`.

### `phrases`

Use this block when the locale needs authored human-readable strings for humanization surfaces.

Put here:

- `relativeDate`
  Relative date phrases such as `now`, `never`, and per-unit `past` and `future` forms
- `duration`
  `TimeSpan.Humanize` and `ToAge` phrases, including `zero`, `age.template`, and per-unit `single` and `multiple` forms
- `dataUnits`
  Humanized data-unit names and symbols
- `timeUnits`
  Humanized time-unit symbols and labels

This is a first-class canonical surface. Do not collapse it into `formatter`.

### `number.words`

Use this block when the locale owns or inherits cardinal or ordinal number rendering through a shared runtime kernel.

Put here:

- lexical tables such as `unitsMap`, `tensMap`, `hundredsMap`
- scale rows
- conjunction and separator words
- grammatical strategy values required by the engine

Supported render engines in current checked-in YAML include:

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

Use this block when the locale supports parsing written numbers.

Put here:

- exact token maps
- scale token lists
- normalization settings
- negative prefixes and ignored tokens

Supported parse engines in current checked-in YAML include:

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

For locale parity work, account for `number.parse` alongside `number.words` so the locale can naturally round-trip the same high-range forms in both directions.

### `ordinal`

Use this block when the locale ordinalizes numeric forms directly.

Put here:

- suffix templates
- modulo rules
- gendered ordinal templates

The nested forms are:

- `ordinal.numeric`
- `ordinal.date`
- `ordinal.dateOnly`

### `clock`

Use this block when the locale has generated clock-phrase output.

Put here:

- phrase templates for rounded or relative clock output
- period-of-day words when the engine uses them

Supported engine:

- `phrase-clock` — the unified clock engine used by all 62 shipped locales

### `compass`

Use this block when the locale owns heading or compass labels.

Put here:

- `full`
  The full 16-point heading labels
- `short`
  The abbreviated 16-point heading labels

### `calendar`

Use this block when the locale needs month names that differ from what `CultureInfo.DateTimeFormat.MonthNames` returns on the user's platform. This is typically needed when platform globalization data differs across platforms, target frameworks, or globalization sources (ICU vs NLS), or when the platform-supplied names are incorrect for the locale.

Put here:

- `months`
  Array of exactly 12 nominative month names, indexed by Gregorian month (0 = January)
- `monthsGenitive`
  Optional parallel array of 12 genitive month names for locales that distinguish nominative and genitive forms

Do not author `calendar:` when `CultureInfo` already returns the correct month names on all platforms. The block is an override, not a requirement.

### `number.formatting`

Use this block when the locale needs a decimal separator, negative sign, or group separator that differs from what `NumberFormatInfo` returns on the user's platform. This is the "output as digits" complement to `number.words` (output as words) and `number.parse` (input).

Put here:

- `decimalSeparator`
  The locale-correct decimal separator character
- `negativeSign`
  The locale-correct negative sign character (e.g., U+2212 minus sign for Nordic/European locales where NLS returns U+002D hyphen-minus)
- `groupSeparator`
  The locale-correct thousands group separator character (e.g., period for lb-LU where NLS returns a space)

Do not author `number.formatting:` when `NumberFormatInfo` already returns the correct values on all platforms.

## Choosing Between A Shared Engine And A New One

Reuse an existing engine when:

- the locale differs only in words
- the locale differs only in lexical tables
- the locale differs only in scales
- the locale differs only in grammatical metadata
- the locale differs only in strategy enum choices

Add a new shared engine only when:

1. the algorithm is structurally reusable
2. at least two locales can share it, or one locale plus an obvious second target already exists
3. the YAML shape stays coherent
4. the runtime stays parse-free and benchmark-safe

Keep a residual locale leaf only when forcing it into YAML would add imperative hooks or an exception-bucket profile.

## Feature-By-Feature Authoring Order

When you are building a locale from scratch, use this order:

1. Add `variantOf` first if the locale is a regional variant.
2. Add `list` only if list joining actually differs from the parent.
3. Add `formatter` only if formatter resource selection or unit grammar differs.
4. Add `phrases` once you know the locale-owned relative-date, duration, data-unit, and time-unit strings.
5. Add `number.words` once you know the render-side engine family.
6. Add `number.parse` once you know the parse-side engine family.
7. Add `ordinal.numeric` if numeric ordinalization exists independently from `number.words`.
8. Add `ordinal.date` or `ordinal.dateOnly` only for date-specific day phrasing.
9. Add `clock` using the unified `phrase-clock` engine. All shipped locales use this single engine.
10. Add `compass` if the locale needs heading labels and does not inherit acceptable same-language values.
11. Add `calendar` only if platform-supplied month names disagree across platforms or target frameworks, or are incorrect.
12. Add `number.formatting` only if the decimal separator, negative sign, or group separator disagrees across platforms or target frameworks, or is incorrect.

This keeps authoring pressure on the generated/shared surfaces first and makes it easier to spot when a new block is really necessary.

## Recipes

### Add A New Neutral Locale

1. Create `src/Humanizer/Locales/<locale>.yml`.
2. Produce a preflight gap report covering every canonical surface.
3. Add or prove every canonical surface through locale ownership or same-language inheritance with proof.
4. Reuse existing engines wherever possible.
5. Add runtime tests under `tests/Humanizer.Tests/Localisation/<culture>`.
6. Add generator assertions if the generated wiring changed.
7. Maintain a parity map artifact until the unresolved set is empty.

### Add A Regional Variant

1. Create `src/Humanizer/Locales/<locale>.yml`.
2. Set `variantOf` to the parent locale.
3. Override only the fields that truly differ.
4. Do not copy the parent block unless the engine itself changes.

### Migrate A Locale Off A Handwritten Converter

1. Identify the actual rule family, not the locale name.
2. Move locale-owned words and switches into YAML.
3. Reuse or extend a shared runtime kernel.
4. Keep the engine name structural if the implementation is truly shared.
5. Add parity tests and run benchmarks before removing the leaf.

### Override Platform-Supplied Globalization Data

Use this recipe when a cross-platform or cross-target-framework probe shows that `CultureInfo` returns different month names, decimal separators, negative signs, or group separators on different platforms or target frameworks (e.g., net48/NLS vs net8+/ICU on Windows) for your locale.

**Month-name override** (minimum YAML):

```yaml
surfaces:
  calendar:
    months:
      - 'January'
      - 'February'
      - 'March'
      - 'April'
      - 'May'
      - 'June'
      - 'July'
      - 'August'
      - 'September'
      - 'October'
      - 'November'
      - 'December'
```

Replace the English names with the correct month names for your locale. Add `monthsGenitive:` only if your locale distinguishes nominative and genitive month forms.

**Decimal-separator override** (minimum YAML):

```yaml
surfaces:
  number:
    formatting:
      decimalSeparator: '.'
```

Replace `'.'` with the correct separator for your locale.

**Negative-sign override** (minimum YAML):

```yaml
surfaces:
  number:
    formatting:
      negativeSign: '−'   # U+2212 minus sign
```

Use this when ICU/CLDR specifies U+2212 (minus sign) but Windows NLS returns U+002D (hyphen-minus) for your locale. This is common for Nordic and European locales (fi, sv, nb, nn, hr, sl, lt). The override ensures that negative numbers rendered by `OrdinalizeExtensions` and `ByteSize.ToString` use the typographically correct minus sign on all platforms.

**Group-separator override** (minimum YAML):

```yaml
surfaces:
  number:
    formatting:
      groupSeparator: '.'
```

Use this when NLS and ICU disagree on the thousands separator for your locale. For example, lb-LU where NLS returns a space but CLDR specifies a period as the group separator. The override ensures that `ByteSize.ToString` uses the locale-correct thousands separator on all platforms.

All formatting overrides are consumed by culture-aware `Ordinalize` overloads (int overloads use override-aware formatting; string overloads use override-aware parsing with all three overrides), byte-size string formatting (`ByteSize.ToString` and `ByteSize.ToFullWords`), and `MetricNumeralExtensions`. They do not modify the global `CultureInfo`. `ByteSize.TryParse` applies only the decimal separator override, and only when an explicit `CultureInfo` is passed as the format provider; it does not use `negativeSign` or `groupSeparator` overrides.

## Validation

Every localization change should end with:

```powershell
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/plan-validation
```

If the change touches a hot runtime path, also run the relevant benchmark suite.

For parity-sensitive locale work, the practical completion test is stronger than “the YAML compiled”:

1. every canonical surface is intentionally locale-owned or intentionally inherited through `variantOf` with proof
2. the locale does not rely on English fallback or unsupported-locale behavior for any shipped localized surface
3. exact-output tests exist for grammar-sensitive or locale-specific behavior
4. the parity artifact ends with an empty unresolved set

If you cannot produce an empty unresolved set for the locale, you must report `parity not complete`.
