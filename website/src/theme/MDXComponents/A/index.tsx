import useBrokenLinks from '@docusaurus/useBrokenLinks';
import MDXA from '@theme-original/MDXComponents/A';
import type {Props} from '@theme/MDXComponents/A';
import type {ReactNode} from 'react';

export default function NamedAnchor(
  props: Props & {name?: string},
): ReactNode {
  const id = props.id ?? props.name;
  useBrokenLinks().collectAnchor(id);
  return <MDXA {...props} id={id} />;
}
