## Description
Close tail-coverage on **core extensions** subset of the 88-95% classes.

**Size:** M
**Files:**
- `tests/Humanizer.Tests/CasingTests.cs` (extend)
- `tests/Humanizer.Tests/OrdinalizeTests.cs` (extend)
- `tests/Humanizer.Tests/ToQuantityTests.cs` (extend)
- `tests/Humanizer.Tests/DefaultFormatterTests.cs` (new or extend the nearest existing formatter tests)
- `tests/Humanizer.Tests/AnalyzersUtilityCoverageTests.cs` (new) — for the `Humanizer.Analyzers` utility class in the main library (NOT the analyzer project)

## Target classes
- `Humanizer.CasingExtensions` (87.5%)
- `Humanizer.DefaultFormatter` (87.9%)
- `Humanizer.OrdinalizeExtensions` (88.2%)
- `Humanizer.Analyzers` (88.5%) — the main-library utility class, NOT the analyzer project (owned by `.7` + `.16`)
- `Humanizer.ToQuantityExtensions` (93.5%)

## Approach
- For each target, read the corresponding entry in `artifacts/fn-9-local-coverage/uncovered.json` (from .1). List uncovered lines + branches as a header comment in the test file. Author the smallest `[Fact]` / `[Theory]` input per branch.
- Because fn-9 depends on fn-8, there is no locale hold list. If an uncovered branch is reachable only through cs/pl/ru/ar/he/hi, write the test against that locale.
- No new test plumbing needed.

## Investigation targets
**Required:**
- `artifacts/fn-9-local-coverage/uncovered.json`
- Each target `.cs` under `src/Humanizer/`
- `tests/Humanizer.Tests/CasingTests.cs`, `OrdinalizeTests.cs`, `ToQuantityTests.cs` — existing test shapes

## Acceptance
- [ ] All 5 target classes reach ≥95% line and ≥90% branch in the merged multi-TFM report.
- [ ] No `[ExcludeFromCodeCoverage]` introduced outside .1's FluentDate T4 changes.
- [ ] No deferral or hold-list language in acceptance.
- [ ] Every new test asserts on specific expected output.

## Done summary
Added tail-branch coverage tests for 4 core extension classes (CasingExtensions, OrdinalizeExtensions, ToQuantityExtensions, DefaultFormatter). Key additions: invalid enum ArgumentOutOfRangeException test, WordForm overloads and null-culture fallback, NormalizeOrdinalNumberString format-character stripping via cloned culture with bidi marks, long/double/formatProvider overload tests, and comprehensive DefaultFormatter branch coverage including RejectingFormatter subclass for error paths and exact-two/count-placement branches via Maltese/Arabic/Romanian locales. Humanizer.Analyzers was dropped — coverage-report namespace artifact, not a testable class.
## Evidence
- Commits: 2295c1a3, c6adf8e2, 32d4a379, 937c6bae, edb2948d
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (40920 passed)
- PRs: