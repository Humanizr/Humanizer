# fn-11-fix-urdu-locale-ci-pr-feedback-rebase.1 Rebase on main + fix UrduHijriDateTests ur-IN ICU failure

## Description
Rebase `feat/urdu-locale` onto latest `origin/main` and fix the CI failure in `UrduHijriDateTests.HijriCalendar_UrIn_InheritsHijriMonths` that occurs on net8.0 and net10.0 (ICU) but passes on net48 (NLS).

**Size:** S
**Files:**
- `tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs`
- (git rebase — no other file changes expected from main delta)

## Approach
1. `git fetch origin && git rebase origin/main` (only 1 commit ahead: `5eefe6aa` TrxReport bump — should be clean).
2. Fix the test: before setting `DateTimeFormat.Calendar = new HijriCalendar()` on the cloned `ur-IN` culture, check whether the runtime accepts it.
3. If `urInHijri.OptionalCalendars` does not contain a `HijriCalendar`, call `Assert.Skip("HijriCalendar is not valid for ur-IN on this runtime (ICU); inheritance is only reachable on NLS/net48.")` — xUnit v3 supports `Assert.Skip`. This is a runtime capability gate, not an acceptance loosening.
4. The rest of the test body is unchanged when reachable.

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
- [ ] Branch rebased onto latest `origin/main` (at or ahead of `5eefe6aa`).
- [ ] `UrduHijriDateTests.HijriCalendar_UrIn_InheritsHijriMonths` passes or is skipped with a runtime-capability reason on ICU platforms.
- [ ] Test still runs and passes on net48 (NLS) — no `#if NET48` guard around the method.
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` passes.
- [ ] `git push --force-with-lease origin feat/urdu-locale` succeeds.
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
