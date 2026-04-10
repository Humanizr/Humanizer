## Description

Full-file audit of fn-3's historical planning artifacts to reconcile every six-locale `calendar.months` reference against the authoritative `FinalOverrideSet` produced by **fn-5.1**. This task is purely markdown editing of two files; it does not touch source code, runtime, or production YAML.

**Why this is a separate task (split from fn-5.1):**
fn-5.1 derives `FinalOverrideSet` and applies all current-reality updates (verification-signoff.md, compare-probes.cs, optional YAML authoring, probe re-run). The fn-3 historical audit is bounded and purely markdown — splitting it out keeps fn-5.1 sized M and makes this audit reviewable in isolation.

**The drift:**
Multiple narrative, Scope, Files list, Per-locale population, Key context, Early proof point, and Acceptance passages across these two files claim `calendar.months` overrides for six locales: `bn, fa, he, ku, zu-ZA, ta`. If fn-5.1 produces `FinalOverrideSet` = the full six locales, this task confirms no drift exists and records that confirmation. If fn-5.1 shrinks the set, this task updates every stale reference.

**Files to audit (full-file, grep-guided):**

1. **`.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md`** — Scope, Acceptance, Early proof point, and any other passage that enumerates `calendar.months` override locales or cites the six-locale claim.

2. **`.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md`** — Description, Files list, Per-locale YAML population section, Key context, Early proof point, Acceptance, and any other passage.

**Audit procedure:**

1. Read `FinalOverrideSet` from the fn-5.1 done-evidence (either the task evidence JSON or the done-summary line that explicitly states the set).
2. For each of the two files, run the drift grep:
   ```bash
   grep -n "\bta\b\|zu-ZA\|6 locale\|six locale\|bn.*fa.*he.*ku" <file>
   ```
   Review every match. For each match:
   - If the reference is already consistent with `FinalOverrideSet`, no change.
   - If the reference names a locale that was removed from the set, either update to match the set or attach a one-line rationale describing the historical context (e.g. "initially claimed as six-locale override; ta evidence showed cross-platform agreement, so ta was removed from FinalOverrideSet at fn-5.1 — see tools/culture-info-month-enumeration-ta-zu-ZA.txt").
   - If the reference is a proof-point example (e.g. "zu-ZA month name" as an illustrative case), and that locale was removed, update the example to a locale still in `FinalOverrideSet` (prefer `bn` for its broad coverage, or any locale from the current set).
3. Do **not** rely on pre-recorded line numbers — the files may have drifted since the initial audit. Full-file grep is the right tool.
4. Attach a one-line rationale per removed locale in the edited file, at the audit site. Do not bury the rationale in the task evidence only.

**If `FinalOverrideSet` = `{bn, fa, he, ku, ta, zu-ZA}` (no shrink):**
No reference updates are required — the original claim is correct. This task still runs the drift grep on both files and records "no stale references found; fn-5.1 kept all six locales" in task evidence as proof that the audit was performed.

**Out of scope:**
- Any changes to `tools/verification-signoff.md`, `tools/compare-probes.cs`, or production YAML — those are handled in fn-5.1.
- Any changes to the fn-3 epic's actual implementation (fn-3.1-.6 are already done) — this audit is about historical planning-artifact accuracy, not re-executing fn-3.
- Any changes to the fn-3 requirement-coverage table or its task numbering.

**Size:** M (2 files, full-file audit, bounded, pure markdown editing)
**Files:**
- `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` (conditional updates; always audited)
- `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md` (conditional updates; always audited)

## Investigation targets

**Required:**
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.1.md` (done-summary + done-evidence section) — source of `FinalOverrideSet` literal
- `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` (full file) — needs full-file drift audit
- `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md` (full file) — needs full-file drift audit
- `src/Humanizer/Locales/*.yml` (via `grep -l "^  calendar:"`) — authoritative post-fn-5.1 YAML state for cross-checking the audit

**Optional:**
- `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.5.md` — fn-3.5 documentation task, for additional cross-references if an edit needs a fn-3.5 pointer
- `docs/localization.md:70-71` — canonical surface list if an audit site needs to enumerate surfaces for context

## Approach

- **Start by reading fn-5.1's done-summary.** Find the line that states `FinalOverrideSet = {...}` (or equivalent). This is the authoritative input — do not guess.
- **Grep then review.** Use the drift grep pattern (above) to surface every candidate line. Review each one; most matches will be in narrative passages, a few in structured sections like Acceptance.
- **Edit in place; do not restructure.** Each edit should be a minimal change to an existing line or paragraph. Do not rewrite sections or reorder content.
- **One-line rationale per removed locale.** Attach at the audit site, not buried in a footnote. Example format: `- ~~zu-ZA~~ (removed at fn-5.1; evidence showed byte-identical MonthNames across macOS/Linux/Windows net10 — see tools/culture-info-month-enumeration-ta-zu-ZA.txt)`.
- **Prefer bn as the default illustrative locale** if a proof-point example needs to be changed — `bn` has the broadest `calendar.months` coverage of the four always-kept locales.
- **Do not edit the fn-3 epic's requirement-coverage table.** That table reflects what fn-3 delivered, not the post-sign-off reality. Any "the `calendar.months` override list was trimmed at fn-5" note belongs in the fn-3 spec's Scope or Key context section, not in the coverage table.
- If both files are clean (no stale references), record the grep output in task evidence as proof, and mark this task done immediately.

## Key context

- This task runs **after** fn-5.1. The dependency is declared via `flowctl dep add fn-5-locale-parity-sign-off-verify-code.6 fn-5-locale-parity-sign-off-verify-code.1`.
- This task is purely markdown editing. It does not touch source code, runtime, production YAML, or tools. The only scope is the two fn-3 planning artifacts listed above.
- fn-5.5 (final sign-off) reads the state of these two files after this task completes and uses the post-audit state as evidence that the fn-3 historical drift is reconciled. If this task leaves stale references, fn-5.5's scans will fail.
- Do not edit fn-3's other tasks (`.1`, `.2`, `.4`, `.5`, `.6`) even if they mention the six-locale claim in a footer or appendix. fn-3.3 is the only task-level file in scope because it was the implementation task for the YAML-authoring work; other tasks only reference the claim in passing context, which is not drift-critical. If the drift grep reveals a stale reference in another fn-3 task file, file a follow-up rather than expanding this task's scope.

## Acceptance

- [ ] `FinalOverrideSet` literal read from fn-5.1 done-summary and recorded in this task's evidence (so the audit's input is traceable)
- [ ] `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` full-file audit complete: `grep -n "\bta\b\|zu-ZA\|6 locale\|six locale\|bn.*fa.*he.*ku" .flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` returns only matches consistent with `FinalOverrideSet` (or explicit one-line rationale for removed locales)
- [ ] `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md` full-file audit complete: same grep returns only matches consistent with `FinalOverrideSet`
- [ ] Proof-point example locale (if any was changed) is one that is still in `FinalOverrideSet`
- [ ] One-line rationale recorded in each updated file for each locale removed from the original six-locale set (at the audit site, not buried in task evidence)
- [ ] If `FinalOverrideSet` = `{bn, fa, he, ku, ta, zu-ZA}` (no shrink), task evidence explicitly records "no stale references found; fn-5.1 kept all six locales, audit confirmed no drift"
- [ ] No changes to files outside `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` and `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md`
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` still passes (markdown-only edits should not trigger formatter, but confirm)

## Done summary
Full-file audit of fn-3 historical planning artifacts (.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md and .flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md) confirmed all six-locale calendar.months references are consistent with FinalOverrideSet = {bn, fa, he, ku, ta, zu-ZA} (all 6 locales kept by fn-5.1). One annotation added to fn-3.3 done summary to clarify that fn-3.3 originally delivered 4 of the 6 overrides and fn-5.1 authored the remaining 2 (ta, zu-ZA).

## Evidence

**FinalOverrideSet source:** fn-5-locale-parity-sign-off-verify-code.1 done-evidence: `["bn", "fa", "he", "ku", "ta", "zu-ZA"]`

**YAML cross-check:** `grep -l "^  calendar:" src/Humanizer/Locales/*.yml` returned: bn.yml, fa.yml, he.yml, ku.yml, ta.yml, zu-ZA.yml (exact match with FinalOverrideSet)

**Spec file grep** (`grep -n "\bta\b\|zu-ZA\|6 locale\|six locale\|bn.*fa.*he.*ku" .flow/specs/fn-3-hard-code-locale-overrides-where-icu.md`):
- Line 9: `ta` in failure-category narrative — consistent (historical context, not override-set claim)
- Line 10: `zu-ZA` in stylistic-variation narrative — consistent (historical context)
- Line 25: `bn, fa, he, ku, zu-ZA, ta` in Scope — matches FinalOverrideSet exactly
- Line 99: `bn, fa, he, ku, zu-ZA, ta` in Acceptance — matches FinalOverrideSet exactly
- Line 118: `zu-ZA` proof-point + "other 5 locales" = 6 total — matches FinalOverrideSet

**Task .3 file grep** (`grep -n "\bta\b\|zu-ZA\|6 locale\|six locale\|bn.*fa.*he.*ku" .flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md`):
- Line 4: `bn, fa, he, ku, zu-ZA, ta` in Description — matches FinalOverrideSet
- Line 13: `bn.yml, fa.yml, he.yml, ku.yml, zu-ZA.yml, ta.yml` in Files list — matches FinalOverrideSet
- Lines 102-103: `zu-ZA` and `ta` per-locale entries — consistent
- Line 123: "the 6 locales" in Key context — matches FinalOverrideSet count
- Lines 124, 129: `zu-ZA` proof-point + "other 5" = 6 — matches FinalOverrideSet
- Line 140: 6 YAML files in Acceptance — matches FinalOverrideSet
- Line 143: `bn, fa, he, ku, zu-ZA, ta` in test Acceptance — matches FinalOverrideSet
- Line 148: Done summary lists 4 locales (bn, fa, he, ku) — historically accurate for what fn-3.3 delivered; fn-5.6 audit annotation added clarifying ta and zu-ZA were authored by fn-5.1

**Annotation added:** One-line audit note appended to fn-3.3 done summary (line 148) explaining that the 4-locale done summary is historically accurate and the remaining 2 locales (ta, zu-ZA) were authored by fn-5.1.

- Commits: 40ce7ffb
- Tests: dotnet format Humanizer.slnx --verify-no-changes
- PRs: