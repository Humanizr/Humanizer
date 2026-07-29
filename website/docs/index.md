---
id: index
title: Humanizer documentation
slug: /
sidebar_position: 0
---

# Humanizer documentation

Turn program-shaped values into text people can read. These guides cover
Humanizer from a first installation through the most common string, date,
number, enum, collection, and localization tasks.

## Start here

- [What Humanizer does](./start/overview.mdx) — decide where it belongs in an application.
- [Install Humanizer](./start/installation.mdx) — choose a version and add the package.
- [Five-minute quick start](./start/quick-start.mdx) — run a verified first example.
- [Choose the right package](./start/package-selection.md) — understand the release-specific package layout.
- [Troubleshoot Humanizer](./start/troubleshooting.mdx) — diagnose version, culture, parser, analyzer, and AOT problems.

## Solve a task

- [Find a Humanizer scenario](./scenarios/index.mdx) — route from the value you have to the text you need.
- [Inflect nouns and render quantities](./scenarios/inflection-and-quantities.mdx)
- [Work with dates, times, durations, and ages](./scenarios/dates-times-durations-and-age.mdx)
- [Format numbers, words, byte sizes, and rates](./scenarios/numbers-words-ordinals-roman-bytes-and-quantities.mdx)
- [Humanize enums, flags, and collections](./scenarios/enums-and-collections.mdx)
- [Configure localization and extensibility](./scenarios/localization-and-extensibility.mdx)

For exact signatures, open the [API reference](./api/index.md). The version
selector keeps guides, examples, and API pages in the same release.

## Use and improve languages

- [Languages and cultures](./languages/index.mdx) — choose a culture and understand the package boundary.
- [Supported cultures](./languages/supported-cultures.mdx) — find the culture codes supported by this version.
- [Report or correct a language issue](./contributing/report-language-issue.mdx) — provide the linguistic and platform context needed for a reliable fix.
- [Contribute to Humanizer](./contributing/index.mdx) — follow the locale, validation, and documentation workflows.

## Maintain a project

- [Plan an upgrade](./upgrading/index.mdx) — follow every compatibility boundary between two supported versions.
- [Upgrade to Humanizer 4](./upgrading/main-preview.mdx) — review verified differences from `3.0.10`.
- [Migrate namespaces with the analyzer](./analyzer/index.mdx) — configure and run the bundled analyzer locally and in CI.
- [Publish trimmed or Native AOT](./concepts/trimming-and-native-aot.mdx) — choose linker-safe enum APIs and run the publish proof.
