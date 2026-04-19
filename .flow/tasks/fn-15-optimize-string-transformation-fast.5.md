## Description

Replace the English article prefix regex in `src/Humanizer/ArticlePrefixSort.cs` with a span-based parser for the exact small grammar it recognizes today.

The parser must preserve current `^((The)|(the)|(a)|(A)|(An)|(an))\s\w+` behavior, including Unicode `\w` and `\s` semantics, unless a separate behavior-change decision is made and documented.

## Acceptance Criteria

- Depends on the Rune/downlevel decision gate task; implementation must not start before that task is complete.
- `EnglishArticleBenchmarks.AppendArticlePrefix` improves or remains neutral and avoids regex work on covered fast-path inputs.
- Tests cover positive prefixes, non-article lookalikes, punctuation-only tails, non-ASCII word tails (`The Éclair`, `An Æon`), `_` and digit word tails, trailing punctuation after a valid word (`The Theater!`), multiple spaces, and non-space whitespace.
- Non-space whitespace behavior is explicitly preserved or explicitly documented as a deliberate behavior change before implementation.
- `PrependArticleSuffix` behavior remains unchanged.
- Sort order continues to use `Array.Sort(transformed)` unchanged.
- Every `string.Create` or span-write path is guarded or backed by a downlevel helper so `net48` and `netstandard2.0` compile under `dotnet pack`.
- No new `System.Text.Rune`, Polyfill Rune-specific API, or package reference appears in the diff.

## Done summary
Replaced English article-prefix regex with a span parser and added edge coverage.
## Evidence
- Commits:
- Tests:
- PRs: