# German Words-To-Number Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents are available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add German locale words-to-number parsing so `de`, `de-CH`, and `de-LI` callers can convert spelled-out numerals and ordinals without hitting `NotSupportedException`.

**Architecture:** Introduce a `GermanWordsToNumberConverter` with German-specific maps, reuse the registry caching, and drive coverage through failing-then-passing tests that exercise `WordsToNumberTests` under German cultures.

**Tech Stack:** C# 12, .NET 10/8, xUnit, `dotnet test` CLI.

---

### Task 1: Expand WordsToNumberTests with German coverage

**Files:**
- Modify: `tests/Humanizer.Tests/WordsToNumberTests.cs`

- [ ] **Step 1: Write the failing tests**
  - Add a fact that runs the same numeral/ordinal strings under `"de-CH"` and `"de-LI"` via `CultureInfo` to ensure Swiss/Liechtenstein derivatives resolve the new converter.

```csharp
[UseCulture("de-DE")]
public class WordsToNumberTests_German
{
    [Theory]
    [InlineData("null", 0)]
    [InlineData("minus fünf", -5)]
    [InlineData("einundzwanzig", 21)]
    [InlineData("zweiunddreißig", 32)]
    [InlineData("einhundertneunundvierzig", 149)]
    [InlineData("zweitausendachtundzwanzig", 2028)]
    [InlineData("dreißigste", 30)]
    [InlineData("einundzwanzigste", 21)]
    public void ToNumber_German(string words, int expected) => Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
}
```

- [ ] **Step 2: Run net10 WordsToNumber tests to confirm failure**
  Run: `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release --framework net10.0 --filter WordsToNumberTests -v m`
  Expected: Failure (NotSupportedException referencing `'de'`).

### Task 2: Implement the German converter

**Files:**
- Create: `src/Humanizer/Localisation/WordsToNumber/GermanWordsToNumberConverter.cs`
- Modify: `src/Humanizer/Configuration/WordsToNumberConverterRegistry.cs`

- [ ] **Step 1: Implement maps/logic**
  - Mirror the English converter structure but populate `NumbersMap`/`OrdinalMap` with German words (`null`, `eins`, `zwei`, ..., `dreißig`, `hundert`, `tausend`, `Million`.
  - Normalize input (`minus`/`negativ`, `und`, `ß` vs `ss`, hyphens, ordinal endings like `-te`, `-ste`).
  - Reuse the same `TryConvertWordsToNumber` strategy so ordinals stop processing and commonscaled units are handled (`hundert`, `tausend`).

- [ ] **Step 2: Register the converter**
  - Update `WordsToNumberConverterRegistry` so `de`, `de-CH`, and `de-LI` resolve to `GermanWordsToNumberConverter`, falling back to default for other languages.

- [ ] **Step 3: Run net10 WordsToNumber tests to see them pass**
  Run: `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release --framework net10.0 --filter WordsToNumberTests -v m`
  Expected: Green.

### Task 3: Confirm net8 compatibility and clean-up

**Files:**
- Same as Task 2 plus `tests/Humanizer.Tests/WordsToNumberTests.cs` if test expectations change.

- [ ] **Step 1: Run net8 WordsToNumber tests**
  Run: `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release --framework net8.0 --filter WordsToNumberTests -v m`
  Expected: Green.

- [ ] **Step 2: Review git status and stage changes**
  Run: `git status -sb`

- [ ] **Step 3: Commit**
  Run: `git commit -am "feat: add german words to number"`
