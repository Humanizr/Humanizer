# fn-1-locale-translation-parity-across-all.1 Add missing registry completeness tests and optional parity diagnostic

## Description
Add 3 missing registry completeness tests to `LocaleTheoryMatrixCompletenessTests` so every shipped locale is verified as registered in ALL 8 registries. Currently only 5 are checked.

**Important:** `DateOnlyToOrdinalWordsConverterRegistry` and `TimeOnlyToClockNotationConvertersRegistry` are `#if NET6_0_OR_GREATER` guarded. New test methods MUST use the same guard.

**Size:** S
**Files:**
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs`

## Approach

Follow the existing pattern at `LocaleTheoryMatrixCompletenessTests.cs:356-377` for the 5 existing registry assertions. Add 3 new `[Theory]` methods for `DateToOrdinalWordsConverterRegistry`, `DateOnlyToOrdinalWordsConverterRegistry`, and `TimeOnlyToClockNotationConvertersRegistry`. Wrap DateOnly and TimeOnly tests in `#if NET6_0_OR_GREATER`.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs:356-377` — existing registry assertion pattern
- `src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs`
- `src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs:1` — `#if NET6_0_OR_GREATER` guard

**Optional:**
- `src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs`

## Key context

This task runs AFTER all locale batches (.2-.9) so that every shipped locale is registered. The tests will fail if any shipped locale is missing from a registry.
## Approach

- Follow the existing pattern at `LocaleTheoryMatrixCompletenessTests.cs:356-377` for the 5 existing registry assertions
- Add 3 new `[Theory]` test methods for `DateToOrdinalWordsConverterRegistry`, `DateOnlyToOrdinalWordsConverterRegistry`, and `TimeOnlyToClockNotationConvertersRegistry`
- Wrap the DateOnly and TimeOnly registry tests in `#if NET6_0_OR_GREATER` / `#endif`
- Use the same `GetRegisteredLocales<TRegistry, TLocaliser>()` helper

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs:356-377` — existing registry assertion pattern
- `src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs` — registry class to reference
- `src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs:1` — `#if NET6_0_OR_GREATER` guard

**Optional:**
- `src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs` — also NET6_0_OR_GREATER guarded
## Approach

- Follow the existing pattern at `LocaleTheoryMatrixCompletenessTests.cs:356-377` for the 5 existing registry assertions
- Add 3 new `[Theory]` test methods for `DateToOrdinalWordsConverterRegistry`, `DateOnlyToOrdinalWordsConverterRegistry`, and `TimeOnlyToClockNotationConvertersRegistry`
- Use the same `GetRegisteredLocales<TRegistry, TLocaliser>()` helper
- For HSG004: Follow the diagnostic pattern at `LocaleRegistryInput.cs` (HSG001-HSG003). Add validation after `LocaleCatalogInput.Create()` that checks each resolved locale has all 7 canonical surfaces.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs:356-377` — existing registry assertion pattern
- `src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs` — registry class to reference
- `src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs` — registry class to reference

**Optional:**
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` — diagnostic descriptor pattern
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` — SupportedSurfaceNames list
## Acceptance
- [ ] `DateToOrdinalWordsConverterRegistryCoversYamlLocale` test exists and asserts all shipped locales
- [ ] `DateOnlyToOrdinalWordsConverterRegistryCoversYamlLocale` test exists, guarded with `#if NET6_0_OR_GREATER`
- [ ] `TimeOnlyToClockNotationConvertersRegistryCoversYamlLocale` test exists, guarded with `#if NET6_0_OR_GREATER`
- [ ] Tests compile on all target frameworks (net10.0, net8.0)
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds
## Done summary
Added 3 missing registry completeness tests to LocaleTheoryMatrixCompletenessTests: DateToOrdinalWordsConverterRegistryCoversYamlLocale, DateOnlyToOrdinalWordsConverterRegistryCoversYamlLocale (#if NET6_0_OR_GREATER), and TimeOnlyToClockNotationConvertersRegistryCoversYamlLocale (#if NET6_0_OR_GREATER). All 8 registries are now covered by completeness tests ensuring every shipped locale is registered.
## Evidence
- Commits: dbac93b34ac719aef036fe6197748dfe5abbd546
- Tests: dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0, dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (40274 passed, 122 pre-existing failures), dotnet build src/Humanizer/Humanizer.csproj -c Release
- PRs: