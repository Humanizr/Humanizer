# Plan: Locale Linguistic Parity Completion

**Generated**: 2026-04-04

## Overview

Status: Completed on 2026-04-04. Every shipped locale now resolves every canonical localization surface intentionally, either locale-owned or same-language inherited. Full validation passed on `net10.0`, `net8.0`, and `dotnet pack`.

The current tree now reflects the completed parity state. The remaining sections are preserved as execution closeout history: they record which task blocks were completed, what was validated, and which files carried the work.

Primary audit inputs:

- [2026-04-04-locale-feature-parity-matrix.md](E:/Dev/Humanizer/artifacts/2026-04-04-locale-feature-parity-matrix.md)
- [2026-04-04-locale-linguistic-completeness-matrix.md](E:/Dev/Humanizer/artifacts/2026-04-04-locale-linguistic-completeness-matrix.md)

## Prerequisites

- Read [docs/adding-a-locale.md](E:/Dev/Humanizer/docs/adding-a-locale.md)
- Read [docs/locale-yaml-how-to.md](E:/Dev/Humanizer/docs/locale-yaml-how-to.md)
- Read [docs/locale-yaml-reference.md](E:/Dev/Humanizer/docs/locale-yaml-reference.md)
- Read [CanonicalLocaleAuthoring.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs)
- Read [parity-checklist.md](E:/Dev/Humanizer/.agents/skills/add-locale/references/parity-checklist.md)
- Each locale worker must maintain `artifacts/YYYY-MM-DD-<locale>-parity-map.md`
- Each locale worker must run the add-locale two-pass term workflow whenever new locale-owned wording is introduced

## Worker Protocol

Every implementation task below is assigned to an agent that must explicitly use [add-locale](E:/Dev/Humanizer/.agents/skills/add-locale/SKILL.md).

For every locale in scope, that agent must:

1. Produce the preflight gap report for all canonical surfaces.
2. Create and maintain the per-locale parity map artifact in `artifacts/`.
3. Record the ownership path for every surface as `locale-owned`, `same-language inherited`, `missing`, `english-fallback`, or `unsupported`.
4. Drive the locale’s effective-gap summary to empty.
5. Add exact-output and sweep proof for every changed surface.

No task is complete just because a YAML file was edited. A task is complete only when every locale in its scope satisfies the add-locale completion gate.

## Dependency Graph

```text
T1 ──┬── T4 ──┬── T7 ──┐
     │        ├── T8 ──┤
T2 ──┼── T5 ──┼── T9 ──┤
     │        ├── T10 ─┤
T3 ──┴── T6 ──┼── T11 ─┤
              ├── T12 ─┤
              └── T13 ─┤

T7 ──┬─────────────────┤
T8 ──┼─────────────────┤
T9 ──┼─────────────────┤
T10 ─┼─────────────────┤── T14 ── T15 ── T16
T11 ─┼─────────────────┤
T12 ─┼─────────────────┤
T13 ─┘─────────────────┘
```

## Tasks

### T1: Build The Execution Backlog From The Parity Matrix
- **depends_on**: []
- **location**: [artifacts/2026-04-04-locale-feature-parity-matrix.md](E:/Dev/Humanizer/artifacts/2026-04-04-locale-feature-parity-matrix.md), [artifacts/2026-04-04-locale-linguistic-completeness-matrix.md](E:/Dev/Humanizer/artifacts/2026-04-04-locale-linguistic-completeness-matrix.md), `artifacts/*-parity-map.md`
- **description**: Convert the existing matrix into per-locale execution artifacts. Generate the initial parity-map files for every incomplete locale, recording ownership paths, unresolved surfaces, likely files to touch, proving tests, and the exact runtime culture/test-folder mapping to use for that locale. Group locales into non-overlapping ownership clusters so each locale is closed by one later task.
- **validation**: Every incomplete locale has a parity-map artifact with no `unknown` rows, an initial unresolved set derived from the matrix, and an explicit test-folder/runtime-culture mapping.
- **status**: Completed
- **log**: Generated per-locale parity-map artifacts for all 54 incomplete locales, added explicit runtime culture/test-folder mappings, and created a locale execution backlog artifact grouping locales by their owning plan task.
- **files edited/created**: [2026-04-04-locale-execution-backlog.md](E:/Dev/Humanizer/artifacts/2026-04-04-locale-execution-backlog.md); `artifacts/2026-04-04-*-parity-map.md`

### T2: Cluster Shared Engine Work For Missing Feature Families
- **depends_on**: []
- **location**: [EngineContractCatalog.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs), [LocaleYamlCatalog.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs), [src/Humanizer/Localisation](E:/Dev/Humanizer/src/Humanizer/Localisation), [tests/Humanizer.SourceGenerators.Tests](E:/Dev/Humanizer/tests/Humanizer.SourceGenerators.Tests)
- **description**: Audit the missing-feature families and decide where existing shared kernels can be reused versus where new structural engines are required. The required families are `formatter`, `ordinal.numeric`, `ordinal.date/dateOnly`, `clock`, and `compass`. Produce a shared-engine decision artifact in `artifacts/YYYY-MM-DD-shared-engine-decisions.md` that names the owning runtime path, generator path, registry path, and source-generator tests for each family. If the audit shows `formatter` or `compass` need shared runtime/generator/registry work before locale rollout, implement that work in this task so downstream locale owners are not blocked by unowned infrastructure.
- **validation**: Each missing-feature family has an implementation decision recorded in a single decision artifact with explicit runtime, generator, registry, and test ownership, and any required shared `formatter` or `compass` infrastructure changes are implemented and covered.
- **status**: Completed
- **log**: Audited the shared feature families and wrote a single decision artifact identifying the owning runtime/generator/registry/test paths. Confirmed that `formatter` and `compass` do not need new shared infrastructure before locale rollout. Also classified `clock` as a reuse-first family because the existing `default`, `phrase-hour`, and `relative-hour` paths already cover part of the missing-locale space; T4-T6 should start from classification against existing engines before adding new contracts.
- **files edited/created**: [2026-04-04-shared-engine-decisions.md](E:/Dev/Humanizer/artifacts/2026-04-04-shared-engine-decisions.md)

### T3: Establish Proof Expectations In Sweep And Registry Tests
- **depends_on**: []
- **location**: [LocaleCoverageData.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs), [LocaleFallbackSweepTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs), [LocaleRegistrySweepTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs), [GeneratedFormatterRuntimeTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/GeneratedLocaleData/GeneratedFormatterRuntimeTests.cs), [ExactLocaleDateAndTimeRegistryTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/ExactLocaleDateAndTimeRegistryTests.cs)
- **description**: Tighten the proving framework so locale tasks can close gaps without silently missing registry or inheritance coverage. Add or restructure theory data to express the final parity target for every canonical surface, including `compass` and child-locale inheritance proof for variant families.
- **validation**: The sweep and registry tests can express the final parity target for every canonical surface, including `compass` and variant-locale inheritance, without relying on outdated locale-exception lists.
- **status**: Completed
- **log**: Expanded the proof harness to cover compass locale round-trips, variant inheritance for compass/date/dateOnly/clock, and broader parent-locale expectations in the registry and fallback sweep tests. Focused validation passed for `LocaleFallbackSweepTests` and `LocaleRegistrySweepTests` on `net10.0` and `net8.0`.
- **files edited/created**: [LocaleCoverageData.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs); [LocaleFallbackSweepTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs); [LocaleRegistrySweepTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs)

### T4: Design And Implement Shared `ordinal.numeric` Expansions
- **depends_on**: [T2]
- **location**: [EngineContractCatalog.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs), [LocaleYamlCatalog.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs), [src/Humanizer/Localisation/Ordinalizers](E:/Dev/Humanizer/src/Humanizer/Localisation/Ordinalizers), [src/Humanizer.SourceGenerators/Generators/ProfileCatalogs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation), [tests/Humanizer.SourceGenerators.Tests/SourceGenerators](E:/Dev/Humanizer/tests/Humanizer.SourceGenerators.Tests/SourceGenerators)
- **description**: Close the shared-engine gaps that block `ordinal.numeric` in missing languages. This includes engine reuse or additions needed for Semitic, East Asian, Indic, Southeast Asian, and thin-locale cases, plus any required generator/schema/registry wiring and source-generator tests.
- **validation**: Shared ordinalizer support exists for every `ordinal.numeric` gap family identified in the matrix, and the owning generator and source-generator tests are in place.
  - **status**: Completed
  - **log**: Shared ordinal.numeric support is present in the validated tree; locale coverage and generator tests now pass without unresolved ordinal gaps.
  - **files edited/created**: `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`; `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`; `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalizerProfileCatalogInput.cs`; `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/CanonicalLocaleSchemaTests.cs`; `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/HumanizerSourceGeneratorTests.cs`; `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`; locale-specific ordinal tests under `tests/Humanizer.Tests/Localisation/<culture>`

### T5: Design And Implement Shared `ordinal.date` / `ordinal.dateOnly` Expansions
- **depends_on**: [T2]
- **location**: [EngineContractCatalog.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs), [LocaleYamlCatalog.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs), [OrdinalDatePattern.cs](E:/Dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs), [DateToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs), [DateOnlyToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs), [src/Humanizer.SourceGenerators/Generators/ProfileCatalogs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs), [tests/Humanizer.SourceGenerators.Tests/SourceGenerators](E:/Dev/Humanizer/tests/Humanizer.SourceGenerators.Tests/SourceGenerators), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Build the reusable date-ordinal path needed by the 44 base-locale gaps. Separate pure-pattern locales from locales that need new day-mode support or procedural shared kernels, and wire the required generator/schema/registry paths.
- **validation**: The missing date-ordinal locales can be expressed through shared patterns or shared runtime kernels without unresolved structural gaps, and the owning generator and source-generator tests are in place.
  - **status**: Completed
  - **log**: Shared ordinal.date and ordinal.dateOnly support is present in the validated tree; date-ordinal coverage now resolves intentionally for all shipped locales.
  - **files edited/created**: `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`; `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`; `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs`; `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/CanonicalLocaleSchemaTests.cs`; `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/HumanizerSourceGeneratorTests.cs`; `tests/Humanizer.Tests/Localisation/ExactLocaleDateAndTimeRegistryTests.cs`; `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`; locale-specific date-ordinal tests under `tests/Humanizer.Tests/Localisation/<culture>`

### T6: Design And Implement Shared `clock` Expansions
- **depends_on**: [T2]
- **location**: [EngineContractCatalog.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs), [LocaleYamlCatalog.cs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs), [src/Humanizer/Localisation/TimeToClockNotation](E:/Dev/Humanizer/src/Humanizer/Localisation/TimeToClockNotation), [TimeOnlyToClockNotationConvertersRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs), [src/Humanizer.SourceGenerators/Generators/ProfileCatalogs](E:/Dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs), [tests/Humanizer.SourceGenerators.Tests/SourceGenerators](E:/Dev/Humanizer/tests/Humanizer.SourceGenerators.Tests/SourceGenerators), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Build the reusable shared engines needed for broad clock coverage. Start from the existing `phrase-hour`, `relative-hour`, and residual leaves, then add the structural families required to cover the 47 base-locale clock gaps, including generator/schema/registry updates and source-generator tests.
- **validation**: Shared clock engines exist for each newly identified phrase family and are covered by exact-output tests, registry assertions, and owning generator/source-generator tests.
  - **status**: Completed
  - **log**: Shared clock coverage is present in the validated tree; clock notation now resolves intentionally for all shipped locales.
  - **files edited/created**: `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs`; `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs`; `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationProfileCatalogInput.cs`; `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationEngineContractFactory.cs`; `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/CanonicalLocaleSchemaTests.cs`; `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/HumanizerSourceGeneratorTests.cs`; `tests/Humanizer.Tests/Localisation/ExactLocaleDateAndTimeRegistryTests.cs`; `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`; locale-specific clock tests under `tests/Humanizer.Tests/Localisation/<culture>`

### T7: Complete English, Portuguese, And Nynorsk Locale Families End-To-End
- **depends_on**: [T1, T2, T3, T4, T5, T6]
- **location**: [en.yml](E:/Dev/Humanizer/src/Humanizer/Locales/en.yml), [en-US.yml](E:/Dev/Humanizer/src/Humanizer/Locales/en-US.yml), [en-GB.yml](E:/Dev/Humanizer/src/Humanizer/Locales/en-GB.yml), [en-IN.yml](E:/Dev/Humanizer/src/Humanizer/Locales/en-IN.yml), [pt.yml](E:/Dev/Humanizer/src/Humanizer/Locales/pt.yml), [pt-BR.yml](E:/Dev/Humanizer/src/Humanizer/Locales/pt-BR.yml), [nn.yml](E:/Dev/Humanizer/src/Humanizer/Locales/nn.yml), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Own these locale families end-to-end in a single task so their parity maps can close cleanly. Resolve thin-file issues, inheritance proof, formatter/date/dateOnly/clock gaps, and any remaining number or compass gaps for `en`, `en-US`, `en-GB`, `en-IN`, `pt`, `pt-BR`, and `nn`.
- **validation**: Every locale in scope has an empty unresolved set, with child-locale inheritance reproved after parent-locale changes.
  - **status**: Completed
  - **log**: English, Portuguese, and Nynorsk family closeout is reflected in the validated tree; the family-specific parity maps now close without unresolved surfaces.
  - **files edited/created**: `src/Humanizer/Locales/en.yml`; `src/Humanizer/Locales/en-US.yml`; `src/Humanizer/Locales/en-GB.yml`; `src/Humanizer/Locales/en-IN.yml`; `src/Humanizer/Locales/pt.yml`; `src/Humanizer/Locales/pt-BR.yml`; `src/Humanizer/Locales/nn.yml`; `tests/Humanizer.Tests/Localisation/en/FamilyInheritanceParityTests.cs`; `tests/Humanizer.Tests/Localisation/nn/ParityTests.cs`; `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`; `tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs`; `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`

### T8: Complete Western Low-Gap Locales End-To-End
- **depends_on**: [T1, T2, T3, T5, T6]
- **location**: [af.yml](E:/Dev/Humanizer/src/Humanizer/Locales/af.yml), [da.yml](E:/Dev/Humanizer/src/Humanizer/Locales/da.yml), [es.yml](E:/Dev/Humanizer/src/Humanizer/Locales/es.yml), [nl.yml](E:/Dev/Humanizer/src/Humanizer/Locales/nl.yml), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Complete the smaller western locales that already have strong base coverage but still miss `formatter`, `ordinal.date`, `ordinal.dateOnly`, and `clock` in various combinations.
- **validation**: Every locale in scope has an empty unresolved set and exact-output proof for each previously missing surface.
  - **status**: Completed
  - **log**: Western low-gap locale closeout is reflected in the validated tree; all locales in this cluster now have complete parity maps and exact-output proof.
  - **files edited/created**: `src/Humanizer/Locales/af.yml`; `src/Humanizer/Locales/da.yml`; `src/Humanizer/Locales/es.yml`; `src/Humanizer/Locales/nl.yml`; `tests/Humanizer.Tests/Localisation/<culture>/*`

### T9: Complete Semitic And Near-East Locales End-To-End
- **depends_on**: [T1, T2, T3, T4, T5, T6]
- **location**: [ar.yml](E:/Dev/Humanizer/src/Humanizer/Locales/ar.yml), [fa.yml](E:/Dev/Humanizer/src/Humanizer/Locales/fa.yml), [he.yml](E:/Dev/Humanizer/src/Humanizer/Locales/he.yml), [ku.yml](E:/Dev/Humanizer/src/Humanizer/Locales/ku.yml), [mt.yml](E:/Dev/Humanizer/src/Humanizer/Locales/mt.yml), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Complete the Semitic and adjacent locales that still miss `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, `clock`, and some formatter coverage.
- **validation**: Every locale in scope has an empty unresolved set, with exact-output proof for ordinals, date ordinals, clock, and any formatter-specific behavior.
  - **status**: Completed
  - **log**: Semitic and near-east locale closeout is reflected in the validated tree; each locale now resolves its canonical surfaces intentionally.
  - **files edited/created**: `src/Humanizer/Locales/ar.yml`; `src/Humanizer/Locales/fa.yml`; `src/Humanizer/Locales/he.yml`; `src/Humanizer/Locales/ku.yml`; `src/Humanizer/Locales/mt.yml`; `tests/Humanizer.Tests/Localisation/<culture>/*`

### T10: Complete Indic, Southeast Asian, And African Locales End-To-End
- **depends_on**: [T1, T2, T3, T4, T5, T6]
- **location**: [bn.yml](E:/Dev/Humanizer/src/Humanizer/Locales/bn.yml), [fil.yml](E:/Dev/Humanizer/src/Humanizer/Locales/fil.yml), [id.yml](E:/Dev/Humanizer/src/Humanizer/Locales/id.yml), [ms.yml](E:/Dev/Humanizer/src/Humanizer/Locales/ms.yml), [ta.yml](E:/Dev/Humanizer/src/Humanizer/Locales/ta.yml), [th.yml](E:/Dev/Humanizer/src/Humanizer/Locales/th.yml), [vi.yml](E:/Dev/Humanizer/src/Humanizer/Locales/vi.yml), [zu-ZA.yml](E:/Dev/Humanizer/src/Humanizer/Locales/zu-ZA.yml), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Complete the Indic, Southeast Asian, and African locales end-to-end, including formatter or compass where missing, plus `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, and `clock`.
- **validation**: Every locale in scope has an empty unresolved set and exact-output proof for each newly introduced surface.
  - **status**: Completed
  - **log**: Indic, Southeast Asian, and African locale closeout is reflected in the validated tree; the cluster now has no unresolved surfaces.
  - **files edited/created**: `src/Humanizer/Locales/bn.yml`; `src/Humanizer/Locales/fil.yml`; `src/Humanizer/Locales/id.yml`; `src/Humanizer/Locales/ms.yml`; `src/Humanizer/Locales/ta.yml`; `src/Humanizer/Locales/th.yml`; `src/Humanizer/Locales/vi.yml`; `src/Humanizer/Locales/zu-ZA.yml`; `tests/Humanizer.Tests/Localisation/<culture>/*`

### T11: Complete East Asian Locale Families End-To-End
- **depends_on**: [T1, T3, T4, T5, T6]
- **location**: [ja.yml](E:/Dev/Humanizer/src/Humanizer/Locales/ja.yml), [ko.yml](E:/Dev/Humanizer/src/Humanizer/Locales/ko.yml), [zh-Hans.yml](E:/Dev/Humanizer/src/Humanizer/Locales/zh-Hans.yml), [zh-Hant.yml](E:/Dev/Humanizer/src/Humanizer/Locales/zh-Hant.yml), [zh-CN.yml](E:/Dev/Humanizer/src/Humanizer/Locales/zh-CN.yml), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Complete the East Asian locale families end-to-end. This includes the remaining ordinal and clock gaps, plus explicit child-locale proof for `zh-CN -> zh-Hans`.
- **validation**: Every locale in scope has an empty unresolved set, and child-locale inheritance has been reproved after parent changes.
  - **status**: Completed
  - **log**: East Asian locale closeout is reflected in the validated tree; `zh-CN -> zh-Hans` inheritance and the Japanese/Korean/CJK surfaces are all intentionally resolved.
  - **files edited/created**: `src/Humanizer/Locales/ja.yml`; `src/Humanizer/Locales/ko.yml`; `src/Humanizer/Locales/zh-Hans.yml`; `src/Humanizer/Locales/zh-Hant.yml`; `src/Humanizer/Locales/zh-CN.yml`; `tests/Humanizer.Tests/Localisation/<culture>/*`

### T12: Complete Baltic And East-European Locale Cluster End-To-End
- **depends_on**: [T1, T3, T4, T5, T6]
- **location**: [az.yml](E:/Dev/Humanizer/src/Humanizer/Locales/az.yml), [bg.yml](E:/Dev/Humanizer/src/Humanizer/Locales/bg.yml), [cs.yml](E:/Dev/Humanizer/src/Humanizer/Locales/cs.yml), [el.yml](E:/Dev/Humanizer/src/Humanizer/Locales/el.yml), [lt.yml](E:/Dev/Humanizer/src/Humanizer/Locales/lt.yml), [lv.yml](E:/Dev/Humanizer/src/Humanizer/Locales/lv.yml), [ro.yml](E:/Dev/Humanizer/src/Humanizer/Locales/ro.yml), [ru.yml](E:/Dev/Humanizer/src/Humanizer/Locales/ru.yml), [uk.yml](E:/Dev/Humanizer/src/Humanizer/Locales/uk.yml), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Complete the Baltic and East-European locales end-to-end, including date-ordinal and clock rollout plus any remaining numeric ordinal gaps such as `el`.
- **validation**: Every locale in scope has an empty unresolved set with exact-output proof for each newly introduced surface.
  - **status**: Completed
  - **log**: Baltic and East-European locale closeout is reflected in the validated tree; every locale in the cluster now has empty unresolved sets.
  - **files edited/created**: `src/Humanizer/Locales/az.yml`; `src/Humanizer/Locales/bg.yml`; `src/Humanizer/Locales/cs.yml`; `src/Humanizer/Locales/el.yml`; `src/Humanizer/Locales/lt.yml`; `src/Humanizer/Locales/lv.yml`; `src/Humanizer/Locales/ro.yml`; `src/Humanizer/Locales/ru.yml`; `src/Humanizer/Locales/uk.yml`; `tests/Humanizer.Tests/Localisation/<culture>/*`

### T13: Complete Remaining Central And Southern European Locales End-To-End
- **depends_on**: [T1, T3, T4, T5, T6]
- **location**: [fi.yml](E:/Dev/Humanizer/src/Humanizer/Locales/fi.yml), [hr.yml](E:/Dev/Humanizer/src/Humanizer/Locales/hr.yml), [hu.yml](E:/Dev/Humanizer/src/Humanizer/Locales/hu.yml), [hy.yml](E:/Dev/Humanizer/src/Humanizer/Locales/hy.yml), [is.yml](E:/Dev/Humanizer/src/Humanizer/Locales/is.yml), [it.yml](E:/Dev/Humanizer/src/Humanizer/Locales/it.yml), [nb.yml](E:/Dev/Humanizer/src/Humanizer/Locales/nb.yml), [pl.yml](E:/Dev/Humanizer/src/Humanizer/Locales/pl.yml), [sk.yml](E:/Dev/Humanizer/src/Humanizer/Locales/sk.yml), [sl.yml](E:/Dev/Humanizer/src/Humanizer/Locales/sl.yml), [sr.yml](E:/Dev/Humanizer/src/Humanizer/Locales/sr.yml), [sr-Latn.yml](E:/Dev/Humanizer/src/Humanizer/Locales/sr-Latn.yml), [sv.yml](E:/Dev/Humanizer/src/Humanizer/Locales/sv.yml), [tr.yml](E:/Dev/Humanizer/src/Humanizer/Locales/tr.yml), [uz-Cyrl-UZ.yml](E:/Dev/Humanizer/src/Humanizer/Locales/uz-Cyrl-UZ.yml), [uz-Latn-UZ.yml](E:/Dev/Humanizer/src/Humanizer/Locales/uz-Latn-UZ.yml), [tests/Humanizer.Tests/Localisation](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation)
- **description**: Complete the remaining central and southern European locale set end-to-end, covering date ordinals, clock notation, and any residual formatter or ordinal gaps.
- **validation**: Every locale in scope has an empty unresolved set with exact-output and sweep proof.
  - **status**: Completed
  - **log**: Central and southern European locale closeout is reflected in the validated tree; every locale in the cluster now has parity proof and no remaining unresolved surfaces.
  - **files edited/created**: `src/Humanizer/Locales/fi.yml`; `src/Humanizer/Locales/hr.yml`; `src/Humanizer/Locales/hu.yml`; `src/Humanizer/Locales/hy.yml`; `src/Humanizer/Locales/is.yml`; `src/Humanizer/Locales/it.yml`; `src/Humanizer/Locales/nb.yml`; `src/Humanizer/Locales/pl.yml`; `src/Humanizer/Locales/sk.yml`; `src/Humanizer/Locales/sl.yml`; `src/Humanizer/Locales/sr.yml`; `src/Humanizer/Locales/sr-Latn.yml`; `src/Humanizer/Locales/sv.yml`; `src/Humanizer/Locales/tr.yml`; `src/Humanizer/Locales/uz-Cyrl-UZ.yml`; `src/Humanizer/Locales/uz-Latn-UZ.yml`; `tests/Humanizer.Tests/Localisation/<culture>/*`

### T14: Reconcile The Global Parity Matrix And Close Remaining Locale Maps
- **depends_on**: [T7, T8, T9, T10, T11, T12, T13]
- **location**: [artifacts/2026-04-04-locale-feature-parity-matrix.md](E:/Dev/Humanizer/artifacts/2026-04-04-locale-feature-parity-matrix.md), [artifacts/2026-04-04-locale-linguistic-completeness-matrix.md](E:/Dev/Humanizer/artifacts/2026-04-04-locale-linguistic-completeness-matrix.md), `artifacts/*-parity-map.md`, [LocaleCoverageData.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs)
- **description**: Audit the finished locale tasks, update the global matrices to reflect the final ownership path and proof, and verify that each locale-owning task already closed its own parity maps and variant reprovals. This is a global reconciliation task, not a substitute owner for locale-level closeout.
- **validation**: The feature matrix has no `gap` cells, the completeness matrix marks every shipped locale complete, every parity-map artifact already has an empty unresolved set from its owning locale task, and no stale child-locale inheritance assertions remain.
  - **status**: Completed
  - **log**: Global parity reconciliation is complete; the top-level matrices and backlog now agree that all shipped locales are closed out.
  - **files edited/created**: `artifacts/2026-04-04-locale-feature-parity-matrix.md`; `artifacts/2026-04-04-locale-linguistic-completeness-matrix.md`; `artifacts/2026-04-04-locale-execution-backlog.md`

### T15: Update Contributor And User Docs For Full Locale Parity
- **depends_on**: [T14]
- **location**: [docs/adding-a-locale.md](E:/Dev/Humanizer/docs/adding-a-locale.md), [docs/locale-yaml-how-to.md](E:/Dev/Humanizer/docs/locale-yaml-how-to.md), [docs/locale-yaml-reference.md](E:/Dev/Humanizer/docs/locale-yaml-reference.md), [docs/localization.md](E:/Dev/Humanizer/docs/localization.md), [readme.md](E:/Dev/Humanizer/readme.md), [release_notes.md](E:/Dev/Humanizer/release_notes.md), [.github/CONTRIBUTING.md](E:/Dev/Humanizer/.github/CONTRIBUTING.md)
- **description**: Update the docs to reflect the final parity state, any new shared engines, any new authoring guidance, and the fact that all shipped locales now resolve all canonical surfaces intentionally.
- **validation**: The docs match the final implementation and no longer describe partial parity assumptions.
  - **status**: Completed
  - **log**: Documentation already reflects the completed parity state; no further truth-alignment edits were required in this pass.
  - **files edited/created**: none

### T16: Final Full Verification And CI Readiness
- **depends_on**: [T15]
- **location**: [tests/Humanizer.SourceGenerators.Tests](E:/Dev/Humanizer/tests/Humanizer.SourceGenerators.Tests), [tests/Humanizer.Tests](E:/Dev/Humanizer/tests/Humanizer.Tests), [src/Humanizer](E:/Dev/Humanizer/src/Humanizer)
- **description**: Run the full validation suite and confirm the repo is ready for CI. If hot shared kernels changed materially, run the benchmark matrix: formatter changes -> [FormatterBenchmarks.cs](E:/Dev/Humanizer/src/Benchmarks/FormatterBenchmarks.cs); clock changes -> [TimeOnlyToClockNotationConverterBenchmarks.cs](E:/Dev/Humanizer/src/Benchmarks/TimeOnlyToClockNotationConverterBenchmarks.cs); ordinal changes -> [OrdinalBenchmarks.cs](E:/Dev/Humanizer/src/Benchmarks/OrdinalBenchmarks.cs); words-to-number changes -> [WordsToNumberBenchmarks.cs](E:/Dev/Humanizer/src/Benchmarks/WordsToNumberBenchmarks.cs).
- **validation**: `dotnet test` passes for generator tests on `net10.0`, runtime tests on `net10.0` and `net8.0`, `dotnet pack` succeeds without warnings or errors, and any changed hot-path family has benchmark evidence recorded.
  - **status**: Completed
  - **log**: Final validation passed on `net10.0`, `net8.0`, and `dotnet pack` from the current tree.
  - **files edited/created**: none

## Parallel Execution Groups

| Wave | Tasks | Can Start When |
|------|-------|----------------|
| 1 | T1, T2, T3 | Immediately |
| 2 | T4, T5, T6 | T2 complete |
| 3 | T7, T8, T9, T10, T11, T12, T13 | T1, T3, and the required shared-engine tasks complete |
| 4 | T14 | All locale implementation waves complete |
| 5 | T15 | T14 complete |
| 6 | T16 | T15 complete |

## Validation

Validation completed successfully in the current tree:

- `dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0` passed.
- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0` passed.
- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0` passed.
- `dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/locale-parity-validation` passed.

## Risks & Mitigations

- Shared-engine overlap could create merge conflicts.
  Mitigation: keep T4, T5, and T6 focused on structural kernel work, then let locale workers consume those finished contracts.
- Native-term quality could drift if workers improvise.
  Mitigation: every locale task must follow the add-locale proposer/reviewer workflow and record the result in the parity map.
- Thin locales could hide ownership ambiguity.
  Mitigation: T7 resolves `en-US`, `nn`, and the English/Portuguese family inheritance cases before wider rollouts depend on them.
- Date and clock families are broad enough to tempt partial rollout.
  Mitigation: T14 does not allow closeout until the global matrices have zero unresolved cells.
