import redirectInventory from '../../redirects.json' with {type: 'json'};

const redirects = redirectInventory.redirects;

function isInternalPath(path) {
  return path.startsWith('/') && !path.startsWith('//');
}

for (const redirect of redirects) {
  for (const source of [...redirect.from, ...(redirect.referrers ?? [])]) {
    if (!isInternalPath(source)) {
      throw new Error(`Legacy redirect source must be local: ${source}`);
    }
  }
  if (!isInternalPath(redirect.to)) {
    throw new Error(`Legacy redirect destination must be local: ${redirect.to}`);
  }
  for (const destination of Object.values(redirect.fragments ?? {})) {
    if (!isInternalPath(destination.to)) {
      throw new Error(
        `Legacy fragment destination must be local: ${destination.to}`,
      );
    }
  }
}

function fragmentName(hash) {
  if (!hash.startsWith('#') || hash.length === 1) {
    return undefined;
  }

  try {
    return decodeURIComponent(hash.slice(1));
  } catch {
    return undefined;
  }
}

function mappedFragment(redirect, hash) {
  const name = fragmentName(hash);
  return name ? redirect.fragments?.[name] : undefined;
}

export function findLegacyRedirect(pathname) {
  return redirects.find(({from}) => from.includes(pathname));
}

export function resolveLegacyRedirect(
  redirect,
  search,
  hash,
) {
  const mapped = mappedFragment(redirect, hash);
  if (mapped) {
    return `${mapped.to}${search}#${mapped.fragment}`;
  }

  return `${redirect.to}${search}${hash}`;
}

export function resolveLegacyFragmentFromReferrer(
  pathname,
  search,
  hash,
  referrer,
  origin,
) {
  let referrerUrl;
  try {
    referrerUrl = new URL(referrer);
  } catch {
    return undefined;
  }

  if (referrerUrl.origin !== origin) {
    return undefined;
  }

  const referrerPathnames = [
    referrerUrl.pathname,
    referrerUrl.pathname.replace(/\/$/, ''),
  ];
  const redirect = redirects.find(
    ({from, referrers = []}) =>
      referrerPathnames.some(
        (referrerPathname) =>
          from.includes(referrerPathname) ||
          referrers.includes(referrerPathname),
      ),
  );
  if (!redirect || redirect.to !== pathname) {
    return undefined;
  }

  const mapped = mappedFragment(redirect, hash);
  if (mapped) {
    return `${mapped.to}${search}#${mapped.fragment}`;
  }

  return undefined;
}

export function resolveInitialLegacyNavigation({
  hash,
  origin,
  pathname,
  referrer,
  search,
}) {
  const redirect = findLegacyRedirect(pathname);
  if (redirect) {
    return resolveLegacyRedirect(redirect, search, hash);
  }

  return resolveLegacyFragmentFromReferrer(
    pathname,
    search,
    hash,
    referrer,
    origin,
  );
}
