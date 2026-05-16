---
name: add-locale-batch
description: Use when coordinating multiple Humanizer locale parity efforts across isolated worktrees, branches, PRs, review/CI babysitting, merges, and cleanup. This is the parent orchestration skill for locale batches; delegate each individual locale to add-locale rather than implementing locale details directly.
---

# Humanizer Locale Batch Orchestration

Use this for multi-locale Humanizer parity work. It is a parent orchestration skill: each locale subagent still uses `$add-locale` for implementation and proof.

## Non-negotiables

- Parent coordinates only: plan, dispatch, track, verify, merge, cleanup.
- One isolated sibling worktree and branch per locale; no child may edit main or a sibling worktree.
- Each locale orchestrator must read `$add-locale` and set its own goal from `.agents/skills/add-locale/references/goal-template.md`.
- Do not use draft PRs unless the user changes policy.
- Merge only when every gate in `references/pr-gates.md` passes.
- If multiple locales need the same generator/runtime/schema change, hold them and land one shared PR first; see `references/batch-state.md`.
- After merge, clean the worktree/branches as described in `references/pr-gates.md`.
- Never commit artifacts containing full local filesystem paths.

## Workflow

1. Load or create batch state (`references/batch-state.md`).
2. Create one worktree/branch per active locale.
3. Dispatch locale orchestrators with `references/locale-orchestrator-contract.md`.
4. Poll with `scripts/locale_batch_status.py`; update phases; wake children only for deltas.
5. Hold new locales when asked; keep closing already-active PRs unless told otherwise.
6. Final rollup: locale, PR, merge commit, validation, babysit result, cleanup, deferred locales.

## Compact dispatch

```text
Read .agents/skills/add-locale-batch/references/locale-orchestrator-contract.md.
Locale: <code> / <name>.
Worktree: <relative-path>. Branch: <branch>. PR: <number-or-new>.
Phase: <batch-state phase>. Known blockers: <none|summary>.
Report compact JSON status only unless blocked.
```

## References

- `references/batch-state.md` — plan/status schema, phases, shared-change hold flow.
- `references/locale-orchestrator-contract.md` — child-agent contract.
- `references/pr-gates.md` — PR polling, merge gate, cleanup.
