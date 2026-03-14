const fs = require("fs");
const path = require("path");

const locale = process.argv[2];
if (!locale) {
  console.error("Usage: node tools/Apply-AdjudicatedLocaleFix.js <locale>");
  process.exit(1);
}

const repoRoot = process.cwd();
const secondPassPath = path.join(repoRoot, "docs", "reviews", "second-pass", "locales", `${locale}.adjudication.json`);

function readJson(filePath) {
  let text = fs.readFileSync(filePath, "utf8");
  if (text.charCodeAt(0) === 0xfeff) {
    text = text.slice(1);
  }
  return JSON.parse(text);
}

function repairMojibake(value) {
  if (!value || !/[ÃÐÑØÙà]/.test(value)) {
    return value;
  }

  return Buffer.from(value, "latin1").toString("utf8");
}

function escapeRegExp(value) {
  return value.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
}

function asArray(value) {
  if (!value) {
    return [];
  }
  if (Array.isArray(value)) {
    return value;
  }
  if (typeof value === "string") {
    return [value];
  }
  return [];
}

if (!fs.existsSync(secondPassPath)) {
  throw new Error(`Adjudication file not found: ${secondPassPath}`);
}

const adjudication = readJson(secondPassPath);
const sourceReviewPath = path.join(repoRoot, adjudication.source_review.replace(/\\\\/g, path.sep));
const sourceReview = readJson(sourceReviewPath);
const resourcePath = path.join(repoRoot, sourceReview.resource_file.replace(/\\\\/g, path.sep));

let resourceText = fs.readFileSync(resourcePath, "utf8");
const changedKeys = [];
const replacements = [];

for (const finding of adjudication.findings || []) {
  if (!["confirmed", "modified"].includes(finding.adjudication)) {
    continue;
  }

  const currentValue = repairMojibake(finding.current_value);
  const replacementValue = repairMojibake(finding.adjudicated_replacement);
  if (currentValue === replacementValue) {
    continue;
  }

  replacements.push({ currentValue, replacementValue });

  const keyPattern = escapeRegExp(finding.key);
  const rx = new RegExp(`(<data name="${keyPattern}"[\\s\\S]*?<value>)([\\s\\S]*?)(</value>)`);
  if (rx.test(resourceText)) {
    resourceText = resourceText.replace(rx, `$1${replacementValue}$3`);
    changedKeys.push(finding.key);
  }
}

fs.writeFileSync(resourcePath, resourceText, "utf8");

const testDirs = new Set();
for (const dir of asArray(sourceReview.direct_test_directories)) {
  testDirs.add(path.join(repoRoot, dir.replace(/\\\\/g, path.sep)));
}
for (const child of asArray(sourceReview.owned_child_cultures)) {
  const childDir = path.join(repoRoot, "tests", "Humanizer.Tests", "Localisation", child);
  if (fs.existsSync(childDir)) {
    testDirs.add(childDir);
  }
}

const updatedTestFiles = [];
for (const dir of testDirs) {
  if (!fs.existsSync(dir)) {
    continue;
  }

  const stack = [dir];
  while (stack.length) {
    const current = stack.pop();
    for (const entry of fs.readdirSync(current, { withFileTypes: true })) {
      const fullPath = path.join(current, entry.name);
      if (entry.isDirectory()) {
        stack.push(fullPath);
        continue;
      }

      if (!entry.isFile() || !fullPath.endsWith(".cs")) {
        continue;
      }

      let content = fs.readFileSync(fullPath, "utf8");
      const original = content;
      for (const replacement of replacements) {
        if (replacement.currentValue && replacement.replacementValue &&
            replacement.currentValue.length >= 4 &&
            replacement.replacementValue.length >= 4) {
          content = content.split(replacement.currentValue).join(replacement.replacementValue);
        }
      }

      if (content !== original) {
        fs.writeFileSync(fullPath, content, "utf8");
        updatedTestFiles.push(fullPath);
      }
    }
  }
}

if (!changedKeys.length) {
  console.log(`No direct resource replacements applied for ${locale}.`);
} else {
  console.log(`Applied direct resource replacements for ${locale}:`);
  for (const key of [...changedKeys].sort()) {
    console.log(`- ${key}`);
  }
}

if (updatedTestFiles.length) {
  console.log(`Updated locale test files for ${locale}:`);
  for (const file of [...updatedTestFiles].sort()) {
    console.log(`- ${file}`);
  }
}
