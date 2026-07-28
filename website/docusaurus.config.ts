import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';
import {themes as prismThemes} from 'prism-react-renderer';
import versionManifest from './humanizer-versions.json';

const latestStable = versionManifest.versions.find(
  (version) => version.latestStable,
);
const preview = versionManifest.versions.find(
  (version) => version.version === 'current',
);

if (!latestStable || !preview) {
  throw new Error('The documentation version manifest is incomplete.');
}

const config: Config = {
  title: 'Humanizer',
  tagline: 'Human-friendly text for .NET',
  url: 'https://humanizr.net',
  baseUrl: '/',
  organizationName: 'Humanizr',
  projectName: 'Humanizer',
  trailingSlash: true,
  onBrokenLinks: 'throw',
  onBrokenAnchors: 'throw',
  headTags: [
    {
      tagName: 'link',
      attributes: {
        rel: 'stylesheet',
        href: '/pagefind/pagefind-component-ui.css',
      },
    },
    {
      tagName: 'script',
      attributes: {
        type: 'module',
        src: '/pagefind/pagefind-component-ui.js',
      },
    },
  ],
  markdown: {
    format: 'detect',
  },
  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },
  presets: [
    [
      'classic',
      {
        docs: {
          routeBasePath: 'docs',
          sidebarPath: './sidebars.ts',
          includeCurrentVersion: true,
          lastVersion: latestStable.version,
          versions: {
            current: {
              label: preview.label,
              path: preview.route,
              banner: 'unreleased',
              badge: true,
              noIndex: true,
            },
            [latestStable.version]: {
              label: latestStable.label,
              path: latestStable.route,
              banner: 'none',
              badge: true,
            },
          },
        },
        blog: false,
        sitemap: {
          changefreq: 'weekly',
          priority: 0.5,
        },
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],
  plugins: [
    [
      '@cmfcmf/docusaurus-search-local',
      {
        indexDocs: true,
        indexBlog: false,
        indexPages: false,
        language: 'en',
      },
    ],
    [
      '@docusaurus/plugin-client-redirects',
      {
        redirects: [
          {
            from: '/quick-start',
            to: '/docs/proof',
          },
        ],
      },
    ],
  ],
  themeConfig: {
    colorMode: {
      defaultMode: 'light',
      respectPrefersColorScheme: true,
    },
    navbar: {
      title: 'Humanizer',
      items: [
        {
          type: 'docSidebar',
          sidebarId: 'docs',
          position: 'left',
          label: 'Docs',
        },
        {
          type: 'docsVersionDropdown',
          position: 'right',
        },
        {
          type: 'html',
          position: 'right',
          value:
            '<pagefind-modal-trigger placeholder="All versions" shortcut="mod+shift+k"></pagefind-modal-trigger><pagefind-modal reset-on-close></pagefind-modal>',
        },
        {
          href: 'https://github.com/Humanizr/Humanizer',
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      copyright: `Copyright © ${new Date().getFullYear()} Humanizer contributors. MIT licensed.`,
    },
    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
