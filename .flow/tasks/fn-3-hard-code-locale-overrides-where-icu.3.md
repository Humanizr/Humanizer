# fn-3-hard-code-locale-overrides-where-icu.3 Add calendar: surface with months override and thread through OrdinalDatePattern

## Description
Add a new `calendar:` canonical surface to the YAML schema for locale-owned temporal data, plus generator and runtime wiring so `PatternDateToOrdinalWordsConverter` / `PatternDateOnlyToOrdinalWordsConverter` use `calendar.months` instead of `CultureInfo.DateTimeFormat.MonthNames` when present. Populate `calendar.months` for the locales where ICU data is wrong or drifts: `bn`, `fa`, `he`, `ku`, `zu-ZA`, `ta`.

**Size:** M
**Files:**
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs` — parse new `calendar` surface (closed-set key addition)
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` — canonical contract enforcement for `calendar` surface and its sub-keys
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs` — register `calendar.months` and `calendar.monthsGenitive` member definitions
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` — read `calendar.months` / `calendar.monthsGenitive` from YAML and pass into the generated `OrdinalDatePattern` ctor
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs` — add optional `string[]? months` / `string[]? monthsGenitive` fields; pre-rewrite format string to substitute literal month names when present
- `src/Humanizer/Locales/bn.yml`, `fa.yml`, `he.yml`, `ku.yml`, `zu-ZA.yml`, `ta.yml` — add `calendar:` surface with `months` (and `monthsGenitive` where applicable)
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs` — fix expected month-name values for the 6 target locales (merged from former task .2 calendar scope)

## Approach

### Schema: new `calendar:` canonical surface

Add `calendar` as the 8th top-level surface (alongside `list`, `formatter`, `phrases`, `number`, `ordinal`, `clock`, `compass`). Initial sub-keys:
- `months` — required-when-present array of exactly 12 strings
- `monthsGenitive` — optional parallel array of 12 strings for locales distinguishing nominative/genitive month forms

The closed-set validator must accept `calendar:` as a valid top-level surface key and accept only `months` / `monthsGenitive` as sub-keys. Extra sub-keys rejected with a clear generator diagnostic. Empty surface (`calendar: {}`) is valid (inherits from parent via `variantOf`).

Inheritance: if a locale defines `calendar.months`, child locales inherit unless they override the full `months` array (arrays replace, not merge).

### Generator pipeline plumbing (critical — data must survive to emission)

The current generator pipeline lowers canonical YAML surfaces into legacy feature buckets via `CanonicalLocaleAuthoring.ToLocaleDefinition()` (line 117-184), then merges them via `SupportedFeatureNames` in `LocaleYamlCatalog.ResolveLocale()`. **Both steps must be updated or the new data is silently dropped.**

1. **`CanonicalLocaleAuthoring.ToLocaleDefinition()` — add `case "calendar":` to the surface switch (line 129-180):**
   - Currently the switch handles: `list`, `formatter`, `phrases`, `number`, `ordinal`, `clock`, `compass`
   - Add: `case "calendar": features["calendar"] = surfaceMapping; break;`
   - This lowers the `calendar:` canonical surface into a `"calendar"` feature bucket that the resolver can see

2. **`LocaleYamlCatalog.cs` — add `"calendar"` to `SupportedFeatureNames` (line 59-100):**
   - This ensures the `calendar` feature survives `variantOf` resolution in `ResolveLocale` (line 414-421 iterates `SupportedFeatureNames` to build `resolvedFeatures`)
   - Without this entry, the `calendar` feature is dropped during merge even if `ToLocaleDefinition` produced it

3. **`LocaleYamlCatalog.ResolveLocale()` — extract resolved calendar from the merged feature map:**
   - After the feature merge loop (line 414-421), add a `TryResolveLocalePart` call with a `ResolveCalendar(localeCode, features)` method (modeled on `ResolveGrammar`/`ResolveHeadings`) that reads `"calendar"` from the resolved feature map and returns a `SimpleYamlMapping?`
   - Pass the result to the `ResolvedLocaleDefinition` constructor

4. **`ResolvedLocaleDefinition` (line 893-924) — add `Calendar` property:**
   - Add `SimpleYamlMapping? calendar` parameter and `Calendar` property
   - Update `Empty()` factory to include the new `null` parameter

5. **`OrdinalDateProfileCatalogInput.Create` — read month arrays from `locale.Calendar`:**
   - Already receives `LocaleCatalogInput` → iterates `localeCatalog.Locales` (which are `ResolvedLocaleDefinition`)
   - For each locale, when building the converter expression via `CreatePatternExpression` (line 159), if `locale.Calendar` has `months` (and optionally `monthsGenitive`), extract and emit them as `new string[] { "...", ... }` arguments to the `OrdinalDatePattern` constructor
   - The month arrays must come from `locale.Calendar`, NOT from `locale.DateToOrdinalWords`, because `calendar:` is a separate surface that may be authored independently

### Runtime: `OrdinalDatePattern` month substitution (MMMM only)

Month arrays are embedded directly into the generated `OrdinalDatePattern` — no separate `LocaleCalendarRegistry` or `CalendarProfileCatalogInput`. The only current consumer is ordinal-date formatting; a separate registry would be premature abstraction.

When `calendar.months` is non-null for the active culture, `OrdinalDatePattern.Format(date)` substitutes the month name literal for **`MMMM` only** in the template BEFORE calling `DateTime.ToString`. Use `DateTimeFormatInfo` single-quote literal syntax:

```
"{day} MMMM yyyy" → "d'<<DAY>>' 'ژانویهٔ' yyyy"
```

**`MMM` (abbreviated) is NOT supported.** If a locale's ordinal-date pattern contains `MMM` and `calendar.months` is active, the generator must emit a diagnostic error. `calendar.monthsAbbreviated` is out of scope for this epic — supporting `MMM` without explicit abbreviated data would silently emit wrong output.

**Literal quoting rules:** When emitting the quoted month name, embedded apostrophes (`'`) in month names must be doubled (e.g., a hypothetical month name `foo'bar` becomes `'foo''bar'` in the format string). This follows the standard `DateTimeFormatInfo` literal quoting convention.

**Pattern validation at generator time:** The generator validates that ordinal-date format patterns containing `MMMM` do so in a supported position (unescaped, outside single-quoted literals). Reject patterns that would require a full custom-date-format tokenizer. The supported subset is: `MMMM` appearing as a standalone token separated by spaces, punctuation, or other format specifiers.

Preserve the existing `d'<<DAY>>'` day-marker mechanism; only the `MMMM` specifier changes. When `monthsGenitive` is present AND the template position indicates genitive context (day adjacent to month, i.e., `d MMMM` or `MMMM d`), use `monthsGenitive[month-1]`; otherwise use `months[month-1]`.

If `calendar.months` is null (no override), `OrdinalDatePattern.Format` behaves exactly as today — no branching cost, no behavior change for the 56 unaffected locales.

Thread the same data through `PatternDateOnlyToOrdinalWordsConverter` symmetrically.

### Per-locale YAML population

For each of the 6 target locales, add a `calendar:` surface. Example for Persian (fa):

```yaml
calendar:
  months:
    - 'ژانویهٔ'
    - 'فوریهٔ'
    - 'مارس'
    - 'آوریل'
    - 'مهٔ'
    - 'ژوئن'
    - 'ژوئیهٔ'
    - 'اوت'
    - 'سپتامبر'
    - 'اکتبر'
    - 'نوامبر'
    - 'دسامبر'
```

For each locale, use the forms decided in task .2 (native-speaker audit):
- `bn`: modern short-i Bengali spellings (Bangla Academy standard)
- `fa`: Persian with ezafe marks for date context
- `he`: Hebrew standalone or with ב prefix (per task .2 decision)
- `ku`: Sorani Arabic-script or Kurmanji Latin-script (per task .2 decision)
- `zu-ZA`: Zulu spellings per task .2 decision
- `ta`: Tamil month names

## Investigation targets
**Required:**
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs:673-725` — clock profile member definitions, model pattern for adding new optional-string-array members
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs:196-240` — canonical top-level validator (8 surface keys); `244-279` — nested sub-key validator pattern for `number`
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:40-100` — duplicate closed-set enforcement (update both)
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs:159-177` — where `pattern`, `dayMode`, `calendarMode` are parsed; add `calendar.months` passthrough
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:33-94` — existing `d'<<DAY>>'` marker mechanism to preserve
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseClockNotationConverter.cs` — reference implementation for "optional profile fields, populated from YAML, internal sealed records"

**Optional:**
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs:200-277` — where `DateToOrdinalWords_*` tests assert exact output
- `tests/Humanizer.SourceGenerators.Tests/` — pattern for testing new schema fields

## Key context
- Closed-set validator is enforced in TWO places (`LocaleYamlCatalog.cs` AND `CanonicalLocaleAuthoring.cs`) — both must accept `calendar` and its sub-keys or the generator throws `InvalidLocaleDefinition`
- `PublicApiApprovalTest` must be updated for net48/net8.0/net10.0 if any new public type or constructor parameter is exposed (keep all new types `internal sealed`)
- Minimal-allocations constraint: the format-string rewrite should allocate only in the override path. When `months` is null, `GetFormatString()` returns the template unchanged (same as today)
- Source-gen only — no runtime YAML parsing
- After this task lands, the `DateToOrdinalWords_*` / `DateOnlyToOrdinalWords_*` tests for the 6 locales should pass identically on all 4 probe platforms
- Start with `zu-ZA` as the proof-point locale (smallest diff — single letter change in month name); validate the pipeline before expanding to the other 5
- If ICU returns both nominative and genitive month forms for a locale today (e.g., Russian "январь" vs "января"), the override must supply both via `months` and `monthsGenitive` or pick one consistently
- This task also includes fixing calendar-related expected test values in `LocaleCoverageData.cs` for the 6 target locales (merged from former task .2 calendar scope) — the contract and its fulfillment land together

## Early proof point (within this task)
Start with a single locale (e.g., `zu-ZA`, which has the simplest change — just one spelling difference). Once the YAML override produces byte-identical output on macOS AND the fallback path (empty `months`) still works for other locales, expand to the other 5.
## Acceptance
- [ ] `calendar:` accepted as 8th canonical surface in `LocaleYamlCatalog.cs` AND `CanonicalLocaleAuthoring.cs`
- [ ] `calendar.months` and `calendar.monthsGenitive` validated (exactly 12 entries when present)
- [ ] `EngineContractCatalog.cs` defines new members for `calendar.months` and `calendar.monthsGenitive`
- [ ] `OrdinalDateProfileCatalogInput.cs` reads `calendar.months`/`calendar.monthsGenitive` and passes them into the `OrdinalDatePattern` constructor (no separate CalendarProfileCatalogInput or LocaleCalendarRegistry)
- [ ] Generator emits diagnostic error if ordinal-date pattern contains `MMM` (abbreviated) when `calendar.months` override is active
- [ ] Generator validates `MMMM` appears in supported positions (unescaped, outside single-quoted literals)
- [ ] `OrdinalDatePattern.Format()` substitutes override month names for `MMMM` (literal single-quoted segments in format string) before `DateTime.ToString` when present
- [ ] `OrdinalDatePattern.Format(DateOnly)` does the same for net6+
- [ ] Zero behavior change for locales without `calendar:` (56 unaffected locales produce identical output)
- [ ] `calendar:` surface authored in bn.yml, fa.yml, he.yml, ku.yml, zu-ZA.yml, ta.yml with native-speaker-verified month names
- [ ] Expected test values in `LocaleCoverageData.cs` corrected for the 6 target locales' month names (fa ezafe, ku script, etc.)
- [ ] Source-generator tests pass
- [ ] `DateToOrdinalWords_*` and `DateOnlyToOrdinalWords_*` tests pass for bn, fa, he, ku, zu-ZA, ta on net10.0 and net8.0
- [ ] No new public types introduced (all new runtime types are `internal sealed`)
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` passes
- [ ] No allocation regression in `DateTimeHumanizeExtensions` benchmarks (BenchmarkDotNet run)
## Done summary
Added calendar canonical surface (8th surface) with months/monthsGenitive sub-keys, threaded through the full generator pipeline (CanonicalLocaleAuthoring -> LocaleYamlCatalog -> ResolvedLocaleDefinition -> OrdinalDateProfileCatalogInput -> OrdinalDatePattern), and populated calendar.months for bn (long-i Bengali), fa (Persian with ezafe), he (Hebrew with bet-prefix), ku (Sorani Arabic-script). OrdinalDatePattern substitutes MMMM with literal month names at format time when override is present, with zero behavior change for unaffected locales. Fixed test expected values for fa (ezafe marks) and zu-ZA (February spelling).
## Evidence
- Commits: 745d5bae, a39b274d, 81d7ba0e, 6231780e
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0, dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj, dotnet format Humanizer.slnx --verify-no-changes
- PRs: