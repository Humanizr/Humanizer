#!/bin/bash

set -euo pipefail

ROOT="$(pwd)"
ARTIFACT_DIR="$ROOT/.docfx-artifacts"
WORKTREE_ROOT="$ROOT/.docfx-worktrees"
STATIC_API_DIR="$ROOT/website/static/api"
WORKTREES_TO_REMOVE=()
DOCFX_CMD="dotnet tool run docfx"

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

build_docfx_for_checkout() {
  local checkout_root="$1"
  local destination_name="$2"

  pushd "$checkout_root" >/dev/null
  $DOCFX_CMD metadata docs/docfx.json
  $DOCFX_CMD build docs/docfx.json
  popd >/dev/null

  mkdir -p "$ARTIFACT_DIR/$destination_name"
  cp -a "$checkout_root/docs/_site/." "$ARTIFACT_DIR/$destination_name/"
}

# Build current checkout (development)
build_docfx_for_checkout "$ROOT" "main"
rm -rf "$ROOT/docs/_site"

# Discover release branches
RELEASE_VERSIONS=()
SKIP_FETCH="${SKIP_RELEASE_FETCH:-0}"
MIN_RELEASE_VERSION="${MIN_DOCFX_RELEASE:-v2.14}"

should_build_release() {
  local version="$1"
  local min="$MIN_RELEASE_VERSION"
  if [[ -z "$min" ]]; then
    return 0
  fi

  local current="${version#v}"
  local required="${min#v}"
  local smallest
  smallest="$(printf '%s\n%s\n' "$current" "$required" | sort -V | head -n1)"
  if [[ "$smallest" == "$required" ]]; then
    return 0
  fi
  return 1
}

if [ "$SKIP_FETCH" != "1" ]; then
  RELEASE_TMP="$(mktemp)"
  if git ls-remote --heads origin 'rel/v*' >"$RELEASE_TMP"; then
    if [ -s "$RELEASE_TMP" ]; then
      git fetch --no-tags origin 'refs/heads/rel/*:refs/remotes/origin/rel/*'
      mapfile -t RELEASE_VERSIONS < <(git for-each-ref --format='%(refname:short)' refs/remotes/origin/rel | sed 's#^origin/rel/##' | sort -rV)
    fi
  else
    echo "Warning: unable to query release branches; continuing with development API only."
  fi
  rm -f "$RELEASE_TMP"
else
  echo "Skipping release branch builds per SKIP_RELEASE_FETCH=$SKIP_FETCH"
fi

LATEST_RELEASE=""
BUILT_RELEASES=()

for version in "${RELEASE_VERSIONS[@]}"; do
  [ -n "$version" ] || continue
  if ! should_build_release "$version"; then
    echo "Skipping $version: older than minimum DocFX release $MIN_RELEASE_VERSION"
    continue
  fi

  branch="origin/rel/$version"
  worktree="$WORKTREE_ROOT/$version"
  git worktree add --force "$worktree" "$branch"
  WORKTREES_TO_REMOVE+=("$worktree")

  if [ ! -f "$worktree/docs/docfx.json" ]; then
    echo "Skipping $version: no DocFX configuration found"
    git worktree remove --force "$worktree"
    unset 'WORKTREES_TO_REMOVE[-1]'
    continue
  fi

  if build_docfx_for_checkout "$worktree" "$version"; then
    BUILT_RELEASES+=("$version")
    if [ -z "$LATEST_RELEASE" ]; then
      LATEST_RELEASE="$version"
    fi
  else
    echo "Failed to build documentation for $version, skipping"
  fi

  git worktree remove --force "$worktree" || true
  unset 'WORKTREES_TO_REMOVE[-1]'
done

rm -rf "$STATIC_API_DIR"
mkdir -p "$STATIC_API_DIR"

cp -a "$ARTIFACT_DIR/main/." "$STATIC_API_DIR/main/"

for version in "${BUILT_RELEASES[@]}"; do
  mkdir -p "$STATIC_API_DIR/$version"
  cp -a "$ARTIFACT_DIR/$version/." "$STATIC_API_DIR/$version/"
done

if [ -n "$LATEST_RELEASE" ]; then
  DEFAULT_RELEASE="$LATEST_RELEASE"
else
  DEFAULT_RELEASE="main"
fi

if [ "${#BUILT_RELEASES[@]}" -gt 0 ]; then
  releases_json="["
  for version in "${BUILT_RELEASES[@]}"; do
    releases_json+="\"$version\",";
  done
  releases_json="${releases_json%,}]"
else
  releases_json="[]"
fi

cat >"$STATIC_API_DIR/versions.json" <<MANIFEST
{
  "latest": "$DEFAULT_RELEASE",
  "releases": $releases_json,
  "development": "main"
}
MANIFEST
