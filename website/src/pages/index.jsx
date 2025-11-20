import React from 'react';
import Layout from '@theme/Layout';
import Link from '@docusaurus/Link';
import styles from './index.module.css';

const highlights = [
  {
    title: 'Human-friendly text',
    description: 'Turn identifiers, numbers, dates, quantities, and enums into phrases that make sense to humans in 40+ locales.'
  },
  {
    title: 'Localization-first',
    description: 'Region-aware resources, grammatical gender/case, and per-culture tooling keep your UI aligned with your audience.'
  },
  {
    title: 'Extensible APIs',
    description: 'Swap formatters, plug in custom vocabularies, and add bespoke transformers without rewiring your app.'
  }
];

export default function Home() {
  return (
    <Layout
      title="Humanizer documentation"
      description="Humanizer documentation powered by Docusaurus with API coverage provided by DocFX">
      <header className="hero hero--primary">
        <div className="container">
          <h1 className="hero__title">Humanizer</h1>
          <p className="hero__subtitle">
            Natural-language helpers for .NET that keep your identifiers, numbers, dates, and collections human friendly across platforms.
          </p>
          <div className={styles.ctaRow}>
            <Link className="button button--secondary button--lg" to="/docs/quick-start">
              Quick start
            </Link>
            <Link className="button button--outline button--lg" to="/api">
              API reference
            </Link>
          </div>
        </div>
      </header>
      <main className="container margin-vert--xl">
        <div className="row">
          {highlights.map(feature => (
            <div className="col col--4" key={feature.title}>
              <div className="card card--full-height">
                <div className="card__body">
                  <h3>{feature.title}</h3>
                  <p>{feature.description}</p>
                </div>
              </div>
            </div>
          ))}
        </div>
      </main>
    </Layout>
  );
}
