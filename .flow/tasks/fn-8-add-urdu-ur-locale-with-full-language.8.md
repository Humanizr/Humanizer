# fn-8-add-urdu-ur-locale-with-full-language.8 Docs, release notes, and skill/memory updates

## Description

Update documentation for the three new shipped locale files (`ur`, `ur-PK`, `ur-IN`) and the schema extensions from `.9` / `.10`, add a release-notes entry, and write back durable lessons to `.agents/skills/add-locale/` and project memory. Verifies `.1`'s skill update for committed parity-map artifacts is present; adds the Regional Variant Checklist exception for no-delta variants; captures all learnings.

**Size:** S
**Files:**
- `CLAUDE.md` (edit: count + wording)
- `ARCHITECTURE.md` (edit: diagram comment)
- `docs/locale-yaml-reference.md` (edit: count + new engine-field sections from `.9` / `.10` + Regional Variant Checklist exception)
- `docs/adding-a-locale.md` (edit: count + any new reference notes)
- `docs/locale-yaml-how-to.md` (edit: count)
- `release_notes.md` (edit: new entry)
- `.agents/skills/add-locale/` (edit IF durable new reference notes — verify `.1`'s parity-map-commit update is present)
- `.flow/memory/pitfalls.md` / `conventions.md` (edit IF new learnings)
- `/Users/claire/.claude/projects/-Users-claire-dev-Humanizer/memory/` (edit IF durable user-relevant lesson)

## Approach

1. **Count update 62 → 65** in the 5 listed doc files. Three new YAML files ship (`ur`, `ur-PK`, `ur-IN`). After edits, `grep -n "62" CLAUDE.md ARCHITECTURE.md docs/locale-yaml-reference.md docs/adding-a-locale.md docs/locale-yaml-how-to.md` shows no remaining `62 locale` / `62 shipped` / `62 YAML` occurrences.
2. **Wording clarification** where the count previously said "locales" but mixed concepts: adopt "65 shipped locale files across {language-count} languages" if both metrics appear. Keep wording consistent across docs.
3. **New schema-field docs** — mandatory when `.9` / `.10` extended the schema:
   - Ordinalizer engine extension (gendered word-ordinal engine) in `docs/locale-yaml-reference.md` under the ordinal section, with worked examples.
   - Calendar surface extension (Hijri month table or calendar-keyed months map) in the calendar section, with examples showing Gregorian + Hijri outputs.
4. **Regional Variant Checklist exception** in `docs/locale-yaml-reference.md`: add an explicit note that epics may ship minimum-valid no-delta variant files (`locale:` + `variantOf:` + `surfaces: {}`) when first-class matrix/sweep coverage is required (e.g., to make regional cultures explicit in `LocaleRegistrySweepTests`).
5. **Verify `.1` parity-map-commit update landed** in `.agents/skills/add-locale/SKILL.md` (or `references/parity-artifacts.md`). If not present, add it here.
6. **`release_notes.md`** — read file style (line 58 as reference). Add entry announcing:
   - Urdu (`ur`) + `ur-PK` + `ur-IN`.
   - Islamic (Hijri) calendar coverage for Urdu dates.
   - Gendered word-ordinals (masc + fem, with neuter fallback) across all gender-bearing locales (from `.12` / `.13` / `.14`).
7. **Write back durable lessons**. User directive: "If you run into any issues, make sure you update any relevant docs/skills." Candidates:
   - Arabic-script letter subset differences (ہ/ی/ک vs ه/ي/ك) — add to `.agents/skills/add-locale/references/` if not already present.
   - CLDR `one/other` rule for Indo-Aryan languages vs `arabic-like` mistake.
   - South Asian lakh/crore — point future Indic locales at `en-IN.yml` + `ur.yml` as templates.
   - Word-ordinal engine gap (numeric-suffix engines produce `5واں` not `پانچواں`) — record so the next locale hits a faster decision.
   - Hijri contract decision (the `DateTime` doesn't carry `Calendar` gotcha) — add to skill references.
   - Parity-map + audit-artifact source-control convention (committed under `artifacts/`).
   - Regional-variant-file exception for matrix coverage.
8. **PR description credit** (for the PR body when opened; not a file edit here): credit community PR #1683 author (@iamahsanmehmood) for vocabulary baseline. NO reopen / rebase.
9. **Branch hygiene**: all commits on `feat/urdu-locale`. Do NOT open the PR from this task.
10. Final gate: re-run `LocaleRegistrySweepTests` + `LocaleTheoryMatrixCompletenessTests` to confirm doc edits didn't break anything.

## Investigation targets

**Required**:
- All doc files listed above
- `release_notes.md:58` for style reference
- `.agents/skills/add-locale/SKILL.md` + `references/parity-checklist.md`
- `.flow/memory/pitfalls.md` + `conventions.md`
- Parity map `.1` (confirm parity-map-commit update was landed there)

**Optional**:
- `/Users/claire/.claude/projects/-Users-claire-dev-Humanizer/memory/MEMORY.md`

## Key context

- Writing lessons back is **part of acceptance**, not cleanup.
- Count must reflect shipped YAML files (3 additions). Getting it wrong ships a false number.
- Do NOT touch `readme.md`, `docs/index.md`, or `docs/localization.md` — no count references there.

## Acceptance

- [ ] `CLAUDE.md` lines 3 and 43: `62` → `65`.
- [ ] `ARCHITECTURE.md` line 73: `62` → `65`.
- [ ] `docs/locale-yaml-reference.md` lines 355 and 855: `62` → `65`.
- [ ] `docs/adding-a-locale.md` line 331: `62` → `65`.
- [ ] `docs/locale-yaml-how-to.md` line 352: `62` → `65`.
- [ ] `grep -n "62" CLAUDE.md ARCHITECTURE.md docs/locale-yaml-reference.md docs/adding-a-locale.md docs/locale-yaml-how-to.md` returns no `62 locale` / `62 shipped` / `62 YAML` occurrences.
- [ ] `release_notes.md` has an entry announcing Urdu + variants + Hijri + cross-locale gendered ordinals.
- [ ] Schema extensions from `.9` / `.10` documented in `docs/locale-yaml-reference.md` with worked examples.
- [ ] **Regional Variant Checklist** updated in `docs/locale-yaml-reference.md` documenting the explicit exception: parity epics may ship minimum-valid no-delta variant files when first-class matrix/sweep coverage is required.
- [ ] `.agents/skills/add-locale/` reflects the committed parity-map convention from `.1` (verified in this task; added if missing).
- [ ] At least one write-back entry in `.agents/skills/add-locale/` OR `.flow/memory/pitfalls.md` / `conventions.md` captures a concrete learning (Arabic-script subset, word-ordinal engine gap, Hijri contract, parity-map-commit, variant-file exception). If no new learnings, record "no new pitfalls" in the parity-map post-mortem.
- [ ] Every commit in the epic on branch `feat/urdu-locale`.
- [ ] Final gate: `LocaleRegistrySweepTests` + `LocaleTheoryMatrixCompletenessTests` still pass on net10.0.

## Done summary
Updated locale file count from 62 to 65 across all 5 target doc files, documented the number-word-suffix ordinalizer engine and calendar.hijriMonths schema extensions in locale-yaml-reference.md with worked examples, added Regional Variant Checklist exception for no-delta variant files, added release notes entries for Urdu/Hijri/gendered-ordinals/gender-audit, and captured durable learnings in skill references and memory files.
## Evidence
- Commits: 1e6d068b, 32efd8d7, d810fa6a
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (40619 passed)
- PRs: