# Contributing to Humanizer

Contributions are welcome. Start with an existing
[issue](https://github.com/Humanizr/Humanizer/issues), or open one before a
large behavior change so the direction is clear. Everyone participating in the
project must follow the [code of conduct](CODE_OF_CONDUCT.md).

## Prepare a change

Fork the repository, branch from the latest `main`, and keep the pull request
focused. Repository toolchains, coding conventions, and the exact build and
test commands are in [`AGENTS.md`](../AGENTS.md).

Before opening a pull request:

- add or update tests for behavior changes;
- update XML documentation and user guidance when behavior or public APIs
  change;
- run the applicable target-framework tests and formatting checks from
  `AGENTS.md`;
- disclose copied code and its license;
- link the issue with `fixes #123` when the change resolves one.

## Documentation

The public site source lives under `website/docs`. It includes runnable
examples, generated API reference, immutable release snapshots, and documented
release and rollback commands. Read the
[documentation contribution guide](../website/docs/contributing/documentation.mdx)
before editing it; never hand-edit `website/versioned_docs`.

## Languages

Language work remains especially valuable. Most localization is authored as
YAML under `src/Humanizer/Locales`, with generated registries and targeted
culture tests. Follow the
[locale contribution guide](../website/docs/contributing/adding-or-updating-a-locale.mdx)
and its linked schema and validation references instead of copying an older
formatter implementation.
