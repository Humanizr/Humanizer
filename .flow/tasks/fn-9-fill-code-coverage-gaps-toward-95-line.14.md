## Description
Close tail-coverage on **words-to-number converters** + `TimeSpanHumanizeExtensions` (6 classes).

**Size:** M
**Files:**
- `tests/Humanizer.Tests/ConverterTail/WordsToNumberTailCoverageTests.cs` (new) — OR extend existing per-locale words-to-number test files
- `tests/Humanizer.Tests/TimeSpanHumanizeTests.cs` (extend) — OR `tests/Humanizer.Tests/ConverterTail/TimeSpanHumanizeTailCoverageTests.cs`

## Target classes
- `Humanizer.VigesimalCompoundWordsToNumberConverter` (90.7%)
- `Humanizer.CompoundScaleWordsToNumberConverter` (90.8%)
- `Humanizer.GreedyCompoundWordsToNumberConverter` (91.1%)
- `Humanizer.LinkingAffixWordsToNumberConverter` (91.9%)
- `Humanizer.ContractedScaleWordsToNumberConverter` (92.1%)
- `Humanizer.TimeSpanHumanizeExtensions` (95.0%)

## Approach
- Read `artifacts/fn-9-baseline/uncovered.json` (from .1).
- Words-to-number converters are culture-specific — write tests under the relevant per-locale folder. Vigesimal: fr etc.; greedy compound: en, de; linking-affix: ja, zh; contracted scale: eu.
- `TimeSpanHumanizeExtensions`: likely missing branches for `TimeSpan.Zero`, negative spans, `MaxValue`, very-long-duration × precision combinations.

## Investigation targets
**Required:**
- `artifacts/fn-9-baseline/uncovered.json`
- Each target `.cs` under `src/Humanizer/Localisation/WordsToNumber/` + `src/Humanizer/TimeSpanHumanizeExtensions.cs`
- Existing per-locale `WordsToNumber` tests and `TimeSpanHumanizeTests.cs`

## Acceptance
- [ ] All 6 target classes reach ≥95% line and ≥90% branch in the merged multi-TFM report.
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
