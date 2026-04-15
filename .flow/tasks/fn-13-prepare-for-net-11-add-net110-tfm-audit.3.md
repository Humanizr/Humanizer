# fn-13-prepare-for-net-11-add-net110-tfm-audit.3 Re-verify fn-3 byte-parity locales on .NET 11 and refresh YAML overrides

## Description
Re-verify fn-3's cross-platform byte-parity assertions for bn, fa, he, ku, zu-ZA, ta, ar, fr-CH on .NET 11 preview. Where ICU 78 drift breaks a prior byte-identical assertion, refresh the locale's YAML `calendar:` and/or `number.formatting:` override surfaces so Humanizer output stays stable across platforms.

**Size:** M
**Files:**
- `src/Humanizer/Locales/{bn,fa,he,ku,zu-ZA,ta,ar,fr-CH}*.yml` (only those with drift)
- potentially new snapshot files under `tests/Humanizer.Tests/**`

## Approach

- Start from fn-3's merged assertion tables (see `.flow/specs/fn-3-*.md` and its tasks for the exact locales + expected strings).
- For each drift, prefer a targeted YAML override over changing the test — the policy established in fn-3 is that Humanizer output should be platform-independent.
- If ICU 78 data is *better* than what fn-3 hard-coded, update the YAML and the test together.

## Investigation targets

**Required:**
- `.flow/specs/fn-3-hard-code-locale-overrides-where-icu.md` — policy + locale list
- `src/Humanizer/Locales/` (per-locale YAML)
- `docs/locale-yaml-reference.md` — override surface reference
## Acceptance
- [ ] Each fn-3 byte-parity locale produces byte-identical output across Linux/macOS/Windows on net11.0
- [ ] Any ICU 78 drift resolved via YAML `calendar:` or `number.formatting:` overrides (not by loosening tests)
- [ ] Updated YAMLs pass existing `tests/Humanizer.Tests` on net8/net10/net11
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
