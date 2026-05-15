# PR Gates and Central Polling

The parent orchestrator should poll PR state centrally whenever possible. This keeps implementation agents from spending tokens narrating pending CI.

## Required Merge Gate

A locale PR may be squash-merged only when all are true:

- PR is open and not draft.
- Review state is approved or otherwise acceptable under repo policy.
- There are zero unresolved, non-outdated review threads.
- Required checks are passing.
- PR is mergeable/clean.
- The current branch head is the validated head.
- Babysit has found no fresh review comments or failing checks.

## Compact `gh` Checks

Use compact queries instead of verbose logs:

```bash
gh pr view <pr> --repo Humanizr/Humanizer \
  --json number,url,state,isDraft,headRefName,headRefOid,reviewDecision,mergeStateStatus,mergeable,mergedAt
```

Unresolved non-outdated threads:

```bash
gh api graphql \
  -f owner=Humanizr -f name=Humanizer -F number=<pr> \
  -f query='query($owner:String!, $name:String!, $number:Int!) { repository(owner:$owner, name:$name) { pullRequest(number:$number) { reviewThreads(first:100) { nodes { isResolved isOutdated } } } } }' \
  --jq '[.data.repository.pullRequest.reviewThreads.nodes[] | select(.isResolved == false and .isOutdated == false)] | length'
```

Checks:

```bash
gh pr checks <pr> --repo Humanizr/Humanizer --watch=false
```

## When to Wake a Locale Agent

Wake or steer the locale orchestrator only for actionable deltas:

- New unresolved review thread.
- Failed required check.
- Rebase needed after a merged dependency.
- PR description/scope mismatch.
- Merge and cleanup authorization after gates are green.

## Cleanup After Merge

After squash merge:

1. Record PR URL and merge commit.
2. Remove the locale worktree.
3. Delete the local branch if it is merged or no longer needed.
4. Prune/delete remote branch when GitHub did not already delete it.
5. Mark status `cleaned`.

Never remove a worktree that still has uncommitted intentional changes.
