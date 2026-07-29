import assert from 'node:assert/strict';
import {spawnSync} from 'node:child_process';
import {readFileSync} from 'node:fs';
import test from 'node:test';
import {fileURLToPath} from 'node:url';

const workflow = readFileSync(
  fileURLToPath(new URL('../../.github/workflows/docs.yml', import.meta.url)),
  'utf8',
);
const normalizedWorkflow = workflow.replaceAll('\r\n', '\n');
const locateStep = normalizedWorkflow
  .split('- name: Locate the currently deployable Pages run')[1]
  .split('- name: Inspect retained prior artifact')[0];
const fallbackSelection = locateStep.slice(
  locateStep.indexOf('if [[ -z "$prior" ]]; then'),
);
const filters = [
  ...fallbackSelection.matchAll(/jq -r '\n([\s\S]*?)\n\s+' <<<"\$runs"/g),
].map((match) => match[1]);

function selectRun(filter, workflowRuns) {
  const result = spawnSync('jq', ['-r', filter], {
    encoding: 'utf8',
    input: JSON.stringify([{workflow_runs: workflowRuns}]),
  });

  assert.equal(result.status, 0, result.stderr);
  return result.stdout.trimEnd();
}

function selectPrior(workflowRuns) {
  const documentation = selectRun(filters[0], workflowRuns);
  return documentation || selectRun(filters[1], workflowRuns);
}

const legacyRun = {
  created_at: '2026-07-28T18:17:11Z',
  head_sha: 'e7a24480ea2e5f796a0528842ef3f2743af4e59a',
  id: 30386738318,
  path: '.github/workflows/jekyll-gh-pages.yml',
};

test('first documentation deployment retains the latest legacy deployment', () => {
  assert.equal(filters.length, 2);
  assert.equal(
    selectPrior([legacyRun]),
    `${legacyRun.id}\t${legacyRun.head_sha}\t${legacyRun.path}`,
  );
});

test('subsequent deployments retain the latest successful documentation run', () => {
  const documentationRun = {
    ...legacyRun,
    created_at: '2026-07-29T00:00:00Z',
    head_sha: '0123456789abcdef0123456789abcdef01234567',
    id: 30410000000,
    path: '.github/workflows/docs.yml',
  };

  assert.equal(
    selectPrior([legacyRun, documentationRun]),
    `${documentationRun.id}\t${documentationRun.head_sha}\t${documentationRun.path}`,
  );
});

test('missing deployment history fails closed', () => {
  assert.equal(selectPrior([]), '');
});

test('release operations have repository context without a checkout', () => {
  const workflowEnv = normalizedWorkflow.match(/^env:\n([\s\S]*?)^\S/m)?.[1];
  assert.match(workflowEnv, /^  GH_REPO: \$\{\{ github\.repository \}\}$/m);
});

test('expensive documentation gates run in parallel before retention', () => {
  const validation = normalizedWorkflow.split('\n  validate:\n')[1].split('\n  build:\n')[0];
  const retention = normalizedWorkflow
    .split('\n  retain-pages-artifacts:\n')[1]
    .split('\n  deploy:\n')[0];

  assert.match(validation, /gate:\n          - manifests\n          - runtime/);
  assert.match(validation, /permissions:\n      actions: read\n      contents: read/);
  assert.match(validation, /\}\}-\$\{\{ matrix\.gate \}\}/);
  assert.match(retention, /needs:\n      - build\n      - validate/);
});
