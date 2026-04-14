# fn-11-fix-urdu-locale-ci-pr-feedback-rebase.3 PR feedback: unify NumberWordSuffixOrdinalizer negative/positive irregulars

## Description
Fix the negative-irregular path in `NumberWordSuffixOrdinalizer` so that negative and positive ordinals both source irregulars from the ordinalizer's own `ExactReplacements`, instead of positive coming from `surfaces.ordinal.numeric` and negative coming from `surfaces.number.words.ordinal` via `INumberToWordsConverter.ConvertToOrdinal`. Add a test that would have caught the divergence.

**Size:** M
**Files:**
- `src/Humanizer/Localisation/Ordinalizers/NumberWordSuffixOrdinalizer.cs`
- `tests/Humanizer.Tests/Localisation/ur/UrduNumberToWordsTests.cs` (or a nearby ordinal-focused Urdu test file — pick the file already covering ordinalizer irregular paths)

## Approach
Per Codex reviewer:

> This branch checks `block.ExactReplacements` for a negative number's magnitude, but then ignores the matched ordinal and delegates to `INumberToWordsConverter.ConvertToOrdinal`. That makes negative irregulars come from `surfaces.number.words.ordinal` while positive irregulars come from `surfaces.ordinal.numeric`, so the two APIs can diverge… Use the matched `negExact` path here so negative and positive irregular handling stays consistent within the ordinalizer configuration.

Concretely: at `src/Humanizer/Localisation/Ordinalizers/NumberWordSuffixOrdinalizer.cs:53`, when `block.ExactReplacements.TryGetValue(magnitude, out var negExact)` matches, use `negExact` to build the negative ordinal (apply the negative-prefix formatting the surrounding block already prescribes) instead of falling through to `INumberToWordsConverter.ConvertToOrdinal`.

Then add a unit test that:
- Configures or exercises an ordinalizer whose `ExactReplacements` (positive side) differs from what the underlying `INumberToWordsConverter.ConvertToOrdinal` would produce for the same magnitude.
- Asserts that the negative case returns the `ExactReplacements`-derived form, not the converter-derived form. Before this fix, that assertion fails; after, it passes.
- If the existing Urdu surface has such a divergence naturally, a locale-driven test is ideal. Otherwise, use an existing cultural surface that has ordinalizer-level irregulars.

Do NOT:
- Change public APIs or ordinalizer wiring beyond this single branch.
- Add unrelated refactors.
- Skip the regression test.

## Investigation targets
**Required:**
- `src/Humanizer/Localisation/Ordinalizers/NumberWordSuffixOrdinalizer.cs:1-120` — read the whole class to see how positive irregulars are emitted (the pattern to mirror) and how `negExact` should plug in.
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalizerProfileCatalogInput.cs` — how `ExactReplacements` are populated into the generated profile, to understand what `negExact` actually holds.
- An existing ordinalizer test (e.g. tests under `tests/Humanizer.Tests/Localisation/ur/` covering ordinal output) for assertion style and culture setup.

**Optional:**
- `src/Humanizer/Localisation/NumberToWords/IndianGroupingGenderedNumberToWordsConverter.cs` — context for how `ConvertToOrdinal` computes its output, to design a divergence that the regression test can exercise.

## Key context
- This is the Codex reviewer's P2 comment on PR #1720. The comment itself includes a link into the file — use the anchor above.
- The fix should be small: one branch, one code path. The test is where the real work is, because it must be designed to fail without the fix.
## Acceptance
- [ ] Negative-irregular branch uses the matched `negExact` from `block.ExactReplacements` instead of delegating to `INumberToWordsConverter.ConvertToOrdinal`.
- [ ] Regression test asserts the negative ordinal matches the ordinalizer's `ExactReplacements` form and would fail on pre-fix code.
- [ ] All Humanizer.Tests pass on net10.0 and net8.0.
- [ ] Lint + build clean.
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
