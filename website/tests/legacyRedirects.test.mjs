import assert from 'node:assert/strict';
import test from 'node:test';
import {
  resolveInitialLegacyNavigation,
  resolveLegacyFragmentFromReferrer,
} from '../src/theme/legacyRedirects.mjs';

const origin = 'https://humanizr.net';

test('exact legacy sources map their query and fragment before navigation', () => {
  assert.equal(
    resolveInitialLegacyNavigation({
      hash: '#supported-locales',
      origin,
      pathname: '/docs/localization.md',
      referrer: '',
      search: '?source=legacy',
    }),
    '/docs/languages/supported-cultures/?source=legacy#orientation',
  );
});

test('same-origin exact legacy referrers can migrate a fragment', () => {
  assert.equal(
    resolveLegacyFragmentFromReferrer(
      '/docs/start/installation/',
      '?source=legacy',
      '#nuget-packages',
      `${origin}/installation/?source=legacy`,
      origin,
    ),
    '/docs/start/installation/?source=legacy#example',
  );
  assert.equal(
    resolveLegacyFragmentFromReferrer(
      '/docs/start/quick-start/',
      '?source=legacy',
      '#basic-examples',
      `${origin}/docs/quick-start.md/?source=legacy`,
      origin,
    ),
    '/docs/start/quick-start/?source=legacy#example',
  );
});

test('direct canonical visits and unrelated referrers cannot migrate fragments', () => {
  for (const referrer of [
    '',
    `${origin}/docs/not-a-legacy-source/`,
    'https://example.com/installation/',
  ]) {
    assert.equal(
      resolveLegacyFragmentFromReferrer(
        '/docs/start/installation/',
        '',
        '#nuget-packages',
        referrer,
        origin,
      ),
      undefined,
    );
  }
});

test('legacy provenance cannot migrate a fragment on an unrelated target', () => {
  assert.equal(
    resolveLegacyFragmentFromReferrer(
      '/docs/start/quick-start/',
      '',
      '#nuget-packages',
      `${origin}/installation/`,
      origin,
    ),
    undefined,
  );
});
