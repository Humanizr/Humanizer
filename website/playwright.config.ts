import {defineConfig} from '@playwright/test';

export default defineConfig({
  testDir: './tests/e2e',
  outputDir: '.docusaurus/playwright-test-results',
  fullyParallel: true,
  reporter: 'line',
  use: {
    baseURL: 'http://127.0.0.1:3000',
    trace: 'retain-on-failure',
  },
  webServer: {
    command: 'npm run serve -- --host 127.0.0.1 --port 3000 --no-open',
    reuseExistingServer: !process.env.CI,
    stderr: 'pipe',
    stdout: 'ignore',
    url: 'http://127.0.0.1:3000',
  },
});
