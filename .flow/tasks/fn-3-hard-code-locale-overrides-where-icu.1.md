# fn-3-hard-code-locale-overrides-where-icu.1 Delete pure ICU-snapshot tests with zero Humanizer coverage

## Description
Delete the 12 test methods and their theory data that pure-snapshot ICU culture formatting without exercising any Humanizer code. These tests call `date.ToString("d"/"D", culture)` and `time.ToString("t"/"T", culture)` directly and assert against hardcoded expected strings — no Humanizer converter is invoked.

**Size:** S
**Files:**
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs`

## Approach
- Remove the 12 test methods from `LocaleRegistrySweepTests.cs`:
  - `DateFormatting_ShortDate_2015January1_UsesExpectedCultureString`
  - `DateFormatting_ShortDate_2015February3_UsesExpectedCultureString`
  - `DateFormatting_ShortDate_2022January25_UsesExpectedCultureString`
  - `DateFormatting_LongDate_2015January1_UsesExpectedCultureString`
  - `DateFormatting_LongDate_2015February3_UsesExpectedCultureString`
  - `DateFormatting_LongDate_2022January25_UsesExpectedCultureString`
  - `TimeFormatting_ShortTime_0105_UsesExpectedCultureString`
  - `TimeFormatting_ShortTime_1323_UsesExpectedCultureString`
  - `TimeFormatting_ShortTime_1325_UsesExpectedCultureString`
  - `TimeFormatting_LongTime_0105_UsesExpectedCultureString`
  - `TimeFormatting_LongTime_1323_UsesExpectedCultureString`
  - `TimeFormatting_LongTime_1325_UsesExpectedCultureString`
- Remove the corresponding `TheoryData<string, DateExpectationRow>` / `TheoryData<string, ClockExpectationRow>` properties from `LocaleCoverageData.cs` (Date[Short|Long]Date2015January1/February3/2022January25ReferenceTheoryData and TimeOnly[Short|Long]Time0105/1323/1325ReferenceTheoryData)
- Remove the matching `LocaleCoverageData_*ReferenceTheoryData_IncludeLocale` matrix completeness assertions from `LocaleTheoryMatrixCompletenessTests.cs`
- Run `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0` to confirm the test count decreased and no unrelated tests broke

## Investigation targets
**Required:**
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:155-200,293-347` — the 12 test methods
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:535` and surrounding — the theory data properties
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs:190-280` — matrix completeness assertions

## Key context
- Rationale: these tests call `.NET BCL` methods directly and compare against hardcoded strings. Zero Humanizer code is exercised.
- Ordinal-date tests (`DateToOrdinalWords_*`) stay — they exercise the Humanizer pattern converter and will be fixed by later tasks in this epic.
- `ClockExpectationRow` and `DateExpectationRow` structs stay — they are still used by the ordinal date tests.
## Acceptance
- [ ] 12 test methods removed from `LocaleRegistrySweepTests.cs`
- [ ] 12 theory data properties removed from `LocaleCoverageData.cs`
- [ ] 12 matrix completeness assertions removed from `LocaleTheoryMatrixCompletenessTests.cs`
- [ ] Test count on net10.0 reduced by ~744 tests
- [ ] No newly-failing tests
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` passes
## Done summary
Deleted 12 pure ICU-snapshot test methods, 12 corresponding TheoryData properties, 12 matrix completeness assertions, and 12 now-unused ZhHantReference constants. These tests called .NET BCL date/time formatting directly without exercising any Humanizer code. Net reduction: 1,488 tests (40,396 to 38,908) and 93 fewer ICU-drift failures (122 to 29).
## Evidence
- Commits: 8e8c71561ea2eaba68d563294cc94cc62136558f
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 -c Release, dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 -c Release, dotnet format Humanizer.slnx --verify-no-changes
- PRs: