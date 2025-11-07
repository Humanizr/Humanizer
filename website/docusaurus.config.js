const {themes} = require('prism-react-renderer');
const lightCodeTheme = themes.github;
const darkCodeTheme = themes.dracula;

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'Humanizer',
  tagline: 'Natural-language helpers for .NET',
  url: 'https://humanizr.github.io',
  baseUrl: '/Humanizer/',
  organizationName: 'Humanizr',
  projectName: 'Humanizer',
  favicon: 'img/logo.png',
  trailingSlash: false,
  onBrokenLinks: 'throw',
  markdown: {
    hooks: {
      onBrokenMarkdownLinks: 'warn'
    }
  },
  deploymentBranch: 'gh-pages',
  i18n: {
    defaultLocale: 'en',
    locales: ['en']
  },
  presets: [
    [
      'classic',
      ({
        docs: {
          path: '../docs',
          routeBasePath: 'docs',
          sidebarPath: require.resolve('./sidebars.js'),
          editUrl: 'https://github.com/Humanizr/Humanizer/edit/main/docs/',
          showLastUpdateTime: true,
          showLastUpdateAuthor: true
        },
        blog: false,
        theme: {
          customCss: require.resolve('./src/css/custom.css')
        }
      })
    ]
  ],
  plugins: [
    [
      '@docusaurus/plugin-client-redirects',
      {
        redirects: [
          // Preserve legacy DocFX deep links by matching the markdown file name
          {
            to: '/docs/quick-start',
            from: ['/quick-start', '/docs/index']
          }
        ]
      }
    ]
  ],
  themeConfig: {
    image: 'img/logo.png',
    colorMode: {
      respectPrefersColorScheme: true
    },
    navbar: {
      title: 'Humanizer',
      logo: {
        alt: 'Humanizer logo',
        src: 'img/logo.png'
      },
      items: [
          {
            type: 'doc',
            docId: 'index',
            position: 'left',
            label: 'Guides'
          },
        {
          label: 'API',
          position: 'left',
          to: '/api'
        },
        {
          href: 'https://github.com/Humanizr/Humanizer',
          position: 'right',
          className: 'header-github-link',
          'aria-label': 'GitHub repository'
        }
      ]
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Docs',
          items: [
            {
              label: 'Quick start',
              to: '/docs/quick-start'
            },
            {
              label: 'Localization',
              to: '/docs/localization'
            },
            {
              label: 'Extensibility',
              to: '/docs/extensibility'
            }
          ]
        },
        {
          title: 'Community',
          items: [
            {
              label: 'GitHub Issues',
              href: 'https://github.com/Humanizr/Humanizer/issues'
            },
            {
              label: 'NuGet',
              href: 'https://www.nuget.org/packages/Humanizer'
            }
          ]
        },
        {
          title: 'More',
          items: [
            {
              label: 'Source',
              href: 'https://github.com/Humanizr/Humanizer'
            },
            {
              label: 'API reference',
              to: '/api'
            }
          ]
        }
      ],
      copyright: `Copyright \u00A9 ${new Date().getFullYear()} .NET Foundation and contributors.`
    },
    prism: {
      theme: lightCodeTheme,
      darkTheme: darkCodeTheme,
      additionalLanguages: ['csharp', 'powershell', 'bash', 'json']
    }
  }
};

module.exports = config;
