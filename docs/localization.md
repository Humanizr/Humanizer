# Localization

Humanizer supports many languages and cultures across number, date, time, ordinal, formatter, collection-humanization, clock-notation, and heading surfaces.

Most consumers only need to set a culture. Contributors now author locale-specific generated behavior in one YAML file per locale, and shared runtime kernels are used whenever the behavior is structurally reusable.

## Supported Languages

Humanizer includes localization for:

Afrikaans (af), Arabic (ar), Azerbaijani (az), Bengali (bn), Bulgarian (bg), Catalan (ca), Chinese (zh-CN, zh-Hans, zh-Hant), Croatian (hr), Czech (cs), Danish (da), Dutch (nl), English (en, en-GB, en-IN, en-US), Finnish (fi), Filipino (fil), French (fr, fr-BE, fr-CH), German (de, de-CH, de-LI), Greek (el), Hebrew (he), Hungarian (hu), Armenian (hy), Icelandic (is), Indonesian (id), Italian (it), Japanese (ja), Korean (ko), Kurdish (ku), Latvian (lv), Lithuanian (lt), Luxembourgish (lb), Malay (ms), Maltese (mt), Norwegian Bokmal (nb), Norwegian Nynorsk (nn), Persian (fa), Polish (pl), Portuguese (pt, pt-BR), Romanian (ro), Russian (ru), Serbian (sr, sr-Latn), Slovak (sk), Slovenian (sl), Spanish (es), Swedish (sv), Tamil (ta), Thai (th), Turkish (tr), Ukrainian (uk), Uzbek (uz-Cyrl-UZ, uz-Latn-UZ), Vietnamese (vi), Zulu (zu-ZA).

## Installing Humanizer

```bash
dotnet add package Humanizer
```

The `Humanizer` package already includes all generated locale data.

## Using Cultures

Most Humanizer methods respect the current thread's `CurrentCulture` or `CurrentUICulture`. You can also explicitly specify a culture:

```csharp
var date = DateTime.UtcNow.AddHours(-2);

date.Humanize();
date.Humanize(culture: new CultureInfo("fr-FR"));
date.Humanize(culture: new CultureInfo("es"));

1234.ToWords();
1234.ToWords(new CultureInfo("es"));
1234.ToNumber(new CultureInfo("es"));
```

## Grammatical Features

Some languages require additional grammatical information:

```csharp
1.ToWords(GrammaticalGender.Masculine, new CultureInfo("ru"));
1.ToWords(GrammaticalGender.Feminine, new CultureInfo("ru"));

1.Ordinalize(GrammaticalGender.Masculine);
1.Ordinalize(GrammaticalGender.Feminine);

var date = new DateTime(2020, 1, 1);
date.ToOrdinalWords(GrammaticalCase.Nominative);
date.ToOrdinalWords(GrammaticalCase.Genitive);
```

## Feature Support

Localized Humanizer features are expected to work correctly for shipped locales. Whether a locale inherits behavior from a same-language parent is an implementation detail, not a support distinction.

Contributor-facing parity audits and gap tracking live in tests and local planning artifacts rather than in the user docs.

## Locale-Owned Data Model

The source of truth for generated localization behavior is `src/Humanizer/Locales/<locale>.yml`.

For the practical authoring workflow, see [Locale YAML How-To](locale-yaml-how-to.md). For the exhaustive block, engine, field, and strategy reference, see [Locale YAML Reference](locale-yaml-reference.md).

Principles:

1. One locale file owns one locale.
2. Locale inheritance is declared in that same file with `variantOf`.
3. Top-level properties are exactly `locale`, `variantOf`, and `surfaces`.
4. Canonical authoring surfaces under `surfaces` are exactly `list`, `formatter`, `phrases`, `number`, `ordinal`, `clock`, `compass`, and `calendar`.
5. Canonical nested members are `number.words`, `number.parse`, `number.formatting`, `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, `calendar.months`, and `calendar.monthsGenitive`.
6. Omit a `surfaces.<surface>` block to inherit it unchanged from the parent locale.
7. Inside a mapped surface, omit unchanged fields to inherit them from the parent mapping.
8. Child sequences replace parent sequences.
9. Changing `engine` replaces that mapped surface instead of merging it.
10. Keep locale-specific words, switches, and mappings in YAML.
11. Keep `number.words` and `number.parse` aligned whenever the locale claims parity.
12. `formatter` and `phrases` are separate canonical surfaces and should not be collapsed into one conceptual bucket.
13. `clock` is the canonical authoring name even though the emitted runtime feature is still `timeOnlyToClockNotation`.
14. `compass` is the canonical authoring name even though the emitted runtime feature is still `headings`.
15. Keep shared generator contracts in typed C# under `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`.
16. Never make authors learn internal generated profile ids just to connect two features inside one locale file.
17. The current authoring model keeps locale YAML at the top level of `src/Humanizer/Locales`.
18. Do not split one locale across multiple YAML files unless there is an explicit redesign that updates the compiler contract, docs, and tests together.

A shipped locale is incomplete unless every canonical surface is explicitly accounted for as locale-owned or same-language inherited with proof. There is no shipped-locale exemption list in this repo.

Example:

```yaml
# Locale-owned generator data for en-US.
locale: 'en-US'
variantOf: 'en'

surfaces:
  list:
    engine: 'oxford'

  number:
    words:
      engine: 'conjunctional-scale'
      minusWord: 'minus'
      andWord: 'and'
      unitsMap:
        - 'zero'
        - 'one'
        - 'two'
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
        two: 2
        hundred: 100
      ordinalNumberToWordsKind: 'self'
```

`surfaces.number.parse.ordinalNumberToWordsKind: 'self'` is intentional. Locale YAML is authored in locale terms, not in generator-internal profile-key terms. The generator resolves `self` to the owning locale profile during code generation.

## How The Generator Pipeline Fits Together

The localization codegen flow is:

1. `Locales/*.yml`
   Locale-owned authoring surface for all generated features.
2. `LocaleYamlCatalog`
   Parses canonical YAML, resolves `variantOf` inheritance, and exposes per-locale feature roots.
3. `EngineContractCatalog`
   Typed generator-side engine contracts that describe how a feature block maps onto a runtime profile object.
4. `ProfileCatalogInput` generators
   Build typed profile catalogs and tables for `numberToWords`, `wordsToNumber`, `ordinalizer`, `date-to-ordinal`, `formatter`, `phrases`, `headings`, and `timeOnlyToClockNotation`.
5. `LocaleRegistryInput`
   Emits the culture-to-implementation registrations that wire generated profiles into the runtime registries.
6. Shared runtime kernels
   Consume the generated profile objects at runtime with no YAML or JSON parsing on the hot path.

This split is deliberate:

- YAML is for locale-owned data.
- Generator-side structural contracts are typed C#.
- C# runtime code is for shared algorithms.

## Structural Naming Rules

Shared kernels must be named after the reusable rule family, not after the first language that used them.

Good structural names:

- `ConjunctionalScaleNumberToWordsConverter`
- `VariantDecadeNumberToWordsConverter`
- `UnitLeadingCompoundNumberToWordsConverter`
- `ContractedScaleWordsToNumberConverter`
- `ProfiledFormatter`

Residual locale names are acceptable only when the behavior is still genuinely locale-specific and forcing it into a shared schema would create imperative hooks or exception-bucket metadata. As of the locale parity completion, no residual leaves remain for any surface.

## Adding Or Updating A Locale

When a locale already fits an existing shared engine:

1. Produce a preflight gap report covering every canonical surface.
2. Create or update `src/Humanizer/Locales/<locale>.yml`.
3. Add `variantOf` if the locale is a regional variant.
4. Fill in the `surfaces` blocks that differ from the parent.
5. Reuse an existing structural engine name.
6. Add tests under `tests/Humanizer.Tests`.
7. Maintain a parity artifact until the unresolved set is empty.
8. Run the relevant source-generator tests and localization tests.

When a locale does not fit an existing shared engine:

1. Prove the behavior is shared by at least two locales, or by one locale plus an obvious second target already present in the repo.
2. Add or update the typed generator contract in `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`.
3. Add a shared runtime kernel with a structural name.
4. Keep the runtime parse-free. YAML is build input only; the generator contract is normal checked-in code.
5. Document the decision in the adjacent locale docs and code comments so the rationale stays with the implementation rather than in a stale execution plan.

Do not add a locale-specific converter just to avoid extending a clearly reusable shared family. Do not add a generic-sounding name if the implementation still hardcodes one language's rules.
Do not split locale-owned YAML into extra files just to make the top-level locale file look smaller. If nested locale YAML documents ever become necessary, treat that as an intentional redesign instead of an incremental cleanup.

## Validation Expectations

Every functional localization change should include:

1. Source-generator coverage in `tests/Humanizer.SourceGenerators.Tests`.
2. Locale behavior coverage in `tests/Humanizer.Tests`.
3. Full `net10.0` and `net8.0` test runs for touched functionality.
4. Benchmark coverage for shared-engine surfaces when the change affects runtime dispatch or hot-path composition.

Recommended verification commands:

```bash
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/plan-validation
```

## Override ICU Where Needed

Modern .NET uses ICU for globalization data on all platforms, but ICU data drifts between versions and can produce different output for the same locale across macOS, Linux, and Windows. When Humanizer delegates to `CultureInfo` for month names, decimal separators, negative signs, or group separators, this platform variance leaks into humanized output.

The `calendar:` surface and `number.formatting:` sub-block let locale authors hard-code the correct values in YAML so that output is byte-identical regardless of the host's ICU version. When present, calendar overrides take priority over `CultureInfo.DateTimeFormat` in ordinal date rendering (`DateTime.ToOrdinalWords` and `DateOnly.ToOrdinalWords`). Number formatting overrides take priority over `NumberFormatInfo` in culture-aware `Ordinalize` overloads, byte-size string formatting (`ByteSize.ToString` and `ByteSize.ToFullWords`), and `MetricNumeralExtensions`. The `ByteSize.TryParse` path intentionally uses only the decimal separator override and is not affected by `negativeSign` or `groupSeparator` overrides. Caller-supplied custom format providers are never overridden.

Use these overrides sparingly. Author them only when a cross-platform probe shows disagreement or when ICU data is demonstrably wrong for your locale. See [Locale YAML Reference](locale-yaml-reference.md) for field details and [Locale YAML How-To](locale-yaml-how-to.md) for a step-by-step recipe.

## Related Topics

- [Adding Or Updating A Locale](adding-a-locale.md)
- [Locale YAML How-To](locale-yaml-how-to.md)
- [Locale YAML Reference](locale-yaml-reference.md)
