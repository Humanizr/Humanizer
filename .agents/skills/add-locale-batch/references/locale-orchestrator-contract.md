# Locale Orchestrator Contract

A locale orchestrator owns one locale worktree, branch, and PR. It implements or finishes locale parity with `$add-locale` and reports compact status to the parent.

## Startup

1. Confirm CWD is the assigned worktree and HEAD is the assigned branch.
2. Read repo instructions and `$add-locale`.
3. Read `.agents/skills/add-locale/references/goal-template.md`.
4. Set a locale-specific goal using the assigned culture code/name and worktree/PR context.

## Scope

Allowed:

- Assigned locale YAML/tests/docs/matrix entries.
- Assigned PR description, pushes, review-thread resolution, CI diagnosis, merge cleanup.
- Shared generator/runtime/schema edits only after parent approval or explicit assignment.

Forbidden:

- Editing main or sibling worktrees.
- Draft PRs unless policy changes.
- Committed full local paths.
- Treating routine review comments or code-related CI failures as blockers without diagnosis/fix.

## Reporting

Report one `LocaleStatus` entry as defined in `batch-state.md`. Keep output JSON-first; include failed logs only, not passing test output.

## Stop and report

Stop for parent coordination when:

- Work needs a shared generator/runtime/schema strategy also needed by another active locale.
- Rebase/conflict resolution risks discarding another locale/shared change.
- GitHub permissions block push, PR creation, thread resolution, or merge.
- CI is repeatedly infrastructure-failing after diagnosis/retry.
- User policy changes.

Do not stop merely because review comments or branch-caused CI failures exist; address them.
