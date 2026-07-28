import {
  useDoc,
  useDocsVersion,
} from '@docusaurus/plugin-content-docs/client';
import DocItemContent from '@theme-original/DocItem/Content';
import type {Props} from '@theme/DocItem/Content';
import type {ReactNode} from 'react';

export default function DocItemContentWithVersion({
  children,
}: Props): ReactNode {
  const {metadata} = useDoc();
  const version = useDocsVersion();

  return (
    <>
      <span
        aria-hidden="true"
        className="pagefindMetadata"
        data-pagefind-ignore
        data-pagefind-meta="title">
        {metadata.title} — {version.label}
      </span>
      <span
        aria-hidden="true"
        className="pagefindMetadata"
        data-pagefind-filter="version"
        data-pagefind-ignore
        data-pagefind-meta="version">
        {version.label}
      </span>
      <DocItemContent>{children}</DocItemContent>
    </>
  );
}
