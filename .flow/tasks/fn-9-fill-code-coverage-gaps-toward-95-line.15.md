## Description
Close tail-coverage on **ordinal NumberToWords engines + `PhraseClockNotationConverter` + `WordFormTemplateOrdinalizer`** (4 classes). Split from the original lumped tail task.

**Size:** M
**Files:**
- `tests/Humanizer.Tests/Localisation/OrdinalAndClockCoverageTests.cs` (new) — OR extend the relevant `tests/Humanizer.Tests/Localisation/<culture>/` file where the uncovered branch is culture-specific
<!-- Updated by plan-sync: fn-9...14 placed tail-coverage tests in Localisation/ not ConverterTail/ -->

## Target classes
- `Humanizer.TerminalOrdinalScaleNumberToWordsConverter` (92.7%)
- `Humanizer.HarmonyOrdinalNumberToWordsConverter` (94.3%)
- `Humanizer.PhraseClockNotationConverter` (91.0%)
- `Humanizer.WordFormTemplateOrdinalizer` (89.7%)

## Approach
- Read `artifacts/fn-9-local-coverage/uncovered.json` (from .1); list uncovered lines per class.
- Ordinal engines (`TerminalOrdinalScale`, `HarmonyOrdinal`): likely missing gendered-ordinal + negative-input + scale-boundary branches. Locales involved: ja, ko, ru, pl, he, ar, fr, es, it, pt, de.
- `PhraseClockNotationConverter`: likely missing edge branches for specific phrase-table lookups (null / fallback) and corner hours (midnight / noon / quarter-past / half-past wrap). Locales: th, vi, ja, km.
- `WordFormTemplateOrdinalizer`: likely missing template-substitution edge branches (null template, unknown placeholder, grammatical-case variants).

## Investigation targets
**Required:**
- `artifacts/fn-9-local-coverage/uncovered.json`
- `src/Humanizer/Localisation/NumberToWords/TerminalOrdinalScaleNumberToWordsConverter.cs`
- `src/Humanizer/Localisation/NumberToWords/HarmonyOrdinalNumberToWordsConverter.cs`
- `src/Humanizer/Localisation/TimeOnlyToClockNotation/PhraseClockNotationConverter.cs`
- `src/Humanizer/Localisation/Ordinalizers/WordFormTemplateOrdinalizer.cs`
- Existing locale test folders for ja / ko / th / vi / ru / pl / he / ar

## Acceptance
- [ ] All 4 target classes reach ≥95% line and ≥90% branch in the merged multi-TFM report.
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
