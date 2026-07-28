import {defineConfig} from '@playwright/test';

const productionBaseUrl = process.env.HUMANIZER_DOCS_BASE_URL;

export default defineConfig({
  testDir: './tests/e2e',
  outputDir: '.docusaurus/playwright-test-results',
  forbidOnly: Boolean(process.env.CI),
  fullyParallel: true,
  reporter: 'line',
  workers: process.env.CI ? 1 : undefined,
  use: {
    baseURL: productionBaseUrl ?? 'http://127.0.0.1:3000',
    trace: 'retain-on-failure',
  },
  webServer: productionBaseUrl
    ? undefined
    : {
        command: 'pnpm run serve --host 127.0.0.1 --port 3000 --no-open',
        reuseExistingServer: !process.env.CI,
        stderr: 'pipe',
        stdout: 'ignore',
        url: 'http://127.0.0.1:3000',
      },
});
