#!/usr/bin/env bash
# ralph.sh — Autonomous Claude Code agent runner
# Usage: ./scripts/ralph/ralph.sh <epic-id> [--dry-run] [--max-cycles N]
#
# Ralph executes a flow-next epic autonomously by:
#   1. Reading the epic spec
#   2. Working through tasks sequentially
#   3. Running quality checks after each task
#   4. Committing passing work, reverting failures
#   5. Looping until all tasks are done or max cycles reached

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"
LOG_DIR="$SCRIPT_DIR/logs"
CONFIG="$SCRIPT_DIR/ralph.yml"

# Defaults
MAX_CYCLES=20
DRY_RUN=false
EPIC_ID=""

# --- Parse args ---
while [[ $# -gt 0 ]]; do
  case "$1" in
    --dry-run)   DRY_RUN=true; shift ;;
    --max-cycles) MAX_CYCLES="$2"; shift 2 ;;
    --help|-h)
      echo "Usage: $0 <epic-id> [--dry-run] [--max-cycles N]"
      echo ""
      echo "  <epic-id>       Flow-next epic ID (e.g., fn-1-add-oauth)"
      echo "  --dry-run       Print what would happen without executing"
      echo "  --max-cycles N  Max agent cycles (default: 20)"
      exit 0
      ;;
    -*) echo "Unknown option: $1" >&2; exit 1 ;;
    *)  EPIC_ID="$1"; shift ;;
  esac
done

if [[ -z "$EPIC_ID" ]]; then
  echo "Error: epic-id is required" >&2
  echo "Usage: $0 <epic-id> [--dry-run] [--max-cycles N]" >&2
  exit 1
fi

# --- Setup ---
mkdir -p "$LOG_DIR"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
LOG_FILE="$LOG_DIR/ralph_${EPIC_ID}_${TIMESTAMP}.log"

log() {
  local msg="[$(date '+%H:%M:%S')] $*"
  echo "$msg" | tee -a "$LOG_FILE"
}

die() {
  log "FATAL: $*"
  exit 1
}

# --- Preflight checks ---
command -v claude >/dev/null 2>&1 || die "claude CLI not found in PATH"
command -v git >/dev/null 2>&1    || die "git not found in PATH"

cd "$REPO_ROOT"

# Verify clean working tree
if [[ -n "$(git status --porcelain)" ]]; then
  die "Working tree is dirty. Commit or stash changes before running Ralph."
fi

# Verify epic exists
EPIC_DIR="$REPO_ROOT/.flow/epics/$EPIC_ID"
if [[ ! -d "$EPIC_DIR" ]]; then
  die "Epic not found: $EPIC_DIR"
fi

EPIC_SPEC="$EPIC_DIR/spec.md"
if [[ ! -f "$EPIC_SPEC" ]]; then
  die "Epic spec not found: $EPIC_SPEC"
fi

# --- Load config ---
BUILD_CMD="dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts"
TEST_CMD="dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0"

if [[ -f "$CONFIG" ]]; then
  # Override from ralph.yml if present (simple grep-based, no yq dependency)
  _build=$(grep -oP 'build_command:\s*"\K[^"]+' "$CONFIG" 2>/dev/null || true)
  _test=$(grep -oP 'test_command:\s*"\K[^"]+' "$CONFIG" 2>/dev/null || true)
  [[ -n "$_build" ]] && BUILD_CMD="$_build"
  [[ -n "$_test" ]] && TEST_CMD="$_test"
fi

log "=== Ralph autonomous agent ==="
log "Epic:       $EPIC_ID"
log "Max cycles: $MAX_CYCLES"
log "Dry run:    $DRY_RUN"
log "Build:      $BUILD_CMD"
log "Test:       $TEST_CMD"
log "Log:        $LOG_FILE"
log ""

# --- Checkpoint helpers ---
save_checkpoint() {
  git stash push -m "ralph-checkpoint-$(date +%s)" --include-untracked 2>/dev/null || true
}

restore_checkpoint() {
  local stash
  stash=$(git stash list | grep "ralph-checkpoint" | head -1 | cut -d: -f1)
  if [[ -n "$stash" ]]; then
    git stash pop "$stash" 2>/dev/null || true
  fi
}

revert_to_clean() {
  log "Reverting uncommitted changes..."
  git checkout -- . 2>/dev/null || true
  git clean -fd 2>/dev/null || true
}

# --- Quality gate ---
run_quality_checks() {
  log "Running build..."
  if ! eval "$BUILD_CMD" >> "$LOG_FILE" 2>&1; then
    log "BUILD FAILED"
    return 1
  fi
  log "Build passed."

  log "Running tests..."
  if ! eval "$TEST_CMD" >> "$LOG_FILE" 2>&1; then
    log "TESTS FAILED"
    return 1
  fi
  log "Tests passed."
  return 0
}

# --- Main loop ---
CYCLE=0
while [[ $CYCLE -lt $MAX_CYCLES ]]; do
  CYCLE=$((CYCLE + 1))
  log "--- Cycle $CYCLE / $MAX_CYCLES ---"

  if [[ "$DRY_RUN" == "true" ]]; then
    log "[dry-run] Would invoke: claude -p '/flow-next:work $EPIC_ID'"
    log "[dry-run] Would run quality checks"
    log "[dry-run] Would commit if passing"
    continue
  fi

  # Invoke Claude Code to work on the next task
  PROMPT="Work on the next incomplete task in epic $EPIC_ID. Use /flow-next:work $EPIC_ID to pick up where we left off. After completing the task, stop and report what you did."

  log "Invoking Claude Code agent..."
  if ! claude -p "$PROMPT" --output-format text >> "$LOG_FILE" 2>&1; then
    log "Agent invocation failed. Pausing..."
    revert_to_clean
    continue
  fi

  # Check if there are any changes
  if [[ -z "$(git status --porcelain)" ]]; then
    log "No changes produced. Epic may be complete."
    break
  fi

  # Run quality checks
  if run_quality_checks; then
    log "Quality checks passed. Committing..."
    git add -A
    git commit -m "$(cat <<EOF
feat($EPIC_ID): ralph cycle $CYCLE

Autonomous work on epic $EPIC_ID, cycle $CYCLE.

Co-Authored-By: Ralph (autonomous agent) <noreply@anthropic.com>
EOF
)" >> "$LOG_FILE" 2>&1
    log "Committed successfully."
  else
    log "Quality checks failed. Reverting cycle $CYCLE changes."
    revert_to_clean
  fi

  log ""
done

log "=== Ralph finished ==="
log "Completed $CYCLE cycles for epic $EPIC_ID"
log "Check log: $LOG_FILE"
