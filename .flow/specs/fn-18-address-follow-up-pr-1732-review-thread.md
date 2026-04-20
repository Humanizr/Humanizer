# Address Follow-up PR #1732 Review Thread

## Goal & Context
A fresh GitHub reviewThreads enumeration after commit 44657a04 found one additional unresolved Copilot thread on ArticlePrefixSort. The helper intended to preserve .NET Regex \w behavior omitted U+200C and U+200D join controls.

## Acceptance Criteria
- [ ] Include U+200C ZERO WIDTH NON-JOINER and U+200D ZERO WIDTH JOINER in the helper that mirrors Regex \w.
- [ ] Add a regression test proving article prefix rearrangement still occurs for those join controls.
- [ ] Re-run targeted tests, format verification, and push the branch.
- [ ] Resolve the review thread after the fix is pushed.

## Boundaries
No unrelated ArticlePrefixSort behavior changes.
