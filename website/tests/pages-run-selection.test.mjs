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
const deploymentGuide = readFileSync(
  fileURLToPath(
    new URL('../docs/contributing/documentation.mdx', import.meta.url),
  ),
  'utf8',
);
const locateStep = normalizedWorkflow
  .split('- name: Locate the currently deployable Pages run')[1]
  .split('- name: Inspect retained prior artifact')[0];
const fallbackSelection = locateStep.slice(
  locateStep.indexOf('if [[ -z "$prior" ]]; then'),
);
const legacyPointerPolicy = locateStep.slice(
  0,
  locateStep.indexOf('if [[ -z "$prior" ]]; then'),
);
const rollbackSourceStep = normalizedWorkflow
  .split('- name: Validate rollback source')[1]
  .split('- name: Restore retained Pages artifact')[0];
const retainStep = normalizedWorkflow
  .split('- name: Retain and verify prior Pages artifact')[1]
  .split('  stage-pages-release:')[0];
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

function selectSuccessfulJobs(jobPages) {
  const result = spawnSync(
    'jq',
    [
      '-r',
      '[.[].jobs[] | select(.conclusion == "success") | .name] | sort | join(",")',
    ],
    {
      encoding: 'utf8',
      input: JSON.stringify(jobPages),
    },
  );

  assert.equal(result.status, 0, result.stderr);
  return result.stdout.trimEnd();
}

function parsePriorTsv(prior, pointer) {
  const script = `
set -euo pipefail
prior=$1
if [[ $2 == pointer ]]; then
  IFS=$'\\t' read -r run_id run_attempt source_sha source_path \\
    deployment_run_id stage_run_attempt deploy_run_attempt \\
    smoke_run_attempt publication_run_attempt bootstrap_source <<<"$prior"
  deployment_run_attempt=$publication_run_attempt
else
  IFS=$'\\t' read -r run_id run_attempt source_sha source_path <<<"$prior"
  deployment_run_id=$run_id
  deployment_run_attempt=$run_attempt
fi
printf '%s\\t%s\\t%s\\t%s\\t%s\\t%s\\n' \\
  "$run_id" "$run_attempt" "$source_sha" "$source_path" \\
  "$deployment_run_id" "$deployment_run_attempt"
`;
  const result = spawnSync('bash', ['-c', script, 'bash', prior, pointer], {
    encoding: 'utf8',
  });

  assert.equal(result.status, 0, result.stderr);
  return result.stdout.trimEnd();
}

function selectPrior(workflowRuns) {
  const documentation = selectRun(filters[0], workflowRuns);
  return documentation || selectRun(filters[1], workflowRuns);
}

function legacyIsCurrent(workflowRuns, legacyId) {
  const documentation = workflowRuns.filter(
    ({path}) => path === '.github/workflows/docs.yml',
  );
  const latestLegacy = workflowRuns
    .filter(({path}) => path === '.github/workflows/jekyll-gh-pages.yml')
    .sort((left, right) => left.created_at.localeCompare(right.created_at))
    .at(-1);

  return documentation.length === 0 && latestLegacy?.id === legacyId;
}

function bootstrapLifecycle(workflowRuns) {
  const retained = selectPrior(workflowRuns);
  if (!retained)
    return {recoverable: false, reselected: '', retained};

  const [runId, , , path] = retained.split('\t');
  const recoverable =
    path === '.github/workflows/docs.yml' ||
    legacyIsCurrent(workflowRuns, Number(runId));
  return {
    recoverable,
    reselected: recoverable ? retained : '',
    retained,
  };
}

function deploymentAssets(manifest) {
  return [
    `deployment-${manifest.deploymentRunId}-${manifest.publicationRunAttempt}.json`,
    `github-pages-${manifest.archiveRunId}-${manifest.archiveRunAttempt}.tar`,
    `github-pages-${manifest.archiveRunId}-${manifest.archiveRunAttempt}.tar.sha256`,
  ].sort();
}

function release({
  archiveAttempt = 1,
  archiveRunId = 100,
  bootstrapSource = false,
  deploymentAttempt = 1,
  deploymentRunId = 200,
  deployAttempt = deploymentAttempt,
  digestValid = true,
  draft = false,
  immutable = true,
  publishedAt = deploymentAttempt,
  smokeAttempt = deploymentAttempt,
  sourceSha = '0123456789abcdef0123456789abcdef01234567',
  stageAttempt = deploymentAttempt,
}) {
  const manifest = {
    archiveRunAttempt: archiveAttempt,
    archiveRunId,
    deploymentRunId,
    deployRunAttempt: deployAttempt,
    publicationRunAttempt: deploymentAttempt,
    schemaVersion: 1,
    smokeRunAttempt: smokeAttempt,
    sourceSha,
    stageRunAttempt: stageAttempt,
  };
  if (bootstrapSource) {
    manifest.bootstrapSource = true;
    delete manifest.deployRunAttempt;
    delete manifest.smokeRunAttempt;
    delete manifest.stageRunAttempt;
  }

  return {
    assets: deploymentAssets(manifest),
    digestValid,
    draft,
    immutable,
    manifest,
    publishedAt,
    tag: `docs-pages-deployment-v1-${deploymentRunId}-${deploymentAttempt}`,
  };
}

function validRelease(candidate) {
  const manifest = candidate.manifest;
  return (
    !candidate.draft &&
    candidate.immutable &&
    candidate.digestValid &&
    manifest.schemaVersion === 1 &&
    candidate.tag ===
      `docs-pages-deployment-v1-${manifest.deploymentRunId}-${manifest.publicationRunAttempt}` &&
    candidate.assets.join() === deploymentAssets(manifest).join()
  );
}

function selectRollback(releases, runId, attempt) {
  return releases
    .filter(validRelease)
    .filter(
      ({manifest}) =>
        manifest.archiveRunId === runId &&
        manifest.archiveRunAttempt === attempt,
    )
    .sort((left, right) => left.publishedAt - right.publishedAt)
    .at(-1);
}

function validEvidence(candidate, {archiveAttempts, deploymentAttempts}) {
  const manifest = candidate.manifest;
  const publication = deploymentAttempts[manifest.publicationRunAttempt];
  const archiveIsValid =
    archiveAttempts[manifest.archiveRunAttempt]?.build === 'success';
  if (manifest.bootstrapSource) {
    return (
      validRelease(candidate) &&
      manifest.archiveRunAttempt === 1 &&
      publication?.conclusion === 'success' &&
      archiveIsValid
    );
  }
  return (
    validRelease(candidate) &&
    ['success', 'failure'].includes(publication?.conclusion) &&
    archiveIsValid &&
    deploymentAttempts[manifest.stageRunAttempt]?.['stage-pages-release'] ===
      'success' &&
    deploymentAttempts[manifest.deployRunAttempt]?.deploy === 'success' &&
    deploymentAttempts[manifest.smokeRunAttempt]?.['production-smoke'] ===
      'success'
  );
}

function productionMatchesCandidate(production, candidate) {
  return (
    production.sourceSha === candidate.sourceSha &&
    production.archiveRunId === candidate.archiveRunId &&
    production.archiveRunAttempt === candidate.archiveRunAttempt
  );
}

function downloadedArtifactMatches(selected, embedded) {
  if (selected.path === '.github/workflows/jekyll-gh-pages.yml')
    return selected.attempt === 1;

  return (
    selected.path === '.github/workflows/docs.yml' &&
    selected.attempt === 1 &&
    embedded.sourceSha === selected.sourceSha
  );
}

const legacyRun = {
  created_at: '2026-07-28T18:17:11Z',
  head_sha: 'e7a24480ea2e5f796a0528842ef3f2743af4e59a',
  id: 30386738318,
  path: '.github/workflows/jekyll-gh-pages.yml',
  run_attempt: 1,
};

test('first documentation deployment retains the latest legacy deployment', () => {
  assert.equal(filters.length, 2);
  assert.equal(
    selectPrior([legacyRun]),
    `${legacyRun.id}\t${legacyRun.run_attempt}\t${legacyRun.head_sha}\t${legacyRun.path}`,
  );
  assert.match(
    normalizedWorkflow,
    /"\$pointer_path" == "\.github\/workflows\/jekyll-gh-pages\.yml"/,
  );
  assert.match(
    normalizedWorkflow,
    /"\$pointer_path" == "\.github\/workflows\/jekyll-gh-pages\.yml" &&\s+"\$pointer_event" != push/,
  );
});

test('pre-staging documentation archives remain honest bootstrap sources', () => {
  const bootstrap = release({
    archiveRunId: 30386738318,
    bootstrapSource: true,
    deploymentRunId: 30386738318,
    publishedAt: 1,
  });
  const bootstrapEvidence = {
    archiveAttempts: {1: {build: 'success'}},
    deploymentAttempts: {1: {conclusion: 'success'}},
  };
  const phased = release({
    archiveRunId: 30430000000,
    deploymentRunId: 30430000000,
    publishedAt: 2,
  });
  const phasedEvidence = {
    archiveAttempts: {1: {build: 'success'}},
    deploymentAttempts: {
      1: {
        conclusion: 'success',
        deploy: 'success',
        'production-smoke': 'success',
        'stage-pages-release': 'success',
      },
    },
  };
  const failedFirstDeployment = release({
    archiveRunId: 30420000000,
    deploymentRunId: 30420000000,
    draft: true,
    immutable: false,
    publishedAt: 2,
  });

  assert.equal(validEvidence(bootstrap, bootstrapEvidence), true);
  assert.equal(
    validEvidence(
      release({
        archiveAttempt: 2,
        archiveRunId: bootstrap.manifest.archiveRunId,
        bootstrapSource: true,
        deploymentRunId: bootstrap.manifest.deploymentRunId,
      }),
      {
        archiveAttempts: {2: {build: 'success'}},
        deploymentAttempts: {1: {conclusion: 'success'}},
      },
    ),
    false,
  );
  assert.equal(validEvidence(failedFirstDeployment, phasedEvidence), false);
  assert.equal(
    selectRollback(
      [bootstrap, failedFirstDeployment],
      bootstrap.manifest.archiveRunId,
      bootstrap.manifest.archiveRunAttempt,
    ),
    bootstrap,
  );
  assert.equal(
    validEvidence(bootstrap, {
      ...bootstrapEvidence,
      deploymentAttempts: {1: {conclusion: 'failure'}},
    }),
    false,
  );
  assert.equal(validEvidence(phased, phasedEvidence), true);
  assert.equal(
    [bootstrap, phased].sort(
      (left, right) => left.publishedAt - right.publishedAt,
    ).at(-1),
    phased,
  );
  assert.match(retainStep, /"bootstrapSource":true/);
  assert.doesNotMatch(
    retainStep,
    /"stageRunAttempt"|"deployRunAttempt"|"smokeRunAttempt"/,
  );
  assert.match(
    normalizedWorkflow,
    /"\$bootstrap_source" == true[\s\S]*?"\$pointer_conclusion" != success/,
  );
  assert.match(
    normalizedWorkflow,
    /manifest_bootstrap[\s\S]*?Rollback bootstrap publication is invalid/,
  );
  assert.match(
    normalizedWorkflow,
    /manifest_bootstrap[\s\S]*?"\$ROLLBACK_RUN_ATTEMPT" != 1[\s\S]*?Rollback bootstrap releases must use attempt one/,
  );
  assert.match(
    normalizedWorkflow,
    /"\$bootstrap_source" == true &&\s+"\$run_attempt" != 1/,
  );
});

test('bootstrap retention accepts only single-attempt artifacts with the historical identity shape', () => {
  const selected = {
    attempt: 1,
    path: '.github/workflows/docs.yml',
    runId: 30418201624,
    sourceSha: '567fa79f7d3f0563910bb73ed9ad27db5e82d58c',
  };
  const embedded = {
    sourceSha: selected.sourceSha,
  };

  assert.equal(downloadedArtifactMatches(selected, embedded), true);
  assert.equal(downloadedArtifactMatches({...selected, attempt: 2}, embedded), false);
  assert.equal(
    downloadedArtifactMatches(selected, {
      sourceSha: '0123456789abcdef0123456789abcdef01234567',
    }),
    false,
  );
  assert.equal(
    downloadedArtifactMatches(
      {...selected, path: '.github/workflows/jekyll-gh-pages.yml'},
      {},
    ),
    true,
  );
  assert.equal(
    downloadedArtifactMatches(
      {
        ...selected,
        attempt: 2,
        path: '.github/workflows/jekyll-gh-pages.yml',
      },
      {},
    ),
    false,
  );
  assert.match(
    retainStep,
    /tar -xOf prior-download\/artifact\.tar \.\/deployment\.json/,
  );
  assert.match(
    normalizedWorkflow,
    /- name: Download prior Pages artifact[\s\S]*?run-id: \$\{\{ steps\.prior\.outputs\.run_id \}\}/,
  );
  assert.match(
    retainStep,
    /\.sourceSha == \$sourceSha' \\\n\s+prior-download\/deployment\.json/,
  );
  assert.match(
    retainStep,
    /"\$PRIOR_RUN_ATTEMPT" != 1[\s\S]*?rerun pre-staging artifact cannot prove its exact attempt/,
  );
  assert.match(
    retainStep,
    /"\$PRIOR_RUN_ATTEMPT" != 1[\s\S]*?rerun legacy artifact cannot prove its exact attempt/,
  );
});

test('bootstrap selection skips reruns and continues to an older provable source', () => {
  const olderDocumentation = {
    ...legacyRun,
    created_at: '2026-07-29T01:00:00Z',
    head_sha: '0123456789abcdef0123456789abcdef01234567',
    id: 30410000000,
    path: '.github/workflows/docs.yml',
  };
  const rerun = {
    ...olderDocumentation,
    created_at: '2026-07-29T02:00:00Z',
    id: 30420000000,
    run_attempt: 2,
  };

  assert.equal(
    selectPrior([legacyRun, olderDocumentation, rerun]),
    `${olderDocumentation.id}\t1\t${olderDocumentation.head_sha}\t${olderDocumentation.path}`,
  );
  assert.equal(
    selectPrior([legacyRun, rerun]),
    '',
  );
  assert.equal(
    filters.every((filter) => filter.includes('.run_attempt == 1')),
    true,
  );
});

test('retain, recovery, and reselection agree after a modern attempt-two deployment', () => {
  const modernRerun = {
    ...legacyRun,
    created_at: '2026-07-29T02:00:00Z',
    id: 30420000000,
    path: '.github/workflows/docs.yml',
    run_attempt: 2,
  };
  const rollbackSupersession = rollbackSourceStep
    .split('latest_docs_run="$(')[1]
    .split('latest_legacy_run="$(')[0];
  const pointerSupersession = legacyPointerPolicy
    .split('latest_docs_run="$(')[1]
    .split('latest_legacy_run="$(')[0];

  assert.equal(legacyIsCurrent([legacyRun, modernRerun], legacyRun.id), false);
  assert.deepEqual(
    bootstrapLifecycle([legacyRun, modernRerun]),
    {recoverable: false, reselected: '', retained: ''},
  );
  const legacyIdentity =
    `${legacyRun.id}\t1\t${legacyRun.head_sha}\t${legacyRun.path}`;
  assert.deepEqual(
    bootstrapLifecycle([legacyRun]),
    {
      recoverable: true,
      reselected: legacyIdentity,
      retained: legacyIdentity,
    },
  );
  assert.doesNotMatch(rollbackSupersession, /run_attempt == 1/);
  assert.doesNotMatch(pointerSupersession, /run_attempt == 1/);
  assert.equal(
    filters.every((filter) => filter.includes('.run_attempt == 1')),
    true,
  );
  assert.match(
    filters[1],
    /if any\([\s\S]*?\.github\/workflows\/docs\.yml[\s\S]*?\) then\s+empty/,
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
    `${documentationRun.id}\t${documentationRun.run_attempt}\t${documentationRun.head_sha}\t${documentationRun.path}`,
  );
});

test('missing deployment history fails closed', () => {
  assert.equal(selectPrior([]), '');
});

test('release operations have repository context without a checkout', () => {
  const workflowEnv = normalizedWorkflow.match(/^env:\n([\s\S]*?)^\S/m)?.[1];
  assert.match(workflowEnv, /^  GH_REPO: \$\{\{ github\.repository \}\}$/m);
  assert.match(normalizedWorkflow, /defaults:\n  run:\n    shell: bash/);
  assert.doesNotMatch(normalizedWorkflow, /2>\/dev\/null \|\|\n\s+true/);
});

test('workflow job queries use the actual paginated slurp response shape', () => {
  const jobPages = [
    {
      jobs: [
        {conclusion: 'success', name: 'deploy'},
        {conclusion: 'failure', name: 'record-production-deployment'},
      ],
      total_count: 3,
    },
    {
      jobs: [{conclusion: 'success', name: 'production-smoke'}],
      total_count: 3,
    },
  ];
  const endpointCount =
    normalizedWorkflow.match(/\/jobs\?per_page=100/g)?.length ?? 0;
  const iteratorCount =
    normalizedWorkflow.match(/\.\[\]\.jobs\[\]/g)?.length ?? 0;

  assert.equal(
    selectSuccessfulJobs(jobPages),
    'deploy,production-smoke',
  );
  assert.ok(endpointCount > 0);
  assert.equal(iteratorCount, endpointCount);
  assert.doesNotMatch(normalizedWorkflow, /\.\[\]\[\]\.jobs\[\]/);
  assert.match(normalizedWorkflow, /\.\[\]\.workflow_runs\[\]/);
});

test('pointer and fallback TSVs preserve their distinct deployment identities', () => {
  const sourceSha = '0123456789abcdef0123456789abcdef01234567';
  const pointer = [
    100,
    1,
    sourceSha,
    '.github/workflows/docs.yml',
    200,
    1,
    2,
    2,
    3,
    false,
  ].join('\t');
  const fallback = [
    300,
    1,
    sourceSha,
    '.github/workflows/jekyll-gh-pages.yml',
  ].join('\t');

  assert.equal(
    parsePriorTsv(pointer, 'pointer'),
    `100\t1\t${sourceSha}\t.github/workflows/docs.yml\t200\t3`,
  );
  assert.equal(
    parsePriorTsv(fallback, 'fallback'),
    `300\t1\t${sourceSha}\t.github/workflows/jekyll-gh-pages.yml\t300\t1`,
  );
  assert.match(
    fallbackSelection,
    /read -r run_id run_attempt source_sha source_path \\\n\s+<<<"\$prior"[\s\S]*?deployment_run_id="\$run_id"[\s\S]*?deployment_run_attempt="\$run_attempt"/,
  );
  assert.doesNotMatch(
    fallbackSelection,
    /source_path \\\n\s+deployment_run_id deployment_run_attempt <<<"\$prior"/,
  );
});

test('expensive documentation gates run in parallel before retention', () => {
  const validation = normalizedWorkflow.split('\n  validate:\n')[1].split('\n  build:\n')[0];
  const retention = normalizedWorkflow
    .split('\n  retain-pages-artifacts:\n')[1]
    .split('\n  deploy:\n')[0];

  assert.match(validation, /gate:\n          - manifests\n          - runtime/);
  assert.match(validation, /permissions:\n      actions: read\n      contents: read/);
  assert.match(validation, /\}\}-\$\{\{ matrix\.gate \}\}/);
  assert.doesNotMatch(validation, /^    needs:/m);
  assert.match(retention, /needs:\n      - build\n      - validate/);
});

test('deployment release stays draft until production is verified', () => {
  const stage = normalizedWorkflow
    .split('\n  stage-pages-release:\n')[1]
    .split('\n  deploy:\n')[0];
  const deploy = normalizedWorkflow
    .split('\n  deploy:\n')[1]
    .split('\n  production-smoke:\n')[0];
  const record = normalizedWorkflow
    .split('\n  record-production-deployment:\n')[1]
    .split('\n  recover-after-production-failure:\n')[0];

  assert.match(stage, /docs-pages-candidate-v1-\$\{GITHUB_RUN_ID\}-\$\{GITHUB_RUN_ATTEMPT\}/);
  assert.match(stage, /gh release create "\$release_tag"[\s\S]*?--draft/);
  assert.match(deploy, /needs\.stage-pages-release\.result == 'success'/);
  assert.match(record, /needs\.production-smoke\.result == 'success'/);
  assert.match(record, /permissions:\n      actions: read\n      contents: write/);
  assert.match(record, /--method PATCH[\s\S]*?-F draft=false/);
  assert.match(record, /"\$draft" != false[\s\S]*?"\$immutable" != true/);
});

test('candidate staging tolerates eventual release-list visibility', () => {
  const sourceSha = '0123456789abcdef0123456789abcdef01234567';
  const stage = normalizedWorkflow
    .split('\n  stage-pages-release:\n')[1]
    .split('\n  deploy:\n')[0];
  const filter = stage.match(
    /jq -r --arg tag "\$release_tag" '\n([\s\S]*?)\n\s+'/,
  )?.[1];
  assert.ok(filter);
  assert.match(
    stage,
    /for attempt in \{1\.\.10\}; do[\s\S]*?release="\$\(find_candidate_release\)"[\s\S]*?\[\[ -n "\$release" \]\] && break[\s\S]*?sleep 1/,
  );

  const candidate = {
    assets: [
      {name: 'github-pages-100-1.tar.sha256'},
      {name: 'deployment-200-1.json'},
      {name: 'github-pages-100-1.tar'},
    ],
    draft: true,
    id: 300,
    immutable: false,
    tag_name: 'docs-pages-candidate-v1-200-1',
    target_commitish: sourceSha,
  };
  const select = (pages) =>
    spawnSync(
      'jq',
      ['-r', '--arg', 'tag', candidate.tag_name, filter],
      {
        encoding: 'utf8',
        input: JSON.stringify(pages),
      },
    );

  assert.equal(select([[]]).stdout, '');
  const visible = select([[], [candidate]]);
  assert.equal(visible.status, 0, visible.stderr);
  assert.equal(
    visible.stdout.trimEnd(),
    `300\ttrue\tfalse\t${sourceSha}\t3\tdeployment-200-1.json,github-pages-100-1.tar,github-pages-100-1.tar.sha256`,
  );
  assert.notEqual(select([[candidate, candidate]]).status, 0);
});

test('immutable releases contain exactly archive, checksum, and manifest', () => {
  assert.doesNotMatch(normalizedWorkflow, /gh release upload/);
  assert.doesNotMatch(normalizedWorkflow, /--method DELETE/);
  assert.match(
    normalizedWorkflow,
    /gh release create "\$release_tag" \\\n\s+"candidate\/\$\{archive_name\}" \\\n\s+"candidate\/\$\{checksum_name\}" \\\n\s+"candidate\/\$\{pointer_name\}"/,
  );
  assert.match(
    normalizedWorkflow,
    /gh release create "\$pointer_tag" \\\n\s+"final\/\$\{archive_name\}" \\\n\s+"final\/\$\{checksum_name\}" \\\n\s+"final\/\$\{pointer_name\}"/,
  );
  assert.match(normalizedWorkflow, /"\$asset_count" != 3/);
  assert.match(
    normalizedWorkflow,
    /"\$asset_names" != "\$\{pointer_name\},\$\{archive_name\},\$\{checksum_name\}"/,
  );
  assert.match(normalizedWorkflow, /sha256sum -c "\$checksum_name"/);
  assert.match(normalizedWorkflow, /cmp "candidate\/\$\{pointer_name\}"/);
});

test('rollback is bound to an exact successful run attempt', () => {
  assert.match(normalizedWorkflow, /inputs:\n\s+rollback_run_id:[\s\S]*?rollback_run_attempt:/);
  assert.match(
    normalizedWorkflow,
    /"\$run_attempt" != "\$ROLLBACK_RUN_ATTEMPT"/,
  );
  assert.match(
    normalizedWorkflow,
    /actions\/runs\/\$\{ROLLBACK_RUN_ID\}\/attempts\/\$\{ROLLBACK_RUN_ATTEMPT\}/,
  );
  assert.match(
    normalizedWorkflow,
    /actions\/runs\/\$\{deployment_run_id\}\/attempts\/\$\{deployment_run_attempt\}/,
  );
  assert.match(
    normalizedWorkflow,
    /actions\/runs\/\$\{run_id\}\/attempts\/\$\{run_attempt\}/,
  );
  assert.match(
    normalizedWorkflow,
    /github-pages-\$\{ROLLBACK_RUN_ID\}-\$\{ROLLBACK_RUN_ATTEMPT\}\.tar/,
  );
  assert.match(
    normalizedWorkflow,
    /\.archiveRunAttempt == \$archiveRunAttempt/,
  );
  assert.match(
    normalizedWorkflow,
    /-f "rollback_run_attempt=\$\{recovery_run_attempt\}"/,
  );
  assert.match(
    normalizedWorkflow,
    /-f "expected_pointer_run_attempt=\$\{expected_pointer_run_attempt\}"/,
  );
});

test('draft, malformed, incomplete, and digest-mismatched releases fail closed', () => {
  const selection = normalizedWorkflow
    .split('- name: Locate the currently deployable Pages run')[1]
    .split('- name: Inspect retained prior artifact')[0];

  assert.match(selection, /select\(\.draft == false and \.immutable == true\)/);
  assert.match(selection, /select\(\.schemaVersion == 1\)/);
  assert.match(selection, /"\$asset_count" != 3/);
  assert.match(selection, /"\$pointer_count" != 1/);
  assert.match(normalizedWorkflow, /sha256sum -c "\$checksum_name"/);

  assert.equal(validRelease(release({draft: true, immutable: false})), false);
  assert.equal(validRelease({...release({}), assets: []}), false);
  assert.equal(validRelease(release({digestValid: false})), false);
});

test('failed smoke or publication dispatches exact prior recovery', () => {
  const recovery = normalizedWorkflow.split(
    '\n  recover-after-production-failure:\n',
  )[1];

  assert.match(recovery, /needs\.production-smoke\.result == 'failure'/);
  assert.match(
    recovery,
    /needs\.record-production-deployment\.result == 'failure'/,
  );
  assert.match(
    recovery,
    /"\$recovery_run_id" == "\$REQUESTED_ROLLBACK_RUN_ID" &&\n\s+"\$recovery_run_attempt" == "\$REQUESTED_ROLLBACK_RUN_ATTEMPT"/,
  );
  assert.doesNotMatch(
    recovery,
    /SMOKE_RESULT[\s\S]*?"\$recovery_run_id" == "\$REQUESTED_ROLLBACK_RUN_ID"/,
  );
});

test('stage attempt one can publish after deploy and smoke succeed on attempt two', () => {
  const published = release({
    deploymentAttempt: 2,
    deploymentRunId: 300,
    deployAttempt: 2,
    smokeAttempt: 2,
    stageAttempt: 1,
  });
  const evidence = {
    archiveAttempts: {1: {build: 'success'}},
    deploymentAttempts: {
      1: {
        conclusion: 'failure',
        deploy: 'failure',
        'stage-pages-release': 'success',
      },
      2: {
        conclusion: 'success',
        deploy: 'success',
        'production-smoke': 'success',
        'record-production-deployment': 'success',
      },
    },
  };

  assert.equal(validEvidence(published, evidence), true);
  assert.equal(published.manifest.stageRunAttempt, 1);
  assert.equal(published.manifest.deployRunAttempt, 2);
  assert.equal(published.manifest.smokeRunAttempt, 2);
  assert.equal(published.manifest.publicationRunAttempt, 2);
  assert.match(
    normalizedWorkflow,
    /release_prefix="docs-pages-candidate-v1-\$\{GITHUB_RUN_ID\}-"/,
  );
  assert.match(
    normalizedWorkflow,
    /pointer_tag="docs-pages-deployment-v1-\$\{GITHUB_RUN_ID\}-\$\{GITHUB_RUN_ATTEMPT\}"/,
  );
});

test('stage and deploy attempt one can publish after smoke succeeds on attempt two', () => {
  const published = release({
    deploymentAttempt: 2,
    deploymentRunId: 301,
    deployAttempt: 1,
    smokeAttempt: 2,
    stageAttempt: 1,
  });
  const evidence = {
    archiveAttempts: {1: {build: 'success'}},
    deploymentAttempts: {
      1: {
        conclusion: 'failure',
        deploy: 'success',
        'production-smoke': 'failure',
        'stage-pages-release': 'success',
      },
      2: {
        conclusion: 'success',
        'production-smoke': 'success',
        'record-production-deployment': 'success',
      },
    },
  };

  assert.equal(validEvidence(published, evidence), true);
  assert.equal(published.manifest.deployRunAttempt, 1);
  assert.equal(published.manifest.smokeRunAttempt, 2);
});

test('publication remains bound to exact archive production identity', () => {
  const staged = release({deploymentRunId: 302});

  assert.match(
    normalizedWorkflow,
    /ARCHIVE_RUN_ATTEMPT: \$\{\{ needs\.build\.outputs\.archive_run_attempt \}\}/,
  );
  assert.match(
    normalizedWorkflow,
    /ARCHIVE_RUN_ID: \$\{\{ needs\.build\.outputs\.archive_run_id \}\}/,
  );
  assert.equal(productionMatchesCandidate(staged.manifest, staged.manifest), true);
  assert.equal(
    productionMatchesCandidate(
      {
        ...staged.manifest,
        archiveRunId: 99,
      },
      staged.manifest,
    ),
    false,
  );
  assert.equal(
    productionMatchesCandidate(
      {...staged.manifest, archiveRunAttempt: 2},
      staged.manifest,
    ),
    false,
  );
  assert.equal(
    productionMatchesCandidate(
      {
        ...staged.manifest,
        sourceSha: 'fedcba9876543210fedcba9876543210fedcba98',
      },
      staged.manifest,
    ),
    false,
  );
  const record = normalizedWorkflow
    .split('\n  record-production-deployment:\n')[1]
    .split('\n  recover-after-production-failure:\n')[0];
  const currentProductionGuard = record
    .split('> current-production.json')[1]
    ?.split('mkdir final')[0];
  assert.ok(currentProductionGuard);
  assert.match(
    currentProductionGuard,
    /--arg sourceSha "\$SOURCE_SHA"[\s\S]*?--argjson archiveRunId "\$archive_run_id"[\s\S]*?--argjson archiveRunAttempt "\$archive_run_attempt"/,
  );
  assert.match(
    currentProductionGuard,
    /\.sourceSha == \$sourceSha\s+and \.archiveRunId == \$archiveRunId\s+and \.archiveRunAttempt == \$archiveRunAttempt[\s\S]*?current-production\.json/,
  );
});

test('ambiguous publication recovers the remotely published candidate', () => {
  const prior = release({deploymentRunId: 399, publishedAt: 1});
  const remotelyPublished = release({
    archiveRunId: 400,
    deploymentRunId: 400,
    publishedAt: 2,
  });

  assert.equal(selectRollback([prior, remotelyPublished], 400, 1), remotelyPublished);
  assert.match(
    normalizedWorkflow,
    /"\$RECORD_RESULT" == failure/,
  );
  assert.match(
    normalizedWorkflow,
    /needs\.record-production-deployment\.result == 'failure'/,
  );
});

test('remote publication remains selectable after the record client fails', () => {
  const published = release({deploymentRunId: 400});
  const evidence = {
    archiveAttempts: {1: {build: 'success'}},
    deploymentAttempts: {
      1: {
        conclusion: 'failure',
        deploy: 'success',
        'production-smoke': 'success',
        'record-production-deployment': 'failure',
        'stage-pages-release': 'success',
      },
    },
  };

  assert.equal(validEvidence(published, evidence), true);
  assert.match(
    normalizedWorkflow,
    /"\$pointer_conclusion" != success &&\s+"\$pointer_conclusion" != failure/,
  );
});

test('every publication phase rejects missing or non-success evidence', () => {
  const published = release({deploymentRunId: 401});
  const valid = {
    archiveAttempts: {1: {build: 'success'}},
    deploymentAttempts: {
      1: {
        conclusion: 'success',
        deploy: 'success',
        'production-smoke': 'success',
        'stage-pages-release': 'success',
      },
    },
  };

  for (const status of [
    undefined,
    'failure',
    'cancelled',
    'timed_out',
    'action_required',
    null,
  ]) {
    for (const phase of [
      ['archiveAttempts', 'build'],
      ['deploymentAttempts', 'stage-pages-release'],
      ['deploymentAttempts', 'deploy'],
      ['deploymentAttempts', 'production-smoke'],
    ]) {
      const evidence = structuredClone(valid);
      evidence[phase[0]][1][phase[1]] = status;
      assert.equal(validEvidence(published, evidence), false);
    }
  }
  for (const conclusion of [
    undefined,
    'cancelled',
    'timed_out',
    'action_required',
    null,
  ]) {
    const evidence = structuredClone(valid);
    evidence.deploymentAttempts[1].conclusion = conclusion;
    assert.equal(validEvidence(published, evidence), false);
  }
  assert.match(
    normalizedWorkflow,
    /actions\/runs\/\$\{deployment_run_id\}\/attempts\/\$\{phase_attempt\}\/jobs/,
  );
  assert.match(
    normalizedWorkflow,
    /"\$archive_jobs" != "build"/,
  );
});

test('a full rerun publishes its new attempt-two candidate', () => {
  const first = release({deploymentRunId: 402});
  const second = release({
    archiveAttempt: 2,
    deploymentAttempt: 2,
    deploymentRunId: 402,
    deployAttempt: 2,
    smokeAttempt: 2,
    stageAttempt: 2,
  });

  assert.equal(second.manifest.stageRunAttempt, 2);
  assert.notEqual(second.assets.join(), first.assets.join());
  assert.match(normalizedWorkflow, /sort_by\(\.deployment_attempt\)[\s\S]*?\(last \/\/ empty\)/);
});

test('older attempts remain exact rollback sources after reruns', () => {
  const first = release({
    archiveAttempt: 1,
    archiveRunId: 500,
    deploymentAttempt: 1,
    deploymentRunId: 500,
  });
  const second = release({
    archiveAttempt: 2,
    archiveRunId: 500,
    deploymentAttempt: 2,
    deploymentRunId: 500,
  });

  assert.equal(selectRollback([first, second], 500, 1), first);
  assert.equal(selectRollback([first, second], 500, 2), second);
  assert.notEqual(first.assets[1], second.assets[1]);
});

test('stored attempt one is not replaced by latest rerun attempt two', () => {
  const runId = 600;
  const storedAttempt = 1;
  const latestAttempt = 2;
  const endpoint = `actions/runs/${runId}/attempts/${storedAttempt}`;

  assert.match(endpoint, /\/attempts\/1$/);
  assert.doesNotMatch(endpoint, new RegExp(`/attempts/${latestAttempt}$`));
});

test('rollback guidance uses the archive identity after redeployment', () => {
  const rolledForward = release({
    archiveAttempt: 1,
    archiveRunId: 700,
    deploymentAttempt: 2,
    deploymentRunId: 700,
  });

  assert.notEqual(
    rolledForward.manifest.archiveRunAttempt,
    rolledForward.manifest.publicationRunAttempt,
  );
  assert.match(deploymentGuide, /jq -r \.archiveRunId/);
  assert.match(deploymentGuide, /jq -r \.archiveRunAttempt/);
  assert.doesNotMatch(deploymentGuide, /gh run view .*--json .*attempt/);
});

test('deployment guidance distinguishes staged candidates from final releases', () => {
  assert.match(
    deploymentGuide,
    /draft `docs-pages-candidate-v1-<run-id>-<attempt>` prerelease/,
  );
  assert.match(
    deploymentGuide,
    /creates and publishes a[\s\S]*?`docs-pages-deployment-v1-<run-id>-<attempt>` release/,
  );
  assert.doesNotMatch(
    deploymentGuide,
    /draft `docs-pages-deployment-v1-<run-id>-<attempt>` prerelease/,
  );
});
