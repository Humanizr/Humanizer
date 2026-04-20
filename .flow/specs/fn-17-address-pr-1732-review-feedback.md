# Address PR 1732 review feedback

## Goal & Context
Re-read all inline review feedback on PR #1732, including historical and bot review threads that the watcher did not surface as new items, then address every actionable comment on the current branch.

## Architecture & Data Models
Keep changes scoped to the files touched by PR feedback. Prefer direct fixes over broad rewrites. Use GitHub review thread state as the source of truth for what remains unresolved.

## API Contracts
No public API changes unless a review comment explicitly requires one.

## Edge Cases & Constraints
Do not reply multiple times to the same non-actionable comment. Resolve actionable review threads only after a fixing commit is pushed. Preserve existing benchmark and validation behavior.

## Acceptance Criteria
- [ ] Enumerate all PR review threads/comments.
- [ ] Patch all actionable unresolved feedback.
- [ ] Reply once to non-actionable unresolved feedback if needed.
- [ ] Run targeted validation for changed files.
- [ ] Commit and push the review-feedback fixes.

## Boundaries
Do not chase unrelated CI failures unless they are caused by the review-feedback fixes.

## Decision Context
The user indicated multiple PR feedback items were missed, so review thread enumeration must happen directly through GitHub rather than relying only on watcher deltas.

## Process Note
For this PR and similar babysitting work, a watcher snapshot that reports no new review items is not enough to claim feedback is clean. Always enumerate GitHub `reviewThreads`, including unresolved outdated threads, and explicitly reconcile each unresolved item before reporting that review feedback is addressed.
