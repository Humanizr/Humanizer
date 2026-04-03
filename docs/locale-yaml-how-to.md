# Locale YAML How-To

This is the practical authoring guide for `src/Humanizer/Locales/*.yml`.

Read this first when you need to add a locale, change a locale, or migrate a locale off a residual runtime leaf. Read [Locale YAML Reference](./locale-yaml-reference.md) beside it when you need the exhaustive field and strategy inventory.

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

1. Does the locale already work through culture fallback?
   If yes, do not add a file.
2. Is this a regional variant of an existing neutral locale?
   If yes, create a child file with `variantOf` and override only the differences.
3. Does the locale fit an existing shared engine?
   If yes, reuse that engine and provide locale-owned data.
4. Does the locale need a new shared structural engine?
   Only add one if the behavior is actually reusable.
5. Is the locale still genuinely procedural?
   Only then keep or add a residual locale leaf.

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

Do not invent new top-level keys.

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
6. For supported number locales, do not rely on English fallback when the locale is supposed to provide its own number words or parser.

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

### `number.words`

Use this block when the locale supports cardinal or ordinal number rendering through a shared runtime kernel.

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

For supported number locales, author `number.parse` alongside `number.words` so the locale can naturally round-trip the same high-range forms in both directions.

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
4. Add `number.words` once you know the render-side engine family.
5. Add `number.parse` once you know the parse-side engine family.
6. Add `ordinal.numeric` if numeric ordinalization exists independently from `number.words`.
7. Add `ordinal.date` or `ordinal.dateOnly` only for date-specific day phrasing.
8. Add `clock` last, after checking whether the locale really fits an existing clock engine.

This keeps authoring pressure on the generated/shared surfaces first and makes it easier to spot when a new block is really necessary.

## Recipes

### Add A New Neutral Locale

1. Create `src/Humanizer/Locales/<locale>.yml`.
2. Add only the feature blocks the locale actually supports.
3. Reuse existing engines wherever possible.
4. Add runtime tests under `tests/Humanizer.Tests/Localisation/<culture>`.
5. Add generator assertions if the generated wiring changed.

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

## Validation

Every localization change should end with:

```powershell
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/plan-validation
```

If the change touches a hot runtime path, also run the relevant benchmark suite.
