import Head from '@docusaurus/Head';
import Link from '@docusaurus/Link';
import {useVersions} from '@docusaurus/plugin-content-docs/client';
import {useLocation} from '@docusaurus/router';
import NotFoundContent from '@theme-original/NotFound/Content';
import type {Props} from '@theme/NotFound/Content';
import type {ReactNode} from 'react';
import {getVersionRouteContext} from '../../versionRoutes.mjs';

export default function VersionAwareNotFoundContent(props: Props): ReactNode {
  const {pathname} = useLocation();
  const versions = useVersions(undefined);
  const context = getVersionRouteContext(pathname, versions);

  if (!context) {
    return <NotFoundContent {...props} />;
  }

  return (
    <>
      <Head>
        <title>Page unavailable in {context.label} | Humanizer</title>
        <meta name="robots" content="noindex, nofollow" />
      </Head>
      <main className="container versionUnavailable">
        <div className="versionUnavailable__panel">
          <p className="humanizerKicker">{context.label}</p>
          <h1>Not available in this version.</h1>
          <p>
            The requested page does not exist in {context.label}. Your selected
            version has not changed.
          </p>
          <code className="versionUnavailable__path">{pathname}</code>
          <div className="versionUnavailable__actions">
            <Link className="button button--primary" to={context.docsRoot}>
              Browse {context.label} docs
            </Link>
            <Link className="humanizerTextLink" to={context.apiRoot}>
              Open {context.label} API <span aria-hidden="true">→</span>
            </Link>
          </div>
        </div>
      </main>
    </>
  );
}
