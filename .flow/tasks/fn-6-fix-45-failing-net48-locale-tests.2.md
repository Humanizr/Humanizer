# fn-6-fix-45-failing-net48-locale-tests.2 Extend source generator for negativeSign + groupSeparator fields

## Description
Extend the `number.formatting` YAML surface to support two new fields: `negativeSign` and `groupSeparator`. These are currently documented as "reserved but not yet implemented" future fields. The existing `decimalSeparator` implementation is the template — extend the validator, emitter, and add a new `GetFormattingNumberFormat` method that applies all overrides for formatting paths while leaving the existing parse-path API unchanged.

The resolved model (`ResolvedLocaleDefinition` in `LocaleYamlCatalog.cs:949`) may already carry YAML keys generically; only add new properties if they are not already flowing through to the emitter.

**Size:** M
**Files:**
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` (validator)
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` (emitter)
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs` (resolver — only if new keys are not already flowing through on `ResolvedLocaleDefinition`)
- `src/Humanizer/Bytes/ByteSize.cs` (update formatting call sites in `ToString(IFormatProvider?)` and `ToString(string?, IFormatProvider?)` to use new API)
- `src/Humanizer/MetricNumeralExtensions.cs` (update formatting call sites if API changes)

## Approach

1. **Validator** (`CanonicalLocaleAuthoring.cs:225-242`): `ValidateNumberFormattingBlock` currently rejects any property other than `decimalSeparator`. Add `negativeSign` and `groupSeparator` to the allowed set.

2. **Resolver**: Check whether `ResolvedLocaleDefinition` (declared in `LocaleYamlCatalog.cs:949`) already carries all YAML keys generically. If `negativeSign` and `groupSeparator` are already flowing through to the emitter, no resolver changes are needed. Only add properties if compile-time evidence shows the new keys are not available to the emitter.

3. **Emitter** (`LocaleRegistryInput.cs:150-234`): `EmitNumberFormattingOverrides` currently generates:
   - A `DecimalSeparatorOverrides` dictionary
   - A `TryGetDecimalSeparator` method
   - A `GetCachedNumberFormat` method that clones NFI and sets `NumberDecimalSeparator`

   Extend to also generate:
   - `NegativeSignOverrides` dictionary + `TryGetNegativeSign` method
   - `GroupSeparatorOverrides` dictionary + `TryGetGroupSeparator` method

4. **New `GetFormattingNumberFormat(CultureInfo)` method** (generated): Looks up ALL applicable overrides (decimal separator, negative sign, group separator) from the three dictionaries. Fast path: when no override exists for a culture, returns `culture.NumberFormat` directly without cloning or caching. Only clone/cache when at least one override applies. Uses a distinct cache key (e.g., `"fmt:{cultureName}"`) to avoid conflicts with the existing parse-path cache.

5. **Preserve existing `GetCachedNumberFormat(CultureInfo, string decimalSep)`**: This method remains unchanged. It is used by `ByteSize.TryParse` (line 432) to apply only the decimal separator override in parse paths. Do NOT modify or remove it.

6. **Update formatting call sites**: `ByteSize.ToString(IFormatProvider?)` (line 210) and `ByteSize.ToString(string?, IFormatProvider?)` (line 245) currently call `TryGetDecimalSeparator` + `GetCachedNumberFormat`. Replace with the new `GetFormattingNumberFormat(culture)`. Also update `MetricNumeralExtensions.cs` (line 370). **Do NOT change `ByteSize.TryParse` (line 430-432)** — it stays on the existing decimal-only API.

## Investigation targets

**Required** (read before coding):
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:225-242` — current validator
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs:150-234` — current emitter
- `src/Humanizer/Bytes/ByteSize.cs:200-214` — `ToString(IFormatProvider?)` formatting path
- `src/Humanizer/Bytes/ByteSize.cs:222-246` — `ToString(string?, IFormatProvider?)` formatting path
- `src/Humanizer/Bytes/ByteSize.cs:418-432` — `TryParse` parse path (must NOT change)
- `src/Humanizer/MetricNumeralExtensions.cs:370` — metric numerals formatting call site
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs:949` — `ResolvedLocaleDefinition` declaration

**Optional** (reference as needed):
- `src/Humanizer/Locales/fr-CH.yml:32-33` — existing decimalSeparator override (reference pattern)
- `tests/Humanizer.SourceGenerators.Tests/` — snapshot tests that will need updating for new dictionaries/methods

## Key context

- The generated `LocaleNumberFormattingOverrides.g.cs` is compiled into the main library (not shipped separately)
- The cache uses `ConcurrentDictionary<string, NumberFormatInfo>` keyed by `culture.Name`; the new formatting method must use a distinct key prefix to avoid cache conflicts with the parse-path method
- When a caller supplies their own `IFormatProvider`/`NumberFormatInfo`, the override is NOT applied — this is by design and must be preserved
- Source generator incremental pipeline: changes to the emitter will require updating snapshot/golden-file tests in `Humanizer.SourceGenerators.Tests`

## Acceptance

- [ ] `ValidateNumberFormattingBlock` accepts `negativeSign` and `groupSeparator` without error
- [ ] Source generator emits `NegativeSignOverrides` and `GroupSeparatorOverrides` dictionaries
- [ ] New `GetFormattingNumberFormat(CultureInfo)` method generated, applies all three override types
- [ ] `GetFormattingNumberFormat` returns `culture.NumberFormat` unchanged when no overrides apply (fast path — no clone, no cache entry)
- [ ] Existing `GetCachedNumberFormat(CultureInfo, string)` remains unchanged for parse paths
- [ ] `ByteSize.ToString(...)` formatting paths updated to use new `GetFormattingNumberFormat`
- [ ] `ByteSize.TryParse` (line 430-432) NOT modified — continues using existing decimal-only API
- [ ] MetricNumeralExtensions formatting path updated
- [ ] All existing tests pass (no regressions) — existing decimal-separator overrides continue to work; new `NegativeSignOverrides`/`GroupSeparatorOverrides` are empty until fn-6.3 adds YAML data
- [ ] Source generator snapshot tests updated for new dictionary/method shapes
- [ ] Build produces zero warnings
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
