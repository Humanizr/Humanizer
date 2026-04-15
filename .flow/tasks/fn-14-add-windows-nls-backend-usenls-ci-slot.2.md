# fn-14-add-windows-nls-backend-usenls-ci-slot.2 Relax #if NET48 guard on ur-IN + HijriCalendar test to include NLS matrix

## Description
Remove the `#if NET48` guard from `HijriCalendar_UrIn_InheritsHijriMonths` (`tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs:96`) once the NLS slot exists. The scenario is reachable on any NLS-backed runtime — net48 is no longer the only TFM with coverage.

**Size:** S
**Files:**
- `tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs`

## Approach

Two options depending on how fn-14.1 activates NLS:

- If NLS is selected via env var per CI slot: the test binary is the same everywhere, so the `#if NET48` wrapper is not the right guard. Use a runtime probe (`CultureInfo("ur-IN").OptionalCalendars.Any(c => c is HijriCalendar)`) at the top of the test and `Assert.Skip` otherwise — same as the pre-ifdef behavior. The NLS slot exercises it; ICU slots skip. This is functionally the same as before but now the skip is accurate across TFMs.
- If NLS is selected via runtimeconfig baked into the binary: gate on a project-level `#if HUMANIZER_NLS` constant defined only in the NLS build.

Prefer the runtime-probe approach; it needs no extra csproj plumbing.

## Investigation targets

**Required:**
- `tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs:80-113`
## Acceptance
- [ ] ur-IN + HijriCalendar test executes (not skips) on the NLS CI slot for net8.0 and net10.0
- [ ] The test continues to skip (cleanly, with a clear reason) on ICU slots
- [ ] No `#if NET48` guard remains on the test
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
