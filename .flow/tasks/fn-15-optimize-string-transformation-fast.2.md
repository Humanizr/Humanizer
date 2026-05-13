## Description

Implement a deeper ASCII fast path for `ToTitleCase.Transform(...)` in `src/Humanizer/Transformer/ToTitleCase.cs`, building on the current branch's partial allocation reductions.

The fast path should avoid regex match enumeration and full `StringBuilder` copy for common ASCII sentence-like inputs. It must fallback for non-ASCII, unsupported apostrophe/word shapes, or any culture/input shape whose behavior is not proven equivalent.

## Acceptance Criteria

- Depends on the Rune/downlevel decision gate task; implementation must not start before that task is complete.
- ASCII title-case benchmarks improve with objective evidence: if baseline allocation is above 500 B, allocated bytes drop by at least 25%; otherwise allocations do not increase and mean improves by at least 10% or the task documents why the target was rejected.
- Fallback/non-ASCII title-case benchmarks do not regress mean or allocation by more than 10% outside benchmark noise.
- Existing and new title-case tests pass for minor words, all-caps acronym words, apostrophes, Turkish/Azeri culture-sensitive casing, and Unicode fallback inputs.
- Implementation does not use arithmetic/invariant ASCII casing for public output unless tests prove exact equivalence.
- Every `string.Create` or span-write path is guarded or backed by a downlevel helper so `net48` and `netstandard2.0` compile under `dotnet pack`.
- No new `System.Text.Rune`, Polyfill Rune-specific API, or package reference appears in the diff.

## Done summary
Implemented ASCII fast path for ToTitleCase while preserving regex fallback for non-ASCII.
## Evidence
- Commits:
- Tests:
- PRs: