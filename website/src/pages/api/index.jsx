import React from 'react';
import Layout from '@theme/Layout';
import useBaseUrl from '@docusaurus/useBaseUrl';

const defaultManifest = {
  latest: 'main',
  releases: [],
  development: 'main'
};

export default function ApiReference() {
  const manifestUrl = useBaseUrl('api/versions.json');
  const [manifest, setManifest] = React.useState(defaultManifest);
  const [selectedVersion, setSelectedVersion] = React.useState(defaultManifest.latest);
  const [status, setStatus] = React.useState('loading');

  React.useEffect(() => {
    fetch(manifestUrl)
      .then(response => {
        if (!response.ok) {
          throw new Error(`Unable to load API manifest (${response.status})`);
        }
        return response.json();
      })
      .then(data => {
        setManifest({
          latest: data.latest ?? 'main',
          releases: Array.isArray(data.releases) ? data.releases : [],
          development: data.development ?? 'main'
        });
        setSelectedVersion(data.latest ?? data.development ?? 'main');
        setStatus('ready');
      })
      .catch(error => {
        console.error('Failed to load API manifest', error);
        setStatus('error');
      });
  }, [manifestUrl]);

  const allOptions = React.useMemo(() => {
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

  const currentSrc = selectedVersion ? useBaseUrl(`api/${selectedVersion}/index.html`) : undefined;

  return (
    <Layout title="API reference" description="DocFX generated API reference wrapped by Docusaurus">
      <main className="container margin-vert--xl">
        <h1>API reference</h1>
        <p>
          API reference pages are generated with DocFX for every release branch (<code>rel/vN.N</code>) and the latest
          development build.
        </p>
        <div className="apiControls">
          <label htmlFor="api-version-select">Version</label>
          <select
            id="api-version-select"
            value={selectedVersion}
            onChange={event => setSelectedVersion(event.target.value)}
          >
            {allOptions.map(option => (
              <option value={option.value} key={option.value}>
                {option.label}
              </option>
            ))}
          </select>
          {status === 'ready' && selectedVersion && (
            <a className="button button--sm button--secondary" href={currentSrc} target="_blank" rel="noreferrer">
              Open in new tab
            </a>
          )}
        </div>
        {status === 'error' && (
          <div className="alert alert--danger" role="alert">
            Unable to load API versions. Please try again later.
          </div>
        )}
        {status === 'loading' && (
          <div className="alert alert--info" role="status">
            Loading API reference…
          </div>
        )}
        {status === 'ready' && currentSrc && (
          <iframe
            title="DocFX API reference"
            src={currentSrc}
            className="apiFrame"
            loading="lazy"
          />
        )}
        {status === 'ready' && !currentSrc && (
          <div className="alert alert--warning" role="alert">
            No API documentation is available for the selected version.
          </div>
        )}
      </main>
    </Layout>
  );
}
