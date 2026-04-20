# Performance follow-up hot paths

## Goal & Context
Follow up PR #1732 after .NET 8 became available on the host. Broaden benchmark coverage to .NET 8 and optimize the remaining meaningful hotspots identified from benchmark comparison artifacts: phrase clock template expansion, multi-part TimeSpan humanization, collection formatting, ByteSize string formatting, and misleading ordinal benchmark setup overhead.

## Architecture & Data Models
Keep runtime changes inside existing Humanizer formatting components. Prefer allocation reductions and one-pass loops over broader API or behavior changes. Benchmark-only changes belong under `src/Benchmarks`; CI benchmark matrix changes belong in `.github/workflows/benchmarks-baseline-vs-current.yml`.

## API Contracts
No public API changes. Existing extension method output and culture-specific formatting must remain identical.

## Edge Cases & Constraints
Preserve one-shot enumerable support in collection formatting. Preserve `countEmptyUnits` TimeSpan precision semantics. Preserve locale-specific phrase-clock Eifeler/day-period behavior. Preserve custom `IFormatProvider` handling in ByteSize.

## Acceptance Criteria
- [ ] Benchmark project and workflow include `net8.0` coverage.
- [ ] Ordinal benchmarks do not allocate `CultureInfo` inside benchmark methods.
- [ ] Runtime hot-path changes keep existing exact-output tests passing.
- [ ] Targeted tests pass on net8.0, net10.0, and net11.0.
- [ ] `dotnet pack`, `verify-packages.ps1`, and formatting verification pass.

## Boundaries
Do not add risky semantic rewrites, locale behavior changes, or new public APIs. Do not attempt .NET Framework validation on macOS.

## Decision Context
The current PR already won the large string transformation hotspots. The remaining useful work is constrained to measurable hot paths with low behavior risk and better benchmark coverage across all modern supported TFMs.
