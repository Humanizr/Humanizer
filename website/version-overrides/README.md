# Historical documentation overlays

Each version directory's `overlay.json` is the exact authority for its
replacement and exclusion paths. Keep that manifest synchronized with the
files in the directory; do not maintain a second per-version inventory here.

`../scenario-api-contract.json` is the exact authority for scenario-to-API
links, including version substitutions and unavailable API targets. Both
current content checks and snapshot validation consume that contract.

Immutable output under `../versioned_docs` and `../versioned_sidebars` changes
only through `tools/docs/snapshot.ps1`. Use a scoped historical correction for
published pages, and let the snapshot transaction update the recorded digests.
Do not edit generated snapshots or their manifest hashes by hand.

To understand why a replacement exists, diff it against the canonical page and
consult the corresponding package, source tag, or API evidence. Overlay
directories do not inherit from one another. Identical authored replacements
across historical versions are acceptable when each accurately describes its
selected package.
