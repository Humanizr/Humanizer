#!/usr/bin/env bash
set -euo pipefail

ROOT="$(pwd)"
ARTIFACT_DIR="$ROOT/.docfx-artifacts"
WORKTREE_ROOT="$ROOT/.docfx-worktrees"
WORKTREES_TO_REMOVE=()

cleanup() {
  for worktree in "${WORKTREES_TO_REMOVE[@]:-}"; do
    if [ -d "$worktree" ]; then
      git worktree remove --force "$worktree" >/dev/null 2>&1 || true
    fi
  done
  rm -rf "$ARTIFACT_DIR" "$WORKTREE_ROOT"
}
trap cleanup EXIT

rm -rf "$ARTIFACT_DIR" "$WORKTREE_ROOT"
mkdir -p "$ARTIFACT_DIR" "$WORKTREE_ROOT"

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
for version in "${RELEASE_VERSIONS[@]}"; do
  if [ -z "$version" ]; then
    continue
  fi

  branch="origin/rel/$version"
  worktree="$WORKTREE_ROOT/$version"
  git worktree add --force "$worktree" "$branch"
  WORKTREES_TO_REMOVE+=("$worktree")
  pushd "$worktree" >/dev/null
  "$ROOT/docfx" metadata docs/docfx.json
  "$ROOT/docfx" build docs/docfx.json
  popd >/dev/null

  mkdir -p "$ARTIFACT_DIR/$version"
  cp -a "$worktree/docs/_site/." "$ARTIFACT_DIR/$version/"
  git worktree remove --force "$worktree"
  unset 'WORKTREES_TO_REMOVE[-1]'

  if [ -z "$LATEST_RELEASE" ]; then
    LATEST_RELEASE="$version"
  fi
done

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

for version in "${RELEASE_VERSIONS[@]}"; do
  mkdir -p "docs/_site/$version"
  cp -a "$ARTIFACT_DIR/$version/." "docs/_site/$version/"
done

if [ "${#RELEASE_VERSIONS[@]}" -gt 0 ]; then
  releases_json="["
  for version in "${RELEASE_VERSIONS[@]}"; do
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
