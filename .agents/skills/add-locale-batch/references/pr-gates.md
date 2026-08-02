# PR Gates and Central Polling

Poll PR state centrally so implementation agents do not spend tokens narrating pending CI.

Preferred refresh:

```bash
python3 .agents/skills/add-locale-batch/scripts/locale_batch_status.py --status artifacts/locale-batch-status.json --repo Humanizr/Humanizer --write
```

Fallback: use equivalent `gh pr view`, review-thread GraphQL count, and `gh pr checks` queries.

## Merge gate

Apply the repository-wide terminal pull request gate in `AGENTS.md`, including
its `needs-human` pause semantics.

For locale batches, the pull request body additionally records the exact-pair
locale, schema, generator, and runtime matrices with no partial or English
fallback. Central status cannot advance to `ready-to-merge` until the
repository-wide gate and locale evidence pass.

Locale batches use squash merge.

## Wake child agent only for deltas

- New unresolved review thread.
- Failed required check.
- Rebase after merged dependency.
- PR description/scope mismatch.
- Green/clean merge + cleanup authorization.

## Cleanup after merge

1. Record PR URL and merge commit.
2. Remove the locale worktree only if it has no uncommitted intentional changes.
3. Delete the safe local branch.
4. Prune/delete remote branch if GitHub did not.
5. Mark status `cleaned`.
