import assert from 'node:assert/strict';
import {readFile} from 'node:fs/promises';
import test from 'node:test';

const read = (path) => readFile(new URL(path, import.meta.url), 'utf8');

test('the 3.0.10 snapshot carries the verified 2.14.1 migration matrix', async () => {
  const [overlay, snapshot, sidebar] = await Promise.all([
    read('../version-overrides/3.0.10/upgrading/version-3-migration.mdx'),
    read('../versioned_docs/version-3.0.10/upgrading/version-3-migration.mdx'),
    read('../versioned_sidebars/version-3.0.10-sidebars.json'),
  ]);

  assert.equal(snapshot, overlay);
  assert.match(snapshot, /^## Consolidate namespaces \{#2-consolidate-namespaces\}$/m);
  assert.match(snapshot, /\| Affected caller or surface \| Break and migration from `2\.14\.1` to `3\.0\.10` \| Evidence \|/);
  assert.match(snapshot, /new DefaultFormatter\(null\)/);
  assert.match(snapshot, /GetSetMethod\(true\)/);
  assert.match(snapshot, /string\? value; value\.Humanize\(\)/);
  assert.match(snapshot, /DateOnly\.ToOrdinalWords/);
  assert.match(snapshot, /System\.Collections\.Immutable >= 9\.0\.10/);
  assert.match(snapshot, /Humanizer\.Core\.fr-BE/);
  assert.match(snapshot, /Humanizer\.Core\.fil-PH/);
  assert.match(snapshot, /"everything0"\.Dehumanize\(\).*Everything0.*Everything 0/);
  assert.match(snapshot, /Registry mutation after first resolution/);
  assert.match(snapshot, /Vietnamese negative numbers/);
  assert.match(sidebar, /upgrading\/version-3-migration/);
  assert.doesNotMatch(snapshot, /preserves no-letter strings in `Humanize`\/`Titleize`/);
});

test('the v4 migration guide is pinned to the audited live preview', async () => {
  const guide = await read('../docs/upgrading/version-4-migration.mdx');

  assert.match(guide, /146d74c025b80fa615bd8660f3c4c4949f61ac56/);
  assert.match(guide, /4\.0\.0-preview\.50/);
  assert.match(guide, /buildId=130158/);
  assert.match(guide, /three billion/);
  assert.match(guide, /Pick\(IComparable\)/);
  assert.match(guide, /"everything0"\.Dehumanize\(\).*Everything 0.*Everything0/);
  assert.match(guide, /`bs` feminine ordinal words \| English fallback `second` \| Bosnian `druga`/);
  assert.match(guide, /blob\/v3\.0\.10\/src\/Humanizer\/Configuration\/NumberToWordsConverterRegistry\.cs/);
  assert.match(guide, /Localisation\/bs\/BosnianLocaleParityTests\.cs/);
  assert.match(guide, /Localisation\/hr\/CroatianGenderedOrdinalTests\.cs/);
  assert.match(guide, /Localisation\/sl\/SlovenianGenderedOrdinalTests\.cs/);
  assert.match(guide, /Localisation\/sr\/SerbianGenderedOrdinalTests\.cs/);
  assert.match(guide, /Localisation\/sr-Latn\/SerbianLatinGenderedOrdinalTests\.cs/);
  assert.match(guide, /Humanizr\/Humanizer\/pull\/1829/);
  assert.doesNotMatch(guide, /42b876dd85a882e3ebee377d36be388b2fbb0b34/);
  assert.doesNotMatch(guide, /`a Great Movie` \| `A Great Movie`/);
  assert.doesNotMatch(guide, /`1\.00k` \| `1k`/);
});
