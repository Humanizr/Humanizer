# fn-11-fix-urdu-locale-ci-pr-feedback-rebase.2 PR feedback: remove .claude lock file + Icelandic namespace

## Description
Address two small Copilot review comments on PR #1720: remove the accidentally committed `.claude/scheduled_tasks.lock` (plus add a gitignore rule) and rename the namespace in `IcelandicGenderedOrdinalTests.cs` to follow the folder-code convention.

**Size:** S
**Files:**
- `.claude/scheduled_tasks.lock` (delete)
- `.gitignore` (add rule)
- `tests/Humanizer.Tests/Localisation/is/IcelandicGenderedOrdinalTests.cs`

## Approach
1. `git rm .claude/scheduled_tasks.lock`.
2. Add an ignore rule to the repo `.gitignore` that covers `.claude/scheduled_tasks.lock` (and the broader `.claude/*.lock` pattern) so this file cannot be reintroduced. Keep it minimal — one pattern is enough.
3. Change `namespace Humanizer.Tests.Localisation.Icelandic;` to `namespace Humanizer.Tests.Localisation.is;` at `tests/Humanizer.Tests/Localisation/is/IcelandicGenderedOrdinalTests.cs:3` to match sibling files (e.g. `da`, `nb`, `ur`).

Note: `is` is a C# contextual keyword, so the namespace token may need `@is` in source. Grep other locale tests that use reserved-word codes (e.g. `is`) in this repo to see the established form — if other `Localisation.is` namespaces already exist elsewhere, match their exact spelling.

## Investigation targets
**Required:**
- `tests/Humanizer.Tests/Localisation/is/IcelandicGenderedOrdinalTests.cs:3` — current namespace.
- One sibling per-locale test (e.g. `tests/Humanizer.Tests/Localisation/da/DanishGenderedOrdinalTests.cs`) — confirm namespace form.
- `.gitignore` — current structure and where `.claude/` patterns would fit.

**Optional:**
- Other files under `tests/Humanizer.Tests/Localisation/is/` — check if any already use `Localisation.@is` or a different variant so the change is consistent.

## Key context
- This task depends on fn-11.1 completing so the rebase + test fix land first.
- PR reviewer comment (Copilot) explicitly flagged `.claude/scheduled_tasks.lock` as "machine/session-specific … shouldn't be committed" and requested a gitignore rule.
- Namespace convention: test folder name is the cultural code (`is`) and namespace matches the folder; deviating to `Icelandic` breaks discovery/grep.
## Acceptance
- [ ] `.claude/scheduled_tasks.lock` removed from git tracking.
- [ ] `.gitignore` prevents reintroduction.
- [ ] `IcelandicGenderedOrdinalTests.cs` namespace matches folder-code convention used by sibling tests.
- [ ] Build + format + affected tests pass.
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
