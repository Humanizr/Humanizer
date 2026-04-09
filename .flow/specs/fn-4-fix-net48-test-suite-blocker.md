# Fix net48 test suite blocker: Enum.GetValues<T>() not available in .NET Framework 4.8

## Goal & Context
`LocaleTheoryMatrixCompletenessTests.cs:439` uses `Enum.GetValues<GrammaticalGender>()` which is a .NET 5+ API not available in .NET Framework 4.8. This prevents running the full Humanizer test suite on net48.

## Problem
The generic overload `Enum.GetValues<T>()` was introduced in .NET 5. When targeting net48, this causes a compilation or runtime error, blocking the entire test suite from running on that framework.

## Fix
Replace `Enum.GetValues<GrammaticalGender>()` with `(GrammaticalGender[])Enum.GetValues(typeof(GrammaticalGender))` or add a `#if` guard for the net48 target.

## Acceptance Criteria
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net48` compiles and runs on Windows
- [ ] No regressions on net10.0 or net8.0

## Boundaries
- Only fix the `Enum.GetValues<T>()` usage; do not change test logic
- This is a pre-existing issue, not introduced by epic fn-3
