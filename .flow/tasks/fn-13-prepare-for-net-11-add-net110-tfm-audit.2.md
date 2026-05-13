# fn-13-prepare-for-net-11-add-net110-tfm-audit.2 Run full suite on net11.0 across Linux/macOS/Windows and triage failures

## Description
Run the full `tests/Humanizer.Tests` suite against net11.0 across Linux, macOS, and Windows once fn-13.1 lands. Capture baseline failures, triage each, and assign follow-up work (to fn-13.3/fn-13.4/fn-13.5 or new tasks) — do **not** paper over failures.

**Size:** M
**Files:** none expected. Output is a triage table committed to this task's spec + follow-up tasks filed against siblings.

## Approach

- Run `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj -f net11.0` on all three runners.
- Group failures by root cause: ICU data drift, analyzer/warning, parser behavior change, unrelated flake.
- Route each group to the appropriate sibling task (fn-13.3 for ICU byte-parity, fn-13.4 for analyzer/warning, fn-13.5 for breaking-change sweep).
- Analyzer tests (`tests/Humanizer.Analyzers.Tests`) and source-generator tests (`tests/Humanizer.SourceGenerators.Tests`) also need the net11 run.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Humanizer.Tests.csproj` — TFM list
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs` — cross-platform conditionals
- `tests/Humanizer.Tests/Localisation/LocaleFormatterExactTheoryData.cs:10-23` — NLS vs ICU conditionals

## Key context

- fn-3 byte-parity locales (bn, fa, he, ku, zu-ZA, ta, ar, fr-CH) are the most likely regression source.
- Do not touch test assertions here — this task only *triages*. Fixes live in fn-13.3+.
## Acceptance
- [ ] Full suite ran on net11.0 for Linux, macOS, Windows (logs attached to task)
- [ ] All failures triaged and grouped; each group filed under fn-13.3/fn-13.4/fn-13.5 or a new task
- [ ] No silent acceptance of failures — zero skips added to paper over breakage
- [ ] Analyzer and source-generator test projects included in the run
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
