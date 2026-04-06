# fn-1-locale-translation-parity-across-all.10 Update documentation for locale parity requirements

## Description
Update documentation to reflect the locale parity requirements, no-English-fallback rule, and any changes to the source generator pipeline.

**Size:** M
**Files:**
- `CLAUDE.md` — Update Localization section (line ~76) to mention parity requirements and all doc references
- `AGENTS.md` — Update Localization Guidance section to match CLAUDE.md
- `.github/CONTRIBUTING.md` — Update "Need your help with localisation" section; remove outdated `DefaultFormatter` subclassing guidance
- `readme.md` — Update "Words to Number" fallback note (line ~662) to clarify scope to unregistered cultures
- `docs/locale-yaml-reference.md` — Add Generator Diagnostics section if needed
- `docs/adding-a-locale.md` — Verify parity workflow references and completeness requirements
- `docs/locale-yaml-how-to.md` — Verify parity rules and authoring guidance are current
- `docs/localization.md` — Verify supported languages list is accurate

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
- [ ] CLAUDE.md Localization section updated with parity requirements and full doc list
- [ ] AGENTS.md Localization Guidance section updated to match
- [ ] CONTRIBUTING.md localisation section updated to remove outdated DefaultFormatter guidance
- [ ] readme.md fallback note clarified for unregistered cultures only
- [ ] docs/locale-yaml-reference.md updated if source generator diagnostics changed
- [ ] docs/adding-a-locale.md verified for accuracy
- [ ] docs/locale-yaml-how-to.md verified for accuracy (updated if parity rules changed)
- [ ] docs/localization.md supported languages list verified
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
