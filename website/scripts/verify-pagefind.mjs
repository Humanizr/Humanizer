import assert from 'node:assert/strict';
import {Buffer} from 'node:buffer';
import {readFile} from 'node:fs/promises';
import {createServer} from 'node:http';
import {extname, join, resolve, sep} from 'node:path';

const siteRoot = resolve(process.argv[2] ?? 'build');
const pagefindRoot = join(siteRoot, 'pagefind');
const manifest = JSON.parse(
  await readFile(new URL('../humanizer-versions.json', import.meta.url)),
);
const versions = manifest.versions;

const contentTypes = new Map([
  ['.js', 'text/javascript'],
  ['.json', 'application/json'],
  ['.wasm', 'application/wasm'],
]);
const server = createServer(async (request, response) => {
  try {
    const requestPath = decodeURIComponent(
      new URL(request.url, 'http://localhost').pathname,
    );
    const relativePath = requestPath.replace(/^\/pagefind\//, '');
    const filePath = resolve(pagefindRoot, relativePath);
    if (
      requestPath === relativePath ||
      (filePath !== pagefindRoot && !filePath.startsWith(`${pagefindRoot}${sep}`))
    ) {
      response.writeHead(404).end();
      return;
    }

    response.setHeader(
      'content-type',
      contentTypes.get(extname(filePath)) ?? 'application/octet-stream',
    );
    response.end(await readFile(filePath));
  } catch {
    response.writeHead(404).end();
  }
});

await new Promise((resolveListen) =>
  server.listen(0, '127.0.0.1', resolveListen),
);
const address = server.address();
assert(address && typeof address !== 'string');

try {
  const pagefindSource = await readFile(
    join(pagefindRoot, 'pagefind.js'),
    'utf8',
  );
  const pagefind = await import(
    `data:text/javascript;base64,${Buffer.from(pagefindSource).toString('base64')}`
  );
  await pagefind.options({
    basePath: `http://127.0.0.1:${address.port}/pagefind/`,
  });

  const search = await pagefind.search('StringHumanizeExtensions');
  const results = await Promise.all(
    search.results.map((result) => result.data()),
  );
  const filters = await pagefind.filters();

  for (const version of versions) {
    const route = version.route ? `${version.route}/` : '';
    const expectedUrl =
      `/docs/${route}api/Humanizer.StringHumanizeExtensions/`;
    const result = results.find(
      (candidate) =>
        new URL(candidate.url, 'https://humanizr.net').pathname === expectedUrl,
    );

    assert(
      result,
      `Pagefind did not return ${expectedUrl}. Results: ${results
        .map((candidate) => candidate.url)
        .join(', ')}`,
    );
    assert.equal(result.meta.version, version.label);
    assert.match(
      result.meta.title,
      new RegExp(version.label.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')),
    );
    assert(Object.hasOwn(filters.version ?? {}, version.label));
  }

  console.log(
    `Pagefind query passed: ${versions.length} exact version URLs and public labels.`,
  );
  await pagefind.destroy();
} finally {
  await new Promise((resolveClose, rejectClose) =>
    server.close((error) => (error ? rejectClose(error) : resolveClose())),
  );
}
