## Description
Close gaps in `WordsToNumberExtension` Try overloads, `TokenMapWordsToNumberOrdinalMapBuilder` (0%), and `SuffixScaleWordsToNumberConverter` (52%).

**Size:** M
**Files:**
- `tests/Humanizer.Tests/WordsToNumberTryTests.cs` (new)
- `tests/Humanizer.Tests/Localisation/TokenMapWordsToNumberOrdinalMapBuilderTests.cs` (new)
- `tests/Humanizer.Tests/Localisation/tr/SuffixScaleWordsToNumberConverterTests.cs` (new or extend existing tr test file)

## Approach
- **WordsToNumberExtension Try overloads.** Actual signatures from `src/Humanizer/WordsToNumberExtension.cs`:
  - `public static bool TryToNumber(this string words, out long parsedNumber, CultureInfo culture)` (`:54`)
  - `public static bool TryToNumber(this string words, out long parsedNumber, CultureInfo culture, out string? unrecognizedWord)` (`:79`)
  Arg order: `unrecognizedWord` is LAST, after `culture`. Cases: (a) valid input → `true` with numeric result; (b) recognized-grammar-but-unknown-word input → `false` with `unrecognizedWord` set. Do NOT add null/empty "returns false" expectations — behavior is underlying-converter-specific.
- **TokenMapWordsToNumberOrdinalMapBuilder.** Two `Build` overloads at `src/Humanizer/Localisation/WordsToNumber/TokenMapWordsToNumberOrdinalMapBuilder.cs:38` and `:54`; three `TokenMapOrdinalGenderVariant` switch arms at `:65` (`None`), `:68` (`MasculineAndFeminine`), `:72` (`All`). Required matrix: one test per (variant × overload) pair = 6 tests minimum (or one parametric Theory with 6 rows). Assert the returned `FrozenDictionary<string, long>` contains the expected ordinal tokens.
- **SuffixScaleWordsToNumberConverter.** Consult `artifacts/fn-9-local-coverage/uncovered.json` (from .1) for exact uncovered line ranges. Author tr inputs for: empty-input throw (`:32-35`), negative-prefix loop (`:46-56`), bare-scale lookahead (`:100-112`), tens/teens suffix (`:191-220`). Assert exact numeric output.

## Investigation targets
**Required:**
- `src/Humanizer/WordsToNumberExtension.cs:1-120`
- `src/Humanizer/Localisation/WordsToNumber/TokenMapWordsToNumberOrdinalMapBuilder.cs:1-90`
- `src/Humanizer/Localisation/WordsToNumber/SuffixScaleWordsToNumberConverter.cs:1-230`
- `artifacts/fn-9-local-coverage/uncovered.json`
- `tests/Humanizer.Tests/WordsToNumberTests.cs`, `WordsToNumberCompatibilityTests.cs`, `WordsToNumberLongTests.cs`

## Acceptance
- [ ] `WordsToNumberExtension` reaches ≥95% line and ≥90% branch; tests match actual signatures.
- [ ] `TokenMapWordsToNumberOrdinalMapBuilder` reaches ≥95% line and ≥85% branch; every (variant × overload) pair covered.
- [ ] `SuffixScaleWordsToNumberConverter` reaches ≥90% line and ≥85% branch in the merged multi-TFM report.
- [ ] Tests assert exact return values and `out` parameters (no "not null" / "not throws" only).
- [ ] No fabricated null/empty contract expectations.

## Done summary
_To be filled on completion._

## Evidence
- Commits:
- Tests:
- PRs:
