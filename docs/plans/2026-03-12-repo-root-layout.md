# Repo Root Layout Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Move repository-scoped solution and build configuration from `src/` to the repo root and update all references so standard parent traversal works without shims.

**Architecture:** Establish the repo root as the canonical solution and MSBuild boundary, then rebase references in project files, the solution file, CI, and docs. Remove forwarding files under `tests/` once root discovery is working.

**Tech Stack:** MSBuild, .NET SDK, xUnit v3 with Microsoft.Testing.Platform, Verify, Azure Pipelines

---

### Task 1: Record The New Root Layout

**Files:**
- Modify: `docs/plans/2026-03-12-repo-root-layout-design.md`
- Modify: `docs/plans/2026-03-12-repo-root-layout.md`

**Step 1: Confirm the moved file set**

Check the approved file set and confirm no test-only overrides remain.

**Step 2: Keep the plan aligned**

Update the design or plan only if implementation uncovers a pathing detail that changes the intended structure.

### Task 2: Move Repo-Scoped Files To Root

**Files:**
- Move: `src/Humanizer.slnx` -> `Humanizer.slnx`
- Move: `src/.editorconfig` -> `.editorconfig`
- Move: `src/Directory.Build.props` -> `Directory.Build.props`
- Move: `src/Directory.Build.targets` -> `Directory.Build.targets`
- Move: `src/Directory.Solution.targets` -> `Directory.Solution.targets`
- Move: `src/Directory.Packages.props` -> `Directory.Packages.props`
- Move: `src/Humanizer.snk` -> `Humanizer.snk`
- Move: `src/nuget.config` -> `nuget.config`
- Move: `src/ResXManager.config.xml` -> `ResXManager.config.xml`
- Delete: `tests/Directory.Build.props`
- Delete: `tests/Directory.Build.targets`
- Delete: `tests/Directory.Packages.props`

**Step 1: Move the files**

Move the repo-scoped files from `src/` to the repository root without changing contents yet.

**Step 2: Remove obsolete shims**

Delete the forwarding files under `tests/` after the root-level files exist.

### Task 3: Rebase Relative Paths

**Files:**
- Modify: `Directory.Build.props`
- Modify: `Humanizer.slnx`
- Modify: `src/Humanizer/Humanizer.csproj`

**Step 1: Fix signing and shared props paths**

Ensure `AssemblyOriginatorKeyFile` and any other root-relative paths still resolve from project locations after the move.

**Step 2: Fix solution item paths**

Rewrite `Humanizer.slnx` entries so root files and project paths are correct from the new location.

**Step 3: Remove Verify-specific overrides**

Delete explicit `SolutionDir` and `SolutionName` from `tests/Humanizer.Tests/Humanizer.Tests.csproj` if root solution discovery works again.

### Task 4: Update Tooling, CI, And Docs

**Files:**
- Modify: `azure-pipelines.yml`
- Modify: `AGENTS.md`
- Modify: `.github/copilot-instructions.md`

**Step 1: Rebase documented build commands**

Update docs so the build command runs from the repo root.

**Step 2: Rebase CI assumptions**

Update pipeline paths only where they currently assume the working directory or solution root is `src/`.

### Task 5: Validate End To End

**Files:**
- Verify: `src/Humanizer/Humanizer.csproj`
- Verify: `tests/Humanizer.Tests/Humanizer.Tests.csproj`
- Verify: `tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.csproj`

**Step 1: Run analyzer tests**

Run:

```powershell
dotnet test --project tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.csproj
```

Expected: pass with no Verify solution-discovery warning.

**Step 2: Run main tests**

Run:

```powershell
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
```

Expected: both pass with no Verify solution-discovery warning.

**Step 3: Run release build and packing**

Run:

```powershell
dotnet build src/Humanizer/Humanizer.csproj -c Release /t:PackNuSpecs /p:PackageOutputPath=artifacts/packages
```

Expected: build succeeds without warnings or errors and still produces analyzer variant outputs.
