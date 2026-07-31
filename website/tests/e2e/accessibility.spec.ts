import AxeBuilder from '@axe-core/playwright';
import {expect, test, type Page} from '@playwright/test';

const routes = [
  {name: 'home', path: '/'},
  {name: 'tutorial', path: '/docs/start/quick-start/'},
  {name: 'how-to', path: '/docs/start/configuration/'},
  {name: 'upgrade', path: '/docs/upgrading/version-3-migration/'},
  {name: 'API', path: '/docs/api/Humanizer.StringHumanizeExtensions/'},
  {name: 'language', path: '/docs/languages/using-cultures/'},
  {
    name: 'version unavailable',
    path: '/docs/next/api/NotInThisVersion/',
  },
  {
    name: 'unsupported legacy version',
    path: '/docs/2.9.9/api/NotPublished/',
  },
  {name: 'redirected legacy page', path: '/installation.html'},
];

const responsiveTableRoutes = [
  {name: 'homepage proof', path: '/'},
  {name: 'ordinary Markdown', path: '/docs/next/scenarios/'},
  {
    name: 'historical wide table',
    path: '/docs/3.0.8/start/troubleshooting/',
  },
  {
    name: 'historical capability table',
    path: '/docs/languages/supported-cultures/',
  },
];

const responsiveTableViewports = [320, 996, 997, 1440];

async function expectNoSeriousAccessibilityViolations(page: Page) {
  const results = await new AxeBuilder({page}).analyze();
  const violations = results.violations.filter(({impact}) =>
    ['serious', 'critical'].includes(impact ?? ''),
  );

  expect(violations).toEqual([]);
}

test('responsive tables retain native headers without JavaScript', async ({
  browser,
}) => {
  const context = await browser.newContext({
    javaScriptEnabled: false,
    viewport: {width: 320, height: 900},
  });
  const page = await context.newPage();

  try {
    await page.goto('/docs/next/scenarios/');

    const table = page.locator('table').first();
    await expect(table).toBeVisible();
    await expect(table).not.toHaveClass(/humanizerResponsiveTable--enhanced/);
    await expect(table.locator('thead th')).toHaveCount(3);
    await expect(table.locator('thead')).toBeVisible();
    expect(
      await page.evaluate(() => {
        const tableElement = document.querySelector('table');
        return (
          document.documentElement.scrollWidth <=
            document.documentElement.clientWidth &&
          Boolean(
            tableElement &&
              tableElement.scrollWidth <= tableElement.clientWidth,
          )
        );
      }),
    ).toBe(true);
  } finally {
    await context.close();
  }
});

for (const colorScheme of ['light', 'dark'] as const) {
  for (const route of routes) {
    test(`${route.name} has no serious accessibility violations in ${colorScheme} mode`, async ({
      page,
    }) => {
      await page.emulateMedia({colorScheme});
      await page.goto(route.path);

      await expectNoSeriousAccessibilityViolations(page);
    });
  }

  for (const viewport of [
    {name: 'desktop', width: 1280, height: 800},
    {name: '320px', width: 320, height: 720},
  ]) {
    test(`supported cultures list is accessible at ${viewport.name} in ${colorScheme} mode`, async ({
      page,
    }) => {
      await page.setViewportSize(viewport);
      await page.emulateMedia({colorScheme});
      await page.goto('/docs/next/languages/supported-cultures/');

      const cultures = page.getByRole('list', {name: 'Supported cultures'});
      await expect(cultures).toBeVisible();
      await expect(cultures.getByText('en', {exact: true})).toBeVisible();

      await expectNoSeriousAccessibilityViolations(page);
    });
  }

  test(`responsive tables preserve width, labels, and focus in ${colorScheme} mode`, async ({
    page,
  }) => {
    await page.emulateMedia({colorScheme});

    for (const route of responsiveTableRoutes) {
      for (const width of responsiveTableViewports) {
        await page.setViewportSize({width, height: 900});
        await page.goto(route.path);

        const tables = page.locator('table');
        await expect(tables.first()).toBeVisible();
        const tableCount = await tables.count();
        expect(
          await page.evaluate(
            () =>
              document.documentElement.scrollWidth <=
              document.documentElement.clientWidth,
          ),
          `${route.name} document overflows at ${width}px`,
        ).toBe(true);

        for (let index = 0; index < tableCount; index += 1) {
          const table = tables.nth(index);
          await expect(table).toHaveAttribute('tabindex', '0');
          await table.focus();
          await expect(table).toBeFocused();

          const result = await table.evaluate((element, cardLayout) => {
            const tableElement = element as HTMLTableElement;
            const headers = Array.from(
              tableElement.tHead?.rows[0]?.cells ?? [],
            );
            const labels = headers.map(
              (header) =>
                header.getAttribute('aria-label') ??
                header.textContent?.trim() ??
                '',
            );
            const failures: string[] = [];

            if (tableElement.scrollWidth > tableElement.clientWidth) {
              failures.push('table overflows');
            }

            let ancestor: HTMLElement | null = tableElement;
            while (ancestor && ancestor !== document.body) {
              const {overflowX} = getComputedStyle(ancestor);
              if (overflowX === 'auto' || overflowX === 'scroll') {
                failures.push(
                  `${ancestor.tagName.toLowerCase()} is a horizontal scroller`,
                );
              }
              ancestor = ancestor.parentElement;
            }

            headers.forEach((header, headerIndex) => {
              if (header.scope !== 'col') {
                failures.push(`header ${headerIndex + 1} has no column scope`);
              }
              if (!header.id) {
                failures.push(`header ${headerIndex + 1} has no id`);
              }
            });

            Array.from(tableElement.tBodies).forEach((body) => {
              Array.from(body.rows).forEach((row) => {
                Array.from(row.cells).forEach((cell, columnIndex) => {
                  if (cell.tagName !== 'TD') {
                    return;
                  }

                  if (cell.dataset.label !== labels[columnIndex]) {
                    failures.push(`cell ${columnIndex + 1} has the wrong label`);
                  }
                  if (
                    headers[columnIndex]?.id &&
                    !cell.headers.split(' ').includes(headers[columnIndex].id)
                  ) {
                    failures.push(
                      `cell ${columnIndex + 1} is not linked to its header`,
                    );
                  }
                  if (
                    cardLayout &&
                    cell.dataset.label &&
                    cell.getAttribute('aria-hidden') !== 'true' &&
                    ['none', 'normal'].includes(
                      getComputedStyle(cell, '::before').content,
                    )
                  ) {
                    failures.push(
                      `cell ${columnIndex + 1} has no visible card label`,
                    );
                  }
                });
              });
            });

            return failures;
          }, width <= 996);

          expect(
            result,
            `${route.name} table ${index + 1} failed at ${width}px`,
          ).toEqual([]);
        }
      }

      await page.setViewportSize({width: 320, height: 900});
      await page.goto(route.path);
      await expectNoSeriousAccessibilityViolations(page);
    }
  });

  test(`all-version modal has no serious accessibility violations in ${colorScheme} mode`, async ({
    page,
  }) => {
    await page.emulateMedia({colorScheme});
    await page.goto('/docs/2.14.1/start/quick-start/');
    await page.getByRole('button', {name: 'All versions'}).first().click();
    await expect(page.locator('pagefind-modal dialog')).toBeVisible();

    await expectNoSeriousAccessibilityViolations(page);
  });
}
