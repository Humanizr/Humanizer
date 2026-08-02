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

test('the v4 migration guide is pinned to live source and the latest main preview', async () => {
  const guide = await read('../docs/upgrading/version-4-migration.mdx');

  assert.match(guide, /087cde3f5de637a2a8e6bbaacd7451a9f5f28e28/);
  assert.match(guide, /4\.0\.0-preview\.51/);
  assert.match(guide, /buildId=130178/);
  assert.doesNotMatch(guide, /4\.0\.0-preview\.50/);
  assert.doesNotMatch(guide, /buildId=130158/);
  assert.match(guide, /Humanizr\/Humanizer\/pull\/1931/);
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
  assert.match(guide, /numeric fallback `2` \| authored word `druga` \(`друга` in Serbian Cyrillic\)/);
  assert.match(guide, /blob\/v3\.0\.10\/src\/Humanizer\/Localisation\/NumberToWords\/CroatianNumberToWordsConverter\.cs/);
  assert.match(guide, /blob\/v3\.0\.10\/src\/Humanizer\/Localisation\/NumberToWords\/SlovenianNumberToWordsConverter\.cs/);
  assert.match(guide, /blob\/v3\.0\.10\/src\/Humanizer\/Localisation\/NumberToWords\/SerbianCyrlNumberToWordsConverter\.cs/);
  assert.match(guide, /blob\/v3\.0\.10\/src\/Humanizer\/Localisation\/NumberToWords\/SerbianNumberToWordsConverter\.cs/);
  assert.match(guide, /Humanizr\/Humanizer\/pull\/1829/);
  assert.match(guide, /`cs`, `sk`, and `ro` clock notation at minute one or two/);
  assert.match(guide, /`pět jeden`; `five two`; `cinci doi`/);
  assert.match(guide, /`pět hodin jedna minuta`; `päť hodín dve minúty`; `ora cinci și două`/);
  assert.match(guide, /TimeOnlyToClockNotationConvertersRegistry\.cs/);
  assert.match(guide, /Localisation\/cs\/CzechClockNotationTests\.cs/);
  assert.match(guide, /Localisation\/sk\/SlovakClockNotationTests\.cs/);
  assert.match(guide, /Localisation\/ro\/RomanianLocaleRegressionTests\.cs/);
  assert.doesNotMatch(guide, /42b876dd85a882e3ebee377d36be388b2fbb0b34/);
  assert.doesNotMatch(guide, /`a Great Movie` \| `A Great Movie`/);
  assert.doesNotMatch(guide, /`1\.00k` \| `1k`/);
});
