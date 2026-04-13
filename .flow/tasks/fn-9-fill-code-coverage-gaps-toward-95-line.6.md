## Description
Close formatter branch gaps: `LocalePhraseTable` (70%), `DelimitedCollectionFormatter` (80.9%, six generic `Humanize<T>` overloads), `CliticCollectionFormatter` (82.3%).

**Size:** M
**Files:**
- `tests/Humanizer.Tests/Localisation/Formatters/LocalePhraseTableTests.cs` (new)
- `tests/Humanizer.Tests/Localisation/CollectionFormatters/DelimitedCollectionFormatterCoverageTests.cs` (new or extend `tests/Humanizer.Tests/CollectionHumanizeTests.cs`)
- `tests/Humanizer.Tests/Localisation/CollectionFormatters/CliticCollectionFormatterCoverageTests.cs` (new or extend `tests/Humanizer.Tests/CollectionHumanizeTests.cs`)

## Approach
- **LocalePhraseTable** (`src/Humanizer/Localisation/Formatters/LocalePhraseTable.cs:56-146`). Four parallel `TryGet*` pairs (datePhrase/timeSpan/dataUnit/timeUnit). Cover (a) phrase found → `true`, out populated; (b) phrase not found → `false`, out defaulted. `LocalePhraseTableCatalog.Resolve` (`:148-169`) tail branch `ResolveCore("en")` via a culture walking past invariant.
- **DelimitedCollectionFormatter** (`src/Humanizer/Localisation/CollectionFormatters/DelimitedCollectionFormatter.cs`). Six public generic `Humanize<T>` overloads at `:8, :11, :14, :17, :20, :23`. Cover: null collection + null objectFormatter `ArgumentNullException` with `ParamName`; whitespace items skipped (`:40-43`); empty / single / multi-item; default vs custom separator; both `Func<T, string?>` and `Func<T, object?>` variants.
- **CliticCollectionFormatter** (`src/Humanizer/Localisation/CollectionFormatters/CliticCollectionFormatter.cs:22-73`). `count switch { 0 => "", 1 => lastItem!, _ => ... }` — all three arms; null guards.

## Investigation targets
**Required:**
- `src/Humanizer/Localisation/Formatters/LocalePhraseTable.cs:56-169`
- `src/Humanizer/Localisation/CollectionFormatters/DelimitedCollectionFormatter.cs:1-80`
- `src/Humanizer/Localisation/CollectionFormatters/CliticCollectionFormatter.cs:1-80`
- `tests/Humanizer.Tests/CollectionHumanizeTests.cs` — existing shapes
- `artifacts/fn-9-local-coverage/uncovered.json`

## Acceptance
- [ ] `LocalePhraseTable` reaches ≥93% line / ≥88% branch; both directions of all four `TryGet*` pairs covered.
- [ ] `DelimitedCollectionFormatter` reaches ≥93% line / ≥88% branch; all six generic `Humanize<T>` overloads covered.
- [ ] `CliticCollectionFormatter` reaches ≥93% line / ≥88% branch; all three switch arms covered.
- [ ] `ArgumentNullException` assertions check `ParamName`.

## Done summary
_To be filled on completion._

## Evidence
- Commits:
- Tests:
- PRs:
