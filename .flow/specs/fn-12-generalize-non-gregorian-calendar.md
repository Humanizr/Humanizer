# Generalize non-Gregorian calendar schema (hijri/hebrew/persian/etc.)

## Overview
Refactor the locale YAML schema and ordinal-date runtime so non-Gregorian calendar overrides scale beyond the current per-calendar-named field (`hijriMonths`). Today, Hijri is the only non-Gregorian calendar with authored month names (in `ur.yml`), and the runtime hardcodes `HijriCalendar or UmAlQuraCalendar` dispatch. Adding Hebrew/Persian/etc. would require a new top-level field and a new branch each time.

Landing this *after* PR #1720 (fn-11) keeps the initial Urdu PR focused on locale support, not schema redesign.

## Scope
- Redesign `calendar:` YAML section to support arbitrary non-Gregorian variants.
- Update source generator (`ProfileDefinitions.cs`, `OrdinalDateProfileCatalogInput.cs`, `CanonicalLocaleAuthoring.cs`) to emit and validate the new shape.
- Update `OrdinalDatePattern.ResolveMonthArray` to a dispatch table keyed by calendar type (`HijriCalendar`/`UmAlQuraCalendar` → `"hijri"`, `HebrewCalendar` → `"hebrew"`, `PersianCalendar` → `"persian"`, etc.).
- Migrate `ur.yml`'s `hijriMonths` to the new shape.
- Keep `calendarMode: Native` semantics (Thai Buddhist pattern) unchanged.
- Update docs (`locale-yaml-reference.md`, `adding-a-locale.md`, `ARCHITECTURE.md`) and the add-locale skill reference.

Out of scope:
- Adding actual Hebrew / Persian / Japanese calendar data for any locale (future work, per-locale PRs).
- Changing `calendarMode` pattern-level semantics.
- Touching `th.yml` (Thai Buddhist works via `Native` mode — no authored month overrides).

## Proposed shape

```yaml
calendar:
  months: ['…', …]              # gregorian, implicit default
  native: hijri                  # optional — identifies the locale's preferred non-greg calendar (metadata, no behavior change yet)
  variants:
    hijri:
      months: ['محرم', …]
      # future: eras, dayNames, monthsGenitive
    hebrew:
      months: [...]
    persian:
      months: [...]
```

Dispatch (in `OrdinalDatePattern.ResolveMonthArray`):

```
HijriCalendar | UmAlQuraCalendar  → "hijri"
HebrewCalendar                     → "hebrew"
PersianCalendar                    → "persian"
JapaneseCalendar                   → "japanese"  (when era support arrives)
ThaiBuddhistCalendar               → (Native-mode path; no override table lookup by default)
default                            → Gregorian `months`
```

## Key context
- Single current consumer: `ur.yml` has `hijriMonths`. Migration is one file.
- Runtime check in `OrdinalDatePattern.cs:95` is the only hardcoded calendar-type branch. Replace with a small dictionary or type-pattern switch.
- `calendarMode: 'Native'` (Thai, Urdu patterns) is orthogonal — it controls *which calendar formats the date*, not *which month-name array is used*. Keep it.
- `calendar.native: hijri` metadata is useful future signal (e.g., "format this date in the locale's preferred calendar by default") but does not need to change current behavior in this epic.

## Quick commands
```bash
# Generator tests
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj

# Urdu + Thai tests (the two calendar-aware locales today)
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --filter "FullyQualifiedName~UrduHijri|FullyQualifiedName~Thai"

# Build + lint
dotnet build Humanizer.slnx -c Release
dotnet format Humanizer.slnx --verify-no-changes
```

## Acceptance
- [ ] New YAML shape under `calendar.variants.<name>.months` is accepted and validated.
- [ ] Old `hijriMonths` is either migrated to the new shape in `ur.yml`, or a shim keeps both working during transition. Choose one, not both.
- [ ] `OrdinalDatePattern.ResolveMonthArray` uses a dispatch table, no hardcoded `HijriCalendar or UmAlQuraCalendar` check.
- [ ] All existing Urdu Hijri tests pass with the new shape; `ur-PK` inheritance still works.
- [ ] Thai tests (`calendarMode: Native`) unchanged.
- [ ] Docs (`locale-yaml-reference.md`, `adding-a-locale.md`, `ARCHITECTURE.md`, `.agents/skills/add-locale/`) describe the new shape and list the supported calendar variant keys.
- [ ] No public API break (runtime-facing APIs like `ToOrdinalWords()` unchanged).

## Early proof point
First task: land the generator schema + dispatch change with `ur.yml` migrated to the new shape, keeping all current Urdu tests green. If that doesn't produce byte-identical output for the existing Hijri test expectations, the migration plan needs rework before touching docs or adding new calendar types.

## Decision context
- **Why not stick with `hijriMonths` pattern?** Scales O(1) per calendar type in both YAML and runtime code. Review comments on #1720 flagged the ad-hoc naming.
- **Why not per-calendar YAML files (`locales/calendars/*.yml`)?** The month names are locale-specific, not calendar-specific. Factoring calendar definitions separately from locales would require cross-joining both at build time with no real gain.
- **Why `variants` nested under `calendar`?** Keeps all month/era data co-located in one place per locale; mirrors how `ordinals` nests `numeric`/`words` under `ordinal`.

## References
- Current hardcoded dispatch: `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:93-101`
- Current schema: `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` (search `hijriMonths`)
- Only current consumer: `src/Humanizer/Locales/ur.yml:596-608`
- Parent PR: https://github.com/Humanizr/Humanizer/pull/1720 — lands fn-11 first, then this epic.
