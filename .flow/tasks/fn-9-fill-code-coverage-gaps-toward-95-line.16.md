## Description
Close analyzer branch gaps in `Humanizer.Analyzers` across both Roslyn arms now that `.7` has wired dual instrumentation.

**Size:** M
**Files:**
- `tests/Humanizer.Analyzers.Tests/WordsToNumberMigrationCodeFixTests.cs` (extend — CS0029 arg path + `IWordsToNumberConverter` path)
- `tests/Humanizer.Analyzers.Tests/NamespaceMigrationCodeFixTests.cs` (extend — qualified-name replacement; `GetReplacementName` `fullName.Length == matchedNamespace.Length` edge)
- `tests/Humanizer.Analyzers.Tests/NamespaceMigrationAnalyzerTests.cs` (extend — `AnalyzeQualifiedName` parent-skip branch at `:88-93`)

## Approach
- **WordsToNumberMigrationCodeFixProvider.**
  - CS0266 int-local assignment (existing — keep)
  - CS0029 is registered defensively but cannot fire for long-to-int (always CS0266); method-argument-requiring-int produces CS1503 not CS0029. Fall-through `case "CS0029": case "CS0266":` is a single branch — no separate coverage needed.
  - Input hitting `AllInterfaces.Any(i => i.Name == "IWordsToNumberConverter")` vs direct `ContainingType.Name == "IWordsToNumberConverter"` (`:77-78`)
  - `CheckedExpression` wrapping (`:83`)
  - `root is null` / `semanticModel is null` guards are in the epic's declared-unreachable appendix — NOT covered here.
- **NamespaceMigrationCodeFixProvider.**
  - Qualified-name replacement (`:89-109`)
  - `GetReplacementName` edge at `:137` (`fullName.Length == matchedNamespace.Length`)
  - Both `ROSLYN_4_14_OR_GREATER` arms (`:111-133`, `:149-164`) — exercised automatically via `.7`'s dual instrumentation
- **NamespaceMigrationAnalyzer.** `AnalyzeQualifiedName` parent-skip (`:88-93`).
- Use `CSharpAnalyzerTest<,,>` / `CSharpCodeFixTest<,,,>` patterns from `tests/Humanizer.Analyzers.Tests/Verifiers.cs:1-51`; markup syntax `{|DIAG001:token|}`; `CodeActionIndex` for multi-fix scenarios.

## Investigation targets
**Required:**
- `src/Humanizer.Analyzers/WordsToNumberMigrationCodeFixProvider.cs:17-96`
- `src/Humanizer.Analyzers/NamespaceMigrationCodeFixProvider.cs:37-178`
- `src/Humanizer.Analyzers/NamespaceMigrationAnalyzer.cs:75-168`
- `tests/Humanizer.Analyzers.Tests/Verifiers.cs`
- `artifacts/fn-9-local-coverage/uncovered.json`
- `https://github.com/dotnet/roslyn-sdk/tree/main/src/Microsoft.CodeAnalysis.Testing`

## Acceptance
- [ ] `WordsToNumberMigrationCodeFixProvider` reaches ≥95% line / ≥85% branch in the merged report across both Roslyn arms.
- [ ] `NamespaceMigrationCodeFixProvider` reaches ≥95% line / ≥85% branch in the merged report across both Roslyn arms.
- [ ] `NamespaceMigrationAnalyzer` reaches ≥95% line / ≥85% branch across both arms.
- [ ] Negative test cases (clean code, no diagnostic) exist for every diagnostic-producing branch.
- [ ] No `[ExcludeFromCodeCoverage]` attributes added.

## Done summary
Added 5 analyzer coverage tests across both Roslyn arms:

**WordsToNumberMigrationCodeFixProvider:**
- `FixesConvertViaInterfaceAssignedToInt`: exercises `Convert` path + `containingType.Name == interfaceName` (line 72, 77)
- `FixesConvertViaImplementorAssignedToInt`: exercises `Convert` path + `AllInterfaces.Any` (line 72-73, 78)

**NamespaceMigrationCodeFixProvider:**
- `FixQualifiedNameExactNamespaceMatch`: exercises `GetReplacementName` exact-match edge at line 137-138 and `IsNamespaceMatch` exact-match at line 126-127

**NamespaceMigrationAnalyzer:**
- `NestedQualifiedName_ReportsOutermostOnly`: exercises parent-skip at lines 88-93
- `QualifiedNameNotOldNamespace_NoDiagnostic`: negative case for qualified names

All 26 tests pass in baseline (Roslyn 3.8), all 24 in Roslyn 4.14 arm.
## Evidence
- Commits:
- Tests:
- PRs: