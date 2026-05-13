# fn-8-add-urdu-ur-locale-with-full-language.6 Add Urdu theory matrix rows and locale-specific exact-output tests

## Description

Wire all three cultures (`ur`, `ur-PK`, `ur-IN`) into every theory dataset covered by `LocaleTheoryMatrixCompletenessTests.ShippedLocaleRows` and add locale-specific exact-output proofs in `tests/Humanizer.Tests/Localisation/ur/`. Every shipped YAML file is a row in every required matrix dataset — there is no "where distinguished" subset.

**Size:** M
**Files (expected):**
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleDateHumanizeTheoryData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleFormatterExactTheoryData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleNumberTheoryData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleNumberMagnitudeTheoryData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleNumberOverloadTheoryData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleAdditionalNumberTheoryData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleAdditionalByteTheoryData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleOrdinalizerMatrixData.cs`
- `tests/Humanizer.Tests/Localisation/LocalePhraseTheoryData.cs`
- `tests/Humanizer.Tests/Localisation/ur/` (new folder)
- `tests/Humanizer.Tests/Localisation/ur/UrduBidiControlSweep.cs` (new helper)

## Approach

1. **Drive-list from the gate**: after `.5`, `.9`, `.10`, `.11` land, run `LocaleTheoryMatrixCompletenessTests`. Its failure output enumerates every dataset missing rows for `ur`, `ur-PK`, `ur-IN`. `ShippedLocaleRows` is computed from every shipped YAML file — all three cultures need rows in every dataset.
<!-- Updated by plan-sync: fn-8.5 already added `ur` rows to LocaleCoverageData.cs (11 rows across date-ordinal, clock, relative-date, list, ordinalizer, and number-to-words datasets). Only `ur-PK` and `ur-IN` rows still need adding there. The separate theory-data files (LocaleDateHumanizeTheoryData, LocaleFormatterExactTheoryData, LocaleNumberTheoryData, LocaleNumberMagnitudeTheoryData, LocaleNumberOverloadTheoryData, LocalePhraseTheoryData, LocaleAdditionalNumberTheoryData, LocaleAdditionalByteTheoryData, LocaleOrdinalizerMatrixData) still need all three rows (`ur`, `ur-PK`, `ur-IN`). -->
2. **Add rows per dataset**: `ur-PK` and `ur-IN` rows in `LocaleCoverageData.cs` (the `ur` rows already exist from `.5`); all three rows (`ur`, `ur-PK`, `ur-IN`) in every other theory-data file, alphabetically placed. Shared content uses identical strings across all three rows; variant-level overrides differ.
3. **Locale-specific proofs** under `tests/Humanizer.Tests/Localisation/ur/`, one file per canonical surface:
   - `UrduNumberToWordsTests.cs` — 0, 21, 99, 100, 1234, 100000, 1234567, 10000000, 1_000_000_000; round-trip parse back to int.
   - `UrduOrdinalizeTests.cs` — all three API paths × masculine / feminine / (neuter → masculine fallback):
     - `5.ToOrdinalWords(Masculine, "ur") == "پانچواں"` and `(Feminine) == "پانچویں"`.
     - `5.Ordinalize(Masculine, "ur") == "پانچواں"` and `(Feminine) == "پانچویں"`.
     - `"5".Ordinalize(Masculine, "ur") == "پانچواں"`.
     - `5.Ordinalize(Neuter, "ur") == Ordinalize(5, Masculine, "ur")` (fallback).
     - Genderless `5.Ordinalize("ur")` uses masculine.
     - **Compound / scale ordinals** (per reviewer concern): `100.Ordinalize(Masculine, "ur")`, `101.Ordinalize(Masculine, "ur")`, `100000.Ordinalize(Masculine, "ur")` — and the feminine counterparts where linguistically applicable — all match parity-map-frozen reviewer-approved strings.
     - Also prove `50.Ordinalize(Masc, "ur") == "پچاسواں"` / `(Fem) == "پچاسویں"`.
   - `UrduDateToOrdinalWordsTests.cs` — handful of dates; applies `UrduBidiControlSweep`.
   - `UrduHijriDateTests.cs` — uses the Hijri contract from `.1` Decision 3; exact-output per month.
   - `UrduRelativeDateTests.cs` — singular + plural phrases per unit, past + future; asserts stem distinctions (`گھنٹہ`/`گھنٹے`, `دن`/`دنوں`, `مہینہ`/`مہینے`).
   - `UrduClockNotationTests.cs` — 1:00, 7:05, 12:00, 12:30, bucket edge.
   - `UrduListHumanizeTests.cs` — two and three-element lists.
   - `UrduDataUnitsTests.cs` — byte-size across units.
   - `UrduCompassTests.cs` — at least one cardinal + one ordinal direction.
4. **`UrduBidiControlSweep` helper**: shared static, asserts string contains no U+200F, U+200E, U+061C. Apply in every test file. This is the guardrail — existing repo tests don't sweep shipped locales for these.
5. **Alphabetical placement**: insert between `tr` and `uz` in datasets ordered alphabetically.
6. `[UseCulture("ur")]` / `[UseCulture("ur-PK")]` / `[UseCulture("ur-IN")]` per CLAUDE.md.
7. Strings in tests match the parity map verbatim — copy, don't paraphrase.
8. Run:
   ```bash
   dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 \
     --filter "FullyQualifiedName~LocaleRegistrySweepTests|LocaleTheoryMatrixCompletenessTests|Localisation.ur"
   ```

## Investigation targets

**Required**:
- `/Users/claire/dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs`
- `/Users/claire/dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`
- `/Users/claire/dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`
- Parity map Decision 3 (Hijri contract test shape)

## Key context

- `ShippedLocaleRows` = one row per shipped YAML file. Three new rows per dataset. Missing = matrix test fails.
- Existing tests do NOT sweep every shipped locale for directionality controls. This task adds the guardrail for Urdu via `UrduBidiControlSweep`. Generalizing it cross-locale is stretch, not required.
- Strings frozen in parity map. Tests copy from the map.

## Acceptance

- [ ] `LocaleTheoryMatrixCompletenessTests` passes; `ur`, `ur-PK`, `ur-IN` rows present in every required dataset.
- [ ] `LocaleRegistrySweepTests` passes.
- [ ] All listed theory-data files contain three new rows each, alphabetically placed.
- [ ] `tests/Humanizer.Tests/Localisation/ur/` folder exists with one file per canonical surface + `UrduBidiControlSweep` helper.
- [ ] `UrduBidiControlSweep` applied in every test file; asserts no U+200F / U+200E / U+061C.
- [ ] Each test uses `[UseCulture("ur")]` / `[UseCulture("ur-PK")]` / `[UseCulture("ur-IN")]`.
- [ ] All three ordinal API paths proven with masculine + feminine + neuter fallback assertions.
- [ ] Compound ordinals (100, 101, 100000, 50) proven for both genders (where linguistically applicable) against parity-map-frozen reviewer-approved strings.
- [ ] All 12 Hijri month names proven via `UrduHijriDateTests` using the `.1` Decision 3 contract.
- [ ] Relative-date tests confirm stem distinctions between one and other counts.
- [ ] Every test string matches the parity-map-frozen reviewer-approved value verbatim.
- [ ] All new tests pass on net10.0 and net8.0 (net48 runtime verified in `.7`).

## Done summary
Added ur-PK and ur-IN rows to all theory matrix datasets checked by LocaleTheoryMatrixCompletenessTests (7 theory data files, 658 new rows total). Created 8 new locale-specific exact-output test files under tests/Humanizer.Tests/Localisation/ur/ covering number-to-words, date-to-ordinal-words, relative dates, clock notation, list humanize, data units, and compass. Created UrduBidiControlSweep helper and applied it in all test files. Updated existing UrduOrdinalTests and UrduHijriDateTests to use the sweep. All 5460 matrix completeness tests and 129 Urdu locale tests pass on both net10.0 and net8.0.
## Evidence
- Commits: b2ec87248e3fd9e6fca498f2e9e1e8f2b2e6e8d2, 9c01dcfa739055e96420dc3cce41737fbbe1826a
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --filter-class Humanizer.Tests.Localisation.LocaleTheoryMatrixCompletenessTests, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --filter-namespace Humanizer.Tests.Localisation.ur, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 --filter-namespace Humanizer.Tests.Localisation.ur
- PRs: