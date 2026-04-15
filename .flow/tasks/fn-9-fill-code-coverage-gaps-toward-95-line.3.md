## Description
Cover the three 50%-line converter registries plus the `DateToOrdinalWordsExtensions` grammatical-case extension path. All four are touched by static `Configurator` init but their default-factory lambdas never fire because every shipping locale registers an override.

`HeadingTableCatalog.Invariant` is handled via the epic's declared-unreachable appendix from .1 — NOT by this task.

**Size:** M
**Files:**
- `tests/Humanizer.Tests/Localisation/Default/RegistryFallbackTests.cs` (new)

## Approach
- Use `eo` (Esperanto) — same known-unregistered culture as existing `HeadingTests.ToHeadingFallsBackToEnglishForUnknownCultures:208`. Do NOT use synthetic codes like `xx-ZZ`.
- Under `UseCulture("eo")`, call:
  - `date.ToOrdinalWords()` — via `DateToOrdinalWordsConverterRegistry`
  - `dateOnly.ToOrdinalWords()` — via `DateOnlyToOrdinalWordsConverterRegistry` (net6+)
  - `timeOnly.ToClockNotation(...)` — via `TimeOnlyToClockNotationConvertersRegistry`
  - `date.ToOrdinalWords(grammaticalCase)` — via `DateToOrdinalWordsExtensions` (the grammatical-case extension path)
- Assert `date.ToOrdinalWords()` and `dateOnly.ToOrdinalWords()` (and the grammatical-case overload) return the corresponding `Default*` converter's output, walking `LocaliserRegistry<T>.ResolveForCulture` → default-factory → `Default*`. `timeOnly.ToClockNotation(...)` is different: `TimeOnlyToClockNotationConvertersRegistry` falls back to `TimeOnlyToClockNotationProfileCatalog.Resolve("en")` (the English clock-notation profile) rather than a `Default*` converter — assert equality with that profile's output instead.
- No registry mutation.

## Investigation targets
**Required:**
- `src/Humanizer/Configuration/DateToOrdinalWordsConverterRegistry.cs:3-8`
- `src/Humanizer/Configuration/DateOnlyToOrdinalWordsConverterRegistry.cs:4-8`
- `src/Humanizer/Configuration/TimeOnlyToClockNotationConvertersRegistry.cs:5-9`
- `src/Humanizer/DateToOrdinalWordsExtensions.cs` — grammatical-case extension
- `src/Humanizer/Configuration/LocaliserRegistry.cs` — `ResolveForCulture` and default-factory invocation
- `tests/Humanizer.Tests/HeadingTests.cs:208` — `eo` precedent

## Acceptance
- [ ] All three registries plus the grammatical-case extension path reach ≥95% line and ≥90% branch.
- [ ] Every test uses `CultureInfo.GetCultureInfo("eo")`.
- [ ] `HeadingTableCatalog.Invariant` is NOT covered here — it lives in the epic's declared-unreachable appendix from .1.
- [ ] No registry mutation (`Register`/`Unregister`) inside any test.

## Done summary
_To be filled on completion._

## Evidence
- Commits:
- Tests:
- PRs:
