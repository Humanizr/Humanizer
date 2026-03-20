# Plan: Verify Default Sufficiency And Complete Locale Feature Coverage

**Generated**: 2026-03-17  
**Completed**: 2026-03-19

## Final Status
- [x] T1 Build authoritative locale-by-registry matrix
- [x] T2 Add shared verification infrastructure
- [x] T3-T56 Locale and non-resource culture tasks
- [x] T57-T64 Registry integration tasks
- [x] T65 Final cross-locale and child-culture sweep
- [x] T66 Full verification

## Completion Summary
This plan is complete. The branch now closes the locale feature matrix across the remaining formatter, ordinalizer, date-to-ordinal-words, date-only-to-ordinal-words, time-only clock-notation, and words-to-number work without restoring historical review artifacts into the branch.

Locale task closure was finished through the recovered registry implementations plus explicit fallback coverage for child cultures and supported non-resource cultures. The final sweep now proves parent-culture reuse and accepted default behavior for `de-CH`, `de-LI`, `fr-CH`, `zh-HK`, `ta`, `nn`, and `en-IN`, alongside the existing locale-specific number and formatter coverage already on the branch.

Registry integration is complete for `CollectionFormatterRegistry`, `FormatterRegistry`, `NumberToWordsConverterRegistry`, `OrdinalizerRegistry`, `DateToOrdinalWordsConverterRegistry`, `DateOnlyToOrdinalWordsConverterRegistry`, `TimeOnlyToClockNotationConvertersRegistry`, and `WordsToNumberConverterRegistry`. The saved matrix and verification tests now align with the implemented registry state.

## Evidence
- Matrix and shared verification: [2026-03-17-locale-feature-matrix.json](E:/Dev/Humanizer/docs/plans/2026-03-17-locale-feature-matrix.json), [LocalisationFeatureVerificationTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/LocalisationFeatureVerificationTests.cs)
- Formatter and fallback sweep: [FormatterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/FormatterRegistry.cs), [DefaultFormatterTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/DefaultFormatterTests.cs), [LocaleFallbackSweepTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs)
- Ordinalizer recovery: [OrdinalizerRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/OrdinalizerRegistry.cs), [OrdinalizerRegistryRecoveryTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/OrdinalizerRegistryRecoveryTests.cs), [Localisation/Ordinalizers](E:/Dev/Humanizer/src/Humanizer/Localisation/Ordinalizers)
- Date/date-only/time recovery: [DateToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs), [DateOnlyToOrdinalWordsConverterRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs), [TimeOnlyToClockNotationConvertersRegistry.cs](E:/Dev/Humanizer/src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs), [ExactLocaleDateAndTimeRegistryTests.cs](E:/Dev/Humanizer/tests/Humanizer.Tests/Localisation/ExactLocaleDateAndTimeRegistryTests.cs)
- Words-to-number compatibility/build follow-through: [GermanWordsToNumberConverter.cs](E:/Dev/Humanizer/src/Humanizer/Localisation/WordsToNumber/GermanWordsToNumberConverter.cs), [DutchWordsToNumberConverter.cs](E:/Dev/Humanizer/src/Humanizer/Localisation/WordsToNumber/DutchWordsToNumberConverter.cs)

## Verification
- [x] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0`
- [x] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`
- [x] `dotnet build Humanizer/Humanizer.csproj -c Release /t:PackNuSpecs /p:PackageOutputPath=E:\Dev\Humanizer\artifacts\locale-packages` from `E:\Dev\Humanizer\src`
- [x] `pwsh -File tests/verify-packages.ps1 -PackageVersion 3.1.0-dev.227.gafa99b11ad -PackagesDirectory E:\Dev\Humanizer\artifacts\locale-packages`

## Notes
- The release build initially failed on `net48`/`netstandard2.0` because `GermanWordsToNumberConverter` and `DutchWordsToNumberConverter` used `string.Replace` overloads unavailable on those targets. That compatibility issue was fixed before rerunning the final package gates.
- `tests/verify-packages.ps1` completed successfully. It logged warnings only for missing SDK 8 and SDK 9 on the machine and still passed all discovered restore/build targets plus package smoke tests.
