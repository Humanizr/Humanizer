# fn-11-fix-urdu-locale-ci-pr-feedback-rebase.1 Rebase on main + fix UrduHijriDateTests ur-IN ICU failure

## Description
Rebase `feat/urdu-locale` onto latest `origin/main` and fix the CI failure in `UrduHijriDateTests.HijriCalendar_UrIn_InheritsHijriMonths` that occurs on net8.0 and net10.0 (ICU) but passes on net48 (NLS).

**Size:** S
**Files:**
- `tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs`
- (git rebase — no other file changes expected from main delta)

## Approach
1. `git fetch origin && git rebase origin/main` (only 1 commit ahead: `5eefe6aa` TrxReport bump — should be clean).
2. Fix the test: before setting `DateTimeFormat.Calendar = new HijriCalendar()` on the cloned `ur-IN` culture, check whether the runtime accepts it using a **type-based check**:
   ```csharp
   if (!urInHijri.OptionalCalendars.Any(static calendar => calendar is HijriCalendar))
   {
       Assert.Skip("HijriCalendar is not valid for ur-IN on this runtime (ICU); inheritance is only reachable on runtimes that allow HijriCalendar for ur-IN.");
   }
   ```
   Do NOT compare with `new HijriCalendar()` via equality — use `is HijriCalendar` pattern match.
3. The rest of the test body is unchanged when reachable.
4. Push the rebased branch with `git push --force-with-lease origin feat/urdu-locale` to establish the new base for fn-11.2/3/4. Note: the *final* push after all tasks complete happens in fn-11.5 (closeout).

Do NOT:
- Use `#if NET48` to conditionally compile the test (keeps one source path).
- Delete the test or lower the assertion.
- Silently swallow the exception.

## Investigation targets
**Required:**
- `tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs:95-108` — the failing `UrIn` test and the adjacent passing `UrPk` test at 80-93 for reference shape.
- `tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs:6-11` — `CreateUrduHijriCulture()` (base `ur` works on ICU; used as a signal).

**Optional:**
- xUnit v3 `Assert.Skip` usage in this repo (grep for `Assert.Skip(` to confirm the API style used).

## Key context
- ICU on `.NET 8+` determines valid calendars per culture from ICU data; `ur-IN` does not include HijriCalendar in ICU's OptionalCalendars, unlike `ur` and `ur-PK`. Windows/NLS (net48) does. Net48 test stays green; net8/10 become skipped on non-Windows ICU runtimes with a clear reason.
- Parent branch already contains a merge from `main` (`e6a3d0e2`), so the rebase delta is tiny (`5eefe6aa` TrxReport bump). Use `git rebase origin/main`, resolve any trivial conflicts, force-push with `--force-with-lease`.

## Acceptance
- [x] Branch rebased onto latest `origin/main` as of task execution; record the resulting base/head SHA in evidence.
- [x] `UrduHijriDateTests.HijriCalendar_UrIn_InheritsHijriMonths` passes or is skipped with a runtime-capability reason on ICU platforms, using `calendar is HijriCalendar` type check.
- [x] Test still runs and passes on net48 (NLS) — no `#if NET48` guard around the method.
- [x] `dotnet format Humanizer.slnx --verify-no-changes` passes.
- [x] `git push --force-with-lease origin feat/urdu-locale` succeeds (establishes rebased base; final push is fn-11.5 closeout).

## Done summary
# fn-11.1 Summary: Rebase + fix UrduHijriDateTests ur-IN ICU failure

## What was done
1. **Rebased** `feat/urdu-locale` onto latest `origin/main` (base: `5eefe6aa`). Clean rebase — the prior merge commit `e6a3d0e2` was eliminated. New head: `2ba68a17`.
2. **Fixed** `HijriCalendar_UrIn_InheritsHijriMonths` test by adding an `Assert.Skip` guard that checks `urInHijri.OptionalCalendars.Any(static calendar => calendar is HijriCalendar)` before setting the Hijri calendar. On ICU platforms where ur-IN doesn't include HijriCalendar, the test is skipped with a clear runtime-capability message. On NLS (net48) and ICU platforms that include it, the test runs normally.
3. **Pushed** with `git push --force-with-lease origin feat/urdu-locale` to establish the rebased base for fn-11.2/3/4.

## Key decisions
- Used `calendar is HijriCalendar` pattern match (not equality with `new HijriCalendar()`) per spec
- No `#if NET48` guard — single code path
- Test body unchanged when reachable
## Evidence
- Commits:
- Tests:
- PRs: