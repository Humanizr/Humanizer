## Description

Make a narrow follow-up pass on the TitleCase ASCII fast path: remove avoidable scans and culture calls while preserving Unicode fallback and Turkic I casing behavior.

## Acceptance Criteria

- ASCII TitleCase avoids the separate up-front non-ASCII scan.
- Ordinary ASCII cultures avoid per-character TextInfo casing calls while Turkish/Azeri still use culture-aware casing.
- Non-ASCII input still falls back to the regex/culture path.
- Benchmarks cover changed ASCII paths and tests pass on available TFMs.

## Done summary
Optimized the TitleCase ASCII fast path by merging non-ASCII detection into the main scan, using ordinal ASCII casing for non-Turkic cultures, preserving Turkish/Azeri culture-sensitive casing, and adding benchmark coverage for additional ASCII title-case paths.
## Evidence
- Commits:
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 -- --filter-class TransformersTests, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net11.0 -- --filter-class TransformersTests, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net11.0, dotnet pack src/Humanizer/Humanizer.csproj -c Release -o /tmp/humanizer-titlecase-pack, pwsh tests/verify-packages.ps1 -PackageVersion 3.5.0-preview.19.g89f4f0a08c -PackagesDirectory /tmp/humanizer-titlecase-pack, dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic, git diff --check
- PRs:
