# Historical documentation handoff

U7 exclusively owns generated snapshots, historical sidebars, version metadata,
and exact narrative-to-API links. Do not populate overlay directories or edit
`versioned_docs` by hand.

Paths below are relative to `website/`. A path not listed for replacement or
exclusion stays canonical, including its executable example. Every replacement
must preserve the canonical page ID, and every retained executable must compile
and run against that snapshot's package before publication.

## Exact scenario-to-API contract

U7 must replace each scenario's temporary `docs/api/index.md` link with links to
the generated files below and make verification reject an API-root-only scenario
link. A target marked 3.x has the shown filename in 3.x and current. Apply the
2.x filename substitutions in the next table, and omit a target when its type is
unavailable in that release.

| Canonical guide | Required generated API targets |
| --- | --- |
| `docs/scenarios/strings-and-casing.mdx` | `api/Humanizer.StringHumanizeExtensions.md`, `api/Humanizer.CasingExtensions.md`, `api/Humanizer.LetterCasing.md` |
| `docs/scenarios/truncation-and-dehumanization.mdx` | `api/Humanizer.TruncateExtensions.md`, `api/Humanizer.Truncator.md`, `api/Humanizer.ITruncator.md`, `api/Humanizer.StringDehumanizeExtensions.md` |
| `docs/scenarios/inflection-and-quantities.mdx` | `api/Humanizer.InflectorExtensions.md`, `api/Humanizer.ToQuantityExtensions.md`, `api/Humanizer.ShowQuantityAs.md`, `api/Humanizer.Vocabularies.md`, `api/Humanizer.Vocabulary.md` |
| `docs/scenarios/dates-times-durations-and-age.mdx` | `api/Humanizer.DateHumanizeExtensions.md`, `api/Humanizer.TimeSpanHumanizeExtensions.md`, `api/Humanizer.NumberToTimeSpanExtensions.md` |
| `docs/scenarios/relative-dates-and-times.mdx` | `api/Humanizer.DateHumanizeExtensions.md`, `api/Humanizer.Configurator.md` |
| `docs/scenarios/durations-and-ages.mdx` | `api/Humanizer.TimeSpanHumanizeExtensions.md` |
| `docs/scenarios/fluent-dates-and-time-spans.mdx` | `api/Humanizer.NumberToTimeSpanExtensions.md`, `api/Humanizer.PrepositionsExtensions.md`, `api/Humanizer.In.md`, `api/Humanizer.On.md` |
| `docs/scenarios/spoken-dates-and-clock-times.mdx` | `api/Humanizer.DateToOrdinalWordsExtensions.md`, `api/Humanizer.TimeOnlyToClockNotationExtensions.md` |
| `docs/scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx` | `api/Humanizer.NumberToWordsExtension.md`, `api/Humanizer.OrdinalizeExtensions.md`, `api/Humanizer.RomanNumeralExtensions.md`, `api/Humanizer.ByteSize.md` |
| `docs/scenarios/numbers-in-words-and-ordinals.mdx` | `api/Humanizer.NumberToWordsExtension.md`, `api/Humanizer.OrdinalizeExtensions.md` |
| `docs/scenarios/parse-number-words.mdx` | `api/Humanizer.WordsToNumberExtension.md` |
| `docs/scenarios/byte-sizes-and-rates.mdx` | `api/Humanizer.ByteSize.md`, `api/Humanizer.ByteSizeExtensions.md`, `api/Humanizer.ByteRate.md` |
| `docs/scenarios/metric-numerals.mdx` | `api/Humanizer.MetricNumeralExtensions.md`, `api/Humanizer.MetricNumeralFormats.md` |
| `docs/scenarios/enums-and-collections.mdx` | `api/Humanizer.EnumHumanizeExtensions.md`, `api/Humanizer.EnumDehumanizeExtensions.md`, `api/Humanizer.CollectionHumanizeExtensions.md` |
| `docs/scenarios/enums-and-flags.mdx` | `api/Humanizer.EnumHumanizeExtensions.md`, `api/Humanizer.EnumDehumanizeExtensions.md`, `api/Humanizer.OnNoMatch.md` |
| `docs/scenarios/collections-and-tuples.mdx` | `api/Humanizer.CollectionHumanizeExtensions.md`, `api/Humanizer.ICollectionFormatter.md`, `api/Humanizer.TupleizeExtensions.md` |
| `docs/scenarios/localization-and-extensibility.mdx` | `api/Humanizer.Configurator.md`, `api/Humanizer.LocaliserRegistry_TLocaliser_.md`, `api/Humanizer.ICulturedStringTransformer.md`, `api/Humanizer.ITruncator.md` |
| `docs/scenarios/specialized-formatting-utilities.mdx` | `api/Humanizer.HeadingExtensions.md`, `api/Humanizer.RomanNumeralExtensions.md`, `api/Humanizer.NumberToNumberExtensions.md`, `api/Humanizer.TupleizeExtensions.md`, `api/Humanizer.TimeUnitToSymbolExtensions.md`, `api/Humanizer.EnglishArticle.md` |

The navigation-only `docs/scenarios/index.mdx` links to the selected-version API
root; its focused children own exact type links. The three category landing
pages link to the union of their focused children's available targets.

### 2.x generated filename substitutions

These are exact generated type filenames, not source-code namespace advice:

| 3.x/current target | 2.x target |
| --- | --- |
| `api/Humanizer.ByteSize.md` | `api/Humanizer.Bytes.ByteSize.md` |
| `api/Humanizer.ByteRate.md` | `api/Humanizer.Bytes.ByteRate.md` |
| `api/Humanizer.Configurator.md` | `api/Humanizer.Configuration.Configurator.md` |
| `api/Humanizer.LocaliserRegistry_TLocaliser_.md` | `api/Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md` |
| `api/Humanizer.ICollectionFormatter.md` | `api/Humanizer.Localisation.CollectionFormatters.ICollectionFormatter.md` |
| `api/Humanizer.Vocabularies.md` | `api/Humanizer.Inflections.Vocabularies.md` |
| `api/Humanizer.Vocabulary.md` | `api/Humanizer.Inflections.Vocabulary.md` |

`api/Humanizer.WordsToNumberExtension.md` has no 2.x replacement.
`api/Humanizer.TimeOnlyToClockNotationExtensions.md` and
`api/Humanizer.TimeUnitToSymbolExtensions.md` begin in `2.13.14`.

## Excluded-route dependency contract

Excluding a page also requires replacing every canonical parent that links to
it. The exact parent replacements are:

| Version | Parent docs that must be replaced to prune excluded links |
| --- | --- |
| `2.10.1` | `docs/index.md`, `docs/scenarios/index.mdx`, `docs/scenarios/dates-times-durations-and-age.mdx`, `docs/scenarios/numbers-in-words-and-ordinals.mdx`, `docs/scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx`, `docs/scenarios/enums-and-collections.mdx`, `docs/scenarios/enums-and-flags.mdx`, `docs/upgrading/index.mdx` |
| `2.11.10` | `docs/index.md`, `docs/scenarios/index.mdx`, `docs/scenarios/dates-times-durations-and-age.mdx`, `docs/scenarios/numbers-in-words-and-ordinals.mdx`, `docs/scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx`, `docs/scenarios/enums-and-collections.mdx`, `docs/scenarios/enums-and-flags.mdx`, `docs/upgrading/index.mdx` |
| `2.13.14` | `docs/index.md`, `docs/scenarios/index.mdx`, `docs/scenarios/numbers-in-words-and-ordinals.mdx`, `docs/scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx`, `docs/scenarios/enums-and-collections.mdx`, `docs/scenarios/enums-and-flags.mdx`, `docs/upgrading/index.mdx` |
| `2.14.1` | `docs/index.md`, `docs/scenarios/index.mdx`, `docs/scenarios/numbers-in-words-and-ordinals.mdx`, `docs/scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx`, `docs/scenarios/enums-and-collections.mdx`, `docs/scenarios/enums-and-flags.mdx`, `docs/upgrading/index.mdx` |
| `3.0.1` | `docs/index.md`, `docs/upgrading/index.mdx` |
| `3.0.8` | `docs/index.md`, `docs/upgrading/index.mdx` |
| `3.0.10` | `docs/index.md`, `docs/upgrading/index.mdx` |

U7 must mechanically remove every excluded doc ID and category link from the
derived sidebar, remove any category left empty, and fail per-version relative-
link and API-link validation when an excluded route remains referenced. A
sidebar omission alone is not sufficient.

## Snapshot dispositions

### 2.10.1

Exclude:

- `docs/scenarios/parse-number-words.mdx`
- `docs/scenarios/spoken-dates-and-clock-times.mdx`
- `docs/concepts/trimming-and-native-aot.mdx`
- `docs/upgrading/main-preview.mdx`
- `_examples/scenarios-parse-number-words/ParseNumberWords.csproj`
- `_examples/scenarios-parse-number-words/Program.cs`
- `_examples/scenarios-spoken-dates/SpokenDates.csproj`
- `_examples/scenarios-spoken-dates/Program.cs`
- `_examples/scenarios-aot/Aot.csproj`
- `_examples/scenarios-aot/Program.cs`

Replace:

- `docs/scenarios/dates-times-durations-and-age.mdx` and
  `docs/scenarios/relative-dates-and-times.mdx`: remove `DateOnly` and
  `TimeOnly` choices.
- `docs/scenarios/durations-and-ages.mdx`,
  `_examples/scenarios-durations/Durations.csproj`, and
  `_examples/scenarios-durations/Program.cs`: remove `ToAge`; it does not exist before
  `3.0.1`.
- `docs/scenarios/fluent-dates-and-time-spans.mdx`: remove `DateOnly`,
  `InDate`, and `OnDate`.
- `docs/scenarios/byte-sizes-and-rates.mdx`,
  `_examples/scenarios-bytes/Bytes.csproj`, and
  `_examples/scenarios-bytes/Program.cs`: use `Humanizer.Bytes`, document only the
  string `ByteSize.TryParse` input, and remove culture-aware
  `ByteRate.Humanize`.
- `docs/scenarios/enums-and-flags.mdx`,
  `_examples/scenarios-enums-flags/EnumsFlags.csproj`, and
  `_examples/scenarios-enums-flags/Program.cs`: remove the generic nullable
  `DehumanizeTo<T>(OnNoMatch)` path and 3.x trimming/AOT annotations.
- `docs/scenarios/inflection-and-quantities.mdx`,
  `_examples/scenarios-inflection/Inflection.csproj`, and
  `_examples/scenarios-inflection/Program.cs`: use `Humanizer.Inflections` and state
  that only `+1`, not `-1`, selects singular.
- `docs/scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx`,
  `_examples/scenarios-numbers/Numbers.csproj`, and
  `_examples/scenarios-numbers/Program.cs`: use the 2.x byte namespace and `+1`
  quantity rule.
- `docs/scenarios/specialized-formatting-utilities.mdx`,
  `_examples/scenarios-specialized/Specialized.csproj`, and
  `_examples/scenarios-specialized/Program.cs`: remove time-unit symbols and use 2.x
  namespaces.
- `docs/start/configuration.mdx`, `docs/concepts/culture-and-configuration.mdx`,
  and `docs/scenarios/localization-and-extensibility.mdx`: use 2.x
  configuration/strategy namespaces and mutable-registry behavior.

### 2.11.10

Use every `2.10.1` exclusion and replacement except:

- Retain `DateOnly`/`TimeOnly` humanization and fluent `DateOnly` sections on
  compatible target frameworks.
- Retain their matching guide content, but keep
  `docs/scenarios/spoken-dates-and-clock-times.mdx` and
  `_examples/scenarios-spoken-dates/SpokenDates.csproj` and
  `_examples/scenarios-spoken-dates/Program.cs` excluded because clock notation
  is not available.

`ToAge`, span-based `ByteSize.TryParse`, words-to-number, enum AOT annotations,
generic nullable enum dehumanization, localized byte-rate formatting, time-unit
symbols, and `ToQuantity(-1)` singular behavior remain unavailable.

### 2.13.14

Exclude:

- `docs/scenarios/parse-number-words.mdx`
- `docs/concepts/trimming-and-native-aot.mdx`
- `docs/upgrading/main-preview.mdx`
- `_examples/scenarios-parse-number-words/ParseNumberWords.csproj`
- `_examples/scenarios-parse-number-words/Program.cs`
- `_examples/scenarios-aot/Aot.csproj`
- `_examples/scenarios-aot/Program.cs`

Replace:

- `docs/scenarios/durations-and-ages.mdx`,
  `_examples/scenarios-durations/Durations.csproj`, and
  `_examples/scenarios-durations/Program.cs`: remove `ToAge`.
- `docs/scenarios/byte-sizes-and-rates.mdx`,
  `_examples/scenarios-bytes/Bytes.csproj`, and
  `_examples/scenarios-bytes/Program.cs`: use `Humanizer.Bytes`; keep localized
  byte-rate formatting, but document only string `ByteSize.TryParse`.
- `docs/scenarios/enums-and-flags.mdx`,
  `_examples/scenarios-enums-flags/EnumsFlags.csproj`, and
  `_examples/scenarios-enums-flags/Program.cs`: remove generic nullable no-match and AOT
  guidance.
- `docs/scenarios/inflection-and-quantities.mdx`,
  `docs/scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx`,
  `_examples/scenarios-inflection/Inflection.csproj`,
  `_examples/scenarios-inflection/Program.cs`,
  `_examples/scenarios-numbers/Numbers.csproj`, and
  `_examples/scenarios-numbers/Program.cs`: use 2.x namespaces and the
  `+1`-only singular rule.
- `docs/scenarios/specialized-formatting-utilities.mdx`,
  `_examples/scenarios-specialized/Specialized.csproj`, and
  `_examples/scenarios-specialized/Program.cs`: keep time-unit symbols but use
  `Humanizer.Localisation.TimeUnit` and other 2.x namespaces.
- The configuration/localization pages listed for `2.10.1`: use 2.x
  namespaces and mutable registries.

Clock notation, time-unit symbols, and localized byte-rate formatting are
available in this snapshot.

### 2.14.1

Use the `2.13.14` exclusions and replacements. The same five absence rules still
apply: no `ToAge`, no span-based `ByteSize.TryParse`, no words-to-number, no
generic nullable enum dehumanization/AOT annotations, and only `+1` is singular
for `ToQuantity`.

Also replace `docs/upgrading/version-3-migration.mdx` links with 2.x generated
API filenames where it discusses the source side of the migration.

### 3.0.1

Exclude:

- `docs/upgrading/main-preview.mdx`

Replace:

- `docs/scenarios/inflection-and-quantities.mdx`,
  `docs/scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx`,
  `_examples/scenarios-inflection/Inflection.csproj`,
  `_examples/scenarios-inflection/Program.cs`,
  `_examples/scenarios-numbers/Numbers.csproj`, and
  `_examples/scenarios-numbers/Program.cs`: avoid the string
  `ToQuantity(int, ...)` overloads absent in early 3.x.
- `docs/scenarios/parse-number-words.mdx` and
  `_examples/scenarios-parse-number-words/ParseNumberWords.csproj` and
  `_examples/scenarios-parse-number-words/Program.cs`: describe and assert the
  `int` result.
- Analyzer pages: omit current-only parser widening/code-fix guidance.

`ToAge`, span-based `ByteSize.TryParse`, words-to-number, and enum AOT
annotations are available from this boundary.

### 3.0.8

Exclude `docs/upgrading/main-preview.mdx`. Retain the quantity examples restored
in this patch line. Replace parser prose/examples with the `int` return contract,
and replace analyzer text with the `3.0.8` package layout rather than the later
`3.0.10` fallback fix.

### 3.0.10

Exclude `docs/upgrading/main-preview.mdx` from the stable sidebar. Keep all
canonical U6A examples. Replace parser prose with the `int` result contract and
keep `Resources`/`ResourceKeys` only as facts about a future preview migration,
not removed stable APIs.

### main/preview

Keep all canonical U6A guides and examples. Label the route `main/preview` until
release authority assigns a public version; do not infer `3.5`, `4`, or feed
coordinates.

## Contributor content

Pages describing locale YAML, source generators, or current validation internals
are main-only unless a historical replacement is written from tagged evidence.
Do not copy the canonical contributor tree unchanged into a 2.x or 3.0.x
sidebar.
