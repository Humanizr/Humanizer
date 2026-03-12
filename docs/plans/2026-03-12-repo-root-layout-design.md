# Repo Root Layout Design

**Date:** 2026-03-12

## Goal

Move the repository-scoped solution, build, package-management, signing, and tooling configuration files from `src/` to the repository root so standard parent-directory traversal works across `src/*` and `tests/*`.

## Scope

Move these files to the repository root:

- `src/Humanizer.slnx`
- `src/.editorconfig`
- `src/Directory.Build.props`
- `src/Directory.Build.targets`
- `src/Directory.Solution.targets`
- `src/Directory.Packages.props`
- `src/Humanizer.snk`
- `src/nuget.config`
- `src/ResXManager.config.xml`

Remove the now-unnecessary forwarding files:

- `tests/Directory.Build.props`
- `tests/Directory.Build.targets`
- `tests/Directory.Packages.props`

## Design

The repository root becomes the canonical solution and MSBuild root. Projects under `src/` and `tests/` should discover the root `Directory.*` files through normal MSBuild parent traversal. `Verify.XunitV3` should discover `Humanizer.slnx` through its existing parent-directory search without explicit `SolutionDir` or `SolutionName` overrides.

The move must preserve existing behavior:

- package version centralization remains repo-wide
- signing still uses `Humanizer.snk`
- nuspec packing still resolves the same files under `NuSpecs/`
- CI and documentation reference the repo root rather than `src/`
- analyzer packaging for Roslyn variants remains unchanged

## Compatibility Notes

- `tests/*` must not keep test-only overrides or shims after the move.
- Relative paths inside `Humanizer.slnx`, `Directory.Build.props`, and `src/Humanizer/Humanizer.csproj` need rebasing.
- `nuget.config` should work through standard NuGet parent traversal once moved to the root.

## Validation

- `dotnet test --project tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.csproj`
- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`
- `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0`
- `dotnet build src/Humanizer/Humanizer.csproj -c Release /t:PackNuSpecs /p:PackageOutputPath=<path>`
- confirm the Verify warning about solution discovery is gone
- confirm Roslyn analyzer variant outputs still build and pack
