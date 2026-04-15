# fn-13-prepare-for-net-11-add-net110-tfm-audit.6 Update CLAUDE.md + docs/ with net11.0 quick commands and matrix notes

## Description
Update repo docs to reflect the net11.0 TFM. Keep it tight — Quick Commands section + any locale authoring docs that enumerate TFMs.

**Size:** S
**Files:**
- `CLAUDE.md` (Quick Commands — add net11.0 to the test invocations)
- `docs/adding-a-locale.md` (if it references TFMs)
- `docs/locale-yaml-reference.md` (if it references TFMs)

## Approach

- Only add net11.0 where net10.0 already appears.
- Do not rewrite existing sections for flow — this is additive.
## Acceptance
- [ ] CLAUDE.md Quick Commands section lists net11.0 test invocations alongside net10.0 / net8.0 / net48
- [ ] Any docs/*.md page that enumerates TFMs includes net11.0
- [ ] No cosmetic rewrites beyond the net11.0 addition
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
