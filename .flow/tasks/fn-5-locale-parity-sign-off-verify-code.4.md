## Description

Refresh the repo-local skill that guides agents through adding a new locale to Humanizer. The skill's surface inventory is stale — it lists 10 flat pre-fn-3 surface names and omits the `number.formatting` sub-block and the `calendar` surface entirely. The skill paradoxically cites `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` as source-of-truth while failing its own stop condition (the checklist says "the parity map does not match the canonical surface list in `CanonicalLocaleAuthoring.cs` — STOP" at `references/parity-checklist.md:239`, and the checklist itself currently violates this).

**Authoritative surface model (from `CanonicalLocaleAuthoring.cs:44-54` and `:198-289`):**
- 8 canonical top-level surfaces: `list`, `formatter`, `phrases`, `number`, `ordinal`, `clock`, `compass`, `calendar`
- Nested members under `number`: `words`, `parse`, `formatting`
- Nested members under `ordinal`: `numeric`, `date`, `dateOnly`
- Nested members under `calendar`: `months`, `monthsGenitive` (exactly 12 entries if present)

**Files to update:**

1. **`.agents/skills/add-locale/SKILL.md`**:
   - Lines ~63-74 "Use the canonical surfaces:" list — replace the 10-flat-name list with the 8-canonical + nested-members structure. Mirror the phrasing in `docs/localization.md:70-71` for consistency.
   - Lines ~101-119 "Required proof subrows" — add rows for `calendar.months`, `calendar.monthsGenitive`, `number.formatting.decimalSeparator`. These rows should be marked "not applicable" / "inherited from parent" when the locale does not author an override — most locales will not need these overrides.

2. **`.agents/skills/add-locale/references/parity-checklist.md`**:
   - Lines ~9-28 "Surface Inventory" — same update as SKILL.md's inventory list.
   - Lines ~98-109 "Required proof subrows" — same update as SKILL.md's proof rows.
   - Lines ~144-155 "Surface-to-files matrix" — add rows for `calendar` and `number.formatting` pointing at their respective source-generator inputs. Use the **correct paths**: `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` (for `calendar`) and `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` (for `number.formatting`). Note: these files are under `Generators/` (and `Generators/ProfileCatalogs/` for the date profile), not `Common/` — the historic path reference was stale.
   - Line ~239 stop-condition text — keep as-is, but verify after your updates that the checklist now satisfies its own stop condition.

**Out of scope:** the skill's agent frontmatter (`agents/openai.yaml`), unrelated skill prose, or any restructuring of the skill beyond the surface inventory updates. Keep edits surgical.

**Size:** M (2 files, 6 specific line ranges)
**Files:**
- `.agents/skills/add-locale/SKILL.md`
- `.agents/skills/add-locale/references/parity-checklist.md`

## Investigation targets

**Required:**
- `.agents/skills/add-locale/SKILL.md` (full file) — to see the current state of the two sections and understand surrounding context
- `.agents/skills/add-locale/references/parity-checklist.md` (full file) — same, plus the surface-to-files matrix structure
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:44-289` — authoritative surface model (this path IS correct; `CanonicalLocaleAuthoring.cs` genuinely lives under `Common/`)
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` — authoritative path for the `number.formatting` generator-input citation (historic `Common/LocaleRegistryInput.cs` reference was stale)
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` — authoritative path for the `calendar` generator-input citation (historic `Common/OrdinalDateProfileCatalogInput.cs` reference was stale)
- `docs/localization.md:70-71` — authoritative doc phrasing to mirror

**Optional:**
- `docs/adding-a-locale.md:68-145` — reference for how the user-facing doc already lists surfaces (for consistency check)
- `docs/locale-yaml-reference.md:25-55` — file-level rules, helpful if the skill needs a pointer

## Approach

- Preserve the existing skill structure and tone — do not rewrite unrelated sections.
- Mirror `docs/localization.md:70-71` phrasing exactly for the inventory list, so the three surface enumerations in the repo (localization.md, SKILL.md, parity-checklist.md) are byte-identical (modulo local formatting).
- For the "required proof subrows" list, clarify that `calendar.*` and `number.formatting.*` rows are only required when the locale authors an override — otherwise the row is marked "inherited from parent via `variantOf`" or "not applicable".
- For the surface-to-files matrix, cite the correct paths:
  - `calendar` → `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs`
  - `number.formatting` → `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`
  - Do NOT cite the stale `Common/` paths.
- After updating, grep the skill directory for "residual leaf" and other stale engine names — remove any remaining references (there should be none per the pre-plan audit, but confirm).

## Key context

- The skill's `CanonicalLocaleAuthoring.cs` citation at `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` remains correct after the update — do not change the source-of-truth reference. The stale paths were for `LocaleRegistryInput.cs` and `OrdinalDateProfileCatalogInput.cs` only.
- `.codex/agents/*.toml` does not mirror this skill and does not need updates per the pre-plan audit.
- Do NOT add a "how to author a calendar override" tutorial to the skill — that content lives in `docs/adding-a-locale.md:234-263` and `docs/locale-yaml-how-to.md:374-425`. The skill should just point at those docs for authoring, and use the inventory list for verification.

## Acceptance

- [ ] `.agents/skills/add-locale/SKILL.md` surface inventory (around lines 63-74) matches `docs/localization.md:70-71`: 8 canonical surfaces + nested members including `number.formatting`, `calendar.months`, `calendar.monthsGenitive`
- [ ] `.agents/skills/add-locale/SKILL.md` proof-subrow list (around lines 101-119) includes rows for `calendar.months`, `calendar.monthsGenitive`, `number.formatting.decimalSeparator`, marked as optional / inherited when the locale doesn't override
- [ ] `.agents/skills/add-locale/references/parity-checklist.md` surface inventory (around lines 9-28) matches the authoritative list
- [ ] `.agents/skills/add-locale/references/parity-checklist.md` proof-subrow list (around lines 98-109) matches SKILL.md
- [ ] `.agents/skills/add-locale/references/parity-checklist.md` surface-to-files matrix (around lines 144-155) has rows for `calendar` (pointing at `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs`) and `number.formatting` (pointing at `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`)
- [ ] Any other mentions of `LocaleRegistryInput.cs` or `OrdinalDateProfileCatalogInput.cs` in the skill use the correct `Generators/` paths (not `Common/`)
- [ ] The skill's own stop condition at `parity-checklist.md:~239` is no longer violated — running `diff` between the skill's inventory list and `CanonicalLocaleAuthoring.cs:44-54` + nested-members enumeration shows no surface missing from the skill
- [ ] `grep -rn "residual\|phrase-hour\|relative-hour" .agents/skills/add-locale/` returns only conceptual mentions with surrounding context (e.g., "the old residual leaf engines were removed in fn-1" would be acceptable; bare "residual leaves" claims as a current-state description are not)
- [ ] No files outside `.agents/skills/add-locale/` modified

## Done summary
Refreshed the repo-local add-locale skill to enumerate all 8 canonical surfaces (list, formatter, phrases, number, ordinal, clock, compass, calendar) plus nested members (including number.formatting, calendar.months, calendar.monthsGenitive). Added missing proof subrows for calendar and number.formatting overrides, and added calendar/number.formatting rows to the surface-to-files matrix with correct generator-input paths under Generators/ (not stale Common/ paths).
## Evidence
- Commits: f1d523814699060007779886cd86dfafa59201ba
- Tests: grep -rn residual|phrase-hour|relative-hour .agents/skills/add-locale/, grep -rn Common/LocaleRegistryInput|Common/OrdinalDateProfileCatalogInput .agents/skills/add-locale/, diff verification: skill surface inventory matches CanonicalLocaleAuthoring.cs:44-54
- PRs: