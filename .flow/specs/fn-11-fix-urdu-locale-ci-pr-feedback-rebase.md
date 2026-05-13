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
- **Test intent**: Validate that `ur-IN` *inherits* Hijri month data from `ur` when a user sets HijriCalendar. Only reachable on platforms where the runtime accepts that calendar for `ur-IN`. Guard with `Assert.Skip(...)` (xUnit v3) based on a type-check of `OptionalCalendars` content — `calendar is HijriCalendar`, not equality with a new instance. This is a runtime capability gate, not an acceptance weakening.
- **Ordinalizer**: `NumberWordSuffixOrdinalizer.cs:53` matches on `block.ExactReplacements` for a negative magnitude but then delegates to `INumberToWordsConverter.ConvertToOrdinal` — so positive irregulars come from `surfaces.ordinal.numeric` while negatives come from `surfaces.number.words.ordinal`. Fix: use the matched `negExact` to compose the negative ordinal, using the converter only to obtain the locale's negative prefix (by comparing `converter.Convert(number)` and `converter.Convert(magnitude)` to extract the prefix, then concatenating prefix + `negExact`). If prefix extraction fails (negative cardinal does not end with positive cardinal), throw `InvalidOperationException` — do not silently drop the exact replacement.
- **Ordinalizer regression test**: The current Urdu YAML has identical exact replacements in both `surfaces.number.words.ordinal` and `surfaces.ordinal.numeric`, so a locale-driven test will not expose the bug. The regression test MUST create a deliberate divergence — instantiate `NumberWordSuffixOrdinalizer` directly with custom `ExactReplacements` containing a sentinel value, then assert with exact equality (`Assert.Equal`) that the negative path returns the locale's negative prefix + the ordinalizer's sentinel.
- **Namespace**: Test files use folder-code namespaces (e.g. `Humanizer.Tests.Localisation.is`). `IcelandicGenderedOrdinalTests.cs` uses `Humanizer.Tests.Localisation.Icelandic` — rename to `Humanizer.Tests.Localisation.@is` in source (`is` is a C# reserved keyword requiring `@` prefix; the compiled/logical namespace is `Humanizer.Tests.Localisation.is`).
- **Lock file**: `.claude/scheduled_tasks.lock` is machine-specific state, accidentally committed. Remove from tree and add `/.claude/*.lock` to `.gitignore` (single root-scoped pattern covers current and future lock files).
- **Empty surfaces**: `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml` carry `surfaces: {}` with no overrides. Make the key optional for variants with no overrides and drop the empty block. `surfaces: null` should remain invalid — `surfaces` must be a mapping when present. When absent AND `variantOf` is set, treat as empty mapping.
- **LegacyLocaleMigration**: `LegacyLocaleMigration.ConvertToCanonicalYaml` currently emits `surfaces: {}` for inherits-only legacy input. Update to omit `surfaces` entirely when `variantOf` is set and no surfaces exist, consistent with the new canonical form.

## Task graph
```
fn-11.1 (rebase + ICU fix)
  ├── fn-11.2 (lock file + namespace)  ─┐
  ├── fn-11.3 (ordinalizer fix)         ├── fn-11.5 (closeout: verify + push)
  └── fn-11.4 (surfaces optional)      ─┘
```
fn-11.2, fn-11.3, fn-11.4 are parallelizable after fn-11.1. fn-11.5 runs after all three complete.

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
- [ ] All three TFMs (net48, net8.0, net10.0) pass the full Humanizer.Tests suite (net48 execution on Windows host or CI).
- [ ] `HijriCalendar_UrIn_InheritsHijriMonths` is executable where the runtime allows (type-checked via `calendar is HijriCalendar`), or skipped with a clear runtime-capability message on platforms where ICU rejects the calendar — not weakened or removed.
- [ ] `.claude/scheduled_tasks.lock` is removed from the repo and `/.claude/*.lock` in `.gitignore` prevents reintroduction.
- [ ] `IcelandicGenderedOrdinalTests.cs` uses `namespace Humanizer.Tests.Localisation.@is;` (compiles to `Humanizer.Tests.Localisation.is`, matching folder convention).
- [ ] Negative and positive irregular ordinals in `NumberWordSuffixOrdinalizer` both source from the ordinalizer's `ExactReplacements`; prefix extraction failure throws `InvalidOperationException`; covered by a regression test with deliberate divergence and exact equality assertion.
- [ ] `surfaces:` is optional for variants without overrides; `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml` no longer contain `surfaces: {}`; non-variant locales without `surfaces` still fail validation.
- [ ] `LegacyLocaleMigration.ConvertToCanonicalYaml` omits `surfaces` for inherits-only variants (instead of emitting `surfaces: {}`).
- [ ] Source generator tests updated: non-variant missing surfaces still fails; variant with `variantOf` and no `surfaces` succeeds and inherits parent; checked-in `ur-PK`, `ur-IN`, `zh-CN` round-trip without structural drift.
- [ ] Branch is rebased onto latest `origin/main`; final `git push --force-with-lease` happens in fn-11.5 after all tasks complete.
- [ ] `dotnet format --verify-no-changes` passes.

## Early proof point
Task `fn-11.1` (rebase + CI fix) validates the fundamental approach — once CI is green on the updated base, the feedback and cleanup fixes proceed against a stable branch. If the CI fix doesn't stabilize, re-evaluate whether the test needs redesign (different inheritance proof path) before touching feedback items.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | All TFMs pass Humanizer.Tests (final) | fn-11.5 (closeout) | — |
| R2  | Hijri ur-IN test fixed without weakening | fn-11.1 | — |
| R3  | `.claude/scheduled_tasks.lock` removed + gitignored | fn-11.2 | — |
| R4  | Icelandic namespace renamed to `@is` | fn-11.2 | — |
| R5  | Ordinalizer negative irregulars unified | fn-11.3 | — |
| R6  | Branch rebased on latest main | fn-11.1 | — |
| R7  | `dotnet format` passes | fn-11.1, fn-11.2, fn-11.3, fn-11.4, fn-11.5 | — |
| R8  | `surfaces:` optional for variants w/o overrides; empty blocks removed | fn-11.4 | — |
| R9  | Non-Gregorian calendar schema generalization | — | Deferred to epic `fn-12` — broader refactor, separate PR |
| R10 | Final integration verification (all TFMs, build, format, push) | fn-11.5 | — |
| R11 | Source generator tests updated for surfaces-optional | fn-11.4 | — |
| R12 | LegacyLocaleMigration updated for no-delta variants | fn-11.4 | — |

## References
- PR: https://github.com/Humanizr/Humanizer/pull/1720
- Failing test: `tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs:96-108`
- Ordinalizer: `src/Humanizer/Localisation/Ordinalizers/NumberWordSuffixOrdinalizer.cs:53`
- Namespace: `tests/Humanizer.Tests/Localisation/is/IcelandicGenderedOrdinalTests.cs:3`
- Lock file: `.claude/scheduled_tasks.lock`
- Empty-surfaces files: `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml`
- Follow-up epic: `fn-12-generalize-non-gregorian-calendar-schema`
