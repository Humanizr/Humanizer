param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path $PSScriptRoot -Parent
$script = @'
const fs = require("fs");
const path = require("path");

const repoRoot = process.cwd();
const adjudicationDir = path.join(repoRoot, "docs", "reviews", "second-pass", "locales");
const reportDir = path.join(repoRoot, "docs", "reviews", "second-pass");

function readJson(filePath) {
  let text = fs.readFileSync(filePath, "utf8");
  if (text.charCodeAt(0) === 0xFEFF) text = text.slice(1);
  return JSON.parse(text);
}

const files = fs.readdirSync(adjudicationDir)
  .filter(name => name.endsWith(".adjudication.json"))
  .sort((a, b) => a.localeCompare(b));

const reviews = files.map(name => readJson(path.join(adjudicationDir, name)));

const confirmed = [];
const modified = [];
const rejected = [];

for (const review of reviews) {
  review.summary.pending = review.findings.filter(x => x.adjudication === "pending").length;
  review.summary.confirmed = review.findings.filter(x => x.adjudication === "confirmed").length;
  review.summary.modified = review.findings.filter(x => x.adjudication === "modified").length;
  review.summary.rejected = review.findings.filter(x => x.adjudication === "rejected").length;

  confirmed.push(...review.findings.filter(x => x.adjudication === "confirmed"));
  modified.push(...review.findings.filter(x => x.adjudication === "modified"));
  rejected.push(...review.findings.filter(x => x.adjudication === "rejected"));
}

const aggregate = {
  generated: new Date().toISOString().slice(0, 10),
  branch: require("child_process").execSync("git branch --show-current", { encoding: "utf8" }).trim(),
  locale_count: reviews.length,
  total_confirmed: confirmed.length,
  total_modified: modified.length,
  total_rejected: rejected.length,
  locales: reviews
};

fs.writeFileSync(
  path.join(reportDir, "2026-03-13-locale-adversarial-second-pass.json"),
  JSON.stringify(aggregate, null, 2) + "\n",
  "utf8"
);

const lines = [];
lines.push("# Locale Adversarial Review Second Pass");
lines.push("");
lines.push(`Generated: ${aggregate.generated}`);
lines.push("");
lines.push("## Summary");
lines.push(`- Locales adjudicated: ${reviews.length}`);
lines.push(`- Confirmed findings: ${confirmed.length}`);
lines.push(`- Modified findings: ${modified.length}`);
lines.push(`- Rejected findings: ${rejected.length}`);
lines.push("");
lines.push("## Adjudication");

for (const bucket of [
  { name: "Modified", items: modified },
  { name: "Rejected", items: rejected },
  { name: "Confirmed", items: confirmed }
]) {
  lines.push(`### ${bucket.name}`);
  if (bucket.items.length === 0) {
    lines.push("None.");
  } else {
    for (const item of bucket.items.sort((a, b) => {
      const locale = a.locale.localeCompare(b.locale);
      return locale !== 0 ? locale : a.key.localeCompare(b.key);
    })) {
      lines.push(`- [${item.locale}] \`${item.key}\``);
      lines.push(`  Original: ${item.current_value} -> ${item.proposed_replacement}`);
      lines.push(`  Final: ${item.adjudicated_replacement}`);
      lines.push(`  Decision: ${item.adjudication}`);
      lines.push(`  Rationale: ${item.adjudicated_rationale}`);
      if (item.adjudicated_evidence && item.adjudicated_evidence.length > 0) {
        lines.push(`  Evidence: ${item.adjudicated_evidence.join("; ")}`);
      }
      if (item.adjudicated_notes) {
        lines.push(`  Notes: ${item.adjudicated_notes}`);
      }
    }
  }
  lines.push("");
}

fs.writeFileSync(
  path.join(reportDir, "2026-03-13-locale-adversarial-second-pass.md"),
  lines.join("\n") + "\n",
  "utf8"
);
'@

node -e $script
