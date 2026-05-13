# fn-16-performance-follow-up-hot-paths.1 Broaden benchmark coverage and optimize remaining formatting hot paths

## Description
TBD

## Acceptance
- [ ] TBD

## Done summary
Broadened benchmark coverage to net8.0 and added ByteSize/collection benchmark fixtures.
Removed CultureInfo allocation from ordinal benchmark methods.
Optimized phrase-clock template expansion with cached template plans and pre-parsed literal/placeholder segments.
Optimized multi-part TimeSpan humanization by building precision-limited parts in one pass.
Optimized default collection formatting and ByteSize string formatting to reduce LINQ/string.Format overhead.
## Evidence
- Commits:
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net11.0, dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/packages/perf-followup-final, pwsh -NoProfile -File tests/verify-packages.ps1 -PackageVersion 3.5.0-preview.20.g869cf91776 -PackagesDirectory artifacts/packages/perf-followup-final, dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic, dotnet build src/Benchmarks/Benchmarks.csproj -c Release -f net8.0, dotnet build src/Benchmarks/Benchmarks.csproj -c Release -f net10.0, dotnet build src/Benchmarks/Benchmarks.csproj -c Release -f net11.0, dotnet run -c Release --project src/Benchmarks/Benchmarks.csproj -f net8.0 -- --filter *ByteSizeBenchmarks* --job short --warmupCount 1 --iterationCount 1, dotnet run -c Release --project src/Benchmarks/Benchmarks.csproj -f net8.0 -- --filter *TimeOnlyToClockNotationConverterBenchmarks* --job short --warmupCount 1 --iterationCount 1, dotnet run -c Release --project src/Benchmarks/Benchmarks.csproj -f net8.0 -- --filter *CollectionHumanizeBenchmarks* *FormatterBenchmarks.RussianTimeSpanHumanizeMultiPart* --job short --warmupCount 1 --iterationCount 1
- PRs: