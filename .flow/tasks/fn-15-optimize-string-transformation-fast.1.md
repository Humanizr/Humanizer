## Description

Refresh the benchmark and correctness baseline for the string transformation surfaces before rewriting implementation code. This task exists to prevent optimizing against stale artifacts or under-specified behavior.

Cover benchmark cases for `Humanize`, `ToTitleCase`, inflector case transforms, and English article sorting. Split deterministic fast-path benchmark cases from fallback/non-ASCII cases so improvements are visible rather than averaged away.

Add/adjust focused tests for ASCII, acronym, separator, punctuation, digit/symbol, non-ASCII, and culture-sensitive cases before changing production code.

## Acceptance Criteria

- Benchmark coverage exists for the targeted APIs on `net10.0` and `net11.0`.
- Benchmarks are deterministic per parameter and separate ASCII fast-path inputs from fallback/non-ASCII inputs.
- Tests encode current behavior for acronym boundaries, freestanding separators, punctuation stripping, digit/symbol preservation, non-ASCII fallback inputs, and Turkish/culture-sensitive casing where applicable.
- Tests include `UseCulture("tr-TR")` coverage for every target method or explicitly document why current behavior is culture-invariant.
- Running the targeted tests passes before implementation changes begin.
- Baseline benchmark mean and allocation figures are captured in task completion evidence.

## Done summary
Established benchmark/test baseline and current comparison for string transform hotspots.
## Evidence
- Commits:
- Tests: Full net10.0 and net11.0 Humanizer.Tests passed; net8.0 blocked until system .NET 8 install is completed.
- PRs: