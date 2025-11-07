import React from 'react';
import Layout from '@theme/Layout';
import useBaseUrl from '@docusaurus/useBaseUrl';
import clsx from 'clsx';
import styles from './api.module.css';

const defaultManifest = {
  latest: 'main',
  releases: [],
  development: 'main'
};

const normalize = value => value?.toLowerCase() ?? '';

export default function ApiReference() {
  const manifestUrl = useBaseUrl('api/versions.json');
  const [manifest, setManifest] = React.useState(defaultManifest);
  const [selectedVersion, setSelectedVersion] = React.useState(defaultManifest.latest);
  const [manifestStatus, setManifestStatus] = React.useState('loading');
  const [toc, setToc] = React.useState(null);
  const [tocStatus, setTocStatus] = React.useState('idle');
  const [selectedItem, setSelectedItem] = React.useState(null);
  const [filter, setFilter] = React.useState('');
  const [expandedNamespaces, setExpandedNamespaces] = React.useState(() => new Set());

  React.useEffect(() => {
    let isMounted = true;
    setManifestStatus('loading');
    fetch(manifestUrl)
      .then(response => {
        if (!response.ok) {
          throw new Error(`Unable to load API manifest (${response.status})`);
        }
        return response.json();
      })
      .then(data => {
        if (!isMounted) {
          return;
        }
        const normalized = {
          latest: data.latest ?? 'main',
          releases: Array.isArray(data.releases) ? data.releases : [],
          development: data.development ?? 'main'
        };
        setManifest(normalized);
        setSelectedVersion(normalized.latest ?? normalized.development ?? 'main');
        setManifestStatus('ready');
      })
      .catch(error => {
        console.error('Failed to load API manifest', error);
        if (isMounted) {
          setManifestStatus('error');
        }
      });

    return () => {
      isMounted = false;
    };
  }, [manifestUrl]);

  const tocUrl = useBaseUrl(`api/${selectedVersion ?? 'main'}/api/toc.json`);

  React.useEffect(() => {
    if (!selectedVersion) {
      setToc(null);
      setSelectedItem(null);
      return;
    }

    const controller = new AbortController();
    setTocStatus('loading');
    fetch(tocUrl, {signal: controller.signal})
      .then(response => {
        if (!response.ok) {
          throw new Error(`Unable to load toc for ${selectedVersion}`);
        }
        return response.json();
      })
      .then(data => {
        const namespaces = Array.isArray(data.items) ? data.items : [];
        setToc({items: namespaces});
        const firstNamespace = namespaces.find(ns => Array.isArray(ns.items) && ns.items.length > 0);
        if (firstNamespace) {
          const firstType = firstNamespace.items[0];
          setSelectedItem({
            namespace: firstNamespace.name,
            name: firstType.name,
            href: firstType.href
          });
        } else {
          setSelectedItem(null);
        }
        setExpandedNamespaces(new Set());
        setTocStatus('ready');
      })
      .catch(error => {
        if (controller.signal.aborted) {
          return;
        }
        console.error('Failed to load API toc', error);
        setToc(null);
        setSelectedItem(null);
        setTocStatus('error');
      });

    return () => controller.abort();
  }, [selectedVersion, tocUrl]);

  const normalizedFilter = normalize(filter);

  const filteredNamespaces = React.useMemo(() => {
    if (!toc?.items) {
      return [];
    }

    if (!normalizedFilter) {
      return toc.items;
    }

    return toc.items
      .map(ns => {
        const children = Array.isArray(ns.items) ? ns.items : [];
        const namespaceMatches = normalize(ns.name).includes(normalizedFilter);
        const filteredChildren = children.filter(child => normalize(child.name).includes(normalizedFilter));
        if (namespaceMatches) {
          return {...ns};
        }
        if (filteredChildren.length > 0) {
          return {...ns, items: filteredChildren};
        }
        return null;
      })
      .filter(Boolean);
  }, [toc, normalizedFilter]);

  const currentApiPath = selectedItem && selectedVersion
    ? `api/${selectedVersion}/api/${selectedItem.href}`
    : null;
  const resolvedApiSrc = useBaseUrl(currentApiPath ?? '');
  const currentApiSrc = currentApiPath ? resolvedApiSrc : null;

  const allVersionOptions = React.useMemo(() => {
    const options = [];
    manifest.releases.forEach(version => {
      options.push({
        value: version,
        label: version === manifest.latest ? `Latest release (${version})` : `Release ${version}`
      });
    });

    if (!manifest.releases.includes(manifest.development)) {
      options.push({
        value: manifest.development,
        label: 'Latest dev (main)'
      });
    }

    if (options.length === 0) {
      options.push({
        value: manifest.development,
        label: 'Development (main)'
      });
    }

    return options;
  }, [manifest]);

  const toggleNamespace = name => {
    setExpandedNamespaces(prev => {
      const next = new Set(prev);
      if (next.has(name)) {
        next.delete(name);
      } else {
        next.add(name);
      }
      return next;
    });
  };

  const shouldExpandNamespace = name => normalizedFilter.length > 0 || expandedNamespaces.has(name) || (selectedItem?.namespace === name);

  return (
    <Layout title="API reference" description="DocFX generated API reference wrapped by Docusaurus">
      <div className="container padding-vert--md">
        <header className="margin-bottom--lg">
          <h1>API reference</h1>
          <p>
            Browse the Humanizer API surface across release branches (<code>rel/vN.N</code>) and the latest development build.
          </p>
          {manifestStatus === 'error' && (
            <div className="alert alert--warning" role="alert">
              Falling back to development docs because the version manifest could not be loaded.
            </div>
          )}
        </header>
        <div className={clsx('row', styles.page)}>
          <div className={clsx('col col--3', styles.sidebarColumn)}>
            <div className={styles.sidebarPanel}>
              <div className={styles.versionPicker}>
                <label htmlFor="api-version-select">Version</label>
                <select
                  id="api-version-select"
                  value={selectedVersion ?? ''}
                  onChange={event => setSelectedVersion(event.target.value)}
                  disabled={manifestStatus === 'loading'}
                >
                  {allVersionOptions.map(option => (
                    <option value={option.value} key={option.value}>
                      {option.label}
                    </option>
                  ))}
                </select>
              </div>
              <input
                type="search"
                className={styles.filterInput}
                placeholder="Filter namespaces or types"
                value={filter}
                onChange={event => setFilter(event.target.value)}
              />
              <div className={styles.namespaceList}>
                {tocStatus === 'loading' && (
                  <div className="alert alert--info" role="status">
                    Loading namespaces…
                  </div>
                )}
                {tocStatus === 'error' && (
                  <div className="alert alert--danger" role="alert">
                    Unable to load API navigation for this version.
                  </div>
                )}
                {tocStatus === 'ready' && filteredNamespaces.length === 0 && (
                  <div className={styles.emptyState}>No matches found.</div>
                )}
                {filteredNamespaces.map(namespace => {
                  const children = Array.isArray(namespace.items) ? namespace.items : [];
                  const expanded = shouldExpandNamespace(namespace.name);
                  return (
                    <div className={styles.namespaceGroup} key={namespace.name}>
                      <button
                        type="button"
                        className={styles.namespaceHeader}
                        onClick={() => toggleNamespace(namespace.name)}
                        aria-expanded={expanded}
                      >
                        {namespace.name}
                      </button>
                      {expanded && (
                        <ul className={styles.typeList}>
                          {children.map(child => (
                            <li key={child.href}>
                              <button
                                type="button"
                                className={clsx(styles.typeButton, {
                                  [styles.typeButtonActive]: selectedItem?.href === child.href
                                })}
                                onClick={() =>
                                  setSelectedItem({
                                    namespace: namespace.name,
                                    name: child.name,
                                    href: child.href
                                  })
                                }
                              >
                                {child.name}
                              </button>
                            </li>
                          ))}
                        </ul>
                      )}
                    </div>
                  );
                })}
              </div>
            </div>
          </div>
          <div className={clsx('col col--9', styles.contentColumn)}>
            {tocStatus === 'error' && (
              <div className="alert alert--warning" role="alert">
                Select another version or rebuild the API reference locally to continue.
              </div>
            )}
            {tocStatus !== 'error' && !currentApiSrc && (
              <div className={styles.emptyState}>Select a type on the left to view its API documentation.</div>
            )}
            {currentApiSrc && (
              <>
                <div className={clsx('margin-bottom--sm', styles.selectedContext)}>
                  <span>
                    Viewing: <strong>{selectedItem?.namespace}.{selectedItem?.name}</strong>
                  </span>
                  <a className="button button--sm button--secondary" href={currentApiSrc} target="_blank" rel="noreferrer">
                    Open in new tab
                  </a>
                </div>
                <iframe title="DocFX API reference" src={currentApiSrc} className={styles.previewFrame} loading="lazy" />
              </>
            )}
          </div>
        </div>
      </div>
    </Layout>
  );
}
