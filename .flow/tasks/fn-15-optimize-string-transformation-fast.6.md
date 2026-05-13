## Description

Complete the Rune/downlevel decision gate before implementation tasks begin. Optionally add supplementary-plane behavior tests that document current behavior, but do not expand this task into a broad Unicode rewrite.

The current plan is not to use Rune for hot ASCII fast paths. The referenced Polyfill Rune commit only adds extension methods for `NETCOREAPP3_0_OR_GREATER` targets that already have `System.Text.Rune`; it does not provide Rune for Humanizer's `net48` or `netstandard2.0` targets.

## Acceptance Criteria

- The epic/task evidence records the final decision: no Rune-led implementation for this epic, no downlevel Rune dependency, and regex/culture fallback for unsupported Unicode/culture cases.
- Implementation tasks 2-5 depend on this task.
- A guardrail is documented for implementers: final diff must contain no new `System.Text.Rune`, Polyfill Rune-specific API, or package reference unless a separate approved task is created first.
- If supplementary-plane behavior tests are added, they document current behavior without forcing a broad Unicode rewrite.
- The task identifies the downlevel allocation strategy: guarded `string.Create` on supported TFMs plus existing or new narrow compat helpers for `net48`/`netstandard2.0`.

## Done summary
Resolved Rune decision: do not introduce Rune fast paths for this change.
## Evidence
- Commits:
- Tests:
- PRs: