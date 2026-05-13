# fn-11-fix-urdu-locale-ci-pr-feedback-rebase.3 PR feedback: unify NumberWordSuffixOrdinalizer negative/positive irregulars

## Description
Fix the negative-irregular path in `NumberWordSuffixOrdinalizer` so that negative and positive ordinals both source irregulars from the ordinalizer's own `ExactReplacements`, instead of positive coming from `surfaces.ordinal.numeric` and negative coming from `surfaces.number.words.ordinal` via `INumberToWordsConverter.ConvertToOrdinal`. Add a regression test that would have caught the divergence.

**Size:** M
**Files:**
- `src/Humanizer/Localisation/Ordinalizers/NumberWordSuffixOrdinalizer.cs`
- `tests/Humanizer.Tests/Localisation/ur/UrduOrdinalTests.cs` (or appropriate ordinal test file)

## Approach

### The bug
At `NumberWordSuffixOrdinalizer.cs:53`, when `block.ExactReplacements.TryGetValue(magnitude, out var negExact)` matches, the code ignores `negExact` and falls through to `INumberToWordsConverter.ConvertToOrdinal`. This makes negative irregulars come from `surfaces.number.words.ordinal` while positive irregulars come from `surfaces.ordinal.numeric`.

### The fix
When the negative-magnitude exact lookup matches, compose the negative ordinal using:
1. Get the converter to produce the negative cardinal and positive cardinal for the same magnitude.
2. Extract the negative prefix by comparing: if the negative cardinal ends with the positive cardinal, the prefix is everything before that suffix.
3. If prefix extraction fails (negative cardinal does not end with positive cardinal), throw `InvalidOperationException` — do NOT silently drop the exact replacement or fall back to `DefaultSuffix`.
4. Return `prefix + negExact`.

Concrete implementation shape:
```csharp
if (number < 0)
{
    var magnitude = number == int.MinValue ? (long)int.MaxValue + 1 : Math.Abs(number);
    if (magnitude <= int.MaxValue &&
        block.ExactReplacements.TryGetValue((int)magnitude, out var negExact))
    {
        var converter = Configurator.GetNumberToWordsConverter(culture);
        var positiveCardinal = converter.Convert((int)magnitude, effectiveGender);
        var negativeCardinal = converter.Convert(number, effectiveGender);

        if (!negativeCardinal.EndsWith(positiveCardinal, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Cannot extract negative prefix for culture '{culture.Name}' from number-word-suffix ordinalizer.");
        }

        return negativeCardinal[..^positiveCardinal.Length] + negExact;
    }

    // No exact match — fall through to existing suffix logic
}
```

The `number-word-suffix` engine only supports converters whose negative cardinal output is `negativePrefix + positiveCardinal`. This invariant holds for all current locales using this engine. Making it throw-on-violation keeps the contract testable and prevents silent data loss.

### The regression test
The current Urdu YAML has **identical** exact replacements in both `surfaces.number.words.ordinal` and `surfaces.ordinal.numeric` (both have `1: 'پہلا'`, `2: 'دوسرا'`, etc.), so a locale-driven test will NOT expose the bug. The regression test MUST create a deliberate divergence.

Approach: instantiate `NumberWordSuffixOrdinalizer` directly with custom `ExactReplacements` containing a sentinel value that differs from what the `INumberToWordsConverter.ConvertToOrdinal` would produce. Assert with **exact equality** that the negative path returns the locale's negative prefix + the ordinalizer's sentinel.

Example intent:
```csharp
[Fact]
public void NegativeExactReplacement_UsesOrdinalizerNotConverter()
{
    var ordinalizer = new NumberWordSuffixOrdinalizer(
        new CultureInfo("ur"),
        new NumberWordSuffixOrdinalizer.Options(
            masculine: new NumberWordSuffixOrdinalizer.GenderBlock(
                "واں",
                new Dictionary<int, string> { [1] = "آزمائشی" }.ToFrozenDictionary()),
            feminine: new NumberWordSuffixOrdinalizer.GenderBlock(
                "ویں",
                FrozenDictionary<int, string>.Empty),
            GrammaticalGender.Masculine));

    var result = ordinalizer.Convert(-1, "-1", GrammaticalGender.Masculine);
    // Exact equality: negative prefix + sentinel, not converter's ordinal
    Assert.Equal("منفی آزمائشی", result);
}
```

Before the fix: returns converter-derived `"منفی پہلا"`. After: returns `"منفی آزمائشی"`.

Do NOT:
- Change public APIs or ordinalizer wiring beyond this single branch.
- Add unrelated refactors.
- Skip the regression test.
- Use a locale-driven test that happens to pass before AND after the fix.
- Use `Assert.Contains` — use `Assert.Equal` for exact verification.
- Silently fall back to `DefaultSuffix` when exact replacement matched but prefix extraction failed.

## Investigation targets
**Required:**
- `src/Humanizer/Localisation/Ordinalizers/NumberWordSuffixOrdinalizer.cs:1-120` — read the whole class to see how positive irregulars are emitted (the pattern to mirror) and how `negExact` should plug in.
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalizerProfileCatalogInput.cs` — how `ExactReplacements` are populated into the generated profile.
- `src/Humanizer/Localisation/NumberToWords/IndianGroupingGenderedNumberToWordsConverter.cs` — how `ConvertToOrdinal` computes its output (to understand what the negative cardinal looks like and how to extract the prefix).

**Optional:**
- Existing ordinalizer tests under `tests/Humanizer.Tests/Localisation/ur/` for assertion style and culture setup.

## Key context
- This is the Codex reviewer's P2 comment on PR #1720.
- The fix is small: one branch, one code path. The test is where the real work is, because it must be designed to fail without the fix.
- The `NumberWordSuffixOrdinalizer.Options` record contains `GenderBlock` with `DefaultSuffix` and `ExactReplacements` — it does NOT contain the locale's negative word/prefix, which is why the converter must be used to extract it.

## Acceptance
- [ ] Negative-irregular branch uses the matched `negExact` from `block.ExactReplacements`, composed with the locale's negative prefix extracted from the converter.
- [ ] Prefix extraction failure throws `InvalidOperationException` (not silent fallback).
- [ ] Regression test uses deliberate divergence (custom `ExactReplacements` with sentinel) and `Assert.Equal` for exact verification; would fail on pre-fix code.
- [ ] All Humanizer.Tests pass on net10.0 and net8.0.
- [ ] Lint + build clean.

## Done summary
Fixed NumberWordSuffixOrdinalizer negative-irregular branch to use matched negExact from ExactReplacements composed with the locale's negative prefix (extracted by comparing negative/positive cardinals), instead of delegating to ConvertToOrdinal. Added regression test with deliberate sentinel divergence that proves the ordinalizer path is taken.
## Evidence
- Commits: 24a46d370203596ca01ee33af29173874fafb4f5
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --filter-class *UrduOrdinalTests, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 --filter-class *UrduOrdinalTests
- PRs: