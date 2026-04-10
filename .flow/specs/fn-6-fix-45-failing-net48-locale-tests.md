# Fix 45 failing net48 locale tests

## Overview

PR #1688 has 45 test failures on net48 (net8.0 and net10.0 pass). All failures stem from ICU vs NLS divergence — .NET Framework 4.8 uses Windows NLS for culture data while .NET 8/10 use ICU/CLDR. The tests expect the CLDR-correct values (which is what a native speaker would expect), but net48 produces different NLS values.

Three distinct failure categories:

| Category | Tests | Root Cause | Fix |
|----------|-------|------------|-----|
| Ordinalizer negative numbers | 40 | NLS returns U+002D (hyphen-minus), CLDR specifies U+2212 (minus sign) for fi, hr, nn, sv, sl, nb, lt, fa | Implement `negativeSign` YAML field, add overrides |
| Serbian date-to-ordinal-words | 3 | NLS returns Latin month names for `sr` culture, CLDR specifies Cyrillic | Add `calendar: months:` to sr.yml |
| lb-LU byte formatting | 2 | NLS returns space as group separator, CLDR specifies period | Implement `groupSeparator` YAML field, add override |

The native-speaker-correct values are the CLDR/ICU values in all three cases:
- **Finnish, Swedish, Norwegian, etc.**: CLDR specifies U+2212 (proper minus sign), which is the typographically correct character
- **Serbian**: Cyrillic is the constitutional official script; `sr` locale defaults to Cyrillic in CLDR
- **Luxembourgish**: Standard Continental European pattern uses period as thousands separator (9.765,6)

## Scope

**In scope:**
- Implement `negativeSign` and `groupSeparator` as new `number.formatting` YAML fields (currently "reserved but not yet implemented")
- Full pipeline: YAML schema validation → source generator → generated C# lookup tables → consumption in ordinalizer + ByteSize
- YAML overrides for 9 affected locale files + Serbian calendar months
- Wire `OrdinalizeExtensions` to use override `NumberFormatInfo`
- Documentation updates (locale-yaml-reference, locale-yaml-how-to, adding-a-locale)

**Out of scope:**
- `ByteSize.TryParse` round-trip consistency for negativeSign (no failing tests; track as follow-up)
- Auditing all 62 locales for additional negativeSign/groupSeparator divergences beyond the 8+1 currently failing
- Implementing remaining "future fields" (`digitSubstitution`, `percentSymbol`)
- sr-Latn locale (passes all tests — ICU and NLS both return Latin for sr-Latn)

## Approach

Follow the established YAML-first architecture: locale data is owned in YAML, source generators transform it to C# lookup tables at build time, runtime behavior is deterministic across all platforms.

The `number.formatting` surface already exists with `decimalSeparator` support (used by ar, ku, fr-CH). Extending it for `negativeSign` and `groupSeparator` follows the exact same pattern:
1. Validator (`CanonicalLocaleAuthoring.cs:225`) accepts new field names
2. Resolver/catalog carries new properties on `ResolvedLocaleDefinition`
3. Emitter (`LocaleRegistryInput.cs:150`) generates per-field override dictionaries + extends `GetCachedNumberFormat`
4. Consumers (`OrdinalizeExtensions`, `ByteSize`, `MetricNumeralExtensions`) use the override NFI

## Quick commands

```bash
# Verify all 45 previously-failing tests now pass on net48 (Windows CI)
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net48 --filter "FullyQualifiedName~Ordinalizer_ExactLocales_UseExpectedNegativeFallbackForms|FullyQualifiedName~DateToOrdinalWords|FullyQualifiedName~UsesExpectedAdditionalByteHumanizeOutputs"

# Verify no regressions on net10.0 and net8.0
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0

# Verify source generator tests still pass
dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj

# Build + pack (no warnings)
dotnet build src/Humanizer/Humanizer.csproj -c Release
```

## Key design decisions

1. **`GetCachedNumberFormat` refactor**: Change from `GetCachedNumberFormat(CultureInfo, string decimalSep)` to an internal method that looks up ALL applicable overrides (decimal, negative sign, group separator) from per-field dictionaries. This simplifies all consumer call sites and ensures the cached NFI always has all overrides applied.

2. **Ordinalizer wiring**: Fix in `OrdinalizeExtensions` at the `number.ToString(culture)` call sites (lines 147, 164, 206, 226). Change to `number.ToString(overrideNFI)`. This is the same pattern ByteSize already uses.

3. **YAML authoring**: Use literal U+2212 character in single-quoted YAML strings (e.g., `negativeSign: '−'`). The UTF-8 bytes are stored in the file directly, consistent with how Cyrillic and other non-ASCII characters are already stored.

4. **`nn` inheritance**: `nn.yml` uses `variantOf: 'nb'`. Verify that the source generator's inheritance mechanism propagates `number.formatting` fields from parent to child. If it does, only `nb.yml` needs the override. If not, add to both.

## Acceptance

- [ ] All 45 previously-failing net48 tests pass
- [ ] Zero regressions on net10.0 and net8.0 test suites
- [ ] Source generator tests pass (update snapshots if needed)
- [ ] `negativeSign` and `groupSeparator` removed from "future fields" list in docs
- [ ] New fields documented in locale-yaml-reference.md with examples
- [ ] Build produces zero warnings

## Early proof point

Task fn-6-fix-45-failing-net48-locale-tests.1 (Serbian calendar months) validates that the existing calendar override pipeline works for `sr.yml` with zero code changes. If it fails, the calendar override mechanism needs debugging before attempting the more complex number.formatting work.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1 | 3 Serbian date tests pass on net48 | fn-6.1 | — |
| R2 | 40 ordinalizer negative tests pass on net48 | fn-6.2, fn-6.3 | — |
| R3 | 2 lb-LU byte formatting tests pass on net48 | fn-6.2, fn-6.3 | — |
| R4 | Zero regressions on net10.0 + net8.0 | fn-6.1, fn-6.2, fn-6.3 | — |
| R5 | Source generator tests pass | fn-6.2 | — |
| R6 | Documentation updated | fn-6.4 | — |
| R7 | ByteSize.TryParse round-trip for negativeSign | — | Follow-up: no failing tests currently |
