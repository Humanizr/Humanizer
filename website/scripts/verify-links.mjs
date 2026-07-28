import assert from 'node:assert/strict';
import {access, readFile} from 'node:fs/promises';
import {join, resolve} from 'node:path';

const buildRoot = resolve(process.argv[2] ?? 'build');
const inventory = JSON.parse(
  await readFile(new URL('../redirects.json', import.meta.url)),
);

function outputPath(source) {
  return join(buildRoot, source.replace(/^\//, ''), 'index.html');
}

assert.equal(inventory.schemaVersion, 1);
assert.match(
  inventory.staticPagesLimitation,
  /HTTP 200, not HTTP 301 or 308/,
);
assert.match(inventory.unsupportedVersionPaths.behavior, /without redirecting/);

await access(join(buildRoot, 'index.html'));
for (const redirect of inventory.redirects) {
  for (const source of redirect.from) {
    if (source.startsWith('/.')) {
      await assert.rejects(access(outputPath(source)));
      continue;
    }

    const html = await readFile(outputPath(source), 'utf8');
    assert.match(html, new RegExp(`url=${RegExp.escape(redirect.to)}`));
    assert.match(html, /window\.location\.search \+ window\.location\.hash/);
  }

  for (const destination of Object.values(redirect.fragments ?? {})) {
    const html = await readFile(outputPath(destination.to), 'utf8');
    assert.match(
      html,
      new RegExp(`id="${RegExp.escape(destination.fragment)}"`),
      `${destination.to} is missing #${destination.fragment}`,
    );
  }
}

for (const liveMarkdownLink of [
  '/docs/index.md',
  '/docs/localization.md',
  '/docs/migration-v3.md',
]) {
  assert(
    inventory.redirects.some(({from}) => from.includes(liveMarkdownLink)),
    `Current production link is missing from the inventory: ${liveMarkdownLink}`,
  );
}

const seoPages = [
  {
    canonical: 'https://humanizr.net/docs/start/quick-start/',
    path: 'docs/start/quick-start/index.html',
  },
  {
    canonical: 'https://humanizr.net/docs/2.14.1/start/quick-start/',
    path: 'docs/2.14.1/start/quick-start/index.html',
  },
  {
    canonical: 'https://humanizr.net/docs/next/start/quick-start/',
    noIndex: true,
    path: 'docs/next/start/quick-start/index.html',
  },
];

for (const page of seoPages) {
  const html = await readFile(join(buildRoot, page.path), 'utf8');
  assert.match(
    html,
    new RegExp(
      `<link[^>]+rel="canonical"[^>]+href="${RegExp.escape(page.canonical)}"`,
    ),
  );
  if (page.noIndex) {
    assert.match(html, /<meta[^>]+name="robots"[^>]+content="noindex/);
  } else {
    assert.doesNotMatch(html, /<meta[^>]+name="robots"[^>]+content="noindex/);
  }
  assert.match(html, /<h2[^>]+id="[^"]+"/);
  assert.match(html, /class="hash-link"/);
}

const robots = await readFile(join(buildRoot, 'robots.txt'), 'utf8');
assert.match(robots, /Sitemap: https:\/\/humanizr\.net\/sitemap\.xml/);

const sitemap = await readFile(join(buildRoot, 'sitemap.xml'), 'utf8');
assert.match(sitemap, /https:\/\/humanizr\.net\/docs\/start\/quick-start\//);
assert.match(
  sitemap,
  /https:\/\/humanizr\.net\/docs\/2\.14\.1\/start\/quick-start\//,
);
assert.doesNotMatch(sitemap, /https:\/\/humanizr\.net\/docs\/next\//);

console.log(
  `Links and SEO passed: ${inventory.redirects.length} reviewed redirect groups, canonical stable/history/preview rules, robots, sitemap, and linkable headings.`,
);
