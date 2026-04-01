# Locale YAML Reference

This document is the authoritative authoring reference for `src/Humanizer/Locales/*.yml`.

If you are adding or changing locale-owned generated behavior, read this document together with [Locale YAML How-To](./locale-yaml-how-to.md) and [Adding Or Updating A Locale](./adding-a-locale.md).

## Scope

These YAML files are the single checked-in authoring surface for locale-owned generated behavior.

They are consumed only at build time by `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`.

They are not parsed at runtime.

## File-Level Rules

1. There is exactly one locale YAML file per locale code.
2. The file name is the locale code, for example `en.yml`, `en-US.yml`, `pt-BR.yml`.
3. Top-level properties are limited to:
   - `inherits`
   - `collectionFormatter`
   - `formatter`
   - `numberToWords`
   - `ordinalizer`
   - `wordsToNumber`
   - `dateToOrdinalWords`
   - `dateOnlyToOrdinalWords`
   - `timeOnlyToClockNotation`
4. Unknown top-level keys are rejected by the generator.
5. Locale words, switches, tables, and strategy choices belong here.
6. Generator implementation contracts do not belong here.

## Merge Rules

Locale inheritance is resolved during code generation.

The rules are:

1. Omitting a top-level feature block inherits the parent feature unchanged.
2. A scalar child value replaces the parent scalar value.
3. A sequence child value replaces the parent sequence value.
4. A mapping child value merges recursively with the parent mapping.
5. If a child mapping changes `engine`, that mapping replaces the parent mapping entirely.

That means regional variants can now override only the fields they actually differ on.

Example:

```yaml
inherits: 'en'

numberToWords:
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

1. The locale still inherits the parent `andWord`, `unitsMap`, and `ordinalizer`.
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

## Top-Level Blocks

The sections below answer two things for every block:

1. what values are valid
2. what the block is responsible for in the runtime pipeline

If you need step-by-step authoring guidance, use this document together with [Locale YAML How-To](./locale-yaml-how-to.md).

### `inherits`

- Type: scalar locale code
- Purpose: declares the parent locale whose blocks and fields should be inherited.
- Examples in the repo:
  - `'en'`
  - `'de'`
  - `'fr'`
  - `'pt'`
  - `'zh-Hans'`

### `collectionFormatter`

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

- owns formatter-specific grammar and resource-key selection metadata
- feeds `ProfiledFormatter`
- should contain only information used while humanizing TimeSpan, dates, quantities, or data units through formatter resources

Supported engine:

- `engine: 'profiled'`

Fields:

- `resourceKeyDetector`
- `resourceKeyOverrides`
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
- `resourceKeyOverrides` maps specific units or counts to explicit resource keys.
- `resourceKeySuffixes` supplies suffix fragments appended during resource-key construction.
- `timeUnitGenders` carries grammatical gender metadata for time-unit resources.
- `prepositionMode` selects locale-specific preposition handling.
- `dataUnitDetector` selects file-size/data-size plural behavior.
- `dataUnitSuffixes` supplies unit-specific suffix overrides for data units.
- `dataUnitFallbackTransform` rewrites fallback data-unit strings when direct resources do not exist.
- `dataUnitNonIntegralForm` tells the formatter which grammatical number to use for non-integral values.
- `secondaryPlaceholderMode` enables special placeholder behavior for locales with secondary article/consonant rules.

### `dateToOrdinalWords`

Purpose:

- owns date-specific day placement and day rendering mode
- feeds the generated date-to-ordinal converter
- should not be used to encode generic ordinal-number behavior

No engine field is used today.

Fields:

- `pattern`: output format string using `{day}` for the day component.
- `dayMode`: day rendering strategy.

### `dateOnlyToOrdinalWords`

The same shape as `dateToOrdinalWords`.

Fields:

- `pattern`
- `dayMode`

### `ordinalizer`

Purpose:

- owns numeric ordinalization rules when the runtime output stays in numeric form
- feeds ordinalizer registries directly
- should not duplicate full `numberToWords` vocabularies unless a specific engine explicitly bridges to them

Supported engines:

- `modulo-suffix`
- `suffix`
- `template`
- `word-form-template`

### `numberToWords`

Purpose:

- owns render-side number composition data
- feeds the generated number-to-words profile catalog
- should contain words, lexical tables, scale rows, and structural strategy choices used while producing text from numbers

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

### `wordsToNumber`

Purpose:

- owns parse-side number lexicons and normalization settings
- feeds the generated words-to-number profile catalog or token-map lexicon generator
- should contain only input-facing tokens and parser behavior, not render-only vocabulary that users never type or speak

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

### `timeOnlyToClockNotation`

Purpose:

- owns clock-phrase templates and phrase-family choices for `TimeOnly` clock notation
- feeds either generated shared clock engines or the small residual handwritten clock leaves
- should stay separate from general time humanization and formatter resource behavior

Supported shapes:

1. Residual locale leaves:
   - `engine: 'french'`
   - `engine: 'german'`
   - `engine: 'luxembourgish'`
2. Generated shared engines:
   - `engine: 'phrase-hour'`
   - `engine: 'relative-hour'`

## Shared Strategy Values

These are the non-lexical option values that currently appear in checked-in locale YAML.

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

### `dateToOrdinalWords` and `dateOnlyToOrdinalWords`

Fields:

- `pattern`
- `dayMode`

Notes:

- `pattern` is a normal output template that must include `{day}` where the formatted day should appear.
- `dayMode` controls whether the day is numeric, ordinal, or conditionally ordinal.
- `pattern` is the owning block's complete output template; the generator does not infer extra punctuation or month placement for you.
- if a locale has both `dateToOrdinalWords` and `dateOnlyToOrdinalWords`, document the reason in the locale file comments because most locales either share the same pattern family or omit one of the two blocks.

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
- `resourceKeyOverrides`
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
- `resourceKeyOverrides` and `resourceKeySuffixes` are the escape hatches for resources that do not fit the default key-construction pattern.
- `timeUnitGenders` belongs here because formatter resources often inflect by unit gender even when `numberToWords` does not.
- `secondaryPlaceholderMode` is rare; if you add it, leave a YAML comment in the locale file explaining the grammatical rule being modeled.

## Time-Only Clock Notation

### Residual leaf engines

- `french`
- `german`
- `luxembourgish`

These still route to locale-specific runtime implementations. They are acceptable leftovers until the repository has a clean structural model for those phrase families.

### `timeOnlyToClockNotation: phrase-hour`

Fields:

- `midnight`
- `midday`
- `oclockFormat`
- `quarterPastFormat`
- `halfPastFormat`
- `twentyToFormat`
- `quarterToFormat`
- `tenToFormat`
- `fiveToFormat`
- `roundHourFormat`
- `genericMinutesFormat`

Notes:

- This engine renders clock notation through fixed phrase templates keyed by rounded minute buckets.

### `timeOnlyToClockNotation: relative-hour`

Fields:

- `midnight`
- `midday`
- `singularArticle`
- `pluralArticle`
- `and`
- `quarterPast`
- `halfPast`
- `twentyFiveTo`
- `twentyTo`
- `quarterTo`
- `tenTo`
- `fiveTo`
- `lateNightPeriod`
- `morningPeriod`
- `afternoonPeriod`
- `eveningPeriod`

Notes:

- This engine expresses time as a relation to the upcoming or current hour and appends period-of-day words.

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
- `ordinalNumberToWordsKind` links to a `numberToWords` profile whose ordinal outputs can be mined for parsing support.
- `sequenceMultiplierThreshold` changes how adjacent values are multiplied versus added.
- `ordinalNumberToWordsKind` may use `self` when ordinal parsing should be derived from the locale's own `numberToWords` block at generation time.

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
- `prefixRules`
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
- `teenUnitOverrides`
- `postTensUnitOverrides`
- `ordinalUnitOverrides`

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
- `negativeJoinWord`
- `underHundredJoinWord`
- `scaleCountJoinWord`
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
- `ordinalOverrideMap`
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
- `unitsMasculineOverrides`
- `unitsFeminineOverrides`
- `unitsNeuterOverrides`
- `unitsInvariantOverrides`
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
   - shared runtime kernels and the small number of accepted residual locale leaves

## Regional Variant Checklist

Before creating a new regional variant file:

1. Check whether plain culture fallback already gives the desired result.
2. If the variant has no locale-owned generated overrides, do not create a YAML file.
3. If the variant differs only in a few fields inside a feature block, use `inherits` and override only those fields.
4. If the variant needs a different `engine`, replace the whole mapping intentionally.

Examples in the current repo:

- `en-US.yml` exists because U.S. ordinal date formatting differs from `en`.
- `ru-RU.yml` does not need to exist when `ru` already covers the generated behavior.
- `en-IN.yml` inherits `en` and overrides only the `numberToWords` fields that genuinely differ.
