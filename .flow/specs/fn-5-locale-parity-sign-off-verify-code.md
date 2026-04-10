# Locale parity sign-off: verify code matches claims and docs match current state

## Overview

fn-1 (all 62 locales x 7 canonical surfaces), fn-3 (calendar + number.formatting overrides), and their documentation tasks (fn-1.10, fn-3.5) are landed. This epic is the final sign-off pass: confirm parity is actually achieved in code, fix remaining doc drift across end-user docs, contributing docs, agent-targeted docs (CLAUDE.md / AGENTS.md / ARCHITECTURE.md / readme.md / release_notes.md), and the repo-local skill at `.agents/skills/add-locale/`, then close fn-2 as proxy-complete.

A thorough pre-plan audit (context-scout + docs-gap-scout + flow-gap-analyst) and three plan-review passes found:

- **One real code/claim discrepancy with multiple stale claim sites**: `tools/verification-signoff.md:43`, `tools/verification-signoff.md:52`, and multiple narrative/Scope/Files/Acceptance passages across `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` and `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md` all claim `calendar.months` overrides for six locales: `bn, fa, he, ku, zu-ZA, ta`. YAML reality: only `bn`, `fa`, `he`, `ku` have `calendar:` blocks. `ta.yml` and `zu-ZA.yml` have none. fn-5.1 audits per locale (ta and zu-ZA are independent decisions — mixed outcomes are allowed) and produces a concrete `FinalOverrideSet`. fn-5.6 consumes `FinalOverrideSet` and reconciles the fn-3 historical spec/task markdown drift.
- **Probe coverage gap AND contract mismatch (surfaced during plan review)**: the existing `tools/probe-*-after.json` files capture only ~7 of 12 months per locale for `ta` (and similar partial coverage for other locales), because the probe samples specific reference dates rather than enumerating all months. On top of that, the probe emits `month_standalone` via `dt.ToString(" MMMM yyyy", culture)` — formatted-output proxy, not the raw `MonthNames` array the decision rule requires. "Zero variance" on partial formatted-output coverage does not prove full-month raw-array parity. fn-5.1's preferred evidence path is to extend **both** probe implementations in lockstep (`tools/locale-probe.cs` for macOS/Linux/Windows net10 AND `tools/locale-probe-net48/Program.cs` for Windows net48) with a new `month_names_raw` field emitting the raw 12-entry array, then re-run each probe on its reachable platform target. The fallback path (direct `CultureInfo.DateTimeFormat.MonthNames` enumeration) must still commit a checked-in artifact under `tools/` so future auditors can reproduce it; task-evidence JSON alone is not sufficient. CLDR data may corroborate an override decision but is **not** sufficient as a standalone sign-off source (CLDR proves the canonical value, not per-platform agreement).
- **Probe-shape reconciliation**: if fn-5.1 extends either `tools/locale-probe.cs` or `tools/locale-probe-net48/Program.cs`, the hardcoded totals / diff counts / "before-after identity" narrative in `tools/verification-signoff.md` may become stale and `tools/compare-probes.cs` may stop compiling against the new JSON shape. fn-5.1's acceptance includes a re-scan-and-reconcile step for both artifacts whenever probe shape changes in either implementation.
- **Verification-tool drift**: `tools/compare-probes.cs:22` hardcodes `string[] calendarOverrideLocales = ["bn", "fa", "he", "ku", "ta", "zu-ZA"]` — six locales. Whatever fn-5.1 decides, this script's array is aligned so it exactly matches `FinalOverrideSet`. Mixed outcomes (4, 5, or 6 locales) are legal.
- **Blocking doc drift** in two agent-targeted files:
  - `CLAUDE.md:76` and `AGENTS.md:45` still say "register in formatter/converter registries", which is false — the source generator wires everything automatically.
  - `CLAUDE.md:17` (test-running comment) and `AGENTS.md:29` both frame net48 as "avoid on Linux" / "avoid on macOS/Linux" when the real blocker is `Enum.GetValues<T>()` on all platforms (fn-4).
- **Missing release notes**: `release_notes.md` `vNext` has no entries for `calendar:` surface, `number.formatting:` sub-block, `phrase-clock` engine consolidation, or `DefaultTimeOnlyToClockNotationConverter` deletion — user-visible changes shipping in this window.
- **Stale repo-local skill**: `.agents/skills/add-locale/SKILL.md` and `references/parity-checklist.md` list 10 flat surface names (pre-fn-3 form), missing `number.formatting`, `calendar`, `calendar.months`, and `calendar.monthsGenitive`. The checklist paradoxically cites `CanonicalLocaleAuthoring.cs` as source-of-truth while failing its own stop condition. Parity-checklist also cites stale `Common/` paths for `LocaleRegistryInput.cs` and `OrdinalDateProfileCatalogInput.cs`.
- **Minor drift**: `readme.md:36` surface enumeration omits `calendar` / `number.formatting`; `ARCHITECTURE.md:40-51` generator table doesn't mention `LocaleNumberFormattingOverrides.g.cs` output; `ARCHITECTURE.md:88` prose lists 5 of 8 canonical surfaces; `tools/verification-signoff.md:64` shows `,` in the ku decimal-separator column but YAML has `٫` (U+066B).
- **Clean**: All four primary user docs (`docs/localization.md`, `docs/adding-a-locale.md`, `docs/locale-yaml-how-to.md`, `docs/locale-yaml-reference.md`) are accurate. No live references to any of the seven deleted clock converter classes exist outside `.flow/` history and intentional `DoesNotContain` assertions in `HumanizerSourceGeneratorTests.cs:68-70`.

## Scope

**In scope:**
- Reconcile the `calendar.months` discrepancy for `ta` and `zu-ZA` on a **per-locale basis**. The final authoritative override set (call it `FinalOverrideSet`) may contain 4, 5, or 6 locales depending on the audit outcome (any subset of the current six-locale claim). Downstream artifacts all derive from `FinalOverrideSet`, not from a hard-coded list. **Produced in fn-5.1.**
- Update current-reality artifacts driven directly by `FinalOverrideSet`: `tools/verification-signoff.md` (lines 43, 52, 64), `tools/compare-probes.cs:22`, optional `src/Humanizer/Locales/ta.yml` + `src/Humanizer/Locales/zu-ZA.yml` YAML authoring if needed, **parallel** extension of `tools/locale-probe.cs` AND `tools/locale-probe-net48/Program.cs` to emit `month_names_raw` (preferred-path evidence), `tools/probe-*-after.json` re-runs on reachable platforms, and any probe-shape reconciliation in `tools/verification-signoff.md` + `tools/compare-probes.cs` if probe shape changes. **Executed in fn-5.1.**
- Full-file audit of fn-3 historical planning artifacts: `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` and `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md`. Every six-locale reference is reconciled against `FinalOverrideSet` with a one-line rationale per removed locale. **Executed in fn-5.6** (split from fn-5.1 to keep both tasks sized M).
- Fix blocking doc drift in `CLAUDE.md` and `AGENTS.md`: stale registry instructions (both files) AND stale net48 framing (both files — `CLAUDE.md:17` and `AGENTS.md:29`). **Executed in fn-5.2.**
- Add missing `vNext` release-notes entries for `calendar:`, `number.formatting:`, `phrase-clock`, deleted converter, and cross-platform parity (locale count in the `calendar:` bullet matches `FinalOverrideSet`). **Executed in fn-5.3** (depends on fn-5.1 for the `calendar:` bullet count).
- Update `readme.md` and `ARCHITECTURE.md` surface lists + generator table. **Executed in fn-5.3.**
- Refresh repo-local skill (`.agents/skills/add-locale/SKILL.md` + `references/parity-checklist.md`) to match the 8-canonical + nested-members model, with corrected generator-input paths under `Generators/`. **Executed in fn-5.4.**
- Verify fn-2's proxy-close checklist (every fn-2 acceptance item maps to a satisfying artifact in fn-3.5 or this epic), gate on residual/regression scans passing, then close fn-2. **Executed in fn-5.5.**
- Append final sign-off section to `tools/verification-signoff.md` recording what was verified, outstanding items (fn-4 net48 blocker), and date. **Executed in fn-5.5; reconciled in fn-5.9 to remove all "DEFERRED TO CI" framing for net8.0 and to drop the fn-4 "blocker" prose after fn-5.7 fixed it.**
- Fix the net48 test build break (`Enum.GetValues<T>()` not resolved in test project) by adding `Polyfill` PackageReference to `tests/Humanizer.Tests/Humanizer.Tests.csproj`; subsumes `fn-4-fix-net48-test-suite-blocker`. **Executed in fn-5.7.**
- Re-run the net8.0 test suite locally (the .NET 8 SDK 8.0.419 is installed; the original fn-5.5 close falsely deferred this); revert the spec deferral language introduced by `commit d40bbbe6`; re-record fn-5.5 task evidence with the actual run output. **Executed in fn-5.8.**
- Reconcile `tools/verification-signoff.md` and `.flow/memory/pitfalls.md` to remove all "DEFERRED TO CI" / "deferred to follow-up" / "external blocker" framing that the prior sign-off pass embedded into the project's memory; close `fn-4-fix-net48-test-suite-blocker` as superseded. **Executed in fn-5.9.**

**Out of scope:**
- ~~Fixing the fn-4 net48 `Enum.GetValues<T>()` blocker — tracked separately, this epic only documents it as a known open item~~ **(Updated 2026-04-09: brought into scope as fn-5.7. The "blocker" was misclassified — it was a one-line PackageReference fix, not an external constraint. fn-4 is closed superseded in fn-5.9.)**
- Source-generator diagnostic that enforces "claimed overrides match YAML" — valuable follow-up, but a new build-time feature with its own test matrix; file as a follow-up epic after sign-off
- CI-lint for executable CLAUDE.md command blocks — docs-hygiene follow-up, not sign-off work
- Any changes to the four primary user docs (already accurate)
- Any changes to the source generator, runtime, or public API
- Any changes to production YAML surfaces beyond the `ta`/`zu-ZA` reconciliation decision in fn-5.1
- Adding drift-detection tests that would cover `tools/compare-probes.cs` automatically (build-time feature; deferred with the source-generator diagnostic)

## Approach

**Set-driven, not branch-name-driven.** fn-5.1 audits `ta` and `zu-ZA` **independently**. The result is a concrete `FinalOverrideSet` — any subset of `{bn, fa, he, ku, ta, zu-ZA}`. All downstream updates (verification-signoff.md, compare-probes.cs, fn-3 spec/task, release notes, ARCHITECTURE.md generator notes) derive from `FinalOverrideSet`. There is no separate "add overrides branch" vs "correct claims branch" — there is a single outcome that may entail YAML edits for some locales and claim corrections for others.

**Two-phase propagation** (the fn-5.1 / fn-5.6 split):
- **fn-5.1** is current-reality work: derive `FinalOverrideSet`, apply all edits to live code/data/tools that derive from the decision (verification-signoff.md, compare-probes.cs, optional YAML authoring, optional probe extension, probe-shape reconciliation). It is bounded M because the fn-3 historical audit is excluded.
- **fn-5.6** is planning-artifact audit work: read `FinalOverrideSet` from fn-5.1's done-summary, then full-file audit the two fn-3 markdown files with grep + one-line rationale per removed locale. It is bounded M because it is purely markdown with grep-guided edits.

This split was introduced after the third plan-review pass flagged fn-5.1 as an L-sized unit (risky execution, too many files). Splitting keeps both tasks within the M sweet spot and isolates the fn-3 historical audit as reviewable in isolation.

**Evidence discipline (fn-5.1).** The authoritative question is: are the 12 raw `CultureInfo.DateTimeFormat.MonthNames` entries for locale L byte-identical across every shipping platform target (macOS net10, Linux net10, Windows net10, Windows net48)? The contract is the raw array, not `ToString("MMMM")` formatted output — the existing probes emit `month_standalone` via `dt.ToString(" MMMM yyyy", culture)`, which is a formatted-output proxy and not suitable as the decision input. fn-5.1 must emit raw `MonthNames` data either by extending **both** probe implementations — `tools/locale-probe.cs` (macOS/Linux/Windows net10) **and** `tools/locale-probe-net48/Program.cs` (Windows net48) — with a new `month_names_raw` field per locale (preferred, checked-in, contract-accurate) or by running a direct `dotnet script` snippet and committing its output (fallback). Both probe implementations must stay in lockstep on the new field's JSON shape. The decision is **deterministic, not judgement-based**: a locale is excluded from `FinalOverrideSet` if and only if (a) **all four** platform targets were actually reached and (b) every reached platform returned a byte-identical 12-entry raw `MonthNames` array. If *any* platform is unreachable in the task environment (most likely Windows net48 on a macOS/Linux developer machine), the locale automatically stays in `FinalOverrideSet` — there is no "representative enough" loophole, and fallback-path evidence is not a substitute for unreachable platforms in the preferred path. CLDR data can corroborate an override decision (e.g., confirm the "correct" value), but CLDR alone does not prove per-platform agreement and therefore cannot justify removing an override from `FinalOverrideSet` on its own.

**Re-use patterns**: any new `calendar:` blocks follow the existing pattern in `src/Humanizer/Locales/bn.yml`, `fa.yml`, `he.yml`, `ku.yml` (grep `^  calendar:` to find the current line ranges — historic line numbers `515-530`, `416-428`, `493-508`, `399-412` were approximate and should be verified before use). Any new override decisions follow the selection-criteria discipline from `docs/locale-decisions.md` (each entry cites source, value, rationale, and a concrete CLDR version).

**Single source of truth** for canonical surfaces: `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:44-54` (8 canonical surfaces) + `:198-202` (number nested members) + `:251-255` (ordinal nested members) + `:284-289` (calendar nested members). Every doc update must either (a) enumerate the current form or (b) link to this file.

**Source-generator entrypoint paths** (corrected from prior audit):
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` (not `Common/…`) — emits `LocaleNumberFormattingOverrides.g.cs` and master registry wiring
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` (not `Common/…`) — consumes `calendar.months` / `calendar.monthsGenitive`

**Proxy-close discipline (fn-5.5)**: before marking fn-2 done, each of its 5 acceptance items must be explicitly mapped to a satisfying artifact with a file:line or task reference — no hand-waving. The close is **gated on all residual/parity/regression scans passing**: fn-5.5 prepares the mapping first, runs all scans second, gates on pass/fail third, then executes `flowctl done fn-2.1` fourth, and appends the final sign-off report fifth. The ordering is a correctness constraint, not a suggestion — if a scan fails, fn-2 must not be closed until the root cause is fixed. The scan battery includes regression checks for stale manual-registry phrasing (fn-5.2 + fn-5.4 regression) and stale `Common/` generator-input paths (fn-5.4 regression).

**Sign-off correction (fn-5.7 / fn-5.8 / fn-5.9, added 2026-04-09)**: the original fn-5.5 close shipped with two defects that violate the project's no-deferrals rule and must be remediated:

1. **net8.0 was falsely deferred to CI.** `tools/verification-signoff.md:98-104` claims `.NET 8.0 SDK is not installed on this machine (only .NET 10.0.2 is available)` and explicitly defers the net8.0 test run. The premise is false: `dotnet --list-sdks` reports `8.0.419` and `10.0.102`, with the .NET 8 runtime also installed. The deferral was then propagated into the spec by commit `d40bbbe6 fix(signoff): update epic spec to allow net8.0 CI deferral`, which softened the R14 acceptance criterion to add a deferral escape clause. **Specs may only be tightened by sign-off work, never loosened.** fn-5.8 reverts the spec edit, runs the net8.0 test suite locally, and re-records fn-5.5's task evidence with the actual run output.

2. **The fn-4 net48 build break was treated as an out-of-scope external blocker.** `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs:379` uses `Enum.GetValues<GrammaticalGender>()`, which fails to compile under `TargetFramework=net48` because the test project does not reference `Polyfill`. The library project DOES reference Polyfill (`PrivateAssets="all"`), and Polyfill 9.18.0 exposes `Enum.GetValues<TEnum>()` via C# 14 `extension(Enum)` syntax — verified by the library's clean net48 build. fn-5.7 fixed it with an `#if NET5_0_OR_GREATER` guard at the call site (the Polyfill PackageReference approach was tried first but caused type conflicts). No "external blocker" existed; the fn-4 epic carried no tasks because the work was misclassified. fn-5.7 made the fix; fn-5.9 closes fn-4 as superseded.

3. **`.flow/memory/pitfalls.md` was actively contaminated.** Lines 61-62 (added during the original sign-off pass) endorsed "update the governing spec to explicitly allow the deferral" as a best practice. That entry is the inverse of the project's actual rule and must be deleted and replaced with the no-loosening rule. Adjacent entries (43-44 "downgrade to documented follow-up", 52-53 "use DEFERRED for untested platforms") encode the same wrong pattern and must be reframed. fn-5.9 owns this cleanup.

The correction tasks must not introduce any new escape clauses, soften any other acceptance criteria, or weaken the sign-off in any way. Spec edits in this correction go in one direction only: **tighter**.

## Task DAG

```
fn-5.1 (derive FinalOverrideSet + current-reality updates)
  ├─→ fn-5.3 (release_notes.md + readme.md + ARCHITECTURE.md; reads FinalOverrideSet)
  └─→ fn-5.6 (fn-3 historical spec/task audit; reads FinalOverrideSet)

fn-5.2 (CLAUDE.md + AGENTS.md drift)          [parallel]
fn-5.4 (add-locale skill refresh)              [parallel]

fn-5.5 (sign-off + scans + fn-2 close)
  depends on: fn-5.1, fn-5.2, fn-5.3, fn-5.4, fn-5.6

# Sign-off correction tasks (added after fn-5.5 close was found to have falsely deferred net8.0)
fn-5.7 (net48 build break fix; subsumes fn-4)  [parallel — no deps on prior tasks]
fn-5.8 (run net8.0 locally + restore strict acceptance + re-record fn-5.5 evidence)
  depends on: fn-5.5

fn-5.9 (reconcile sign-off doc + remove improper pitfall entries + close fn-4 superseded)
  depends on: fn-5.7, fn-5.8
```

## Quick commands

```bash
# Verify docs build / lint after changes
dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic

# Full test suite on modern targets (run BOTH; no deferral)
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 -c Release
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0  -c Release

# net48 build sanity (after fn-5.7 lands; test execution still requires Windows host)
dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net48

# Parity-claim verification: enumerate the authoritative override set from YAML
grep -l "^  calendar:" src/Humanizer/Locales/*.yml   # Should match FinalOverrideSet exactly
grep -l "^    formatting:" src/Humanizer/Locales/*.yml

# Full 12-month enumeration for ta and zu-ZA directly from CultureInfo
# (run on each platform target to confirm zero variance before removing a locale from FinalOverrideSet)
dotnet script -e 'using System.Globalization; foreach (var c in new[]{"ta","zu-ZA"}) { var d = CultureInfo.GetCultureInfo(c).DateTimeFormat; Console.WriteLine($"{c}: {string.Join(\"|\", d.MonthNames.Take(12))}"); }'

# Deleted-converter residual scan (should only match .flow/ history and HumanizerSourceGeneratorTests.cs DoesNotContain assertions)
grep -r "GermanTimeOnlyToClockNotationConverter\|FrenchTimeOnlyToClockNotationConverter\|LuxembourgishTimeOnlyToClockNotationConverter\|JapaneseTimeOnlyToClockNotationConverter\|DefaultTimeOnlyToClockNotationConverter\|PhraseHourClockNotationConverter\|RelativeHourClockNotationConverter" src/ tests/ docs/ .agents/ .claude/ CLAUDE.md AGENTS.md ARCHITECTURE.md readme.md release_notes.md

# Residual conceptual-language scan
grep -rn "residual" docs/ CLAUDE.md AGENTS.md ARCHITECTURE.md readme.md

# net48 framing audit (should return zero after fn-5.2)
grep -rn "avoid net48 on" CLAUDE.md AGENTS.md

# Manual-registry phrasing audit (fn-5.2 + fn-5.4 regression check)
grep -rn "register in formatter/converter registries\|register new formatters/converters" CLAUDE.md AGENTS.md .agents/skills/add-locale/

# Stale Common/ generator-input path audit (fn-5.4 regression check)
grep -rn "Common/LocaleRegistryInput\|Common/OrdinalDateProfileCatalogInput" .agents/ CLAUDE.md AGENTS.md ARCHITECTURE.md readme.md docs/

# fn-3 historical drift audit (fn-5.6 regression check)
grep -n "\bta\b\|zu-ZA\|6 locale\|six locale" .flow/specs/fn-3-hard-code-locale-overrides-where-icu.md .flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md
```

## Acceptance

- [ ] `FinalOverrideSet` determined per-locale for `ta` and `zu-ZA`, producing a concrete set with 4, 5, or 6 members, stated explicitly in fn-5.1 done-summary
- [ ] Decision for each audited locale (`ta`, `zu-ZA`) grounded in full 12-month raw `CultureInfo.DateTimeFormat.MonthNames` evidence — NOT `ToString("MMMM")` formatted-output samples
- [ ] **Preferred-path two-probe lockstep**: if the preferred path is used, **both** `tools/locale-probe.cs` AND `tools/locale-probe-net48/Program.cs` are extended in parallel to emit a `month_names_raw` field with byte-identical JSON shape, each probe is re-run on its reachable platform target, and all updated `tools/probe-*-after.json` files are committed. Updating only one probe implementation does NOT satisfy this criterion.
- [ ] **Fallback-path artifact**: if the fallback path is used for any platform, a checked-in artifact under `tools/` contains the raw enumeration output for every platform covered by the fallback, alongside the snippet source used to produce it
- [ ] Path chosen, rationale, and any unreachable platforms explicitly documented in fn-5.1 done-evidence
- [ ] For each locale in `FinalOverrideSet` that does not already exist in YAML: `src/Humanizer/Locales/{locale}.yml` contains `calendar:` block with valid 12-entry `months:` array
- [ ] `grep -l "^  calendar:" src/Humanizer/Locales/*.yml` output matches `FinalOverrideSet` exactly (no extra, no missing)
- [ ] `tools/compare-probes.cs:22` `calendarOverrideLocales` array literal matches `FinalOverrideSet` exactly; the script still compiles/runs with whatever size the array ends up
- [ ] `tools/verification-signoff.md` has zero remaining six-locale `calendar.months` claims outside the final sign-off section — every such claim (lines 43, 52, and any others introduced by fn-3.5) updated to match `FinalOverrideSet`
- [ ] If fn-5.1 changed probe shape in **either** `tools/locale-probe.cs` or `tools/locale-probe-net48/Program.cs` (or any `tools/probe-*-after.json` re-run with new coverage), `tools/verification-signoff.md` has been re-scanned for hardcoded totals / diff counts / "before-after identity" claims and updated accordingly, and `tools/compare-probes.cs` has been re-verified to still compile against the new JSON shape; if no shape change in either probe, this is recorded as N/A in fn-5.1 task evidence
- [ ] `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` full-file audited: every `bn, fa, he, ku, zu-ZA, ta` (or `ta, zu-ZA`) reference in Scope / Acceptance / Early proof point / any other section updated to match `FinalOverrideSet`, with one-line rationale for each locale removed from the set (fn-5.6)
- [ ] `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md` full-file audited: every six-locale reference in Description, Files list, Per-locale YAML population section, Key context, Early proof point, Acceptance, and anywhere else updated to match `FinalOverrideSet`, with one-line rationale for each locale removed (fn-5.6)
- [ ] `CLAUDE.md:76` no longer instructs contributors to "register in formatter/converter registries"; explains source generator wires registries automatically (fn-5.2)
- [ ] `AGENTS.md:45` same stale instruction removed (fn-5.2)
- [ ] `CLAUDE.md:17` net48 test-running comment reframed: real blocker is `Enum.GetValues<T>()` on all platforms (see fn-4), not "avoid on macOS/Linux" (fn-5.2)
- [ ] `AGENTS.md:29` net48 guidance reframed identically: blocked by `Enum.GetValues<T>()` (see fn-4), not Linux-specific (fn-5.2)
- [ ] `grep -rn "avoid net48 on" CLAUDE.md AGENTS.md` returns zero matches
- [ ] Both `CLAUDE.md` and `AGENTS.md` mention `calendar:` and `number.formatting:` surfaces as the override escape hatch for ICU-divergent locales (fn-5.2)
- [ ] `release_notes.md` `vNext` section contains entries for: unified `phrase-clock` engine (replaces `phrase-hour`, `relative-hour`, four residual leaves), `calendar:` surface with `months`/`monthsGenitive` naming `FinalOverrideSet` members (read from fn-5.1 done-summary), `number.formatting.decimalSeparator`, cross-platform consistency guarantee for overridden locales (fn-5.3)
- [ ] `readme.md` Locale Data section (line ~32-36) enumerates the 8 canonical surfaces including `calendar:` and `number.formatting:` (or links to `docs/localization.md` as authoritative) (fn-5.3)
- [ ] `ARCHITECTURE.md` generator table mentions `LocaleNumberFormattingOverrides.g.cs` output and `calendar.months`/`calendar.monthsGenitive` wiring via `OrdinalDateProfileCatalogInput` (fn-5.3)
- [ ] `ARCHITECTURE.md:88` prose summary lists all 8 canonical surfaces (or omits enumeration and links to `docs/localization.md`) (fn-5.3)
- [ ] `.agents/skills/add-locale/SKILL.md` surface inventory (lines ~63-74) updated to match `CanonicalLocaleAuthoring.cs:44-54`: 8 canonical surfaces + nested members including `number.formatting`, `calendar.months`, `calendar.monthsGenitive` (fn-5.4)
- [ ] `.agents/skills/add-locale/SKILL.md` required-proof-subrows (lines ~101-119) include rows for `calendar.months`, `calendar.monthsGenitive`, `number.formatting.decimalSeparator` (marked "not applicable" / "inherited" when the locale does not author an override) (fn-5.4)
- [ ] `.agents/skills/add-locale/references/parity-checklist.md` surface inventory (lines ~9-28), proof subrows (lines ~98-109), and surface-to-files matrix (lines ~144-155) all updated the same way, with generator-input paths corrected to `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` and `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` (fn-5.4)
- [ ] `tools/verification-signoff.md:64` `ku` decimal-separator override column shows `٫` (U+066B) to match `src/Humanizer/Locales/ku.yml:333` (fn-5.1)
- [ ] fn-2 proxy-close executed: every fn-2 acceptance item mapped to a satisfying artifact (fn-3.5 task section or this epic's tasks), **mapping prepared before scans, fn-2 closed only after scans pass**, then fn-2 marked done via `flowctl` (fn-5.5)
- [ ] fn-5.5 scan battery (2a-2i) passes: deleted-converter residual (scope-based allowlist, not count-based), conceptual-language, stale net48 framing, stale manual-registry phrasing, stale `Common/` paths, calendar.months / number.formatting YAML **exact-set equality** against every claim site (not just cardinality), fn-3 historical drift, supported-languages list, full modern-target test suite — all capturing verbatim output in task evidence
- [ ] Deleted-converter residual scan: every match (regardless of count) falls within an allowlisted scope: (1) `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/HumanizerSourceGeneratorTests.cs` lines 68-70, or (2) `release_notes.md` vNext section (changelog entries documenting converter removal); any match outside these scopes is a regression
- [ ] `tools/verification-signoff.md` has a final sign-off section with date, verified items, the `FinalOverrideSet` literal, explicit out-of-scope list (fn-4 net48 blocker), six-task enumeration (fn-5.1 through fn-5.6), and reference to this epic (fn-5.5)
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` passes
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0` passes
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0` passes
- [ ] `tests/Humanizer.Tests/Humanizer.Tests.csproj` builds cleanly for net48 (`dotnet build -f net48` exits 0; net48 test execution requires a Windows host but the project compiles on every platform) — verified in fn-5.7, which subsumes the work originally tracked in fn-4
- [ ] `.flow/specs/fn-5-locale-parity-sign-off-verify-code.md` contains no deferral escape-clause language (no loosening of acceptance, no SDK-unavailability exceptions) anywhere in the Acceptance section or the requirement-coverage table — strict acceptance, no deferral path (fn-5.8)
- [ ] `tools/verification-signoff.md` contains no `DEFERRED TO CI`, `DEFERRED (.NET 8 not installed)`, `Next CI run`, or `Explicitly deferred to CI` framing in the post-execution / verdict / outstanding-items sections; cross-platform runs that occur on a non-macOS host are framed as "REQUIRES <host>" CI verification, not as "deferred" items (fn-5.9)
- [ ] `.flow/memory/pitfalls.md` does not contain the line `update the governing spec to explicitly allow the deferral` or any equivalent endorsement of weakening acceptance criteria; replaced with the inverse rule (fix the work, never weaken the criterion) (fn-5.9)
- [ ] `fn-4-fix-net48-test-suite-blocker` epic is closed (`status: done`) with completion review note containing `Superseded by fn-5-locale-parity-sign-off-verify-code.7` and a reference to the fn-5.7 commit (fn-5.9)
- [ ] `CLAUDE.md` and `AGENTS.md` no longer frame net48 as "blocked on all platforms by Enum.GetValues<T>()" or "tracked as fn-4" or "do not invoke it"; replaced with honest framing (build green on every platform; test execution requires Windows host) (fn-5.7)

## Early proof point

Task `fn-5-locale-parity-sign-off-verify-code.1` (derive `FinalOverrideSet` + current-reality updates) validates the fundamental sign-off premise: that claims can be verified against YAML ground truth with reproducible evidence. The plan-review passes surfaced that (a) existing probe data only samples ~7 of 12 months per locale, (b) the probe emits formatted-output (`month_standalone`) rather than raw `MonthNames`, (c) CLDR alone is insufficient to prove per-platform agreement, (d) the audit must allow mixed per-locale outcomes, (e) evidence must be checked-in and reproducible — not trapped in task-evidence JSON, and (f) the repo ships **two** probe implementations (`tools/locale-probe.cs` for net10 platforms and `tools/locale-probe-net48/Program.cs` for Windows net48), both of which must be extended in lockstep when taking the preferred path. If fn-5.1's preferred path (parallel extension of both probes + re-running on reachable platforms) is not practical in the task environment, the fallback is direct `CultureInfo.DateTimeFormat.MonthNames` enumeration with a committed artifact under `tools/`; if even that is blocked on some platforms, the deterministic rule applies — the locale stays in `FinalOverrideSet`, the evidence gap is documented, and a follow-up is filed to tighten later.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1 | Determine `FinalOverrideSet` per-locale for `ta`/`zu-ZA` with full 12-month raw `MonthNames` evidence (preferred: extend **both** `tools/locale-probe.cs` AND `tools/locale-probe-net48/Program.cs` in lockstep emitting `month_names_raw` + re-run each probe on reachable platforms; fallback: direct `CultureInfo` enumeration with checked-in artifact under `tools/`; CLDR corroboration only) | .1 | — |
| R1a | Update every stale six-locale `calendar.months` claim site in `tools/verification-signoff.md` to match `FinalOverrideSet` | .1 | — |
| R1b | Align `tools/compare-probes.cs:22` `calendarOverrideLocales` array to `FinalOverrideSet` | .1 | — |
| R1c | Full-file audit of `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` for stale six-locale claims (not line-specific) | .6 | — |
| R1d | Full-file audit of `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.3.md` for stale six-locale claims (not line-specific) | .6 | — |
| R1e | If fn-5.1 changes probe shape, reconcile any probe-shape-dependent narrative (totals, diff counts, "before-after identity") in `tools/verification-signoff.md` | .1 | — |
| R2 | Fix `ku` decimal-separator display typo in signoff tracker | .1 | — (same file as R1a) |
| R3 | Fix stale "register in formatter/converter registries" in CLAUDE.md and AGENTS.md | .2 | — |
| R4 | Reframe `AGENTS.md:29` AND `CLAUDE.md:17` net48 guidance (real blocker is `Enum.GetValues<T>()`) | .2 | — |
| R5 | Mention `calendar:` / `number.formatting:` escape hatch in CLAUDE.md + AGENTS.md | .2 | — |
| R6 | Add missing `vNext` release-notes entries (`calendar:` bullet naming `FinalOverrideSet`, `number.formatting:`, `phrase-clock`, deleted converter) | .3 | — (depends on .1 for FinalOverrideSet count) |
| R7 | Update `readme.md` canonical surface enumeration | .3 | — |
| R8 | Update `ARCHITECTURE.md` generator table + prose | .3 | — |
| R9 | Update `.agents/skills/add-locale/SKILL.md` surface inventory + proof rows | .4 | — |
| R10 | Update `.agents/skills/add-locale/references/parity-checklist.md` surface inventory + proof rows + matrix (with corrected generator-input paths) | .4 | — |
| R11 | Execute fn-2 proxy-close (mapping table + gated `flowctl done`) — mapping prepared before scans, close executed only after scans pass | .5 | — |
| R12 | Residual / regression scan battery: no stale "residual leaves" prose, no live deleted-converter references outside allowlisted file/line scope, no stale "avoid net48 on Linux" phrasing, no stale manual-registry phrasing, no stale `Common/` generator-input paths, calendar.months / number.formatting YAML **exact-set equality** (string-for-string, not cardinality) with every claim site, fn-3 historical drift reconciled | .5 | — |
| R13 | Append final sign-off section to `tools/verification-signoff.md` | .5 | — |
| R14 | `dotnet format --verify-no-changes` passes AND full modern-target test suite passes on **both** `net10.0` and `net8.0` (run locally; no deferral) | .5, .8 | — |
| R15 | Source-generator diagnostic enforcing claim/YAML parity | — | Deferred — new build-time feature with own test matrix; file as follow-up epic after sign-off |
| R16 | CI-lint for executable CLAUDE.md command blocks | — | Deferred — docs-hygiene follow-up, not sign-off work |
| R17 | Fix fn-4 net48 test-suite blocker | .7, .9 | Subsumed by fn-5.7; fn-4 closed superseded in fn-5.9 |
| R18 | Drift-detection test that would catch future `compare-probes.cs` / claim-site divergence automatically | — | Deferred — build-time feature; fold into R15 follow-up epic |
| R19 | Restore strict net8.0 acceptance: revert deferral language in spec (R14 acceptance bullet + requirement-coverage row), run net8.0 test suite locally, re-record fn-5.5 task evidence with the actual test run output | .8 | — |
| R20 | Reconcile `tools/verification-signoff.md` to remove all "DEFERRED TO CI" framing for net8.0 (now passing locally) and reframe non-macOS host runs as CI verification (not deferrals); remove all stale fn-4 / "Enum.GetValues blocker" prose; update `### Verification status grid`, `### Final Verdict`, and `### Outstanding Cross-Platform Items` sections | .9 | — |
| R21 | Reconcile `.flow/memory/pitfalls.md`: delete the line-61-62 entry endorsing "update the governing spec to explicitly allow the deferral"; replace with the inverse no-loosening rule; reframe lines 43-44 and 52-53 to align with the no-deferrals rule | .9 | — |
| R22 | Close `fn-4-fix-net48-test-suite-blocker` as superseded with completion review note + add `> **Status: Superseded by fn-5.7**` blockquote at top of fn-4 spec | .9 | — |
| R23 | Fix net48 test build break (`Enum.GetValues<T>()` not resolved): add `#if NET5_0_OR_GREATER` guard at `LocaleTheoryMatrixCompletenessTests.cs:379` (Polyfill PackageReference approach caused type conflicts); verify all 3 TFMs build green; verify no test regressions on net8.0 / net10.0; update `CLAUDE.md` and `AGENTS.md` to drop the "blocked / see fn-4" framing | .7 | — |
