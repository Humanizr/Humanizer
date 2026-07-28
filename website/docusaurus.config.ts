import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';
import {themes as prismThemes} from 'prism-react-renderer';
import versionManifest from './humanizer-versions.json';
import nativeVersions from './versions.json';

const repositoryUrl = 'https://github.com/Humanizr/Humanizer';
const latestStable = versionManifest.versions.find(
  (version) => version.latestStable,
);
const preview = versionManifest.versions.find(
  (version) => version.version === 'current',
);

if (!latestStable || !preview) {
  throw new Error('The documentation version manifest is incomplete.');
}

const publishedVersions: Record<
  string,
  {label: string; path: string; banner: 'none'; badge: boolean}
> = Object.fromEntries(
  nativeVersions.map((versionName) => {
    const version = versionManifest.versions.find(
      (candidate) => candidate.version === versionName,
    );
    if (!version?.published) {
      throw new Error(`Published documentation version is invalid: ${versionName}`);
    }

    return [
      versionName,
      {
        label: version.label,
        path: version.route,
        banner: 'none',
        badge: true,
      },
    ];
  }),
);

const config: Config = {
  title: 'Humanizer',
  tagline: 'Human-friendly text for .NET',
  favicon: 'img/favicon.svg',
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
          breadcrumbs: true,
          includeCurrentVersion: true,
          lastVersion: latestStable.version,
          editUrl: ({version, docPath, permalink}) => {
            if (version === 'current' && !docPath.startsWith('api/')) {
              return `${repositoryUrl}/edit/main/website/docs/${docPath}`;
            }

            const issueTitle = encodeURIComponent(
              `Documentation issue: ${docPath}`,
            );
            const issueBody = encodeURIComponent(
              `Documentation version: ${version}\nPage: https://humanizr.net${permalink}\n\nDescribe the problem:\n`,
            );
            return `${repositoryUrl}/issues/new?title=${issueTitle}&body=${issueBody}`;
          },
          versions: {
            current: {
              label: preview.label,
              path: preview.route,
              banner: 'unreleased',
              badge: true,
              noIndex: true,
            },
            ...publishedVersions,
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
            to: '/docs/start/quick-start',
          },
        ],
      },
    ],
  ],
  themeConfig: {
    image: 'img/social-card.png',
    metadata: [
      {name: 'theme-color', content: '#172026'},
      {name: 'twitter:card', content: 'summary_large_image'},
      {property: 'og:type', content: 'website'},
    ],
    colorMode: {
      defaultMode: 'light',
      disableSwitch: false,
      respectPrefersColorScheme: true,
    },
    navbar: {
      title: 'Humanizer',
      logo: {
        alt: 'Humanizer home',
        src: 'img/humanizer-mark.svg',
      },
      items: [
        {
          type: 'docSidebar',
          sidebarId: 'docs',
          position: 'left',
          label: 'Guides',
        },
        {
          type: 'doc',
          docId: 'api/api',
          position: 'left',
          label: 'API',
        },
        {
          type: 'docsVersionDropdown',
          position: 'right',
          className: 'humanizerVersionDropdown',
        },
        {
          type: 'html',
          position: 'right',
          value:
            '<pagefind-modal-trigger placeholder="All versions" shortcut="mod+shift+k"></pagefind-modal-trigger><pagefind-modal reset-on-close></pagefind-modal>',
        },
        {
          href: repositoryUrl,
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Humanizer',
          items: [
            {label: 'Documentation home', to: '/'},
            {label: 'NuGet package', href: 'https://www.nuget.org/packages/Humanizer'},
          ],
        },
        {
          title: 'Project',
          items: [
            {label: 'GitHub', href: repositoryUrl},
            {
              label: 'Report a documentation issue',
              href: `${repositoryUrl}/issues/new`,
            },
          ],
        },
      ],
      copyright: `Copyright © ${new Date().getFullYear()} Humanizer contributors. MIT licensed.`,
    },
    prism: {
      theme: prismThemes.vsLight,
      darkTheme: prismThemes.dracula,
      additionalLanguages: ['csharp'],
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
