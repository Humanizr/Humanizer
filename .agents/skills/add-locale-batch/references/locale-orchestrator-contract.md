# Locale Orchestrator Contract

A locale orchestrator owns exactly one locale worktree and branch. It implements or finishes locale parity using `$add-locale` and reports compact status to the parent.

## Required Startup

1. Confirm current directory is the assigned worktree.
2. Confirm current branch is the assigned branch.
3. Read repo instructions and `$add-locale`.
4. Read `.agents/skills/add-locale/references/goal-template.md`.
5. Set a locale-specific goal from the template using the assigned culture code/name and worktree/PR context.

## Scope Boundary

Allowed:

- Assigned locale YAML/tests/docs/matrix entries.
- Shared generator/runtime/schema changes only when explicitly assigned by the parent or when the agent stops and gets approval for a shared strategy.
- PR description updates, review-thread resolution, CI diagnosis, and branch pushes for its assigned PR.

Forbidden:

- Editing the main checkout.
- Editing sibling worktrees.
- Creating draft PRs unless policy changes.
- Committing artifacts with full local filesystem paths.
- Declaring review comments or routine CI failures as blockers without an attempted diagnosis/fix.

## Required JSON Status

Report this shape on progress and final output:

```json
{
  "locale": "cy",
  "name": "Welsh",
  "worktree": "../Humanizer-locale-cy",
  "branch": "feat-cy",
  "pr": 1770,
  "head": "4bca9264",
  "phase": "babysit",
  "validation": {
    "sourceGeneratorsNet10": "pass",
    "humanizerNet8": "pass",
    "humanizerNet10": "pass",
    "humanizerNet11": "pass",
    "pack": "pass",
    "format": "pass"
  },
  "review": {
    "decision": "APPROVED",
    "unresolvedThreads": 0
  },
  "checks": "pending",
  "mergeable": "UNKNOWN",
  "merged": false,
  "mergeCommit": null,
  "worktreeCleaned": false,
  "blocker": null
}
```

Use `"not-run"`, `"pass"`, `"fail"`, or `"not-applicable"` for validation entries. Include detailed logs only for failures.

## Stop Conditions

Stop and report to the parent when:

- The work needs a shared generator/runtime/schema strategy also needed by another active locale.
- A rebase or merge conflict risks discarding another locale/shared change.
- GitHub permissions prevent push, review-thread resolution, PR creation, or merge.
- CI fails repeatedly for infrastructure reasons after reasonable retry/diagnosis.
- The user policy changes.

Do not stop merely because there are review comments or code-related CI failures; address them.
