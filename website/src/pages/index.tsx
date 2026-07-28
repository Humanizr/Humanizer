import Link from '@docusaurus/Link';
import Layout from '@theme/Layout';
import styles from './index.module.css';

const scenarioGroups = [
  {
    number: '01',
    title: 'Strings & casing',
    description:
      'Humanize identifiers, transform casing, and reverse display text.',
    sample: '"PascalCaseInput".Humanize()',
  },
  {
    number: '02',
    title: 'Dates & time',
    description: 'Describe moments, durations, ages, and calendar values naturally.',
    sample: 'date.Humanize()',
  },
  {
    number: '03',
    title: 'Numbers & quantities',
    description: 'Write numbers, ordinals, byte sizes, and quantities for people.',
    sample: '42.ToWords()',
  },
  {
    number: '04',
    title: 'Enums & collections',
    description: 'Turn application values and lists into readable interface copy.',
    sample: 'items.Humanize()',
  },
];

export default function Home(): React.JSX.Element {
  return (
    <Layout
      title="Human-friendly text for .NET"
      description="Humanizer documentation for turning dates, times, numbers, quantities, and strings into human-friendly text.">
      <main className={styles.home}>
        <section className={styles.homeHero} aria-labelledby="home-title">
          <div className={styles.homeHero__copy}>
            <p className={styles.homeEyebrow}>Humanizer for .NET</p>
            <h1 id="home-title">
              Make software
              <span>sound like people.</span>
            </h1>
            <p className={styles.homeHero__lede}>
              A mature .NET library for turning dates, times, numbers,
              quantities, strings, enums, and collections into language people
              understand.
            </p>
            <div className={styles.homeHero__actions}>
              <Link className="button button--primary" to="/docs/">
                Start with Humanizer
              </Link>
              <Link className="humanizerTextLink" to="/docs/api/">
                Browse the API <span aria-hidden="true">↗</span>
              </Link>
            </div>
          </div>
          <div
            className={styles.homeSpecimen}
            aria-label="Illustrative Humanizer code example">
            <div className={styles.homeSpecimen__label}>
              <span>Illustrative C#</span>
              <span>input → output</span>
            </div>
            <pre>
              <code>
                <span className={styles.homeCode__comment}>
                  // less machine, more human
                </span>
                {'\n'}
                <span className={styles.homeCode__string}>
                  &quot;PascalCaseInput&quot;
                </span>
                .Humanize()
                {'\n'}
                <span className={styles.homeCode__result}>
                  → &quot;Pascal case input&quot;
                </span>
              </code>
            </pre>
          </div>
        </section>

        <nav className={styles.homeIndex} aria-label="Documentation sections">
          <a href="#install">Install</a>
          <a href="#quick-start">Quick start</a>
          <a href="#scenarios">Scenarios</a>
          <a href="#upgrading">Upgrading</a>
          <Link to="/docs/api/">API</Link>
          <a href="#languages">Languages</a>
        </nav>

        <section className={styles.homeStart} aria-labelledby="quick-start">
          <div className={styles.homeSectionIntro}>
            <p className="humanizerKicker">Start</p>
            <h2 id="quick-start">From package to useful text in minutes.</h2>
            <p>
              Install the package, import the namespace, and call the extension
              that matches the value you want to present.
            </p>
          </div>
          <div className={styles.homeStart__steps}>
            <article id="install">
              <span className={styles.homeStepNumber}>01</span>
              <div>
                <h3>Install</h3>
                <p>Add the latest stable Humanizer package to your project.</p>
                <pre>
                  <code>dotnet add package Humanizer</code>
                </pre>
              </div>
            </article>
            <article>
              <span className={styles.homeStepNumber}>02</span>
              <div>
                <h3>Humanize</h3>
                <p>
                  Use focused extension methods on the values you already have.
                </p>
                <span className={styles.homeExampleLabel}>
                  Illustrative example
                </span>
                <pre>
                  <code>
                    <span className={styles.homeCode__keyword}>using</span>{' '}
                    Humanizer;
                    {'\n\n'}
                    <span className={styles.homeCode__string}>
                      &quot;Underscored_input_string_is_turned_into_sentence&quot;
                    </span>
                    {'\n    '}.Humanize();
                  </code>
                </pre>
              </div>
            </article>
            <article>
              <span className={styles.homeStepNumber}>03</span>
              <div>
                <h3>Go deeper</h3>
                <p>
                  Follow the version-correct guide, then move directly into
                  precise reference when you need it.
                </p>
                <Link className="humanizerTextLink" to="/docs/">
                  Read the overview <span aria-hidden="true">→</span>
                </Link>
              </div>
            </article>
          </div>
        </section>

        <section
          className={styles.homeScenarios}
          id="scenarios"
          aria-labelledby="scenarios-title">
          <div className={styles.homeSectionIntro}>
            <p className="humanizerKicker">Scenarios</p>
            <h2 id="scenarios-title">Start with the job in front of you.</h2>
          </div>
          <div className={styles.homeScenarioList}>
            {scenarioGroups.map((scenario) => (
              <article key={scenario.number}>
                <span className={styles.homeScenarioList__number}>
                  {scenario.number}
                </span>
                <div>
                  <h3>{scenario.title}</h3>
                  <p>{scenario.description}</p>
                </div>
                <code>{scenario.sample}</code>
              </article>
            ))}
          </div>
        </section>

        <section
          className={styles.homeRoutes}
          aria-label="More documentation paths">
          <article id="upgrading">
            <p className="humanizerKicker">Upgrading</p>
            <h2>Move between releases with the boundaries in view.</h2>
            <p>
              Use version-specific guidance for package, namespace, analyzer,
              and behavior changes instead of reconstructing history.
            </p>
            <Link className="humanizerTextLink" to="/docs/">
              Open upgrading guidance <span aria-hidden="true">→</span>
            </Link>
          </article>
          <article>
            <p className="humanizerKicker">Reference</p>
            <h2>Exact signatures, in the version you selected.</h2>
            <p>
              Generated API pages live beside the guides, share their version,
              and remain searchable as one documentation set.
            </p>
            <Link className="humanizerTextLink" to="/docs/api/">
              Browse the API <span aria-hidden="true">→</span>
            </Link>
          </article>
          <article id="languages">
            <p className="humanizerKicker">Languages</p>
            <h2>Culture-aware output, with a direct correction path.</h2>
            <p>
              Understand culture selection and locale behavior, then find the
              focused contribution guidance when language output needs work.
            </p>
            <Link className="humanizerTextLink" to="/docs/">
              Explore language support <span aria-hidden="true">→</span>
            </Link>
          </article>
        </section>
      </main>
    </Layout>
  );
}
