import {useLocation} from '@docusaurus/router';
import {useVersions} from '@docusaurus/plugin-content-docs/client';
import DropdownNavbarItem from '@theme-original/NavbarItem/DropdownNavbarItem';
import type {Props} from '@theme/NavbarItem/DropdownNavbarItem';
import type {ReactNode} from 'react';
import {preserveUnavailableVersionPath} from '../../versionRoutes.mjs';

function VersionDropdownNavbarItem(props: Props): ReactNode {
  const {pathname} = useLocation();
  const versions = useVersions(undefined);

  return (
    <DropdownNavbarItem
      {...props}
      items={preserveUnavailableVersionPath(
        props.items,
        pathname,
        versions,
      )}
    />
  );
}

export default function VersionAwareDropdownNavbarItem(
  props: Props,
): ReactNode {
  if (props.className?.split(/\s+/).includes('humanizerVersionDropdown')) {
    return <VersionDropdownNavbarItem {...props} />;
  }

  return <DropdownNavbarItem {...props} />;
}
