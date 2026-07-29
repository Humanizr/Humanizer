import assert from 'node:assert/strict';
import {readFile} from 'node:fs/promises';
import test from 'node:test';
import {fileURLToPath} from 'node:url';

const repositoryLogo = fileURLToPath(new URL('../../logo.png', import.meta.url));
const publicLogo = fileURLToPath(
  new URL('../static/img/logo.png', import.meta.url),
);
const socialLogo = fileURLToPath(
  new URL('../static/img/social-logo.png', import.meta.url),
);

test('documentation publishes the canonical Humanizer logo unchanged', async () => {
  assert.deepEqual(await readFile(publicLogo), await readFile(repositoryLogo));
});

test('social metadata image meets Open Graph dimensions', async () => {
  const image = await readFile(socialLogo);

  assert.equal(image.readUInt32BE(16), 200);
  assert.equal(image.readUInt32BE(20), 200);
});
