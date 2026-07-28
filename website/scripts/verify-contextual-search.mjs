import assert from 'node:assert/strict';
import {readFile} from 'node:fs/promises';
import {createRequire} from 'node:module';
import {join, resolve} from 'node:path';

const require = createRequire(import.meta.url);
const lunr = require(
  '../node_modules/@cmfcmf/docusaurus-search-local/lib/lunr.js',
);
const siteRoot = resolve(process.argv[2] ?? 'build');
const manifest = JSON.parse(
  await readFile(new URL('../humanizer-versions.json', import.meta.url)),
);
const contexts = manifest.versions;
const versionRoots = contexts.map(({route}) =>
  route ? `/docs/${route}/` : '/docs/',
);

function resultRoutes(index, documents, query) {
  return index
    .search(query)
    .map(({ref}) =>
      documents.find((document) => String(document.id) === ref),
    )
    .filter(Boolean)
    .map(({sectionRoute}) => sectionRoute);
}

for (const [contextIndex, context] of contexts.entries()) {
  const path = join(
    siteRoot,
    `search-index-docs-default-${context.version}.json`,
  );
  const data = JSON.parse(await readFile(path));
  const root = versionRoots[contextIndex];
  const otherRoots = versionRoots.filter(
    (candidate, index) => index !== contextIndex && candidate !== '/docs/',
  );

  assert(data.documents.length > 0, `${context.label} index is empty.`);
  for (const {sectionRoute} of data.documents) {
    assert(
      sectionRoute.startsWith(root),
      `${context.label} contains an out-of-context route: ${sectionRoute}`,
    );
    assert(
      !otherRoots.some((otherRoot) => sectionRoute.startsWith(otherRoot)),
      `${context.label} contains another version route: ${sectionRoute}`,
    );
  }

  const index = lunr.Index.load(data.index);
  const cases = [
    {
      name: 'PascalCase API',
      query: 'StringHumanizeExtensions',
      route: 'api/Humanizer.StringHumanizeExtensions/',
    },
    {
      name: 'fully qualified API',
      query: 'Humanizer.StringHumanizeExtensions',
      route: 'api/Humanizer.StringHumanizeExtensions/',
    },
    {
      name: 'overload',
      query: 'Truncate',
      route:
        'api/Humanizer.TruncateExtensions/#truncateextensionstruncatethis-string-int',
    },
    {
      name: 'common prose',
      query: 'relative dates',
      route: 'scenarios/relative-dates-and-times/',
    },
  ];

  for (const searchCase of cases) {
    const routes = resultRoutes(index, data.documents, searchCase.query);
    assert(
      routes.some((route) => route.includes(searchCase.route)),
      `${context.label} ${searchCase.name} search missed ${searchCase.route}.`,
    );
  }

  console.log(
    `Contextual queries passed: ${context.label} (${data.documents.length} sections)`,
  );
}
