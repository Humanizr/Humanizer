# fn-8-add-urdu-ur-locale-with-full-language.9 Wire gendered word-ordinals end-to-end (both `Ordinalize` and `ToOrdinalWords` paths)

## Description

Produce Urdu word-ordinal output (`پانچواں`, not `5واں`) across all three public ordinal API paths (`ToOrdinalWords`, int `Ordinalize`, string `Ordinalize`) with grammatical gender dispatch (masculine / feminine, neuter → masculine fallback). **This task owns the full `ordinal.numeric` YAML surface authoring for Urdu (both masculine AND feminine tables)** — `.4` only authors `ordinal.date` / `.dateOnly` / `clock`. Own both runtime catalogs involved:

1. **`IOrdinalizer`** path — extend / add an ordinalizer engine that converts numeric input to Urdu word stem then appends gendered suffix.
2. **`INumberToWordsConverter`** path — if the number-words engine chosen in `.1` Decision 1b doesn't already emit per-gender word ordinals, extend it here so `5.ToOrdinalWords(gender, "ur")` works.

Keep ordinalizer data in `LocaleFeature.ProfileRoot`; do NOT add typed per-gender properties to `ResolvedLocaleDefinition`.

**Size:** M-L (may split further if `.1`'s decisions expand blast radius — re-plan trigger below)
**Files (expected):**
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs` (edit IF new ordinalizer engine)
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalizerProfileCatalogInput.cs` (edit for new engine / shape)
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/NumberToWordsProfileCatalogInput.cs` (edit IF `.1` 1b owned by `.9`)
- `src/Humanizer/Localisation/Ordinalizers/*.cs` (new / edit — runtime engine)
- `src/Humanizer/Localisation/NumberToWords/*.cs` (edit IF `.1` 1b owned by `.9`)
- `src/Humanizer/Locales/ur.yml` (edit — author the full `surfaces.ordinal.numeric` block covering masculine + feminine tables once the engine lands)
- `tests/Humanizer.SourceGenerators.Tests/…` (new generator test cases)

## Ownership (from `.1` Decisions 1a, 1b)

`.9` owns both paths that produce Urdu word ordinals:
- **Path 1: `IOrdinalizer`** for `(int|string).Ordinalize` — ALWAYS owned by `.9`.
- **Path 2: `INumberToWordsConverter` ordinal branch** for `int.ToOrdinalWords` — owned by `.9` IF the `.1` Decision 1b said the number-words engine needs extension; otherwise already handled in `.3`.

Read parity map `.1` Decisions 1a, 1b to confirm which sub-paths are in scope here.

## Approach

1. Re-anchor on `.1` Decisions 1a + 1b. Current state (from review):
   - `OrdinalizeExtensions` passes numeric strings into `IOrdinalizer.Convert`.
   - `suffix` / `template` / `word-form-template` engines already expose per-gender fields via `OrdinalizerProfileCatalogInput`.
   - None of those engines calls `NumberToWords` to produce a word stem — that's the real gap.
   - `ToOrdinalWords` routes through `INumberToWordsConverter`, a SEPARATE catalog.
2. **Path 1 — `IOrdinalizer`**. Any engine that calls `NumberToWords(n, culture)` to produce a word stem MUST be culture-bound via `OrdinalizerProfileCatalogInput`'s `RequiresCulture` mechanism (`useCulture: true` in the profile definition). The generator today emits parameterless ordinalizer constructors unless the profile opts in; the new / extended engine MUST flip that bit so the generated runtime receives the resolving `CultureInfo` through its constructor. Implement the strategy chosen in `.1` Decision 1a:
   - **(a)** Extend `word-form-template`: add a flag (e.g. `useNumberWordStem: true`) that, when set, substitutes `$numberWords` with `NumberToWords(n, culture)` before template resolution.
   - **(b)** Add a new `number-word-suffix` ordinalizer engine. Runtime: `IOrdinalizer.Convert(int n, string _, GrammaticalGender g)` → `NumberToWords(n, culture) + suffixByGender[g]`, with a suppletive exceptions table for 1–6.
   - **(c)** Exact replacement table (~200 entries covering 1–99 × 2 genders + scale compounds). Acceptable only if reviewer committed in `.1`.
3. **Path 2 — `INumberToWordsConverter`** (if `.1` Decision 1b routed here):
   - Extend the chosen number engine's `ConvertToOrdinal(int, GrammaticalGender)` to compose `NumberToWords(n) + suffixByGender[g]` with the same 1–6 exceptions — ideally sharing the exceptions table with the ordinalizer engine so vocabulary stays consistent.
   - `NumberToWordsProfileCatalogInput` emits per-gender ordinal entries (the profile shape likely exists already for some engines; confirm by inspection).
4. **Keep profile data in `LocaleFeature.ProfileRoot`**. Do NOT add typed per-gender ordinal properties to `ResolvedLocaleDefinition` — that would break the profile-catalog architecture.
5. **Neuter fallback**: `LocaleTheoryMatrixCompletenessTests.AllLocaleGenderTheoryData` enumerates every enum gender per locale. Implement `Neuter → Masculine` fallback for Urdu at a single consistent site (runtime engine entry point or profile lookup) and document. This fallback pattern is the canonical approach used by `.12` / `.13` / `.14`.
6. **Urdu ordinal.numeric data (both genders)** — author the full block here (not split across `.4` / `.9`):
   - Masculine suppletive 1–6: پہلا، دوسرا، تیسرا، چوتھا، پانچواں، چھٹا; productive `-واں` for 7+.
   - Feminine suppletive 1–6: پہلی، دوسری، تیسری، چوتھی، پانچویں، چھٹی; productive `-ویں` for 7+.
   - Exact strings recorded in parity map.
7. **Generator tests**:
   - Synthesize a locale YAML with the chosen engine's per-gender tables; assert profile emission.
   - Assert backward compat: locales with single-table ordinals unchanged.
8. **Runtime tests** (mirror `.6` sanity checks):
   - All six API-path × gender combinations (epic acceptance table).
   - Compound ordinals: `100`, `101`, `100000`, `50` × masculine / feminine (where linguistically applicable) per parity map strings.
   - `Ordinalize(n, Neuter, "ur") == Ordinalize(n, Masculine, "ur")`.
   - Genderless `Ordinalize(n, "ur")` → masculine.
   - Existing locales (`en`, etc.) unchanged.
9. **Re-plan trigger**: if blast radius > ~5 files of generator/runtime, STOP and post re-plan note BEFORE `.12` starts.

## Investigation targets

**Required**:
- Parity map `.1` Decisions 1a, 1b
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalizerProfileCatalogInput.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/NumberToWordsProfileCatalogInput.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/OrdinalizeExtensions.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/Localisation/Ordinalizers/`
- `/Users/claire/dev/Humanizer/src/Humanizer/Localisation/NumberToWords/`

**Optional**:
- `.flow/memory/pitfalls.md` — "data on ResolvedLocaleDefinition" cautions

## Key context

- Review flagged: gender fields already exist; the gap is numeric-input → digit-ordinal output. Focus engine-level, not generic gender plumbing.
- Keep ordinal profile data in `LocaleFeature.ProfileRoot`.
- User-memory: "fewest engines, source-gen over code, minimal allocations." Prefer engine extension over new engine UNLESS extension muddles semantics more.
- Re-plan trigger: >~5-file blast radius → stop.

## Acceptance

- [ ] Engine strategy from `.1` Decision 1a implemented for `IOrdinalizer`.
- [ ] IF `.1` Decision 1b routed to `.9`: engine strategy implemented for `INumberToWordsConverter` gendered ordinal branch.
- [ ] All six assertions pass (epic acceptance table):
  - `5.ToOrdinalWords(Masculine, "ur") == "پانچواں"` and `(Feminine) == "پانچویں"`.
  - `5.Ordinalize(Masculine, "ur") == "پانچواں"` and `(Feminine) == "پانچویں"`.
  - `"5".Ordinalize(Masculine, "ur") == "پانچواں"`.
  - `5.Ordinalize(Neuter, "ur") == Ordinalize(5, Masculine, "ur")` (fallback).
- [ ] Plus: `50.Ordinalize(Masc, "ur") == "پچاسواں"`, `50.Ordinalize(Fem, "ur") == "پچاسویں"`.
- [ ] **Compound ordinals** pass against parity-map-frozen reviewer-approved strings: `100.Ordinalize(Masc, "ur")`, `101.Ordinalize(Masc, "ur")`, `100000.Ordinalize(Masc, "ur")` (and feminine counterparts where linguistically applicable). Same on `ToOrdinalWords`.
- [ ] Genderless call defaults to masculine.
- [ ] `Neuter → Masculine` fallback implemented at a single consistent site and documented as the canonical pattern for `.12` / `.13` / `.14`.
- [ ] Ordinalizer profile data in `LocaleFeature.ProfileRoot`. No typed per-gender properties on `ResolvedLocaleDefinition`.
- [ ] Generator tests for the new / extended engine(s): profile emission + per-gender shape + backward compat.
- [ ] **Culture-binding**: any ordinalizer engine that internally calls `NumberToWords(n, culture)` sets `useCulture: true` (or equivalent) in its profile shape so `OrdinalizerProfileCatalogInput.RequiresCulture` returns true; the generated runtime constructor receives the resolved `CultureInfo` (e.g. `new NumberWordSuffixOrdinalizer(culture, profile)`, NOT a parameterless constructor). Generator tests assert this construction shape.
- [ ] Runtime test proves `ur-PK` / `ur-IN` resolve through the culture-bound ordinalizer — `5.Ordinalize(Masc, "ur-PK") == "پانچواں"` via the same engine resolving `ur` via parent-walk.
- [ ] Full `surfaces.ordinal.numeric` block authored in `ur.yml` here: masculine suppletive 1–6 + `-واں` 7+ AND feminine suppletive 1–6 + `-ویں` 7+. `.4` authored no `ordinal.numeric` content.
- [ ] `dotnet build` passes; source-generator diagnostics clean for every existing locale.
- [ ] Re-plan note posted if blast radius > ~5 files.
- [ ] Parity map updated with per-gender Urdu ordinal entries (all compound tests documented).

## Done summary
Implemented the number-word-suffix ordinalizer engine end-to-end for Urdu gendered word ordinals. The engine resolves the culture's NumberToWordsConverter at runtime, computes cardinal word stems, and applies gendered suffixes with exact replacements for irregular ordinals (1-4, 6, 9). Added ordinal.numeric YAML surface to ur.yml, wired the engine in the source generator with intrinsic culture binding, created the NumberWordSuffixOrdinalizer runtime class, added comprehensive test data across all ordinalizer matrix datasets, and added dedicated UrduOrdinalTests verifying both API paths (Ordinalize + ToOrdinalWords), neuter fallback, and regional variant resolution (ur-PK, ur-IN).
## Evidence
- Commits: 682fdd56292b980b5a2462ba008a5da35a4a064c, 852a2f0a43169fff18e9683f7fd137bd21656808
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0, dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
- PRs: