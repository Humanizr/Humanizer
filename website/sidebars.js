/** @type {import('@docusaurus/plugin-content-docs').SidebarsConfig} */
const sidebars = {
  guidesSidebar: [
    'index',
    {
      type: 'category',
      label: 'Getting started',
      collapsed: false,
      items: ['quick-start', 'installation', 'configuration', 'application-integration']
    },
    {
      type: 'category',
      label: 'Strings and collections',
      items: [
        'string-humanization',
        'string-dehumanization',
        'string-transformations',
        'string-truncation',
        'heading',
        'collection-humanization',
        'inflector-methods'
      ]
    },
    {
      type: 'category',
      label: 'Dates and times',
      items: ['dates-and-times', 'time-unit-symbols']
    },
    {
      type: 'category',
      label: 'Numbers and quantities',
      items: ['numbers', 'enumerations', 'bytesize', 'pluralization']
    },
    {
      type: 'category',
      label: 'Localization and customization',
      items: ['localization', 'custom-vocabularies', 'extensibility']
    },
    {
      type: 'category',
      label: 'Operations',
      items: ['testing', 'performance', 'troubleshooting', 'v3-namespace-migration', 'contributing']
    }
  ]
};

module.exports = sidebars;
