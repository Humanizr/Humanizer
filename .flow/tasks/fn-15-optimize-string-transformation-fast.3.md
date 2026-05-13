## Description

Optimize `StringHumanizeExtensions.Humanize()` common paths in `src/Humanizer/StringHumanizeExtensions.cs`.

Replace regex/LINQ-heavy PascalCase and freestanding separator detection with ASCII span scanners where behavior is straightforward. Keep the existing regex implementation available as the fallback for non-ASCII, punctuation-heavy, culture-sensitive, or ambiguous inputs.

## Acceptance Criteria

- Depends on the Rune/downlevel decision gate task; implementation must not start before that task is complete.
- `Humanize PascalCase`, mixed acronym, underscore, dash, and freestanding separator benchmarks improve under the epic's objective benchmark thresholds.
- Existing behavior for all-uppercase acronym inputs, `I`, punctuation omission, underscores/dashes, other-letter Unicode tests, and Turkish `UseCulture("tr-TR")` cases is preserved.
- Fast paths use culture-aware casing or fallback; no arithmetic/invariant ASCII casing is used for public output unless tests prove exact equivalence.
- Fallback boundaries are documented in code comments only where needed for maintainability.
- Every `string.Create` or span-write path is guarded or backed by a downlevel helper so `net48` and `netstandard2.0` compile under `dotnet pack`.
- No new `System.Text.Rune`, Polyfill Rune-specific API, or package reference appears in the diff.

## Done summary
Implemented ASCII PascalCase Humanize fast path and manual freestanding separator scan.
## Evidence
- Commits:
- Tests:
- PRs: