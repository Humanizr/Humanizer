import assert from 'node:assert/strict';
import {readFile} from 'node:fs/promises';
import test from 'node:test';
import {fileURLToPath} from 'node:url';

const websiteRoot = fileURLToPath(new URL('..', import.meta.url));
const versions = JSON.parse(
  await readFile(new URL('../versions.json', import.meta.url)),
);
const predecessors = new Map([
  ...versions.slice(0, -1).map(
    (version, index) => [version, versions[index + 1]],
  ),
  ['2.10.1', '2.9.9'],
]);

test('every published snapshot has release-specific What’s New content', async () => {
  for (const version of versions) {
    const content = await readFile(
      `${websiteRoot}/versioned_docs/version-${version}/whats-new/index.mdx`,
      'utf8',
    );
    const predecessor = predecessors.get(version);

    assert.match(content, /^id: whats-new$/m, version);
    assert.match(
      content,
      new RegExp(
        `^title: What's new in Humanizer ${RegExp.escape(version)}$`,
        'm',
      ),
      version,
    );
    assert.match(
      content,
      new RegExp(
        `^# What's new in Humanizer ${RegExp.escape(version)}$`,
        'm',
      ),
      version,
    );
    assert.match(
      content,
      new RegExp(`https://github\\.com/Humanizr/Humanizer/releases/tag/v${RegExp.escape(version)}`),
      version,
    );
    assert.match(
      content,
      new RegExp(`https://github\\.com/Humanizr/Humanizer/compare/v${RegExp.escape(predecessor)}\\.\\.\\.v${RegExp.escape(version)}`),
      version,
    );
    assert.doesNotMatch(content, /\b4\.0\b|main\/preview|\/docs\/next\//, version);
  }
});

test('every published snapshot exposes What’s New in its own sidebar', async () => {
  for (const version of versions) {
    const sidebar = JSON.parse(
      await readFile(
        `${websiteRoot}/versioned_sidebars/version-${version}-sidebars.json`,
        'utf8',
      ),
    );

    assert.deepEqual(sidebar.docs.slice(0, 2), [
      'index',
      'whats-new/whats-new',
    ], version);
    assert.equal(
      sidebar.docs.filter((item) => item === 'whats-new/whats-new').length,
      1,
      version,
    );
  }
});
