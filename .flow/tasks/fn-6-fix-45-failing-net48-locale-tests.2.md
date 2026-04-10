# fn-6-fix-45-failing-net48-locale-tests.2 Extend source generator for negativeSign + groupSeparator fields

## Description
Extend the `number.formatting` YAML surface to support two new fields: `negativeSign` and `groupSeparator`. These are currently documented as "reserved but not yet implemented" future fields. The existing `decimalSeparator` implementation is the template — extend the validator, resolver, emitter, and `GetCachedNumberFormat` to handle all three fields.

**Size:** M
**Files:**
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` (validator)
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` (emitter)
- `src/Humanizer.SourceGenerators/Common/ResolvedLocaleDefinition.cs` (or equivalent property carrier)
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs` (resolver — if it mediates number.formatting fields)
- `src/Humanizer/Bytes/ByteSize.cs` (update call sites if GetCachedNumberFormat API changes)
- `src/Humanizer/MetricNumeralExtensions.cs` (update call sites if API changes)

## Approach

1. **Validator** (`CanonicalLocaleAuthoring.cs:225-242`): `ValidateNumberFormattingBlock` currently rejects any property other than `decimalSeparator`. Add `negativeSign` and `groupSeparator` to the allowed set.

2. **Resolver**: Ensure the new YAML fields are carried on `ResolvedLocaleDefinition` (or equivalent) so the emitter can read them. Follow the same pattern as `decimalSeparator`.

3. **Emitter** (`LocaleRegistryInput.cs:150-234`): `EmitNumberFormattingOverrides` currently generates:
   - A `DecimalSeparatorOverrides` dictionary
   - A `TryGetDecimalSeparator` method
   - A `GetCachedNumberFormat` method that clones NFI and sets `NumberDecimalSeparator`

   Extend to also generate:
   - `NegativeSignOverrides` dictionary + `TryGetNegativeSign` method
   - `GroupSeparatorOverrides` dictionary + `TryGetGroupSeparator` method
   - Refactor `GetCachedNumberFormat` to apply ALL applicable overrides (decimal separator, negative sign, group separator) when creating the cached NFI

4. **GetCachedNumberFormat refactor**: The current method signature is `GetCachedNumberFormat(CultureInfo culture, string decimalSeparator)`. Refactor so the method internally looks up all applicable overrides from the three dictionaries. Consumer call sites should simplify to `GetCachedNumberFormat(CultureInfo culture)` — the method itself decides which overrides to apply.

5. **Update existing call sites**: `ByteSize.cs` (lines 210, 245, 432) and `MetricNumeralExtensions.cs` (line 370) currently call `GetCachedNumberFormat` with explicit decimal separator. Update to use the new simplified API.

## Investigation targets

**Required** (read before coding):
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:225-242` — current validator
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs:150-234` — current emitter
- `src/Humanizer/Bytes/ByteSize.cs:207-210,242-245,429-432` — current GetCachedNumberFormat call sites
- `src/Humanizer/MetricNumeralExtensions.cs:370` — metric numerals call site

**Optional** (reference as needed):
- `src/Humanizer/Locales/fr-CH.yml:32-33` — existing decimalSeparator override (reference pattern)
- `src/Humanizer/Locales/ar.yml` — another locale with decimalSeparator override
- `tests/Humanizer.SourceGenerators.Tests/` — snapshot tests that may need updating

## Key context

- The generated `LocaleNumberFormattingOverrides.g.cs` is compiled into the main library (not shipped separately)
- The cache uses `ConcurrentDictionary<string, NumberFormatInfo>` keyed by `culture.Name`
- When a caller supplies their own `IFormatProvider`/`NumberFormatInfo`, the override is NOT applied — this is by design and must be preserved
- Source generator incremental pipeline: changes to the emitter may require updating snapshot/golden-file tests in `Humanizer.SourceGenerators.Tests`

## Acceptance

- [ ] `ValidateNumberFormattingBlock` accepts `negativeSign` and `groupSeparator` without error
- [ ] Source generator emits `NegativeSignOverrides` and `GroupSeparatorOverrides` dictionaries
- [ ] `GetCachedNumberFormat` applies all three override types (decimal, negative sign, group separator)
- [ ] Existing ByteSize and MetricNumerals call sites work with refactored API
- [ ] All existing tests pass (no regressions) — the new fields have no YAML data yet, so overrides are empty
- [ ] Source generator snapshot tests updated if needed
- [ ] Build produces zero warnings
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
