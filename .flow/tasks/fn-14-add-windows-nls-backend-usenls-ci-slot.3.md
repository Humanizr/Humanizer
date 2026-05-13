# fn-14-add-windows-nls-backend-usenls-ci-slot.3 Sweep for other NLS-only scenarios and add coverage

## Description
Identify other Humanizer test paths where ICU and NLS diverge — especially in Arabic-script locales, Serbian calendar tests (fn-6), and anywhere `OptionalCalendars` or culture-bound calendar dispatch matters. Add NLS-specific assertions or lift existing skips.

**Size:** M
**Files:** TBD via audit

## Approach

- Diff suite output between the ICU slot and the new NLS slot from fn-14.1.
- Focus on: `tests/Humanizer.Tests/Localisation/sr*` (fn-6 Serbian calendar), `tests/Humanizer.Tests/Localisation/ar*`, `tests/Humanizer.Tests/Localisation/fa*`, `tests/Humanizer.Tests/Localisation/he*`, and anywhere `Assert.Skip` mentions ICU or NLS.
- For every NLS-only behavior we want to guarantee, add an explicit assertion (not just skip-on-ICU).

## Investigation targets

**Required:**
- `.flow/specs/fn-6-fix-45-failing-net48-locale-tests.md` — prior NLS vs ICU analysis
- `tests/Humanizer.Tests/Localisation/LocaleFormatterExactTheoryData.cs:10-23` — NLS vs ICU conditionals
- grep for `Assert.Skip` and `PlatformDetection` equivalents in tests
## Acceptance
- [ ] Every NLS-vs-ICU divergence surfaced by fn-14.1 is categorized (intentional / regression)
- [ ] Intentional divergences have explicit tests on the NLS slot
- [ ] Regressions filed as follow-ups or resolved here
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
