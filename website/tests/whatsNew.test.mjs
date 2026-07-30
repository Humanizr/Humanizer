import assert from 'node:assert/strict';
import {access, readFile} from 'node:fs/promises';
import path from 'node:path';
import test from 'node:test';

const pagePath = new URL('../docs/whats-new/index.mdx', import.meta.url);
const page = await readFile(pagePath, 'utf8');

test("Humanizer 4 overview stays short and feature-focused", () => {
  const headings = [...page.matchAll(/^## (.+)$/gm)].map((match) => match[1]);
  assert.deepEqual(headings, [
    'Install one package and reach every supported culture',
    'Make durations fit the interface',
    'Choose exactly what a byte unit means',
    'Read and write richer numbers',
    'Preserve the text that carries meaning',
  ]);

  const words = page.match(/\b[\p{L}\p{N}][\p{L}\p{N}'’-]*\b/gu) ?? [];
  assert(words.length >= 550 && words.length <= 1000);

  for (const section of page.split(/^## /m).slice(1))
    assert((section.match(/^```csharp$/gm) ?? []).length <= 1);

  for (const prohibited of [
    '## Orientation',
    '## Pitfall',
    '## Version notes',
    'PluralizationForms',
    'TryPluralize',
    'TrySingularize',
    '#1892',
    'Assert.Equal',
  ])
    assert(!page.includes(prohibited), `Unexpected overview content: ${prohibited}`);
});

test('Humanizer 4 overview links to current deeper guides', async () => {
  const localLinks = [...page.matchAll(/\[[^\]]+\]\((\.\.\/[^)]+)\)/g)]
    .map((match) => match[1].split('#', 1)[0]);

  assert(localLinks.length >= 8);
  await Promise.all(localLinks.map((link) =>
    access(path.resolve(path.dirname(pagePath.pathname), link))));
});
