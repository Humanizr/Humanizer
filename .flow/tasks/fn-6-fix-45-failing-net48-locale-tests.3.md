# fn-6-fix-45-failing-net48-locale-tests.3 Add YAML overrides + wire ordinalizer to use override NFI (42 tests)

## Description
Add YAML `number.formatting` overrides to 9 locale files and wire the ordinalizer to use the formatting-specific `NumberFormatInfo` API so that all 42 previously-failing tests pass on net48.

**Size:** M
**Files:**
- `src/Humanizer/Locales/fi.yml` — negativeSign
- `src/Humanizer/Locales/hr.yml` — negativeSign
- `src/Humanizer/Locales/nb.yml` — negativeSign (nn inherits from nb via variantOf)
- `src/Humanizer/Locales/sv.yml` — negativeSign
- `src/Humanizer/Locales/sl.yml` — negativeSign
- `src/Humanizer/Locales/lt.yml` — negativeSign
- `src/Humanizer/Locales/fa.yml` — negativeSign
- `src/Humanizer/Locales/lb.yml` — groupSeparator
- `src/Humanizer/OrdinalizeExtensions.cs` — wire to use `GetFormattingNumberFormat`
- `src/Humanizer/Localisation/Ordinalizers/WordFormTemplateOrdinalizer.cs` — verify or wire override NFI

## Approach

### YAML overrides

Add `negativeSign: '−'` (literal U+2212 character) to the `number.formatting` section of 7 locale files (fi, hr, nb, sv, sl, lt, fa):

```yaml
surfaces:
  number:
    formatting:
      negativeSign: '−'
```

Add `groupSeparator: '.'` to lb.yml:

```yaml
surfaces:
  number:
    formatting:
      groupSeparator: '.'
```

**nn inheritance**: `nn.yml` uses `variantOf: 'nb'` and defines no `number` surface. The documented merge rules state that omitted surfaces inherit unchanged from the parent. Therefore `nb.yml` is the single source of truth — adding negativeSign to `nb.yml` propagates to `nn` automatically. A verification test must confirm this. Only modify `nn.yml` if inheritance is verified as broken.

### Ordinalizer wiring

`OrdinalizeExtensions.cs` has 4 call sites where `number.ToString(culture)` is called (lines 147, 164, 206, 226). Change these to use `LocaleNumberFormattingOverrides.GetFormattingNumberFormat(culture)` (the new formatting-only API from fn-6.2).

Pattern: replace `number.ToString(culture)` with `number.ToString(LocaleNumberFormattingOverrides.GetFormattingNumberFormat(culture))`.

### WordFormTemplateOrdinalizer verification (REQUIRED)

`WordFormTemplateOrdinalizer.cs` (lines 43-44) may format negative numbers via culture-aware `ToString(...)`. This MUST be investigated:
- If it formats via `ToString(culture)` independently of `OrdinalizeExtensions`, it needs the same override wiring using `GetFormattingNumberFormat`
- If all runtime paths go through `OrdinalizeExtensions` (which already has the fix), then document why no change is needed
- The acceptance criteria require one of these outcomes to be recorded

### Source generator snapshots

Adding real YAML data to locale files populates the `NegativeSignOverrides` and `GroupSeparatorOverrides` dictionaries in generated code. This changes the generated output, so source generator snapshot tests may need updating in this task (in addition to fn-6.2 which handles the shape changes).

## Investigation targets

**Required** (read before coding):
- `src/Humanizer/OrdinalizeExtensions.cs:146-147,163-164,205-206,225-226` — all ToString(culture) call sites
- `src/Humanizer/Localisation/Ordinalizers/WordFormTemplateOrdinalizer.cs:40-47` — NegativeNumberMode handling, verify whether it formats independently or delegates to OrdinalizeExtensions
- `src/Humanizer/Locales/nn.yml` — confirm variantOf and absence of number: surface
- `src/Humanizer/Locales/nb.yml` — parent locale for nn

**Optional** (reference as needed):
- `src/Humanizer/Locales/fr-CH.yml:32-33` — existing number.formatting override pattern
- `tests/Humanizer.Tests/Localisation/LocaleOrdinalizerMatrixData.cs:3740-3997` — test expectations
- `tests/Humanizer.Tests/Localisation/LocaleAdditionalByteTheoryData.cs:507-508` — lb-LU expectations

## Key context

- The negativeSign YAML value must be the literal U+2212 character (MINUS SIGN), not the ASCII U+002D (HYPHEN-MINUS). In YAML single quotes, this is the UTF-8 encoded character directly in the file (3 bytes: 0xE2 0x88 0x92).
- CLDR specifies U+2212 for all 8 affected locales
- CLDR specifies period (.) as the group separator for lb-LU (standard Continental European pattern)
- fa.yml already has a `calendar:` section — the negativeSign goes in a separate `number.formatting:` section
- Use `GetFormattingNumberFormat` (formatting-only API from fn-6.2), NOT the existing `GetCachedNumberFormat` which is for parse paths

## Acceptance

- [ ] All 40 ordinalizer negative number tests pass on net48 (fi, hr, nn, sv, sl, nb, lt, fa × 5 values each)
- [ ] Both lb-LU byte formatting tests pass on net48
- [ ] Zero regressions on net10.0 and net8.0
- [ ] nn locale verified to inherit negativeSign from nb (or explicitly overridden if inheritance broken)
- [ ] All affected YAML files contain correct overrides
- [ ] OrdinalizeExtensions uses `GetFormattingNumberFormat` for all negative number formatting
- [ ] WordFormTemplateOrdinalizer verified: either updated to use `GetFormattingNumberFormat`, or documented as not needing changes (with explanation)
- [ ] Source generator snapshot tests updated if adding YAML data changed generated output
- [ ] Existing parse tests pass without modification (confirms no parse behavior changes)
- [ ] Build produces zero warnings
## Done summary
Added negativeSign (U+2212) YAML overrides to fi, hr, nb, sv, sl, lt, fa locales and groupSeparator ('.') override to lb locale. Wired all 4 culture-aware OrdinalizeExtensions call sites to use GetFormattingNumberFormat for cross-platform formatting consistency. WordFormTemplateOrdinalizer verified as not needing changes since it receives pre-formatted strings from OrdinalizeExtensions.
## Evidence
- Commits:
- Tests:
- PRs:
