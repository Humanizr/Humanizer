# Humanizer Locale Parity Checklist

Use this file after reading `SKILL.md`. It is the detailed repo map for proving that locale work is actually complete.

## Surface Inventory

Treat these as the localized surfaces that must be intentionally accounted for when a shipped locale is added or brought to parity:

Canonical authoring surfaces under `surfaces` are exactly `list`, `formatter`, `phrases`, `number`, `ordinal`, `clock`, `compass`, and `calendar`. Canonical nested members are `number.words`, `number.parse`, `number.formatting`, `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, `calendar.months`, `calendar.monthsGenitive`, and `calendar.hijriMonths`.

- `list`
  Collection formatting and conjunction behavior.
- `formatter`
  Formatter grammar/resources and formatter-side grammatical metadata.
- `phrases`
  Relative date phrases, duration phrases, data-unit phrases, and time-unit phrases.
- `number`
  Parent surface for `number.words`, `number.parse`, and `number.formatting`.
  - `number.words` — Cardinal words, ordinal words, tuple conversion, and any grammar-sensitive number rendering.
  - `number.parse` — Native words-to-number parsing for the locale's natural written forms.
  - `number.formatting` — Decimal separator override for stable cross-platform numeric output. Only authored when the locale's ICU-supplied decimal separator differs across platforms.
- `ordinal`
  Parent surface for `ordinal.numeric`, `ordinal.date`, and `ordinal.dateOnly`.
  - `ordinal.numeric` — Numeric ordinalization for `Ordinalize`.
  - `ordinal.date` — `DateTime.ToOrdinalWords`.
  - `ordinal.dateOnly` — `DateOnly.ToOrdinalWords` on supported targets.
- `clock`
  `TimeOnly.ToClockNotation` on supported targets.
- `compass`
  Locale-owned compass directions if the locale surface exists or is being introduced.
- `calendar`
  Month-name overrides (nominative and genitive) for stable cross-platform date output. Only authored when the locale's ICU-supplied month names differ across platforms.
  - `calendar.months` — Exactly 12 nominative month-name entries when present.
  - `calendar.monthsGenitive` — Exactly 12 genitive month-name entries when present.
  - `calendar.hijriMonths` — Exactly 12 Islamic (Hijri) month-name entries when present. Used when the culture's calendar is Hijri/UmAlQura and `calendarMode` is `Native`.

There is no shipped-locale exemption list in this repo. If any canonical surface is unresolved, parity is incomplete.

Parity means each surface above is one of:

- locale-owned and tested
- intentionally inherited from a same-language parent and tested where inheritance matters

Parity does not allow:

- English fallback for shipped locales
- unsupported-locale exceptions for shipped locales
- "we only translated the YAML we already had"
- leaving a missing surface for a later pass

## Required Parity Map Artifact

Create and maintain a working artifact at:

`artifacts/YYYY-MM-DD-<locale>-parity-map.md`

This file is committed to source control so downstream tasks and reviewers can reference it throughout the epic. It is the working proof that the locale has been audited across all shipped localized surfaces.

Use this table shape:

| surface | ownership path | current state | target state | files to change | tests proving parity | proof file/assertion | verification command | verification exit status | verified at | status | term review status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |

Use these state values only:

- `locale-owned`
- `same-language inherited`
- `missing`
- `english-fallback`
- `unsupported`

Use `term review status` values such as:

- `not-needed`
- `pending`
- `proposer-approved`
- `reviewer-rejected`
- `resolved`

Use `status` values such as:

- `not-started`
- `blocked`
- `in-progress`
- `proved`

`proof file/assertion` should name the concrete file and assertion or exact-output expectation that proves the surface.

`verification command` should be the command that exercises the proof, even if it is a focused test command used before the final full-suite run.

`verification exit status` should record the actual exit code.

`verified at` should record the local timestamp when the proof was run.

The artifact is not done until every shipped localized surface is present and no row remains at `missing`, `english-fallback`, or `unsupported`.

Below the table, add an `Effective Gap Summary` section listing any surfaces still unresolved for the locale after considering same-language inheritance. This should end empty.

Also include:

- a `Preflight Gap Report` section with one row per canonical surface marked `locale-owned`, `same-language inherited`, `missing`, `english-fallback`, `unsupported`, or `unknown`
- a `Before/After Parity Delta` section listing the initial unresolved surfaces and the final unresolved surfaces
- a `Surface Closeout` section with one line per canonical surface recording final ownership and proof

Required proof subrows:

- `phrases.relativeDate`
- `phrases.duration`
- `phrases.dataUnits`
- `phrases.timeUnits`
- `number.words.cardinal`
- `number.words.ordinal`
- `number.parse.cardinal`
- `number.parse.ordinal`
- `number.formatting.decimalSeparator` (only when the locale authors a `number.formatting` override; mark "inherited from parent" or "not applicable" otherwise)
- `calendar.months` (only when the locale authors a `calendar` override; mark "inherited from parent" or "not applicable" otherwise)
- `calendar.monthsGenitive` (only when the locale authors a `calendar` override with a genitive array; mark "inherited from parent" or "not applicable" otherwise)
- `calendar.hijriMonths` (only when the locale authors a `calendar.hijriMonths` override; mark "inherited from parent" or "not applicable" otherwise)

Add more `number.words.*` and `number.parse.*` proof subrows whenever the selected engine owns additional meaningful branches such as tuple handling, gendered variants, abbreviation parsing, or special composition paths.

## Primary Repo Files

Start with these:

- `docs/adding-a-locale.md`
- `docs/locale-yaml-how-to.md`
- `docs/locale-yaml-reference.md`
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs`
- `src/Humanizer/Locales/<locale>.yml`
- parent locale YAML files when `variantOf` is involved

Inspect these when parity requires runtime or generator work:

- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/*`
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`
- `src/Humanizer/Localisation/*`

Inspect these when proving parity:

- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs`
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`
- `tests/Humanizer.Tests/Localisation/GeneratedLocaleData/GeneratedFormatterRuntimeTests.cs`
- `tests/Humanizer.Tests/Localisation/ExactLocaleDateAndTimeRegistryTests.cs`
- `tests/Humanizer.Tests/Localisation/<culture>/*`
- `tests/Humanizer.SourceGenerators.Tests/*`

## Surface-To-Files Matrix

Use this as the default starting point for where each surface usually needs work:

| surface | primary locale source | likely runtime/generator touchpoints | likely tests |
| --- | --- | --- | --- |
| `list` | `src/Humanizer/Locales/<locale>.yml` | generated collection formatter wiring | `LocaleCoverageData`, `LocaleFallbackSweepTests`, locale-specific collection tests |
| `formatter` | `src/Humanizer/Locales/<locale>.yml` | formatter profile generation, formatter registries, shared formatter kernels, grammar extraction | `GeneratedFormatterRuntimeTests`, `LocaleCoverageData`, locale-specific formatter tests |
| `phrases` | `src/Humanizer/Locales/<locale>.yml` | phrase profile generation and generated phrase tables used by formatter/runtime paths | `GeneratedFormatterRuntimeTests`, `LocaleCoverageData`, locale-specific phrase and humanize tests |
| `number.words` | `src/Humanizer/Locales/<locale>.yml` | `EngineContractCatalog.cs`, number profile catalogs, number-to-words runtime kernels | `LocaleRegistrySweepTests`, locale-specific `NumberToWordsTests` |
| `number.parse` | `src/Humanizer/Locales/<locale>.yml` | parse profile catalogs, `WordsToNumberConverterRegistry`, words-to-number runtime kernels | `LocaleRegistrySweepTests`, `WordsToNumberTests`, locale-specific parsing tests |
| `ordinal.numeric` | `src/Humanizer/Locales/<locale>.yml` | ordinalizer registry/profile wiring or residual ordinalizer runtime | `LocaleRegistrySweepTests`, locale-specific `OrdinalizeTests` |
| `ordinal.date` | `src/Humanizer/Locales/<locale>.yml` | date-to-ordinal registries, ordinal date runtime patterns/kernels | `LocaleRegistrySweepTests`, `ExactLocaleDateAndTimeRegistryTests`, locale-specific date-ordinal tests |
| `ordinal.dateOnly` | `src/Humanizer/Locales/<locale>.yml` | date-only ordinal registries, shared ordinal date runtime paths | `LocaleRegistrySweepTests`, `ExactLocaleDateAndTimeRegistryTests`, locale-specific date-only tests |
| `clock` | `src/Humanizer/Locales/<locale>.yml` | clock notation registries, `src/Humanizer/Localisation/TimeToClockNotation/*` | `LocaleRegistrySweepTests`, `ExactLocaleDateAndTimeRegistryTests`, locale-specific clock tests |
| `compass` | `src/Humanizer/Locales/<locale>.yml` | generated compass/profile wiring and any compass-specific runtime support | locale-specific compass tests and any sweep coverage that exercises registry presence |
| `calendar` | `src/Humanizer/Locales/<locale>.yml` | `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs`, calendar month-name override generation | `LocaleRegistrySweepTests`, locale-specific date-ordinal tests |
| `number.formatting` | `src/Humanizer/Locales/<locale>.yml` | `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`, decimal-separator override generation | `LocaleRegistrySweepTests`, locale-specific number-formatting tests |

## Implementation Decisions

Use this order:

1. Check whether the locale already works through explicit same-language inheritance.
2. Reuse an existing shared engine if the behavior is the same algorithm with different locale data.
3. Add or extend a shared structural engine if the rule family is reusable.
4. Keep or add a locale-specific runtime leaf only when the behavior is genuinely procedural.

If step 4 happens, justify it in code and tests. Do not quietly normalize a locale-specific leaf as if it were generic.

If inheritance is used, record the full chain to the terminal owner and add at least one locale-specific proving assertion for each inherited canonical surface.

## Term Authoring Review

When the work introduces new locale-owned wording, use two subagents before finalizing terms:

- a proposer subagent to generate the most language-appropriate terms for the target feature surface
- an independent reviewer subagent acting as a native speaker to review those terms for naturalness and correctness

The reviewer must actively look for:

- literal translations that miss how native speakers actually express the concept
- awkward register or phrasing
- incorrect grammar, gender, plurality, or inflection
- parser tokens or number terms that are technically understandable but not idiomatic

Do not accept locale-owned wording until both passes converge or the disagreement is resolved with a justified final choice.

If the work introduces new locale-owned wording, run the reviewer again on representative composed runtime outputs. Approving isolated terms is not enough if the final Humanizer sentences are awkward. If the reviewer cannot credibly claim native-level or near-native confidence for judging naturalness, completion is blocked.

Term acceptance rubric:

- the phrase is something native speakers would actually say in this product context
- grammar, gender, plurality, and inflection match the runtime usage
- parser tokens reflect natural written forms rather than dictionary glosses
- the phrase fits Humanizer semantics, not just literal translation of an English resource key
- rejected alternatives and final rationale are recorded in the parity map artifact
- reviewer confidence, reviewer limitations, and reviewed runtime outputs are recorded in the parity map artifact

## Test Expectations

Every parity change should leave behind both exact-output and sweep-style proof:

- culture-specific tests under `tests/Humanizer.Tests/Localisation/<culture>`
- registry and fallback sweep updates when the locale joins or changes a shipped surface
- source-generator tests when YAML schema, inheritance, profile generation, or registry wiring change

When auditing failures, distinguish:

- direct locale ownership
- acceptable same-language inheritance
- unresolved effective gaps after following `variantOf`

Useful checks already in the repo:

- formatter coverage and parent-inheritance expectations in `LocaleCoverageData`
- fallback sweep coverage in `LocaleFallbackSweepTests`
- number, ordinal, date, clock, and words-to-number registry coverage in `LocaleRegistrySweepTests`
- exact generated formatter behavior in `GeneratedFormatterRuntimeTests`
- exact locale-owned date and clock behavior in `ExactLocaleDateAndTimeRegistryTests`

Do not treat "the test suite passed" as proof by itself. Each canonical surface and required proof subrow needs an identifiable proving test, assertion, or exact-output expectation recorded in the parity artifact.

## Verification Commands

```
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release 
```

Run narrower focused tests first if you are iterating, but do not claim parity until the full commands above pass.

## Explicit Stop Conditions

Stop and keep working if any of these remain true:

- a surface still depends on implicit fallback rather than intentional ownership
- the preflight gap report still has `unknown` entries
- a same-language parent output looks unnatural for the child locale
- the parity map does not match the canonical surface list in `CanonicalLocaleAuthoring.cs`
- the parity map has gaps or unresolved review states
- any surface row or required proof subrow lacks concrete proof or is not yet `proved`
- exact-output coverage is missing for grammar-sensitive behavior
- runtime or generator ownership is still unclear from the code paths you changed

## Final Question

Before marking the work complete, answer this literally:

"Can every shipped localized feature for this locale now execute through intentional locale ownership or intentional same-language inheritance, with no English fallback, no unsupported-locale gaps, and passing parity tests?"

If the answer is not an unqualified yes, keep working.

## Common Rationalizations To Reject

- "The parent locale probably covers that."
- "That surface was never implemented for this locale anyway."
- "The YAML shape is valid, so parity is basically done."
- "I only changed wording, so sweep coverage is unnecessary."
- "The output is understandable even if it is not what native speakers would say."
- "I can leave the remaining surfaces for a later locale pass."

All of these mean the locale is not yet at parity.
