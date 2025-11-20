#!/usr/bin/env bash

set -euo pipefail

usage() {
  cat <<'USAGE'
Usage: scripts/generate-docs.sh [--serve] [--skip-release-fetch]

Options:
  --serve                Serve the freshly built Docusaurus site (blocks until stopped).
  --skip-release-fetch   Skip fetching rel/v* branches when building DocFX versions (faster offline builds).
  -h, --help             Show this help message.
USAGE
}

SERVE=0
SKIP_RELEASE_FETCH_FLAG=${SKIP_RELEASE_FETCH:-0}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --serve)
      SERVE=1
      shift
      ;;
    --skip-release-fetch)
      SKIP_RELEASE_FETCH_FLAG=1
      shift
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      echo "Unknown option: $1" >&2
      usage >&2
      exit 1
      ;;
  esac
done

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")"/.. && pwd)"

cd "$ROOT"
echo "[docs] Restoring dotnet tools..."
dotnet tool restore

echo "[docs] Building DocFX API output (release fetch: $SKIP_RELEASE_FETCH_FLAG)..."
SKIP_RELEASE_FETCH=$SKIP_RELEASE_FETCH_FLAG .github/workflows/scripts/build-docfx.sh

echo "[docs] Installing website dependencies..."
pushd website >/dev/null
npm ci

echo "[docs] Building Docusaurus site..."
npm run build

if [[ $SERVE -eq 1 ]]; then
  echo "[docs] Serving build output at http://localhost:3000"
  npm run serve -- --dir build
fi

popd >/dev/null

echo "[docs] Done. Output available under website/build"
