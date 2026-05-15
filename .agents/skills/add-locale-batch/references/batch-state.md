# Locale Batch State

Use small machine-readable state so orchestration can resume without chat history. Keep volatile state in `artifacts/`; shared/committed state must omit machine-local paths.

## Batch plan

Suggested local path: `artifacts/locale-batches/<batch>.json`.

Required fields:

- `batch`, `repo`, `branchPattern`, `mergePolicy`, `draftPrs`
- `locales[]`: `code`, `name`, `worktree`, `branch`, `pr`, `phase`
- `heldLocales[]` when the user pauses future work

## LocaleStatus

Suggested local path: `artifacts/locale-batch-status.json`. The object is keyed by locale code. Each entry should use these fields when known:

- identity: `name`, `worktree`, `branch`, `pr`, `prUrl`, `head`
- workflow: `phase`, `blocker`
- review/CI: `reviewDecision`, `unresolvedThreads`, `checks`, `mergeable`, `mergeStateStatus`, `isDraft`
- completion: `merged`, `mergedAt`, `mergeCommit`, `worktreeCleaned`
- validation: compact map of command names to `not-run`, `pass`, `fail`, or `not-applicable`

Agents should report one `LocaleStatus` entry. Include detailed logs only for failures.

## Phases

`planned`, `worktree`, `implement`, `review-fix`, `ci-fix`, `babysit`, `ready-to-merge`, `merged`, `cleaned`, `held`, `blocked`.

## Shared changes

When two or more locales need the same generator/runtime/schema change:

1. Mark affected locales `held` with the same `blocker`.
2. Create and land one shared worktree/branch/PR.
3. Rebase held locale branches on latest main.
4. Resume locales with duplicated shared edits removed.
