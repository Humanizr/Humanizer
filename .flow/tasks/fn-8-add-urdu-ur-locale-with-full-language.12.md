# fn-8-add-urdu-ur-locale-with-full-language.12 Cross-locale grammatical-gender audit + Romance/Germanic fills

## Description

Audit every shipped locale for grammatical-gender coverage (ordinals primarily, other surfaces if `.9`'s plumbing exposes them), then fill Romance and Germanic locale gaps in this task. Slavic fills are in `.13`; Semitic / Indic / other fills are in `.14`. Each follows the same engine plumbing introduced by `.9`. Any ordinary data gap is blocking and must land in `.12`/`.13`/`.14`. Only gaps requiring research beyond CLDR Ordinal Rules + one authoritative published grammar per locale may escape to a follow-up task outside the epic — IDs recorded in the audit artifact.

**Size:** M (audit + Romance/Germanic fill; Slavic in `.13`, Semitic/Indic in `.14`)
**Files:**
- `artifacts/2026-04-12-grammatical-gender-audit.md` (new, committed)
- Per-locale YAML edits for Romance + Germanic locales (`es`, `fr`, `it`, `pt`, `ro`, `de`, `nl`, `sv`, any others in those families)
- Per-locale test edits under `tests/Humanizer.Tests/Localisation/{locale}/` for each filled locale

## Approach

1. **Audit** — produce `artifacts/2026-04-12-grammatical-gender-audit.md`:
   - Section per **currently shipped** gender-bearing locale, discovered by enumerating `src/Humanizer/Locales/*.yml` and cross-referencing `LocaleCoverageData.ShippedLocales`. Candidates typical for this repo include `ar`, `bg`, `cs`, `de`, `el`, `es`, `fr`, `he`, `hr`, `it`, `nl`, `pl`, `pt`, `ro`, `ru`, `sk`, `sl`, `sr`, `sv`, `uk` — confirm against shipped YAML before including. Do NOT include non-shipped locales (e.g. `hi` is not shipped; do not add it here).
   - Fields: gender system (masc/fem, masc/fem/neuter, common/neuter…); current Humanizer YAML coverage; gaps; fill owner (`.12` / `.13` / `.14` / external follow-up).
2. **Classify each gap**:
   - **(A) In-scope fill**: CLDR Ordinal Rules + one authoritative published grammar resolves it — owned by `.12` / `.13` / `.14` based on language family.
   - **(B) Research-bound escape**: published grammars disagree or data is insufficient. Requires explicit evidence in the audit. Spawns a follow-up Flow task OUTSIDE the epic; task ID recorded.
3. **Fill Romance + Germanic in `.12`**:
   - Romance: `es` (Spanish), `fr` (French), `it` (Italian), `pt` (Portuguese), `ro` (Romanian).
   - Germanic: `de` (German), `nl` (Dutch), `sv` (Swedish), plus any others in that family if shipped.
   - Use `.9` engine plumbing as-is. No per-locale runtime code.
   - YAML additions per each locale's ordinalizer profile shape (per-gender `suffix` / `template` / `word-form-template` fields).
   - Proposer+reviewer per term.
   - `Neuter → Masculine` fallback for languages without neuter ordinals (same canonical pattern from `.9`).
4. **Test additions**:
   - Extend (or create) `tests/Humanizer.Tests/Localisation/{locale}/` with per-gender ordinal assertions.
   - `LocaleTheoryMatrixCompletenessTests.AllLocaleGenderTheoryData` passes for every filled locale.
5. **Re-plan trigger**: if audit reveals structural assumptions from `.9` don't hold for Romance/Germanic ordinals (unlikely — these families fit suffix/template engines cleanly), post re-plan note before filling. Slavic/Indic families live in `.13`/`.14` with their own re-plan triggers.

## Investigation targets

**Required**:
- `src/Humanizer/Locales/*.yml` — all shipped locales post-`.9`
- Parity map `.9` entry — gendered-ordinal plumbing shape
- https://www.unicode.org/cldr/charts/latest/supplemental/language_plural_rules.html — CLDR Ordinal Rules
- https://wals.info/feature/30A — WALS gender inventory

**Optional**:
- Per-language authoritative published grammars (one reference per locale to back proposer+reviewer)

## Key context

- User-memory: "No deferrals or spec-loosening." Only Category B research-bound gaps escape; everything else is blocking.
- `.12` owns audit + Romance/Germanic. `.13` owns Slavic. `.14` owns Semitic/Indic/other. `.7` blocks on all three.
- `Neuter → Masculine` fallback is the canonical pattern established by `.9` for two-gender languages.
- Backward-compat spine: every locale with no grammatical gender (`en`, `tr`, `ja`, `zh`, `ko`, `fi`, `hu`, `et`, `lv`, …) continues to emit identical output to pre-`.9` state. Regression tests pass.

## Acceptance

- [ ] `artifacts/2026-04-12-grammatical-gender-audit.md` exists (committed). Contains:
  - Every shipped locale's gender system + current Humanizer coverage + identified gaps.
  - Each gap classified A (in-scope fill with owning task `.12` / `.13` / `.14`) or B (research-bound follow-up outside epic with Flow ID + evidence).
- [ ] Romance locales (`es`, `fr`, `it`, `pt`, `ro`) with identified Category A gaps filled: YAML per-gender tables added + per-gender tests added under `tests/Humanizer.Tests/Localisation/{locale}/`.
- [ ] Germanic locales (`de`, `nl`, `sv`, any others) with identified Category A gaps filled similarly.
- [ ] `LocaleTheoryMatrixCompletenessTests.AllLocaleGenderTheoryData` passes for every locale filled in this task (including Neuter-fallback rows for languages without linguistic neuter).
- [ ] Backward-compat: every no-grammatical-gender locale continues to emit identical output to its pre-`.9` state.
- [ ] Proposer+reviewer audit trail recorded for every new gendered term.
- [ ] Re-plan note posted if Romance/Germanic structural assumptions from `.9` don't hold.
- [ ] `LocaleRegistrySweepTests` passes for every locale.
- [ ] No locale marked "deferred" without Category B evidence + external follow-up Flow task ID.

## Done summary
Completed cross-locale grammatical-gender audit and Romance/Germanic fills:

1. Created `artifacts/2026-04-12-grammatical-gender-audit.md` documenting all 65 shipped locales' gender systems, ordinalizer engine types, current YAML coverage, and identified gaps classified by owning task (.12/.13/.14) or external follow-up.

2. Filled Category A gaps for 6 Germanic locales (de, nl, da, nb, is, lb) by adding explicit `feminineSuffix` and `neuterSuffix` YAML entries. All have gender-invariant numeric ordinals — suffixes are identical to masculine but now self-documenting rather than relying on engine fallback.

3. All Romance locales (es, fr, it, pt, ro, ca) and their variants (fr-BE, fr-CH, pt-BR) already had complete gender coverage — no fills needed. Regional Germanic variants (de-CH, de-LI, nn) inherit from their parent locales.

4. No Category B (research-bound) gaps found — all shipped locales' ordinals are well-documented in CLDR and standard grammars.

5. Slavic fills (.13) and Semitic/Indic fills (.14) documented in audit with owning task IDs.

Full suite result: 40,309/40,310 passing; one pre-existing unrelated uz-Latn-UZ clock notation failure.

## Evidence
- Commits: (pending)
- Tests: full_suite (40309/40310 pass), source_generator_tests (71/71 pass)
- PRs: