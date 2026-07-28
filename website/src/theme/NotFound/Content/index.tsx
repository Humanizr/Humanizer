import Head from '@docusaurus/Head';
import Link from '@docusaurus/Link';
import {useVersions} from '@docusaurus/plugin-content-docs/client';
import {useLocation} from '@docusaurus/router';
import NotFoundContent from '@theme-original/NotFound/Content';
import type {Props} from '@theme/NotFound/Content';
import type {ReactNode} from 'react';
import {
  getUnsupportedVersionContext,
  getVersionRouteContext,
} from '../../versionRoutes.mjs';

export default function VersionAwareNotFoundContent(props: Props): ReactNode {
  const {pathname} = useLocation();
  const versions = useVersions(undefined);
  const unsupportedVersion = getUnsupportedVersionContext(pathname, versions);
  const context = unsupportedVersion
    ? undefined
    : getVersionRouteContext(pathname, versions);

  if (!context && !unsupportedVersion) {
    return <NotFoundContent {...props} />;
  }

  const label =
    context?.label ?? `Humanizer ${unsupportedVersion?.requestedVersion}`;
  const docsRoot = context?.docsRoot ?? unsupportedVersion?.docsRoot;
  const apiRoot = context?.apiRoot ?? unsupportedVersion?.apiRoot;
  const earliestLabel = unsupportedVersion?.earliestLabel;

  return (
    <>
      <Head>
        <title>Page unavailable in {label} | Humanizer</title>
        <meta name="robots" content="noindex, nofollow" />
      </Head>
      <main className="container versionUnavailable">
        <div className="versionUnavailable__panel">
          <p className="humanizerKicker">{label}</p>
          <h1>
            {context
              ? 'Not available in this version.'
              : 'Documentation is not published for this version.'}
          </h1>
          {context ? (
            <p>
              The requested page does not exist in {context.label}. Your
              selected version has not changed.
            </p>
          ) : (
            <p>
              Versioned documentation starts at Humanizer {earliestLabel}. This
              address stays on the requested legacy version instead of silently
              showing current behavior.
            </p>
          )}
          <code className="versionUnavailable__path">{pathname}</code>
          <div className="versionUnavailable__actions">
            <Link className="button button--primary" to={docsRoot}>
              {context
                ? `Browse ${context.label} docs`
                : `Browse ${earliestLabel} docs`}
            </Link>
            <Link className="humanizerTextLink" to={apiRoot}>
              {context
                ? `Open ${context.label} API`
                : `Open ${earliestLabel} API`}{' '}
              <span aria-hidden="true">→</span>
            </Link>
          </div>
        </div>
      </main>
    </>
  );
}
