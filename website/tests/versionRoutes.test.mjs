import assert from 'node:assert/strict';
import test from 'node:test';
import {
  getUnsupportedVersionContext,
  getVersionRouteContext,
  preserveUnavailableVersionPath,
} from '../src/theme/versionRoutes.mjs';

const versions = [
  {label: '3.0.10 (latest)', name: '3.0.10', path: '/docs'},
  {label: '2.10.1', name: '2.10.1', path: '/docs/2.10.1'},
  {label: 'main/preview', name: 'current', path: '/docs/next'},
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

test('unsupported versions are limited to releases before 2.10.1', () => {
  assert.deepEqual(
    getUnsupportedVersionContext(
      '/docs/2.9.9/api/Humanizer/',
      versions,
    ),
    {
      apiRoot: '/docs/2.10.1/api/',
      docsRoot: '/docs/2.10.1/',
      earliestLabel: '2.10.1',
      requestedVersion: '2.9.9',
    },
  );
  assert.deepEqual(
    getUnsupportedVersionContext('/docs/v1.37/start/', versions),
    {
      apiRoot: '/docs/2.10.1/api/',
      docsRoot: '/docs/2.10.1/',
      earliestLabel: '2.10.1',
      requestedVersion: '1.37',
    },
  );
  assert.equal(
    getUnsupportedVersionContext('/docs/2.10.1/api/Missing/', versions),
    undefined,
  );
  assert.equal(
    getUnsupportedVersionContext('/docs/3.5/api/Missing/', versions),
    undefined,
  );
});

test('unsupported-version policy fails closed without published semver data', () => {
  assert.equal(
    getUnsupportedVersionContext('/docs/1.0/api/Missing/', [
      {label: 'main/preview', name: 'current', path: '/docs/next'},
    ]),
    undefined,
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
