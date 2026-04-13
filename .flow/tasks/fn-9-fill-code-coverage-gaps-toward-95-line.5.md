## Description
Cover `OrdinalDatePattern` reachable branches and `NoMatchFoundException` public ctors (33%). The `GetPatternCulture` `ArgumentOutOfRangeException` fallback is the one branch listed in the epic's declared-unreachable appendix; every OTHER branch in `OrdinalDatePattern` must be covered here. No task-level Boundaries beyond the epic appendix.

**Size:** M
**Files:**
- `tests/Humanizer.Tests/Localisation/DateToOrdinalWords/OrdinalDatePatternTests.cs` (new or extend)
- `tests/Humanizer.Tests/NoMatchFoundExceptionTests.cs` (new, tiny)

## Approach
- **OrdinalDatePattern day-mode arms** (`src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:288-299`):
  - `MasculineOrdinalWhenDayIsOne` (`:296`) — reach via a culture whose phrase-table declares this mode
  - `DotSuffix` (`:297`) — same
  - `InvalidOperationException` default throw (`:298`) — reach via direct-instantiation test passing an invalid enum value (internals visible to Humanizer.Tests; the throw is behaviorally part of the contract)
- **TFM-conditional branches** (`:143-147`, `:238-242`) — net48 job exercises these; merged report captures them.
- **DateOnly overload** (`:65-72`) under `NET6_0_OR_GREATER`.
- **GetPatternCulture AOORE fallback** (`:270-285`) — listed in epic's declared-unreachable appendix. If `.1/uncovered.json` proves a deterministic reachable trigger, cover it here; otherwise leave it as appendix-absorbed and exclude it from this task's per-class percentage calculation is NOT an option — the epic's tolerance already accounts for these ~15 lines.
- **NoMatchFoundException.** Three public ctors (verified from source — no serialization ctor):
  - `new NoMatchFoundException()`
  - `new NoMatchFoundException(string message)`
  - `new NoMatchFoundException(string message, Exception? inner)`
  Assert `Message` and `InnerException` on each.

## Investigation targets
**Required:**
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:1-310`
- `src/Humanizer/NoMatchFoundException.cs`
- `artifacts/fn-9-baseline/uncovered.json`
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
_To be filled on completion._

## Evidence
- Commits:
- Tests:
- PRs:
