Implement full Humanizer locale parity for <culture-code> 

Use $add-locale.

Goal:
Add or bring <culture-code> to full parity across every shipped Humanizer localized surface, with no English fallback, unsupported-locale gaps, generic fallback dependency, or partial completion.

Requirements:
- Re-anchor on repo instructions and the add-locale skill.
- Read the canonical surface source: src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs.
- Read the target locale YAML and parent locale chain, if any.
- Build a local parity map at artifacts/YYYY-MM-DD-<culture-code>-parity-map.md using .agents/skills/add-locale/references/parity-checklist.md.
- Complete preflight before implementation:
  - classify every canonical surface and required proof subrow
  - record ownership paths
  - eliminate all unknowns
  - identify missing/fallback/unsupported surfaces
- Use same-language inheritance only when output is natural and explicitly proved.
- Prefer YAML data plus shared engines; extend generator/runtime contracts when needed.
- Use proposer + independent native-speaker reviewer subagents for new locale-owned wording, parser tokens, grammar, or composed outputs.
- Add exact-output tests and sweep/registry coverage proving real locale behavior, not just non-empty/generic output.
- Update source-generator tests when schema, inheritance, generated wiring, or engine contracts change.
- Keep regional no-delta variants inheritance-only and omit surfaces unless real overrides exist.

Fast-fail risks to check before editing:
- ordinal API split: number.words.ordinal, ordinal.numeric, ordinal.date, ordinal.dateOnly
- culture binding for engines that call NumberToWords or parsing services
- ICU/NLS calendar differences for non-Gregorian behavior
- optional YAML fields surviving parser/resolver/emitter/migration/runtime paths
- script/codepoint lookalikes
- plural-rule family
- number scale family
- shared-engine bugs masked by real locale data
- stray local files, build outputs, namespace mismatches, unrelated edits

Validation before completion:
- dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
- dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
- dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
- dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net11.0
- dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/locale-parity-validation

Run net48 tests only on Windows hosts.

Completion requires:
- empty final unresolved surface set
- no missing, english-fallback, unsupported, unknown, or generic-fallback proof rows
- justified not-applicable conditional proof subrows only
- one closeout line per canonical surface and required proof subrow
- exact validation commands and results
- clean git status except intentional changes
