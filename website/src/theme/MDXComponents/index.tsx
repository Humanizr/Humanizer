import MDXComponents from '@theme-original/MDXComponents';
import {
  Fragment,
  useCallback,
  useEffect,
  useId,
  type ComponentPropsWithoutRef,
  type PropsWithChildren,
} from 'react';

function enhanceTable(table: HTMLTableElement, tableId: string) {
  table.classList.add('humanizerResponsiveTable');
  table.tabIndex = 0;

  const headers = Array.from(table.tHead?.rows[0]?.cells ?? []);
  headers.forEach((header, index) => {
    if (!header.scope) {
      header.scope = 'col';
    }
    if (!header.id) {
      header.id = `${tableId}-column-${index + 1}`;
    }
  });

  Array.from(table.tBodies).forEach((body, bodyIndex) => {
    Array.from(body.rows).forEach((row, rowIndex) => {
      const rowHeader = Array.from(row.cells).find(
        (cell) => cell.tagName === 'TH',
      );
      if (rowHeader) {
        if (!rowHeader.scope) {
          rowHeader.scope = 'row';
        }
        if (!rowHeader.id) {
          rowHeader.id = `${tableId}-row-${bodyIndex + 1}-${rowIndex + 1}`;
        }
      }

      Array.from(row.cells).forEach((cell, columnIndex) => {
        if (cell.tagName !== 'TD') {
          return;
        }

        const header = headers[columnIndex];
        const label =
          header?.getAttribute('aria-label') ?? header?.textContent?.trim() ?? '';
        cell.dataset.label = label;

        if (!cell.headers) {
          cell.headers = [header?.id, rowHeader?.id].filter(Boolean).join(' ');
        }
      });
    });
  });

  table.classList.add('humanizerResponsiveTable--enhanced');
}

export function ResponsiveTable({
  className,
  ...props
}: ComponentPropsWithoutRef<'table'>) {
  const id = useId().replaceAll(':', '');
  const tableRef = useCallback(
    (table: HTMLTableElement | null) => {
      if (table) {
        enhanceTable(table, `humanizer-table-${id}`);
      }
    },
    [id],
  );

  return (
    <div className="humanizerResponsiveTableFrame">
      <table
        {...props}
        className={['humanizerResponsiveTable', className]
          .filter(Boolean)
          .join(' ')}
        ref={tableRef}
        tabIndex={0}
      />
    </div>
  );
}

function ResponsiveTableScope({children}: PropsWithChildren) {
  useEffect(() => {
    document
      .querySelectorAll<HTMLTableElement>('.theme-doc-markdown table')
      .forEach((table, index) =>
        enhanceTable(table, `humanizer-mdx-table-${index + 1}`),
      );
  }, [children]);

  return <Fragment>{children}</Fragment>;
}

export default {
  ...MDXComponents,
  table: ResponsiveTable,
  wrapper: ResponsiveTableScope,
};
