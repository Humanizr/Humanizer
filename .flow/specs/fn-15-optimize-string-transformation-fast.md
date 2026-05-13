# Optimize String Transformation Fast Paths

## Goal & Context

Improve the next set of Humanizer performance hotspots after PR #1732 by replacing regex/LINQ/multi-pass string shaping in common ASCII-heavy string APIs with allocation-conscious span scanners while preserving existing Unicode, culture, and target-framework behavior through explicit fallback paths.

This epic intentionally builds on the current branch `codex/performance-hotspots-net11`. PR #1732 already improves several non-string hotspots and partially improves `ToTitleCase`; this plan targets follow-on string transformation work and must keep PR hygiene clear by separating pre-existing branch changes from changes made for this epic.

Primary target surfaces:

- `StringHumanizeExtensions.Humanize()` and `Humanize(LetterCasing)` in `src/Humanizer/StringHumanizeExtensions.cs`
- `ToTitleCase.Transform(...)` in `src/Humanizer/Transformer/ToTitleCase.cs`
- Inflector case transforms in `src/Humanizer/InflectorExtensions.cs`: `Pascalize`, `Camelize`, `Underscore`, `Kebaberize`, and indirectly `Titleize`
- English article sort helpers in `src/Humanizer/ArticlePrefixSort.cs`
- Benchmark coverage in `src/Benchmarks`
- Regression coverage in `tests/Humanizer.Tests`

The current local benchmark artifacts indicate the largest remaining measured string hotspot is title casing at longer input sizes. Representative local artifacts show `TransformersBenchmarks.TitleCase` at roughly `24.5 us / 73 KB` for 1000 chars and `AllTransforms` at roughly `23.7 us / 77 KB` for 1000 chars. Exact numbers must be refreshed in task 1 because artifacts and branch contents can drift.

## Required Sequencing

1. Establish benchmark and correctness baseline.
2. Decide and document the Rune/downlevel policy before implementation tasks begin.
3. Implement each fast path behind tests and objective benchmark criteria.
4. Run final validation and PR hygiene checks.

Tasks 2-5 depend on task 6 as well as task 1 so implementation cannot start before the Rune/downlevel decision is made explicit.

## Rune / Polyfill Decision Gate

Do not lead with `System.Text.Rune` for performance. The .NET docs describe `Rune` as useful when code explicitly handles Unicode scalar values or surrogate pairs, and also state it is unnecessary for exact `char` matches or known `char` delimiters. These target methods are hot mostly because of regex, match allocation, LINQ, `StringBuilder` copies, and repeated whole-string transforms. For ASCII identifier inputs, Rune decoding would usually add work rather than remove it.

Implementation policy:

- Use ASCII/span scanners as the primary fast path.
- Preserve existing regex/culture paths as fallback when input contains non-ASCII, punctuation edge cases, apostrophe forms, culture-sensitive casing, or behavior that is not yet proven equivalent.
- Add Rune only if a separate approved task explicitly improves supplementary-plane Unicode correctness.
- Do not add a dependency or source import solely to make Rune available on `net48` or `netstandard2.0` for these perf fast paths.
- The final diff for this epic must contain no new `System.Text.Rune` usage, no new Polyfill Rune-specific import, and no new package/reference change for Rune unless a separate approved task is created first.

Polyfill note: the referenced SimonCropp/Polyfill commit adds extension methods such as `string.Contains(Rune)` and `StringBuilder.Append(Rune)`, but the raw source is guarded with `#if !NET11_0_OR_GREATER && NETCOREAPP3_0_OR_GREATER` and uses `System.Text.Rune`. That means it relies on the platform already providing `System.Text.Rune`; it does not solve Humanizer's `net48` or `netstandard2.0` Rune availability. If downlevel Rune support is ever desired, that is a separate dependency/design decision, not a prerequisite for this string performance epic.

Relevant external references checked during planning:

- Polyfill commit: https://github.com/SimonCropp/Polyfill/commit/b710fcacfc84276688190f46c13f55bbf253f15f
- Polyfill raw `Polyfill_String_Rune.cs`: https://raw.githubusercontent.com/SimonCropp/Polyfill/b710fcacfc84276688190f46c13f55bbf253f15f/src/Polyfill/Polyfill_String_Rune.cs
- Microsoft Rune docs: https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-text-rune

## Culture And Casing Rules

ASCII input does not imply ASCII output. Turkish and Azeri casing can map ASCII `i`/`I` to non-ASCII `İ`/`ı`, and Humanizer exposes culture-aware casing through `TextInfo` and `UseCulture` tests.

Fast-path rules:

- Do not use arithmetic ASCII casing such as `c | 0x20`, `c - 32`, or invariant-only assumptions in public string transforms unless tests prove equivalence for that exact path.
- Prefer `culture.TextInfo.ToUpper(char)` / `ToLower(char)` or an existing culture-aware path for casing changes.
- If a proposed fast path cannot handle a culture where ASCII casing is non-invariant, fallback to the existing implementation for that culture.
- Required tests must cover `UseCulture("tr-TR")` for `Humanize`, `Humanize(LetterCasing.Title)`, `ToTitleCase`, `Pascalize`, `Camelize`, `Underscore`, and `Kebaberize`, or document why a method is culture-invariant today and prove unchanged behavior.

## Downlevel Allocation Strategy

Humanizer targets `net11.0`, `net10.0`, `net8.0`, `net48`, and `netstandard2.0`. Any single-allocation write path must account for all targets.

Rules:

- Use `string.Create` only behind target-framework guards where it is available.
- For downlevel targets, use existing compat helpers or add narrow internal helpers that preserve current compile support.
- Every implementation task that introduces `string.Create`, span writes, or stack buffers must include `dotnet pack src/Humanizer/Humanizer.csproj -c Release -o <temp-output>` evidence proving all library TFMs compile.

## Architecture & Implementation Strategy

### 1. Benchmark First

Before changing implementation, add or refresh deterministic benchmarks that isolate intended fast paths and fallback paths separately. Random strings are acceptable only when seeded and separated from targeted ASCII scanner cases.

Benchmark dimensions should include:

- `Humanize()` fast path and fallback cases.
- `ToTitleCase` fast path and fallback cases.
- Inflector transforms fast path and fallback cases.
- English article sort positive and negative cases.

Benchmark acceptance thresholds:

- For a task's primary ASCII fast-path benchmark with baseline allocations above 500 B, allocated bytes should drop by at least 25% or the task must document why the validated behavior constraints make that target inappropriate.
- For primary ASCII fast-path benchmarks below 500 B baseline allocation, allocation count or bytes should not increase, and mean should improve by at least 10% or be explicitly justified.
- Fallback/non-ASCII benchmarks must not regress mean or allocated bytes by more than 10% outside benchmark noise; any apparent regression requires re-run or explanation.
- Benchmarks must report both mean and allocation deltas in completion evidence.

### 2. Add Golden Behavior Tests Before Rewrites

Before replacing regex logic, add focused tests for the exact behavior being preserved. Do not rely only on existing examples.

Required test categories:

- Existing ASCII outputs for `Humanize`, `Titleize`, `Pascalize`, `Camelize`, `Underscore`, `Kebaberize`.
- Acronym behavior, especially all-uppercase whole input and uppercase runs inside mixed identifiers.
- Punctuation, digit, symbol, leading/trailing/consecutive separator behavior.
- Non-ASCII behavior, including existing `CanHumanizeOtherUnicodeLetter` style cases.
- Culture-sensitive casing, including Turkish tests described above.
- Article prefix/suffix sorting behavior for `The`, `the`, `A`, `a`, `An`, `an`, plus Unicode word tails, `_`, digit tails, trailing punctuation, multiple spaces, and non-space whitespace.

### 3. `ToTitleCase` Follow-On Fast Path

Current branch already removed the frozen minor-word set and avoids some match string allocations. The next step is a deeper ASCII fast path that avoids regex match enumeration and full `StringBuilder` copy for common ASCII sentence-like inputs.

Proposed design:

- Add an internal scanner that detects word spans using ASCII rules compatible with the current regex for ASCII inputs: letters/digits/underscore plus optional apostrophe continuation where current behavior supports it.
- If the input contains non-ASCII, fallback to the current regex/culture implementation unless Unicode-equivalent scanning is proven with tests.
- For ASCII words, avoid `StringBuilder` when no characters need changing; return input unchanged.
- When changes are needed, allocate the destination string once on supported TFMs; use downlevel helpers on older TFMs.
- Preserve first-word capitalization and minor-word skip semantics exactly.

Risks:

- Regex `\w` includes underscore and Unicode word characters; scanner must match ASCII cases and fallback otherwise.
- Apostrophe handling must match the current `+'?\w*` shape.
- All-caps detection must not change acronym preservation.
- Turkish/Azeri casing must remain culture-correct or must fallback.

### 4. `Humanize()` PascalCase / Separator Fast Path

The current PascalCase path uses regex matches, LINQ, `string.Join`, word lowercasing, a whole-result uppercase scan, and a final concat. Replace only the common ASCII identifier path first.

Proposed design:

- Keep existing fast path for all-uppercase acronym input, replacing LINQ `All` with a simple loop.
- Replace freestanding separator regex with a span scanner that preserves current semantics.
- For `_` and `-` separated inputs, preserve current behavior that replaces separators with spaces without lowercasing.
- For ASCII Pascal/camel identifiers, emit a single string by scanning boundaries.
- If non-ASCII, punctuation omission, or acronym boundary behavior cannot be proven equivalent, call the existing regex implementation.

Risks:

- Existing regex strips some punctuation by omission; scanner must not accidentally keep punctuation unless tests say so.
- `\p{Lo}` handling is important for other-letter Unicode cases; fallback is preferred.
- Current behavior around mixed acronym inputs is subtle and must be covered before implementation.

### 5. Inflector Case Transform Fast Paths

The inflector methods currently lean on regex replacement chains. Add dedicated ASCII fast paths before regex fallback.

Proposed design:

- `Pascalize`: scan separators (`space`, `_`, `-`) and word starts; preserve current behavior for non-letter characters captured after separators.
- `Camelize`: use the same internal worker with a lower-first option rather than Pascalize + second allocation.
- `Underscore`: replace the three-regex chain with an ASCII scanner that inserts `_` at the same uppercase acronym and lower/digit-to-upper boundaries, maps `-` and whitespace to `_`, and applies culture-aware lowercasing or fallback.
- `Kebaberize`: write directly with `-` separators rather than `Underscore().Dasherize()` to avoid a second allocation.
- `Titleize`: benefit indirectly from `Humanize` + `TitleCase`; do not add bespoke logic unless benchmark evidence shows it remains hot.

Risks:

- The current branch already broadened `PascalizePattern` to capture any char after separators; tests must preserve digit/symbol behavior.
- Regex replacement semantics for consecutive separators and leading separators must be captured by tests.
- `ToLower()` is current-culture in current implementation; fast paths must preserve that behavior through `TextInfo` or fallback.

### 6. English Article Sort Span Rewrite

This is lower-impact but low-risk only if parser parity is precise.

Proposed design:

- Replace `ArticleRegex().IsMatch(item)` with a local span prefix parser that matches current behavior for `^((The)|(the)|(a)|(A)|(An)|(an))\s\w+`.
- Preserve prefix matching semantics and trimming behavior.
- Include explicit tests for non-ASCII word tails (`The Éclair`, `An Æon`), `_` and digit word tails, trailing punctuation after a valid word (`The Theater!`), multiple spaces, and non-space whitespace.
- For non-space whitespace, explicitly preserve current regex behavior unless a separate behavior-change decision is made.
- Keep `Array.Sort(transformed)` behavior unchanged.

### 7. Verification Matrix

Implementation is not complete until these pass, adjusted only for host capability:

- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`
- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net11.0`
- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0`; if skipped, include `dotnet --list-runtimes` evidence showing the runtime is absent.
- `dotnet pack src/Humanizer/Humanizer.csproj -c Release -o <temp-output>`
- `dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic`
- `git diff --check`
- targeted benchmarks for changed surfaces on `net10.0` and `net11.0`

For each implementation PR, capture:

- before/after mean and allocation for each targeted benchmark
- correctness test command output
- list of fast-path fallback triggers and why they exist
- PR notes separating pre-existing branch changes from this epic's changes

## Task Breakdown

1. Benchmark and behavior baseline for string transforms.
2. `ToTitleCase` ASCII scanner and single-allocation/downlevel-safe write path.
3. `Humanize()` PascalCase/separator scanner and fallback partition.
4. Inflector fast paths for `Pascalize`, `Camelize`, `Underscore`, `Kebaberize`.
5. English article sort span parser.
6. Rune/downlevel decision gate and optional supplementary-plane behavior tests.
7. Final benchmark comparison, format, pack, test matrix, and PR notes.

## Acceptance Criteria

- [ ] Each optimized method has pre-change behavior tests for edge cases before implementation changes land.
- [ ] The Rune/downlevel decision is complete before implementation tasks begin.
- [ ] ASCII fast paths avoid regex and LINQ allocations on benchmarked common cases.
- [ ] Non-ASCII and culture-sensitive cases either remain on existing fallback or are proven equivalent with targeted tests.
- [ ] No new Rune dependency/import/API usage is added without a separate approved task.
- [ ] `net48` and `netstandard2.0` compatibility is preserved and proven by pack.
- [ ] Benchmark results meet the objective thresholds above or document why a target was intentionally rejected.
- [ ] CI validation and local formatting/pack/diff checks pass before PR handoff.

## Boundaries

Out of scope for this epic:

- Rewriting pluralization/singularization vocabulary internals.
- Changing public API contracts or documented outputs.
- Adding broad grapheme-cluster support to truncation APIs.
- Importing a Rune shim for `net48`/`netstandard2.0`.
- Reworking the benchmark GitHub workflow beyond adding benchmark coverage needed for this work.

## Decision Context

The main engineering decision is to optimize by removing unnecessary machinery on common ASCII inputs, not by making all string code Unicode-scalar-aware. This matches the benchmark evidence and the .NET guidance for `Rune`: use Rune when code needs Unicode scalar semantics or surrogate-pair correctness; do not use it for known `char` delimiters or ASCII identifier scanning. The fallback-first approach preserves Humanizer's broad locale behavior while giving the common string transformation APIs a faster path.
