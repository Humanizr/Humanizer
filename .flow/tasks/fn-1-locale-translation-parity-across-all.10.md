# fn-1-locale-translation-parity-across-all.10 Update documentation for locale parity requirements

## Description
Update documentation to reflect the `phrase-clock` engine consolidation, removal of residual leaf converters, and full locale parity.

**Size:** M
**Files:**
- `docs/locale-yaml-reference.md` — HIGH PRIORITY: Replace `phrase-hour`/`relative-hour` engine sections + residual leaf descriptions with `phrase-clock` engine. Add all new YAML fields (hourMode, dayPeriods, minute buckets, applyEifelerRule, hourWordsMap, compactConjunction, compactMinuteWords, paucalLowOnly, minuteGender, etc.). Document the `{dayPeriod}` inline template placeholder (allows day-period placement without automatic prefix/suffix spacing). Update "Shared Strategy Values" with `hourMode` values. Document the new ordinal.date `calendarMode` YAML field (`Gregorian` default, `Native` for Thai Buddhist calendar) and the `OrdinalDateCalendarMode` enum added in task .8.
<!-- Updated by plan-sync: fn-1-locale-translation-parity-across-all.8 — added calendarMode YAML field and OrdinalDateCalendarMode enum to documentation scope -->
<!-- Updated by plan-sync: fn-1-locale-translation-parity-across-all.7 — added hourWordsMap, compactConjunction, {dayPeriod} placeholder to documentation scope -->
- `docs/locale-yaml-how-to.md` — HIGH PRIORITY: Replace clock engine list. Update "Choosing Between A Shared Engine And A New One" and "Feature-By-Feature Authoring Order" step 9.
- `docs/adding-a-locale.md` — MEDIUM: Update "When A Residual Locale Leaf Is Acceptable" (no clock leaves remain). Add registry completeness tests to "Testing Requirements" contributor checklist.
- `ARCHITECTURE.md` — MEDIUM: Remove references to clock residual leaves. Verify pipeline table still accurate.

## Approach

For each file:
1. Read current content
2. Identify sections referencing old engines (`phrase-hour`, `relative-hour`, `french`, `german`, `luxembourgish`, `japanese`)
3. Replace with `phrase-clock` documentation
4. Add new field descriptions
5. Verify cross-references between docs are consistent

## Investigation targets

**Required:**
- `docs/locale-yaml-reference.md:341+` and `:661-713` — clock surface sections
- `docs/locale-yaml-how-to.md:339-347` — clock engine list
- `docs/adding-a-locale.md` — residual leaf sections and testing requirements
- `ARCHITECTURE.md:48-88` — pipeline table and residual leaf language
- `src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs:678-723` — definitive phrase-clock field list (source of truth for docs)
- `src/Humanizer/Locales/ku.yml:372-382` — example of `{dayPeriod}` inline placeholder usage
- `src/Humanizer/Locales/ar.yml:540-566` — example of `hourWordsMap` + `compactConjunction` usage
- `src/Humanizer/Localisation/TimeToClockNotation/PhraseClockNotationConverter.cs:486-502` — `{dayPeriod}` placeholder semantics (skips auto-append)
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDateCalendarMode.cs` — new enum (`Gregorian` | `Native`) for ordinal.date calendarMode
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:96-105` — `GetPatternCulture()` Native mode (skips Gregorian override)
- `src/Humanizer/Locales/th.yml:356-360` — example of `calendarMode: 'Native'` usage (Thai Buddhist)
<!-- Updated by plan-sync: fn-1-locale-translation-parity-across-all.7 — added source references for new phrase-clock features -->
<!-- Updated by plan-sync: fn-1-locale-translation-parity-across-all.8 — added calendarMode/OrdinalDateCalendarMode investigation targets -->
## Approach

Each doc update should be minimal — reflect the new reality that all shipped locales have complete translations. Do not rewrite sections; update the specific claims that are now outdated.

Follow existing formatting conventions per file:
- CLAUDE.md/AGENTS.md: compact bullet-point lists
- CONTRIBUTING.md: flowing prose with `<a id>` anchors
- readme.md: Markdown with inline code
- docs/: numbered step-by-step lists and tables

## Investigation targets

**Required:**
- `CLAUDE.md:76-77` — current Localization section
- `AGENTS.md:42-44` — current Localization Guidance
- `.github/CONTRIBUTING.md:48-50` — current localisation section
- `readme.md:662` — fallback note
- `docs/locale-yaml-how-to.md` — parity rules and authoring guidance

**Optional:**
- `docs/adding-a-locale.md` — full parity workflow
- `docs/locale-yaml-reference.md` — field reference
- `docs/localization.md` — supported languages list
## Approach

Each doc update should be minimal — reflect the new reality that all shipped locales have complete translations. Do not rewrite sections; update the specific claims that are now outdated.

Follow existing formatting conventions per file:
- CLAUDE.md/AGENTS.md: compact bullet-point lists
- CONTRIBUTING.md: flowing prose with `<a id>` anchors
- readme.md: Markdown with inline code
- docs/: numbered step-by-step lists and tables

## Investigation targets

**Required:**
- `CLAUDE.md:76-77` — current Localization section
- `AGENTS.md:42-44` — current Localization Guidance
- `.github/CONTRIBUTING.md:48-50` — current localisation section
- `readme.md:662` — fallback note

**Optional:**
- `docs/adding-a-locale.md` — full parity workflow
- `docs/locale-yaml-reference.md` — field reference
- `docs/localization.md` — supported languages list
## Acceptance
- [ ] `docs/locale-yaml-reference.md` updated: `phrase-clock` engine documented with all fields, old engines removed
- [ ] `docs/locale-yaml-how-to.md` updated: clock engine list shows only `phrase-clock`
- [ ] `docs/adding-a-locale.md` updated: no clock residual leaves, registry tests in checklist
- [ ] `ARCHITECTURE.md` updated: no clock residual leaf references
- [ ] No broken cross-references between docs
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds (docs don't affect build, but verify)
## Done summary
Updated documentation for locale parity: replaced old clock engine sections (phrase-hour, relative-hour, residual leaves) with unified phrase-clock engine documentation in locale-yaml-reference.md, added all 34 YAML fields, calendarMode for ordinal.date, and new Shared Strategy Values sections. Updated locale-yaml-how-to.md clock engine list, adding-a-locale.md contributor checklist, and verified ARCHITECTURE.md was already clean.
## Evidence
- Commits: 36c17d362a57ced7f07e2bddf2a9e22f4ae17b66, 8f87a4cfbb6aba6b34dde5a4f94e8ddcec6d0a50, 679679a2772e0731539c32313e6a5add4f39023a
- Tests: dotnet build src/Humanizer/Humanizer.csproj -c Release
- PRs: