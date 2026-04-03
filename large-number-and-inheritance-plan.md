# Plan: Large Number And Inheritance Execution

**Generated**: 2026-04-03

## Execution Status

Completed on 2026-04-03.

Final verification:
- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`
- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0`
- `dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0`
- `dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/plan-validation`
- `pwsh ./tests/verify-packages.ps1 -PackageVersion 3.5.0-dev.9.g5fcbcaf9df -PackagesDirectory ./artifacts/plan-validation`

All commands passed.

## Overview

This plan executes the work tracked in `artifacts/2026-04-03-large-number-and-inheritance-tracking.md`.
The goal is to make supported locale number surfaces behave naturally at their language-appropriate high-range ceilings, keep `ToWords` and `ToNumber` aligned, and make supported locale variants resolve through localized parent inheritance rather than cross-language English fallback.

One structural constraint changes the execution order: the current public parser surface is still `int`-based, while the target range is "as high as is natural within `long`". The parser contract therefore has to move to `long` before locale-family work can credibly claim parity with `ToWords(long)`.

## Prerequisites

- .NET SDKs required by the repo (`net10.0`, `net8.0`)
- Locale YAML authoring under `src/Humanizer/Locales`
- Source-generator pipeline under `src/Humanizer.SourceGenerators`
- xUnit shared theory patterns with `Theory`, `InlineData`, and `MemberData` / `TheoryData`

## Dependency Graph

```text
T1 ── T2 ── T3 ──┬── T4 ── T5 ──┬── T6 ──┐
                 │              ├── T7 ──┤
                 │              ├── T8 ──┤
                 │              ├── T9 ──┤
                 │              └── T10 ─┤
                 └───────────────────────┘
                                          ├── T11 ── T12 ── T13 ── T14
```

## Tasks

### T1: Build The Support Matrix And Inheritance Inventory
- **depends_on**: []
- **location**: `src/Humanizer/Locales/*.yml`, `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`, `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`, `artifacts/2026-04-03-large-number-and-inheritance-tracking.md`
- **description**: Convert the tracking document into an executable support matrix. Record, per locale, the intended high-range ceiling, numbering tradition, parent-locale chain, current authoring ceiling, parser status, and support state. The support state must distinguish at least `Supported`, `Inherited`, and `Unsupported` so shared tests can model localized inheritance separately from unsupported fallback.
- **validation**: There is one authoritative locale matrix covering all tracked locales, and every variant locale has an explicit parent-chain decision that matches the current `variantOf` model or an intentional override.
- **status**: Completed
- **log**: Built the executable locale support/inheritance matrix in runtime tests and the tracking artifact, distinguishing supported, inherited, and unsupported number-surface behavior.
- **files edited/created**: `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`, `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`, `artifacts/2026-04-03-large-number-and-inheritance-tracking.md`

### T2: Move The Parser Contract From `int` To `long`
- **depends_on**: [T1]
- **location**: `src/Humanizer/WordsToNumberExtension.cs`, `src/Humanizer/Localisation/WordsToNumber/IWordsToNumberConverter.cs`, `src/Humanizer/Localisation/WordsToNumber/*.cs`, `src/Humanizer/Configuration/*`, `tests/Humanizer.Tests/ApiApprover/PublicApiApprovalTest.Approve_Public_Api.DotNet10_0.verified.txt`, `tests/Humanizer.Tests/ApiApprover/PublicApiApprovalTest.Approve_Public_Api.DotNet8_0.verified.txt`, `tests/Humanizer.Tests/ApiApprover/PublicApiApprovalTest.Approve_Public_Api.Net4_8.verified.txt`
- **description**: Expand `ToNumber` and `TryToNumber` from `int` to `long` end to end so the public parser contract can match the target high-range support. This task owns the public API change, converter interface change, compatibility review, and API approval updates. It also owns the parser overflow contract for values outside the supported `long` range.
- **validation**: The public parser APIs and converter interfaces are `long`-based, API approvals are updated, and parser overflow/error behavior is explicit and covered by tests.
- **status**: Completed
- **log**: Moved the public parser contract and the converter interface chain from `int` to `long`, updated overflow behavior, and refreshed API approval baselines.
- **files edited/created**: `src/Humanizer/WordsToNumberExtension.cs`, `src/Humanizer/Localisation/WordsToNumber/IWordsToNumberConverter.cs`, `src/Humanizer/Localisation/WordsToNumber/*.cs`, `tests/Humanizer.Tests/ApiApprover/PublicApiApprovalTest.Approve_Public_Api.*.verified.txt`

### T3: Lock The Core Number-Surface Contract In Runtime, Generator, And Generator Tests
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs`, `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`, `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`, `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`, `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/NumberToWordsProfileCatalogInput.cs`, `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/WordsToNumberProfileCatalogInput.cs`, `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/*.cs`, `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`, `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`
- **description**: Formalize the repo-wide support contract after the parser API change. This task owns the central semantics: supported locales are expected to align `number.words` and `number.parse`, inherited locales resolve through localized parents, and supported non-English locales do not normalize to English fallback. Source-generator tests must be updated here as an early gate rather than left until the end.
- **validation**: Runtime support policy, generator inputs, and source-generator tests agree on the same contract before any locale-family expansion begins.
- **status**: Completed
- **log**: Locked the supported/inherited/no-cross-language-fallback contract in the runtime locale sweep, generator inputs, canonical schema handling, and source-generator coverage.
- **files edited/created**: `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs`, `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`, `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`, `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/*.cs`, `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/*.cs`, `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`, `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`

### T4: Add Shared High-Range And Boundary Test Infrastructure
- **depends_on**: [T3]
- **location**: `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`, `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`, `tests/Humanizer.Tests/Localisation/LocaleNumberMagnitudeTheoryData.cs`, `tests/Humanizer.Tests/Localisation/LocaleNumberOverloadTheoryData.cs`, new helper files under `tests/Humanizer.Tests/Localisation` if needed
- **description**: Build the shared xUnit theory infrastructure after the support contract is settled. This task owns family-wide theory data for short-scale, long-scale, East Asian grouped, Indian grouping, and compositional systems. It also owns first-class boundary/error rows: `long.MinValue`, `long.MaxValue`, `maximumValue + 1`, parser overflow, null, empty, whitespace, and ambiguous or unrecognized token behavior.
- **validation**: Shared theory data covers writer, parser, round-trip, inheritance, and hard boundary/error cases without hard-coding stale fallback assumptions.
- **status**: Completed
- **log**: Added shared magnitude, overload, round-trip, and registry-sweep coverage for high-range writer/parser behavior, including long-boundary cases and localized inheritance expectations.
- **files edited/created**: `tests/Humanizer.Tests/Localisation/LocaleNumberMagnitudeTheoryData.cs`, `tests/Humanizer.Tests/Localisation/LocaleNumberOverloadTheoryData.cs`, `tests/Humanizer.Tests/Localisation/LocaleNumberTheoryData.cs`, `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`, `tests/Humanizer.Tests/WordsToNumberLongTests.cs`

### T5: Refactor Shared Number And Parser Kernels Before Locale Waves
- **depends_on**: [T4]
- **location**: `src/Humanizer/Localisation/NumberToWords/*.cs`, `src/Humanizer/Localisation/WordsToNumber/*.cs`, `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/*.cs`, targeted shared tests under `tests/Humanizer.Tests/Localisation` and `tests/Humanizer.SourceGenerators.Tests/SourceGenerators`
- **description**: Make all cross-locale shared changes before the locale-family waves start. This task owns shared kernel refactors, shared parser/tokenization/overflow behavior, and any generator profile changes needed for higher ceilings. The locale-family tasks that follow should be able to focus primarily on locale-authored data, localized expectations, and family-specific spot behavior rather than competing over shared abstractions.
- **validation**: Shared kernels and generator profile flows can support the tracked high-range families, and the locale-family waves can proceed in parallel with minimal overlap on shared engine files.
- **status**: Completed
- **log**: Refactored shared number-to-words and words-to-number kernels for `long`-range support, fixed remainder truncation in high-scale engines, and aligned generator helpers with 64-bit scale values.
- **files edited/created**: `src/Humanizer/Localisation/NumberToWords/*.cs`, `src/Humanizer/Localisation/WordsToNumber/*.cs`, `src/Humanizer.SourceGenerators/Common/GenerationHelpers.cs`, `src/Humanizer.SourceGenerators/Generators/TokenMapWordsToNumberInput.cs`, `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/WordsToNumberEngineContractFactory.cs`

### T6: Implement English-Family And Indian-Grouping High-Range Support
- **depends_on**: [T5]
- **location**: `src/Humanizer/Locales/en.yml`, `src/Humanizer/Locales/en-US.yml`, `src/Humanizer/Locales/en-GB.yml`, `src/Humanizer/Locales/en-IN.yml`, `src/Humanizer/Locales/bn.yml`, `src/Humanizer/Locales/ta.yml`, family-specific tests under `tests/Humanizer.Tests/Localisation/en*`, `tests/Humanizer.Tests/Localisation/bn`, `tests/Humanizer.Tests/Localisation/ta`
- **description**: Complete the English-family and Indian-grouping locale work. Preserve British conjunction behavior for `en-GB`, default/American behavior for `en` and `en-US`, and Indian grouping for `en-IN`, `bn`, and `ta`. Writer and parser support must land together for each supported locale, with natural high-range terms and mixed-remainder phrases. For Tamil, use the current Tamil large-number ladder rather than English `shankh` borrowing, including `சங்கம்` at `10^15` and `அர்த்தம்` at `10^17`.
- **validation**: English-family and Indian-grouping locales pass shared writer, parser, round-trip, overload, and inheritance tests through their intended ceilings.
- **status**: Completed
- **log**: Implemented high-range English-family and Indian-grouping behavior, including `en-GB` conjunction differences, `en-IN`/`bn`/`ta` parser parity, and Tamil high-range round-trip coverage.
- **files edited/created**: `src/Humanizer/Locales/en.yml`, `src/Humanizer/Locales/en-IN.yml`, `src/Humanizer/Locales/bn.yml`, `src/Humanizer/Locales/ta.yml`, `tests/Humanizer.Tests/WordsToNumberTests.cs`, `tests/Humanizer.Tests/Localisation/en-IN/*.cs`, `tests/Humanizer.Tests/Localisation/bn-BD/*.cs`, `tests/Humanizer.Tests/Localisation/ta/*.cs`

### T7: Implement Short-Scale Western Locale Families
- **depends_on**: [T5]
- **location**: `src/Humanizer/Locales/pt-BR.yml`, `src/Humanizer/Locales/ru.yml`, `src/Humanizer/Locales/uk.yml`, `src/Humanizer/Locales/bg.yml`, `src/Humanizer/Locales/lv.yml`, `src/Humanizer/Locales/lt.yml`, `src/Humanizer/Locales/tr.yml`, `src/Humanizer/Locales/az.yml`, `src/Humanizer/Locales/hy.yml`, `src/Humanizer/Locales/he.yml`, `src/Humanizer/Locales/ar.yml`, `src/Humanizer/Locales/fa.yml`, `src/Humanizer/Locales/ku.yml`, `src/Humanizer/Locales/id.yml`, `src/Humanizer/Locales/ms.yml`, `src/Humanizer/Locales/fil.yml`, matching locale tests under `tests/Humanizer.Tests/Localisation`
- **description**: Extend short-scale locales to their natural `10^18` terms, including pluralization, inflection, construct-state, and authored maximum-value changes where appropriate. Keep the task focused on locale-owned data and locale-specific expectations unless a family-specific gap remains after T5.
- **validation**: Short-scale target locales pass high-range writer, parser, mixed-remainder, round-trip, and negative-value tests where applicable.
- **status**: Completed
- **log**: Extended the short-scale western locale wave through natural `10^18` terms, with locale-authored scales, parser vocabulary, and high-range round-trip tests.
- **files edited/created**: `src/Humanizer/Locales/{af,ar,az,bg,fil,he,hy,id,ku,lt,lv,ms,ru,tr,uk}.yml`, `tests/Humanizer.Tests/Localisation/*/WordsToNumberHighRangeTests.cs`

### T8: Implement Long-Scale European Locale Families
- **depends_on**: [T5]
- **location**: `src/Humanizer/Locales/de*.yml`, `src/Humanizer/Locales/fr*.yml`, `src/Humanizer/Locales/es.yml`, `src/Humanizer/Locales/ca.yml`, `src/Humanizer/Locales/it.yml`, `src/Humanizer/Locales/pt.yml`, `src/Humanizer/Locales/ro.yml`, `src/Humanizer/Locales/cs.yml`, `src/Humanizer/Locales/sk.yml`, `src/Humanizer/Locales/pl.yml`, `src/Humanizer/Locales/nl.yml`, `src/Humanizer/Locales/lb.yml`, `src/Humanizer/Locales/da.yml`, `src/Humanizer/Locales/nb.yml`, `src/Humanizer/Locales/nn.yml`, `src/Humanizer/Locales/sv.yml`, `src/Humanizer/Locales/is.yml`, `src/Humanizer/Locales/hu.yml`, `src/Humanizer/Locales/fi.yml`, `src/Humanizer/Locales/af.yml`, `src/Humanizer/Locales/hr.yml`, `src/Humanizer/Locales/sr.yml`, `src/Humanizer/Locales/sr-Latn.yml`, `src/Humanizer/Locales/sl.yml`, matching locale tests under `tests/Humanizer.Tests/Localisation`
- **description**: Extend long-scale locales through their natural `10^18` terms. Raise authored caps that still stop at `int`, preserve long-scale naming and compound rules, and keep locale variants parent-driven unless they own a real difference. Keep changes locale-authored wherever the shared kernel work from T5 is already sufficient.
- **validation**: Long-scale target locales pass high-range writer, parser, round-trip, overload, and inheritance tests through their intended ceilings.
- **status**: Completed
- **log**: Extended the long-scale European family through natural `10^18` terms, raised authored caps, and aligned parser/writer expectations and overload tests.
- **files edited/created**: `src/Humanizer/Locales/{af,da,fi,fr,hr,hu,lb,nl,sl,sr,sr-Latn}.yml`, related localization tests under `tests/Humanizer.Tests/Localisation`

### T9: Implement East Asian Grouped And Other Compositional High-Range Systems
- **depends_on**: [T5]
- **location**: `src/Humanizer/Locales/zh-Hans.yml`, `src/Humanizer/Locales/zh-CN.yml`, `src/Humanizer/Locales/zh-Hant.yml`, `src/Humanizer/Locales/ja.yml`, `src/Humanizer/Locales/ko.yml`, `src/Humanizer/Locales/th.yml`, `src/Humanizer/Locales/vi.yml`, matching locale tests under `tests/Humanizer.Tests/Localisation`
- **description**: Complete the grouped/compositional families. Chinese and Japanese families should reach natural `10^16` grouping, Korean should reach `경`, and Thai/Vietnamese should use natural compositional large-number phrasing rather than forced western scale words. Parser support must match the supported writer behavior and preserve native orthography.
- **validation**: East Asian/compositional locales pass writer, parser, round-trip, mixed-remainder, and inheritance tests through their intended ceilings.
- **status**: Completed
- **log**: Landed grouped/compositional high-range support for Chinese, Japanese, Korean, Thai, and Vietnamese, with natural writer outputs and parser coverage at the intended ceilings.
- **files edited/created**: `src/Humanizer/Locales/{ja,ko,th,vi,zh-Hans,zh-Hant}.yml`, `tests/Humanizer.Tests/Localisation/{ja,ko-KR,th-TH,vi,zh-CN}/*.cs`

### T10: Implement The Resolved Greek, Maltese, And Uzbek High-Range Targets
- **depends_on**: [T5]
- **location**: `src/Humanizer/Locales/el.yml`, `src/Humanizer/Locales/mt.yml`, `src/Humanizer/Locales/uz-Cyrl-UZ.yml`, `src/Humanizer/Locales/uz-Latn-UZ.yml`, matching locale tests under `tests/Humanizer.Tests/Localisation`, `artifacts/2026-04-03-large-number-and-inheritance-tracking.md`
- **description**: Implement the previously unresolved locales now that the target terms are researched. Greek should use the calqued ladder through `πεντάκις εκατομμύριο`, Maltese should extend through `kwintiljun`, and Uzbek should extend through `квинтиллион` / `kvintillion` in the appropriate script. Raise authored caps, add parser vocabulary, and add family-specific tests without reopening the target-selection question during implementation.
- **validation**: Greek, Maltese, and both Uzbek locales pass writer, parser, round-trip, mixed-remainder, and boundary tests through their resolved target ranges.
- **status**: Completed
- **log**: Implemented the resolved Greek, Maltese, and Uzbek targets, including high-range scale rows, parse vocabulary, and dedicated high-range round-trip tests.
- **files edited/created**: `src/Humanizer/Locales/{el,mt,uz-Cyrl-UZ,uz-Latn-UZ}.yml`, `tests/Humanizer.Tests/Localisation/{el,mt,uz-Cyrl-UZ,uz-Latn-UZ}/WordsToNumberHighRangeTests.cs`

### T11: Run The Pre-Documentation Implementation Gate
- **depends_on**: [T6, T7, T8, T9, T10]
- **location**: `tests/Humanizer.Tests`, `tests/Humanizer.SourceGenerators.Tests`, `src/Humanizer/Humanizer.csproj`
- **description**: Verify the implementation before touching the main documentation set. This gate should flush out runtime, generator, and contract regressions while the work is still in code/test mode. If this gate fails, fix code/tests first and do not proceed to T12.
- **validation**: Runtime tests for `net10.0` and `net8.0` pass, source-generator tests pass, and there are no unresolved implementation failures blocking documentation.
- **status**: Completed
- **log**: Ran the pre-documentation implementation gate and fixed the remaining Finnish remainder truncation plus Tamil and `en-IN` parsing gaps before moving to docs.
- **files edited/created**: `src/Humanizer/Localisation/NumberToWords/AgglutinativeOrdinalScaleNumberToWordsConverter.cs`, `src/Humanizer/Locales/en-IN.yml`, `src/Humanizer/Locales/ta.yml`, `tests/Humanizer.Tests/Localisation/*`

### T12: Update Documentation, XML Docs, And Maintainer Comments
- **depends_on**: [T11]
- **location**: `readme.md`, `release_notes.md`, `docs/index.md`, `docs/installation.md`, `docs/quick-start.md`, `docs/localization.md`, `docs/adding-a-locale.md`, `docs/locale-yaml-how-to.md`, `docs/locale-yaml-reference.md`, `.github/CONTRIBUTING.md`, `.github/copilot-instructions.md`, `docs/migration-v3.md`, `docs/v3-namespace-migration.md`, `src/Humanizer.Analyzers/README.md`, `src/Humanizer/NumberToWordsExtension.cs`, `src/Humanizer/WordsToNumberExtension.cs`, the shared number/parser kernels, and the generator comment targets captured in the tracking doc
- **description**: Update the user-facing docs, contributor docs, XML docs, engine comments, and generator comments after behavior is stable. Remove stale references to code-only locale work, stale resource-file guidance, and stale package-split guidance. Document the current canonical schema, inheritance model, single-package story, and high-range number behavior.
- **validation**: The file inventory in the tracking doc is satisfied and the examples match the actual shipped behavior.
- **status**: Completed
- **log**: Updated the main user docs, localization/contributor docs, XML docs, and generator/runtime maintainer comments to reflect the canonical schema, inheritance model, single-package story, and high-range number behavior.
- **files edited/created**: `readme.md`, `release_notes.md`, `docs/*.md`, `.github/CONTRIBUTING.md`, `.github/copilot-instructions.md`, `src/Humanizer.Analyzers/README.md`, `src/Humanizer/NumberToWordsExtension.cs`, `src/Humanizer/WordsToNumberExtension.cs`, `src/Humanizer.SourceGenerators/Common/*.cs`, `src/Humanizer.SourceGenerators/Generators/*.cs`

### T13: Run The Final Verification And Packaging Gate
- **depends_on**: [T12]
- **location**: `tests/Humanizer.Tests`, `tests/Humanizer.SourceGenerators.Tests`, `tests/verify-packages.ps1`, `src/Humanizer/Humanizer.csproj`
- **description**: Run the final end-to-end validation after documentation is updated. This includes runtime tests, source-generator tests, `dotnet pack`, and package verification. If verification changes behavior or examples, reopen T12 before completing this task.
- **validation**: `dotnet test` passes for the runtime and source-generator suites, `dotnet pack` succeeds, `tests/verify-packages.ps1` succeeds, and docs/examples still match the verified behavior.
- **status**: Completed
- **log**: Re-ran runtime and source-generator suites after the documentation pass, packed the main package, and verified analyzer/package behavior against the produced nupkg.
- **files edited/created**: `artifacts/plan-validation/Humanizer.3.5.0-dev.9.g5fcbcaf9df.nupkg`

### T14: Reconcile The Tracking Doc And Close Remaining Gaps
- **depends_on**: [T13]
- **location**: `artifacts/2026-04-03-large-number-and-inheritance-tracking.md`, any final touched files surfaced by T13
- **description**: Update the tracking doc with completed work and any explicit follow-ups that remain after the final verification gate. This closes the loop so the execution record matches the actual codebase and test state.
- **validation**: The tracking doc accurately reflects the implemented state and any remaining follow-up work is explicitly scoped.
- **status**: Completed
- **log**: Reconciled the execution plan and tracking artifact so the recorded status matches the verified codebase and package output.
- **files edited/created**: `large-number-and-inheritance-plan.md`, `artifacts/2026-04-03-large-number-and-inheritance-tracking.md`

## Parallel Execution Groups

| Wave | Tasks | Can Start When |
|------|-------|----------------|
| 1 | T1 | Immediately |
| 2 | T2 | T1 complete |
| 3 | T3 | T1 and T2 complete |
| 4 | T4 | T3 complete |
| 5 | T5 | T4 complete |
| 6 | T6, T7, T8, T9, T10 | T5 complete |
| 7 | T11 | T6, T7, T8, T9, and T10 complete |
| 8 | T12 | T11 complete |
| 9 | T13 | T12 complete |
| 10 | T14 | T13 complete |

## Testing Strategy

- Use shared xUnit `Theory` plus `MemberData` / `TheoryData` for family-wide locale matrices.
- Treat parser and writer support as one contract for supported locales.
- Add first-class boundary coverage for:
  - `long.MinValue`
  - `long.MaxValue`
  - locale `maximumValue`
  - `maximumValue + 1`
  - parser overflow
  - null, empty, and whitespace input
  - ambiguous or unrecognized tokens
- Validate writer behavior with exact powers and mixed remainders, not just one-scale smoke tests.
- Validate parser behavior with naturally written locale phrases, not English placeholders.
- Add inheritance tests that distinguish:
  - localized parent fallback
  - locale-owned overrides
  - unsupported-surface fallback
- Keep family-specific spot tests where grammar is too irregular for a generic theory to express cleanly.

## Risks & Mitigations

- **Risk**: The parser API expansion is a public contract change and may ripple through multiple converter implementations.
  - **Mitigation**: Isolate it in T2, update API approvals there, and do not begin locale-family work until it is stable.
- **Risk**: Shared number/parser kernels become contention points across locale-family tasks.
  - **Mitigation**: Land all shared kernel work in T5, then keep family tasks primarily locale-authored and test-focused.
- **Risk**: Existing tests still encode stale fallback or ceiling assumptions.
  - **Mitigation**: Move the support policy and boundary model into shared theory data in T3-T4 before locale expansion starts.
- **Risk**: Documentation trails the implementation.
  - **Mitigation**: Require the pre-doc implementation gate in T11 and the final docs/packaging gate in T13.
