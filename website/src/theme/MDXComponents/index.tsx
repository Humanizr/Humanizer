import MDXComponents from '@theme-original/MDXComponents';
import type {ComponentPropsWithoutRef} from 'react';

function FocusableTable(props: ComponentPropsWithoutRef<'table'>) {
  return <table {...props} tabIndex={0} />;
}

export default {
  ...MDXComponents,
  table: FocusableTable,
};
