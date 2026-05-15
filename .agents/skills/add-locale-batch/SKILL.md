---
name: add-locale-batch
description: Use when coordinating multiple Humanizer locale parity efforts across isolated worktrees, branches, PRs, review/CI babysitting, merges, and cleanup. This is the parent orchestration skill for locale batches; delegate each individual locale to add-locale rather than implementing locale details directly.
---

# Humanizer Locale Batch Orchestration

Use this skill when the task is to run, resume, or monitor more than one Humanizer locale parity PR.

This is an orchestration skill. It does not replace `$add-locale`; each locale subagent must still use `$add-locale` for implementation and parity proof.

## Core Rules

- Keep the parent as coordinator only: plan, dispatch, track, verify, merge, and clean up.
- Use one isolated sibling worktree and branch per locale.
- Never allow a locale subagent to edit the main checkout or another locale's worktree.
- Dispatch one locale orchestrator per locale. Give it only its locale, worktree, branch, PR context, and current phase.
- Centralize PR polling in the parent where practical. Wake locale agents only for actionable review, CI, conflict, or merge/cleanup work.
- Pause conflicting locale work when several locales need the same shared generator/runtime/schema change. Land one explicit shared PR first, then resume locales.
- Do not use draft PRs unless the user explicitly changes policy.
- Squash merge only after final gates are clean: approved or acceptable review state, zero unresolved non-outdated review threads, required checks green, mergeable clean, and babysit clean.
- After merge, remove the locale worktree and delete/prune merged branches when safe.
- Do not commit artifacts that contain full local filesystem paths.

## Minimal Workflow

1. Create or load a batch plan/status file. See `references/batch-state.md`.
2. Create one worktree/branch per active locale.
3. Dispatch each locale orchestrator with the compact contract in `references/locale-orchestrator-contract.md`.
4. Poll PR gates centrally using `scripts/locale_batch_status.py` or equivalent `gh` queries.
5. Steer only deltas to agents: unresolved review threads, CI failures, rebase requirements, or merge/cleanup authorization.
6. Update batch status after every meaningful transition.
7. Stop starting new locales when the user says to hold; keep closing already-active PRs unless told otherwise.
8. Final rollup: locale, PR URL, merge commit, validation result, babysit status, cleanup status, and deferred locales.

## Dispatch Pattern

Use this brief shape instead of repeating the whole workflow:

```text
Read .agents/skills/add-locale-batch/references/locale-orchestrator-contract.md.
You are assigned only: <culture-code> / <language-name>.
Worktree: <relative-worktree-path>.
Branch: <branch>.
PR: <number-or-new>.
Phase: <implement|review-fix|babysit|merge-cleanup>.
Known blockers: <short list or none>.
Report compact JSON status only unless blocked.
```

The locale orchestrator must set its own goal from `.agents/skills/add-locale/references/goal-template.md`, filled with its culture code/name and worktree context.

## References

- `references/batch-state.md` — batch/status file format and phase state machine.
- `references/locale-orchestrator-contract.md` — compact child-agent contract.
- `references/pr-gates.md` — centralized PR polling, merge gates, and cleanup rules.

## Token Discipline

- Prefer compact JSON status over narrative updates.
- Include failed logs only; passing validation should be command + pass/fail + URL if applicable.
- Do not keep implementation agents alive just to narrate CI polling when the parent can poll PR state.
- Resume from status files, not chat history.
