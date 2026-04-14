# Fix urdu-locale CI + PR feedback + rebase

## Overview
PR #1720 (Urdu locale) has blocking issues: a CI test failure on ICU platforms, three open review comments, and a schema noise cleanup (empty `surfaces:` blocks in variant locales). Rebase onto latest `main` and land minimal fixes without loosening acceptance.

## Scope
- Fix `UrduHijriDateTests.HijriCalendar_UrIn_InheritsHijriMonths` on net8.0/net10.0 (ICU).
- Address 3 reviewer comments: committed `.claude/scheduled_tasks.lock`, `IcelandicGenderedOrdinalTests.cs` namespace, `NumberWordSuffixOrdinalizer.cs` negative-irregular divergence.
- Make YAML `surfaces:` optional for variants with no overrides; strip `surfaces: {}` from `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml` (the only three locales currently carrying it as noise).
- Rebase onto latest `main` (1 commit ahead: `5eefe6aa`).

Out of scope: generalizing the non-Gregorian calendar schema (tracked in a separate epic, fn-12).

## Key context
- **ur-IN + HijriCalendar on ICU**: `new CultureInfo("ur-IN").DateTimeFormat.Calendar = new HijriCalendar()` throws on ICU (net8/10) because ICU's `ur-IN` OptionalCalendars does not contain HijriCalendar. On net48 (NLS), it does. `ur-PK` works on both.
- **Test intent**: Validate that `ur-IN` *inherits* Hijri month data from `ur` when a user sets HijriCalendar. Only reachable on platforms where the runtime accepts that calendar for `ur-IN`. Guard with `Assert.Skip(...)` (xUnit v3) based on `OptionalCalendars` content — this is a runtime capability gate, not an acceptance weakening.
- **Ordinalizer**: `NumberWordSuffixOrdinalizer.cs:53` matches on `block.ExactReplacements` for a negative magnitude but then delegates to `INumberToWordsConverter.ConvertToOrdinal` — so positive irregulars come from `surfaces.ordinal.numeric` while negatives come from `surfaces.number.words.ordinal`. Fix: use the matched `negExact` so both paths draw from the same ordinalizer replacement source.
- **Namespace**: Test files use folder-code namespaces (e.g. `Humanizer.Tests.Localisation.is`). `IcelandicGenderedOrdinalTests.cs` uses `Humanizer.Tests.Localisation.Icelandic` — rename.
- **Lock file**: `.claude/scheduled_tasks.lock` is machine-specific state, accidentally committed. Remove from tree and add `.claude/scheduled_tasks.lock` (and `.claude/*.lock`) to `.gitignore`.
- **Empty surfaces**: `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml` carry `surfaces: {}` with no overrides. Make the key optional for variants with no overrides and drop the empty block.

## Quick commands
```bash
# Reproduce CI failure locally (net10.0)
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --filter "FullyQualifiedName~UrduHijriDateTests"

# Full Urdu test sweep
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --filter "FullyQualifiedName~Localisation.ur"

# Source generator tests (for the surfaces-optional change)
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj

# Build
dotnet build Humanizer.slnx -c Release

# Lint
dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic
```

## Acceptance
- [ ] All three TFMs (net48, net8.0, net10.0) pass the full Humanizer.Tests suite (net48 execution on Windows host).
- [ ] `HijriCalendar_UrIn_InheritsHijriMonths` is executable where the runtime allows, or skipped with a clear runtime-capability message on platforms where ICU rejects the calendar — not weakened or removed.
- [ ] `.claude/scheduled_tasks.lock` is removed from the repo and `.gitignore` prevents reintroduction.
- [ ] `IcelandicGenderedOrdinalTests.cs` uses `Humanizer.Tests.Localisation.is` namespace (matching folder convention).
- [ ] Negative and positive irregular ordinals in `NumberWordSuffixOrdinalizer` both source from the ordinalizer's `ExactReplacements`; covered by a regression test.
- [ ] `surfaces:` is optional for variants without overrides; `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml` no longer contain `surfaces: {}`.
- [ ] Branch is rebased onto latest `origin/main` with a clean push.
- [ ] `dotnet format --verify-no-changes` passes.

## Early proof point
Task `fn-11.1` (rebase + CI fix) validates the fundamental approach — once CI is green on the updated base, the feedback and cleanup fixes proceed against a stable branch. If the CI fix doesn't stabilize, re-evaluate whether the test needs redesign (different inheritance proof path) before touching feedback items.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | All TFMs pass Humanizer.Tests | fn-11.1, fn-11.4 | — |
| R2  | Hijri ur-IN test fixed without weakening | fn-11.1 | — |
| R3  | `.claude/scheduled_tasks.lock` removed + gitignored | fn-11.2 | — |
| R4  | Icelandic namespace renamed to `is` | fn-11.2 | — |
| R5  | Ordinalizer negative irregulars unified | fn-11.3 | — |
| R6  | Branch rebased on latest main | fn-11.1 | — |
| R7  | `dotnet format` passes | fn-11.1, fn-11.2, fn-11.3, fn-11.4 | — |
| R8  | `surfaces:` optional for variants w/o overrides; empty blocks removed | fn-11.4 | — |
| R9  | Non-Gregorian calendar schema generalization | — | Deferred to epic `fn-12` — broader refactor, separate PR |

## References
- PR: https://github.com/Humanizr/Humanizer/pull/1720
- Failing test: `tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs:96-108`
- Ordinalizer: `src/Humanizer/Localisation/Ordinalizers/NumberWordSuffixOrdinalizer.cs:53`
- Namespace: `tests/Humanizer.Tests/Localisation/is/IcelandicGenderedOrdinalTests.cs:3`
- Lock file: `.claude/scheduled_tasks.lock`
- Empty-surfaces files: `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml`
- Follow-up epic: `fn-12-generalize-non-gregorian-calendar-schema`
