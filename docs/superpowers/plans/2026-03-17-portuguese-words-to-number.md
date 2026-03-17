# Portuguese Words-to-Number Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents are available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Provide native Portuguese words-to-number conversion while keeping TDD discipline and culture-specific tests.

**Architecture:** Add a dedicated `PortugueseWordsToNumberConverter` that mirrors the Portuguese `NumberToWords` logic for cardinal/ordinal dictionaries, hook it into `WordsToNumberConverterRegistry`, and drive the behavior through new `[UseCulture("pt-PT")]` xUnit coverage.

**Tech Stack:** C# 12, .NET 10 / .NET 8, xUnit, `System.Globalization`, `FrozenDictionary` helpers.

---

### Task 1: Implement Portuguese words-to-number pipeline

**Files:**
- Create: `src/Humanizer/Localisation/WordsToNumber/PortugueseWordsToNumberConverter.cs`
- Modify: `src/Humanizer/Configuration/WordsToNumberConverterRegistry.cs`
- Modify: `tests/Humanizer.Tests/WordsToNumberTests.cs`

**Tests:**
- Scope: `tests/Humanizer.Tests/Humanizer.Tests.csproj`
- Command (Step 2 failure): `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --filter FullyQualifiedName~WordsToNumberTests --framework net10.0`
- Command (Step 4 verification): run the same filter for both `net10.0` and `net8.0`

- [ ] **Step 1: Write the failing Portuguese spec.** Add a `[UseCulture("pt-PT")] WordsToNumberTests_Portuguese` class that exercises cardinal/ordinal parsing plus invalid input. The new test class should compile but fail because no converter exists yet.
- [ ] **Step 2: Run the failing regression.** Invoke `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --filter FullyQualifiedName~WordsToNumberTests --framework net10.0` expecting at least the Portuguese tests to fail with `NotSupportedException`.
- [ ] **Step 3: Implement the converter/registry.** Create `PortugueseWordsToNumberConverter` (deriving from `GenderlessWordsToNumberConverter`) with Portuguese cardinal map, ordinal map built from `PortugueseNumberToWordsConverter`, normalization (lowercase, strip punctuation, replace `-` with spaces), `TryConvert` handling negative prefixes (`"menos"`) and ordinal abbreviations (`"1º"`, `"1ª"` etc.). Update `WordsToNumberConverterRegistry` to register `"pt"` in both registration lists (explicit and culture fallback).
- [ ] **Step 4: Run the full validation.** Execute the filtered test suite twice—once for `net10.0`, once for `net8.0`—and verify all Portuguese cases pass.
- [ ] **Step 5: Commit the work.** `git add` the new converter, registry change, and tests, then commit with `git commit -m "feat: add Portuguese words-to-number support"`.
