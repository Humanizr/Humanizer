# fn-3-hard-code-locale-overrides-where-icu.4 Add number.formatting: sub-block with decimalSeparator and thread through ByteSize

## Description
Add a new `number.formatting:` sub-block under the existing `number:` canonical surface, with an initial `decimalSeparator` field. Thread it through the two runtime consumers that currently call `NumberFormatInfo.NumberDecimalSeparator`: `ByteSize.ToString` and `MetricNumeralExtensions`. Populate the override for `ar`, `ku`, and `fr-CH`.

**Title clarification:** Despite the original task title referencing `number.words.decimalSeparator`, the implementation places the field in a new `number.formatting:` sub-block (decided in planning). `number.words` is about spelling numbers as words ("three point one four"); `number.parse` is about parsing any form of input; `number.formatting` is about rendering numeric literals ("3.14"). Symmetric, self-explanatory, leaves room for `groupSeparator`, `digitSubstitution`, etc.

**Size:** S
**Files:**
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs` — accept `number.formatting` as a third sub-key alongside `number.words` and `number.parse`
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` — closed-set enforcement
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs` — register `number.formatting.decimalSeparator` member
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` — emit `LocaleNumberFormattingOverrides.g.cs`
- `src/Humanizer/Bytes/ByteSize.cs` (lines ~230-274, ~413-425) — consult override before `double.ToString(format, provider)`
- `src/Humanizer/MetricNumeralExtensions.cs` (line ~370) — consult override instead of direct `CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator` read
- `src/Humanizer/Locales/ar.yml`, `ku.yml`, `fr-CH.yml` — add `number.formatting.decimalSeparator`

## Approach

### Schema: new `number.formatting:` sub-block

`number:` currently accepts `number.words` and `number.parse` as sub-keys. Extend the validator to accept `number.formatting` as a third sub-key with these initial fields:
- `decimalSeparator` — string (single character today, but schema should not force length-1 in case a locale needs a multi-code-point separator)
- Future: `groupSeparator`, `digitSubstitution`, `percentSymbol`, `negativeSign`

Update `LocaleYamlCatalog.cs:244-279` and `CanonicalLocaleAuthoring.cs` together (closed-set validation lives in both).

Inheritance: `number.formatting` sub-keys merge recursively via `variantOf` (consistent with existing `number.words` merge rules).

### Generator pipeline plumbing (critical — data must survive to emission)

The `number.formatting` sub-block is nested under `number:`, but `AddNumberFeatures()` (line 186-211) currently only accepts `words` and `parse` — it **rejects** any other sub-key with an error. **Both the lowering and resolution must be updated.**

1. **`CanonicalLocaleAuthoring.AddNumberFeatures()` (line 186-211) — accept `formatting` as a third sub-key:**
   - Currently the validation check (line 193) is `property is not ("words" or "parse")` — extend to `property is not ("words" or "parse" or "formatting")`
   - Add: `if (numberSurface.TryGetValue("formatting", out var fmtValue) && fmtValue is SimpleYamlMapping fmtMapping) features["numberFormatting"] = fmtMapping;`
   - This lowers `number.formatting` into a `"numberFormatting"` feature bucket

2. **`LocaleYamlCatalog.cs` — add `"numberFormatting"` to `SupportedFeatureNames` (line 59-100):**
   - Ensures the `numberFormatting` feature survives `variantOf` resolution in `ResolveLocale`

3. **`LocaleYamlCatalog.ResolveLocale()` — extract resolved number formatting:**
   - After the feature merge loop, add a `TryResolveLocalePart` call with a `ResolveNumberFormatting` method that reads `"numberFormatting"` from the resolved feature map
   - Pass the result to `ResolvedLocaleDefinition`

4. **`ResolvedLocaleDefinition` (line 893-924) — add `NumberFormatting` property:**
   - Add `SimpleYamlMapping? numberFormatting` parameter and `NumberFormatting` property
   - Update `Empty()` factory to include the new `null` parameter

5. **`LocaleRegistryInput` — emit `LocaleNumberFormattingOverrides.g.cs`:**
   - Add a new emission pass that iterates `locales`, checks `locale.NumberFormatting` for `decimalSeparator`, and emits the override registry with `CultureInfo.Parent` fallback

### Source-gen: `LocaleNumberFormattingOverrides` registry

Generate a `LocaleNumberFormattingOverrides.g.cs` file with a single static class exposing:
```csharp
internal static bool TryGetDecimalSeparator(CultureInfo culture, out string? decimalSeparator);
```

**Resolution model:** The generated registry is populated from **resolved locale data** (after `variantOf` inheritance is applied by the source generator). This means if `ar` authors `decimalSeparator: "."`, and a hypothetical `ar-EG` inherits via `variantOf: ar` without overriding, the registry contains entries for both `ar` and `ar-EG`.

**Runtime fallback:** The `TryGetDecimalSeparator` implementation must walk `CultureInfo.Parent` (same semantics as `LocaliserRegistry.FindLocaliser` in `LocaliserRegistry.cs:108`). This ensures that arbitrary cultures not in the generated dictionary (e.g., a user-created `ar-EG` CultureInfo that doesn't have a Humanizer YAML file) still fall back to the `ar` override. This matches how all other Humanizer locale resolution works — `variantOf` handles Humanizer-authored child locales at generator time, and `CultureInfo.Parent` handles runtime fallback for unlisted cultures.

### Runtime wiring

**Critical contract: preserve caller-supplied providers.** The override mechanism ONLY applies when the formatting path uses a culture-backed `IFormatProvider`. If the caller explicitly passes a custom `NumberFormatInfo` or `IFormatProvider` that is not a `CultureInfo`, it must be used as-is — overriding it would be a behavior regression.

**`ByteSize.ToString`** — before calling `double.ToString(format, provider)`, when `provider` is a `CultureInfo`:

```csharp
// Only inject override for culture-backed providers, never for caller-supplied custom NFI
if (provider is CultureInfo culture 
    && LocaleNumberFormattingOverrides.TryGetDecimalSeparator(culture, out var sep))
{
    provider = LocaleNumberFormattingOverrides.GetCachedNumberFormat(culture, sep);
}
// If provider is a custom NumberFormatInfo or non-CultureInfo IFormatProvider, use as-is
```

The `GetCachedNumberFormat` helper returns a cached `NumberFormatInfo` per culture name (e.g., `ConcurrentDictionary<string, NumberFormatInfo>`). Zero allocation in the common no-override path. One-time allocation per override culture on first use — no per-call cloning.

**`MetricNumeralExtensions`** — same pattern; the existing direct `CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator` read becomes an override-aware helper call. Since this path always uses `CultureInfo.CurrentCulture` (not a caller-supplied provider), the override always applies.

**Shared helper**: `internal static class LocaleNumberFormattingOverrides` in the generated code. Both consumers use `TryGetDecimalSeparator` + `GetCachedNumberFormat`. The `TryGetDecimalSeparator` implementation walks `CultureInfo.Parent` (matching `LocaliserRegistry.FindLocaliser` semantics) so that unlisted child cultures fall back to the parent override.

### Parse-side symmetry

`ByteSize.FromString` also builds `specialCharsSet` from `nfi.NumberDecimalSeparator` etc. Verify whether parse should also respect the override — a user parsing `"1.95 KB"` in the `ar` culture (which now overrides to `.`) should succeed. Update the parse path to use the same override-aware helper.

### Per-locale YAML

Add to the 3 affected locales:

```yaml
number:
  formatting:
    decimalSeparator: '.'
```

Specifically (using values confirmed in task .2):
- `ar`: `.` (per current ICU; confirm it's correct for modern Arabic)
- `ku`: `,` or `٫` depending on Sorani vs Kurmanji decision
- `fr-CH`: `.` (Swiss French uses dot; France uses comma)

## Investigation targets
**Required:**
- `src/Humanizer/Bytes/ByteSize.cs:230-274` — output path (`ToString`)
- `src/Humanizer/Bytes/ByteSize.cs:413-425` — parse path (`FromString` / `TryParse`)
- `src/Humanizer/MetricNumeralExtensions.cs:370` — the only other `NumberDecimalSeparator` consumer
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` — reference for how existing per-locale registries are generated

## Key context
- The existing `#if NET48` conditionals in `tests/Humanizer.Tests/Localisation/LocaleFormatterExactTheoryData.cs:10-23` acknowledge that NLS and ICU disagree on Arabic decimal separator. Once this task lands, those conditionals should be removed because both paths consult the override and produce the same output.
- Cloning `NumberFormatInfo` is cheap (~100 bytes) and only happens in the override path. Acceptable for `ar`/`ku`/`fr-CH`.
- Do NOT expose the override helper in the public API. Keep `LocaleNumberFormattingOverrides` and `NumberFormattingOverrides` as `internal static`.
- Closed-set validation pitfall: the new `number.formatting` key must be added to BOTH the main parser AND the canonical authoring contract.
- This task also includes fixing decimal-separator-related expected test values in `LocaleCoverageData.cs` and `LocaleFormatterExactTheoryData.cs` for the 3 target locales (merged from former task .2 decimal separator scope) — the contract and its fulfillment land together
- Cache the resolved override-aware `NumberFormatInfo` per culture to avoid churn on the parse-side `ConditionalWeakTable<NumberFormatInfo, HashSet<char>>` cache (cloning a new NFI per call would undermine that cache)
## Acceptance
- [ ] `number.formatting` accepted as third sub-key under `number:` in both `LocaleYamlCatalog.cs` and `CanonicalLocaleAuthoring.cs`
- [ ] `number.formatting.decimalSeparator` validated as a non-empty string
- [ ] `EngineContractCatalog.cs` defines the new member
- [ ] Generator emits `LocaleNumberFormattingOverrides.g.cs` with entries for locales that have overrides
- [ ] `ByteSize.ToString` consults the override; resolved `NumberFormatInfo` cached per culture (not cloned per call)
- [ ] `ByteSize.FromString` / parse path respects the override (parse-side symmetry); cache-friendly with `ConditionalWeakTable`
- [ ] `MetricNumeralExtensions` uses the same override mechanism
- [ ] `ar.yml`, `ku.yml`, `fr-CH.yml` populated with correct separators
- [ ] Expected test values in `LocaleCoverageData.cs` and `LocaleFormatterExactTheoryData.cs` corrected for ar, ku, fr-CH decimal separators
- [ ] `UsesExpectedByteSizeHumanizeSymbols` passes for ar, ku, fr-CH on net10.0 and net8.0
- [ ] `#if NET48` branches in `LocaleFormatterExactTheoryData.cs` for affected locales removed (tests produce same output on NLS and ICU now)
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` passes
- [ ] No new public types introduced
## Done summary
Added number.formatting YAML surface with decimalSeparator field under the existing number: canonical surface. Threaded override through ByteSize.ToString/TryParse and MetricNumeralExtensions via source-generated LocaleNumberFormattingOverrides registry with CultureInfo.Parent fallback. Populated ar (.), ku (momayyiz), fr-CH (.) overrides and corrected test expected values.
## Evidence
- Commits: 6e727fdd42489a66316a11ac0c21f64b40ac09aa
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (38908 passed), dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj (58 passed), dotnet format Humanizer.slnx --verify-no-changes (clean)
- PRs: