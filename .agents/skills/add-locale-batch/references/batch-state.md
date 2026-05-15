# Locale Batch State

Keep batch state small and machine-readable. Use one committed or shared plan file for intended work and one local status file for volatile run state.

## Batch Plan

Suggested path for an intentional plan:

`artifacts/locale-batches/<batch-name>.json`

`artifacts/` is normally local-only. If a plan must be reviewed in PR, place it under `docs/` and remove machine-local paths.

Example:

```json
{
  "batch": "2026-05-locale-parity",
  "repo": "Humanizr/Humanizer",
  "branchPattern": "feat-{locale}",
  "mergePolicy": "squash_when_green_clean_approved",
  "draftPrs": false,
  "locales": [
    {
      "code": "cy",
      "name": "Welsh",
      "worktree": "../Humanizer-locale-cy",
      "branch": "feat-cy",
      "pr": 1770,
      "phase": "babysit"
    }
  ],
  "heldLocales": ["eu", "so", "as", "or", "tk", "kok"]
}
```

## Live Status

Suggested local path:

`artifacts/locale-batch-status.json`

Example:

```json
{
  "cy": {
    "name": "Welsh",
    "worktree": "../Humanizer-locale-cy",
    "branch": "feat-cy",
    "pr": 1770,
    "head": "4bca9264",
    "phase": "babysit",
    "reviewDecision": "APPROVED",
    "checks": "pending",
    "unresolvedThreads": 0,
    "mergeable": "UNKNOWN",
    "merged": false,
    "mergeCommit": null,
    "worktreeCleaned": false,
    "blocker": null
  }
}
```

## Phases

- `planned` — not started.
- `worktree` — worktree/branch exists, no implementation yet.
- `implement` — locale orchestrator is building parity.
- `review-fix` — PR has actionable review feedback.
- `ci-fix` — checks failed and need diagnosis/fix.
- `babysit` — PR is open; parent or child is polling gates.
- `ready-to-merge` — zero unresolved threads, checks green, mergeable, acceptable review.
- `merged` — PR merged; cleanup still may be pending.
- `cleaned` — worktree/branches removed where safe.
- `held` — intentionally paused by user or dependency.
- `blocked` — requires user decision or shared strategy.

## Shared Change Coordination

If two or more locales need the same generator/runtime/schema change:

1. Mark affected locales `held` with the same `blocker` key.
2. Create a separate shared worktree/branch/PR.
3. Land the shared PR first.
4. Rebase held locale branches on latest main.
5. Resume locales with duplicated shared changes removed.
