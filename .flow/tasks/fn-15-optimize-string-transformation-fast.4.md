## Description

Optimize inflector case transformation methods in `src/Humanizer/InflectorExtensions.cs`: `Pascalize`, `Camelize`, `Underscore`, and `Kebaberize`.

Introduce shared internal ASCII scanning helpers where doing so removes real duplication without obscuring behavior. Avoid chaining transformations when a direct single-allocation write can produce the final output.

## Acceptance Criteria

- Depends on the Rune/downlevel decision gate task; implementation must not start before that task is complete.
- Benchmarks for `Pascalize`, `Camelize`, `Underscore`, and `Kebaberize` meet the epic's objective fast-path thresholds or document why a target was rejected.
- Tests preserve behavior for leading/trailing/consecutive separators, acronym boundaries, lower/digit-to-upper transitions, dash/space/underscore mapping, and current branch behavior that preserves non-letter characters captured after separators.
- Turkish `UseCulture("tr-TR")` tests cover `Pascalize`, `Camelize`, `Underscore`, and `Kebaberize`, or the task documents and proves current culture-invariance for a specific method.
- `Camelize` does not allocate a full PascalCase intermediate solely to lowercase the first character on fast-path inputs.
- `Kebaberize` writes hyphenated output directly rather than relying on `Underscore().Dasherize()` for fast-path inputs.
- `Underscore`/`Kebaberize` preserve Unicode lowercasing through fallback or culture-aware casing.
- Every `string.Create` or span-write path is guarded or backed by a downlevel helper so `net48` and `netstandard2.0` compile under `dotnet pack`.
- No new `System.Text.Rune`, Polyfill Rune-specific API, or package reference appears in the diff.

## Done summary
Implemented ASCII fast paths for Pascalize, Camelize, Underscore, and Kebaberize.
## Evidence
- Commits:
- Tests:
- PRs: