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
- `docs/localization.md:133-157` — stale residual-leaves claims
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
- [ ] All stale "residual clock leaves" / "handwritten residual leaves" text removed from `docs/localization.md`
- [ ] Stale "small number of accepted residual locale leaves" removed from `docs/locale-yaml-reference.md`
- [ ] `CLAUDE.md` says "62 locales" instead of "60+ locales"
- [ ] Grep confirms no remaining stale residual-leaves claims across all docs
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
