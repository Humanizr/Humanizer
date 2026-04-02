# Locale YAML How-To

This is the practical authoring guide for `src/Humanizer/Locales/*.yml`.

Read this first when you need to add a locale, change a locale, or migrate a locale off a handwritten converter. Read [Locale YAML Reference](./locale-yaml-reference.md) beside it when you need the exhaustive field and strategy inventory.

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
  4: 'forty'
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

Do not do this:

```yaml
unitsMap:
  - ''
  - ''
  - 'two'
```

```yaml
tensMap:
  2: 'twenty'
```

```yaml
cardinalMap:
  null: 0
```

Do this instead when the locale really uses the literal token `"null"`:

```yaml
cardinalMap:
  'null': 0
```

Why:

1. In lexical tables, a missing numeric slot means "no word at this index".
2. In token maps, keys are literal input tokens.
3. YAML keywords such as `null`, `true`, `false`, `yes`, and `no` must be quoted when you mean the literal word rather than the YAML scalar value.

## Top-Level Block Guide

Every top-level block has the same authoring question:

What locale-owned data does this runtime surface need, and what shared engine should consume it?

Use these sections as ownership rules. If a value feels like it belongs to two blocks, pick the one whose runtime API actually consumes it.

### `list`

Use this block when the locale needs a generated collection joiner.

Put here:

- the collection formatter engine
- the locale-owned conjunction or delimiter token

Do not put here:

- time-unit formatting rules
- number words
- punctuation rules unrelated to list joining

Minimal example:

```yaml
collectionFormatter:
  engine: 'conjunction'
  value: 'og'
```

Checklist:

1. Decide whether the locale uses a built-in shorthand such as `oxford` or a mapped engine.
2. Put only the actual join word or delimiter token here.
3. Keep list-joining punctuation here only when the selected collection formatter engine consumes it directly.

### `formatter`

Use this block for resource-key selection, time-unit gender metadata, data-unit fallback rules, and similar formatter-only grammar.

Put here:

- formatter strategy selectors
- resource key overrides
- grammatical metadata for units
- data-unit fallback handling

Do not put here:

- number words
- ordinal words
- parser tokens

Minimal example:

```yaml
formatter:
  engine: 'profiled'
  dataUnitFallbackTransform: 'trim-trailing-s'
```

Checklist:

1. Start from the smallest possible formatter block.
2. Add only the strategy selectors that change runtime resource selection.
3. Keep resource overrides close to the exact unit or plural forms they affect.
4. Do not duplicate number words here just because a formatter output happens to contain them.

### `number.words`

Use this block when the locale supports cardinal or ordinal number rendering through a shared runtime kernel.

Put here:

- lexical tables such as `unitsMap`, `tensMap`, `hundredsMap`
- scale rows
- conjunction and separator words
- grammatical strategy values required by the engine

Do not put here:

- parsing-only tokens
- generator implementation details
- locale fallback wiring

Authoring order:

1. Pick the engine.
2. Fill in the required scalar words.
3. Add lexical tables.
4. Add scale metadata.
5. Add only the optional strategy values the engine actually uses.

What usually lives here:

1. Cardinal and ordinal vocabularies used while rendering whole numbers.
2. Scale rows such as thousand, million, crore, or billion.
3. Joiners and separators used while composing number phrases.
4. Gender, form, and ordinal strategy values that materially change composition.

What usually does not live here:

1. Tokens used only while parsing words back into numbers.
2. Resource-key rules for TimeSpan or byte-size formatters.
3. Date-only ordinal patterns.

### `number.parse`

Use this block when the locale supports parsing written numbers.

Put here:

- exact token maps
- scale token lists
- normalization settings
- negative prefixes and ignored tokens

Do not put here:

- rendering-only strings that the parser never consumes
- formatter configuration

Authoring order:

1. Pick the engine.
2. Add the main cardinal token map or positional maps.
3. Add ordinal and scale maps if the engine supports them.
4. Add normalization and ignored-token rules last.

What usually lives here:

1. Literal tokens the parser should recognize.
2. Normalization switches that explain how user input should be cleaned before tokenization.
3. Scale token lists and multiplier behavior.
4. Negative markers, ignored filler words, and ordinal parsing support.

What usually does not live here:

1. Render-only forms that are never accepted as input.
2. Date or formatter metadata.
3. Generator implementation hints.

### `ordinal`

Use this block when the locale ordinalizes numeric forms directly.

Put here:

- suffix templates
- modulo rules
- gendered ordinal templates

Do not put here:

- full number-to-words data
- date formatting templates

Checklist:

1. Use `ordinal.numeric` when the locale can ordinalize numeric output without spelling the whole number out.
2. Prefer a suffix or template engine when the locale only varies by affix or gendered template.
3. Keep whole-number lexical tables in `number.words`, not here.

### `ordinal.date` and `ordinal.dateOnly`

Use these nested members when the locale needs generated ordinal day rendering for dates.

Put here:

- the date pattern
- the day rendering mode

Do not put here:

- general date formatting rules
- month translation tables

Minimal example:

```yaml
dateToOrdinalWords:
  pattern: '{day} MMMM yyyy'
  dayMode: 'Ordinal'
```

Checklist:

1. Put the finished output shape in `pattern`.
2. Use `{day}` exactly where the rendered day should appear.
3. Pick `dayMode` based on whether the locale uses numeric, ordinal, or conditional ordinal day rendering.
4. Keep month-name ownership in resources or culture data, not in this block.

### `clock`

Use this block when the locale has generated clock-phrase output.

Put here:

- phrase templates for rounded or relative clock output
- period-of-day words when the engine uses them

Do not put here:

- general time humanization rules
- number parsing data

If the locale still needs a residual handwritten clock-notation engine, keep the locale name honest. Do not pretend it is generic.

Checklist:

1. Reuse `phrase-hour` when the locale mostly rounds into fixed minute buckets such as quarter past or ten to.
2. Reuse `relative-hour` when the locale phrases time relative to the current or upcoming hour plus a day-period label.
3. Keep general time humanization out of this block.
4. If a locale still needs a handwritten leaf, document why the phrase family does not fit the shared engines yet.

## Choosing Between A Shared Engine And A New One

Reuse an existing engine when the locale differs only in:

- words
- lexical tables
- scales
- grammatical metadata
- strategy enum choices

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
