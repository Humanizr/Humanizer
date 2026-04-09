## Description

Investigate and reconcile the `calendar.months` discrepancy between fn-3's documented claim and the actual YAML content, on a strictly **per-locale** basis, then apply all immediate code/data/tool changes that derive directly from the decision. The historical spec/task drift in fn-3 markdown is handled by the downstream task **fn-5.6**, which gates on this task's `FinalOverrideSet`.

The output of this task is a concrete set `FinalOverrideSet ⊆ {bn, fa, he, ku, ta, zu-ZA}` recorded in task evidence, plus all current-reality updates (tools, YAML, probes). Mixed outcomes are legal — `ta` and `zu-ZA` are independent decisions.

**The discrepancy:**
- YAML reality (verified via `grep -l "^  calendar:" src/Humanizer/Locales/*.yml`): only `bn.yml`, `fa.yml`, `he.yml`, `ku.yml` contain `calendar:` blocks today. `ta.yml` and `zu-ZA.yml` have no `calendar:` surface and no `months:` key anywhere.
- Claim sites that state a six-locale override list: `tools/verification-signoff.md` (lines 43 and 52, plus any others introduced later), `tools/compare-probes.cs:22`, and the fn-3 planning artifacts `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` + `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md`.
- This task updates the first two claim sites (verification-signoff.md lines in scope + compare-probes.cs:22). The fn-3 planning artifact audit is **out of scope here** and lives in fn-5.6.

**Probe coverage gap AND contract-mismatch (both surfaced during plan review):**
Two orthogonal problems with the existing probe as evidence for month-name parity:
1. **Coverage gap.** `tools/probe-*-after.json` samples specific reference dates rather than enumerating all 12 months. Inspection of the macOS probe for `ta` shows only **7 of 12 distinct months** (Jan, Feb, Mar, Sep, Oct, Nov, Dec) — April through August are never sampled.
2. **Contract mismatch.** The probe currently emits `month_standalone` via `dt.ToString(" MMMM yyyy", culture)` (formatted-output proxy), but the authoritative decision rule (below) is defined in terms of raw `CultureInfo.DateTimeFormat.MonthNames` entries. Even with 12-month coverage, formatted-output samples are not the same contract as the raw `MonthNames` array. The two are usually equivalent but can diverge for edge cases (e.g. month-name variants used only in specific date patterns, or locale-specific `MonthGenitiveNames` fallback behavior).

Both problems must be addressed together: the probe must be extended to emit the raw 12-entry `DateTimeFormat.MonthNames` array (and `MonthGenitiveNames` if the locale authors `calendar.monthsGenitive`) as a new top-level field per locale, alongside (not replacing) the existing `month_standalone` samples. "Zero variance on sampled months" on the formatted-output samples does **not** prove the raw-array contract; the raw array must be enumerated and compared directly.

**Evidence rule (deterministic, no subjective judgement):**
For locale L (where L ∈ {`ta`, `zu-ZA`}), L is **excluded** from `FinalOverrideSet` if and only if **both** of the following hold:
1. **Full platform coverage.** Every shipping platform target has been reached and evidence collected: macOS net10, Linux net10, Windows net10, and Windows net48. If *any* of those four targets is unreachable in the task environment, L stays in `FinalOverrideSet` — no subjective "representative enough" judgement is permitted. Record the unreachable target(s) and the conservative keep decision in task evidence (always) and in the committed artifact (only if the fallback evidence path was used; the preferred-path `tools/probe-*-after.json` files serve as the committed artifact inherently, so unreachable-platform notes for that path live in task evidence only).
2. **Byte-identical MonthNames.** On every reached platform, the 12-entry `CultureInfo.GetCultureInfo(L).DateTimeFormat.MonthNames` array is byte-identical to the array on every other reached platform.

Otherwise L stays in `FinalOverrideSet`, either keeping an existing override or authoring a new one.

**Acceptable evidence sources, in preference order:**
1. **Preferred — extended probes emitting raw `MonthNames` (checked-in, reproducible, contract-accurate).** This repo ships **two** probe implementations and both must be extended in parallel to avoid leaving Windows net48 without raw-array coverage:
   - `tools/locale-probe.cs` — the `dotnet run`-style script used for macOS net10, Linux net10, and Windows net10.
   - `tools/locale-probe-net48/Program.cs` (plus its `locale-probe-net48.csproj`) — the .NET Framework 4.8 variant used for Windows net48 (NLS path, not ICU).

   Add a new per-locale field (e.g. `month_names_raw`) containing the 12-entry `CultureInfo.GetCultureInfo(L).DateTimeFormat.MonthNames` array (stripping the null 13th entry that DateTimeFormat appends) and optionally `month_genitive_names_raw` with the corresponding `MonthGenitiveNames` array. This is a new top-level field per locale — do NOT replace the existing `month_standalone` sampled-date list in either probe, which is load-bearing for other verification flows. Keep the two probe implementations in lockstep so their JSON shapes agree byte-for-byte on the new fields. Re-run both probes on every reachable platform target and commit updated `tools/probe-*-after.json` files. This path satisfies both the coverage gap (raw arrays cover all 12 months) and the contract mismatch (raw arrays are the exact contract named in the decision rule).

   If a platform target is unreachable in the task environment (most likely Windows net48 on a macOS/Linux developer machine), record the unreachable target in task evidence and treat the locale as kept in `FinalOverrideSet` per the deterministic rule — do not substitute fallback-path evidence for the unreachable platform.
2. **Fallback — direct CultureInfo enumeration with committed artifact.** If **either** `tools/locale-probe.cs` or `tools/locale-probe-net48/Program.cs` cannot be extended and re-run in this environment (e.g. probe generator tooling is unavailable on some platform target, or the net48 probe requires Windows and the environment is macOS/Linux), run a small `dotnet script` / `dotnet run --project` / LINQPad snippet that prints `CultureInfo.GetCultureInfo(L).DateTimeFormat.MonthNames` and `DateTimeFormat.MonthGenitiveNames` on each reachable platform target. **Commit the raw output as a checked-in artifact** under `tools/` (e.g. `tools/culture-info-month-enumeration-ta-zu-ZA.txt`) alongside the snippet/source, so future auditors can reproduce it. Do not rely solely on task evidence JSON for this — the downstream sign-off (fn-5.5) cites the artifact, and future regressions need a stable comparison point. This fallback path emits the same raw-array contract as the preferred path, just without probe-file integration. Note: the fallback path is a substitute for an **unreachable** platform, not for a **skipped** one — if Windows net48 is reachable (you're on Windows), you must still extend `tools/locale-probe-net48/Program.cs` as part of the preferred path; fallback is only acceptable when the probe implementation cannot run on the target.
3. **Corroborating only — CLDR lookup.** Read the current CLDR data directly (e.g. `cldr-dates-full/main/ta/ca-gregorian.json`) to confirm the canonical value. CLDR alone **cannot** justify excluding a locale from `FinalOverrideSet` — it proves the canonical value, not per-platform agreement. Use CLDR to sanity-check a conclusion drawn from (1) or (2), not as a standalone source.

Both options (1) and (2) produce raw `MonthNames` arrays as the comparison input. Do **not** use `month_standalone` formatted-output samples as the decision input — they are useful as corroborating context but not as the authoritative contract.

**Per-locale audit procedure:**
1. For each of `ta`, `zu-ZA`:
   1. Gather full 12-month evidence per the rule above (prefer option 1, fall back to option 2 only if option 1 is blocked).
   2. Apply the evidence rule. Record the decision (`exclude L` / `keep L`) with cited evidence per platform.
   3. If decision is `keep L` and YAML currently has no `calendar:` block for L, author one (see "If authoring overrides" below).
2. Assemble `FinalOverrideSet = {bn, fa, he, ku} ∪ {keeps from step 1}`. This set has 4, 5, or 6 members.
3. Apply in-scope current-reality updates (see "Propagation — this task" below). The fn-3 historical audit is deferred to fn-5.6.
4. Record `FinalOverrideSet` as the authoritative output of this task — in task evidence JSON, in the task done-summary, and (if checking in an enumeration artifact) in that artifact's header. fn-5.3 and fn-5.6 read `FinalOverrideSet` from this task's done-evidence file.

**If authoring overrides** (locale kept in `FinalOverrideSet` but YAML currently has no `calendar:` block): follow the pattern of the existing `calendar:` blocks in `src/Humanizer/Locales/bn.yml`, `fa.yml`, `he.yml`, `ku.yml` (grep `^  calendar:` to find the current line ranges; historic references like `bn.yml:515-530` were approximate). Consult `docs/locale-decisions.md` for the selection-criteria discipline (each decision cites source, value, CLDR version, and rationale). After adding, re-run the reachable probes on the current environment and commit updated `probe-*-after.json` files touched by the change.

**Propagation — this task (current-reality updates only):**
- `tools/verification-signoff.md` — every stale six-locale `calendar.months` claim (lines 43, 52, and any others discovered by full-file grep). Also fix `tools/verification-signoff.md:64` ku decimal-separator typo (unconditional — see below). Also reconcile probe-shape-dependent narrative if either probe's JSON shape changed.
- `tools/compare-probes.cs` — line 22 (`calendarOverrideLocales` array literal) plus any script behavior that must still make sense after shrinking the array (usages at lines 149-152, 198). Additionally, re-verify it still compiles/runs against the post-extension probe JSON shape if `month_names_raw` (or any other new field) was added. Do not refactor beyond the array literal + directly-broken usages.
- `src/Humanizer/Locales/ta.yml` / `src/Humanizer/Locales/zu-ZA.yml` — only if `FinalOverrideSet` keeps the locale AND YAML currently lacks a `calendar:` block. Author a new block following the bn/fa/he/ku pattern.
- `tools/locale-probe.cs` — **preferred path, required whenever preferred path is chosen**: extend with a new `month_names_raw` per-locale field emitting the raw 12-entry `DateTimeFormat.MonthNames` array. Covers macOS net10, Linux net10, Windows net10.
- `tools/locale-probe-net48/Program.cs` — **preferred path, required whenever preferred path is chosen, in lockstep with `locale-probe.cs`**: extend with the same `month_names_raw` per-locale field emitting byte-identical JSON shape. Covers Windows net48. Updating only one of the two probes does NOT satisfy the preferred path.
- `tools/probe-macos-after.json`, `tools/probe-linux-after.json`, `tools/probe-windows-net10-after.json`, `tools/probe-windows-net48-after.json` — re-run each reachable probe after the matching implementation is extended; commit updates for reached platforms only; unreachable platforms explicitly noted.
- `tools/culture-info-month-enumeration-*.txt` — only if fallback path used for any platform; committed alongside the snippet/source.

**Propagation — deferred to fn-5.6 (historical planning-artifact audit):**
- `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` — full-file audit (Scope, Acceptance, Early proof point, any other passage)
- `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md` — full-file audit (Description, Files list, Per-locale YAML population, Key context, Early proof point, Acceptance)

fn-5.6 consumes `FinalOverrideSet` from this task's done-evidence and applies propagation to those two files with one-line rationale per removed locale.

**Probe-shape reconciliation (follow-on inside this task):**
If this task extends `tools/locale-probe.cs` and/or `tools/locale-probe-net48/Program.cs` or otherwise changes probe shape (additional reference dates, new sampled months, new `month_names_raw` field, different JSON structure), the hardcoded totals and "before/after probe identity" narrative in `tools/verification-signoff.md` may also become stale. Specifically: any line citing raw diff counts, sample totals, or claims like "before and after probes are byte-identical copies for locales without overrides" must be re-scanned and updated to match the post-extension probe shape. Perform this reconciliation inside this task, not as a future cleanup — the sign-off task (fn-5.5) reads verification-signoff.md as already-coherent. Also verify that `tools/compare-probes.cs` still compiles and runs after the probe-shape change; if it hardcodes JSON field names that no longer exist, update it.

**Also in this task:** fix `tools/verification-signoff.md:64` — the `ku` row's "Humanizer override" column shows `,` but the YAML at `src/Humanizer/Locales/ku.yml:333` holds `٫` (U+066B, Arabic decimal separator, "momayyiz"). Line 56 of the same signoff file already identifies it correctly as momayyiz; the typo is only in the table cell. This edit is unconditional — perform it regardless of the `FinalOverrideSet` outcome.

**Size:** M (bounded by per-locale conditional work; does not include the fn-3 historical audit, which is in fn-5.6)
**Files:**
- `tools/locale-probe.cs` (preferred evidence path — extend with `month_names_raw` field; macOS/Linux/Windows net10 coverage)
- `tools/locale-probe-net48/Program.cs` (preferred evidence path — parallel extension for Windows net48 coverage; keep JSON shape in lockstep with `locale-probe.cs`)
- `tools/probe-macos-after.json`, `tools/probe-linux-after.json`, `tools/probe-windows-net10-after.json`, `tools/probe-windows-net48-after.json` (conditional — re-run each reachable probe after extending both probe implementations; unreachable platforms explicitly noted)
- `tools/verification-signoff.md` (always — at minimum ku typo fix; also every stale six-locale claim site if `FinalOverrideSet` shrinks, plus probe-shape reconciliation if probe changed)
- `tools/compare-probes.cs` (conditional — update array literal if `FinalOverrideSet` differs from current six; also update any hardcoded JSON field-name references if probe shape changed)
- `tools/culture-info-month-enumeration-*.txt` (only if fallback evidence path chosen on any platform; checked-in artifact)
- `src/Humanizer/Locales/ta.yml` (conditional — only if ta is kept in `FinalOverrideSet` and needs a new override)
- `src/Humanizer/Locales/zu-ZA.yml` (conditional — only if zu-ZA is kept in `FinalOverrideSet` and needs a new override)

## Investigation targets

**Required:**
- `tools/locale-probe.cs` — current sampled date set in the net10 probe; understand what needs extending and verify the emission pattern
- `tools/locale-probe-net48/Program.cs` and `tools/locale-probe-net48/locale-probe-net48.csproj` — net48 probe variant; understand the parallel extension surface and the NLS-specific quirks (no TimeOnly, etc.)
- `tools/probe-macos-after.json`, `probe-linux-after.json`, `probe-windows-net10-after.json`, `probe-windows-net48-after.json` — current month-name evidence and confirmation of the month-coverage gap for ta/zu-ZA across all four shipping targets
- `src/Humanizer/Locales/bn.yml`, `fa.yml`, `he.yml`, `ku.yml` — canonical `calendar:` block pattern (grep `^  calendar:` to find the exact lines)
- `src/Humanizer/Locales/ku.yml:332-340` — canonical `number.formatting` block pattern (for surface-level context and the decimal-separator glyph)
- `tools/verification-signoff.md` (full file) — all stale override claim sites, the ku typo, AND any probe-shape-dependent narrative that needs reconciling
- `tools/compare-probes.cs` — hardcoded `calendarOverrideLocales` array and its usages (lines 22, 149-152, 198)

**Optional:**
- `docs/locale-decisions.md` — selection-criteria discipline template if authoring new decisions
- Public CLDR data (`cldr-json/cldr-dates-full/main/ta/ca-gregorian.json`) — corroborating evidence source only
- A working `CultureInfo` enumeration tool (`dotnet script`, `dotnet run`, LINQPad, or `csi`) — only needed for the fallback evidence source

## Approach

- **Per-locale, not branch-level.** `ta` and `zu-ZA` are independent decisions. Do not bundle them into a single `{add-overrides | correct-claims}` branch. The final output is the set `FinalOverrideSet`, which may be `{bn, fa, he, ku}` (both removed), `{bn, fa, he, ku, ta}` or `{bn, fa, he, ku, zu-ZA}` (one removed), or `{bn, fa, he, ku, ta, zu-ZA}` (neither removed).
- **Prefer the extended-probe path.** The checked-in probe files are the load-bearing evidence for the entire locale-parity sign-off. If evidence source (1) is blocked, fall back to (2) and commit the raw enumeration output as a repo-local artifact — do not leave the decision with only task-evidence JSON.
- **Conservative default is deterministic, not judgement-based.** If *any* shipping platform target (macOS net10, Linux net10, Windows net10, Windows net48) is unreachable in the task environment, the locale automatically stays in `FinalOverrideSet`. Do not attempt to argue that the reachable targets are "representative enough" — the rule is binary. Record the unreachable targets and the conservative keep decision; land a follow-up if later platform access would enable a more complete audit.
- **No speculative overrides.** Do not add a YAML `calendar:` block to a locale "just to match the claim" — the governing principle is technical correctness (platform variance), not list symmetry. But if variance is confirmed (or evidence is incomplete), authoring the override is the right call.
- Record decisions in task evidence: probe month-set coverage, CultureInfo output per platform (or explanation of why a platform was unreachable), CLDR version if used for corroboration, one paragraph per locale with the resulting decision and the resulting `FinalOverrideSet`.
- The ku decimal-separator typo fix is unconditional — it is an independent stale artifact.
- When updating `tools/compare-probes.cs`, do not refactor the script beyond the array literal + any directly broken usages. A full rewrite is out of scope.
- When probe shape changes in either `tools/locale-probe.cs` or `tools/locale-probe-net48/Program.cs`, audit `tools/verification-signoff.md` for any hardcoded diff counts, totals, or "before/after identity" claims and update them to match the post-extension shape. Use grep to find candidates (`grep -n "diff\|identical\|total\|copies" tools/verification-signoff.md`). Also re-verify `tools/compare-probes.cs` compiles against the new JSON shape.

## Key context

- `calendar.months` requires **exactly 12** entries if present — validated by `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:284-289`
- `calendar.months` indices are Gregorian January=0 unless `calendarMode: Native` is set (ta/zu-ZA would be Gregorian default)
- If adding overrides, the generator emits a diagnostic error if `MMM` (abbreviated month) appears in any `OrdinalDatePattern` for that locale — abbreviated months are out of scope per fn-3 Architecture
- `variantOf` inheritance: child locales inherit parent `calendar.months` unless authoring their own — but ta and zu-ZA are base locales with no parent, so this doesn't apply
- The `tools/compare-probes.cs` update is in scope for this task (explicitly listed in fn-5 epic scope). It is not a refactor — it is alignment of a stale claim to current reality.
- **`FinalOverrideSet` is the output contract of this task.** fn-5.3 and fn-5.6 read it from this task's done-evidence file. Record the set explicitly in the done-summary (not buried in prose), e.g. `FinalOverrideSet = {bn, fa, he, ku, ta, zu-ZA}`.

## Acceptance

- [ ] Probe month-coverage AND contract-mismatch for ta and zu-ZA explicitly documented in task evidence (the old probe samples ~7 of 12 months via `month_standalone` formatted-output proxy, not raw `MonthNames`)
- [ ] Raw 12-entry `CultureInfo.DateTimeFormat.MonthNames` array obtained for ta and zu-ZA via preferred path OR fallback path; path chosen is explicitly documented in task evidence, with rationale if fallback was used. `month_standalone` formatted-output samples are NOT used as the decision input — only the raw `MonthNames` arrays are.
- [ ] **Preferred-path two-probe lockstep**: if the preferred path is used, **both** probe implementations are extended in parallel — `tools/locale-probe.cs` AND `tools/locale-probe-net48/Program.cs` — each emitting a `month_names_raw` field with byte-identical JSON shape; updates to only one probe implementation do NOT satisfy this acceptance item. Re-run each reachable probe and commit the updated `tools/probe-*-after.json` files. Evidence must cite the git diff (or file edit) for BOTH probe source files.
- [ ] **Fallback-path artifact**: if the fallback path is used on any platform (e.g. Windows net48 unreachable from macOS/Linux dev machine so the preferred net48 path is blocked), a checked-in artifact under `tools/` (e.g. `tools/culture-info-month-enumeration-ta-zu-ZA.txt`) contains the raw enumeration output for every reachable platform, alongside the snippet source used to produce it.
- [ ] Evidence recorded per reachable platform per locale; unreachable platforms explicitly listed in both task evidence and (if fallback path) the committed artifact
- [ ] **Deterministic rule honored**: any locale for which any of the four shipping platform targets (macOS net10, Linux net10, Windows net10, Windows net48) is unreachable automatically stays in `FinalOverrideSet` — no subjective "representative enough" judgement made in any decision
- [ ] CLDR is only used for corroboration, never as a standalone justification for excluding a locale from `FinalOverrideSet`
- [ ] Per-locale decision recorded independently for ta and zu-ZA; `FinalOverrideSet` assembled and stated explicitly in the task done-summary (the exact set literal, not buried in prose)
- [ ] For each locale kept in `FinalOverrideSet` that currently has no YAML `calendar:` block: `src/Humanizer/Locales/{locale}.yml` contains a `calendar:` block with valid 12-entry `months:` array, and the probe has been re-run on the current environment with updated `probe-*-after.json` committed
- [ ] `grep -l "^  calendar:" src/Humanizer/Locales/*.yml` output matches `FinalOverrideSet` exactly
- [ ] `tools/compare-probes.cs:22` `calendarOverrideLocales` array matches `FinalOverrideSet` exactly; script still compiles/runs cleanly
- [ ] `tools/verification-signoff.md` has zero remaining stale `calendar.months` override claims outside the `FinalOverrideSet` — every such claim (lines 43, 52, and any others found via full-file grep) updated
- [ ] `tools/verification-signoff.md:64` ku decimal-separator override column shows `٫` (U+066B) to match `src/Humanizer/Locales/ku.yml:333` (unconditional; always performed)
- [ ] If **either** probe implementation (`tools/locale-probe.cs` or `tools/locale-probe-net48/Program.cs`) or any `tools/probe-*-after.json` file had its shape changed, `tools/verification-signoff.md` has been re-scanned for hardcoded totals / diff counts / "before-after identity" narrative and updated to match the post-extension shape; `tools/compare-probes.cs` has been re-verified to still compile and run against the new JSON shape; if no probe-shape change in either probe, this is recorded as N/A in task evidence
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds (catches YAML schema errors if overrides were added)
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0` passes
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0` passes — **DEFERRED TO CI**: .NET 8 SDK not installed locally (only 10.0.2 available); overrides are framework-agnostic (build-time generated); see `tools/verification-signoff.md` §8 "Verification completeness"

## Done summary
Reconciled calendar.months discrepancy for ta and zu-ZA: authored YAML override blocks for both locales following the bn/fa/he/ku pattern, extended both probe implementations (locale-probe.cs and locale-probe-net48/Program.cs) with month_names_raw and month_genitive_names_raw fields in lockstep, re-ran macOS probe, and fixed ku decimal-separator typo in verification-signoff.md. FinalOverrideSet = {bn, fa, he, ku, ta, zu-ZA} (all 6 kept; 3 of 4 platform targets unreachable, conservative deterministic rule applied).

**Deferred acceptance item**: `dotnet test --framework net8.0` not run locally (.NET 8 SDK unavailable; only 10.0.2 installed). Deferred to CI pipeline. Overrides are framework-agnostic (source-generated at build time), so net8 correctness is expected but unverified locally. See `tools/verification-signoff.md` §8 "Verification completeness" for the full deferral inventory.
## Evidence
- Commits: 831ae5f4c36db0ec8ffc5e3a6e3b3bc2b25c3dc2, 7197b1dc8ab57e6e5e5e7f2a38459e9c3e8f5e4a, 5a0e74e4197c58d6ea26011c33b8236251c39ce7
- Tests: dotnet build src/Humanizer/Humanizer.csproj -c Release, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0, dotnet run tools/compare-probes.cs --after, dotnet run tools/compare-probes.cs --before-vs-after
- PRs: