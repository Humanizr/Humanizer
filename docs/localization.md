# Localization

Humanizer supports many languages and cultures across number, date, time, ordinal, formatter, and collection-humanization surfaces.

Most consumers only need to set a culture. Contributors now have a stricter authoring model too: locale-specific generated behavior should live in one YAML file per locale, and runtime implementations should be shared or generated whenever the behavior is structurally reusable.

## Supported Languages

Humanizer includes localization for:

Arabic (ar), Azerbaijani (az), Bulgarian (bg), Bengali (bn-BD), Czech (cs), Danish (da), German (de), Greek (el), Spanish (es), Persian (fa), Finnish (fi), French (fr), Hebrew (he), Croatian (hr), Hungarian (hu), Armenian (hy), Indonesian (id), Icelandic (is), Italian (it), Japanese (ja), Korean (ko), Kurdish (ku), Latvian (lv), Malay (ms-MY), Maltese (mt), Norwegian Bokmal (nb, nb-NO), Dutch (nl), Polish (pl), Portuguese (pt, pt-BR), Romanian (ro), Russian (ru), Slovak (sk), Slovenian (sl), Serbian (sr, sr-Latn), Swedish (sv), Thai (th), Turkish (tr), Ukrainian (uk), Uzbek (uz-Cyrl-UZ, uz-Latn-UZ), Vietnamese (vi), Chinese (zh-CN, zh-Hans, zh-Hant).

## Installing Humanizer

### Package

```bash
dotnet add package Humanizer
```

The `Humanizer` package already includes all generated locale data.

## Using Cultures

Most Humanizer methods respect the current thread's `CurrentCulture` or `CurrentUICulture`. You can also explicitly specify a culture:

### DateTime Humanization

```csharp
var date = DateTime.UtcNow.AddHours(-2);

date.Humanize();
date.Humanize(culture: new CultureInfo("fr-FR"));
date.Humanize(culture: new CultureInfo("es"));
```

### Number to Words

```csharp
1234.ToWords();
1234.ToWords(new CultureInfo("es"));
1234.ToWords(new CultureInfo("fr"));
```

### TimeSpan Humanization

```csharp
TimeSpan.FromDays(1).Humanize();
TimeSpan.FromDays(1).Humanize(culture: new CultureInfo("de"));
TimeSpan.FromDays(3).Humanize(culture: new CultureInfo("ru"));
```

## Grammatical Features

Some languages require additional grammatical information:

### Grammatical Gender

```csharp
1.ToWords(GrammaticalGender.Masculine, new CultureInfo("ru"));
1.ToWords(GrammaticalGender.Feminine, new CultureInfo("ru"));

1.Ordinalize(GrammaticalGender.Masculine);
1.Ordinalize(GrammaticalGender.Feminine);
```

### Grammatical Case

```csharp
var date = new DateTime(2020, 1, 1);

date.ToOrdinalWords(GrammaticalCase.Nominative);
date.ToOrdinalWords(GrammaticalCase.Genitive);
```

### Word Forms

```csharp
3.Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation);
3.Ordinalize(GrammaticalGender.Masculine, WordForm.Normal);
```

## Feature Support by Language

Not all features are available in all languages:

| Feature | Widely Supported | Limited Support |
|---------|------------------|-----------------|
| String Humanization | All languages | - |
| DateTime Humanization | All languages | - |
| TimeSpan Humanization | All languages | - |
| Number to Words | Most languages | Some Asian languages |
| Ordinalization | Most European languages | Limited in Asian languages |
| Pluralization | English only | - |

## Locale-Owned Data Model

The source of truth for generated localization behavior is `src/Humanizer/Locales/<locale>.yml`.

For the practical authoring workflow, see [Locale YAML How-To](locale-yaml-how-to.md). For the exhaustive block, engine, field, and strategy reference, see [Locale YAML Reference](locale-yaml-reference.md).

Principles:

1. One locale file owns one locale.
2. Locale inheritance is declared in that same file with `variantOf`.
3. Omit a `surfaces.<surface>` block to inherit it unchanged from the parent locale.
4. Inside a mapped surface, omit unchanged fields to inherit them from the parent mapping.
5. Child sequences replace parent sequences.
6. Changing `engine` replaces that mapped surface instead of merging it.
7. Keep locale-specific words, switches, and mappings in YAML.
8. Keep shared generator contracts in typed C# under `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`.
9. Never make authors learn internal generated profile ids just to connect two features inside one locale file.
10. The current authoring model keeps locale YAML at the top level of `src/Humanizer/Locales`.
11. Do not split one locale across multiple YAML files unless there is an explicit redesign that updates the compiler contract, docs, and tests together.

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
      defaultAddAnd: true
      addAndMode: 'use-caller-flag'
      andStrategy: 'within-group-and-after-scale-sub-hundred-remainder'
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
   Build typed profile catalogs for `numberToWords`, `wordsToNumber`, `ordinalizer`, `dateToOrdinalWords`, `formatter`, and `timeOnlyToClockNotation`.
5. `LocaleRegistryInput`
   Emits the culture-to-implementation registrations that wire generated profiles and handwritten residual leaves into the runtime registries.
6. Shared runtime kernels
   Consume the generated profile objects at runtime with no YAML or JSON parsing on the hot path.

This split is deliberate:

- YAML is for locale-owned data.
- Generator-side structural contracts are typed C#.
- C# runtime code is for shared algorithms or accepted residual leaves.

## Structural Naming Rules

Shared kernels must be named after the reusable rule family, not after the first language that used them.

Good structural names:

- `ConjunctionalScaleNumberToWordsConverter`
- `VariantDecadeNumberToWordsConverter`
- `UnitLeadingCompoundNumberToWordsConverter`
- `ContractedScaleWordsToNumberConverter`
- `ProfiledFormatter`

Residual locale names are acceptable only when the behavior is still genuinely locale-specific and forcing it into a shared schema would create imperative hooks or exception-bucket metadata.

Current accepted residual leaves are limited to a small set of `TimeOnlyToClockNotation` converters where the repository still lacks a clean multi-locale abstraction.

## Adding Or Updating A Locale

When a locale already fits an existing shared engine:

1. Create or update `src/Humanizer/Locales/<locale>.yml`.
2. Add `variantOf` if the locale is a regional variant.
3. Fill in the `surfaces` blocks that differ from the parent.
4. Reuse an existing structural engine name.
5. Add or update resource files under `src/Humanizer/Properties` if the feature still depends on resources.
6. Add tests under `tests/Humanizer.Tests`.
7. Run the relevant source-generator tests, localization tests, and benchmarks.

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
pwsh ./tests/verify-packages.ps1 -PackageVersion <version> -PackagesDirectory ./artifacts/plan-validation
```

## Related Topics

- [Adding Or Updating A Locale](adding-a-locale.md) - Contributor guide for the locale YAML and generator pipeline
- [Installation](installation.md) - How to install language packages
- [Number to Words](number-to-words.md) - Language-specific number formatting
- [DateTime Humanization](datetime-humanization.md) - Relative time in different languages
