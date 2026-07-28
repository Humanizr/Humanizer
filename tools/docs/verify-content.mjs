import {readdir, readFile} from 'node:fs/promises';
import path from 'node:path';
import process from 'node:process';

const docsRoot = path.resolve(process.cwd(), 'docs');
const requestedAreas = process.argv.slice(2);
const areas = requestedAreas.length > 0
  ? requestedAreas
  : ['start', 'scenarios', 'concepts', 'upgrading', 'analyzer', 'languages', 'contributing'];
const roles = new Set(['tutorial', 'how-to', 'explanation', 'reference']);
const prohibitedProductName = ['Flow', 'Next'].join(' ');
const failures = [];

for (const area of areas) {
  const areaRoot = path.join(docsRoot, area);
  let entries;
  try {
    entries = await readdir(areaRoot, {withFileTypes: true});
  } catch {
    failures.push(`${area}: documentation area does not exist`);
    continue;
  }

  const pages = entries
    .filter((entry) => entry.isFile() && /\.mdx?$/.test(entry.name))
    .map((entry) => path.join(areaRoot, entry.name));
  if (pages.length === 0) {
    failures.push(`${area}: no documentation pages found`);
    continue;
  }

  for (const page of pages) {
    const relativePage = path.relative(docsRoot, page);
    const content = await readFile(page, 'utf8');
    const frontMatter = content.match(/^---\r?\n([\s\S]*?)\r?\n---\r?\n/);
    if (!frontMatter) {
      failures.push(`${relativePage}: missing front matter`);
      continue;
    }

    const metadata = Object.fromEntries(
      frontMatter[1]
        .split(/\r?\n/)
        .map((line) => line.match(/^([a-z_]+):\s*(.+)$/))
        .filter(Boolean)
        .map((match) => [match[1], match[2].trim()]),
    );
    if (!roles.has(metadata.diataxis)) {
      failures.push(`${relativePage}: diataxis must name one supported role`);
    }
    if (!metadata.persona) {
      failures.push(`${relativePage}: missing primary persona`);
    }

    for (const section of [
      'Orientation',
      'Example',
      'Pitfall',
      'Version notes',
      'Related guides and API',
    ]) {
      if (!new RegExp(`^## ${section}$`, 'm').test(content)) {
        failures.push(`${relativePage}: missing "${section}" section`);
      }
    }

    const hasExample = /!!raw-loader!.*Program\.cs/.test(content);
    const hasLabeledIllustration = metadata.example === 'illustrative' &&
      /```[a-z]+[\s\S]+?```/.test(content);
    if (!hasExample && !hasLabeledIllustration) {
      failures.push(
        `${relativePage}: example must import tested source or be labeled illustrative`,
      );
    }

    const relatedHeading = content.match(/^## Related guides and API\r?\n/m);
    const contentAfterRelated = relatedHeading
      ? content.slice((relatedHeading.index ?? 0) + relatedHeading[0].length)
      : '';
    const nextHeadingIndex = contentAfterRelated.search(/^## /m);
    const related = nextHeadingIndex >= 0
      ? contentAfterRelated.slice(0, nextHeadingIndex)
      : contentAfterRelated;
    const links = [...related.matchAll(/\[[^\]]+\]\(([^)]+)\)/g)]
      .map((match) => match[1]);
    if (links.length < 2) {
      failures.push(`${relativePage}: related section needs at least two links`);
    }
    if (!links.some((link) => /(^|\/)api(\/|$|\/index\.md$)/.test(link))) {
      failures.push(`${relativePage}: related section needs a same-version API link`);
    }

    if (metadata.diataxis === 'tutorial' && !/^## Result$/m.test(content)) {
      failures.push(`${relativePage}: tutorial needs a deterministic result`);
    }
    if (content.toLowerCase().includes(prohibitedProductName.toLowerCase())) {
      failures.push(`${relativePage}: contains prohibited product wording`);
    }
  }
}

if (failures.length > 0) {
  console.error(failures.join('\n'));
  process.exitCode = 1;
} else {
  console.log(`Content checks passed: ${areas.join(', ')}`);
}
