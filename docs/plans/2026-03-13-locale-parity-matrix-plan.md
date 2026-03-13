# Plan: Locale Surface Parity Completion

**Generated**: 2026-03-13

## Overview
The existing `codex/locale-translation-completion` branch only completed a narrow subset of the locale backlog. This replacement plan decomposes the remaining work into machine-checkable, per-locale tasks instead of broad regional batches.

The execution model is:
1. Rebuild the authoritative locale/surface matrix from repo facts.
2. Lock shared validation and parity guardrails.
3. Execute one locale per task, with the missing surface families called out explicitly.
4. Run full verification only after every locale task is complete.

This plan is intentionally stricter than the previous one. A locale task is only complete when all matrix-approved missing keys for its listed surface families are implemented and backed by locale-specific tests.

Current branch baseline to preserve:
- Commit `51160280` already contains partial work for `af`, `da`, `de`, `es`, `hu`, `mt`, `nb`, `nl`, `pt`, `pt-BR`, `sr`, `sr-Latn`, `sv`, and shared fixes in collection formatting and heading parsing.
- [2026-03-12-locale-translation-completion-plan.md](E:/Dev/Humanizer-locale/docs/plans/2026-03-12-locale-translation-completion-plan.md) has been corrected to reflect partial completion only.

## Prerequisites
- .NET 10 SDK from [global.json](E:/Dev/Humanizer-locale/global.json)
- xUnit v3 via [Humanizer.Tests.csproj](E:/Dev/Humanizer-locale/tests/Humanizer.Tests/Humanizer.Tests.csproj)
- Existing localization infrastructure in [CollectionFormatterRegistry.cs](E:/Dev/Humanizer-locale/src/Humanizer/Configuration/CollectionFormatterRegistry.cs), [HeadingExtensions.cs](E:/Dev/Humanizer-locale/src/Humanizer/HeadingExtensions.cs), and `src/Humanizer/Localisation/**/*`
- Preserve all valid fixes already present on `codex/locale-translation-completion`

## Dependency Graph

```text
T1 --> T2 --> T54
        |
        +--> T3  -+
        +--> T4  -+
        +--> T5  -+
        +--> ... -+--> T54
        +--> T53 -+
```

`T1` and `T2` establish the authoritative matrix and shared validation rules. Every locale task depends on them. `T54` is the final verification gate.

## Tasks

### T1: Rebuild the authoritative locale/surface matrix
- **depends_on**: []
- **location**: `src/Humanizer/Properties/Resources*.resx`, `src/Humanizer/Configuration/*.cs`, `src/Humanizer/Localisation/**/*.cs`, `tests/Humanizer.Tests/Localisation/**/*`, `docs/plans/2026-03-13-locale-parity-matrix-plan.md`
- **description**: Produce the machine-checked locale/surface matrix that defines the remaining backlog. Use registries, direct resource consumers, existing locale tests, and neutral-resource parity to decide which surfaces are intended for each locale and which missing keys are actionable. Record the exact approved surface families per locale before implementation starts.
- **validation**: Matrix artifact or task log enumerates every locale and approved surface family, with no regional-batch ambiguity.
- **status**: Completed
- **log**: Generated the authoritative parity artifact at `docs/plans/2026-03-13-locale-parity-matrix.json` from repo facts: neutral-resource key families, locale resource files, existing locale test files, and the current branch baseline at commit `51160280`. The matrix records every locale, its missing surface families, the exact missing keys per family, and the currently associated locale test files.
- **files edited/created**: `docs/plans/2026-03-13-locale-parity-matrix-plan.md`, `docs/plans/2026-03-13-locale-parity-matrix.json`

### T2: Lock shared parity guardrails and validation helpers
- **depends_on**: [T1]
- **location**: `src/Humanizer/Configuration/*.cs`, `src/Humanizer/Localisation/**/*.cs`, `tests/Humanizer.Tests/DateHumanize.cs`, `tests/Humanizer.Tests/ResourceKeyTests.cs`, `tests/Humanizer.Tests/Localisation/**/*`
- **description**: Add or refresh shared guardrails needed to make locale completion machine-checkable, including any missing helper coverage, resource-key parity checks, registry assertions, or shared parser/formatter protections discovered by T1. Preserve the already-correct Danish and heading-parser fixes.
- **validation**: Shared tests fail before the guardrails, pass after them, and locale tasks can use them as completion gates.
- **status**: Completed
- **log**: Expanded `ResourceKeyTests` to cover stale shared generator gaps: `DateHumanize` week and millisecond resource keys, `DateHumanize_Never`, `TimeUnit` symbol keys, `DataUnit` keys, and heading resource keys. The new tests went RED immediately on missing neutral `DateHumanize` week and millisecond entries, which were then added to the neutral resource file. Shared validation is now green on both `net10.0` and `net8.0`.
- **files edited/created**: `tests/Humanizer.Tests/ResourceKeyTests.cs`, `src/Humanizer/Properties/Resources.resx`

### T3: Complete Afrikaans locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.af.resx`, `tests/Humanizer.Tests/Localisation/af/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `af`, preserving the current branch fixes and adding direct locale tests for each completed surface.
- **validation**: Targeted `af` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T4: Complete Arabic locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ar.resx`, `tests/Humanizer.Tests/Localisation/ar/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `ar`, including RTL-safe wording validation and direct locale tests for each completed surface.
- **validation**: Targeted `ar` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added explicit Arabic residual resources (`DateHumanize_Never`, `TwoDays*`, `TimeSpanHumanize_Age`), full `TimeUnit_*`, `DataUnit_*`, and the 8-point heading set exercised by the locale tests. Added direct Arabic regression coverage for residual resources, `ToAge()`, `TimeUnit.ToSymbol()`, `ByteSize.ToFullWords()`, and heading formatting/reverse lookup. This task depends on the shared Arabic data-unit formatter widening from commit `ed17baaa`.
- **files edited/created**: `src/Humanizer/Properties/Resources.ar.resx`, `tests/Humanizer.Tests/Localisation/ar/ResourcesTests.cs`, `tests/Humanizer.Tests/Localisation/ar/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/ar/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/ar/Bytes/ToFullWordsTests.cs`, `tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs`

### T5: Complete Azerbaijani locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.az.resx`, `tests/Humanizer.Tests/Localisation/az/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `az`, adding direct locale tests for each completed surface.
- **validation**: Targeted `az` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Azerbaijani `DataUnit_*`, `TimeUnit_*`, residual `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`, `Never`), `TimeSpanHumanize_Age`, and heading resources. Added direct regression coverage for residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`. This task depends on the shared trim-suffix formatter registration widened for `az`.
- **files edited/created**: `src/Humanizer/Properties/Resources.az.resx`, `tests/Humanizer.Tests/Localisation/az/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/az/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/az/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/az/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/az/Bytes/ToFullWordsTests.cs`

### T6: Complete Bulgarian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.bg.resx`, `tests/Humanizer.Tests/Localisation/bg/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `bg`, adding direct locale tests for each completed surface.
- **validation**: Targeted `bg` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T7: Complete Bengali locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.bn.resx`, `tests/Humanizer.Tests/Localisation/bn-BD/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `bn`, preserving current date/time tests and expanding them to cover each completed surface directly.
- **validation**: Targeted `bn-BD` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Bengali `DataUnit_*`, `TimeUnit_*`, residual `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`, `Never`), and `TimeSpanHumanize_Age` resources. Added direct Bengali regression coverage for residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, and `ByteSize.ToFullWords()`. This task depends on the shared trim-suffix data-unit formatter path from commit `b26dca1b`.
- **files edited/created**: `src/Humanizer/Properties/Resources.bn.resx`, `tests/Humanizer.Tests/Localisation/bn-BD/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/bn-BD/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/bn-BD/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/bn-BD/Bytes/ToFullWordsTests.cs`

### T8: Complete Catalan residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ca.resx`, `tests/Humanizer.Tests/Localisation/ca/**/*`
- **description**: Finish the remaining T1-approved `DateHumanize` and `TimeSpanHumanize` gaps for `ca` and extend tests so the locale is fully closed out on approved surfaces.
- **validation**: Targeted `ca` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the missing Catalan `DateHumanize` residual keys (`TwoDays*`, `MultipleDays*_Paucal`) and the missing `TimeSpanHumanize_Age` plus `TimeSpanHumanize_MultipleDays_Paucal` entries. Added direct Catalan regression coverage for explicit residual resource values and `ToAge` behavior.
- **files edited/created**: `src/Humanizer/Properties/Resources.ca.resx`, `tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/ca/TimeSpanHumanizeTests.cs`

### T9: Complete Czech locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.cs.resx`, `tests/Humanizer.Tests/Localisation/cs/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `cs`, adding direct locale tests for each completed surface.
- **validation**: Targeted `cs` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T10: Complete Danish residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.da.resx`, `tests/Humanizer.Tests/Localisation/da/**/*`
- **description**: Finish the remaining T1-approved `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, and `DataUnit` gaps for `da`, preserving the current collection formatter and heading fixes.
- **validation**: Targeted `da` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T11: Complete German residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.de.resx`, `tests/Humanizer.Tests/Localisation/de/**/*`
- **description**: Finish the remaining T1-approved `DateHumanize` and `TimeSpanHumanize` gaps for `de`, preserving the existing heading, unit, and byte coverage.
- **validation**: Targeted `de` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Closed the actionable German residual gap by adding an explicit `TimeSpanHumanize_Age` resource (`{0} alt`) and regression coverage for numeric and `toWords` `ToAge` behavior.
- **files edited/created**: `src/Humanizer/Properties/Resources.de.resx`, `tests/Humanizer.Tests/Localisation/de/TimeSpanHumanizeTests.cs`

### T12: Complete Greek locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.el.resx`, `tests/Humanizer.Tests/Localisation/el/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `el`, adding direct locale tests for each completed surface.
- **validation**: Targeted `el` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T13: Complete Spanish residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.es.resx`, `tests/Humanizer.Tests/Localisation/es/**/*`
- **description**: Finish the remaining T1-approved `DateHumanize` and `TimeSpanHumanize` gaps for `es`, preserving the existing heading, unit, and byte fixes.
- **validation**: Targeted `es` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the missing Spanish residual parity resources for `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`) and `TimeSpanHumanize` (`Age`, `MultipleDays_Paucal`). Added regression tests that assert explicit Spanish residual resource presence and values.
- **files edited/created**: `src/Humanizer/Properties/Resources.es.resx`, `tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/es/TimeSpanHumanizeTests.cs`

### T14: Complete Persian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.fa.resx`, `tests/Humanizer.Tests/Localisation/fa/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `fa`, including RTL-safe wording validation and direct locale tests for each completed surface.
- **validation**: Targeted `fa` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T15: Complete Finnish locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.fi.resx`, `tests/Humanizer.Tests/Localisation/fi-FI/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `fi`, expanding beyond the current limited date/time tests to full locale parity coverage.
- **validation**: Targeted `fi-FI` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T16: Complete Filipino locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.fil.resx`, `tests/Humanizer.Tests/Localisation/fil-PH/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `fil`, adding the missing direct `DateHumanize` coverage and completing the remaining resource families.
- **validation**: Targeted `fil-PH` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the missing Filipino `DateHumanize` keys for `TwoDays*` and `MultipleDays*_Paucal`, corrected malformed day humanization strings, filled the missing `TimeSpanHumanize` age and `*_Words` resources, and added Filipino `TimeUnit`, `DataUnit`, and heading resources. New direct locale tests now cover `DateHumanize`, `Heading`, `TimeUnit.ToSymbol`, `ByteSize.ToFullWords`, and expanded `TimeSpanHumanize` behavior including `ToAge` and `toWords`.
- **files edited/created**: `src/Humanizer/Properties/Resources.fil.resx`, `tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/fil-PH/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/fil-PH/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/fil-PH/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/fil-PH/Bytes/ToFullWordsTests.cs`

### T17: Complete French locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.fr.resx`, `tests/Humanizer.Tests/Localisation/fr/**/*`, `tests/Humanizer.Tests/Localisation/fr-BE/**/*`, `tests/Humanizer.Tests/Localisation/fr-CH/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, and heading surfaces for `fr` and add or extend tests accordingly without disturbing the existing byte/unit coverage.
- **validation**: Targeted French locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the missing French residual `DateHumanize` keys (`TwoDays*`, `MultipleDays*_Paucal`) plus `TimeSpanHumanize_Age` and `TimeSpanHumanize_MultipleDays_Paucal`. Added direct French regression coverage for explicit residual resource values and presence.
- **files edited/created**: `src/Humanizer/Properties/Resources.fr.resx`, `tests/Humanizer.Tests/Localisation/fr/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/fr/TimeSpanHumanizeTests.cs`

### T18: Complete Hebrew locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.he.resx`, `tests/Humanizer.Tests/Localisation/he/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `he`, including RTL-safe wording validation and direct locale tests for each completed surface.
- **validation**: Targeted `he` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T19: Complete Croatian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.hr.resx`, `tests/Humanizer.Tests/Localisation/hr/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `hr`, adding direct locale tests for each completed surface.
- **validation**: Targeted `hr` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T20: Complete Hungarian residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.hu.resx`, `tests/Humanizer.Tests/Localisation/hu/**/*`
- **description**: Finish the remaining T1-approved `TimeSpanHumanize` and `DataUnit` gaps for `hu`, preserving the current heading and `TimeUnit` fixes.
- **validation**: Targeted `hu` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Hungarian `ToAge` regression coverage and a new Hungarian byte full-word test suite. RED exposed that Hungarian `DataUnit` localization could not be fixed by resource entries alone because `DefaultFormatter` was appending English plural `s` suffixes. Resolved this by adding Hungarian `DataUnit_*` resources, a `TimeSpanHumanize_Age` resource entry, and a dedicated `HungarianFormatter` wired through the formatter registry to trim the English plural suffix for non-symbol data units.
- **files edited/created**: `src/Humanizer/Configuration/FormatterRegistry.cs`, `src/Humanizer/Localisation/Formatters/HungarianFormatter.cs`, `src/Humanizer/Properties/Resources.hu.resx`, `tests/Humanizer.Tests/Localisation/hu/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/hu/Bytes/ToFullWordsTests.cs`

### T21: Complete Armenian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.hy.resx`, `tests/Humanizer.Tests/Localisation/hy/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `hy`, adding direct locale tests for each completed surface.
- **validation**: Targeted `hy` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Armenian residual `DateHumanize` (`Never`, `TwoDays*`), `TimeSpanHumanize_Age`, full `TimeUnit_*`, `DataUnit_*`, and 8-point heading resources. Routed Armenian through the trim-suffix formatter to stop English `s` plural leakage in `ByteSize.ToFullWords()`. Added direct Armenian regression coverage for explicit residual resources, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`.
- **files edited/created**: `src/Humanizer/Configuration/FormatterRegistry.cs`, `src/Humanizer/Properties/Resources.hy.resx`, `tests/Humanizer.Tests/Localisation/hy/ResourcesTests.cs`, `tests/Humanizer.Tests/Localisation/hy/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/hy/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/hy/Bytes/ToFullWordsTests.cs`

### T22: Complete Indonesian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.id.resx`, `tests/Humanizer.Tests/Localisation/id/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `id`, adding direct locale tests for each completed surface.
- **validation**: Targeted `id` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Indonesian `DataUnit_*`, `TimeUnit_*`, residual `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`, `Never`), `TimeSpanHumanize_Age`, and heading resources. Added direct regression coverage for residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting, and `ByteSize.ToFullWords()`. This task depends on the shared trim-suffix formatter registration widened for `id`.
- **files edited/created**: `src/Humanizer/Properties/Resources.id.resx`, `tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/id/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/id/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/id/Bytes/ToFullWordsTests.cs`

### T23: Complete Icelandic residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.is.resx`, `tests/Humanizer.Tests/Localisation/is/**/*`
- **description**: Finish the remaining T1-approved `DateHumanize`, `TimeSpanHumanize`, and `TimeUnit` gaps for `is`, preserving the existing heading and byte coverage.
- **validation**: Targeted `is` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the missing Icelandic residual `DateHumanize` keys (`TwoDays*`, `MultipleDays*_Dual`, `MultipleDays*_Paucal`) and the missing `TimeSpanHumanize` residual keys (`Age`, `MultipleDays_Dual`, `MultipleDays_Paucal`). Added direct Icelandic regression coverage asserting those resources resolve to Icelandic values instead of falling back to English.
- **files edited/created**: `src/Humanizer/Properties/Resources.is.resx`, `tests/Humanizer.Tests/Localisation/is/ResourcesTests.cs`

### T24: Complete Italian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.it.resx`, `tests/Humanizer.Tests/Localisation/it/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `it`, adding direct locale tests for each completed surface.
- **validation**: Targeted `it` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T25: Complete Japanese locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ja.resx`, `tests/Humanizer.Tests/Localisation/ja/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `ja`, adding direct locale tests for each completed surface.
- **validation**: Targeted `ja` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Japanese `DataUnit_*`, `TimeUnit_*`, residual `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`, `Never`), `TimeSpanHumanize_Age`, and Japanese heading resources. Added direct regression coverage for residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`. This task depends on the shared trim-suffix formatter registration widened for `ja`.
- **files edited/created**: `src/Humanizer/Properties/Resources.ja.resx`, `tests/Humanizer.Tests/Localisation/ja/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/ja/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/ja/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/ja/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/ja/Bytes/ToFullWordsTests.cs`

### T26: Complete Korean locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ko.resx`, `tests/Humanizer.Tests/Localisation/ko-KR/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `ko`, adding the missing direct `DateHumanize` coverage and completing the remaining resource families.
- **validation**: Targeted `ko-KR` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Korean `DataUnit_*`, `TimeUnit_*`, residual `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`), `TimeSpanHumanize_Age`, `TimeSpanHumanize_MultipleDays_Paucal`, missing month/year variants, `*_Words` entries, and full/short heading resources. Added direct Korean regression coverage for residual date resources, `ToAge()`, `TimeUnit.ToSymbol()`, `ByteSize.ToFullWords()`, and heading round-tripping. This task depends on the shared trim-suffix data-unit formatter path from commit `b26dca1b`.
- **files edited/created**: `src/Humanizer/Properties/Resources.ko.resx`, `tests/Humanizer.Tests/Localisation/ko-KR/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/ko-KR/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/ko-KR/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/ko-KR/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/ko-KR/Bytes/ToFullWordsTests.cs`

### T27: Complete Kurdish locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ku.resx`, `tests/Humanizer.Tests/Localisation/ku/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `ku`, preserving current date/time coverage and extending tests to all completed surfaces.
- **validation**: Targeted `ku` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the missing Kurdish residual `DateHumanize` keys (`TwoDays*`, `MultipleDays*_Paucal`) plus `TimeSpanHumanize_Age`. Added direct Kurdish regression coverage for explicit residual resource values and stable `ToAge()` behavior.
- **files edited/created**: `src/Humanizer/Properties/Resources.ku.resx`, `tests/Humanizer.Tests/Localisation/ku/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/ku/TimeSpanHumanizeTests.cs`

### T28: Complete Luxembourgish locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.lb.resx`, `tests/Humanizer.Tests/Localisation/lb/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, and heading surfaces for `lb`, preserving the existing collection formatter and byte coverage.
- **validation**: Targeted `lb` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the missing Luxembourgish residual `DateHumanize` keys (`TwoDays*`, `MultipleDays*_Paucal`) and `TimeSpanHumanize` residual keys (`Age`, `MultipleDays_Paucal`). Added direct Luxembourgish regression coverage for explicit residual resource presence/value and `ToAge()` behavior.
- **files edited/created**: `src/Humanizer/Properties/Resources.lb.resx`, `tests/Humanizer.Tests/Localisation/lb/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/lb/TimeSpanHumanizeTests.cs`

### T29: Complete Lithuanian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.lt.resx`, `tests/Humanizer.Tests/Localisation/lt/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `lt`, adding direct locale tests for each completed surface.
- **validation**: Targeted `lt` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Lithuanian residual `DateHumanize` (`Never`, `TwoDays*`), `TimeSpanHumanize_Age`, full `TimeUnit_*`, full 16-point heading resources, and full `DataUnit_*` resources with singular/plural Lithuanian forms. Extended `LithuanianFormatter` with `DataUnitHumanize` support so full-word byte sizes use localized singular and genitive plural forms instead of English suffixing. Added direct regression coverage for explicit residual resources, `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`.
- **files edited/created**: `src/Humanizer/Localisation/Formatters/LithuanianFormatter.cs`, `src/Humanizer/Properties/Resources.lt.resx`, `tests/Humanizer.Tests/Localisation/lt/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/lt/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/lt/ResourcesTests.cs`, `tests/Humanizer.Tests/Localisation/lt/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/lt/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/lt/Bytes/ToFullWordsTests.cs`

### T30: Complete Latvian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.lv.resx`, `tests/Humanizer.Tests/Localisation/lv/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `lv`, including the currently missing direct `DateHumanize` coverage and all remaining resource-family gaps.
- **validation**: Targeted `lv` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added explicit Latvian residual `DateHumanize` resources (`TwoDays*`, `MultipleDays*_Paucal`), `TimeSpanHumanize_Age`, full `TimeUnit_*`, full heading resources, and `DataUnit_*` plural stems. Added direct regression coverage for residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`. This task depends on the dedicated Latvian formatter path from commit `4ef5f583`.
- **files edited/created**: `src/Humanizer/Properties/Resources.lv.resx`, `tests/Humanizer.Tests/Localisation/lv/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/lv/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/lv/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/lv/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/lv/Bytes/ToFullWordsTests.cs`

### T31: Complete Malay locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ms.resx`, `tests/Humanizer.Tests/Localisation/ms-MY/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `ms`, including the currently missing direct `DateHumanize` coverage and all remaining resource-family gaps.
- **validation**: Targeted `ms-MY` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Malay residual `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`), `TimeSpanHumanize_Age`, full `TimeUnit_*`, and `DataUnit_*` resources. Added direct regression coverage for explicit residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, and `ByteSize.ToFullWords()`. Per task execution, `ToAge(toWords: true)` was treated as out of scope because Malay does not currently have independent number-to-words coverage in this locale scope.
- **files edited/created**: `src/Humanizer/Properties/Resources.ms.resx`, `tests/Humanizer.Tests/Localisation/ms-MY/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/ms-MY/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/ms-MY/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/ms-MY/Bytes/ToFullWordsTests.cs`

### T32: Complete Maltese residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.mt.resx`, `tests/Humanizer.Tests/Localisation/mt/**/*`
- **description**: Finish the remaining T1-approved `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, and `DataUnit` gaps for `mt`, preserving the current heading fixes.
- **validation**: Targeted `mt` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added explicit Maltese `DateHumanize` residual keys (`TwoDays*`, `MultipleDays*_Paucal`), `TimeSpanHumanize_Age`, full `TimeUnit_*` resources, and explicit `DataUnit_*` resources. Added direct regression coverage for residual date/time resources, `ToAge`, `TimeUnit.ToSymbol`, `ByteSize.ToFullWords`, and Maltese `DataUnit` presence.
- **files edited/created**: `src/Humanizer/Properties/Resources.mt.resx`, `tests/Humanizer.Tests/Localisation/mt/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/mt/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/mt/Bytes/ToFullWordsTests.cs`, `tests/Humanizer.Tests/Localisation/mt/TimeUnitToSymbolTests.cs`

### T33: Complete Norwegian Bokmal locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.nb.resx`, `tests/Humanizer.Tests/Localisation/nb/**/*`, `tests/Humanizer.Tests/Localisation/nb-NO/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `nb`, preserving the current date-resource fixes.
- **validation**: Targeted Norwegian Bokmal locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T34: Complete Dutch locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.nl.resx`, `tests/Humanizer.Tests/Localisation/nl/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `nl`, preserving the current date-resource fixes.
- **validation**: Targeted `nl` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T35: Complete Polish locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.pl.resx`, `tests/Humanizer.Tests/Localisation/pl/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `pl`, adding direct locale tests for each completed surface.
- **validation**: Targeted `pl` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T36: Complete Portuguese residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.pt.resx`, `tests/Humanizer.Tests/Localisation/pt/**/*`
- **description**: Finish the remaining T1-approved `DateHumanize` and `TimeSpanHumanize` gaps for `pt`, preserving the current heading, unit, and byte coverage.
- **validation**: Targeted `pt` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the missing Portuguese residual parity resources for `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`) and `TimeSpanHumanize_Age`. Added direct Portuguese tests for explicit residual resource presence/values and `ToAge` behavior.
- **files edited/created**: `src/Humanizer/Properties/Resources.pt.resx`, `tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs`

### T37: Complete Brazilian Portuguese residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.pt-BR.resx`, `tests/Humanizer.Tests/Localisation/pt-BR/**/*`
- **description**: Finish the remaining T1-approved `DateHumanize` and `TimeSpanHumanize` gaps for `pt-BR`, preserving the current heading, unit, and byte coverage.
- **validation**: Targeted `pt-BR` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added the four missing Brazilian Portuguese `DateHumanize` residual parity keys (`TwoDays*`, `MultipleDays*_Paucal`) and direct `pt-BR` tests asserting those residual resources, plus explicit `ToAge` coverage in the `TimeSpanHumanize` tests.
- **files edited/created**: `src/Humanizer/Properties/Resources.pt-BR.resx`, `tests/Humanizer.Tests/Localisation/pt-BR/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs`

### T38: Complete Romanian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ro.resx`, `tests/Humanizer.Tests/Localisation/ro-Ro/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `ro`, preserving the existing collection formatter/date coverage and closing the remaining parity gap.
- **validation**: Targeted `ro-Ro` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T39: Complete Russian residual parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ru.resx`, `tests/Humanizer.Tests/Localisation/ru-RU/**/*`
- **description**: Finish the remaining T1-approved `TimeSpanHumanize` gap for `ru` and confirm the locale remains closed on date, heading, unit, and byte surfaces.
- **validation**: Targeted `ru-RU` locale tests cover all approved surfaces and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Closed the single remaining Russian parity gap by adding an explicit `TimeSpanHumanize_Age` resource and a regression that asserts `ru-RU` resolves its own age format instead of silently inheriting the neutral English form.
- **files edited/created**: `src/Humanizer/Properties/Resources.ru.resx`, `tests/Humanizer.Tests/Localisation/ru-RU/TimeSpanHumanizeTests.cs`

### T40: Complete Slovak locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sk.resx`, `tests/Humanizer.Tests/Localisation/sk/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `sk`, adding direct locale tests for each completed surface.
- **validation**: Targeted `sk` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T41: Complete Slovenian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sl.resx`, `tests/Humanizer.Tests/Localisation/sl/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `sl`, adding direct locale tests for each completed surface.
- **validation**: Targeted `sl` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Slovenian residual `DateHumanize` (`TwoDays*`, `Never`), `TimeSpanHumanize_Age`, full `TimeUnit_*`, full 16-point heading resources, and full `DataUnit_*` resources with Slovenian singular/dual/paucal/plural forms. Extended `SlovenianFormatter` with `DataUnitHumanize` support so full-word byte sizes resolve to Slovenian number forms instead of English suffixing. Added direct regression coverage for explicit residual resources, `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`.
- **files edited/created**: `src/Humanizer/Localisation/Formatters/SlovenianFormatter.cs`, `src/Humanizer/Properties/Resources.sl.resx`, `tests/Humanizer.Tests/Localisation/sl/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/sl/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/sl/ResourcesTests.cs`, `tests/Humanizer.Tests/Localisation/sl/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/sl/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/sl/Bytes/ToFullWordsTests.cs`

### T42: Complete Serbian Cyrillic locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sr.resx`, `tests/Humanizer.Tests/Localisation/sr/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `sr`, preserving the current teen-number formatter fix and extending the locale to full approved parity.
- **validation**: Targeted `sr` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T43: Complete Serbian Latin locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sr-Latn.resx`, `tests/Humanizer.Tests/Localisation/sr-Latn/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `sr-Latn`, preserving the current teen-number formatter fix and extending the locale to full approved parity.
- **validation**: Targeted `sr-Latn` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T44: Complete Swedish locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sv.resx`, `tests/Humanizer.Tests/Localisation/sv/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `sv`, preserving the current singular-hour wording fix.
- **validation**: Targeted `sv` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T45: Complete Thai locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.th.resx`, `tests/Humanizer.Tests/Localisation/th-TH/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `th`, preserving the existing direct date coverage and adding the missing `TimeSpanHumanize` and remaining parity surfaces.
- **validation**: Targeted `th-TH` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Thai residual `DateHumanize` (`TwoDays*`, `MultipleDays*_Paucal`), `TimeSpanHumanize_Age`, full `TimeUnit_*`, and `DataUnit_*` resources. Added direct regression coverage for explicit residual date resources, `ToAge()`, `TimeUnit.ToSymbol()`, and `ByteSize.ToFullWords()`. This task depends on the shared trim-suffix data-unit formatter path from commit `b26dca1b`.
- **files edited/created**: `src/Humanizer/Properties/Resources.th.resx`, `tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/th-TH/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/th-TH/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/th-TH/Bytes/ToFullWordsTests.cs`

### T46: Complete Turkish locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.tr.resx`, `tests/Humanizer.Tests/Localisation/tr/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `tr`, adding direct locale tests for each completed surface.
- **validation**: Targeted `tr` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Turkish residual `DateHumanize` (`Never`, `TwoDays*`, `MultipleDays*_Paucal`), `TimeSpanHumanize_Age`, Turkish `TimeSpanHumanize_Single*_Words` entries for `ToAge(toWords: true)`, full `TimeUnit_*`, `DataUnit_*`, and 8-point heading resources. Added direct regression coverage for residual date resources, `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`. This task depends on the shared trim-suffix formatter registration widened for `tr`.
- **files edited/created**: `src/Humanizer/Properties/Resources.tr.resx`, `tests/Humanizer.Tests/Localisation/tr/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/tr/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/tr/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/tr/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/tr/Bytes/ToFullWordsTests.cs`

### T47: Complete Ukrainian locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.uk.resx`, `tests/Humanizer.Tests/Localisation/uk-UA/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `uk`, preserving current date/time coverage and extending the locale to full approved parity.
- **validation**: Targeted `uk-UA` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T48: Complete Uzbek Cyrillic locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx`, `tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `uz-Cyrl-UZ`, adding direct locale tests for each completed surface.
- **validation**: Targeted `uz-Cyrl-UZ` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Uzbek Cyrillic `DataUnit_*`, `TimeUnit_*`, residual `DateHumanize`, `TimeSpanHumanize_Age`, and heading resources with direct regression coverage for residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`. This task depends on the shared trim-suffix formatter registration widened for `uz-Cyrl-UZ`.
- **files edited/created**: `src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx`, `tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/Bytes/ToFullWordsTests.cs`

### T49: Complete Uzbek Latin locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.uz-Latn-UZ.resx`, `tests/Humanizer.Tests/Localisation/uz-Latn-UZ/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `uz-Latn-UZ`, adding direct locale tests for each completed surface.
- **validation**: Targeted `uz-Latn-UZ` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Uzbek Latin `DataUnit_*`, `TimeUnit_*`, residual `DateHumanize`, `TimeSpanHumanize_Age`, and heading resources with direct regression coverage for residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, heading formatting/reverse lookup, and `ByteSize.ToFullWords()`. This task depends on the shared trim-suffix formatter registration widened for `uz-Latn-UZ`.
- **files edited/created**: `src/Humanizer/Properties/Resources.uz-Latn-UZ.resx`, `tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/uz-Latn-UZ/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/uz-Latn-UZ/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/uz-Latn-UZ/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/uz-Latn-UZ/Bytes/ToFullWordsTests.cs`

### T50: Complete Vietnamese locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.vi.resx`, `tests/Humanizer.Tests/Localisation/vi/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `vi`, adding direct locale tests for each completed surface.
- **validation**: Targeted `vi` locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Completed
- **log**: Added Vietnamese residual `DateHumanize` (`Never`, `TwoDays*`, `MultipleDays*_Paucal`), `TimeSpanHumanize_Age`, full `TimeUnit_*`, `DataUnit_*`, and 8-point heading resources. Added direct regression coverage for residual date resources, numeric `ToAge()`, `TimeUnit.ToSymbol()`, `ByteSize.ToFullWords()`, and heading formatting/reverse lookup. This task depends on the shared trim-suffix formatter registration widened for `vi`.
- **files edited/created**: `src/Humanizer/Properties/Resources.vi.resx`, `tests/Humanizer.Tests/Localisation/vi/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/vi/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/vi/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/vi/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/vi/Bytes/ToFullWordsTests.cs`

### T51: Complete Chinese Simplified locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.zh-CN.resx`, `src/Humanizer/Properties/Resources.zh-Hans.resx`, `tests/Humanizer.Tests/Localisation/zh-CN/**/*`, `tests/Humanizer.Tests/Localisation/zh-Hans/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for the Simplified Chinese locales, resolving any `zh-CN` versus `zh-Hans` sharing rules explicitly in the matrix.
- **validation**: Targeted Simplified Chinese locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T52: Complete Chinese Traditional locale parity
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.zh-Hant.resx`, `tests/Humanizer.Tests/Localisation/zh-Hant/**/*`, `tests/Humanizer.Tests/Localisation/zh-HK/**/*`
- **description**: Complete all T1-approved missing `DateHumanize`, `TimeSpanHumanize`, `TimeUnit`, `DataUnit`, and heading surfaces for `zh-Hant`, resolving any Traditional Chinese fallback rules explicitly in the matrix.
- **validation**: Targeted Traditional Chinese locale tests cover every completed surface and prove no English fallback remains on those surfaces.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T53: Complete advanced locale backlog sweep for the remaining high-gap locales
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.*.resx`, `tests/Humanizer.Tests/Localisation/**/*`
- **description**: After the per-locale tasks above are underway, sweep for any locale omitted by the current matrix snapshot or any matrix-approved surface that was reclassified during T1/T2. This is the catch-all task for deltas discovered by the authoritative matrix so the plan remains exhaustive without silently dropping surfaces.
- **validation**: Matrix artifact shows no unassigned locale/surface backlog after the task is complete.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T54: Full verification and documentation closeout
- **depends_on**: [T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53]
- **location**: `src/Humanizer/Humanizer.csproj`, `tests/Humanizer.Tests/Humanizer.Tests.csproj`, `readme.md`, `docs/plans/2026-03-13-locale-parity-matrix-plan.md`
- **description**: Run the full branch verification suite after every locale task is complete. Update documentation only if supported behavior changed materially, and update this plan with actual completion logs instead of batch-level claims.
- **validation**: `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`, `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0`, and `dotnet build Humanizer/Humanizer.csproj -c Release /t:PackNuSpecs /p:PackageOutputPath=<path>` all succeed.
- **status**: Not Completed
- **log**:
- **files edited/created**:

## Parallel Execution Groups

| Wave | Tasks | Can Start When |
|------|-------|----------------|
| 1 | T1 | Immediately |
| 2 | T2 | T1 complete |
| 3 | T3-T17 | T1, T2 complete |
| 4 | T18-T35 | T1, T2 complete |
| 5 | T36-T53 | T1, T2 complete |
| 6 | T54 | T3-T53 complete |

## Testing Strategy
- Use T1 to lock the exact approved surface families per locale before editing any resources.
- For each locale task, add or update direct locale tests for every completed surface family instead of relying on incidental coverage.
- Prefer targeted `net10.0` locale test runs while executing individual tasks, then use T54 for the required full `net10.0` and `net8.0` suite runs.
- Preserve valid current fixes from `51160280` and only extend them where parity requires more work.
- Use shared guardrails from T2 to prevent silent English fallback and to catch missing locale keys deterministically.

## Risks & Mitigations
- Risk: Neutral-resource parity overstates intended surfaces for some locales.
  Mitigation: T1 is a hard gate; tasks implement only the matrix-approved surfaces, not blind neutral parity.
- Risk: Shared localization files become merge hotspots.
  Mitigation: T2 centralizes shared guardrail work before locale tasks fan out.
- Risk: Locale wording quality varies by script and grammar.
  Mitigation: Require locale-specific regression tests and note translation provenance in task logs where native validation is unavailable.
- Risk: Batch-level completion gets claimed again without locale-level evidence.
  Mitigation: Every locale has its own task, its own validation, and T54 cannot start until all locale tasks are complete.
