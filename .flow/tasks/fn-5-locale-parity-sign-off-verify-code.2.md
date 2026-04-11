## Description

Fix blocking doc drift in the two agent-targeted files at the repo root: `CLAUDE.md` and `AGENTS.md`. These files instruct agents and contributors on how to make changes, and a stale instruction here actively produces wrong code. The plan-review pass surfaced that CLAUDE.md has a second stale net48 site that was missed in the initial audit — both files must be fixed consistently.

**Four concrete issues:**

1. **`CLAUDE.md:76`** — current text: "To add a locale: duplicate a YAML file, translate, register in formatter/converter registries". This is false for the post-fn-1 state. The source generator wires all registries automatically: `LocaleRegistryInput` (see `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs`) emits the master registry wiring; `FormatterRegistry.cs` reads generated profiles via `FormatterRegistryRegistrations.Register(this)`. A contributor who literally registers a locale manually will double-register and fail or confuse the generator.

2. **`AGENTS.md:45`** — parallel stale instruction: "Register new formatters/converters in the appropriate registries (see `Configuration/FormatterRegistry.cs` and number converter factories)". Same fix — remove the manual-registration step, point to `docs/adding-a-locale.md` as authoritative.

3. **`CLAUDE.md:17`** — current text: `# Run tests (use net10.0 or net8.0; avoid net48 on macOS/Linux)`. This frames net48 as platform-conditional, which is wrong. The real blocker is `Enum.GetValues<GrammaticalGender>()` in `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs` (circa line 439), which is a .NET 5+ API not available on .NET Framework 4.8. net48 test execution is therefore blocked on **all** platforms, not just macOS/Linux. Tracked as fn-4. Reframe the comment to name the real blocker and reference fn-4.

4. **`AGENTS.md:29`** — current text: "Avoid invoking the net48 target on Linux, and allow a few minutes for each run to complete". Same issue as #3, worded slightly differently. Same fix: reframe to reference `Enum.GetValues<T>()` + fn-4, drop the Linux-specific framing.

**Positive additions (both files):** Add one sentence noting that when ICU data is wrong or drifts across platforms for a locale, the fix is to author a `calendar:` override (month names) or `number.formatting:` override (decimal separator) in that locale's YAML — not to change `CultureInfo` itself. Keep it brief; `docs/adding-a-locale.md` and `docs/localization.md` already carry the full story.

**Size:** S/M (2 files, 4 specific line ranges, 5-6 acceptance criteria)
**Files:**
- `CLAUDE.md` — modify section around line 17 (net48 test comment) AND around line 76 (locale contributor guidance)
- `AGENTS.md` — modify section around line 29 (net48 guidance) and line 45 (registry instructions)

## Investigation targets

**Required:**
- `CLAUDE.md` (full file — short) — to see context around lines 17 and 76 and confirm there are no other stale claims
- `AGENTS.md` (full file — short) — to see context around lines 29 and 45 and confirm no other stale claims
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` — confirm what the generator emits so the replacement text is accurate
- `src/Humanizer/Configuration/FormatterRegistry.cs` — confirm the `Register` call pattern
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs` (around lines 430-445) — confirm the exact symbol causing the net48 block (cited in both reframes)

**Optional:**
- `docs/adding-a-locale.md:27-145` — authoritative locale-authoring workflow to link from the updated agent files
- `.flow/specs/fn-4-fix-net48-test-suite-blocker.md` — for the net48 reframe cross-reference

## Approach

- Match the existing terse style of both files — short bullets, no long prose.
- Do not rewrite surrounding paragraphs just because they're nearby; touch only the stale lines.
- Both files should cross-reference `docs/adding-a-locale.md` as the authoritative workflow (they already do for some topics; keep that pattern consistent).
- For the net48 fix, use the **same** corrected framing in both files: net48 tests currently do not compile due to `Enum.GetValues<T>()` usage in `LocaleTheoryMatrixCompletenessTests.cs`; tracked as `fn-4`. Do not add long explanation of what `Enum.GetValues<T>()` does, and do not frame it as a platform-specific restriction.
- `CLAUDE.md:17` is inside a bash quick-commands comment block — the edit should still read naturally as a terminal comment, not become a paragraph.

## Key context

- Do NOT add a general "update CLAUDE.md / AGENTS.md for new features" workflow or any generic meta-instructions — the task is the four specific fixes plus the one escape-hatch sentence.
- Do NOT rewrite the net48 guidance to promise a fix — just describe the current reality and point at fn-4.
- The CLAUDE.md and AGENTS.md net48 wording does not need to be byte-identical — they live in different sections (CLAUDE.md is a terminal-comment, AGENTS.md is a prose sentence) — but both must name the same real blocker and reference fn-4.

## Acceptance

- [ ] `CLAUDE.md:76` locale-authoring guidance no longer mentions manual formatter/converter registry registration; instead describes the source-generator-driven flow and links to `docs/adding-a-locale.md`
- [ ] `AGENTS.md:45` no longer tells agents to "register new formatters/converters in the appropriate registries"; instead links to `docs/adding-a-locale.md`
- [ ] `CLAUDE.md:17` net48 test comment reframed: references `Enum.GetValues<T>()` / `fn-4` as the real blocker (not "avoid on macOS/Linux")
- [ ] `AGENTS.md:29` net48 guidance reframed: blocked by `Enum.GetValues<T>()` in `LocaleTheoryMatrixCompletenessTests.cs` (not a Linux-specific restriction); cross-references `fn-4-fix-net48-test-suite-blocker`
- [ ] Both files mention the `calendar:` / `number.formatting:` override escape hatch for ICU-divergent locales in one terse sentence each
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` still passes (these are markdown edits, should not trigger formatter errors, but confirm no cross-file side effects)
- [ ] `grep -n "register.*formatter" CLAUDE.md AGENTS.md` returns zero matches for the stale manual-registry phrasing
- [ ] `grep -rn "avoid net48 on" CLAUDE.md AGENTS.md` returns zero matches (both stale platform-conditional framings are gone)

## Done summary
Fixed four stale doc drift issues in CLAUDE.md and AGENTS.md: reframed net48 test guidance from platform-conditional ("avoid on macOS/Linux") to the real blocker (Enum.GetValues<T>() on all platforms, tracked as fn-4); replaced manual "register in formatter/converter registries" locale-authoring instruction with source-generator auto-wiring description; added ICU override escape hatch (calendar:/number.formatting: YAML surfaces) to AGENTS.md Localization Guidance section.
## Evidence
- Commits: b31ed4e39304233843a4f7f16cf3885fc206747b
- Tests: dotnet format Humanizer.slnx --verify-no-changes, grep -n register.*formatter CLAUDE.md AGENTS.md (zero matches), grep -rn avoid net48 on CLAUDE.md AGENTS.md (zero matches), grep -n calendar: CLAUDE.md AGENTS.md (both files match), grep -n number.formatting: CLAUDE.md AGENTS.md (both files match)
- PRs: