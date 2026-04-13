## Description
Test the three `Default*` fallback converters (currently 0% line). They run when no locale-specific converter is registered for a culture.

**Size:** M
**Files:**
- `tests/Humanizer.Tests/Localisation/Default/DefaultDateToOrdinalWordConverterTests.cs` (new)
- `tests/Humanizer.Tests/Localisation/Default/DefaultDateOnlyToOrdinalWordConverterTests.cs` (new, under `NET6_0_OR_GREATER`)
- `tests/Humanizer.Tests/Localisation/Default/DefaultNumberToWordsConverterTests.cs` (new)

## Approach
- Actual public API from `src/Humanizer/Localisation/DateToOrdinalWords/DefaultDateToOrdinalWordConverter.cs`:
  - `Convert(DateTime date)`
  - `Convert(DateTime date, GrammaticalCase grammaticalCase)`
  - Same two for `DateOnly` on net6+.
  The method reads `CultureInfo.CurrentCulture`; it does **not** accept a `CultureInfo` parameter. Use `UseCultureAttribute` or a local try/finally scope to pin culture.
- **English path.** Pin `CurrentCulture` to `en-US`; call `Convert` with a sample `DateTime`; assert the output is the expected English ordinal phrasing.
- **Non-English / mark-stripping path.** To cover `SanitizeNonEnglishDate` deterministically across Windows/Linux/macOS (ICU/NLS drift on `ar-SA`, `he-IL`, `fa-IR` can skip marks), clone a `CultureInfo`, customize `DateTimeFormat.ShortDatePattern` to include literal `\u200E` (LTR), `\u200F` (RTL), `\u061C` (ALM) bracketed as string literals inside the pattern, and set `TwoLetterISOLanguageName` via a non-English culture base (e.g. start from `eo` or a cloned `ar` invariant). Assert the returned string contains none of the three mark code points.
- **`DefaultNumberToWordsConverter`.** Call `Convert(long)` and `ConvertToOrdinal(int)` under `en-US`; assert each returns `number.ToString(culture)` exactly.
- Direct instantiation is permitted — `InternalsVisibleTo("Humanizer.Tests")` is already configured.

## Investigation targets
**Required:**
- `src/Humanizer/Localisation/DateToOrdinalWords/DefaultDateToOrdinalWordConverter.cs:10-55` — all public methods + `SanitizeNonEnglishDate`
- `src/Humanizer/Localisation/DateToOrdinalWords/DefaultDateOnlyToOrdinalWordConverter.cs:12-52`
- `src/Humanizer/Localisation/NumberToWords/DefaultNumberToWordsConverter.cs:11-30`
- `tests/Humanizer.Tests/UseCultureAttribute.cs:22-54` — culture scoping
- `tests/Humanizer.Tests/OrdinalizerRegistryRecoveryTests.cs` — precedent for direct-instantiation tests
- `artifacts/fn-9-local-coverage/uncovered.json` (from `.1`) — confirms exact uncovered lines per class

## Acceptance
- [ ] `DefaultDateToOrdinalWordConverter` reaches ≥95% line and ≥90% branch; every branch of `SanitizeNonEnglishDate` (LTR, RTL, ALM) hit via a cloned-culture `ShortDatePattern` containing the three code points.
- [ ] `DefaultDateOnlyToOrdinalWordConverter` reaches ≥95% line / ≥90% branch on net6+.
- [ ] `DefaultNumberToWordsConverter` reaches 100% line; both public methods asserted with exact expected output.
- [ ] No dependency on platform-specific `ar-SA` / `he-IL` / `fa-IR` `ToString("d")` output.
- [ ] Tests use `UseCultureAttribute` or local try/finally; thread culture is restored on all code paths.
- [ ] No new `[ExcludeFromCodeCoverage]` attributes introduced.

## Done summary
Added tests for all three Default fallback converters (DefaultDateToOrdinalWordConverter, DefaultDateOnlyToOrdinalWordConverter, DefaultNumberToWordsConverter) covering English ordinal path, non-English short-date path, GrammaticalCase delegation, and deterministic bidi mark sanitization (LTR U+200E, RTL U+200F, ALM U+061C) via cloned-culture ShortDatePatterns with StringComparison.Ordinal assertions.
## Evidence
- Commits: 8dea36aab39e41e8aa2f29fbb25ba2df13bfe35a, 204133427d77adecc7591bbab37df4897576cb1a
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0, dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
- PRs: