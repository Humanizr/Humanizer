#!/usr/bin/env bash
set -euo pipefail

ROOT="$(pwd)"
ARTIFACT_DIR="$ROOT/.docfx-artifacts"
WORKTREE_ROOT="$ROOT/.docfx-worktrees"
WORKTREES_TO_REMOVE=()

cleanup() {
  if [ ${#WORKTREES_TO_REMOVE[@]} -gt 0 ]; then
    for worktree in "${WORKTREES_TO_REMOVE[@]}"; do
      if [ -d "$worktree" ]; then
        git worktree remove --force "$worktree" >/dev/null 2>&1 || true
      fi
    done
  fi
  rm -rf "$ARTIFACT_DIR" "$WORKTREE_ROOT"
}
trap cleanup EXIT

rm -rf "$ARTIFACT_DIR" "$WORKTREE_ROOT"
mkdir -p "$ARTIFACT_DIR" "$WORKTREE_ROOT"

# Verify docfx tool exists
if [ ! -f "$ROOT/docfx" ]; then
  echo "Error: docfx tool not found at $ROOT/docfx"
  echo "Please run: dotnet tool install --tool-path . docfx"
  exit 1
fi

./docfx metadata docs/docfx.json
./docfx build docs/docfx.json
mv docs/_site "$ARTIFACT_DIR/main"

RELEASE_VERSIONS=()
RELEASE_TMP="$(mktemp)"
git ls-remote --heads origin 'rel/v*' >"$RELEASE_TMP"
if [ -s "$RELEASE_TMP" ]; then
  git fetch --no-tags origin 'refs/heads/rel/*:refs/remotes/origin/rel/*'
  mapfile -t RELEASE_VERSIONS < <(git for-each-ref --format='%(refname:short)' refs/remotes/origin/rel | sed 's#^origin/rel/##' | sort -rV)
fi
rm -f "$RELEASE_TMP"

LATEST_RELEASE=""
BUILT_VERSIONS=()
if [ "${#RELEASE_VERSIONS[@]}" -gt 0 ]; then
  for version in "${RELEASE_VERSIONS[@]}"; do
    if [ -z "$version" ]; then
      continue
    fi

    branch="origin/rel/$version"
    worktree="$WORKTREE_ROOT/$version"
    git worktree add --force "$worktree" "$branch"
    WORKTREES_TO_REMOVE+=("$worktree")
    
    # Skip branches without DocFX configuration
    if [ ! -f "$worktree/docs/docfx.json" ]; then
      echo "Skipping $version: no DocFX configuration found"
      git worktree remove --force "$worktree"
      unset 'WORKTREES_TO_REMOVE[-1]'
      continue
    fi
    
    pushd "$worktree" >/dev/null
    if "$ROOT/docfx" metadata docs/docfx.json && "$ROOT/docfx" build docs/docfx.json; then
      popd >/dev/null

      mkdir -p "$ARTIFACT_DIR/$version"
      cp -a "$worktree/docs/_site/." "$ARTIFACT_DIR/$version/"
      git worktree remove --force "$worktree"
      unset 'WORKTREES_TO_REMOVE[-1]'

      BUILT_VERSIONS+=("$version")
      if [ -z "$LATEST_RELEASE" ]; then
        LATEST_RELEASE="$version"
      fi
    else
      echo "Failed to build documentation for $version, skipping"
      popd >/dev/null
      git worktree remove --force "$worktree"
      unset 'WORKTREES_TO_REMOVE[-1]'
    fi
  done
fi

rm -rf docs/_site
mkdir -p docs/_site

if [ -n "$LATEST_RELEASE" ]; then
  cp -a "$ARTIFACT_DIR/$LATEST_RELEASE/." docs/_site/
else
  cp -a "$ARTIFACT_DIR/main/." docs/_site/
  LATEST_RELEASE="main"
fi

mkdir -p docs/_site/main
cp -a "$ARTIFACT_DIR/main/." docs/_site/main/

for version in "${BUILT_VERSIONS[@]}"; do
  mkdir -p "docs/_site/$version"
  cp -a "$ARTIFACT_DIR/$version/." "docs/_site/$version/"
done

if [ "${#BUILT_VERSIONS[@]}" -gt 0 ]; then
  releases_json="["
  for version in "${BUILT_VERSIONS[@]}"; do
    releases_json+="\"$version\",";
  done
  releases_json="${releases_json%,}"
  releases_json+="]"
else
  releases_json="[]"
fi

{
  printf '{\n'
  printf '  "latest": "%s",\n' "$LATEST_RELEASE"
  printf '  "releases": %s,\n' "$releases_json"
  printf '  "development": "main"\n'
  printf '}\n'
} > docs/_site/versions.json
