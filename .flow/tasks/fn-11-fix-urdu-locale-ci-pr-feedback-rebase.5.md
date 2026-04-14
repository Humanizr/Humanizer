# fn-11-fix-urdu-locale-ci-pr-feedback-rebase.5 Final integration verification + force-with-lease push

## Description
After all implementation tasks (fn-11.2, fn-11.3, fn-11.4) are complete, run full integration verification across all TFMs and push the final branch state.

**Size:** S
**Files:** None (verification-only task)

## Approach
1. Verify all implementation tasks (fn-11.2, fn-11.3, fn-11.4) are marked done.
2. Run full verification suite:

```bash
# Full test suite on all available TFMs
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0

# net48 on Windows host or CI — REQUIRED for epic acceptance
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net48

# Source generator tests
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj

# Full solution build
dotnet build Humanizer.slnx -c Release

# Lint verification
dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic
```

3. If net48 cannot run locally (non-Windows host), document that net48 evidence must come from Windows CI after push.
4. Final push:
```bash
git push --force-with-lease origin feat/urdu-locale
```

Do NOT:
- Make code changes in this task (it is verification-only).
- Skip any verification step.
- Push without passing net8.0 and net10.0 tests locally.

## Key context
- This task exists to ensure the combined result of fn-11.2/3/4 (which run in parallel) is verified as a whole before the final push.
- net48 test execution requires a Windows host. If running on macOS, the net48 tests will build but not execute. In that case, net48 evidence comes from CI after the push — but net8.0 and net10.0 must pass locally before pushing.

## Acceptance
- [ ] `dotnet test` passes on net10.0 and net8.0 for Humanizer.Tests.
- [ ] `dotnet test` passes for Humanizer.SourceGenerators.Tests.
- [ ] `dotnet build Humanizer.slnx -c Release` succeeds.
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` passes.
- [ ] Full Humanizer.Tests pass on net48 (Windows host or CI evidence).
- [ ] `git push --force-with-lease origin feat/urdu-locale` succeeds.
