# Hard-code locale overrides where ICU data is wrong or drifts

## Overview

The cross-platform audit (4 probes: macOS, Linux, Windows net10, Windows net48) revealed that Humanizer produces different output across platforms for the same locale because it delegates date/time/number formatting to `CultureInfo`, which pulls data from ICU on modern .NET and NLS on .NET Framework. ICU data also drifts between ICU versions. The platform agreement matrix shows only 72.3–95.4% agreement on date/time/number values across the four environments.

Three failure categories were identified:
1. **Test expected values were wrong from day one** (en-US short time should be 12-hour, fr-CH decimal should be `.`, fa month name should include ezafe)
2. **New ICU data is a regression for native speakers** (ta Tamil loses native day-period words, ku switches from Sorani-Arabic script to Kurmanji-Latin script)
3. **Both old and new are acceptable stylistic variations** (bn/he/zu-ZA/bg/el spelling and modernization)

Existing test infrastructure is sufficient: `DateToOrdinalWords_*`, `UsesExpectedByteSizeHumanizeSymbols`, `LocaleRegistrySweepTests` already compare exact strings across all 62 locales. The fix is to:

- Own the correct locale data in YAML via two clearly-scoped schema extensions
- Delete pure ICU-snapshot tests that exercise zero Humanizer code
- Fix test expected values where they were wrong from the start
- Document the override pattern and fold in the stale-documentation fixes from fn-2

## Scope

**In scope:**
- **New canonical surface `calendar:`** for temporal locale data: `months`, `monthsGenitive` (optional), with room for future `monthsAbbreviated`, `days`, `daysAbbreviated`, `dayPeriods`, `amDesignator`, `pmDesignator`
- **New sub-block `number.formatting:`** under the existing `number` surface (symmetric with `number.words` and `number.parse`): `decimalSeparator`, with room for future `groupSeparator`, `digitSubstitution`, `percentSymbol`
- Runtime wiring: `PatternDateToOrdinalWordsConverter` / `PatternDateOnlyToOrdinalWordsConverter` consult `calendar.months` before `DateTime.ToString`; `ByteSize.ToString` and `MetricNumeralExtensions` consult `number.formatting.decimalSeparator` before `NumberFormatInfo.NumberDecimalSeparator`
- Populate `calendar.months` for `bn`, `fa`, `he`, `ku`, `zu-ZA`, `ta` (and any others surfaced by native-speaker audit in task .2)
- Populate `number.formatting.decimalSeparator` for `ar`, `ku`, `fr-CH`
- Delete pure-snapshot tests: `DateFormatting_ShortDate_*`, `DateFormatting_LongDate_*`, `TimeFormatting_ShortTime_*`, `TimeFormatting_LongTime_*` from `LocaleRegistrySweepTests.cs` + matching theory data + matrix completeness assertions
- Fix test expected values that were wrong from day one (en-US 24h→12h; fr-CH comma→dot; fa missing ezafe; others as surfaced)
- Document the two new schema extensions in `docs/locale-yaml-reference.md`, `docs/adding-a-locale.md`, `docs/locale-yaml-how-to.md`, and `docs/localization.md`
- **Fold in fn-2 documentation fixes**: `docs/localization.md` Supported Languages list (missing 16 locales, 3 wrong codes), stale "residual leaves" claims in `localization.md` lines 133-157 and `locale-yaml-reference.md` line 1650, `CLAUDE.md` "60+ locales" → "62 locales"
- Re-run the cross-platform probe and commit `probe-*-after.json` for all 4 environments; verify 0 test failures on modern .NET (macOS/Linux/Windows net10.0 and net8.0); net48 test-suite execution is a documented follow-up due to a pre-existing `Enum.GetValues<T>()` blocker (net48 probe output is a hard gate)

**Out of scope:**
- Overriding long-date format patterns (weekday + day + month + year ordering) — this is stylistic variation with no single "correct" form
- Overriding short-date patterns — Humanizer does not mediate short dates
- Overriding abbreviated month names (`MMM`) — no current YAML pattern uses them; generator emits diagnostic error if `MMM` appears in an ordinal-date pattern when `calendar.months` override is active; `calendar.monthsAbbreviated` can be added later if needed
- Runtime YAML parsing (everything remains build-time source-gen)
- Filing upstream issues against CLDR or .NET
- Overriding `CultureInfo` itself (overrides are scoped to Humanizer consumers only)

## Architecture

### Schema additions

**New canonical surface `calendar:`** (8th surface, added to the closed-set validator):
- `months` — array of exactly 12 strings, indexed by calendar month (index 0 = January for Gregorian, or native calendar month 1 when using `calendarMode: Native`)
- `monthsGenitive` — optional parallel array for locales distinguishing nominative/genitive forms (cs, pl, ru, uk, bg, hr, sk, sl)
- Future fields: `monthsAbbreviated`, `days`, `daysAbbreviated`, `dayPeriods.{earlyMorning,morning,afternoon,evening,night}`, `amDesignator`, `pmDesignator`, `eraNames`
- Inherits via `variantOf`: child locale inherits parent's `calendar.months` unless it authors its own
- Validated at generator time: exactly 12 entries if present; empty/absent = no override (fall through to `CultureInfo`)

**New sub-block under existing `number:` surface — `number.formatting:`**
- `decimalSeparator` — single character (or multi-character for locales like Persian that may use `٫`); overrides `NumberFormatInfo.NumberDecimalSeparator` in Humanizer's two consumer call sites
- Future fields: `groupSeparator`, `digitSubstitution`, `percentSymbol`, `negativeSign`
- Symmetric with existing `number.words` (output-as-words) and `number.parse` (input) — `number.formatting` is "output as digits"
- Inherits via `variantOf`

Both extensions land in `EngineContractCatalog.cs`, `CanonicalLocaleAuthoring.cs`, and `LocaleYamlCatalog.cs` (all three places require updates due to closed-set validation).

### Runtime changes

- **`OrdinalDatePattern`** gains optional `string[]? months` and `string[]? monthsGenitive` fields, embedded directly from the `calendar:` YAML surface via `OrdinalDateProfileCatalogInput` (no separate `LocaleCalendarRegistry` — only one consumer exists today). Before calling `DateTime.ToString(formatString, culture)`, if `months` is non-null, pre-rewrite the format string to replace **`MMMM` only** (not `MMM`) with the literal month-name for the current month (using `DateTimeFormatInfo` single-quote literal quoting so the rest of the pattern stays intact). If an ordinal-date pattern contains `MMM` and `calendar.months` is active, the generator emits a diagnostic error — `calendar.monthsAbbreviated` is out of scope. The generator also validates that `MMMM` appears in a supported position (unescaped, outside single-quoted literals). Preserve the existing `d'<<DAY>>'` marker. Apply to both `PatternDateToOrdinalWordsConverter` and `PatternDateOnlyToOrdinalWordsConverter`.
- **`ByteSize.ToString`**: before calling `double.ToString(format, provider)`, **only when `provider` is a `CultureInfo`**, consult a source-generated `LocaleNumberFormattingOverrides.TryGetDecimalSeparator(culture, out var sep)` registry. When an override exists, use a cached per-culture `NumberFormatInfo` with the overridden `NumberDecimalSeparator`. Caller-supplied custom `NumberFormatInfo`/`IFormatProvider` is preserved as-is (overriding it would be a behavior regression). Zero-allocation fast path when no override. Cache the resolved `NumberFormatInfo` per culture to avoid undermining the `ConditionalWeakTable<NumberFormatInfo, HashSet<char>>` cache on the parse side. The generated registry is populated from **resolved locale data** (after `variantOf` inheritance). Runtime `TryGetDecimalSeparator` walks `CultureInfo.Parent` (same fallback semantics as `LocaliserRegistry.FindLocaliser`) so unlisted child cultures fall back to the parent override.
- **`MetricNumeralExtensions`**: same helper. Replace the direct `CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator` read with the override-aware lookup.
- **New runtime registry**: `LocaleNumberFormattingOverrides` (keyed by locale, holds decimal separator). Source-generated from YAML. `internal static` (not public). Month arrays are embedded into generated `OrdinalDatePattern` instances, not a separate registry.

### Source-generator changes

- `CanonicalLocaleAuthoring.ToLocaleDefinition()` — add `case "calendar":` to the surface switch (line 129-180) to lower into `features["calendar"]`; extend `AddNumberFeatures()` (line 186-211) to accept `formatting` as a third sub-key and lower into `features["numberFormatting"]`
- `CanonicalLocaleAuthoring.cs` — enforce closed-set rules for the new surface and sub-block in the validation methods
- `LocaleYamlCatalog.cs` — add `"calendar"` and `"numberFormatting"` to `SupportedFeatureNames` (line 59-100) so they survive `variantOf` resolution; add `ResolveCalendar` and `ResolveNumberFormatting` methods to extract resolved data from the merged feature map
- `ResolvedLocaleDefinition` — add `Calendar` and `NumberFormatting` properties (`SimpleYamlMapping?`) to carry resolved data to downstream generators; update `Empty()` factory
- `EngineContractCatalog.cs` — register members for the new fields
- `LocaleRegistryInput.cs` — emit `LocaleNumberFormattingOverrides` registry from `locale.NumberFormatting` property; runtime `TryGetDecimalSeparator` walks `CultureInfo.Parent` (same fallback as `LocaliserRegistry.FindLocaliser`)
- `OrdinalDateProfileCatalogInput.cs` — read `locale.Calendar.months` + `calendar.monthsGenitive` and pass them into the generated `OrdinalDatePattern` ctor; validate `MMMM`-only constraint and pattern structure

## Quick commands
```bash
# Build and run generator tests
dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj

# Run the affected test buckets across all 62 locales
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0

# Re-probe after changes to verify cross-platform consistency
dotnet run tools/locale-probe.cs --json > tools/probe-macos-after.json

# Compare before/after
dotnet run tools/compare-probes.cs
```

## Acceptance
- [ ] New `calendar:` canonical surface accepted by the closed-set validator with `months` and `monthsGenitive` fields
- [ ] New `number.formatting:` sub-block accepted under existing `number:` surface with `decimalSeparator` field
- [ ] All 62 locales have byte-identical `DateToOrdinalWords` / `DateOnlyToOrdinalWords` output on macOS, Linux, Windows net10, and Windows net48 (for locales with `calendar.months` authored)
- [ ] All 62 locales have byte-identical `ByteSize.Humanize("KB", culture)` decimal separator output across the 4 probe environments (for locales with `number.formatting.decimalSeparator` authored)
- [ ] Pure-ICU-snapshot tests deleted: 12 test methods + 12 theory data properties + 12 matrix completeness assertions
- [ ] Test expected values corrected in `LocaleCoverageData.cs` and `LocaleFormatterExactTheoryData.cs` for the overridden locales (merged into tasks .3 and .4 for atomicity)
- [ ] `calendar.months` populated for `bn`, `fa`, `he`, `ku`, `zu-ZA`, `ta`
- [ ] `number.formatting.decimalSeparator` populated for `ar`, `ku`, `fr-CH`
- [ ] `docs/locale-yaml-reference.md` documents `calendar:` surface and `number.formatting:` sub-block
- [ ] `docs/adding-a-locale.md` contributor checklist gains a cross-platform verification bullet
- [ ] `docs/locale-yaml-how-to.md` has a preflight question and a recipe for the override pattern
- [ ] `docs/localization.md` Supported Languages list enumerates all 62 shipped locales with correct codes (rolls in fn-2 fix R1)
- [ ] `docs/localization.md` stale "residual clock leaves" claims removed (rolls in fn-2 fix R2)
- [ ] `docs/locale-yaml-reference.md` stale "residual locale leaves" language removed (rolls in fn-2 fix R3)
- [ ] `CLAUDE.md` says "62 locales" instead of "60+ locales" (rolls in fn-2 fix R4)
- [ ] Test run on net10.0 produces 0 locale-related failures on macOS/Linux/Windows (was 122)
- [ ] Test run on net8.0 produces 0 locale-related failures on macOS/Linux/Windows
- [ ] net48 test-suite blocker (`Enum.GetValues<T>()`) filed as separate issue; net48 probe output committed as hard gate
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` passes
- [ ] `dotnet pack` succeeds and `pwsh tests/verify-packages.ps1` passes

## Early proof point

Task fn-3-hard-code-locale-overrides-where-icu.1 deletes the pure-ICU-snapshot tests. If that reveals unexpected dependencies (e.g., matrix completeness asserting coverage of deleted data sets, or tests referencing deleted theory data), re-evaluate the split between "pure snapshot" and "exercises Humanizer code" before continuing.

Task fn-3-hard-code-locale-overrides-where-icu.3 (first locale with `calendar.months` override) proves the new YAML surface + generator + runtime pipeline works end-to-end on a single locale (recommend `zu-ZA` as the smallest diff). If it doesn't produce identical output across the four probe environments, re-evaluate the substitution approach in `OrdinalDatePattern.Format` before authoring `calendar.months` for the other 5 locales.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | Delete pure-ICU-snapshot tests | fn-3-hard-code-locale-overrides-where-icu.1 | — |
| R2  | Audit and decide correct locale values | fn-3-hard-code-locale-overrides-where-icu.2 | Decision-only; expected-value code changes merged into .3 and .4 for atomicity |
| R3  | Add `calendar:` canonical surface + populate affected locales | fn-3-hard-code-locale-overrides-where-icu.3 | — |
| R4  | Add `number.formatting:` sub-block + populate affected locales | fn-3-hard-code-locale-overrides-where-icu.4 | — |
| R5  | Document override pattern + fold in fn-2 documentation fixes | fn-3-hard-code-locale-overrides-where-icu.5 | — |
| R6  | Verify cross-platform consistency via probe tools | fn-3-hard-code-locale-overrides-where-icu.6 | — |
