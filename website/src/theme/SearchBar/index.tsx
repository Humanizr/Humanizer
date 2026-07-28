import {
  useActiveVersion,
  useDocsPreferredVersion,
  useLatestVersion,
} from '@docusaurus/plugin-content-docs/client';
import SearchBar from '@theme-original/SearchBar';
import type {ReactNode} from 'react';

export default function VersionContextSearchBar(): ReactNode {
  const activeVersion = useActiveVersion(undefined);
  const {preferredVersion} = useDocsPreferredVersion();
  const latestVersion = useLatestVersion(undefined);
  const version = activeVersion ?? preferredVersion ?? latestVersion;

  return (
    <div className="humanizerContextSearch">
      <span className="humanizerContextSearch__version">
        {version.label}
      </span>
      <SearchBar />
    </div>
  );
}
