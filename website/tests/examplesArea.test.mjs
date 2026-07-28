import assert from 'node:assert/strict';
import {spawnSync} from 'node:child_process';
import path from 'node:path';
import test from 'node:test';
import {fileURLToPath} from 'node:url';

const websiteRoot = fileURLToPath(new URL('..', import.meta.url));
const repositoryRoot = path.dirname(websiteRoot);
const verifier = path.join(repositoryRoot, 'tools/docs/verify-examples.ps1');

test('example verification rejects a missing requested area', () => {
  const result = spawnSync(
    'pwsh',
    [
      verifier,
      '-Version', 'current',
      '-Area', 'Start,DoesNotExist',
      '-ExamplesRoot', path.join(websiteRoot, 'docs/_examples'),
    ],
    {
      cwd: repositoryRoot,
      encoding: 'utf8',
    },
  );

  assert.equal(result.status, 1);
  assert.match(
    result.stderr,
    /No runnable documentation examples were found in area DoesNotExist/,
  );
});
