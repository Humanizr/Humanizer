# fn-2-fix-stale-locale-documentation-after.1 Fix stale locale documentation across all doc files

## Description
Fix all stale documentation that references residual locale leaves, has incomplete locale lists, or uses wrong locale codes. All changes are documentation-only — no code changes.

**Size:** M
**Files:**
- `docs/localization.md`
- `docs/locale-yaml-reference.md`
- `CLAUDE.md`

## Approach
- Rebuild Supported Languages list from `src/Humanizer/Locales/*.yml` filenames
- Remove/rewrite all "residual leaves" paragraphs to reflect current state (no residual leaves remain)
- Update locale count in CLAUDE.md

## Investigation targets
**Required** (read before editing):
- `docs/localization.md:11` — Supported Languages list (missing 16 locales, 3 wrong codes)
- `docs/localization.md:133,141,155-157` — stale residual-leaves claims (4 specific locations)
- `docs/locale-yaml-reference.md:1650` — "small number of accepted residual locale leaves"
- `CLAUDE.md:3` — "60+ locales"

**Optional** (verify consistency):
- `docs/adding-a-locale.md:292` — already correct (says no residual leaves), verify unchanged
- `docs/locale-yaml-how-to.md:341` — already correct, verify unchanged

## Key context
- All 62 locales now have all 7 canonical surfaces (verified by test run: 40,274/40,396 pass, 122 failures are OS ICU data version differences only)
- `bn-BD`, `ms-MY`, `nb-NO` in docs should be `bn`, `ms`, `nb` (matching YAML filenames)
- 16 locales completely missing from the list: af, ca, de-CH, de-LI, en, en-GB, en-IN, en-US, fil, fr-BE, fr-CH, lb, lt, nn, ta, zu-ZA
## Acceptance
- [ ] `docs/localization.md` Supported Languages list enumerates all 62 shipped locales with correct codes matching YAML filenames
- [ ] Stale residual-leaves claims removed from `docs/localization.md` at lines ~133, ~141, ~155-157
- [ ] Stale "small number of accepted residual locale leaves" removed from `docs/locale-yaml-reference.md` at line ~1650
- [ ] `CLAUDE.md` says "62 locales" instead of "60+ locales"
- [ ] Grep for `residual` across docs confirms only allowlisted conceptual mentions remain (adding-a-locale.md:281-292, locale-yaml-how-to.md:5/39/371, locale-yaml-reference.md:353/678/756)
## Done summary
Closed as done-by-proxy. All 5 fn-2 acceptance items satisfied by fn-3.5 (commit a2e86e41) and verified by fn-5.5 scan battery. See proxy-close mapping in .flow/specs/fn-2-fix-stale-locale-documentation-after.md for per-item artifact citations.
## Evidence
- Commits: a2e86e41
- Tests: grep verification of residual mentions, fn-5.5 scan 2b (residual conceptual-language scan)
- PRs: