# fn-3-hard-code-locale-overrides-where-icu.6 Verify cross-platform consistency via probe tools

## Description
After the schema + runtime changes (tasks .3 and .4) land, verify byte-identical output across all 4 probe environments (macOS, Linux, Windows net10, Windows net48) using the probe tool, AND ensure the actual test suite produces 0 locale-related failures on each platform.

**This task is a GATE, not a report.** It must produce committed artifacts and explicit signoff before the epic can be closed.

**Size:** S
**Files:**
- `tools/probe-*-after.json` (new committed baseline files)
- `tools/compare-probes.cs` (existing — may need minor update to compare before/after)
- `tools/verification-signoff.md` (NEW: structured signoff document)

## Approach
1. Rebuild on each platform
2. Run the probe: `dotnet run tools/locale-probe.cs --json > tools/probe-<platform>-after.json`
3. For net48 on Windows: `tools/locale-probe-net48/bin/Release/net48/locale-probe-net48.exe --json > tools/probe-windows-net48-after.json`
4. Run the comparison: `dotnet run tools/compare-probes.cs` — output must show 100% agreement for overridden data points
5. Run the actual test suite on each platform:
   - macOS net10.0 and net8.0
   - Linux net10.0 and net8.0
   - Windows net10.0 and net8.0
   - Windows net48 (if Humanizer.Tests.csproj still targets net48 after the pre-existing `Enum.GetValues<T>()` issue is fixed)
6. Produce a structured signoff document

## Gate criteria (ALL must pass)

### Committed artifacts
- [ ] `tools/probe-macos-after.json` committed
- [ ] `tools/probe-linux-after.json` committed
- [ ] `tools/probe-windows-net10-after.json` committed
- [ ] `tools/probe-windows-net48-after.json` committed

### Deterministic comparison
- [ ] `compare-probes.cs` output shows 100% platform agreement for overridden locales (bn, fa, he, ku, zu-ZA, ta) on `DateToOrdinalWords` / `DateOnlyToOrdinalWords` output
- [ ] `compare-probes.cs` output shows 100% platform agreement for overridden locales (ar, ku, fr-CH) on decimal separator output

### Test suite signoff (hard gate — modern .NET)
For each environment, document exact pass/fail counts:
- [ ] macOS net10.0: 0 failures in `DateToOrdinalWords_*`, `DateOnlyToOrdinalWords_*`, `UsesExpectedByteSizeHumanizeSymbols`
- [ ] macOS net8.0: same
- [ ] Linux net10.0: same (if not locally runnable, document how verified — CI, SSH, container)
- [ ] Windows net10.0: same

### Test suite signoff (documented follow-up — net48)
Windows net48 test-suite execution is **not a hard gate** for this epic due to a pre-existing `Enum.GetValues<GrammaticalGender>()` blocker in `LocaleTheoryMatrixCompletenessTests.cs:439` that is out of scope. Instead:
- [ ] Windows net48 **probe output** captured and committed (this IS a hard gate — proves the override data is correct)
- [ ] Document the net48 test-suite blocker and file it as a separate issue/epic
- [ ] Once the blocker is resolved, net48 test-suite signoff becomes a follow-up verification item

### Regression check
- [ ] No test that passed before tasks .3/.4 now fails on any platform (macOS/Linux/Windows net10.0/net8.0)
- [ ] `compare-probes.cs` agreement percentage for non-overridden locales has not decreased

### Manual release checklist (if not all environments can be automated)
For any environment not covered by CI:
- [ ] Named person responsible for manual verification
- [ ] Exact commands to run
- [ ] Expected output documented
- [ ] Deadline for manual signoff

## Investigation targets
**Required:**
- `tools/locale-probe.cs` — the probe tool built earlier
- `tools/locale-probe-net48/` — the net48 variant
- `tools/compare-probes.cs` — the comparison tool

## Key context
- The "before" baselines (`tools/probe-macos.json`, `tools/probe-linux.json`, etc.) are already in the repo
- For overridden locales, output MUST be byte-identical across all 4 platforms
- For non-overridden locales, platform differences are expected and acceptable (short-dates, long-dates, etc. which we're not overriding)
- If a test that passed before now fails, that's a regression that BLOCKS epic closure
- The pre-existing `Enum.GetValues<GrammaticalGender>()` issue in `LocaleTheoryMatrixCompletenessTests.cs:439` blocks net48 test runs; note it but do NOT fix it in this task

## Acceptance
- [ ] All 4 `probe-*-after.json` files committed to the repo
- [ ] `tools/verification-signoff.md` committed with structured signoff for all environments
- [ ] All hard gate criteria above pass (probes, modern .NET test suites, regression check)
- [ ] net48 test-suite blocker filed as separate issue
- [ ] Task completion report includes before/after agreement matrix
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
