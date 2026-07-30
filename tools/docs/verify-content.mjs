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
let scenarioApiTargets = new Map();
if (areas.includes('scenarios')) {
  const scenarioApiContract = JSON.parse(
    await readFile(
      path.resolve(process.cwd(), 'scenario-api-contract.json'),
      'utf8',
    ),
  );
  if (scenarioApiContract.schemaVersion !== 1) {
    throw new Error('Unsupported scenario API contract schema.');
  }
  const scenarioPageContracts = new Map(
    Object.entries(scenarioApiContract.pages),
  );
  function resolveScenarioTargets(page, resolving = new Set()) {
    const entry = scenarioPageContracts.get(page);
    if (Array.isArray(entry)) {
      return entry;
    }
    if (!entry || !Array.isArray(entry.unionOf) || resolving.has(page)) {
      throw new Error(`Invalid scenario API union: ${page}`);
    }
    const nextResolving = new Set(resolving).add(page);
    return [...new Set(entry.unionOf.flatMap(
      (member) => resolveScenarioTargets(member, nextResolving),
    ))];
  }
  scenarioApiTargets = new Map(
    [...scenarioPageContracts.keys()].map(
      (page) => [page, resolveScenarioTargets(page)],
    ),
  );
  const canonicalScenarioPages = (
    await readdir(path.join(docsRoot, 'scenarios'))
  )
    .filter((name) => name !== 'index.mdx' && /\.mdx?$/.test(name))
    .map((name) => `scenarios/${name}`)
    .sort();
  if (
    JSON.stringify([...scenarioApiTargets.keys()].sort()) !==
    JSON.stringify(canonicalScenarioPages)
  ) {
    throw new Error(
      'The scenario API contract must exactly cover every non-index scenario page.',
    );
  }
}

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
    if (!metadata.title) {
      failures.push(`${relativePage}: missing title`);
    }
    if (!metadata.persona) {
      failures.push(`${relativePage}: missing primary persona`);
    }

    const hasExampleSection = /^## Example$/m.test(content);
    const hasExample = /!!raw-loader!.*Program\.cs/.test(content);
    const hasLabeledIllustration = metadata.example === 'illustrative' &&
      /```[a-z]+[\s\S]+?```/.test(content);
    if (
      (metadata.diataxis === 'tutorial' || hasExampleSection) &&
      !hasExample &&
      !hasLabeledIllustration
    ) {
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
    if (relatedHeading && links.length < 2) {
      failures.push(`${relativePage}: related section needs at least two links`);
    }
    if (
      relatedHeading &&
      !links.some((link) => /(^|\/)api(\/|$|\/index\.md$)/.test(link))
    ) {
      failures.push(`${relativePage}: related section needs a same-version API link`);
    }
    if (area === 'scenarios' && path.basename(page) !== 'index.mdx') {
      const contractPath = relativePage.replaceAll(path.sep, '/');
      if (!relatedHeading) {
        failures.push(`${relativePage}: focused scenario needs related API links`);
      }
      if (!scenarioApiTargets.has(contractPath)) {
        failures.push(`${relativePage}: missing scenario API contract`);
      }
      const rootOnly = links.filter((link) => /\/api\/?index\.md$/.test(link));
      if (rootOnly.length > 0) {
        failures.push(`${relativePage}: focused scenario cannot link only to the API root`);
      }
      const expectedTargets = [...(scenarioApiTargets.get(contractPath) ?? [])].sort();
      const actualTargets = [
        ...new Set(
          links
            .filter((link) => /^\.\.\/api\/[^/]+\.md$/.test(link))
            .map((link) => path.basename(link)),
        ),
      ].sort();
      if (JSON.stringify(actualTargets) !== JSON.stringify(expectedTargets)) {
        failures.push(
          `${relativePage}: API targets differ; actual ${actualTargets.join(', ')}; ` +
          `expected ${expectedTargets.join(', ')}`,
        );
      }
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
