import assert from 'node:assert/strict';
import {mkdtemp, mkdir, rm, writeFile} from 'node:fs/promises';
import {tmpdir} from 'node:os';
import path from 'node:path';
import {spawnSync} from 'node:child_process';
import test from 'node:test';
import {fileURLToPath} from 'node:url';

const checker = fileURLToPath(
  new URL('../../tools/docs/verify-content.mjs', import.meta.url),
);
const concisePage = `---
title: Configure a task
diataxis: how-to
persona: existing user
---

Choose a task.
`;
const validPage = `---
title: Configure a task
diataxis: how-to
persona: existing user
---

## Example

This fragment is illustrative:

\`\`\`csharp
Console.WriteLine("illustrative");
\`\`\`
`;
const relatedPage = `${concisePage}
## Related guides and API

- [Guide](../start/quick-start.mdx)
- [API](../api/index.md)
`;

async function withFixture(page, run) {
  const root = await mkdtemp(path.join(tmpdir(), 'humanizer-content-check-'));
  try {
    const area = path.join(root, 'docs', 'sample');
    await mkdir(area, {recursive: true});
    await writeFile(path.join(area, 'page.md'), page);
    await run(root);
  } finally {
    await rm(root, {recursive: true, force: true});
  }
}

test('content checker accepts a concise page without template sections', async () => {
  await withFixture(concisePage, (cwd) => {
    const result = spawnSync(process.execPath, [checker, 'sample'], {
      cwd,
      encoding: 'utf8',
    });

    assert.equal(result.status, 0, result.stderr);
  });
});

test('content checker requires a page title', async () => {
  await withFixture(concisePage.replace('title: Configure a task\n', ''), (cwd) => {
    const result = spawnSync(process.execPath, [checker, 'sample'], {
      cwd,
      encoding: 'utf8',
    });

    assert.equal(result.status, 1);
    assert.match(result.stderr, /missing title/);
  });
});

test('content checker accepts a related section that ends at EOF', async () => {
  await withFixture(relatedPage, (cwd) => {
    const result = spawnSync(process.execPath, [checker, 'sample'], {
      cwd,
      encoding: 'utf8',
    });

    assert.equal(result.status, 0, result.stderr);
  });
});

test('content checker rejects an unlabeled illustrative fragment', async () => {
  await withFixture(validPage, (cwd) => {
    const result = spawnSync(process.execPath, [checker, 'sample'], {
      cwd,
      encoding: 'utf8',
    });

    assert.equal(result.status, 1);
    assert.match(
      result.stderr,
      /must import tested source or be labeled illustrative/,
    );
  });
});

test('content checker accepts a labeled illustrative fragment', async () => {
  const fragment = validPage.replace(
    'persona: existing user',
    'persona: existing user\nexample: illustrative',
  );

  await withFixture(fragment, (cwd) => {
    const result = spawnSync(process.execPath, [checker, 'sample'], {
      cwd,
      encoding: 'utf8',
    });

    assert.equal(result.status, 0, result.stderr);
  });
});

test('content checker requires an example and result for tutorials', async () => {
  const tutorial = concisePage.replace('how-to', 'tutorial');

  await withFixture(tutorial, (cwd) => {
    const result = spawnSync(process.execPath, [checker, 'sample'], {
      cwd,
      encoding: 'utf8',
    });

    assert.equal(result.status, 1);
    assert.match(result.stderr, /example must import tested source/);
    assert.match(result.stderr, /tutorial needs a deterministic result/);
  });
});
