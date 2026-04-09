# fn-3-hard-code-locale-overrides-where-icu.2 Audit and decide correct locale values for overrides

## Description
Investigate and record the correct canonical values for each locale where ICU data is wrong or drifts. This task produces a **decision document only** — no code changes. The actual test expected-value updates are merged into the tasks that land the runtime override (.3 for calendar, .4 for decimal separator), so each runtime task is atomically landable with its matching test contract.

**Size:** S
**Files:**
- `.flow/tasks/fn-3-hard-code-locale-overrides-where-icu.2.decisions.md` — NEW: decision document recording chosen values and rationale

## Approach

Cross-reference the `tools/probe-*.json` files and the failure audit to identify values that were never correct for any native speaker. For each, record the correct canonical value with rationale.

**Category: Month names missing required grammatical features**
- `fa` (Persian) — month names missing ezafe mark (ٔ) in date context. `ژانویه` → `ژانویهٔ`, etc. Verify with CLDR formal writing guide.
- `bn` (Bengali) — modern short-i spellings (Bangla Academy standard) vs. ICU's forms
- `he` (Hebrew) — standalone month names or with ב prefix for embedded use
- `ku` (Kurdish) — Sorani (Arabic script) vs Kurmanji (Latin script) decision
- `zu-ZA` (Zulu) — verify correct Zulu month spellings
- `ta` (Tamil) — verify Tamil month names

**Category: Decimal separator overrides**
- `ar` — verify `.` is correct for modern Arabic numeral formatting
- `ku` — `,` or `٫` depending on Sorani vs Kurmanji decision
- `fr-CH` — `.` (Swiss French uses dot; France uses comma)

**Category: Test values wrong from day one (non-override)**
- `en-US` short time expected values — verify and document whether 12-hour format is correct (but note: en-US short-time tests are being deleted in task .1 as pure-ICU-snapshot tests)

## Investigation targets
**Required:**
- `tools/probe-macos.json`, `tools/probe-linux.json`, `tools/probe-windows-net10.json`, `tools/probe-windows-net48.json`
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs:36-100` — current expected values
- `tests/Humanizer.Tests/Localisation/LocaleFormatterExactTheoryData.cs:406-470` — `ByteSizeSymbolCases`

## Key context
- Goal: produce a decision document that .3 and .4 can reference for the exact values to use
- When in doubt between two acceptable forms, pick the one that's more stable in CLDR history (fewer recent changes)
- en-US short-time tests are being deleted in task .1 (they're pure ICU snapshots), so no expected-value fix is needed for those — just document the finding

## Acceptance
- [ ] Decision document created with chosen canonical values for all 6 calendar locales (bn, fa, he, ku, zu-ZA, ta)
- [ ] Decision document records chosen decimal separators for ar, ku, fr-CH with rationale
- [ ] `ku` script/dialect decision recorded (Sorani vs Kurmanji) with rationale
- [ ] Each decision references the CLDR version or native-speaker source used
- [ ] No code changes in this task (decisions only)
- [ ] Diff from HEAD shows only `.flow/tasks/` additions
## Done summary
Created decision document recording canonical locale override values for 4 calendar locales (bn long-i, fa with ezafe, he with bet-prefix, ku Sorani) and 3 decimal separators (ar=., ku=momayyiz, fr-CH=.), with CLDR-versioned sources and downstream action summaries. Flagged scope revisions (zu-ZA/ta no override needed) and bn spec contradiction.
## Evidence
- Commits: d5cc87b1, 29a942b9, bc0c0327, 721580a8, e086a25a
- Tests: no code changes -- decision document only
- PRs: