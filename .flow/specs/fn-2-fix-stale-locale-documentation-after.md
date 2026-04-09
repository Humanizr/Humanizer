# Fix stale locale documentation after parity completion

> **SUPERSEDED by fn-3-hard-code-locale-overrides-where-icu** — all three original documentation fixes listed below are folded into `fn-3-hard-code-locale-overrides-where-icu.5` (Document override pattern and update contributor checklist) alongside new schema documentation for the `calendar:` surface and `number.formatting:` sub-block. This epic is retained for history but should not be worked directly. Task fn-2.1 is blocked; close this epic as done-by-proxy after fn-3.5 completes.

## Overview

With the completion of fn-1-locale-translation-parity-across-all (all 62 locales now have all 7 canonical surfaces), several documentation files contain stale claims, incomplete locale lists, and wrong locale codes. This epic fixes all documentation inconsistencies.

## Scope

Three files need stale-doc fixes (all folded into fn-3.5):
1. `docs/localization.md` — Supported Languages list + stale residual-leaves claims
2. `docs/locale-yaml-reference.md` — inconsistent residual-leaves language
3. `CLAUDE.md` — locale count

fn-3.5 also adds additional new work beyond fn-2's scope: documentation for the new `calendar:` and `number.formatting:` schema extensions, canonical surface inventory updates across all doc files, contributor checklist updates, and preflight questions.

## Quick commands
```bash
# Verify docs build / lint after changes
dotnet format Humanizer.slnx --verify-no-changes
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
```

## Acceptance

### Proxy-close checklist (all must be verified in fn-3.5 before closing fn-2)
Each item below cites the exact fn-3.5 acceptance text it maps to:
- [ ] `docs/localization.md` Supported Languages list enumerates all 62 shipped locales with correct codes — maps to fn-3.5 "fn-2 rollup" acceptance: "`docs/localization.md` Supported Languages list enumerates all 62 shipped locales with correct codes"
- [ ] `docs/localization.md` stale residual-leaves claims removed from lines ~133, ~141, ~155-157 — maps to fn-3.5 "fn-2 rollup" acceptance: "`docs/localization.md` stale residual-leaves claims removed from lines ~133, ~141, ~155-157"
- [ ] `docs/locale-yaml-reference.md` stale "residual locale leaves" language removed at line ~1650 — maps to fn-3.5 "fn-2 rollup" acceptance: "`docs/locale-yaml-reference.md` stale 'residual locale leaves' language removed at line ~1650"
- [ ] `CLAUDE.md` says "62 locales" instead of "60+ locales" — maps to fn-3.5 "fn-2 rollup" acceptance: "`CLAUDE.md` says '62 locales' instead of '60+ locales'"
- [ ] Grep for `residual` across docs confirms only allowlisted conceptual mentions remain — maps to fn-3.5 "Quality" acceptance: "Grep for `residual` across docs confirms only the allowlisted conceptual mentions remain"
