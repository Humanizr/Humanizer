---
id: index
title: Humanizer documentation
slug: /
sidebar_position: 0
---

# Humanizer documentation

Humanizer 4 turns program-shaped values into text people can read. These docs
cover the current package, APIs, and behavior.

## Start here

- [What Humanizer does](./start/overview.mdx) — decide where it belongs in an application.
- [Install Humanizer](./start/installation.mdx) — add the package to a project.
- [Developer quick start](./start/quick-start.mdx) — run a small, deterministic example.
- [Configuration basics](./start/configuration.mdx) — choose per-call, ambient, or global configuration.
- [Troubleshoot Humanizer](./start/troubleshooting.mdx) — diagnose package, culture, parser, analyzer, and AOT problems.

## Solve a task

- [Find a Humanizer scenario](./scenarios/index.mdx) — route from the value you have to the text you need.
- [Inflect nouns and render quantities](./scenarios/inflection-and-quantities.mdx)
- [Work with dates, times, durations, and ages](./scenarios/dates-times-durations-and-age.mdx)
- [Format numbers, words, byte sizes, and rates](./scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx)
- [Humanize enums, flags, and collections](./scenarios/enums-and-collections.mdx)
- [Configure localization and extensibility](./scenarios/localization-and-extensibility.mdx)

For exact signatures, open the [API reference](./api/index.md). It is generated
from Humanizer 4.

## Use and improve languages

- [Languages and cultures](./languages/index.mdx) — choose a culture and understand localization behavior.
- [Supported cultures](./languages/supported-cultures.mdx) — find the culture codes supported by Humanizer 4.
- [Report or correct a language issue](./contributing/report-language-issue.mdx) — provide the linguistic and platform context needed for a reliable fix.
- [Contribute to Humanizer](./contributing/index.mdx) — follow the locale, validation, and documentation workflows.

## Maintain a project

- [What's new in Humanizer 4](./whats-new/index.mdx) — review the current release highlights.
- [Upgrade from Humanizer 3.0.10](./upgrading/version-4-migration.mdx) — migrate packages, APIs, analyzers, and output expectations.
- [Upgrade from an earlier release](./upgrading/index.mdx) — follow the version-specific migration guidance.
- [Migrate namespaces with the analyzer](./analyzer/index.mdx) — configure and run the bundled analyzer locally and in CI.
- [Publish trimmed or Native AOT](./concepts/trimming-and-native-aot.mdx) — choose linker-safe enum APIs and run the publish proof.
