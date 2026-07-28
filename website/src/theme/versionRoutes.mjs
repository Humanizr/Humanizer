// @ts-check

/** @typedef {Pick<import('@docusaurus/plugin-content-docs/client').GlobalVersion, 'label' | 'name' | 'path'>} VersionRoute */
/** @typedef {{readonly to?: string}} VersionLink */
/** @typedef {{apiRoot: string, docsRoot: string, label: string, path: string, relativePath: string}} VersionRouteContext */
/** @typedef {{apiRoot: string, docsRoot: string, earliestLabel: string, requestedVersion: string}} UnsupportedVersionContext */
/** @typedef {{parts: [number, number, number], version: string}} SemanticVersion */

/**
 * @param {string} path
 * @returns {string}
 */
export function getDocsRoot(path) {
  return `${path.replace(/\/+$/, '')}/`;
}

/**
 * @param {string} value
 * @returns {SemanticVersion | undefined}
 */
function parseSemanticVersion(value) {
  const match = value.match(/^v?(\d+)\.(\d+)(?:\.(\d+))?$/);
  return match
      ? {
        parts: [Number(match[1]), Number(match[2]), Number(match[3] ?? 0)],
        version: value.replace(/^v/, ''),
      }
    : undefined;
}

/**
 * @param {number[]} left
 * @param {number[]} right
 * @returns {number}
 */
function compareSemanticVersions(left, right) {
  for (let index = 0; index < 3; index += 1) {
    const comparison = left[index] - right[index];
    if (comparison !== 0) {
      return comparison;
    }
  }
  return 0;
}

/**
 * @param {string} pathname
 * @param {readonly VersionRoute[]} versions
 * @returns {UnsupportedVersionContext | undefined}
 */
export function getUnsupportedVersionContext(pathname, versions) {
  const requestedMatch = pathname.match(
    /^\/docs\/(v?\d+\.\d+(?:\.\d+)?)(?:\/|$)/,
  );
  const requested = requestedMatch
    ? parseSemanticVersion(requestedMatch[1])
    : undefined;
  if (!requested) {
    return undefined;
  }

  const published = versions
    .flatMap((version) => {
      const parsed = parseSemanticVersion(version.name);
      return parsed ? [{parsed, version}] : [];
    })
    .sort((left, right) =>
      compareSemanticVersions(left.parsed.parts, right.parsed.parts),
    );
  const earliest = published[0];
  if (
    !earliest ||
    compareSemanticVersions(requested.parts, earliest.parsed.parts) >= 0
  ) {
    return undefined;
  }

  const docsRoot = getDocsRoot(earliest.version.path);
  return {
    apiRoot: `${docsRoot}api/`,
    docsRoot,
    earliestLabel: earliest.version.label,
    requestedVersion: requested.version,
  };
}

/**
 * @param {string} pathname
 * @param {readonly VersionRoute[]} versions
 * @returns {VersionRouteContext | undefined}
 */
export function getVersionRouteContext(
  pathname,
  versions,
) {
  const orderedVersions = [...versions].sort(
    (left, right) => right.path.length - left.path.length,
  );
  const version = orderedVersions.find(({path}) => {
    const docsRoot = getDocsRoot(path);
    return pathname === docsRoot.slice(0, -1) || pathname.startsWith(docsRoot);
  });

  if (!version) {
    return undefined;
  }

  const docsRoot = getDocsRoot(version.path);
  return {
    apiRoot: `${docsRoot}api/`,
    docsRoot,
    label: version.label,
    path: version.path,
    relativePath: pathname.slice(docsRoot.length),
  };
}

/**
 * @template {VersionLink} T
 * @param {readonly T[]} items
 * @param {string} pathname
 * @param {readonly VersionRoute[]} versions
 * @returns {T[]}
 */
export function preserveUnavailableVersionPath(items, pathname, versions) {
  const current = getVersionRouteContext(pathname, versions);
  if (!current?.relativePath) {
    return [...items];
  }

  const versionRoots = new Set(versions.map(({path}) => getDocsRoot(path)));
  return items.map((item) => {
    if (!item.to) {
      return item;
    }

    const suffixIndex = item.to.search(/[?#]/);
    const targetPath =
      suffixIndex === -1 ? item.to : item.to.slice(0, suffixIndex);
    const targetRoot = getDocsRoot(targetPath);
    if (!versionRoots.has(targetRoot)) {
      return item;
    }

    const suffix = suffixIndex === -1 ? '' : item.to.slice(suffixIndex);
    return {
      ...item,
      'data-noBrokenLinkCheck': true,
      to: `${targetRoot}${current.relativePath}${suffix}`,
    };
  });
}
