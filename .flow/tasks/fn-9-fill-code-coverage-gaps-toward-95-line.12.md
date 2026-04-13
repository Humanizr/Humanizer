## Description
Close tail-coverage on **ByteSize + Truncator** classes.

**Size:** M
**Files:**
- `tests/Humanizer.Tests/Bytes/CreatingTests.cs`, `tests/Humanizer.Tests/Bytes/ParsingTests.cs`, `tests/Humanizer.Tests/Bytes/ToStringTests.cs` (extend in place — canonical location under `tests/Humanizer.Tests/Bytes/`)
- `tests/Humanizer.Tests/TruncatorTests.cs` (extend — the actual existing truncator test file at the repo root level)

## Target classes
- `Humanizer.Bytes.ByteSize` (89.8%) — lives under `src/Humanizer/Bytes/`
- `Humanizer.FixedLengthTruncator` (88.8%)
- `Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator` (89.6%)
- `Humanizer.DynamicLengthAndPreserveWordsTruncator` (94.8%)
- `Humanizer.FixedNumberOfWordsTruncator` (92.3%)
- `Humanizer.FixedNumberOfCharactersTruncator` (92.5%)

## Approach
- Read `artifacts/fn-9-local-coverage/uncovered.json` (from .1) for each target; list uncovered lines + branches in the test-file header comment.
- `ByteSize`: add Theory rows for edge values (negative, zero, `long.MinValue`, `long.MaxValue`, unit-boundary crossings). Extend the three existing `Bytes/*.cs` test files in place — `ParsingTests.cs` for parse errors, `ToStringTests.cs` for format precision, `CreatingTests.cs` for factory-method edges.
- Truncators: branches for `truncationString` longer than input, empty input, whitespace-only input, exact-fit, just-over-fit. Extend `TruncatorTests.cs` (existing file at `tests/Humanizer.Tests/TruncatorTests.cs`).

## Investigation targets
**Required:**
- `artifacts/fn-9-local-coverage/uncovered.json`
- `src/Humanizer/Bytes/ByteSize.cs`
- `src/Humanizer/Truncation/*.cs` (per-truncator sources)
- `tests/Humanizer.Tests/Bytes/{CreatingTests,ParsingTests,ToStringTests}.cs`
- `tests/Humanizer.Tests/TruncatorTests.cs`

## Acceptance
- [ ] All 6 target classes reach ≥95% line and ≥90% branch in the merged multi-TFM report.
- [ ] ByteSize tests land in `tests/Humanizer.Tests/Bytes/` (extending the three existing files).
- [ ] Truncator tests extend `tests/Humanizer.Tests/TruncatorTests.cs` (no stray new truncator test file in `Truncation/` subfolder).
- [ ] Every new test asserts on specific expected output.
- [ ] No `[ExcludeFromCodeCoverage]` introduced.
- [ ] No deferral or hold-list language.

## Done summary
Added tail-branch coverage tests for ByteSize (Equals, GetHashCode, CompareTo, comparison operators, LargestWholeNumber tiers for all unit sizes including negatives, parse edge cases for fractional/out-of-range bits, ToString format variations) and 5 Truncator classes (FixedLength null/long truncation string, FixedNumberOfWords single-word fallthrough, FixedNumberOfCharacters non-alpha separators, DynamicLengthAndPreserveWords effective-length-zero/no-space/whitespace-prefix, DynamicNumberOfCharactersAndPreserveWords delimiter-longer-than-total/no-complete-word/whitespace-only/alpha-zero branches).
## Evidence
- Commits: f8ea835a, 65002f8f, 38422db6
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
- PRs: