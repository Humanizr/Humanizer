## Description
Close tail-coverage on the **scale + gender NumberToWords converter family** (8 classes). Scope narrowed from the original 12-class lump — ordinal NumberToWords engines + PhraseClockNotation + WordFormTemplateOrdinalizer moved to `.15`.

**Size:** M
**Files:**
- `tests/Humanizer.Tests/ConverterTail/NumberToWordsScaleGenderCoverageTests.cs` (new) — OR extend the relevant `tests/Humanizer.Tests/Localisation/<culture>/` file where the uncovered branch is culture-specific

## Target classes
- `Humanizer.PluralizedScaleNumberToWordsConverter` (90.3%)
- `Humanizer.GenderedNumberToWordsConverter` (90.9%)
- `Humanizer.ScaleStrategyNumberToWordsConverter` (92.6%)
- `Humanizer.ConjunctionalScaleNumberToWordsConverter` (93.1%)
- `Humanizer.ConjoinedGenderedScaleNumberToWordsConverter` (93.2%)
- `Humanizer.SegmentedScaleNumberToWordsConverter` (93.5%)
- `Humanizer.WestSlavicGenderedNumberToWordsConverter` (94.5%)
- `Humanizer.AppendedGroupNumberToWordsConverter` (94.9%)

## Approach
- Read `artifacts/fn-9-baseline/uncovered.json` (from .1) for each target; list uncovered lines + branches in a test-file header comment.
- Per-locale-idiomatic branches: write tests under the relevant `tests/Humanizer.Tests/Localisation/<culture>/` folder. Because fn-9 depends on fn-8's completion, every locale is safe to touch. Cultures involved across these engines: ru, pl, cs, uk, ar, fr, de, es, he, ja.
- Common uncovered shapes: `long.MinValue` overflow guard (test the existing throw; do NOT change source), negative-input branches, scale-word boundaries (999 → 1 000, 999 999 → 1 000 000, etc.), gender-specific overloads.

## Investigation targets
**Required:**
- `artifacts/fn-9-baseline/uncovered.json`
- Each target `.cs` under `src/Humanizer/Localisation/NumberToWords/`
- Existing locale test folders for ru / pl / cs / uk / ar / fr / de / es / he

## Acceptance
- [ ] All 8 target classes reach ≥95% line and ≥90% branch in the merged multi-TFM report.
- [ ] No source changes (tests only).
- [ ] Every new test asserts on specific expected output.
- [ ] No `[ExcludeFromCodeCoverage]` introduced.
- [ ] No deferral or hold-list language.

## Done summary
_To be filled on completion._

## Evidence
- Commits:
- Tests:
- PRs:
