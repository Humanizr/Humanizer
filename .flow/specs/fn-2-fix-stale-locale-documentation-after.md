# Fix stale locale documentation after parity completion

## Overview

With the completion of fn-1-locale-translation-parity-across-all (all 62 locales now have all 7 canonical surfaces), several documentation files contain stale claims, incomplete locale lists, and wrong locale codes. This epic fixes all documentation inconsistencies.

## Scope

Four files need updates:
1. `docs/localization.md` — Supported Languages list + stale residual-leaves claims
2. `docs/locale-yaml-reference.md` — inconsistent residual-leaves language
3. `CLAUDE.md` — locale count

## Quick commands
```bash
# Verify docs build / lint after changes
dotnet format Humanizer.slnx --verify-no-changes
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
```

## Acceptance
- [ ] `docs/localization.md` Supported Languages list enumerates all 62 shipped locales with correct locale codes (matching YAML filenames)
- [ ] `docs/localization.md` removes all stale references to "residual clock leaves" or "handwritten residual leaves" (lines ~133-157)
- [ ] `docs/locale-yaml-reference.md` removes stale "small number of accepted residual locale leaves" language (~line 1650)
- [ ] `CLAUDE.md` says "62 locales" instead of "60+ locales"
- [ ] No other documentation files contain stale residual-leaves claims

## Early proof point
Task fn-2-fix-stale-locale-documentation-after.1 fixes the highest-impact issue (supported languages list). If it surfaces unexpected naming conventions or locale aliases, re-evaluate the locale code mapping before continuing.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | Supported Languages list complete and correct | fn-2-fix-stale-locale-documentation-after.1 | — |
| R2  | Remove stale residual-leaves claims from localization.md | fn-2-fix-stale-locale-documentation-after.1 | — |
| R3  | Remove stale residual-leaves language from locale-yaml-reference.md | fn-2-fix-stale-locale-documentation-after.1 | — |
| R4  | CLAUDE.md locale count updated | fn-2-fix-stale-locale-documentation-after.1 | — |
| R5  | No other stale residual-leaves claims | fn-2-fix-stale-locale-documentation-after.1 | — |
