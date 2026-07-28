import {expect, test} from '@playwright/test';
import versionManifest from '../../humanizer-versions.json';

const searchContexts = versionManifest.versions
  .filter(({published, version}) => published || version === 'current')
  .map(({label, route, version}) => ({
    index: version,
    label,
    path: `/docs/${route ? `${route}/` : ''}start/quick-start/`,
  }));
const searchIndexes = new Set(
  searchContexts.map(({index}) => `docs-default-${index}`),
);

test('navigation remains operable and unclipped at reader breakpoints', async ({
  page,
}) => {
  for (const width of [320, 375, 768, 1024, 1440]) {
    await page.setViewportSize({width, height: 900});
    await page.goto('/');

    const pageWidth = await page.evaluate(
      () => document.documentElement.scrollWidth,
    );
    expect(pageWidth).toBeLessThanOrEqual(width);
    await expect(
      page.getByRole('heading', {name: 'Make software sound like people.'}),
    ).toBeVisible();
    await expect(
      page.getByRole('navigation', {name: 'Documentation sections'}),
    ).toBeVisible();
  }

  await page.setViewportSize({width: 320, height: 800});
  await page.goto('/');
  await page.keyboard.press('Tab');
  await expect(page.getByText('Skip to main content')).toBeFocused();
  await page.keyboard.press('Enter');
  await page.getByRole('button', {name: 'Toggle navigation bar'}).click();
  await expect(page.getByRole('link', {name: 'Guides'})).toBeVisible();
  await expect(page.getByText('Versions', {exact: true})).toBeVisible();
  await expect(
    page
      .locator('.navbar-sidebar')
      .getByRole('button', {name: /Switch between dark and light mode/}),
  ).toBeVisible();
  await expect(page.getByRole('button', {name: /Search/}).first()).toBeVisible();
  await expect(page.getByRole('button', {name: 'All versions'})).toBeVisible();

  for (const path of [
    '/docs/start/quick-start/',
    '/docs/api/Humanizer/',
  ]) {
    await page.goto(path);
    const contentWidth = await page.evaluate(
      () => document.documentElement.scrollWidth,
    );
    expect(contentWidth).toBeLessThanOrEqual(320);
  }
});

test('theme follows the system default and remains keyboard switchable', async ({
  page,
}) => {
  await page.emulateMedia({colorScheme: 'dark'});
  await page.goto('/');
  await expect(page.locator('html')).toHaveAttribute('data-theme', 'dark');

  const themeToggle = page.getByRole('button', {
    name: /Switch between dark and light mode/,
  });
  await themeToggle.focus();
  await page.keyboard.press('Enter');
  await expect(page.locator('html')).toHaveAttribute('data-theme', 'light');
});

test('version preview is labeled, noindex, and self-canonical', async ({
  page,
}) => {
  await page.goto('/docs/next/start/quick-start/');

  await expect(page.getByText(/unreleased documentation/i)).toBeVisible();
  await expect(page.locator('meta[name="robots"]')).toHaveAttribute(
    'content',
    /noindex/,
  );
  await expect(page.locator('link[rel="canonical"]')).toHaveAttribute(
    'href',
    'https://humanizr.net/docs/next/start/quick-start/',
  );

  await page.goto('/docs/start/quick-start/');
  await expect(page.locator('link[rel="canonical"]')).toHaveAttribute(
    'href',
    'https://humanizr.net/docs/start/quick-start/',
  );
  await expect(
    page.getByRole('link', {name: 'Edit or report this page'}),
  ).toBeVisible();
});

test('version not found preserves the requested API path and target version', async ({
  page,
}) => {
  await page.goto('/docs/api/Humanizer.Resources/');
  await page.locator('.humanizerVersionDropdown').focus();
  await page.keyboard.press('Enter');
  await page.getByRole('link', {name: 'main/preview'}).click();

  await expect(page).toHaveURL(
    /\/docs\/next\/api\/Humanizer\.Resources\/$/,
  );
  await expect
    .poll(() =>
      page.evaluate(() =>
        localStorage.getItem('docs-preferred-version-default'),
      ),
    )
    .toBe('current');
  await expect(
    page.getByRole('heading', {name: 'Not available in this version.'}),
  ).toBeVisible();
  await expect(
    page.getByText('main/preview', {exact: true}).first(),
  ).toBeVisible();
  await expect(
    page.getByRole('link', {name: 'Browse main/preview docs'}),
  ).toHaveAttribute('href', '/docs/next/');
  await expect(
    page.getByRole('link', {name: 'Open main/preview API'}),
  ).toHaveAttribute('href', '/docs/next/api/');

  await page.goto('/');
  await page.reload();
  await expect(page.getByRole('link', {name: 'Guides'})).toHaveAttribute(
    'href',
    '/docs/next/',
  );

  await page.goto('/docs/next/api/Humanizer.ByteSize/');
  await page.locator('.humanizerVersionDropdown').focus();
  await page.keyboard.press('Enter');
  await page.getByRole('link', {name: '3.0.10 (latest)'}).click();
  await expect(page).toHaveURL(
    /\/docs\/api\/Humanizer\.ByteSize\/$/,
  );
  await expect(
    page.getByRole('heading', {name: 'Humanizer.ByteSize'}),
  ).toBeVisible();
  await expect
    .poll(() =>
      page.evaluate(() =>
        localStorage.getItem('docs-preferred-version-default'),
      ),
    )
    .toBe('3.0.10');
});

test('contextual search fetches only the selected manifest index', async ({
  page,
}) => {
  const indexRequests: string[] = [];
  page.on('request', (request) => {
    const match = new URL(request.url()).pathname.match(
      /search-index-(docs-default-[^/]+)\.json$/,
    );
    if (match && searchIndexes.has(match[1])) {
      indexRequests.push(match[1]);
    }
  });

  for (const context of searchContexts) {
    indexRequests.length = 0;
    await page.goto(context.path);
    await expect(
      page.locator('.humanizerContextSearch__version'),
    ).toHaveText(context.label);

    await page.locator('.aa-DetachedSearchButton').click();
    await page.locator('.aa-Input').fill('StringHumanizeExtensions');
    await expect(page.locator('.aa-ItemLink').first()).toBeVisible();
    await expect
      .poll(() => [...new Set(indexRequests)])
      .toEqual([`docs-default-${context.index}`]);
    await page.keyboard.press('Escape');
  }
});

test('all-version search is lazy, labeled, keyboard operable, and exact', async ({
  page,
}) => {
  const pagefindAssetRequests: string[] = [];
  const pagefindDataRequests: string[] = [];
  page.on('request', (request) => {
    const path = new URL(request.url()).pathname;
    if (path.startsWith('/pagefind/')) {
      pagefindAssetRequests.push(path);
    }
    if (path.startsWith('/pagefind/pagefind.')) {
      pagefindDataRequests.push(path);
    }
  });

  await page.goto('/docs/2.14.1/start/quick-start/');
  expect(pagefindAssetRequests).toEqual([]);
  expect(pagefindDataRequests).toEqual([]);

  const trigger = page.getByRole('button', {name: 'All versions'}).first();
  await trigger.focus();
  await page.keyboard.press('Enter');

  const dialog = page.locator('pagefind-modal dialog[open]');
  await expect(dialog).toBeVisible();
  expect(pagefindAssetRequests).toContain(
    '/pagefind/pagefind-component-ui.css',
  );
  expect(pagefindAssetRequests).toContain('/pagefind/pagefind-component-ui.js');

  await dialog
    .getByRole('searchbox')
    .fill('Humanizer.StringHumanizeExtensions');
  await expect.poll(() => pagefindDataRequests.length).toBeGreaterThan(0);
  const exactResult = dialog.locator(
    'a[href="/docs/2.14.1/api/Humanizer.StringHumanizeExtensions/"]',
  );
  await expect(exactResult).toBeVisible();
  await expect(exactResult).toContainText('2.14.1');

  await page.keyboard.press('Escape');
  await expect(dialog).not.toBeVisible();
  await expect(trigger).toBeFocused();
});

test('mobile all-version modal preserves touch size, focus, and viewport', async ({
  page,
}) => {
  await page.emulateMedia({colorScheme: 'dark'});
  await page.setViewportSize({width: 320, height: 720});
  await page.goto('/docs/2.14.1/start/quick-start/');
  await page.getByRole('button', {name: 'Toggle navigation bar'}).click();

  const trigger = page.getByRole('button', {name: 'All versions'});
  const sidebar = page.locator('.navbar-sidebar');
  await expect
    .poll(async () => (await sidebar.boundingBox())?.x)
    .toBeGreaterThanOrEqual(0);
  const sidebarBox = await sidebar.boundingBox();
  const box = await trigger.boundingBox();
  expect(box?.height).toBeGreaterThanOrEqual(44);
  expect(box?.width).toBeGreaterThanOrEqual(44);
  expect(box?.x).toBeGreaterThanOrEqual(sidebarBox?.x ?? 0);
  expect((box?.x ?? 0) + (box?.width ?? 0)).toBeLessThanOrEqual(
    (sidebarBox?.x ?? 0) + (sidebarBox?.width ?? 0),
  );
  expect((box?.y ?? 0) + (box?.height ?? 0)).toBeLessThanOrEqual(720);
  expect(await trigger.evaluate((element) => element.closest('nav'))).toBeNull();
  await trigger.click();

  const dialog = page.locator('pagefind-modal dialog[open]');
  await expect(dialog).toBeVisible();
  await expect(dialog.getByRole('searchbox')).toBeFocused();
  expect(
    await page.evaluate(() => document.documentElement.scrollWidth),
  ).toBeLessThanOrEqual(320);
  await dialog.getByRole('button', {name: 'Close'}).click();
  await expect(trigger).toBeFocused();

  const themeToggle = sidebar.getByRole('button', {
    name: /Switch between dark and light mode/,
  });
  const initialTheme = await page.locator('html').getAttribute('data-theme');
  await themeToggle.click();
  await expect(page.locator('html')).not.toHaveAttribute(
    'data-theme',
    initialTheme ?? '',
  );

  await sidebar
    .getByRole('button', {name: 'Close navigation bar'})
    .click();
  await expect(page.locator('.navbar-sidebar--show')).toHaveCount(0);
  await expect(trigger).not.toBeVisible();
});

test('legacy URLs preserve supported destinations, queries, and fragments', async ({
  context,
  page,
}) => {
  for (const legacy of [
    {
      source: '/installation/?source=legacy#nuget-packages',
      target: '/docs/start/installation/?source=legacy#example',
      targetId: 'example',
    },
    {
      source: '/quick-start/#basic-examples',
      target: '/docs/start/quick-start/#example',
      targetId: 'example',
    },
    {
      source: '/string-humanization/#basic-usage',
      target: '/docs/scenarios/strings-and-casing/#example',
      targetId: 'example',
    },
  ]) {
    await page.goto(legacy.source);
    await expect(page).toHaveURL(legacy.target);
    const target = page.locator(`#${legacy.targetId}`);
    await expect(target).toBeVisible();
    await expect
      .poll(() =>
        target.evaluate((element) => {
          const {top} = element.getBoundingClientRect();
          return top >= 0 && top < Math.min(window.innerHeight / 2, 300);
        }),
      )
      .toBe(true);
  }

  const directPage = await context.newPage();
  await directPage.goto(
    '/docs/start/installation/?source=direct#nuget-packages',
  );
  await expect(directPage).toHaveURL(
    '/docs/start/installation/?source=direct#nuget-packages',
  );
  await directPage.close();

  const unrelatedReferrerPage = await context.newPage();
  await unrelatedReferrerPage.goto(
    '/docs/start/installation/#nuget-packages',
    {
      referer: 'http://127.0.0.1:3000/docs/not-a-legacy-source/',
    },
  );
  await expect(unrelatedReferrerPage).toHaveURL(
    '/docs/start/installation/#nuget-packages',
  );
  await unrelatedReferrerPage.close();

  for (const legacy of [
    {
      source: '/docs/index.md?source=legacy',
      target: '/docs/?source=legacy',
    },
    {
      source: '/docs/localization.md?source=legacy#supported-locales',
      target:
        '/docs/languages/supported-cultures/?source=legacy#orientation',
      targetId: 'orientation',
    },
    {
      source:
        '/docs/migration-v3.md?source=legacy#namespace-consolidation-source-breaking',
      target:
        '/docs/upgrading/version-3-migration/?source=legacy#2-consolidate-namespaces',
      targetId: '2-consolidate-namespaces',
    },
    {
      source: '/.github/CONTRIBUTING.md?source=legacy',
      target: '/docs/contributing/?source=legacy',
    },
  ]) {
    const targetPath = new URL(legacy.target, 'http://127.0.0.1').pathname;
    const finalResponse = page.waitForResponse(
      (response) =>
        response.request().isNavigationRequest() &&
        new URL(response.url()).pathname === targetPath &&
        response.status() === 200,
    );
    const initialResponse = await page.goto(legacy.source);
    expect(initialResponse?.status()).toBe(404);
    await expect(page).toHaveURL(legacy.target);
    expect((await finalResponse).status()).toBe(200);

    if (legacy.targetId) {
      const target = page.locator(`[id="${legacy.targetId}"]`);
      await expect(target).toBeVisible();
      await expect
        .poll(() =>
          target.evaluate((element) => {
            const {top} = element.getBoundingClientRect();
            return top >= 0 && top < Math.min(window.innerHeight / 2, 300);
          }),
        )
        .toBe(true);
    }
  }

  await page.goto('/docs/2.9.9/api/Humanizer.StringHumanizeExtensions/');
  await expect(page).toHaveURL(
    '/docs/2.9.9/api/Humanizer.StringHumanizeExtensions/',
  );
  await expect(
    page.getByRole('heading', {
      name: 'Documentation is not published for this version.',
    }),
  ).toBeVisible();
  await expect(page.getByText('Humanizer 2.9.9', {exact: true})).toBeVisible();
  await expect(
    page.getByRole('link', {name: 'Browse 2.10.1 docs'}),
  ).toHaveAttribute('href', '/docs/2.10.1/');

  const prefixResponse = await page.goto('/docs/localization.md/extra');
  expect(prefixResponse?.status()).toBe(404);
  await expect(page).toHaveURL('/docs/localization.md/extra/');
});

test('version dropdown exposes every manifest snapshot and preview', async ({
  page,
}) => {
  await page.goto('/docs/start/quick-start/');
  await page.locator('.humanizerVersionDropdown').click();

  for (const context of searchContexts) {
    await expect(
      page.getByRole('link', {name: context.label, exact: true}),
    ).toBeVisible();
  }
});
