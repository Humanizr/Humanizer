# fn-14-add-windows-nls-backend-usenls-ci-slot.4 Document NLS vs ICU CI matrix in CLAUDE.md + local repro

## Description
Document the NLS vs ICU CI matrix in CLAUDE.md and explain how a contributor reproduces the NLS slot locally.

**Size:** S
**Files:**
- `CLAUDE.md`

## Approach

Additive section near the existing "Testing" block: one paragraph on backends, one command block showing `DOTNET_SYSTEM_GLOBALIZATION_USENLS=1 dotnet test ... -f net10.0`.
## Acceptance
- [ ] CLAUDE.md explains NLS vs ICU difference in one paragraph
- [ ] CLAUDE.md shows the exact command to reproduce the NLS slot locally (Windows-only)
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
