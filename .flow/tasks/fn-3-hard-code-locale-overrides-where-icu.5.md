# fn-3-hard-code-locale-overrides-where-icu.5 Document override pattern and update contributor checklist

## Description
Document the new `calendar:` surface and `number.formatting:` sub-block in the locale authoring docs so contributors know to use them when ICU data for their locale is wrong or drifts. Also fold in the stale-documentation fixes from the superseded fn-2 epic: correct the `localization.md` Supported Languages list (missing 16 locales, 3 wrong codes), remove stale "residual clock leaves" claims from `localization.md` and `locale-yaml-reference.md`, and update the `CLAUDE.md` locale count.

**Size:** M (combined schema docs + fn-2 doc fixes)
**Files:**
- `docs/locale-yaml-reference.md` — add sections for `calendar:` surface and `number.formatting:` sub-block; update File-Level Rules surface list; fix stale "residual locale leaves" language (~line 1650)
- `docs/adding-a-locale.md` — update all canonical surface inventories (What Goes In, Contract, Responsibilities, Skeleton); contributor checklist bullet
- `docs/locale-yaml-how-to.md` — update supported surfaces list, authoring skeleton, and Top-Level Block Guide; preflight question and recipe
- `docs/localization.md` — update Principles #4-5 surface/member lists; rebuild Supported Languages list (fn-2 R1); remove stale residual-leaves claims (fn-2 R2); add short "Override ICU Where Needed" subsection
- `CLAUDE.md` — fix "60+ locales" → "62 locales" (fn-2 R4), optionally add short pointer to override pattern

## Approach

### Canonical surface inventory updates (all four docs)

Every place the docs enumerate the canonical surface set or nested member set must be updated to include `calendar` and `number.formatting`. The complete list of locations:

**`docs/localization.md`**:
- Line ~70 Principle #4: Add `calendar` to the `surfaces` enumeration (currently 7, becomes 8)
- Line ~71 Principle #5: Add `number.formatting`, `calendar.months`, `calendar.monthsGenitive` to canonical nested members

**`docs/locale-yaml-reference.md`**:
- Lines ~25-32 File-Level Rules #4: Add `calendar` to canonical surface names
- Line ~8 Rule #8: Add `number.formatting` alongside `number.words` and `number.parse`

**`docs/adding-a-locale.md`**:
- Line ~27: Add `number.formatting`, `calendar.months`, `calendar.monthsGenitive` to nested member examples
- Lines ~68-76 "What Goes In": Add `calendar` to supported canonical surfaces
- Lines ~92-100 "Canonical Locale Contract" surface list: Add `calendar`
- Lines ~102-108 "Canonical Locale Contract" nested members: Add `number.formatting`, `calendar.months`, `calendar.monthsGenitive`
- Line ~121 "Canonical Surface Responsibilities": Add `calendar` row describing month-name and genitive ownership
- Line ~181 "Canonical Locale Skeleton": Add `calendar:` section and `number.formatting:` under `number:`

**`docs/locale-yaml-how-to.md`**:
- Lines ~69-76 "Supported `surfaces` members": Add `calendar`
- Line ~81 "Canonical Authoring Skeleton": Add `calendar:` section and `number.formatting:` under `number:`
- Lines ~203+ "Top-Level Block Guide": Add `### calendar` and `### number.formatting` blocks describing when/what to author

### Schema documentation (new reference sections)

**`docs/locale-yaml-reference.md`** — Add a new top-level "Calendar Surface" section documenting:
- The `calendar:` canonical surface and its purpose
- `calendar.months` — type (array of 12 strings), semantics, example YAML, what happens when absent (inherits from CultureInfo)
- `calendar.monthsGenitive` — type, when to use, example
- Future fields placeholder section noting what may be added (`monthsAbbreviated`, `days`, etc.)

Add a new sub-section under `Number Surface` for `number.formatting:`:
- Relationship to `number.words` (output as words) and `number.parse` (input)
- `number.formatting.decimalSeparator` — type, semantics, example, fallback behavior
- Future fields placeholder

### Contributor checklist and recipe additions

**`docs/adding-a-locale.md`** — Add to the existing "Contributor Checklist" (lines 371-384):
> Verify that `date.ToOrdinalWords()` and `ByteSize` output for your locale is byte-identical on macOS, Linux, and Windows. If ICU-supplied data (month names, decimal separators) disagrees across platforms or is incorrect for your locale, author explicit overrides in `calendar:` and/or `number.formatting:` rather than relying on `CultureInfo`. Use `tools/locale-probe.cs` to capture platform values for comparison.

**`docs/locale-yaml-how-to.md`** — Add to "Before You Create A Locale File":
> Does your locale need month names, day names, or decimal separators that differ from what `CultureInfo.DateTimeFormat` / `NumberFormatInfo` returns on the user's platform? If yes, author them in `calendar:` or `number.formatting:` so output is stable across .NET globalization modes and operating systems.

Add a short "Override ICU-Supplied Data" recipe showing the minimum YAML for a month-name override and a decimal-separator override.

### fn-2 documentation fixes (folded in)

**`docs/localization.md`**:
- Rebuild the Supported Languages list (line 11) to enumerate all 62 shipped locales with correct codes matching YAML filenames. Add: af, ca, de-CH, de-LI, en, en-GB, en-IN, en-US, fil, fr-BE, fr-CH, lb, lt, nn, ta, zu-ZA. Correct: `bn-BD` → `bn`, `ms-MY` → `ms`, `nb-NO` → `nb`.
- Remove these specific stale residual-leaves claims:
  - Line ~133: "handwritten residual leaves" in generator registration description
  - Line ~141: "accepted residual leaves" in C# runtime code description
  - Lines ~155-157: "Residual locale names are acceptable..." paragraph and "Current accepted residual leaves are limited to a small set of `TimeOnlyToClockNotation` converters..." paragraph (both fully stale — no clock residual leaves remain)
- Add a short new subsection "Override ICU Where Needed" (2-3 paragraphs) explaining the philosophy and pointing to `calendar:` / `number.formatting:` as the escape hatch.

**`docs/locale-yaml-reference.md`**:
- Remove/rewrite line ~1650: "shared runtime kernels and the small number of accepted residual locale leaves" — no residual leaves remain for any surface.

**`CLAUDE.md`**:
- Line 3: "across 60+ locales" → "across 62 locales"
- Optionally add a short bullet under the Localization section pointing at the new override fields

### Residual-leaves grep verification
After all edits, grep for `residual` across docs. The following conceptual mentions should remain (they describe the general policy, not stale state claims):
- `docs/adding-a-locale.md:281-292` — "When A Residual Locale Leaf Is Acceptable" section
- `docs/locale-yaml-how-to.md:5,39,371` — general authoring guidance
- `docs/locale-yaml-reference.md:353,678,756` — historical context in clock surface docs (already accurate)

All other `residual` mentions must be removed or rewritten.

## Investigation targets
**Required:**
- `docs/locale-yaml-reference.md:17-36` — File-Level Rules (surface list to update)
- `docs/locale-yaml-reference.md:139+` — Canonical Surface Sections (style reference for new sections)
- `docs/adding-a-locale.md:27,58-82,84-117,121-140,181+` — all canonical inventories to update
- `docs/locale-yaml-how-to.md:69-81,203+` — supported surfaces, skeleton, Top-Level Block Guide
- `docs/localization.md:11,70-71,133-157` — Supported Languages, Principles #4-5, stale claims
- `src/Humanizer/Locales/` — authoritative list of 62 YAML files
- `CLAUDE.md:3` — locale count line

**Optional:**
- `docs/extensibility.md` — cross-link if there's a natural place

## Key context
- Follow the existing doc style: short prose, field tables, concrete YAML examples. No philosophy essays.
- The "Override ICU Where Needed" subsection in `localization.md` should be 2-3 paragraphs max
- The Supported Languages list rebuild is mechanical — derive from `ls src/Humanizer/Locales/*.yml`
- After this task lands, the superseded fn-2 epic can stay closed
- Do not document `calendar:` / `number.formatting:` in `docs/extensibility.md` — that's about `Configurator.*.Register`
## Acceptance

### Canonical surface inventory consistency
- [ ] `docs/localization.md` Principle #4 (line ~70) lists 8 canonical surfaces including `calendar`
- [ ] `docs/localization.md` Principle #5 (line ~71) lists canonical nested members including `number.formatting`, `calendar.months`, `calendar.monthsGenitive`
- [ ] `docs/locale-yaml-reference.md` File-Level Rules #4 (lines ~25-32) lists 8 canonical surfaces including `calendar`
- [ ] `docs/adding-a-locale.md` "What Goes In" surface list (lines ~68-76) includes `calendar`
- [ ] `docs/adding-a-locale.md` "Canonical Locale Contract" surface list (lines ~92-100) includes `calendar`
- [ ] `docs/adding-a-locale.md` "Canonical Locale Contract" nested members (lines ~102-108) includes `number.formatting`, `calendar.months`, `calendar.monthsGenitive`
- [ ] `docs/adding-a-locale.md` "Canonical Surface Responsibilities" (line ~121) has a `calendar` row
- [ ] `docs/adding-a-locale.md` "Canonical Locale Skeleton" (line ~181) includes `calendar:` and `number.formatting:`
- [ ] `docs/locale-yaml-how-to.md` "Supported `surfaces` members" (lines ~69-76) includes `calendar`
- [ ] `docs/locale-yaml-how-to.md` "Canonical Authoring Skeleton" (line ~81) includes `calendar:` and `number.formatting:`
- [ ] `docs/locale-yaml-how-to.md` "Top-Level Block Guide" (line ~203) has `### calendar` and `### number.formatting` blocks

### New reference documentation
- [ ] `docs/locale-yaml-reference.md` documents the new `calendar:` surface with `months` and `monthsGenitive`
- [ ] `docs/locale-yaml-reference.md` documents `number.formatting:` sub-block with `decimalSeparator`

### Contributor workflow updates
- [ ] `docs/adding-a-locale.md` contributor checklist has a new cross-platform verification bullet pointing at `calendar:` / `number.formatting:` overrides
- [ ] `docs/locale-yaml-how-to.md` has a preflight question and an "Override ICU-Supplied Data" recipe
- [ ] `docs/localization.md` has a short "Override ICU Where Needed" subsection

### fn-2 rollup (stale-doc fixes)
- [ ] `docs/localization.md` Supported Languages list enumerates all 62 shipped locales with correct codes
- [ ] `docs/localization.md` stale residual-leaves claims removed from lines ~133, ~141, ~155-157
- [ ] `docs/locale-yaml-reference.md` stale "residual locale leaves" language removed at line ~1650
- [ ] `CLAUDE.md` says "62 locales" instead of "60+ locales"

### Quality
- [ ] All doc changes match existing style (tables, short prose, concrete examples)
- [ ] Grep for `residual` across docs confirms only the allowlisted conceptual mentions remain (adding-a-locale.md:281-292, locale-yaml-how-to.md:5/39/371, locale-yaml-reference.md:353/678/756)
## Done summary
Documented the calendar: surface and number.formatting: sub-block across all four docs files. Updated canonical surface inventories in localization.md, locale-yaml-reference.md, adding-a-locale.md, and locale-yaml-how-to.md. Added new reference sections with field tables and YAML examples. Added contributor checklist bullet for cross-platform verification, preflight question, and Override ICU-Supplied Data recipe. Rebuilt Supported Languages list with all 62 locales and correct codes. Removed stale residual-leaves claims. Fixed CLAUDE.md locale count to 62.
## Evidence
- Commits: a2e86e41
- Tests: grep verification of residual mentions
- PRs: