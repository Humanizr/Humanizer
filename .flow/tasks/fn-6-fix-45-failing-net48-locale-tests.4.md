# fn-6-fix-45-failing-net48-locale-tests.4 Update documentation for new number.formatting fields

## Description
Update project documentation to reflect the newly implemented `negativeSign` and `groupSeparator` fields, removing them from the "future fields" list and adding proper documentation.

**Size:** S
**Files:**
- `docs/locale-yaml-reference.md`
- `docs/locale-yaml-how-to.md`
- `docs/adding-a-locale.md`
- `ARCHITECTURE.md` (if it describes LocaleNumberFormattingOverrides scope)

## Approach

### locale-yaml-reference.md

1. **Line 454**: Remove `negativeSign` and `groupSeparator` from the "Future fields" list. Update to: "Future fields: `digitSubstitution`, `percentSymbol`. These are reserved but not yet implemented."

2. **Line 437 area**: Add field documentation for both new fields, following the exact pattern used for `decimalSeparator`:
   - Purpose bullet
   - Fields list with name, type, and description
   - Notes: behavior on empty/absent, inheritance semantics, runtime registry behavior
   - Minimal YAML example

### locale-yaml-how-to.md

1. **"Override ICU-Supplied Data" recipe (line 452 area)**: Add two new sub-recipes:
   - "Negative sign override" — when to use (ICU U+2212 vs NLS U+002D for Nordic/European locales), minimal YAML
   - "Group separator override" — when to use (NLS vs ICU thousands separator), minimal YAML

### adding-a-locale.md

1. **Contributor Checklist (line 422)**: Update "decimal separators" to "decimal separators, negative signs, group separators"
2. **Canonical Surface Responsibilities table (line 143)**: Extend `number.formatting` description to cover all three fields

### ARCHITECTURE.md

1. If it describes `LocaleNumberFormattingOverrides.g.cs` as owning "per-locale decimal-separator overrides", update to include negative sign and group separator overrides

## Investigation targets

**Required** (read before coding):
- `docs/locale-yaml-reference.md:427-454` — current number.formatting section
- `docs/locale-yaml-how-to.md:452-487` — current ICU override recipe
- `docs/adding-a-locale.md:141-144,422` — surface responsibilities table + checklist

**Optional** (reference as needed):
- `ARCHITECTURE.md` — check if it references LocaleNumberFormattingOverrides scope

## Acceptance

- [ ] `negativeSign` and `groupSeparator` no longer listed as "future fields"
- [ ] Both fields have complete documentation entries matching `decimalSeparator` style
- [ ] locale-yaml-how-to.md has new sub-recipes for negative sign and group separator overrides
- [ ] adding-a-locale.md checklist and surface table updated
- [ ] No broken markdown links or formatting issues
## Done summary
Documented negativeSign and groupSeparator as implemented number.formatting fields across locale-yaml-reference.md, locale-yaml-how-to.md, adding-a-locale.md, localization.md, and ARCHITECTURE.md. Removed both from "future fields" list, added complete field documentation matching decimalSeparator style, added override recipes, and updated contributor checklist and surface responsibilities table.
## Evidence
- Commits: cc750b6f, 17c72d22, edae073e, 2c7b3f69
- Tests: documentation-only: no test commands required
- PRs: