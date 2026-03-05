import React from 'react';
import OriginalDocSidebar from '@theme-original/DocSidebar';
import {useHistory, useLocation} from '@docusaurus/router';
import useBaseUrl from '@docusaurus/useBaseUrl';
import versionsTemplate from '@site/src/data/apiVersionDefaults.js';
import styles from './styles.module.css';

function getSlugFromPathname(pathname, options, defaultSlug) {
  if (!pathname.startsWith('/api')) {
    return defaultSlug;
  }

  const segments = pathname.slice('/api'.length).split('/').filter(Boolean);
  if (segments.length === 0) {
    return defaultSlug;
  }

  const known = options.find(option => option.slug === segments[0]);
  return known ? known.slug : defaultSlug;
}

export default function DocSidebarWrapper(props) {
  if (props.sidebarId !== 'apiSidebar') {
    return <OriginalDocSidebar {...props} />;
  }

  const history = useHistory();
  const location = useLocation();
  const versionsUrl = useBaseUrl('/api/versions.json');
  const [apiVersions, setApiVersions] = React.useState(versionsTemplate);
  const versionOptions = React.useMemo(
    () => [...apiVersions.releases, apiVersions.development],
    [apiVersions]
  );
  const defaultSlug = apiVersions.defaultSlug || apiVersions.development.slug;
  const currentSlug = getSlugFromPathname(location.pathname, versionOptions, defaultSlug);

  React.useEffect(() => {
    let cancelled = false;
    fetch(versionsUrl)
      .then(response => (response.ok ? response.json() : null))
      .then(data => {
        if (data && !cancelled) {
          setApiVersions(data);
        }
      })
      .catch(() => {
        // ignore fetch errors; template data already loaded
      });
    return () => {
      cancelled = true;
    };
  }, [versionsUrl]);

  const handleVersionChange = event => {
    const targetSlug = event.target.value;
    const base = '/api';
    const segments = location.pathname.startsWith(base)
      ? location.pathname.slice(base.length).split('/').filter(Boolean)
      : [];

    let restSegments = segments;
    if (segments.length > 0 && versionOptions.some(option => option.slug === segments[0])) {
      restSegments = segments.slice(1);
    }

    const restPath = restSegments.join('/');
    const nextPath = restPath ? `${base}/${targetSlug}/${restPath}` : `${base}/${targetSlug}`;
    history.push(nextPath);
  };

  return (
    <div className={styles.apiSidebarWrapper}>
      <div className={styles.versionPicker}>
        <label htmlFor="api-version-select">API version</label>
        <select id="api-version-select" value={currentSlug} onChange={handleVersionChange}>
          {apiVersions.releases.map(release => (
            <option value={release.slug} key={release.slug}>
              {release.label}
            </option>
          ))}
          <option value={apiVersions.development.slug}>{apiVersions.development.label}</option>
        </select>
      </div>
      <OriginalDocSidebar {...props} />
    </div>
  );
}
