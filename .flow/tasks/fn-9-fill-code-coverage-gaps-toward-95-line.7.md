## Description
Establish dual-Roslyn instrumentation for `Humanizer.Analyzers.Tests` so both `#if ROSLYN_4_14_OR_GREATER` arms get measurable coverage. This task is **infrastructure-only** — the actual analyzer branch tests live in `.16`, which depends on this task. **No escape hatch**: if dual instrumentation proves technically infeasible after investigation, the task stops and escalates to the epic for explicit scope amendment.

**Size:** M
**Files:**
- `tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.csproj` — multi-target or conditional `ProjectReference` so the analyzer builds against both Roslyn versions. Preferred shape: two `ProjectReference` conditions using the same `RoslynVersion` property the analyzer csproj uses, e.g. `<ProjectReference ... Condition="'$(RoslynVersion)'=='4.8'">` plus a sibling for `RoslynVersion=4.14+`. Alternative: two companion test projects sharing source via `<Compile Include="..." />`.
- `Directory.Packages.props` — confirm `Microsoft.CodeAnalysis.CSharp` versions align with the toggle
- `tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.DualRoslyn.Tests.cs` (new, small) — a pair of smoke tests, one per Roslyn arm, that import a single symbol from each arm and run. Their coverage data confirms the instrumentation wiring works.

## Approach
- Investigate `src/Humanizer.Analyzers/Humanizer.Analyzers.csproj` for how `ROSLYN_4_14_OR_GREATER` is selected today (MSBuild `Condition` on `PackageReference` version or a `DefineConstants` toggle). Replicate the same mechanism in the test project.
- Run the test project and capture cobertura. Verify lines inside both `#if ROSLYN_4_14_OR_GREATER` and its `#else` branch are instrumented (hit or miss — just visible in the report). This is the proof point.
- If dual instrumentation is technically infeasible (e.g. `Microsoft.CodeAnalysis.CSharp` package cannot coexist in both versions in one test project and companion-project pattern conflicts with existing repo structure), **halt** and escalate — do NOT fall back to single-arm instrumentation with a Boundaries note. The epic must consciously reduce scope if that happens.

## Investigation targets
**Required:**
- `src/Humanizer.Analyzers/Humanizer.Analyzers.csproj` — the `ROSLYN_4_14_OR_GREATER` toggle
- `tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.csproj` — current single-target config
- `Directory.Packages.props` — `Microsoft.CodeAnalysis.CSharp` package declarations
- `https://learn.microsoft.com/visualstudio/msbuild/msbuild-conditional-constructs` — MSBuild conditions reference

## Acceptance
- [ ] Test project CI run produces coverage data distinguishable for both Roslyn arms (e.g. two artifact sets or a merged cobertura containing both conditional branches instrumented).
- [ ] A smoke test per arm executes and is visible in the merged report.
- [ ] No `[ExcludeFromCodeCoverage]` or gate-filter exclusions added.
- [ ] If dual instrumentation is infeasible, no code is merged; a note is added to the epic describing the constraint and requesting scope amendment.

## Done summary
_To be filled on completion._

## Evidence
- Commits:
- Tests:
- PRs:
