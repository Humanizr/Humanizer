# Flow-Next Usage Guide

Task tracking for AI agents. All state lives in `.flow/`.

## CLI

```bash
.flow/bin/flowctl --help              # All commands
.flow/bin/flowctl <cmd> --help        # Command help
```

## File Structure

```
.flow/
├── bin/flowctl             # CLI (this install)
├── epics/fn-N-slug.json    # Epic metadata (e.g., fn-1-add-oauth.json)
├── specs/fn-N-slug.md      # Epic specifications
├── tasks/fn-N-slug.M.json  # Task metadata (e.g., fn-1-add-oauth.1.json)
├── tasks/fn-N-slug.M.md    # Task specifications
├── memory/                 # Context memory
└── meta.json               # Project metadata
```

## IDs

- Epics: `fn-N-slug` where slug is derived from title (e.g., fn-1-add-oauth, fn-2-fix-login-bug)
- Tasks: `fn-N-slug.M` (e.g., fn-1-add-oauth.1, fn-2-fix-login-bug.2)

**Backwards compatibility**: Legacy formats `fn-N`, `fn-N-xxx`, `fn-N.M`, and `fn-N-xxx.M` still work.

## Common Commands

```bash
# List
.flow/bin/flowctl list                          # All epics + tasks grouped
.flow/bin/flowctl epics                         # All epics with progress
.flow/bin/flowctl tasks                         # All tasks
.flow/bin/flowctl tasks --epic fn-1-add-oauth   # Tasks for epic
.flow/bin/flowctl tasks --status todo           # Filter by status

# View
.flow/bin/flowctl show fn-1-add-oauth           # Epic with all tasks
.flow/bin/flowctl show fn-1-add-oauth.2         # Single task
.flow/bin/flowctl cat fn-1-add-oauth            # Epic spec (markdown)
.flow/bin/flowctl cat fn-1-add-oauth.2          # Task spec (markdown)

# Status
.flow/bin/flowctl ready --epic fn-1-add-oauth   # What's ready to work on
.flow/bin/flowctl validate --all                # Check structure
.flow/bin/flowctl state-path                    # Show state directory (for worktrees)

# Create
.flow/bin/flowctl epic create --title "..."
.flow/bin/flowctl task create --epic fn-1-add-oauth --title "..."
.flow/bin/flowctl task create --epic fn-1-add-oauth --title "..." --deps fn-1-add-oauth.1,fn-1-add-oauth.2

# Dependencies
.flow/bin/flowctl task set-deps fn-1-add-oauth.3 --deps fn-1-add-oauth.1,fn-1-add-oauth.2
.flow/bin/flowctl dep add fn-1-add-oauth.3 fn-1-add-oauth.1

# Work
.flow/bin/flowctl start fn-1-add-oauth.2        # Claim task
.flow/bin/flowctl done fn-1-add-oauth.2 --summary-file s.md --evidence-json e.json
```

## Workflow

1. `.flow/bin/flowctl epics` - list all epics
2. `.flow/bin/flowctl ready --epic fn-N-slug` - find available tasks
3. `.flow/bin/flowctl start fn-N-slug.M` - claim task
4. Implement the task
5. `.flow/bin/flowctl done fn-N-slug.M --summary-file ... --evidence-json ...` - complete

## Evidence JSON Format

```json
{"commits": ["abc123"], "tests": ["npm test"], "prs": []}
```

## Parallel Worktrees

Runtime state (status, assignee, etc.) is stored in `.git/flow-state/`, shared across worktrees:

```bash
.flow/bin/flowctl state-path              # Show state directory
.flow/bin/flowctl migrate-state           # Migrate existing repo
.flow/bin/flowctl migrate-state --clean   # Migrate + remove runtime from tracked files
```

Migration is optional — existing repos work without changes.

## More Info

- Human docs: https://github.com/gmickel/flow-next/blob/main/plugins/flow-next/docs/flowctl.md
- CLI reference: `.flow/bin/flowctl --help`
