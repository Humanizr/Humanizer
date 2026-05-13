# fn-13-prepare-for-net-11-add-net110-tfm-audit.5 Sweep targeted .NET 11 breaking changes (Meiji era, 24:00 parsing, decimal NumberStyles, ur-IN NativeDigits)

## Description
Sweep the codebase for test fixtures and call sites that could be affected by the named .NET 11 / ICU 78 breaking changes, confirm each is either unaffected or patched.

**Size:** M
**Files:** none new; targeted grepping + small fixes

## Approach

Four independent audit passes:

1. **Japanese Meiji era (1868-09-08 → 1868-10-23)** — `src/Humanizer/Locales/ja.yml` has no era dates; no `tests/Humanizer.Tests/Localisation/ja*/` folder. Confirm no fixture in the repo uses a 1868-Sep/Oct date via `grep -r 1868`.
2. **ur-IN NativeDigits on Apple** — `NativeDigits` has zero callers in src; confirm fn-8's "Latin digits per CLDR defaultNumberingSystem" assertion still holds on Apple net11 (`.flow/tasks/fn-8-add-urdu-ur-locale-with-full-language.3.md:56`). Any Urdu test that asserts platform `ToString()` output gets a Humanizer-local override.
3. **ISO 8601 `"24:00"` parsing** — zero `"24:00"` fixtures exist. If fn-13.2 surfaces a new pass/fail where one failed before, record it.
4. **`decimal` NumberStyles tightening** — zero `decimal.Parse`/`TryParse` hits; `ByteSize` uses `double`. Confirm `ByteSize.Parse` tests still pass on net11.

## Investigation targets

**Required:**
- `src/Humanizer/Locales/ja.yml`
- `src/Humanizer/Locales/ur*.yml`
- `src/Humanizer/Bytes/ByteSize.cs:26,478`
- `.flow/tasks/fn-8-add-urdu-ur-locale-with-full-language.3.md:56`
## Acceptance
- [ ] Each of the four breaking-change categories audited and documented (affected / not affected)
- [ ] Any affected fixture or call site has a patch merged (or a follow-up task filed)
- [ ] fn-8's Latin-digit ur policy confirmed on Apple net11.0
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
