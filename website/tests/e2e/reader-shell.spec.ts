import {expect, test} from '@playwright/test';

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
    '/docs/api/Humanizer.ByteSize/',
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
