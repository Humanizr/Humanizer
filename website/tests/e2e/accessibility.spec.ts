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
}
