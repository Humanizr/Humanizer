// @ts-check

/** @typedef {Pick<import('@docusaurus/plugin-content-docs/client').GlobalVersion, 'label' | 'path'>} VersionRoute */
/** @typedef {{readonly to?: string}} VersionLink */
/** @typedef {{apiRoot: string, docsRoot: string, label: string, path: string, relativePath: string}} VersionRouteContext */

/**
 * @param {string} path
 * @returns {string}
 */
export function getDocsRoot(path) {
  return `${path.replace(/\/+$/, '')}/`;
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
