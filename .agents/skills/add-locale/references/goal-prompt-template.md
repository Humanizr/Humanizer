# Add-Locale `/goal` Prompt Template

Use this template when starting a long-running `/goal` to add a new Humanizer locale or bring an existing locale to parity.

- `<culture-code>` — target culture code, such as `ur`, `ur-PK`, `hi`, or `fr-CA`

```text
culture-code = 

Implement full Humanizer locale parity for <culture-code>.

Use $add-locale.

Follow .agents/skills/add-locale/SKILL.md as the authoritative workflow. Use .agents/skills/add-locale/references/parity-checklist.md for the canonical surface inventory, parity-map schema, file/test map, proof rows, validation commands, and completion gate. If any red flag applies, read .agents/skills/add-locale/references/urdu-epic-learnings.md before planning: Arabic-script locale, Indic/South Asian numbers, plural-rule uncertainty, ordinals, Hijri/non-Gregorian calendar behavior, no-delta variants, optional YAML fields, schema/generator changes, shared-engine changes, or PR hygiene issues.

Goal: every shipped localized Humanizer surface for <culture-code> must execute through intentional locale ownership or intentional same-language inheritance, with no English fallback, unsupported-locale gap, generic fallback dependency, or partial completion.

Required workflow:
1. Re-anchor on repo instructions and the add-locale skill.
2. Read src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs.
3. Read the target locale YAML and full variantOf parent chain, if any.
4. Build artifacts/YYYY-MM-DD-<culture-code>-parity-map.md using the checklist reference.
5. Complete preflight before implementation: classify every canonical surface and required proof subrow, record ownership paths, eliminate unknowns, and identify missing/fallback/unsupported surfaces.
6. Implement the smallest honest path: same-language inheritance, existing shared engine, shared engine/generator extension, or locale-specific runtime only when genuinely procedural.
7. Use proposer + independent native-speaker reviewer subagents for new locale-owned wording, parser tokens, grammar, or composed outputs.
8. Add exact-output tests plus sweep/registry coverage proving real locale behavior, not non-empty or generic output.
9. Update source-generator tests for schema, inheritance, generated wiring, or engine-contract changes.
10. Close out only with an empty final unresolved set, justified not-applicable conditional rows, one closeout line per canonical surface and proof subrow, exact validation commands/results, and clean git status except intentional changes.

Run required final validation from the checklist. Include net48 only on Windows hosts.
```
