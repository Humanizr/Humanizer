# fn-1-locale-translation-parity-across-all.1 Add missing registry completeness tests and optional parity diagnostic

## Description
Add the 3 missing registry completeness test assertions to `LocaleTheoryMatrixCompletenessTests` so that every shipped locale is verified to be registered in ALL 8 registries. Currently only 5 are checked.

**Important:** `DateOnlyToOrdinalWordsConverterRegistry` and `TimeOnlyToClockNotationConvertersRegistry` are `#if NET6_0_OR_GREATER` guarded. The new test methods MUST be wrapped in the same conditional compilation guard to avoid build failures on lower TFMs.

**Size:** S (combine with early validation)
**Files:**
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs`

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
TBD

## Evidence
- Commits:
- Tests:
- PRs:
