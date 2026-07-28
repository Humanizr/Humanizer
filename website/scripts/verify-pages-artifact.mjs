import assert from 'node:assert/strict';
import {lstat, readFile, readdir} from 'node:fs/promises';
import {join, resolve} from 'node:path';

const buildRoot = resolve(process.argv[2] ?? 'build');
const bootstrapRollback = process.argv.includes('--bootstrap-rollback');
const requiredFiles = bootstrapRollback
  ? ['deployment.json', 'index.html']
  : [
      '.nojekyll',
      '404.html',
      'CNAME',
      'docs/2.10.1/index.html',
      'docs/index.html',
      'docs/next/index.html',
      'index.html',
      'pagefind/pagefind.js',
      'robots.txt',
      'search-index-docs-default-current.json',
      'sitemap.xml',
    ];

for (const path of requiredFiles) {
  const stats = await lstat(join(buildRoot, path));
  assert(stats.isFile(), `Pages artifact is missing ${path}.`);
}

if (!bootstrapRollback) {
  assert.equal(
    (await readFile(join(buildRoot, 'CNAME'), 'utf8')).trim(),
    'humanizr.net',
  );
  assert.match(
    await readFile(join(buildRoot, 'robots.txt'), 'utf8'),
    /Sitemap: https:\/\/humanizr\.net\/sitemap\.xml/,
  );
}

async function rejectLinks(directory) {
  for (const entry of await readdir(directory, {withFileTypes: true})) {
    const path = join(directory, entry.name);
    const stats = await lstat(path);
    assert(!stats.isSymbolicLink(), `Pages artifact contains a link: ${path}`);
    if (stats.isDirectory()) {
      await rejectLinks(path);
    } else {
      assert(stats.isFile(), `Pages artifact contains a special file: ${path}`);
      assert.equal(stats.nlink, 1, `Pages artifact contains a hard link: ${path}`);
    }
  }
}

await rejectLinks(buildRoot);
console.log(
  bootstrapRollback
    ? 'Bootstrap rollback artifact passed: deployment identity, homepage, and link safety.'
    : 'Pages artifact passed: custom domain, stable/history/preview roots, search, SEO, and link safety.',
);
