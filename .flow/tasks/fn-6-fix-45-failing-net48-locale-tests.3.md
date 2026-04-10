# fn-6-fix-45-failing-net48-locale-tests.3 Add YAML overrides + wire ordinalizer to use override NFI (42 tests)

## Description
Add YAML `number.formatting` overrides to 9 locale files and wire the ordinalizer to use the override `NumberFormatInfo` so that all 42 previously-failing tests pass on net48.

**Size:** M
**Files:**
- `src/Humanizer/Locales/fi.yml` — negativeSign
- `src/Humanizer/Locales/hr.yml` — negativeSign
- `src/Humanizer/Locales/nb.yml` — negativeSign
- `src/Humanizer/Locales/nn.yml` — negativeSign (only if inheritance from nb doesn't propagate; check first)
- `src/Humanizer/Locales/sv.yml` — negativeSign
- `src/Humanizer/Locales/sl.yml` — negativeSign
- `src/Humanizer/Locales/lt.yml` — negativeSign
- `src/Humanizer/Locales/fa.yml` — negativeSign
- `src/Humanizer/Locales/lb.yml` — groupSeparator
- `src/Humanizer/OrdinalizeExtensions.cs` — wire to use override NFI

## Approach

### YAML overrides

Add `negativeSign: '−'` (literal U+2212 character) to the `number.formatting` section of 8 locale files (fi, hr, nb, sv, sl, lt, fa, and potentially nn):

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

**nn inheritance check**: `nn.yml` uses `variantOf: 'nb'`. Before adding negativeSign to nn.yml, verify whether the source generator's YAML inheritance mechanism propagates `number.formatting` fields from parent (nb) to child (nn). If it does, skip nn.yml. If not, add the override to nn.yml too.

### Ordinalizer wiring

`OrdinalizeExtensions.cs` has 4 call sites where `number.ToString(culture)` is called (lines 147, 164, 206, 226). Change these to use the override `NumberFormatInfo` from `LocaleNumberFormattingOverrides.GetCachedNumberFormat(culture)`.

Pattern: replace `number.ToString(culture)` with `number.ToString(LocaleNumberFormattingOverrides.GetCachedNumberFormat(culture))` (or equivalent based on the Task 2 API shape).

Also check `WordFormTemplateOrdinalizer.cs` lines 43-44 for similar `ToString(culture)` calls that may need the same treatment.

## Investigation targets

**Required** (read before coding):
- `src/Humanizer/OrdinalizeExtensions.cs:146-147,163-164,205-206,225-226` — all ToString(culture) call sites
- `src/Humanizer/Localisation/Ordinalizers/WordFormTemplateOrdinalizer.cs:40-47` — NegativeNumberMode handling
- `src/Humanizer/Locales/nn.yml` — check variantOf and whether it has a number: surface
- `src/Humanizer/Locales/nb.yml` — parent locale for nn

**Optional** (reference as needed):
- `src/Humanizer/Locales/fr-CH.yml:32-33` — existing number.formatting override pattern
- `tests/Humanizer.Tests/Localisation/LocaleOrdinalizerMatrixData.cs:3740-3997` — test expectations
- `tests/Humanizer.Tests/Localisation/LocaleAdditionalByteTheoryData.cs:507-508` — lb-LU expectations

## Key context

- The negativeSign YAML value must be the literal U+2212 character (MINUS SIGN), not the ASCII U+002D (HYPHEN-MINUS). In YAML single quotes, this is the UTF-8 encoded character directly in the file (3 bytes: 0xE2 0x88 0x92).
- CLDR specifies U+2212 for all 8 affected locales: this is the typographically correct minus sign for Finnish, Swedish, Norwegian (Bokmål + Nynorsk), Croatian, Slovenian, Lithuanian, and Persian
- CLDR specifies period (.) as the group separator for lb-LU (standard Continental European pattern)
- fa.yml already has a `calendar:` section — the negativeSign goes in a separate `number.formatting:` section

## Acceptance

- [ ] All 40 ordinalizer negative number tests pass on net48 (fi, hr, nn, sv, sl, nb, lt, fa × 5 values each)
- [ ] Both lb-LU byte formatting tests pass on net48
- [ ] Zero regressions on net10.0 and net8.0
- [ ] nn locale inherits or explicitly has negativeSign override
- [ ] All affected YAML files contain correct overrides
- [ ] OrdinalizeExtensions uses override NFI for all negative number formatting
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
