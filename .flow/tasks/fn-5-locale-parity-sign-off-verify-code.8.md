# fn-5-locale-parity-sign-off-verify-code.8 Run net8.0 tests locally; restore strict net8.0 acceptance; re-record fn-5.5 evidence

## Description

The previous fn-5.5 sign-off pass deferred the net8.0 test run to CI on the false premise that "the .NET 8.0 SDK is not installed on this machine (only .NET 10.0.2 is available)" (`tools/verification-signoff.md:100`). This was wrong: `dotnet --list-sdks` reports both `8.0.419` and `10.0.102` installed, with `Microsoft.NETCore.App 8.0.25` and `Microsoft.AspNetCore.App 8.0.25` runtimes also present. The net8.0 test target is reachable on this developer machine and must actually be run before fn-5 can be signed off.

The mistake was then compounded by editing the governing spec (`commit d40bbbe6 fix(signoff): update epic spec to allow net8.0 CI deferral`) to soften the R14 acceptance criterion from `"net10.0 AND net8.0 pass"` to `"net10.0 passes locally; net8.0 passes locally OR is deferred to CI when SDK unavailable"`. **That spec edit must be reverted.** Specs are tightened by sign-off work, never loosened.

This task does four things:
1. **Actually run** `dotnet test --framework net8.0` locally and capture the output as committed evidence.
2. **Revert the deferral language** in `.flow/specs/fn-5-locale-parity-sign-off-verify-code.md` so the R14 acceptance bullet and the R14 requirement-coverage row both state `net10.0 AND net8.0 must pass` with no escape clause.
3. **Re-record fn-5.5 task evidence** (`.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.json` + `.md`) so the closed task evidence reflects an actual net8.0 pass instead of a "deferred" stub.
4. **Reconcile fn-5.7 task metadata** (`.flow/tasks/fn-5-locale-parity-sign-off-verify-code.7.md`) so the title, Files list, and Acceptance match the shipped `#if NET5_0_OR_GREATER` guard implementation rather than the abandoned Polyfill path.

This task does **not** edit `tools/verification-signoff.md` (handed off to fn-5.9, which reconciles the whole sign-off doc together with the pitfalls cleanup).

**Size:** M (3 files + 1 test run + 1 task-evidence update; tightly coupled because they all describe the same change).

**Files:**
- `.flow/specs/fn-5-locale-parity-sign-off-verify-code.md` (revert deferral language in two locations: R14 acceptance bullet near line ~150 and R14 requirement-coverage row near line ~178; reconcile Scope/out-of-scope fn-5.7 references to match shipped #if guard fix)
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.json` (re-record evidence with net8.0 pass output; status stays `done`)
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.md` (sync `## Done summary` and `## Evidence` sections to match the JSON update)
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.7.md` (reconcile title, Files, and Acceptance to match the shipped #if guard implementation, not the abandoned Polyfill path)
- (no changes to `tools/verification-signoff.md` — that is fn-5.9's scope)

## Approach

**Step 1 — Run the test suite for both modern targets locally.** The .NET 8 SDK 8.0.419 is installed at `/usr/local/share/dotnet/sdk` and the .NET 8 runtime 8.0.25 is installed at `/usr/local/share/dotnet/shared/Microsoft.NETCore.App`. `global.json` pins SDK 10.0.100 with `rollForward: latestFeature`, which still allows targeting `net8.0` via the multi-targeted test project. Run:

```bash
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 -c Release
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0  -c Release
```

Capture the full output (test count, failed count, duration) for both runs. If either fails, **stop and fix the failure** before any spec edits — do not weaken the criterion to accommodate failure.

**Step 2 — Revert the spec deferral language.** Open `.flow/specs/fn-5-locale-parity-sign-off-verify-code.md` and locate the two passages introduced by commit `d40bbbe6`:

- The R14 acceptance bullet currently reads: `` - [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0` passes locally, OR is deferred to CI when the .NET 8 SDK is not installed locally (overrides are framework-agnostic, generated at build time; deferral must be explicitly documented in the sign-off section) ``. Replace the entire line with the strict pre-d40bbbe6 form: `` - [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0` passes ``.

- The R14 requirement-coverage row currently reads: ``| R14 | `dotnet format --verify-no-changes` passes AND `net10.0` test suite passes locally; `net8.0` passes locally or deferred to CI when SDK unavailable (net48 deferred to fn-4) | .5 | — |``. Replace the description cell with the strict form: ``` `dotnet format --verify-no-changes` passes AND full modern-target test suite passes on **both** `net10.0` and `net8.0` (run locally; no deferral) ```. Update the `Task(s)` cell to `.5, .8` since fn-5.8 now owns the actual run.

Use `git show d40bbbe6 -- .flow/specs/fn-5-locale-parity-sign-off-verify-code.md` to confirm the exact pre-edit text, then revert each line one at a time with the `Edit` tool. Do **not** introduce any new escape clauses; do **not** soften the language elsewhere in the spec.

Also update R17 in the requirement-coverage table: `Fix fn-4 net48 test-suite blocker` previously read `Out of scope — tracked as fn-4; sign-off only documents it as known open item` — change the gap-justification cell to `Subsumed by fn-5.7; fn-4 closed superseded in fn-5.9` and add `.7, .9` to the Task(s) cell. (R17 stays in the table; only its mapping changes.)

**Step 3 — Re-record fn-5.5 task evidence.** Open `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.json` and locate the `evidence.scan_details` block. The current value records "macOS net8.0: DEFERRED (.NET 8 not installed)" — replace with the actual net8.0 test run results from Step 1 (test count, failed count, duration, exit code, capture command). Add the net8.0 pass row to the verification grid. Mirror the same updates in `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.md` `## Done summary` and `## Evidence` sections so the markdown spec stays in sync with the JSON.

The fn-5.5 task `status` remains `done`. We are correcting the evidence captured during sign-off, not re-opening the task. Note in the new evidence that this re-record happened in fn-5.8 with a back-reference to the fn-5.8 commit.

## Investigation targets

**Required** (read before coding):
- `tools/verification-signoff.md:96-110` — current "macOS net8.0: DEFERRED TO CI" section (do not edit in this task; just understand what's wrong)
- `.flow/specs/fn-5-locale-parity-sign-off-verify-code.md` lines ~150 (R14 acceptance bullet) and ~178 (R14 requirement-coverage row) — the two passages to revert
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.json` `evidence.scan_details` block — what to update
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.md` `## Done summary` and `## Evidence` sections — what to update
- `git show d40bbbe6` — confirms exact pre-edit text of the two passages to revert
- `global.json` and `dotnet --list-sdks` / `dotnet --list-runtimes` output — confirms .NET 8 SDK + runtime are installed

**Optional** (reference as needed):
- `commit 3d0c0c0a` (fn-5.5 original sign-off) — for the original task-evidence shape

## Key context

- **The deferral was a process failure, not a tooling gap.** The .NET 8 SDK is installed; the test command works; the previous sign-off pass invented a constraint that did not exist. Future sign-off work in this repo must verify constraint claims (`dotnet --list-sdks`, `which dotnet`, etc.) before deferring anything.
- **Spec edits in sign-off work go in one direction only: tighter.** Adding `OR deferred` is forbidden. Adding `must` / `AND` / removing escape clauses is fine. This applies to *every* spec in the repo, not just fn-5.
- **fn-5.5's status stays `done`.** We are correcting the captured evidence after the fact. The cleanup happens here in fn-5.8 with full git history of both the original (incorrect) close and the correction.
- **The macOS test runs cover only the macOS host.** Windows and Linux net8.0 / net10.0 still require their own host runs to *verify cross-platform agreement*, but those runs are out of scope for this task — they were already captured as "CI verification" rows in `tools/verification-signoff.md` and the strict R14 only requires both modern targets to pass on the developer's host. fn-5.9 will tighten the cross-platform language in the sign-off doc; this task's job is just to undo the one false claim about net8.0 being unreachable locally.

## Acceptance

- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 -c Release` exits 0; full test count + 0 failures captured in task evidence verbatim
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 -c Release` exits 0; full test count + 0 failures captured in task evidence verbatim
- [ ] `.flow/specs/fn-5-locale-parity-sign-off-verify-code.md` R14 acceptance bullet reads exactly `` - [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0` passes `` (strict, no `OR deferred`, no `if SDK unavailable`, no `must be documented in the sign-off section`)
- [ ] `.flow/specs/fn-5-locale-parity-sign-off-verify-code.md` R14 requirement-coverage row description cell reads exactly ``` `dotnet format --verify-no-changes` passes AND full modern-target test suite passes on **both** `net10.0` and `net8.0` (run locally; no deferral) ``` and the Task(s) cell reads `.5, .8`
- [ ] `.flow/specs/fn-5-locale-parity-sign-off-verify-code.md` R17 row gap-justification cell reads `Subsumed by fn-5.7; fn-4 closed superseded in fn-5.9` and Task(s) cell includes `.7, .9`
- [ ] `grep -n "deferred to CI when\|OR is deferred\|deferred to CI when SDK unavailable" .flow/specs/fn-5-locale-parity-sign-off-verify-code.md` returns zero matches
- [ ] `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.json` `evidence.scan_details` no longer claims `DEFERRED (.NET 8 not installed)`; the new value records (test count, failures, duration, command) verbatim from the Step 1 run
- [ ] `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.json` `status` stays `done` (this is an evidence correction, not a re-open)
- [ ] `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.md` `## Done summary` and `## Evidence` sections updated to mirror the JSON changes; both files reference the fn-5.8 correction commit by short hash
- [ ] `grep -n "DEFERRED.*not installed\|net8.0.*deferred\|deferred to CI" .flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.md` returns zero matches
- [ ] `dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic` passes
- [ ] `flowctl validate --epic fn-5-locale-parity-sign-off-verify-code` reports `Valid: True`
- [ ] Task evidence records: full `dotnet --list-sdks` and `dotnet --list-runtimes` output as the proof that the deferral premise was false; the two test-run capture commands and their full output

## Done summary
Ran net8.0 and net10.0 test suites locally (both 38908 passed, 0 failed), restored strict net8.0 acceptance in the fn-5 epic spec (reverted deferral escape clause from commit d40bbbe6), re-recorded fn-5.5 task evidence with actual test run output and SDK proof, reconciled fn-5.7 task metadata to match the shipped #if guard implementation, and narrowed fn-5.5 done summary scope claim.

## Evidence
- Commits: dd476b9a, 03688f88, 63b29123

### Verbatim: dotnet --list-sdks
```
8.0.419 [/usr/local/share/dotnet/sdk]
10.0.102 [/usr/local/share/dotnet/sdk]
```

### Verbatim: dotnet --list-runtimes
```
Microsoft.AspNetCore.App 8.0.25 [/usr/local/share/dotnet/shared/Microsoft.AspNetCore.App]
Microsoft.AspNetCore.App 10.0.2 [/usr/local/share/dotnet/shared/Microsoft.AspNetCore.App]
Microsoft.NETCore.App 8.0.25 [/usr/local/share/dotnet/shared/Microsoft.NETCore.App]
Microsoft.NETCore.App 10.0.2 [/usr/local/share/dotnet/shared/Microsoft.NETCore.App]
```

### Verbatim: dotnet test --framework net10.0 -c Release
```
Test run summary: Passed!
  total: 38908
  failed: 0
  succeeded: 38908
  skipped: 0
  duration: 6s 828ms
```

### Verbatim: dotnet test --framework net8.0 -c Release
```
Test run summary: Passed!
  total: 38908
  failed: 0
  succeeded: 38908
  skipped: 0
  duration: 10s 667ms
```

### Verbatim: dotnet format Humanizer.slnx --verify-no-changes
```
Formatted 0 of 1596 files.
```

### Verbatim: flowctl validate --epic fn-5-locale-parity-sign-off-verify-code
```
Validation for fn-5-locale-parity-sign-off-verify-code:
  Tasks: 9
  Valid: True
```

- PRs:
