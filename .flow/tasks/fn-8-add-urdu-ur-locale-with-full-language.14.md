# fn-8-add-urdu-ur-locale-with-full-language.14 Grammatical-gender fills: Semitic / Indic / other gender-bearing locales

## Description

Fill grammatical-gender ordinal gaps in remaining gender-bearing locale families from the `.12` audit — primarily Semitic (`ar`, `he`), any currently-shipped Indic locales discovered by the audit (Hindi `hi` is NOT currently shipped — adding it is out of scope for this epic), and any remaining gender-bearing locales such as `el` (Greek). Uses the engine plumbing introduced by `.9`. Only Category A gaps in scope; Category B research-bound gaps escape via external Flow task IDs in the audit.

**Size:** M
**Files:** per-locale YAML edits for whichever of `ar`, `he`, `el`, plus any other remaining gender-bearing locale actually present in `src/Humanizer/Locales/`; per-locale test edits under `tests/Humanizer.Tests/Localisation/{locale}/`; `artifacts/2026-04-12-grammatical-gender-audit.md` (update).

Scope is data-driven — audit `src/Humanizer/Locales/*.yml` actually shipped after `.11` lands; only fill locales that exist. Do NOT add new locales in this task.

## Approach

1. Re-anchor on `.12` audit artifact — Semitic / Indic / other sections.
2. **YAML additions** per locale's ordinalizer profile:
   - **Arabic (`ar`)**: masculine + feminine; review `arabic-like` plural-rule interaction with ordinal gender.
   - **Hebrew (`he`)**: masculine + feminine; review existing formatter plural-rule with ordinal gender.
   - **Greek (`el`)** (if shipped and audit finds gaps): masculine + feminine + neuter where the language distinguishes.
   - Any **currently-shipped** Indic locale surfaced by the audit. Hindi (`hi`) is NOT shipped today — adding it is out of scope for this epic.
3. **Proposer+reviewer** per term. CLDR Ordinal Rules (where available) + one authoritative published grammar per locale.
4. **Tests**: per-gender assertions + one compound ordinal (100, 101) per gender per locale.
5. **Re-plan triggers**:
   - Arabic six-form plural-rule interaction with ordinal gender may expose engine gaps.
   - Hebrew construct-state nuances may require additional fields.
   - If any family exposes a gap `.9`'s engine cannot close, STOP and re-plan before filling.
6. Category B escapes: explicit evidence + external Flow task IDs in the audit.

## Investigation targets

**Required**:
- `.12` audit artifact — Semitic / Indic / other sections
- Parity map `.9` entry — gendered-ordinal plumbing shape
- CLDR Ordinal Rules per locale
- One authoritative published grammar per locale

## Key context

- Languages without linguistic neuter use Neuter→Masculine fallback (canonical pattern from `.9`).
- Languages with true three-gender (Greek) get real neuter tables.
- Back-compat: unrelated locales unchanged.
- `.7` blocks on `.12`, `.13`, `.14` collectively.

## Acceptance

- [ ] Every Semitic / Indic / other gender-bearing locale with Category A gaps filled: YAML per-gender tables + per-gender tests.
- [ ] `LocaleTheoryMatrixCompletenessTests.AllLocaleGenderTheoryData` passes for every filled locale.
- [ ] Proposer+reviewer audit trail recorded in `.12` audit artifact.
- [ ] Re-plan note posted if `.9`'s plumbing was insufficient for any shipped Semitic/Indic/other target.
- [ ] `LocaleRegistrySweepTests` passes.
- [ ] No locale marked "deferred" without Category B evidence + external follow-up Flow task ID.

## Done summary

Filled grammatical-gender ordinal gaps for all 3 Semitic locales (ar, he, mt) by adding explicit `feminineSuffix: ''` and `neuterSuffix: ''` YAML keys to each locale's `ordinal.numeric` section. All use empty string as a gender-invariant numeric ordinal suffix (bare digits, no suffix marker).

Created 3 per-locale gendered ordinal test classes (ArabicGenderedOrdinalTests, HebrewGenderedOrdinalTests, MalteseGenderedOrdinalTests) with coverage for values 1, 2, 23, 100, 101 across all three `GrammaticalGender` enum values, plus gender-invariance assertions.

Updated the audit artifact with filled status for all 3 Semitic locales, proposer+reviewer evidence trail entries with named authoritative grammar references (Wright for Arabic, Glinert for Hebrew, Borg & Azzopardi-Alexander for Maltese).

No other gender-bearing locales had Category A gaps remaining: Greek (el) already had full M/F/N coverage, Bengali (bn) and Tamil (ta) have gender-invariant ordinals, Hindi (hi) is not shipped. No re-plan needed: the existing suffix engine and source generator fully support the added YAML keys without any runtime changes.

## Evidence
- Commits: 00bc600b, e5612077
- Tests: ArabicGenderedOrdinalTests, HebrewGenderedOrdinalTests, MalteseGenderedOrdinalTests, LocaleTheoryMatrixCompletenessTests, LocaleRegistrySweepTests (40618 pass, 1 pre-existing uz-Latn-UZ failure)
- PRs:
