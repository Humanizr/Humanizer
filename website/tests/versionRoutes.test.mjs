import assert from 'node:assert/strict';
import test from 'node:test';
import {
  getVersionRouteContext,
  preserveUnavailableVersionPath,
} from '../src/theme/versionRoutes.mjs';

const versions = [
  {label: '3.0.10 (latest)', path: '/docs'},
  {label: 'main/preview', path: '/docs/next'},
];

test('version context prefers the explicit preview route', () => {
  assert.deepEqual(
    getVersionRouteContext('/docs/next/api/Missing/', versions),
    {
      apiRoot: '/docs/next/api/',
      docsRoot: '/docs/next/',
      label: 'main/preview',
      path: '/docs/next',
      relativePath: 'api/Missing/',
    },
  );
});

test('version switch preserves a path only when Docusaurus fell back to a root', () => {
  const items = [
    {label: '3.0.10', to: '/docs/api/Humanizer.ByteSize/'},
    {
      label: 'main/preview',
      onClick: 'save preferred version',
      to: '/docs/next?query=bytes#members',
    },
  ];

  assert.deepEqual(
    preserveUnavailableVersionPath(
      items,
      '/docs/api/Humanizer.ByteSize/',
      versions,
    ),
    [
      {label: '3.0.10', to: '/docs/api/Humanizer.ByteSize/'},
      {
        'data-noBrokenLinkCheck': true,
        label: 'main/preview',
        onClick: 'save preferred version',
        to: '/docs/next/api/Humanizer.ByteSize/?query=bytes#members',
      },
    ],
  );
});

test('version switch leaves a version root unchanged from a version root', () => {
  assert.deepEqual(
    preserveUnavailableVersionPath(
      [{label: 'main/preview', to: '/docs/next/'}],
      '/docs/',
      versions,
    ),
    [{label: 'main/preview', to: '/docs/next/'}],
  );
});
