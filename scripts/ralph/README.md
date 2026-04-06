# Ralph — Autonomous Agent Harness

Ralph is a local shell harness that runs Claude Code in a loop to autonomously execute flow-next epics.

## Quick Start

```bash
# Run an epic autonomously
./scripts/ralph/ralph.sh fn-1-add-feature

# Dry run (preview what would happen)
./scripts/ralph/ralph.sh fn-1-add-feature --dry-run

# Limit cycles
./scripts/ralph/ralph.sh fn-1-add-feature --max-cycles 5
```

## How It Works

1. **Reads** the epic spec from `.flow/epics/<epic-id>/spec.md`
2. **Invokes** Claude Code with `/flow-next:work` to pick up the next task
3. **Validates** changes pass build + tests
4. **Commits** passing work, reverts failures
5. **Loops** until all tasks complete or max cycles reached

## Safety

- Requires a **clean git working tree** before starting
- **Reverts** any changes that fail quality checks
- **Logs** every cycle to `scripts/ralph/logs/`
- Stops after `--max-cycles` (default: 20) to prevent runaway

## Configuration

Edit `ralph.yml` to customize:
- Build/test commands
- Max cycles and cooldowns
- Commit behavior
- Branch strategy

## Logs

All runs are logged to `scripts/ralph/logs/` with timestamps:
```
scripts/ralph/logs/ralph_fn-1-add-feature_20260406_160000.log
```

## Prerequisites

- `claude` CLI in PATH
- `git` in PATH
- Clean working tree (no uncommitted changes)
- Epic spec at `.flow/epics/<epic-id>/spec.md`
