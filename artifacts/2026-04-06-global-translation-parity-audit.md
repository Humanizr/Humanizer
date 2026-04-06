# Global Translation Parity Audit

Generated: 2026-04-06 02:14:46 -04:00
Scope: shipped locale translation parity for non-inflectional surfaces only.
Excluded from verdict: grammatical gender, word-form, and overload inflection matrices.

## Verdict

No. The current tree does **not** prove 100% translation parity for everything except inflections.

What is proved:
- Shared exact-output matrix coverage is complete for all 62 shipped locales.
- Formatter, list, phrase, number.parse, number.words.cardinal, numeric ordinalizer, and compass slices are green on `net10.0` and `net8.0`.

What is not proved:
- `ordinal.date`
- `ordinal.dateOnly`
- `clock`

Those three surfaces currently fail exact-output parity sweeps with authored locale-owned expectations.

## Verification Summary

| command | result |
| --- | --- |
| `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --no-restore -- --filter-class Humanizer.Tests.Localisation.LocaleTheoryMatrixCompletenessTests` | pass, 5766/5766 |
| `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 --no-restore -- --filter-class Humanizer.Tests.Localisation.LocaleTheoryMatrixCompletenessTests` | pass, 5766/5766 |
| `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --no-restore -- --filter-class Humanizer.Tests.Localisation.FormatterExactOutputTests --filter-class Humanizer.Tests.Localisation.ResourceBackedPhraseTests --filter-class Humanizer.Tests.Localisation.NumberWordPhraseTests --filter-class Humanizer.Tests.Localisation.NumberWordMagnitudeTests --filter-class Humanizer.Tests.Localisation.NumberWordOverloadTests` | pass, 17170/17170 |
| `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 --no-restore -- --filter-class Humanizer.Tests.Localisation.FormatterExactOutputTests --filter-class Humanizer.Tests.Localisation.ResourceBackedPhraseTests --filter-class Humanizer.Tests.Localisation.NumberWordPhraseTests --filter-class Humanizer.Tests.Localisation.NumberWordMagnitudeTests --filter-class Humanizer.Tests.Localisation.NumberWordOverloadTests` | pass, 17170/17170 |
| `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --no-restore -- --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.Formatter_NativeLocales_UseExpectedRelativeDateAndDurationStrings --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.CollectionFormatter_NativeLocales_UseExpectedConjunction --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.NumberToWords_NativeLocales_UseExpectedCardinalForms --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.Ordinalizer_NativeLocales_UseExpectedForms --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.Ordinalizer_ExactLocales_UseExpectedDefaultForms --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.Ordinalizer_ExactLocales_UseExpectedNegativeFallbackForms --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.WordsToNumber_NativeLocales_ParseNativeWords --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.WordsToNumber_RegisteredLocales_RoundTripNativeWords` | pass, 1736/1736 |
| `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 --no-restore -- --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.Formatter_NativeLocales_UseExpectedRelativeDateAndDurationStrings --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.CollectionFormatter_NativeLocales_UseExpectedConjunction --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.NumberToWords_NativeLocales_UseExpectedCardinalForms --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.Ordinalizer_NativeLocales_UseExpectedForms --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.Ordinalizer_ExactLocales_UseExpectedDefaultForms --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.Ordinalizer_ExactLocales_UseExpectedNegativeFallbackForms --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.WordsToNumber_NativeLocales_ParseNativeWords --filter-method Humanizer.Tests.Localisation.LocaleRegistrySweepTests.WordsToNumber_RegisteredLocales_RoundTripNativeWords` | pass, 1736/1736 |
| `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 --no-restore -- --filter-class Humanizer.Tests.Localisation.LocaleRegistrySweepTests` | fail, 390 mismatches |
| `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 --no-restore -- --filter-class Humanizer.Tests.Localisation.LocaleRegistrySweepTests` | fail, 390 mismatches |

## Surface Status

| canonical surface | status | proof |
| --- | --- | --- |
| `list` | proved | `FormatterExactOutputTests.UsesExpectedCollectionHumanizeOutputs` plus completeness matrix |
| `formatter` | proved | `FormatterExactOutputTests` exact-output rows plus formatter registry sweep methods |
| `phrases.relativeDate` | proved | `LocaleRegistrySweepTests.Formatter_NativeLocales_UseExpectedRelativeDateAndDurationStrings` plus `ResourceBackedPhraseTests.UsesExpectedDateHumanizePhrases` |
| `phrases.duration` | proved | `LocaleRegistrySweepTests.Formatter_NativeLocales_UseExpectedRelativeDateAndDurationStrings` plus `ResourceBackedPhraseTests.UsesExpectedTimeSpanHumanizePhrases` |
| `phrases.dataUnits` | proved | `FormatterExactOutputTests.UsesExpectedByteSizeHumanizeSymbols` and `UsesExpectedByteSizeFullWords` |
| `phrases.timeUnits` | proved | `FormatterExactOutputTests.UsesExpectedTimeUnitSymbols` |
| `number.words.cardinal` | proved | `NumberWordPhraseTests.UsesExpectedCardinalCases` plus `UsesExpectedAdditionalCardinalCases` |
| `number.words.ordinal` | partial / excluded from verdict | shared ordinal rows exist, but this verdict excludes inflection-heavy ordinal/gender/word-form branches |
| `number.parse.cardinal` | proved | `LocaleRegistrySweepTests.WordsToNumber_NativeLocales_ParseNativeWords` plus `WordsToNumber_RegisteredLocales_RoundTripNativeWords` |
| `number.parse.ordinal` | partial / excluded from verdict | parser variants tied to inflectional forms are excluded from this verdict |
| `ordinal.numeric` | proved | `LocaleRegistrySweepTests.Ordinalizer_NativeLocales_UseExpectedForms` plus default and negative exact sweeps |
| `ordinal.date` | not proved | exact-output parity fails for 42 locales |
| `ordinal.dateOnly` | not proved | exact-output parity fails for 42 locales |
| `clock` | not proved | exact-output parity fails for 46 locales |
| `compass` | proved | `FormatterExactOutputTests.UsesExpectedLocalizedHeadingOutputs`, `ParsesExpectedLocalizedHeadingAbbreviations`, and `UsesExpectedLocalizedCardinalHeadingAbbreviations` |

## Failing Proof Surfaces

### `ordinal.date`

Exact-output theories fail for 42 locales on all three representative samples: `2022-01-25`, `2015-01-01`, and `2015-02-03`.

Failing locales:
`af`, `az`, `bg`, `bn`, `cs`, `da`, `el`, `fi`, `fil`, `he`, `hr`, `hu`, `hy`, `id`, `is`, `it`, `ko`, `ms`, `mt`, `nb`, `nl`, `nn`, `pl`, `pt`, `ro`, `ru`, `sk`, `sl`, `sr`, `sr-Latn`, `sv`, `ta`, `th`, `tr`, `uk`, `uz-Cyrl-UZ`, `uz-Latn-UZ`, `vi`, `zh-CN`, `zh-Hans`, `zh-Hant`, `zu-ZA`

Locales currently proved for this surface:
`ar`, `ca`, `de`, `de-CH`, `de-LI`, `en`, `en-GB`, `en-IN`, `en-US`, `es`, `fa`, `fr`, `fr-BE`, `fr-CH`, `ja`, `ku`, `lb`, `lt`, `lv`, `pt-BR`

### `ordinal.dateOnly`

Exact-output theories fail for the same 42 locales on the same three representative samples.

Failing locales:
`af`, `az`, `bg`, `bn`, `cs`, `da`, `el`, `fi`, `fil`, `he`, `hr`, `hu`, `hy`, `id`, `is`, `it`, `ko`, `ms`, `mt`, `nb`, `nl`, `nn`, `pl`, `pt`, `ro`, `ru`, `sk`, `sl`, `sr`, `sr-Latn`, `sv`, `ta`, `th`, `tr`, `uk`, `uz-Cyrl-UZ`, `uz-Latn-UZ`, `vi`, `zh-CN`, `zh-Hans`, `zh-Hant`, `zu-ZA`

Locales currently proved for this surface:
`ar`, `ca`, `de`, `de-CH`, `de-LI`, `en`, `en-GB`, `en-IN`, `en-US`, `es`, `fa`, `fr`, `fr-BE`, `fr-CH`, `ja`, `ku`, `lb`, `lt`, `lv`, `pt-BR`

### `clock`

Exact-output theories fail for 46 locales on all three representative samples: `13:23`, rounded `13:23`, and `01:05`.

Failing locales:
`af`, `ar`, `az`, `bg`, `bn`, `cs`, `da`, `el`, `fa`, `fi`, `fil`, `he`, `hr`, `hu`, `hy`, `id`, `is`, `it`, `ko`, `ku`, `lt`, `lv`, `ms`, `mt`, `nb`, `nl`, `nn`, `pl`, `ro`, `ru`, `sk`, `sl`, `sr`, `sr-Latn`, `sv`, `ta`, `th`, `tr`, `uk`, `uz-Cyrl-UZ`, `uz-Latn-UZ`, `vi`, `zh-CN`, `zh-Hans`, `zh-Hant`, `zu-ZA`

Locales currently proved for this surface:
`ca`, `de`, `de-CH`, `de-LI`, `en`, `en-GB`, `en-IN`, `en-US`, `es`, `fr`, `fr-BE`, `fr-CH`, `ja`, `lb`, `pt`, `pt-BR`

Clock-only gaps beyond the date/dateOnly set:
- `ar`
- `fa`
- `ku`
- `lt`
- `lv`

Date/dateOnly-only gap beyond the clock set:
- `pt`

## Effective Conclusion

The repo currently proves broad shared coverage and non-date/time translation parity, but it does **not** prove full non-inflectional translation parity across all shipped locales.

The blocking gaps are concentrated and explicit:
- 42 shipped locales still fail locale-owned `DateTime.ToOrdinalWords` exact-output parity.
- 42 shipped locales still fail locale-owned `DateOnly.ToOrdinalWords` exact-output parity.
- 46 shipped locales still fail locale-owned `TimeOnly.ToClockNotation` exact-output parity.

Until those three surfaces are closed, the answer to:

> Can every shipped localized feature for this locale now execute through intentional locale ownership or intentional same-language inheritance, with no English fallback, no unsupported-locale gaps, and passing parity tests?

is **no** at the repository level.
