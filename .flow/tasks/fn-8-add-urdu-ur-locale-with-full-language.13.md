# fn-8-add-urdu-ur-locale-with-full-language.13 Grammatical-gender fills: Slavic + Baltic locales

## Description

Fill grammatical-gender ordinal gaps in Slavic locales identified by the `.12` audit artifact. Slavic languages typically have three genders (masculine / feminine / neuter) and require real per-gender ordinal tables — no silent neuter→masculine fallback in languages that genuinely distinguish three forms. Uses the engine plumbing introduced by `.9`. Only Category A gaps (CLDR Ordinal Rules + one authoritative published grammar resolves) are in scope here; Category B research-bound gaps escape to follow-up tasks outside the epic with IDs recorded in the audit.

**Size:** M
**Files:** per-locale YAML edits under `src/Humanizer/Locales/` for `bg`, `cs`, `hr`, `pl`, `sk`, `sl`, `sr`, `sr-Latn`, `lt`, `lv` (ru/uk already filled); per-locale test files under `tests/Humanizer.Tests/Localisation/{locale}/`; `artifacts/2026-04-12-grammatical-gender-audit.md` (update).

## Approach

1. Re-anchor on `.12` audit artifact — Slavic section.
2. **YAML additions** per locale's ordinalizer profile shape:
   - Masculine table (often the existing root table).
   - Feminine table.
   - Neuter table for languages that distinguish it — do NOT silently fall back to masculine.
3. **Proposer+reviewer** per term. Use CLDR Ordinal Rules + one authoritative published grammar per locale.
4. **Tests**: per-gender assertions plus one compound ordinal (100, 101) per gender.
5. **Re-plan trigger**: Slavic three-gender ordinals may require an engine feature `.9` didn't include (e.g. animate/inanimate masculine in Russian). If so, STOP and post a re-plan note before filling any Slavic locale.
6. Category B escapes: explicit evidence + external Flow task IDs in the audit.

## Investigation targets

**Required**:
- `.12` audit artifact — Slavic section
- Parity map `.9` entry — gendered-ordinal plumbing shape
- CLDR Ordinal Rules per Slavic locale (https://www.unicode.org/cldr/charts/latest/supplemental/language_plural_rules.html ordinals column)
- One authoritative published grammar per Slavic locale

## Key context

- Three-gender languages get real neuter tables — NOT Neuter→Masculine fallback.
- `.9`'s engine may be insufficient for Slavic; re-plan if so, don't silently degrade.
- Back-compat: unrelated locales unchanged.

## Acceptance

- [ ] Every Slavic locale with Category A gaps filled: YAML per-gender tables + per-gender tests.
- [ ] Three-gender Slavic locales have real neuter tables where the language distinguishes neuter.
- [ ] `LocaleTheoryMatrixCompletenessTests.AllLocaleGenderTheoryData` passes for every filled Slavic locale.
- [ ] Proposer+reviewer audit trail recorded in `.12` audit artifact for every new term.
- [ ] Re-plan note posted if `.9`'s plumbing was insufficient for some Slavic feature.
- [ ] `LocaleRegistrySweepTests` passes.
- [ ] No Slavic locale marked "deferred" without Category B evidence + external follow-up Flow task ID.

## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
