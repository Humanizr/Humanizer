## Description
Cover `OrdinalDatePattern` reachable branches and `NoMatchFoundException` public ctors (33%). The `GetPatternCulture` `ArgumentOutOfRangeException` fallback is the one branch listed in the epic's declared-unreachable appendix; every OTHER branch in `OrdinalDatePattern` must be covered here. No task-level Boundaries beyond the epic appendix.

**Size:** M
**Files:**
- `tests/Humanizer.Tests/Localisation/DateToOrdinalWords/OrdinalDatePatternTests.cs` (new or extend)
- `tests/Humanizer.Tests/NoMatchFoundExceptionTests.cs` (new, tiny)

## Approach
- **OrdinalDatePattern day-mode arms** (`src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:306-317`, the `FormatDay` method):
  <!-- Updated by plan-sync: fn-9.1 actual line numbers differ from original plan -->
  - `MasculineOrdinalWhenDayIsOne` (`:314`) — reach via a culture whose phrase-table declares this mode
  - `DotSuffix` (`:315`) — same
  - `InvalidOperationException` default throw (`:316`) — reach via direct-instantiation test passing an invalid enum value (internals visible to Humanizer.Tests; the throw is behaviorally part of the contract)
- **TFM-conditional branches** (`:64`, `:161`, `:256`, `:326`) — net48 job exercises these; merged report captures them.
  <!-- Updated by plan-sync: fn-9.1 actual #if locations differ from original plan -->
- **DateOnly overload** (`:64-79`) under `NET6_0_OR_GREATER`.
  <!-- Updated by plan-sync: fn-9.1 actual line range is :64-79 not :65-72 -->
- **GetPatternCulture AOORE fallback** (`:288-290`) — listed in epic's declared-unreachable appendix. If `.1/uncovered.json` proves a deterministic reachable trigger, cover it here; otherwise leave it as appendix-absorbed and exclude it from this task's per-class percentage calculation is NOT an option — the epic's tolerance already accounts for these ~3 lines.
  <!-- Updated by plan-sync: fn-9.1 actual lines :288-290 not :270-285; ~3 lines not ~15 -->
- **NoMatchFoundException.** Three public ctors (verified from source — no serialization ctor):
  - `new NoMatchFoundException()`
  - `new NoMatchFoundException(string message)`
  - `new NoMatchFoundException(string message, Exception? inner)`
  Assert `Message` and `InnerException` on each.

## Investigation targets
**Required:**
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:1-334`
  <!-- Updated by plan-sync: fn-9.1 file is 334 lines not 310 -->
- `src/Humanizer/NoMatchFoundException.cs`
- `artifacts/fn-9-local-coverage/uncovered.json`
- Cultures with non-default day-mode: scan `src/Humanizer/Locales/*.yml` for day-mode declarations (`ar.yml`, `uk.yml`, locales emitting `MasculineOrdinalWhenDayIsOne` or `DotSuffix`)

**Optional:**
- `tests/Humanizer.Tests/Localisation/ar/` — Arabic calendar test shapes

## Acceptance
- [ ] `OrdinalDatePattern` reaches ≥90% line / ≥85% branch in the merged multi-TFM report.
- [ ] EVERY day-mode switch arm (`MasculineOrdinalWhenDayIsOne`, `DotSuffix`, `InvalidOperationException` default) is covered — no task-level escape hatch.
- [ ] `NoMatchFoundException` reaches 100% line coverage across its three public ctors; `Message` and `InnerException` asserted.
- [ ] No serialization-ctor acceptance (the source has none).
- [ ] The only `OrdinalDatePattern` branch that may remain uncovered is the `GetPatternCulture` AOORE fallback (epic-appendix item); it is absorbed in the aggregate threshold, not excluded at the class level.

## Done summary
Added coverage tests for all OrdinalDatePattern reachable branches (every day-mode switch arm, SubstituteMonth with overrides/genitive/apostrophe escaping, FindAdjacentDayOfMonth scanning, ReplaceDayMarker fallback, StripDirectionalityControls with literal bidi marks, ResolveMonthArray Hijri/Gregorian paths, DateOnly overload, GetPatternCulture Native/Gregorian modes) and NoMatchFoundException three public constructors with Message/InnerException assertions.
## Evidence
- Commits: 924beb5e, 4ece058d
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (40785 passed)
- PRs: