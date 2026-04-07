# fn-1-locale-translation-parity-across-all.10 Update documentation for locale parity requirements

## Description
Update documentation to reflect the `phrase-clock` engine consolidation, removal of residual leaf converters, and full locale parity.

**Size:** M
**Files:**
- `docs/locale-yaml-reference.md` — HIGH PRIORITY: Replace `phrase-hour`/`relative-hour` engine sections + residual leaf descriptions with `phrase-clock` engine. Add all new YAML fields (hourMode, dayPeriods, minute buckets, applyEifelerRule, etc.). Update "Shared Strategy Values" with `hourMode` values.
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
TBD

## Evidence
- Commits:
- Tests:
- PRs:
