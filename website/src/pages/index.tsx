import Link from '@docusaurus/Link';
import Layout from '@theme/Layout';

export default function Home(): React.JSX.Element {
  return (
    <Layout
      title="Human-friendly text for .NET"
      description="Documentation for Humanizer">
      <main className="container margin-vert--xl">
        <h1>Humanizer documentation</h1>
        <p>
          Use Humanizer to turn dates, times, numbers, quantities, and strings
          into human-friendly text.
        </p>
        <Link className="button button--primary" to="/docs/">
          Read the latest stable documentation
        </Link>
      </main>
    </Layout>
  );
}
