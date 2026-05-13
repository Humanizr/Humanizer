# fn-6-fix-45-failing-net48-locale-tests.1 Add Cyrillic calendar months to sr.yml (3 tests)

## Description
Add a `calendar: months:` section to `sr.yml` with the 12 Cyrillic nominative month names, so that `DateToOrdinalWords` produces Cyrillic output on all platforms instead of falling through to `DateTime.ToString()` which returns Latin names on net48/NLS.

**Size:** S
**Files:** `src/Humanizer/Locales/sr.yml`

## Approach

- Follow the exact pattern already used by bn, fa, he, ku, zu-ZA, ta (6 locales already have `calendar: months:` overrides)
- Add only `months:` (nominative), NOT `monthsGenitive:` — test expectations use nominative forms ("фебруар" not "фебруара")
- Use the 12 standard Cyrillic month names: јануар, фебруар, март, април, мај, јун, јул, август, септембар, октобар, новембар, децембар
- The source generator already handles this field — `OrdinalDateProfileCatalogInput.cs:33-36` extracts `months` from the YAML `calendar` block and passes them to `OrdinalDatePattern`
- `OrdinalDatePattern.SubstituteMonth` (line 78-84) replaces `MMMM` with the literal month name from the array, bypassing culture-dependent DateTime formatting

## Investigation targets

**Required** (read before coding):
- `src/Humanizer/Locales/fa.yml:415-428` — reference pattern for calendar months override
- `src/Humanizer/Locales/sr.yml:474-481` — current ordinal.date section (has pattern but no calendar block)
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:78-84` — SubstituteMonth logic

**Optional** (reference as needed):
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs:33-36` — ExtractCalendarMonths
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:74,141,208` — expected values for sr

## Key context

- sr-Latn is NOT affected (passes all tests, uses Latin on both ICU and NLS)
- The `sr` culture defaults to Cyrillic script (sr-Cyrl-RS) on both NLS and ICU per CLDR
- On net48/NLS, `DateTime.ToString("MMMM", new CultureInfo("sr"))` returns Latin month names despite the culture resolving to sr-Cyrl-RS — this is a known NLS quirk
- The calendar override completely bypasses this issue by substituting the month name before DateTime.ToString is called

## Acceptance

- [ ] 3 Serbian DateToOrdinalWords tests pass on net48: DateToOrdinalWords_2015February3 (sr), DateToOrdinalWords_2022January25 (sr), DateToOrdinalWords_2015January1 (sr)
- [ ] Zero regressions on net10.0 and net8.0 for all sr and sr-Latn tests
- [ ] sr.yml has exactly 12 months in calendar block, all Cyrillic nominative forms
## Done summary
Added Cyrillic calendar month overrides to sr.yml with 12 nominative month names, enabling DateToOrdinalWords to produce Cyrillic output on all platforms including net48/NLS. All 38,908 tests pass on both net10.0 and net8.0 with zero regressions.
## Evidence
- Commits: 86096be5e52d0166563d7c7f4c9cff318a7b32f6
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (38908 passed), dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 (38908 passed), dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj (58 passed), dotnet build src/Humanizer/Humanizer.csproj -c Release (0 warnings)
- PRs: