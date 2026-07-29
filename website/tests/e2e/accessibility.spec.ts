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

async function expectNoSeriousAccessibilityViolations(page: Page) {
  const results = await new AxeBuilder({page}).analyze();
  const violations = results.violations.filter(({impact}) =>
    ['serious', 'critical'].includes(impact ?? ''),
  );

  expect(violations).toEqual([]);
}

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

  test(`ordinary Markdown tables are keyboard accessible at 320px in ${colorScheme} mode`, async ({
    page,
  }) => {
    await page.setViewportSize({width: 320, height: 720});
    await page.emulateMedia({colorScheme});
    await page.goto('/docs/next/scenarios/');

    const table = page.locator('article table').first();
    await expect(table).toBeVisible();
    await expect(table).toHaveAttribute('tabindex', '0');
    await table.focus();
    await expect(table).toBeFocused();

    await expectNoSeriousAccessibilityViolations(page);
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
