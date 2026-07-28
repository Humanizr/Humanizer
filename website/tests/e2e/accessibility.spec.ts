import AxeBuilder from '@axe-core/playwright';
import {expect, test} from '@playwright/test';

const routes = [
  {name: 'home', path: '/'},
  {name: 'docs', path: '/docs/proof/'},
  {
    name: 'version unavailable',
    path: '/docs/next/api/NotInThisVersion/',
  },
];

for (const colorScheme of ['light', 'dark'] as const) {
  for (const route of routes) {
    test(`${route.name} has no serious accessibility violations in ${colorScheme} mode`, async ({
      page,
    }) => {
      await page.emulateMedia({colorScheme});
      await page.goto(route.path);

      const results = await new AxeBuilder({page}).analyze();
      const violations = results.violations.filter(({impact}) =>
        ['serious', 'critical'].includes(impact ?? ''),
      );

      expect(violations).toEqual([]);
    });
  }

  for (const viewport of [
    {name: 'desktop', width: 1280, height: 800},
    {name: '320px', width: 320, height: 720},
  ]) {
    test(`supported cultures table is accessible at ${viewport.name} in ${colorScheme} mode`, async ({
      page,
    }) => {
      await page.setViewportSize(viewport);
      await page.emulateMedia({colorScheme});
      await page.goto('/docs/next/languages/supported-cultures/');

      const region = page.getByRole('region', {
        name: 'Current locale capability coverage',
      });
      await expect(region).toBeVisible();
      await expect(region).toHaveAttribute('tabindex', '0');
      await expect(
        region.getByText('Current locale capability coverage', {exact: true}),
      ).toBeVisible();
      await region.focus();
      await expect(region).toBeFocused();

      const results = await new AxeBuilder({page}).analyze();
      const violations = results.violations.filter(({impact}) =>
        ['serious', 'critical'].includes(impact ?? ''),
      );

      expect(violations).toEqual([]);
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

    const results = await new AxeBuilder({page}).analyze();
    const violations = results.violations.filter(({impact}) =>
      ['serious', 'critical'].includes(impact ?? ''),
    );

    expect(violations).toEqual([]);
  });
}
