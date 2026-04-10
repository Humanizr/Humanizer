## Description

Final sign-off task: prepare the fn-2 proxy-close mapping, run all residual/parity/regression scans, gate the fn-2 closure on scan success, execute the fn-2 close, and append a final sign-off section to `tools/verification-signoff.md`. This task gates the whole epic — the scans must pass before the epic can be marked done.

**Five sub-activities in strict order (the ordering is a correctness constraint, not a suggestion):**

### 1. Prepare fn-2 proxy-close mapping (no close yet)

fn-2-fix-stale-locale-documentation-after was superseded by fn-3.5 but never formally closed. Its spec explicitly says "close this epic as done-by-proxy after fn-3.5 completes". fn-3 is now done (6/6 tasks), and fn-5.1-.4 + fn-5.6 have fixed any remaining doc drift. Time to prepare the closure — but do not execute `flowctl done` yet.

**Prepare procedure:**
- Read `.flow/specs/fn-2-fix-stale-locale-documentation-after.md` and list its 5 acceptance items (lines 18-24 per the pre-plan audit).
- For each acceptance item, write one-line mapping to the satisfying artifact: fn-3.5 task section, fn-5.2/.3/.4/.6 task, or direct file:line citation.
- Write the mapping as a section at the end of `.flow/specs/fn-2-fix-stale-locale-documentation-after.md` titled "## Proxy-close mapping (completed by fn-5)".
- **Do not execute `flowctl done` in this sub-activity.** Closure happens in sub-activity 4, only after scans pass.

### 2. Run all residual / parity / regression scans

Run these scans and capture verbatim output in the task done-evidence. **All scans must pass before proceeding to sub-activity 3.**

**2a. Residual deleted-converter scan (scope-based allowlist, not count-based):**
```bash
grep -rn "GermanTimeOnlyToClockNotationConverter\|FrenchTimeOnlyToClockNotationConverter\|LuxembourgishTimeOnlyToClockNotationConverter\|JapaneseTimeOnlyToClockNotationConverter\|DefaultTimeOnlyToClockNotationConverter\|PhraseHourClockNotationConverter\|RelativeHourClockNotationConverter" src/ tests/ docs/ .agents/ .claude/ CLAUDE.md AGENTS.md ARCHITECTURE.md readme.md release_notes.md
```
**Expected**: every match must fall within the allowlisted scopes: (1) `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/HumanizerSourceGeneratorTests.cs` lines 68-70 (intentional `DoesNotContain` assertions), or (2) `release_notes.md` vNext section (changelog entries documenting the removal of deleted converters, added by fn-5.3). The pass criterion is **scope-based, not count-based** — any number of matches is fine as long as all of them are within an allowlisted scope. Any match outside those scopes is a regression — do NOT proceed; file a follow-up to fn-5.2/.3/.4. Verify by inspecting the file paths and line numbers in the grep output; do not assume a stable count.

**2b. Residual conceptual-language scan:**
```bash
grep -rn "residual" docs/ CLAUDE.md AGENTS.md ARCHITECTURE.md readme.md release_notes.md .agents/
```
**Expected**: only conceptual mentions with surrounding context (e.g., "the four residual leaf converters were removed"). Bare "residual leaves" claims as a current-state description are regressions.

**2c. Stale net48 framing scan (regression check for fn-5.2):**
```bash
grep -rn "avoid net48 on" CLAUDE.md AGENTS.md
```
**Expected**: zero matches.

**2d. Stale manual-registry phrasing scan (regression check for fn-5.2 and fn-5.4):**
```bash
grep -rn "register in formatter/converter registries\|register new formatters/converters in the appropriate registries\|register.*formatter.*registry" CLAUDE.md AGENTS.md .agents/skills/add-locale/
```
**Expected**: zero matches. Any hit is either fn-5.2 or fn-5.4 regression — do NOT proceed until it's fixed.

**2e. Stale `Common/` generator-input path scan (regression check for fn-5.4):**
```bash
grep -rn "Common/LocaleRegistryInput\|Common/OrdinalDateProfileCatalogInput" .agents/ CLAUDE.md AGENTS.md ARCHITECTURE.md readme.md docs/
```
**Expected**: zero matches. Both files now live under `Generators/` and `Generators/ProfileCatalogs/` respectively.

**2f. Calendar.months claim verification (exact-set comparison, not cardinality):**
```bash
# Enumerate the actual YAML-authored override set as a normalized, sorted list of locale codes
YAML_CAL_SET=$(grep -l "^  calendar:" src/Humanizer/Locales/*.yml \
  | sed 's|.*/||;s|\.yml$||' \
  | sort -u \
  | tr '\n' ',' | sed 's/,$//')
YAML_FMT_SET=$(grep -l "^    formatting:" src/Humanizer/Locales/*.yml \
  | sed 's|.*/||;s|\.yml$||' \
  | sort -u \
  | tr '\n' ',' | sed 's/,$//')

# Compare against the claim sites (exact string match, not counts).
#
# Calendar.months claim sites:
echo "YAML calendar override set: $YAML_CAL_SET"
grep -n "calendarOverrideLocales" tools/compare-probes.cs
grep -n "calendar" tools/verification-signoff.md
grep -n "calendar" release_notes.md

# Number.formatting.decimalSeparator claim sites:
echo "YAML number.formatting set:  $YAML_FMT_SET"
grep -n "decimalOverrideLocales\|numberFormattingOverrideLocales\|decimalSeparator" tools/compare-probes.cs
grep -n "number\.formatting\|decimalSeparator\|decimal separator" tools/verification-signoff.md
grep -n "number\.formatting\|decimalSeparator\|decimal separator" release_notes.md
```
**Expected — exact set equality on both sets, not cardinality**:
- `$YAML_CAL_SET` must equal (sorted, string-for-string) the normalized locale-code set named in:
  - `tools/compare-probes.cs:22` `calendarOverrideLocales` array literal
  - every `tools/verification-signoff.md` claim that enumerates `calendar.months` override locales
  - the `release_notes.md` `vNext` `calendar:` bullet
  - the `FinalOverrideSet` literal recorded in fn-5.1 done-summary
- `$YAML_FMT_SET` must equal (sorted, string-for-string) the normalized locale-code set named in:
  - any `decimalOverrideLocales` / `numberFormattingOverrideLocales` array literal in `tools/compare-probes.cs` (if present — if not, record N/A)
  - every `tools/verification-signoff.md` claim that enumerates `number.formatting` / decimal-separator override locales
  - the `release_notes.md` `vNext` `number.formatting` / `decimalSeparator` bullet
  - Expected current value: `ar, fr-CH, ku` (confirm the expected literal matches reality after any late edits)

Record `$YAML_CAL_SET` and `$YAML_FMT_SET` verbatim in task evidence, plus the full grep output of every claim site, so the equality check is auditable. Counts alone are **not** sufficient — `{bn, fa, he, ku, ta}` and `{bn, fa, he, ku, zu-ZA}` are both size-5 but different sets, and the gate must distinguish them. The same principle applies to the number.formatting set: `{ar, fr-CH, ku}` vs `{ar, fr-CH, he}` are both size-3 but different. Any mismatch on either set is a regression — STOP.

**2g. fn-3 historical drift verification (regression check for fn-5.6):**
```bash
FINAL_SET_GREP='\bta\b\|zu-ZA\|6 locale\|six locale'
grep -n "$FINAL_SET_GREP" .flow/specs/fn-3-hard-code-locale-overrides-where-icu.md
grep -n "$FINAL_SET_GREP" .flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md
```
**Expected**: only matches consistent with `FinalOverrideSet` or matches that are accompanied by explicit one-line rationale (fn-5.6's output). Any unexplained match is a fn-5.6 regression.

**2h. Supported-languages list exact-set comparison (not cardinality):**
```bash
# Enumerate YAML-authored locale codes as a normalized sorted comma-separated string
YAML_LOCALE_SET=$(ls src/Humanizer/Locales/*.yml \
  | sed 's|.*/||;s|\.yml$||' \
  | sort -u \
  | tr '\n' ',' | sed 's/,$//')

# Extract the locale-code list from docs/localization.md Supported Languages section.
# The section format (verified at planning time) is a single paragraph with entries like
# "Afrikaans (af), Arabic (ar), Chinese (zh-CN, zh-Hans, zh-Hant), ..."
# Extract every parenthesized group, split on comma+space, and normalize.
DOC_LOCALE_SET=$(sed -n '/^## Supported Languages/,/^##[^#]/p' docs/localization.md \
  | grep -oE '\([a-z][a-zA-Z0-9-]*(,[[:space:]]*[a-z][a-zA-Z0-9-]*)*\)' \
  | tr -d '()' \
  | tr ',' '\n' \
  | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' \
  | grep -v '^$' \
  | sort -u \
  | tr '\n' ',' | sed 's/,$//')

echo "YAML locales: $YAML_LOCALE_SET"
echo "Doc locales:  $DOC_LOCALE_SET"

# Expected count for sanity (still record, but not the gate):
ls src/Humanizer/Locales/*.yml | wc -l  # expect 62
```
**Expected**: `$YAML_LOCALE_SET` must equal `$DOC_LOCALE_SET` string-for-string after sorting. Both sets recorded verbatim in task evidence. The `wc -l` count is informational only — the gate is exact-set equality, so a same-count-different-members regression (e.g. YAML adds `xh`, doc still lists `zu`) is caught. The extraction pipeline above was designed for the current doc format ("Afrikaans (af), Arabic (ar), Chinese (zh-CN, zh-Hans, zh-Hant), …"); if the doc structure has drifted, record the exact replacement extraction approach in task evidence and ensure it still yields a sorted comma-separated locale-code string comparable to `$YAML_LOCALE_SET`.

**2i. Full modern-target test suite** (net10.0 and net8.0 required locally; net48 build-only on non-Windows):
```bash
dotnet build src/Humanizer/Humanizer.csproj -c Release
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic
```
**net8.0 is required locally.** Both SDK 8.0.419 and runtime 8.0.25 are installed (`dotnet --list-sdks`). The original task run falsely claimed the SDK was unavailable; fn-5.8 corrected this by running net8.0 locally (38908 passed, 0 failed).

### 3. Gate on scan results

- **If all scans above pass** (2a-2i), proceed to sub-activity 4.
- **If any scan fails**, STOP. Do **not** execute `flowctl done` on fn-2. Record the failure in task evidence. File or extend a follow-up task under fn-5.2, fn-5.3, fn-5.4, or fn-5.6 to fix the root cause (depending on which scan failed). Re-run the failed scan after the follow-up lands, then resume here.

### 4. Execute fn-2 proxy-close (only after sub-activity 3 gate passes)

- Mark fn-2.1 done via `flowctl done fn-2-fix-stale-locale-documentation-after.1 --summary-file <path> --evidence-json <path>`. If fn-2.1 is not in an unblocked state, use `flowctl task unblock` first (or whatever the current flowctl API requires).
- fn-2 epic should then be auto-closed when all tasks are done, or use `flowctl epic close` explicitly if manual closing is required.
- Record the close timestamp and any fn-2 state transitions in task evidence.

### 5. Append final sign-off report

Append a section to `tools/verification-signoff.md` (at the bottom) titled "## Final sign-off" containing:
- Date (2026-04-09 or current date)
- "Verified" checklist: each acceptance criterion from the fn-5 epic with a cited artifact
- "Out of scope" list: fn-4 net48 `Enum.GetValues<T>()` blocker tracked separately
- "Outstanding deferrals" list: source-gen diagnostic for claim-parity (R15), CI-lint for CLAUDE.md command blocks (R16), drift-detection test for compare-probes.cs (R18) — all noted as follow-up candidates, not gates
- Reference to this epic (`fn-5-locale-parity-sign-off-verify-code`) and the **six** sub-tasks (fn-5.1 through fn-5.6)
- The `FinalOverrideSet` literal (read from fn-5.1 done-summary)
- The commit hash or branch state at sign-off time

**Scope clarification** (addresses plan-review finding on fn-5.5 vs fn-5.1 apparent conflict): fn-5.5 only **appends** the "Final sign-off" section at the bottom of `tools/verification-signoff.md`. It operates on the post-fn-5.1 state of the file — meaning the ku decimal-separator typo fix on line 64 and any stale `calendar.months` claim corrections at lines 43/52 (plus any probe-shape reconciliation) have already landed when fn-5.5 runs. fn-5.5 must not revert, contradict, or re-edit any prior content; it only adds net-new content below the existing body. The task's "purely additive" property applies to fn-5.5's own edits, not to the file as a whole.

**Size:** M (prep + shell scans + flowctl commands + one markdown section; touches 2 files + task tracker)
**Files:**
- `.flow/specs/fn-2-fix-stale-locale-documentation-after.md` (append proxy-close mapping section in sub-activity 1)
- `tools/verification-signoff.md` (append final sign-off section in sub-activity 5; do not touch prior lines)
- `.flow/` task/epic state via flowctl (fn-2.1 done, fn-2 epic closed, in sub-activity 4)

## Investigation targets

**Required:**
- `.flow/specs/fn-2-fix-stale-locale-documentation-after.md` (full — short) — source of the 5 acceptance items to map
- `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` (relevant sections only, lines ~140-180 for fn-3.5 rollup) — for the satisfying-artifact citations
- `tools/verification-signoff.md` (full — post-fn-5.1 state, to understand the tone and structure of the new section)
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.1.md` done-evidence — to read `FinalOverrideSet` for sub-activity 5's sign-off report
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.6.md` done-evidence — to confirm fn-3 historical drift is reconciled before running scan 2g

**Optional:**
- `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.5.md` — fn-3.5 deliverables for detailed cross-reference if needed
- Output of `git log --oneline baeca85e^..HEAD` to capture the sign-off commit range

## Approach

- Run sub-activities in strict order 1 → 2 → 3 → 4 → 5. The ordering is a correctness constraint: if scans fail, fn-2 must not be closed until the root cause is fixed.
- Record scan output verbatim in the task done-evidence JSON — not just "passed". Each of 2a-2i gets its own evidence entry.
- The proxy-close mapping section should be copy-paste-able: one row per fn-2 acceptance item, formatted as a markdown table or numbered list.
- The sign-off report's "Verified" section should enumerate every acceptance criterion from the fn-5 epic spec — not just a summary.
- When appending the "## Final sign-off" section to `tools/verification-signoff.md`, locate the actual end-of-file (the post-fn-5.1 state, which has the ku typo fix, calendar claim corrections, and any probe-shape reconciliation already applied). Do not reference or re-edit any pre-existing content.
- Scan 2d is specifically designed to catch cases where fn-5.2 fixed CLAUDE.md but missed AGENTS.md (or vice versa), or where fn-5.4 fixed one skill file but missed the other. Treat any hit as "do not close fn-2 yet" — not as a minor cleanup.

## Key context

- Do NOT try to fix fn-4 (net48 blocker) from this task — it is explicitly out of scope. If net48 tests fail, that is expected and should be noted in the "Out of scope" section, not fixed here.
- Do NOT mark fn-2 done before running the scans — the scans may reveal a stale reference that requires another pass through fn-5.2/.3/.4/.6 first. This is why sub-activities are ordered 1 → 2 → 3 → 4 → 5 and not "close first, scan later".
- The proxy-close is considered successful only if every fn-2 acceptance item has an explicit satisfying artifact. If any item cannot be mapped (e.g., fn-3.5 forgot something), file a new sign-off sub-task before closing fn-2 — do not hand-wave.
- fn-5.5 adds **net new content** to `tools/verification-signoff.md`; fn-5.1's edits to that same file (ku typo + stale claim corrections + probe-shape reconciliation) happen earlier in the task chain and are a prerequisite, not a conflict.
- fn-5.5 runs after fn-5.6 completes (declared dependency), so the fn-3 historical drift is already reconciled by the time scan 2g runs.

## Acceptance

- [ ] Sub-activity 1 complete: fn-2 proxy-close mapping section appended to `.flow/specs/fn-2-fix-stale-locale-documentation-after.md` with explicit artifact per acceptance item (fn-2 NOT yet closed at this point)
- [ ] Sub-activity 2 complete: all nine scans (2a-2i) run with verbatim output captured in task done-evidence
- [ ] Scan 2a: deleted-converter residual scan — every match (regardless of count) falls within an allowlisted scope: (1) `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/HumanizerSourceGeneratorTests.cs` lines 68-70 (DoesNotContain assertions), or (2) `release_notes.md` vNext section (changelog entries documenting converter removal, added by fn-5.3). Any match outside these scopes is a regression. Pass criterion is scope-based, not count-based.
- [ ] Scan 2b: residual conceptual-language scan produces only allowlisted mentions (no stale "residual leaves" as current-state claim)
- [ ] Scan 2c: `grep -rn "avoid net48 on" CLAUDE.md AGENTS.md` returns zero matches
- [ ] Scan 2d: stale manual-registry phrasing scan returns zero matches in `CLAUDE.md`, `AGENTS.md`, and `.agents/skills/add-locale/`
- [ ] Scan 2e: stale `Common/LocaleRegistryInput` / `Common/OrdinalDateProfileCatalogInput` path scan returns zero matches
- [ ] Scan 2f: `calendar.months` and `number.formatting` YAML-authored locale sets **exact-set equal** (string-for-string after sorting) the sets named in `tools/compare-probes.cs:22` `calendarOverrideLocales` array, `tools/verification-signoff.md` claims, `release_notes.md` `vNext` `calendar:` bullet, and the `FinalOverrideSet` literal in fn-5.1 done-summary. Both `$YAML_CAL_SET` and `$YAML_FMT_SET` recorded verbatim in task evidence. Cardinality alone is insufficient — same-size-different-members sets must be caught.
- [ ] Scan 2g: fn-3 historical drift verification returns only matches consistent with `FinalOverrideSet` or accompanied by explicit one-line rationale
- [ ] Scan 2h: `$YAML_LOCALE_SET` (normalized sorted locale codes from YAML filenames) **exact-set equal** to `$DOC_LOCALE_SET` (normalized sorted locale codes extracted from `docs/localization.md` Supported Languages section). Both strings recorded verbatim in task evidence. Cardinality is informational only — same-count-different-members regressions must be caught.
- [ ] Scan 2i: `dotnet build`, `dotnet test` on both net10.0 and net8.0, and `dotnet format --verify-no-changes` all pass locally (both SDKs installed; no deferral)
- [ ] Sub-activity 3 gate passed: all scans pass (or the task stopped, filed follow-up, and re-ran until gate passed)
- [ ] Sub-activity 4 complete: fn-2.1 marked done via `flowctl done`; fn-2 epic confirmed closed (done state visible in `flowctl epics`); timestamp captured
- [ ] Sub-activity 5 complete: `tools/verification-signoff.md` has a new "## Final sign-off" section at the bottom with date, verified checklist, out-of-scope list (fn-4), deferred follow-ups list (R15/R16/R18), epic reference, six-task enumeration (fn-5.1 through fn-5.6), the `FinalOverrideSet` literal, and commit/branch state
- [ ] fn-5.5 only appends content to `tools/verification-signoff.md` — no prior content (including the post-fn-5.1 ku typo fix on line 64, any claim corrections on lines 43/52, or probe-shape reconciliation) is reverted or re-edited
- [ ] net48 explicitly deferred to fn-4 (not run by this task)
- [ ] Epic fn-5 can be marked done after this task completes (all acceptance criteria from the epic spec satisfied)

## Done summary
Executed all five sub-activities of the fn-5.5 sign-off gate task in strict order: (1) prepared fn-2 proxy-close mapping with per-acceptance-item artifact citations in .flow/specs/fn-2-fix-stale-locale-documentation-after.md, (2) ran all 9 residual/parity/regression scans (2a-2i), (3) gate passed, (4) closed fn-2 epic as done-by-proxy via flowctl, (5) appended Final sign-off section to tools/verification-signoff.md with verified checklist, FinalOverrideSet={bn,fa,he,ku,ta,zu-ZA}, out-of-scope items (fn-4 net48), and follow-up candidates (R15/R16/R18). All locally runnable fn-5.5 gate checks passed, including both net10.0 and net8.0 test suites. **(Evidence corrected by fn-5.8: the original close falsely claimed the .NET 8 SDK was unavailable and skipped net8.0 locally. `dotnet --list-sdks` confirms 8.0.419 is installed. net8.0 tests run locally: 38908 passed, 0 failed, 10.7s.)**
## Evidence
- Commits: 3d0c0c0a, adf314a8, 482dbdef, e6e018f0, d40bbbe6 (evidence corrected by fn-5.8 commits dd476b9a, 03688f88, 63b29123)
- Tests: dotnet build src/Humanizer/Humanizer.csproj -c Release, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (38908 passed, 0 failed, 6.8s), dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 -c Release (38908 passed, 0 failed, 10.7s), dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic, scan 2a: deleted-converter residual (PASS - scope-based), scan 2b: residual conceptual-language (PASS), scan 2c: stale net48 framing (PASS - zero matches), scan 2d: stale manual-registry phrasing (PASS - zero matches), scan 2e: stale Common/ paths (PASS - zero matches), scan 2f: calendar/number.formatting exact-set equality (PASS), scan 2g: fn-3 historical drift (PASS - consistent with FinalOverrideSet), scan 2h: supported-languages exact-set equality (PASS - 62 locales), scan 2i: build+test+format (PASS - net10.0: 38908 tests/0 failures; net8.0: 38908 tests/0 failures)
- SDK proof (fn-5.8): dotnet --list-sdks: 8.0.419, 10.0.102; dotnet --list-runtimes: Microsoft.NETCore.App 8.0.25, Microsoft.NETCore.App 10.0.2
- PRs: