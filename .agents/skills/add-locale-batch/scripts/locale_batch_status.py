#!/usr/bin/env python3
"""Refresh compact GitHub PR state in a locale batch status JSON file."""

from __future__ import annotations

import argparse
import json
import subprocess
import sys
from pathlib import Path
from typing import Any


GH_TIMEOUT_SECONDS = 30


def run_gh(args: list[str]) -> subprocess.CompletedProcess[str]:
    try:
        completed = subprocess.run(
            args,
            text=True,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            timeout=GH_TIMEOUT_SECONDS,
        )
    except subprocess.TimeoutExpired as exc:
        raise RuntimeError(f"command timed out after {GH_TIMEOUT_SECONDS}s: {' '.join(args)}") from exc

    if completed.returncode != 0:
        output = (completed.stdout + completed.stderr).strip()
        raise RuntimeError(f"command failed: {' '.join(args)}\n{output}")

    return completed


def run_json(args: list[str]) -> Any:
    return json.loads(run_gh(args).stdout or "null")


def run_text(args: list[str]) -> str:
    return run_gh(args).stdout


def unresolved_threads(repo: str, pr: int) -> int:
    owner, name = repo.split("/", 1)
    query = (
        "query($owner:String!, $name:String!, $number:Int!) { "
        "repository(owner:$owner, name:$name) { "
        "pullRequest(number:$number) { "
        "reviewThreads(first:100) { nodes { isResolved isOutdated } } "
        "} } }"
    )
    data = run_json([
        "gh", "api", "graphql",
        "-f", f"owner={owner}",
        "-f", f"name={name}",
        "-F", f"number={pr}",
        "-f", f"query={query}",
    ])
    nodes = data["data"]["repository"]["pullRequest"]["reviewThreads"]["nodes"]
    return sum(1 for node in nodes if not node["isResolved"] and not node["isOutdated"])


def checks_state(repo: str, pr: int) -> str:
    output = run_text(["gh", "pr", "checks", str(pr), "--repo", repo, "--watch=false"])
    states: list[str] = []
    for line in output.splitlines():
        parts = line.split("\t")
        if len(parts) >= 2:
            states.append(parts[1].strip().lower())
    if not states:
        return "unknown"
    if any(state in {"fail", "failing", "failure", "cancelled", "timed_out"} for state in states):
        return "failed"
    if any(state in {"pending", "queued", "in_progress", "waiting"} for state in states):
        return "pending"
    if all(state in {"pass", "passing", "success", "skipping", "skipped"} for state in states):
        return "green"
    return "mixed"


def refresh_entry(repo: str, entry: dict[str, Any]) -> dict[str, Any]:
    pr = entry.get("pr")
    if not pr:
        return entry

    view = run_json([
        "gh", "pr", "view", str(pr), "--repo", repo,
        "--json", "number,url,state,isDraft,headRefName,headRefOid,reviewDecision,mergeStateStatus,mergeable,mergedAt",
    ])

    entry.update({
        "pr": view["number"],
        "prUrl": view["url"],
        "head": view.get("headRefOid"),
        "branch": entry.get("branch") or view.get("headRefName"),
        "reviewDecision": view.get("reviewDecision"),
        "mergeable": view.get("mergeable"),
        "mergeStateStatus": view.get("mergeStateStatus"),
        "isDraft": view.get("isDraft"),
        "merged": view.get("state") == "MERGED",
        "mergedAt": view.get("mergedAt"),
        "unresolvedThreads": unresolved_threads(repo, int(pr)),
        "checks": checks_state(repo, int(pr)),
    })
    if entry["merged"] and entry.get("phase") != "cleaned":
        entry["phase"] = "merged"
    return entry


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--status", required=True)
    parser.add_argument("--repo", default="Humanizr/Humanizer")
    parser.add_argument("--write", action="store_true")
    args = parser.parse_args()

    path = Path(args.status)
    status = json.loads(path.read_text())
    for entry in status.values():
        if isinstance(entry, dict):
            refresh_entry(args.repo, entry)

    rendered = json.dumps(status, ensure_ascii=False, indent=2, sort_keys=True) + "\n"
    if args.write:
        path.write_text(rendered)
    sys.stdout.write(rendered)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
