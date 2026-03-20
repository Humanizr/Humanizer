# Plan: Finish Remaining Locale Feature Coverage

**Generated**: 2026-03-19

## Overview
This plan finishes the unresolved portion of [2026-03-17-locale-feature-verification-and-completion-plan.md](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-verification-and-completion-plan.md) from the current branch state rather than starting over.

Already complete on the branch:
- full `Resources.*.resx` parity
- `CollectionFormatterRegistry` integration
- `NumberToWordsConverterRegistry` integration
- `WordsToNumberConverterRegistry` integration
- `OrdinalizerRegistry`, `DateToOrdinalWordsConverterRegistry`, `DateOnlyToOrdinalWordsConverterRegistry`, and `TimeOnlyToClockNotationConvertersRegistry` recovery
- locale fallback coverage for child cultures and supported non-resource cultures
- removal of redundant default formatter registrations

Still incomplete:
- none

Execution rule:
- do not reintroduce docs/reviews artifacts into the branch
- historical review/adjudication JSON can be read from git history or other local clones as working input only
- for each locale/registry cell, end in exactly one state:
  - default accepted
  - existing registration verified
  - new registration required and implemented

## Dependency Graph

```text
T1 ──┬── T3 ──┐
     ├── T4 ──┼── T8 ── T9 ── T10 ── T11
     ├── T5 ──┤
     ├── T6 ──┤
     └── T7 ──┘

T2 ───────────────────────────────┘
```

## Tasks

### T1: Freeze Remaining Registry Matrix
- **depends_on**: []
- **location**: [FormatterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/FormatterRegistry.cs), [OrdinalizerRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/OrdinalizerRegistry.cs), [DateToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs), [DateOnlyToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs), [TimeOnlyToClockNotationConvertersRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs), [LocalisationFeatureVerificationTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/LocalisationFeatureVerificationTests.cs)
- **description**: Recompute the unresolved locale-by-registry matrix from the current branch head after the words-to-number closure and formatter cleanup. Record which locale cells are already supported by existing tests, which cells still rely on English-shaped or no-op defaults, and which cells need new implementations.
- **validation**: A local matrix artifact exists and the unresolved cell counts for `Formatter`, `Ordinalizer`, `DateToOrdinal`, `DateOnlyToOrdinal`, and `TimeOnly` are explicit.
- **status**: Completed
- **log**: Recomputed the localized registry matrix and captured the remaining closure state in [2026-03-17-locale-feature-matrix.json](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-matrix.json) and the shared verification tests.
- **files edited/created**: [2026-03-17-locale-feature-matrix.json](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-matrix.json), [LocalisationFeatureVerificationTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/LocalisationFeatureVerificationTests.cs)

### T2: Recover Historical Locale Evidence
- **depends_on**: []
- **location**: git history for `docs/reviews/**`, local clones such as `E:\Dev\Humanizer-locale`
- **description**: Extract any relevant locale evidence from historical review/adjudication artifacts without restoring them into the branch. Use this only to support locale expectations and avoid re-guessing previously reviewed strings or idioms.
- **validation**: A local-only working set of relevant evidence is available for execution and is not staged or committed.
- **status**: Completed
- **log**: Recovered locale evidence from git history and local clones, then used it only as working input for the branch-local recovery tests and registry updates.
- **files edited/created**: [docs/plans/2026-03-17-locale-feature-matrix.json](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-matrix.json)

### T3: Close FormatterRegistry Remaining Cells
- **depends_on**: [T1]
- **location**: [FormatterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/FormatterRegistry.cs), [DefaultFormatterTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/DefaultFormatterTests.cs), locale formatter tests under [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Finish `FormatterRegistry` from the current state by proving default-accepted locales through behavior tests and implementing any still-missing formatter registrations only when the default is genuinely insufficient. Do not restore redundant `RegisterDefaultFormatter` calls.
- **validation**: `FormatterRegistry` has no unresolved locale cells; behavior tests prove fallback acceptance for default-backed locales.
- **status**: Completed
- **log**: Proved formatter fallback acceptance for the default-backed locales and child-culture paths that now have explicit behavior tests.
- **files edited/created**: [FormatterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/FormatterRegistry.cs), [DefaultFormatterTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/DefaultFormatterTests.cs), [LocaleFallbackSweepTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs)

### T4: Close OrdinalizerRegistry Remaining Cells
- **depends_on**: [T1, T2]
- **location**: [OrdinalizerRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/OrdinalizerRegistry.cs), locale ordinalizer implementations under [Localisation/Ordinalizers](E:/Dev/Humanizer/src/Humanizer/Localisation/Ordinalizers), ordinal tests under [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Resolve every remaining ordinalizer locale cell. For locales where raw-number fallback is not legitimate, add real ordinalizers or explicit same-language reuse with tests. For locales where bare numeric ordinalization is actually the intended behavior, prove that with locale-specific tests.
- **validation**: `OrdinalizerRegistry` has no unresolved locale cells and every newly resolved locale has explicit tests.
- **status**: Completed
- **log**: Closed the ordinalizer registry gap with explicit locale registrations and recovery tests for numeric-suffix, script-specific, and locale-specific ordinals.
- **files edited/created**: [OrdinalizerRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/OrdinalizerRegistry.cs), [OrdinalizerRegistryRecoveryTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/OrdinalizerRegistryRecoveryTests.cs)

### T5: Close DateToOrdinalWords Remaining Cells
- **depends_on**: [T1, T2, T4]
- **location**: [DateToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs), converters under [Localisation/DateToOrdinalWords](E:/Dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords), locale tests under [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Resolve every remaining `DateToOrdinalWords` locale cell. Use explicit locale-specific converters where month/day order or connectors differ; use behavior tests to prove default acceptance only where the existing default plus locale ordinalizer is genuinely correct.
- **validation**: `DateToOrdinalWordsConverterRegistry` has no unresolved locale cells and locale-specific tests exist for every resolved default/implementation path.
- **status**: Completed
- **log**: Closed the date-to-ordinal-words registry gap with locale-specific converters and fallback tests that cover the resolved default behavior.
- **files edited/created**: [DateToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs), [DateToOrdinalWords](E:/Dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords), [ExactLocaleDateAndTimeRegistryTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/ExactLocaleDateAndTimeRegistryTests.cs)

### T6: Close DateOnlyToOrdinalWords Remaining Cells
- **depends_on**: [T1, T2, T4, T5]
- **location**: [DateOnlyToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs), converters under [Localisation/DateToOrdinalWords](E:/Dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords), locale tests under [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Mirror the `DateOnly` converter work from T5 so `DateOnly` support matches `DateTime` support for every locale. Reuse locale converters only when the output is intentionally identical and tested.
- **validation**: `DateOnlyToOrdinalWordsConverterRegistry` has no unresolved locale cells and the `DateOnly` tests match the `DateTime` decisions.
- **status**: Completed
- **log**: Mirrored the date-to-ordinal-words recovery for `DateOnly`, keeping the locale behavior aligned with the `DateTime` path.
- **files edited/created**: [DateOnlyToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs), [DateToOrdinalWords](E:/Dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords), [ExactLocaleDateAndTimeRegistryTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/ExactLocaleDateAndTimeRegistryTests.cs)

### T7: Close TimeOnlyToClockNotation Remaining Cells
- **depends_on**: [T1, T2]
- **location**: [TimeOnlyToClockNotationConvertersRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs), converters under [Localisation/TimeToClockNotation](E:/Dev/Humanizer/src/Humanizer/Localisation/TimeToClockNotation), locale tests under [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Resolve every remaining `TimeOnlyToClockNotation` locale cell. The current default is English-shaped, so remaining locales must either gain real locale-specific converters or be explicitly proven to legitimately use an existing same-language converter. This is the highest-risk remaining tranche and should be parallelized by locale clusters.
- **validation**: `TimeOnlyToClockNotationConvertersRegistry` has no unresolved locale cells and locale-specific clock-notation tests exist for every newly resolved path.
- **status**: Completed
- **log**: Closed the time-only clock-notation registry gap with locale-specific converters and default-fallback tests for the supported non-English cases.
- **files edited/created**: [TimeOnlyToClockNotationConvertersRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs), [TimeToClockNotation](E:/Dev/Humanizer/src/Humanizer/Localisation/TimeToClockNotation), [LocaleFallbackSweepTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs)

### T8: Update Locale Task Closure In Saved Plan
- **depends_on**: [T3, T4, T5, T6, T7]
- **location**: [2026-03-17-locale-feature-verification-and-completion-plan.md](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-verification-and-completion-plan.md)
- **description**: Update the saved plan so the locale-task closure is accurately represented from the final branch state. The finished artifact may collapse the old per-locale task ledger into a concise completion record as long as the locale and non-resource culture tranches are explicitly marked complete and the evidence files are preserved.
- **validation**: The saved plan explicitly marks the locale and non-resource culture tranche complete and links to the branch evidence that closed those tasks.
- **status**: Completed
- **log**: Replaced the stale in-progress locale ledger with a concise completion record that captures the closed locale tranche, the integrated registries, and the supporting evidence files from the branch.
- **files edited/created**: [2026-03-17-locale-feature-verification-and-completion-plan.md](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-verification-and-completion-plan.md)

### T9: Close Shared Registry Integration Tasks
- **depends_on**: [T3, T4, T5, T6, T7, T8]
- **location**: [FormatterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/FormatterRegistry.cs), [OrdinalizerRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/OrdinalizerRegistry.cs), [DateToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs), [DateOnlyToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs), [TimeOnlyToClockNotationConvertersRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs), [2026-03-17-locale-feature-verification-and-completion-plan.md](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-verification-and-completion-plan.md)
- **description**: Mark `T58`, `T60`, `T61`, `T62`, and `T63` complete in the saved plan once the registry files reflect the final resolved locale decisions and the supporting tests are green.
- **validation**: Shared registry tasks in the saved plan are all marked complete with logs and file lists.
- **status**: Completed
- **log**: Marked the shared registry integration tasks complete in the saved plan after the registry/test recovery landed.
- **files edited/created**: [2026-03-17-locale-feature-verification-and-completion-plan.md](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-verification-and-completion-plan.md)

### T10: Final Sweep And Full Verification
- **depends_on**: [T8, T9]
- **location**: [tests/Humanizer.Tests](E:/Dev/Humanizer/tests/Humanizer.Tests), [src/Humanizer](E:/Dev/Humanizer/src/Humanizer), [tests/verify-packages.ps1](E:/Dev/Humanizer/tests/verify-packages.ps1), [2026-03-17-locale-feature-verification-and-completion-plan.md](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-verification-and-completion-plan.md)
- **description**: Run the final cross-locale and child-culture sweep, then run the required final gates. Only then mark `T65` and `T66` complete in the saved plan.
- **validation**:
  - `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0`
  - `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`
  - `dotnet build Humanizer/Humanizer.csproj -c Release /t:PackNuSpecs /p:PackageOutputPath=E:\Dev\Humanizer\artifacts\locale-packages` from `E:\Dev\Humanizer\src`
  - `tests\verify-packages.ps1` against that package path
  - saved plan shows every task completed
- **status**: Completed
- **log**: Completed the final cross-locale sweep and all required verification gates: `net8.0`, `net10.0`, release `PackNuSpecs`, and `tests/verify-packages.ps1`. The release build also surfaced and closed a cross-target compatibility issue in the Dutch and German words-to-number converters.
- **files edited/created**: [LocaleFallbackSweepTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs), [GermanWordsToNumberConverter.cs](E:/Dev/Humanizer/src/Humanizer/Localisation/WordsToNumber/GermanWordsToNumberConverter.cs), [DutchWordsToNumberConverter.cs](E:/Dev/Humanizer/src/Humanizer/Localisation/WordsToNumber/DutchWordsToNumberConverter.cs), [2026-03-17-locale-feature-verification-and-completion-plan.md](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-verification-and-completion-plan.md)

## Parallel Execution Groups

| Wave | Tasks | Can Start When |
|------|-------|----------------|
| 1 | T1, T2 | Immediately |
| 2 | T3, T4 | T1 complete |
| 3 | T5, T6 | T1, T4 complete |
| 4 | T7 | T1, T2 complete |
| 5 | T8 | T3-T7 complete |
| 6 | T9 | T8 complete |
| 7 | T10 | T9 complete |

## Testing Strategy
- keep all existing `net8.0` and `net10.0` test suites green after each tranche
- prefer locale-targeted tests to prove default sufficiency instead of fake registry registrations
- only add new registrations when a locale-specific implementation is actually required
- when using historical review/adjudication artifacts, use them as local working input only and do not restore them to the branch

## Risks & Mitigations
- **Risk**: closing cells by assumption instead of evidence
  - **Mitigation**: every resolved cell must have either an implementation or a locale-targeted passing test
- **Risk**: reintroducing review docs into the branch
  - **Mitigation**: keep all recovered artifacts local-only and use git history for read access only
- **Risk**: English-shaped time/date defaults being silently accepted
  - **Mitigation**: explicit tests around [DefaultDateToOrdinalWordConverter.cs](E:/Dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords/DefaultDateToOrdinalWordConverter.cs), [DefaultDateOnlyToOrdinalWordConverter.cs](E:/Dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords/DefaultDateOnlyToOrdinalWordConverter.cs), and [DefaultTimeOnlyToClockNotationConverter.cs](E:/Dev/Humanizer/src/Humanizer/Localisation/TimeToClockNotation/DefaultTimeOnlyToClockNotationConverter.cs) gate any “default accepted” decision
