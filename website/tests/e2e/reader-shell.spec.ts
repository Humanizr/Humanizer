import path from 'node:path';
import {expect, test, type Route} from '@playwright/test';
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
      page.getByRole('heading', {
        name: 'Human-friendly text for .NET.',
      }),
    ).toBeVisible();
    await expect(
      page.getByRole('table', {name: 'Humanizer input and output examples'}),
    ).toBeVisible();
    const releaseCue = page.getByText('What’s new in v4', {exact: true});
    await expect(releaseCue).toBeVisible();
    expect(
      await releaseCue.evaluate((element) => element.closest('a, button')),
    ).toBeNull();
  }

  const install = page.getByText('dotnet add package Humanizer', {exact: true});
  const proof = page.getByRole('table', {
    name: 'Humanizer input and output examples',
  });
  expect(
    await install.evaluate(
      (element) => element.getBoundingClientRect().bottom <= window.innerHeight,
    ),
  ).toBe(true);
  expect(
    await proof.evaluate(
      (element) => element.getBoundingClientRect().bottom <= window.innerHeight,
    ),
  ).toBe(true);

  await page.setViewportSize({width: 320, height: 800});
  await page.goto('/');
  await page.keyboard.press('Tab');
  await expect(page.getByText('Skip to main content')).toBeFocused();
  await page.keyboard.press('Enter');
  await page.getByRole('button', {name: 'Toggle navigation bar'}).click();
  const mobileSidebar = page.locator('.navbar-sidebar');
  expect((await mobileSidebar.boundingBox())?.height).toBe(800);
  await expect(
    page.getByRole('link', {name: 'Guides', exact: true}),
  ).toBeVisible();
  await expect(page.getByText('Versions', {exact: true})).toBeVisible();
  await page.getByRole('button', {name: 'Expand the dropdown'}).click();
  await expect(page.getByRole('link', {name: '4.0 (next)'})).toBeVisible();
  await expect(
    mobileSidebar
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

test('canonical branding renders in the shell and page metadata', async ({
  page,
}) => {
  const logoPath = '/img/logo.png';

  for (const colorScheme of ['light', 'dark'] as const) {
    await page.emulateMedia({colorScheme});
    await page.goto('/');

    const navbarLogo = page
      .getByRole('img', {name: 'Humanizer home'})
      .filter({visible: true});
    const heroLogo = page.getByRole('img', {name: 'Humanizer logo'});

    await expect(navbarLogo).toHaveAttribute('src', logoPath);
    await expect(heroLogo).toHaveAttribute('src', logoPath);
    await expect(heroLogo).toBeVisible();
    await expect
      .poll(() =>
        heroLogo.evaluate((image: HTMLImageElement) => image.naturalWidth),
      )
      .toBe(115);

    for (const logo of [navbarLogo, heroLogo]) {
      expect(
        await logo.evaluate((image) => ({
          background: getComputedStyle(image).backgroundColor,
          padding: getComputedStyle(image).padding,
        })),
      ).toEqual({
        background: 'rgba(0, 0, 0, 0)',
        padding: '0px',
      });
    }
  }

  await expect(page.locator('link[rel="icon"]')).toHaveAttribute(
    'href',
    logoPath,
  );
  await expect(page.locator('meta[property="og:image"]')).toHaveAttribute(
    'content',
    'https://humanizr.net/img/social-logo.png',
  );
  await expect(page.locator('meta[name="twitter:image"]')).toHaveAttribute(
    'content',
    'https://humanizr.net/img/social-logo.png',
  );
  await expect(page.locator('meta[name="twitter:card"]')).toHaveAttribute(
    'content',
    'summary',
  );
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

test('homepage quick-start and language paths open their dedicated guides', async ({
  page,
}) => {
  for (const destination of [
    {
      heading: 'Five-minute quick start',
      link: 'Run the five-minute example',
      path: '/docs/start/quick-start/',
    },
    {
      heading: 'Supported cultures',
      link: 'See supported cultures',
      path: '/docs/languages/supported-cultures/',
    },
  ]) {
    await page.goto('/');
    await page.getByRole('link', {name: destination.link}).click();
    await expect(page).toHaveURL(destination.path);
    await expect(
      page.getByRole('heading', {name: destination.heading, level: 1}),
    ).toBeVisible();
  }
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
  await page.getByRole('link', {name: '4.0 (next)'}).click();

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
    page.getByText('4.0 (next)', {exact: true}).first(),
  ).toBeVisible();
  await expect(
    page.getByRole('link', {name: 'Browse 4.0 (next) docs'}),
  ).toHaveAttribute('href', '/docs/next/');
  await expect(
    page.getByRole('link', {name: 'Open 4.0 (next) API'}),
  ).toHaveAttribute('href', '/docs/next/api/');

  await page.goto('/');
  await page.reload();
  await expect(
    page.getByRole('link', {name: 'Guides', exact: true}),
  ).toHaveAttribute('href', '/docs/next/');
  await expect(
    page.getByRole('link', {name: 'Run the five-minute example'}),
  ).toHaveAttribute('href', '/docs/next/start/quick-start/');
  await expect(
    page.getByRole('link', {name: 'See supported cultures'}),
  ).toHaveAttribute('href', '/docs/next/languages/supported-cultures/');
  await expect(
    page.getByRole('link', {name: 'Open the guides'}),
  ).toHaveAttribute('href', '/docs/next/');
  await expect(
    page.getByRole('link', {name: 'Browse exact signatures'}),
  ).toHaveAttribute('href', '/docs/next/api/');

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

test('all-version search recovers after its first component load fails', async ({
  page,
}) => {
  let releaseFailedAsset = () => {};
  const failedAssetMayFinish = new Promise<void>((resolve) => {
    releaseFailedAsset = resolve;
  });
  let markAssetRequested = () => {};
  const failedAssetRequested = new Promise<void>((resolve) => {
    markAssetRequested = resolve;
  });
  const componentAsset = '**/pagefind/pagefind-component-ui.css';
  const failFirstComponentLoad = async (route: Route) => {
    markAssetRequested();
    await failedAssetMayFinish;
    await route.abort('failed');
  };
  await page.route(componentAsset, failFirstComponentLoad);

  await page.goto('/docs/2.14.1/start/quick-start/');
  const trigger = page.locator('.humanizerAllVersionsTrigger').first();
  await trigger.click();
  await failedAssetRequested;
  await expect(trigger).toHaveAttribute('aria-busy', 'true');
  await expect(trigger).toHaveText('Loading…');

  releaseFailedAsset();
  await expect(trigger).toHaveAttribute('aria-busy', 'false');
  await expect(trigger).toHaveText('All versions');
  await expect(page.locator('pagefind-modal dialog[open]')).toHaveCount(0);

  await page.unroute(componentAsset, failFirstComponentLoad);
  await trigger.click();
  await expect(page.locator('pagefind-modal dialog[open]')).toBeVisible();
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
    {
      source: '/docs/adding-a-locale.md?source=legacy',
      target:
        '/docs/next/contributing/adding-or-updating-a-locale/?source=legacy',
      heading: 'Add or update a locale',
    },
    {
      source: '/docs/locale-yaml-how-to.md?source=legacy',
      target: '/docs/next/contributing/locale-yaml-how-to/?source=legacy',
      heading: 'Locale YAML how-to',
    },
    {
      source: '/docs/locale-yaml-reference.md?source=legacy',
      target: '/docs/next/contributing/locale-yaml-reference/?source=legacy',
      heading: 'Locale YAML reference',
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
    if (legacy.heading) {
      await expect(
        page.getByRole('heading', {name: legacy.heading, level: 1}),
      ).toBeVisible();
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

test('static redirect documents remap fragments after directory normalization', async ({
  page,
}) => {
  await page.route('**/docs/quick-start.md/**', (route) =>
    route.fulfill({
      contentType: 'text/html',
      path: path.resolve(
        __dirname,
        '../../build/docs/quick-start.md/index.html',
      ),
    }),
  );

  await page.goto('/docs/quick-start.md/?source=legacy#basic-examples');
  await expect(page).toHaveURL(
    '/docs/start/quick-start/?source=legacy#example',
  );
  await expect(page.locator('#example')).toBeVisible();
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
