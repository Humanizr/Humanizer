# Humanizer Locale Parity Checklist

This is the authoritative detailed reference for `$add-locale`. `SKILL.md` intentionally stays short; this file owns the canonical surface inventory, parity-map schema, file/test map, fast-fail matrix, and validation details. Do not duplicate these tables back into `SKILL.md`.

Use this file after reading `SKILL.md` and the target locale/parent YAML files.

## Evidence Lanes

- **Local scratch:** `artifacts/YYYY-MM-DD-<locale>-parity-map.md`. It is gitignored working evidence and must not be committed.
- **Committed proof:** locale YAML, generator/runtime changes, xUnit tests, and docs when authoring behavior changes.
- **PR/final evidence:** copy the parity map closeout summary, effective-gap summary, before/after delta, and validation commands into the PR or final response.

Do not claim parity from the scratch artifact alone. Passing tests also is not enough unless those tests cover every required row.

## Surface Inventory

Treat these as the localized surfaces that must be intentionally accounted for when a shipped locale is added or brought to parity:

Canonical authoring surfaces under `surfaces` are exactly `list`, `formatter`, `phrases`, `number`, `ordinal`, `clock`, `compass`, and `calendar`. Canonical nested members are `number.words`, `number.parse`, `number.formatting`, `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, `calendar.months`, `calendar.monthsGenitive`, and `calendar.hijriMonths`.

- `list` — collection formatting and conjunction behavior.
- `formatter` — formatter grammar/resources and formatter-side grammatical metadata.
- `phrases` — relative date phrases, duration phrases, data-unit phrases, and time-unit phrases.
- `number`
  - `number.words` — cardinal words, ordinal words, tuple conversion, and grammar-sensitive number rendering.
  - `number.parse` — native words-to-number parsing for natural written forms.
  - `number.formatting` — decimal separator override for stable cross-platform numeric output when ICU/NLS data differs.
- `ordinal`
  - `ordinal.numeric` — numeric ordinalization for `Ordinalize`.
  - `ordinal.date` — `DateTime.ToOrdinalWords`.
  - `ordinal.dateOnly` — `DateOnly.ToOrdinalWords` on supported targets.
- `clock` — `TimeOnly.ToClockNotation` on supported targets.
- `compass` — locale-owned compass directions when the surface exists or is introduced.
- `calendar`
  - `calendar.months` — exactly 12 nominative month entries when present.
  - `calendar.monthsGenitive` — exactly 12 genitive month entries when present.
  - `calendar.hijriMonths` — exactly 12 Islamic month entries when authored or inherited.

Parity allows only:

- locale-owned behavior that is tested
- same-language inherited behavior that is tested and natural for the child locale

Parity rejects English fallback, unsupported-locale exceptions, generic/default runtime fallback, and follow-up promises.

## Required Parity Map Artifact

Create and maintain:

`artifacts/YYYY-MM-DD-<locale>-parity-map.md`

Use this table shape:

| surface | ownership path | current state | target state | support state | proof kind | files to change | tests proving parity | proof file/assertion | verification command | verification exit status | verified at | status | term review status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |

Allowed `current state` and `target state` values:

- `locale-owned`
- `same-language inherited`
- `missing`
- `english-fallback`
- `unsupported`
- `unknown` only during preflight, never at implementation start
- `not applicable` only for conditional proof subrows, never top-level canonical surface rows

Allowed `support state` values:

- `supported`
- `not supported`
- `not applicable` only for justified conditional proof subrows

Allowed `proof kind` values:

- `locale-owned exact-output`
- `same-language inherited exact-output`
- `locale-owned structural assertion`
- `same-language inherited structural assertion`
- `generic fallback`
- `unsupported`
- `not applicable` only for justified conditional proof subrows

Any `proof kind: generic fallback` row fails parity. `not applicable` must name why the conditional row does not apply.

Allowed `status` values include:

- `not-started`
- `blocked`
- `in-progress`
- `proved`

Allowed `term review status` values include:

- `not-needed`
- `pending`
- `proposer-approved`
- `reviewer-rejected`
- `resolved`

A row can be `proved` only after it names a concrete proving file/assertion and a verification command with exit status.

### Required Rows

Add top-level rows and proof subrows for:

- `list`
- `formatter`
- `phrases.relativeDate`
- `phrases.duration`
- `phrases.dataUnits`
- `phrases.timeUnits`
- `number.words`
- `number.words.cardinal`
- `number.words.ordinal`
- `number.parse`
- `number.parse.cardinal`
- `number.parse.ordinal`
- `number.formatting.decimalSeparator` when the locale authors a `number.formatting` override; otherwise mark inherited or not applicable
- `ordinal.numeric`
- `ordinal.date`
- `ordinal.dateOnly`
- `clock`
- `compass`
- `calendar.months` when the locale authors a `calendar` override; otherwise mark inherited or not applicable
- `calendar.monthsGenitive` when the locale authors a genitive array; otherwise mark inherited or not applicable
- `calendar.hijriMonths` when authored or inherited; otherwise mark not applicable

Add more `number.words.*`, `number.parse.*`, or engine-specific rows when the selected engine has tuple handling, gendered variants, abbreviation parsing, special composition paths, or other meaningful branches.

### Required Sections

The parity map must also include:

- `Preflight Gap Report` — one row per canonical surface with initial state and ownership path.
- `Effective Gap Summary` — unresolved surfaces after same-language inheritance; it must end empty except for justified conditional proof subrows marked `not applicable`.
- `Before/After Parity Delta` — initial unresolved set and final unresolved set.
- `Surface Closeout` — one line per canonical surface and proof subrow with final ownership path, support state, proof kind, proof assertion, and term-review status.

## Fast-Fail Matrix

| Risk | Applies when | Probe | Required proof | Reference |
| --- | --- | --- | --- | --- |
| Artifact policy mismatch | A parity map or audit file is used as evidence | Is the file under gitignored `artifacts/`? | Keep scratch local; commit tests/docs; copy closeout summary to PR/final response. | Evidence Lanes |
| Ordinal API split | Any ordinal work | Are `number.words.ordinal`, `ordinal.numeric`, `ordinal.date`, and `ordinal.dateOnly` planned separately? | Separate rows, implementation paths, and tests. | Surface Inventory |
| Word ordinal output | Locale needs ordinal words, gendered ordinals, or non-digit ordinal phrasing | Would suffix/template engines emit digit+suffix output? | Use a word-aware engine and exact-output tests for irregular stems. | Urdu learnings: ordinal |
| Culture binding | Engine calls `NumberToWords`, parser, or another culture-aware service | Could ambient current culture leak into target-culture output? | Test with current culture different from target culture. | Urdu learnings: culture |
| ICU/NLS calendar differences | Hijri/UmAlQura or non-Gregorian behavior | Does a culture support the calendar on every target TFM/platform? | Capability-gated tests that keep the proof strong instead of weakening expectations. | Urdu learnings: calendar |
| Optional schema drop | New optional YAML field, omitted `surfaces`, or schema extension | Could parser/resolver/emitter/migration silently drop it? | Source-generator tests for parse, inheritance/defaulting, emission/profile input, and runtime consumption. | Urdu learnings: schema |
| Arabic-script lookalikes | Arabic-script or neighboring script-family locale | Are visually similar code points copied from a different language? | Codepoint sanity check and exact-output tests using target script letters. | Urdu learnings: arabic-script |
| Plural rule copy | Nearby language has different plural categories | Was plural family copied by proximity? | CLDR-style plural family decision and resource-key tests. | Urdu learnings: plural-rules |
| South Asian scales | Indic/South Asian number words | Are lakh/crore/arab/kharab natural for the locale? | Large-number word and parse tests across scale boundaries. | Urdu learnings: indic |
| No-delta variants | Regional file exists only for matrix/registry coverage | Does it have real overrides or only parent inheritance? | Omit `surfaces` for no-delta variants; prove same-language inheritance. | Urdu learnings: variant |
| Shared engine masking | Shared kernel or registry dispatch changes | Could real locale data make positive and negative/edge paths look identical? | Synthetic/sentinel tests exercising divergent branches. | Urdu learnings: engine-tests |
| Repo hygiene | Before PR/final response | Are `.claude/*.lock`, build outputs, unrelated edits, or namespace mismatches present? | Clean status and targeted tests; namespace matches culture folder. | Urdu learnings: hygiene |

## Primary Repo Files

Start with these:

- `docs/adding-a-locale.md`
- `docs/locale-yaml-how-to.md`
- `docs/locale-yaml-reference.md` when schema/engine options are unclear
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

| surface | primary locale source | likely runtime/generator touchpoints | likely tests |
| --- | --- | --- | --- |
| `list` | `src/Humanizer/Locales/<locale>.yml` | generated collection formatter wiring | `LocaleCoverageData`, `LocaleFallbackSweepTests`, culture-specific collection tests |
| `formatter` | `src/Humanizer/Locales/<locale>.yml` | formatter profile generation, formatter registries, shared formatter kernels, grammar extraction | `GeneratedFormatterRuntimeTests`, `LocaleCoverageData`, culture-specific formatter tests |
| `phrases` | `src/Humanizer/Locales/<locale>.yml` | phrase profile generation and generated phrase tables | `GeneratedFormatterRuntimeTests`, `LocaleCoverageData`, culture-specific phrase/humanize tests |
| `number.words` | `src/Humanizer/Locales/<locale>.yml` | `EngineContractCatalog.cs`, number profile catalogs, number-to-words runtime kernels | `LocaleRegistrySweepTests`, culture-specific `NumberToWordsTests` |
| `number.parse` | `src/Humanizer/Locales/<locale>.yml` | parse profile catalogs, `WordsToNumberConverterRegistry`, words-to-number runtime kernels | `LocaleRegistrySweepTests`, `WordsToNumberTests`, culture-specific parsing tests |
| `ordinal.numeric` | `src/Humanizer/Locales/<locale>.yml` | ordinalizer registry/profile wiring or residual ordinalizer runtime | `LocaleRegistrySweepTests`, culture-specific `OrdinalizeTests` |
| `ordinal.date` | `src/Humanizer/Locales/<locale>.yml` | date-to-ordinal registries, ordinal date runtime patterns/kernels | `LocaleRegistrySweepTests`, `ExactLocaleDateAndTimeRegistryTests`, culture-specific date-ordinal tests |
| `ordinal.dateOnly` | `src/Humanizer/Locales/<locale>.yml` | date-only ordinal registries and shared ordinal-date runtime paths | `LocaleRegistrySweepTests`, `ExactLocaleDateAndTimeRegistryTests`, culture-specific date-only tests |
| `clock` | `src/Humanizer/Locales/<locale>.yml` | clock notation registries, `src/Humanizer/Localisation/TimeToClockNotation/*` | `LocaleRegistrySweepTests`, `ExactLocaleDateAndTimeRegistryTests`, culture-specific clock tests |
| `compass` | `src/Humanizer/Locales/<locale>.yml` | generated compass/profile wiring and compass runtime support | culture-specific compass tests and sweep coverage when present |
| `calendar` | `src/Humanizer/Locales/<locale>.yml` | calendar month-name override generation and ordinal-date profile catalogs | `LocaleRegistrySweepTests`, culture-specific date-ordinal tests |
| `number.formatting` | `src/Humanizer/Locales/<locale>.yml` | decimal-separator override generation in locale registry/profile input | `LocaleRegistrySweepTests`, culture-specific number-formatting tests |

## Implementation Decisions

Use this order:

1. Check whether same-language inheritance already works and is natural.
2. Reuse an existing shared engine if the algorithm matches with locale-owned words or tables.
3. Add or extend a shared structural engine when the rule family is reusable.
4. Keep or add a locale-specific runtime leaf only when the behavior is genuinely procedural.

When inheritance is used, record the full chain to the terminal owner and add at least one locale-specific proving assertion for each inherited canonical surface.

## Term Authoring Review

When new locale-owned wording is introduced, use two independent passes before finalizing terms:

- proposer subagent: suggest terms for the exact surface and runtime usage
- reviewer subagent: native or near-native review for naturalness, correctness, register, grammar, inflection, and parser implications

Then review representative composed runtime outputs. Isolated word approval is not enough.

Record in the parity map:

- accepted terms and rejected alternatives
- final rationale
- reviewer confidence and limitations
- reviewed runtime outputs
- unresolved disagreements, if any

Unresolved disagreement or non-credible reviewer confidence blocks completion.

## Test Expectations

Every parity change should leave behind both exact-output and sweep-style proof:

- culture-specific tests under `tests/Humanizer.Tests/Localisation/<culture>`
- registry and fallback sweep updates when the locale joins or changes a shipped surface
- source-generator tests when YAML schema, inheritance, profile generation, or registry wiring changes
- synthetic shared-engine tests when a shared engine or dispatch path changes

When auditing failures, distinguish:

- direct locale ownership
- acceptable same-language inheritance
- unresolved effective gaps after following `variantOf`
- generic/default fallback that happens to return a value

Useful checks already in the repo:

- formatter coverage and parent-inheritance expectations in `LocaleCoverageData`
- fallback sweep coverage in `LocaleFallbackSweepTests`
- number, ordinal, date, clock, and words-to-number registry coverage in `LocaleRegistrySweepTests`
- exact generated formatter behavior in `GeneratedFormatterRuntimeTests`
- exact locale-owned date and clock behavior in `ExactLocaleDateAndTimeRegistryTests`

Do not treat "the suite passed" as row-level proof. Each canonical surface and proof subrow needs a proving assertion or exact-output expectation recorded in the parity artifact.

## Verification Commands

Run focused commands while iterating. Before claiming parity, run:

```bash
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/locale-parity-validation
```

Run `net48` tests only on Windows hosts when the test project includes that TFM.

## Explicit Stop Conditions

Stop and keep working if any of these remain true:

- a surface still depends on implicit fallback rather than intentional ownership
- the preflight gap report still has `unknown` entries
- a same-language parent output looks unnatural for the child locale
- the parity map does not match the canonical surface list in `CanonicalLocaleAuthoring.cs`
- the parity map has gaps or unresolved review states
- any surface row or proof subrow lacks concrete proof or is not yet `proved`
- exact-output coverage is missing for grammar-sensitive behavior
- runtime or generator ownership is unclear from the code paths changed
- shared engine changes lack synthetic edge-branch coverage

Before marking a locale complete, answer this literally:

> Can every shipped localized feature for this locale now execute through intentional locale ownership or intentional same-language inheritance, with no English fallback, no unsupported-locale gaps, no generic fallback dependency, and passing parity tests?

If the answer is not an unqualified yes, keep working.
