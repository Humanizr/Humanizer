import {expect, test} from '@playwright/test';

test('deployed site preserves reader, version, search, and legacy contracts', async ({
  page,
  request,
}) => {
  await page.goto('/');
  await expect(
    page.getByRole('heading', {name: 'Make software sound like people.'}),
  ).toBeVisible();
  await expect(page.getByRole('button', {name: 'All versions'})).toBeVisible();

  for (const expected of [
    {
      canonical: 'https://humanizr.net/docs/start/quick-start/',
      path: '/docs/start/quick-start/',
    },
    {
      canonical: 'https://humanizr.net/docs/2.10.1/start/quick-start/',
      path: '/docs/2.10.1/start/quick-start/',
    },
    {
      canonical: 'https://humanizr.net/docs/next/start/quick-start/',
      noIndex: true,
      path: '/docs/next/start/quick-start/',
    },
  ]) {
    await page.goto(expected.path);
    await expect(
      page.getByRole('heading', {name: 'Five-minute quick start'}),
    ).toBeVisible();
    await expect(page.locator('link[rel="canonical"]')).toHaveAttribute(
      'href',
      expected.canonical,
    );
    if (expected.noIndex) {
      await expect(page.locator('meta[name="robots"]')).toHaveAttribute(
        'content',
        /noindex/,
      );
    }
  }

  await page.goto('/docs/quick-start.md?source=production#basic-examples');
  await expect(page).toHaveURL(
    '/docs/start/quick-start/?source=production#example',
  );

  await page.goto('/.github/CONTRIBUTING.md?source=production');
  await expect(page).toHaveURL('/docs/contributing/?source=production');
  await expect(
    page.getByRole('heading', {name: 'Contribute to Humanizer'}),
  ).toBeVisible();

  const sitemap = await request.get('/sitemap.xml');
  expect(sitemap.ok()).toBe(true);
  expect(await sitemap.text()).toContain(
    'https://humanizr.net/docs/start/quick-start/',
  );
});
