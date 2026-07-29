/*
 * THESIS: Humanizer is a small, direct transformation from program values to
 * readable language.
 * OWN-WORLD: A working proof sheet pairs real C# input with human output.
 * STORY: Understand the library, install it, choose the value you have, then
 * follow the version-correct guide.
 * FIRST VIEWPORT: Canonical identity, promise, install command, first call, and
 * output are all visible without scrolling on a typical laptop.
 * FORM: One typographic sheet with ruled transformation rows; no card grid,
 * decorative metrics, or invented product imagery.
 */
import Link from '@docusaurus/Link';
import {
  useLatestVersion,
  useVersions,
} from '@docusaurus/plugin-content-docs/client';
import useBaseUrl from '@docusaurus/useBaseUrl';
import Layout from '@theme/Layout';
import {useEffect, useState} from 'react';
import styles from './index.module.css';

const transformations = [
  {
    input: '"PascalCaseInput".Humanize()',
    output: '"Pascal case input"',
  },
  {
    input: '"person".Pluralize()',
    output: '"people"',
  },
  {
    input: '42.ToWords(CultureInfo.GetCultureInfo("en"))',
    output: '"forty-two"',
  },
  {
    input: '"person".ToQuantity(2)',
    output: '"2 people"',
  },
];

const taskRoutes = [
  {
    title: 'Strings',
    prompt: 'Identifiers, casing, pluralization',
    sample: '.Humanize()  .Pluralize()  .Singularize()',
    to: 'scenarios/strings-and-casing/',
  },
  {
    title: 'Dates & time',
    prompt: 'Relative time, durations, ages, clocks',
    sample: '.Humanize()  .ToAge()  .ToClockNotation()',
    to: 'scenarios/dates-times-durations-and-age/',
  },
  {
    title: 'Numbers',
    prompt: 'Words, ordinals, quantities, byte sizes',
    sample: '.ToWords()  .Ordinalize()  .Bytes()',
    to: 'scenarios/numbers-words-ordinals-roman-bytes-and-quantities/',
  },
  {
    title: 'Application values',
    prompt: 'Enums, flags, collections',
    sample: '.Humanize()  .DehumanizeTo<T>()',
    to: 'scenarios/enums-and-collections/',
  },
];

export default function Home(): React.JSX.Element {
  const versions = useVersions(undefined);
  const latestVersion = useLatestVersion(undefined);
  const [docsVersion, setDocsVersion] = useState(latestVersion);

  useEffect(() => {
    const preferredVersionName = localStorage.getItem(
      'docs-preferred-version-default',
    );
    setDocsVersion(
      versions.find(({name}) => name === preferredVersionName) ?? latestVersion,
    );
  }, [latestVersion, versions]);

  const docsPath = (path: string) => `${docsVersion.path}/${path}`;
  const logoUrl = useBaseUrl('/img/logo.png');

  return (
    <Layout
      title="Human-friendly text for .NET"
      description="Turn .NET strings, dates, times, numbers, quantities, enums, and collections into text people can read.">
      <main className={styles.home}>
        <section className={styles.hero} aria-labelledby="home-title">
          <div className={styles.heroCopy}>
            <div className={styles.identity}>
              <img
                alt="Humanizer logo"
                className={styles.logo}
                height="115"
                src={logoUrl}
                width="115"
              />
              <span>Humanizer for .NET</span>
            </div>
            <h1 id="home-title">Turn .NET values into words people understand.</h1>
            <p className={styles.lede}>
              Humanizer adds focused extension methods for readable strings,
              dates, times, numbers, quantities, enums, and collections—across
              every supported culture.
            </p>
            <div className={styles.heroActions}>
              <Link className="button button--primary" to={docsPath('start/quick-start/')}>
                Run the five-minute example
              </Link>
              <Link className={styles.textLink} to={docsPath('scenarios/')}>
                Find an API by task <span aria-hidden="true">→</span>
              </Link>
            </div>
            <div className={styles.install}>
              <span>Install from NuGet</span>
              <code>dotnet add package Humanizer</code>
            </div>
          </div>

          <table className={styles.proof}>
            <caption className={styles.proofCaption}>
              Humanizer input and output examples
            </caption>
            <thead>
              <tr>
                <th scope="col">C# input</th>
                <th aria-label="becomes" scope="col" />
                <th scope="col">Readable output</th>
              </tr>
            </thead>
            <tbody>
              {transformations.map(({input, output}) => (
                <tr className={styles.proofRow} key={input}>
                  <td><code>{input}</code></td>
                  <td aria-hidden="true">→</td>
                  <td><samp>{output}</samp></td>
                </tr>
              ))}
            </tbody>
          </table>
        </section>

        <section className={styles.tasks} aria-labelledby="tasks-title">
          <div className={styles.sectionLead}>
            <p>Start with the value you have</p>
            <h2 id="tasks-title">Use the method that says what it does.</h2>
          </div>
          <div className={styles.taskList}>
            {taskRoutes.map(({title, prompt, sample, to}) => (
              <Link className={styles.task} key={title} to={docsPath(to)}>
                <div>
                  <h3>{title}</h3>
                  <p>{prompt}</p>
                </div>
                <code>{sample}</code>
                <span aria-hidden="true">↗</span>
              </Link>
            ))}
          </div>
        </section>

        <section className={styles.languagePromise} aria-labelledby="languages-title">
          <p>Language support</p>
          <div>
            <h2 id="languages-title">Supported means supported.</h2>
            <p>
              Humanizer 4 makes one promise: every listed culture supports
              every applicable localized feature. Missing behavior or English
              fallback is a bug, not a capability tier.
            </p>
          </div>
          <Link className={styles.textLink} to={docsPath('languages/supported-cultures/')}>
            See supported cultures <span aria-hidden="true">→</span>
          </Link>
        </section>

        <section className={styles.reference} aria-labelledby="reference-title">
          <div>
            <p>Version-correct by design</p>
            <h2 id="reference-title">Guides and signatures stay together.</h2>
          </div>
          <p>
            Choose your Humanizer version once. The guides, runnable examples,
            search results, and generated API reference follow that package.
          </p>
          <div className={styles.referenceLinks}>
            <Link className="button button--primary" to={docsPath('')}>
              Open the guides
            </Link>
            <Link className={styles.textLink} to={docsPath('api/')}>
              Browse exact signatures <span aria-hidden="true">→</span>
            </Link>
          </div>
        </section>
      </main>
    </Layout>
  );
}
