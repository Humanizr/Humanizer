# fn-5-locale-parity-sign-off-verify-code.9 Reconcile sign-off doc, remove improper pitfall entries, close fn-4 superseded

## Description

After fn-5.7 fixes the net48 build break and fn-5.8 actually runs the net8.0 test suite locally, the sign-off doc and the project's pitfall memory both still describe a world that no longer exists:

- `tools/verification-signoff.md` has a "macOS net8.0: DEFERRED TO CI" section, a Linux/Windows-net8.0 "CI VERIFICATION" section, a Windows-net48 "DOCUMENTED FOLLOW-UP" section, a "Final Verdict" section enumerating "Explicitly deferred to CI" items, and a "Cross-platform deferred items" outstanding-items table — all of which mis-state the post-fn-5.7/fn-5.8 reality.
- `.flow/memory/pitfalls.md` lines 61-62 explicitly endorse "update the governing spec to explicitly allow the deferral" as a best practice. That entry is wrong and must be removed. Two adjacent entries (lines 43-44 and 52-53) encode similar "deferral is acceptable" patterns that contradict the project's no-deferrals rule and must be reframed.
- `fn-4-fix-net48-test-suite-blocker` is still open with zero tasks. Its work has been subsumed by fn-5.7. The epic must be closed with a one-line "superseded by fn-5.7" note.

This task is a pure documentation/state-reconciliation task. No code changes. It runs after both fn-5.7 (net48 build green) and fn-5.8 (net8.0 evidence captured) have landed, so the new doc text can cite real commits and real test results.

**Size:** M (3 files + 1 epic close, with substantial rewrites of the sign-off doc).

**Files:**
- `tools/verification-signoff.md` (rewrite the post-execution / Final Verdict / outstanding-items sections to reflect actual local verification)
- `.flow/memory/pitfalls.md` (delete line 61-62; reframe lines 43-44 and 52-53)
- `.flow/specs/fn-4-fix-net48-test-suite-blocker.md` (append "Superseded by fn-5.7" note at top — see Step 4)
- `.flow/epics/fn-4-fix-net48-test-suite-blocker.json` (status → `done`, completion review note: superseded)

## Approach

**Step 1 — Reconcile `tools/verification-signoff.md`.**

Audit the entire file for stale "deferred" / "DEFERRED TO CI" / "blocker" / "fn-4" / "Enum.GetValues" prose and rewrite each occurrence to match the post-fn-5.7/fn-5.8 reality:

- **Section headed `### macOS net8.0: DEFERRED TO CI`** (current line ~98): rename to `### macOS net8.0: PASS` and replace the body with the actual test run capture from fn-5.8 (test count, failures, duration, command, commit hash). The "DEFERRED TO CI" framing is gone entirely.
- **Section headed `### Linux net10.0 / net8.0: CI VERIFICATION`** (current line ~107): keep the section, but rename to `### Linux net10.0 / net8.0: REQUIRES LINUX HOST` and rewrite the body to honestly state that running these on Linux is a host-OS requirement (not a deferral or a failure). Cite the existing CI workflow that runs them, and remove any prose that frames Linux test execution as an "outstanding item" — it is the normal way you verify Linux behavior, not a gap.
- **Section headed `### Windows net10.0 / net8.0: CI VERIFICATION`** (current line ~115): same treatment — rename to `### Windows net10.0 / net8.0: REQUIRES WINDOWS HOST`, frame as host-OS requirement, cite the existing CI workflow.
- **Section headed `### Windows net48: DOCUMENTED FOLLOW-UP`** (current line ~123): rename to `### Windows net48: REQUIRES WINDOWS HOST (build now green on all platforms)` and rewrite the body. Drop all references to "blocked by Enum.GetValues<T>()", "fn-4", and "follow-up". State plainly: the test project compiles for net48 on every platform now (verified in fn-5.7); test execution still requires a Windows host because .NET Framework 4.8 is Windows-only; this is identical to the Linux/Windows host requirement above. Cite the fn-5.7 commit hash.
- **`### Verification status grid`** (current line ~196-212 area): the row `macOS net8.0: 0 failures | DEFERRED (.NET 8 not installed)` becomes `macOS net8.0: 0 failures | PASS` with the actual count. The row `net48 blocker documented | PASS (filed as fn-4-...)` becomes `net48 build green on all platforms | PASS (verified in fn-5.7; test execution requires Windows host)`. The row `probe-windows-net48-after.json committed | PASS (copy of before; not re-run with extended probe — Windows unreachable)` is unchanged in this task (probe coverage is not in scope here).
- **`### Final Verdict — Cross-Platform Verification Status`** section (current line ~216 area): the "Explicitly deferred to CI" subsection currently lists 4 items. Remove `macOS net8.0` from that list (it's now PASS). Reframe the remaining 3 items (`Linux net10.0 / net8.0`, `Windows net10.0 / net8.0`, `Windows net48`) under a renamed subsection `**Requires non-macOS host (verified in CI):**` so they are not framed as "deferred" — they are simply runs that occur on a different host. The "This sign-off does not claim full cross-platform verification" sentence stays but the supporting prose changes to say cross-platform runs happen in CI as a normal part of the workflow, not as deferred items.
- **`### Outstanding Cross-Platform Items`** table (current line ~240 area): the `macOS net8.0 | CI pipeline | ... | Next CI run` row gets removed entirely (it's now done locally). The other rows are reframed identically: `CI pipeline` is not a "deferred location", it is the normal location for that host's run.
- **Any inline `(fn-4 net48 blocker)` mentions** elsewhere in the doc: replace with `(net48 test execution requires Windows host)` and remove the fn-4 reference.

The principle: **the doc should now read as honest local verification on macOS for both net10.0 and net8.0, with non-macOS hosts running in CI as a normal part of the workflow — not as a list of unmet deferred items.**

After rewriting, run `grep -n "DEFERRED\|deferred\|blocker\|fn-4\|Enum\.GetValues" tools/verification-signoff.md`. The only remaining matches should be the ku/ta/zu-ZA decimal-separator / calendar.months audit prose where "deferred" appears in a different context (probe-shape narrative) — verify each remaining match is contextually correct, not a leftover from the old framing.

**Step 2 — Reconcile `.flow/memory/pitfalls.md`.**

Three entries to fix:

- **Line 61-62** (`When a sign-off or gate summary claims all acceptance criteria are met but one criterion is deferred, update the governing spec to explicitly allow the deferral -- do not only soften the summary language while leaving the spec as a hard requirement`): **delete entirely**. This entry directly contradicts the project's no-deferral rule. Replace with a new entry that captures the inverse rule:

  ```
  ## 2026-04-10 manual [pitfall]
  When a sign-off or gate criterion cannot be met as written, fix the underlying work or block the gate -- do not weaken the criterion, do not edit the spec to add escape clauses ("OR deferred", "if SDK unavailable", "if reachable"), and do not soften the summary language to obscure the gap. Specs may only be tightened by sign-off work, never loosened.
  ```

- **Line 52-53** (`When documenting verification status in gate summaries, match claim strength to actual evidence scope -- do not state cross-platform agreement as PASS when only one platform was exercised; use DEFERRED for untested platforms`): the first half is correct (match claim strength to evidence) but the prescription "use DEFERRED for untested platforms" is wrong. **Reframe** to: `When documenting verification status in gate summaries, match claim strength to actual evidence scope -- do not state cross-platform agreement as PASS when only one platform was exercised. State the verified host explicitly ("PASS on macOS net10.0 + net8.0 (local)") and identify other-host runs by their host requirement, not as "DEFERRED" -- non-macOS host runs are CI verification, not deferred items.`

- **Line 43-44** (`When a verification gate depends on a known external blocker (e.g., net48 test suite blocked by Enum.GetValues<T>), either make it a tracked dependency or explicitly downgrade to documented follow-up — never leave a hard gate that cannot close`): the example is now stale (the "blocker" was a missing Polyfill ref, fixable in <5 minutes — there was no real "external blocker"). **Reframe** to: `When a verification gate appears to depend on an "external blocker", verify the blocker actually exists and is genuinely external before accepting it -- many "blockers" are actually local fixable issues (missing package reference, missing SDK, missing config) that can be resolved as part of the gate work. Never accept a deferral on the basis of an unverified blocker claim.`

The pitfall file's existing line ordering and date headers should be preserved; only the body of the three entries above changes (one delete + new entry, two reframings). Other entries are unchanged.

**Step 3 — Close `fn-4-fix-net48-test-suite-blocker` as superseded.**

Use `flowctl epic complete fn-4-fix-net48-test-suite-blocker` (or the equivalent JSON status update if `complete` is not supported for zero-task epics) with a completion review note that reads:

```
Superseded by fn-5-locale-parity-sign-off-verify-code.7. The net48 build break was caused by the test project missing a Polyfill PackageReference; fn-5.7 added it and verified `dotnet build -f net48` succeeds on all platforms. fn-4 carried no tasks and required no separate work; closing as superseded with no executed tasks. See fn-5.7 commit <hash>.
```

Also add a one-line note at the top of `.flow/specs/fn-4-fix-net48-test-suite-blocker.md` (above the existing `# Title`):

```
> **Status: Superseded by fn-5-locale-parity-sign-off-verify-code.7 (closed in fn-5.9).** The Polyfill 9.18.0 package — already a library dependency — exposes `Enum.GetValues<TEnum>()` via C# 14 `extension(Enum)` syntax. The fix was a one-line PackageReference addition to the test project; no source change was required.
```

Verify with `flowctl show fn-4-fix-net48-test-suite-blocker --json` that the status is `done` and the completion note is recorded.

**Step 4 — Run the full validation gate.**

Re-run the fn-5.5 scan battery + the new fn-5.8 test runs in fresh evidence, captured in this task's done-summary:

```bash
# net48 framing audit (must return zero)
grep -rn "see fn-4\|tracked as fn-4\|blocked.*Enum\.GetValues" CLAUDE.md AGENTS.md tools/verification-signoff.md .flow/memory/

# Deferral language audit (must return zero outside of explicitly-allowed contexts)
grep -rn "DEFERRED TO CI\|deferred to CI\|OR is deferred\|deferred to CI when SDK unavailable" .flow/specs/fn-5-locale-parity-sign-off-verify-code.md tools/verification-signoff.md .flow/memory/pitfalls.md

# Format + tests
dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 -c Release
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0  -c Release
dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net48
```

Capture verbatim output for each in task evidence.

## Investigation targets

**Required** (read before coding):
- `tools/verification-signoff.md` — the entire file (this task rewrites multiple sections; need full context)
- `.flow/memory/pitfalls.md` lines 40-65 — the three entries to fix
- `.flow/specs/fn-4-fix-net48-test-suite-blocker.md` — the spec being closed
- `.flow/epics/fn-4-fix-net48-test-suite-blocker.json` — the epic JSON to update
- `commit d40bbbe6` — for context on what was wrong with the prior spec edit

**Optional** (reference as needed):
- `flowctl epic complete --help` — confirm CLI for closing an epic with a note
- `commit 3d0c0c0a` (fn-5.5 original sign-off) — for the original sign-off doc shape (so the rewrite preserves correct structure)

## Key context

- **Cross-platform runs in CI are not "deferrals".** Running net8.0 / net10.0 on Linux requires a Linux host. Running them on Windows requires a Windows host. Neither is a "deferral" in the pejorative sense that started this whole correction — they are just where those runs happen. The sign-off doc must distinguish between "this run was skipped because the work wasn't done" (a deferral, never acceptable) and "this run happens on a different host as part of the standard CI matrix" (normal). The first must never appear; the second is fine to document as such.
- **Scope discipline.** This task touches doc/state files only. Do not edit any source code, csproj, or YAML files. If any of those need fixing as a side effect, file a follow-up — do not fold them in here.
- **The fn-4 closure carries history.** Even though fn-4 had no tasks, it had a spec, a plan-review status, and a created/updated timestamp. Closing it as superseded preserves that history; deleting it would lose it. Leave the file in place; just mark it done with the supersede note.
- **No new pitfall entries beyond the one in Step 2.** Resist the urge to add "lessons learned" entries about the prior sign-off failure. The single replacement entry (the no-loosening rule) is enough; any further "lessons" would be process-meta that future agents can derive from the rule itself.

## Acceptance

- [ ] `tools/verification-signoff.md` `### macOS net8.0` section is renamed `PASS` (not `DEFERRED TO CI`) and contains the actual test run output from fn-5.8 (test count, failures, duration, command, fn-5.8 commit hash)
- [ ] `tools/verification-signoff.md` no longer contains the strings `DEFERRED TO CI`, `DEFERRED (.NET 8 not installed)`, `Next CI run`, or `Explicitly deferred to CI` in the post-execution / verdict / outstanding-items sections
- [ ] `tools/verification-signoff.md` no longer contains `(fn-4 net48 blocker)`, `tracked as fn-4`, or `blocked by Enum.GetValues<T>()`. Replacement prose accurately states net48 build is green on all platforms and test execution requires a Windows host.
- [ ] `tools/verification-signoff.md` `### Windows net48` section title contains the phrase `build now green on all platforms` (or equivalent literal that signals fn-5.7 fixed it)
- [ ] `tools/verification-signoff.md` `### Verification status grid` row for macOS net8.0 reads `PASS` with the actual test count from fn-5.8 (not `DEFERRED`)
- [ ] `tools/verification-signoff.md` `### Outstanding Cross-Platform Items` table no longer has a `macOS net8.0` row
- [ ] `grep -n "DEFERRED TO CI\|DEFERRED (.NET 8 not installed)\|Next CI run\|Explicitly deferred to CI" tools/verification-signoff.md` returns zero matches
- [ ] `grep -n "see fn-4\|tracked as fn-4\|blocked.*Enum\.GetValues" tools/verification-signoff.md` returns zero matches
- [ ] `.flow/memory/pitfalls.md` no longer contains the line `update the governing spec to explicitly allow the deferral`
- [ ] `.flow/memory/pitfalls.md` contains the new no-loosening rule entry with the wording from Step 2 (or equivalent that captures: fix the work, never weaken the criterion, never add escape clauses, specs may only be tightened by sign-off work)
- [ ] `.flow/memory/pitfalls.md` line 52-53 entry no longer recommends `use DEFERRED for untested platforms`; reframed wording aligns with the no-deferrals rule
- [ ] `.flow/memory/pitfalls.md` line 43-44 entry no longer recommends `explicitly downgrade to documented follow-up`; reframed wording aligns with the no-deferrals rule
- [ ] `grep -n "use DEFERRED\|downgrade to documented follow-up\|update the governing spec to explicitly allow the deferral" .flow/memory/pitfalls.md` returns zero matches
- [ ] `fn-4-fix-net48-test-suite-blocker` epic status is `done` with completion review note containing `Superseded by fn-5-locale-parity-sign-off-verify-code.7` and a reference to the fn-5.7 commit hash
- [ ] `.flow/specs/fn-4-fix-net48-test-suite-blocker.md` has a one-line `> **Status: Superseded by ...**` blockquote at the top, above the `# Title` heading
- [ ] `flowctl show fn-4-fix-net48-test-suite-blocker --json` reports `status: done` and the supersede note
- [ ] `flowctl show fn-5-locale-parity-sign-off-verify-code --json` reports all 9 tasks done and 0 errors from `flowctl validate --epic fn-5-locale-parity-sign-off-verify-code`
- [ ] `dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic` passes
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 -c Release` passes (re-verification, no regression)
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 -c Release` passes (re-verification, no regression)
- [ ] `dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net48` exits 0 with 0 errors and 0 warnings (re-verification of fn-5.7 fix)
- [ ] Task evidence captures verbatim output from all three test/build commands above and the four grep commands listed in Step 4

## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
